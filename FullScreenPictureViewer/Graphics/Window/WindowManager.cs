using System;
using System.Collections.Generic;
using System.ComponentModel;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace FullScreenPictureViewer.Graphics.Window
{
    public interface IApplicationActionEventProvider
    {
        void Subscribe(ApplicationActionType actionType, Action callback);
    }

    internal class KeyManager : IApplicationKeyManager,IApplicationActionEventProvider
    {
        
        public IReadOnlyDictionary<Keyboard.Key, ApplicationAction> KeyHandlers => _keyHandlers;
        private readonly Dictionary<Keyboard.Key, ApplicationAction> _keyHandlers;
        private readonly Dictionary<ApplicationActionType, Action> _actionMap;

        public KeyManager(IKeyConfig config)
        {
            _keyHandlers = new Dictionary<Keyboard.Key, ApplicationAction>(config.KeyMap);
            _actionMap = new Dictionary<ApplicationActionType, Action>();
        }

        

        public void Handle(ApplicationActionType appAction)
        {
            _actionMap[appAction]?.Invoke();
        }

        public void Subscribe(ApplicationActionType actionType, Action callBack)
        {
            _actionMap[actionType] = callBack;
        }
    }

    public class WindowManager
    {
        private readonly IApplicationKeyManager _keyManager;
        private RenderWindow _window;
        private bool _isRunning;
        private bool _isFullScreen;
        private Scene _currentScene;
        private bool _fullScreenToggled;
        private readonly object _windowLock = new object();

        public WindowManager(IApplicationKeyManager keyManager)
        {
            _keyManager = keyManager;
        }

        private void CreateWindow()
        {
            UnsubscribeFromEvents();
            var baseStyle = Styles.None;
            var style = _isFullScreen ? baseStyle | Styles.Fullscreen : baseStyle;
            _window?.SetVisible(false);
            _window?.Dispose();
            _window = null;
            _window = new RenderWindow(VideoMode.DesktopMode, "FSIV", style);
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            if (_window == null) return;
            _window.Closed += WindowOnClosed;
            _window.KeyReleased += WindowOnKeyReleased;
        }

        private void UnsubscribeFromEvents()
        {
            if (_window == null) return;
            _window.Closed -= WindowOnClosed;
            _window.KeyReleased -= WindowOnKeyReleased;
        }

        private void WindowOnKeyReleased(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Enter && e.Alt)
            {
                _isFullScreen = !_isFullScreen;
                _fullScreenToggled = true;
            }

            if (_keyManager.KeyHandlers.TryGetValue(e.Code, out var applicationAction))
                _keyManager.Handle(applicationAction.Id);
        }

        private void WindowOnClosed(object sender, System.EventArgs e)
        {
            _isRunning = false;
        }

        public void Run()
        {
            CreateWindow();
            var clock = new Clock();
            _isRunning = true;
            while (_isRunning)
            {
                var dt = clock.Restart();
                _window?.DispatchEvents();
                Update(dt.AsSeconds());
                Render();
            }

            _currentScene?.Dispose();
            _window.Close();
        }

        public void SetScene(Scene scene)
        {
            _currentScene = scene;
        }

        private void Update(float dt)
        {
            if (_fullScreenToggled)
            {
                lock (_windowLock)
                {
                    _fullScreenToggled = false;
                    CreateWindow();
                }
            }
            
            
            if (_currentScene != null)
            {
                _currentScene.Update(dt);
                _window.SetView(_currentScene.CurrentView??_window.DefaultView);
            }

            
        }

        private void Render()
        {
            _window.Clear(Color.Transparent);
            if (_currentScene != null)
                _window.Draw(_currentScene);
            _window.Display();
        }
    }
}