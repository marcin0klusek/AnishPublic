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
        public int playersPerPage = 6;
        public int playersCount = 0;

        public PlayersModel(DataContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            players = _db.GetPlayersIncludePosition(0, playersPerPage);
            teams = _db.GetTeams();
            playersCount = _db.Player.Count();
        }

        public PartialViewResult OnGetPlayersWithFilters(int pageNum, int pageSize, int teamID, string query)
        {
            if (query == null) query = String.Empty;
            List<Player> playersInTeam;

            if (teamID == -1) 
            {
                playersInTeam = _db.GetPlayersIncludePosition();
            }
            else
            {
                playersInTeam = _db.Team.Where(t => t.TeamID == teamID)
                    .SelectMany(t => t.PlayerTeam.Where(x => x.ExitDate == null).Select(p => p.Player)).Include(p => p.PlayerPosition)
                    .ToList();
            }

            this.playersCount = playersInTeam.Count();

            if (query != String.Empty)
            {
                playersInTeam = playersInTeam.Where(p => (p.FirstName.Contains(query)) 
                || (p.LastName.Contains(query))
                || (p.NickName.Contains(query))).ToList();
            }

            playersInTeam = playersInTeam.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();

            return Partial("~/Pages/Shared/PartialViews/_PlayersPage.cshtml", playersInTeam);
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
