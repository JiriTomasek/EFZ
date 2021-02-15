namespace EFZ.WebApplication.Helpers
{
    public static class ErrorHelper
    {
        public const string BadRequest = "400";
        public const string Unauthorized = "401";
        public const string Forbidden = "403";
        public const string PageNotFound = "404";
        public const string TimedOut = "408";
        public const string InternalServerError = "500";
        public const string BadGateway = "502";
        public const string ServiceUnavailable = "503";
        public const string GatewayTimeout = "504";
        public const string AccessDenied = "AccessDenied";
        public const string ErrorIcon = "fas fa-exclamation-triangle";
        public const string AccDeniedIcon = "fas fa-user-alt-slash";

        internal static string GetMessageByCode(string code)
        {
            switch (code)
            {
                case BadRequest:
                    return "400 - Bad Request";
                case Forbidden:
                    return "403 - Forbidden";
                case PageNotFound:
                    return "404 - Page Not Found";
                case TimedOut:
                    return "408 - Request timed out";
                case AccessDenied:
                    return "Access Denied";
                case InternalServerError:
                    return "500 - Internal Server Error";
                case BadGateway:
                    return "502 - Bad Gateway";
                case ServiceUnavailable:
                    return "503 - ServiceUnavailable";
                case GatewayTimeout:
                    return "504 - Gateway Timeout";

                default:
                    return $"{code} - Unexpected error occured";
            }
        }

        internal static string GetImageById(string id)
        {
            switch (id)
            {
                case AccessDenied:
                    return AccDeniedIcon;
                default:
                    return ErrorIcon;
            }

        }
    }
}
