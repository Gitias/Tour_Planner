namespace Tourplanner.Shared
{
    using System.Text.Json.Serialization;

    public class GeocodeResult
    {
        [JsonPropertyName("features")]
        public List<Feature> Features { get; set; }
    }

    public class Feature
    {
        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; }
    }

    public class Geometry
    {
        [JsonPropertyName("coordinates")]
        public List<double> Coordinates { get; set; }
    }
}
