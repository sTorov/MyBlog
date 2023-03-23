using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BusinesLogic.Models
{
    public class User
    {
        public int Id { get; }
        public string FirstName { get; }
        public string SecondName { get; }
        public string LastName { get; }
        public DateTime BirthDate { get; }
        public string Email { get; }
        public string PasswordHash { get; }
        public string Login { get; }
        public string Photo { get; }

        public List<Post> Posts { get; }
        public List<Comment> Comments { get; }
    }
}
