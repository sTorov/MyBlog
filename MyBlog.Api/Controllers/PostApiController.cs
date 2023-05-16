using Microsoft.AspNetCore.Mvc;
using MyBlog.Services.ApiModels.Posts.Request;
using MyBlog.Services.Services.Interfaces;

namespace MyBlog.Api.Controllers
{
    /// <summary>
    /// ���������� ������ (API)
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PostApiController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ICheckDataService _checkDataService;

        public PostApiController(IPostService postService, ICheckDataService checkDataService)
        {
            _postService = postService;
            _checkDataService = checkDataService;
        }

        /// <summary>
        /// ��������� ������� ������
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {

        }

        /// <summary>
        /// �������� ������
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PostApiCreateModel model)
        {

        }

        /// <summary>
        /// ���������� ������
        /// </summary>
        /// <param name="id"></param>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromBody] PostApiUpdateModel model)
        {

        }

        /// <summary>
        /// �������� ������
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

        }
    }
}