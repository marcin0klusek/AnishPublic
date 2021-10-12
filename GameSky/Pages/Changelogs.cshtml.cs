using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameSky.Pages
{
    public class ChangelogsModel : PageModel
    {
        public List<NewsHeader> Updates { get; set; }
        private DataContext Db { get; }

        public ChangelogsModel(DataContext db)
        {
            Db = db;
        }
        public void OnGet()
        {
            Updates = Db.GetNewsUpdatesHeaders();
        }
    }
}
