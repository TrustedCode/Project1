using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Animation_Engine.Engine.Core.Scene.Scenegraph;
using Animation_Engine.Engine.Core.Util.Threading;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK;

namespace Animation_Engine.Engine.Core.Scene.Scenegraph.FX.WaterFoil
{
    public class WaterFoil : SGNode
    {
        private FastParallel _parallel;
        private ParallelState _parallelState;
        private Action<Int32> _parallelWorker;

        private WaterFoilPatch[] _backPatches;
        private WaterFoilPatch[] _currentPatches;

        private int _patchWidth;
        private int _patchHeight;
        private int _threads;

        private float _fuidFactor = 1f;
        private float _distributionFactor = 1f;

        public WaterFoil(int width, int height, int threads)
        {
            _patchWidth = width;
            _patchHeight = height;
            if (_patchWidth % 2 != 0)
            {
                Console.WriteLine("ERROR \"width\" must be power of 2!");
                _patchWidth++;
            }
            if (_patchHeight % 2 != 0)
            {
                Console.WriteLine("ERROR \"height\" must be power of 2!");
                _patchHeight++;
            }
            _threads = threads;
        }
        
        protected override void OnInit()
        {
            _backPatches = new WaterFoilPatch[_patchWidth * _patchHeight];
            _currentPatches = new WaterFoilPatch[_patchWidth * _patchHeight];
            if (_threads != 1)
            {
                _parallel = FastParallel.Instance(_threads);
                _parallelWorker = new Action<Int32>(CalculateWaves);
                _parallelState = _parallel.CreateState(0, _backPatches.Length, _parallelWorker);
            }
            InitDefaultWaterFoil();
        }

        private void InitDefaultWaterFoil()
        {
            for(int i = 0; i < _backPatches.Length; i++)
            {
                _backPatches[i].CurrentHeight = 1;
                _backPatches[i].IsSolid = false;
                _backPatches[i].MinimalHeight = 0;
            }
            SwapBuffers();
        }

        private void SwapBuffers()
        {
            Array.Copy(_currentPatches, 0, _backPatches, 0, _currentPatches.Length);
        }

        protected override void OnUpdate(float delta)
        {
            if (_threads != 1)
            {
                _parallel.Loop(_parallelState);
            }
            else
            {
                for (int i = 0; i < _currentPatches.Length; i++)
                {
                    CalculateWaves(i);
                }
            }

            if (Engine.InputManager.Keyboard.DownKeys[(int)Key.V])
            {
                _currentPatches[32 * 32].VerticalImpuls = 3f;
            }
            SwapBuffers();
        }

        /// <summary>
        /// Inline should improve performance significantly
        /// </summary>
        /// <param name="index"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CalculateWaves(int index)
        {
            float x = (float)(index / _patchWidth);
            float y = (float)(index % _patchWidth);

            //_currentPatches[index].VerticalImpuls += _currentPatches[index].VerticalPreImpuls; //Apply stored impuls
            //_currentPatches[index].VerticalPreImpuls = 0; // Clear

            int verticalSign = -Math.Sign(_currentPatches[index].VerticalImpuls);
            float waveNegDirection = (verticalSign * _currentPatches[index].VerticalImpuls * 0.1f) + _currentPatches[index].VerticalImpuls;

            if(_currentPatches[index].DirectionalWaveForce.X > 1 || _currentPatches[index].DirectionalWaveForce.Y > 1)
            {
               _currentPatches[index].DirectionalWaveForce.Normalize();
            }
            if(_currentPatches[index].DirectionalWaveForce.Length < 0.001)
            {
                _currentPatches[index].DirectionalWaveForce = Vector2.Multiply(_currentPatches[index].DirectionalWaveForce, 0);
            }
            
            if(index == 1024)
            {
                Console.WriteLine(waveNegDirection);
            }
            if(_currentPatches[index].VerticalImpuls < 0.001f)
            {
                _currentPatches[index].VerticalImpuls = 0;
            }
            _currentPatches[index].CurrentHeight = waveNegDirection;
            if (waveNegDirection != 0)
            {
                // RIGHT
                if (index < _currentPatches.Length - 1 && _currentPatches[index].DirectionalWaveForce.X <= 0) // make <= 0 to apply impuls forces
                {
                    _currentPatches[index + 1].VerticalImpuls = waveNegDirection;
                    _currentPatches[index + 1].DirectionalWaveForce += new Vector2(1, 0);
                }
                // LEFT
                if (index > 0 && _currentPatches[index].DirectionalWaveForce.X >= 0)
                {
                    _currentPatches[index - 1].VerticalImpuls = waveNegDirection;
                    _currentPatches[index - 1].DirectionalWaveForce += new Vector2(-1, 0);
                }
                // DOWN
                if (index < _currentPatches.Length - _patchWidth - 1 && _currentPatches[index].DirectionalWaveForce.Y >= 0)
                {
                    _currentPatches[index + _patchWidth].VerticalImpuls = waveNegDirection;
                    _currentPatches[index + _patchWidth].DirectionalWaveForce += new Vector2(0, 1);
                }
                //UP
                if (index > _patchWidth && _currentPatches[index].DirectionalWaveForce.Y <= 0)
                {
                    _currentPatches[index - _patchWidth].VerticalImpuls = waveNegDirection;
                    _currentPatches[index - _patchWidth].DirectionalWaveForce += new Vector2(0, -1);
                }
                _currentPatches[index].VerticalImpuls *= 0.99f;
                _currentPatches[index].DirectionalWaveForce = Vector2.Multiply(_currentPatches[index].DirectionalWaveForce, 0.5f); // Make vec it smaller

            }

        }
        

        protected override void OnRender()
        { 
            GL.Color3(1f,0f,0f);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.Begin(PrimitiveType.Quads);
            for (int i = 0; i < _patchHeight - 1; i++)
            {
                for (int j = 0; j < _patchWidth - 1; j++)
                {
                    int index = i*_patchWidth + j;
                    int index2 = (i + 1) * _patchWidth + j;
                    int index3 = (i + 1) * _patchWidth + j+ 1;
                    int index4 = (i) * _patchWidth + j + 1;
                    GL.Vertex3(j, _backPatches[index].CurrentHeight,i);
                    GL.Vertex3(j, _backPatches[index2].CurrentHeight, i + 1);
                    GL.Vertex3(j + 1, _backPatches[index3].CurrentHeight, i + 1);
                    GL.Vertex3(j + 1, _backPatches[index4].CurrentHeight, i);
                }
            }
            GL.End();
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }

        protected override void OnDispose()
        {
            _parallel.Dispose();
        }

        protected override void OnDisposeUnmanaged()
        {
        }

        public float FuidFactor
        {
            get
            {
                return _fuidFactor;
            }

            set
            {
                _fuidFactor = value;
            }
        }

        public float DistributionFactor
        {
            get
            {
                return _distributionFactor;
            }

            set
            {
                _distributionFactor = value;
            }
        }
    }
}
