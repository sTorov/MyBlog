namespace MyBlog.App.Utils
{
    public class Helper
    {
        public static int GetIntValue(string str)
        {
            int.TryParse(str, out var value);
            return value;
        }

        public static Uri GetUri(HttpRequest request)
        {
            //var r = request.Query.Any();
            //var r2 = request.RouteValues.Any();

            var builder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Port = request.Host.Port ?? default,
                Path = request.Path,
                Query = request.QueryString.ToUriComponent()
            };
            return builder.Uri;
        }
    }
}
