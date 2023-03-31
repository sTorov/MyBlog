using AutoMapper;
using MyBlog.App.Controllers;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Comments;
using MyBlog.Data.DBModels.Comments;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Users;
using MyBlog.App.Utils.Extensions;
using MyBlog.Data.Repositories;
using MyBlog.Data.Repositories.Interfaces;

namespace MyBlog.App.Utils.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IPostService _postService;

        private readonly CommentRepository _commentRepository;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService, IPostService postService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _postService = postService;

            _commentRepository = GetCommentRepository();
        }

        private CommentRepository GetCommentRepository() => (CommentRepository)_unitOfWork.GetRepository<Comment>();

        /// <summary>
        /// Переделать
        /// </summary>
        public async Task<(User?, Post?)> CheckDataAtCreateComment(CommentController controller, CommentCreateViewModel model)
        {
            var checkUser = await _userService.GetUserByIdAsync(model.UserId);
            if (checkUser == null)
                controller.ModelState.AddModelError(string.Empty, $"Пользователь с ID [{model.UserId}] не найден!");

            var checkPost = await _postService.GetPostByIdAsync(model.PostId);
            if (checkPost == null)
                controller.ModelState.AddModelError(string.Empty, $"Пост с ID [{model.PostId}] не найден!");

            return (checkUser, checkPost);
        }

        public async Task CreateComment(User user, Post post, CommentCreateViewModel model)
        {
            var comment = _mapper.Map<Comment>(model);
            comment.Post = post;
            comment.User = user;

            await _commentRepository.CreateAsync(comment);
        }

        public async Task<CommentsViewModel> GetCommentsViewModel(int? postId)
        {
            var model = new CommentsViewModel
            {
                Comments = postId == null
                ? await _commentRepository.GetAllAsync()
                : await _commentRepository.GetCommentsByPostId((int)postId)
            };

            return model;
        }

        public async Task<Comment?> GetCommentByIdAsync(int id) => await _commentRepository.GetAsync(id);

        public async Task<bool> DeleteComment(int id)
        {
            var deletedComment = await GetCommentByIdAsync(id);
            if (deletedComment == null)
                return false;

            await _commentRepository.DeleteAsync(deletedComment);
            return true;
        }

        public async Task<CommentEditViewModel?> GetCommentEditViewModel(int id)
        {
            var comment = await GetCommentByIdAsync(id);
            var model = comment == null ? null : _mapper.Map<CommentEditViewModel>(comment);

            return model;
        }

        public async Task<bool> UpdateComment(CommentEditViewModel model)
        {
            var currentComment = await GetCommentByIdAsync(model.Id);
            if (currentComment == null)
                return false;

            currentComment.Convert(model);
            await _commentRepository.UpdateAsync(currentComment);
            return true;
        }
    }
}
