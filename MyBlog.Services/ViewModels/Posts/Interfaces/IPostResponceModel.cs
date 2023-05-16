namespace MyBlog.Services.ViewModels.Posts.Interfaces
{
    /// <summary>
    /// Интерфейс модели ответа статьи 
    /// </summary>
    public interface IPostResponceModel
    {
        string? PostTags { get; set; }
        string Title { get; set; }
        string Content { get; set; }
    }
}
