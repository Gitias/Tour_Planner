using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Tourplanner.BL;
using Tourplanner.BL.MapService;
using Tourplanner.Shared;
using Tourplanner.Shared.Logging;
using Tourplanner_.Features.AddLog;
using Tourplanner_.Features.AddTour;
using Tourplanner_.Features.Search;
using Tourplanner_.Features.Validierung;

namespace Tourplanner_.Features.TourView
{
    public class TourViewModel : BaseViewModel
    {
        public ObservableCollection<Tour>? Tours { get; set; } = new ObservableCollection<Tour>();

        public ObservableCollection<TourLog>? TourLogs { get; set; } = new ObservableCollection<TourLog>();

        public Tour? SelectedTour
        {
            get => _selectedTour;
            set
            {
                _selectedTour = value;
                OnPropertyChanged(nameof(SelectedTour));
                OnPropertyChanged(nameof(Popularity));
                OnPropertyChanged(nameof(ChildFriendliness));
                LoadTourLogsAsync();
                GenerateRouteMap();
            }
        }

        public TourLog? SelectedTourLog
        {
            get => _selectedTourLog;
            set
            {
                _selectedTourLog = value;
                OnPropertyChanged(nameof(SelectedTourLog));
            }
        }

        public BitmapImage? RouteMapImage
        {
            get => routeMapImage;
            set
            {
                routeMapImage = value;
                OnPropertyChanged(nameof(RouteMapImage));
            }
        }

        public string? RouteMapImagePath
        {
            get => routeMapImagePath;
            set
            {
                routeMapImagePath = value;
                OnPropertyChanged(nameof(RouteMapImagePath));
            }
        }

        public double? RouteTime
        {
            get => routeTime;
            set
            {
                routeTime = value;
                OnPropertyChanged(nameof(RouteTime));
            }
        }

        public double? RouteDistance
        {
            get => routeDistance;
            set
            {
                routeDistance = value;
                OnPropertyChanged(nameof(RouteDistance));
            }
        }

        public bool ShowFavoritesOnly
        {
            get => showFavoritesOnly;
            set
            {
                showFavoritesOnly = value;
                OnPropertyChanged(nameof(ShowFavoritesOnly));
                FilterTours();
            }
        }

        public string Popularity => SelectedTour != null ? _tourAttributeCalculator.CalculatePopularity(SelectedTour) : string.Empty;

        public string ChildFriendliness => SelectedTour != null ? _tourAttributeCalculator.CalculateChildFriendliness(SelectedTour) : string.Empty;

        public ICommand? AddTourCommand { get; set; }
        public ICommand? EditTourCommand { get; set; }
        public ICommand? DeleteTourCommand { get; set; }

        public ICommand? AddTourLogCommand { get; set; }
        public ICommand? EditTourLogCommand { get; set; }
        public ICommand? DeleteTourLogCommand { get; set; }

        public ICommand SearchCommand { get; set; }
        public ICommand ExportCommand { get; set; }
        public ICommand ImportCommand { get; set; }
        public ICommand TourPdfCommand { get; set; }
        public ICommand SummarizePdfCommand { get; set; }

        public TourViewModel(
            IServiceProvider serviceProvider,
            ITourService tourService,
            ITourLogService tourLogService,
            IInputValidator inputValidator,
            TourAttributeCalculator tourAttributeCalculator,
            IExportService exportService,
            IImportService importService,
            MapService mapService,
            IPdfReportService pdfReportService,
            ILoggerFactory loggerFactory)
        {
            _serviceProvider = serviceProvider;
            _tourService = tourService;
            _tourLogService = tourLogService;
            _inputValidator = inputValidator;
            _tourAttributeCalculator = tourAttributeCalculator;
            _exportService = exportService;
            _importService = importService;
            _mapService = mapService;
            _pdfReportService = pdfReportService;

            logger = loggerFactory.CreateLogger<TourViewModel>();

            _alltours = new ObservableCollection<Tour>();

            AddTourCommand = new RelayCommand(_ => AddTour());
            EditTourCommand = new RelayCommand(_ => EditTour(), _ => SelectedTour != null);
            DeleteTourCommand = new RelayCommand(async _ => await DeleteTour(), _ => SelectedTour != null);

            AddTourLogCommand = new RelayCommand(_ => AddTourLog(), _ => SelectedTour != null);
            EditTourLogCommand = new RelayCommand(_ => EditTourLog(), _ => SelectedTourLog != null);
            DeleteTourLogCommand = new RelayCommand(async _ => await DeleteTourLog(), _ => SelectedTourLog != null);

            SearchCommand = new RelayCommand(async p => await PerformSearch(p));
            ExportCommand = new RelayCommand(async _ => await ExportTourData(), _ => SelectedTour != null);
            ImportCommand = new RelayCommand(async _ => await ImportTourData());
            TourPdfCommand = new RelayCommand(async _ => await GenerateTourPdf(), _ => SelectedTour != null);
            SummarizePdfCommand = new RelayCommand(async _ => await GenerateSummaryPdf(), _ => Tours?.Count > 0);

            LoadTours();
        }

