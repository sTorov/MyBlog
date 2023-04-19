using AutoMapper;
using MyBlog.App.Controllers;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Comments;
using MyBlog.Data.DBModels.Comments;
using MyBlog.App.Utils.Extensions;
using MyBlog.Data.Repositories;
using MyBlog.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<bool> DeleteCommentAsync(int id, int? userId, bool fullAccess)
        {
            var deletedComment = await GetCommentByIdAsync(id);
            var check = fullAccess
                ? deletedComment != null
                : deletedComment != null && deletedComment.UserId == userId;

            if (!check) return false;

            if(await _commentRepository.DeleteAsync(deletedComment!) == 0) return false;
            return true;
        }

        public async Task<CommentEditViewModel?> GetCommentEditViewModelAsync(int id, int? userId, bool fullAccess)
        {
            var comment = await GetCommentByIdAsync(id);
            var check = fullAccess
                ? comment != null
                : comment != null && comment.UserId == userId;

            var model = !check ? null : _mapper.Map<CommentEditViewModel>(comment);

            return model;
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
    }
}
