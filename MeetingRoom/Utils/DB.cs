using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using MeetingRoom.Models;

namespace MeetingRoom.Utils
{
    internal class DB
    {
        public string ConnString { get; set; }

        public DB(string connString)
        {
            this.ConnString = connString;
        }

        public bool testConnection()
        {
            bool result = false;

            try
            {
                using (SqlConnection dbconn = new SqlConnection(ConnString))
                {
                    try
                    {   
                        dbconn.Open();
                        return true;
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception)
            {
                //not going to make any decent error handling in the scope of this task
                throw;
            }

            return result;
        }

        public List<Reservation> getReservations()
        {
            List<Reservation> reservations = new();

            try
            {
                using (var dbconn = new SqlConnection(ConnString))
                {
                    dbconn.Open();
                    var comm = new SqlCommand("SELECT * FROM reservation;", dbconn);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reservations.Add(new Reservation() { 
                                Id = !reader.IsDBNull(0) ? reader.GetInt32(0) : -1,
                                MeetingRoomId = !reader.IsDBNull(1) ? reader.GetInt32(1) : -1,
                                Host = !reader.IsDBNull(2) ? reader.GetString(2) : "",
                                From = !reader.IsDBNull(3) ? reader.GetDateTime(3) : DateTime.MinValue,
                                To = !reader.IsDBNull(4) ? reader.GetDateTime(4) : DateTime.MinValue
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return reservations;
        }

        public bool makeReservation(Reservation reservation)
        {
            bool result = true;

            try
            {
                using (var dbconn = new SqlConnection(ConnString))
                {
                    try
                    {
                        dbconn.Open();
                    }
                    catch
                    {
                    }
                    var comm = new SqlCommand("INSERT INTO reservation (meeting_room_id, host, time_from, time_to) VALUES (@roomID, @host, @from, @to);", dbconn);
                    comm.Parameters.AddWithValue("@roomID", reservation.MeetingRoomId);
                    comm.Parameters.AddWithValue("@host", reservation.Host);
                    comm.Parameters.AddWithValue("@from", reservation.From);
                    comm.Parameters.AddWithValue("@to", reservation.To);
                    comm.ExecuteScalar();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public bool AddRooms(List<Room> rooms)
        {
            bool result = true;

            try
            {
                using (var dbconn = new SqlConnection(ConnString))
                {
                    dbconn.Open();
                    foreach (var room in rooms)
                    {
                        var comm = new SqlCommand("INSERT INTO meeting_room (name, location, seats) VALUES (@name, @location, @seats);", dbconn);
                        comm.Parameters.AddWithValue("@name", room.Name);
                        comm.Parameters.AddWithValue("@location", room.Location);
                        comm.Parameters.AddWithValue("@seats", room.Seats);
                        comm.ExecuteScalar();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public List<Room> GetRooms()
        {
            List<Room> rooms = new();

            try
            {
                using (var dbconn = new SqlConnection(ConnString))
                {

                    dbconn.Open();
                    var comm = new SqlCommand("SELECT * FROM meeting_room; ", dbconn);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rooms.Add(new Room()
                            {
                                Id = !reader.IsDBNull(0) ? reader.GetInt32(0) : -1,
                                Name = !reader.IsDBNull(1) ? reader.GetString(1) : "",
                                Location = !reader.IsDBNull(2) ? reader.GetString(2) : "",
                                Seats = !reader.IsDBNull(3) ? reader.GetInt32(3) : -1
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return rooms;
        }

        public List<Reservation> FetchReservations(DateTime newDate)
        {
            List<Reservation> reservations = new();

            try
            {
                using (var dbconn = new SqlConnection(ConnString))
                {
                    dbconn.Open();
                    var comm = new SqlCommand("SELECT * FROM reservation WHERE CONVERT(date, time_from) = @from OR CONVERT(date, time_to) = @to;", dbconn);
                    comm.Parameters.AddWithValue("@from", newDate.Date);
                    comm.Parameters.AddWithValue("@to", newDate.Date);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reservations.Add(new Reservation()
                            {
                                Id = !reader.IsDBNull(0) ? reader.GetInt32(0) : -1,
                                MeetingRoomId = !reader.IsDBNull(1) ? reader.GetInt32(1) : -1,
                                Host = !reader.IsDBNull(2) ? reader.GetString(2) : "",
                                From = !reader.IsDBNull(3) ? reader.GetDateTime(3) : DateTime.MinValue,
                                To = !reader.IsDBNull(4) ? reader.GetDateTime(4) : DateTime.MinValue
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return reservations;
        }

        internal bool IsFreeToReserve(DateTime from, DateTime to, int meetingRoomId)
        {
            bool result = true;
            try
            {
                using (var dbconn = new SqlConnection(ConnString))
                {
                    dbconn.Open();
                    var comm = new SqlCommand("SELECT COUNT(*) FROM reservation WHERE meeting_room_id = @roomid AND " +
                        "((time_from < @to AND time_from >= @from) OR " +
                        "(time_to > @from AND time_to <= @to));", dbconn);
                    comm.Parameters.AddWithValue("@from", from);
                    comm.Parameters.AddWithValue("@to", to);
                    comm.Parameters.AddWithValue("@roomid", meetingRoomId);

                    result = (int)comm.ExecuteScalar() == 0;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }
}
