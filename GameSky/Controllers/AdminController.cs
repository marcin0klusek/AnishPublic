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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameSky.Controllers
{
    [Authorize(Roles = "Admin")]
    [Controller]
    public class AdminController : Controller
    {
        public DataContext Db { get; }
        public INotyfService Notyf { get; }
        private UserManager<ApplicationUser> userManager { get; set; }
        public AdminController(DataContext db, INotyfService notyf, UserManager<ApplicationUser> userManager)
        {
            Db = db;
            Notyf = notyf;
            this.userManager = userManager;
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

        #region Ticket
        [Route("admin/tickets")]
        public IActionResult Tickets()
        {
            var tickets = Db.GetTickets();
            TicketsPackage package = new(
                //new tickets
                tickets.Where(x => x.Status == TicketStatus.New).ToList(),
                //tickets todo
                tickets.Where(x => (x.Status == TicketStatus.Taken)
                || (x.Status == TicketStatus.Responded)
                || (x.Status == TicketStatus.Needs_Attention)).ToList(),
                //tickets completed
                tickets.Where(x => x.Status == TicketStatus.Closed).ToList()

                );
            return View("Ticket/Index", package);
        }
        [Route("admin/ChangeTicketStatus")]
        public async Task<ActionResult> OnPostChangeTicketStatus(string Status, int ticketId)
        {
            TicketStatus myStatus = (TicketStatus)Enum.Parse(typeof(TicketStatus), Status, true);
            var ticket = Db.GetTicketById(ticketId);

            if(ticket.Status == TicketStatus.Taken && myStatus == TicketStatus.Taken)
            {
                Notyf.Error("Zgłoszenie jest już obsługiwane.");
                return new JsonResult(new { })
                {
                    StatusCode = 200,
                    Value = "" + TicketStatus.Taken
                };
            }

            ticket.Status = myStatus;
            ticket.LastModify = DateTime.Now;
            var result = Db.SaveChanges();

            //changes saved
            if (result > 0)
            {
                await CallTicketStatusChanges(ticket);
                Notyf.Information("Zmieniono status zgłoszenia.");
                return new JsonResult(new { })
                {
                    StatusCode = 200,
                    Value = ticket.LastModify.ToString("dd MMMM yyyy, HH:mm")
                };
            }
            else
            {
                Notyf.Error("Nie udało się zmienić statusu zgłoszenia.");
                return new JsonResult(new { })
                {
                    StatusCode = 200,
                    Value = "error"
                };
            }
        }
        private async Task CallTicketStatusChanges(Ticket ticket)
        {
            TicketHub.UpdateStatus(ticket.Id, ticket.Status);
            if(ticket.Status == TicketStatus.Closed)
            {
                TicketHub.CompleteTicket(ticket.Id);
            }
        }
        #endregion

    }

}
