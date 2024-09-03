using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Tourplanner.BL;
using Tourplanner.DAL;
using Tourplanner_.Features.AddLog;
using Tourplanner_.Features.AddTour;
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
            services.AddSingleton<AppDbContext>();
            services.AddSingleton<ITourService, TourService>();
            services.AddSingleton<ITourRepository, TourRepository>();
            services.AddSingleton<ITourLogRepository, TourLogRepository>();
            services.AddSingleton<ITourLogService, TourLogService>();
            services.AddSingleton<IInputValidator, InputValidator>();
            services.AddSingleton<TourAttributeCalculator>();

            services.AddTransient<TourView>();
            services.AddTransient<TourViewModel>();
            services.AddTransient<AddTourView>();
            services.AddTransient<AddTourViewModel>();
            services.AddTransient<MainWindow>();
            services.AddTransient<AddLogView>();
            services.AddTransient<AddLogViewModel>();
        }
    }

}
