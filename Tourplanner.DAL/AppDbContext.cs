using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Tourplanner.Shared;

namespace Tourplanner.DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<Tour> Tours { get; set; }
        public DbSet<TourLog> TourLogs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
