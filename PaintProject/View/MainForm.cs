using PaintProject.Elementos;
using PaintProject.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaintProject
{
    public partial class MainForm : Form
    {
        private bool isDrawing = false;
        private Point lastPoint;
        private PictureBoxCanvas principalLienzo;
        private bool isPencilMode = true;
        private bool isFillMode = false;
        private bool isEraserMode = false;
        private Color selectedColor = Color.Black;
        private LineTool lineTool;
        private SquareTool squareTool;
        private CircleTool circleTool;
        private TrianguloEquilatero trianguloEquilatero;
        private Rombo rombo;
        private Pentagono pentagono;
        private Hexagono hexagono;
        private Arrow arrow;
        private Star star;
        private Star6 star6;
        private Star7 star7;
        private Heart heart;
        private Vineta vineta;
        private PrintDocument printDocument;


        private string figuraActual = "";
        public MainForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            panelCanvasContainer.AutoScroll = true;

            principalLienzo = new PictureBoxCanvas(panelCanvasContainer)
            {
                BorderStyle = BorderStyle.None,
                BackColor = Color.White
            };

            principalLienzo.MinimumSize = new Size(100, 100);
            principalLienzo.Size = new Size(700, 400); // Tamaño inicial

            panelCanvasContainer.Controls.Add(principalLienzo);

            // Cargar una imagen inicial (puedes cambiar esto por una imagen desde un archivo o un recurso)
            principalLienzo.LoadImage(new Bitmap(700, 400));

            // Suscribirse a los eventos de mouse
            principalLienzo.MouseDown += PrincipalLienzo_MouseDown;
            principalLienzo.MouseMove += PrincipalLienzo_MouseMove;
            principalLienzo.MouseUp += PrincipalLienzo_MouseUp;

            // Configurar el grosor del lápiz (puedes ajustar esto según tus necesidades)
            widthTrackBar.Minimum = 1;
            widthTrackBar.Maximum = 100; // Ajusta el máximo según tus necesidades
            widthTrackBar.Value = 1;
            isPencilMode = true;
            principalLienzo.PenWidth = widthTrackBar.Value;
            principalLienzo.EraserWidth = 10.0f;

            // Configurar los botones de herramientas
            btnPencil.Click += BtnPencil_Click;
            btnFill.Click += BtnFill_Click;
            btnEraser.Click += BtnEraser_Click;
            btnColorPicker.Click += BtnColorPicker_Click;
            btnDrawRombo.Click += BtnDrawRombo_Click;

            btnDrawLinea.Click += BtnDrawLinea_Click;
            btnDrawCuadrado.Click += BtnDrawCuadrado_Click;
            btnDrawCirculo.Click += BtnDrawCirculo_Click;
            btnDrawTrianguloEq.Click += BtnDrawTrianguloEq_Click;
            btnDrawPentagono.Click += BtnDrawPentagono_Click;
            btnDrawHexagono.Click += BtnDrawHexagono_Click;
            btnDrawArrow.Click += BtnDrawArrow_Click;
            btnStar4.Click += BtnStar4_Click;
            btnDrawStar6.Click += BtnDrawStar6_Click;
            btnDrawStar7.Click += BtnDrawStar7_Click;
            btnDrawHeart.Click += BtnDrawHeart_Click;

            // Centrar el lienzo al inicio
            CenterCanvas();

            // Suscribirse al evento Resize del contenedor para centrar el lienzo cuando el tamaño del contenedor cambie
            panelCanvasContainer.Resize += (s, e) => CenterCanvas();

            // Suscribirse al evento ValueChanged del TrackBar
            widthTrackBar.ValueChanged += WidthTrackBar_ValueChanged;

            lineTool = new LineTool(principalLienzo);
            squareTool = new SquareTool(principalLienzo);
            printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
        }

        private void PrincipalLienzo_MouseDown(object sender, MouseEventArgs e)
        {
            if (isPencilMode || isEraserMode)
            {
                if (e.Button == MouseButtons.Left)
                {
                    isDrawing = true;
                    lastPoint = e.Location;
                }
            }
            else if (isFillMode)
            {
                if (e.Button == MouseButtons.Left)
                {
                    Point point = new Point((int)(e.X / principalLienzo.ZoomFactor), (int)(e.Y / principalLienzo.ZoomFactor));
                    principalLienzo.FloodFill(point, selectedColor);
                    principalLienzo.ApplyZoom();
                }
            }
            else
            {
                switch (figuraActual)
                {
                    case "linea":
                        lineTool.MouseDown(e);
                        break;
                    case "cuadrado":
                        squareTool.MouseDown(e);
                        break;
                    case "circulo":
                        circleTool.MouseDown(e);
                        break;
                    case "trianguloEquilatero":
                        trianguloEquilatero.MouseDown(e);
                        break;
                    case "rombo":
                        rombo.MouseDown(e);
                        break;
                    case "pentagono":
                        pentagono.MouseDown(e);
                        break;
                    case "hexagono":
                        hexagono.MouseDown(e);
                        break;
                    case "arrow":
                        arrow.MouseDown(e);
                        break;
                    case "star4":
                        star.MouseDown(e);
                        break;
                    case "star6":
                        star6.MouseDown(e);
                        break;
                    case "star7":
                        star7.MouseDown(e);
                        break;
                    case "heart":
                        heart.MouseDown(e);
                        break;
                }
            }
        }

        private void PrincipalLienzo_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPencilMode || isEraserMode || isFillMode)
            {
                if (isDrawing)
                {
                    principalLienzo.DrawLine(lastPoint, e.Location);
                    lastPoint = e.Location;
                }
            }

            // No hacemos nada para figuras en MouseMove
        }

        private void PrincipalLienzo_MouseUp(object sender, MouseEventArgs e)
        {
            if (isPencilMode || isEraserMode || isFillMode)
            {
                isDrawing = false;
            }
            else
            {
                switch (figuraActual)
                {
                    case "linea":
                        lineTool.MouseUp(e);
                        break;
                    case "cuadrado":
                        squareTool.MouseUp(e);
                        break;
                    case "circulo":
                        circleTool.MouseUp(e);
                        break;
                    case "trianguloEquilatero":
                        trianguloEquilatero.MouseUp(e);
                        break;
                    case "rombo":
                        rombo.MouseUp(e);
                        break;
                    case "pentagono":
                        pentagono.MouseUp(e);
                        break;
                    case "hexagono":
                        hexagono.MouseUp(e);
                        break;
                    case "arrow":
                        arrow.MouseUp(e);
                        break;
                    case "star4":
                        star.MouseUp(e);
                        break;
                    case "star6":
                        star6.MouseUp(e);
                        break;
                    case "star7":
                        star7.MouseUp(e);
                        break;
                    case "heart":
                        heart.MouseUp(e);
                        break;
                }

                //figuraActual = "";
            }
        }


        private void WidthTrackBar_ValueChanged(object sender, EventArgs e)
        {
            if (isPencilMode || figuraActual != "")
            {
                principalLienzo.PenWidth = widthTrackBar.Value;
            }
            else if (isEraserMode)
            {
                principalLienzo.EraserWidth = widthTrackBar.Value;
            }
        }

        private void BtnPencil_Click(object sender, EventArgs e)
        {
            isPencilMode = true;
            isEraserMode = false;
            isFillMode = false;
            lineTool = null;
            figuraActual = "";
            principalLienzo.SetPencilMode(true);
            principalLienzo.SetEraserMode(false);
            principalLienzo.SetFillMode(false);
            widthTrackBar.Value = (int)principalLienzo.PenWidth; 
        }

        private void BtnFill_Click(object sender, EventArgs e)
        {
            isPencilMode = false;
            isEraserMode = false;
            isFillMode = true;
            lineTool = null; 
            figuraActual = "";
            principalLienzo.SetPencilMode(false);
            principalLienzo.SetEraserMode(false);
            principalLienzo.SetFillMode(true);
        }

        private void BtnEraser_Click(object sender, EventArgs e)
        {
            isEraserMode = true;
            isPencilMode = false;
            isFillMode = false;
            lineTool = null;
            figuraActual = "";
            principalLienzo.SetPencilMode(false);
            principalLienzo.SetEraserMode(true);
            principalLienzo.SetFillMode(false);
            widthTrackBar.Value = (int)principalLienzo.EraserWidth; 
        }

        private void BtnColorPicker_Click(object sender, EventArgs e)
        {
            using (colorPicker)
            {
                if (colorPicker.ShowDialog() == DialogResult.OK)
                {
                    selectedColor = colorPicker.Color;

                    // Actualizar el color del lápiz o del relleno según el modo actual
                    if (isPencilMode || figuraActual!="")
                    {
                        principalLienzo.SetPenColor(selectedColor);
                    }
                    else if (isFillMode)
                    {
                        principalLienzo.SetFillColor(selectedColor);
                    }
                    else if (isEraserMode)
                    {
                        // El borrador no cambia de color, solo aseguramos que el modo de borrador esté configurado
                        principalLienzo.SetEraserColor(Color.White); // Suponiendo que el color de borrado es blanco
                    }
                }
            }
        }

        private void CenterCanvas()
        {
            if (principalLienzo != null)
            {
                principalLienzo.CenterCanvas();
            }
        }
        private void disableTools()
        {
            isPencilMode = false;
            isEraserMode = false;
            isFillMode = false;
            principalLienzo.SetPencilMode(false);
            principalLienzo.SetEraserMode(false);
            principalLienzo.SetFillMode(false);
        }

        private void BtnDrawLinea_Click(object sender, EventArgs e)
        {
            figuraActual="linea";
            lineTool = new LineTool(principalLienzo);

            disableTools();
        }

        private void BtnDrawCuadrado_Click(object sender, EventArgs e)
        {
            figuraActual = "cuadrado";
            squareTool = new SquareTool(principalLienzo);

            disableTools();
        }

        private void BtnDrawCirculo_Click(object sender, EventArgs e)
        {
            figuraActual = "circulo";
            circleTool = new CircleTool(principalLienzo);

            disableTools();
        }

        private void BtnDrawTrianguloEq_Click(object sender, EventArgs e)
        {
            figuraActual = "trianguloEquilatero";
            trianguloEquilatero = new TrianguloEquilatero(principalLienzo);

            disableTools();
        }
        private void BtnDrawRombo_Click(object sender, EventArgs e)
        {
            figuraActual = "rombo";
            rombo = new Rombo(principalLienzo);

            disableTools();
        }
        private void BtnDrawPentagono_Click(object sender, EventArgs e)
        {
            figuraActual = "pentagono";
            pentagono = new Pentagono(principalLienzo);

            disableTools();
        }
        private void BtnDrawHexagono_Click(object sender, EventArgs e)
        {
            figuraActual = "hexagono";
            hexagono = new Hexagono(principalLienzo);

            disableTools();
        }
        private void BtnDrawArrow_Click(object sender, EventArgs e)
        {
            figuraActual = "arrow";
            arrow = new Arrow(principalLienzo);

            disableTools();
        }
        private void BtnStar4_Click(object sender, EventArgs e)
        {
            figuraActual = "star4";
            star = new Star(principalLienzo);

            disableTools();
        }
        private void BtnDrawStar6_Click(object sender, EventArgs e)
        {
            figuraActual = "star6";
            star6 = new Star6(principalLienzo);

            disableTools();
        }
        private void BtnDrawStar7_Click(object sender, EventArgs e)
        {
            figuraActual = "star7";
            star7 = new Star7(principalLienzo);

            disableTools();
        }
        private void BtnDrawHeart_Click(object sender, EventArgs e)
        {
            figuraActual = "heart";
            heart = new Heart(principalLienzo);

            disableTools();
        }
        private void BtnDrawVineta_Click(object sender, EventArgs e)
        {
            figuraActual = "vineta";
            vineta = new Vineta(principalLienzo);

            disableTools();
        }

        private void archivoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        public void setButtonColor(object sender)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                if (isPencilMode || figuraActual != "")
                {
                    principalLienzo.SetPenColor(clickedButton.BackColor);
                }
                else if (isFillMode)
                {
                    principalLienzo.SetFillColor(clickedButton.BackColor);
                }
            }
        }
        private void buttonMenu1_Click(object sender, EventArgs e)
        {
            setButtonColor(sender);
        }

        private void btnSetWhite_Click(object sender, EventArgs e)
        {
            setButtonColor(sender);
        }

        private void btnColor1_Click(object sender, EventArgs e)
        {
            setButtonColor(sender);
        }
        
        private void btnColor2_Click(object sender, EventArgs e)
        {
            setButtonColor(sender);
        }

        private void btnColor3_Click(object sender, EventArgs e)
        {
            setButtonColor(sender);
        }

        private void btnColor4_Click(object sender, EventArgs e)
        {
            setButtonColor(sender);
        }

        private void btnColor5_Click(object sender, EventArgs e)
        {
            setButtonColor(sender);
        }

        private void btnColor6_Click(object sender, EventArgs e)
        {
            setButtonColor(sender);
        }

        private void btnColor7_Click(object sender, EventArgs e)
        {
            setButtonColor(sender);
        }

        private void btnColor8_Click(object sender, EventArgs e)
        {
            setButtonColor(sender);
        }

        private void btnColor9_Click(object sender, EventArgs e)
        {
            setButtonColor(sender);
        }

        private void btnColor10_Click(object sender, EventArgs e)
        {
            setButtonColor(sender);
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuardarLienzo();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
                openFileDialog.Title = "Open Image File";
                openFileDialog.ShowDialog();

                if (openFileDialog.FileName != "")
                {
                    // Cargar la imagen en el lienzo
                    principalLienzo.LoadImageFromFile(openFileDialog.FileName);
                }
            }
        }
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Configura el área de impresión
            Rectangle rect = new Rectangle(0, 0, principalLienzo.Width, principalLienzo.Height);

            // Crea un Bitmap del tamaño del lienzo
            using (Bitmap bmp = new Bitmap(principalLienzo.Width, principalLienzo.Height))
            {
                principalLienzo.DrawToBitmap(bmp, rect);
                e.Graphics.DrawImage(bmp, rect);
            }
        }
        private void imprimirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Desea guardar el dibujo actual?", "Guardar", MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                if (GuardarLienzo())
                {
                    ReiniciarAplicacion();
                }
            }
            else if (result == DialogResult.No)
            {
                ReiniciarAplicacion();
            }
        }

        private bool GuardarLienzo()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Imagen PNG|*.png",
                Title = "Guardar Imagen"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (Bitmap bmp = new Bitmap(principalLienzo.Width, principalLienzo.Height))
                    {
                        principalLienzo.DrawToBitmap(bmp, new Rectangle(0, 0, principalLienzo.Width, principalLienzo.Height));
                        bmp.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    return true; // Se guardó correctamente
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false; // Ocurrió un error
                }
            }

            return false; // El usuario canceló el diálogo de guardado
        }

        private void ReiniciarAplicacion()
        {
            // Cierra la aplicación actual y la reinicia
            Application.Restart();
            Environment.Exit(0); // Asegura que la aplicación se cierra correctamente
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }


}
