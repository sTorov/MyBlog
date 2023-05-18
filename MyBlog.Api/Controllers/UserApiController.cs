using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Services.ApiModels.Users.Response;
using MyBlog.Services.ApiModels.Users.Request;
using MyBlog.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace MyBlog.Api.Controllers
{
    /// <summary>
    /// Контроллер пользователей (API)
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ICheckDataService _checkDataService;
        private readonly IMapper _mapper;

        public UserApiController(IUserService userService, IRoleService roleService, ICheckDataService checkDataService, IMapper mapper)
        {
            _userService = userService;
            _roleService = roleService;
            _checkDataService = checkDataService;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение объекта пользователя. Получение списка всех пользователей.
        /// </summary>
        /// <remarks>Данный метод возвращает массив пользователей</remarks>
        /// <param name="id">ID пользователя. Оставить пустым для получения полного списка пользователей</param>
        /// <response code="200">Массив пользователей. При указании ID массив будет состоять из одного элемента</response>
        /// <response code="404">Пользователь не найден по указанному идентификатору</response>
        [HttpGet]
        [ProducesResponseType(typeof(UserApiModel[]), 200)]
        public async Task<IActionResult> Get([FromQuery] int? id)
        {
            var response = new List<UserApiModel>();

            if (id == null)
                response = _mapper.Map<List<UserApiModel>>(await _userService.GetAllUsersAsync());
            else
            {
                var user = await _userService.GetUserByIdAsync((int)id);
                if (user == null)
                    return StatusCode(404, $"Пользователь не найден!");

                response.Add(_mapper.Map<UserApiModel>(user));
            }

            return StatusCode(200, response);
        }

        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <remarks>
        /// Данный метод позволяет создать нового пользователя. Для подробного описания свойств см. схему UserApiCreateModel
        /// </remarks>
        /// <response code="200">Новый пользователь успешно создан</response>
        /// <response code="400">Ошибка при создании нового пользователя</response>
        /// <response code="409">Ошибки при указании данных для создания пользователя</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserApiCreateModel model)
        {
            var messages = new List<string>();
            messages.AddRange(await _checkDataService.CheckDataForCreateUserAsync(model));
            messages.AddRange(await _checkDataService.CheckRolesForUserChanged(model.Roles));

            if (messages.Count == 0)
            {
                var (result, _) = await _userService.CreateUserAsync(model, await _roleService.ConvertRoleNamesInRoles(model.Roles));
                if (!result)
                    return StatusCode(400, $"Произошла ошибка при создании пользователя!");

                return StatusCode(200, $"Пользователь успешно создан.");
            }

            return StatusCode(409, messages);
        }

        /// <summary>
        /// Обновление пользователя
        /// </summary>
        /// <remarks>
        /// Данный метод позволяет обновлять информацию существующего пользователя. Для подробного описания свойств см. схему UserApiUpdateModel
        /// </remarks>
        /// <response code="200">Возвращает объект пользователя с обновленными данными</response>
        /// <response code="400">Ошибка при обновлении пользователя</response>
        /// <response code="404">Пользователь для обновления не найден</response>
        /// <response code="422">Ошибки при обновлении ролей пользователя</response>
        [HttpPut]
        [ProducesResponseType(typeof(UserApiModel), 200)]
        public async Task<IActionResult> Update([FromBody] UserApiUpdateModel model)
        {
            var (user, messages) = await _checkDataService.CheckDataForEditUserAsync(model);
            if (user == null)
                return StatusCode(404, messages[0]);

            var errors = await _checkDataService.CheckRolesForUserChanged(model.Roles);
            if (errors.Count > 0)
                return StatusCode(422, errors);

            var result = await _userService.UpdateUserAsync(model);
            if (result)
                return StatusCode(200, _mapper.Map<UserApiModel>(user));

            return StatusCode(400, $"Произошла ошибка при обновлении пользователя!");
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <remarks>Данный метод позволяет удалить пользователя</remarks>
        /// <param name="id">Идентификатор пользователя, которого необходимо удалить</param>
        /// <response code="200">Возвращает объект пользователя, который был удалён</response>
        /// <response code="400">Ошибка при удалении пользователя</response>
        /// <response code="404">Пользователь для удаления не найден</response>
        [HttpDelete]
        [ProducesResponseType(typeof(UserApiModel), 200)]
        [Produces("application/json")]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deletedUser = await _userService.GetUserByIdAsync(id);
            if (deletedUser == null)
                return StatusCode(404, $"Пользователь не найден!");

            var result = await _userService.DeleteByIdAsync(deletedUser);
            if (!result)
                return StatusCode(400, $"Ошибка при удалении пользователя!");

            return StatusCode(200, _mapper.Map<UserApiModel>(deletedUser));
        }
    }
}