using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace API_Manager
{
    /// <summary>
    /// Used to obtain and store the key needed for data manipulation.
    /// </summary>
    public class TrippingKey
    {
        [JsonProperty("@odata.context")]
        public string Context { get; set; }
        private string RemoveStringA { get; set; } = @"https://services.odata.org/TripPinRESTierService/";
        private string RemoveStringB { get; set; } = @"/$metadata";
        private string _value = null;
        [JsonIgnore]
        public string Value
        {
            get
            {
                if (string.IsNullOrEmpty(_value))
                {
                    _value = Context.Remove(Context.IndexOf(RemoveStringA), RemoveStringA.Length);
                    _value = _value.Remove(_value.IndexOf(RemoveStringB));
                }
                return _value;
            }
        }
    }

    public static class DataRequester
    {
        public enum FilterOperation
        {
            Equal,
            Contains
        }

        public static string GetTrippingKey()
        {
            string url = @"https://services.odata.org/TripPinRESTierService";

            return GetHTTPResponse(url).Result;
        }

        #region simpleRequests
        /// <summary>
        /// List all users in the dataset
        /// </summary>
        /// <returns>a JSON formatted string</returns>
        public static string ListAllUsers(string key = "")
        {
            string url = @"https://services.odata.org/TripPinRESTierService/"+key+"/People";

            return GetHTTPResponse(url).Result;
        }

        /// <summary>
        /// Get an user using his username
        /// </summary>
        /// <param name="UserId">username</param>
        /// <returns>a JSON formatted string</returns>
        public static string GetUserByID(string UserId, string key = "")
        {
            string url = @"https://services.odata.org/TripPinRESTierService/" + key + "/People('" + UserId+"')";

            return GetHTTPResponse(url).Result;
        }
        #endregion

        #region queries
        public static string FilteredQuery(string PropertyLookedAgainst, FilterOperation operation, string ValueWeLookFor, string key = "")
        {
            string filter = "";
            switch (operation)
            {
                case (FilterOperation.Equal):
                    filter = "?$filter="+PropertyLookedAgainst+" eq '"+ValueWeLookFor+"'";
                    break;
                case (FilterOperation.Contains):
                    filter = "?$filter=contains(" + PropertyLookedAgainst + ",'" + ValueWeLookFor + "')";
                    break;
            }
            string url = @"https://services.odata.org/TripPinRESTierService/" + key + "/People"+filter;

            return GetHTTPResponse(url).Result;
        }
        #endregion
        /// <summary>
        /// This method will Request the specified URL
        /// </summary>
        /// <param name="clientBaseAdress">The URL to request</param>
        /// <returns>a JSON format string or null</returns>
        private static async Task<string> GetHTTPResponse(string clientBaseAdress)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(clientBaseAdress);

                HttpResponseMessage response = await client.GetAsync(clientBaseAdress);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
            return null;
        }
    }
}
