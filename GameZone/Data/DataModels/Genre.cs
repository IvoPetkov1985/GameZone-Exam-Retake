using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static GameZone.Data.Common.DataConstants;

namespace GameZone.Data.DataModels
{
    [Comment("Game genre")]
    public class Genre
    {
        [Key]
        [Comment("Genre identifier")]
        public int Id { get; set; }

        [Required]
        [MaxLength(GenreNameMaxLength)]
        [Comment("Genre name")]
        public string Name { get; set; } = string.Empty;

        public IEnumerable<Game> Games { get; set; } = new List<Game>();
    }
}
