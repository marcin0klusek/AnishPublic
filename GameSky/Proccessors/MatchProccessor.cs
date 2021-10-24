using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFDataAccessLibrary.Models;
using EFDataAccessLibrary.DataAccess;
using System.Threading;
using GameSky.Hubs;
using Microsoft.AspNetCore.SignalR;
using GameSky.Models;

namespace GameSky.Proccessors
{
    public class MatchProccessor
    {
        private DataContext db;
        private Match match;
        private List<Player> Team1, Team2;

        private Random rnd = new Random();
        private int round = 0;
        public MatchProccessor(DataContext db)
        {
            this.db = db;
        }

        public void StartMatch(Match match)
        {
            StartMatch(match.MatchID);
        }

        public void StartMatch(int matchId)
        {
            this.match = db.GetMatchById(matchId);
            Team1 = db.GetTeamByID(match.Team1.TeamID).GetActiveRoster();
            Team2 = db.GetTeamByID(match.Team2.TeamID).GetActiveRoster();
            StartMatch();
        }

        private async Task StartMatch()
        {
            if (match is null) return;
            match.ScoreTeam1 = 0;
            match.ScoreTeam2 = 0;

            var mapId = rnd.Next(1, db.Map.Count());
            match.Map = db.Map.FirstOrDefault(x => x.MapID == mapId);

            await SendAction("NewAction", $"Rozpoczęto mecz.");
            await ResultsHub.Current.Clients.All.SendAsync("MatchLive", match.MatchID, match.Map.Tag);
            Thread.Sleep(3000); // just to give time to move LIVE
            Console.WriteLine("Startuje wątek");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine(String.Format("Rozpoczęto mecz {0} vs {1}.",
                match.Team1.TeamName, match.Team2.TeamName
                ));
            Console.WriteLine("-----------------------------------");
           while (match.ScoreTeam1 < 16 && match.ScoreTeam2 < 16)
            {
                Thread.Sleep(2000);
                await PlayRound();
                await ResultsHub.Current.Clients.All.SendAsync("UpdateScore", match.MatchID, match.ScoreTeam1, match.ScoreTeam2);
                db.SaveChanges();
            }
            match.EndDate = DateTime.Now;
            var result = db.SaveChanges();
            string team1result = "won";
            string team2result = "lost";
            if(match.ScoreTeam1 < match.ScoreTeam2)
            {
                team1result = "lost";
                team2result = "won";
            }
            await SendAction("NewAction", $"Zakończono mecz.");
            await ResultsHub.Current.Clients.All.SendAsync("MatchEnded", match.MatchID, team1result, team2result);
        }

        private async Task PlayRound()
        {
            //Aliving every player
            Dictionary<string, Boolean> team1players = new();
            Dictionary<string, Boolean> team2players = new();
            foreach (var player in Team1)
            {
                team1players.Add(player.NickName, true);
            }
            foreach (var player in Team2)
            {
                team2players.Add(player.NickName, true);
            }
            //Starting round
            await SendAction("NewAction", $"Rozpoczęto roundę {round}");
            Console.WriteLine("----------- Runda " + (round++) + " --------------");

            int amountAliveT1 = team1players.Values.Where(x => x == true).Count();
            int amountAliveT2 = team2players.Values.Where(x => x == true).Count();
            while (amountAliveT1 > 0 && amountAliveT2 > 0)
            {
                string p1 = team1players.FirstOrDefault(p => p.Value == true).Key;
                string p2 = team2players.FirstOrDefault(p => p.Value == true).Key;

                if (rnd.Next(1, 100) <= 60)
                {
                    team1players[p1] = true;
                    team2players[p2] = false;
                    amountAliveT2--;
                    await SendAction("NewAction", $"{p1} {MatchActionEnum.Kill.ToString()} {p2}");
                }
                {

                    team2players[p2] = true;
                    team1players[p1] = false;
                    amountAliveT1--;
                    await SendAction("NewAction", $"{p2} {MatchActionEnum.Kill.ToString()} {p1}");
                }
                Thread.Sleep(1000);
            }

            if(amountAliveT1 > amountAliveT2)
            {
                match.ScoreTeam1 += 1;
                await SendAction("NewAction", $"Rundę wygrywa drużyna {match.Team1.TeamName}");
                Console.WriteLine($"Rundę wygrywa drużyna {match.Team1.TeamName}");
            }
            else
            {
                match.ScoreTeam2 += 1;
                await SendAction("NewAction", $"Rundę wygrywa drużyna {match.Team2.TeamName}");
                Console.WriteLine($"Rundę wygrywa drużyna {match.Team2.TeamName}");
            }
            Console.WriteLine(
                String.Format("{0} {1} - {2} {3}",
                match.Team1.TeamName, match.ScoreTeam1, match.ScoreTeam2, match.Team2.TeamName
                ));
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("");

            await SendAction("NewAction", $"{match.Team1.TeamName} {match.ScoreTeam1} - {match.ScoreTeam2} {match.Team2.TeamName}");
        }

        private async Task SendAction(string method, string text)
        {
            await MatchesHub.Current.Clients.Group($"Match{match.MatchID}").SendAsync(method, text);
            Console.WriteLine(text);
        }
    }
}
