using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LoungeSaber.Configuration;
using LoungeSaber.Models.Divisions;
using LoungeSaber.Models.Maps;
using LoungeSaber.Models.Networking;
using LoungeSaber.Models.User;
using Newtonsoft.Json.Linq;
using SiraUtil.Logging;
using Zenject;

namespace LoungeSaber.Server.MatchRoom
{
    public class LoungeServerInterfacer : IInitializable
    {
        [Inject] private readonly PluginConfig _config = null;
        [Inject] private readonly SiraLog _siraLog = null;
        
        private TcpClient _client;
        private Thread ServerListenerThread;

        private bool _listenToServer = true;
        
        public event Action<VotingMap, DateTime> MatchMapDecidedAndStartTimeGiven;
        public event Action<VotingMap> OpponentVoted;
        public event Action<User, VotingMap[]> MatchCreated;
        public event Action<string> MatchEnded;
        public event Action<DateTime> StartWarning;
        public event Action<int, int> ResultsRecieved;
        public event Action<int> UserCountUpdated;

        public event Action<Exception> OnExceptionOccured;

        public event Action OnDisconnect;

        public event Action<string> OnDisconnectByServer; 
        
        public event Action OnStartConnect;
        public event Action OnConnected;

        public void Initialize()
        {
            ServerListenerThread = new Thread(ListenToServer);

            _client = new TcpClient();
        }

        public async Task ConnectToLoungeServer(Division division)
        {
            OnStartConnect?.Invoke();
            
            _client = new TcpClient();
            _listenToServer = true;
            ServerListenerThread.Start();
            await _client.ConnectAsync(IPAddress.Parse(_config.ServerIp), _config.ServerPort);

            var userInfo = await BS_Utils.Gameplay.GetUserInfo.GetUserAsync();
            
            await SendPacketToServer(new UserPacket(UserPacket.PacketType.Join, new JObject
            {
                {"divisionName", division.DivisionName },
                {"userId", userInfo.platformUserId },
                {"userName", userInfo.userName }
            }));
            
            OnConnected?.Invoke();
        }

        public async Task DisconnectFromLoungeServer()
        {
            await SendPacketToServer(new UserPacket(UserPacket.PacketType.Leave, new JObject()));
            
            _listenToServer = false;
            _client.Close();
            OnDisconnect?.Invoke();
        }

        private void ListenToServer()
        {
            while (_listenToServer)
            {
                try
                {
                    if (!_client.Connected) 
                        continue;

                    while (!_client.GetStream().DataAvailable);

                    var buffer = new byte[1024];
                    var bufferLength = _client.Client.Receive(buffer);

                    Array.Resize(ref buffer, bufferLength);

                    var decodedJson = Encoding.UTF8.GetString(buffer);
                    
                    _siraLog.Info(decodedJson);
                    
                    var serverAction = ServerPacket.Deserialize(decodedJson);

                    switch (serverAction.Type)
                    {
                        case ServerPacket.ActionType.StartWarning:
                            if (!DateTime.TryParse(serverAction.Data["startTime"]?.ToObject<string>(), out var time))
                                throw new Exception("Starting time could not be parsed!");
                        
                            StartWarning?.Invoke(time.ToUniversalTime());
                            break;
                        case ServerPacket.ActionType.CreateMatch:
                            if (!serverAction.Data.TryGetValue("opponent", out var opponent))
                                throw new Exception("Could not get opponent data!");
                            if (!serverAction.Data.TryGetValue("votingOptions", out var votingOptionsJToken))
                                throw new Exception("Could not get voting options");

                            var votingOptions = ((JArray) votingOptionsJToken).ToObject<List<VotingMap>>().ToArray();
                        
                            MatchCreated?.Invoke(opponent.ToObject<User>(), votingOptions);
                            break;
                        case ServerPacket.ActionType.OpponentVoted:
                            if (!serverAction.Data.TryGetValue("opponentVote", out var opponentVote))
                                throw new Exception("Could not get opponent vote!");
                        
                            OpponentVoted?.Invoke(opponentVote.ToObject<VotingMap>());
                            break;
                        case ServerPacket.ActionType.StartMatch:
                            if (!serverAction.Data.TryGetValue("selectedMap", out var selectedMap))
                                throw new Exception("Could not get starting map!");
                            if (!serverAction.Data.TryGetValue("startingTime", out var startingTime))
                                throw new Exception("Could not get match starting time!");

                            if (!DateTime.TryParse(startingTime.ToObject<string>(), out var startingDateTime))
                                throw new Exception("Could not parse match starting time!");
                        
                            MatchMapDecidedAndStartTimeGiven?.Invoke(selectedMap.ToObject<VotingMap>(), startingDateTime.ToUniversalTime());
                            break;
                        case ServerPacket.ActionType.Results:
                            if (!serverAction.Data.TryGetValue("opponentScore", out var oppoentScoreToken))
                                throw new Exception("Could not get opponent score!");
                            if (!serverAction.Data.TryGetValue("mmrChange", out var mmrChangeToken))
                                throw new Exception("Could not get MMR change!");
                        
                            ResultsRecieved?.Invoke(oppoentScoreToken.ToObject<int>(), mmrChangeToken.ToObject<int>());
                            break;
                        case ServerPacket.ActionType.MatchEnded:
                            if (!serverAction.Data.TryGetValue("reason", out var reason))
                                throw new Exception("Could not get reason for match end!");
                        
                            MatchEnded?.Invoke(reason.ToObject<string>());
                            break;
                        case ServerPacket.ActionType.UpdateConnectedUserCount:
                            if (!serverAction.Data.TryGetValue("userCount", out var userCount))
                                throw new Exception("Could not get user count!");
                            
                            UserCountUpdated?.Invoke(userCount.ToObject<int>());
                            break;
                        case ServerPacket.ActionType.Disconnect:
                            if (!serverAction.Data.TryGetValue("reason", out var disconnectReason))
                                throw new Exception($"Disconnected by server: {disconnectReason.ToObject<string>()}");
                            
                            OnDisconnectByServer?.Invoke(disconnectReason.ToObject<string>());
                            break;
                    }
                }
                catch (Exception e)
                {
                    if (e is ObjectDisposedException)
                        return;
                    
                    _siraLog.Error(e);
                    OnExceptionOccured?.Invoke(e);
                }
            }
        }

        private async Task SendPacketToServer(UserPacket packet)
        {
            await _client.Client.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(packet.Serialize())), SocketFlags.None);
        }
    }
}