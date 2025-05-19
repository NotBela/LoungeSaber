using System.Linq;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using SiraUtil.Zenject;
using LoungeSaber.Configuration;
using LoungeSaber.Installers;
using IPALogger = IPA.Logging.Logger;

namespace LoungeSaber
{
    [Plugin(RuntimeOptions.DynamicInit),
     NoEnableDisable] // NoEnableDisable supresses the warnings of not having a OnEnable/OnStart
    // and OnDisable/OnExit methods
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }

        [Init]
        public void Init(Zenjector zenjector, IPALogger logger, Config config)
        {
            Instance = this;

            zenjector.UseLogger(logger);
            zenjector.UseMetadataBinder<Plugin>();

            // This logic also goes for installing to Menu and Game. "Location." will give you a list of places to install to.
            zenjector.Install<AppInstaller>(Location.App, config.Generated<PluginConfig>());
            zenjector.Install<MenuInstaller>(Location.Menu);
            zenjector.Install<GameInstaller>(Location.StandardPlayer);
        }
    }
}