using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.Utils.Extensions;
using MyBlog.App.ViewModels.Users;
using MyBlog.Data.DBModels.Users;

namespace MyBlog.App.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Register(UserRegisterViewModel model = null)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PostRegister(UserRegisterViewModel model)
        {
            var checkName = (await _userManager.FindByNameAsync(model.Login))?.UserName;
            if (checkName != null)
                ModelState.AddModelError(string.Empty, $"Пользователь с никнеймом [{model.Login}] уже существует!");

            var checkEmail = (await _userManager.FindByEmailAsync(model.EmailReg))?.Email;
            if (checkEmail != null)
                ModelState.AddModelError(string.Empty, $"Адрес [{model.EmailReg}] уже зарегистрирован!");

            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);
                var result = await _userManager.CreateAsync(user, model.PasswordReg);

                if (result.Succeeded)
                {
                    //заглушка
                    return Json(user);
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
            var model = new UsersVIewModel();
            if (id == null)
                model.Users = _userManager.Users.ToList();
            else
            {
                var user = await _userManager.FindByIdAsync(id?.ToString() ?? string.Empty);
                if (user != null) model.Users.Add(user);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null) await _userManager.DeleteAsync(user);

            return RedirectToAction("GetUser");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                var model = _mapper.Map<UserEditViewModel>(user);
                return View(model);
            }
            else
                return RedirectToAction("GetUser");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            var currentUser = await _userManager.FindByIdAsync(model.Id.ToString());
            if (currentUser != null)
            {
                var checkLogin = (await _userManager.FindByNameAsync(model.Login))?.UserName;
                if (checkLogin != null && checkLogin != currentUser.UserName)
                    ModelState.AddModelError(string.Empty, $"Никнейм [{model.Login}] уже используется!");

                var checkEmail = (await _userManager.FindByEmailAsync(model.Email))?.Email;
                if (checkEmail != null && checkEmail != currentUser.Email)
                    ModelState.AddModelError(string.Empty, $"Адрес [{model.Email}] уже зарегистрирован!");

                if (ModelState.IsValid)
                {
                    currentUser.Convert(model);

                    var result = await _userManager.UpdateAsync(currentUser);

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
