namespace Tourplanner.Testing
{
    using Tourplanner.Shared;
    using Tourplanner_.Features.AddTour;

    [TestFixture]
    public class AddTourViewModelTests
    {
        [SetUp]
        public void Setup()
        {
            _addTourViewModel = new AddTourViewModel();
        }

        [Test]
        public void AddTour_WithValidTour_ShouldReturnTrue()
        {
            var tour = new Tour
            {
                Name = "TestTour",
                Description = "TestDescription",
                From = "TestFrom",
                To = "TestTo"
            };

            var result = _addTourViewModel.AddTourCommand.CanExecute(tour);

            Assert.That(result, Is.True);
        }

        [Test]
        public void SettingName_RaisesPropertyChangedEvent()
        {
            var raised = false;
            _addTourViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(AddTourViewModel.Name))
                {
                    raised = true;
                }
            };

            _addTourViewModel.Name = "Test";

            Assert.That(raised, Is.True);
        }

        [Test]
        public void Properties_DefaultValues_AreNullOrEmpty()
        {
            Assert.That(_addTourViewModel.Name, Is.Null.Or.Empty);
            Assert.That(_addTourViewModel.Description, Is.Null.Or.Empty);
            Assert.That(_addTourViewModel.From, Is.Null.Or.Empty);
            Assert.That(_addTourViewModel.To, Is.Null.Or.Empty);
            Assert.That(_addTourViewModel.TransportType, Is.Null.Or.Empty);
        }

        private AddTourViewModel _addTourViewModel;
    }
}
