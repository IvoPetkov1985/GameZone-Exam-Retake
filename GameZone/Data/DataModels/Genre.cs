using System.ComponentModel.DataAnnotations;
using static GameZone.Data.Common.DataConstants;

namespace GameZone.Data.DataModels
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GenreNameMaxLength)]
        public string Name { get; set; } = string.Empty;

        public IEnumerable<Game> Games { get; set; } = new List<Game>();
    }
}
