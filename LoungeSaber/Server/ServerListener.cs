﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LoungeSaber_Server.Models.Packets.ServerPackets;
using LoungeSaber.Configuration;
using LoungeSaber.Interfaces;
using LoungeSaber.Models.Packets;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Models.Packets.ServerPackets.Match;
using LoungeSaber.Models.Packets.UserPackets;
using SiraUtil.Logging;
using Zenject;

namespace LoungeSaber.Server
{
    public class ServerListener : IServerListener
    {
        [Inject] private readonly PluginConfig _config = null;
        [Inject] private readonly SiraLog _siraLog = null;

        private TcpClient _client = new();
        private Thread _listenerThread;

        private bool _shouldListenToServer = false;
        
        public event Action<MatchCreatedPacket> OnMatchCreated;
        public event Action<OpponentVoted> OnOpponentVoted;
        public event Action<MatchStarted> OnMatchStarting;
        public event Action<MatchResultsPacket> OnMatchResults;
        
        public event Action OnDisconnected;
        public event Action OnConnected;
        public event Action<PrematureMatchEnd> OnPrematureMatchEnd;
        
        [Inject] private readonly IPlatformUserModel _platformUserModel = null;

        private bool Connected
        {
            get
            {
                try
                {
                    var poll = _client.Client.Poll(1, SelectMode.SelectRead) && !_client.GetStream().DataAvailable;

                    return !poll;
                }
                catch (SocketException e)
                {
                    return e.SocketErrorCode is SocketError.WouldBlock or SocketError.Interrupted;
                }
            }
        }

        public async Task Connect(Action<JoinResponse> onConnectedCallBack)
        {
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(IPAddress.Parse(_config.ServerIp), _config.ServerPort);

                var userPlatformData = await _platformUserModel.GetUserInfo(CancellationToken.None);
                
                await SendPacket(new JoinRequestPacket(userPlatformData.userName, userPlatformData.platformUserId));

                while (!_client.GetStream().DataAvailable)
                    await Task.Delay(25);
                
                var bytes = new byte[1024];
                
                var bytesRead = _client.GetStream().Read(bytes, 0, bytes.Length);
                Array.Resize(ref bytes, bytesRead);
                
                _siraLog.Info(Encoding.UTF8.GetString(bytes));
                
                var responsePacket = ServerPacket.Deserialize(Encoding.UTF8.GetString(bytes)) as JoinResponse ?? throw new Exception("Could not deserialize response!");

                onConnectedCallBack.Invoke(responsePacket);

                if (responsePacket.Successful)
                {
                    _listenerThread = new Thread(ListenToServer);
                    _shouldListenToServer = true;
                    _listenerThread.Start();
                    OnConnected?.Invoke();
                }
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }

        public async Task SendPacket(UserPacket packet)
        { 
            var bytes = packet.SerializeToBytes();
            await _client.GetStream().WriteAsync(bytes, 0, bytes.Length);
        }

        public void Disconnect()
        {
            if (!_shouldListenToServer)
                return;
            
            _shouldListenToServer = false;
            _client.Close();
            OnDisconnected?.Invoke();
        }

        private void ListenToServer()
        {
            while (_shouldListenToServer)
            {
                try
                {
                    if (!Connected)
                        return;
                    
                    var data = new byte[1024];

                    var bytesRead = _client.GetStream().Read(data, 0, data.Length);
                    Array.Resize(ref data, bytesRead);

                    var json = Encoding.UTF8.GetString(data);
                    
                    if (json == "") 
                        continue;
                    
                    _siraLog.Info(json);

                    var packet = ServerPacket.Deserialize(json);

                    switch (packet.PacketType)
                    {
                        case ServerPacket.ServerPacketTypes.MatchCreated:
                            OnMatchCreated?.Invoke(packet as MatchCreatedPacket);
                            break;
                        case ServerPacket.ServerPacketTypes.OpponentVoted:
                            OnOpponentVoted?.Invoke(packet as OpponentVoted);
                            break;
                        case ServerPacket.ServerPacketTypes.MatchStarted:
                            OnMatchStarting?.Invoke(packet as MatchStarted);
                            break;
                        case ServerPacket.ServerPacketTypes.MatchResults:
                            OnMatchResults?.Invoke(packet as MatchResultsPacket);
                            break;
                        case ServerPacket.ServerPacketTypes.PrematureMatchEnd:
                            OnPrematureMatchEnd?.Invoke(packet as PrematureMatchEnd);
                            break;
                        default:
                            throw new Exception("Could not get packet type!");
                    }
                }
                catch (Exception e)
                {
                    _siraLog.Error(e);
                    Disconnect();
                }
            }
        }
    }
}