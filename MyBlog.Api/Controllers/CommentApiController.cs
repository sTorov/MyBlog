using Microsoft.AspNetCore.Mvc;
using MyBlog.Services.ApiModels.Comments.Request;
using MyBlog.Services.Services.Interfaces;

namespace MyBlog.Api.Controllers
{
    /// <summary>
    /// Контроллер комментариев (API)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CommentApiController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly ICheckDataService _checkDataService;

        public CommentApiController(ICommentService commentService, ICheckDataService checkDataService)
        {
            _commentService = commentService;
            _checkDataService = checkDataService;
        }

        /// <summary>
        /// Получение объекта комментария
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return StatusCode(200);
        }

        /// <summary>
        /// Создание комментария
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommentApiCreateModel model)
        {
            return StatusCode(200);
        }

        /// <summary>
        /// Обновление комментария
        /// </summary>
        /// <param name="id"></param>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromBody] CommentApiUpdateModel model)
        {
            return StatusCode(200);
        }

        /// <summary>
        /// Удаление комментария
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