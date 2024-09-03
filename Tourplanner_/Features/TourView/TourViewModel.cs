using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Tourplanner.BL;
using Tourplanner.Shared;
using Tourplanner_.Features.AddLog;
using Tourplanner_.Features.AddTour;
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

        public string Popularity => SelectedTour != null ? _tourAttributeCalculator.CalculatePopularity(SelectedTour) : string.Empty;

        public string ChildFriendliness => SelectedTour != null ? _tourAttributeCalculator.CalculateChildFriendliness(SelectedTour) : string.Empty;

        public ICommand? AddTourCommand { get; set; }
        public ICommand? EditTourCommand { get; set; }
        public ICommand? DeleteTourCommand { get; set; }

        public ICommand? AddTourLogCommand { get; set; }
        public ICommand? EditTourLogCommand { get; set; }
        public ICommand? DeleteTourLogCommand { get; set; }

        public TourViewModel(IServiceProvider serviceProvider, ITourService tourService, ITourLogService tourLogService, IInputValidator inputValidator, TourAttributeCalculator tourAttributeCalculator)
        {
            _serviceProvider = serviceProvider;
            _tourService = tourService;
            _tourLogService = tourLogService;
            _inputValidator = inputValidator;
            _tourAttributeCalculator = tourAttributeCalculator;

            AddTourCommand = new RelayCommand(_ => AddTour());
            EditTourCommand = new RelayCommand(_ => EditTour(), _ => SelectedTour != null);
            DeleteTourCommand = new RelayCommand(async _ => await DeleteTour(), _ => SelectedTour != null);

            AddTourLogCommand = new RelayCommand(_ => AddTourLog(), _ => SelectedTour != null);
            EditTourLogCommand = new RelayCommand(_ => EditTourLog(), _ => SelectedTourLog != null);
            DeleteTourLogCommand = new RelayCommand(async _ => await DeleteTourLog(), _ => SelectedTourLog != null);

            LoadTours();
        }

        private async void LoadTours()
        {
            try
            {
                var tours = await _tourService.GetAllToursAsync();
                Tours?.Clear();
                foreach (var tour in tours)
                {
                    Tours?.Add(tour);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der Touren", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Löschen eines TourLogs", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                                return;
                            }

                            await _tourLogService.UpdateTourLogAsync(SelectedTourLog);

                            await LoadTourLogsAsync();
                        }

                        editTourLogView.Close();
                    };

                    editTourLogView.ShowDialog();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Bearbeiten eines TourLogs", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                                return;
                            }

                            await _tourLogService.AddTourLogAsync(tourLog);

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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Löschen einer Tour", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                                return;
                            }

                            await _tourService.UpdateTourAsync(SelectedTour);

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
                            return;
                        }

                        await _tourService.AddTourAsync(tour);

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
            }
        }

        private TourLog? _selectedTourLog;
        private Tour? _selectedTour;
        private readonly ITourService _tourService;
        private readonly ITourLogService _tourLogService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IInputValidator _inputValidator;
        private readonly TourAttributeCalculator _tourAttributeCalculator;
    }
}
