using System.ComponentModel.DataAnnotations;
namespace TestPostgrasql.Models
{
    public class RegisterModel
    {
    	[Required(ErrorMessage="Имя обязательно")]
    	[Display(Name = "Имя")]
    	public string NickName {get;set;}
    	[Required(ErrorMessage="Email обязателен")]
    	[Display(Name = "Email")]
    	public string Email{get;set;}
    	[Required(ErrorMessage="Пароль обязателен")]
    	[DataType(DataType.Password)]
    	[Display(Name = "Пароль")]
    	public string Password{get;set;}
    	[Required(ErrorMessage="Подтверждение пароля обязательно")]
    	[DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    	[Display(Name="Подтвердите пароль")]
    	public string ConfirmationPassword{get;set;}
    }
}