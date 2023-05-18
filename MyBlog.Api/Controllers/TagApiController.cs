using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Services.ApiModels.Tags.Request;
using MyBlog.Services.ApiModels.Tags.Response;
using MyBlog.Services.Services.Interfaces;

namespace MyBlog.Api.Controllers
{
    /// <summary>
    /// Контроллер тегов (API)
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
        /// Получение объекта тега
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);
            if (tag == null)
                return StatusCode(404, $"Тег не найден!");

            return StatusCode(200, _mapper.Map<TagApiModel>(tag));
        }

        /// <summary>
        /// Получение списка всех тегов. Возможна фильтрация по идентификатору статьи.
        /// </summary>
        /// <param name="postId"></param>
        [HttpGet]
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
        /// Создание тега
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TagApiCreateModel model)
        {
            var message = await _checkDataService.CheckTagNameAsync(model);

            if(message == string.Empty)
            {
                var result = await _tagService.CreateTagAsync(model);
                if (!result)
                    return StatusCode(400, $"Произошла ошибка при создании тега!");

                return StatusCode(201, $"Тег успешно создан.");
            }

            return StatusCode(409, message);
        }

        /// <summary>
        /// Обновление тега
        /// </summary>
        /// <param name="model"></param>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TagApiUpdateModel model)
        {
            var tag = await _tagService.GetTagByIdAsync(model.Id);
            if (tag == null)
                return StatusCode(404, $"Тег не найден");

            var message = await _checkDataService.CheckTagNameAsync(model);

            if (message == string.Empty)            
            {
                var result = await _tagService.UpdateTagAsync(model);
                if (!result)
                    return StatusCode(400, $"Произошла ошибка при обновлении тега!");

                return StatusCode(200, _mapper.Map<TagApiModel>(tag));
            }

            return StatusCode(409, message);
        }

        /// <summary>
        /// Удаление тега
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);
            if (tag == null)
                return StatusCode(404, $"Тег не найден");

            var result = await _tagService.DeleteTagAsync(id);
            if(!result)
                return StatusCode(400, $"Произошла ошибка при удалении тега!");

            return StatusCode(200, _mapper.Map<TagApiModel>(tag));
        }
    }
}