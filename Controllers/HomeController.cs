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
using BlogsWebAplication.Models;


namespace TestPostgrasql.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlogsReposite blogsReposite;
        private readonly CategoriesReposite categoriesReposite;
        private readonly int pageSize = 15;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public HomeController(ILogger<HomeController> logger,BlogsReposite blogsReposite,
                                CategoriesReposite categoriesReposite, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            this.blogsReposite = blogsReposite;
            this.categoriesReposite = categoriesReposite;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        [HttpGet]
        public  IActionResult Index()
        {
            if (User.Identity.Name != null)
            {

                ViewBag.IsAuthorize = true;
            }
            else
                ViewBag.IsAuthorize = false;
            var blogs = blogsReposite.GetAllBlogs().Select(x => new DisplayBlogModel 
            {
                BlogId = x.BlogId,
                Title = x.Title,
                Text = x.Text,
                CategoryId = x.CategoryId,
                UserName = x.User.NickName
            }).Take(pageSize).ToList();
            ViewBag.Categories = categoriesReposite.GetAllCategories().ToList();
            return View(new BlogsViewModel 
            {
                Blogs = blogs,
                PaggingInfo = new PaggingInfo 
                {
                    CurrentPage = 1,
                    ItemsPerPage = pageSize,
                    TotalItems = blogsReposite.Count()
                }
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Shared/Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Search(string searchLine, string categoryId, int productPage = 1)
        {
            var blogs = blogsReposite.FindBlogsByText(searchLine);
            if (categoryId != "0")
                blogs = blogs.Where(x => x.CategoryId == int.Parse(categoryId)).ToList();
            var totalItems = blogs.Count;
            blogs = blogs.Skip((productPage - 1) * pageSize).Take(pageSize).ToList();
            return Json(new BlogsViewModel { 
                Blogs = blogs,
                PaggingInfo = new PaggingInfo 
                {
                    CurrentPage = productPage,
                    ItemsPerPage = pageSize,
                    TotalItems = totalItems
                }
            });
        }
    }
}
