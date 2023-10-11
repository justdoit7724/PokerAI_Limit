namespace PokerAI
{
    partial class Form1
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
            this.BTN_MAIN_PLAY = new System.Windows.Forms.Button();
            this.BTN_GTO_CHART = new System.Windows.Forms.Button();
            this.RD_DIFFICULTY_NORMAL = new System.Windows.Forms.RadioButton();
            this.RD_DIFFICULTY_HARD = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BTN_MAIN_PLAY
            // 
            this.BTN_MAIN_PLAY.Location = new System.Drawing.Point(16, 14);
            this.BTN_MAIN_PLAY.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BTN_MAIN_PLAY.Name = "BTN_MAIN_PLAY";
            this.BTN_MAIN_PLAY.Size = new System.Drawing.Size(109, 78);
            this.BTN_MAIN_PLAY.TabIndex = 0;
            this.BTN_MAIN_PLAY.Text = "Play";
            this.BTN_MAIN_PLAY.UseVisualStyleBackColor = true;
            this.BTN_MAIN_PLAY.Click += new System.EventHandler(this.BTN_MAIN_PLAY_Click);
            // 
            // BTN_GTO_CHART
            // 
            this.BTN_GTO_CHART.Location = new System.Drawing.Point(16, 99);
            this.BTN_GTO_CHART.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BTN_GTO_CHART.Name = "BTN_GTO_CHART";
            this.BTN_GTO_CHART.Size = new System.Drawing.Size(209, 78);
            this.BTN_GTO_CHART.TabIndex = 1;
            this.BTN_GTO_CHART.Text = "GTO Chart";
            this.BTN_GTO_CHART.UseVisualStyleBackColor = true;
            this.BTN_GTO_CHART.Click += new System.EventHandler(this.BTN_GTO_CHART_Click);
            // 
            // RD_DIFFICULTY_NORMAL
            // 
            this.RD_DIFFICULTY_NORMAL.AutoSize = true;
            this.RD_DIFFICULTY_NORMAL.Location = new System.Drawing.Point(8, 21);
            this.RD_DIFFICULTY_NORMAL.Name = "RD_DIFFICULTY_NORMAL";
            this.RD_DIFFICULTY_NORMAL.Size = new System.Drawing.Size(72, 19);
            this.RD_DIFFICULTY_NORMAL.TabIndex = 1;
            this.RD_DIFFICULTY_NORMAL.Text = "Normal";
            this.RD_DIFFICULTY_NORMAL.UseVisualStyleBackColor = true;
            this.RD_DIFFICULTY_NORMAL.CheckedChanged += new System.EventHandler(this.RD_DIFFICULTY_NORMAL_CheckedChanged);
            // 
            // RD_DIFFICULTY_HARD
            // 
            this.RD_DIFFICULTY_HARD.AutoSize = true;
            this.RD_DIFFICULTY_HARD.Checked = true;
            this.RD_DIFFICULTY_HARD.Location = new System.Drawing.Point(8, 47);
            this.RD_DIFFICULTY_HARD.Name = "RD_DIFFICULTY_HARD";
            this.RD_DIFFICULTY_HARD.Size = new System.Drawing.Size(58, 19);
            this.RD_DIFFICULTY_HARD.TabIndex = 2;
            this.RD_DIFFICULTY_HARD.TabStop = true;
            this.RD_DIFFICULTY_HARD.Text = "Hard";
            this.RD_DIFFICULTY_HARD.UseVisualStyleBackColor = true;
            this.RD_DIFFICULTY_HARD.CheckedChanged += new System.EventHandler(this.RD_DIFFICULTY_HARD_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RD_DIFFICULTY_HARD);
            this.groupBox1.Controls.Add(this.RD_DIFFICULTY_NORMAL);
            this.groupBox1.Location = new System.Drawing.Point(133, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(92, 78);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Difficulty";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 190);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BTN_GTO_CHART);
            this.Controls.Add(this.BTN_MAIN_PLAY);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BTN_MAIN_PLAY;
        private System.Windows.Forms.Button BTN_GTO_CHART;
        private System.Windows.Forms.RadioButton RD_DIFFICULTY_NORMAL;
        private System.Windows.Forms.RadioButton RD_DIFFICULTY_HARD;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

