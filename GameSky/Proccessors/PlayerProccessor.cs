using EFDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using RandomNameGeneratorLibrary;
using EFDataAccessLibrary.DataAccess;

namespace GameSky.Proccessors
{
    public class PlayerProccessor
    {
        static PersonNameGenerator generator = new();
        static Random random = new();
        public static Player GeneratePlayer(DataContext db)
        {
            Player p = new();
            p.FirstName = generator.GenerateRandomFirstName();
            p.LastName = generator.GenerateRandomLastName();
            p.NickName += p.FirstName.Length >= 3 ? p.FirstName[0..3] : p.FirstName;
            p.NickName += p.LastName.Length >= 3 ? p.LastName[0..3] : p.LastName;
            p.NickName += random.Next(100,1000);

            //BirthDate generator
            DateTime start = new DateTime(1990, 1, 1);
            int range = ((DateTime.Today.AddYears(-10)) - start).Days;
            p.BirthDate =
             start.AddDays(random.Next(range));

            //Random pick of PlayerPosition
            var positions = db.GetPlayerPositions();
            p.PlayerPosition = positions.ElementAt(random.Next(0, positions.Count()));
            p.PositionID = p.PlayerPosition.PositionID;

            p.Potencial = random.Next(0,4);
            p.Aim = random.Next(0, 4);
            p.Knowledge = random.Next(0, 4);
            p.Quality = GetRandomQuality();
            p.RecalcPlayerStats();
            return p;
        }

        private static PlayerCardQuality GetRandomQuality()
        {
            var qualities = Enum.GetValues(typeof(PlayerCardQuality)).Cast<PlayerCardQuality>().ToList();
            int total = 0;
            //Sum all values of Qualities
            foreach(var q in qualities)
            {
                total += (int)q;
            }
            
            var quality = random.Next(0, total + 1);
            //Pick the quality
            foreach(var q in qualities)
            {
                if(quality < (int)q)
                {
                    return q;
                }
                quality -= (int)q;
            }
            return PlayerCardQuality.Wooden;
        }
    }
}
