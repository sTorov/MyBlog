﻿using AutoMapper;
using MyBlog.Services.ViewModels.Comments.Response;
using MyBlog.Services.ViewModels.Posts.Request;
using MyBlog.Services.ViewModels.Posts.Response;
using MyBlog.Services.ViewModels.Roles.Request;
using MyBlog.Services.ViewModels.Roles.Response;
using MyBlog.Services.ViewModels.Tags.Request;
using MyBlog.Services.ViewModels.Tags.Response;
using MyBlog.Services.ViewModels.Users.Request;
using MyBlog.Services.ViewModels.Users.Response;
using MyBlog.Data.DBModels.Comments;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Roles;
using MyBlog.Data.DBModels.Tags;
using MyBlog.Data.DBModels.Users;

namespace MyBlog.Services
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserRegisterViewModel, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(m => m.Login))
                .ForMember(u => u.Email, opt => opt.MapFrom(m => m.EmailReg))
                .ForMember(u => u.PasswordHash, opt => opt.MapFrom(m => m.PasswordReg.GetHashCode()));
            CreateMap<User, UserEditViewModel>()
                .ForMember(u => u.Login, opt => opt.MapFrom(m => m.UserName));
            CreateMap<User, UserViewModel>();

            CreateMap<PostCreateViewModel, Post>();
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
        }
    }
}