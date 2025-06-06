namespace LoungeSaber.Models.Packets.ServerPackets
{
    public class JoinedQueue : ServerPacket
    {
        public override ServerPacketTypes PacketType => ServerPacketTypes.JoinedQueue;
    }
}