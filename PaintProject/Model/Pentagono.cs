using PaintProject.Elementos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaintProject.Model
{
    public class Pentagono
    {
        private PictureBoxCanvas canvas;
        private Point startPoint;
        private bool isDrawing = false;

        public Pentagono(PictureBoxCanvas canvas)
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
                DrawPentagon(startPoint, e.Location);
            }
        }

        private void DrawPentagon(Point start, Point end)
        {
            // Calcular el tamaño y posición del pentágono
            int radius = (int)Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));
            double angle = 2 * Math.PI / 5; // 72 grados en radianes
            Point[] pentagonPoints = new Point[5];

            for (int i = 0; i < 5; i++)
            {
                double x = start.X + radius * Math.Cos(angle * i - Math.PI / 2);
                double y = start.Y + radius * Math.Sin(angle * i - Math.PI / 2);
                pentagonPoints[i] = new Point((int)x, (int)y);
            }

            // Dibujar el pentágono en el lienzo
            using (Graphics g = Graphics.FromImage(canvas.CurrentBitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(canvas.PenColor, canvas.PenWidth))
                {
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                    g.DrawPolygon(pen, pentagonPoints);
                }
            }

            // Actualizar la imagen mostrada
            using (Graphics g = Graphics.FromImage(canvas.DisplayBitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(canvas.PenColor, canvas.PenWidth))
                {
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                    g.DrawPolygon(pen, pentagonPoints);
                }
            }

            canvas.Invalidate();
        }
    }

}
