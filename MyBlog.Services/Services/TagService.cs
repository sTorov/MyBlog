using AutoMapper;
using MyBlog.Services.Extensions;
using MyBlog.Services.Services.Interfaces;
using MyBlog.Services.ViewModels.Tags.Interfaces;
using MyBlog.Services.ViewModels.Tags.Request;
using MyBlog.Services.ViewModels.Tags.Response;
using MyBlog.Data.DBModels.Tags;
using MyBlog.Data.Repositories;
using MyBlog.Data.Repositories.Interfaces;
using System.Text.RegularExpressions;

namespace MyBlog.Services.Services
{
    /// <summary>
    /// Сервисы сущности тега
    /// </summary>
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

        public async Task<Tag?> GetTagByNameAsync(string name) => await _tagRepository.GetTagByNameAsync(name);

        public async Task<List<Tag>?> GetTagByPostAsync(int postId) => await _tagRepository.GetTagsByPostIdAsync(postId);

        public async Task<bool> CreateTagAsync(TagCreateViewModel model)
        {
            var tag = _mapper.Map<Tag>(model);

            if(await _tagRepository.CreateAsync(tag) == 0) return false;
            return true;
        }

        public async Task<List<Tag>> SetTagsForPostAsync(string? postTags)
        {
            var tags = new List<Tag>();

            if (postTags != null)
            {
                var tagSet = Regex.Replace(postTags, @"\s+", " ").Trim().Split(" ");

                foreach (var tagName in tagSet)
                {
                    var tag = await GetTagByNameAsync(tagName);
                    if (tag != null) tags.Add(tag);
                }
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

        public async Task<TagViewModel?> GetTagViewModelAsync(int id)
        {
            var tag = await _tagRepository.GetAsync(id);
            if (tag == null) return null;

            return _mapper.Map<TagViewModel>(tag);
        }
    }
}
