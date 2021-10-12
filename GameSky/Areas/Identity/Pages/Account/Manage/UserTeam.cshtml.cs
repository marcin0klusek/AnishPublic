using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameSky.Areas.Identity.Pages.Account.Manage
{
    public class UserTeamModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;

        public DataContext Db { get; }
        public INotyfService Notyf { get; }
        public Boolean HasTeam = false;
        public Team userTeam { get; set; }

        public UserTeamModel(DataContext db, UserManager<ApplicationUser> manager, INotyfService notyf)
        {
            Db = db;
            userManager = manager;
            Notyf = notyf;
        }

        public void OnGet()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = HttpContext.User;
            var _user = userManager.GetUserAsync(currentUser).Result;
            if(_user.OwningTeamId is not null)
            {
                HasTeam = true;
                userTeam = Db.GetTeamByID((int)_user.OwningTeamId);
            }
        }

        public async Task<IActionResult> OnPostCreateTeam(IFormCollection form)
        {

            System.Security.Claims.ClaimsPrincipal currentUser = HttpContext.User;
            var _user = userManager.GetUserAsync(currentUser).Result;

            if (_user.OwningTeamId is not null)
            {
                return RedirectToPage("UserTeam");
            }

            string teamname = form["TeamName"].ToString();
            string tag = form["Tag"].ToString();

            Team team = Db.GetTeamByName(teamname);
            if(team is not null)
            {
                Notyf.Error("Dru¿yna o takiej nazwie ju¿ istnieje!");
                return Page();
            }

            team = new Team { 
                TeamName = teamname,
                Tag = tag,
                Country = "PL",
                OwnerID = _user.Id
            };

            Db.Team.Add(team);
            _user.OwningTeam = team;
            var result = Db.SaveChanges();
            if(result > 0)
            {
                Notyf.Success("Poprawnie stworzono dru¿ynê");
            }

            return RedirectToPage("UserTeam");
        }
    }
}
