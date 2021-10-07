
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserInformation
{
    /// <summary>
    /// ths custom Converter class will manage the different type of users
    /// Currently we only need the ReadJson part.
    /// </summary>
    public class UsersJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Users newUsers = new Users();
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject item = JObject.Load(reader);
                foreach(JToken user in item["value"])
                {
                    if (user["@odata.type"] != null)
                    {
                        switch (user["@odata.type"].Value<string>())
                        {
                            case "#Trippin.Employee":
                                newUsers.Add(JsonConvert.DeserializeObject<Employee>(user.ToString()));
                                break;
                            case "#Trippin.Manager":
                                newUsers.Add(JsonConvert.DeserializeObject<Manager>(user.ToString()));
                                break;
                        }
                    }
                    else
                    {
                        newUsers.Add(JsonConvert.DeserializeObject<User>(user.ToString()));
                    }
                }
            }
            return newUsers;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    [JsonConverter(typeof(UsersJsonConverter))]
    public class Users
    {
        public List<User> ListOfUsers { get; set; } = new List<User>();

        public void Add(User user)
        {
            ListOfUsers.Add(user);
        }
    }

    public class User
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public Genders? Gender { get; set; }
        public byte? Age { get; set; }   //using a byte for age as it is a number between 0-255 and it's unlikely to get anything outside that bracket.
        public List<string> Emails { get; set; }
        public List<string> Features { get; set; } = new List<string>();
        public string FavoriteFeature { get; set; }
        public List<AddressInformation> AddressInfo { get; set; } = new List<AddressInformation>();
        public AddressInformation HomeAdress { get; set; }
    }
    
    public class Manager : User
    {
        public long Budget { get; set; }
    }

    public class Employee : User
    {
        public long Cost { get; set; }
    }

    public class AddressInformation  
    {
        public string Address { get; set; }
        public City City { get; set; }
    }

    public class City
    {
        public string Name { get; set; }
        public string CountryRegion { get; set; }
        public string Region { get; set; }
    }

    public enum Genders
    {
        Male,
        Female,
        Other
    }
}
