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
    public class Star7
    {
        private PictureBoxCanvas canvas;
        private Point startPoint;
        private bool isDrawing = false;

        public Star7(PictureBoxCanvas canvas)
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
                DrawStar7Point(startPoint, e.Location);
            }
        }

        private void DrawStar7Point(Point start, Point end)
        {
            // Calcular el tamaño y posición de la estrella
            int radius = (int)Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));
            Point[] starPoints = new Point[14];
            double angle = 2 * Math.PI / 14; // 25.71 grados en radianes

            for (int i = 0; i < 14; i++)
            {
                double r = (i % 2 == 0) ? radius : radius / 2.5; // Radio grande para los picos y pequeño para los huecos
                double x = start.X + r * Math.Cos(angle * i - Math.PI / 2);
                double y = start.Y + r * Math.Sin(angle * i - Math.PI / 2);
                starPoints[i] = new Point((int)x, (int)y);
            }

            // Dibujar la estrella en el lienzo
            using (Graphics g = Graphics.FromImage(canvas.CurrentBitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(canvas.PenColor, canvas.PenWidth))
                {
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                    g.DrawPolygon(pen, starPoints);
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
                    g.DrawPolygon(pen, starPoints);
                }
            }

            canvas.Invalidate();
        }
    }

}
