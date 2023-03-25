using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.ViewModels.Posts;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Users;
using MyBlog.Data.Repositories;
using MyBlog.Data.Repositories.Interfaces;

namespace MyBlog.App.Controllers
{
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public PostController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("CreatePost")]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostCreateViewModel model)
        {
            //заглушка
            //var user = User;
            //_ = int.TryParse(_userManager.GetUserId(user), out int userId);
            //model.UsertId = userId;

            var repo = _unitOfWork.GetRepository<Post>() as PostRepository;

            var creater = await _userManager.FindByIdAsync(model.UsertId.ToString());
            if (creater == null)
                ModelState.AddModelError(string.Empty, $"Пользователя с ID [{model.UsertId}] не существует!");

            if (ModelState.IsValid)
            {
                var post = _mapper.Map<Post>(model);
                post.User = creater!;

                await repo!.CreateAsync(post);

                return RedirectToAction("GetPost");
            }
            else
                return View("Create", model);
        }

        [HttpGet]
        [Route("GetPost/{userId?}")]
        public async Task<IActionResult> GetPost([FromRoute]int? userId)
        {
            var model = new PostsVIewModel();
            var repo = _unitOfWork.GetRepository<Post>() as PostRepository;

            if (userId == null)
                model.Posts = await repo!.GetAllAsync();
            else
                model.Posts = await repo!.GetPostsByUserIdAsync((int)userId);

            return View(model);
        }
    }
}
