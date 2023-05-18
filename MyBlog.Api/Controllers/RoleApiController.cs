using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Services.ApiModels.Roles.Request;
using MyBlog.Services.ApiModels.Roles.Response;
using MyBlog.Services.Services.Interfaces;

namespace MyBlog.Api.Controllers
{
    /// <summary>
    /// ���������� ����� (API)
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
        /// ��������� ������� ����
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
                return StatusCode(404, $"���� �� �������!");

            return StatusCode(200, _mapper.Map<RoleApiModel>(role));
        }

        /// <summary>
        /// ��������� ���� �����. �������� ���������� �� �������������� ������������
        /// </summary>
        [HttpGet]
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
        /// �������� ����
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleApiCreateModel model)
        {
            var message = await _checkDataService.CheckDataForCreateRoleAsync(model);
            if (message != string.Empty)
                return StatusCode(409, message);

            var result = await _roleService.CreateRoleAsync(model);
            if (!result)
                return StatusCode(400, $"��������� ������ ��� �������� ����!");

            return StatusCode(201, $"���� ������� �������.");
        }

        /// <summary>
        /// ���������� ����
        /// </summary>
        /// <param name="model"></param>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RoleApiUpdateModel model)
        {
            var role = await _roleService.GetRoleByIdAsync(model.Id);
            if (role == null)
                return StatusCode(404, $"���� �� �������!");

            var message = await _checkDataService.CheckDataForEditRoleAsync(model);
            if (message != string.Empty)
                return StatusCode(409, message);

            var check = await _checkDataService.CheckChangeDefaultRolesAsync(model.Id, model.Name);
            if (!check)
                return StatusCode(209, $"���������� �������� ��� ����������� ���� ����������!");

            var result = await _roleService.UpdateRoleAsync(model);
            if (!result)
                return StatusCode(400, $"��������� ������ ��� ���������� ����!");

            return StatusCode(200, _mapper.Map<RoleApiModel>(role));
        }

        /// <summary>
        /// �������� ����
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
                return StatusCode(404, $"���� �� �������!");

            var check = await _checkDataService.CheckChangeDefaultRolesAsync(id);
            if (!check)
                return StatusCode(209, $"���������� �������� ����������� ���� ����������!");

            var result = await _roleService.DeleteRoleAsync(id);
            if (!result)
                return StatusCode(400, $"��������� ������ ��� �������� ����!");

            return StatusCode(200, _mapper.Map<RoleApiModel>(role));
        }
    }
}