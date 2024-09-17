namespace Tourplanner.Testing
{
    using Tourplanner.Shared;
    using Tourplanner_.Features.AddLog;

    [TestFixture]
    public class AddLogViewModelTests
    {

        [SetUp]
        public void Setup()
        {
            _addLogViewModel = new AddLogViewModel();
        }

        [Test]
        public void SettingDate_RaisesPropertyChangedEvent()
        {
            var raised = false;
            _addLogViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(AddLogViewModel.Date))
                {
                    raised = true;
                }
            };

            _addLogViewModel.Date = DateTime.Now;

            Assert.That(raised, Is.True);
        }

        [Test]
        public void Properties_DefaultValues_AreNullOrEmpty()
        {
            Assert.That(_addLogViewModel.Date, Is.EqualTo(default(DateTime)));
            Assert.That(_addLogViewModel.Comment, Is.Null.Or.Empty);
            Assert.That(_addLogViewModel.Difficulty, Is.Null.Or.Empty);
            Assert.That(_addLogViewModel.Distance, Is.EqualTo(default(double)));
            Assert.That(_addLogViewModel.TotalTime, Is.EqualTo(default(TimeSpan)));
            Assert.That(_addLogViewModel.Rating, Is.EqualTo(default(int)));
        }

        [Test]
        public void AddLog_WithValidLog_ShouldReturnTrue()
        {
            var log = new TourLog
            {
                Comment = "TestComment",
                Distance = 10,
                TotalTime = TimeSpan.FromHours(1),
                Rating = 5,
                Date = DateTime.Now,
                Difficulty = "3"
            };

            var result = _addLogViewModel.AddLogCommand.CanExecute(log);

            Assert.That(result, Is.True);
        }


        private AddLogViewModel _addLogViewModel;
    }
}
