using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;
using GameSky.Proccessors;

namespace GameSky.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<NewsHeader> NewsHeaders { get; set; }
        public List<Match> results { get; set; }
        private readonly DataContext _db;
        private int NewsToTake = 8;
        private Stopwatch stopwatch = new Stopwatch();
        public string elapsedTime = "puste";
        public List<Player> BestPlayerOnMarket { get; set; }
        public WarningBar Warning { get; set; }


        public IndexModel(ILogger<IndexModel> logger, DataContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            NewsHeaders = await _db.GetPublishedNews(NewsToTake);
            Warning = _db.GetWarningBar();
            ViewData["Warning"] = Warning;
            results = Task.Run(() => _db.GetFinishedMatches().Take(15).ToList()).Result;
            BestPlayerOnMarket = _db.GetPlayersForMarket(2).ToList();
            return Page();
        }


        public JsonResult OnPostUsersGold()
        {

            System.Security.Claims.ClaimsPrincipal currentUser = this.User;

            ApplicationUser user = _db.GetUserByName(currentUser.Identity.Name);
            if (user is null) return new JsonResult("-1");
            return new JsonResult(string.Format("{0:N}", user.Gold));
        }
    }
}
