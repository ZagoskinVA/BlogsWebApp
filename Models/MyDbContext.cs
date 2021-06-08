using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BlogsWebAplication.Models;

namespace TestPostgrasql.Models
{
    public class MyDbContext:IdentityDbContext<User>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options):base(options) {  }
        public DbSet<User> User {get; set;}
        public DbSet<Blog> Blog {get; set;}
        public DbSet<Comment> Comment{get; set;}
        public DbSet<Category> Category {get;set;}

        public DbSet<LikedUser> LikedUsers { get; set; }
        
    }

  
	
    
}