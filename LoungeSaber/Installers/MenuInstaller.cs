using LoungeSaber.UI;
using LoungeSaber.UI.FlowCoordinators;
using Zenject;

namespace LoungeSaber.Installers
{
    public class MenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MenuButtonManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<QueueMenuFlowCoordinator>().FromNewComponentOnNewGameObject().AsSingle();
        }
    }
}