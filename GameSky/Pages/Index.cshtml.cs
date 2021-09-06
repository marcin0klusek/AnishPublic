using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameSky.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<NewsHeader> NewsHeaders = null;
        public List<Match> results { get; set; }
        private readonly DataContext _db;
        private int NewsToTake = 15;

        public IndexModel(ILogger<IndexModel> logger, DataContext db)
        {
            _logger = logger;
            _db = db;
        }


        public void OnGet()
        {
            NewsHeaders = _db.NewsHeader
                .OrderByDescending(d => d.NewsPublishDate)
                .Where(d=>d.IsPublished == true)
                .Take(NewsToTake)
                .ToList();
            results = _db.Match.Where(x => x.EndDate != null).OrderBy(x => x.StartDate)
                .Include(x => x.Team1)
                .Include(x => x.Team2)
                .Include(x => x.Event)
                .ToList();
        }
    }
}
