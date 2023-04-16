﻿using Microsoft.EntityFrameworkCore;
using MyBlog.Data.DBModels.Comments;

namespace MyBlog.Data.Repositories
{
    public class CommentRepository : Repository<Comment>
    {
        public CommentRepository(MyBlogContext context) : base(context) { }

        public async Task<List<Comment>> GetCommentsByPostId(int postId) =>
            await Set.Where(c => c.PostId == postId).ToListAsync();

        public async Task<List<Comment>> GetCommentsByUserId(int userId) =>
            await Set.Where(c => c.UserId == userId).ToListAsync();
    }
}
