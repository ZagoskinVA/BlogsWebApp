using BlogsWebAplication.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestPostgrasql.Models
{
    public class Blog
    {
    	[Required]
		[Display(Name = "Title")]
		public string Title{get;set;}
		[Required]
		[Display(Name = "Text")]
		public string Text {get;set;}
		[ForeignKey("UserId")]
		public string UserId{get;set;}
		public User User{get;set;}

		[Key]
		public long BlogId{get;set;}
		public int Rating {get;set;}
		public List<Comment> Comments {get;set;}
		[ForeignKey("CategoryId")]
		public int CategoryId {get;set;}
		
		public Category Category {get;set;}


		public byte[] Picture { get; set; }

		public List<LikedUser> LikedUsers { get; set; }



		
    }
}