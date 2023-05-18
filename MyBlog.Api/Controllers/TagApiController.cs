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
        /// <remarks>Данный метод позволяет получить объект тега по идентификатору</remarks>
        /// <param name="id">Идентификатор тега</param>
        /// <response code="200">Получение объекта тега</response>
        /// <response code="404">Не удалось найти тег по указанному идентификатору</response>
        [HttpGet]
        [ProducesResponseType(typeof(TagApiModel), 200)]
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
        /// <remarks>
        /// Данный метод позволяет получить список всех тегов. Так же, при указании идентификатора статьи,
        /// возможно получение списка тегов для данной статьи
        /// </remarks>
        /// <param name="postId">
        /// Идентификатор статьи. Указать для фильтрации тегов по идентификатору статьи. 
        /// Оставить пустым для получения полного списка тегов
        /// </param>
        /// <response code="200">Получение массива тегов</response>
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
        /// Создание тега
        /// </summary>
        /// <remarks>
        /// Данный метод позволяет создать новый тег. Подробное описание свойств  -  см. схему TagApiCreateModel
        /// </remarks>
        /// <response code="200">Успешное создание нового тега</response>
        /// <response code="400">Ошибка при создании тега</response>
        /// <response code="409">Указанное имя нового тега совпадает с именем существующего тега</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TagApiCreateModel model)
        {
            var message = await _checkDataService.CheckTagNameAsync(model);

            if(message == string.Empty)
            {
                var result = await _tagService.CreateTagAsync(model);
                if (!result)
                    return StatusCode(400, $"Произошла ошибка при создании тега!");

                return StatusCode(200, $"Тег успешно создан.");
            }

            return StatusCode(409, message);
        }

        /// <summary>
        /// Обновление тега
        /// </summary>
        /// <remarks>
        /// Данный метод позволяет обновить существующий тег. Подробное описание свойств  -  см. схему TagApiUpdateModel
        /// </remarks>
        /// <response code="200">Получение объекта тега с обновленными данными</response>
        /// <response code="400">Ошибка при обновлении тега</response>
        /// <response code="404">Тег не найден по указанному идентификатору</response>
        /// <response code="409">Новое имя тега совпадает с именем существующего тега</response>
        [HttpPut]
        [ProducesResponseType(typeof(TagApiModel), 200)]
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
        /// <remarks>
        /// Данный метод позволяет удалить тег по его идентификатору
        /// </remarks>
        /// <param name="id">Идентификатор тега, который необходимо удалить</param>
        /// <response code="200">Получение объекта удалённого тега</response>
        /// <response code="400">Ошибка при удалении тега</response>
        /// <response code="404">Тег не найден по указанному идентификатору</response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(TagApiModel), 200)]
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