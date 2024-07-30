using PaintProject.Elementos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaintProject.Model
{
    public class Heart
    {
        private PictureBoxCanvas canvas;
        private Point startPoint;
        private bool isDrawing = false;

        public Heart(PictureBoxCanvas canvas)
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
                DrawHeart(startPoint, e.Location);
            }
        }

        private void DrawHeart(Point start, Point end)
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

                    GraphicsPath path = CreateHeartPath(new Rectangle(left, top, width, height));
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

                    GraphicsPath path = CreateHeartPath(new Rectangle(left, top, width, height));
                    g.DrawPath(pen, path);
                }
            }

            canvas.Invalidate();
        }

        private GraphicsPath CreateHeartPath(Rectangle bounds)
        {
            GraphicsPath path = new GraphicsPath();

            float width = bounds.Width;
            float height = bounds.Height;
            float x = bounds.X;
            float y = bounds.Y;

            // Crear el contorno del corazón con dos curvas de Bézier
            path.AddBezier(x + width / 2, y + height / 4, x + width / 7, y - height / 6, x, y + height / 2, x + width / 2, y + height);
            path.AddBezier(x + width / 2, y + height / 4, x + width - width / 7, y - height / 6, x + width, y + height / 2, x + width / 2, y + height);

            // Remover la línea del medio
            path.CloseFigure();

            return path;
        }
    }

}
