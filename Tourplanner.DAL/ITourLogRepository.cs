namespace Tourplanner.DAL
{
    using Tourplanner.Shared;

    public interface ITourLogRepository
    {
        Task AddTourLogAsync(TourLog tourLog);
        Task UpdateTourLogAsync(TourLog tourLog);
        Task DeleteTourLogAsync(int id);
        Task<IEnumerable<TourLog>> GetAllTourLogsFromTourAsync(int tourId);
    }
}
