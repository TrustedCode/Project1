using OpenTK;

namespace Animation_Engine.Engine.Core.Scene.Camera
{
    public class OrthographicCamera : BaseCamera
    {
        public float Left { get; private set; }

        public float Right { get; private set; }

        public float Top { get; private set; }

        public float Bottom { get; private set; }

        public float Near { get; private set; }

        public float Far { get; private set; }

        public void Orthographic(float left, float right, float top, float bottom, float near, float far)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
            Near = near;
            Far = far;

            _projectionMatrix = Matrix4.CreateOrthographicOffCenter(Left, Right, Top, Bottom, Near, Far);
        }
    }
}
