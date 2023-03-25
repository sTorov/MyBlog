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

        public UserController(SignInManager<User> signInManager, IUserService userService)
        {
            _signInManager = signInManager;
            _userService = userService;
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Register(UserRegisterViewModel? model = null) => View(model);

        [HttpPost]
        public async Task<IActionResult> PostRegister(UserRegisterViewModel model)
        {
            await _userService.CheckDataAtRegistration(this, model);

            if (ModelState.IsValid)
            {
                var result = await _userService.CreateUserAsync(model);

                if (result.Succeeded)
                {
                    //заглушка(SignIn)
                    return RedirectToAction("GetUser");
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
                    }
                }
            }
            return View(model);
        }
    }
}
