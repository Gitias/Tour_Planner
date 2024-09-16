using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.BL;
using Tourplanner.Shared;

namespace Tourplanner_.Features.Search
{
    public class SearchViewModel : BaseViewModel
    {
        public ObservableCollection<Tour> SearchResults { get; set; } = new ObservableCollection<Tour>();

        public SearchViewModel(TourAttributeCalculator tourAttributeCalculator, ITourLogService tourLogService)
        {
            _tourAttributeCalculator = tourAttributeCalculator;
            _tourLogService = tourLogService;
        }

        public async Task Search(string searchTerm, ObservableCollection<Tour>? tours)
        {
            SearchResults.Clear();

            if (tours == null)
            {
                return;
            }

            foreach (var tour in tours)
            {
                string popularity = _tourAttributeCalculator.CalculatePopularity(tour);

                string childFriendliness = _tourAttributeCalculator.CalculateChildFriendliness(tour);

                bool matches = tour.Name?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true
                    || tour.Description?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true
                    || tour.From?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true
                    || tour.To?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true
                    || tour.TransportType?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true
                    || popularity.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                    || childFriendliness.Contains(searchTerm, StringComparison.OrdinalIgnoreCase);

                if (!matches)
                {
                    var logs = await _tourLogService.GetAllTourLogsFromTourAsync(tour.Id);

                    if (logs != null)
                    {
                        foreach (var log in logs)
                        {
                            matches = log.Comment?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true
                                || log.Difficulty?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true
                                || log.Distance.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                                || log.TotalTime.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                                || log.Rating.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase);

                            if (matches)
                            {
                                break;
                            }
                        }
                    }
                }

                if (matches)
                {
                    SearchResults.Add(tour);
                }
            }
        }

        private readonly ITourLogService _tourLogService;
        private TourAttributeCalculator _tourAttributeCalculator;
    }
}
