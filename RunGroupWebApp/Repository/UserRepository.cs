using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext dbContext;
		public UserRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public bool Add(AppUser user)
		{
			throw new NotImplementedException();
		}

		public bool Delete(AppUser user)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<AppUser>> GetAllUsers()
		{
			return await dbContext.Users.ToListAsync();
		}

		public async Task<AppUser> GetUserById(string id)
		{
			return await dbContext.Users.FindAsync(id);
		}

		public bool Save()
		{
			var saved = dbContext.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool Update(AppUser user)
		{
			dbContext.Update(user);
			return Save();
		}
	}
}
