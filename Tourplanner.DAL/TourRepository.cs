using Microsoft.EntityFrameworkCore;
using Tourplanner.Shared;

namespace Tourplanner.DAL
{
    public class TourRepository : ITourRepository
    {
        public TourRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddTourAsync(Tour tour)
        {
            _context.Tours.Add(tour);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTourAsync(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour != null) {
                _context.Tours.Remove(tour);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Tour>> GetAllToursAsync()
        {
            return await _context.Tours.ToListAsync();
        }

        public async Task UpdateTourAsync(Tour tour)
        {
            _context.Tours.Update(tour);
            await _context.SaveChangesAsync();
        }

        private readonly AppDbContext _context;
    }
}