        private void FilterTours()
        {
            if(ShowFavoritesOnly)
            {
                Tours = new ObservableCollection<Tour>(_alltours.Where(t => t.IsFavorite));
            }
            else
            {
                Tours = new ObservableCollection<Tour>(_alltours);
            }

            OnPropertyChanged(nameof(Tours));
        }

        private async Task GenerateSummaryPdf()
        {
            try
            {
                await _pdfReportService.GenerateSummaryReport(Tours);
                MessageBox.Show("PDF erfolgreich erstellt", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                logger.Info("PDF erfolgreich erstellt");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Erstellen des PDFs", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error("Fehler beim Erstellen des PDFs");
            }
        }

        private async Task GenerateTourPdf()
        {
            try
            {
                SelectedTour.ChildFriendliness = ChildFriendliness;
                SelectedTour.Popularity = Popularity;

                await _pdfReportService.GenerateTourReport(SelectedTour);

                MessageBox.Show("PDF erfolgreich erstellt", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                logger.Info("PDF erfolgreich erstellt");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Erstellen des PDFs", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error("Fehler beim Erstellen des PDFs");
            }
        }

        private async Task ImportTourData()
        {
            try
            {
                var result = await _importService.ImportTourFromJsonAsync();

                if (result != null)
                {
                    Tours?.Add(result);
                    await _tourService.AddTourAsync(result);

                    SelectedTour = result;
                    MessageBox.Show("Tourdaten erfolgreich importiert", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    logger.Info("Tourdaten erfolgreich importiert");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Importieren der Tourdaten", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error("Fehler beim Importieren der Tourdaten");
            }
        }

        private async Task ExportTourData()
        {
            try
            {
                SelectedTour.ChildFriendliness = ChildFriendliness;
                SelectedTour.Popularity = Popularity;

                var result = await _exportService.ExportTourToJsonAsync(SelectedTour);

                if (result) 
                {
                    MessageBox.Show("Tourdaten erfolgreich exportiert", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    logger.Info("Tourdaten erfolgreich exportiert");
                }
                else
                {
                    MessageBox.Show("Export der Tourdaten abgebrochen", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    logger.Error("Export der Tourdaten abgebrochen");
                }

                SelectedTour = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Exportieren der Tourdaten", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error("Fehler beim Exportieren der Tourdaten");
            }
        }

        private async Task PerformSearch(object p)
        {
            var search = p as string;

            if (string.IsNullOrWhiteSpace(search))
            {
                return;
            }

            var searchViewModel = _serviceProvider.GetRequiredService<SearchViewModel>();
            var searchView = _serviceProvider.GetRequiredService<SearchView>();

            if (searchViewModel != null && searchView != null)
            {
                searchView.DataContext = searchViewModel;

                await searchViewModel.Search(search, Tours);

                if (searchViewModel.SearchResults.Count == 0)
                {
                    MessageBox.Show("Keine Ergebnisse gefunden", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    logger.Info("Keine Ergebnisse bei der Suche gefunden");
                    return;
                }

                searchView.ShowDialog();
                logger.Info("Erfolgreiche Suche durchgeführt");
            }
        }

        private async void LoadTours()
        {
            try
            {
                var tours = await _tourService.GetAllToursAsync();

                _alltours.Clear();

                foreach (var tour in tours)
                {
                    _alltours.Add(tour);
                }

                FilterTours();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der Touren", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error("Fehler beim Laden der Touren");
            }
        }
        private async Task LoadTourLogsAsync()
        {
            try
            {
                if (SelectedTour == null)
                {
                    return;
                }

                var tourLogs = await _tourLogService.GetAllTourLogsFromTourAsync(SelectedTour.Id);
                TourLogs?.Clear();
                foreach (var tourLog in tourLogs)
                {
                    TourLogs?.Add(tourLog);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der TourLogs", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error("Fehler beim Laden der TourLogs");
            }
        }

        private async Task DeleteTourLog()
        {
            try
            {
                var result = MessageBox.Show("Möchten Sie den TourLog wirklich löschen?", "TourLog löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    await _tourLogService.DeleteTourLogAsync(SelectedTourLog.Id);
                    TourLogs?.Remove(SelectedTourLog);
                    SelectedTourLog = null;
                    logger.Info("TourLog erfolgreich gelöscht");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Löschen eines TourLogs", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error("Fehler beim Löschen eines TourLogs");
            }
        }

        private void EditTourLog()
        {
            try
            {
                var editTourLogViewModel = _serviceProvider.GetRequiredService<AddLogViewModel>();
                var editTourLogView = _serviceProvider.GetRequiredService<AddLogView>();

                if (editTourLogViewModel != null && editTourLogView != null)
                {
                    editTourLogView.DataContext = editTourLogViewModel;

                    editTourLogViewModel.Comment = SelectedTourLog?.Comment;
                    editTourLogViewModel.Difficulty = SelectedTourLog?.Difficulty;
                    editTourLogViewModel.Distance = SelectedTourLog.Distance;
                    editTourLogViewModel.Rating = SelectedTourLog.Rating;
                    editTourLogViewModel.TotalTime = SelectedTourLog.TotalTime;

                    editTourLogViewModel.LogAdded += async () =>
                    {
                        if (SelectedTourLog != null)
                        {
                            SelectedTourLog.Comment = editTourLogViewModel.Comment;
                            SelectedTourLog.Difficulty = editTourLogViewModel.Difficulty;
                            SelectedTourLog.Distance = editTourLogViewModel.Distance;
                            SelectedTourLog.Rating = editTourLogViewModel.Rating;
                            SelectedTourLog.TotalTime = editTourLogViewModel.TotalTime;

                            var result = _inputValidator.ValidateTourLog(SelectedTourLog, out var error);

                            if (!result)
                            {
                                MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                logger.Error("Fehler beim Bearbeiten eines TourLogs");
                                return;
                            }

                            await _tourLogService.UpdateTourLogAsync(SelectedTourLog);

                            await LoadTourLogsAsync();

                            logger.Info("TourLog erfolgreich bearbeitet");
                        }

                        editTourLogView.Close();
                    };

                    editTourLogView.ShowDialog();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Bearbeiten eines TourLogs", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error("Fehler beim Bearbeiten eines TourLogs");
            }
        }

        private void AddTourLog()
        {
            try
            {
                var addTourLogViewModel = _serviceProvider.GetRequiredService<AddLogViewModel>();
                var addTourLogView = _serviceProvider.GetRequiredService<AddLogView>();

                if (addTourLogView != null && addTourLogViewModel != null)
                {
                    addTourLogView.DataContext = addTourLogViewModel;

                    addTourLogViewModel.LogAdded += async () =>
                    {
                        if (SelectedTour != null)
                        {
                            var tourLog = new TourLog
                            {
                                Comment = addTourLogViewModel.Comment,
                                Difficulty = addTourLogViewModel.Difficulty,
                                Distance = addTourLogViewModel.Distance,
                                Rating = addTourLogViewModel.Rating,
                                TotalTime = addTourLogViewModel.TotalTime,
                                TourId = SelectedTour.Id
                            };

                            var result = _inputValidator.ValidateTourLog(tourLog, out var error);

                            if (!result)
                            {
                                MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                logger.Error("Fehler beim Hinzufügen eines TourLogs");
                                return;
                            }

                            await _tourLogService.AddTourLogAsync(tourLog);

                            logger.Info("TourLog erfolgreich hinzugefügt");

                            TourLogs?.Add(tourLog);
                            addTourLogView.Close();
                        }
                    };

                    addTourLogView.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Hinzufügen eines TourLogs", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error("Fehler beim Hinzufügen eines TourLogs");
            }
        }

        private async Task DeleteTour()
        {
            try
            {
                var result = MessageBox.Show("Möchten Sie die Tour wirklich löschen?", "Tour löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes && SelectedTour != null)
                {
                    await _tourService.DeleteTourAsync(SelectedTour.Id);
                    Tours?.Remove(SelectedTour);
                    SelectedTour = null;
                    TourLogs?.Clear();

                    logger.Info("Tour erfolgreich gelöscht");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Löschen einer Tour", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error("Fehler beim Löschen einer Tour");
            }
        }

        private async void EditTour()
        {
            try
            {
                var editTourViewModel = _serviceProvider.GetRequiredService<AddTourViewModel>();
                var editTourView = _serviceProvider.GetRequiredService<AddTourView>();

                if (editTourView != null && editTourViewModel != null)
                {
                    editTourView.DataContext = editTourViewModel;

                    editTourViewModel.Name = SelectedTour?.Name;
                    editTourViewModel.Description = SelectedTour?.Description;
                    editTourViewModel.From = SelectedTour?.From;
                    editTourViewModel.To = SelectedTour?.To;
                    editTourViewModel.TransportType = SelectedTour?.TransportType;

                    editTourViewModel.TourAdded += async () =>
                    {
                        if (SelectedTour != null)
                        {
                            SelectedTour.Name = editTourViewModel.Name;
                            SelectedTour.Description = editTourViewModel.Description;
                            SelectedTour.From = editTourViewModel.From;
                            SelectedTour.To = editTourViewModel.To;
                            SelectedTour.TransportType = editTourViewModel.TransportType;

                            var result = _inputValidator.ValidateTour(SelectedTour, out var error);

                            if (!result)
                            {
                                MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                logger.Error("Fehler beim Bearbeiten einer Tour");
                                return;
                            }

                            await _tourService.UpdateTourAsync(SelectedTour);
                            logger.Info("Tour erfolgreich bearbeitet");
                            LoadTours();
                        }

                        editTourView.Close();
                    };

                    editTourView.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Bearbeiten einer Tour", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error("Fehler beim Bearbeiten einer Tour");
            }
        }

        private void AddTour()
        {
            try
            {
                var addTourViewModel = _serviceProvider.GetRequiredService<AddTourViewModel>();
                var addTourView = _serviceProvider.GetRequiredService<AddTourView>();

                if (addTourView != null && addTourViewModel != null)
                {
                    addTourView.DataContext = addTourViewModel;

                    addTourViewModel.TourAdded += async () =>
                    {
                        var tour = new Tour
                        {
                            Name = addTourViewModel.Name,
                            Description = addTourViewModel.Description,
                            From = addTourViewModel.From,
                            To = addTourViewModel.To,
                            TransportType = addTourViewModel.TransportType
                        };

                        var result = _inputValidator.ValidateTour(tour, out var error);

                        if (!result)
                        {
                            MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            logger.Error("Fehler beim Erstellen einer Tour");
                            return;
                        }

                        await _tourService.AddTourAsync(tour);
                        
                        logger.Info("Tour erfolgreich hinzugefügt");

                        Tours?.Add(tour);
                        SelectedTour = tour;
                        addTourView.Close();
                    };

                    addTourView.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Erstellen einer Tour", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error("Fehler beim Erstellen einer Tour");
            }
        }

        private async Task GenerateRouteMap()
        {
            if (SelectedTour != null)
            {
                var mapInfo = await _mapService.GenerateRouteMapAsync(SelectedTour);

                if (mapInfo != null)
                {
                    RouteMapImage = ConvertPathToImage(mapInfo.MapImagePath);
                    RouteMapImagePath = mapInfo.MapImagePath;
                    RouteTime = mapInfo.Time;
                    RouteDistance = mapInfo.Distance;

                    OnPropertyChanged(nameof(RouteMapImage));
                    OnPropertyChanged(nameof(RouteMapImagePath));
                    OnPropertyChanged(nameof(RouteTime));
                    OnPropertyChanged(nameof(RouteDistance));
                }
            }
        }

        private BitmapImage ConvertPathToImage(string path)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(path);
            image.EndInit();
            return image;
        }

        private ObservableCollection<Tour> _alltours;
        private bool showFavoritesOnly;
        private BitmapImage? routeMapImage;
        private string? routeMapImagePath;
        private double? routeTime;
        private double? routeDistance;
        private TourLog? _selectedTourLog;
        private Tour? _selectedTour;
        private readonly ITourService _tourService;
        private readonly ITourLogService _tourLogService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IInputValidator _inputValidator;
        private readonly IExportService _exportService;
        private readonly IImportService _importService;
        private readonly TourAttributeCalculator _tourAttributeCalculator;
        private readonly MapService _mapService;
        private readonly IPdfReportService _pdfReportService;
        private readonly ILogger logger;
    }
}
