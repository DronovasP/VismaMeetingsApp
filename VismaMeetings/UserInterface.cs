using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VismaMeetings
{
    internal class UserInterface
    {
        private Meetings meetings;
        public UserInterface(){
            meetings = new Meetings(@"json.txt");
        }
        public bool MainMenu()
        {
            MainMenuText();
            string? input = Console.ReadLine();
            switch (input)
            {
                case "exit":
                    return false;
                case "0":
                    Console.Clear();                    
                    ListMenu();
                    return true;
                case "1":
                    Console.Clear();
                    NewMeetingMenu();
                    return true;
                case "2":
                    Console.Clear();
                    DeleteMeetingMenu();
                    return true;
                case "3":
                    Console.Clear();
                    AddPersonToMeetingMenu();
                    return true;
                case "4":
                    Console.Clear();
                    RemovePersonFromMeetingMenu();
                    return true;
                default:
                    if (input == null)
                    {
                        Console.WriteLine("Enter the number of the action");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Not a valid number");
                    }
                    return true;
            }
        }
        public void MainMenuText()
        {
            Console.WriteLine("Enter the number to choose your action");
            Console.WriteLine("Possible actions:");
            Console.WriteLine("0 - List all meetings");
            Console.WriteLine("1 - Create new meeting");
            Console.WriteLine("2 - Delete meeting");
            Console.WriteLine("3 - Add a person to the meeting");
            Console.WriteLine("4 - Remove a person from a meeting\n");
            Console.WriteLine("Enter exit to quit the program");
        }
        private void NewMeetingMenu()
        {
            Console.WriteLine("Enter the name of the meeting:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter the responsible person of the meeting:");
            string responsiblePerson = Console.ReadLine();
            Console.WriteLine("Enter the description of the meeting:");
            string description = Console.ReadLine();
            Console.WriteLine("Enter the category of the meeting:\n Possible categories: {0}", String.Join("/", Meeting.GetCategories()));
            int category = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the type of the meeting:\n Possible types: Live / InPerson");
            int type = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the start date of the meeting:\n Format: 2000-01-01 15:15");
            DateTime startDate = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Enter the end date of the meeting:\n Format: 2000-01-01 15:30");
            DateTime endDate = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Enter the number of attendees:");
            int n = int.Parse(Console.ReadLine());
            List<Attendee> attendees = new List<Attendee>();
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine("Enter the name of" + i+1 + " attendee");
                string attendee = Console.ReadLine();
                Console.WriteLine("Enter the time when the" + i+1 + " attendee will join (0 if when the meeting starts)");
                string option = Console.ReadLine();
                DateTime time;
                if (option == "0")
                    time = startDate;
                else
                    time = DateTime.Parse(option);
                attendees.Add(new Attendee(attendee,time));
            }
            meetings.CreateMeeting(name, responsiblePerson, description, category, type, startDate, endDate, attendees);
        }
        private void DeleteMeetingMenu()
        {
            Console.WriteLine("State your full name:");
            string name = Console.ReadLine();            
            if (name == "" || name.Any(char.IsDigit))
            {
                Console.WriteLine("Name cannot be empty or with numbers. Try again.");
                return;
            }

            if (meetings.getCount() == 0)
            {
                Console.WriteLine(meetings.ListAllMeetings());
                return;
            }
            Console.WriteLine(meetings.ListAllMeetings());
            Console.WriteLine("Which meeting would you like to delete? Enter the name of the meeting");
            string choice = Console.ReadLine();
            meetings.DeleteMeeting(name, choice);
        }
        private void AddPersonToMeetingMenu()
        {
            //Missing implementation for: prevention of adding the same person, no duplication ensurance. Possible additions: cannot add a person with a time that is past the meeting time
            Console.Clear();
            if (meetings.getCount() == 0)
            {
                Console.WriteLine(meetings.ListAllMeetings());
                return;
            }
            Console.WriteLine(meetings.ListAllMeetings());
            Console.WriteLine("Which meeting should a person be added to? Enter the name of the meeting");
            string choice = Console.ReadLine();
            if (choice != String.Empty)
            {                
                Console.WriteLine("State the full name of the attendee");
                string attendee = Console.ReadLine();
                Console.WriteLine("Enter the time when "+ attendee + " will join (0 if when the meeting starts)");
                string option = Console.ReadLine();
                DateTime time;
                if (option == "0")
                    time = meetings.GetMeetingsTime(choice);
                else
                    time = DateTime.Parse(option);
                meetings.AddPersonToMeeting(choice, new Attendee(attendee, time));                
            }
        }
        private void RemovePersonFromMeetingMenu()
        {
            Console.Clear();
            if (meetings.getCount() == 0)
            {
                Console.WriteLine(meetings.ListAllMeetings());
                return;
            }
            Console.WriteLine("State your full name");
            string name = Console.ReadLine();
            Console.WriteLine(meetings.ListAllMeetings());
            Console.WriteLine("Choose the meeting you want to remove the person from. Enter the name of the meeting");
            string choice = Console.ReadLine();
            meetings.RemovePersonFromMeeting(choice, name);
        }
        public void ListMenu()
        {
            ListMenuText();
            string? result = null, condition = null;
            string? caseInput = Console.ReadLine();           
            switch (caseInput)
            {
                case "back":
                    Console.Clear();
                    break;
                case "0": case null:
                    Console.Clear();
                    result = meetings.ListAllMeetings();
                    Console.WriteLine(result);
                    break;
                case "1":
                    Console.Clear();
                    Console.WriteLine("Enter description:");
                    condition = Console.ReadLine();
                    result = meetings.ListAllMeetings(Meetings.FilterDescription, condition);
                    Console.WriteLine(result);
                    break;
                case "2":
                    Console.Clear();
                    Console.WriteLine("Enter responsible person:");
                    condition = Console.ReadLine();
                    result = meetings.ListAllMeetings(Meetings.FilterResponsiblePerson, condition);
                    Console.WriteLine(result);
                    break;
                case "3":
                    Console.Clear();
                    Console.WriteLine("Enter the category:");
                    Categories[] categories = Meeting.GetCategories();
                    Console.WriteLine(String.Format("Possible categories: {0}", String.Join("/", categories)));
                    condition = Console.ReadLine();
                    bool categoryExists = false;
                    for(int i=0; i<categories.Length; i++)
                    {
                        string str = string.Join('"', categories[i]);
                        if (str.Contains(condition))
                        {
                            categoryExists = true;
                        }
                    }
                    if (categoryExists)
                    {
                        result = meetings.ListAllMeetings(Meetings.FilterCategory, condition);
                        Console.WriteLine(result);
                    }
                    else
                        Console.WriteLine("No such category");
                    break;
                case "4":
                    Console.Clear();
                    Console.WriteLine("Enter type:");
                    Types[] types = Meeting.GetTypes();
                    Console.WriteLine(String.Format("Possible types: {0}", String.Join("/", types)));
                    condition = Console.ReadLine();
                    bool typeExists = false;
                    for (int i = 0; i < types.Length; i++)
                    {
                        string str = string.Join('"', types[i]);
                        if (str.Contains(condition))
                        {
                            typeExists = true;
                        }
                    }
                    if (typeExists)
                    {
                        result = meetings.ListAllMeetings(Meetings.FilterType, condition);
                        Console.WriteLine(result);
                    }
                    else
                        Console.WriteLine("No such category");
                    break;
                case "5":
                    //Missing implementation: Cannot choose to date and from to at once
                    Console.Clear();
                    Console.WriteLine("Enter the time from which to filter: ");
                    caseInput = Console.ReadLine();
                    result = meetings.ListAllMeetings(Meetings.FilterDate, caseInput);
                    Console.WriteLine(result);
                    break;
                case "6":
                    Console.Clear();
                    Console.WriteLine("Enter from what number of attendees to filter");
                    caseInput = Console.ReadLine();
                    result = meetings.ListAllMeetings(Meetings.FilterNumberOfAttendees, caseInput);
                    Console.WriteLine(result);
                    break;
                default:
                    break;
            }           
        }
        public void ListMenuText()
        {
            Console.WriteLine("Enter the number to choose how to filter the data");
            Console.WriteLine("Possible filters:");
            Console.WriteLine("0 - No filter");
            Console.WriteLine("1 - Filter by description");
            Console.WriteLine("2 - Filter by responsible person");
            Console.WriteLine("3 - Filter by category");
            Console.WriteLine("4 - Filter by type");
            Console.WriteLine("5 - Filter by dates");
            Console.WriteLine("6 - Filter by the number of attendees\n");
            Console.WriteLine("Enter back to go back to the list of actions");
        }

    }
}
