using MyBlog.Data.Entities.Posts;

namespace MyBlog.Data.Entities.Tags
{
    public class TagEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<PostEntity> Posts { get; set; }
    }
}
