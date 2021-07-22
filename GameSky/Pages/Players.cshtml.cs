using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFDataAccessLibrary.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameSky.Pages
{
    public class PlayersModel : PageModel
    {
        private readonly DataContext _db;
        public ICollection players;

        public PlayersModel(DataContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            //stworzyæ backend do pobierania, jest blad Invalid Column Name
        }
    }
}
