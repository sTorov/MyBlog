namespace MyBlog.Services.ViewModels
{
    /// <summary>
    /// Модель представления для подробного описания ошибки для разработчиков
    /// </summary>
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}