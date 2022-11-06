using MeetingRoom.Extensions;
using MeetingRoom.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace MeetingRoom.ViewModels
{
    internal class ReservationsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public readonly DateTime MinDate = DateTime.Now.Date;
        private DateTime selectedDate = DateTime.Now.Date;
        private List<Room> rooms = new();

        public List<Room> Rooms
        {
            get { return rooms; }
            set { rooms = value;
                OnPropertyChanged(nameof(Rooms));
            }
        }


        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set 
            {
                SelectedDateChanged(selectedDate.Date, value.Date);
                selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        private void SelectedDateChanged(DateTime oldDate, DateTime newDate)
        {
            if (Parent?.DBHandler == null)
            {
                return;
            }

            if (oldDate != newDate)
            {
                List<Reservation> reservations = Parent.DBHandler.FetchReservations(newDate);
                RecreateReservationTable(newDate, reservations);
            }
        }

        public ICommand CreateNewReservationCommand
        {
            get 
            {
                return new RelayCommand(r => OpenReservationOverlay(r), r=> ReservationCanBeMade(r));
            }
        }

        private void OpenReservationOverlay(object r)
        {
            if (r is TimeSlot)
            {
                TimeSlot timeSlot = (TimeSlot)r;

                Parent.NewReservation = new NewReservationViewModel(Parent);
                Parent.NewReservation.NewReservation = new Reservation() { From = timeSlot.StartTime, To = timeSlot.EndTime, MeetingRoomId = timeSlot.RoomId, MeetingRoom = Rooms.FirstOrDefault(x => x.Id == timeSlot.RoomId) };
                Parent.NewReservation.ChangeVisibility();
            }
        }

        private bool ReservationCanBeMade(object r)
        {
            bool result = false;
            if (r is TimeSlot)
            {
                TimeSlot slot = (TimeSlot)r;
                if (!slot.Reserved)
                {
                    result = true;
                }
            }

            return result;
        }

        private void RecreateReservationTable(DateTime date, List<Reservation> reservations)
        {
            if (Parent?.DBHandler == null)
            {
                return;
            }

            if (Rooms.Count == 0)
            {
                Rooms = Parent.DBHandler.GetRooms();
            }

            foreach (var room in Rooms)
            {
                room.CreateTimeSlots(date);
            }

            foreach (var reservation in reservations)
            {
                foreach (var timeSlot in Rooms.First(x=>x.Id == reservation.MeetingRoomId).TimeSlots.Where(
                    z=> (z.StartTime < reservation.To && z.StartTime >= reservation.From) || (z.EndTime > reservation.From && z.EndTime <= reservation.To)))
                {
                    timeSlot.Reserved = true;
                    timeSlot.ReservationId = reservation.Id;
                }
            }

            OnPropertyChanged(nameof(Rooms));
        }

        public void InitializeReservationTable()
        {
            if (Parent?.DBHandler == null)
            {
                return;
            }

            List<Reservation> reservations = Parent.DBHandler.FetchReservations(SelectedDate);
            RecreateReservationTable(SelectedDate, reservations);
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private MainWindowViewModel? Parent { get; set; }
        public ReservationsViewModel()
        {

        }
        public ReservationsViewModel(MainWindowViewModel parent)
        {
            Parent = parent;
        }
    }
}
