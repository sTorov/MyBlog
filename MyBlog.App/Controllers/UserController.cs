using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.Utils.Attributes;
using MyBlog.Data.DBModels.Users;
using MyBlog.Services.Services.Interfaces;
using MyBlog.Services.ViewModels.Users.Request;
using System.Security.Claims;

namespace MyBlog.App.Controllers
{
    /// <summary>
    /// Контроллер пользователей
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class UserController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ICheckDataService _checkDataService;

        public UserController(SignInManager<User> signInManager, IUserService userService, IRoleService roleService, ICheckDataService checkDataService)
        {
            _signInManager = signInManager;
            _userService = userService;
            _roleService = roleService;
            _checkDataService = checkDataService;
        }

        /// <summary>
        /// Страница регистрации пользователя
        /// </summary>
        [HttpGet]
        [Route("Register")]
        public IActionResult Register() => View();

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            await _checkDataService.CheckDataForCreateUserAsync(this, model);
            if (ModelState.IsValid)
            {
                var (result, user) = await _userService.CreateUserAsync(model);
                if (result)
                {
                    await _signInManager.SignInWithClaimsAsync(user, false, await _userService.GetUserClaimsAsync(user));
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError(string.Empty, $"Произошла ошибка при регистрации пользователя!");
            }

            return View(model);
        }

        /// <summary>
        /// Страница авторизации пользователя
        /// </summary>
        [HttpGet]
        [Route("Login")]
        public IActionResult Login() => View();

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        [HttpPost]
        [Route("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel model)
        {
            var user = await _checkDataService.CheckDataForLoginAsync(this, model);
            if (ModelState.IsValid)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user!, model.Password, false);
                if (result.Succeeded)
                {
                    var claims = await _userService.GetUserClaimsAsync(user!);
                    await _signInManager.SignInWithClaimsAsync(user!, false, claims);

                    var userId = claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl) && userId == Request.Query["UserId"])
                        return Redirect(model.ReturnUrl);
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError(string.Empty, "Неверный email или(и) пароль!");
            }

            return View(model);
        }

        /// <summary>
        /// Выход пользователя из системы
        /// </summary>
        [Authorize]
        [HttpPost]
        [Route("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout([FromQuery] string returnUrl)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", new { ReturnUrl = returnUrl, UserId = userId });
        }

        /// <summary>
        /// Повторная авторизация при отсутствии необходимых утветждений у пользователя
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("Refresh")]
        public async Task<IActionResult> Refresh(string returnUrl)
        {
            if (User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value == null)
            {
                var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? string.Empty;
                var user = await _userService.GetUserByNameAsync(userName);

                if (user != null)
                    await _signInManager.SignInWithClaimsAsync(user!, false, await _userService.GetUserClaimsAsync(user!));
            }

            if (returnUrl != null && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Страница всех пользователей (получение пользователей с указаной ролью)
        /// </summary>
        [Authorize(Roles = "Admin"), CheckUserId]
        [HttpGet]
        [Route("GetUsers/{roleId?}")]
        public async Task<IActionResult> GetUsers([FromRoute] int? roleId)
        {
            var model = await _userService.GetUsersViewModelAsync(roleId);
            return View(model);
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        [Authorize, CheckUserId]
        [HttpPost]
        public async Task<IActionResult> Remove(int id, [FromForm] int? userId)
        {
            var result = await _userService.DeleteByIdAsync(id, userId, User.IsInRole("Admin"));
            if (!result) return BadRequest();

            if (User.IsInRole("Admin")) 
                return RedirectToAction("GetUsers");

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Страница редактирования пользователя
        /// </summary>
        [Authorize, CheckUserId]
        [HttpGet]
        [Route("EditUser/{id?}")]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
            var (model, result) = await _userService.GetUserEditViewModelAsync(id, userId, User.IsInRole("Admin"));

            if (model == null) return result!;

            return View(model);
        }

        /// <summary>
        /// Редактирование пользователя
        /// </summary>
        [Authorize, CheckUserId]
        [HttpPost]
        [Route("EditUser/{id}")]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            await _checkDataService.CheckDataForEditUserAsync(this, model);
            if (ModelState.IsValid)
            {
                model.Roles = _roleService.GetEnabledRoleNamesWithRequest(this.Request);
                var result = await _userService.UpdateUserAsync(model);

                if (result)
                    return RedirectToAction("Index", "Home", new { model.ReturnUrl });
                else
                    ModelState.AddModelError(string.Empty, $"Произошла ошибка при обновлении пользователя!");
            }

            model.Roles ??= (await _roleService.GetRolesByUserAsync(model.Id)).Select(r => r.Name!).ToList();
            model.AllRoles ??= (await _roleService.GetAllRolesAsync()).Select(r => r.Name!).ToList();

            return View(model);
        }

        /// <summary>
        /// Страница отображения профиля пользователя
        /// </summary>
        [Authorize, CheckUserId]
        [HttpGet]
        [Route("ViewUser/{id}")]
        public async Task<IActionResult> View([FromRoute] int id)
        {
            var model = await _userService.GetUserViewModelAsync(id);
            if (model == null) return NotFound();

            return View(model);
        }

        /// <summary>
        /// Страница создания пользователя
        /// </summary>
        [Authorize(Roles = "Admin"), CheckUserId]
        [HttpGet]
        public async Task<IActionResult> Create() => View(new UserCreateViewModel(await _roleService.GetAllRolesAsync()));

        /// <summary>
        /// Создание пользователя
        /// </summary>
        [Authorize(Roles = "Admin"), CheckUserId]
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            await _checkDataService.CheckDataForCreateUserAsync(this, model);
            if (ModelState.IsValid)
            {
                model.Roles = _roleService.GetEnabledRoleNamesWithRequest(this.Request);
                var (result, _) = await _userService.CreateUserAsync(model, await _roleService.ConvertRoleNamesInRoles(model.Roles));

                if (result)
                    return RedirectToAction("GetUsers");
                else
                    ModelState.AddModelError(string.Empty, $"Произошла ошибка при создании пользователя!");
            }

            model.AllRoles ??= (await _roleService.GetAllRolesAsync()).Select(r => r.Name!).ToList();
            return View(model);
        }
    }
}
