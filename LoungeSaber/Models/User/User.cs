using JetBrains.Annotations;

namespace LoungeSaber.Models.User
{
    public class User
    {
        public User(string id, int mmr, [CanBeNull] string discordId = null)
        {
            ID = id;
            MMR = mmr;
            DiscordId = discordId;
        }

        public string ID { get; private set; }
        public int MMR { get; private set; }

        [CanBeNull] public string DiscordId { get; private set; }
    }
}