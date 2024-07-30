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
    public class TrianguloEquilatero
    {
        private PictureBoxCanvas canvas;
        private Point startPoint;
        private bool isDrawing = false;

        public TrianguloEquilatero(PictureBoxCanvas canvas)
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
                DrawTriangle(startPoint, e.Location);
            }
        }

        private void DrawTriangle(Point start, Point end)
        {
            // Calcular el tamaño y posición del triángulo equilátero
            int width = Math.Abs(end.X - start.X);
            int height = width; // En un triángulo equilátero, altura = ancho
            int x = Math.Min(start.X, end.X);
            int y = Math.Min(start.Y, end.Y);

            // Calcular los puntos del triángulo equilátero
            Point p1 = new Point(start.X, end.Y);
            Point p2 = new Point(end.X, end.Y);
            Point p3 = new Point((start.X + end.X) / 2, start.Y - height);

            Point[] trianglePoints = { p1, p2, p3 };

            // Dibujar el triángulo en el lienzo
            using (Graphics g = Graphics.FromImage(canvas.CurrentBitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(canvas.PenColor, canvas.PenWidth))
                {
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                    g.DrawPolygon(pen, trianglePoints);
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
                    g.DrawPolygon(pen, trianglePoints);
                }
            }

            canvas.Invalidate();
        }
    }

}
