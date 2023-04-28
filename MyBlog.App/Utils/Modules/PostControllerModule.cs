using MyBlog.App.Controllers;
using MyBlog.App.Utils.Modules.Interfaces;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Services.Services.Interfaces;
using MyBlog.Services.ViewModels.Posts.Response;

namespace MyBlog.App.Utils.Modules
{
    public class PostControllerModule : IPostControllerModule
    {
        private readonly IPostService _postService;

        public PostControllerModule(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<Post?> CheckDataAtUpdatePostAsync(PostController controller, PostEditViewModel model)
        {
            var currentPost = await _postService.GetPostByIdAsync(model.Id);
            if (currentPost == null)
                controller.ModelState.AddModelError(string.Empty, $"Статья с Id [{model.Id}] не найдена!");

            return currentPost;
        }
    }
}
