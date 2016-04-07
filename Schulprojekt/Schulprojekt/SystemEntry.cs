
#define DIGITALRUNE_PROFILE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Animation_Engine
{
    public static class SystemEntry
    {
        public static bool SYSTEM_SHUTDOWN = false;
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            EngineForm mainWindow = new EngineForm();
            mainWindow.Show();
            Application.Run(mainWindow);
        }
    }
}
