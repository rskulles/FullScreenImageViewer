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

        private static KeyManager _keyManager;
        private static WindowManager _windowManager;

        private static void CloseApp(){
            _windowManager.Close();
        }

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                IKeyConfig config = new DefaultKeyConfig();
                _keyManager = new KeyManager(config);
                _keyManager.Subscribe(ApplicationActionType.Close, CloseApp);
                var path = args[0];
                if (File.Exists(path))
                {

                    _windowManager = new WindowManager(_keyManager);
                     _windowManager.SetScene(new ImageScene(path));
                     _windowManager.Run();
                }
                else if (Directory.Exists(path))
                {
                    
                   
                   _windowManager = new WindowManager(_keyManager);
                     _windowManager.SetScene(new MultiImageScene(path, _keyManager));
                     _windowManager.Run();
                }
            }
        }
    }
   
}