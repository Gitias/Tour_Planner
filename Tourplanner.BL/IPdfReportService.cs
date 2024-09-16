namespace Tourplanner.BL
{
    using Tourplanner.Shared;

    public interface IPdfReportService
    {
        Task GenerateTourReport(Tour tour);
        Task GenerateSummaryReport(IEnumerable<Tour> tours);
    }
}
