using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EFZ.Core;
using EFZ.Core.Entities.Dao;
using EFZ.Core.Enums;
using EFZ.Core.Mapping;
using EFZ.Core.Validation.Interfaces;
using EFZ.Domain.BusinessLogic.Impl;
using EFZ.Domain.BusinessLogic.Interface;
using EFZ.Entities.Entities;
using EFZ.WebApplication.Middleware;
using EFZ.WebApplication.Models.Attachment;
using EFZ.WebApplication.Models.SelectModel;
using EFZ.WebApplication.Models.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EFZ.WebApplication.Controllers
{
    [BaseAuthorize(PermissionItem.Admin)]
    public class SettingsController : BaseController
    {
        private readonly IJobSchedulerBlProvider _jobSchedulerBlProvider;
        private readonly UserManager<User> _userManager;
        private readonly ISettingsBlProvider _settingsBlProvider;
        private readonly ILogger _logger;

        public SettingsController(IJobSchedulerBlProvider jobSchedulerBlProvider,
            UserManager<User> userManager, ISettingsBlProvider settingsBlProvider,ILoggerFactory loggerFactory, IConfiguration cfg, IUserBlProvider userBlProvider) : base(cfg, userBlProvider)
        {
            _jobSchedulerBlProvider = jobSchedulerBlProvider;
            _userManager = userManager;
            _settingsBlProvider = settingsBlProvider;

            _logger = loggerFactory.CreateLogger<SettingsController>();
            IndexModel = new SettingsModel();
        }

        public IActionResult Index()
        {
            return View();
        }

      
        public IActionResult UserManagement()
        {
            var model = (SettingsModel) IndexModel;
            model.Users = UserBlProvider.GetUsers().Select(UserVm.MapToViewModel).ToList();

            return View(model);
        }

        public IActionResult CustomerManagement()
        {
            var model = (SettingsModel)IndexModel;
            model.Customers = _settingsBlProvider.GetCustomers().Select(CustomerVm.MapToViewModel).ToList();
          

            return View(model);
        }

        public IActionResult JobsManagement()
        {
            var model = (SettingsModel)IndexModel;
            model.Jobs = _settingsBlProvider.GetJobs().ToList();
            model.JobSchedulers = _settingsBlProvider.GetJobScheduler(15).ToList();

         

            return View(model);
        }

        public IActionResult CompanyManagement()
        {
            var model = (SettingsModel)IndexModel;
            model.Company = CompanyVm.MapToViewModel(_settingsBlProvider.GetCompany());
           
            if ((model.Company != null && model.Company.Address == null))
                model.Company.Address = new AddressVm();
            return View(model);
        }

        public IActionResult XmlFiles()
        {
            var model = (SettingsModel)IndexModel;
            model.XmlFiles = _settingsBlProvider.GetXmlFilesForProcessing();
          
            return View(model);
        }
        public IActionResult AttachmentScan()
        {
            var model = (SettingsModel)IndexModel;
            model.AttachmentScansFiles = _settingsBlProvider.GetAttachmentScanFilesForProcessing();

            return View(model);
        }

        [HttpPost]
        public IActionResult EditUser(UserVm model)
        {
            if (ModelState.IsValid)
            {
                var entityModel = UserVm.MapToEntityModel(model);

               
                UserBlProvider.UpdateUser(entityModel, model.RolesIdList);
            }

            return RedirectToAction("UserManagement", "Settings");
        }

        [HttpGet]
        public IActionResult NewUser()
        {
            var userVm = new UserVm();
            ViewBag.Customers =
                _settingsBlProvider.GetCustomers()
                    .Select(t => new SelectModel() { label = t.Name, value = t.Id }).ToList();

            return PartialView("_NewUser", userVm);
        }

        [HttpPost]
        public IActionResult NewUser(UserVm model)
        {
            if (ModelState.IsValid)
            {
                var entityModel = UserVm.MapToEntityModel(model);

                entityModel.UserName = entityModel.Email;


                var result = _userManager.CreateAsync(entityModel);
                result.Wait();
                var user = UserBlProvider.GetUserByEmail(entityModel.Email);
                if (user != null)
                {
                    var hash = new PasswordHasher<User>().HashPassword(user, model.Password);

                    user.PasswordHash = hash;

                    UserBlProvider.UpdateUser(user, user.UserRoles.Select(t => t.RoleId).ToList());
                }
                    





            }

            return RedirectToAction("UserManagement", "Settings");
        }

        [HttpGet]
        public IActionResult EditUser(int userId)
        {
            var userVm = UserVm.MapToViewModel(UserBlProvider.GetUserById(userId));
            ViewBag.Customers =
                _settingsBlProvider.GetCustomers()
                    .Select(t => new SelectModel() { label = t.Name, value = t.Id }).ToList();

            return PartialView("_EditUser", userVm);
        }


        [HttpPost]
        public IActionResult DeleteUser(UserVm model)
        {
                var entityModel = UserVm.MapToEntityModel(model);



                UserBlProvider.DeleteUser(entityModel);
          

            return RedirectToAction("UserManagement", "Settings");
        }

        [HttpGet]
        public IActionResult DeleteUser(int userId)
        {
            var userVm = UserVm.MapToViewModel(UserBlProvider.GetUserById(userId));

            return PartialView("_DeleteUser", userVm);
        }

        [HttpPost]
        public IActionResult NewOrEditCustomer(CustomerVm model)
        {
            if (ModelState.IsValid)
            {

                var entityModel = CustomerVm.MapToEntityModel(model);

                _settingsBlProvider.AddOrUpdateCustomer(entityModel, model.UserIds);
            }

            return RedirectToAction("CustomerManagement", "Settings");
        }

        [HttpGet]
        public IActionResult NewOrEditCustomer(long customerId)
        {
            var customerVm = CustomerVm.MapToViewModel(_settingsBlProvider.GetCustomerById(customerId) ?? new Customer());

            ViewBag.AllUsers = UserBlProvider.GetUsers().Select(t => new SelectModel() { label = t.Email, value = t.Id.ToString() }).ToList();

            if(customerVm?.Address == null)
                customerVm.Address = new AddressVm();

            return PartialView("_NewOrEditCustomer", customerVm);
        }

        [HttpPost]
        public IActionResult DeleteCustomer(CustomerVm model)
        {
                var entityModel = CustomerVm.MapToEntityModel(model);



                _settingsBlProvider.DeleteCustomer(entityModel);
          

            return RedirectToAction("CustomerManagement", "Settings");
        }

        [HttpGet]
        public IActionResult DeleteCustomer(int customerId)
        {
            var customerVm = CustomerVm.MapToViewModel(_settingsBlProvider.GetCustomerById(customerId));

            return PartialView("_DeleteCustomer", customerVm);
        }


        public IActionResult RunManuallyJob(Guid jobId)
        {
            if(jobId.Equals(Guid.Parse(Constants.JobXmlProcessingId)))
                _jobSchedulerBlProvider.RunXmlManually();
            else if (jobId.Equals(Guid.Parse(Constants.JobInvoicingCompletionId)))
                _jobSchedulerBlProvider.RunCompletionManually();
            else if (jobId.Equals(Guid.Parse(Constants.JobAttachmentScanProcessingId)))
                _jobSchedulerBlProvider.RunAttachmentScansManually();

            return Ok("Success");
        }

        public IActionResult StartJob(Guid jobId)
        {

            _settingsBlProvider.StartJob(jobId);

            return Ok("Success");
        }
        public IActionResult StopJob(Guid jobId)
        {

            _settingsBlProvider.StopJob(jobId);

            return Ok("Success");
        }

        [HttpPost]
        public IActionResult EditJob(JobVm model)
        {
            if (ModelState.IsValid)
            {
                var entityModel = MappingUtils.MapToNew<JobVm, Job>(model);
                _settingsBlProvider.UpdateJob(entityModel);
            }

            return RedirectToAction("JobsManagement", "Settings");
        }

        [HttpGet]
        public IActionResult EditJob(Guid jobId)
        {
            var jobVm = MappingUtils.MapToNew<Job, JobVm>(_settingsBlProvider.GetSingleJob(jobId));
            

            return PartialView("_EditJob", jobVm);
        }

        [HttpGet]
        public IActionResult GetJobLog(long jobId)
        {
            var logs = _settingsBlProvider.GetSingleJobScheduler(jobId).Logs.ToList();

            return PartialView("_JobLog", logs);
        }


        [HttpPost]
        public IActionResult SaveCompany(SettingsModel model)
        {
            if (ModelState.IsValid)
            {

                var entityModel = CompanyVm.MapToEntityModel(model.Company);


                _settingsBlProvider.UpdateCompany(entityModel);

            }

            return RedirectToAction("CompanyManagement", "Settings");
        }

        [HttpPost]
        public IActionResult XmlFilesUpload(List<IFormFile> files)
        {
            if (files is null ) return BadRequest();

            foreach (var formFile in files)
            {
                if (formFile.ContentType == "text/xml")
                {
                    using var memoryStream = new MemoryStream();
                    formFile.CopyTo(memoryStream);
                    _settingsBlProvider.UploadXmlFile(memoryStream, formFile.FileName);
                }
            }

            return RedirectToAction("XmlFiles", "Settings");

        }


        [HttpPost]
        public IActionResult AttachmentScanUpload(List<IFormFile> files)
        {
            if (files is null) return BadRequest();

            foreach (var formFile in files)
            {
                if (formFile.ContentType == "image/jpg" || formFile.ContentType == "image/jpeg"|| formFile.ContentType == "image/png"|| formFile.ContentType == "image/tif"|| formFile.ContentType == "image/bmp")
                {
                    using var memoryStream = new MemoryStream();
                    formFile.CopyTo(memoryStream);
                    _settingsBlProvider.UploadAttachmentScanFile(memoryStream, formFile.FileName);
                }
            }

            return RedirectToAction("AttachmentScan", "Settings");

        }

        [HttpPost]
        public IActionResult DeleteXmlFile(string path)
        {

            _settingsBlProvider.DeleteFile(path);

            return Ok("Success");

        }
        [HttpPost]
        public IActionResult AttachmentScanDelete(string path)
        {

            _settingsBlProvider.DeleteFile(path);

            return Ok("Success");

        }
    }

   
}