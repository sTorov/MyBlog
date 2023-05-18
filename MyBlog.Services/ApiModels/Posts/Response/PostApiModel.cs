namespace MyBlog.Services.ApiModels.Posts.Response
{
    /// <summary>
    /// Модель статьи для API
    /// </summary>
    public class PostApiModel
    {
        /// <summary>
        /// Идентификатор статьи
        /// </summary>
        /// <example>11</example>
        public int Id { get; set; }

        /// <summary>
        /// Заголовок статьи
        /// </summary>
        /// <example>title</example>
        public string Title { get; set; }

        /// <summary>
        /// Содержание статьи
        /// </summary>
        /// <example>content</example>
        public string Content { get; set; }

        /// <summary>
        /// Дата создания статьи
        /// </summary>
        /// <example>12.12.2000</example>
        public string CreatedDate { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        /// <example>2</example>
        public int UserId { get; set; }

        /// <summary>
        /// Список тегов
        /// </summary>
        /// <example>["tag1", "tag2"]</example>
        public List<string> Tags { get; set; }
    }
}
