using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WiimoteLib;
using System.Drawing;

namespace Practical_3_Template
{
    // A Global Class so you can access these from all classes.
    static class Globals
    {
        public static State state;
        public static Wiimote1 WiiMote;
        public static Form1 Form;
        public static Drumming Drumming;
        public static IRTracking IRTracking;
        public static Graphics Graphics;
    }
    // A State Enum to define different States
    public enum State
    {
        Drumming,
        IRTracking
    }
}
