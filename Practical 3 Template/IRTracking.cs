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
        const int pointerSize = 10;
        const int boardSize = 50;
        const int powerbarWidth = 50;
        const int powerbarHeight = 150;

        bool isHoldingButtonA = false;
        bool wasHoldingButtonA = false;
        bool buttonA;

        float x, y, xSensor0, xSensor1, ySensor0, ySensor1;

        int dartsThrown = 0;
        int totalscore = 0;
        int centerX = 5; //Globals.Form.ClientSize.Width / 2;
        int centerY = 5; //Globals.Form.ClientSize.Height / 2;
        System.Drawing.Point[] dart = new System.Drawing.Point[20];

        Font fontType = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular);


        public IRTracking()
        {
            Globals.WiiMote.WiimoteChanged += WiimoteChanged;
        }

        public void Update(float dt)
        {
            if (Globals.WiiMote.WiimoteState.ButtonState.A)
            {
                if (!isHoldingButtonA)
                {
                    x = 1 - Globals.WiiMote.WiimoteState.IRState.IRSensors[0].Position.X;
                    y = Globals.WiiMote.WiimoteState.IRState.IRSensors[0].Position.Y;
                }

                isHoldingButtonA = true;

                if (Globals.WiiMote.WiimoteState.AccelState.Values.Z > 1.5)
                {
                    ThrowDart();
                }
            }
            else
                isHoldingButtonA = false;

        }

        public void Draw(float dt)
        {
            Graphics g = Globals.Graphics;

            g.Clear(Color.Black);

            // Youri: g.DrawImage(Image.FromFile(@"Wall.jpg", true), 0, 0);

            // Show how to play info
            g.DrawString("To play: Hold the Wiimote like a dart arrow. Aim at the center and hold the A button. Now move the Wiimote forward and release the A button.", fontType, Brushes.White, new System.Drawing.Point(20, 50));
            g.DrawString("Score: " + totalscore, fontType, Brushes.White, new System.Drawing.Point(20, 65));

            // Draw the board, thrown darts and pointer
            paintBoard(g);
            paintDarts(g);
            paintPointer(g);

            // Draw the debug stuff
            xSensor0 = Globals.WiiMote.WiimoteState.IRState.IRSensors[0].Position.X;
            xSensor1 = Globals.WiiMote.WiimoteState.IRState.IRSensors[1].Position.X;
            ySensor0 = Globals.WiiMote.WiimoteState.IRState.IRSensors[0].Position.Y;
            ySensor1 = Globals.WiiMote.WiimoteState.IRState.IRSensors[1].Position.Y;
            g.DrawString("X0: " + Math.Round(xSensor0, 3).ToString(), fontType, Brushes.White, new System.Drawing.Point(20, 150));
            g.DrawString("X1: " + Math.Round(xSensor1, 3).ToString(), fontType, Brushes.White, new System.Drawing.Point(20, 165));
            g.DrawString("Y0: " + Math.Round(ySensor0, 3).ToString(), fontType, Brushes.White, new System.Drawing.Point(20, 180));
            g.DrawString("Y1: " + Math.Round(ySensor1, 3).ToString(), fontType, Brushes.White, new System.Drawing.Point(20, 195));
            g.DrawString("Size: " + Globals.WiiMote.WiimoteState.IRState.IRSensors[0].Size.ToString(), fontType, Brushes.White, new System.Drawing.Point(20, 210));
            g.DrawString("Diff X1-X0: " + Math.Round((xSensor1 - xSensor0), 3).ToString(), fontType, Brushes.White, new System.Drawing.Point(20, 225));
            g.DrawString("Diff Y1-Y0: " + Math.Round((ySensor1 - ySensor0), 3).ToString(), fontType, Brushes.White, new System.Drawing.Point(20, 240));
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
            int dartx = (int)x * Globals.Form.ClientSize.Width - pointerSize;
            int darty = (int)y * Globals.Form.ClientSize.Height - pointerSize;

            dart[dartsThrown] = new System.Drawing.Point(dartx, darty);
            dartsThrown++;

            updateScore(dartx, darty);
        }

        public void updateScore(int dartx, int darty)
        {
            double distance = distanceToCenter(Math.Abs(dartx - centerX), Math.Abs(darty - centerY));

            int score = 10 - (int)distance / boardSize;
            if (score < 0)
                score = 0;

            totalscore += score;
        }

        // Calculate the shortest distance from point (x, y) to the center using Pythagoras
        private static double distanceToCenter(double x, double y)
        {
            return Math.Sqrt(x * x + y * y);
        }

        /*
        
        // Calculate the distance to the sensorbar
        private static float calculateDepth()
        {
            float xSensor0, xSensor1, ySensor0, ySensor1;

            xSensor0 = Globals.WiiMote.WiimoteState.IRState.IRSensors[0].Position.X;
            ySensor0 = Globals.WiiMote.WiimoteState.IRState.IRSensors[0].Position.Y;
            xSensor1 = Globals.WiiMote.WiimoteState.IRState.IRSensors[1].Position.X;
            ySensor1 = Globals.WiiMote.WiimoteState.IRState.IRSensors[1].Position.Y;

            return xSensor1;
        }
         
         */

        private void WiimoteChanged(object sender, WiimoteChangedEventArgs e)
        {
            A = e.WiimoteState.ButtonState.A;

        }

        public bool A { set { buttonA = value; } }
    }
}
