namespace Tourplanner.BL.MapService
{
    using Microsoft.Extensions.Configuration;
    using System.IO;
    using System.Net.Http;

    public class TileService
    {
        public const string TileServerUrl = "https://tile.openstreetmap.org";

        public TileService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            tilePath = configuration["MapService:ImagePath"]
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task DownloadTileAsync(int x, int y, int zoom, string path)
        {
            try
            {
                string url = $"{TileServerUrl}/{zoom}/{x}/{y}.png";

                _httpClient.DefaultRequestHeaders.Add("User-Agent", "Tourplanner");

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    byte[] bytes = await response.Content.ReadAsByteArrayAsync();

                    string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tilePath);

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    string fullPath = Path.Combine(directory, path);

                    await File.WriteAllBytesAsync(fullPath, bytes);
                }
            }
            catch (Exception ex)
            {
                // Log error
            }
        }

        public static (int x, int y) LatLonToTile(double lat, double lon, int zoom)
        {
            double latRad = Math.PI / 180.0 * lat;

            double n = Math.Pow(2.0, zoom);

            int xTile = (int)((lon + 180.0) / 360.0 * n);
            int yTile = (int)((1.0 - Math.Log(Math.Tan(latRad) + 1 / Math.Cos(latRad)) / Math.PI) / 2.0 * n);

            return (xTile, yTile);
        }

        private readonly HttpClient _httpClient;
        private readonly string tilePath;
    }
}
