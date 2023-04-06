using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Users;
using MyBlog.Data.DBModels.Roles;
using MyBlog.Data.DBModels.Users;
using System.Net;

namespace MyBlog.App.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UserController(SignInManager<User> signInManager, IUserService userService, IRoleService roleService)
        {
            _signInManager = signInManager;
            _userService = userService;
            _roleService = roleService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Register")]
        public IActionResult Register() => View();

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> PostRegister(UserRegisterViewModel model)
        {
            await _userService.CheckDataAtRegistration(this, model);

            if (ModelState.IsValid)
            {
                var (result, user) = await _userService.CreateUserAsync(model);

                if (result.Succeeded)
                {
                    await _signInManager.SignInWithClaimsAsync(user, false, _roleService.GetRoleClaims(user));
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View("Register", model);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Login")]
        public IActionResult Login() => View();

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostLogin(UserLoginViewModel model)
        {
            var user = await _userService.GetUserByEmailAsync(model.UserEmail);

            if(user != null)
            {
                user.Roles = await _roleService.GetRolesByUserAsync(user.Id);
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (result.Succeeded)
                {
                    await _signInManager.SignInWithClaimsAsync(user, false, _roleService.GetRoleClaims(user));
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError(string.Empty, "Неверный email и(или) пароль!");
            }

            return View("Login", model);
        }

        [Authorize]
        [HttpPost]
        [Route("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetUser/{id?}")]
        public async Task<IActionResult> GetUser([FromRoute] int? id = null)
        {
            var model = new UsersViewModel();

            if (id == null)
                model.Users = await _userService.GetAllUsersAsync();
            else
            {
                var user = await _userService.GetUserByIdAsync((int)id);
                if (user != null) model.Users.Add(user);
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            _ = await _userService.DeleteByIdAsync(id);

            return RedirectToAction("GetUser");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _userService.GetUserEditViewModelAsync(id);
            if (model != null)
                return View(model);
            else
                return RedirectToAction("GetUser");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            var currentUser = await _userService.GetUserByIdAsync(model.Id);
            if (currentUser != null)
            {
                await _userService.CheckDataAtEdition(this, model, currentUser);

                if (ModelState.IsValid)
                {
                    var result = await _userService.UpdateUserAsync(model, currentUser);

                    if (result.Succeeded)
                        return RedirectToAction("GetUser");
                    else
                    {
                        foreach (var error in result.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);

                        return View(model);
                    }
                }
            }
            return RedirectToAction("GetUser");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetRoles(int id)
        {
            var model = await _roleService.GetUserRolesViewModelAsync(id);
            if(model == null)
                return RedirectToAction("GetUser");

            return View("Roles", model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SetRoles(UserRolesViewModel model)
        {
            var roles = await _roleService.GetRolesFromModelAsync(model);
            var user = await _userService.GetUserByIdAsync(model.UserId);

            if (user != null)
            {
                user.Roles = roles;
                await _userService.UpdateUserAsync(user);
            }

            return RedirectToAction("GetUser");
        }
    }
}
