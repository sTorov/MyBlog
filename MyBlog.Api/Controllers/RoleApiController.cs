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
    [Route("[controller]")]
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
        /// <param name="id"></param>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id = 0)
        {
            var list = new List<RoleApiModel>();

            if (id == 0)
                list = _mapper.Map<List<RoleApiModel>>(await _roleService.GetAllRolesAsync());
            else
            {
                var role = await _roleService.GetRoleByIdAsync(id);
                if (role == null)
                    return StatusCode(404, $"Роль не найдена!");

                list.Add(_mapper.Map<RoleApiModel>(role));
            }

            return StatusCode(200, list);
        }

        /// <summary>
        /// Создание роли
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleApiCreateModel model)
        {
            var message = await _checkDataService.CheckDataForCreateRoleAsync(model);

            if(message == string.Empty)
            {
                var result = await _roleService.CreateRoleAsync(model);
                if (!result)
                    return StatusCode(400, $"Произошла ошибка при создании роли!");

                return StatusCode(201, $"Роль успешно создана.");
            }

            return StatusCode(409, message);
        }

        /// <summary>
        /// Обновление роли
        /// </summary>
        /// <param name="model"></param>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RoleApiUpdateModel model)
        {
            var role = await _roleService.GetRoleByIdAsync(model.Id);
            if (role == null)
                return StatusCode(404, $"Роль не найдена!");

            var check = await _checkDataService.CheckChangeDefaultRolesAsync(model.Id, model.Name);
            if (!check)
                return StatusCode(209, $"Невозможно изменить имя стандартной роли приложения!");

            var result = await _roleService.UpdateRoleAsync(model);
            if (!result)
                return StatusCode(400, $"Произошла ошибка при обновлении роли!");

            return StatusCode(200, _mapper.Map<RoleApiModel>(role));
        }

        /// <summary>
        /// Удаление роли
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
                return StatusCode(404, $"Роль не найдена!");

            var check = await _checkDataService.CheckChangeDefaultRolesAsync(id);
            if (!check)
                return StatusCode(209, $"Невозможно удаление стандартной роли приложения!");

            var result = await _roleService.DeleteRoleAsync(id);
            if (!result)
                return StatusCode(400, $"Произошла ошибка при удалении роли!");

            return StatusCode(200, _mapper.Map<RoleApiModel>(role));
        }
    }
}