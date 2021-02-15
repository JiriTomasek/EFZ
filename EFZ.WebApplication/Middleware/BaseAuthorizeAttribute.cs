using System;
using System.Linq;
using System.Threading.Tasks;
using EFZ.Core;
using EFZ.Core.Enums;
using EFZ.Domain.BusinessLogic.Impl;
using EFZ.Domain.BusinessLogic.Interface;
using EFZ.Entities.Entities;
using EFZ.WebApplication.Controllers;
using EFZ.WebApplication.Helpers;
using EFZ.WebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace EFZ.WebApplication.Middleware
{
    public class BaseAuthorizeAttribute : TypeFilterAttribute
    {
        #region constructor

        public BaseAuthorizeAttribute(PermissionItem item) : base(typeof(BaseAuthorizeActionFilter))
        {
            Arguments = new object[]
            {
                item
            };
        }

        #endregion

        public class BaseAuthorizeActionFilter : IAsyncActionFilter
        {
            #region fields

            private readonly PermissionItem _item;

            #endregion

            #region constructor

            public BaseAuthorizeActionFilter(PermissionItem item= PermissionItem.Guest)
            {
                _item = item;
            }

            #endregion

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.Controller == null)
                {
                    throw (new Exception("Authorize received Controller==null"));
                }

                if (!context.Controller.GetType()
                    .IsSubclassOf(typeof(BaseController)))
                {
                    throw (new Exception(
                        "Authorize attribute can be used only on BaseController based classes"));
                }

                var baseController = (BaseController) context.Controller;
                

                var isAuthorized = IsAuthorized(_item, baseController.UserBlProvider, baseController.Cfg,
                    context.HttpContext);

                if (!isAuthorized)
                {
                  

                    context.Result = baseController.RedirectToErrorPage(ErrorHelper.Unauthorized);
                }
                else
                {
                    await next();
                }
            }

        }

      
        public static bool IsAuthorized(
        PermissionItem item,
        IUserBlProvider userBlProvider,
        IConfiguration cfg,
        HttpContext ctx)
    {
        if (item == PermissionItem.Guest)
        {
            return true;
        }
        User user = null;
        user = userBlProvider.GetUser(ctx.User);

        if (user == null)
            return false;
        switch (item)
        {

            case PermissionItem.Admin:
                {
                    var isAdmin = user.UserRoles.Any(r => r.Role != null && r.Role.Name.Contains(Constants.Admin));
                    return isAdmin;
                }

            case PermissionItem.Member:
                {
                    var isMember = user.UserRoles.Any(
                        r => r.Role != null && (r.Role.Name.Contains(Constants.Admin) || r.Role.Name.Contains(Constants.Member)));
                    return isMember;
                }
        }

        return false;
    }
}
}