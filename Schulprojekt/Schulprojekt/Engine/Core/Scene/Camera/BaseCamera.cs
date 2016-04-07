using System;
using Animation_Engine.Engine.Core.MathUtil;
using Animation_Engine.Engine.Core.Scene.Camera.Plugins;
using OpenTK;

namespace Animation_Engine.Engine.Core.Scene.Camera
{
    public abstract class BaseCamera : Transform
    {
        protected Matrix4 _projectionMatrix = Matrix4.Identity;

        public Matrix4 ProjectionMatrix
        {
            get
            {
                return _projectionMatrix;
            }
        }
    }
}
