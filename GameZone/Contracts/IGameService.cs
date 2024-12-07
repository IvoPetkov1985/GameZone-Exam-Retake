using GameZone.Models;

namespace GameZone.Contracts
{
    public interface IGameService
    {
        Task AddGameToMyZoneAsync(string userId, int id);

        Task AddNewGameAsync(GameFormModel model, string userId);

        Task<GameDeleteViewModel> CreateDeleteModelAsync(int id);

        Task<GameDetailsViewModel> CreateDetailsModelAsync(int id);

        Task<GameFormModel> CreateEditModelAsync(int id);

        Task DeleteGameAsync(int id);

        Task EditGameAsync(GameFormModel model, int id);

        Task<IEnumerable<GameAllViewModel>> GetAllGamesAsync();

        Task<IEnumerable<GenreViewModel>> GetAllGenresAsync();

        Task<IEnumerable<GameAllViewModel>> GetMyZoneAsync(string userId);

        Task<bool> IsGameExistingAsync(int id);

        Task<bool> IsGameInMyZoneAsync(string userId, int id);

        Task<bool> IsUserAuthorizedAsync(string userId, int id);

        Task StrikeOutGameAsync(string userId, int id);
    }
}
