using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository raceRepository;
        public RaceController(IRaceRepository raceRepository)
        {
            this.raceRepository = raceRepository;
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
	}
}
