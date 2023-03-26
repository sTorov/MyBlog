using MyBlog.App.ViewModels.Posts;
using MyBlog.Data.DBModels.Posts;

namespace MyBlog.App.Utils.Extensions
{
    public static class PostExtensions
    {
        public static Post Convert(this Post post, PostEditViewModel model)
        {
            post.Title = model.Title;
            post.Content = model.Content;

            return post;
        }
    }
}
