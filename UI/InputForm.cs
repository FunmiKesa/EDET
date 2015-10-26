using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using UI;

namespace Presentation
{
    public partial class InputForm : Form
    {
        OpenFileDialog opd;
        private double wf;
        private int nog;
        private int nop;
        private ProblemType pt;
        private string deApproach;
        private string filename;
        private int strategy;
        private double cf;
        private double cr;
        private IProblem iprob;
        private string message;
        private Logic.Optimizer opt;
        private Logic.ProblemLogic problogic;
        private InputStructure input;
        private Display display;
        private OutputForm inf;
        private bool supportDisplay = false;
        private int heuristic;
        private int runs;
        public InputForm()
        {
            InitializeComponent();


            backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
        }
        private void startAsyncButton_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;

            if (btn.Text.Equals("Optimize"))
            {
                runs = int.Parse(runsTextBox.Text);
                heuristic = heuristicComboBox.SelectedIndex + 1;
                strategy = strategyCB.SelectedIndex + 1;
                cr = double.Parse(crTxtBox.Text);
                cf = new int[] { 3, 8 }.Contains(strategy) ? double.Parse(cfTxtBox.Text) : 0;
                wf = double.Parse(wfTxtBox.Text);
                nog = int.Parse(nogTxtBox.Text);
                nop = int.Parse(numberOfPopulationtxt.Text);
                if (!Enum.TryParse(problemTypeComboBox.Text, true, out pt))
                {
                    pt = ProblemType.UNKNOWN;
                }
                deApproach = heuristicComboBox.Text;
                filename = filenameTextBox.Text;

                optimizeBtn.Text = "Cancel";
                backgroundWorker1.RunWorkerAsync();


            }
            else if (btn.Text.Equals("Cancel"))
            {
                opt.ProgressIndicator.Cancel = true;
                problogic.ProgressIndicator.Cancel = true;
                backgroundWorker1.CancelAsync();
            }
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Boolean successful = false;
            Boolean minimize = true;
            if (!String.IsNullOrEmpty(filename) && strategy != 0 && heuristic != 0)
            //&& !String.IsNullOrEmpty(pt) && !String.IsNullOrEmpty(deApproach))
            {
                switch (pt)
                {
                    case ProblemType.ATSP:
                    case ProblemType.TSP:
                        problogic = new Logic.TSPLogic();
                        iprob = (TSP)problogic.Problem;
                        display = new Display(iprob);
                        supportDisplay = (iprob as TSP).Cities != null ? true : false;
                        successful = problogic.LoadFile(filename);
                        break;
                    case ProblemType.QAP:
                        problogic = new Logic.QAPLogic();
                        iprob = (QAP)problogic.Problem;
                        iprob.Type = ProblemType.QAP;
                        supportDisplay = false;
                        successful = problogic.LoadFile(filename);
                  //      Console.WriteLine(problogic.ToString());
                        break;
                    case ProblemType.LOP:
                        problogic = new Logic.LOPLogic();
                        iprob = (LOP)problogic.Problem;
                        iprob.Type = ProblemType.LOP;
                        supportDisplay = false;
                        minimize = false;
                        successful = problogic.LoadFile(filename);
                      //  Console.WriteLine(problogic.ToString());
                        break;
                    case ProblemType.PFSP:
                        problogic = new Logic.PFSPLogic();
                        iprob = (PFSP)problogic.Problem;
                        supportDisplay = false;
                        iprob.Type = ProblemType.PFSP;
                        successful = problogic.LoadFile(filename);
                        break;
                    default:
                        throw new Exception("Please, complete the required fields");
                }
                if (successful)
                {
                    inf = new OutputForm();
                    inf.display.ReadOnly = true;
                    inf.display.WordWrap = false;
                    //inf.Show();
                    problogic.ProgressIndicator.Progress += new Logic.ProgressIndicator.ProgressDelegate(DisplayProgress);
                    opt = new Logic.Optimizer(problogic.ObjectiveFunction, minimize);
                    opt.ProgressIndicator.Progress += new Logic.ProgressIndicator.ProgressDelegate(DisplayProgress);
                    input = new InputStructure
                    {
                        ThresholdMin = iprob.OptimalValue,
                        CrossoverRate = cr,
                        NumberOfGenerations = nog,
                        ProblemSize = iprob.Dimension,
                        WeightingFactor = wf,
                        PopulationSize = nop,
                        MinimumBound = new int[iprob.Dimension],
                        MaximumBound = new int[iprob.Dimension],
                        ControlVariable = cf,
                        Strategy = strategy,
                        Heuristic = heuristic,
                    };

                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        problogic.ProgressIndicator.Cancel = true;
                        opt.ProgressIndicator.Cancel = true;
                        problogic.ProgressIndicator.RaiseProgress("Operation cancelled", 0);
                        return;
                    }
                    var name = filename.Split(new char[] { '\\' }).LastOrDefault();
                    iprob.Name = String.IsNullOrEmpty(iprob.Name) ? name : iprob.Name;
                    var runsString = "\nHeuristic"+ heuristic+" "+ iprob.Name+"\n" ;
                    double sum = 0;
                    for (int i = 0; i < runs; i++)
                    {
                        var output = opt.Optimize(input);
                        sum += output.BestValue;
                        runsString += String.Format(" Run {0}: {1}", i, output.BestValue);
                        e.Result = output;
                    }
                    var aver = sum/runs;
                    var proximity = (aver - iprob.OptimalValue) / iprob.OptimalValue;
                    var outputpath = filename + "output.txt";
                    runsString += " aver:" + aver + "\nOptimal value: " + iprob.OptimalValue + "\nProximity = " + proximity;
                    File.AppendAllText(outputpath, runsString);
                    MessageBox.Show(runsString);

                }
            }
            else
            {
                throw new Exception("Please, complete the required fields");
            }

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.toolStripProgressBar1.Value = e.ProgressPercentage;
            this.statuslbl.Text = message;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                problogic.ProgressIndicator.Cancel = false;
                opt.ProgressIndicator.Cancel = false;
                MessageBox.Show("Operation Cancelled");
            }
            //else
            //{
            //    OutputStructure output = e.Result as OutputStructure;
            //    if (output != null)
            //    {
            //        var log = String.Format("Problem Name: {0}\nProblem Type: {1}\nComment: {2}"
            //            , iprob.Name, iprob.Type.ToString(), iprob.Comment) + output.Log;
            //        var items = filename.Split('.');
            //        var outputpath = items.Length >= 0 ? items[0] + "output.txt" : "output.txt";
            //        File.WriteAllText(outputpath, log);
            //        inf.MdiParent = this.MdiParent;
            //        //inf.Show();
            //        inf.display.Text = log;

            //    }
            //}
            optimizeBtn.Text = "Optimize";
            statuslbl.Text = "";
            toolStripProgressBar1.Value = 0;
            optimizeBtn.Enabled = true;
            browseBtn.Enabled = true;
        }
        private void UpdateTextChanged(object sender, EventArgs e)
        {
            var strategy = strategyCB.SelectedIndex + 1;
            if ((sender as ComboBox) != null)
            {
                if ((strategyCB.Name).Equals((sender as ComboBox).Name))
                {
                    if (new int[] { 3, 8 }.Contains(strategy))
                    {
                        cflbl.Visible = true;
                        cfTxtBox.Visible = true;
                    }
                    else
                    {
                        cflbl.Visible = false;
                        cfTxtBox.Visible = false;
                    }
                }
            }
            else if ((filenameTextBox).Equals(sender as TextBox))
            {
                filename = filenameTextBox.Text;
                var part = filename.Split('.');
                var extension = part.Length > 1 ? part[1].ToLower() : "";
                if (Enum.TryParse(extension, true, out pt))
                {
                    problemTypeComboBox.Text = pt.ToString();
                }
            }

            if (new int[] { 4, 9 }.Contains(strategy))
            {
                if (!String.IsNullOrEmpty(numberOfPopulationtxt.Text))
                {
                    if (int.Parse(numberOfPopulationtxt.Text) <= 4)
                    {
                        numberOfPopulationtxt.Text = "" + 5;
                    }
                }
            }
            else if (new int[] { 5, 10 }.Contains(strategy))
            {
                if (!String.IsNullOrEmpty(numberOfPopulationtxt.Text))
                {
                    if (int.Parse(numberOfPopulationtxt.Text) <= 5)
                    {
                        numberOfPopulationtxt.Text = "" + 6;
                    }
                }
            }

        }

        private void browseBtn_Click(object sender, EventArgs e)
        {
            opd = new OpenFileDialog();
            opd.Filter = "All Files|*.*|Problem Files|*.tsp;*.qap;*.lop;*.pfssp;*.fsp";
            var result = opd.ShowDialog();
            if (result == DialogResult.OK)
            {
                filenameTextBox.Text = opd.FileName;
            }
        }



        public void DisplayProgress(String message, int percent, dynamic output = null)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Logic.ProgressIndicator.ProgressDelegate(DisplayProgress), new Object[] { message, percent, output });

            }
            else
            {

                this.message = message;
                backgroundWorker1.ReportProgress(percent);
                if (output != null)
                {
                    if (supportDisplay)
                    {
                        display.Show();
                        display.Invoke(new Logic.ProgressIndicator.ProgressDelegate(display.DisplayProgress), new Object[] { message, percent, output });

                    }
                    else
                    {
                        inf.Show();
                        var log = output.GetType().GetProperty("log");
                        if (log != null)
                        {
                            message = log.GetValue(output, null).ToString();
                            inf.Invoke(new Logic.ProgressIndicator.ProgressDelegate(inf.DisplayProgress), new Object[] { message, percent, null });
                        }
                    }
                }
            }
        }

        private void runsTextBox_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(runsTextBox.Text)) return;
            var value = int.Parse(runsTextBox.Text);
            if ( value < 1)
            {
                runsTextBox.Text = "" + 1;
            }
        }

      

    }
}
