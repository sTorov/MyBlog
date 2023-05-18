using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Services.ApiModels.Comments.Request;
using MyBlog.Services.ApiModels.Comments.Response;
using MyBlog.Services.Services.Interfaces;

namespace MyBlog.Api.Controllers
{
    /// <summary>
    /// Контроллер комментариев (API)
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
        /// Получение объекта комментария
        /// </summary>
        /// <remarks>Данный метод позволяет получить комментарий по его идентификатору</remarks>
        /// <param name="id">Идентификатор комментария</param>
        /// <response code="200">Получение объекта комментария</response>
        /// <response code="404">Не удалось найти комментарий по указанному идентификатору</response>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(CommentApiModel), 200)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment == null)
                return StatusCode(404, $"Комментарий не найден!");

            return StatusCode(200, _mapper.Map<CommentApiModel>(comment));
        }

        /// <summary>
        /// Получение всех комментариев. Возможна фильтрация по идентификаторам пользователя и статьи
        /// </summary>
        /// <remarks>
        /// Данный метод позволяет получить список всех комментариев. Возможно получить
        /// список комментариев определённого пользователя при указании его идентификатора,
        /// список комментариев определённой статьи при указании её идентификатора, либо 
        /// применить сразу два фильтра
        /// </remarks>
        /// <param name="userId">
        /// Идентификатор пользователя. Указать для получения списка комментариев определённого пользователя.
        /// Не указывать для получения полного списка комментариев
        /// </param>
        /// <param name="postId">
        /// Идентификатор статьи. Указать для получения списка комментариев определённой статьи.
        /// Не указывать для получения полного списка комментариев
        /// </param>
        /// <response code="200">Получение списка комментариев</response>
        /// <response code="404">Не найден один или несколько указанных объектов</response>
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
        /// Создание комментария
        /// </summary>
        /// <remarks>
        /// Данный метод позволяет создать комментарий. Подробное описание свойств  -  см. схему CommentApiCreateModel
        /// </remarks>
        /// <response code="200">Комментарий успешно создан</response>
        /// <response code="400">Ошибка при создании комментария</response>
        /// <response code="404">Не найден один или несколько указанных объектов</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommentApiCreateModel model)
        {
            var messages = await _checkDataService.CheckEntitiesByIdAsync(userId: model.UserId, postId: model.PostId);

            if(messages.Count == 0)
            {
                var result = await _commentService.CreateCommentAsync(model);
                if (!result)
                    return StatusCode(400, $"Произошла ошибка при создании комментария!");

                return StatusCode(200, $"Комментарий успешно создан.");
            }

            return StatusCode(404, messages);
        }

        /// <summary>
        /// Обновление комментария
        /// </summary>
        /// <remarks>
        /// Данный метод позволяет обновить существующий комментарий. Подробное описание свойств  -  см. схему CommentApiUpdateModel
        /// </remarks>
        /// <response code="200">Получение модели комментария с обновленными данными</response>
        /// <response code="400">Ошибка при обновлении комментария</response>
        /// <response code="404">Не найден комментарий по указанному идентификатору</response>
        [HttpPut]
        [ProducesResponseType(typeof(CommentApiModel), 200)]
        public async Task<IActionResult> Update([FromBody] CommentApiUpdateModel model)
        {
            var comment = await _commentService.GetCommentByIdAsync(model.Id);
            if (comment == null)
                return StatusCode(404, $"Комментармй не найден!");

            var result = await _commentService.UpdateCommentAsync(model);
            if (!result)
                return StatusCode(400, $"Произошла ошибка при обновлении комментария!");

            return StatusCode(200, _mapper.Map<CommentApiModel>(comment));
        }

        /// <summary>
        /// Удаление комментария
        /// </summary>
        /// <remarks>
        /// Данный метод позволяет удалить комментарий по его идентификатору.
        /// </remarks>
        /// <param name="id">Идентификатор комментария, который необходимо удалить</param>
        /// <response code="200">Получение модели удалённого комментария</response>
        /// <response code="400">Ошибка при удалении комментария</response>
        /// <response code="404">Не найден комментарий по указанному идентификатору</response>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment == null)
                return StatusCode(404, $"Комментармй не найден!");

            var result = await _commentService.DeleteCommentAsync(comment);
            if (result == 0)
                return StatusCode(400, $"Произошла ошибка при удалении комментария!");

            return StatusCode(200, _mapper.Map<CommentApiModel>(comment));

        }
    }
}