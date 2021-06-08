using System.ComponentModel.DataAnnotations;
namespace TestPostgrasql.Models
{
    public class LoginModel
    {
    	[Required]
        [Display(Name = "Email")]
    	public string Email {get;set;}
    	[Required]
    	[Display(Name="Password")]
    	[DataType(DataType.Password)]
    	public string Password{get;set;}
    	[Display(Name="Запомнить ?")]
    	public bool Remember {get;set;} = false;
	
    }
}