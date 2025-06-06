using System;
using BeatSaberMarkupLanguage.MenuButtons;
using LoungeSaber.Configuration;
using LoungeSaber.Server;
using LoungeSaber.UI.FlowCoordinators;
using Zenject;

namespace LoungeSaber.UI
{
    public class MenuButtonManager : IInitializable, IDisposable
    {
        [Inject] private readonly MainFlowCoordinator _mainFlowCoordinator = null;
        [Inject] private readonly QueueMenuFlowCoordinator _queueFlowCoordinator = null;
        
        private readonly MenuButton _menuButton;
        
        public MenuButtonManager()
        {
            _menuButton = new MenuButton("LoungeSaber", OnClick);
        }

        private void OnClick() => _mainFlowCoordinator.PresentFlowCoordinator(_queueFlowCoordinator);

        public void Initialize() => MenuButtons.Instance.RegisterButton(_menuButton);

        public void Dispose() => MenuButtons.Instance.UnregisterButton(_menuButton);
    }
}