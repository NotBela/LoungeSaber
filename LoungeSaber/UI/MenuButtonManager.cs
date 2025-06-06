using System;
using BeatSaberMarkupLanguage.MenuButtons;
using LoungeSaber.Server;
using Zenject;

namespace LoungeSaber.UI
{
    public class MenuButtonManager : IInitializable, IDisposable
    {
        [Inject] private readonly ServerListener _serverListener = null;
        
        private readonly MenuButton _menuButton;
        
        public MenuButtonManager()
        {
            _menuButton = new MenuButton("LoungeSaber", OnClick);
        }

        private void OnClick()
        {
            _serverListener.Connect();
        }

        public void Initialize() => MenuButtons.Instance.RegisterButton(_menuButton);

        public void Dispose() => MenuButtons.Instance.UnregisterButton(_menuButton);
    }
}