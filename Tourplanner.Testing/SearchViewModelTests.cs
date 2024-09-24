namespace Tourplanner.Testing
{
    using Moq;
    using System.Collections.ObjectModel;
    using Tourplanner.BL;
    using Tourplanner.Shared;
    using Tourplanner_.Features.Search;

    [TestFixture]
    public class SearchViewModelTests
    {
        [SetUp]
        public void Setup()
        {
            _tourLogServiceMock = new Mock<ITourLogService>();
            _tourAttributeCalculatorMock = new Mock<TourAttributeCalculator>();
            _searchViewModel = new SearchViewModel(_tourAttributeCalculatorMock.Object, _tourLogServiceMock.Object);
        }

        [Test]
        public void Search_WithSearchTerm_ReturnsSearchResults()
        {
            var tours = new ObservableCollection<Tour>
            {
                new Tour
                {
                    Name = "Stadtrundgang",
                    Description = "Eine Tour durch die Stadt",
                    From = "Wien",
                    To = "Wien",
                    TransportType = "Fuß"
                },
                new Tour
                {
                    Name = "Wanderung",
                    Description = "Eine Wanderung durch die Berge",
                    From = "Innsbruck",
                    To = "Innsbruck",
                    TransportType = "Fuß"
                }
            };

            _searchViewModel.Search("Stadt", tours);    

            Assert.That(_searchViewModel.SearchResults.Count, Is.EqualTo(1));
            Assert.That(_searchViewModel.SearchResults[0].Name, Is.EqualTo("Stadtrundgang"));
        }

        [Test]
        public void Search_WithSearchTermAndTourLogs_ReturnsSearchResults()
        {
            var tours = new ObservableCollection<Tour>
            {
                new Tour
                {
                    Name = "Stadtrundgang",
                    Description = "Eine Tour durch die Stadt",
                    From = "Wien",
                    To = "Wien",
                    TransportType = "Fuß",
                    Id = 1
                },
                new Tour
                {
                    Name = "Wanderung",
                    Description = "Eine Wanderung durch die Berge",
                    From = "Innsbruck",
                    To = "Innsbruck",
                    TransportType = "Fuß",
                    Id = 2
                }
            };

            var logs = new ObservableCollection<TourLog>
            {
                new TourLog
                {
                    Comment = "Schöne Tour",
                    Distance = 10,
                    TotalTime = TimeSpan.FromHours(1),
                    Rating = 5,
                    Date = DateTime.Now,
                    Difficulty = "3",
                    TourId = 1
                }
            };

            _tourLogServiceMock.Setup(x => x.GetAllTourLogsFromTourAsync(1)).ReturnsAsync(logs);

            _searchViewModel.Search("Schöne", tours);

            Assert.That(_searchViewModel.SearchResults.Count, Is.EqualTo(1));
            Assert.That(_searchViewModel.SearchResults[0].Name, Is.EqualTo("Stadtrundgang"));
        }

        [Test]
        public void Search_NoMatchingSearchTerm_ReturnsNoSearchResults()
        {
            var tours = new ObservableCollection<Tour>
            {
                new Tour
                {
                    Name = "Stadtrundgang",
                    Description = "Eine Tour durch die Stadt",
                    From = "Wien",
                    To = "Wien",
                    TransportType = "Fuß"
                },
                new Tour
                {
                    Name = "Wanderung",
                    Description = "Eine Wanderung durch die Berge",
                    From = "Innsbruck",
                    To = "Innsbruck",
                    TransportType = "Fuß"
                }
            };

            _searchViewModel.Search("Test", tours);

            Assert.That(_searchViewModel.SearchResults.Count, Is.EqualTo(0));
        }

        private SearchViewModel _searchViewModel;
        private Mock<ITourLogService> _tourLogServiceMock;
        private Mock<TourAttributeCalculator> _tourAttributeCalculatorMock;
    }
}
