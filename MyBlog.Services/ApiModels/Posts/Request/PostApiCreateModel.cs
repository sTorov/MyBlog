using MyBlog.Services.ViewModels.Posts.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ApiModels.Posts.Request
{
    /// <summary>
    /// Модель обновления статьи для API
    /// </summary>
    public class PostApiCreateModel : IPostCreateModel
    {
        public int UserId { get; set; }
        public string? PostTags { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Добавьте контент!")]
        public string Content { get; set; }

    }
}
