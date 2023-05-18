namespace MyBlog.Services.ViewModels.Posts.Interfaces
{
    /// <summary>
    /// Интерфейс модели создания статьи
    /// </summary>
    public interface IPostCreateModel : IPostRequestModel
    {
        int UserId { get; set; }
    }
}
