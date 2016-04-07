using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animation_Engine.Engine.Log
{
    public class Log
    {
    }

    enum LogState : int
    {
        DEFAULT = 0,
        WARNING = 14,
        FATAL = 12
    }

    [FlagsAttribute]
    enum LogType : byte
    {
        DEFAULT = 0, /* Main Thread */
        GRAPHICS = 1, /* GPU allocating routines */
        LOGIC = 2, /* Physics or other logical calculations */
        EVENT = 3, /* Event distribution */
        RESOURCE = 4, /* Resource loading */
        HIDDEN = 5, /* Seperate thread */
        DEBUG = 6 /* In debugging mode */
    }
}
