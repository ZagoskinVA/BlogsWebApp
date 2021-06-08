
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestPostgrasql.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using TestPostgrasql.Email;

namespace TestPostgrasql.Controllers
{
    public class AccountController: Controller
    {
    	private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
    	public AccountController (UserManager<User> userManager,SignInManager<User> signInManager,RoleManager<IdentityRole> roleManager) 
    	{
    		this.userManager = userManager;
    		this.signInManager = signInManager;
            this.roleManager = roleManager;
    	}
    	[HttpGet]
    	public IActionResult Register()
    	{
            if(User.Identity.Name != null)
                return RedirectToAction("UserPage","User");
    		return View();
    	}
    	[HttpPost]
    	public async Task<IActionResult> Register(RegisterModel model)
    	{
    		if(ModelState.IsValid)
    		{

                var name = await userManager.FindByNameAsync(model.Email);
                if(name != null)
                {

                    ModelState.AddModelError(string.Empty,"Такой логин уже существует");
                    return View(model);
                }
    			var user = new User {NickName = model.NickName, Email = model.Email,UserName = model.Email, EmailConfirmed = true};
    			var result = await userManager.CreateAsync(user,model.Password);
    			if(result.Succeeded)
    			{
                    user = await userManager.FindByEmailAsync(model.Email);
                    user.EmailConfirmed = true;
                    await userManager.AddToRoleAsync(user, "user");
                    await userManager.UpdateAsync(user);
                    await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                    return RedirectToAction("UserPage","User");
                    /*
                    var email = new EmailService();
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);
                    await email.SendEmailAsync(model.Email, "Confirm your account",
                        $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");
    				return Content("Подтвердите регистрацию пройдя по ссылке в письме");*/
    			}
    			else
    			{
    				foreach(var error in result.Errors)
    					ModelState.AddModelError(string.Empty,error.Description);
    			}

    		}
    		return View(model);
    	}

        [HttpGet]
        public IActionResult Login()
        {
            if(User.Identity.Name != null)
                return RedirectToAction("UserPage","User");
            
            return View();
        } 

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if(user != null)
                {
                    if (!await userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, "Вы не подтвердили свой email");
                        return View(model);
                    }
                }


                var  result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.Remember, false);
                if(result.Succeeded)
                {
                    return RedirectToAction("UserPage","User");
                }
                ModelState.AddModelError("", "Неправильный логин или пароль");
            }
            return View(model);
        }



    	[Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Register", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await userManager.ConfirmEmailAsync(user, code);
            if(result.Succeeded)
            {
                await userManager.AddToRoleAsync(user,"user");
                return RedirectToAction("Login", "Account");
            }
            else
                return View("Error");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string email)
        {
            if(email != null)
            {
                var user = await userManager.FindByEmailAsync(email);
                if(user != null)
                {
                    var resetPasswordToken = await userManager.GeneratePasswordResetTokenAsync(user);
                    var url = Url.Action(
                        "ChangePassword",
                        "Account",
                        new {userId = user.Id,token = resetPasswordToken},
                        protocol: HttpContext.Request.Scheme);
                   // var emaiService = new EmailService();
                    //await emaiService.SendEmailAsync(email, 
                        //"Сброс пароля",$"Для изменения пароля на сайте перейдите по ссылке:<a href = {url}>link</a>");
                    return Content("Чтобы изменить пароль перейдите по ссылке в письме");
                }
            }
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ChangePassword(string userId,string token)
        {
            var model = new ChangePasswordModel {Id = userId,ResetPasswordToken = token};
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.Id);
                if(user != null)
                {
                   var result = await userManager.ResetPasswordAsync(user,model.ResetPasswordToken,model.Password);
                   if(result.Succeeded)
                    return RedirectToAction("Login");
                }
                else
                    return View("Error");
            }
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult AccessDenied() 
        {
            return Content("Доступ запрещён");
        }





        /*
        public async Task<IActionResult> LoadData()
        {
            string str = "";
            List<Task> tasks = new List<Task>();

            using(var sr = new StreamReader("Controllers/TestData"))
            {
                str = sr.ReadToEnd();

            }
            var list = str.Split(')');
            
            foreach(var s in list)
            {
                var spt = s.Split(',');
                var nick = spt[0];
                var email = spt[4];
                var pass = spt[6];
                var user = new RegisterModel {NickName = nick, Email = email,Password = pass,ConfirmPassword = pass};
                await Register(user);
                int i = 0;
            }

            return Content($"Успех ");

        }*/



    }
}