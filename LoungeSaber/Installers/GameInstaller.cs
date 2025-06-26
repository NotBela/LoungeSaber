using System.Threading.Tasks;
using LoungeSaber.AffinityPatches;
using LoungeSaber.Game;
using Zenject;

namespace LoungeSaber.Installers
{
    public class GameInstaller : Installer
    {
        public override void InstallBindings()
        {
            if (!Container.Resolve<MatchManager>().InMatch) 
                return;
            
            Container.BindInterfacesAndSelfTo<MatchStartUnpauseController>().AsSingle();
            Container.BindInterfacesAndSelfTo<PausePatch>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelBarSetupPatch>().AsSingle();
        }
    }
}