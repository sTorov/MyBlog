using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Services.ApiModels.Users.Response;
using MyBlog.Services.ApiModels.Users.Request;
using MyBlog.Services.Services.Interfaces;

namespace MyBlog.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICheckDataService _checkDataService;
        private readonly IMapper _mapper;

        public UserApiController(IUserService userService, ICheckDataService checkDataService, IMapper mapper)
        {
            _userService = userService;
            _checkDataService = checkDataService;
            _mapper = mapper;
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id?}")]
        public async Task<IActionResult> Get([FromRoute] int id = 0)
        {
            var response = new List<UserApiModel>();

            if (id == 0)
                response = _mapper.Map<List<UserApiModel>>(await _userService.GetAllUsersAsync());
            else
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                    return StatusCode(404, $"Пользователь не найден!");

                response.Add(_mapper.Map<UserApiModel>(user));
            }

            return StatusCode(200, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserApiCreateModel model)
        {
            var message = await _checkDataService.CheckDataForCreateUserAsync(model);

            if (message == string.Empty)
            {
                var (result, _) = await _userService.CreateUserAsync(model);
                if (!result.Succeeded)
                    return StatusCode(400, $"Произошла ошибка при создании пользователя!");

                return StatusCode(201, $"Пользователь успешно создан.");
            }

            return StatusCode(409, message);
        }

        [HttpPut]
        [Route("{id}")]
        public void Update([FromRoute] int id /*[FromBody] model*/)
        {

        }

        [HttpDelete]
        [Route("{id}")]
        public void Delete([FromRoute] int id)
        {

        }
    }
}