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
        /// <param name="id"></param>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if(post == null)
                return StatusCode(404, $"Статья не найдена!");

            return StatusCode(200, _mapper.Map<PostApiModel>(post));
        }

        /// <summary>
        /// Получение списка всех статей. Возможна фильтрация по идентификаторам пользователя и тега
        /// </summary>
        /// <param name="userId"></param>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? userId, [FromQuery] int? tagId)
        {
            var list = await _postService.GetAllPostsAsync();

            if (tagId != null)
            {
                list = list.SelectMany(p => p.Tags, (p, t) => new { Post = p, Tag = t })
                    .Where(o => o.Tag.Id == tagId).Select(o => o.Post).ToList();
            }
            if(userId != null)
                list = list.Where(p => p.UserId == userId).ToList();

            return StatusCode(200, _mapper.Map<List<PostApiModel>>(list));
        }

        /// <summary>
        /// Создание статьи
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PostApiCreateModel model)
        {
            var messages = await _checkDataService.CheckEntitiesByIdAsync(userId: model.UserId);

            if(messages.Count == 0)
            {
                var check = await _checkDataService.CheckTagsForCreatePostAsync(model.PostTags ?? string.Empty);
                if (!check)
                    return StatusCode(422, $"Указаны несуществующие теги!");

                var result = await _postService.CreatePostAsync(model);
                if (!result)
                    return StatusCode(400, $"Произошла ошибка при создании статьи!");

                return StatusCode(201, $"Статья успешно создана!");
            }

            return StatusCode(404, messages[0]);
        }

        /// <summary>
        /// Обновление статьи
        /// </summary>
        /// <param name="model"></param>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PostApiUpdateModel model)
        {
            var post = await _postService.GetPostByIdAsync(model.Id);
            if (post == null)
                return StatusCode(404, $"Статья не найдена!");

            var check = await _checkDataService.CheckTagsForCreatePostAsync(model.PostTags ?? string.Empty);
            if (!check)
                return StatusCode(422, $"Указаны несуществующие теги!");

            var result = await _postService.UpdatePostAsync(model);
            if (!result)
                return StatusCode(400, $"Произошла ошибка при обновлении статьи!");

            return StatusCode(200, _mapper.Map<PostApiModel>(post));
        }

        /// <summary>
        /// Удаление статьи
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        [Route("{id}")]
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