using GameZone.Data.Configuration;
using GameZone.Data.DataModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Data
{
    public class GameZoneDbContext : IdentityDbContext
    {
        public GameZoneDbContext(DbContextOptions<GameZoneDbContext> options)
            : base(options)
        {
        }

        public DbSet<Game> Games { get; set; } = null!;

        public DbSet<Genre> Genres { get; set; } = null!;

        public DbSet<GamerGame> GamersGames { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new GenreConfiguration());
            builder.ApplyConfiguration(new GameConfiguration());
            builder.ApplyConfiguration(new GamerGameConfuguration());

            base.OnModelCreating(builder);
        }
    }
}
