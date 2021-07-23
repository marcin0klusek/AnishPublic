using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFDataAccessLibrary.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameSky.Pages
{
    public class PlayersModel : PageModel
    {
        private readonly DataContext _db;
        public ICollection players;
        public ICollection teams;

        public PlayersModel(DataContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            players = _db.GetPlayersIncludePosition();
            teams = _db.GetTeams();
            //stworzyæ backend do pobierania, jest blad Invalid Column Name
        }

        public static string ColorSkillGroup(int skillPoints)
        {
            if (skillPoints <= 3)
            {
                return "#FF0000";
            }else if(skillPoints >= 8 && skillPoints <= 9)
            {
                return "#23FF00";
            }else if(skillPoints == 10)
            {
                return "#ffd700";
            }
            return "#fff";
        }
    }
}
