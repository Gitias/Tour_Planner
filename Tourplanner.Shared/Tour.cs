namespace Tourplanner.Shared
{
    public class Tour
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public string? TransportType { get; set; }
        public string? ChildFriendliness { get; set; }
        public string? Popularity { get; set; }
        public virtual ICollection<TourLog> TourLogs { get; set; }
    }
}
