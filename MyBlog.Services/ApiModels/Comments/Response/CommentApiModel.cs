namespace MyBlog.Services.ApiModels.Comments.Response
{
    /// <summary>
    /// Модель комментария для API
    /// </summary>
    public class CommentApiModel
    {
        /// <summary>
        /// Идентификатор комментария
        /// </summary>
        /// <example>100</example>
        public int Id { get; set; }

        /// <summary>
        /// Текст комметрария
        /// </summary>
        /// <example>text</example>
        public string Text { get; set; }

        /// <summary>
        /// Дата создания комментария
        /// </summary>
        /// <example>12.12.2000</example>
        public string CreatedDate { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        /// <example>2</example>
        public int UserId { get; set; }

        /// <summary>
        /// Идентификатор статьи
        /// </summary>
        /// <example>11</example>
        public int PostId { get; set; }
    }
}
