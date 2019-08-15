using System.Collections.Generic;
using SFML.Window;

namespace FullScreenPictureViewer.Graphics.Window
{
    internal class DefaultKeyConfig:IKeyConfig{
        public IReadOnlyDictionary<Keyboard.Key, ApplicationAction> KeyMap { get; } = new Dictionary<Keyboard.Key, ApplicationAction>
        {
            {Keyboard.Key.Left,new ApplicationAction("Previous Image",ApplicationActionType.PreviousImage) },
            {Keyboard.Key.Right,new ApplicationAction("Next Image",ApplicationActionType.NextImage) },
        };
    }
}