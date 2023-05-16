namespace MyBlog.Services.ApiModels.Comments.Response
{
    /// <summary>
    /// Модель комментария для API
    /// </summary>
    public class CommentApiModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string CreatedDate { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
    }
}
