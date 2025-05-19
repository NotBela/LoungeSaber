using System.Collections.Generic;
using LoungeSaber_Server.Models.Maps;

namespace LoungeSaber.Models.Maps
{
    public class Map
    {
        public Map(string hash, List<MapDifficulty> difficulties)
        {
            Hash = hash;
            Difficulties = difficulties;
        }

        public string Hash { get; }

        public List<MapDifficulty> Difficulties { get; }

        public PlaylistMap GetPlaylistMap() => new PlaylistMap(Hash, Difficulties.ToArray());

        public VotingMap ToVotingMap(MapDifficulty difficulty) => new VotingMap(Hash, difficulty);
    }
}