using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]

namespace LoungeSaber.Configuration
{
    public class PluginConfig
    {
        public virtual string ServerIp { get; set; } = "127.0.0.1";
        public virtual int ServerPort { get; set; } = 8008;
        public virtual int ServerApiPort { get; set; } = 7198;
        
        public virtual bool DownloadMapsAutomatically { get; set; } = false;
    }
}