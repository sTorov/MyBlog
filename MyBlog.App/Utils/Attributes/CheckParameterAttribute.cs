using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyBlog.App.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CheckParameterAttribute : Attribute, IAuthorizationFilter
    {
        public string ParameterName { get; set; }
        public int ParameterCount { get; set; }
        public string Path { get; set; }

        public CheckParameterAttribute(string parameterName = "", string path = "", int paramCount = 1)
        {
            ParameterName = parameterName;
            Path = path.ToLower();
            ParameterCount = paramCount;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var param = SetParam(context) ?? string.Empty;
            GetMethod(context);

            //if (context.HttpContext.Request.Method == "GET")
            //else
            //    PostMethod(param, context);
        }

        private string? SetParam(AuthorizationFilterContext context)
        {
            var Id = context.HttpContext.Request.Query.FirstOrDefault(p => p.Key == ParameterName).Value.ToString();
            if (string.IsNullOrWhiteSpace(Id))
            {
                Id = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
                context.HttpContext.Request.QueryString = context.HttpContext.Request.QueryString.Add(ParameterName, Id ?? "");
            }

            return Id;
        }

        private void GetMethod(AuthorizationFilterContext context)
        {
            var path = context.HttpContext.Request.Path.ToString().ToLower();
            var query = context.HttpContext.Request.Query;
            
            if (path.StartsWith($"/{Path}") && query.Any(p => p.Key == ParameterName) && query.Count == ParameterCount)
                return;
            else
                context.Result = new NotFoundResult();
        }

        //private void PostMethod(string param, AuthorizationFilterContext context)
        //{
        //    string? paramValue = context.HttpContext.Request.Form[ParameterName];

        //    if (paramValue == null)
        //    {
        //        context.Result = new NotFoundResult();
        //        return;
        //    }
        //    if (paramValue != param)
        //        context.Result = new ForbidResult();
        //}
    }
}
