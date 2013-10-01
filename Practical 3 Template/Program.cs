using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using WiimoteLib;

namespace Practical_3_Template
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Connect the wiiMote
            Globals.WiiMote = new Wiimote1();
            Globals.WiiMote.Disconnect();
            Globals.WiiMote.Connect();
            Globals.WiiMote.SetLEDs(true, false, false, false);
            Globals.WiiMote.SetReportType(InputReport.IRAccel, true);

            // Creating the Drumming and IRTracking instance
            Globals.Drumming = new Drumming();
            Globals.IRTracking = new IRTracking();

            // Enabling form threads
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
