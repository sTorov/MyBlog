using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BusinesLogic.Models
{
    public class Post
    {
        public int Id { get; }
        public string Title { get; }
        public string Content { get; }
        public int UserId { get; }
        public DateTime CreatedDate { get; }

        public List<Tag> Tags { get; }
    }
}
