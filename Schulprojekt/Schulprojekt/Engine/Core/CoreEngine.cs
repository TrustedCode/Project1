using System;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Animation_Engine.Engine.Core.Graphics;
using Animation_Engine.Engine.Core.Input;
using Animation_Engine.Engine.Core.Resource.Cache;
using Animation_Engine.Engine.Core.Scene;
using Animation_Engine.Engine.Core.Scene.Camera;
using Animation_Engine.Engine.Core.Util.Threading;
using DigitalRune.Diagnostics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Stopwatch = System.Diagnostics.Stopwatch;

namespace Animation_Engine.Engine.Core
{
    public class CoreEngine
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool Beep(uint dwFreq, uint dwDuration);

        public static readonly HierarchicalProfiler _rootProfiler = new HierarchicalProfiler("Core Profiler");
        private const String PROFILE_ROOT = "_root_engine";

        private RenderingEngine _renderingEngine;
        private InputManager _inputManager;
               
        private SceneManager _sceneManager;
        private ThreadCache _globalCache = new ThreadCache(4);
        private FastParallel _parallelMgr;

        private EngineForm _baseForm;
        private int FPS = 60;

        public CoreEngine()
        {
            _renderingEngine = new RenderingEngine(this);
        }
        
        public void StartEngine(EngineForm form)
        {
            _baseForm = form;
            _inputManager = new InputManager(_baseForm);
            _sceneManager = new SceneManager();
            GCSettings.LatencyMode = GCLatencyMode.LowLatency; // Quick GCs
            new Thread(new ThreadStart(EnterLoop)).Start();
        }

        public void EnterLoop()
        {
            int CACHE_ID = _globalCache.LinkThread();
            Stopwatch watch = new Stopwatch();
            
            _parallelMgr = FastParallel.Instance(12);

            RenderingEngine.InitRenderingEngine(this, _rootProfiler);
            InputManager.InitDevices();
            _rootProfiler.Reset();

            SetupScene();
            while (!SystemEntry.SYSTEM_SHUTDOWN)
            {
                watch.Reset();
                watch.Start();

                _rootProfiler.NewFrame();
                _rootProfiler.Start(PROFILE_ROOT);
                
                InputManager.UpdateDevices(_rootProfiler);
                _sceneManager.UpdateScene(1f / FPS);

                RenderingEngine.Render();

                _rootProfiler.Stop();
                if (InputManager.Keyboard.UpKeys[(int) Key.F1])
                {
                    Console.WriteLine(_rootProfiler.Dump(_rootProfiler.Root, 10));
                }
                Application.DoEvents();
                watch.Stop();
                Thread.Sleep((int)Math.Max(0, (1000f / (float)FPS) - watch.Elapsed.TotalMilliseconds));
            }
            ParallelMgr.Dispose();
            SceneManager.Dispose();
        }

        private void SetupScene()
        {
            _sceneManager.InitScene(this, _rootProfiler);

            PerspectiveCamera camera = new PerspectiveCamera();
            camera.Perspective((float)(70f / 180f * Math.PI), (float)_renderingEngine.RenderingWindow.Width / (float)_renderingEngine.RenderingWindow.Height, 0.1f, 999f);
            _sceneManager.SetCamera(camera);
        }
        public EngineForm GetEngineForm
        {
            get { return _baseForm; }
        }

        public RenderingEngine RenderingEngine
        {
            get
            {
                return _renderingEngine;
            }

            set
            {
                _renderingEngine = value;
            }
        }

        public InputManager InputManager
        {
            get { return _inputManager; }
            set { _inputManager = value; }
        }

        public SceneManager SceneManager
        {
            get { return _sceneManager; }
            set { _sceneManager = value; }
        }

        public ThreadCache GlobalCache
        {
            get { return _globalCache; }
        }

        public FastParallel ParallelMgr
        {
            get { return _parallelMgr; }
        }
    }
}
