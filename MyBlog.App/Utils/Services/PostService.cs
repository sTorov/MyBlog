using AutoMapper;
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

        public async Task<PostEditViewModel?> GetPostEditViewModel(int id, string? userId)
        {
            var post = await GetPostByIdAsync(id);
            if (post != null && post.UserId == Helper.GetIntValue(userId!))
                return _mapper.Map<PostEditViewModel>(post);

            return null;
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

        public async Task<List<Tag>?> CreateTagAtPostAsync(string? postTags)
        {
            if (postTags == null) return null;
            
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
