namespace MyBlog.Services.Services.Data
{
    /// <summary>
    /// Стандартные роли приложения
    /// </summary>
    internal static class DefaultRoles
    {
        internal static List<string> DefaultRoleNames = new() 
        { 
            "Admin",
            "Moderator",
            "User" 
        };
    }
}
