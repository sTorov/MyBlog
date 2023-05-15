using MyBlog.Services.ViewModels.Users.Response;
using MyBlog.Data.DBModels.Users;
using MyBlog.Services.ViewModels.Users.Intefaces;

namespace MyBlog.Services.Extensions
{
    /// <summary>
    /// Расширения пользователя
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// Присвоение значений модели редактирования сущности пользователя
        /// </summary>
        public static User Convert(this User user, IUserUpdateModel model)
        {
            user.FirstName = model.FirstName;
            user.SecondName = model.SecondName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Login;
            user.BirthDate = model.BirthDate;
            user.Photo = model.Photo;

            return user;
        } 
    }
}
