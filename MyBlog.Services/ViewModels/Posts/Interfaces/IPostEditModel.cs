namespace MyBlog.Services.ViewModels.Posts.Interfaces
{
    /// <summary>
    /// Интерфейс модели создания статьи
    /// </summary>
    public interface IPostUpdateModel : IPostResponceModel
    {
        int Id { get; set; }
    }
}
