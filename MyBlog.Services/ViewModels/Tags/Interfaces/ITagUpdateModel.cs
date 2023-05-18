namespace MyBlog.Services.ViewModels.Tags.Interfaces
{
    /// <summary>
    /// Интерфейс моделей для обновления тега
    /// </summary>
    public interface ITagUpdateModel : ITagRequestViewModel
    {
        int Id { get; set; }
    }
}
