using LoungeSaber.Configuration;
using LoungeSaber.Game;
using LoungeSaber.Interfaces;
using LoungeSaber.Server;
using LoungeSaber.Server.Debug;
using Zenject;

namespace LoungeSaber.Installers
{
    internal class AppInstaller : Installer
    {
        private readonly PluginConfig _config;

        public AppInstaller(PluginConfig config)
        {
            _config = config;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(_config);
            
            Container.BindInterfacesAndSelfTo<MatchManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<DisconnectHandler>().AsSingle();

            if (_config.ConnectToDebugQueue)
            {
                Container.BindInterfacesAndSelfTo<DebugServerListener>().AsSingle();
                Container.BindInterfacesAndSelfTo<DebugApi>().AsSingle();
                return;
            }
            
            Container.BindInterfacesAndSelfTo<ServerListener>().AsSingle();
            Container.BindInterfacesAndSelfTo<Api>().AsSingle();
        }
    }
}