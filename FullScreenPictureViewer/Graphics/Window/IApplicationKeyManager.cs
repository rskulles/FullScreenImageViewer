using System.Collections.Generic;
using SFML.Window;

namespace FullScreenPictureViewer.Graphics.Window
{
    public interface IApplicationKeyManager
    {
        IReadOnlyDictionary<Keyboard.Key, ApplicationAction> KeyHandlers { get; }

        void Handle(ApplicationActionType appAction);
    }
}