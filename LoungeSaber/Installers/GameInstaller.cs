using System.Threading.Tasks;
using LoungeSaber.AffinityPatches;
using LoungeSaber.AffinityPatches.EnergyPatches;
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
            
            // affinity patches
            Container.BindInterfacesAndSelfTo<PausePatch>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelBarSetupPatch>().AsSingle();
            Container.BindInterfacesAndSelfTo<PauseMenuStartPatch>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<EnergyBarInitPatch>().AsSingle();
        }
    }
}