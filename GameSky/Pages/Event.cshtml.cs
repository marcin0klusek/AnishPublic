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
    public class EventModel : PageModel
    {
        private readonly DataContext _db;
        public Event _event { get;set; }
        public List<Team> _teams { get; set; }
        public List<Match> _matches { get; set; }

        public EventModel(DataContext db)
        {
            this._db = db;
        }
        public void OnGet(int id)
        {
            _event = _db.Event.Where(e => e.EventID == id)
                .Include(x => x.EventTeams)
                .Include(x => x.Matches)
                .FirstOrDefault();

            _teams =_db.Event.Where(e => e.EventID == id)
                .SelectMany(x => x.EventTeams).Select(x => x.Team).ToList();

            _matches = _event.Matches.ToList();
        }
    }
}
