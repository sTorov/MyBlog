using MyBlog.Data.DBModels.Tags;
using MyBlog.Data.DBModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data.DBModels.Posts
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }

        public int CreaterId { get; set; }
        public User Creater { get; set; }

        public List<Tag> Tags { get; set; }
    }
}
