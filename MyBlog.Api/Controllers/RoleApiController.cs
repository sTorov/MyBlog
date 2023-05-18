using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Services.ApiModels.Roles.Request;
using MyBlog.Services.ApiModels.Roles.Response;
using MyBlog.Services.Services.Interfaces;

namespace MyBlog.Api.Controllers
{
    /// <summary>
    /// Контроллер ролей (API)
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RoleApiController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ICheckDataService _checkDataService;
        private readonly IMapper _mapper;

        public RoleApiController(IRoleService roleService, ICheckDataService checkDataService, IMapper mapper)
        {
            _roleService = roleService;
            _checkDataService = checkDataService;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение объекта роли
        /// </summary>
        /// <remarks>Данный метод позволяет получить роль по её идентификатору</remarks>
        /// <param name="id">Идентификатор роли</param>
        /// <response code="200">Получение объекта роли</response>
        /// <response code="404">Не удалось найти роль по указанному идентификатору</response>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(RoleApiModel), 200)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
                return StatusCode(404, $"Роль не найдена!");

            return StatusCode(200, _mapper.Map<RoleApiModel>(role));
        }

        /// <summary>
        /// Получение всех ролей. Возможна фильтрация по идентификатору пользователя
        /// </summary>
        /// <remarks>
        /// Данный метод позволяет получить список всех ролей. Так же, возможно получить
        /// список ролей определённого пользователя при указании его идентификатора
        /// </remarks>
        /// <param name="userId">
        /// Идентификатор пользователя. Указать для получения списка ролей определённого пользователя.
        /// Не указывать для получения полного списка ролей
        /// </param>
        /// <response code="200">Получение списка ролей</response>
        [HttpGet]
        [ProducesResponseType(typeof(RoleApiModel[]), 200)]
        public async Task<IActionResult> GetAll([FromQuery] int? userId)
        {
            var list = new List<RoleApiModel>();

            if (userId != null)
                list = _mapper.Map<List<RoleApiModel>>(await _roleService.GetRolesByUserAsync((int)userId));
            else
                list = _mapper.Map<List<RoleApiModel>>(await _roleService.GetAllRolesAsync());

            return StatusCode(200, list);
        }

        /// <summary>
        /// Создание роли
        /// </summary>
        /// <remarks>
        /// Данный метод позволяет создать новую роль. Подробное описание свойств  -  см. схему RoleApiCreateModel
        /// </remarks>
        /// <response code="200">Роль успешно создана</response>
        /// <response code="400">Ошибка при создании роли</response>
        /// <response code="409">Имя новой роли совпадает с именем существующей роли</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleApiCreateModel model)
        {
            var message = await _checkDataService.CheckDataForCreateRoleAsync(model);
            if (message != string.Empty)
                return StatusCode(409, message);

            var result = await _roleService.CreateRoleAsync(model);
            if (!result)
                return StatusCode(400, $"Произошла ошибка при создании роли!");

            return StatusCode(200, $"Роль успешно создана.");
        }

        /// <summary>
        /// Обновление роли
        /// </summary>
        /// <remarks>
        /// Данный метод позволяет обновить существующую роль. Подробное описание свойств  -  см. схему RoleApiUpdateModel
        ///     
        ///     *Нельзя изменить имена ролей по умолчанию:
        ///     ["User", "Moderator", "Admin"]
        /// 
        /// </remarks>
        /// <response code="200">Получение модели роли с обновленными данными</response>
        /// <response code="400">Ошибка при обновлении роли</response>
        /// <response code="404">Не найдена роли по указанному идентификатору</response>
        /// <response code="409">Редактирование имени стандартной роли невозможно</response>
        /// <response code="422">Новое имя роли совпадает с именем существующей роли</response>
        [HttpPut]
        [ProducesResponseType(typeof(RoleApiModel), 200)]
        public async Task<IActionResult> Update([FromBody] RoleApiUpdateModel model)
        {
            var role = await _roleService.GetRoleByIdAsync(model.Id);
            if (role == null)
                return StatusCode(404, $"Роль не найдена!");

            var message = await _checkDataService.CheckDataForEditRoleAsync(model);
            if (message != string.Empty)
                return StatusCode(422, message);

            var check = await _checkDataService.CheckChangeDefaultRolesAsync(model.Id, model.Name);
            if (!check)
                return StatusCode(409, $"Невозможно изменить имя стандартной роли приложения!");

            var result = await _roleService.UpdateRoleAsync(model);
            if (!result)
                return StatusCode(400, $"Произошла ошибка при обновлении роли!");

            return StatusCode(200, _mapper.Map<RoleApiModel>(role));
        }

        /// <summary>
        /// Удаление роли
        /// </summary>
        /// <remarks>
        /// Данный метод позволяет удалить роль по её идентификатору.
        ///     
        ///     *Нельзя удалить роли по умолчанию:
        ///     ["User", "Moderator", "Admin"]
        /// 
        /// </remarks>
        /// <param name="id">Идентификатор роли, которую необходимо удалить</param>
        /// <response code="200">Получение модели удалённой роли</response>
        /// <response code="400">Ошибка при удалении роли</response>
        /// <response code="404">Не найдена роли по указанному идентификатору</response>
        /// <response code="409">Удаление стандартной роли невозможно</response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(RoleApiModel), 200)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
                return StatusCode(404, $"Роль не найдена!");

            var check = await _checkDataService.CheckChangeDefaultRolesAsync(id);
            if (!check)
                return StatusCode(409, $"Невозможно удаление стандартной роли приложения!");

            var result = await _roleService.DeleteRoleAsync(id);
            if (!result)
                return StatusCode(400, $"Произошла ошибка при удалении роли!");

            return StatusCode(200, _mapper.Map<RoleApiModel>(role));
        }
    }
}