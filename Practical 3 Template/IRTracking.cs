using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WiimoteLib;

namespace Practical_3_Template
{
    class IRTracking
    {
        #region Variables
        const int pointerSize = 10;
        const int boardSize = 50;
        const int powerbarWidth = 50;
        const int powerbarHeight = 150;

        const int estDistanceToSBar = 10;
        const int SBarWidth = 5;

        bool isHoldingButtonA = false;
        bool cheat = false;
        bool thrown = false;

        float x, y, xSensor0, xSensor1, ySensor0, ySensor1;

        int dartsThrown = 0;
        int totalscore = 0;
        double distanceToSensor;
        int cheatdist = 1000;
        
        List<System.Drawing.Point> dart = new List<System.Drawing.Point>();

        Font fontType = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular);
        Graphics g;
        #endregion

        public IRTracking()
        {
        }

        public void Update(float dt)
        {
            if (Globals.WiiMote.WiimoteState.ButtonState.A)
            {
                if (!isHoldingButtonA)
                {
                    x = 1 - Globals.WiiMote.WiimoteState.IRState.IRSensors[0].Position.X;
                    y = Globals.WiiMote.WiimoteState.IRState.IRSensors[0].Position.Y;
                    isHoldingButtonA = true;
                }

                if (distanceToSensor < (cheatdist + 200) && distanceToSensor > (cheatdist - 200) && !thrown)
                    ThrowDart();
                if (distanceToSensor > (cheatdist + 200) || distanceToSensor < (cheatdist - 200) )
                {
                    if (!cheat && isHoldingButtonA)
                    {
                        cheat = true;
                        if (MessageBox.Show("Dont cheat! Haven't you read the rules??", "Cheater!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) == DialogResult.OK)
                            cheat = false;
                    }
                }
            }
            else
            {
                isHoldingButtonA = false;
                thrown = false;
            }

            if (Globals.WiiMote.WiimoteState.ButtonState.One)
                cheatdist += 5;
            if (Globals.WiiMote.WiimoteState.ButtonState.Two)
                cheatdist -= 5;
        }

        public void Draw(float dt)
        {
            g = Globals.Graphics;
            g.Clear(Color.Black);

            // Show how to play info
            g.DrawString("To play: Hold the Wiimote like a dart arrow. Aim at the center and hold the A button.\nMove the Wiimote forward and release the A button.\nFirst move 1 meter away from your sensorbar.\nThe distance should show " + cheatdist + " (mm).\nDont cheat!", fontType, Brushes.White, new System.Drawing.Point(20, 50));
            g.DrawString("Score: " + totalscore, fontType, Brushes.White, new System.Drawing.Point(20, 150));

            // Draw the board, thrown darts and pointer
            paintBoard(g);
            paintDarts(g);
            paintPointer(g);
            
            xSensor0 = Globals.WiiMote.WiimoteState.IRState.IRSensors[0].Position.X;
            xSensor1 = Globals.WiiMote.WiimoteState.IRState.IRSensors[1].Position.X;
            ySensor0 = Globals.WiiMote.WiimoteState.IRState.IRSensors[0].Position.Y;
            ySensor1 = Globals.WiiMote.WiimoteState.IRState.IRSensors[1].Position.Y;

            // Draw debug values
            /*
            g.DrawString("X0: " + Math.Round(xSensor0, 3).ToString(), fontType, Brushes.White, new System.Drawing.Point(20, 210));
            g.DrawString("X1: " + Math.Round(xSensor1, 3).ToString(), fontType, Brushes.White, new System.Drawing.Point(20, 235));
            g.DrawString("Y0: " + Math.Round(ySensor0, 3).ToString(), fontType, Brushes.White, new System.Drawing.Point(20, 250));
            g.DrawString("Y1: " + Math.Round(ySensor1, 3).ToString(), fontType, Brushes.White, new System.Drawing.Point(20, 265));
            g.DrawString("Size: " + Globals.WiiMote.WiimoteState.IRState.IRSensors[0].Size.ToString(), fontType, Brushes.White, new System.Drawing.Point(20, 280));
            g.DrawString("Diff X1-X0: " + Math.Round((xSensor1 - xSensor0), 3).ToString(), fontType, Brushes.White, new System.Drawing.Point(20, 295));
            g.DrawString("Diff Y1-Y0: " + Math.Round((ySensor1 - ySensor0), 3).ToString(), fontType, Brushes.White, new System.Drawing.Point(20, 310));
            */

            // Distance calculation and information
            distanceToSensor = (estDistanceToSBar * SBarWidth) / distanceToCenter((xSensor1 - xSensor0), (ySensor1 - ySensor0));
            g.DrawString("Distance: " + Math.Round(distanceToSensor).ToString(), fontType, Brushes.White, new System.Drawing.Point(20, 330));
            g.DrawString("Threshold: " + cheatdist.ToString(), fontType, Brushes.White, new System.Drawing.Point(20, 350));
            g.DrawString("(use button 1 and 2 \nto manipulate the threshold)", fontType, Brushes.White, new System.Drawing.Point(20, 370));
        }

        // Draw the gameboard
        private void paintBoard(Graphics g)
        {
            for (int x = 10; x >= 0; x--)
            {
                g.FillEllipse(Convert.ToBoolean(x % 2) ? Brushes.White : Brushes.Red,
                    (Globals.Form.ClientSize.Width / 2) - x * (boardSize / 2),
                    (Globals.Form.ClientSize.Height / 2) - x * (boardSize / 2),
                    x * boardSize, x * boardSize);

                g.DrawString((11 - x).ToString(), fontType,
                    Convert.ToBoolean(x % 2) ? Brushes.Black : Brushes.White,
                    (Globals.Form.ClientSize.Width / 2) - 6,
                    (Globals.Form.ClientSize.Height / 2) - x * (boardSize / 2) + 2);

                g.DrawString((11 - x).ToString(), fontType,
                    Convert.ToBoolean(x % 2) ? Brushes.Black : Brushes.White,
                    (Globals.Form.ClientSize.Width / 2) - 6,
                    (Globals.Form.ClientSize.Height / 2) + x * (boardSize / 2) - 22);
            }
        }

        private void paintDarts(Graphics g)
        {
            for (int i = 0; i < dartsThrown; i++)
            {
                g.FillEllipse(Brushes.Black, new Rectangle(dart[i], new Size(20, 20)));
            }
        }

        private void paintPointer(Graphics g)
        {
            if (isHoldingButtonA)
            {
                g.FillEllipse(Brushes.Yellow, x * Globals.Form.ClientSize.Width - (pointerSize / 2), y * Globals.Form.ClientSize.Height - (pointerSize / 2), pointerSize, pointerSize);
                g.FillRectangle(Brushes.Green, Globals.Form.ClientSize.Width - powerbarWidth, Globals.Form.ClientSize.Height - xSensor1 * 100, powerbarWidth, xSensor1 * 100);
            }
            else
                g.FillEllipse(Brushes.Blue, (1 - Globals.WiiMote.WiimoteState.IRState.IRSensors[0].Position.X) * Globals.Form.ClientSize.Width - (pointerSize / 2), Globals.WiiMote.WiimoteState.IRState.IRSensors[0].Position.Y * Globals.Form.ClientSize.Height - (pointerSize / 2), pointerSize, pointerSize);
        }

        public void ThrowDart()
        {
            int dartx = (int)(x * Globals.Form.ClientSize.Width - pointerSize / 2);
            int darty = (int)(y * Globals.Form.ClientSize.Height - pointerSize / 2);

            dart.Add(new System.Drawing.Point(dartx, darty));
            dartsThrown++;
            updateScore(dartx, darty);
            thrown = true;
        }

        public void updateScore(int dartx, int darty)
        {
            double distance = distanceToCenter(Math.Abs((dartx + pointerSize) - Globals.centerX), Math.Abs((darty + pointerSize) - Globals.centerY));

            int score = 10 - (int)(distance / (boardSize * 0.5));
            if (score < 0)
                score = 0;

            totalscore += score;
        }

        // Calculate the shortest distance from point (x, y) to the center using Pythagoras
        private static double distanceToCenter(double x, double y)
        {
            return Math.Sqrt(x * x + y * y);
        }
    }
}
