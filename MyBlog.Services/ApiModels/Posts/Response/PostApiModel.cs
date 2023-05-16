namespace MyBlog.Services.ApiModels.Posts.Response
{
    /// <summary>
    /// Модель статьи для API
    /// </summary>
    public class PostApiModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CreatedDate { get; set; }
        public int UserId { get; set; }
        public List<string> Tags { get; set; }
    }
}
