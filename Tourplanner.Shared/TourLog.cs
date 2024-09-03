namespace Tourplanner.Shared
{
    public class TourLog
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Comment { get; set; }
        public string? Difficulty { get; set; }
        public double Distance { get; set; }
        public TimeSpan TotalTime { get; set; }
        public int Rating { get; set; }
    }
}
