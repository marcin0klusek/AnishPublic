using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFDataAccessLibrary.Models;
using EFDataAccessLibrary.DataAccess;
using System.Threading;
using GameSky.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace GameSky.Proccessors
{
    public class MatchProccessor
    {
        private DataContext db;
        private Match match;

        public MatchProccessor(DataContext db)
        {
            this.db = db;
        }

        public void StartMatch(Match match)
        {
            this.match = db.GetMatchById(match.MatchID);
            StartMatch();
        }

        public void StartMatch(int matchId)
        {
            this.match = db.GetMatchById(match.MatchID);
            StartMatch();
        }

        private async Task StartMatch()
        {
            if (match is null) return;
            match.ScoreTeam1 = 0;
            match.ScoreTeam2 = 0;
            await ResultsHub.Current.Clients.All.SendAsync("UpdateScore", match.MatchID, match.ScoreTeam1, match.ScoreTeam2);
               
            Console.WriteLine("-----------------------------------");
            Console.WriteLine(String.Format("Rozpoczęto mecz {0} vs {1}.",
                match.Team1.TeamName, match.Team2.TeamName
                ));
            Console.WriteLine("-----------------------------------");
           while (match.ScoreTeam1 < 16 && match.ScoreTeam2 < 16)
            {
                Thread.Sleep(new Random().Next(1000, 3000));
                PlayRound();
                await ResultsHub.Current.Clients.All.SendAsync("UpdateScore", match.MatchID, match.ScoreTeam1, match.ScoreTeam2);
            }
            match.Map = db.Map.FirstOrDefault();
            match.EndDate = DateTime.Now;
           var result = db.SaveChanges();
        }

        private void PlayRound()
        {
            Console.WriteLine("----------- Runda " + (match.ScoreTeam1 + match.ScoreTeam2 + 1) + " --------------");
            Random rnd = new Random();
            if(rnd.Next(1, 100) <= 60)
            {
                match.ScoreTeam1 += 1;
                Console.WriteLine("Rundę wygrywa drużyna " + match.Team1.TeamName);
            }
            else
            {
                match.ScoreTeam2 += 1;
                Console.WriteLine("Rundę wygrywa drużyna " + match.Team2.TeamName);
            }
            Console.WriteLine(
                String.Format("{0} {1} - {2} {3}",
                match.Team1.TeamName, match.ScoreTeam1, match.ScoreTeam2, match.Team2.TeamName
                ));
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("");
        }
    }
}
