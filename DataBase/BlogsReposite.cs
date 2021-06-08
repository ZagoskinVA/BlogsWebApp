using System.Security.Cryptography.X509Certificates;
using TestPostgrasql.Models;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BlogsWebAplication.Models;

namespace TestPostgrasql.DataBase
{
    public class BlogsReposite
    {
		private readonly MyDbContext context;
		public BlogsReposite (MyDbContext context) 
		{
		  this.context = context;
		}
		public Blog GetBlogById(long id)
		{
			return context.Blog.Include(x => x.Comments).ThenInclude(x => x.User).Include(x => x.User)
								.Include(x => x.Category)
								.FirstOrDefault(x => x.BlogId == id);
		}
		public IQueryable<Blog> GetAllBlogs()
		{
			return context.Blog.Include(x => x.Category).OrderBy(x => x.Rating).Include(x => x.User);
		}
		public IQueryable<Blog> GetBlogsByUserId(string userId)
		{
			
			return context.Blog.Include(x => x.Category).Where(x => x.User.Id == userId);
		}
		public async Task AddBlogAsync(Blog blog)
		{
			if(blog.BlogId == 0)
				context.Entry(blog).State = Microsoft.EntityFrameworkCore.EntityState.Added;
			else
				context.Entry(blog).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
			await context.SaveChangesAsync();
		}
		public async Task RemoveBlogAsync(Blog blog)
		{
			context.Blog.Remove(blog);
			await context.SaveChangesAsync();
		}

		public IQueryable<Blog> FindBlogsByCategory(int categoryId)
		{
			return context.Blog.Include(x => x.User).Include(x => x.Category).Where(x => x.CategoryId == categoryId);
		}

		public async Task<int> AddLike(long blogId, string userName)
		{
			var blog = context.Blog.Include(x => x.LikedUsers).FirstOrDefault(x => x.BlogId == blogId);
			if(blog.LikedUsers.Any(x => x.Name == userName))
			{
				blog.Rating--;
				var user = blog.LikedUsers.FirstOrDefault(x => x.Name == userName);
				blog.LikedUsers.Remove(user);
			}
			else
			{
				blog.Rating++;
				blog.LikedUsers.Add(new LikedUser { Name = userName, BlogId = blogId});
			}
			await AddBlogAsync(blog);
			return blog.Rating;
		}

		public List<DisplayBlogModel> FindBlogsByText(string searchLine)
		{
			IQueryable<Blog> blogs = context.Blog.Include(x => x.User);
			if (!string.IsNullOrEmpty(searchLine))
				blogs = context.Blog.Where(x => x.Title.Contains(searchLine) ||
				x.Text.Contains(searchLine)).OrderBy(x => x.Rating);
			return blogs.Select(x => new DisplayBlogModel 
			{
				BlogId = x.BlogId,
				Title = x.Title,
				Text = x.Text,
				CategoryId = x.CategoryId,
				UserName = x.User.NickName
			}).ToList();
		}

		
		public async Task SavePicture(byte[] picture, int blogId) 
		{
			var blog = GetBlogById(blogId);
			blog.Picture = picture;
			await AddBlogAsync(blog);
		}

		
		public byte[] LoadPicture(int blogId) 
		{
			var blog = GetBlogById(blogId);
			return blog.Picture;
		}

		public int Count() => context.Blog.Count();

    }
}