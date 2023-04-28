namespace MyBlog.Services.ViewModels.Users.Request
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string? LastName { get; set; }
        public string UserName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Photo { get; set; }
        public string Email { get; set; }
    }
}
