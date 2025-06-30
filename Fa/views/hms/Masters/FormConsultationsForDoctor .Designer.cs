namespace fa.views.hms.Masters
{
    partial class FormConsultationForDoctor
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConsultationForDoctor));
            statusStrip1 = new StatusStrip();
            ErrorMsgConsultationDoctor = new ToolStripStatusLabel();
            BtnConsultationDoctorExit = new Button();
            BtnConsultationDoctorCancel = new Button();
            BtnConsultationDoctorSave = new Button();
            GridViewConsultationDoctor = new controls.DataViewVerticalScroll();
            Column1 = new DataGridViewComboBoxColumn();
            Column6 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new controls.grid.DataGridViewCurrencyColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            Column7 = new DataGridViewTextBoxColumn();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewConsultationDoctor).BeginInit();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsgConsultationDoctor });
            statusStrip1.Location = new Point(0, 475);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(923, 22);
            statusStrip1.TabIndex = 46;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsgConsultationDoctor
            // 
            ErrorMsgConsultationDoctor.Name = "ErrorMsgConsultationDoctor";
            ErrorMsgConsultationDoctor.Size = new Size(19, 17);
            ErrorMsgConsultationDoctor.Text = "    ";
            // 
            // BtnConsultationDoctorExit
            // 
            BtnConsultationDoctorExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnConsultationDoctorExit.Location = new Point(815, 437);
            BtnConsultationDoctorExit.Name = "BtnConsultationDoctorExit";
            BtnConsultationDoctorExit.Size = new Size(83, 24);
            BtnConsultationDoctorExit.TabIndex = 45;
            BtnConsultationDoctorExit.Text = "Exit [F10]";
            BtnConsultationDoctorExit.UseVisualStyleBackColor = true;
            BtnConsultationDoctorExit.Click += BtnConsultationDoctorExit_Click;
            // 
            // BtnConsultationDoctorCancel
            // 
            BtnConsultationDoctorCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnConsultationDoctorCancel.Location = new Point(637, 437);
            BtnConsultationDoctorCancel.Name = "BtnConsultationDoctorCancel";
            BtnConsultationDoctorCancel.Size = new Size(83, 24);
            BtnConsultationDoctorCancel.TabIndex = 44;
            BtnConsultationDoctorCancel.Text = "Cancel [Esc]";
            BtnConsultationDoctorCancel.UseVisualStyleBackColor = true;
            BtnConsultationDoctorCancel.Click += BtnConsultationDoctorCancel_Click;
            // 
            // BtnConsultationDoctorSave
            // 
            BtnConsultationDoctorSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnConsultationDoctorSave.Location = new Point(726, 437);
            BtnConsultationDoctorSave.Name = "BtnConsultationDoctorSave";
            BtnConsultationDoctorSave.Size = new Size(83, 24);
            BtnConsultationDoctorSave.TabIndex = 42;
            BtnConsultationDoctorSave.Text = "Save [F8]";
            BtnConsultationDoctorSave.UseVisualStyleBackColor = true;
            BtnConsultationDoctorSave.Click += BtnConsultationDoctorSave_Click;
            // 
            // GridViewConsultationDoctor
            // 
            GridViewConsultationDoctor.BackgroundColor = SystemColors.Control;
            GridViewConsultationDoctor.ColumnHeadersHeight = 20;
            GridViewConsultationDoctor.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewConsultationDoctor.Columns.AddRange(new DataGridViewColumn[] { Column1, Column6, Column2, Column3, Column4, Column5, Column7 });
            GridViewConsultationDoctor.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewConsultationDoctor.EnableHeadersVisualStyles = false;
            GridViewConsultationDoctor.Location = new Point(12, 12);
            GridViewConsultationDoctor.Name = "GridViewConsultationDoctor";
            GridViewConsultationDoctor.RowHeadersVisible = false;
            GridViewConsultationDoctor.RowTemplate.Height = 20;
            GridViewConsultationDoctor.ShowCellToolTips = false;
            GridViewConsultationDoctor.Size = new Size(898, 412);
            GridViewConsultationDoctor.TabIndex = 47;
            // 
            // Column1
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridViewCellStyle1.ForeColor = Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = Color.White;
            dataGridViewCellStyle1.SelectionForeColor = Color.Black;
            Column1.DefaultCellStyle = dataGridViewCellStyle1;
            Column1.FlatStyle = FlatStyle.Popup;
            Column1.HeaderText = "Name";
            Column1.Name = "Column1";
            Column1.Resizable = DataGridViewTriState.True;
            Column1.SortMode = DataGridViewColumnSortMode.Automatic;
            Column1.Width = 200;
            // 
            // Column6
            // 
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = Color.White;
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            Column6.DefaultCellStyle = dataGridViewCellStyle2;
            Column6.HeaderText = "Display As";
            Column6.Name = "Column6";
            Column6.Width = 225;
            // 
            // Column2
            // 
            Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.White;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            Column2.DefaultCellStyle = dataGridViewCellStyle3;
            Column2.HeaderText = "Description";
            Column2.Name = "Column2";
            // 
            // Column3
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.ForeColor = Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = Color.White;
            dataGridViewCellStyle4.SelectionForeColor = Color.Black;
            Column3.DefaultCellStyle = dataGridViewCellStyle4;
            Column3.HeaderText = "Fee";
            Column3.Name = "Column3";
            Column3.Resizable = DataGridViewTriState.True;
            Column3.SortMode = DataGridViewColumnSortMode.Automatic;
            Column3.Width = 75;
            // 
            // Column4
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.ForeColor = Color.Black;
            dataGridViewCellStyle5.NullValue = "X";
            dataGridViewCellStyle5.SelectionBackColor = Color.White;
            dataGridViewCellStyle5.SelectionForeColor = Color.Black;
            Column4.DefaultCellStyle = dataGridViewCellStyle5;
            Column4.HeaderText = "...";
            Column4.Name = "Column4";
            Column4.Width = 25;
            // 
            // Column5
            // 
            Column5.HeaderText = "ID";
            Column5.Name = "Column5";
            Column5.Visible = false;
            // 
            // Column7
            // 
            Column7.HeaderText = "CID";
            Column7.Name = "Column7";
            Column7.Visible = false;
            // 
            // FormConsultationForDoctor
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(923, 497);
            Controls.Add(GridViewConsultationDoctor);
            Controls.Add(statusStrip1);
            Controls.Add(BtnConsultationDoctorExit);
            Controls.Add(BtnConsultationDoctorCancel);
            Controls.Add(BtnConsultationDoctorSave);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormConsultationForDoctor";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Consultation Details & Fees by Care Taker";
            Load += FormConsultationForDoctor_Load;
            Controls.SetChildIndex(BtnConsultationDoctorSave, 0);
            Controls.SetChildIndex(BtnConsultationDoctorCancel, 0);
            Controls.SetChildIndex(BtnConsultationDoctorExit, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(GridViewConsultationDoctor, 0);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewConsultationDoctor).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button BtnConsultationDoctorExit;
        private System.Windows.Forms.Button BtnConsultationDoctorCancel;
        private System.Windows.Forms.Button BtnConsultationDoctorSave;
        private System.Windows.Forms.ToolStripStatusLabel ErrorMsgConsultationDoctor;
        private controls.DataViewVerticalScroll GridViewConsultationDoctor;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private controls.grid.DataGridViewCurrencyColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
    }
}