using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestPostgrasql.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using TestPostgrasql.DataBase;
namespace TestPostgrasql.Controllers
{
    public class CommentController : Controller
    {
    	private readonly CommentsReposite commentsReposite;
    	public CommentController(CommentsReposite commentsReposite) 
    	{
    	  this.commentsReposite = commentsReposite;
    	}
    	[HttpPost]
		public async Task<IActionResult> CreateComment(string text, string userId,long blogId)
		{
			long id = blogId;
			var comment = new Comment {Text = text, UserId = userId, BlogId = blogId};
			await commentsReposite.AddCommentAsync(comment);
			return RedirectToAction("OpenBlog","Blog",new {id});
		}
		[HttpPost]
		public async Task<IActionResult> RemoveComment(long id,long blogId)
		{
			var comment = commentsReposite.FindCommentById(id);
			await commentsReposite.RemoveCommentAsync(comment);
			id = blogId;
			return RedirectToAction("OpenBlog","Blog",new {id});
		}
    }
}