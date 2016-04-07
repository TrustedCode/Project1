#define USE_PARALLEL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Animation_Engine.Engine.Core.Graphics;
using Animation_Engine.Engine.Core.MathUtil;
using DigitalRune.Diagnostics;
using Animation_Engine.Engine.Core.Resource;
using Animation_Engine.Engine.Core.Util.Threading;
using static OpenTK.Graphics.OpenGL.GL;

namespace Animation_Engine.Engine.Core.Scene.Scenegraph
{
    public abstract class SGNode : DisposableResource
    {
#if USE_PARALLEL
        private ParallelState _parallelState;

        const int MIN_CHILDREN_TILL_PARALLEL = 10024;
#else
        const int MIN_CHILDREN_TILL_PARALLEL = Int32.MaxValue;
#endif
        private readonly List<SGNode> _children = new List<SGNode>();
        private CoreEngine _engine;
        private HierarchicalProfiler _hierarchicalProfiler;
        private Transform _transform = new Transform();
        private SGNode _parent;
        private Int32 _id;
        private int _existingTicks = 0;

        public void Add(SGNode node)
        {
            node._parent = this;
            _children.Add(node);
            node.Init(_engine, _hierarchicalProfiler);
        }

        protected void Init(CoreEngine engine, HierarchicalProfiler profiler)
        {
            Engine = engine;
            Profiler = profiler;

            _id = MathHelperExt.RANDOM.Next();

            OnInit();
            foreach (SGNode node in _children)
            {
                node.Init(Engine, Profiler);
            }
        }
      
        public virtual void Update(float delta)
        {
            _existingTicks++;
            OnUpdate(delta);

            if (_children.Count >= MIN_CHILDREN_TILL_PARALLEL)
            {
                if (_parallelState == null)
                {
                    _parallelState = Engine.ParallelMgr.CreateState(0, _children.Count, (index) => { _children[index].Update(delta); });
                }
                Engine.ParallelMgr.Loop(_parallelState);
            }
            else
            {
                foreach (SGNode node in _children)
                {
                    node.Update(delta);
                }
            }
            Transform.CalculateTransform();
        }

        public virtual void Render()
        {
            PushMatrix();
            Transform.LoadMatrix();
            OnRender();
            foreach (SGNode node in _children)
            {
                node.Render();
            }
            PopMatrix();
        }

        protected abstract void OnInit();
        protected abstract void OnUpdate(float delta);
        protected abstract void OnRender();
        protected abstract void OnDispose();
        protected abstract void OnDisposeUnmanaged();

        protected CoreEngine Engine
        {
            get { return _engine; }
            set { _engine = value; }
        }

        protected HierarchicalProfiler Profiler
        {
            get { return _hierarchicalProfiler; }
            set { _hierarchicalProfiler = value; }
        }

        public SGNode Parent
        {
            get { return _parent; }
        }

        public Transform Transform
        {
            get { return _transform; }
            set { _transform = value; }
        }

        public int Id
        {
            get { return _id; }
        }

        public int ExistingTicks
        {
            get { return _existingTicks; }
        }

        protected override void DisposeManaged()
        {
            OnDispose();
            foreach (SGNode node in _children)
            {
                node.DisposeManaged();
            }
        }

        protected override void DisposeUnmanaged()
        {
            OnDisposeUnmanaged();
            foreach (SGNode node in _children)
            {
                node.DisposeUnmanaged();
            }
        }
    }
}
