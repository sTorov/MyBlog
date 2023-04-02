using AutoMapper;
using MyBlog.App.Controllers;
using MyBlog.App.Utils.Extensions;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Posts;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Tags;
using MyBlog.Data.DBModels.Users;
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

        private readonly PostRepository _postRepository;
        private readonly TagRepository _tagRepository;

        public PostService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;

            _postRepository = (PostRepository)_unitOfWork.GetRepository<Post>();
            _tagRepository = (TagRepository)_unitOfWork.GetRepository<Tag>();
        }

        public async Task<User?> CheckDataAtCreated(PostController controller, PostCreateViewModel model)
        {
            //var user = User;
            //_ = int.TryParse(_userManager.GetUserId(user), out int userId);
            //model.UsertId = userId;

            var creater = await _userService.GetUserByIdAsync(model.UserId);
            if (creater == null)
                controller.ModelState.AddModelError(string.Empty, $"Пользователя с ID [{model.UserId}] не существует!");
            return creater;
        }

        /// <summary>
        /// Доработать (User Auth)
        /// </summary>
        public async Task CreatePost(User user, PostCreateViewModel model, List<Tag> tags)
        {
            var post = _mapper.Map<Post>(model);
            post.User = user;
            if(tags != null) post.Tags = tags;

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
            if (!string.IsNullOrEmpty(model.PostTags))
            {
                var list = await CreateTagAtPostAsync(model.PostTags);
                currentPost.Tags = list;
            }

            await _postRepository.UpdateAsync(currentPost);
            return true;
        }

        public async Task<List<Tag>> CreateTagAtPostAsync(string postTags)
        {
            var normalizedStringTags = Regex.Replace(postTags, @"\s*", "");
            var tagSetName = normalizedStringTags.Split(",").ToHashSet();
            
            var allTags = (await _tagRepository.GetAllAsync()).Select(t => t.Name);

            var createdTags = tagSetName.Except(allTags);
            foreach (var tag in createdTags)
                await _tagRepository.CreateAsync(new Tag(tag));
            
            var tags = new List<Tag>();
            foreach (var tagName in tagSetName)
            {
                var tag = await _tagRepository.GetTagByNameAsync(tagName);
                if(tag != null) tags.Add(tag);
            }

            return tags;
        }
    }
}
