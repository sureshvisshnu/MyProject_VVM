namespace fa.views.purchase
{
    partial class FormSearchBatch
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
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSearchBatch));
            BtnSearchSelect = new Button();
            GridViewBatch = new controls.DataViewVerticalScroll();
            PatientName = new DataGridViewTextBoxColumn();
            PatientAddress = new DataGridViewTextBoxColumn();
            Column2 = new controls.grid.DataGridViewQuantityColumn();
            uom = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            BatchMRP = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            BtnSearchCancel = new Button();
            toolStrip = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            TextBoxSearchBatch = new ToolStripTextBox();
            BtnSearchBatch = new ToolStripButton();
            statusStrip1 = new StatusStrip();
            BatchSearchErrorMsg = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)GridViewBatch).BeginInit();
            toolStrip.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // BtnSearchSelect
            // 
            BtnSearchSelect.Enabled = false;
            BtnSearchSelect.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSearchSelect.Location = new Point(582, 217);
            BtnSearchSelect.Name = "BtnSearchSelect";
            BtnSearchSelect.Size = new Size(88, 23);
            BtnSearchSelect.TabIndex = 1;
            BtnSearchSelect.Text = "Select [F8]";
            BtnSearchSelect.UseVisualStyleBackColor = true;
            BtnSearchSelect.Click += BtnSearchSelect_Click;
            BtnSearchSelect.PreviewKeyDown += BtnSearchSelect_PreviewKeyDown;
            // 
            // GridViewBatch
            // 
            GridViewBatch.AllowUserToAddRows = false;
            GridViewBatch.AllowUserToDeleteRows = false;
            GridViewBatch.AllowUserToResizeColumns = false;
            GridViewBatch.AllowUserToResizeRows = false;
            GridViewBatch.BackgroundColor = SystemColors.Control;
            GridViewBatch.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewBatch.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewBatch.ColumnHeadersHeight = 20;
            GridViewBatch.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewBatch.Columns.AddRange(new DataGridViewColumn[] { PatientName, PatientAddress, Column2, uom, Column3, BatchMRP, Column1 });
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = SystemColors.Window;
            dataGridViewCellStyle8.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle8.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            GridViewBatch.DefaultCellStyle = dataGridViewCellStyle8;
            GridViewBatch.EditMode = DataGridViewEditMode.EditProgrammatically;
            GridViewBatch.EnableHeadersVisualStyles = false;
            GridViewBatch.Location = new Point(4, 34);
            GridViewBatch.MultiSelect = false;
            GridViewBatch.Name = "GridViewBatch";
            GridViewBatch.ReadOnly = true;
            GridViewBatch.RowHeadersVisible = false;
            GridViewBatch.RowTemplate.Height = 20;
            GridViewBatch.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewBatch.ShowCellToolTips = false;
            GridViewBatch.ShowEditingIcon = false;
            GridViewBatch.Size = new Size(675, 177);
            GridViewBatch.TabIndex = 0;
            GridViewBatch.CellDoubleClick += GridViewBatch_CellDoubleClick;
            GridViewBatch.Enter += GridViewBatch_Enter;
            GridViewBatch.KeyDown += GridViewBatch_KeyDown;
            GridViewBatch.Leave += GridViewBatch_Leave;
            // 
            // PatientName
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            PatientName.DefaultCellStyle = dataGridViewCellStyle2;
            PatientName.HeaderText = "Batch No";
            PatientName.Name = "PatientName";
            PatientName.ReadOnly = true;
            PatientName.Resizable = DataGridViewTriState.False;
            PatientName.SortMode = DataGridViewColumnSortMode.NotSortable;
            PatientName.Width = 150;
            // 
            // PatientAddress
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            PatientAddress.DefaultCellStyle = dataGridViewCellStyle3;
            PatientAddress.HeaderText = "Exp Date";
            PatientAddress.Name = "PatientAddress";
            PatientAddress.ReadOnly = true;
            PatientAddress.Resizable = DataGridViewTriState.False;
            PatientAddress.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            Column2.DefaultCellStyle = dataGridViewCellStyle4;
            Column2.HeaderText = "QTY";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Resizable = DataGridViewTriState.False;
            // 
            // uom
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            uom.DefaultCellStyle = dataGridViewCellStyle5;
            uom.HeaderText = "UOM";
            uom.Name = "uom";
            uom.ReadOnly = true;
            uom.Resizable = DataGridViewTriState.False;
            uom.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column3
            // 
            Column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            Column3.DefaultCellStyle = dataGridViewCellStyle6;
            Column3.HeaderText = "Location";
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            Column3.Resizable = DataGridViewTriState.False;
            Column3.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // BatchMRP
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.TopRight;
            BatchMRP.DefaultCellStyle = dataGridViewCellStyle7;
            BatchMRP.HeaderText = "MRP";
            BatchMRP.Name = "BatchMRP";
            BatchMRP.ReadOnly = true;
            BatchMRP.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column1
            // 
            Column1.HeaderText = "Id";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Visible = false;
            // 
            // BtnSearchCancel
            // 
            BtnSearchCancel.DialogResult = DialogResult.Cancel;
            BtnSearchCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSearchCancel.Location = new Point(491, 217);
            BtnSearchCancel.Name = "BtnSearchCancel";
            BtnSearchCancel.Size = new Size(88, 23);
            BtnSearchCancel.TabIndex = 2;
            BtnSearchCancel.Text = "Cancel [Esc]";
            BtnSearchCancel.UseVisualStyleBackColor = true;
            // 
            // toolStrip
            // 
            toolStrip.BackColor = SystemColors.ControlLight;
            toolStrip.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStrip.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip.Items.AddRange(new ToolStripItem[] { toolStripLabel1, TextBoxSearchBatch, BtnSearchBatch });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Padding = new Padding(5);
            toolStrip.Size = new Size(682, 31);
            toolStrip.TabIndex = 34;
            toolStrip.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(59, 18);
            toolStripLabel1.Text = "Search For";
            // 
            // TextBoxSearchBatch
            // 
            TextBoxSearchBatch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxSearchBatch.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxSearchBatch.HideSelection = false;
            TextBoxSearchBatch.MaxLength = 10;
            TextBoxSearchBatch.Name = "TextBoxSearchBatch";
            TextBoxSearchBatch.Size = new Size(250, 21);
            TextBoxSearchBatch.KeyDown += TextBoxSearchBatch_KeyDown;
            TextBoxSearchBatch.TextChanged += TextBoxSearchBatch_TextChanged;
            // 
            // BtnSearchBatch
            // 
            BtnSearchBatch.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnSearchBatch.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSearchBatch.Image = (Image)resources.GetObject("BtnSearchBatch.Image");
            BtnSearchBatch.ImageTransparentColor = Color.Magenta;
            BtnSearchBatch.Name = "BtnSearchBatch";
            BtnSearchBatch.Size = new Size(26, 18);
            BtnSearchBatch.Text = "Go";
            BtnSearchBatch.Click += BtnSearchBatch_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { BatchSearchErrorMsg });
            statusStrip1.Location = new Point(0, 249);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(682, 22);
            statusStrip1.TabIndex = 35;
            statusStrip1.Text = "statusStrip1";
            // 
            // BatchSearchErrorMsg
            // 
            BatchSearchErrorMsg.Name = "BatchSearchErrorMsg";
            BatchSearchErrorMsg.Size = new Size(46, 17);
            BatchSearchErrorMsg.Text = "             ";
            // 
            // FormSearchBatch
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(682, 271);
            Controls.Add(statusStrip1);
            Controls.Add(toolStrip);
            Controls.Add(BtnSearchSelect);
            Controls.Add(GridViewBatch);
            Controls.Add(BtnSearchCancel);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSearchBatch";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Search Item Batch";
            Load += FormSearchBatch_Load;
            ((System.ComponentModel.ISupportInitialize)GridViewBatch).EndInit();
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtnSearchSelect;
        private controls.DataViewVerticalScroll GridViewBatch;
        private Button BtnSearchCancel;
        private ToolStrip toolStrip;
        private ToolStripLabel toolStripLabel1;
        private ToolStripTextBox TextBoxSearchBatch;
        private ToolStripButton BtnSearchBatch;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel BatchSearchErrorMsg;
        private DataGridViewTextBoxColumn PatientName;
        private DataGridViewTextBoxColumn PatientAddress;
        private controls.grid.DataGridViewQuantityColumn Column2;
        private DataGridViewTextBoxColumn uom;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn BatchMRP;
        private DataGridViewTextBoxColumn Column1;
    }
}