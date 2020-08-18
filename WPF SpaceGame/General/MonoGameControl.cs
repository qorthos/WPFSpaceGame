﻿// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.ApplicationServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WPFSpaceGame.Game.Graphics;

namespace WPFSpaceGame.General
{
    public sealed class MonoGameControl : ItemsControl, IDisposable
    {
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(
            "Item", typeof(MonoGameItem), typeof(MonoGameControl), new PropertyMetadata(null));

        public static readonly DependencyProperty ItemDataProperty = DependencyProperty.Register(
            "ItemData", typeof(object), typeof(MonoGameControl), new PropertyMetadata(null));

        MonoGameGraphicsDeviceService graphicsDeviceService;
        private int _instanceCount;
        private readonly GameTime _gameTime = new GameTime();
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private D3DImage _direct3DImage;
        private RenderTarget2D _renderTarget;
        private SharpDX.Direct3D9.Texture _renderTargetD3D9;
        private bool _isFirstLoad = true;
        private bool _isInitialized;


        public GraphicsDevice GraphicsDevice => graphicsDeviceService?.GraphicsDevice;
        public bool IsDisposed { get; private set; }


        public MonoGameItem Item
        {
            get
            {
                return (MonoGameItem)GetValue(ItemProperty);
            }

            set
            {
                SetValue(ItemProperty, value);                
            }
        }


        public object ItemData
        {
            get
            {
                return (object)GetValue(ItemDataProperty);
            }

            set
            {
                SetValue(ItemDataProperty, value);
            }
        }


        public MonoGameControl() : base()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            if (ServiceProvider.Instance.GetService<IGraphicsDeviceService>() == null)
            {
                graphicsDeviceService = new MonoGameGraphicsDeviceService();
                ServiceProvider.Instance.AddService<IGraphicsDeviceService>(graphicsDeviceService);
            }
            else
            {
                graphicsDeviceService = (MonoGameGraphicsDeviceService)ServiceProvider.Instance.GetService<IGraphicsDeviceService>();
            }

            _instanceCount++;
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            SizeChanged += (sender, args) => Item?.SizeChanged(sender, args);           
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            //Item?.Dispose(); // throws an error on the thread =[
            _renderTarget?.Dispose();
            _renderTargetD3D9?.Dispose();
            _instanceCount--;

            if (_instanceCount <= 0)
                graphicsDeviceService?.Dispose();

            IsDisposed = true;
        }

        ~MonoGameControl()
        {
            Dispose(false);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            Item?.OnActivated(this, EventArgs.Empty);
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            Item?.OnDeactivated(this, EventArgs.Empty);
            base.OnLostFocus(e);
        }

        private void Start()
        {
            if(_isInitialized)
                return;

            if (Application.Current.MainWindow == null)
                throw new InvalidOperationException("The application must have a MainWindow");

            OnFirstLoad();
            Application.Current.MainWindow.ContentRendered += (sender, args) =>
            {
                if (_isFirstLoad)
                {
                    OnFirstLoad();
                }
            };
            Application.Current.MainWindow.Closing += (sender, args) => Item?.OnExiting(this, EventArgs.Empty);

            _direct3DImage = new D3DImage();

            AddChild(new Image { Source = _direct3DImage, Stretch = Stretch.None });

            //_direct3DImage.IsFrontBufferAvailableChanged += OnDirect3DImageIsFrontBufferAvailableChanged;



            _renderTarget = CreateRenderTarget();
            CompositionTarget.Rendering += OnRender;
            _stopwatch.Start();
            _isInitialized = true;
        }

        private void OnFirstLoad()
        {
            graphicsDeviceService.StartDirect3D(Application.Current.MainWindow);
            _isFirstLoad = false;
        }


        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            
            //// sometimes OnRenderSizeChanged happens before OnLoaded.
            //Start();
            //ResetBackBufferReference();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Start();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Item?.UnloadContent();

