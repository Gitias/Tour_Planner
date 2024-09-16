namespace Tourplanner.BL
{
    using Tourplanner.Shared;

    public interface IImportService
    {
        Task<Tour?> ImportTourFromJsonAsync();
    }
}
