namespace GameZone.Data.Common
{
    public static class DataConstants
    {
        // Game constants:
        public const int TitleMinLength = 2;
        public const int TitleMaxLength = 50;

        public const int DescriptionMinLength = 10;
        public const int DescriptionMaxLength = 500;

        public const int ImageUrlMaxLength = 255;
        public const string DateFormat = "yyyy-MM-dd";
        public const string DateRegex = @"^\d{4}-\d{2}-\d{2}$";
        public const string DateFormatErrorMessage = "Invalid date format.";
        public const string DateInvalidErrorMessage = "Invalid date.";

        // Genre constants:
        public const int GenreNameMinLength = 3;
        public const int GenreNameMaxLength = 25;

        public const string GenreInvalidErrorMessage = "This genre does not exist,";

        // Names of actions and controllers:
        public const string AllAction = "All";
        public const string GameContr = "Game";
    }
}
