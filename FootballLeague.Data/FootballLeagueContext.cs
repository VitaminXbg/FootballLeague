using FootballLeague.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace FootballLeague.Data
{
    public class FootballLeagueContext : DbContext
    { 
        public DbSet<Team> Teams { get; set; }
        public DbSet<Match> Matches { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Data Source = localhost;Initial Catalog=FootballLeague;Trusted_Connection=True";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
