using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using GameSky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameSky.Pages
{
    public class TeamModel : PageModel
    {
        public DataContext _db { get; private set; }
        public Team _Team { get; set; }
        public List<Event> _Events { get; set; }
        public List<Player> _ActivePlayers { get; set; }
        public List<Player> _PastPlayers { get; set; }
        public List<Match> _Matches { get; set; }
        private static Toaster _notyf;

        public TeamModel(DataContext db, INotyfService notyf)
        {
            _db = db;
            _notyf = new Toaster(notyf);
        }

        public IActionResult OnGet(int id)
        {
            _Team = _db.GetTeamByID(id);
            if (_Team is null) {
                _notyf.Warning("Dru¿yna o takim ID nie zosta³a odnaleziona!");
                return RedirectToPage("/Index"); 
            }
            _Events = _db.GetEventsForTeam(_Team.TeamID);
            _ActivePlayers = _db.GetActivePlayersForTeam(_Team.TeamID);
            _PastPlayers = _db.GetPastPlayersForTeam(_Team.TeamID);
            _Matches = _db.GetMatchesForTeam(_Team.TeamID);

            return Page();
        }
    }
}
