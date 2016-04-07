using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DigitalRune.Diagnostics;

namespace Animation_Engine.Engine.Core.Input
{
    public class InputManager
    {
        private Mouse _mouse;
        private Keyboard _keyboard;
        private EngineForm _inputForm;

        public InputManager(EngineForm inputForm)
        {
            InputForm = inputForm;
            if (InputForm == null)
            {
                throw new ArgumentNullException("Focusable Form cannot be null!");
            }
        }
        
        public void InitDevices()
        {
            _mouse = new Mouse();
            _keyboard = new Keyboard();
            
            Mouse.InitDevice(this);
            Keyboard.InitDevice(this);
        }

        public void UpdateDevices(HierarchicalProfiler profiler)
        {
            if (HasFocus)
            {
                profiler.Start("Updating Input States: Mouse");
                Mouse.Update();
                profiler.Stop();

                profiler.Start("Updating Input States: Mouse");
                Keyboard.Update();
                profiler.Stop();
            }
        }

        public Mouse Mouse
        {
            get
            {
                return _mouse;
            }
        }

        public bool HasFocus
        {
            get
            {
                if (InputForm != null)
                {
                    return InputForm.HasFocus;
                }
                return false;
            }
        }

        public Keyboard Keyboard
        {
            get
            {
                return _keyboard;
            }
        }

        public EngineForm InputForm
        {
            get
            {
                return _inputForm;
            }

            set
            {
                _inputForm = value;
            }
        }
    }
}
