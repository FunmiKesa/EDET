namespace Presentation
{
    partial class InputForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cfTxtBox = new System.Windows.Forms.TextBox();
            this.crTxtBox = new System.Windows.Forms.TextBox();
            this.wfTxtBox = new System.Windows.Forms.TextBox();
            this.numberOfPopulationtxt = new System.Windows.Forms.TextBox();
            this.nogTxtBox = new System.Windows.Forms.TextBox();
            this.strategyCB = new System.Windows.Forms.ComboBox();
            this.cflbl = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.filenameTextBox = new System.Windows.Forms.TextBox();
            this.problemTypeComboBox = new System.Windows.Forms.ComboBox();
            this.optimizeBtn = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.browseBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.heuristicComboBox = new System.Windows.Forms.ComboBox();
            this.statusbar = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.statuslbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label8 = new System.Windows.Forms.Label();
            this.runsTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.statusbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cfTxtBox);
            this.groupBox1.Controls.Add(this.crTxtBox);
            this.groupBox1.Controls.Add(this.wfTxtBox);
            this.groupBox1.Controls.Add(this.numberOfPopulationtxt);
            this.groupBox1.Controls.Add(this.nogTxtBox);
            this.groupBox1.Controls.Add(this.strategyCB);
            this.groupBox1.Controls.Add(this.cflbl);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(639, 153);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(447, 295);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PARAMETERS";
            // 
            // cfTxtBox
            // 
            this.cfTxtBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cfTxtBox.Location = new System.Drawing.Point(246, 237);
            this.cfTxtBox.Name = "cfTxtBox";
            this.cfTxtBox.Size = new System.Drawing.Size(168, 26);
            this.cfTxtBox.TabIndex = 11;
            this.cfTxtBox.Text = "0.5";
            this.cfTxtBox.Visible = false;
            // 
            // crTxtBox
            // 
            this.crTxtBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.crTxtBox.Location = new System.Drawing.Point(246, 197);
            this.crTxtBox.Name = "crTxtBox";
            this.crTxtBox.Size = new System.Drawing.Size(168, 26);
            this.crTxtBox.TabIndex = 9;
            this.crTxtBox.Text = "0.9";
            // 
            // wfTxtBox
            // 
            this.wfTxtBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wfTxtBox.Location = new System.Drawing.Point(246, 157);
            this.wfTxtBox.Name = "wfTxtBox";
            this.wfTxtBox.Size = new System.Drawing.Size(170, 26);
            this.wfTxtBox.TabIndex = 7;
            this.wfTxtBox.Text = "0.3";
            // 
            // numberOfPopulationtxt
            // 
            this.numberOfPopulationtxt.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numberOfPopulationtxt.Location = new System.Drawing.Point(245, 37);
            this.numberOfPopulationtxt.Name = "numberOfPopulationtxt";
            this.numberOfPopulationtxt.Size = new System.Drawing.Size(169, 26);
            this.numberOfPopulationtxt.TabIndex = 1;
            this.numberOfPopulationtxt.Text = "100";
            this.numberOfPopulationtxt.TextChanged += new System.EventHandler(this.UpdateTextChanged);
            // 
            // nogTxtBox
            // 
            this.nogTxtBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.nogTxtBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nogTxtBox.Location = new System.Drawing.Point(245, 77);
            this.nogTxtBox.Name = "nogTxtBox";
            this.nogTxtBox.Size = new System.Drawing.Size(169, 26);
            this.nogTxtBox.TabIndex = 3;
            this.nogTxtBox.Text = "500";
            // 
            // strategyCB
            // 
            this.strategyCB.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.strategyCB.FormattingEnabled = true;
            this.strategyCB.Items.AddRange(new object[] {
            "Strategy 1",
            "Strategy 2",
            "Strategy 3",
            "Strategy 4",
            "Strategy 5",
            "Strategy 6",
            "Strategy 7",
            "Strategy 8",
            "Strategy 9",
            "Strategy 10"});
            this.strategyCB.Location = new System.Drawing.Point(246, 117);
            this.strategyCB.Name = "strategyCB";
            this.strategyCB.Size = new System.Drawing.Size(170, 27);
            this.strategyCB.TabIndex = 5;
            this.strategyCB.Text = "Strategy 1";
            this.strategyCB.SelectedIndexChanged += new System.EventHandler(this.UpdateTextChanged);
            // 
            // cflbl
            // 
            this.cflbl.AutoSize = true;
            this.cflbl.ForeColor = System.Drawing.Color.Maroon;
            this.cflbl.Location = new System.Drawing.Point(28, 237);
            this.cflbl.Name = "cflbl";
            this.cflbl.Size = new System.Drawing.Size(117, 21);
            this.cflbl.TabIndex = 10;
            this.cflbl.Text = "Control Factor";
            this.cflbl.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Maroon;
            this.label7.Location = new System.Drawing.Point(28, 197);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(122, 21);
            this.label7.TabIndex = 8;
            this.label7.Text = "Crossover Rate";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Maroon;
            this.label6.Location = new System.Drawing.Point(28, 157);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(138, 21);
            this.label6.TabIndex = 6;
            this.label6.Text = "Weighting Factor";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Maroon;
            this.label5.Location = new System.Drawing.Point(28, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 21);
            this.label5.TabIndex = 4;
            this.label5.Text = "Strategy";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Maroon;
            this.label4.Location = new System.Drawing.Point(28, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(178, 21);
            this.label4.TabIndex = 2;
            this.label4.Text = "Number of Generation";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Maroon;
            this.label3.Location = new System.Drawing.Point(28, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 21);
            this.label3.TabIndex = 0;
            this.label3.Text = "Population Size";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(149, 211);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 21);
            this.label2.TabIndex = 6;
            this.label2.Text = "Problem Type";
            // 
            // filenameTextBox
            // 
            this.filenameTextBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filenameTextBox.Location = new System.Drawing.Point(3, 45);
            this.filenameTextBox.Name = "filenameTextBox";
            this.filenameTextBox.Size = new System.Drawing.Size(746, 26);
            this.filenameTextBox.TabIndex = 1;
            this.filenameTextBox.TextChanged += new System.EventHandler(this.UpdateTextChanged);
            // 
            // problemTypeComboBox
            // 
            this.problemTypeComboBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.problemTypeComboBox.FormattingEnabled = true;
            this.problemTypeComboBox.Items.AddRange(new object[] {
            "TSP",
            "LOP",
            "PFSP",
            "QAP"});
            this.problemTypeComboBox.Location = new System.Drawing.Point(292, 211);
            this.problemTypeComboBox.Name = "problemTypeComboBox";
            this.problemTypeComboBox.Size = new System.Drawing.Size(289, 27);
            this.problemTypeComboBox.TabIndex = 2;
            // 
            // optimizeBtn
            // 
            this.optimizeBtn.BackColor = System.Drawing.Color.Maroon;
            this.optimizeBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.optimizeBtn.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optimizeBtn.ForeColor = System.Drawing.Color.White;
            this.optimizeBtn.Location = new System.Drawing.Point(542, 482);
            this.optimizeBtn.Name = "optimizeBtn";
            this.optimizeBtn.Size = new System.Drawing.Size(176, 35);
            this.optimizeBtn.TabIndex = 4;
            this.optimizeBtn.Text = "Optimize";
            this.optimizeBtn.UseVisualStyleBackColor = false;
            this.optimizeBtn.Click += new System.EventHandler(this.startAsyncButton_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(149, 151);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 21);
            this.label9.TabIndex = 5;
            this.label9.Text = "Heuristic";
            // 
            // browseBtn
            // 
            this.browseBtn.BackColor = System.Drawing.Color.Maroon;
            this.browseBtn.ForeColor = System.Drawing.Color.White;
            this.browseBtn.Location = new System.Drawing.Point(755, 43);
            this.browseBtn.Name = "browseBtn";
            this.browseBtn.Size = new System.Drawing.Size(89, 31);
            this.browseBtn.TabIndex = 2;
            this.browseBtn.Text = "Browse";
            this.browseBtn.UseVisualStyleBackColor = false;
            this.browseBtn.Click += new System.EventHandler(this.browseBtn_Click);
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.browseBtn);
            this.panel1.Controls.Add(this.filenameTextBox);
            this.panel1.Location = new System.Drawing.Point(110, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(944, 98);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Choose data file";
            // 
            // heuristicComboBox
            // 
            this.heuristicComboBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.heuristicComboBox.FormattingEnabled = true;
            this.heuristicComboBox.Items.AddRange(new object[] {
            "EDE",
            "EDET"});
            this.heuristicComboBox.Location = new System.Drawing.Point(292, 151);
            this.heuristicComboBox.Name = "heuristicComboBox";
            this.heuristicComboBox.Size = new System.Drawing.Size(289, 27);
            this.heuristicComboBox.TabIndex = 1;
            this.heuristicComboBox.Text = "EDET";
            // 
            // statusbar
            // 
            this.statusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.statuslbl});
            this.statusbar.Location = new System.Drawing.Point(0, 494);
            this.statusbar.Name = "statusbar";
            this.statusbar.Size = new System.Drawing.Size(1166, 22);
            this.statusbar.TabIndex = 7;
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // statuslbl
            // 
            this.statuslbl.Name = "statuslbl";
            this.statuslbl.Size = new System.Drawing.Size(0, 17);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(149, 276);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(132, 21);
            this.label8.TabIndex = 8;
            this.label8.Text = "Number of Runs";
            // 
            // runsTextBox
            // 
            this.runsTextBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runsTextBox.Location = new System.Drawing.Point(292, 275);
            this.runsTextBox.Name = "runsTextBox";
            this.runsTextBox.Size = new System.Drawing.Size(289, 26);
            this.runsTextBox.TabIndex = 9;
            this.runsTextBox.Text = "15";
            this.runsTextBox.TextChanged += new System.EventHandler(this.runsTextBox_TextChanged);
            // 
            // InputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Olive;
            this.ClientSize = new System.Drawing.Size(1166, 516);
            this.Controls.Add(this.runsTextBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.statusbar);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.heuristicComboBox);
            this.Controls.Add(this.optimizeBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.problemTypeComboBox);
            this.Controls.Add(this.groupBox1);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "InputForm";
            this.Text = "Input Form";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusbar.ResumeLayout(false);
            this.statusbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label cflbl;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox filenameTextBox;
        private System.Windows.Forms.ComboBox problemTypeComboBox;
        private System.Windows.Forms.Button optimizeBtn;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button browseBtn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox heuristicComboBox;
        private System.Windows.Forms.ComboBox strategyCB;
        private System.Windows.Forms.TextBox cfTxtBox;
        private System.Windows.Forms.TextBox crTxtBox;
        private System.Windows.Forms.TextBox wfTxtBox;
        private System.Windows.Forms.TextBox numberOfPopulationtxt;
        private System.Windows.Forms.TextBox nogTxtBox;
        private System.Windows.Forms.StatusStrip statusbar;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel statuslbl;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox runsTextBox;

    }
}