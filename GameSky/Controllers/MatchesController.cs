using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFDataAccessLibrary.Models;
using EFDataAccessLibrary;
using EFDataAccessLibrary.DataAccess;
using GameSky.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using GameSky.Hubs;

namespace GameSky.Pages.Matches
{
    [Controller]
    public class  MatchesController : Controller
    {
        private readonly DataContext _db;
        private static Toaster _notyf;

        public IHubContext<MatchesHub> Hub;

        public MatchesController(DataContext db, INotyfService notyf, IHubContext<MatchesHub> hub)
        {
            _db = db;
            Hub = hub;
            _notyf = new Toaster(notyf);
        }

        [Route("results")]
        public ActionResult Results()
        {

            List<Match> _finishedMatches = Task.Run(() => _db.GetFinishedMatches()).Result;
            List<Match> _upcomingMatches = Task.Run(() =>_db.GetUpcomingMatches()).Result;
            List<Match> _live = _upcomingMatches.Where(x => (x.StartDate < DateTime.Now) && (x.EndDate is null) && (x.Map is not null)).ToList();
            _upcomingMatches = _upcomingMatches.Where(x => (x.StartDate > DateTime.Now) || x.Map is null).ToList();

            ViewBag.Live = _live;
            ViewBag.Upcoming = _upcomingMatches;
            ViewBag.Finished = _finishedMatches;

            if (_upcomingMatches != null && _finishedMatches != null) {
                _notyf.ShowInformation("Prawidłowo pobrano mecze");
            }
            else
            {
                _notyf.Warning("Coś poszło nie tak!");
            }

            return View();
        }

        [Route("match/{id}")]
        public async Task<ActionResult> Match(int id)
        {
            Match match = await Task.Run(() => _db.GetMatchById(id));
            if (match == null)
            {
                _notyf.ShowInformation("Nie można odnaleźć meczu o id: " + id);
                return RedirectToPage("/Index");
            }
            ViewBag.Match = match;
            if (match.EndDate != null)
            {
                return View("MatchFinished");
            }
            else
            {
                return View("MatchUpcoming");
            }
        }
    }
}
