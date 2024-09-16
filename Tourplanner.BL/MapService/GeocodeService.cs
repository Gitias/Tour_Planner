namespace Tourplanner.BL.MapService;

using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using Tourplanner.Shared;

public class GeocodeService
{
    public GeocodeService(IConfiguration configuration)
    {
        _apiKey = configuration["MapService:ApiKey"]
            ?? throw new ArgumentNullException(nameof(configuration));

        _httpClient = new HttpClient();
    }

    public async Task<GeocodeResult?> GetGeocodeResultAsync(string address)
    {
        try
        {
            string url = $"https://api.openrouteservice.org/geocode/search?api_key={_apiKey}&text={Uri.EscapeDataString(address)}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<GeocodeResult>(json, options);
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
