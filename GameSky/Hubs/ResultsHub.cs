using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.SignalR;

namespace GameSky.Hubs
{
    public class ResultsHub : Hub
    {
        private readonly DataContext db;
        public static IHubContext<ResultsHub> Current { get; set; }

        public async Task UpdateScore(int matchId, int scoreTeam1, int scoreTeam2)
        {
            await Clients.All.SendAsync("UpdateScore", matchId, scoreTeam1, scoreTeam2);
        }

        /*public async Task GetUpdateOnScore(int matchId)
        {
            Match match = db.GetMatchById(matchId);
            if(match.ScoreTeam1 is null || match.ScoreTeam2 is null)
            {
                match.ScoreTeam1 = 0;
                match.ScoreTeam2 = 0;
            }
            bool team1 = false;
            if (match.ScoreTeam1 > match.ScoreTeam2)
            {
                team1 = match.ScoreTeam1 % 2 == 0 ? true : false;
            }
            else
            {
                team1 = match.ScoreTeam2 % 2 == 0 ? true : false;
            }
            if (team1)
            {
                match.ScoreTeam1 += 1;
            }
            else
            {
                match.ScoreTeam2 += 1;
            }

            if (match.ScoreTeam1 == 16 || match.ScoreTeam2 == 16)
            {
                match.EndDate = DateTime.Now;
                match.Map = db.Map.FirstOrDefault();
            }

            var result = await db.SaveChangesAsync();
            if(result > 0)
            {
                await UpdateScore(matchId, match.ScoreTeam1.Value, match.ScoreTeam2.Value);
            }
        }*/
    }
}
