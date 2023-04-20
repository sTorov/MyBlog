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
            await _userService.CheckDataAtCreationAsync(this, model);
            if (ModelState.IsValid)
            {
                var (result, user) = await _userService.CreateUserAsync(model);
                if (result.Succeeded)
                {
                    await _signInManager.SignInWithClaimsAsync(user, false, await _userService.GetClaimsAsync(user));
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
        [Route("Login/{userId?}")]
        public IActionResult Login() => View();

        [HttpPost]
        [Route("Login/{userId?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel model, [FromRoute] int? userId)
        {
            var user = await _userService.CheckDataAtLoginAsync(this, model);
            if(ModelState.IsValid)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user!, model.Password, false);

                if (result.Succeeded)
                {
                    var claims = await _userService.GetClaimsAsync(user!);
                    await _signInManager.SignInWithClaimsAsync(user!, false, claims);

                    var check = userId != null && claims.FirstOrDefault(c => c.Type == "UserID")?.Value == userId.ToString();
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl) && check)
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
        public async Task<IActionResult> Logout([FromQuery] string returnUrl,[FromQuery] int? userId)
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", new { returnUrl, userId });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetUsers/{id?}")]
        public async Task<IActionResult> GetUsers([FromRoute] int? id)
        {
            var model = await _userService.GetUsersViewModelAsync(id);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Remove(int id, [FromForm] int? userId)
        {
            var result = await _userService.DeleteByIdAsync(id, userId,  User.IsInRole("Admin"));
            if (!result)
                return RedirectToAction("BadRequest", "Error");

            if(User.IsInRole("Admin")) return RedirectToAction("GetUsers");

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize, CheckParameter(parameterName: "userId", path: "EditUser")]
        [HttpGet]
        [Route("EditUser/{id?}")]
        public async Task<IActionResult> Edit([FromRoute]int id, [FromQuery] int? userId)
        {
            var model = await _userService.GetUserEditViewModelAsync(id, userId, User.IsInRole("Admin"));
            if(model == null)
                return RedirectToAction("BadRequest", "Error");

            model.AllRoles = await _roleService.GetEnabledRolesForUserAsync(id);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [Route("EditUser/{id}")]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            var currentUser = await _userService.CheckDataAtEditionAsync(this, model);
            if (ModelState.IsValid)
            {
                model.AllRoles = await _userService.UpdateRoleStateForEditUserAsync(this);
                var result = await _userService.UpdateUserAsync(model, currentUser!);

                if (result.Succeeded)
                {
                    if(User.IsInRole("Admin")) return RedirectToAction("GetUsers");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            model.AllRoles = await _roleService.GetEnabledRolesForUserAsync(model.Id);
            return View(model);
        }

        [Authorize, CheckParameter(parameterName:"userId", path:"ViewUser")]
        [HttpGet]
        [Route("ViewUser/{id}")]
        public async Task<IActionResult> View([FromRoute]int id,[FromQuery] int? userId)
        {
            var model = await _userService.GetUserViewModelAsync(id, userId, User.IsInRole("Admin"));
            if (model == null)
                return RedirectToAction("BadRequest", "Error");

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new UserCreateViewModel() { AllRoles = await GetDictionaryRolesDefault() };
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            await _userService.CheckDataAtCreationAsync(this, model);
            if (ModelState.IsValid)
            {
                model.AllRoles = await _userService.UpdateRoleStateForEditUserAsync(this);
                var result = await _userService.CreateUserAsync(model);

                if(!result.Succeeded)
                    return RedirectToAction("BadRequest", "Error");

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
