using MeetingRoom.Models;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace MeetingRoom.Utils
{
    internal static class FileUtils
    {
        public static void ReadXMLToDb(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }
            
            FileInfo file = new FileInfo(filePath);

            if (file.Extension != "xml")
            {
                return;
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(filePath);


        }

        public static string OpenFileBrowser()
        {
            string filepath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                filepath = openFileDialog.FileName;
            }

            return filepath;
        }

        internal static List<Room> ParseRoomXml(string path)
        {
            List<Room> roomList = new List<Room>();

            try
            {
                var xmldoc = XElement.Load(path);
                var rooms = xmldoc.Elements("meeting_room");
                foreach (var room in rooms)
                {
                    string loc = room.Attribute("location") != null ? room.Attribute("location").Value : "";
                    string name = room.Attribute("name") != null ? room.Attribute("name").Value : "";
                    int seats = int.TryParse(room.Attribute("seats")?.Value, out int parsed) ? parsed : 0;
                    roomList.Add(new Room() { Location = loc, Name = name, Seats = seats});
                }
            }
            catch
            {
                //not bothered with error handling for these extra features.
            }

            return roomList;
        }
    }
}
