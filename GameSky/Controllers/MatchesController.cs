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

        // GET: MatchesController
        [Route("results")]
        public ActionResult Results()
        {
            List<NewsHeader> aktualnosci = _db.NewsHeader.Take(4).ToList();

            if (aktualnosci == null)
            {
                Console.WriteLine("Aktualnosci sa puste");
                return RedirectToPage("./Index");
            }
            Console.WriteLine("Aktualnosci posiadaja: " + aktualnosci.Count);
            ViewBag.News = aktualnosci;
            return View();
        }

        [Route("matches/{id}")]
        public ActionResult Match(int id)
        {
            NewsHeader news = (NewsHeader)_db.NewsHeader.First(a => a.NewsId == id);
            if (news == null)
            {
                return RedirectToPage("./Index");
            }
            ViewBag.News = news;
            if (news.IsPublished)
            {
               // _notyf.ShowInformation("Mecz zakonczony");
                return View("MatchFinished");
            }
            else
            {
               // _notyf.ShowInformation("Mecz nadchodzacy");
                return View("MatchUpcoming");
            }
        }
        [Route("matches/matchheaderrefresh")]
        public ActionResult MatchHeaderRefresh(int id)
        {
            var newNews = (NewsHeader)_db.NewsHeader.First(a => a.NewsId == id);
            if(newNews != null)
            {
                _notyf.ShowInformation("Prawidłowo zaktualizowano nagłówek!");
            }
            else
            {
                _notyf.Warning("Nie udało się zaktualizować nagłówka.");
            }
            return PartialView("~/Views/Shared/Matches/_MatchHeader.cshtml", newNews);
        }
    }
}
