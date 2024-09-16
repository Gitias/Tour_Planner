namespace Tourplanner.BL.MapService;

using Microsoft.Extensions.Configuration;
using System.Drawing;
using System.IO;
using Tourplanner.Shared;

public class MapService
{
    public MapService(
        GeocodeService geocodeService,
        DirectionsService directionsService,
        TileService tileService,
        IConfiguration configuration)
    {
        _geocodeService = geocodeService;
        _directionsService = directionsService;
        _tileService = tileService;
        _tilePath = configuration["MapService:ImagePath"]
            ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<TourMapInfo?> GenerateRouteMapAsync(Tour tour, int zoom = 17)
    {
        try
        {
            // Get geocode results for from and to addresses
            GeocodeResult? startGeocodeResult = await _geocodeService.GetGeocodeResultAsync(tour.From);
            GeocodeResult? endGeocodeResult = await _geocodeService.GetGeocodeResultAsync(tour.To);

            if (startGeocodeResult != null && endGeocodeResult != null)
            {
                List<double> startCoordinates = startGeocodeResult.Features[0].Geometry.Coordinates;
                List<double> endCoordinates = endGeocodeResult.Features[0].Geometry.Coordinates;

                if (startCoordinates.Count == 2 && endCoordinates.Count == 2)
                {
                    // Get directions result for start and end coordinates
                    DirectionsResult? directionsResult = await _directionsService.GetDirectionsResultAsync(startCoordinates, endCoordinates);

                    if (directionsResult != null)
                    {
                        (int x, int y) topLeftTile = TileService.LatLonToTile(directionsResult.Bbox[3], directionsResult.Bbox[0], zoom);

                        (int x, int y) bottomRightTile = TileService.LatLonToTile(directionsResult.Bbox[1], directionsResult.Bbox[2], zoom);

                        string imagePath = await DownloadAndCombineTilesAsync(
                            topLeftTile,
                            bottomRightTile,
                            zoom,
                            startCoordinates,
                            endCoordinates,
                            tour.Name);

                        return new TourMapInfo
                        {
                            TourName = tour.Name,
                            MapImagePath = imagePath,
                            TourId = tour.Id,
                            Distance = directionsResult.Features[0].Properties.Segements[0].Distance,
                            Time = directionsResult.Features[0].Properties.Segements[0].Duration
                        };
                    }

                }
            }

            return null;

        }
        catch (Exception ex)
        {
            // Log exception
            return null;
        }
    }

    private async Task<string> DownloadAndCombineTilesAsync(
        (int x, int y) topLeftTile,
        (int x, int y) bottomRightTile,
        int zoom,
        List<double> startCoordinates,
        List<double> endCoordinates,
        string tourName)
    {
        int width = (bottomRightTile.x - topLeftTile.x + 1) * 256;
        int height = (bottomRightTile.y - topLeftTile.y + 1) * 256;

        string mapPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _tilePath);

        if (!Directory.Exists(mapPath))
        {
            Directory.CreateDirectory(mapPath);
        }

        string tourPath = Path.Combine(mapPath, $"{tourName}_map.png");

        using var finalMap = new Bitmap(width, height);
        using var graphics = Graphics.FromImage(finalMap);

        var startTile = TileService.LatLonToTile(startCoordinates[1], startCoordinates[0], zoom);
        var endTile = TileService.LatLonToTile(endCoordinates[1], endCoordinates[0], zoom);

        for (int x = topLeftTile.x; x <= bottomRightTile.x; x++)
        {
            for (int y = topLeftTile.y; y <= bottomRightTile.y; y++)
            {
                string tileFileName = $"tile_{x}_{y}.png";

                await _tileService.DownloadTileAsync(x, y, zoom, tileFileName);

                string tilePath = Path.Combine(mapPath, tileFileName);

                if (File.Exists(tilePath))
                {
                    using var tileImage = Image.FromFile(tilePath);

                    graphics.DrawImage(tileImage, (x - topLeftTile.x) * 256, (y - topLeftTile.y) * 256);

                    if (x == startTile.x && y == startTile.y)
                    {
                        graphics.FillEllipse(Brushes.Green, (x - topLeftTile.x) * 256 + 100, (y - topLeftTile.y) * 256 + 100, 50, 50); // Large marker
                        graphics.DrawEllipse(Pens.Black, (x - topLeftTile.x) * 256 + 100, (y - topLeftTile.y) * 256 + 100, 50, 50); // Black outline
                    }

                    if (x == endTile.x && y == endTile.y)
                    {
                        graphics.FillEllipse(Brushes.Red, (x - topLeftTile.x) * 256 + 100, (y - topLeftTile.y) * 256 + 100, 50, 50); // Large marker
                        graphics.DrawEllipse(Pens.Black, (x - topLeftTile.x) * 256 + 100, (y - topLeftTile.y) * 256 + 100, 50, 50); // Black outline
                    }
                }
            }
        }

        finalMap.Save(tourPath);
        return tourPath;
    }

    private readonly string _tilePath;
    private readonly GeocodeService _geocodeService;
    private readonly DirectionsService _directionsService;
    private readonly TileService _tileService;
}

