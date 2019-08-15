using System;
using System.Collections.Generic;
using System.IO;
using FullScreenPictureViewer.Graphics.Window;
using SFML.Graphics;


namespace FullScreenPictureViewer.Scenes
{
    public class MultiImageScene:Scene{
       
        private readonly IApplicationActionEventProvider _eventProvider;

        private  List<Texture> _textures;
        private Sprite _sprite;
        private int imageIndex = 0;

        public MultiImageScene(string path, IApplicationActionEventProvider eventProvider)
        {
           
            _eventProvider = eventProvider;
            _eventProvider.Subscribe(ApplicationActionType.PreviousImage,Previous);
            _eventProvider.Subscribe(ApplicationActionType.NextImage,Next);
            LoadImages(path);
        }

        private void LoadImages(string path)
        {
            _textures = new List<Texture>();
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                if (file.EndsWith(".png") || file.EndsWith(".jpg"))
                {
                    _textures.Add(new Texture(file));
                }

            }

            _sprite = new Sprite(_textures[0]);
        }

        private void Next()
        {
            imageIndex++;
            if (imageIndex >= _textures.Count)
                imageIndex = 0;

            _sprite.Texture=_textures[imageIndex];
        }

        private void Previous()
        {
            imageIndex--;
            if (imageIndex < 0)
                imageIndex = _textures.Count-1;

            _sprite.Texture=_textures[imageIndex];
        }

        public override void Update(float dt)
        {
           
            _sprite.TextureRect = new IntRect(0,0,(int)_textures[imageIndex].Size.X,(int)_textures[imageIndex].Size.Y);
            var rect = _sprite.TextureRect;
            CurrentView = new View(new FloatRect(0,0, rect.Width,rect.Height));
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_sprite,states);
        }

        protected override void ReleaseUnmanagedResources()
        {
            for (int i = 0; i < _textures.Count; i++)
            {
                _textures[i].Dispose();    
            }
            
            _sprite.Dispose();
        }
    }

    public class ImageScene:Scene
    {
        private  Texture _texture;
        private Sprite _sprite;

        public ImageScene(string imagePath)
        {
            _texture = new Texture(imagePath);
            _sprite = new Sprite(_texture);
        }

        public override void Update(float dt)
        {
         
            var rect = _sprite.TextureRect;
            CurrentView = new View(new FloatRect(0,0, rect.Width,rect.Height));
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
           
            
            target.Draw(_sprite,states);
            target.SetView(target.DefaultView);
        }

        protected override void ReleaseUnmanagedResources()
        {
            _texture.Dispose();
            _sprite.Dispose();
        }
    }
}