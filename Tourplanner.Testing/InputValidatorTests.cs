namespace Tourplanner.Testing
{
    using Tourplanner.Shared;
    using Tourplanner_.Features.Validierung;

    [TestFixture]
    public class InputValidatorTests
    {

        [SetUp]
        public void Setup()
        {
            _inputValidator = new InputValidator();
        }

        [Test]
        public void ValidateTour_WithValidTour_ShouldReturnTrue()
        {
            var tour = new Tour
            {
                Name = "TestTour",
                Description = "TestDescription",
                From = "TestFrom",
                To = "TestTo"
            };

            var result = _inputValidator.ValidateTour(tour, out var error);

            Assert.That(result, Is.True);
            Assert.That(error, Is.Empty);
        }

        [Test]
        public void ValidateTour_WithInvalidTour_ShouldReturnFalse()
        {
            var tour = new Tour
            {
                Name = "",
                Description = "TestDescription",
                From = "TestFrom",
                To = "TestTo"
            };

            var result = _inputValidator.ValidateTour(tour, out var error);

            Assert.That(result, Is.False);
            Assert.That(error, Is.Not.Empty);
        }

        [Test]
        public void ValidateTour_NameContainsUnallowedCharacters_ShouldReturnFalse()
        {
            var tour = new Tour
            {
                Name = "<script>alert('XSS')</script>",
                Description = "TestDescription",
                From = "TestFrom",
                To = "TestTo"
            };

            var result = _inputValidator.ValidateTour(tour, out var error);

            Assert.That(result, Is.False);
            Assert.That(error, Is.Not.Empty);
        }

        [Test]
        public void ValidateTour_MultipleInvalidFields_ShouldReturnFalse()
        {
            var tour = new Tour
            {
                Name = "",
                Description = "<script>alert('XSS')</script>",
                From = "TestFrom",
                To = "TestTo"
            };

            var result = _inputValidator.ValidateTour(tour, out var error);

            Assert.That(result, Is.False);
            Assert.That(error, Does.Contain("The input must not be empty."));
            Assert.That(error, Does.Contain("The input contains unallowed characters."));
        }

        [Test]
        public void ValidateTourLog_WithValidTourLog_ShouldReturnTrue()
        {
            var tourLog = new TourLog
            {
                Comment = "TestComment",
                Distance = 10,
                TotalTime = TimeSpan.FromHours(1),
                Rating = 5,
                Date = DateTime.Now,
                Difficulty = "3"
            };

            var result = _inputValidator.ValidateTourLog(tourLog, out var error);

            Assert.That(result, Is.True);
            Assert.That(error, Is.Empty);
        }

        [Test]
        public void ValidateTourLog_WithInvalidTourLog_ShouldReturnFalse()
        {
            var tourLog = new TourLog
            {
                Comment = "",
                Distance = 10,
                TotalTime = TimeSpan.FromHours(1),
                Rating = 5,
                Date = DateTime.Now,
                Difficulty = "3"
            };

            var result = _inputValidator.ValidateTourLog(tourLog, out var error);

            Assert.That(result, Is.False);
            Assert.That(error, Is.Not.Empty);
        }

        [Test]
        public void ValidateTourLog_CommentContainsUnallowedCharacters_ShouldReturnFalse()
        {
            var tourLog = new TourLog
            {
                Comment = "<script>alert('XSS')</script>",
                Distance = 10,
                TotalTime = TimeSpan.FromHours(1),
                Rating = 5,
                Date = DateTime.Now,
                Difficulty = "3"
            };

            var result = _inputValidator.ValidateTourLog(tourLog, out var error);

            Assert.That(result, Is.False);
            Assert.That(error, Is.Not.Empty);
        }

        private IInputValidator _inputValidator; 
    }
}
