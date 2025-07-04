using LoungeSaber.UI;
using LoungeSaber.UI.BSML;
using LoungeSaber.UI.BSML.Leaderboard;
using LoungeSaber.UI.BSML.Match;
using LoungeSaber.UI.BSML.Menu;
using LoungeSaber.UI.FlowCoordinators;
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
            Container.BindInterfacesAndSelfTo<UI.BSML.Leaderboard.LoungeSaberLeaderboardViewController>().FromNewComponentAsViewController().AsSingle();
        }
    }
}