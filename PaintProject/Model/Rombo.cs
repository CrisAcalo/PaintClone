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
    public class Rombo
    {
        private PictureBoxCanvas canvas;
        private Point startPoint;
        private bool isDrawing = false;

        public Rombo(PictureBoxCanvas canvas)
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
                DrawDiamond(startPoint, e.Location);
            }
        }

        private void DrawDiamond(Point start, Point end)
        {
            // Calcular el tamaño y posición del rombo
            int width = Math.Abs(end.X - start.X);
            int height = width; // En un rombo, altura = ancho
            int x = (start.X + end.X) / 2;
            int y = (start.Y + end.Y) / 2;

            // Calcular los puntos del rombo
            Point p1 = new Point(x, start.Y);
            Point p2 = new Point(end.X, y);
            Point p3 = new Point(x, end.Y);
            Point p4 = new Point(start.X, y);

            Point[] diamondPoints = { p1, p2, p3, p4 };

            // Dibujar el rombo en el lienzo
            using (Graphics g = Graphics.FromImage(canvas.CurrentBitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(canvas.PenColor, canvas.PenWidth))
                {
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                    g.DrawPolygon(pen, diamondPoints);
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
                    g.DrawPolygon(pen, diamondPoints);
                }
            }

            canvas.Invalidate();
        }
    }

}
