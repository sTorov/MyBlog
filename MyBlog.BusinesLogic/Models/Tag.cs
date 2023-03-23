namespace MyBlog.BusinesLogic.Models
{
    public class Tag
    {
        public int Id { get; }
        public string Name { get; }

        public List<Post> Posts { get; }
    }
}
