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
    
        public UserPacket(ActionType actionType, JObject data)
        {
            Type = actionType;

            JsonData = data;
        }

        public string Serialize() => JsonConvert.SerializeObject(this);
    
        public enum ActionType
        {
            VoteOnMap,
            PostScore,
            Join,
            Leave
        }
    }
}