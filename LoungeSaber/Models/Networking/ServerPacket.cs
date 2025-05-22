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

        public static ServerPacket Deserialize(string json) => JsonConvert.DeserializeObject<ServerPacket>(json);

        public enum ActionType
        {
            StartMatch,
            OpponentVoted,
            CreateMatch,
            MatchEnded,
            StartWarning,
            Results,
            UpdateConnectedUserCount,
            Disconnect
        }
    }
}