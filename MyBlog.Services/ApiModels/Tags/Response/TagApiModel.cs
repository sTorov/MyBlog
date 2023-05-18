namespace MyBlog.Services.ApiModels.Tags.Response
{
    /// <summary>
    /// Модель тега для API
    /// </summary>
    public class TagApiModel 
    {
        /// <summary>
        /// Идентификатор тега
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Имя тега
        /// </summary>
        /// <example>tag_name</example>
        public string Name { get; set; }
    }
}
