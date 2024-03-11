using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
	public class DashboardController : Controller
	{
		private readonly IDashboardRepository dashboardRepository;
        public DashboardController(IDashboardRepository dashboardRepository)
        {
			this.dashboardRepository = dashboardRepository;
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
	}
}
