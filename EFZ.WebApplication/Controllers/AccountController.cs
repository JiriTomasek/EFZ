using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text.Json;
using EFZ.Domain.BusinessLogic.Interface;
using EFZ.Entities.Entities;
using EFZ.Resources;
using EFZ.WebApplication.Models;
using EFZ.WebApplication.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EFZ.WebApplication.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILoggerFactory loggerFactory, IUserBlProvider userBlProvider, IConfiguration cfg) : base(cfg, userBlProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return PartialView("_LoginDilog", new LoginViewModel());
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                //var user = UserBlProvider.GetUserByEmail(model.Email);
                //var hash = new PasswordHasher<User>().HashPassword(user, model.Password);

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    return Ok("Success");
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    return Ok("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Ok("Invalid login attempt.");
                }
            }

            // If we got this far, something failed, redisplay form
            return Ok("Invalid login attempt.");
        }

        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return PartialView("_RegisterDialog", new RegisterViewModel());
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var list = new List<string>();
            if (!model.RegisterPassword.Equals(model.ConfirmPassword))
            {
                list.Add(ErrorMessages.valPasswordNotMatch);
                return Json(list);
            }

            if (!ModelState.IsValid) return Ok("Invalid data");

            var user = new User { UserName = model.RegisterEmail, Email = model.RegisterEmail };



            var result = await _userManager.CreateAsync(user, model.RegisterPassword);
            if (result.Succeeded)
            {

                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation(3, "User created a new account with password.");
                list.Add("Success");
                return Json(list);
            }
            AddErrors(result);

            list.AddRange(GetErrorMessage(result));

            return Json(list);

        }

        private List<string> GetErrorMessage(IdentityResult result)
        {
            var list = new List<string>();
            foreach (var error in result.Errors)
            {
                if(error.Code.Equals("PasswordRequiresNonAlphanumeric"))
                    list.Add(ErrorMessages.valPasswordRequiresNonAlphanumeric);
                else if (error.Code.Equals("PasswordRequiresDigit"))
                    list.Add(ErrorMessages.valPasswordRequiresDigit);
                else if (error.Code.Equals("PasswordRequiresUpper"))
                    list.Add(ErrorMessages.valPasswordRequiresUpper);
                else if (error.Code.Equals("PasswordRequiresLower"))
                    list.Add(ErrorMessages.valPasswordRequiresLower);
                else
                {
                    list.Add(error.Description);
                }
            }

            return list;
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}