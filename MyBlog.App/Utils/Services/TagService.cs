using AutoMapper;
using MyBlog.App.Controllers;
using MyBlog.App.Utils.Extensions;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Tags;
using MyBlog.App.ViewModels.Tags.Interfaces;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Tags;
using MyBlog.Data.Repositories;
using MyBlog.Data.Repositories.Interfaces;
using System.Text.RegularExpressions;

namespace MyBlog.App.Utils.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly TagRepository _tagRepository;

        public TagService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

            _tagRepository = (TagRepository)_unitOfWork.GetRepository<Tag>();
        }

        public async Task<Tag?> GetTagByIdAsync(int id) => await _tagRepository.GetAsync(id);

        public async Task<List<Tag>> GetAllTagsAsync() => await _tagRepository.GetAllAsync();

        public async Task<TagsViewModel?> GetTagsViewModelAsync(int? tagId, int? postId)
        {
            var model = new TagsViewModel();

            if(tagId == null)
            {
                model.Tags = postId == null
                    ? await _tagRepository.GetAllAsync()
                    : await _tagRepository.GetTagsByPostIdAsync((int)postId);
            }
            else
            {
                var tag = postId == null
                    ? await GetTagByIdAsync(tagId ?? 0)
                    : (await _tagRepository.GetTagsByPostIdAsync((int)postId)).FirstOrDefault(t => t.Id == tagId);
                if (tag != null) model.Tags.Add(tag);
            }

            return model;
        }

        public async Task<TagEditViewModel?> GetTagEditViewModelAsync(int id)
        {
            var tag = await GetTagByIdAsync(id);
            var model = tag == null ? null : _mapper.Map<TagEditViewModel>(tag);

            return model;
        }

        public async Task<Tag?> CheckTagNameAsync<T>(TagController controller, T model)
            where T : ITagViewModel
        {
            var checkTag = await _tagRepository.GetTagByNameAsync(model.Name);
            var check = model is TagEditViewModel editModel 
                ? (checkTag != null && checkTag.Id != editModel.Id) : checkTag != null;

            if(check)
                controller.ModelState.AddModelError(string.Empty, $"Тег с именем [{model.Name}] уже существует!");

            return checkTag;
        }

        public async Task<bool> CreateTagAsync(TagCreateViewModel model)
        {
            var tag = _mapper.Map<Tag>(model);

            if(await _tagRepository.CreateAsync(tag) == 0) return false;
            return true;
        }

        public async Task<List<Tag>?> CreateTagForPostAsync(string? postTags)
        {
            if (postTags == null) return null;

            var tagSet = await CreateUndefinedTags(postTags ?? "");

            var tags = new List<Tag>();
            foreach (var tagName in tagSet)
            {
                var tag = await _tagRepository.GetTagByNameAsync(tagName);
                if (tag != null) tags.Add(tag);
            }

            return tags;
        }

        public async Task<bool> UpdateTagAsync(TagEditViewModel model)
        {
            var currentTag = await GetTagByIdAsync(model.Id);
            if(currentTag == null)
                return false;

            currentTag.Convert(model);
            if(await _tagRepository.UpdateAsync(currentTag) == 0) return false;
            return true;
        }

        public async Task<bool> DeleteTagAsync(int id)
        {
            var currentTag = await GetTagByIdAsync(id);
            if (currentTag == null) return false;

            if(await _tagRepository.DeleteAsync(currentTag) == 0) return false;
            return true;
        }

        private async Task<IEnumerable<string>> CreateUndefinedTags(string stringTags)
        {
            var tagSet = Regex.Replace(stringTags, @"\s+", " ").Trim().Split(" ").ToHashSet();
            var allTagsName = (await _tagRepository.GetAllAsync()).Select(t => t.Name);
            var createdTags = tagSet.Except(allTagsName);

            foreach (var tagName in createdTags)
                await _tagRepository.CreateAsync(new Tag(tagName));

            return tagSet;
        }
    }
}
