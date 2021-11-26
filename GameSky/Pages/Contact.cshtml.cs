using GameSky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Web.Helpers;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GameSky.Pages
{
    public class ContactModel : PageModel
    {
        public void OnGet()
        {
        }
        [BindProperty]
        public EmailModel Email { get; set; }
        
        public async Task OnPost()
        {
            var body = $"<p>Email From: {Email.EmailAddress}</p><p>Message:</p><p>{Email.Text}</p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress("marcin0klusek@o2.pl")); 
            message.From = new MailAddress(Email.EmailAddress); 
            message.Subject = Email.Topic;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "napalonytomek69@gmail.com", 
                    Password = "anime150"
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
        ViewData["Success"] = "Wiadomoœæ zosta³a wys³ana!";
        }
    }
}
