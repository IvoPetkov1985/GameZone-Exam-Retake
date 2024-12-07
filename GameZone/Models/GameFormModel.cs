using System.ComponentModel.DataAnnotations;
using static GameZone.Data.Common.DataConstants;

namespace GameZone.Models
{
    public class GameFormModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = string.Empty;

        [StringLength(ImageUrlMaxLength)]
        public string? ImageUrl { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [RegularExpression(DateRegex, ErrorMessage = DateFormatErrorMessage)]
        public string ReleasedOn { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int GenreId { get; set; }

        public IEnumerable<GenreViewModel> Genres { get; set; } = new List<GenreViewModel>();
    }
}
