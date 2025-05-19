using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LoungeSaber.Models.Networking
{
    public class ServerPacket
    {
        public ServerPacket(ServerPacket.ActionType actionType, JObject data)
        {
            Data = data;
            Type = actionType;
        }

        public JObject Data { get; set; }
        public ActionType Type { get; set; }

        public string Serialize() => JsonConvert.SerializeObject(this);

        public enum ActionType
        {
            StartMatch,
            OpponentVoted,
            CreateMatch,
            MatchEnded,
            StartWarning,
            Results,
            UpdateConnectedUserCount
        }
    }
}