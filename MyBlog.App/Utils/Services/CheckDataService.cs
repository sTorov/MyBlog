using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data.DBModels.Comments;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Users;
using MyBlog.Data.Repositories;
using MyBlog.Data.Repositories.Interfaces;

namespace MyBlog.App.Utils.Services
{
    public class CheckDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager; 

        public CheckDataService(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<bool> CheckPostForUser(Controller controller, int postId)
        {
            if (controller.User.IsInRole("Admin")) return true;

            var userName = controller.User!.Identity!.Name;

            var currentUser = await _userManager.Users
                .Include(u => u.Posts).FirstOrDefaultAsync(u => u.UserName == userName);
            if (currentUser == null) return false;

            var post = await ((PostRepository)_unitOfWork.GetRepository<Post>()).GetAsync(postId);
            if(post == null) return false;

            return currentUser.Posts.Any(c => c.Id == postId);
        }

        public async Task<bool> CheckCommentForUser(Controller controller, int commentId)
        {
            if (controller.User.IsInRole("Admin")) return true;

            var userName = controller.User!.Identity!.Name;

            var currentUser = await _userManager.Users
                .Include(u => u.Comments).FirstOrDefaultAsync(u => u.UserName == userName);
            if (currentUser == null) return false;

            var comment = ((CommentRepository)_unitOfWork.GetRepository<Comment>()).GetAsync(commentId);
            if (comment == null) return false;

            return currentUser.Comments.Any(c => c.Id == commentId);
        }

        public async Task<bool> CheckEditUser(Controller controller, int userId)
        {
            if (controller.User.IsInRole("Admin")) return true;

            var userName = controller.User.Identity!.Name!;
            var currentUser = await _userManager.FindByNameAsync(userName);

            if(currentUser == null || currentUser.Id != userId)
                return false;
            return true;
        }
    }
}
