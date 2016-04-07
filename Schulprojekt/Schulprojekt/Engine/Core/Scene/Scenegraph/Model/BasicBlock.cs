using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Animation_Engine.Engine.Core.MathUtil;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Animation_Engine.Engine.Core.Scene.Scenegraph.Model
{
    public class BasicBlock : SGNode
    {
        private int _index = 0;
        private static int GL_ID = -1;
        private double[] _outData = new double[3];

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        protected override void DisposeManaged()
        {
        }

        protected override void DisposeUnmanaged()
        {
        }

        protected override void OnInit()
        {
        }

        protected override void OnUpdate(float delta)
        {
            Transform.SetY((float)Math.Sin(ExistingTicks * 0.02f + _index) * 4f);
            Transform.SetZ((float)Math.Cos(ExistingTicks * 0.02f + _index) * 4f);
            Transform.SetX(_index / 20f);
        }

        protected override void OnRender()
        {
            if (GL_ID == -1)
            {
                GL_ID = GL.GenLists(1);
                GL.NewList(GL_ID, ListMode.Compile);
                // Black side - FRONT
                GL.Begin(PrimitiveType.Polygon);
                GL.Color3(0.0, 0.0, 0.0);
                GL.Vertex3(0.5, -0.5, -0.5);
                GL.Vertex3(0.5, 0.5, -0.5);
                GL.Vertex3(-0.5, 0.5, -0.5);
                GL.Vertex3(-0.5, -0.5, -0.5);
                GL.End();

                // White side - BACK
                GL.Begin(PrimitiveType.Polygon);
                GL.Color3(1.0, 1.0, 1.0);
                GL.Vertex3(0.5, -0.5, 0.5);
                GL.Vertex3(0.5, 0.5, 0.5);
                GL.Vertex3(-0.5, 0.5, 0.5);
                GL.Vertex3(-0.5, -0.5, 0.5);
                GL.End();

                // Purple side - RIGHT
                GL.Begin(PrimitiveType.Polygon);
                GL.Color3(1.0, 0.0, 1.0);
                GL.Vertex3(0.5, -0.5, -0.5);
                GL.Vertex3(0.5, 0.5, -0.5);
                GL.Vertex3(0.5, 0.5, 0.5);
                GL.Vertex3(0.5, -0.5, 0.5);
                GL.End();

                // Green side - LEFT
                GL.Begin(PrimitiveType.Polygon);
                GL.Color3(0.0, 1.0, 0.0);
                GL.Vertex3(-0.5, -0.5, 0.5);
                GL.Vertex3(-0.5, 0.5, 0.5);
                GL.Vertex3(-0.5, 0.5, -0.5);
                GL.Vertex3(-0.5, -0.5, -0.5);
                GL.End();

                // Blue side - TOP
                GL.Begin(PrimitiveType.Polygon);
                GL.Color3(0.0, 0.0, 1.0);
                GL.Vertex3(0.5, 0.5, 0.5);
                GL.Vertex3(0.5, 0.5, -0.5);
                GL.Vertex3(-0.5, 0.5, -0.5);
                GL.Vertex3(-0.5, 0.5, 0.5);
                GL.End();

                // Red side - BOTTOM
                GL.Begin(PrimitiveType.Polygon);
                GL.Color3(1.0, 0.0, 0.0);
                GL.Vertex3(0.5, -0.5, -0.5);
                GL.Vertex3(0.5, -0.5, 0.5);
                GL.Vertex3(-0.5, -0.5, 0.5);
                GL.Vertex3(-0.5, -0.5, -0.5);
                GL.End();

                GL.EndList();
            }
            GL.CallList(GL_ID);
        }

        protected override void OnDispose()
        {
            
        }

        protected override void OnDisposeUnmanaged()
        {

        }
    }
}
