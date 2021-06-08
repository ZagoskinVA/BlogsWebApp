using TestPostgrasql.Models;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace TestPostgrasql.DataBase
{
    public class CommentsReposite
    {
		private readonly MyDbContext context;
		public CommentsReposite (MyDbContext context) 
		{
		  this.context = context;
		}
		public IQueryable<Comment> GetAllComments()
		{
			return context.Comment.OrderBy(x => x.Rating).Include(x => x.User);
		}
		public IQueryable<Comment> GetCommentsByBlogId(long id)
		{
			return context.Comment.Where(x => x.BlogId == id).OrderBy(x => x.Rating).Include(x => x.User);
		}

		public async Task AddCommentAsync(Comment comment)
		{
			if(comment.CommentId == 0)
				context.Entry(comment).State = Microsoft.EntityFrameworkCore.EntityState.Added;
			else
				context.Entry(comment).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
			await context.SaveChangesAsync();
		}

		public async Task RemoveCommentAsync(Comment comment)
		{
			context.Comment.Remove(comment);
			await context.SaveChangesAsync();
		}
		public Comment FindCommentById(long id)
		{
			return context.Comment.FirstOrDefault(x => x.CommentId == id);
		}

    }
}