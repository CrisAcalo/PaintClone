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
    public class SquareTool
{
    private PictureBoxCanvas canvas;
    private Point startPoint;
    private bool isDrawing = false;

    public SquareTool(PictureBoxCanvas canvas)
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


    public void MouseUp(MouseEventArgs e)
    {
        if (isDrawing)
        {
            isDrawing = false;
            // Finaliza el dibujo del cuadrado
            DrawRectangle(startPoint, e.Location);
        }
    }

    private void DrawRectangle(Point start, Point end)
    {
        // Calcular el tamaño y posición del cuadrado
        int width = Math.Abs(end.X - start.X);
        int height = Math.Abs(end.Y - start.Y);
        int x = Math.Min(start.X, end.X);
        int y = Math.Min(start.Y, end.Y);

        // Dibujar el cuadrado en el lienzo
        using (Graphics g = Graphics.FromImage(canvas.CurrentBitmap))
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (Pen pen = new Pen(canvas.PenColor, canvas.PenWidth))
            {
                pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                g.DrawRectangle(pen, x, y, width, height);
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
                g.DrawRectangle(pen, x, y, width, height);
            }
        }

        canvas.Invalidate();
    }
}

}
