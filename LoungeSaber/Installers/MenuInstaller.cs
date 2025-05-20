using LoungeSaber.UI;
using LoungeSaber.UI.BSML;
using LoungeSaber.UI.FlowCoordinators;
using Zenject;

namespace LoungeSaber.Installers
{
    public class MenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MenuButtonManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<MenuFlowCoordinator>().FromNewComponentOnNewGameObject().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadingViewController>().FromNewComponentAsViewController().AsSingle();
            Container.BindInterfacesAndSelfTo<DivisionSelectorViewController>().FromNewComponentAsViewController()
                .AsSingle();
        }
    }
}