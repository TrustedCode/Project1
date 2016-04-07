using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalRune.Diagnostics;

namespace Animation_Engine.Engine.Core.Scene.Scenegraph
{
    public class SGRootNode : SGNode
    {
        public SGRootNode(CoreEngine engine, HierarchicalProfiler profiler)
        {
            Init(engine, profiler);
        }

        public override void Update(float delta)
        {
            Profiler.Start("Updating SGNode [" + Id + "]");
            base.Update(delta);
            Profiler.Stop();
        }

        public override void Render()
        {
            Profiler.Start("Rendering SGNode [" + Id + "]");
            base.Render();
            Profiler.Stop();
        }

        protected override void OnInit()
        {
        }

        protected override void OnUpdate(float delta)
        {
        }

        protected override void OnRender()
        {
        }

        protected override void OnDispose()
        { 
        }

        protected override void OnDisposeUnmanaged()
        {
        }
    }
}
