using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using System.Diagnostics;

namespace RunGroupWebApp.Repository
{
	public class ClubRepository : IClubRepository
	{
		private readonly ApplicationDbContext dbContext;
		public ClubRepository(ApplicationDbContext dbContext)
        {
			this.dbContext = dbContext;
		}

        public bool Add(Club club)
		{
			dbContext.Add(club);
			return Save();
		}

		public bool Delete(Club club)
		{
			dbContext.Remove(club);
			return Save();
		}

		public async Task<IEnumerable<Club>> GetAllAsync()
		{
			return await dbContext.Clubs.ToListAsync();
		}

		public async Task<Club> GetByIdAsync(int id)
		{
			return await dbContext.Clubs.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<Club> GetByIdAsyncNoTracking(int id)
		{
			return await dbContext.Clubs.Include(x => x.Address).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<IEnumerable<Club>> GetClubsByCityAsync(string city)
		{
			return await dbContext.Clubs.Where(x => x.Address.City.Contains(city) == true).ToListAsync();
		}

		public bool Save()
		{
			var saved = dbContext.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool Update(Club club)
		{
			dbContext.Update(club);
			return Save();
		}
	}
}
