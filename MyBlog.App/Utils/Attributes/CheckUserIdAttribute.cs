using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyBlog.App.Utils.Attributes
{
    /// <summary>
    /// Аттрибут проверки наличия утверждения UserID у текущего пользователя
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class CheckUserIdAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authCheck = context?.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
            if (!authCheck)
            {
                context!.Result = new UnauthorizedResult();
                return;
            }

            var userId = context!.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
            if (userId == null)
                context!.HttpContext.Response.Redirect($"/Refresh?ReturnUrl={context.HttpContext.Request.GetEncodedPathAndQuery()}");
        }
    }
}
