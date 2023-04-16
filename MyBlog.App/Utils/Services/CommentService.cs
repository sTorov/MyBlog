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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

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

            _commentRepository = (CommentRepository)_unitOfWork.GetRepository<Comment>();
        }

        public async Task<IActionResult?> CheckDataAtCreateComment(CommentController controller)
        {
            if (!int.TryParse(controller.Request.Query["userId"].ToString(), out int userId) ||
                await _userService.GetUserByIdAsync(userId) == null)
                    return new NotFoundResult();

            if (!int.TryParse(controller.Request.Query["postId"].ToString(), out int postId) ||
                await _postService.GetPostByIdAsync(postId) == null)
                    return new NotFoundResult();

            return null;
        }

        public async Task<bool> CreateComment(CommentCreateViewModel model)
        {
            var user = await _userService.GetUserByIdAsync(model.UserId);
            if (user == null) return false;

            var post = await _postService.GetPostByIdAsync(model.PostId);
            if (post == null) return false;

            var comment = _mapper.Map<Comment>(model);
            comment.Post = post;
            comment.User = user;

            await _commentRepository.CreateAsync(comment);
            return true;
        }

        public async Task<CommentsViewModel> GetCommentsViewModel(int? postId, string? userId)
        {
            var model = new CommentsViewModel();

            if (postId == null && userId == null)
                model.Comments = await _commentRepository.GetAllAsync();
            else if (postId != null && userId == null)
                model.Comments = await _commentRepository.GetCommentsByPostId((int)postId);
            else if (postId == null && userId != null)
                model.Comments = await _commentRepository.GetCommentsByUserId(Helper.GetIntValue(userId));
            else
                model.Comments = (await _commentRepository.GetCommentsByPostId((int)postId!))
                    .Where(c => c.UserId == Helper.GetIntValue(userId!)).ToList();
            
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
