namespace LoungeSaber.Models.Packets.ServerPackets.Event;

public class OutOfEventPacket : ServerPacket
{
    public override ServerPacketTypes PacketType => ServerPacketTypes.OutOfEvent;
}