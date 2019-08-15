using System.Collections.Generic;
using SFML.Window;

namespace FullScreenPictureViewer.Graphics.Window
{
    public interface IKeyConfig
    {
        IReadOnlyDictionary<Keyboard.Key,ApplicationAction> KeyMap { get; }
        
    }
}