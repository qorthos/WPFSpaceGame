using System;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tweening;

namespace WPFSpaceGame.General
{
    public interface IMonoGameItem : IDisposable
    {
        IGraphicsDeviceService GraphicsDeviceService { get; set; }
        bool IsInitialized { get; }

        void Initialize();
        void LoadContent();
        void UnloadContent();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
        void OnActivated(object sender, EventArgs args);
        void OnDeactivated(object sender, EventArgs args);
        void OnExiting(object sender, EventArgs args);

        void SizeChanged(object sender, SizeChangedEventArgs args);
    }

    public class MonoGameItem : IMonoGameItem
    {
        bool isInitialized;
        protected readonly Tweener Tweener = new Tweener();
        public bool IsInitialized
        {
            get { return isInitialized; }
        }
        public object ItemData
        {
            get; set;
        }


        public MonoGameItem()
            :base()
        {
        }

        public void Dispose()
        {
            Content?.Dispose();
        }

        public IGraphicsDeviceService GraphicsDeviceService { get; set; }
        protected GraphicsDevice GraphicsDevice => GraphicsDeviceService?.GraphicsDevice;
        protected ServiceProvider Services { get; private set; }
        protected ContentManager Content { get; set; }

        public virtual void Initialize()
        {
            Services = ServiceProvider.Instance;
            GraphicsDeviceService = ServiceProvider.Instance.GetService<IGraphicsDeviceService>();
            Content = new ContentManager(Services) { RootDirectory = "Content" };
            isInitialized = true;
        }

        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }
        public virtual void Update(GameTime gameTime) { Tweener.Update(gameTime.ElapsedGameTime.TotalSeconds); }
        public virtual void Draw(GameTime gameTime) {  }
        public virtual void OnActivated(object sender, EventArgs args) { }
        public virtual void OnDeactivated(object sender, EventArgs args) { }
        public virtual void OnExiting(object sender, EventArgs args) { }
        public virtual void SizeChanged(object sender, SizeChangedEventArgs args) { }
    }
}
