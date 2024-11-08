using GameZone.Contracts;
using GameZone.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static GameZone.Data.Common.DataConstants;

namespace GameZone.Controllers
{
    public class GameController : BaseController
    {
        private readonly IGameService service;

        public GameController(IGameService gameService)
        {
            service = gameService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var model = await service.GetAllGamesAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var genres = await service.GetAllGenresAsync();

            var model = new GameFormModel()
            {
                Genres = genres
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(GameFormModel model)
        {
            DateTime date;

            if (!DateTime.TryParseExact(model.ReleasedOn, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                ModelState.AddModelError(nameof(model.ReleasedOn), DateInvalid);
            }

            var genres = await service.GetAllGenresAsync();

            if (!genres.Any(g => g.Id == model.GenreId))
            {
                ModelState.AddModelError(nameof(model.GenreId), MissingGenre);
            }

            if (!ModelState.IsValid)
            {
                model.Genres = genres;

                return View(model);
            }

            string userId = GetUserId();

            await service.CreateNewGameAsync(userId, model);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (await IsExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (await IsAuthorizedAsync(userId, id) == false)
            {
                return Unauthorized();
            }

            var modelToEdit = await service.CreateEditModelAsync(id);
            modelToEdit.Genres = await service.GetAllGenresAsync();

            return View(modelToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GameFormModel model, int id)
        {
            if (await IsExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (await IsAuthorizedAsync(userId, id) == false)
            {
                return Unauthorized();
            }

            DateTime date;

            if (!DateTime.TryParseExact(model.ReleasedOn, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                ModelState.AddModelError(nameof(model.ReleasedOn), DateInvalid);
            }

            var genres = await service.GetAllGenresAsync();

            if (!genres.Any(g => g.Id == model.GenreId))
            {
                ModelState.AddModelError(nameof(model.GenreId), MissingGenre);
            }

            if (!ModelState.IsValid)
            {
                model.Genres = genres;

                return View(model);
            }

            await service.EditGameAsync(model, id);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> AddToMyZone(int id)
        {
            if (await IsExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            await service.AddGameToMyZoneAsync(userId, id);

            return RedirectToAction(nameof(MyZone));
        }

        [HttpGet]
        public async Task<IActionResult> MyZone()
        {
            string userId = GetUserId();

            var model = await service.CreateMyZoneModelAsync(userId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> StrikeOut(int id)
        {
            if (await IsExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            await service.StrikeOutGameAsync(userId, id);

            return RedirectToAction(nameof(MyZone));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (await IsExistingAsync(id) == false)
            {
                return BadRequest();
            }

            var model = await service.CreateDetailsModelAsync(id);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (await IsExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (await IsAuthorizedAsync(userId, id) == false)
            {
                return Unauthorized();
            }

            var deleteModel = await service.CreateDeleteModelAsync(userId, id);

            return View(deleteModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(DeleteGameViewModel model)
        {
            if (await IsExistingAsync(model.Id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (await IsAuthorizedAsync(userId, model.Id) == false)
            {
                return Unauthorized();
            }

            await service.DeleteGameAsync(model.Id);

            return RedirectToAction(nameof(All));
        }

        private async Task<bool> IsExistingAsync(int id)
        {
            return await service.IsGameExistingAsync(id);
        }

        private async Task<bool> IsAuthorizedAsync(string userId, int id)
        {
            return await service.IsUserAuthorisedAsync(userId, id);
        }
    }
}