            if (graphicsDeviceService != null)
            {
                CompositionTarget.Rendering -= OnRender;
                ResetBackBufferReference();
                graphicsDeviceService.DeviceResetting -= OnGraphicsDeviceServiceDeviceResetting;
            }
        }

        private void OnGraphicsDeviceServiceDeviceResetting(object sender, EventArgs e)
        {
            ResetBackBufferReference();
        }

        private void ResetBackBufferReference()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            if (_renderTarget != null)
            {
                _renderTarget.Dispose();
                _renderTarget = null;
            }

            if (_renderTargetD3D9 != null)
            {
                _renderTargetD3D9.Dispose();
                _renderTargetD3D9 = null;
            }

            _direct3DImage.Lock();
            _direct3DImage.SetBackBuffer(D3DResourceType.IDirect3DSurface9, IntPtr.Zero);
            _direct3DImage.Unlock();
        }

        private RenderTarget2D CreateRenderTarget()
        {
            var actualWidth = (int)ActualWidth;
            var actualHeight = (int)ActualHeight;

            if (actualWidth == 0 || actualHeight == 0)
                return null;

            if (GraphicsDevice == null)
                return null;

            var renderTarget = new RenderTarget2D(GraphicsDevice, actualWidth, actualHeight,
                false, SurfaceFormat.Bgra32, DepthFormat.Depth24Stencil8, 1,
                RenderTargetUsage.PlatformContents, true);

            var handle = renderTarget.GetSharedHandle();

            if (handle == IntPtr.Zero)
                throw new ArgumentException("Handle could not be retrieved");

            _renderTargetD3D9 = new SharpDX.Direct3D9.Texture(graphicsDeviceService.Direct3DDevice, renderTarget.Width,
                renderTarget.Height,
                1, SharpDX.Direct3D9.Usage.RenderTarget, SharpDX.Direct3D9.Format.A8R8G8B8,
                SharpDX.Direct3D9.Pool.Default, ref handle);

            using (var surface = _renderTargetD3D9.GetSurfaceLevel(0))
            {
                _direct3DImage.Lock();
                _direct3DImage.SetBackBuffer(D3DResourceType.IDirect3DSurface9, surface.NativePointer);
                _direct3DImage.Unlock();
            }

            return renderTarget;
        }

        private void OnRender(object sender, EventArgs e)
        {
            _gameTime.ElapsedGameTime = _stopwatch.Elapsed;
            _gameTime.TotalGameTime += _gameTime.ElapsedGameTime;
            _stopwatch.Restart();

            
            if (Item?.IsInitialized == false)
            {
                Item.Initialize();
                Item.LoadContent();
            }

            if (Item != null)
            {
                Item.ItemData = ItemData;
            }

            if (CanBeginDraw())
            {
                try
                {
                    _direct3DImage.Lock();

                    if (_renderTarget == null)
                        _renderTarget = CreateRenderTarget();

                    if (_renderTarget != null)
                    {
                        GraphicsDevice.SetRenderTarget(_renderTarget);
                        SetViewport();

                        Item?.Update(_gameTime);
                        Item?.Draw(_gameTime);

                        GraphicsDevice.Flush();
                        _direct3DImage.AddDirtyRect(new Int32Rect(0, 0, (int)ActualWidth, (int)ActualHeight));
                    }
                }
                finally
                {
                    _direct3DImage.Unlock();
                    GraphicsDevice.SetRenderTarget(null);
                }
            }
        }

        private bool CanBeginDraw()
        {
            // If we have no graphics device, we must be running in the designer.
            if (graphicsDeviceService == null)
                return false;

            if (!_direct3DImage.IsFrontBufferAvailable)
                return false;

            // Make sure the graphics device is big enough, and is not lost.
            if (!HandleDeviceReset())
                return false;

            return true;
        }

        private void SetViewport()
        {
            // Many GraphicsDeviceControl instances can be sharing the same
            // GraphicsDevice. The device backbuffer will be resized to fit the
            // largest of these controls. But what if we are currently drawing
            // a smaller control? To avoid unwanted stretching, we set the
            // viewport to only use the top left portion of the full backbuffer.
            var width = Math.Max(1, (int)ActualWidth);
            var height = Math.Max(1, (int)ActualHeight);
            GraphicsDevice.Viewport = new Viewport(0, 0, width, height);
        }

        private bool HandleDeviceReset()
        {
            if (GraphicsDevice == null)
                return false;

            var deviceNeedsReset = false;

            switch (GraphicsDevice.GraphicsDeviceStatus)
            {
                case GraphicsDeviceStatus.Lost:
                    // If the graphics device is lost, we cannot use it at all.
                    return false;

                case GraphicsDeviceStatus.NotReset:
                    // If device is in the not-reset state, we should try to reset it.
                    deviceNeedsReset = true;
                    break;
            }

            if (deviceNeedsReset)
            {
                graphicsDeviceService.ResetDevice((int)ActualWidth, (int)ActualHeight);
                return false;
            }

            return true;
        }
    }
}
