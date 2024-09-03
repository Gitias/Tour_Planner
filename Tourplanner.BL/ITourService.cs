using Tourplanner.Shared;

namespace Tourplanner.BL
{
    public interface ITourService
    {
        Task AddTourAsync(Tour tour);
        Task UpdateTourAsync(Tour tour);
        Task DeleteTourAsync(int id);
        Task<IEnumerable<Tour>> GetAllToursAsync();
    }
}
