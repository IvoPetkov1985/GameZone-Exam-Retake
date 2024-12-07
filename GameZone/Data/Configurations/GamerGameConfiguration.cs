using GameZone.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameZone.Data.Configurations
{
    public class GamerGameConfiguration : IEntityTypeConfiguration<GamerGame>
    {
        public void Configure(EntityTypeBuilder<GamerGame> builder)
        {
            builder.HasKey(gg => new
            {
                gg.GameId,
                gg.GamerId
            });

            builder.HasOne(gg => gg.Game)
                .WithMany(g => g.GamersGames)
                .HasForeignKey(gg => gg.GameId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(gg => gg.Gamer)
                .WithMany()
                .HasForeignKey(gg => gg.GamerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
