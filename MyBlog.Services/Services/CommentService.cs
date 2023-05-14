using AutoMapper;
using MyBlog.Services.Services.Interfaces;
using MyBlog.Data.DBModels.Comments;
using MyBlog.Services.Extensions;
using MyBlog.Data.Repositories;
using MyBlog.Data.Repositories.Interfaces;
using MyBlog.Services.ViewModels.Comments.Response;
using MyBlog.Services.ViewModels.Comments.Request;
using Microsoft.AspNetCore.Mvc;

namespace MyBlog.Services.Services
{
    /// <summary>
    /// Сервисы сущности комментария
    /// </summary>
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

            _commentRepository = (CommentRepository)_unitOfWork.GetRepository<Comment>();
        }

        public async Task<bool> CreateCommentAsync(CommentCreateViewModel model)
        {
            var user = await _userService.GetUserByIdAsync(model.UserId);
            if (user == null) return false;

            var post = await _postService.GetPostByIdAsync(model.PostId);
            if (post == null) return false;

            var comment = _mapper.Map<Comment>(model);
            comment.Post = post;
            comment.User = user;

            if(await _commentRepository.CreateAsync(comment) == 0) return false;
            return true;
        }

        public async Task<CommentsViewModel> GetCommentsViewModelAsync(int? postId, int? userId)
        {
            var model = new CommentsViewModel();

            if (postId == null && userId == null)
                model.Comments = await _commentRepository.GetAllAsync();
            else if (postId != null && userId == null)
                model.Comments = await _commentRepository.GetCommentsByPostIdAsync((int)postId);
            else if (postId == null && userId != null)
                model.Comments = await _commentRepository.GetCommentsByUserIdAsync((int)userId);
            else
                model.Comments = (await _commentRepository.GetCommentsByPostIdAsync((int)postId!))
                    .Where(c => c.UserId == (int)userId!).ToList();
            
            return model;
        }

        public async Task<Comment?> GetCommentByIdAsync(int id) => await _commentRepository.GetAsync(id);

        public async Task<(IActionResult?, bool)> DeleteCommentAsync(int id, int? userId, bool fullAccess)
        {
            var deletedComment = await GetCommentByIdAsync(id);
            if (deletedComment == null) return (new NotFoundResult(), false);

            if(fullAccess || deletedComment.UserId == userId)
            {
                if (await _commentRepository.DeleteAsync(deletedComment!) == 0)
                    return (new BadRequestResult(), false);

                return (null, true);
            }
            
            return (new ForbidResult(), false);
        }

        public async Task<(CommentEditViewModel?, IActionResult?)> GetCommentEditViewModelAsync(int id, string? userId, bool fullAccess)
        {
            var comment = await GetCommentByIdAsync(id);
            if (comment == null) return (null, new NotFoundResult());

            if (fullAccess || comment.UserId.ToString() == userId)
                return (_mapper.Map<CommentEditViewModel>(comment), null);

            return (null, new ForbidResult());
        }

        public async Task<bool> UpdateCommentAsync(CommentEditViewModel model)
        {
            var currentComment = await GetCommentByIdAsync(model.Id);
            if (currentComment == null)
                return false;

            currentComment.Convert(model);
            if(await _commentRepository.UpdateAsync(currentComment) == 0) return false;
            return true;
        }

        public async Task<List<Comment>> GetAllCommentsByPostIdAsync(int postId) =>
            await _commentRepository.GetCommentsByPostIdAsync(postId);
    }
}
