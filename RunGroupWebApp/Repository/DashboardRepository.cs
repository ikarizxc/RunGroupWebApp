using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public DashboardRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Club>> GetAllUserClubs()
        {
            var curUser = httpContextAccessor.HttpContext?.User.GetUserId();
            var userClubs = dbContext.Clubs.Where(x => x.AppUser.Id == curUser);
            return userClubs.ToList();
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var curUser = httpContextAccessor.HttpContext?.User.GetUserId();
            var userRaces = dbContext.Races.Where(x => x.AppUser.Id == curUser);
            return userRaces.ToList();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await dbContext.Users.FindAsync(id);
        }

		public async Task<AppUser> GetUserByIdNoTracking(string id)
		{
            return await dbContext.Users.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
		}

        public bool Update(AppUser user)
        {
            dbContext.Update(user);
            return Save();
        }

        public bool Save()
        {
            var saved = dbContext.SaveChanges();
            return saved > 0 ? true : false;
        }
	}
}
