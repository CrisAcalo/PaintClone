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
    public class Arrow
    {
        private PictureBoxCanvas canvas;
        private Point startPoint;
        private bool isDrawing = false;

        public Arrow(PictureBoxCanvas canvas)
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
                DrawArrow(startPoint, e.Location);
            }
        }

        private void DrawArrow(Point start, Point end)
        {
            // Calcular el ángulo de la flecha
            double angle = Math.Atan2(end.Y - start.Y, end.X - start.X);
            double arrowHeadLength = 10; // Longitud de la cabeza de la flecha
            double arrowHeadAngle = Math.PI / 6; // 30 grados en radianes

            // Calcular los puntos de la cabeza de la flecha
            Point p1 = new Point(
                (int)(end.X - arrowHeadLength * Math.Cos(angle - arrowHeadAngle)),
                (int)(end.Y - arrowHeadLength * Math.Sin(angle - arrowHeadAngle))
            );

            Point p2 = new Point(
                (int)(end.X - arrowHeadLength * Math.Cos(angle + arrowHeadAngle)),
                (int)(end.Y - arrowHeadLength * Math.Sin(angle + arrowHeadAngle))
            );

            // Dibujar la flecha
            using (Graphics g = Graphics.FromImage(canvas.CurrentBitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(canvas.PenColor, canvas.PenWidth))
                {
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                    g.DrawLine(pen, start, end);
                    g.DrawLine(pen, end, p1);
                    g.DrawLine(pen, end, p2);
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
                    g.DrawLine(pen, start, end);
                    g.DrawLine(pen, end, p1);
                    g.DrawLine(pen, end, p2);
                }
            }

            canvas.Invalidate();
        }
    }


}
