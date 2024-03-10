using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Data;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ApplicationDbContext dbContext;
        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, ApplicationDbContext dbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.dbContext = dbContext;
        }

        public IActionResult Login()
        {
            var responce = new LoginViewModel();
            return View(responce);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            var user = await userManager.FindByEmailAsync(loginVM.EmailAddress);

            if (user != null)
            {
                // user is found
                var passwordCheck = await userManager.CheckPasswordAsync(user, loginVM.Password);

                //password is correct
                if (passwordCheck)
                {
                    var result = await signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Race");
                    }
                }
                //password is incorrect
                else
                {
                    TempData["Error"] = "Wrong credentials. Please, try again.";
                    return View(loginVM);
                }
            }

            // user isn't found
            TempData["Error"] = "Wrong credentials. Please, try again.";
            return View(loginVM);
        }

        public IActionResult Register()
        {
            var responce = new RegisterViewModel();
            return View(responce);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            var user = await userManager.FindByEmailAsync(registerVM.EmailAddress);

            if (user != null)
            {
                TempData["Error"] = "This email address is already in use.";
                return View(registerVM);
            }

            var newUser = new AppUser()
            {
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress,
            };

            var newUserResponce = await userManager.CreateAsync(newUser, registerVM.Password);
            if (newUserResponce.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, UserRoles.User); 
                return RedirectToAction("Index", "Race");
            }

            foreach (var error in newUserResponce.Errors)
            {
                TempData["Error"] += $"{error.Description}\n";
            }

            return View(registerVM);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Race");
        }
    }
}
