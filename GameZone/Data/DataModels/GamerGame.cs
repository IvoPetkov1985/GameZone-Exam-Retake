using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameZone.Data.DataModels
{
    [Comment("The mapping table between users and games")]
    public class GamerGame
    {
        [Required]
        [Comment("Name identifier")]
        public int GameId { get; set; }

        [ForeignKey(nameof(GameId))]
        public Game Game { get; set; } = null!;

        [Required]
        [Comment("User identifier")]
        public string GamerId { get; set; } = string.Empty;

        [ForeignKey(nameof(GamerId))]
        public IdentityUser Gamer { get; set; } = null!;
    }
}
