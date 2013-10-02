using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;

namespace Practical_3_Template
{
    class Drumming
    {
        // This is just an example, you can empty all the methods

        PointF circlePosition;
        float xSpeed;
        const int pointerSize = 10;

        SoundPlayer drum1, drum2, drum3;
        bool hit = false;
        Image drumKit = Image.FromFile("drumkit.png", true);

        public Drumming()
        {
            drum1 = new SoundPlayer();
            drum2 = new SoundPlayer();
            drum3 = new SoundPlayer();
            drum1.SoundLocation = "shortsn.WAV";
            drum2.SoundLocation = "Miami_Conga1.wav";
            drum3.SoundLocation = "Miami_Rim1.wav";


            circlePosition = new PointF(200, 100);
            xSpeed = 200;
        }

        // Put your update loop here.
        public void Update(float dt)
        {
            //System.Threading.Thread.Sleep(50);
            if (Globals.roundValuesX == -1 && Globals.roundValuesY == 0 && hit)
            {
                drum1.Play();
                hit = false;
            }
            if (Globals.roundValuesX == 1 && Globals.roundValuesY == 0 && hit)
            {
                drum2.Play();
                hit = false;
            }
            if (Globals.roundValuesX == 0 && Globals.roundValuesY == 0 && hit)
            {
                drum3.Play();
                hit = false;
            }
            if (Globals.roundValuesX == 0 && Globals.roundValuesY == -1)
                hit = true;
            





            circlePosition.X += xSpeed * dt;

            // Check for collisions with the walls
            if (circlePosition.X < 0 || circlePosition.X + 100 > Globals.Form.ClientSize.Width)
                xSpeed *= -1;
        }

        public void Draw(float dt)
        {
            // Create the graphics object so we can draw
            Graphics g = Globals.Graphics;
            

            // Clear the screen
            g.Clear(Color.Black);
            g.DrawImage(drumKit, 0, 0);
            g.FillEllipse(Brushes.Red, circlePosition.X, circlePosition.Y, 50, 50);
            g.FillEllipse(Brushes.White, Globals.WiiMote.WiimoteState.IRState.IRSensors[0].Position.X * Globals.Form.ClientSize.Width - (pointerSize / 2),
                Globals.WiiMote.WiimoteState.IRState.IRSensors[0].Position.Y * Globals.Form.ClientSize.Height - (pointerSize / 2), pointerSize, pointerSize);
        }
    }
}
