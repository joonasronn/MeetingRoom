using MeetingRoom.Extensions;
using MeetingRoom.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace MeetingRoom.ViewModels
{
    internal class NewReservationViewModel : INotifyPropertyChanged
    {
        private Visibility visible = Visibility.Collapsed;
        private Reservation newReservation;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public MainWindowViewModel Parent { get; }
        public Reservation NewReservation { get => newReservation; set { newReservation = value; OnPropertyChanged(nameof(NewReservation)); } }
        public Visibility Visible { get => visible; set { visible = value; OnPropertyChanged(nameof(Visible)); } }
        public NewReservationViewModel(MainWindowViewModel mainWindowViewModel)
        {
            Parent = mainWindowViewModel;
        }
        public NewReservationViewModel()
        {

        }


        public ICommand SaveAndCloseCommand
        {
            get { return new RelayCommand(r => SaveAndClose(), r=> CanBeSaved()); }
        }

        private bool CanBeSaved()
        {
            if (Parent?.DBHandler == null)
            {
                return false;
            }

            if (NewReservation == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(NewReservation.Host))
            {
                return false;
            }

            if (NewReservation.MeetingRoomId < 0)
            {
                return false;
            }

            if (NewReservation.From > newReservation.To)
            {
                return false;
            }

            if (!Parent.DBHandler.IsFreeToReserve(NewReservation.From, newReservation.To, newReservation.MeetingRoomId))
            {
                return false;
            }
            

            return true;
            
        }

        private void SaveAndClose()
        {
            Parent.DBHandler.makeReservation(NewReservation);
            Parent.Reservations.InitializeReservationTable();
            ChangeVisibility();
        }

        public ICommand OpenOrCloseCommand
        {
            get { return new RelayCommand(r => ChangeVisibility(), r=> true); }
    }

        public void ChangeVisibility()
        {
            if (Visible == Visibility.Visible)
            {
                Visible = Visibility.Collapsed;
            }
            else
            {
                Visible = Visibility.Visible;
            }
        }
    }
}
