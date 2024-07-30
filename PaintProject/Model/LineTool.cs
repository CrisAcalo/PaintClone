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
    public class LineTool
    {
        private PictureBoxCanvas canvas;
        private Point startPoint;
        private Point endPoint;
        private bool isDrawing = false;

        public LineTool(PictureBoxCanvas canvas)
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
                endPoint = e.Location;
                // Dibuja la línea solamente cuando el mouse se levanta
                DrawLine();
                isDrawing = false;
            }
        }

        private void DrawLine()
        {
            // Utiliza el color y el ancho del lápiz actual
            canvas.DrawLine(startPoint, endPoint);
        }
    }


}
