﻿using AutoMapper;
using MyBlog.App.Controllers;
using MyBlog.App.Utils.Extensions;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Tags;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Tags;
using MyBlog.Data.Repositories;
using MyBlog.Data.Repositories.Interfaces;

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

            _tagRepository = GetTagRepository();

        }
        private TagRepository GetTagRepository() => (TagRepository)_unitOfWork.GetRepository<Tag>();

        public async Task<Tag?> GetTagByIdAsync(int id) => await _tagRepository.GetAsync(id);

        public async Task<List<Tag>> GetAllTags() => await _tagRepository.GetAllAsync();

        public async Task<TagsViewModel?> GetTagsViewModelAsync(int? id)
        {
            var model = new TagsViewModel();

            if(id == null)
                model.Tags = await GetAllTags();
            else
            {
                var tag = await GetTagByIdAsync((int)id);
                if(tag != null) model.Tags.Add(tag);
            }

            return model;
        }

        public async Task<TagEditViewModel?> GetTagEditViewModelAsync(int id)
        {
            var tag = await GetTagByIdAsync(id);
            var model = tag == null 
                ? null   
                : _mapper.Map<TagEditViewModel>(tag);

            return model;
        }

        public async Task<Tag?> CheckDataAtCreateTagAsync(TagController controller, TagCreateViewModel model)
        {
            var checkTag = await _tagRepository.GetTagByNameAsync(model.Name);
            if (checkTag != null)
                controller.ModelState.AddModelError(string.Empty, $"Тег с именем [{model.Name}] уже существует!");

            return checkTag;
        }

        public async Task<Tag?> CheckDataAtEditTagAsync(TagController controller, TagEditViewModel model)
        {
            var checkTag = await _tagRepository.GetTagByNameAsync(model.Name);
            if (checkTag != null)
                controller.ModelState.AddModelError(string.Empty, $"Тег с именем [{model.Name}] уже существует!");

            return checkTag;
        }

        public async Task CreateTagAsync(TagCreateViewModel model)
        {
            var tag = _mapper.Map<Tag>(model);

            await _tagRepository.CreateAsync(tag);
        }

        public async Task<bool> UpdateTagAsync(TagEditViewModel model)
        {
            var currentTag = await GetTagByIdAsync(model.Id);
            if(currentTag == null)
                return false;

            currentTag.Convert(model);
            await _tagRepository.UpdateAsync(currentTag);
            return true;
        }

        public async Task<bool> DeleteTagAsync(int id)
        {
            var currentTag = await GetTagByIdAsync(id);
            if (currentTag == null)
                return false;

            await _tagRepository.DeleteAsync(currentTag);
            return true;
        }

    }
}