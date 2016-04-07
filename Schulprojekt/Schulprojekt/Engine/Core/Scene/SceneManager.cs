using System;
using Animation_Engine.Engine.Core.Graphics;
using Animation_Engine.Engine.Core.Scene.Camera;
using DigitalRune.Diagnostics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Animation_Engine.Engine.Core.MathUtil;
using Animation_Engine.Engine.Core.Resource;
using Animation_Engine.Engine.Core.Scene.Scenegraph;
using Animation_Engine.Engine.Core.Scene.Scenegraph.FX.WaterFoil;
using Animation_Engine.Engine.Core.Scene.Scenegraph.Model;
using OpenTK.Input;

namespace Animation_Engine.Engine.Core.Scene
{
    public class SceneManager : DisposableResource
    {
        private CoreEngine _engine;
        private RenderingEngine _renderingEngine;
        private HierarchicalProfiler _profiler;

        private SGRootNode _rootNode;
        private WaterFoil _water;
        

        private BaseCamera _baseCamera = new PerspectiveCamera();
        
        public void SetCamera(BaseCamera camera)
        {
            _baseCamera = camera;
        }

        public void InitScene(CoreEngine engine, HierarchicalProfiler profiler)
        {
            Engine = engine;
            RenderingEngine = Engine.RenderingEngine;
            Profiler = profiler;

            _rootNode = new SGRootNode(Engine, Profiler);
            Random rand = new Random();
            BasicBlock block = null;
            for (int i = 0; i < 1000; i++)
            {
                float distribution = (float)i/50;
                block = new BasicBlock();
                block.Transform.SetScale(1, 1, 1);
                block.Index = i;
                RootNode.Add(block);
            }

            _water = new WaterFoil(64,64,4);
            //RootNode.Add(_water);
        }

        public void UpdateScene(float delta)
        {
            Profiler.Start("Processing Camera Input");
            {
                if (Engine.InputManager.Keyboard.Keys[(int) Key.W])
                {
                    _baseCamera.Move(_baseCamera.ForwardVec3, 1);
                }

                if (Engine.InputManager.Keyboard.Keys[(int) Key.S])
                {
                    _baseCamera.Move(_baseCamera.ForwardVec3*-1, 1);
                }

                if (Engine.InputManager.Keyboard.Keys[(int) Key.A])
                {
                    _baseCamera.Move(_baseCamera.LeftVec3, 1);
                }

                if (Engine.InputManager.Keyboard.Keys[(int) Key.D])
                {
                    _baseCamera.Move(_baseCamera.LeftVec3*-1, 1);
                }

                if (Engine.InputManager.Keyboard.Keys[(int) Key.Space])
                {
                    _baseCamera.Move(_baseCamera.DownVec3*-1, 1);
                }

                if (Engine.InputManager.Keyboard.Keys[(int) Key.LShift])
                {
                    _baseCamera.Move(_baseCamera.DownVec3, 1);
                }

                Profiler.Start("Transforming Camera");
                {
                    _baseCamera.RotateX((float) Engine.InputManager.Mouse.DeltaY*delta*10f);
                    _baseCamera.RotateY((float) Engine.InputManager.Mouse.DeltaX*delta*10f);
                    _baseCamera.CalculateTransform();
                }
                Profiler.Stop();
            }
            Profiler.Stop();
            RootNode.Update(delta);
            
        }

        public void RenderScene()
        {
            Profiler.Start("Setting up rendering Matrices");
            {
                Matrix4 viewMatrix = Matrix4.Identity;
                Matrix4 projectionMatrix = Matrix4.Identity;

                if (_baseCamera != null)
                {
                    viewMatrix = _baseCamera.TransformationMat4;
                    projectionMatrix = _baseCamera.ProjectionMatrix;
                }

                /* PROJECTION INITIALIZATION */
                int w = RenderingEngine.RenderingWindow.Width;
                int h = RenderingEngine.RenderingWindow.Height;

                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();
                GL.LoadMatrix(ref projectionMatrix);
                GL.Viewport(0, 0, w, h); // Use all of the glControl painting area

                /* TRANSFORMATION INITIALIZATION */
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadMatrix(ref viewMatrix);
            }
            Profiler.Stop();

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            
            RootNode.Render();
        }

        protected internal RenderingEngine RenderingEngine
        {
            get { return _renderingEngine; }
            set { _renderingEngine = value; }
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

        public SGRootNode RootNode
        {
            get { return _rootNode; }
        }

        protected override void DisposeManaged()
        {
            RootNode.Dispose();
        }

        protected override void DisposeUnmanaged()
        {

        }
    }
}
