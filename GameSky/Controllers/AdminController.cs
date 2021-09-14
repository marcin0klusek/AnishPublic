using AspNetCoreHero.ToastNotification.Abstractions;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameSky.Models;
using Hangfire;
using Hangfire.SqlServer;
using GameSky.Proccessors;
using Microsoft.AspNetCore.Http;
using GameSky.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace GameSky.Controllers
{
    [Authorize(Roles = "Admin")]
    [Controller]
    public class AdminController : Controller
    {
        public DataContext Db { get; }

        private IHubContext<ResultsHub> hubContext;

        public INotyfService Notyf { get; }
        public AdminController(DataContext db, INotyfService notyf)
        {
            Db = db;
            Notyf = notyf;
        }
        #region AdminController
        [Route("admin")]
        public IActionResult Index()
        {
            Notyf.Information("Nie ma gotowej strony Zarządzaj dla admina");

            UsersIndexModel model = new UsersIndexModel(Db);
            return View(model);
        }
        #endregion

        #region Event
        [Route("admin/event")]
        public IActionResult Event()
        {
            List<Event> events = Db.Event.ToList();
            return View("Event/Index", events);
        }
        #endregion

        #region Match
        [Route("admin/match")]
        public IActionResult Match()
        {
            List<Match> matches = Db.GetMatches().Result.OrderByDescending(x => x.MatchID).ToList();
            return View("Match/Index", matches);

        }
        [HttpGet]
        [Route("admin/match/add")]
        public IActionResult MatchAdd()
        {
            var teams = Db.GetTeams().Result;
            var events = Db.Event.ToList();
            ViewBag.Teams = teams;
            ViewBag.Events = events;
            return View("Match/Add");
        }
        [HttpPost]
        [Route("admin/match/add")]
        public async Task<IActionResult> OnPostMatchAdd(IFormCollection form)
        {
            Match match = new();

            match.Event = Db.GetEventById(Int32.Parse(form["Event"].ToString()));
            match.Team1 = Db.GetTeamByID(Int32.Parse(form["Team1.TeamID"].ToString()));
            match.Team2 = Db.GetTeamByID(Int32.Parse(form["Team2.TeamID"].ToString()));
            match.StartDate = DateTime.Parse(form["StartDate"].ToString());

            if (ModelState.IsValid)
            {
                Db.Add<Match>(match);
                var result = await Db.SaveChangesAsync();
                BackgroundJob.Schedule(() => new MatchProccessor(Db).StartMatch(match), match.StartDate);
            }
            else
            {
                Notyf.Error("Utworzenie meczu nie powiodło się.");
                return View("Match/Add");
            }
            List<Match> matches = Db.GetMatches().Result.OrderByDescending(x => x.MatchID).ToList();
            return Match();
        }
        #endregion

        #region News
        [Route("admin/news")]
        public IActionResult News()
        {
            List<NewsHeader> news = Db.GetNewsHeaderListIncludeContent().Result;
            return View("News/Index", news);
        }
        #endregion

        #region Players
        [Route("admin/players")]
        public IActionResult Players()
        {
            List<Player> players = Db.GetPlayersIncludePosition().Result;
            return View("Players/Index", players);
        }
        #endregion

        #region Teams
        [Route("admin/teams")]
        public IActionResult Teams()
        {
            List<Team> teams = Db.GetTeams().Result;
            return View("Teams/Index", teams);
        }
        #endregion

        #region User
        [Route("admin/users")]
        public new IActionResult User()
        {
            List<ApplicationUser> users = Db.Users.ToList();
            return View("User/Index", users);
        }
        #endregion

    }
}
