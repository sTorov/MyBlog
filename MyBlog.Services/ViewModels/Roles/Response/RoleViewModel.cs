namespace MyBlog.Services.ViewModels.Roles.Response
{
    /// <summary>
    /// Модель представления роли
    /// </summary>
    public class RoleViewModel
    {
        /// <summary>
        /// Идентификатор роли
        /// </summary>
        /// <example>4</example>
        public int Id { get; set; }

        /// <summary>
        /// Имя роли
        /// </summary>
        /// <example>roleName</example>
        public string Name { get; set; }

        /// <summary>
        /// Описание роли
        /// </summary>
        /// <example>description</example>
        public string Description { get; set; }
    }
}
