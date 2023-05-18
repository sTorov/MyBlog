using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Services.ApiModels.Users.Response;
using MyBlog.Services.ApiModels.Users.Request;
using MyBlog.Services.Services.Interfaces;

namespace MyBlog.Api.Controllers
{
    /// <summary>
    /// ���������� ������������� (API)
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
        /// ��������� ������� ������������. ��������� ������ ���� �������������.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? id)
        {
            var response = new List<UserApiModel>();

            if (id == null)
                response = _mapper.Map<List<UserApiModel>>(await _userService.GetAllUsersAsync());
            else
            {
                var user = await _userService.GetUserByIdAsync((int)id);
                if (user == null)
                    return StatusCode(404, $"������������ �� ������!");

                response.Add(_mapper.Map<UserApiModel>(user));
            }

            return StatusCode(200, response);
        }

        /// <summary>
        /// �������� ������������
        /// </summary>
        /// <response code="201">����� ������������ ������� ������</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] UserApiCreateModel model)
        {
            var messages = new List<string>();
            messages.AddRange(await _checkDataService.CheckDataForCreateUserAsync(model));
            messages.AddRange(await _checkDataService.CheckRolesForUserChanged(model.Roles));

            if (messages.Count == 0)
            {
                var (result, _) = await _userService.CreateUserAsync(model, await _roleService.ConvertRoleNamesInRoles(model.Roles));
                if (!result)
                    return StatusCode(400, $"��������� ������ ��� �������� ������������!");

                return StatusCode(201, $"������������ ������� ������.");
            }

            return StatusCode(409, messages);
        }

        /// <summary>
        /// ���������� ������������
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
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

            return StatusCode(400, $"��������� ������ ��� ���������� ������������!");
        }

        /// <summary>
        /// �������� ������������
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deletedUser = await _userService.GetUserByIdAsync(id);
            if (deletedUser == null) 
                return StatusCode(404, $"������������ �� ������!");

            var result = await _userService.DeleteByIdAsync(deletedUser);
            if (!result) 
                return StatusCode(400, $"������ ��� �������� ������������!");

            return StatusCode(200, _mapper.Map<UserApiModel>(deletedUser));
        }
    }
}