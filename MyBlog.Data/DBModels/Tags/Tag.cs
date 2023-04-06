using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Users;

namespace MyBlog.Data.DBModels.Tags
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Post> Posts { get; set; }

        public Tag() { }

        public Tag(string name) 
        {
            Name = name;
        }
    }
}
