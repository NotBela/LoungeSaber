using System.Text;
using Newtonsoft.Json;

namespace LoungeSaber.Models.Packets
{
    public abstract class Packet
    {
        public byte[] SerializeToBytes() => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
    }
}