namespace fa.views.hms.helper
{
    partial class FormRecentPrescription
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
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRecentPrescription));
            BtnPrescriptionCancel = new Button();
            BtnPrescrptionSelect = new Button();
            GridViewRecentPrescription = new controls.DataViewVerticalScroll();
            Date = new DataGridViewTextBoxColumn();
            Amount = new DataGridViewTextBoxColumn();
            Patient = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            InvNo = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewCheckBoxColumn();
            Column2 = new DataGridViewCheckBoxColumn();
            Column10 = new DataGridViewTextBoxColumn();
            statusStrip1 = new StatusStrip();
            RecentPrescriptionErrorMsg = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)GridViewRecentPrescription).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(172, 215);
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(172, 189);
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(172, 163);
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // BtnPrescriptionCancel
            // 
            BtnPrescriptionCancel.DialogResult = DialogResult.Cancel;
            BtnPrescriptionCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrescriptionCancel.Location = new Point(655, 233);
            BtnPrescriptionCancel.Name = "BtnPrescriptionCancel";
            BtnPrescriptionCancel.Size = new Size(88, 23);
            BtnPrescriptionCancel.TabIndex = 5;
            BtnPrescriptionCancel.Text = "Cancel [Esc]";
            BtnPrescriptionCancel.UseVisualStyleBackColor = true;
            BtnPrescriptionCancel.Click += BtnPrescriptionCancel_Click;
            // 
            // BtnPrescrptionSelect
            // 
            BtnPrescrptionSelect.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrescrptionSelect.Location = new Point(749, 233);
            BtnPrescrptionSelect.Name = "BtnPrescrptionSelect";
            BtnPrescrptionSelect.Size = new Size(88, 23);
            BtnPrescrptionSelect.TabIndex = 4;
            BtnPrescrptionSelect.Text = "Select [F8]";
            BtnPrescrptionSelect.UseVisualStyleBackColor = true;
            BtnPrescrptionSelect.Click += BtnPrescrptionSelect_Click;
            BtnPrescrptionSelect.PreviewKeyDown += BtnPrescrptionSelect_PreviewKeyDown;
            // 
            // GridViewRecentPrescription
            // 
            GridViewRecentPrescription.AllowUserToAddRows = false;
            GridViewRecentPrescription.AllowUserToDeleteRows = false;
            GridViewRecentPrescription.AllowUserToResizeColumns = false;
            GridViewRecentPrescription.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            GridViewRecentPrescription.BackgroundColor = SystemColors.Control;
            GridViewRecentPrescription.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewRecentPrescription.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewRecentPrescription.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GridViewRecentPrescription.Columns.AddRange(new DataGridViewColumn[] { Date, Amount, Patient, Column3, InvNo, Column1, Column2, Column10 });
            GridViewRecentPrescription.EnableHeadersVisualStyles = false;
            GridViewRecentPrescription.Location = new Point(4, 5);
            GridViewRecentPrescription.MultiSelect = false;
            GridViewRecentPrescription.Name = "GridViewRecentPrescription";
            GridViewRecentPrescription.ReadOnly = true;
            GridViewRecentPrescription.RowHeadersVisible = false;
            GridViewRecentPrescription.RowTemplate.Height = 20;
            GridViewRecentPrescription.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewRecentPrescription.ShowCellToolTips = false;
            GridViewRecentPrescription.Size = new Size(858, 219);
            GridViewRecentPrescription.TabIndex = 3;
            GridViewRecentPrescription.TabStop = false;
            GridViewRecentPrescription.CellDoubleClick += GridViewRecentPrescription_CellDoubleClick;
            GridViewRecentPrescription.KeyDown += GridViewRecentPrescription_KeyDown;
            // 
            // Date
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            Date.DefaultCellStyle = dataGridViewCellStyle2;
            Date.HeaderText = "Date";
            Date.Name = "Date";
            Date.ReadOnly = true;
            Date.Resizable = DataGridViewTriState.False;
            Date.SortMode = DataGridViewColumnSortMode.NotSortable;
            Date.Width = 110;
            // 
            // Amount
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            Amount.DefaultCellStyle = dataGridViewCellStyle3;
            Amount.HeaderText = "Token No";
            Amount.Name = "Amount";
            Amount.ReadOnly = true;
            Amount.Resizable = DataGridViewTriState.False;
            Amount.SortMode = DataGridViewColumnSortMode.NotSortable;
            Amount.Width = 70;
            // 
            // Patient
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            Patient.DefaultCellStyle = dataGridViewCellStyle4;
            Patient.HeaderText = "Patient Name";
            Patient.Name = "Patient";
            Patient.ReadOnly = true;
            Patient.Resizable = DataGridViewTriState.False;
            Patient.SortMode = DataGridViewColumnSortMode.NotSortable;
            Patient.Width = 175;
            // 
            // Column3
            // 
            Column3.HeaderText = "Patient ID";
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            Column3.Resizable = DataGridViewTriState.False;
            Column3.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column3.Width = 150;
            // 
            // InvNo
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            InvNo.DefaultCellStyle = dataGridViewCellStyle5;
            InvNo.HeaderText = "Prescriptions";
            InvNo.Name = "InvNo";
            InvNo.ReadOnly = true;
            InvNo.Resizable = DataGridViewTriState.False;
            InvNo.SortMode = DataGridViewColumnSortMode.NotSortable;
            InvNo.Width = 200;
            // 
            // Column1
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.NullValue = false;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            Column1.DefaultCellStyle = dataGridViewCellStyle6;
            Column1.HeaderText = "Payment Received?";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.Width = 65;
            // 
            // Column2
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.NullValue = false;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            Column2.DefaultCellStyle = dataGridViewCellStyle7;
            Column2.HeaderText = "Deliverd?";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Resizable = DataGridViewTriState.False;
            Column2.Width = 65;
            // 
            // Column10
            // 
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
            Column10.DefaultCellStyle = dataGridViewCellStyle8;
            Column10.HeaderText = "Id";
            Column10.Name = "Column10";
            Column10.ReadOnly = true;
            Column10.Resizable = DataGridViewTriState.False;
            Column10.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column10.Visible = false;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { RecentPrescriptionErrorMsg });
            statusStrip1.Location = new Point(0, 267);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(865, 22);
            statusStrip1.TabIndex = 154;
            statusStrip1.Text = "statusStrip1";
            // 
            // RecentPrescriptionErrorMsg
            // 
            RecentPrescriptionErrorMsg.Name = "RecentPrescriptionErrorMsg";
            RecentPrescriptionErrorMsg.Size = new Size(49, 17);
            RecentPrescriptionErrorMsg.Text = "              ";
            // 
            // FormRecentPrescription
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(865, 289);
            Controls.Add(statusStrip1);
            Controls.Add(BtnPrescriptionCancel);
            Controls.Add(BtnPrescrptionSelect);
            Controls.Add(GridViewRecentPrescription);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormRecentPrescription";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Recent Prescription";
            Load += FormRecentPrescription_Load;
            Controls.SetChildIndex(GridViewRecentPrescription, 0);
            Controls.SetChildIndex(BtnPrescrptionSelect, 0);
            Controls.SetChildIndex(BtnPrescriptionCancel, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            ((System.ComponentModel.ISupportInitialize)GridViewRecentPrescription).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button BtnPrescriptionCancel;
        private System.Windows.Forms.Button BtnPrescrptionSelect;
        private controls.DataViewVerticalScroll GridViewRecentPrescription;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel RecentPrescriptionErrorMsg;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Patient;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn InvNo;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
    }
}