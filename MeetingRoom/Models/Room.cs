using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MeetingRoom.Models
{
    public class Room : INotifyPropertyChanged
    {
        private List<TimeSlot> timeSlots;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public int Id { get; set; }
        public int Seats { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public List<TimeSlot> TimeSlots { get => timeSlots; set { timeSlots = value; OnPropertyChanged(nameof(TimeSlots)); } }

        internal void CreateTimeSlots(DateTime date)
        {
            TimeSlots = new();
            DateTime tempDate = date.Date;
            while (tempDate.Date == date.Date)
            {
                TimeSlots.Add(new TimeSlot()
                {
                    StartTime = tempDate,
                    EndTime = tempDate.AddMinutes(30),
                    ReservationId = -1,
                    Reserved = false,
                    RoomId = Id
                });
                tempDate = tempDate.AddMinutes(30);
            }

        }
    }
}
