using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Roles;
using MyBlog.Data.DBModels.Tags;
using MyBlog.Data.DBModels.Users;
using MyBlog.Services.ApiModels.Roles.Request;
using MyBlog.Services.ApiModels.Tags.Request;
using MyBlog.Services.ApiModels.Users.Request;
using MyBlog.Services.Services.Data;
using MyBlog.Services.Services.Interfaces;
using MyBlog.Services.ViewModels.Posts.Response;
using MyBlog.Services.ViewModels.Roles.Response;
using MyBlog.Services.ViewModels.Tags.Interfaces;
using MyBlog.Services.ViewModels.Tags.Response;
using MyBlog.Services.ViewModels.Users.Intefaces;
using MyBlog.Services.ViewModels.Users.Response;
using SQLitePCL;

namespace MyBlog.Services.Services
{
    /// <summary>
    /// Сервис проверки данных
    /// </summary>
    public class CheckDataService : ICheckDataService
    {
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly ITagService _tagService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public CheckDataService(IPostService postService, RoleManager<Role> roleManager, ITagService tagService, UserManager<User> userManager, ICommentService commentService)
        {
            _postService = postService;
            _tagService = tagService;
            _commentService = commentService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<string>> CheckEntitiesByIdAsync(int? postId = null, int? userId = null, int? roleId = null, int? tagId = null, int? commentId = null)
        {
            var list = new List<string>();

            if(postId != null)
            {
                var post = await _postService.GetPostByIdAsync((int)postId);
                if (post == null) list.Add($"Статья не найдена! Id = [{postId}]");
            }

            if(userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId.ToString()!);
                if (user == null) list.Add($"Пользователь не найден! Id = [{userId}]");
            }

            if(roleId != null)
            {
                var role = _roleManager.FindByIdAsync(roleId.ToString()!);
                if (role == null) list.Add($"Роль не найдена! Id = [{roleId}]");
            }

            if(tagId != null)
            {
                var tag = _tagService.GetTagByIdAsync((int)tagId);
                if (tag == null) list.Add($"Тег не найден! Id = [{tagId}]");
            }

            if(commentId != null)
            {
                var comment = _commentService.GetCommentByIdAsync((int)commentId);
                if (comment == null) list.Add($"Комментарий не найден! [{commentId}]");
            }

            return list;
        }

        public async Task<List<string>> CheckEntitiesByNameAsync(string? userName = null, string? roleName = null, string? tagName = null)
        {
            var list = new List<string>();

            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null) list.Add($"Пользователь не найден! Name = [{userName}]");
            }

            if (roleName != null)
            {
                var role = _roleManager.FindByNameAsync(roleName);
                if (role == null) list.Add($"Роль не найдена! Name = [{roleName}]");
            }

            if (tagName != null)
            {
                var tag = _tagService.GetTagByNameAsync(tagName);
                if (tag == null) list.Add($"Тег не найден! Name = [{tagName}]");
            }

            return list;
        }

        #region PostController
        public async Task CheckDataForUpdatePostAsync(Controller controller, PostEditViewModel model)
        {
            var messages = await CheckEntitiesByIdAsync(model.Id);

            if (messages.Count > 0)
                controller.ModelState.AddModelError(string.Empty, messages[0]);
        }
        #endregion


        #region RoleController
        public async Task CheckDataForEditRoleAsync(Controller controller, RoleEditViewModel model)
        {
            var messages = await CheckEntitiesByIdAsync(roleId: model.Id);

            if (messages.Count > 0)
                controller.ModelState.AddModelError(string.Empty, messages[0]);
        }

        public async Task CheckDataForCreateRoleAsync(Controller controller, RoleCreateViewModel model)
        {
            var messages = await CheckEntitiesByNameAsync(roleName: model.Name);

            if (messages.Count > 0)
                controller.ModelState.AddModelError(string.Empty, messages[0]);
        }

        public async Task<bool> CheckChangeDefaultRolesAsync(int roleId, string roleName = "")
        {
            if (DefaultRoles.DefaultRoleNames.Contains(roleName)) 
                return true;

            foreach(var defaultRoleName in DefaultRoles.DefaultRoleNames)
            {
                var role = await _roleManager.FindByNameAsync($"{defaultRoleName}");
                if (role != null && role.Id == roleId)
                    return false;
            }

            return true;
        }
        #endregion


        #region TagController
        public async Task CheckTagNameAsync<T>(Controller controller, T model) where T : ITagResponseViewModel
        {
            var message = await CheckTagNameAsync(model);

            if (message != string.Empty)
                controller.ModelState.AddModelError(string.Empty, message);
        }

        public async Task<string> CheckTagNameAsync<T>(T model) where T : ITagResponseViewModel
        {
            var checkTag = await _tagService.GetTagByNameAsync(model.Name);
            var check = model is ITagUpdateModel updateModel
                ? (checkTag != null && checkTag.Id != updateModel.Id) : checkTag != null;

            if (check)
                return $"Тег с именем [{model.Name}] уже существует!";
            return string.Empty;
        }
        #endregion


        #region UserController
        public async Task CheckDataForCreateUserAsync(Controller controller, UserRegisterViewModel model)
        {
            var messages = await CheckDataForCreateUserAsync(model);

            if(messages.Count > 0)
            {
                foreach (var message in messages)
                    controller.ModelState.AddModelError(string.Empty, message);
            }
        }

        public async Task<List<string>> CheckDataForCreateUserAsync(UserRegisterViewModel model)
        {
            var messages = await CheckEntitiesByNameAsync(userName: model.Login);

            var checkEmail = (await _userManager.FindByEmailAsync(model.EmailReg))?.Email;
            if (checkEmail != null) 
                messages.Add($"Почта {model.EmailReg} уже зарегистрирована!");

            return messages;
        }

        public async Task<User?> CheckDataForEditUserAsync(Controller controller, UserEditViewModel model)
        {
            var (currentUser, messages) = await CheckDataForEditUserAsync(model);

            if(messages.Count > 0)
            {
                foreach (var message in messages)
                    controller.ModelState.AddModelError(string.Empty, message);
            }

            return currentUser;
        }

        public async Task<(User?, List<string>)> CheckDataForEditUserAsync(IUserUpdateModel model)
        {
            var list = new List<string>();

            var currentUser = await _userManager.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == model.Id);
            if (currentUser == null)
            {
                list.Add($"Пользователь не найден!");
                return (null, list);
            }

            var checkLogin = (await _userManager.FindByNameAsync(model.Login))?.UserName;
            if (checkLogin != null && checkLogin != currentUser.UserName) 
                list.Add($"Никнейм [{model.Login}] уже используется!");

            var checkEmail = (await _userManager.FindByEmailAsync(model.Email))?.Email;
            if (checkEmail != null && checkEmail != currentUser.Email)
                list.Add($"Адрес [{model.Email}] уже зарегистрирован!");

            return (currentUser, list);
        }

        public async Task<User?> CheckDataForLoginAsync(Controller controller, UserLoginViewModel model)
        {
            var user = await _userManager.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Email == model.UserEmail);
            if (user == null)
                controller.ModelState.AddModelError(string.Empty, "Неверный email или(и) пароль!");

            return user;
        }

        public async Task<bool> CheckRolesForUserUpdateModel(IUserUpdateModel model)
        {
            foreach(var role in model.AllRoles)
            {
                if (await _roleManager.FindByNameAsync(role.Key) == null)
                    return false;
            }

            return true;
        }
        #endregion
    }
}
