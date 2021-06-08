using System.Runtime.CompilerServices;
using System.Xml.Schema;
using System.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestPostgrasql.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using TestPostgrasql.DataBase;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace TestPostgrasql.Controllers
{
    public class BlogController : Controller
    {
		private readonly UserManager<User> userManager;
		private readonly BlogsReposite blogsReposite;
		private readonly CommentsReposite commentsReposite;
		private readonly CategoriesReposite categoriesReposite;
		public BlogController (UserManager<User> userManager, BlogsReposite blogsReposite,
								CommentsReposite commentsReposite,CategoriesReposite categoriesReposite) 
		{
		  this.userManager = userManager;
		  this.blogsReposite = blogsReposite;
		  this.commentsReposite = commentsReposite;
		  this.categoriesReposite = categoriesReposite;
		}
		
		[HttpGet]
        [AllowAnonymous]
		public async Task<IActionResult> OpenBlog(long id)
		{
			var blog = blogsReposite.GetBlogById(id);
        	var name = User.Identity.Name;
        	if(name == null)
        		return View(blog);
			var user = await userManager.FindByNameAsync(name);
			ViewBag.IsAuthorize = true;
			ViewBag.CurrentUserId = user.Id;
			return View(blog);
		}
		
		[HttpGet]
        [Authorize]
        public async Task<IActionResult> EditBlog(long id)
        {
        	var name = User.Identity.Name;
			var user = await userManager.FindByNameAsync(name);
			var blog = blogsReposite.GetBlogById(id);
			if(blog.UserId == user.Id)
				return View(blog);
			else
				return StatusCode(403);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditBlog(Blog blog, IFormFile file)
        {
        	if(ModelState.IsValid)
        	{
				byte[] picture = null;
				var newBlog = blogsReposite.GetBlogById(blog.BlogId);
				if(file != null)
					picture = StreamToByteArray(file.OpenReadStream());
				newBlog.Picture = picture;
				newBlog.Title = blog.Title;
				newBlog.Text = blog.Text;
        		await blogsReposite.AddBlogAsync(newBlog);
        		long id = blog.BlogId;
        		return RedirectToAction("OpenBlog","Blog",new {id});
        	}
        	else
        		return View(blog);
        }
		[HttpGet]
		[Authorize(Roles="admin,moderator")]
		public IActionResult AddCategory()
		{
			return View();
		}
		[HttpPost]
		[Authorize(Roles="admin,moderator")]
		public async Task<IActionResult> AddCategory(Category model)
		{
			if(ModelState.IsValid)
			{
				await categoriesReposite.AddCategory(model);
				return RedirectToAction("UserPage","User"); 
			}
			return View();

		}

		[Authorize]
		[HttpGet]
		public IActionResult CreateBlog()
		{
			ViewBag.Categories = categoriesReposite.GetAllCategories().ToList();
			return View();
		}
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> CreateBlog(Blog model,string categoryName, IFormFile file)
		{
			if(ModelState.IsValid)
			{
				byte[] picture = null;
				if(file != null)
					picture = StreamToByteArray(file.OpenReadStream());
				model.Picture = picture;
				var name = User.Identity.Name;
				var user = await userManager.FindByNameAsync(name);
				var category = categoriesReposite.FindCategoryByName(categoryName);
				model.CategoryId = category?.CategoryId ?? 0;
				model.UserId = user.Id;
				await blogsReposite.AddBlogAsync(model);
				return RedirectToAction("UserPage","User");
			}
			return View(model);
		}
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> RemoveBlog(long BlogId)
		{

			await blogsReposite.RemoveBlogAsync(blogsReposite.GetBlogById(BlogId));
			return RedirectToAction("UserPage","User");
		}

		[Authorize]
		[HttpPost]
		public async Task<JsonResult> SetLike(long blogId)
		{
			var name = User.Identity.Name;
			var rating = await blogsReposite.AddLike(blogId,name);
			return Json(new {Rating = rating});
		}

		[HttpPost]
		[Authorize]
		public async Task<JsonResult> RemovePicture(long blogId) 
		{
			var blog = blogsReposite.GetBlogById(blogId);
			blog.Picture = null;
			await blogsReposite.AddBlogAsync(blog);
			return Json(new { error = false });
		}


		public static byte[] StreamToByteArray(Stream input)
		{
			byte[] buffer = new byte[16 * 1024];
			using (MemoryStream ms = new MemoryStream())
			{
				int read;
				while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
				{
					ms.Write(buffer, 0, read);
				}
				return ms.ToArray();
			}
		}
	}
}