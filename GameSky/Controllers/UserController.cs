using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameSky.Controllers
{
    [Controller]
    public class UserController : Controller
    {
        public UserController(DataContext db, UserManager<ApplicationUser> manager)
        {
            Db = db;
            Manager = manager;
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
    }
}
