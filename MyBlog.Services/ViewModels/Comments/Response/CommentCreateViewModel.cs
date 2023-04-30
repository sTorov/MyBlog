using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ViewModels.Comments.Response
{
    public class CommentCreateViewModel
    {
        public int UserId { get; set; }
        public int PostId { get; set; }

        [Required]
        [Display(Name = "Комментарий")]
        public string Text { get; set; }
    }
}
