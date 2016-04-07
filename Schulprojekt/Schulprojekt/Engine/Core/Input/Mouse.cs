using System;
using System.Diagnostics;
using System.Windows.Forms;
using OpenTK.Input;

namespace Animation_Engine.Engine.Core.Input
{
    public class Mouse : IInputDevice
    {
        private MouseState current, previous;

        private int _deltaX;
        private int _deltaY;
        private int _wheelDelta;
        private bool _grabbed;
        private InputManager _inputManager;

        public void InitDevice(InputManager manager)
        {
            _inputManager = manager;
        }

        public void Update()
        {
            Current = OpenTK.Input.Mouse.GetState();

            _deltaX = Current.X - previous.X;
            _deltaY = Current.Y - previous.Y;
            _wheelDelta = Current.Wheel - previous.Wheel;
            
            previous = Current;

            if (Grabbed)
            {
                OpenTK.Input.Mouse.SetPosition(_inputManager.InputForm.Location.X + _inputManager.InputForm.Width / 2,
                                                _inputManager.InputForm.Location.Y + _inputManager.InputForm.Height / 2);

            }
        }

        public void SetGrabbed(bool grabbed)
        {
            if (_grabbed == grabbed) return;

            _grabbed = grabbed;
            if (_grabbed)
            {
                _inputManager.InputForm.BeginInvoke(new Action(() => System.Windows.Forms.Cursor.Hide()));
            }
            else
            {
                _inputManager.InputForm.BeginInvoke(new Action(() => System.Windows.Forms.Cursor.Show()));
            }
        }

        public int DeltaX
        {
            get { return _deltaX; }
            set { _deltaX = value; }
        }

        public int DeltaY
        {
            get { return _deltaY; }
            set { _deltaY = value; }
        }

        public int WheelDelta
        {
            get { return _wheelDelta; }
            set { _wheelDelta = value; }
        }

        public MouseState Current
        {
            get {return current;}
            set {current = value;}
        }

        public MouseState Previous
        {
            get { return previous; }
            set { previous = value; }
        }

        public bool Grabbed
        {
            get { return _grabbed; }
        }
    }
}
