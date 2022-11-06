using System;
using System.ComponentModel;

namespace MeetingRoom.Models
{
    public class TimeSlot : INotifyPropertyChanged
    {
        private bool reserved = false;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public DateTime StartTime { get; set; }
        public string StartTimeString
        {
            get
            {
                return $"{StartTime.ToString("HH:mm")} - {EndTime.ToString("HH:mm")}";
            }
        }
        public DateTime EndTime { get; set; }
        public bool Reserved { get => reserved; set { reserved = value; OnPropertyChanged(nameof(Reserved)); } }
        public int RoomId { get; set; }
        public int? ReservationId { get; set; }
    }
}
