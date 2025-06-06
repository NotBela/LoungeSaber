using LoungeSaber.Configuration;
using LoungeSaber.Server;
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

            Container.BindInterfacesAndSelfTo<ServerListener>().AsSingle();
        }
    }
}