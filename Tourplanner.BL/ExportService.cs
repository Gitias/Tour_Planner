namespace Tourplanner.BL
{
    using Microsoft.Win32;
    using System.IO;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Tourplanner.Shared;

    public class ExportService : IExportService
    {
        public async Task<bool> ExportTourToJsonAsync(Tour tour)
        {
            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "JSON files (*.json)|*.json",
                    Title = "Export Tour to JSON",
                    FileName = $"{tour.Name}.json"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string json = JsonSerializer.Serialize(tour);
                    await File.WriteAllTextAsync(saveFileDialog.FileName, json);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
