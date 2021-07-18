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
        private readonly DataContext _db;

        public IndexModel(ILogger<IndexModel> logger, DataContext db)
        {
            _logger = logger;
            _db = db;
        }

        [BindProperty(SupportsGet = true)]
        public string City { get; set; }


        public void OnGet()
        {

            if (_db.NewsHeader.Count() == 0)
            {
                City = "Pustki";
            }
            else
            {
                NewsHeaders = _db.NewsHeader.OrderByDescending(d => d.NewsPublishDate).Where(d=>d.isPublished == true).Take(5).ToList();
            }

            //Testing models and dependencies
            if (string.IsNullOrWhiteSpace(City))
            {
                City = "e-Sport";
            }
        }
    }
}
