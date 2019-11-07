using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data
{
    public class FootballBettingContext: DbContext
    {
        public FootballBettingContext()
        {

        }

        public FootballBettingContext( DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Bet> Bets { get; set; }

        public DbSet<User> Users { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureTeamEntity(modelBuilder);
            ConfigureGameEntity(modelBuilder);
            ConfigurePlayerStatisticEntity(modelBuilder);
        }

        private void ConfigurePlayerStatisticEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerStatistic>()
                .HasKey(k => new { k.GameId, k.PlayerId });
        }

        private void ConfigureGameEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                   .HasOne(g => g.HomeTeam)
                   .WithMany(t => t.HomeGames)
                   .HasForeignKey(t => t.HomeTeamId);

            modelBuilder.Entity<Game>()
                  .HasOne(g => g.AwayTeam)
                  .WithMany(t => t.AwayGames)
                  .HasForeignKey(t => t.AwayTeamId);
        }

        private void ConfigureTeamEntity(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Team>()
                .HasOne(t => t.PrimaryKitColor)
                .WithMany(c => c.PrimaryKitTeams)
                .HasForeignKey(c => c.PrimaryKitColorId);

            modelBuilder.Entity<Team>()
               .HasOne(t => t.SecondaryKitColor)
               .WithMany(c => c.SecondaryKitTeams)
               .HasForeignKey(c => c.SecondaryKitColorId);
        }
    }
}
