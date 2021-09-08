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

namespace GameSky.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<NewsHeader> NewsHeaders { get; set; }
        public List<Match> results { get; set; }
        private readonly DataContext _db;
        private int NewsToTake = 15;
        private Stopwatch stopwatch = new Stopwatch();
        public string elapsedTime = "puste";


        public IndexModel(ILogger<IndexModel> logger, DataContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task OnGetAsync()
        {
            NewsHeaders = await _db.GetPublishedNews(NewsToTake);

            results = await _db.GetResults();
        }
    }
}
