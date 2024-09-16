namespace Tourplanner.BL
{
    using Tourplanner.Shared;

    public interface IExportService
    {
        Task<bool> ExportTourToJsonAsync(Tour tour);
    }
}
