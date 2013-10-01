using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Practical_3_Template
{
    class Drumming
    {
        // This is just an example, you can empty all the methods

        PointF circlePosition;
        float xSpeed;

        public Drumming()
        {
            circlePosition = new PointF(200, 100);
            xSpeed = 200;
        }

        // Put your update loop here.
        public void Update(float dt)
        {
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
            g.Clear(Color.CornflowerBlue);
            g.FillEllipse(Brushes.Red, circlePosition.X, circlePosition.Y, 100, 100);
        }
    }
}
