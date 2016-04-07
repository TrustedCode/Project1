using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animation_Engine.Engine.Core.Input
{
    public interface IInputDevice
    {
        void InitDevice(InputManager manager);

        void Update();
    }
}
