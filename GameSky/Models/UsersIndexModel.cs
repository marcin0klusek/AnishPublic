using EFDataAccessLibrary.DataAccess;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameSky.Models
{
    public class UsersIndexModel
    {
        private readonly DataContext db;
        public int usersAmount { get; set; }
        public int playersAmount { get; set; }
        public int eventsAmount { get; set; }
        public int ticketsAmount { get; set; }

        public UsersIndexModel(DataContext db)
        {
            this.db = db;
            usersAmount = db.Users.Count();
            playersAmount = db.Player.Count();
            eventsAmount = db.Event.Count();
            ticketsAmount = db.GetUpcomingMatches().Count();
        }
    }
}
