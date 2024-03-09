using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repository;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository clubRepository;
        private readonly IPhotoService photoService;
        public ClubController(IClubRepository clubRepository, IPhotoService photoService)
        {
            this.clubRepository = clubRepository;
            this.photoService = photoService;
        }

        public async Task<IActionResult> Index()
        {
            var clubs = await clubRepository.GetAllAsync();

            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
			Club club = await clubRepository.GetByIdAsync(id);

            return View(club);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
				ModelState.AddModelError("", "Photo upload error");
				return View(clubVM);
			}

			var result = await photoService.AddPhotoAsync(clubVM.Image);

			var club = new Club
			{
				Title = clubVM.Title,
				Description = clubVM.Description,
				Image = result.Url.ToString(),
				Address = new Address
				{
					Street = clubVM.Address.Street,
					City = clubVM.Address.City,
					State = clubVM.Address.State,
				}
			};
			clubRepository.Add(club);

			return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var club = await clubRepository.GetByIdAsync(id);
            if (club == null)
            {
                return View("Error");
            }

            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                URL = club.Image,
                ClubCategory = club.ClubCategory,
            };

            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
				ModelState.AddModelError("", "Failed to edit club");
				return View("Edit", clubVM);
			}

            var userClub = await clubRepository.GetByIdAsyncNoTracking(id);
            if (userClub == null)
            {
                return View(clubVM);
            }

            try
            {
                await photoService.DeletePhotoAsync(userClub.Image);
            }
            catch
            {
                ModelState.AddModelError("", "Couldn't delete photo");
                return View(clubVM);
            }

            var photoResult = await photoService.AddPhotoAsync(clubVM.Image);

            var club = new Club
			{
                Id = id,
                Title = clubVM.Title,
                Description = clubVM.Description,
                AddressId = clubVM.AddressId,
                Address = clubVM.Address,
                ClubCategory = clubVM.ClubCategory,
                Image = photoResult.Url.ToString(),
            };

            clubRepository.Update(club);

            return RedirectToAction("Index");
        }
    }
}
