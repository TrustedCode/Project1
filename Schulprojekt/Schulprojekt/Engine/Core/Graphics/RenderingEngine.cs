using System;
using System.Drawing;
using Animation_Engine.Engine.Core.Event.RendererEvents;
using DigitalRune.Diagnostics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Animation_Engine.Engine.Core.Scene;

namespace Animation_Engine.Engine.Core.Graphics
{
    public class RenderingEngine
    {
        private const String PROFILE_RENDERING_ROOT = "_root_rendering";

#region EVENTS
        public event EventHandler<EventRenderTargetResizedArgs> RenderTargetResizedEvent;
        private EventRenderTargetResizedArgs _tempRenderTargetResizedArgs = new EventRenderTargetResizedArgs(0, 0, 0, 0);
#endregion

        private CoreEngine _engine;
        private HierarchicalProfiler _profiler;

        private GLControl _renderingWindow;
        private CoreEngine _coreEngine;
        private int _previousRenderWidth;
        private int _previousRenderHeight;
        private int _currentRenderWidth;
        private int _currentRenderHeight;

        public RenderingEngine(CoreEngine engine)
        {
            _coreEngine = engine;
        }

        public void SetRenderingTarget(GLControl window)
        {
            _renderingWindow = window;
        }

        public void InitRenderingEngine(CoreEngine engine, HierarchicalProfiler profiler)
        {
            Engine = engine;
            Profiler = profiler;
            _renderingWindow.MakeCurrent();    
        }

        public void Render()
        {
            try
            {
                if (_renderingWindow.Context != null && RenderingWindow.Context.IsCurrent)
                {
                    Profiler.Start(PROFILE_RENDERING_ROOT);

                    CheckRenderBounds();

                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    GL.ClearColor(Color.SkyBlue);

                    Engine.SceneManager.RenderScene();
                    _renderingWindow.SwapBuffers();
                    Profiler.Stop();
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                Profiler.Stop();
            }
        }

        /// <summary>
        /// Checks if the window was resized and raises an event
        /// </summary>
        private void CheckRenderBounds()
        {
            _previousRenderWidth = _currentRenderWidth;
            _previousRenderHeight = _currentRenderHeight;

            _currentRenderWidth = _renderingWindow.Width;
            _currentRenderHeight = _renderingWindow.Height;

            if (_previousRenderHeight == 0 && _previousRenderWidth == 0)
            {
                return;
            }
            if (_previousRenderWidth != _currentRenderWidth || _previousRenderHeight != _currentRenderHeight)
            {
                _tempRenderTargetResizedArgs.Width = _currentRenderWidth;
                _tempRenderTargetResizedArgs.Height = _currentRenderHeight;
                _tempRenderTargetResizedArgs.PrevWidth = _previousRenderWidth;
                _tempRenderTargetResizedArgs.PrevHeight = _previousRenderHeight;

                EventHandler<EventRenderTargetResizedArgs> handler = RenderTargetResizedEvent;
                if (handler != null)
                {
                    handler(this, _tempRenderTargetResizedArgs);
                }
            }
        }

        public GLControl RenderingWindow
        {
            get
            {
                return _renderingWindow;
            }

            set
            {
                _renderingWindow = value;
            }
        }

        public int PreviousRenderWidth
        {
            get { return _previousRenderWidth; }
        }

        public int PreviousRenderHeight
        {
            get { return _previousRenderHeight; }
        }

        public int CurrentRenderWidth
        {
            get { return _currentRenderWidth; }
        }

        public int CurrentRenderHeight
        {
            get { return _currentRenderHeight; }
        }
        
        protected internal HierarchicalProfiler Profiler
        {
            get { return _profiler; }
            set { _profiler = value; }
        }

        protected internal CoreEngine Engine
        {
            get { return _engine; }
            set { _engine = value; }
        }
    }
}
