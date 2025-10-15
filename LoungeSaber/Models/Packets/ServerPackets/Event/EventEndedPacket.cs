namespace LoungeSaber.Models.Packets.ServerPackets.Event;

public class EventEndedPacket : ServerPacket
{
    public override ServerPacketTypes PacketType => ServerPacketTypes.EventEnded;
}