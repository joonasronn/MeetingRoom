using MeetingRoom.Utils;
using System.ComponentModel;

namespace MeetingRoom.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private ReservationsViewModel? reservations;
        private EnvViewModel? env;
        private NewReservationViewModel? newReservation;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private DB? dbHandler;

        public DB DBHandler
        {
            get { return dbHandler; }
            set { dbHandler = value; }
        }

        public ReservationsViewModel? Reservations { get => reservations; set { reservations = value; OnPropertyChanged(nameof(Reservations)); } }
        public EnvViewModel? Env { get => env; set { env = value; OnPropertyChanged(nameof(Env)); } }
        public NewReservationViewModel? NewReservation { get => newReservation; set { newReservation = value; OnPropertyChanged(nameof(NewReservation)); } }
        public MainWindowViewModel()
        {
            this.Reservations = new ReservationsViewModel(this);
            this.Env = new EnvViewModel(this);
            this.NewReservation = new NewReservationViewModel(this);
        }

    }
}
