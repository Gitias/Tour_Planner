namespace Tourplanner.BL
{
    using Tourplanner.Shared;

    public class TourAttributeCalculator
    {
        public string CalculatePopularity(Tour tour)
        {
            if (tour.TourLogs == null || tour.TourLogs.Count == 0)
            {
                return "Unknown";
            }

            int logsCount = tour.TourLogs.Count;

            switch (logsCount)
            {
                case < 3:
                    return "Not Popular";
                case < 6:
                    return "Popular";
                case >= 6:
                    return "Very Popular";
                default:
                    return "Unknown";
            }
        }

        public string CalculateChildFriendliness(Tour tour)
        {
            if (tour.TourLogs == null || tour.TourLogs.Count == 0)
            {
                return "Unknown";
            }

            //Convert values to int and filter out invalid values
            var difficulties = tour.TourLogs
                .Where(log => int.TryParse(log.Difficulty, out _))
                .Select(log => int.Parse(log.Difficulty))
                .ToList();

            var averageDifficulty = difficulties.Count > 0 ? difficulties.Average() : 0;

            var averageTotalTime = tour.TourLogs.Any() ? tour.TourLogs.Average(log => log.TotalTime.TotalHours) : 0;

            var averageDistance = tour.TourLogs.Any() ? tour.TourLogs.Average(log => log.Distance) : 0;

            double childFriendliness = (averageDifficulty + averageTotalTime + averageDistance) / 3;

            return childFriendliness switch
            {
                < 3 => "Very Child Friendly",
                < 6 => "Child Friendly",
                >= 6 => "Not Child Friendly",
                _ => "Unknown"
            }; 
        }
    }
}
