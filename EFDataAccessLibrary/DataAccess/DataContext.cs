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
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerPosition> PlayerPosition { get; set; }

        public DbSet<Team> Teams { get; set; }

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
                entity.HasKey(i => i.PlayerID);
                entity.HasOne(p => p.PlayerPosition);
            });

            modelBuilder.Entity<PlayerPosition>(entity =>
            {
                entity.HasKey(i => i.PositionID);
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
            return Players.ToList();
        }

        public PlayerPosition GetPlayerPosition(string name)
        {
            return PlayerPosition.Where(p => p.Name == name).FirstOrDefault();
        }

        public List<Player> GetPlayersIncludePosition()
        {
            return Players.Include(p => p.PlayerPosition).ToList();
        }

    }

}