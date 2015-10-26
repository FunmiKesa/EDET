using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation
{
    public partial class OutputForm : Form
    {
        public OutputForm()
        {
            InitializeComponent();

            backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            display.Text = e.UserState.ToString();
           
        }



        public void DisplayProgress(String message, int percent, dynamic output = null)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Logic.ProgressIndicator.ProgressDelegate(DisplayProgress), new Object[] { message, percent, output });

            }
            else
            {
                               
                  backgroundWorker1.ReportProgress(percent,message);
            }
        }

        private void display_TextChanged(object sender, EventArgs e)
        {

            display.SelectionStart = display.TextLength;
            display.ScrollToCaret();
        }
    }
}
