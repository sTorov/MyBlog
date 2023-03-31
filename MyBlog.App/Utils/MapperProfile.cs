using AutoMapper;
using MyBlog.App.ViewModels.Comments;
using MyBlog.App.ViewModels.Posts;
using MyBlog.App.ViewModels.Users;
using MyBlog.Data.DBModels.Comments;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Users;

namespace MyBlog.App.Utils
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

            CreateMap<PostCreateViewModel, Post>();
            CreateMap<Post, PostEditViewModel>();

            CreateMap<CommentCreateViewModel, Comment>();
        }
    }
}
