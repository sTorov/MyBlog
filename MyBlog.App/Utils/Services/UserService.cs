using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.App.Controllers;
using MyBlog.App.Utils.Extensions;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Users;
using MyBlog.Data.DBModels.Users;

namespace MyBlog.App.Utils.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IdentityResult> CreateUserAsync(UserRegisterViewModel model)
        {
            var user = _mapper.Map<User>(model);
            return await _userManager.CreateAsync(user, model.PasswordReg);
        }

        public async Task<IdentityResult> UpdateUserAsync(UserEditViewModel model, User user)
        {
            user.Convert(model);
            return await _userManager.UpdateAsync(user);
        }

        public async Task<List<User>> GetAllUsersAsync() => await _userManager.Users.ToListAsync();

        public async Task<User?> GetUserByIdAsync(int? id) => await _userManager.FindByIdAsync(id?.ToString() ?? string.Empty);

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null) 
                return (await _userManager.DeleteAsync(user)).Succeeded;
            return false;
        }

        public async Task<UserEditViewModel?> GetUserEditViewModelAsync(int id)
        {
            var user = await GetUserByIdAsync(id);
            if(user != null)
                return _mapper.Map<UserEditViewModel>(user);
            return null;
        }

        public async Task CheckDataAtRegistration(UserController controller, UserRegisterViewModel model)
        {
            var checkName = (await _userManager.FindByNameAsync(model.Login))?.UserName;
            if (checkName != null)
                controller.ModelState.AddModelError(string.Empty, $"Пользователь с никнеймом [{model.Login}] уже существует!");

            var checkEmail = (await _userManager.FindByEmailAsync(model.EmailReg))?.Email;
            if (checkEmail != null)
                controller.ModelState.AddModelError(string.Empty, $"Адрес [{model.EmailReg}] уже зарегистрирован!");
        }

        public async Task CheckDataAtEdition(UserController controller, UserEditViewModel model, User currentUser)
        {
            var checkLogin = (await _userManager.FindByNameAsync(model.Login))?.UserName;
            if (checkLogin != null && checkLogin != currentUser.UserName)
                controller.ModelState.AddModelError(string.Empty, $"Никнейм [{model.Login}] уже используется!");

            var checkEmail = (await _userManager.FindByEmailAsync(model.Email))?.Email;
            if (checkEmail != null && checkEmail != currentUser.Email)
                controller.ModelState.AddModelError(string.Empty, $"Адрес [{model.Email}] уже зарегистрирован!");
        }

    }
}
