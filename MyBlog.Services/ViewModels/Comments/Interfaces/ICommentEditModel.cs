namespace MyBlog.Services.ViewModels.Comments.Interfaces
{
    /// <summary>
    /// Интерфейс модели обновления комментария
    /// </summary>
    public interface ICommentEditModel
    {
        int Id { get; set; }
        string Text { get; set; }
    }
}
