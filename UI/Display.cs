using Core;
using Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class Display : Form
    {
        private IProblem iprob;
        public Display(IProblem iprob)
        {
            this.iprob = iprob;
            InitializeComponent();
            this.list = new List<Coordinate>();
            this.tours = null;
            richTextBox1.Text = "";

            backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            richTextBox1.AppendText(message);

        }


        private List<Core.Coordinate> list;

        Dictionary<Node, Point> coordinates = new Dictionary<Node, Point>();
        Dictionary<Rectangle, Node> rectangles = new Dictionary<Rectangle, Node>();
        private static Bitmap _nodeBg = new Bitmap(30, 25);
        private static Font font = new Font("Tahoma", 8);
        private static readonly float Coef = _nodeBg.Width / 40f;
        private SizeF nodeWidth;
        private int[][] tours;
        private int best;
        private string message;


        private void Draw()
        {
            if (list != null)
            {
                nodeWidth = new SizeF();

                Graphics g = splitContainer1.Panel1.CreateGraphics();
                Pen pen;
                g.Clear(Color.White);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                // determine bounds of data
                double left = Double.PositiveInfinity;
                double right = Double.NegativeInfinity;
                double bottom = Double.PositiveInfinity;
                double top = Double.NegativeInfinity;

                for (int i = 0; i < list.Count; i++)
                {
                    var city = list[i];
                    nodeWidth = g.MeasureString(i.ToString(), font);

                    left = Math.Min(left, city.Y);
                    right = Math.Max(right, city.Y);
                    bottom = Math.Min(bottom, city.X);
                    top = Math.Max(top, city.X);
                }

                // calculate the bounds of the drawing
                int displayWidth = (splitContainer1.Panel1).Width;
                int displayHeight = (splitContainer1.Panel1).Height;
                double scaleX = (displayWidth - nodeWidth.Width - nodeWidth.Width) / (right - left);
                double scaleY = (displayHeight - nodeWidth.Height - nodeWidth.Height) / (top - bottom);
                double scale = Math.Min(scaleX, scaleY);
                double offsetX = (displayWidth - nodeWidth.Width - nodeWidth.Width - scale * (right - left)) / 2.0;
                double offsetY = (displayHeight - nodeWidth.Height - nodeWidth.Height - scale * (top - bottom)) / 2.0;

                // draw the tours
                for (int i = 0; i < tours.GetLength(0); i++)
                {
                    var tour = tours[ i];
                    if (best == i)
                    {
                        pen = new Pen(Brushes.Red, 2f);
                    }
                    else
                    {
                        pen = new Pen(Brushes.Gray, 0.5f);
                    }

                    for (int j = 0; j < tour.Length; j++)
                    {
                        var city1 = list[tour[j % tour.Length] - 1];
                        var city2 = list[tour[(j + 1) % tour.Length] - 1];

                        var point1 = new PointF(
                            (float)(displayWidth - (offsetX + scale * (city1.Y - left) + nodeWidth.Width)),
                            (float)(displayHeight - (offsetY + scale * (city1.X - bottom) + nodeWidth.Height))
                            );

                        var point2 = new PointF(
                         (float)(displayWidth - (offsetX + scale * (city2.Y - left) + nodeWidth.Width)),
                         (float)(displayHeight - (offsetY + scale * (city2.X - bottom) + nodeWidth.Height))
                         );
                        g.DrawLine(pen, point1, point2);
                    }

                }

                // draw the nodes

                for (int i = 0; i < list.Count; i++)
                {
                    var istring = (i + 1).ToString();
                    var city = list[i];
                    var pointf = new PointF(
                        (float)(displayWidth - (offsetX + scale * (city.Y - left) + nodeWidth.Width) - (nodeWidth.Width / 2.0)),
                        (float)(displayHeight - (offsetY + scale * (city.X - bottom) + nodeWidth.Height) - (nodeWidth.Height / 2.0)));


                    RectangleF rectf = new RectangleF(pointf.X, pointf.Y, nodeWidth.Width - 2, nodeWidth.Height - 2);

                    g.FillEllipse(new LinearGradientBrush(new PointF(0, 0), new PointF(nodeWidth.Width, nodeWidth.Height), Color.Goldenrod, Color.Black), rectf);
                    g.DrawString(istring, font, Brushes.White, pointf);
                }
            }

        }
        public void LoadTours(int[][] tours, int best, String message)
        {

            this.list = (iprob as TSP).Cities;
            this.tours = tours;
            this.best = best;
            this.message = message;

            Draw();
        }


       
        public void DisplayProgress(String message, int percent, dynamic output = null)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Logic.ProgressIndicator.ProgressDelegate(DisplayProgress), new Object[] { message, percent, output });

            }
            else
            {
                if (output != null)
                {

                    var individuals = output.GetType().GetProperty("individuals");
                    var best = output.GetType().GetProperty("best");
                    if(individuals != null && best != null){
                        LoadTours(individuals.GetValue(output, null), best.GetValue(output, null), message);
                        backgroundWorker1.ReportProgress(percent);

                    }
                }

            }
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            Draw();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.ScrollToCaret();
        }
    }

}
