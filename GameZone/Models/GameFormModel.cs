using System.ComponentModel.DataAnnotations;
using static GameZone.Data.Common.DataConstants;

namespace GameZone.Models
{
    public class GameFormModel
    {
        [Required]
        [StringLength(GameTitleMaxLength, MinimumLength = GameTitleMinLength, ErrorMessage = GameInputErrorMessage)]
        public string Title { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        [Required]
        [StringLength(GameDescriptionMaxLength, MinimumLength = GameDescriptionMinLength, ErrorMessage = GameInputErrorMessage)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [RegularExpression(DateRegex, ErrorMessage = DateErrorFormatMessage)]
        public string ReleasedOn { get; set; } = string.Empty;

        [Required]
        public int GenreId { get; set; }

        public IEnumerable<GenreViewModel> Genres { get; set; } = new List<GenreViewModel>();
    }
}
