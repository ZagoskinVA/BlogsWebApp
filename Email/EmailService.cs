using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
namespace TestPostgrasql.Email
{
    public class EmailService
    {
		public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Администрация сайта", ""));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };
             
            using (var client = new SmtpClient())
            {
                client.CheckCertificateRevocation = true;
                await client.ConnectAsync("*********", 465, true);
                await client.AuthenticateAsync("", "");
                await client.SendAsync(emailMessage);
 
                await client.DisconnectAsync(true);
            }

        }
    }
}