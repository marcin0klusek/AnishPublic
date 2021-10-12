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
    public class UpdateModel : PageModel
    {
        public NewsHeader update { get; set; }
        private DataContext Db { get; }

        public UpdateModel(DataContext db)
        {
            Db = db;
        }
        public void OnGet(int id)
        {
            update = Db.GetNewsUpdateById(id);
        }
    }
}
