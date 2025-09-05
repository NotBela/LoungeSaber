using System.Threading.Tasks;
using LoungeSaber.AffinityPatches;
using LoungeSaber.AffinityPatches.EnergyPatches;
using LoungeSaber.AffinityPatches.PausePatches;
using LoungeSaber.AffinityPatches.ScorePatches;
using LoungeSaber.Game;
using LoungeSaber.UI.BSML.PauseMenu;
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
            Container.BindInterfacesAndSelfTo<PauseMenuViewController>().FromNewComponentAsViewController().AsSingle();
            
            // affinity patches
            Container.BindInterfacesAndSelfTo<PausePatch>().AsSingle();
            Container.BindInterfacesAndSelfTo<PauseMenuStartPatch>().AsSingle();
            Container.BindInterfacesAndSelfTo<ContinuePausePatch>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<EnergyBarInitPatch>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<ScoreDisplayPatch>().AsSingle();
            Container.BindInterfacesAndSelfTo<ImmediateRankDisplayPatch>().AsSingle();
        }
    }
}