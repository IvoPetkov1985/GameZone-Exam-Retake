using GameZone.Contracts;
using GameZone.Data;
using GameZone.Data.DataModels;
using GameZone.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static GameZone.Data.Common.DataConstants;

namespace GameZone.Services
{
    public class GameService : IGameService
    {
        private readonly GameZoneDbContext context;

        public GameService(GameZoneDbContext dbContext)
        {
            context = dbContext;
        }

        public async Task AddGameToMyZoneAsync(string userId, int id)
        {
            if (await context.GamersGames.AnyAsync(g => g.GamerId == userId && g.GameId == id) == false)
            {
                var entry = new GamerGame()
                {
                    GamerId = userId,
                    GameId = id
                };

                await context.GamersGames.AddAsync(entry);
                await context.SaveChangesAsync();
            }
        }

        public async Task<DeleteGameViewModel> CreateDeleteModelAsync(string userId, int id)
        {
            var gameToDelete = await context.Games
                .AsNoTracking()
                .Where(g => g.Id == id)
                .Select(g => new DeleteGameViewModel()
                {
                    Id = g.Id,
                    Title = g.Title,
                    Publisher = g.Publisher.UserName
                })
                .FirstAsync();

            return gameToDelete;
        }

        public async Task<DetailsGameViewModel> CreateDetailsModelAsync(int id)
        {
            var model = await context.Games
                .AsNoTracking()
                .Where(g => g.Id == id)
                .Select(g => new DetailsGameViewModel()
                {
                    Id = g.Id,
                    Title = g.Title,
                    Description = g.Description,
                    ImageUrl = g.ImageUrl,
                    ReleasedOn = g.ReleasedOn.ToString(DateFormat),
                    Genre = g.Genre.Name,
                    Publisher = g.Publisher.UserName
                })
                .FirstAsync();

            return model;
        }

        public async Task<GameFormModel> CreateEditModelAsync(int id)
        {
            var entityToEdit = await context.Games
                .AsNoTracking()
                .Where(g => g.Id == id)
                .Select(g => new GameFormModel()
                {
                    Title = g.Title,
                    Description = g.Description,
                    ImageUrl = g.ImageUrl,
                    GenreId = g.GenreId,
                    ReleasedOn = g.ReleasedOn.ToString(DateFormat)
                })
                .FirstAsync();

            return entityToEdit;
        }

        public async Task<IEnumerable<AllGameViewModel>> CreateMyZoneModelAsync(string userId)
        {
            var myZoneModel = await context.GamersGames
                .AsNoTracking()
                .Where(gg => gg.GamerId == userId)
                .Select(gg => new AllGameViewModel()
                {
                    Id = gg.Game.Id,
                    Title = gg.Game.Title,
                    ImageUrl = gg.Game.ImageUrl,
                    ReleasedOn = gg.Game.ReleasedOn.ToString(DateFormat),
                    Genre = gg.Game.Genre.Name,
                    Publisher = gg.Game.Publisher.UserName
                })
                .ToListAsync();

            return myZoneModel;
        }

        public async Task CreateNewGameAsync(string userId, GameFormModel model)
        {
            var entityToAdd = new Game()
            {
                Title = model.Title,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                ReleasedOn = DateTime.ParseExact(model.ReleasedOn, DateFormat, CultureInfo.InvariantCulture),
                GenreId = model.GenreId,
                PublisherId = userId
            };

            await context.Games.AddAsync(entityToAdd);
            await context.SaveChangesAsync();
        }

        public async Task DeleteGameAsync(int id)
        {
            var gameToDelete = await context.Games
                .FirstAsync(g => g.Id == id);

            context.Games.Remove(gameToDelete);
            await context.SaveChangesAsync();
        }

        public async Task EditGameAsync(GameFormModel model, int id)
        {
            var game = await context.Games
                .FirstAsync(g => g.Id == id);

            game.Title = model.Title;
            game.Description = model.Description;
            game.ImageUrl = model.ImageUrl;
            game.ReleasedOn = DateTime.ParseExact(model.ReleasedOn, DateFormat, CultureInfo.InvariantCulture);
            game.GenreId = model.GenreId;

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AllGameViewModel>> GetAllGamesAsync()
        {
            var allGames = await context.Games
                .AsNoTracking()
                .Select(g => new AllGameViewModel()
                {
                    Id = g.Id,
                    ImageUrl = g.ImageUrl,
                    Title = g.Title,
                    Genre = g.Genre.Name,
                    Publisher = g.Publisher.UserName,
                    ReleasedOn = g.ReleasedOn.ToString(DateFormat)
                })
                .ToListAsync();

            return allGames;
        }

        public async Task<IEnumerable<GenreViewModel>> GetAllGenresAsync()
        {
            var genres = await context.Genres
                .AsNoTracking()
                .Select(g => new GenreViewModel()
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToListAsync();

            return genres;
        }

        public async Task<bool> IsGameExistingAsync(int id)
        {
            var game = await context.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id);

            return game != null;
        }

        public async Task<bool> IsUserAuthorisedAsync(string userId, int id)
        {
            var game = await context.Games
                .AsNoTracking()
                .FirstAsync(g => g.Id == id);

            return game.PublisherId == userId;
        }

        public async Task StrikeOutGameAsync(string userId, int id)
        {
            if (await context.GamersGames.AnyAsync(gg => gg.GamerId == userId && gg.GameId == id))
            {
                var entry = await context.GamersGames.FirstAsync(gg => gg.GamerId == userId && gg.GameId == id);

                context.GamersGames.Remove(entry);
                await context.SaveChangesAsync();
            }
        }
    }
}
