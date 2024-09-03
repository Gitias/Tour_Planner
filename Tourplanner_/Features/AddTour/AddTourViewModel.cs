using System.Windows.Input;
using Tourplanner.Shared;

namespace Tourplanner_.Features.AddTour
{
    public class AddTourViewModel : BaseViewModel
    {
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public string From
        {
            get => _from;
            set
            {
                _from = value;
                OnPropertyChanged(nameof(From));
            }
        }

        public string To
        {
            get => _to;
            set
            {
                _to = value;
                OnPropertyChanged(nameof(To));
            }
        }

        public string TransportType
        {
            get => _transportType;
            set
            {
                _transportType = value;
                OnPropertyChanged(nameof(TransportType));
            }
        }

        public ICommand AddTourCommand { get; set; }

        public event Action? TourAdded;

        public AddTourViewModel()
        {
            AddTourCommand = new RelayCommand(_ => AddTour());
        }

        private void AddTour()
        {
            TourAdded?.Invoke();
        }

        private string _name;
        private string _description;
        private string _from;
        private string _to;
        private string _transportType;
    }
}
