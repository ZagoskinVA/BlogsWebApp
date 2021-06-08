using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TestPostgrasql.Models
{
    public class Comment
    {
		public string Text{get;set;}
		[ForeignKey("UserId")]
		public string UserId{get;set;}
		public User User{get;set;}
		[Key]
		public long CommentId{get;set;}
		public int Rating {get;set;} = 0;
		[ForeignKey("BlogId")]
		public long BlogId{get;set;}
		public Blog Blog{get;set;}
    }
}