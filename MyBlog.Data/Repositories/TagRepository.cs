using MyBlog.Data.Entities.Tags;

namespace MyBlog.Data.Repositories
{
    public class TagRepository : Repository<TagEntity>
    {
        public TagRepository(MyBlogContext context) : base(context) { }
    }
}
