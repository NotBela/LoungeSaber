using LoungeSaber.Game;
using LoungeSaber.UI;
using LoungeSaber.UI.BSML;
using LoungeSaber.UI.BSML.Leaderboard;
using LoungeSaber.UI.BSML.Match;
using LoungeSaber.UI.BSML.Menu;
using LoungeSaber.UI.FlowCoordinators;
using LoungeSaber.UI.ViewManagers;
using Zenject;

namespace LoungeSaber.Installers
{
    public class MenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MenuButtonManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<MatchmakingMenuFlowCoordinator>().FromNewComponentOnNewGameObject().AsSingle();
            Container.BindInterfacesAndSelfTo<MatchmakingMenuViewController>().FromNewComponentAsViewController().AsSingle();
            Container.BindInterfacesAndSelfTo<MatchFlowCoordinator>().FromNewComponentOnNewGameObject().AsSingle();
            Container.BindInterfacesAndSelfTo<VotingScreenViewController>().FromNewComponentAsViewController().AsSingle();
            Container.BindInterfacesAndSelfTo<AwaitingMapDecisionViewController>().FromNewComponentAsViewController().AsSingle();
            Container.BindInterfacesAndSelfTo<WaitingForMatchToStartViewController>().FromNewComponentAsViewController().AsSingle();
            Container.BindInterfacesAndSelfTo<AwaitMatchEndViewController>().FromNewComponentAsViewController()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<MatchResultsViewController>().FromNewComponentAsViewController()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<LeaderboardPanelViewController>().FromNewComponentAsViewController()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<LoungeSaberLeaderboardViewController>().FromNewComponentAsViewController().AsSingle();
            Container.BindInterfacesAndSelfTo<CantConnectToServerViewController>().FromNewComponentAsViewController().AsSingle();
            Container.BindInterfacesAndSelfTo<CheckingServerStatusViewController>().FromNewComponentAsViewController().AsSingle();
            Container.BindInterfacesAndSelfTo<MissingMapsViewController>().FromNewComponentAsViewController().AsSingle();
            Container.BindInterfacesAndSelfTo<ServerCheckingFlowCoordinator>().FromNewComponentOnNewGameObject().AsSingle();

            Container.BindInterfacesAndSelfTo<GameplaySetupViewManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<StandardLevelDetailViewManager>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<MapDownloader>().AsSingle();
        }
    }
}