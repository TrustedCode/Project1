using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace Animation_Engine.Engine.Core.Input
{
    public class Keyboard : IInputDevice
    {
        private KeyboardState _current, _previous;
        private bool[] _keys = new bool[132];
        private bool[] _downKeys = new bool[132];
        private bool[] _upKeys = new bool[132];
        
        public void InitDevice(InputManager manager)
        {
        }

        public void Update()
        {
            _current = OpenTK.Input.Keyboard.GetState();
            if (_current != _previous)
            {
                /*Key downs*/
                for (int i = 0; i < 132; i++)
                {
                    _keys[i] = _current[(Key) i];
                    if (_current.IsKeyDown((Key)i) && !_previous.IsKeyDown((Key)i))
                    {
                        _downKeys[i] = true;
                    }
                    else
                    {
                        _downKeys[i] = false;
                    }
                }

                /*Key ups*/
                for (int i = 0; i < 132; i++)
                {
                    if (!_current.IsKeyUp((Key)i) && _previous.IsKeyUp((Key)i))
                    {
                        _upKeys[i] = true;
                    }
                    else
                    {
                        _upKeys[i] = false;
                    }
                }
            }
            _previous = _current;
        }

        public bool[] DownKeys
        {
            get { return _downKeys; }
            set { _downKeys = value; }
        }

        public bool[] UpKeys
        {
            get { return _upKeys; }
            set { _upKeys = value; }
        }

        public bool[] Keys
        {
            get { return _keys; }
            set { _keys = value; }
        }
    }
}
