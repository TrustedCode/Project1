using OpenTK;

namespace Animation_Engine.Engine.Core.Scene.Camera
{
    public class PerspectiveCamera : BaseCamera
    {
        private float _fov;
        private float _aspect;
        private float _near;
        private float _far;

        public void Perspective(float fov, float aspect, float near, float far)
        {
            _fov = fov;
            _aspect = aspect;
            _near = near;
            _far = far;
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(fov, aspect, near, far);
        }

        public float Fov
        {
            get
            {
                return _fov;
            }
        }

        public float Aspect
        {
            get
            {
                return _aspect;
            }
        }

        public float Near
        {
            get
            {
                return _near;
            }
        }

        public float Far
        {
            get
            {
                return _far;
            }
        }
    }
}
