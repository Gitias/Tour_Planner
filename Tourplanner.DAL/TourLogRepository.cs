namespace Tourplanner.DAL
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Tourplanner.Shared;

    public class TourLogRepository : ITourLogRepository
    {
        public TourLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddTourLogAsync(TourLog tourLog)
        {
            _context.TourLogs.Add(tourLog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTourLogAsync(int id)
        {
            var tourLog = await _context.TourLogs.FindAsync(id);

            if (tourLog != null)
            {
                _context.TourLogs.Remove(tourLog);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TourLog>> GetAllTourLogsFromTourAsync(int tourId)
        {
            var tourLogs = await _context.TourLogs.ToListAsync();

            return tourLogs.FindAll(tourLog => tourLog.TourId == tourId);
        }

        public async Task UpdateTourLogAsync(TourLog tourLog)
        {
            _context.TourLogs.Update(tourLog);
            await _context.SaveChangesAsync();
        }

        private readonly AppDbContext _context;
    }
}
