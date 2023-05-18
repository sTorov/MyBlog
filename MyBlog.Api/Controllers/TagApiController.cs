using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Services.ApiModels.Tags.Request;
using MyBlog.Services.ApiModels.Tags.Response;
using MyBlog.Services.Services.Interfaces;

namespace MyBlog.Api.Controllers
{
    /// <summary>
    /// ���������� ����� (API)
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TagApiController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly ICheckDataService _checkDataService;
        private readonly IMapper _mapper;

        public TagApiController(ITagService tagService, ICheckDataService checkDataService, IMapper mapper)
        {
            _tagService = tagService;
            _checkDataService = checkDataService;
            _mapper = mapper;
        }

        /// <summary>
        /// ��������� ������� ����
        /// </summary>
        /// <remarks>������ ����� ��������� �������� ������ ���� �� ��������������</remarks>
        /// <param name="id">������������� ����</param>
        /// <response code="200">��������� ������� ����</response>
        /// <response code="404">�� ������� ����� ��� �� ���������� ��������������</response>
        [HttpGet]
        [ProducesResponseType(typeof(TagApiModel), 200)]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);
            if (tag == null)
                return StatusCode(404, $"��� �� ������!");

            return StatusCode(200, _mapper.Map<TagApiModel>(tag));
        }

        /// <summary>
        /// ��������� ������ ���� �����. �������� ���������� �� �������������� ������.
        /// </summary>
        /// <remarks>
        /// ������ ����� ��������� �������� ������ ���� �����. ��� ��, ��� �������� �������������� ������,
        /// �������� ��������� ������ ����� ��� ������ ������
        /// </remarks>
        /// <param name="postId">
        /// ������������� ������. ������� ��� ���������� ����� �� �������������� ������. 
        /// �������� ������ ��� ��������� ������� ������ �����
        /// </param>
        /// <response code="200">��������� ������� �����</response>
        [HttpGet]
        [ProducesResponseType(typeof(TagApiModel[]), 200)]
        public async Task<IActionResult> GetAll([FromQuery] int? postId)
        {
            var list = new List<TagApiModel>();

            if (postId != null)
                list = _mapper.Map<List<TagApiModel>>(await _tagService.GetTagByPostAsync((int)postId));
            else
                list = _mapper.Map<List<TagApiModel>>(await _tagService.GetAllTagsAsync());

            return StatusCode(200, list);
        }

        /// <summary>
        /// �������� ����
        /// </summary>
        /// <remarks>
        /// ������ ����� ��������� ������� ����� ���. ��������� �������� �������  -  ��. ����� TagApiCreateModel
        /// </remarks>
        /// <response code="200">�������� �������� ������ ����</response>
        /// <response code="400">������ ��� �������� ����</response>
        /// <response code="409">��������� ��� ������ ���� ��������� � ������ ������������� ����</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TagApiCreateModel model)
        {
            var message = await _checkDataService.CheckTagNameAsync(model);

            if(message == string.Empty)
            {
                var result = await _tagService.CreateTagAsync(model);
                if (!result)
                    return StatusCode(400, $"��������� ������ ��� �������� ����!");

                return StatusCode(200, $"��� ������� ������.");
            }

            return StatusCode(409, message);
        }

        /// <summary>
        /// ���������� ����
        /// </summary>
        /// <remarks>
        /// ������ ����� ��������� �������� ������������ ���. ��������� �������� �������  -  ��. ����� TagApiUpdateModel
        /// </remarks>
        /// <response code="200">��������� ������� ���� � ������������ �������</response>
        /// <response code="400">������ ��� ���������� ����</response>
        /// <response code="404">��� �� ������ �� ���������� ��������������</response>
        /// <response code="409">����� ��� ���� ��������� � ������ ������������� ����</response>
        [HttpPut]
        [ProducesResponseType(typeof(TagApiModel), 200)]
        public async Task<IActionResult> Update([FromBody] TagApiUpdateModel model)
        {
            var tag = await _tagService.GetTagByIdAsync(model.Id);
            if (tag == null)
                return StatusCode(404, $"��� �� ������");

            var message = await _checkDataService.CheckTagNameAsync(model);

            if (message == string.Empty)            
            {
                var result = await _tagService.UpdateTagAsync(model);
                if (!result)
                    return StatusCode(400, $"��������� ������ ��� ���������� ����!");

                return StatusCode(200, _mapper.Map<TagApiModel>(tag));
            }

            return StatusCode(409, message);
        }

        /// <summary>
        /// �������� ����
        /// </summary>
        /// <remarks>
        /// ������ ����� ��������� ������� ��� �� ��� ��������������
        /// </remarks>
        /// <param name="id">������������� ����, ������� ���������� �������</param>
        /// <response code="200">��������� ������� ��������� ����</response>
        /// <response code="400">������ ��� �������� ����</response>
        /// <response code="404">��� �� ������ �� ���������� ��������������</response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(TagApiModel), 200)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);
            if (tag == null)
                return StatusCode(404, $"��� �� ������");

            var result = await _tagService.DeleteTagAsync(id);
            if(!result)
                return StatusCode(400, $"��������� ������ ��� �������� ����!");

            return StatusCode(200, _mapper.Map<TagApiModel>(tag));
        }
    }
}