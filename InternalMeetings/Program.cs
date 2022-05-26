using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace InternalMeetings
{
    public class Program
    {
        private static Person _person;

        public static List<Room> RoomsList { get; set; }

        public static void Main(string[] args)
        {
            AskForUsersInfo();
            ReadJson();

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Select operation:");
                Console.WriteLine("1 - create a room");
                Console.WriteLine("2 - add person to a room");
                Console.WriteLine("4 - filter by responsible person");
                Console.WriteLine("5 - filter by category");
                Console.WriteLine("6 - filter by attendees");
                Console.WriteLine("7 - filter by date");
                Console.WriteLine("8 - filter by description");
                Console.WriteLine("0 - exit program");

                if (int.TryParse(Console.ReadLine(), out var button))
                {
                    Console.Clear();
                    switch (button)
                    {

                        case 0: exit = true; break;
                        case 1: CreateRoom(RoomsList); break;
                        case 2: AddPerson(); break;
                        case 3: RemovePerson(); break;
                        case 4: FilterByResponsiblePerson(); break;
                        case 5: FilterByCategory(); break;
                        case 6: FilterByAttendees(); break;
                        case 7: FilterByDate(); break;
                        case 8: FilterByDescription(); break;
                        default: Console.WriteLine("Invalid operation. Try again."); break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Try again.");
                }
            }
        }

        private static void CreateRoom(List<Room> RoomsList)
        {
            Console.Clear();

            DateTime dt;
            string[] answer = new string[10];
            Console.WriteLine("Enter room name:");
            answer[0] = Console.ReadLine();
            Console.WriteLine("Enter room description:");
            answer[1] = Console.ReadLine();
            while (true)
            {
                Console.WriteLine("Choose room category:\n1. CodeMonkey\n2. Hub\n3. Short\n4. TeamBuilding");
                int.TryParse(Console.ReadLine(),out var input);
                if (input == 1)
                {
                    answer[2] = "CodeMonkey";
                    break;
                }
                else if (input == 2)
                {
                    answer[2] = "Hub";
                    break;

                }
                else if (input == 3)
                {
                    answer[2] = "Short";
                    break;
                }
                else if (input == 4)
                {
                    answer[2] = "TeamBuilding";
                    break;
                }
                else
                {
                    Console.WriteLine("Something went wrong\nChoose number from 1 to 4");
                }
            }

            while (true)
            {
                Console.WriteLine("Choose room type:\n1. Live\n2. InPerson");
                int.TryParse(Console.ReadLine(),out var input);
                if (input == 1)
                {
                    answer[3] = "Live";
                    break;
                }
                else if (input == 2)
                {
                    answer[3] = "InPerson";
                    break;
                }
                else
                {
                    Console.WriteLine("Something went wrong\nChoose number from 1 to 2");
                }
            }

            Console.WriteLine("Enter start date of meeting yyyy/MM/dd: ");
            answer[4] = Console.ReadLine();
            while (!DateTime.TryParseExact(answer[4], "yyyy/MM/dd", null, System.Globalization.DateTimeStyles.None, out dt))
            {
                Console.WriteLine("Invalid date, please retry");
                answer[4] = Console.ReadLine();
            }
            Console.WriteLine("Enter end date of meeting yyyy/MM/dd: ");
            answer[5] = Console.ReadLine();
            while (!DateTime.TryParseExact(answer[5], "yyyy/MM/dd", null, System.Globalization.DateTimeStyles.None, out dt))
            {
                Console.WriteLine("Invalid date, please retry");
                answer[5] = Console.ReadLine();
            }

            RoomsList.Add(new Room
            {
                RoomName = answer[0],
                ResponsiblePerson = _person.FirstName,
                Description = answer[1],
                Category = answer[2],
                Type = answer[3],
                StartDate = Convert.ToDateTime(answer[4]),
                EndDate = Convert.ToDateTime(answer[5]),
                Participants = new List<string>()
            });
            /* RoomsList.Add(new Room
             {
                 RoomName = "Jonas",
                 ResponsiblePerson = "Jonas",
                 Description = "Jonas",
                 Category = "Jonas",
                 Type = "Jonas",
                 StartDate = new DateTime(2022/08/27),
                 EndDate = new DateTime(2022/08/29),
             });*/
            RoomsList[RoomsList.Count - 1].Participants.Add(_person.FirstName);
            Console.Clear();

            string strResultJSON = JsonConvert.SerializeObject(RoomsList, Formatting.Indented);
            File.WriteAllText(@"kambarys.json", strResultJSON);
            Console.WriteLine("Stored");
        }

        private static void AskForUsersInfo()
        {
            Console.WriteLine("Enter your name:");
            string input = Console.ReadLine();
            input = char.ToUpper(input[0]) + input.Substring(1);
            _person = new Person(input);
            //Console.WriteLine("Enter Your ID:");
            //person.PersonID = int.Parse(Console.ReadLine());
            Console.Clear();
        }

        private static void RemovePerson()
        {
            int answer;
            for (int i = 0; i < RoomsList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {RoomsList[i].RoomName}");
            }
            Console.WriteLine("Enter rooms number from which you want to remove a person: ");
            answer = int.Parse(Console.ReadLine());
            Console.Clear();
            Console.WriteLine("If you want to go back enter: 0");
            Console.WriteLine("Enter persons name to remove a person from the meeting: ");
            string input = Console.ReadLine();

            input = char.ToUpper(input[0]) + input.Substring(1);
            string rperson = RoomsList[answer - 1].ResponsiblePerson;
            if ((input.Equals(rperson)))
            {
                Console.WriteLine("This person is responsible for the meeting, you can't remove him!");
                return;

            }
            else if (RoomsList[answer - 1].Participants.Contains(input) && input != "0")
            {

                RoomsList[answer - 1].Participants.Remove(input);
                string strResultJSON = JsonConvert.SerializeObject(RoomsList, Formatting.Indented);
                File.WriteAllText(@"kambarys.json", strResultJSON);
                Console.Clear();
                Console.WriteLine($"{input} has been removed");

            }
            else if (input == "0") { return; }

            else
            {
                Console.WriteLine("There isn't such person");
            }
        }

        private static void AddPerson()
        {
            for (int i = 0; i < RoomsList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {RoomsList[i].RoomName}");
            }
            Console.WriteLine("Enter rooms number in which you want to add a person: ");
            int.TryParse(Console.ReadLine(),out var answer);
            Console.Clear();
            Console.WriteLine("If you want to go back enter: 0");
            Console.WriteLine("Enter persons name to add a person to the meeting: ");
            string input = Console.ReadLine();
            input = char.ToUpper(input[0]) + input.Substring(1);


            if (!RoomsList[answer - 1].Participants.Contains(input) && input != "0")
            {
                RoomsList[answer - 1].Participants.Add(input);
                string strResultJSON = JsonConvert.SerializeObject(RoomsList, Formatting.Indented);
                File.WriteAllText(@"kambarys.json", strResultJSON);
                Console.WriteLine("Stored");
                Console.Clear();

            }
            else if (input == "0") { return; }

            else
            {
                Console.WriteLine("You can't add same person twice");
            }
        }

        private static void ReadJson()
        {
            if (File.Exists("kambarys.json"))
            {
            string jsonData = File.ReadAllText("kambarys.json");
            RoomsList = JsonConvert.DeserializeObject<List<Room>>(jsonData);
            }
            else
            {
                File.Create("kambarys.json");
            }
            RoomsList ??= new List<Room>();
        }

        static void FilterByResponsiblePerson()
        {
            Console.Clear();
            Console.WriteLine("Enter name of the responsible person you want to filter by:");
            string input = Console.ReadLine();
            input = char.ToUpper(input[0]) + input.Substring(1);
            for (int i = 0; i < RoomsList.Count; i++)
            {
                if (input.Equals(RoomsList[i].ResponsiblePerson))
                {
                    Console.WriteLine(RoomsList[i].RoomName);
                }
                else Console.WriteLine("Such person is not responsible for any room");
            }
        }

        private static void FilterByCategory()
        {
            Console.Clear();

            string category;
            Console.WriteLine("Choose category you want to filter by:");
            while (true)
            {
                Console.WriteLine("Choose room category:\n1. CodeMonkey\n2. Hub\n3. Short\n4. TeamBuilding");
                int.TryParse(Console.ReadLine(),out var input);
                if (input == 1)
                {
                    category = "CodeMonkey";
                    break;
                }
                else if (input == 2)
                {
                    category = "Hub";
                    break;

                }
                else if (input == 3)
                {
                    category = "Short";
                    break;
                }
                else if (input == 4)
                {
                    category = "TeamBuilding";
                    break;
                }
                else
                {
                    Console.WriteLine("Something went wrong\nChoose number from 1 to 4");
                }
            }
            for (int i = 0; i < RoomsList.Count; i++)
            {
                if (category.Equals(RoomsList[i].Category))
                    Console.WriteLine(RoomsList[i].RoomName);

            }
        }

        private static void FilterByType()
        {
            Console.Clear();
            string type;
            while (true)
            {
                Console.WriteLine("Choose room type you want to filter by:\n1. Live\n2. InPerson");
                int.TryParse(Console.ReadLine(),out var input);
                if (input == 1)
                {
                    type = "Live";
                    break;
                }
                else if (input == 2)
                {
                    type = "InPerson";
                    break;
                }
                else
                {
                    Console.WriteLine("Something went wrong\nChoose number from 1 to 2");
                }
            }

            for (int i = 0; i < RoomsList.Count; i++)
            {
                if (type.Equals(RoomsList[i].Type))
                {
                    Console.WriteLine(RoomsList[i].RoomName);
                }
            }
        }

        private static void FilterByDate()
        {
            Console.Clear();
            DateTime dt, dt2;
            string answer;
            Console.WriteLine("Enter start date of meeting yyyy/MM/dd: ");
            answer = Console.ReadLine();
            while (!DateTime.TryParseExact(answer, "yyyy/MM/dd", null, System.Globalization.DateTimeStyles.None, out dt))
            {
                Console.WriteLine("Invalid date, please retry");
                answer = Console.ReadLine();
            }
            Console.WriteLine("Enter end date of meeting yyyy/MM/dd: ");
            answer = Console.ReadLine();
            while (!DateTime.TryParseExact(answer, "yyyy/MM/dd", null, System.Globalization.DateTimeStyles.None, out dt2))
            {
                Console.WriteLine("Invalid date, please retry");
                answer = Console.ReadLine();
            }

            for (int i = 0; i < RoomsList.Count; i++)
            {
                if (RoomsList[i].StartDate >= dt && RoomsList[i].EndDate <= dt2)
                    Console.WriteLine(RoomsList[i].RoomName);
            }
        }

        private static void FilterByDescription()
        {
            Console.Clear();
            string input;
            Console.WriteLine("Enter description words you want to filter by:");
            input = Console.ReadLine();
            for (int i = 0; i < RoomsList.Count; i++)
            {
                if (input.Any(RoomsList[i].Description.Contains))
                    Console.WriteLine(RoomsList[i].RoomName);
            }
        }

        private static void FilterByAttendees()
        {
            Console.Clear();

            Console.WriteLine("Filter by the number of attendees: ");
            int.TryParse(Console.ReadLine(),out var input);
            Console.WriteLine($"Press 1 if you want to filter meeting that have over {input} people atending");
            Console.WriteLine($"Press 2 if you want to filter meeting that have less than {input} people atending");
            int.TryParse(Console.ReadLine(),out var index);
            if (index == 1)
            {
                for (int i = 0; i < RoomsList.Count; i++)
                {
                    if (RoomsList[i].Participants.Count >= input)
                        Console.WriteLine(RoomsList[i].RoomName);
                }
            }
            else if (index == 2)
            {
                for (int i = 0; i < RoomsList.Count; i++)
                {
                    if (RoomsList[i].Participants.Count <= input)
                        Console.WriteLine(RoomsList[i].RoomName);
                }
            }
            else
            {
                Console.WriteLine("Something went wrong");
                FilterByAttendees();
            }
        }
    }
}
