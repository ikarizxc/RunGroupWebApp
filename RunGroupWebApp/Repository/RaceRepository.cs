using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repository
{
	public class RaceRepository : IRaceRepository
	{
		private readonly ApplicationDbContext dbContext;
		public RaceRepository(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public bool Add(Race race)
		{
			dbContext.Add(race);
			return Save();
		}

		public bool Delete(Race race)
		{
			dbContext.Remove(race);
			return Save();
		}

		public async Task<IEnumerable<Race>> GetAllAsync()
		{
			return await dbContext.Races.ToListAsync();
		}

		public async Task<Race> GetByIdAsync(int id)
		{
			return await dbContext.Races.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<IEnumerable<Race>> GetRacesByCityAsync(string city)
		{
			return await dbContext.Races.Where(x => x.Address.City.Contains(city) == true).ToListAsync();
		}

		public bool Save()
		{
			var saved = dbContext.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool Update(Race race)
		{
			dbContext.Update(race);
			return Save();
		}
	}
}
