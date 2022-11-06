using System;

namespace MeetingRoom.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int MeetingRoomId { get; set; }
        public string Host { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Room MeetingRoom { get; set; }
    }
}
