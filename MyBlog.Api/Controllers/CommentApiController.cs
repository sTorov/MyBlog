using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Services.ApiModels.Comments.Request;
using MyBlog.Services.ApiModels.Comments.Response;
using MyBlog.Services.Services.Interfaces;

namespace MyBlog.Api.Controllers
{
    /// <summary>
    /// ���������� ������������ (API)
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CommentApiController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly ICheckDataService _checkDataService;
        private readonly IMapper _mapper;

        public CommentApiController(ICommentService commentService, ICheckDataService checkDataService, IMapper mapper)
        {
            _commentService = commentService;
            _checkDataService = checkDataService;
            _mapper = mapper;
        }

        /// <summary>
        /// ��������� ������� �����������
        /// </summary>
        /// <remarks>������ ����� ��������� �������� ����������� �� ��� ��������������</remarks>
        /// <param name="id">������������� �����������</param>
        /// <response code="200">��������� ������� �����������</response>
        /// <response code="404">�� ������� ����� ����������� �� ���������� ��������������</response>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(CommentApiModel), 200)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment == null)
                return StatusCode(404, $"����������� �� ������!");

            return StatusCode(200, _mapper.Map<CommentApiModel>(comment));
        }

        /// <summary>
        /// ��������� ���� ������������. �������� ���������� �� ��������������� ������������ � ������
        /// </summary>
        /// <remarks>
        /// ������ ����� ��������� �������� ������ ���� ������������. �������� ��������
        /// ������ ������������ ������������ ������������ ��� �������� ��� ��������������,
        /// ������ ������������ ����������� ������ ��� �������� � ��������������, ���� 
        /// ��������� ����� ��� �������
        /// </remarks>
        /// <param name="userId">
        /// ������������� ������������. ������� ��� ��������� ������ ������������ ������������ ������������.
        /// �� ��������� ��� ��������� ������� ������ ������������
        /// </param>
        /// <param name="postId">
        /// ������������� ������. ������� ��� ��������� ������ ������������ ����������� ������.
        /// �� ��������� ��� ��������� ������� ������ ������������
        /// </param>
        /// <response code="200">��������� ������ ������������</response>
        /// <response code="404">�� ������ ���� ��� ��������� ��������� ��������</response>
        [HttpGet]
        [ProducesResponseType(typeof(CommentApiModel[]), 200)]
        public async Task<IActionResult> GetAll([FromQuery] int? postId, [FromQuery] int? userId)
        {
            var messages = await _checkDataService.CheckEntitiesByIdAsync(postId: postId, userId: userId);

            if(messages.Count == 0)
            {
                var list = _mapper.Map<List<CommentApiModel>>(await _commentService.GetAllComments());

                if (postId != null)
                    list = list.Where(m => m.PostId == postId).ToList();
                if (userId != null)
                    list = list.Where(m => m.UserId == userId).ToList();

                return StatusCode(200, list);
            }

            return StatusCode(404, messages);
        }

        /// <summary>
        /// �������� �����������
        /// </summary>
        /// <remarks>
        /// ������ ����� ��������� ������� �����������. ��������� �������� �������  -  ��. ����� CommentApiCreateModel
        /// </remarks>
        /// <response code="200">����������� ������� ������</response>
        /// <response code="400">������ ��� �������� �����������</response>
        /// <response code="404">�� ������ ���� ��� ��������� ��������� ��������</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommentApiCreateModel model)
        {
            var messages = await _checkDataService.CheckEntitiesByIdAsync(userId: model.UserId, postId: model.PostId);

            if(messages.Count == 0)
            {
                var result = await _commentService.CreateCommentAsync(model);
                if (!result)
                    return StatusCode(400, $"��������� ������ ��� �������� �����������!");

                return StatusCode(200, $"����������� ������� ������.");
            }

            return StatusCode(404, messages);
        }

        /// <summary>
        /// ���������� �����������
        /// </summary>
        /// <remarks>
        /// ������ ����� ��������� �������� ������������ �����������. ��������� �������� �������  -  ��. ����� CommentApiUpdateModel
        /// </remarks>
        /// <response code="200">��������� ������ ����������� � ������������ �������</response>
        /// <response code="400">������ ��� ���������� �����������</response>
        /// <response code="404">�� ������ ����������� �� ���������� ��������������</response>
        [HttpPut]
        [ProducesResponseType(typeof(CommentApiModel), 200)]
        public async Task<IActionResult> Update([FromBody] CommentApiUpdateModel model)
        {
            var comment = await _commentService.GetCommentByIdAsync(model.Id);
            if (comment == null)
                return StatusCode(404, $"����������� �� ������!");

            var result = await _commentService.UpdateCommentAsync(model);
            if (!result)
                return StatusCode(400, $"��������� ������ ��� ���������� �����������!");

            return StatusCode(200, _mapper.Map<CommentApiModel>(comment));
        }

        /// <summary>
        /// �������� �����������
        /// </summary>
        /// <remarks>
        /// ������ ����� ��������� ������� ����������� �� ��� ��������������.
        /// </remarks>
        /// <param name="id">������������� �����������, ������� ���������� �������</param>
        /// <response code="200">��������� ������ ��������� �����������</response>
        /// <response code="400">������ ��� �������� �����������</response>
        /// <response code="404">�� ������ ����������� �� ���������� ��������������</response>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment == null)
                return StatusCode(404, $"����������� �� ������!");

            var result = await _commentService.DeleteCommentAsync(comment);
            if (result == 0)
                return StatusCode(400, $"��������� ������ ��� �������� �����������!");

            return StatusCode(200, _mapper.Map<CommentApiModel>(comment));

        }
    }
}