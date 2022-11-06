using MeetingRoom.Extensions;
using MeetingRoom.Models;
using MeetingRoom.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace MeetingRoom.ViewModels
{
    internal class EnvViewModel : INotifyPropertyChanged
    {
        private string xMLPathText = "";
        private string connStringText = "Data Source=localhost;Initial Catalog=meeting_rooms;Integrated Security=True";
        private bool dBhasRooms = false;
        private bool dBconnOk = false;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public string ConnStringText { get => connStringText; set { connStringText = value; OnPropertyChanged(nameof(ConnStringText)); OnPropertyChanged(nameof(CanStartDbConn)); } }
        public string XMLPathText { get => xMLPathText; set { xMLPathText = value; OnPropertyChanged(nameof(XMLPathText)); } }
        public bool DBhasRooms { get => dBhasRooms; set { dBhasRooms = value; OnPropertyChanged(nameof(DBhasRooms)); } }
        public bool DBconnOk { get => dBconnOk; set { dBconnOk = value; OnPropertyChanged(nameof(DBconnOk)); } }
        MainWindowViewModel? Parent { get; set; }
        public EnvViewModel()
        {

        }
        public EnvViewModel(MainWindowViewModel parent)
        {
            Parent = parent;
        }

        public ICommand ReadXML
        {
            get
            {
                return new RelayCommand(r => ReadXMLFile(), r => CanReadXmlFile());
            }
        }

        private void ReadXMLFile()
        {
            if (string.IsNullOrEmpty(XMLPathText) || !File.Exists(XMLPathText))
            {
                XMLPathText = FileUtils.OpenFileBrowser();
            }

            if (!string.IsNullOrEmpty(XMLPathText))
            {
                List<Room> rooms = FileUtils.ParseRoomXml(XMLPathText);
                List<Room> existingRooms = Parent.DBHandler.GetRooms();
                if (existingRooms?.Any() == true)
                {
                    Parent.DBHandler.AddRooms(rooms.Where(x=>!existingRooms.Any(y=>y.Seats == x.Seats && y.Name==x.Name && y.Location==x.Location)).ToList());
                }
                else
                {
                    Parent.DBHandler.AddRooms(rooms);
                }
                
            }
        }

        public bool CanReadXmlFile()
        {
            return DBconnOk;
        }

        public ICommand EstablishDBConnection
        {
            get 
            {
                return new RelayCommand(r => StartAndTestDbConnection(), r=>CanStartDbConn());
            }
            
        }

        private void StartAndTestDbConnection()
        {
            DB testConn = new DB(ConnStringText);
            DBconnOk = testConn.testConnection();

            if (DBconnOk)
            {
                Parent.DBHandler = testConn;
                if (Parent.Reservations != null)
                {
                    Parent.Reservations.InitializeReservationTable();
                }
            }
        }

        public bool CanStartDbConn()
        {
            return !string.IsNullOrEmpty(ConnStringText);
        }
    }
}
