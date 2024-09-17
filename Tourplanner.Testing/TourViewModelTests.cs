namespace Tourplanner.Testing
{
    using Moq;
    using System.Collections.ObjectModel;
    using Tourplanner.BL;
    using Tourplanner.BL.MapService;
    using Tourplanner.Shared;
    using Tourplanner.Shared.Logging;
    using Tourplanner_.Features;
    using Tourplanner_.Features.TourView;
    using Tourplanner_.Features.Validierung;

    [TestFixture]
    public class TourViewModelTests
    {
        [SetUp]
        public void Setup()
        {
            _tourServiceMock = new Mock<ITourService>();
            _tourLogServiceMock = new Mock<ITourLogService>();
            _inputValidatorMock = new Mock<IInputValidator>();
            _exportServiceMock = new Mock<IExportService>();
            _importServiceMock = new Mock<IImportService>();
            _pdfReportServiceMock = new Mock<IPdfReportService>();
            _loggerMock = new Mock<ILogger>();
            _loggerFactoryMock = new Mock<ILoggerFactory>();
            _attributeCalculatorMock = new Mock<TourAttributeCalculator>();
            _mapServiceMock = new Mock<IMapService>();
            serviceProvider = new Mock<IServiceProvider>().Object;

            _loggerFactoryMock.Setup(x => x.CreateLogger<TourViewModel>()).Returns(_loggerMock.Object);

            _tourViewModel = new TourViewModel(
                serviceProvider,
                _tourServiceMock.Object,
                _tourLogServiceMock.Object,
                _inputValidatorMock.Object,
                _attributeCalculatorMock.Object,
                _exportServiceMock.Object,
                _importServiceMock.Object,
                _mapServiceMock.Object,
                _pdfReportServiceMock.Object,
                _loggerFactoryMock.Object);
        }

        [Test]
        public void ShowFavoritesOnly_WithFavoritesOnly_ShouldReturnTrue()
        {
            var allTours = new ObservableCollection<Tour>
            {
                new Tour { Id = 1, Name = "Tour1", IsFavorite = true },
                new Tour { Id = 2, Name = "Tour2", IsFavorite = false },
                new Tour { Id = 3, Name = "Tour3", IsFavorite = true }
            };

            _tourServiceMock.Setup(x => x.GetAllToursAsync()).ReturnsAsync(allTours);

            _tourViewModel.LoadTours();
            _tourViewModel.ShowFavoritesOnly = true;

            Assert.That(_tourViewModel.Tours.Count, Is.EqualTo(2));
            Assert.IsTrue(_tourViewModel.Tours.All(x => x.IsFavorite));
        }

        [Test]
        public void SelectedTour_Setter_RaisesPropertyChanged()
        {
            bool raised = false;
            _tourViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(_tourViewModel.SelectedTour))
                {
                    raised = true;
                }
            };

            var tour = new Tour { Id = 1, Name = "TestTour" };

            _tourViewModel.SelectedTour = tour;

            Assert.That(raised, Is.True);
            Assert.That(_tourViewModel.SelectedTour, Is.EqualTo(tour));
        }

        [Test]
        public void EditTourCommand_UpdatesSelectedTour()
        {
            var originalTour = new Tour { Id = 1, Name = "OriginalTour" };

            _tourViewModel.Tours = new ObservableCollection<Tour> { originalTour };
            _tourViewModel.SelectedTour = originalTour;

            var newName = "EditedTour";

            _tourViewModel.EditTourCommand = new RelayCommand(_ =>
            {
                _tourViewModel.SelectedTour.Name = newName;
            }, _ => _tourViewModel.SelectedTour != null);

            _tourViewModel.EditTourCommand.Execute(null);


            var editedTour = _tourViewModel.SelectedTour;

            Assert.That(_tourViewModel.SelectedTour, Is.EqualTo(editedTour));
            Assert.That(editedTour, Is.EqualTo(_tourViewModel.Tours[0]));
        }

        [Test]
        public void SelectedTour_CalculatesPopularity()
        {
            var tourLogs = new ObservableCollection<TourLog>
            {
                new TourLog { Rating = 5 },
                new TourLog { Rating = 3 },
                new TourLog { Rating = 4 }
            };

            var tour = new Tour { Id = 1, Name = "TestTour", TourLogs = tourLogs };

            _tourViewModel.SelectedTour = tour;

            Assert.That(_tourViewModel.Popularity, Is.EqualTo("Popular"));
        }

        private TourViewModel _tourViewModel;
        private Mock<ITourService> _tourServiceMock;
        private Mock<ITourLogService> _tourLogServiceMock;
        private Mock<IInputValidator> _inputValidatorMock;
        private Mock<IExportService> _exportServiceMock;
        private Mock<IImportService> _importServiceMock;
        private Mock<IPdfReportService> _pdfReportServiceMock;
        private Mock<ILogger> _loggerMock;
        private Mock<ILoggerFactory> _loggerFactoryMock;
        private Mock<TourAttributeCalculator> _attributeCalculatorMock;
        private Mock<IMapService> _mapServiceMock;
        private IServiceProvider serviceProvider;
    }
}
