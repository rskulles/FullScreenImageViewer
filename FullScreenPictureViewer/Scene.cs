using System;
using SFML.Graphics;

namespace FullScreenPictureViewer
{
    public abstract class Scene:Drawable,IDisposable
    {
        public View CurrentView { get; protected set; }

        public abstract void Update(float dt);
        public abstract void Draw(RenderTarget target, RenderStates states);

        protected abstract void ReleaseUnmanagedResources();

        protected virtual void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Scene()
        {
            Dispose(false);
        }
    }
}