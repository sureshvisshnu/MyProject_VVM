namespace fa.views.account.masters
{
    partial class FormSearchSupplier
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSearchSupplier));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            toolStrip = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            TextBoxSearchSupplier = new ToolStripTextBox();
            BtnSearchSupplier = new ToolStripButton();
            GridViewSupplierSearch = new controls.DataViewVerticalScroll();
            PatientName = new DataGridViewTextBoxColumn();
            PatientAddress = new DataGridViewTextBoxColumn();
            PatientDateOfBirth = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            BtnCancel = new Button();
            statusStrip1 = new StatusStrip();
            SearchErrorMsg = new ToolStripStatusLabel();
            BtnSelect = new Button();
            BtnNewSupplier = new Button();
            toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewSupplierSearch).BeginInit();
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
            // toolStrip
            // 
            toolStrip.BackColor = SystemColors.ControlLight;
            toolStrip.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStrip.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip.Items.AddRange(new ToolStripItem[] { toolStripLabel1, TextBoxSearchSupplier, BtnSearchSupplier });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Padding = new Padding(5);
            toolStrip.Size = new Size(631, 31);
            toolStrip.TabIndex = 1;
            toolStrip.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(59, 18);
            toolStripLabel1.Text = "Search For";
            // 
            // TextBoxSearchSupplier
            // 
            TextBoxSearchSupplier.BorderStyle = BorderStyle.FixedSingle;
            TextBoxSearchSupplier.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxSearchSupplier.HideSelection = false;
            TextBoxSearchSupplier.MaxLength = 63;
            TextBoxSearchSupplier.Name = "TextBoxSearchSupplier";
            TextBoxSearchSupplier.Size = new Size(350, 21);
            TextBoxSearchSupplier.KeyDown += TextBoxSearchSupplier_KeyDown;
            TextBoxSearchSupplier.TextChanged += TextBoxSearchSupplier_TextChanged;
            // 
            // BtnSearchSupplier
            // 
            BtnSearchSupplier.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnSearchSupplier.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSearchSupplier.Image = (Image)resources.GetObject("BtnSearchSupplier.Image");
            BtnSearchSupplier.ImageTransparentColor = Color.Magenta;
            BtnSearchSupplier.Name = "BtnSearchSupplier";
            BtnSearchSupplier.Size = new Size(26, 18);
            BtnSearchSupplier.Text = "Go";
            BtnSearchSupplier.Click += BtnSearchSupplier_Click;
            // 
            // GridViewSupplierSearch
            // 
            GridViewSupplierSearch.AllowUserToAddRows = false;
            GridViewSupplierSearch.AllowUserToDeleteRows = false;
            GridViewSupplierSearch.AllowUserToResizeColumns = false;
            GridViewSupplierSearch.AllowUserToResizeRows = false;
            GridViewSupplierSearch.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            GridViewSupplierSearch.BackgroundColor = SystemColors.Control;
            GridViewSupplierSearch.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewSupplierSearch.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewSupplierSearch.ColumnHeadersHeight = 20;
            GridViewSupplierSearch.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewSupplierSearch.Columns.AddRange(new DataGridViewColumn[] { PatientName, PatientAddress, PatientDateOfBirth, Column1 });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            GridViewSupplierSearch.DefaultCellStyle = dataGridViewCellStyle3;
            GridViewSupplierSearch.EditMode = DataGridViewEditMode.EditProgrammatically;
            GridViewSupplierSearch.EnableHeadersVisualStyles = false;
            GridViewSupplierSearch.Location = new Point(5, 34);
            GridViewSupplierSearch.MultiSelect = false;
            GridViewSupplierSearch.Name = "GridViewSupplierSearch";
            GridViewSupplierSearch.ReadOnly = true;
            GridViewSupplierSearch.RowHeadersVisible = false;
            GridViewSupplierSearch.RowTemplate.Height = 20;
            GridViewSupplierSearch.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewSupplierSearch.ShowCellToolTips = false;
            GridViewSupplierSearch.ShowEditingIcon = false;
            GridViewSupplierSearch.Size = new Size(621, 232);
            GridViewSupplierSearch.TabIndex = 2;
            GridViewSupplierSearch.CellDoubleClick += GridViewSupplierSearch_CellDoubleClick;
            GridViewSupplierSearch.Enter += GridViewSupplierSearch_Enter;
            GridViewSupplierSearch.KeyDown += GridViewSupplierSearch_KeyDown;
            GridViewSupplierSearch.Leave += GridViewSupplierSearch_Leave;
            // 
            // PatientName
            // 
            PatientName.HeaderText = "Name";
            PatientName.Name = "PatientName";
            PatientName.ReadOnly = true;
            PatientName.Resizable = DataGridViewTriState.False;
            PatientName.SortMode = DataGridViewColumnSortMode.NotSortable;
            PatientName.Width = 200;
            // 
            // PatientAddress
            // 
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            PatientAddress.DefaultCellStyle = dataGridViewCellStyle2;
            PatientAddress.HeaderText = "Address";
            PatientAddress.Name = "PatientAddress";
            PatientAddress.ReadOnly = true;
            PatientAddress.Resizable = DataGridViewTriState.False;
            PatientAddress.SortMode = DataGridViewColumnSortMode.NotSortable;
            PatientAddress.Width = 275;
            // 
            // PatientDateOfBirth
            // 
            PatientDateOfBirth.HeaderText = "Phone No";
            PatientDateOfBirth.Name = "PatientDateOfBirth";
            PatientDateOfBirth.ReadOnly = true;
            PatientDateOfBirth.Resizable = DataGridViewTriState.False;
            PatientDateOfBirth.SortMode = DataGridViewColumnSortMode.NotSortable;
            PatientDateOfBirth.Width = 125;
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
            // BtnCancel
            // 
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(437, 272);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(88, 23);
            BtnCancel.TabIndex = 28;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.PreviewKeyDown += BtnSelect_PreviewKeyDown;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { SearchErrorMsg });
            statusStrip1.Location = new Point(0, 300);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(631, 22);
            statusStrip1.TabIndex = 29;
            statusStrip1.Text = "statusStrip1";
            // 
            // SearchErrorMsg
            // 
            SearchErrorMsg.Name = "SearchErrorMsg";
            SearchErrorMsg.Size = new Size(49, 17);
            SearchErrorMsg.Text = "              ";
            // 
            // BtnSelect
            // 
            BtnSelect.Enabled = false;
            BtnSelect.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSelect.Location = new Point(531, 272);
            BtnSelect.Name = "BtnSelect";
            BtnSelect.Size = new Size(88, 23);
            BtnSelect.TabIndex = 27;
            BtnSelect.Text = "Select [F8]";
            BtnSelect.UseVisualStyleBackColor = true;
            BtnSelect.Click += BtnSelect_Click;
            BtnSelect.PreviewKeyDown += BtnSelect_PreviewKeyDown;
            // 
            // BtnNewSupplier
            // 
            BtnNewSupplier.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnNewSupplier.Location = new Point(491, 4);
            BtnNewSupplier.Name = "BtnNewSupplier";
            BtnNewSupplier.Size = new Size(119, 23);
            BtnNewSupplier.TabIndex = 33;
            BtnNewSupplier.Text = "New Supplier [F3]";
            BtnNewSupplier.UseVisualStyleBackColor = true;
            BtnNewSupplier.Click += BtnNewSupplier_Click;
            // 
            // FormSearchSupplier
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnCancel;
            ClientSize = new Size(631, 322);
            Controls.Add(BtnNewSupplier);
            Controls.Add(BtnCancel);
            Controls.Add(statusStrip1);
            Controls.Add(BtnSelect);
            Controls.Add(GridViewSupplierSearch);
            Controls.Add(toolStrip);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSearchSupplier";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Search Supplier";
            Load += FormSearchSupplier_Load;
            Controls.SetChildIndex(toolStrip, 0);
            Controls.SetChildIndex(GridViewSupplierSearch, 0);
            Controls.SetChildIndex(BtnSelect, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(BtnCancel, 0);
            Controls.SetChildIndex(BtnNewSupplier, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewSupplierSearch).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox TextBoxSearchSupplier;
        private System.Windows.Forms.ToolStripButton BtnSearchSupplier;
        private controls.DataViewVerticalScroll GridViewSupplierSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn PatientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PatientAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn PatientDateOfBirth;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel SearchErrorMsg;
        private System.Windows.Forms.Button BtnSelect;
        private System.Windows.Forms.Button BtnNewSupplier;
    }
}