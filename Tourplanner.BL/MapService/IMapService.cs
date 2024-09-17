namespace Tourplanner.BL.MapService
{
    using Tourplanner.Shared;

    public interface IMapService
    {
        Task<TourMapInfo?> GenerateRouteMapAsync(Tour tour, int zoom);
    }
}
