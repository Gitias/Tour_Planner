using Tourplanner.DAL;
using Tourplanner.Shared;

namespace Tourplanner.BL
{
    public class TourService : ITourService
    {
        public TourService(ITourRepository tourRepository)
        {
            _tourRepository = tourRepository;
        }
        public async Task AddTourAsync(Tour tour)
        {
            await _tourRepository.AddTourAsync(tour);
        }

        public async Task DeleteTourAsync(int id)
        {
            await _tourRepository.DeleteTourAsync(id);
        }

        public async Task<IEnumerable<Tour>> GetAllToursAsync()
        {
            return await _tourRepository.GetAllToursAsync();
        }

        public async Task UpdateTourAsync(Tour tour)
        {
            await _tourRepository.UpdateTourAsync(tour);
        }

        private readonly ITourRepository _tourRepository;
    }
}
