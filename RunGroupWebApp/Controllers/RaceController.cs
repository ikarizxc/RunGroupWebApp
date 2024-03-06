using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public RaceController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var races = dbContext.Races.ToList();

            return View(races);
        }

		public IActionResult Detail(int id)
		{
			Race race = dbContext.Races.Include(a => a.Address).FirstOrDefault(x => x.Id == id);

			return View(race);
		}
	}
}
