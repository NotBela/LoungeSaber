using System;
using BeatSaberMarkupLanguage.MenuButtons;
using LoungeSaber.FlowCoordinators;
using Zenject;

namespace LoungeSaber.UI
{
    public class MenuButtonManager : IInitializable, IDisposable
    {
        [Inject] private readonly MenuFlowCoordinator _menuFlowCoordinator = null;
        [Inject] private readonly MainFlowCoordinator _mainFlowCoordinator = null;
        
        private readonly MenuButton _menuButton;
        
        public MenuButtonManager()
        {
            _menuButton = new MenuButton("LoungeSaber", OnClick);
        }

        private void OnClick() => _mainFlowCoordinator.PresentFlowCoordinator(_menuFlowCoordinator);

        public void Initialize() => MenuButtons.Instance.RegisterButton(_menuButton);

        public void Dispose() => MenuButtons.Instance.UnregisterButton(_menuButton);
    }
}