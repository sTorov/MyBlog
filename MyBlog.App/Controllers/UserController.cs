using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> PostRegister(UserRegisterViewModel model)
        {
            await _userService.CheckDataAtRegistration(this, model);

            if (ModelState.IsValid)
            {
                var (result, user) = await _userService.CreateUserAsync(model);

                if (result.Succeeded)
                {
                    var claims = await _roleService.GetRoleClaims(user);

                    await _signInManager.SignInWithClaimsAsync(user, false, claims);
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
        public async Task<IActionResult> PostLogin(UserLoginViewModel model)
        {
            var user = await _userService.GetUserByEmailAsync(model.UserEmail);
            if(user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (result.Succeeded)
                {
                    await _signInManager.SignInWithClaimsAsync(user, false, await _roleService.GetRoleClaims(user));
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError(string.Empty, "Неверный email и(или) пароль!");
            }

            return View("Login", model);
        }

        [HttpPost]
        [Route("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("GetUser/{id?}")]
        public async Task<IActionResult> GetUser([FromRoute] int? id = null)
        {
            var model = new UsersViewModel();

            if (id == null)
                model.Users = await _userService.GetAllUsersAsync();
            else
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user != null) model.Users.Add(user);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            _ = await _userService.DeleteByIdAsync(id);

            return RedirectToAction("GetUser");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _userService.GetUserEditViewModelAsync(id);
            if (model != null)
                return View(model);
            else
                return RedirectToAction("GetUser");
        }

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
    }
}
