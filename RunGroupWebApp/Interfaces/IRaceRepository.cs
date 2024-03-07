using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interfaces
{
	public interface IRaceRepository
	{
		bool Add(Race race);
		Task<IEnumerable<Race>> GetAllAsync();
		Task<Race> GetByIdAsync(int id);
		Task<IEnumerable<Race>> GetRacesByCityAsync(string city);
		bool Update(Race race);
		bool Delete(Race race);
		bool Save();
	}
}
