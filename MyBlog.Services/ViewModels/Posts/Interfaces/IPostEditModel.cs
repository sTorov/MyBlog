namespace MyBlog.Services.ViewModels.Posts.Interfaces
{
    /// <summary>
    /// Интерфейс модели создания статьи
    /// </summary>
    public interface IPostUpdateModel : IPostRequestModel
    {
        int Id { get; set; }
    }
}
