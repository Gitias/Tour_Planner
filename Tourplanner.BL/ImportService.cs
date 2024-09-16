namespace Tourplanner.BL
{
    using Microsoft.Win32;
    using System.IO;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Tourplanner.Shared;

    public class ImportService : IImportService
    {
        public async Task<Tour?> ImportTourFromJsonAsync()
        {
            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "JSON files (*.json)|*.json",
                    Title = "Import Tour from JSON"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    var json = await File.ReadAllTextAsync(openFileDialog.FileName);

                    return JsonSerializer.Deserialize<Tour>(json);
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
