using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using FullScreenPictureViewer.Graphics.Window;
using FullScreenPictureViewer.Scenes;
using SFML.Window;

namespace FullScreenPictureViewer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                

                var path = args[0];
                if (File.Exists(path))
                {

                    var manager = new WindowManager(new NullManager());
                    manager.SetScene(new ImageScene(path));
                    manager.Run();
                }
                else if (Directory.Exists(path))
                {
                    IKeyConfig config = new DefaultKeyConfig();
                    var applicationKeyManager = new KeyManager(config);
                    var manager = new WindowManager(applicationKeyManager);
                    manager.SetScene(new MultiImageScene(path, applicationKeyManager));
                    manager.Run();
                }
            }
        }
    }

    internal class NullManager : IApplicationKeyManager
    {
        public IReadOnlyDictionary<Keyboard.Key, ApplicationAction> KeyHandlers { get; }

        public NullManager()
        {
            KeyHandlers = new Dictionary<Keyboard.Key, ApplicationAction>();
        }
        public void Handle(ApplicationActionType appAction)
        {
            //No Op
        }
    }
}