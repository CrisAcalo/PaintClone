using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PaintProject.Elementos
{
    public class PictureBoxCanvas : PictureBox
    {
        private float zoomFactor = 1.0f;
        private const float minZoomFactor = 0.1f; // 10%
        private const float maxZoomFactor = 8.0f; // 800%
        private Bitmap originalBitmap;
        private Bitmap currentBitmap;
        private Bitmap displayBitmap;

        private float penWidth;
        private float eraserWidth; // Añadido para el ancho del borrador
        private Panel containerPanel;
        private bool isFillMode = false;
        private bool isPencilMode = true;
        private bool isEraserMode = false; // Añadido para el modo borrador
        private Color penColor = Color.Black; // Color por defecto del lápiz
        private Color fillColor = Color.Red; // Default fill color
        private Color eraserColor = Color.White; // Color del borrador (puede ser el color de fondo)

        public Bitmap CurrentBitmap   // property
        {
            get { return currentBitmap; }   // get method
            set { currentBitmap = value; }  // set method
        }
        public Bitmap DisplayBitmap   // property
        {
            get { return displayBitmap; }   // get method
            set { displayBitmap = value; }  // set method
        }
        public float PenWidth   // property
        {
            get { return penWidth; }   // get method
            set { penWidth = value; }  // set method
        }
        public Color PenColor   // property
        {
            get { return penColor; }   // get method
            set { penColor = value; }  // set method
        }
        public float EraserWidth   // property
        {
            get { return eraserWidth; }   // get method
            set { eraserWidth = value; }  // set method
        }
        public float ZoomFactor   // property
        {
            get { return zoomFactor; }   // get method
            set { zoomFactor = value; }  // set method
        }

        public PictureBoxCanvas(Panel container)
        {
            containerPanel = container;
            this.DoubleBuffered = true;
            this.MouseWheel += PictureBoxCanvas_MouseWheel;
            this.SizeMode = PictureBoxSizeMode.AutoSize;
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.None;
        }

        public void LoadImage(Image image)
        {
            originalBitmap = new Bitmap(image);
            currentBitmap = new Bitmap(originalBitmap);
            displayBitmap = new Bitmap(originalBitmap);
            this.Image = displayBitmap;
            this.Size = new Size(700, 400);
            CenterCanvas();
        }

        private void PictureBoxCanvas_MouseWheel(object sender, MouseEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                if (e.Delta > 0)
                {
                    ZoomIn();
                }
                else if (e.Delta < 0)
                {
                    ZoomOut();
                }
            }
        }

        public void ZoomIn()
        {
            if (zoomFactor < maxZoomFactor)
            {
                zoomFactor *= 1.1f;
                ApplyZoom();
            }
        }

        public void ZoomOut()
        {
            if (zoomFactor > minZoomFactor)
            {
                zoomFactor /= 1.1f;
                ApplyZoom();
            }
        }

        public void ApplyZoom()
        {
            if (originalBitmap == null) return;

            var newSize = new Size((int)(originalBitmap.Width * zoomFactor), (int)(originalBitmap.Height * zoomFactor));
            displayBitmap = new Bitmap(currentBitmap, newSize);

            using (Graphics g = Graphics.FromImage(displayBitmap))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(currentBitmap, 0, 0, newSize.Width, newSize.Height);
            }

            this.Image = displayBitmap;
            this.Size = newSize;
            CenterCanvas();
            containerPanel.AutoScrollMinSize = new Size(this.Width, this.Height);
        }

        public void CenterCanvas()
        {
            if (containerPanel == null) return;

            int x = Math.Max((containerPanel.ClientSize.Width - this.Width) / 2, 0);
            int y = Math.Max((containerPanel.ClientSize.Height - this.Height) / 2, 0);

            this.Location = new Point(x, y);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CenterCanvas();
        }

        public void DrawLine(Point start, Point end)
        {
            PointF scaledStart = new PointF(start.X / zoomFactor, start.Y / zoomFactor);
            PointF scaledEnd = new PointF(end.X / zoomFactor, end.Y / zoomFactor);

            float adjustedPenWidth = isEraserMode ? eraserWidth * zoomFactor : penWidth * zoomFactor;

            using (Graphics g = Graphics.FromImage(currentBitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(isEraserMode ? eraserColor : penColor, isEraserMode ? eraserWidth : penWidth))
                {
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                    g.DrawLine(pen, scaledStart, scaledEnd);
                }
            }

            using (Graphics g = Graphics.FromImage(displayBitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(isEraserMode ? eraserColor : penColor, adjustedPenWidth))
                {
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                    g.DrawLine(pen, start, end);
                }
            }

            this.Invalidate();
        }

        public void SetPencilMode(bool isPencil)
        {
            isPencilMode = isPencil;
        }
        public void SetFillMode(bool isFill)
        {
            isFillMode = isFill;
        }

        public void SetEraserMode(bool isEraser)
        {
            isEraserMode = isEraser;
        }
        public void SetPenColor(Color color)
        {
            penColor = color;
        }
        public void SetFillColor(Color color)
        {
            fillColor = color;
        }

        public void SetEraserColor(Color color)
        {
            eraserColor = color;
        }

        public void FloodFill(Point point, Color fillColor)
        {
            Stack<Point> pixels = new Stack<Point>();
            Color targetColor = currentBitmap.GetPixel(point.X, point.Y);
            if (targetColor.ToArgb() == fillColor.ToArgb())
            {
                return; // No need to fill if the target color is the same as the fill color
            }
            pixels.Push(point);

            while (pixels.Count > 0)
            {
                Point temp = pixels.Pop();
                if (temp.X < 0 || temp.X >= currentBitmap.Width || temp.Y < 0 || temp.Y >= currentBitmap.Height)
                    continue;

                Color currentColor = currentBitmap.GetPixel(temp.X, temp.Y);
                if (currentColor == targetColor)
                {
                    currentBitmap.SetPixel(temp.X, temp.Y, fillColor);

                    pixels.Push(new Point(temp.X - 1, temp.Y));
                    pixels.Push(new Point(temp.X + 1, temp.Y));
                    pixels.Push(new Point(temp.X, temp.Y - 1));
                    pixels.Push(new Point(temp.X, temp.Y + 1));
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (isFillMode)
            {
                Point point = new Point((int)(e.X / zoomFactor), (int)(e.Y / zoomFactor));
                FloodFill(point, fillColor);
                ApplyZoom();
            }
        }
        public void SaveCanvasAsImage(string filePath)
        {
            using (Bitmap bmp = new Bitmap(this.Width, this.Height))
            {
                this.DrawToBitmap(bmp, new Rectangle(0, 0, this.Width, this.Height));
                bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        public void LoadImageFromFile(string filePath)
        {
            try
            {
                using (Image image = Image.FromFile(filePath))
                {
                    this.Image = new Bitmap(image); // Convertir a Bitmap para permitir su manipulación
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la imagen: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
