using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository clubRepository;
        public ClubController(IClubRepository clubRepository)
        {
            this.clubRepository = clubRepository;
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
    }
}
