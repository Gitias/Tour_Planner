namespace Tourplanner.BL
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Tourplanner.DAL;
    using Tourplanner.Shared;

    public class TourLogService : ITourLogService
    {
        public TourLogService(ITourLogRepository tourLogRepository)
        {
            _tourLogRepository = tourLogRepository;
        }

        public async Task AddTourLogAsync(TourLog tourLog)
        {
            await _tourLogRepository.AddTourLogAsync(tourLog);
        }

        public async Task DeleteTourLogAsync(int id)
        {
            await _tourLogRepository.DeleteTourLogAsync(id);
        }

        public async Task<IEnumerable<TourLog>> GetAllTourLogsFromTourAsync(int tourId)
        {
            return await _tourLogRepository.GetAllTourLogsFromTourAsync(tourId);
        }

        public async Task UpdateTourLogAsync(TourLog tourLog)
        {
            await _tourLogRepository.UpdateTourLogAsync(tourLog);
        }

        private readonly ITourLogRepository _tourLogRepository;
    }
}
