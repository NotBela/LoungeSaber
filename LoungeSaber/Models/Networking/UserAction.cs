using System;
using LoungeSaber_Server.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LoungeSaber.Models.Networking
{
    public class UserAction
    {
        public readonly ActionType Type;
        public readonly JObject JsonData;
    
        private UserAction(ActionType actionType, JObject data)
        {
            Type = actionType;

            JsonData = data;
        }

        public static UserAction Parse(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<UserAction>(json);
        
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