using API_Manager;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UserInformation;

namespace OData_Jibble_ConsoleOutput
{
    class Program
    {
        public static TrippingKey Key { get; set; } = InitiateTrippingKey();
        public static Users ListOfUsers { get; set; } = new Users();
        /*
         *     Create a C# Console application utilizing public OData API from: https://www.odata.org/odata-services/ (use v4). The app should :-

        -List people
        -Allow searching/filtering people
        -Show details on a specific Person

This assessment will demonstrate your ability to work with libraries and APIs, implementation will be discussed during the technical interview. Bonus points for:

        -Modifying data
        -Advanced techniques

        */
        static void Main(string[] args)
        {
            Console.WriteLine("Select Operation : ");
            Console.WriteLine(" 1 : List All Users");
            Console.WriteLine(" 2 : Look for specific User");

            switch (Int32.Parse(Console.ReadLine()))
            {
                case 1:
                    ListAllUsers();
                    break;
                case 2:
                    FilterUser();
                    break;
            }

            DisplayUsers();
        }

        static TrippingKey InitiateTrippingKey()
        {
            return JsonConvert.DeserializeObject<TrippingKey>(DataRequester.GetTrippingKey());
        }

        static void ListAllUsers()
        {
            ListOfUsers = JsonConvert.DeserializeObject<Users>(DataRequester.ListAllUsers());
        }

        static void FindUserByUsername(string username)
        {
            ListOfUsers = new Users();
            ListOfUsers.Add(JsonConvert.DeserializeObject<User>(DataRequester.GetUserByID(username)));
        }

        static void FilterUser()
        {
            bool GoodValueHasBeenEntered = false;
            while (!GoodValueHasBeenEntered)
            {
                Console.WriteLine("choose on which parameter to filter User : ");
                Console.WriteLine(" 1 : Username");
                Console.WriteLine(" 2 : FirstName");
                Console.WriteLine(" 3 : Adress");

                Console.WriteLine("Enter number to choose : ");
                int value = Int32.Parse(Console.ReadLine());

                switch (value)
                {
                    case 1:
                        Console.WriteLine("specify Username : ");
                        FindUserByUsername(Console.ReadLine());
                        GoodValueHasBeenEntered = true;
                        break;
                    case 2:
                        Console.WriteLine("specify FirstName : ");
                        ListOfUsers = JsonConvert.DeserializeObject<Users>(DataRequester.FilteredQuery("FirstName", DataRequester.FilterOperation.Equal, Console.ReadLine(), Key.Value));
                        GoodValueHasBeenEntered = true;
                        break;
                }
            }
        }

        static void DisplayUsers()
        {
            foreach(User user in ListOfUsers.ListOfUsers)
            {
                Console.WriteLine(JsonConvert.SerializeObject(user, Formatting.Indented));
            }
        }
    }
}
