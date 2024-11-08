namespace GameZone.Data.Common
{
    public static class DataConstants
    {
        // Game constants.
        public const int GameTitleMinLength = 2;
        public const int GameTitleMaxLength = 50;

        public const int GameDescriptionMinLength = 10;
        public const int GameDescriptionMaxLength = 500;

        public const string GameInputErrorMessage = "Field {0} should be between {2} and {1} symbols long";

        public const string DateFormat = "yyyy-MM-dd";
        public const string DateRegex = @"^\d{4}-\d{2}-\d{2}$";
        public const string DateErrorFormatMessage = "Date should be in format: yyyy-MM-dd";
        public const string DateInvalid = "Invalid date";

        // Genre constants.
        public const int GenreNameMinLength = 3;
        public const int GenreNameMaxLength = 25;

        public const string MissingGenre = "This genre does not exist";
    }
}
