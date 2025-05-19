using System;
using LoungeSaber_Server.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LoungeSaber.Models.Networking
{
    public class UserPacket
    {
        public readonly ActionType Type;
        public readonly JObject JsonData;
    
        private UserPacket(ActionType actionType, JObject data)
        {
            Type = actionType;

            JsonData = data;
        }

        public static UserPacket Parse(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<UserPacket>(json);
        
            if (deserialized == null) 
                throw new Exception("User action could not be deserialized");
        
            return deserialized;
        }
    
        public enum ActionType
        {
            VoteOnMap,
            PostScore,
            Join,
            Leave
        }
    }
}