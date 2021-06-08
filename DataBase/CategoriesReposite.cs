using TestPostgrasql.Models;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace TestPostgrasql.DataBase
{
    public class CategoriesReposite
    {
        private readonly MyDbContext context;
        public CategoriesReposite(MyDbContext context)
        {
            this.context = context;
        }
        public IQueryable<Category> GetAllCategories()
        {
            return context.Category;
        }
        public Category GetCategory(int id)
        {
            return context.Category.FirstOrDefault(x => x.CategoryId == id);
        }
        public async Task AddCategory(Category category)
        {
            if(category.CategoryId == 0)
				context.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Added;
			else
				context.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
			await context.SaveChangesAsync();
		
        }
        public async Task RemoveCategory(Category category)
        {
            context.Category.Remove(category);
            await context.SaveChangesAsync();
        }
        public Category FindCategoryByName(string name)
        {
            return context.Category.FirstOrDefault(x => x.Name == name);
        }
    }
}