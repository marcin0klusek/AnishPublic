using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFDataAccessLibrary.Models;
using EFDataAccessLibrary;
using EFDataAccessLibrary.DataAccess;
using GameSky.Data;
using GameSky.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GameSky.Pages.Matches
{
    [Controller]
    public class  MatchesController : Controller
    {
        private readonly DataContext _db;
        private static Toaster _notyf;
        public MatchesController(DataContext db, INotyfService notyf)
        {
            _db = db;
            _notyf = new Toaster(notyf);
        }

        [Route("results")]
        public ActionResult Results()
        {
        List<Match> _upcomingMatches =
                _db.Match
                .Where(x => x.EndDate == null)
                .OrderBy(x => x.StartDate)
                .Include(x => x.Team1)
                .Include(x => x.Team2)
                .Include(x => x.Map)
                .Include(x => x.Event)
                .ToList();

            List<Match> _finishedMatches = _db.Match
                .Where(x => x.EndDate != null)
                .OrderByDescending(x => x.EndDate)
                .Include(x => x.Team1)
                .Include(x => x.Team2)
                .Include(x => x.Event)
                .Include(x => x.Map)
                .ToList();

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

        [Route("matches/{id}")]
        public ActionResult Match(int id)
        {
            Match match = (Match)_db.Match
                .Include(x => x.Team1)
                .Include(x => x.Team2)
                .Include(x => x.Map)
                .Include(x => x.Event)
                .FirstOrDefault(m => m.MatchID == id);
            if (match == null)
            {
                _notyf.ShowInformation("Nie można odnaleźć meczu o id" + id);
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
        [Route("matches/matchheaderrefresh")]
        public ActionResult MatchHeaderRefresh(int id)
        {
            var newMatch = (Match)_db.Match
                .Include(x => x.Team1)
                .Include(x => x.Team2)
                .Include(x => x.Map)
                .Include(x => x.Event)
                .First(a => a.MatchID == id);
            if(newMatch != null)
            {
                _notyf.ShowInformation("Prawidłowo zaktualizowano nagłówek!");
            }
            else
            {
                _notyf.Warning("Nie udało się zaktualizować nagłówka.");
            }
            return PartialView("~/Views/Shared/Matches/_MatchHeader.cshtml", newMatch);
        }
    }
}
