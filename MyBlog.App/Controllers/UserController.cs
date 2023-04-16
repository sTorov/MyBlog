using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.Utils.Attributes;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Users;
using MyBlog.Data.DBModels.Users;

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

        [HttpGet]
        [Route("Register")]
        public IActionResult Register() => View();

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            await _userService.CheckDataAtRegistration(this, model);
            if (ModelState.IsValid)
            {
                var (result, user) = await _userService.CreateUserAsync(model);
                if (result.Succeeded)
                {
                    await _signInManager.SignInWithClaimsAsync(user, false, await _userService.GetClaims(user));
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

        [HttpGet]
        [Route("Login")]
        public IActionResult Login() => View();

        [HttpPost]
        [Route("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel model)
        {
            var user = await _userService.GetUserByEmailAsync(model.UserEmail);
            if (user == null)
                ModelState.AddModelError(string.Empty, "Неверный email или(и) пароль!");

            if(ModelState.IsValid)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user!, model.Password, false);

                if (result.Succeeded)
                {
                    await _signInManager.SignInWithClaimsAsync(user!, false, await _userService.GetClaims(user!));

                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        return Redirect(model.ReturnUrl);
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError(string.Empty, "Неверный email или(и) пароль!");
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [Route("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string returnUrl)
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", new { returnUrl });
        }

        [Authorize, CheckUserId(parameterName: "id", actionName: "GetUser", fullAccess: "Admin")]
        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser(int? id = null)
        {
            var model = await _userService.GetUsersViewModelAsync(id);
            return View(model);
        }

        [Authorize, CheckUserId(parameterName: "id", fullAccess: "Admin")]
        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            _ = await _userService.DeleteByIdAsync(id);

            if(User.IsInRole("Admin"))
                return RedirectToAction("GetUser");

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize, CheckUserId(parameterName: "id", actionName: "EditUser", fullAccess: "Admin")]
        [HttpGet]
        [Route("EditUser")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _userService.GetUserEditViewModelAsync(id);
            if (model != null)
                return View(model);
            else
                return NotFound();
        }

        [Authorize]
        [HttpPost]
        [Route("EditUser")]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
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
