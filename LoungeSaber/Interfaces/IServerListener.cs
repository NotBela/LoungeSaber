using LoungeSaber_Server.Models.Packets.ServerPackets;
using LoungeSaber.Models.Packets;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Models.Packets.ServerPackets.Event;
using LoungeSaber.Models.Packets.ServerPackets.Match;

namespace LoungeSaber.Interfaces;

public interface IServerListener
{
    public event Action<MatchCreatedPacket> OnMatchCreated;
    public event Action<OpponentVoted> OnOpponentVoted;
    public event Action<MatchStarted> OnMatchStarting;
    public event Action<MatchResultsPacket> OnMatchResults;
    
    public event Action OnDisconnected;
    public event Action OnConnected;
    public event Action<PrematureMatchEnd> OnPrematureMatchEnd;

    public event Action<EventMatchCreatedPacket> OnEventMatchStarted;
    
    public event Action<EventStartedPacket> OnEventStarted;

    public Task Connect(string queue, Action<JoinResponse> onConnectedCallback);

    public Task SendPacket(UserPacket packet);

    public void Disconnect();
}