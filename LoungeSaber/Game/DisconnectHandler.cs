using HMUI;
using JetBrains.Annotations;
using LoungeSaber_Server.Models.Packets.ServerPackets;
using LoungeSaber.Configuration;
using LoungeSaber.Interfaces;
using LoungeSaber.UI.FlowCoordinators;
using SiraUtil.Logging;
using UnityEngine.SceneManagement;
using Zenject;

namespace LoungeSaber.Game;

public class DisconnectHandler : IInitializable, IDisposable
{
    [Inject] private readonly IServerListener _serverListener = null;
    [Inject] private readonly MatchManager _matchManager = null;
    
    [Inject] private readonly SiraLog _siraLog = null;
    
    [CanBeNull] public event Action<string> ShouldShowDisconnectScreen;

    public bool WillShowDisconnectScreen { get; private set; } = false;
    
    public void Initialize()
    {
        _serverListener.OnDisconnected += OnDisconnect;
        _serverListener.OnPrematureMatchEnd += OnPrematureMatchEnd;
    }

    private void EndLevelAndShowDisconnectScreen(string reason)
    {
        WillShowDisconnectScreen = true;

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            ShouldShowDisconnectScreen?.Invoke(reason);
            WillShowDisconnectScreen = false;
            return;
        }
        
        _matchManager.StopMatch(() =>
        {
            ShouldShowDisconnectScreen?.Invoke(reason);
            WillShowDisconnectScreen = false;
        });
    }

    private void OnPrematureMatchEnd(PrematureMatchEnd packet)
    {
        EndLevelAndShowDisconnectScreen(packet.Reason);
    }

    private void OnDisconnect()
    {
        EndLevelAndShowDisconnectScreen("Disconnected");
    }

    public void Dispose()
    {
        _serverListener.OnDisconnected -= OnDisconnect;
        _serverListener.OnPrematureMatchEnd -= OnPrematureMatchEnd;
    }
}