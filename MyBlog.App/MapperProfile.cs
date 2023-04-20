using AutoMapper;
using MyBlog.App.ViewModels.Comments;
using MyBlog.App.ViewModels.Posts;
using MyBlog.App.ViewModels.Roles;
using MyBlog.App.ViewModels.Tags;
using MyBlog.App.ViewModels.Users;
using MyBlog.Data.DBModels.Comments;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Roles;
using MyBlog.Data.DBModels.Tags;
using MyBlog.Data.DBModels.Users;

namespace MyBlog.App
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

            CreateMap<RoleCreateViewModel, Role>()
                .ForMember(m => m.NormalizedName, opt => opt.MapFrom(p => p.Name.ToUpper()));
            CreateMap<Role, RoleEditViewModel>();
            CreateMap<Role, RoleViewModel>();
        }
    }
}
