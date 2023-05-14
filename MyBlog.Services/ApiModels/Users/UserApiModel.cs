namespace MyBlog.Services.ApiModels.Users
{
    public class UserApiModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string? LastName { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string BirthDate { get; set; }
        public string Photo { get; set; }

        public List<string> Roles { get; set; }
    }
}
