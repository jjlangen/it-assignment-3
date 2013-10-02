using System;
using System.Drawing;
using System.Windows.Forms;

namespace Practical_3_Template
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Move along, nothing to see here
        DateTime prev;
        float dt;
        BufferedGraphics graphics;
        BufferedGraphicsContext context;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void OnResize(object sender, EventArgs e)
        {
            // Re-create the graphics buffer for a new window size.
            context.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
            if (graphics != null)
            {
                graphics.Dispose();
                graphics = null;
            }
            graphics = context.Allocate(this.CreateGraphics(),
                new Rectangle(0, 0, this.Width, this.Height));
            graphics.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Globals.Graphics = graphics.Graphics;
            this.Refresh();
        }

        private void Init()
        {
            prev = DateTime.Now;
            DoubleBuffered = true;
            context = BufferedGraphicsManager.Current;
            context.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
            graphics = context.Allocate(this.CreateGraphics(),
                 new Rectangle(0, 0, this.Width, this.Height));
            graphics.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Globals.Graphics = graphics.Graphics;
            this.Resize += new EventHandler(OnResize);
            //this.Paint += new PaintEventHandler(OnPaint);
        }

        void CalculateDT()
        {
            TimeSpan s = DateTime.Now.Subtract(prev);
            dt = (float)s.TotalSeconds;
            prev = DateTime.Now;
        }

        void OnPaint(object sender, PaintEventArgs e)
        {
            graphics.Render(graphics.Graphics);
        }
        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnChange = new System.Windows.Forms.Button();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.labelAcc = new System.Windows.Forms.Label();
            this.labelIRValues = new System.Windows.Forms.Label();
            this.labelAcc2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnChange
            // 
            this.btnChange.Location = new System.Drawing.Point(13, 13);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(75, 23);
            this.btnChange.TabIndex = 0;
            this.btnChange.Text = "Drumming";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Enabled = true;
            this.tmrUpdate.Interval = 20;
            this.tmrUpdate.Tick += new System.EventHandler(this.Update_Tick);
            // 
            // labelAcc
            // 
            this.labelAcc.AutoSize = true;
            this.labelAcc.Location = new System.Drawing.Point(218, 13);
            this.labelAcc.Name = "labelAcc";
            this.labelAcc.Size = new System.Drawing.Size(35, 13);
            this.labelAcc.TabIndex = 4;
            this.labelAcc.Text = "label1";
            // 
            // labelIRValues
            // 
            this.labelIRValues.AutoSize = true;
            this.labelIRValues.Location = new System.Drawing.Point(138, 13);
            this.labelIRValues.Name = "labelIRValues";
            this.labelIRValues.Size = new System.Drawing.Size(35, 13);
            this.labelIRValues.TabIndex = 3;
            this.labelIRValues.Text = "label1";
            // 
            // labelAcc2
            // 
            this.labelAcc2.AutoSize = true;
            this.labelAcc2.Location = new System.Drawing.Point(306, 13);
            this.labelAcc2.Name = "labelAcc2";
            this.labelAcc2.Size = new System.Drawing.Size(35, 13);
            this.labelAcc2.TabIndex = 5;
            this.labelAcc2.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 662);
            this.Controls.Add(this.labelAcc2);
            this.Controls.Add(this.labelAcc);
            this.Controls.Add(this.labelIRValues);
            this.Controls.Add(this.btnChange);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.Timer tmrUpdate;
        private Label labelAcc;
        private Label labelIRValues;
        private Label labelAcc2;
    }
}

