﻿using AutoMapper;
using MyBlog.Services.ViewModels.Comments.Request;
using MyBlog.Services.ViewModels.Posts.Response;
using MyBlog.Services.ViewModels.Posts.Request;
using MyBlog.Services.ViewModels.Roles.Response;
using MyBlog.Services.ViewModels.Roles.Request;
using MyBlog.Services.ViewModels.Tags.Response;
using MyBlog.Services.ViewModels.Tags.Request;
using MyBlog.Services.ViewModels.Users.Response;
using MyBlog.Services.ViewModels.Users.Request;
using MyBlog.Data.DBModels.Comments;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Roles;
using MyBlog.Data.DBModels.Tags;
using MyBlog.Data.DBModels.Users;
using MyBlog.Services.ApiModels.Users.Response;
using MyBlog.Services.ApiModels.Tags.Response;
using MyBlog.Services.ApiModels.Posts.Response;
using MyBlog.Services.ApiModels.Comments.Response;
using MyBlog.Services.ApiModels.Roles.Response;
using MyBlog.Services.ViewModels.Posts.Interfaces;

namespace MyBlog.Services
{
    /// <summary>
    /// Конфигурация маппера
    /// </summary>
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserRegisterViewModel, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(m => m.Login))
                .ForMember(u => u.Email, opt => opt.MapFrom(m => m.EmailReg))
                .ForMember(u => u.PasswordHash, opt => opt.MapFrom(m => m.PasswordReg.GetHashCode()));
            CreateMap<User, UserEditViewModel>()
                .ForMember(u => u.Login, opt => opt.MapFrom(m => m.UserName))
                .ForMember(u => u.Roles, opt => opt.MapFrom(m => m.Roles.Select(r => r.Name)));
            CreateMap<User, UserViewModel>();

            CreateMap<IPostCreateModel, Post>();
            CreateMap<Post, PostEditViewModel>()
                .ForMember(m => m.PostTags, opt => opt.MapFrom(p => string.Join(" ", p.Tags.Select(p => p.Name))));
            CreateMap<Post, PostViewModel>();

            CreateMap<CommentCreateViewModel, Comment>();
            CreateMap<Comment, CommentEditViewModel>();

            CreateMap<TagCreateViewModel, Tag>();
            CreateMap<Tag, TagEditViewModel>();
            CreateMap<Tag, TagViewModel>();

            CreateMap<RoleCreateViewModel, Role>()
                .ForMember(m => m.NormalizedName, opt => opt.MapFrom(p => p.Name.ToUpper()));
            CreateMap<Role, RoleEditViewModel>();
            CreateMap<Role, RoleViewModel>();

            CreateMap<User, UserApiModel>()
                .ForMember(m => m.Login, opt => opt.MapFrom(u => u.UserName))
                .ForMember(m => m.Roles, opt => opt.MapFrom(u => u.Roles.Select(r => r.Name)))
                .ForMember(m => m.BirthDate, opt => opt.MapFrom(u => u.BirthDate.ToString("d")));

            CreateMap<Tag, TagApiModel>();

            CreateMap<Role, RoleApiModel>();

            CreateMap<Post, PostApiModel>()
                .ForMember(m => m.Tags, opt => opt.MapFrom(p => p.Tags.Select(t => t.Name)))
                .ForMember(m => m.CreatedDate, opt => opt.MapFrom(u => u.CreatedDate.ToString("d")));

            CreateMap<Comment, CommentApiModel>()
                .ForMember(m => m.CreatedDate, opt => opt.MapFrom(u => u.CreatedDate.ToString("d")));
        }
    }
}
