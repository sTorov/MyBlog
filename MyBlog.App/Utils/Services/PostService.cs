using AutoMapper;
using MyBlog.App.Controllers;
using MyBlog.App.Utils.Extensions;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Posts;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Tags;
using MyBlog.Data.Repositories;
using MyBlog.Data.Repositories.Interfaces;
using System.Text.RegularExpressions;

namespace MyBlog.App.Utils.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ITagService _tagService;

        private readonly PostRepository _postRepository;

        public PostService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService, ITagService tagService) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _tagService = tagService;

            _postRepository = (PostRepository)_unitOfWork.GetRepository<Post>();
        }

        public async Task<bool> CreatePost(PostCreateViewModel model, List<Tag>? tags)
        {
            var user = await _userService.GetUserByIdAsync(model.UserId);
            if (user == null) return false;

            var post = _mapper.Map<Post>(model);
            post.User = user;
            if(tags != null) post.Tags = tags;

            await _postRepository.CreateAsync(post);
            return true;
        }

        public async Task<PostsViewModel> GetPostViewModel(int? postId, string? userId) 
        {
            var model = new PostsViewModel();

            if (postId == null && userId == null)
                model.Posts = await _postRepository.GetAllAsync();
            else if (postId == null && userId != null)
                model.Posts = await _postRepository.GetPostsByUserIdAsync(Helper.GetIntValue(userId));
            else
            {
                var post = await _postRepository.GetAsync(postId ?? 0);
                if (post != null)
                    model.Posts = new List<Post> { post };
            }
            
            return model;
        }

        public async Task<PostEditViewModel?> GetPostEditViewModel(int id, string? userId, bool fullAccess)
        {
            var post = await GetPostByIdAsync(id);
            var check = fullAccess
                ? post != null
                : post != null && post.UserId == Helper.GetIntValue(userId!);

            if (!check)
                return null;
            return _mapper.Map<PostEditViewModel>(post);
        }

        public async Task<Post?> GetPostByIdAsync(int id) => await _postRepository.GetAsync(id);

        public async Task<bool> DeletePost(int id)
        {
            var post = await GetPostByIdAsync(id);
            if (post == null)
                return false;

            await _postRepository.DeleteAsync(post!);
            return true;
        }

        public async Task<bool> UpdatePostAsync(PostEditViewModel model, Post post)
        {
            post.Convert(model);
            if (!string.IsNullOrEmpty(model.PostTags))
                post.Tags = await _tagService.CreateTagForPostAsync(model.PostTags) ?? new List<Tag>();

            await _postRepository.UpdateAsync(post);
            return true;
        }

        public async Task<Post?> CheckDataAtUpdatePostAsync(PostController controller, PostEditViewModel model)
        {
            var currentPost = await GetPostByIdAsync(model.Id);
            if (currentPost == null)
                controller.ModelState.AddModelError(string.Empty, $"Статья с Id [{model.Id}] не найдена!");

            return currentPost;
        }
    }
}
