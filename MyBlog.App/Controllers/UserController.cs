using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.Utils;
using MyBlog.App.Utils.Services;
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
        private readonly CheckDataService _checkDataService;

        public UserController(SignInManager<User> signInManager, IUserService userService, IRoleService roleService, CheckDataService checkDataService)
        {
            _signInManager = signInManager;
            _userService = userService;
            _roleService = roleService;
            _checkDataService = checkDataService;
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

        [Authorize]
        [HttpGet]
        [Route("GetUser/{id?}")]
        public async Task<IActionResult> GetUser([FromRoute] int? id = null)
        {
            var isAdmin = await _checkDataService.CheckEditUser(this, id ?? 0);

            var userName = User.Identity!.Name;
            var model = await _userService.GetUsersViewModelAsync(id, isAdmin, userName!);

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var check = await _checkDataService.CheckEditUser(this, id);
            if (!check)
                return Redirect(Constants.ACCESS_DENIED_PATH);

            _ = await _userService.DeleteByIdAsync(id);

            if(User.IsInRole("Admin"))
                return RedirectToAction("GetUser");

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!await _checkDataService.CheckEditUser(this, id))
                return Redirect(Constants.ACCESS_DENIED_PATH);

            var model = await _userService.GetUserEditViewModelAsync(id);
            if (model != null)
                return View(model);
            else
                return RedirectToAction("GetUser");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (!await _checkDataService.CheckEditUser(this, model.Id))
                return Redirect(Constants.ACCESS_DENIED_PATH);
        
            var currentUser = await _userService.CheckDataAtEdition(this, model);

            if (ModelState.IsValid)
            {
                var result = await _userService.UpdateUserAsync(model, currentUser!);

                if (result.Succeeded)
                    return RedirectToAction("GetUser");
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
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
