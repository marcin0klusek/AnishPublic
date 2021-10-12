using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameSky.Pages
{
    public class PostModel : PageModel
    {
        public NewsHeader news;
        public NewsContent content;
        private readonly DataContext _db;

        public PostModel(DataContext db)
        {
            _db = db;
        }

        public IActionResult OnGet(int id)
        {
            news = (NewsHeader)_db.GetNewsHeaderIncludeContent(id);
            
            if (!news.IsPublished || news.NewsContent is null)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}
