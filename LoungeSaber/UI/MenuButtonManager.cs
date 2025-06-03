using System;
using BeatSaberMarkupLanguage.MenuButtons;
using Zenject;

namespace LoungeSaber.UI
{
    public class MenuButtonManager : IInitializable, IDisposable
    {
        
        private readonly MenuButton _menuButton;
        
        public MenuButtonManager()
        {
            _menuButton = new MenuButton("LoungeSaber", OnClick);
        }

        private void OnClick(){}

        public void Initialize() => MenuButtons.Instance.RegisterButton(_menuButton);

        public void Dispose() => MenuButtons.Instance.UnregisterButton(_menuButton);
    }
}