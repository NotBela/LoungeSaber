using LoungeSaber_Server.Models.Packets.ServerPackets;
using LoungeSaber.Interfaces;
using LoungeSaber.Models.Packets;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Models.Packets.ServerPackets.Event;
using LoungeSaber.Models.Packets.ServerPackets.Match;
using LoungeSaber.Models.Packets.UserPackets;
using SiraUtil.Logging;
using Zenject;

namespace LoungeSaber.Server.Debug;

public class DebugServerListener : IServerListener
{
    [Inject] private readonly SiraLog _siraLog = null;
    
    public event Action<MatchCreatedPacket> OnMatchCreated;
    public event Action<OpponentVoted> OnOpponentVoted;
    public event Action<MatchStarted> OnMatchStarting;
    public event Action<MatchResultsPacket> OnMatchResults;
    
    public event Action<OutOfEventPacket> OnOutOfEvent;
    public event Action OnDisconnected;
    public event Action OnConnected;
    public event Action<PrematureMatchEnd> OnPrematureMatchEnd;
    
    public event Action<EventStartedPacket> OnEventStarted;

    private bool _isConnected;
    
    public async Task Connect(string queue, Action<JoinResponse> onConnectedCallback)
    {
        await Task.Delay(1000);

        _isConnected = true;
        
        onConnectedCallback?.Invoke(new JoinResponse(true, ""));
        OnConnected?.Invoke();
        _siraLog.Info("connected");

        await Task.Delay(1000);
        await SendPacket(new JoinRequestPacket(DebugApi.Self.Username, DebugApi.Self.UserId, queue));
    }

    public async Task SendPacket(UserPacket packet)
    {
        if (!_isConnected)
        {
            _siraLog.Info("tried to send packet when not connected!");
            return;
        }

        switch (packet.PacketType)
        {
            case UserPacket.UserPacketTypes.JoinRequest:
                if (((JoinRequestPacket)packet).Queue == "test")
                {
                    await Task.Delay(5000);
                    OnEventStarted?.Invoke(new EventStartedPacket());

                    await Task.Delay(5000);
                    OnMatchStarting?.Invoke(new MatchStarted(DebugApi.Maps[0], 15,
                        10, DebugApi.DebugOpponent));
                    return;
                }

                await Task.Delay(1000);
                OnMatchCreated?.Invoke(new MatchCreatedPacket(DebugApi.Maps, DebugApi.DebugOpponent));
                _siraLog.Info("join request");
                break;
            case UserPacket.UserPacketTypes.Vote:
                OnOpponentVoted?.Invoke(new OpponentVoted(0));
                await Task.Delay(1000);
                OnMatchStarting?.Invoke(new MatchStarted(DebugApi.Maps[0], 15,
                    10, DebugApi.DebugOpponent));
                _siraLog.Info("voted");
                break;
            case UserPacket.UserPacketTypes.ScoreSubmission:
                _siraLog.Info("score submitted");
                await Task.Delay(1000);
                OnMatchResults?.Invoke(new MatchResultsPacket(new MatchScore(DebugApi.Self, new Score(100000, 1f, true, 0, true)),
                    new MatchScore(DebugApi.DebugOpponent, Score.Empty), 100));

                _siraLog.Info("match results invoked");
                break;
            default:
                throw new NotImplementedException();
        }
    }

    public void Disconnect()
    {
        if (!_isConnected) return;
        
        OnDisconnected?.Invoke();
    }
}