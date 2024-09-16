namespace Tourplanner.BL.MapService;

using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using Tourplanner.Shared;

public class DirectionsService
{

    public DirectionsService(IConfiguration configuration)
    {
        _apiKey = configuration["MapService:ApiKey"]
            ?? throw new ArgumentNullException(nameof(configuration));

        _httpClient = new HttpClient();
    }

    public async Task<DirectionsResult?> GetDirectionsResultAsync(List<double> startCoordinates, List<double> endCoordinates)
    {
        try
        {
            string start = $"{startCoordinates[0].ToString(CultureInfo.InvariantCulture)},{startCoordinates[1].ToString(CultureInfo.InvariantCulture)}";

            string end = $"{endCoordinates[0].ToString(CultureInfo.InvariantCulture)},{endCoordinates[1].ToString(CultureInfo.InvariantCulture)}";

            string url = $"https://api.openrouteservice.org/v2/directions/driving-car?api_key={_apiKey}&start={start}&end={end}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<DirectionsResult>(json, options);
            }

            return null;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private readonly string _apiKey;
    private HttpClient _httpClient;
}
