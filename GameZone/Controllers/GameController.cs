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

        public GameController(IGameService _service)
        {
            service = _service;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<GameAllViewModel> model = await service.GetAllGamesAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            IEnumerable<GenreViewModel> genres = await service.GetAllGenresAsync();

            GameFormModel model = new()
            {
                Genres = genres
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(GameFormModel model)
        {
            IEnumerable<GenreViewModel> genres = await service.GetAllGenresAsync();

            if (genres.Any(g => g.Id == model.GenreId) == false)
            {
                ModelState.AddModelError(nameof(model.GenreId), GenreInvalidErrorMessage);
            }

            if (DateTime.TryParseExact(model.ReleasedOn, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultDate) == false)
            {
                ModelState.AddModelError(nameof(model.ReleasedOn), DateInvalidErrorMessage);
            }

            if (ModelState.IsValid == false)
            {
                model.Genres = genres;

                return View(model);
            }

            string userId = GetUserId();

            await service.AddNewGameAsync(model, userId);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (await service.IsGameExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (await service.IsUserAuthorizedAsync(userId, id) == false)
            {
                return Unauthorized();
            }

            GameFormModel model = await service.CreateEditModelAsync(id);

            IEnumerable<GenreViewModel> genres = await service.GetAllGenresAsync();

            model.Genres = genres;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GameFormModel model, int id)
        {
            if (await service.IsGameExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (await service.IsUserAuthorizedAsync(userId, id) == false)
            {
                return Unauthorized();
            }

            IEnumerable<GenreViewModel> genres = await service.GetAllGenresAsync();

            if (genres.Any(g => g.Id == model.GenreId) == false)
            {
                ModelState.AddModelError(nameof(model.GenreId), GenreInvalidErrorMessage);
            }

            if (DateTime.TryParseExact(model.ReleasedOn, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultDate) == false)
            {
                ModelState.AddModelError(nameof(model.ReleasedOn), DateInvalidErrorMessage);
            }

            if (ModelState.IsValid == false)
            {
                model.Genres = genres;

                return View(model);
            }

            await service.EditGameAsync(model, id);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (await service.IsGameExistingAsync(id) == false)
            {
                return BadRequest();
            }

            GameDetailsViewModel model = await service.CreateDetailsModelAsync(id);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MyZone()
        {
            string userId = GetUserId();

            IEnumerable<GameAllViewModel> model = await service.GetMyZoneAsync(userId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddToMyZone(int id)
        {
            if (await service.IsGameExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (await service.IsGameInMyZoneAsync(userId, id))
            {
                return RedirectToAction(nameof(All));
            }

            await service.AddGameToMyZoneAsync(userId, id);

            return RedirectToAction(nameof(MyZone));
        }

        [HttpGet]
        public async Task<IActionResult> StrikeOut(int id)
        {
            if (await service.IsGameExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            await service.StrikeOutGameAsync(userId, id);

            return RedirectToAction(nameof(MyZone));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (await service.IsGameExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (await service.IsUserAuthorizedAsync(userId, id) == false)
            {
                return Unauthorized();
            }

            GameDeleteViewModel model = await service.CreateDeleteModelAsync(id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(GameDeleteViewModel model, int id)
        {
            if (await service.IsGameExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (await service.IsUserAuthorizedAsync(userId, id) == false)
            {
                return Unauthorized();
            }

            await service.DeleteGameAsync(model.Id);

            return RedirectToAction(nameof(All));
        }
    }
}
