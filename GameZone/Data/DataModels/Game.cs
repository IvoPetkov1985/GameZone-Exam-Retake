using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static GameZone.Data.Common.DataConstants;

namespace GameZone.Data.DataModels
{
    [Comment("The game with its properties")]
    public class Game
    {
        [Key]
        [Comment("Game identifier")]
        public int Id { get; set; }

        [Required]
        [MaxLength(TitleMaxLength)]
        [Comment("Game title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        [Comment("Details about the game")]
        public string Description { get; set; } = string.Empty;

        [MaxLength(ImageUrlMaxLength)]
        [Comment("Link to the official image of the game")]
        public string? ImageUrl { get; set; }

        [Required]
        [Comment("Publisher (user) identifier")]
        public string PublisherId { get; set; } = string.Empty;

        [ForeignKey(nameof(PublisherId))]
        public IdentityUser Publisher { get; set; } = null!;

        [Required]
        [Comment("Release date of the game")]
        public DateTime ReleasedOn { get; set; }

        [Required]
        [Comment("Genre identifier")]
        public int GenreId { get; set; }

        [ForeignKey(nameof(GenreId))]
        public Genre Genre { get; set; } = null!;

        public IEnumerable<GamerGame> GamersGames { get; set; } = new List<GamerGame>();
    }
}
