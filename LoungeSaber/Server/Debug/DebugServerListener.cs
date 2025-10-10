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
    public event Action OnDisconnected;
    public event Action OnConnected;
    public event Action<PrematureMatchEnd> OnPrematureMatchEnd;
    
    public event Action<EventMatchCreatedPacket> OnEventMatchStarted;
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

    public Task SendPacket(UserPacket packet)
    {
        if (!_isConnected)
        {
            _siraLog.Info("tried to send packet when not connected!");
            return Task.CompletedTask;
        }
        
        switch (packet.PacketType)
        {
            case UserPacket.UserPacketTypes.JoinRequest:
                Task.Run(() =>
                {
                    if (((JoinRequestPacket) packet).Queue == "test")
                        return;
                    
                    Task.Delay(1000);
                    OnMatchCreated?.Invoke(new MatchCreatedPacket(DebugApi.Maps, DebugApi.DebugOpponent));
                    _siraLog.Info("join request");
                });
                break;
            case UserPacket.UserPacketTypes.Vote:
                Task.Run(async () =>
                {
                    OnOpponentVoted?.Invoke(new OpponentVoted(0));
                    await Task.Delay(1000);
                    OnMatchStarting?.Invoke(new MatchStarted(DebugApi.Maps[0], DateTime.UtcNow.AddSeconds(15),
                        DateTime.UtcNow.AddSeconds(25), DebugApi.DebugOpponent));
                    _siraLog.Info("voted");
                });
                break;
            case UserPacket.UserPacketTypes.ScoreSubmission:
                Task.Run(() =>
                {
                    _siraLog.Info("score submitted");
                    Task.Delay(1000);
                    OnMatchResults?.Invoke(new MatchResultsPacket(new MatchScore(DebugApi.Self, Score.Empty),
                        new MatchScore(DebugApi.DebugOpponent, Score.Empty), 100));
                    
                    _siraLog.Info("match results invoked");
                });
                break;
            default:
                throw new NotImplementedException();
        }

        return Task.CompletedTask;
    }

    public void Disconnect()
    {
        if (!_isConnected) return;
        
        OnDisconnected?.Invoke();
    }
}