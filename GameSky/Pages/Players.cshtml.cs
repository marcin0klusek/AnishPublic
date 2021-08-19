using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameSky.Pages
{
    public class PlayersModel : PageModel
    {
        private readonly DataContext _db;
        public List<Player> players;
        public List<Team> teams;
        public int pagesOfPlayers = 0;
        public int playersPerPage = 6;

        public PlayersModel(DataContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            players = _db.GetPlayersIncludePosition(0, playersPerPage);
            teams = _db.GetTeams();
            int playersInDb = _db.Player.Count();
            pagesOfPlayers = (int)(_db.Player.Count() / playersPerPage);
            if (playersInDb % playersPerPage > 0) pagesOfPlayers++;
        }

        public PartialViewResult OnGetPlayers(int pageNum, int pageSize)
        {
            List<Player> items = _db.Player.Skip((pageNum - 1) * pageSize).Include(x => x.PlayerPosition).Take(pageSize).ToList();
            return Partial("~/Pages/Shared/PartialViews/_PlayersPage.cshtml", items);
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
