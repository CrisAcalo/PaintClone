using PaintProject.Elementos;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaintProject.Model
{
    public class Vineta
    {
        private PictureBoxCanvas canvas;
        private Point startPoint;
        private bool isDrawing = false;

        public Vineta(PictureBoxCanvas canvas)
        {
            this.canvas = canvas;
        }

        public void MouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startPoint = e.Location;
                isDrawing = true;
            }
        }

        public void MouseMove(MouseEventArgs e)
        {
            // No hacemos nada en MouseMove para las figuras
        }

        public void MouseUp(MouseEventArgs e)
        {
            if (isDrawing)
            {
                isDrawing = false;
                DrawBalloon(startPoint, e.Location);
            }
        }

        private void DrawBalloon(Point start, Point end)
        {
            int width = Math.Abs(end.X - start.X);
            int height = Math.Abs(end.Y - start.Y);

            int left = Math.Min(start.X, end.X);
            int top = Math.Min(start.Y, end.Y);

            using (Graphics g = Graphics.FromImage(canvas.CurrentBitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(canvas.PenColor, canvas.PenWidth))
                {
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;

                    GraphicsPath path = CreateBalloonPath(new Rectangle(left, top, width, height));
                    g.DrawPath(pen, path);
                }
            }

            using (Graphics g = Graphics.FromImage(canvas.DisplayBitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(canvas.PenColor, canvas.PenWidth))
                {
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;

                    GraphicsPath path = CreateBalloonPath(new Rectangle(left, top, width, height));
                    g.DrawPath(pen, path);
                }
            }

            canvas.Invalidate();
        }

        private GraphicsPath CreateBalloonPath(Rectangle bounds)
        {
            GraphicsPath path = new GraphicsPath();

            float width = bounds.Width;
            float height = bounds.Height;
            float x = bounds.X;
            float y = bounds.Y;

            float tailWidth = width / 5;
            float tailHeight = height / 5;

            path.AddEllipse(x, y, width, height);
            path.AddPolygon(new PointF[]
            {
            new PointF(x + width / 2 - tailWidth / 2, y + height),
            new PointF(x + width / 2 + tailWidth / 2, y + height),
            new PointF(x + width / 2, y + height + tailHeight)
            });

            return path;
        }


    }
}
