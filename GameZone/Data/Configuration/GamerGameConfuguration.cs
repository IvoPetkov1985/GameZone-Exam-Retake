using GameZone.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameZone.Data.Configuration
{
    public class GamerGameConfuguration : IEntityTypeConfiguration<GamerGame>
    {
        public void Configure(EntityTypeBuilder<GamerGame> builder)
        {
            builder.HasKey(gg => new
            {
                gg.GameId,
                gg.GamerId
            });

            builder.HasOne(g => g.Gamer)
                .WithMany()
                .HasForeignKey(g => g.GamerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
