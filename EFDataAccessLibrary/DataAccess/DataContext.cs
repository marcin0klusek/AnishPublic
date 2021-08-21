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
        public DbSet<NewsHeader> NewsHeader { get; set; }
        public DbSet<NewsContent> NewsContent { get; set; }
        public DbSet<PlayerPosition> PlayerPosition { get; set; }
        public DbSet<Player> Player { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<PlayerTeam> PlayerTeam { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().HasKey(q => q.PlayerID);
            modelBuilder.Entity<Team>().HasKey(q => q.TeamID);

            modelBuilder.Entity<PlayerTeam>().HasKey(x =>
            new
            {
                x.PlayerID, x.TeamID
            });
        }

        public NewsHeader GetFirstNewsHeader()
        {
            return NewsHeader.FirstOrDefault(n => n.NewsId == 1);
        }

        public List<NewsHeader> GetNewsHeaderList()
        {
            return NewsHeader.ToList();
        }

        public List<NewsHeader> GetNewsHeaderListIncludeContent()
        {
            
            return NewsHeader.Include(e => e.NewsContent).ToList();
        }

        public List<Player> GetPlayers()
        {
            return Player.OrderBy(p => p.PlayerLevel).ToList();
        }

        public PlayerPosition GetPlayerPosition(string name)
        {
            return PlayerPosition.Where(p => p.Name == name).FirstOrDefault();
        }

        public List<Player> GetPlayersIncludePosition()
        {
            return Player.OrderBy(p => p.PlayerLevel).Include(p => p.PlayerPosition).ToList();
        }

        public List<Player> GetPlayersIncludePosition(int skip, int take)
        {
            return Player.OrderBy(p => p.PlayerLevel).Skip(skip).Include(p => p.PlayerPosition).Take(take).ToList();
        }

        public PlayerPosition GetPositionByName(string position)
        {
            return PlayerPosition.Where(x => x.Name == position).SingleOrDefault();
        }

        public List<Team> GetTeams()
        {
            return Team.ToList();
        }
    }

}