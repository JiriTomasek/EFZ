using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFZ.Core.Enums;
using EFZ.Domain.BusinessLogic.Impl;
using EFZ.Domain.BusinessLogic.Interface;
using EFZ.WebApplication.Middleware;
using EFZ.WebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EFZ.WebApplication.Controllers
{

    [ServiceFilter(typeof(BaseControllerFilter))]

    [BaseAuthorize(PermissionItem.Guest)]
    public class BaseController : Controller
    {
        public BaseController(IConfiguration cfg, IUserBlProvider userBlProvider)
        {
            Cfg = cfg;
            UserBlProvider = userBlProvider;
        }

        public IConfiguration Cfg { get; set; }
        public IUserBlProvider UserBlProvider { get; }

        public BaseIndexModel IndexModel { get; set; }

       
        [Route("RedirectToErrorPage")]
        public ActionResult RedirectToErrorPage(string id )
        {
            return RedirectToAction(nameof(HomeController.AccessDenied), "Home");
        }
    }
}