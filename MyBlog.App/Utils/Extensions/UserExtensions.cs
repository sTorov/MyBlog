using MyBlog.App.ViewModels.Users.Response;
using MyBlog.Data.DBModels.Users;

namespace MyBlog.App.Utils.Extensions
{
    public static class UserExtensions
    {
        public static User Convert(this User user, UserEditViewModel model)
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
