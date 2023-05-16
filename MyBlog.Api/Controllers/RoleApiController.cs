using Microsoft.AspNetCore.Mvc;
using MyBlog.Services.ApiModels.Posts.Request;
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

        public RoleApiController(IRoleService roleService, ICheckDataService checkDataService)
        {
            _roleService = roleService;
            _checkDataService = checkDataService;
        }

        /// <summary>
        /// Получение объекта роли
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return StatusCode(200);
        }

        /// <summary>
        /// Создание роли
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PostApiCreateModel model)
        {
            return StatusCode(200);
        }

        /// <summary>
        /// Обновление роли
        /// </summary>
        /// <param name="model"></param>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromBody] PostApiUpdateModel model)
        {
            return StatusCode(200);
        }

        /// <summary>
        /// Удаление роли
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return StatusCode(200);
        }
    }
}