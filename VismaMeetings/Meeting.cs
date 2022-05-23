namespace VismaMeetings
{
    public enum Categories { CodeMonkey = 0, Hub = 1, Short = 2, TeamBuilding = 3 }
    public enum Types { Live = 0, InPerson = 1}
    internal class Meeting
    {
        public string Name { get; set; }
        public string ResponsiblePerson { get; set; }
        public string Description { get; set; }
        public Categories Category { get; set; }
        public Types Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Attendee> Attendees {get; set; }
        
        public Meeting(string name, string responsiblePerson, string description, int category, int type, DateTime startDate, DateTime endDate, List<Attendee> attendees)
        {
            Name = name;
            ResponsiblePerson = responsiblePerson;
            Description = description;
            Category = (Categories)category;
            Type = (Types)type;
            StartDate = startDate;
            EndDate = endDate;
            Attendees = attendees;
        }
        public Meeting() { }
        public static Categories[] GetCategories()
        {
            return (Categories[])Enum.GetValues(typeof(Categories));           
        }
        public static Types[] GetTypes()
        {
            return (Types[])Enum.GetValues(typeof(Types));
        }

        public void AddNewPerson(Attendee attendee)
        {
            Attendees.Add(attendee);
        }

        public static string GetFormat()
        {
            return String.Format("|{0,20}|{1,20}|{2,20}|{3,20}|{4,20}|{5,20}|{6,20}|{7,20}", "Name", "Responsible Person", "Description", "Category", "Type", "Start Date", "End Date", "Attendees");
        }
        public override string ToString()
        {
            return String.Format("|{0,20}|{1,20}|{2,20}|{3,20}|{4,20}|{5,20}|{6,20}|{7,20}|", Name, ResponsiblePerson, Description, Category, Type, StartDate.ToString("yyyy/MM/dd H:mm"), EndDate.ToString("yyyy/MM/dd H:mm"), String.Format("{0}", string.Join(" | ", Attendees.Select(a=> a.ToString()))));           
        }

        public bool OverDate(string fro)
        {
            return StartDate > DateTime.Parse(fro) ? true : false;
        }

        internal void RemoveAttendee(string choice)
        {
            foreach(Attendee attendee in Attendees)
            {
                if(attendee.Name == choice)
                {
                    Attendees.Remove(attendee);
                    return;
                }
            }
        }
    }
}
