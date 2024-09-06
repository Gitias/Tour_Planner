namespace Tourplanner_.Features.AddLog
{
    using System.Windows.Input;

    public class AddLogViewModel : BaseViewModel
    {

        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public string? Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }

        public string? Difficulty
        {
            get => _difficulty;
            set
            {
                _difficulty = value;
                OnPropertyChanged(nameof(Difficulty));
            }
        }

        public double Distance
        {
            get => _distance;
            set
            {
                _distance = value;
                OnPropertyChanged(nameof(Distance));
            }
        }

        public TimeSpan TotalTime
        {
            get => _totalTime;
            set
            {
                _totalTime = value;
                OnPropertyChanged(nameof(TotalTime));
            }
        }

        public int Rating
        {
            get => _rating;
            set
            {
                _rating = value;
                OnPropertyChanged(nameof(Rating));
            }
        }

        public ICommand AddLogCommand { get; set; }

        public event Action? LogAdded;

        public AddLogViewModel()
        {
            AddLogCommand = new RelayCommand(_ => AddLog());
        }

        private void AddLog()
        {
            LogAdded?.Invoke();
        }

        private DateTime _date;
        private string? _comment;
        private string? _difficulty;
        private double _distance;
        private TimeSpan _totalTime;
        private int _rating;
    }
}
