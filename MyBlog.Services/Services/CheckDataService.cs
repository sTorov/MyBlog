using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Roles;
using MyBlog.Data.DBModels.Tags;
using MyBlog.Data.DBModels.Users;
using MyBlog.Services.ApiModels.Users.Request;
using MyBlog.Services.Services.Interfaces;
using MyBlog.Services.ViewModels.Posts.Response;
using MyBlog.Services.ViewModels.Roles.Response;
using MyBlog.Services.ViewModels.Tags.Interfaces;
using MyBlog.Services.ViewModels.Tags.Response;
using MyBlog.Services.ViewModels.Users.Response;

namespace MyBlog.Services.Services
{
    /// <summary>
    /// Сервис проверки данных
    /// </summary>
    public class CheckDataService : ICheckDataService
    {
        private readonly IPostService _postService;
        private readonly RoleManager<Role> _roleManager;
        private readonly ITagService _tagService;
        private readonly UserManager<User> _userManager;

        public CheckDataService(IPostService postService, RoleManager<Role> roleManager, ITagService tagService, UserManager<User> userManager)
        {
            _postService = postService;
            _roleManager = roleManager;
            _tagService = tagService;
            _userManager = userManager;
        }

        #region PostController
        public async Task<Post?> CheckDataForUpdatePostAsync(Controller controller, PostEditViewModel model)
        {
            var currentPost = await _postService.GetPostByIdAsync(model.Id);
            if (currentPost == null)
                controller.ModelState.AddModelError(string.Empty, $"Статья с Id [{model.Id}] не найдена!");

            return currentPost;
        }
        #endregion


        #region RoleController
        public async Task<Role?> CheckDataForEditRoleAsync(Controller controller, RoleEditViewModel model)
        {
            var checkRole = await _roleManager.FindByIdAsync(model.Id.ToString());
            if (checkRole == null)
                controller.ModelState.AddModelError(string.Empty, $"Роль с Id [{model.Id}] не найдена!");

            return checkRole;
        }

        public async Task<Role?> CheckDataForCreateRoleAsync(Controller controller, RoleCreateViewModel model)
        {
            var checkRole = await _roleManager.FindByNameAsync(model.Name);
            if (checkRole != null)
                controller.ModelState.AddModelError(string.Empty, $"Роль с именем [{model.Name}] уже существует!");

            return checkRole;
        }
        #endregion


        #region TagController
        public async Task<Tag?> CheckTagNameAsync<T>(Controller controller, T model) where T : ITagResponseViewModel
        {
            var checkTag = await _tagService.GetTagByNameAsync(model.Name);
            var check = model is TagEditViewModel editModel
                ? (checkTag != null && checkTag.Id != editModel.Id) : checkTag != null;

            if (check)
                controller.ModelState.AddModelError(string.Empty, $"Тег с именем [{model.Name}] уже существует!");

            return checkTag;
        }
        #endregion


        #region UserController
        public async Task CheckDataForCreateUserAsync(Controller controller, UserRegisterViewModel model)
        {
            var checkName = (await _userManager.FindByNameAsync(model.Login))?.UserName;
            if (checkName != null)
                controller.ModelState.AddModelError(string.Empty, $"Пользователь с никнеймом [{model.Login}] уже существует!");

            var checkEmail = (await _userManager.FindByEmailAsync(model.EmailReg))?.Email;
            if (checkEmail != null)
                controller.ModelState.AddModelError(string.Empty, $"Адрес [{model.EmailReg}] уже зарегистрирован!");
        }

        public async Task<string> CheckDataForCreateUserAsync(UserApiCreateModel model)
        {
            var checkName = (await _userManager.FindByNameAsync(model.Login))?.UserName;
            if (checkName != null) return $"Логин {model.Login} уже используется!";

            var checkEmail = (await _userManager.FindByEmailAsync(model.EmailReg))?.Email;
            if (checkEmail != null) return $"Почта {model.EmailReg} уже зарегистрирована!";

            return string.Empty;
        }

        public async Task<User?> CheckDataForEditUserAsync(Controller controller, UserEditViewModel model)
        {
            var currentUser = await _userManager.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == model.Id);
            if (currentUser == null)
            {
                controller.ModelState.AddModelError(string.Empty, $"Произошла непредвиденная ошибка! Пользователь не найден!");
                return null;
            }

            var checkLogin = (await _userManager.FindByNameAsync(model.Login))?.UserName;
            if (checkLogin != null && checkLogin != currentUser.UserName)
                controller.ModelState.AddModelError(string.Empty, $"Никнейм [{model.Login}] уже используется!");

            var checkEmail = (await _userManager.FindByEmailAsync(model.Email))?.Email;
            if (checkEmail != null && checkEmail != currentUser.Email)
                controller.ModelState.AddModelError(string.Empty, $"Адрес [{model.Email}] уже зарегистрирован!");

            return currentUser;
        }

        public async Task<User?> CheckDataForLoginAsync(Controller controller, UserLoginViewModel model)
        {
            var user = await _userManager.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Email == model.UserEmail);
            if (user == null)
                controller.ModelState.AddModelError(string.Empty, "Неверный email или(и) пароль!");

            return user;
        }
        #endregion
    }
}
