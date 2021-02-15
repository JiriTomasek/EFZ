using System.Threading.Tasks;
using EFZ.Core.Enums;
using EFZ.WebApplication.Controllers;
using EFZ.WebApplication.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EFZ.WebApplication.Middleware
{
    public class BaseControllerFilter : IActionFilter, IAsyncActionFilter
    {
        public static class ViewDataFlags
        {
            #region fields


            public const string IsAdmin = "IsAdminFlag";

            public const string IsMember = "IsMemberFlag";

            public const string IsGuest = "IsGuestFlag";

            #endregion
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // do something after the action executes
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerName = context.RouteData.Values["controller"];
            var actionName = context.RouteData.Values["action"];

            if (context.Controller is BaseController)
            {
                var baseController = (BaseController)context.Controller;
                
                if (baseController.IndexModel != null)
                {
                    baseController.IndexModel.ActiveAction = actionName.ToString();
                    baseController.IndexModel.ActiveController = controllerName.ToString();
                }
            }
            populate(context);
        }
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controllerName = context.RouteData.Values["controller"];
            var actionName = context.RouteData.Values["action"];

            if (context.Controller is BaseController)
            {
                var baseController = (BaseController)context.Controller;
                var model = baseController.ViewData.Model;
                if (baseController.IndexModel != null)
                {
                    baseController.IndexModel.ActiveAction = actionName.ToString();
                    baseController.IndexModel.ActiveController = controllerName.ToString();
                }
            }
            populate(context);
            return next();
        }
        private void populate(ActionExecutingContext context)
        {
            if (context.Controller is BaseController)
            {
                var ctrl = (BaseController)context.Controller;
                ctrl.ViewData[ViewDataFlags.IsGuest] = BaseAuthorizeAttribute.IsAuthorized(
                    PermissionItem.Guest,
                    ctrl.UserBlProvider,
                    ctrl.Cfg,
                    ctrl.HttpContext)
                    ? "true"
                    : "false";
                ctrl.ViewData[ViewDataFlags.IsMember] = BaseAuthorizeAttribute.IsAuthorized(
                    PermissionItem.Member,
                    ctrl.UserBlProvider,
                    ctrl.Cfg,
                    ctrl.HttpContext)
                    ? "true"
                    : "false";
                ctrl.ViewData[ViewDataFlags.IsAdmin] = BaseAuthorizeAttribute.IsAuthorized(
                    PermissionItem.Admin,
                    ctrl.UserBlProvider,
                    ctrl.Cfg,
                    ctrl.HttpContext)
                    ? "true"
                    : "false";
            }
        }
    }
}