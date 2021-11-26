using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using GameSky.Areas.Identity.Pages.Account;
using GameSky.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameSky.Controllers
{
    [Controller]
    public class UserController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public UserController(DataContext db, UserManager<ApplicationUser> manager, 
            SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger)
        {
            Db = db;
            Manager = manager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public DataContext Db { get; }
        public UserManager<ApplicationUser> Manager { get; }
        [Route("/user/PaymentDollars")]
        public async Task OnPostPaymentDollars(int amount)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = HttpContext.User;
            var _user = Manager.GetUserAsync(currentUser).Result;

            _user.Dollars += amount;
            _user.Gold += amount;

            Db.SaveChanges();
        }

        [Route("/user/SetNotificationAsReaded")]
        public async Task OnPostSetNotificationAsReaded(int NotifId)
        {
            var notif = Db.Notifications.Where(n => n.ID == NotifId).FirstOrDefault();
            notif.Readed = true;
            Db.SaveChanges();
        }


        [Route("/user/logindemo")]
        public async Task<IActionResult> LoginDemo(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var result = await _signInManager.PasswordSignInAsync(
                $"demo@demo.pl",
                "Demo123;",
                false,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return LocalRedirect(returnUrl);
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        [HttpPost]
        [Route("user/SendEmail")]
        public async Task<IActionResult> OnPostSendEmail(IFormCollection form)
        {
            string adr = form["EmailAddress"].ToString();
            string top = form["Topic"].ToString();
            string text = form["Text"].ToString();

            Console.WriteLine($"Email {top} od {adr}");
            return RedirectToPage("./Index");
        }

        [HttpPost]
        [Route("user/sendemail")]
        public async Task<IActionResult> OnPostSendEmail(EmailModel email)
        {
            Console.WriteLine($"Email {email.Topic} od {email.EmailAddress}");
            return RedirectToPage("./Index");
        }
    }
}
