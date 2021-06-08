using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestPostgrasql.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using TestPostgrasql.DataBase; 
namespace TestPostgrasql.Controllers
{
    public class UserController:Controller
    {
        private readonly UserManager<User> userManager;
    	private readonly BlogsReposite blogsReposite;

		private readonly RoleManager<IdentityRole> roleManager;
    	public UserController (UserManager<User> userManager,BlogsReposite blogsReposite,RoleManager<IdentityRole> roleManager) 
    	{
		  this.roleManager = roleManager;
    	  this.userManager = userManager;
    	  this.blogsReposite = blogsReposite;
    	}
    	[Authorize]
		public async Task<IActionResult> UserPage()
		{
			var names = new List<string>();
			var name = User.Identity.Name;
			var user = await userManager.FindByNameAsync(name);
			var blogs = blogsReposite.GetBlogsByUserId(user.Id).ToList();
			var roles = await userManager.GetRolesAsync(user);
			user.Blogs = blogs;
			if(roles.FirstOrDefault(x => x == "admin") != null || roles.FirstOrDefault(x => x == "moderator") != null)
			{
				ViewBag.IsAdmin = true;
				ViewBag.IsModerator = true;
			}
			else
			{

				ViewBag.IsAdmin = true;
				ViewBag.IsModerator = true;
			}
			return View(user);
		}

		[Authorize(Roles= "admin,moderator")]
		public async Task<IActionResult> ControlUsers()
		{
			var name = User.Identity.Name;
			var user = await userManager.FindByNameAsync(name);
			var roles = await userManager.GetRolesAsync(user);
			var users = userManager.Users.ToList();
			foreach(var x in users)
			{
				var rls = await userManager.GetRolesAsync(x);
				x.Roles = rls.ToList();
			}
			users = users.Where(x => x.Roles[0].CompareTo(roles[0]) == 1).ToList();
			ViewBag.CurrentRole = roles[0];
			
			return View(users);
		}

		[HttpPost]

		public async Task<IActionResult> DeleteUser(string id)
		{
			var user = await userManager.FindByIdAsync(id);
			var blogs = blogsReposite.GetBlogsByUserId(user.Id);
			foreach(var blog in blogs)
			{
				await blogsReposite.RemoveBlogAsync(blog);
			}
			await userManager.DeleteAsync(user);
			return RedirectToAction("ControlUsers","User");
		}

		[HttpPost]
		public async Task<IActionResult> ChangeRole(string id)
		{
			var user = await userManager.FindByIdAsync(id);
			var userRoles = await userManager.GetRolesAsync(user);
			var allRoles =  roleManager.Roles.ToList();
			var model = new ChangeRolesModel
			{
				UserId = user.Id,
				Email = user.Email,
				AllRoles = allRoles,
				UserRoles = userRoles			
			};
			return View(model);
		}

		[HttpPost]
        public async Task<IActionResult> ChangeRoles(string email, List<string> roles)
        {
            var user = await userManager.FindByEmailAsync(email);
            if(user != null)
            {
                // получем список ролей пользователя
                var userRoles = await userManager.GetRolesAsync(user);
                // получаем все роли
                var allRoles = roleManager.Roles.ToList();
                // получаем список ролей, которые были добавлены
                var addedRoles = roles.Except(userRoles);
                // получаем роли, которые были удалены
                var removedRoles = userRoles.Except(roles);
 
                await userManager.AddToRolesAsync(user, addedRoles);
 
                await userManager.RemoveFromRolesAsync(user, removedRoles);
 
                return RedirectToAction("ControlUsers","User");
            }
 
            return NotFound();
        }




    }
}