using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using GameSky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameSky.Pages
{
    public class PlayersModel : PageModel
    {
        private readonly DataContext _db;
        public List<Player> players;
        public List<EFDataAccessLibrary.Models.Team> teams;
        public int playersPerPage = 5;
        public int playersCount = 0;

        public PlayersModel(DataContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            players = _db.GetPlayersForMarket(playersPerPage);
            teams = _db.GetTeams().Result;
            playersCount = _db.Player.Where(x => x.IsForSale).Count();
        }

        public PartialViewResult OnGetPlayersWithFilters(int pageNum, int pageSize, string roleName, string quality, string query)
        {
            if (query == null) { query = String.Empty; }
            List<Player> players;

            bool roleB = !roleName.Equals("Any");
            bool qualityB = !quality.Equals("Any");

            int total = 0;

            if(roleB && qualityB)
            {
                players = _db.Player.Include(p => p.PlayerPosition)
                    .Where(p => (p.IsForSale) && (p.PlayerPosition.Name == roleName) && (p.Quality == (PlayerCardQuality)Int32.Parse(quality)))
                    .OrderBy(p => p.Quality).ThenByDescending(p => p.PlayerLevel)
                    .Skip((pageNum-1)*pageSize).Take(pageSize)
                    .ToList();
                total = _db.Player.Include(p => p.PlayerPosition)
                    .Where(p => (p.IsForSale) && (p.PlayerPosition.Name == roleName) && (p.Quality == (PlayerCardQuality)Int32.Parse(quality))).Count();
            }
            else if (roleB)
            {
                players = _db.Player.Include(p => p.PlayerPosition)
                    .Where(p => (p.IsForSale) && p.PlayerPosition.Name == roleName)
                    .OrderBy(p => p.Quality).ThenByDescending(p => p.PlayerLevel)
                    .Skip((pageNum - 1) * pageSize).Take(pageSize)
                    .ToList();
                total = _db.Player.Include(p => p.PlayerPosition)
                    .Where(p => (p.IsForSale) && p.PlayerPosition.Name == roleName).Count();
            }
            else if (qualityB)
            {
                players = _db.Player.Include(p => p.PlayerPosition)
                    .Where(p => (p.IsForSale) && p.Quality == (PlayerCardQuality)Int32.Parse(quality))
                    .OrderBy(p => p.Quality).ThenByDescending(p => p.PlayerLevel)
                    .Skip((pageNum - 1) * pageSize).Take(pageSize)
                    .ToList();
                total = _db.Player.Include(p => p.PlayerPosition)
                    .Where(p => (p.IsForSale) && p.Quality == (PlayerCardQuality)Int32.Parse(quality)).Count();
            }
            else
            {
                players = _db.Player.Include(p => p.PlayerPosition)
                    .Where(p => p.IsForSale)
                    .OrderBy(p => p.Quality).ThenByDescending(p => p.PlayerLevel)
                    .Skip((pageNum - 1) * pageSize).Take(pageSize)
                    .ToList();
                total = _db.Player.Include(p => p.PlayerPosition)
                    .Where(p => p.IsForSale).Count();
            }

            if(query != String.Empty)
            {
                players = players.Where(
                    p => (p.FirstName.Contains(query) || p.NickName.Contains(query) || p.LastName.Contains(query))
                    )
                .ToList();
            }

            var playerspage = new PlayersPageModel()
            {
                Players = players,
                CurrentPage = pageNum,
                ItemsPerPage = playersPerPage,
                TotalAmount = total
            };

            return Partial("~/Pages/Shared/PartialViews/_PlayersPage.cshtml", playerspage);
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
