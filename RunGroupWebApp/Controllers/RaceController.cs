using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repository;
using RunGroupWebApp.Services;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository raceRepository;
        private readonly IPhotoService photoService;
        public RaceController(IRaceRepository raceRepository, IPhotoService photoService)
        {
            this.raceRepository = raceRepository;
            this.photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            var races = await raceRepository.GetAllAsync();

			return View(races);
        }

		public async Task<IActionResult> Detail(int id)
		{
			Race race = await raceRepository.GetByIdAsync(id);

			return View(race);
		}

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceVM)
        {
            if (!ModelState.IsValid)
            {
				ModelState.AddModelError("", "Photo upload error");
				return View(raceVM);
			}

			var result = await photoService.AddPhotoAsync(raceVM.Image);

			var race = new Race
			{
				Title = raceVM.Title,
				Description = raceVM.Description,
				Image = result.Url.ToString(),
				Address = new Address
				{
					Street = raceVM.Address.Street,
					City = raceVM.Address.City,
					State = raceVM.Address.State,
				}
			};
			raceRepository.Add(race);

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Edit(int id)
		{
			var race = await raceRepository.GetByIdAsync(id);
			if (race == null)
			{
				return View("Error");
			}

			var raceVM = new EditRaceViewModel
			{
				Title = race.Title,
				Description = race.Description,
				AddressId = race.AddressId,
				Address = race.Address,
				URL = race.Image,
				RaceCategory = race.RaceCategory,
			};

			return View(raceVM);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("", "Failed to edit club");
				return View("Edit", raceVM);
			}

			var userRace = await raceRepository.GetByIdAsyncNoTracking(id);
			if (userRace == null)
			{
				return View(raceVM);
			}

			try
			{
				await photoService.DeletePhotoAsync(userRace.Image);
			}
			catch
			{
				ModelState.AddModelError("", "Couldn't delete photo");
				return View(raceVM);
			}

			var photoResult = await photoService.AddPhotoAsync(raceVM.Image);

			var race = new Race
			{
				Id = id,
				Title = raceVM.Title,
				Description = raceVM.Description,
				AddressId = raceVM.AddressId,
				Address = raceVM.Address,
				RaceCategory = raceVM.RaceCategory,
				Image = photoResult.Url.ToString(),
			};

			raceRepository.Update(race);

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete(int id)
		{
			var race = await raceRepository.GetByIdAsync(id);

			if (race == null)
			{
				return View("Error");
			}

			return View(race);
		}

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteClub(int id)
		{
			var race = await raceRepository.GetByIdAsync(id);

			if (race == null)
			{
				return View("Error");
			}

			raceRepository.Delete(race);
			return RedirectToAction("Index");
		}
	}
}
