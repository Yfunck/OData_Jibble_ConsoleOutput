using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace JSonManagement
{
    //using a static class as there is no point in having this class be instanced
    //we're using the nuget package Newtonsoft to manage the serialization/deserialization of our JSON string
    //in a more complex situation the better solution would be to use a custom JSON converter, as it gives us more control
    //but it is not needed in this situation as the objects we work with are relatively simple.
    public static class SerializationManager
    {
        public static string Serialize()
        {
            return null;
        }

        public static T Deserialize<T>(string Json)
        {
            return JsonConvert.DeserializeObject<T>(Json);
        }
    }
    
}
