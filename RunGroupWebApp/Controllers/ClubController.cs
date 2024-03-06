using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public ClubController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var clubs = dbContext.Clubs.ToList();

            return View(clubs);
        }

        public IActionResult Detail(int id)
        {
            Club club = dbContext.Clubs.Include(a => a.Address).FirstOrDefault(x => x.Id == id);

            return View(club);
        }
    }
}
