using LoungeSaber.FlowCoordinators;
using LoungeSaber.UI;
using LoungeSaber.UI.BSML;
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
        }
    }
}