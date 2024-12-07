using GameZone.Contracts;
using GameZone.Data;
using GameZone.Data.DataModels;
using GameZone.Models;
using Microsoft.EntityFrameworkCore;
using static GameZone.Data.Common.DataConstants;

namespace GameZone.Services
{
    public class GameService : IGameService
    {
        private readonly GameZoneDbContext context;

        public GameService(GameZoneDbContext _context)
        {
            context = _context;
        }

        public async Task AddGameToMyZoneAsync(string userId, int id)
        {
            GamerGame entry = new()
            {
                GameId = id,
                GamerId = userId
            };

            await context.GamersGames.AddAsync(entry);

            await context.SaveChangesAsync();
        }

        public async Task AddNewGameAsync(GameFormModel model, string userId)
        {
            Game game = new()
            {
                Title = model.Title,
                ImageUrl = model.ImageUrl,
                Description = model.Description,
                ReleasedOn = DateTime.Parse(model.ReleasedOn),
                GenreId = model.GenreId,
                PublisherId = userId
            };

            await context.Games.AddAsync(game);

            await context.SaveChangesAsync();
        }

        public async Task<GameDeleteViewModel> CreateDeleteModelAsync(int id)
        {
            GameDeleteViewModel deleteModel = await context.Games
                .AsNoTracking()
                .Where(g => g.Id == id)
                .Select(g => new GameDeleteViewModel()
                {
                    Id = g.Id,
                    Title = g.Title,
                    Publisher = g.Publisher.UserName
                })
                .SingleAsync();

            return deleteModel;
        }

        public async Task<GameDetailsViewModel> CreateDetailsModelAsync(int id)
        {
            GameDetailsViewModel detailsModel = await context.Games
                .AsNoTracking()
                .Where(g => g.Id == id)
                .Select(g => new GameDetailsViewModel()
                {
                    Id = g.Id,
                    Title = g.Title,
                    Description = g.Description,
                    ImageUrl = g.ImageUrl,
                    ReleasedOn = g.ReleasedOn.ToString(DateFormat),
                    Genre = g.Genre.Name,
                    Publisher = g.Publisher.UserName
                })
                .SingleAsync();

            return detailsModel;
        }

        public async Task<GameFormModel> CreateEditModelAsync(int id)
        {
            GameFormModel editModel = await context.Games
                .AsNoTracking()
                .Where(g => g.Id == id)
                .Select(g => new GameFormModel()
                {
                    Title = g.Title,
                    ImageUrl = g.ImageUrl,
                    Description = g.Description,
                    ReleasedOn = g.ReleasedOn.ToString(DateFormat),
                    GenreId = g.GenreId
                })
                .SingleAsync();

            return editModel;
        }

        public async Task DeleteGameAsync(int id)
        {
            Game gameToDelete = await context.Games
                .SingleAsync(g => g.Id == id);

            context.Games.Remove(gameToDelete);

            await context.SaveChangesAsync();
        }

        public async Task EditGameAsync(GameFormModel model, int id)
        {
            Game game = await context.Games
                .SingleAsync(g => g.Id == id);

            game.Title = model.Title;
            game.ImageUrl = model.ImageUrl;
            game.Description = model.Description;
            game.ReleasedOn = DateTime.Parse(model.ReleasedOn);
            game.GenreId = model.GenreId;

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<GameAllViewModel>> GetAllGamesAsync()
        {
            IEnumerable<GameAllViewModel> allGames = await context.Games
                .AsNoTracking()
                .Select(g => new GameAllViewModel()
                {
                    Id = g.Id,
                    Title = g.Title,
                    ImageUrl = g.ImageUrl,
                    Genre = g.Genre.Name,
                    Publisher = g.Publisher.UserName,
                    ReleasedOn = g.ReleasedOn.ToString(DateFormat)
                })
                .ToListAsync();

            return allGames;
        }

        public async Task<IEnumerable<GenreViewModel>> GetAllGenresAsync()
        {
            IEnumerable<GenreViewModel> allGenres = await context.Genres
                .AsNoTracking()
                .Select(g => new GenreViewModel()
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToListAsync();

            return allGenres;
        }

        public async Task<IEnumerable<GameAllViewModel>> GetMyZoneAsync(string userId)
        {
            IEnumerable<GameAllViewModel> myZoneGames = await context.GamersGames
                .AsNoTracking()
                .Where(gg => gg.GamerId == userId)
                .Select(gg => new GameAllViewModel()
                {
                    Id = gg.Game.Id,
                    Title = gg.Game.Title,
                    ImageUrl = gg.Game.ImageUrl,
                    ReleasedOn = gg.Game.ReleasedOn.ToString(DateFormat),
                    Genre = gg.Game.Genre.Name,
                    Publisher = gg.Game.Publisher.UserName
                })
                .ToListAsync();

            return myZoneGames;
        }

        public async Task<bool> IsGameExistingAsync(int id)
        {
            Game? game = await context.Games
                .AsNoTracking()
                .SingleOrDefaultAsync(g => g.Id == id);

            return game != null;
        }

        public async Task<bool> IsGameInMyZoneAsync(string userId, int id)
        {
            GamerGame entry = new()
            {
                GameId = id,
                GamerId = userId
            };

            return await context.GamersGames.ContainsAsync(entry);
        }

        public async Task<bool> IsUserAuthorizedAsync(string userId, int id)
        {
            Game game = await context.Games
                .AsNoTracking()
                .SingleAsync(g => g.Id == id);

            return game.PublisherId == userId;
        }

        public async Task StrikeOutGameAsync(string userId, int id)
        {
            GamerGame entry = new()
            {
                GameId = id,
                GamerId = userId
            };

            if (await context.GamersGames.ContainsAsync(entry))
            {
                context.GamersGames.Remove(entry);

                await context.SaveChangesAsync();
            }
        }
    }
}
