using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EFZ.Core.Enums;
using EFZ.Domain.BusinessLogic.Interface;
using EFZ.Entities.Entities;
using EFZ.WebApplication.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EFZ.WebApplication.Models;
using EFZ.WebApplication.Models.AccountViewModels;
using EFZ.WebApplication.Models.Home;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace EFZ.WebApplication.Controllers
{
    public class HomeController : BaseController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IUserBlProvider _userBlProvider;
        private readonly IConfiguration _cfg;

        public HomeController(
            SignInManager<User> signInManager,  ILogger<HomeController> logger, IUserBlProvider userBlProvider, IConfiguration cfg) : base(cfg, userBlProvider)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userBlProvider = userBlProvider;
            _cfg = cfg;
            IndexModel = new HomeIndexModel();


        }

        public IActionResult Index()
        {
            var test = User;

            UserBlProvider.RefreshUser();
            return View(IndexModel);
        }

        public IActionResult Privacy()
        {
            return View(IndexModel);
        }
        public IActionResult AccessDenied()
        {
            if (BaseAuthorizeAttribute.IsAuthorized(PermissionItem.Member, _userBlProvider, _cfg, HttpContext))
                return RedirectToAction("Index");
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
