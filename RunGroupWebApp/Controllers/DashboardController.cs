using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
	public class DashboardController : Controller
	{
		private readonly IDashboardRepository dashboardRepository;
		private readonly IHttpContextAccessor httpContextAccessor;
		private readonly IPhotoService photoService;
        public DashboardController(IDashboardRepository dashboardRepository, 
			IHttpContextAccessor httpContextAccessor, IPhotoService photoService)
        {
			this.dashboardRepository = dashboardRepository;
			this.httpContextAccessor = httpContextAccessor;
			this.photoService = photoService;
        }

        public async Task<IActionResult> Index()
		{
			var userRaces = await dashboardRepository.GetAllUserRaces();
			var userClubs = await dashboardRepository.GetAllUserClubs();

			var dashboardVM = new DashboardViewModel
			{
				Races = userRaces,
				Clubs = userClubs,
			};

			return View(dashboardVM);
		}

		public async Task<IActionResult> EditUserProfile()
		{
			var curUserId = httpContextAccessor.HttpContext.User.GetUserId();
			var user = await dashboardRepository.GetUserById(curUserId);

			if (user == null)
			{
				return View("Error");
			}

			var userVM = new EditUserDashboardViewModel()
			{
				Id = user.Id,
				Pace = user.Pace,
				Kilometers = user.Kilometers,
				ProfileImageUrl = user.ProfileImageUrl,
				City = user.City,
				State = user.State,
			};

			return View(userVM);
		}

		[HttpPost]
		public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel userVM)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("", "Failed to edit profile");
				return View(userVM);
			}

			var user = await dashboardRepository.GetUserByIdNoTracking(userVM.Id);
			
			if (user.ProfileImageUrl != "" && user.ProfileImageUrl != null)
			{
				try
				{
					await photoService.DeletePhotoAsync(user.ProfileImageUrl);
				}
				catch
				{
					ModelState.AddModelError("", "Couldn't delete photo");
					return View(userVM);
				}
			}

			var photoResult = await photoService.AddPhotoAsync(userVM.Image);
			MapUserEdit(user, userVM, photoResult);

			dashboardRepository.Update(user);

			return RedirectToAction("Index", "Dashboard");
		}

		private void MapUserEdit(AppUser user, EditUserDashboardViewModel userVM, ImageUploadResult imageResult)
		{
			user.Id = userVM.Id;
			user.Pace = userVM.Pace;
			user.Kilometers = userVM.Kilometers;
			user.ProfileImageUrl = imageResult.Url.ToString();
			user.City = userVM.City;
			user.State = userVM.State;
		}
	}
}
