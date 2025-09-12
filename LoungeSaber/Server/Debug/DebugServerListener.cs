using LoungeSaber_Server.Models.Packets.ServerPackets;
using LoungeSaber.Interfaces;
using LoungeSaber.Models.Packets;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Models.Packets.ServerPackets.Match;

namespace LoungeSaber.Server.Debug;

public class DebugServerListener : IServerListener
{
    public event Action<MatchCreatedPacket> OnMatchCreated;
    public event Action<OpponentVoted> OnOpponentVoted;
    public event Action<MatchStarted> OnMatchStarting;
    public event Action<MatchResultsPacket> OnMatchResults;
    public event Action OnDisconnected;
    public event Action OnConnected;
    public event Action<PrematureMatchEnd> OnPrematureMatchEnd;
    
    public Task Connect(Action<JoinResponse> onConnectedCallback)
    {
        throw new NotImplementedException();
    }

    public Task SendPacket(UserPacket packet)
    {
        throw new NotImplementedException();
    }

    public void Disconnect()
    {
        throw new NotImplementedException();
    }
}