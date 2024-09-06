namespace Tourplanner.BL
{
    using Tourplanner.Shared;

    public interface ITourLogService
    {
        Task AddTourLogAsync(TourLog tourLog);
        Task UpdateTourLogAsync(TourLog tourLog);
        Task DeleteTourLogAsync(int id);
        Task<IEnumerable<TourLog>> GetAllTourLogsFromTourAsync(int tourId);
    }
}
