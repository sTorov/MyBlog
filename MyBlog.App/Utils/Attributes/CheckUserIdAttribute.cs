﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyBlog.App.Utils.Attributes
{
    /// <summary>
    /// Аттрибут для проверки наличия утверждения UserID у пользователя
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class CheckUserIdAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
            if (userId == null)
                context.Result = new UnauthorizedResult();
        }
    }
}