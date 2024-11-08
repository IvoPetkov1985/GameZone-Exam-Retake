using GameZone.Models;

namespace GameZone.Contracts
{
    public interface IGameService
    {
        Task AddGameToMyZoneAsync(string userId, int id);

        Task<DeleteGameViewModel> CreateDeleteModelAsync(string userId, int id);

        Task<DetailsGameViewModel> CreateDetailsModelAsync(int id);

        Task<GameFormModel> CreateEditModelAsync(int id);

        Task<IEnumerable<AllGameViewModel>> CreateMyZoneModelAsync(string userId);

        Task CreateNewGameAsync(string userId, GameFormModel model);

        Task DeleteGameAsync(int id);

        Task EditGameAsync(GameFormModel model, int id);

        Task<IEnumerable<AllGameViewModel>> GetAllGamesAsync();

        Task<IEnumerable<GenreViewModel>> GetAllGenresAsync();

        Task<bool> IsGameExistingAsync(int id);

        Task<bool> IsUserAuthorisedAsync(string userId, int id);

        Task StrikeOutGameAsync(string userId, int id);
    }
}
