using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Repository;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
	public class UserController : Controller
	{
		private readonly IUserRepository userRepository;

		public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<IActionResult> Index()
		{
			var users = await userRepository.GetAllUsers();
			List<IndexUserViewModel> result = new List<IndexUserViewModel>();

			foreach (var user in users)
			{
				var userVM = new IndexUserViewModel()
				{
					Id = user.Id,
					UserName = user.UserName,
					Pace = user.Pace,
					Kilometers = user.Kilometers,
					ProfileImageUrl = user.ProfileImageUrl,
					State = user.State,
					City = user.City,
				};
				result.Add(userVM);
			}

			return View(result);
		}

		public async Task<IActionResult> Detail(string id)
		{
			var user = await userRepository.GetUserById(id);
			var userData = new DetailUserViewModel()
			{
				Id = user.Id,
				UserName = user.UserName,
				Pace = user.Pace,
				Kilometers = user.Kilometers,
				ProfileImageUrl = user.ProfileImageUrl,
				State = user.State,
				City = user.City,
			};

			return View(userData);
		}
	}
}
