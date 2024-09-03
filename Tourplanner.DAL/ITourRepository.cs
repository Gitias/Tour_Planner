using Tourplanner.Shared;

namespace Tourplanner.DAL
{
    public interface ITourRepository
    {
        Task AddTourAsync(Tour tour);
        Task UpdateTourAsync(Tour tour);
        Task DeleteTourAsync(int id);
        Task<IEnumerable<Tour>> GetAllToursAsync();
    }
}
