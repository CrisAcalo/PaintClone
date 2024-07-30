using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace PaintProject.Elementos
{
    public class ButtonMenu : Button
    {
        // Fields
        private int borderSize = 0;
        private int borderRadius = 0;
        private Color borderColor = Color.PaleVioletRed;
        private Color hoverBackColor = Color.MediumSlateBlue;
        private Color hoverBorderColor = Color.PaleVioletRed;
        private Color clickBackColor = Color.MediumSlateBlue;
        private Color clickBorderColor = Color.PaleVioletRed;
        private Color hoverTextColor = Color.White;
        private Color clickTextColor = Color.White;

        private Color originalBackColor;
        private Color originalBorderColor;
        private Color originalTextColor;

        // Properties
        [Category("Atributos Personalizados")]
        public int BorderSize
        {
            get { return borderSize; }
            set { borderSize = value; this.Invalidate(); }
        }

        [Category("Atributos Personalizados")]
        public int BorderRadius
        {
            get { return borderRadius; }
            set { borderRadius = value; this.Invalidate(); }
        }

        [Category("Atributos Personalizados")]
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; this.Invalidate(); }
        }

        [Category("Atributos Personalizados")]
        public Color BackgroundColor
        {
            get { return this.BackColor; }
            set { this.BackColor = value; }
        }

        [Category("Atributos Personalizados")]
        public Color TextColor
        {
            get { return this.ForeColor; }
            set { this.ForeColor = value; }
        }

        [Category("Atributos Personalizados")]
        public Color HoverBackColor
        {
            get { return hoverBackColor; }
            set { hoverBackColor = value; }
        }

        [Category("Atributos Personalizados")]
        public Color HoverBorderColor
        {
            get { return hoverBorderColor; }
            set { hoverBorderColor = value; }
        }

        [Category("Atributos Personalizados")]
        public Color ClickBackColor
        {
            get { return clickBackColor; }
            set { clickBackColor = value; }
        }

        [Category("Atributos Personalizados")]
        public Color ClickBorderColor
        {
            get { return clickBorderColor; }
            set { clickBorderColor = value; }
        }

        [Category("Atributos Personalizados")]
        public Color HoverTextColor
        {
            get { return hoverTextColor; }
            set { hoverTextColor = value; }
        }

        [Category("Atributos Personalizados")]
        public Color ClickTextColor
        {
            get { return clickTextColor; }
            set { clickTextColor = value; }
        }

        // Constructor
        public ButtonMenu()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(150, 40);
            this.BackColor = Color.MediumSlateBlue;
            this.ForeColor = Color.White;
            this.Resize += new EventHandler(Button_Resize);
            this.originalBackColor = this.BackColor;
            this.originalBorderColor = this.BorderColor;
            this.originalTextColor = this.ForeColor;
        }

        // Methods
        private GraphicsPath GetFigurePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            Rectangle rectSurface = this.ClientRectangle;
            Rectangle rectBorder = Rectangle.Inflate(rectSurface, -borderSize, -borderSize);
            int smoothSize = 2;
            if (borderSize > 0)
                smoothSize = borderSize;

            if (borderRadius > 2) // Rounded button
            {
                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - borderSize))
                using (Pen penSurface = new Pen(this.Parent.BackColor, smoothSize))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    // Button surface
                    this.Region = new Region(pathSurface);
                    // Draw surface border for HD result
                    pevent.Graphics.DrawPath(penSurface, pathSurface);

                    // Button border
                    if (borderSize >= 1)
                        // Draw control border
                        pevent.Graphics.DrawPath(penBorder, pathBorder);
                }
            }
            else // Normal button
            {
                pevent.Graphics.SmoothingMode = SmoothingMode.None;
                // Button surface
                this.Region = new Region(rectSurface);
                // Button border
                if (borderSize >= 1)
                {
                    using (Pen penBorder = new Pen(borderColor, borderSize))
                    {
                        penBorder.Alignment = PenAlignment.Inset;
                        pevent.Graphics.DrawRectangle(penBorder, 0, 0, this.Width - 1, this.Height - 1);
                    }
                }
            }
        }

        //protected override void OnHandleCreated(EventArgs e)
        //{
        //    base.OnHandleCreated(e);
        //    this.Parent.BackColorChanged += new EventHandler(Container_BackColorChanged);
        //}

        private void Container_BackColorChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void Button_Resize(object sender, EventArgs e)
        {
            if (borderRadius > this.Height)
                borderRadius = this.Height;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            originalBackColor = this.BackColor;
            originalBorderColor = this.borderColor;
            originalTextColor = this.ForeColor;
            this.BackColor = hoverBackColor;
            this.ForeColor = hoverTextColor;
            this.borderColor = hoverBorderColor;
            this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.BackColor = originalBackColor;
            this.ForeColor = originalTextColor;
            this.borderColor = originalBorderColor;
            this.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            this.BackColor = clickBackColor;
            this.ForeColor = clickTextColor;
            this.borderColor = clickBorderColor;
            this.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            if (this.ClientRectangle.Contains(mevent.Location))
            {
                this.BackColor = hoverBackColor;
                this.ForeColor = hoverTextColor;
                this.borderColor = hoverBorderColor;
            }
            else
            {
                this.BackColor = originalBackColor;
                this.ForeColor = originalTextColor;
                this.borderColor = originalBorderColor;
            }
            this.Invalidate();
        }
    }
}
