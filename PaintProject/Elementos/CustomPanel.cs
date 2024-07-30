using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaintProject.Elementos
{
    public class CustomPanel : Panel
    {
        // Fields
        private bool showTopBorder = true;
        private bool showBottomBorder = true;
        private bool showLeftBorder = true;
        private bool showRightBorder = true;
        private int borderSize = 1;
        private Color borderColor = Color.Black;

        // Properties
        [Category("Custom Appearance")]
        public bool ShowTopBorder
        {
            get { return showTopBorder; }
            set { showTopBorder = value; this.Invalidate(); }
        }

        [Category("Custom Appearance")]
        public bool ShowBottomBorder
        {
            get { return showBottomBorder; }
            set { showBottomBorder = value; this.Invalidate(); }
        }

        [Category("Custom Appearance")]
        public bool ShowLeftBorder
        {
            get { return showLeftBorder; }
            set { showLeftBorder = value; this.Invalidate(); }
        }

        [Category("Custom Appearance")]
        public bool ShowRightBorder
        {
            get { return showRightBorder; }
            set { showRightBorder = value; this.Invalidate(); }
        }

        [Category("Custom Appearance")]
        public int BorderSize
        {
            get { return borderSize; }
            set { borderSize = value; this.Invalidate(); }
        }

        [Category("Custom Appearance")]
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; this.Invalidate(); }
        }

        // Constructor
        public CustomPanel()
        {
            this.Resize += new EventHandler(CustomPanel_Resize);
        }

        private void CustomPanel_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        // Override OnPaint
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (Pen pen = new Pen(borderColor, borderSize))
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                if (showTopBorder)
                    e.Graphics.DrawLine(pen, 0, 0, this.Width, 0);

                if (showBottomBorder)
                    e.Graphics.DrawLine(pen, 0, this.Height - borderSize, this.Width, this.Height - borderSize);

                if (showLeftBorder)
                    e.Graphics.DrawLine(pen, 0, 0, 0, this.Height);

                if (showRightBorder)
                    e.Graphics.DrawLine(pen, this.Width - borderSize, 0, this.Width - borderSize, this.Height);
            }
        }
    }
}
