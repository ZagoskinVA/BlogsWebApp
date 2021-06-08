using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TestPostgrasql.Models
{
    public class Category
    {
        [Key]
        public int CategoryId{get;set;}
        public List<Blog> Blog {get;set;}

        [Required]
        public string Name {get;set;}
    }
}