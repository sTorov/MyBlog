using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Services.ApiModels.Posts.Request;
using MyBlog.Services.ApiModels.Posts.Response;
using MyBlog.Services.Services.Interfaces;

namespace MyBlog.Api.Controllers
{
    /// <summary>
    /// Контроллер статей (API)
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PostApiController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ICheckDataService _checkDataService;
        private readonly IMapper _mapper;

        public PostApiController(IPostService postService, ICheckDataService checkDataService, IMapper mapper)
        {
            _postService = postService;
            _checkDataService = checkDataService;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение объекта статьи
        /// </summary>
        /// <remarks>Данный метод позволяет получить статью по её идентификатору</remarks>
        /// <param name="id">Идентификатор статьи</param>
        /// <response code="200">Получение объекта статьи</response>
        /// <response code="404">Не удалось найти статью по указанному идентификатору</response>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(PostApiModel), 200)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
                return StatusCode(404, $"Статья не найдена!");

            return StatusCode(200, _mapper.Map<PostApiModel>(post));
        }

        /// <summary>
        /// Получение всех статей. Возможна фильтрация по идентификаторам пользователя и тега
        /// </summary>
        /// <remarks>
        /// Данный метод позволяет получить список всех статей. Возможно получить
        /// список статей определённого пользователя при указании его идентификатора,
        /// список статей с определённым тегом при указании его идентификатора, либо 
        /// применить сразу два фильтра
        /// </remarks>
        /// <param name="userId">
        /// Идентификатор пользователя. Указать для получения списка статей определённого пользователя.
        /// Не указывать для получения полного списка статей
        /// </param>
        /// <param name="tagId">
        /// Идентификатор тега. Указать для получения списка статей с указанным тегом.
        /// Не указывать для получения полного списка статей
        /// </param>
        /// <response code="200">Получение списка статей</response>
        [HttpGet]
        [ProducesResponseType(typeof(PostApiModel[]), 200)]
        public async Task<IActionResult> GetAll([FromQuery] int? userId, [FromQuery] int? tagId)
        {
            var list = await _postService.GetAllPostsAsync();

            if (tagId != null)
            {
                list = list.SelectMany(p => p.Tags, (p, t) => new { Post = p, Tag = t })
                    .Where(o => o.Tag.Id == tagId).Select(o => o.Post).ToList();
            }
            if (userId != null)
                list = list.Where(p => p.UserId == userId).ToList();

            return StatusCode(200, _mapper.Map<List<PostApiModel>>(list));
        }

        /// <summary>
        /// Создание статьи
        /// </summary>
        /// <remarks>
        /// Данный метод позволяет создать новую статью. Подробное описание свойств  -  см. схему PostApiCreateModel
        /// </remarks>
        /// <response code="200">Статья успешно создана</response>
        /// <response code="400">Ошибка при создании статьи</response>
        /// <response code="404">Не найден пользователь по указанному идентификатору</response>
        /// <response code="422">Указаны несуществующие теги</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PostApiCreateModel model)
        {
            var messages = await _checkDataService.CheckEntitiesByIdAsync(userId: model.UserId);

            if (messages.Count == 0)
            {
                var errors = await _checkDataService.CheckTagsForCreatePostAsync(model.PostTags ?? string.Empty);
                if (errors.Count > 0)
                    return StatusCode(422, errors);

                var result = await _postService.CreatePostAsync(model);
                if (!result)
                    return StatusCode(400, $"Произошла ошибка при создании статьи!");

                return StatusCode(200, $"Статья успешно создана!");
            }

            return StatusCode(404, messages[0]);
        }

        /// <summary>
        /// Обновление статьи
        /// </summary>
        /// <remarks>
        /// Данный метод позволяет обновить существующую статью. Подробное описание свойств  -  см. схему PostApiUpdateModel
        /// </remarks>
        /// <response code="200">Получение модели статьи с обновленными данными</response>
        /// <response code="400">Ошибка при обновлении статьи</response>
        /// <response code="404">Не найдена статья по указанному идентификатору</response>
        /// <response code="422">Указаны несуществующие теги</response>
        [HttpPut]
        [ProducesResponseType(typeof(PostApiModel), 200)]
        public async Task<IActionResult> Update([FromBody] PostApiUpdateModel model)
        {
            var post = await _postService.GetPostByIdAsync(model.Id);
            if (post == null)
                return StatusCode(404, $"Статья не найдена!");

            var errors = await _checkDataService.CheckTagsForCreatePostAsync(model.PostTags ?? string.Empty);
            if (errors.Count > 0)
                return StatusCode(422, errors);

            var result = await _postService.UpdatePostAsync(model);
            if (!result)
                return StatusCode(400, $"Произошла ошибка при обновлении статьи!");

            return StatusCode(200, _mapper.Map<PostApiModel>(post));
        }

        /// <summary>
        /// Удаление статьи
        /// </summary>
        /// <remarks>
        /// Данный метод позволяет удалить статью по её идентификатору.
        /// </remarks>
        /// <param name="id">Идентификатор статьи, которую необходимо удалить</param>
        /// <response code="200">Получение модели удалённой статьи</response>
        /// <response code="400">Ошибка при удалении статьи</response>
        /// <response code="404">Не найдена статья по указанному идентификатору</response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(PostApiModel), 200)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
                return StatusCode(404, $"Статья не найдена!");

            var result = await _postService.DeletePostAsync(post);
            if (result == 0)
                return StatusCode(400, $"Произошла ошибка при удалении статьи!");

            return StatusCode(200, _mapper.Map<PostApiModel>(post));
        }
    }
}