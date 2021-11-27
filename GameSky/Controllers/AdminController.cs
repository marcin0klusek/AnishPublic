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
using System.Text.Json;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace GameSky.Controllers
{
    [Authorize(Roles = "Admin")]
    [Controller]
    public class AdminController : Controller
    {
        public DataContext Db { get; }
        public INotyfService Notyf { get; }
        private UserManager<ApplicationUser> userManager { get; set; }
        public IHubContext<MatchesHub> MatchesHub { get; }
        public AdminController(DataContext db, INotyfService notyf, UserManager<ApplicationUser> userManager, IHubContext<MatchesHub> matchesHub)
        {
            Db = db;
            Notyf = notyf;
            this.userManager = userManager;
            MatchesHub = matchesHub;
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
            match.StartDate = DateTime.SpecifyKind(DateTime.Parse(form["StartDate"].ToString()), DateTimeKind.Utc).AddHours(-1);

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

        #region NewsHeaderUpdate
        [Route("admin/update")]
        public IActionResult IndexUpdate()
        {
            var news = Db.GetNewsUpdatesHeaders();
            return View("Updates/List", news);
        }

        [Route("/admin/update/create")]
        public IActionResult UpdateCreate()
        {
            return View("Updates/Create");
        }
        [Route("/admin/CreateNewsHeaderUpdate")]
        public IActionResult CreateNewsHeaderUpdate(NewsHeader news)
        {
            if(news is null)
            {
                Console.WriteLine("News jest pusty");
                Notyf.Error("Coś poszło nie tak. Kod błędu: 1");
            }
            if(news.NewsTitle is null || news.NewsTitle == String.Empty)
            {
                Console.WriteLine("Zawartość NEWS jest pusta...");
                Notyf.Error("Coś poszło nie tak. Kod błędu: 2");
            }
            else
            {
                news.NewsCreateDate = DateTime.Now;
                Console.WriteLine("Zawartosć NEWS jest gitarka! :)))");
                Notyf.Success($"Poprawnie stworzono aktualizację: {news.NewsTitle}");
                ViewBag.NewsHeader = news;
                return View("Updates/UpdateCreate");
            }
            return RedirectToAction("UpdateCreate");
        }

        [HttpPost]
        [Route("/admin/CreateUpdateChanges")]
        public IActionResult CreateUpdateChanges(IFormCollection form)
        {
            NewsHeader n = JsonConvert.DeserializeObject<NewsHeader>(Request.Form["Header"]);
            var changesToParse = Request.Form["UpdateChanges"].ToString();
            NewsUpdate update = new NewsUpdate { Changes = ParseChanges(changesToParse) };
            n.NewsUpdate = update;

            Db.Add<NewsHeader>(n);

            var result = Db.SaveChanges();
            if(result > 0)
            {
                Notyf.Success("Dodano nowy wpis do bloga!");
                return RedirectToPage("/Changelogs");
            }else if(result == 0)
            {
                Notyf.Warning("Nie wykryto zmian!");
            }
            else
            {
                Notyf.Error("Coś poszło nie tak!");
            }

            return RedirectToAction("UpdateCreate");
        }

        private List<Change> ParseChanges(string text)
        {
            text = text.Trim();
            using (StringReader reader = new StringReader(text))
            {
                var changes = new List<Change>();
                int changeIndex = -1;
                bool isChange = false;

                var elements = new List<ChangeElement>();
                int elIndex = -1;
                bool isEl = false;

                Change lastChange = new Change();

                string line;
                while((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("/"))
                    {
                        if(elements.Count > 0)
                        {
                            lastChange.Elements = elements;
                            changes.Add(lastChange);
                            elements = new List<ChangeElement>();
                            elIndex = -1;
                        }
                        changeIndex++;
                        isChange = true;
                        isEl = false;
                        lastChange = new Change { Title = line[1..] };
                    }else if (line.StartsWith("-"))
                    {
                        elIndex++;
                        isChange = false;
                        isEl = true;
                        elements.Add(new ChangeElement { Text = line[1..] });
                    }
                    else
                    {
                        if (line.Trim() != String.Empty || line.Trim() != "")
                        {
                            try
                            {
                                if (isChange)
                                {
                                    changes.ElementAt(changeIndex).Title += line;
                                }
                                if (isEl)
                                {
                                    elements.ElementAt(elIndex).Text += line;
                                }
                            }
                            catch (IndexOutOfRangeException outOfRangeEx)
                            {
                                //do nothing
                            }
                        }
                    }
                }
                lastChange.Elements = elements;
                changes.Add(lastChange);
                return changes;
            }

        }
        [HttpPost]
        [Route("admin/DeleteUpdate")]
        public IActionResult DeleteUpdate(int id)
        {
            var news = Db.GetNewsUpdateById(id);
            if(news != null)
            {
                Db.Remove(news);
                var result = Db.SaveChanges();
                if(result > 0)
                {
                    Notyf.Success($"Usunięto akutalizację: {news.NewsTitle}");
                }
            }
            else
            {
                Notyf.Error("Nie odnaleziono aktualizacji do usunięcia");
            }
            return RedirectToAction("IndexUpdate");
        }
        #endregion
        #region WarningBar
        [Route("admin/warningbar")]
        [HttpGet]
        public IActionResult WarningBar()
        {
            ViewBag.Bar = Db.GetWarningBar();
            return View("WarningBar/Manage");
        }

        [Route("admin/warningbar")]
        [HttpPost]
        public IActionResult WarningBar(WarningBar bar)
        {
            bar.PublishDate = DateTime.Now;
            Db.Update(bar);
            var result = Db.SaveChanges();
            if(result < 0)
            {
                Notyf.Error("Nie udało się zapisać zmian");
            }
            ViewBag.Bar = bar;
            return View("WarningBar/Manage");
        }
        #endregion
    }

}
