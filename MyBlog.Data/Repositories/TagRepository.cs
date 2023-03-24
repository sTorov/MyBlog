using MyBlog.Data.DBModels.Tags;

namespace MyBlog.Data.Repositories
{
    public class TagRepository : Repository<Tag>
    {
        public TagRepository(MyBlogContext context) : base(context) { }
    }
}
