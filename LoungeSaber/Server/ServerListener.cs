using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LoungeSaber.Configuration;
using LoungeSaber.Models.Packets;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Models.Packets.UserPackets;
using SiraUtil.Logging;
using Zenject;

namespace LoungeSaber.Server
{
    public class ServerListener
    {
        [Inject] private readonly PluginConfig _config = null;
        [Inject] private readonly SiraLog _siraLog = null;

        private readonly TcpClient _client = new TcpClient();
        private Thread _listenerThread;

        private bool _shouldListenToServer = false;

        public event Action<JoinedQueue> OnJoinedQueuePacketRecieved;

        public async Task Connect()
        {
            try
            {
                // TODO: replace this with config values
                await _client.ConnectAsync(IPAddress.Parse(_config.ServerIp), _config.ServerPort);

                var userPlatformData = await BS_Utils.Gameplay.GetUserInfo.GetUserAsync();
                
                _listenerThread = new Thread(ListenToServer);
                _shouldListenToServer = true;
                _listenerThread.Start();
                
                await SendPacket(new JoinRequestPacket(userPlatformData.userName, userPlatformData.platformUserId));
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

        private void ListenToServer()
        {
            while (_shouldListenToServer)
            {
                try
                {
                    var data = new byte[1024];

                    var bytesRead = _client.GetStream().Read(data, 0, data.Length);
                    Array.Resize(ref data, bytesRead);

                    var json = Encoding.UTF8.GetString(data);
                    
                    _siraLog.Info(json);

                    var packet = ServerPacket.Deserialize(json);

                    switch (packet.PacketType)
                    {
                        case ServerPacket.ServerPacketTypes.JoinedQueue:
                            OnJoinedQueuePacketRecieved?.Invoke(packet as JoinedQueue);
                            _siraLog.Info("joined queue!!");
                            break;
                        default:
                            throw new Exception("Could not get packet type!");
                    }
                }
                catch (Exception e)
                {
                    _siraLog.Error(e);
                }
            }
        }
    }
}