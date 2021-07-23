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
        public DataContext(DbContextOptions options) : base(options) {}
        public DbSet<NewsHeader> NewsHeader { get; set; }
        public DbSet<NewsContent> NewsContent { get; set; }
        public DbSet<PlayerPosition> PlayerPosition { get; set; }
        public DbSet<Player> Player { get; set; }

        public DbSet<Team> Team { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsHeader>(entity =>
            {
                entity.HasKey(e => e.NewsId);
                entity.HasOne(s => s.NewsContent);
            });

            modelBuilder.Entity<NewsContent>(entity =>
            {
                entity.HasKey(e => e.NewsContentId);
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(ap => ap.PlayerID);
                entity.HasOne(p => p.PlayerPosition).WithOne().HasForeignKey<Player>(x => x.PositionID);
            });

            modelBuilder.Entity<PlayerPosition>(entity =>
            {
                entity.HasKey(p => p.PositionID);
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
            return Player.ToList();
        }

        public PlayerPosition GetPlayerPosition(string name)
        {
            return PlayerPosition.Where(p => p.Name == name).FirstOrDefault();
        }

        public List<Player> GetPlayersIncludePosition()
        {
            return Player.Include(p => p.PlayerPosition).ToList();
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