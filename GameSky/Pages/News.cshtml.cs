using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace GameSky.Pages
{
    public class NewsModel : PageModel
    {
        private readonly DataContext _db;
        private readonly INotyfService _notyf;
        public List<NewsHeader> newsList = new();
        private readonly int newsToTake = 10;
        public List<NewsHeader> newsToAdd = new();


        public NewsModel(DataContext db, INotyfService notyf)
        {
            _db = db;
            _notyf = notyf;
        }

        public IActionResult OnGetTakeNews(int step)
        {
            if(_db.NewsHeader.Any())
            {
                newsToAdd.Clear();
                newsToAdd = _db.NewsHeader
                    .OrderByDescending(n => n.NewsPublishDate)
                    .Where(n => n.IsPublished == true && n.NewsContent != null)
                    .Skip(newsToTake * step)
                    .Take(newsToTake)
                    .ToList();

                if (newsToAdd.Count > 0)
                {
                    newsList.AddRange(newsToAdd);
                    return Partial("~/Pages/Shared/_NewNews.cshtml", newsToAdd);
                }
                else
                {
                    _notyf.Warning("There is no more news to load.", 1);
                }
            }
            return Partial("~/Pages/Shared/_NewNews.cshtml", new List<NewsHeader>());
        }

        public void OnGet()
        {
            OnGetTakeNews(0); //step 0 to get first newsToTake
        }
    }
}
