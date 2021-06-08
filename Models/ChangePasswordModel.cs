using System.ComponentModel.DataAnnotations;
namespace TestPostgrasql.Models
{
    public class ChangePasswordModel
    {
    	[Required(ErrorMessage="Это поле обязателтно")]
    	[Display(Name="Новый пароль")]
		public string Password {get;set;}
		[Required(ErrorMessage="Это поле обязательно")]
		[Display(Name="Подтвердите пароль")]
		[Compare("Password",ErrorMessage="Пароли должны совпадать")]
		public string ConfirmationPassword {get;set;}
		public string ResetPasswordToken {get;set;}
		public  string Id {get;set;}

    }
}