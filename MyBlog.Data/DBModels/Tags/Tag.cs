﻿using MyBlog.Data.DBModels.Posts;

namespace MyBlog.Data.DBModels.Tags
{
    /// <summary>
    /// Сущность тега
    /// </summary>
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
