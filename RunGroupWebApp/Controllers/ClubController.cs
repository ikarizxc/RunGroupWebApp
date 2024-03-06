using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Data;

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
    }
}
