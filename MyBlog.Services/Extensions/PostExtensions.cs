using MyBlog.Services.ViewModels.Posts.Response;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Tags;
using System.Text.RegularExpressions;

namespace MyBlog.Services.Extensions
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
