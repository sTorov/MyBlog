using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyBlog.App.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CheckUserIdAttribute : Attribute, IAuthorizationFilter
    {
        public string ParameterName { get; set; }
        public string ActionName { get; set; }
        public string FullAccess { get; set; }

        public CheckUserIdAttribute(string parameterName = "", string actionName = "", string fullAccess = "")
        {
            ParameterName = parameterName;
            ActionName = actionName.ToLower();
            FullAccess = fullAccess;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (CheckAccess(context)) return;

            var userId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
            if (userId == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (context.HttpContext.Request.Method == "GET")
                GetMethod(userId, context);
            else
                PostMethod(userId, context);
        }

        private bool CheckAccess(AuthorizationFilterContext context)
        {
            if (FullAccess == "") return false;

            var check = false;
            var roles = FullAccess.Replace(" ", "").Split(",");
            foreach (var role in roles)
            {
                if (context.HttpContext.User.IsInRole(role))
                {
                    check = true;
                    break;
                }
            }
            return check;
        }

        private void GetMethod(string userId, AuthorizationFilterContext context)
        {
            var path = context.HttpContext.Request.Path.ToString().ToLower() +
                context.HttpContext.Request.QueryString;
            string? paramValue = context.HttpContext.Request.Query[ParameterName];

            if (path == $"/{ActionName}?{ParameterName}={userId}")
                return;
            else if (path == $"/{ActionName}" && paramValue == null)
                context.Result = new RedirectResult($"{path}?{ParameterName}={userId}");
            else
                context.Result = new NotFoundResult();
        }

        private void PostMethod(string userId, AuthorizationFilterContext context)
        {
            string? paramValue = context.HttpContext.Request.Form[ParameterName];

            if (paramValue == null)
            {
                context.Result = new NotFoundResult();
                return;
            }
            if (paramValue != userId)
                context.Result = new ForbidResult();
        }
    }
}
