using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.Utils.Attributes;
using MyBlog.App.Utils.Modules.Interfaces;
using MyBlog.Data.DBModels.Users;
using MyBlog.Services.Services.Interfaces;
using MyBlog.Services.ViewModels.Users.Response;

namespace MyBlog.App.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IUserControllerModule _module;

        public UserController(SignInManager<User> signInManager, IUserService userService, IRoleService roleService, IUserControllerModule module)
        {
            _signInManager = signInManager;
            _userService = userService;
            _roleService = roleService;
            _module = module;
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Register() => View();

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            await _module.CheckDataAtCreationAsync(this, model);
            if (ModelState.IsValid)
            {
                var (result, user) = await _userService.CreateUserAsync(model);
                if (result.Succeeded)
                {
                    await _signInManager.SignInWithClaimsAsync(user, new AuthenticationProperties { 
                        IsPersistent = false,
                        AllowRefresh = true                        
                    }, await _userService.GetClaimsAsync(user));

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
            var user = await _module.CheckDataAtLoginAsync(this, model);
            if (ModelState.IsValid)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user!, model.Password, false);

                if (result.Succeeded)
                {
                    var claims = await _userService.GetClaimsAsync(user!);
                    await _signInManager.SignInWithClaimsAsync(user!, new AuthenticationProperties {
                        IsPersistent = false,
                        AllowRefresh = true
                    }, claims);

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
        public async Task<IActionResult> Logout([FromQuery] string returnUrl)
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", new { ReturnUrl = returnUrl });
        }

        [Authorize(Roles = "Admin"), CheckUserId]
        [HttpGet]
        [Route("GetUsers/{roleId?}")]
        public async Task<IActionResult> GetUsers([FromRoute] int? roleId)
        {
            var model = await _userService.GetUsersViewModelAsync(roleId);
            return View(model);
        }

        [Authorize, CheckUserId]
        [HttpPost]
        public async Task<IActionResult> Remove(int id, [FromForm] int? userId)
        {
            var result = await _userService.DeleteByIdAsync(id, userId, User.IsInRole("Admin"));
            if (!result)
                return BadRequest();

            if (User.IsInRole("Admin")) return RedirectToAction("GetUsers");

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize, CheckUserId]
        [HttpGet]
        [Route("EditUser/{id?}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromQuery] int? userId)
        {
            var model = await _userService.GetUserEditViewModelAsync(id, userId, User.IsInRole("Admin"));
            if (model == null)
                return BadRequest();

            model.AllRoles = await _roleService.GetEnabledRolesForUserAsync(id);
            return View(model);
        }

        [Authorize, CheckUserId]
        [HttpPost]
        [Route("EditUser/{id}")]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            var currentUser = await _module.CheckDataAtEditionAsync(this, model);
            if (ModelState.IsValid)
            {
                model.AllRoles = await _module.UpdateRoleStateForEditUserAsync(this);
                var result = await _userService.UpdateUserAsync(model, currentUser!);

                if (result.Succeeded)
                    return RedirectToAction("Index", "Home", new { model.ReturnUrl });
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            model.AllRoles = await _roleService.GetEnabledRolesForUserAsync(model.Id);
            return View(model);
        }

        [Authorize, CheckUserId]
        [HttpGet]
        [Route("ViewUser/{id}")]
        public async Task<IActionResult> View([FromRoute] int id)
        {
            var model = await _userService.GetUserViewModelAsync(id);
            if (model == null)
                return BadRequest();

            return View(model);
        }

        [Authorize(Roles = "Admin"), CheckUserId]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new UserCreateViewModel() { AllRoles = await GetDictionaryRolesDefault() };
            return View(model);
        }

        [Authorize(Roles = "Admin"), CheckUserId]
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            await _module.CheckDataAtCreationAsync(this, model);
            if (ModelState.IsValid)
            {
                model.AllRoles = await _module.UpdateRoleStateForEditUserAsync(this);
                var result = await _userService.CreateUserAsync(model);

                if (!result.Succeeded)
                    return BadRequest();

                return RedirectToAction("GetUsers");
            }

            model.AllRoles = await GetDictionaryRolesDefault();
            return View(model);
        }

        private async Task<Dictionary<string, bool>> GetDictionaryRolesDefault()
        {
            var roles = await _roleService.GetAllRolesAsync();
            var dict = new Dictionary<string, bool>();
            foreach (var role in roles)
            {
                if (role.Name == "User")
                    dict.Add(role.Name, true);
                else
                    dict.Add(role.Name!, false);
            }
            return dict;
        }
    }
}
