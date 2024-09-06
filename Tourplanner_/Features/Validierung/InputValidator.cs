namespace Tourplanner_.Features.Validierung
{
    using System.Text.RegularExpressions;
    using Tourplanner.Shared;

    public class InputValidator : IInputValidator
    {
        public bool ValidateTour(Tour tour, out string error)
        {
            var errors = new List<string>();

            if (!ValidateInput(tour.Name, out var nameError))
            {
                errors.Add(nameError);
            }

            if (!ValidateInput(tour.Description, out var descriptionError))
            {
                errors.Add(descriptionError);
            }

            if (!ValidateInput(tour.From, out var fromError))
            {
                errors.Add(fromError);
            }

            if (!ValidateInput(tour.To, out var toError))
            {
                errors.Add(toError);
            }

            error = string.Join(Environment.NewLine, errors);

            return errors.Count == 0;
        }

        public bool ValidateTourLog(TourLog tourLog, out string error)
        {
            var errors = new List<string>();

            if (!ValidateInput(tourLog.Comment, out var errorMessage))
            {
                errors.Add(errorMessage);
            }

            error = string.Join(Environment.NewLine, errors);

            return errors.Count == 0;
        }

        private static bool ValidateString(string? value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        private static bool ContainsUnallowedCharacters(string? value)
        {
            var unsafePattern = @"<[^>]*>|<script[^>]*>.*?</script>";

            return Regex.IsMatch(value, unsafePattern);
        }

        private static bool ValidateInput(string? value, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (!ValidateString(value))
            {
                errorMessage = "The input must not be empty.";
                return false;
            }

            if (ContainsUnallowedCharacters(value))
            {
                errorMessage = "The input contains unallowed characters.";
                return false;
            }

            return true;
        }

    }
}
