using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Tourplanner.BL;
using Tourplanner.Shared;
using Tourplanner_.Features.AddTour;

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

        public ICommand? AddTourCommand { get; set; }
        public ICommand? EditTourCommand { get; set; }
        public ICommand? DeleteTourCommand { get; set; }

        public ICommand? AddTourLogCommand { get; set; }
        public ICommand? EditTourLogCommand { get; set; }
        public ICommand? DeleteTourLogCommand { get; set; }

        public TourViewModel(IServiceProvider serviceProvider, ITourService tourService)
        {
            _serviceProvider = serviceProvider;
            _tourService = tourService;

            AddTourCommand = new RelayCommand(_ => AddTour());
            EditTourCommand = new RelayCommand(_ => EditTour());
            DeleteTourCommand = new RelayCommand(_ => DeleteTour());

            AddTourLogCommand = new RelayCommand(_ => AddTourLog());
            EditTourLogCommand = new RelayCommand(_ => EditTourLog());
            DeleteTourLogCommand = new RelayCommand(_ => DeleteTourLog());

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

        private void DeleteTourLog()
        {
            throw new NotImplementedException();
        }

        private void EditTourLog()
        {
            throw new NotImplementedException();
        }

        private void AddTourLog()
        {
            throw new NotImplementedException();
        }

        private void DeleteTour()
        {
            throw new NotImplementedException();
        }

        private void EditTour()
        {
            throw new NotImplementedException();
        }

        private void AddTour()
        {
            try
            {
                var addTourViewModel = _serviceProvider.GetRequiredService<AddTourViewModel>();
                var addTourView = _serviceProvider.GetRequiredService<AddTourView>();

                if(addTourView != null && addTourViewModel != null)
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
        private readonly IServiceProvider _serviceProvider;
    }
}
