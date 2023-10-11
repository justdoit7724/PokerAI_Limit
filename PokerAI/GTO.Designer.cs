namespace PokerAI
{
    partial class GTO
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.C1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CQ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CJ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BTN_GTO_CHECK = new System.Windows.Forms.Button();
            this.BTN_GTO_RAISE = new System.Windows.Forms.Button();
            this.RB_GTO_SUITED = new System.Windows.Forms.RadioButton();
            this.RB_GTO_OFFSUIT = new System.Windows.Forms.RadioButton();
            this.LB_GTO_HISTORY = new System.Windows.Forms.ListBox();
            this.BTN_GTO_CLEAR = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TB_GTO_FOLD = new System.Windows.Forms.TextBox();
            this.TB_GTO_RAISE = new System.Windows.Forms.TextBox();
            this.TB_GTO_CALL = new System.Windows.Forms.TextBox();
            this.TB_GTO_CHECK = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.C1,
            this.CK,
            this.CQ,
            this.CJ,
            this.C10,
            this.C9,
            this.C8,
            this.C7,
            this.C6,
            this.C5,
            this.C4,
            this.C3,
            this.C2});
            this.dataGridView1.Location = new System.Drawing.Point(166, 13);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(1392, 700);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
            this.dataGridView1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridView1_CellPainting);
            // 
            // C1
            // 
            this.C1.HeaderText = "A";
            this.C1.MinimumWidth = 6;
            this.C1.Name = "C1";
            this.C1.Width = 80;
            // 
            // CK
            // 
            this.CK.HeaderText = "K";
            this.CK.MinimumWidth = 6;
            this.CK.Name = "CK";
            this.CK.Width = 80;
            // 
            // CQ
            // 
            this.CQ.HeaderText = "Q";
            this.CQ.MinimumWidth = 6;
            this.CQ.Name = "CQ";
            this.CQ.Width = 80;
            // 
            // CJ
            // 
            this.CJ.HeaderText = "J";
            this.CJ.MinimumWidth = 6;
            this.CJ.Name = "CJ";
            this.CJ.Width = 80;
            // 
            // C10
            // 
            this.C10.HeaderText = "10";
            this.C10.MinimumWidth = 6;
            this.C10.Name = "C10";
            this.C10.Width = 80;
            // 
            // C9
            // 
            this.C9.HeaderText = "9";
            this.C9.MinimumWidth = 6;
            this.C9.Name = "C9";
            this.C9.Width = 80;
            // 
            // C8
            // 
            this.C8.HeaderText = "8";
            this.C8.MinimumWidth = 6;
            this.C8.Name = "C8";
            this.C8.Width = 80;
            // 
            // C7
            // 
            this.C7.HeaderText = "7";
            this.C7.MinimumWidth = 6;
            this.C7.Name = "C7";
            this.C7.Width = 80;
            // 
            // C6
            // 
            this.C6.HeaderText = "6";
            this.C6.MinimumWidth = 6;
            this.C6.Name = "C6";
            this.C6.Width = 80;
            // 
            // C5
            // 
            this.C5.HeaderText = "5";
            this.C5.MinimumWidth = 6;
            this.C5.Name = "C5";
            this.C5.Width = 80;
            // 
            // C4
            // 
            this.C4.HeaderText = "4";
            this.C4.MinimumWidth = 6;
            this.C4.Name = "C4";
            this.C4.Width = 80;
            // 
            // C3
            // 
            this.C3.HeaderText = "3";
            this.C3.MinimumWidth = 6;
            this.C3.Name = "C3";
            this.C3.Width = 80;
            // 
            // C2
            // 
            this.C2.HeaderText = "2";
            this.C2.MinimumWidth = 6;
            this.C2.Name = "C2";
            this.C2.Width = 80;
            // 
            // BTN_GTO_CHECK
            // 
            this.BTN_GTO_CHECK.Location = new System.Drawing.Point(21, 105);
            this.BTN_GTO_CHECK.Name = "BTN_GTO_CHECK";
            this.BTN_GTO_CHECK.Size = new System.Drawing.Size(117, 49);
            this.BTN_GTO_CHECK.TabIndex = 1;
            this.BTN_GTO_CHECK.Text = "CHECK";
            this.BTN_GTO_CHECK.UseVisualStyleBackColor = true;
            this.BTN_GTO_CHECK.Click += new System.EventHandler(this.BTN_GTO_CHECK_Click);
            // 
            // BTN_GTO_RAISE
            // 
            this.BTN_GTO_RAISE.Location = new System.Drawing.Point(21, 160);
            this.BTN_GTO_RAISE.Name = "BTN_GTO_RAISE";
            this.BTN_GTO_RAISE.Size = new System.Drawing.Size(117, 49);
            this.BTN_GTO_RAISE.TabIndex = 2;
            this.BTN_GTO_RAISE.Text = "RAISE";
            this.BTN_GTO_RAISE.UseVisualStyleBackColor = true;
            this.BTN_GTO_RAISE.Click += new System.EventHandler(this.BTN_GTO_RAISE_Click);
            // 
            // RB_GTO_SUITED
            // 
            this.RB_GTO_SUITED.AutoSize = true;
            this.RB_GTO_SUITED.Checked = true;
            this.RB_GTO_SUITED.Location = new System.Drawing.Point(21, 31);
            this.RB_GTO_SUITED.Name = "RB_GTO_SUITED";
            this.RB_GTO_SUITED.Size = new System.Drawing.Size(69, 19);
            this.RB_GTO_SUITED.TabIndex = 1;
            this.RB_GTO_SUITED.TabStop = true;
            this.RB_GTO_SUITED.Text = "Suited";
            this.RB_GTO_SUITED.UseVisualStyleBackColor = true;
            this.RB_GTO_SUITED.CheckedChanged += new System.EventHandler(this.RB_GTO_SUITED_CheckedChanged);
            // 
            // RB_GTO_OFFSUIT
            // 
            this.RB_GTO_OFFSUIT.AutoSize = true;
            this.RB_GTO_OFFSUIT.Location = new System.Drawing.Point(21, 56);
            this.RB_GTO_OFFSUIT.Name = "RB_GTO_OFFSUIT";
            this.RB_GTO_OFFSUIT.Size = new System.Drawing.Size(71, 19);
            this.RB_GTO_OFFSUIT.TabIndex = 2;
            this.RB_GTO_OFFSUIT.Text = "Offsuit";
            this.RB_GTO_OFFSUIT.UseVisualStyleBackColor = true;
            this.RB_GTO_OFFSUIT.CheckedChanged += new System.EventHandler(this.RB_GTO_OFFSUIT_CheckedChanged);
            // 
            // LB_GTO_HISTORY
            // 
            this.LB_GTO_HISTORY.FormattingEnabled = true;
            this.LB_GTO_HISTORY.ItemHeight = 15;
            this.LB_GTO_HISTORY.Location = new System.Drawing.Point(21, 270);
            this.LB_GTO_HISTORY.Name = "LB_GTO_HISTORY";
            this.LB_GTO_HISTORY.Size = new System.Drawing.Size(120, 109);
            this.LB_GTO_HISTORY.TabIndex = 3;
            // 
            // BTN_GTO_CLEAR
            // 
            this.BTN_GTO_CLEAR.Location = new System.Drawing.Point(21, 215);
            this.BTN_GTO_CLEAR.Name = "BTN_GTO_CLEAR";
            this.BTN_GTO_CLEAR.Size = new System.Drawing.Size(117, 49);
            this.BTN_GTO_CLEAR.TabIndex = 4;
            this.BTN_GTO_CLEAR.Text = "Clear";
            this.BTN_GTO_CLEAR.UseVisualStyleBackColor = true;
            this.BTN_GTO_CLEAR.Click += new System.EventHandler(this.BTN_GTO_CLEAR_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(4, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Check";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TB_GTO_FOLD);
            this.groupBox1.Controls.Add(this.TB_GTO_RAISE);
            this.groupBox1.Controls.Add(this.TB_GTO_CALL);
            this.groupBox1.Controls.Add(this.TB_GTO_CHECK);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(21, 402);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(120, 192);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Detail";
            // 
            // TB_GTO_FOLD
            // 
            this.TB_GTO_FOLD.Location = new System.Drawing.Point(61, 149);
            this.TB_GTO_FOLD.Name = "TB_GTO_FOLD";
            this.TB_GTO_FOLD.ReadOnly = true;
            this.TB_GTO_FOLD.Size = new System.Drawing.Size(53, 25);
            this.TB_GTO_FOLD.TabIndex = 12;
            // 
            // TB_GTO_RAISE
            // 
            this.TB_GTO_RAISE.Location = new System.Drawing.Point(61, 108);
            this.TB_GTO_RAISE.Name = "TB_GTO_RAISE";
            this.TB_GTO_RAISE.ReadOnly = true;
            this.TB_GTO_RAISE.Size = new System.Drawing.Size(53, 25);
            this.TB_GTO_RAISE.TabIndex = 11;
            // 
            // TB_GTO_CALL
            // 
            this.TB_GTO_CALL.Location = new System.Drawing.Point(61, 67);
            this.TB_GTO_CALL.Name = "TB_GTO_CALL";
            this.TB_GTO_CALL.ReadOnly = true;
            this.TB_GTO_CALL.Size = new System.Drawing.Size(53, 25);
            this.TB_GTO_CALL.TabIndex = 10;
            // 
            // TB_GTO_CHECK
            // 
            this.TB_GTO_CHECK.Location = new System.Drawing.Point(61, 24);
            this.TB_GTO_CHECK.Name = "TB_GTO_CHECK";
            this.TB_GTO_CHECK.ReadOnly = true;
            this.TB_GTO_CHECK.Size = new System.Drawing.Size(53, 25);
            this.TB_GTO_CHECK.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(6, 159);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "Fold";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label3.Location = new System.Drawing.Point(6, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Raise";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label2.Location = new System.Drawing.Point(6, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "Call";
            // 
            // GTO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1566, 721);
            this.Controls.Add(this.BTN_GTO_CLEAR);
            this.Controls.Add(this.LB_GTO_HISTORY);
            this.Controls.Add(this.RB_GTO_OFFSUIT);
            this.Controls.Add(this.RB_GTO_SUITED);
            this.Controls.Add(this.BTN_GTO_RAISE);
            this.Controls.Add(this.BTN_GTO_CHECK);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox1);
            this.Name = "GTO";
            this.Text = "GTO";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button BTN_GTO_CHECK;
        private System.Windows.Forms.Button BTN_GTO_RAISE;
        private System.Windows.Forms.RadioButton RB_GTO_SUITED;
        private System.Windows.Forms.RadioButton RB_GTO_OFFSUIT;
        private System.Windows.Forms.DataGridViewTextBoxColumn C1;
        private System.Windows.Forms.DataGridViewTextBoxColumn CK;
        private System.Windows.Forms.DataGridViewTextBoxColumn CQ;
        private System.Windows.Forms.DataGridViewTextBoxColumn CJ;
        private System.Windows.Forms.DataGridViewTextBoxColumn C10;
        private System.Windows.Forms.DataGridViewTextBoxColumn C9;
        private System.Windows.Forms.DataGridViewTextBoxColumn C8;
        private System.Windows.Forms.DataGridViewTextBoxColumn C7;
        private System.Windows.Forms.DataGridViewTextBoxColumn C6;
        private System.Windows.Forms.DataGridViewTextBoxColumn C5;
        private System.Windows.Forms.DataGridViewTextBoxColumn C4;
        private System.Windows.Forms.DataGridViewTextBoxColumn C3;
        private System.Windows.Forms.DataGridViewTextBoxColumn C2;
        private System.Windows.Forms.ListBox LB_GTO_HISTORY;
        private System.Windows.Forms.Button BTN_GTO_CLEAR;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox TB_GTO_FOLD;
        private System.Windows.Forms.TextBox TB_GTO_RAISE;
        private System.Windows.Forms.TextBox TB_GTO_CALL;
        private System.Windows.Forms.TextBox TB_GTO_CHECK;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}