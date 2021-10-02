using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameSky.Areas.Identity.Pages.Account.Manage
{
    public class SupportTicketsModel : PageModel
    {
        public DataContext Db { get; set; }
        public List<Ticket> _new {get; set;}
        public List<Ticket> _todo { get; set; }
        public List<Ticket> _completed { get; set; }

        private UserManager<ApplicationUser> _userManager { get;}
        public SupportTicketsModel(DataContext db, UserManager<ApplicationUser> userManager)
        {
            Db = db;
            _userManager = userManager;
        }

        public void OnGet()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = HttpContext.User;
            var _user = _userManager.GetUserAsync(currentUser).Result;

            var tickets = Db.GetTicketsForUser(_user.Id);
            _new = tickets.Where(x => x.Status == TicketStatus.New).ToList();
            _todo = tickets.Where(x => (x.Status == TicketStatus.Taken) || (x.Status == TicketStatus.Responded) || (x.Status == TicketStatus.Needs_Attention)).ToList();
            _completed = tickets.Where(x => x.Status == TicketStatus.Closed).ToList();

        }
    }
}
