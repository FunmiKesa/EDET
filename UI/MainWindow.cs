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

namespace Presentation
{
    public partial class MainWindow : Form
    {
        TabControl tc = new TabControl();
        //Form previous;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void Form1_MdiChildActivate(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild == null)
            {
                tabForms.Visible = false;
            }
            else
            {
                this.ActiveMdiChild.WindowState = FormWindowState.Maximized;

                if (this.ActiveMdiChild.Tag == null)
                {
                    TabPage tp = new TabPage(this.ActiveMdiChild.Text);
                    tp.Tag = this.ActiveMdiChild;
                    tp.Parent = tabForms;
                    tabForms.SelectedTab = tp;

                    this.ActiveMdiChild.Tag = tp;
                    this.ActiveMdiChild.FormClosed += new FormClosedEventHandler(ActiveMdiChild_FormClosed);
                }
                if (!tabForms.Visible) tabForms.Visible = true;
            }
        }
        private void tabForms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((tabForms.SelectedTab != null) && (tabForms.SelectedTab.Tag != null))
            {
                (tabForms.SelectedTab.Tag as Form).Select();
            }
        }
        private void ActiveMdiChild_FormClosed(object sender, FormClosedEventArgs e)
        {

            ((sender as Form).Tag as TabPage).Dispose();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputForm inf = new InputForm();
            inf.MdiParent = this;
            inf.Show();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabForms.SelectedTab != null)
            {
                (tabForms.SelectedTab.Tag as Form).Dispose();
                tabForms.SelectedTab.Dispose();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog opd = new OpenFileDialog();
            if (DialogResult.OK == opd.ShowDialog(this))
            {
                var filename = opd.FileName;
                StreamReader reader = new StreamReader(filename);
                OutputForm inf = new OutputForm();
                inf.MdiParent = this;
                inf.Text = opd.SafeFileName;
                inf.Show();
                inf.display.Text = "";
                String line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    inf.display.Text += line +"\n";
                }


            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabForms.SelectedTab != null)
            {
                var tabform = tabForms.SelectedTab.Tag as Form;
                object text;
                if (tabform is OutputForm)
                {
                    text = (tabform as OutputForm).display.Text;
                    SaveFileDialog sfd = new SaveFileDialog();
                    if (sfd.ShowDialog(this) == DialogResult.OK)
                    {
                        using (Stream s = File.Open(sfd.FileName, FileMode.Create))
                        using (StreamWriter sw = new StreamWriter(s))
                        {
                            sw.Write(text.ToString());
                        }
                    }
                }
            }

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabForms.SelectedTab != null)
            {
                var tabform = tabForms.SelectedTab.Tag as Form;
                object text;
                if (tabform is OutputForm)
                {
                    text = (tabform as OutputForm).display.Text;
                    SaveFileDialog sfd = new SaveFileDialog();
                    if (sfd.ShowDialog(this) == DialogResult.OK)
                    {
                        using (Stream s = File.Open(sfd.FileName, FileMode.Create))
                        using (StreamWriter sw = new StreamWriter(s))
                        {
                            sw.Write(text.ToString());
                        }
                    }
                }
            }

        }


    }
}
