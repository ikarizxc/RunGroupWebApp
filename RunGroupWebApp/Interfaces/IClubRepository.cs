using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interfaces
{
	public interface IClubRepository
	{
		bool Add(Club club);
		Task<IEnumerable<Club>> GetAllAsync();
		Task<Club> GetByIdAsync(int id);
		Task<Club> GetByIdAsyncNoTracking(int id);
		Task<IEnumerable<Club>> GetClubsByCityAsync(string city);
		bool Update(Club club);
		bool Delete(Club club);
		bool Save();
	}
}
