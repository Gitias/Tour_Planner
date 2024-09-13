using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;
using Tourplanner.BL;
using Tourplanner.BL.MapService;
using Tourplanner.DAL;
using Tourplanner.Shared.Logging;
using Tourplanner_.Features.AddLog;
using Tourplanner_.Features.AddTour;
using Tourplanner_.Features.Search;
using Tourplanner_.Features.TourView;
using Tourplanner_.Features.Validierung;

namespace Tourplanner_
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();

            mainWindow.DataContext = ServiceProvider.GetRequiredService<TourViewModel>();

            mainWindow.Show();
        }

        private void ConfigureServices(ServiceCollection services)
        {

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional:false, reloadOnChange: true)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddSingleton<AppDbContextFactory>();
            services.AddSingleton<ITourService, TourService>();
            services.AddSingleton<ITourRepository, TourRepository>();
            services.AddSingleton<ITourLogRepository, TourLogRepository>();
            services.AddSingleton<ITourLogService, TourLogService>();
            services.AddSingleton<IInputValidator, InputValidator>();
            services.AddSingleton<IExportService, ExportService>();
            services.AddSingleton<IImportService, ImportService>();
            services.AddSingleton<IPdfReportService, PdfReportService>();
            services.AddSingleton<ILogger, Log4NetLogger>();
            services.AddSingleton<ILoggerFactory, Log4NetFactory>();
            services.AddSingleton<TourAttributeCalculator>();
            services.AddSingleton<DirectionsService>();
            services.AddSingleton<GeocodeService>();
            services.AddSingleton<MapService>();
            services.AddSingleton<TileService>();

            services.AddTransient<TourView>();
            services.AddTransient<TourViewModel>();
            services.AddTransient<AddTourView>();
            services.AddTransient<AddTourViewModel>();
            services.AddTransient<MainWindow>();
            services.AddTransient<AddLogView>();
            services.AddTransient<AddLogViewModel>();
            services.AddTransient<SearchViewModel>();
            services.AddTransient<SearchView>();
        }
    }

}
