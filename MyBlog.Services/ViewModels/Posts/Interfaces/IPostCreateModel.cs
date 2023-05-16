namespace MyBlog.Services.ViewModels.Posts.Interfaces
{
    /// <summary>
    /// Интерфейс модели создания статьи
    /// </summary>
    public interface IPostCreateModel : IPostResponceModel
    {
        int UserId { get; set; }
    }
}
