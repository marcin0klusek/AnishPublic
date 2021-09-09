using EFDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.DataAccess
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        
        #region Entities
        public DbSet<NewsHeader> NewsHeader { get; set; }
        public DbSet<NewsContent> NewsContent { get; set; }
        public DbSet<PlayerPosition> PlayerPosition { get; set; }
        public DbSet<Player> Player { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<PlayerTeam> PlayerTeam { get; set; }
        public DbSet<Map> Map { get; set; }
        public DbSet<Match> Match { get; set; }
        public DbSet<EventTeam> EventTeam { get; set; }
        public DbSet<Event> Event { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().HasKey(q => q.PlayerID);
            modelBuilder.Entity<Team>().HasKey(q => q.TeamID);

            modelBuilder.Entity<PlayerTeam>().HasKey(x =>
            new
            {
                x.PlayerID, x.TeamID
            });

            modelBuilder.Entity<EventTeam>().HasKey(x =>
            new
            {
                x.EventID,
                x.TeamID
            });
        }

        #region Methods

        #region NewsHeader
        public NewsHeader GetNewsHeaderIncludeContent(int id)
        {
            return NewsHeader.Where(a => a.NewsId == id).Include(e => e.NewsContent).FirstOrDefault();
        }

        public async Task<NewsHeader> GetFirstNewsHeader()
        {
            return await NewsHeader.FirstOrDefaultAsync(n => n.NewsId == 1);
        }

        public async Task<List<NewsHeader>> GetPublishedNews(int NewsToTake)
        {
            List<NewsHeader> news = new List<NewsHeader>();
            news = await Task.Run(() => NewsHeader
                .OrderByDescending(d => d.NewsPublishDate)
                .Where(d => d.IsPublished)
                .Take(NewsToTake)
                .ToListAsync());
            return news;
        }

        public async Task<List<NewsHeader>> GetNewsHeaderList()
        {
            return await NewsHeader.ToListAsync();
        }

        public async Task<NewsHeader> GetNewsHeaderById(int id)
        {
            return await NewsHeader.FirstOrDefaultAsync(x => x.NewsId == id);
        }

        public async Task<List<NewsHeader>> GetNewsHeaderListIncludeContent()
        {
            return await NewsHeader.Include(e => e.NewsContent).ToListAsync();
        }
        #endregion

        #region NewsContext

        #endregion

        #region PlayerPosition
        public async Task<PlayerPosition> GetPositionByName(string position)
        {
            PlayerPosition pos = await PlayerPosition.Where(x => x.Name == position).SingleOrDefaultAsync();
            return pos;
        }
        #endregion

        #region Player
        public async Task<List<Player>> GetPlayers()
        {
            return await Player.OrderBy(p => p.PlayerLevel).ToListAsync();
        }

        public  async Task<Player> GetPlayerByIdIncludePositon(int id) => await Player.Where(p => p.PlayerID == id).Include(p => p.PlayerPosition).FirstOrDefaultAsync();


        public async Task<List<Player>> GetPlayersIncludePosition()
        {
            return await Player.OrderBy(p => p.PlayerLevel).Include(p => p.PlayerPosition).ToListAsync();
        }

        public async Task<List<Player>> GetPlayersIncludePosition(int skip, int take)
        {
            return await Player.OrderBy(p => p.PlayerLevel).Skip(skip).Include(p => p.PlayerPosition).Take(take).ToListAsync();
        }
        #endregion

        #region Team
        public async Task<List<Team>> GetTeams()
        {
            return await Team.ToListAsync();
        }

        public Team GetTeamByID(int id)
        {
            return Team.FirstOrDefault(t => t.TeamID == id);
        }

        public List<Event> GetEventsForTeam(Team team)
        {
            return GetEventsForTeam(team.TeamID);
        }

        public List<Event> GetEventsForTeam(int teamid)
        {
            return Team.Where(t => t.TeamID == teamid)
                .SelectMany(t => t.EventTeams)
                .Select(x => x.Event)
                .OrderBy(e => e.StartDate)
                .ToList();
        }

        public List<Player> GetActivePlayersForTeam(Team team)
        {
            return GetActivePlayersForTeam(team.TeamID);
        }

        public List<Player> GetActivePlayersForTeam(int teamid)
        {
            return Team.Where(t => t.TeamID == teamid)
                    .SelectMany(t => t.PlayerTeam.Where(x => x.ExitDate == null).Select(p => p.Player))
                    .Include(p => p.PlayerPosition)
                    .ToList();
        }

        public List<Player> GetPastPlayersForTeam(int teamid)
        {
            return Team.Where(t => t.TeamID == teamid)
                    .SelectMany(t => t.PlayerTeam.Where(x => x.ExitDate < DateTime.Now).OrderByDescending(x => x.ExitDate).Select(p => p.Player))
                    .Include(p => p.PlayerPosition)
                    .ToList();
        }

        #endregion

        #region PlayerTeam

        #endregion

        #region Map

        #endregion

        #region Match

        public async Task<List<Match>> GetResults()
        {
            List<Match> results = new List<Match>();

            results = await Task.Run(() => Match.Where(x => x.EndDate != null)
                .OrderBy(x => x.StartDate)
                .Include(x => x.Team1)
                .Include(x => x.Team2)
                .Include(x => x.Event)
                .ToListAsync());

            return results;
        }

        public Match GetMatchById(int id)
        {
            return Match
                 .Include(x => x.Team1)
                 .Include(x => x.Team2)
                 .Include(x => x.Map)
                 .Include(x => x.Event)
                 .FirstOrDefault(m => m.MatchID == id);
        }

        public List<Match> GetUpcomingMatches()
        {
            return Match.Where(x => x.EndDate == null)
                .OrderBy(x => x.StartDate)
                .Include(x => x.Team1)
                .Include(x => x.Team2)
                .Include(x => x.Map)
                .Include(x => x.Event)
                .ToList();
        }

        public List<Match> GetFinishedMatches()
        {
            return Match.Where(x => x.EndDate != null)
                .OrderByDescending(x => x.EndDate)
                .Include(x => x.Team1)
                .Include(x => x.Team2)
                .Include(x => x.Event)
                .Include(x => x.Map)
                .ToList();
        }

        public List<Match> GetMatchesForTeam(int teamid)
        {
            return Match.Include(x => x.Team1)
                .Include(x => x.Team2)
                .Where(x => (x.Team1.TeamID == teamid || x.Team2.TeamID == teamid))
                .Include(x => x.Event)
                .ToList();
        }
        #endregion

        #region EventTeam

        #endregion

        #region Event
        public Event GetEventById(int id)
        {
            return Event.Where(e => e.EventID == id)
                .Include(x => x.EventTeams)
                .Include(x => x.Matches)
                .FirstOrDefault();
        }

        public List<Team> GetTeamsFromEvent(Event _event)
        {
            if(_event is null) { return new List<Team>(); }

            return Event.Where(e => e.EventID == _event.EventID)
                .SelectMany(x => x.EventTeams).Select(x => x.Team).ToList();
        }
        #endregion

        #endregion
    }
}