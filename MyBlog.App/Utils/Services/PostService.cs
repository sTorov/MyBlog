using AutoMapper;
using MyBlog.App.Controllers;
using MyBlog.App.Utils.Extensions;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Posts;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Users;
using MyBlog.Data.Repositories;
using MyBlog.Data.Repositories.Interfaces;

namespace MyBlog.App.Utils.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        private readonly PostRepository _postRepository;

        public PostService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;

            _postRepository = GetPostRepository();
        }

        private PostRepository GetPostRepository() => (PostRepository)_unitOfWork.GetRepository<Post>();

        public async Task<User?> CheckDataAtCreated(PostController controller, PostCreateViewModel model)
        {
            //var user = User;
            //_ = int.TryParse(_userManager.GetUserId(user), out int userId);
            //model.UsertId = userId;

            var creater = await _userService.GetUserByIdAsync(model.UsertId);
            if (creater == null)
                controller.ModelState.AddModelError(string.Empty, $"Пользователя с ID [{model.UsertId}] не существует!");
            return creater;
        }

        /// <summary>
        /// Доработать (User Auth)
        /// </summary>
        public async Task CreatePost(User user, PostCreateViewModel model)
        {
            var post = _mapper.Map<Post>(model);
            post.User = user;

            await _postRepository.CreateAsync(post);
        }

        public async Task<PostsViewModel> GetPostViewModel(int? userId) 
        {
            var model = new PostsViewModel();

            model.Posts = userId == null
                ? model.Posts = await _postRepository.GetAllAsync()
                : model.Posts = await _postRepository.GetPostsByUserIdAsync((int)userId);  
            
            return model;
        }

        public async Task<PostEditViewModel?> GetPostEditViewModel(int id)
        {
            var post = await GetPostByIdAsync(id);
            var model = post == null ? null : _mapper.Map<PostEditViewModel>(post);

             return model;
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

        public async Task<bool> UpdatePostAsync(PostEditViewModel model)
        {
            var currentPost = await GetPostByIdAsync(model.Id);
            if (currentPost == null)
                return false;

            currentPost.Convert(model);
            await _postRepository.UpdateAsync(currentPost);
            return true;
        }
    }
}
