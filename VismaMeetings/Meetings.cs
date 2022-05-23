using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VismaMeetings
{
    internal class Meetings
    {
        public delegate bool Filter(Meeting meeting, object? condition);

        List<Meeting> meetings;
        public Meetings(string input)
        {
            ReadFromJson(@"json.txt");
        }
        public string ListAllMeetings(Filter? filter = null, object? condition = null)
        {            
            if(meetings.Count == 0)
            {
                return "No booked meetings";
            }
            string format = Meeting.GetFormat();
            string result = String.Format("{0}\n{1}\n{0}\n",new string('-', format.Length), format);           
            foreach(Meeting meeting in meetings)
            {
                if (filter != null)
                {
                    if (!filter(meeting, condition))
                    {
                        continue;
                    }
                }
                result += meeting.ToString() + '\n';
            }
            result += new string('-', format.Length) + '\n';
            return result;
        }
        public static bool FilterDescription(Meeting meeting, object? description)
        {
            string? descp = description as string;
            if (meeting.Description == null || descp == null)
            {
                return false;
            }            
            return meeting.Description.Contains(descp);
        }
        internal static bool FilterResponsiblePerson(Meeting meeting, object? responsiblePerson)
        {
            string? respon = responsiblePerson as string;
            if (meeting.ResponsiblePerson == null || respon == null)
            {
                return false;
            }
            return meeting.ResponsiblePerson.Contains(respon);
        }
        internal static bool FilterCategory(Meeting meeting, object? category)
        {
            string? cat = category as string;
            if (meeting.Category == null || cat == null)
            {
                return false;
            }
            return meeting.Category.ToString().Contains(cat);
        }
        internal static bool FilterType(Meeting meeting, object? type)
        {
            string? typ = type as string;
            if (typ == null)
            {
                return false;
            }
            return meeting.Type.ToString().Contains(typ);
        }
        internal static bool FilterDate(Meeting meeting, object? from)
        {
            string? fro = from as string;
            if (fro == null)
            {
                return false;
            }
            return meeting.OverDate(fro);
        }
        internal static bool FilterNumberOfAttendees(Meeting meeting, object? attendees)
        {
            string? fro = attendees as string;
            if (fro == null)
            {
                return false;
            }
            return meeting.Attendees.Count >= Convert.ToInt32(attendees) ? true : false;
        }

        public void RemovePersonFromMeeting(string choice, string name)
        {
            var list = meetings.Where(s => s.Name == choice);
            var meeting = list.First();
            if (meeting.ResponsiblePerson == name)
            {
                Console.WriteLine("You cannot remove yourself from the meeting if you are responsible for it");
                return;
            }
            meeting.RemoveAttendee(choice);
        }

        public DateTime GetMeetingsTime(string choice)
        {
            var list = meetings.Where(s => s.Name.Contains(choice)).Select(s => s.StartDate);
            return list.First();
        }
        internal void AddPersonToMeeting(string choice, Attendee attendee)
        {
            var list = meetings.Where(s => s.Name == choice);
            list.First().AddNewPerson(attendee);
            WriteToJson(@"json.txt");
        }
        internal void CreateMeeting(string name, string responsiblePerson, string description,  int category, int type, DateTime startDate, DateTime endDate, List<Attendee> attendees)
        {
            meetings.Add(new Meeting(name, responsiblePerson, description, category, type, startDate, endDate, attendees));
            WriteToJson(@"json.txt");
            Console.Clear();
        }
        private List<Meeting> ReadFromJson(string fileName)
        {
            meetings = new List<Meeting>();
            string jsonString;

            if (!File.Exists(fileName))
            {
                File.Create(fileName).Close();
            }

            jsonString = File.ReadAllText(fileName);

            if (jsonString != "")
            {
                meetings = JsonSerializer.Deserialize<List<Meeting>>(jsonString);
            }
            return meetings;
        }

        internal void DeleteMeeting(string name, string? choice)
        {
            var meeting = meetings.Find(m => m.Name == choice);
            
            if(meeting.ResponsiblePerson != name)
            {
                Console.WriteLine("You cannot remove this meeting because you are not responsible for it");
            }
            meetings.Remove(meeting);
            WriteToJson(@"json.txt");
            Console.WriteLine("Meeting succesfully removed");
        }

        public int getCount()
        {
            return meetings.Count;
        }

        private void WriteToJson(string fileName)
        {
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Close();
            }
            string jsonString = JsonSerializer.Serialize(meetings);
            File.WriteAllText(fileName, jsonString);
        }
    }
}
