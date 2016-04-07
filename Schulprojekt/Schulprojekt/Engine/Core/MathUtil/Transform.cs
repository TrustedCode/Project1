using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Animation_Engine.Engine.Core.MathUtil
{
    public class Transform
    {
        private Matrix4 _transformation = Matrix4.Identity;
        private Vector3 _positionVec3 = new Vector3();
        private Vector3 _scale = Vector3.One;
        private Quaternion _rotationQuat = Quaternion.Identity;
        private Vector3 _eulerRotationVec3 = new Vector3();
        private Vector3 _down = new Vector3(0, 1, 0);
        private Vector3 _forward = new Vector3(0, 0, 1);
        private Vector3 _left = new Vector3(1, 0, 0);

        private bool _transformNeedsUpdate = false;
        
        public void Move(Vector3 direction, float distance)
        {
            Move(direction * distance);
        }

        public void Move(Vector3 moveVec)
        {
            _transformNeedsUpdate = true;
            _positionVec3 += moveVec;
        }
        
        public void RotateX(float x)
        {
            _transformNeedsUpdate = true;
            _eulerRotationVec3.X += x;
        }

        public void RotateY(float y)
        {
            _transformNeedsUpdate = true;
            _eulerRotationVec3.Y += y;
        }

        public void RotateZ(float z)
        {
            _transformNeedsUpdate = true;
            _eulerRotationVec3.Z += z;
        }

        public void Rotate(Vector3 vec)
        {
            _transformNeedsUpdate = true;
            _eulerRotationVec3 += vec;
        }

        public void CalculateTransform()
        {
            if (_transformNeedsUpdate)
            {
                _rotationQuat = Quaternion.FromAxisAngle(Vector3.UnitX, _eulerRotationVec3.X*MathHelperExt.TO_RADIANS);
                _rotationQuat *= Quaternion.FromAxisAngle(Vector3.UnitY, _eulerRotationVec3.Y*MathHelperExt.TO_RADIANS);
                _rotationQuat *= Quaternion.FromAxisAngle(Vector3.UnitZ, _eulerRotationVec3.Z*MathHelperExt.TO_RADIANS);

                Vector3 forward = Vector3.UnitZ;
                Vector3 up = Vector3.UnitY;
                Vector3 right = Vector3.UnitX;
                Matrix4 rot = Matrix4.Invert(Matrix4.CreateFromQuaternion(_rotationQuat));
                    // Inverted, to fit into the left-handed coordinate system
                Vector3.TransformVector(ref forward, ref rot, out _forward);
                Vector3.TransformVector(ref up, ref rot, out _down);
                Vector3.TransformVector(ref right, ref rot, out _left);

                _transformation = Matrix4.Mult(Matrix4.Mult(Matrix4.CreateScale(_scale), Matrix4.CreateTranslation(_positionVec3)),Matrix4.CreateFromQuaternion(_rotationQuat));

                _transformNeedsUpdate = false;
            }
        }

        public void LoadMatrix()
        {
            GL.MultMatrix(ref _transformation);    
        }

        public void SetPosition(Vector3 pos)
        {
            _positionVec3 = pos;
            _transformNeedsUpdate = true;
        }

        public void SetX(float x)
        {
            _positionVec3.X = x;
            _transformNeedsUpdate = true;
        }

        public void SetY(float y)
        {
            _positionVec3.Y = y;
            _transformNeedsUpdate = true;
        }

        public void SetZ(float z)
        {
            _positionVec3.Z = z;
            _transformNeedsUpdate = true;
        }

        public void SetScale(float x, float y, float z)
        {
            _scale.X = x;
            _scale.Y = y;
            _scale.Z = z;
            _transformNeedsUpdate = true;
        }

        public Matrix4 TransformationMat4
        {
            get
            {
                if (_transformNeedsUpdate) //Forgot to call the function on your own, mh ?
                {
                    CalculateTransform();
                }
                return _transformation;
            }
        }
        
        public Vector3 ForwardVec3
        {
            get
            {
                if (_transformNeedsUpdate) //Forgot to call the function on your own, mh ?
                {
                    CalculateTransform();
                }
                return _forward;
            }
        }

        public Vector3 DownVec3
        {
            get
            {
                if (_transformNeedsUpdate) //Forgot to call the function on your own, mh ?
                {
                    CalculateTransform();
                }
                return _down;
            }
        }

        public Vector3 LeftVec3
        {
            get
            {
                if (_transformNeedsUpdate) //Forgot to call the function on your own, mh ?
                {
                    CalculateTransform();
                }
                return _left;
            }
        }

        public Vector3 PositionVec3
        {
            get { return _positionVec3; }
        }

        public Quaternion RotationQuat
        {
            get { return _rotationQuat; }
        }

        public Vector3 EulerRotationVec3
        {
            get { return _eulerRotationVec3; }
            set
            {
                _eulerRotationVec3 = value;
                _transformNeedsUpdate = true;
            }
        }
    }
}