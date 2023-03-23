using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BusinesLogic.Models
{
    public class Comment
    {
        public int Id { get; }
        public int UserId { get; }
        public int BlogId { get; }
        public DateTime DateCreated { get; }
        public string Text { get; }
    }
}
