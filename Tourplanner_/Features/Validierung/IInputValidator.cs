namespace Tourplanner_.Features.Validierung
{
    using Tourplanner.Shared;

    public interface IInputValidator
    {
        bool ValidateTour(Tour tour, out string error);
        bool ValidateTourLog(TourLog tourLog, out string error);
    }
}
