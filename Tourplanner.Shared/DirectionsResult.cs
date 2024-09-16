namespace Tourplanner.Shared
{
    using System.Text.Json.Serialization;

    public class DirectionsResult
    {
        [JsonPropertyName("features")]
        public List<RouteFeatures> Features { get; set; }

        [JsonPropertyName("bbox")]
        public List<double> Bbox { get; set; }
    }

    public class RouteFeatures
    {
        [JsonPropertyName("properties")]
        public RouteProperties Properties { get; set; }
    }

    public class RouteProperties
    {
        [JsonPropertyName("segments")]
        public List<Segment> Segements { get; set; }
    }

    public class Segment
    {
        [JsonPropertyName("distance")]
        public double Distance { get; set; }

        [JsonPropertyName("duration")]
        public double Duration { get; set; }
    }
}
