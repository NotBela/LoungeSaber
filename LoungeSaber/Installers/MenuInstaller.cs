using LoungeSaber.UI;
using Zenject;

namespace LoungeSaber.Installers
{
    public class MenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MenuButtonManager>().AsSingle();
        }
    }
}