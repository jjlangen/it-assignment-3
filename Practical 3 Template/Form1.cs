using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WiimoteLib;

namespace Practical_3_Template
{
    public partial class Form1 : Form
    {
        // Init
        public Form1()
        {
            InitializeComponent();
            Init();
        }

        // Load
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Practical 2 - Drumming and IR Tracking";
            Globals.state = State.Drumming;
            Globals.Form = this;
        }

        // Change State
        private void btnChange_Click(object sender, EventArgs e)
        {
            if (Globals.state == State.IRTracking)
            {
                btnChange.Text = "Drumming";
                Globals.state = State.Drumming;
            }
            else
            {
                btnChange.Text = "IR Tracking";
                Globals.state = State.IRTracking;
            }
        }

        private void Update_Tick(object sender, EventArgs e)
        {
            // Calculates the time difference between the last frame and the current frame
            CalculateDT();

            // Print the values on the labels
            labelIRValues.Text = Globals.WiiMote.WiimoteState.IRState.IRSensors[0].Found.ToString();
            Globals.roundValuesX = Math.Round(Globals.WiiMote.WiimoteState.AccelState.Values.X);
            labelAcc.Text = Globals.roundValuesX.ToString();
            Globals.roundValuesY = Math.Round(Globals.WiiMote.WiimoteState.AccelState.Values.Y);
            labelAcc2.Text = Globals.roundValuesY.ToString();

            switch (Globals.state)
            {
                case State.Drumming:
                    Globals.Drumming.Update(dt);
                    Globals.Drumming.Draw(dt);
                    break;
                case State.IRTracking:
                    Globals.IRTracking.Update(dt);
                    Globals.IRTracking.Draw(dt);
                    break;
            }            
            // Renders the screen
            graphics.Render(Graphics.FromHwnd(this.Handle));
        }
    }
}
