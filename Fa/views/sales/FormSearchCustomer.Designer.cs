namespace fa.views.sales
{
    partial class FormSearchCustomer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSearchCustomer));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            BtnNewCustomer = new Button();
            toolStrip = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            TextBoxSearchCustomer = new ToolStripTextBox();
            BtnSearchCustomer = new ToolStripButton();
            GridViewCustomerSearch = new controls.DataViewVerticalScroll();
            BtnCancel = new Button();
            BtnSelect = new Button();
            statusStrip1 = new StatusStrip();
            SearchErrorMsg = new ToolStripStatusLabel();
            PatientName = new DataGridViewTextBoxColumn();
            PatientAddress = new DataGridViewTextBoxColumn();
            PatientDateOfBirth = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewCustomerSearch).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // BtnNewCustomer
            // 
            BtnNewCustomer.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnNewCustomer.Location = new Point(486, 4);
            BtnNewCustomer.Name = "BtnNewCustomer";
            BtnNewCustomer.Size = new Size(132, 23);
            BtnNewCustomer.TabIndex = 35;
            BtnNewCustomer.Text = "New Customer [F3]";
            BtnNewCustomer.UseVisualStyleBackColor = true;
            BtnNewCustomer.Click += BtnNewCustomer_Click;
            // 
            // toolStrip
            // 
            toolStrip.BackColor = SystemColors.ControlLight;
            toolStrip.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStrip.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip.Items.AddRange(new ToolStripItem[] { toolStripLabel1, TextBoxSearchCustomer, BtnSearchCustomer });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Padding = new Padding(5);
            toolStrip.Size = new Size(630, 31);
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
            // TextBoxSearchCustomer
            // 
            TextBoxSearchCustomer.BorderStyle = BorderStyle.FixedSingle;
            TextBoxSearchCustomer.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxSearchCustomer.HideSelection = false;
            TextBoxSearchCustomer.MaxLength = 63;
            TextBoxSearchCustomer.Name = "TextBoxSearchCustomer";
            TextBoxSearchCustomer.Size = new Size(350, 21);
            TextBoxSearchCustomer.KeyDown += TextBoxSearchCustomer_KeyDown;
            TextBoxSearchCustomer.TextChanged += TextBoxSearchCustomer_TextChanged;
            // 
            // BtnSearchCustomer
            // 
            BtnSearchCustomer.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnSearchCustomer.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSearchCustomer.Image = (Image)resources.GetObject("BtnSearchCustomer.Image");
            BtnSearchCustomer.ImageTransparentColor = Color.Magenta;
            BtnSearchCustomer.Name = "BtnSearchCustomer";
            BtnSearchCustomer.Size = new Size(26, 18);
            BtnSearchCustomer.Text = "Go";
            BtnSearchCustomer.Click += BtnSearchCustomer_Click;
            // 
            // GridViewCustomerSearch
            // 
            GridViewCustomerSearch.AllowUserToAddRows = false;
            GridViewCustomerSearch.AllowUserToDeleteRows = false;
            GridViewCustomerSearch.AllowUserToResizeColumns = false;
            GridViewCustomerSearch.AllowUserToResizeRows = false;
            GridViewCustomerSearch.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            GridViewCustomerSearch.BackgroundColor = SystemColors.Control;
            GridViewCustomerSearch.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewCustomerSearch.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewCustomerSearch.ColumnHeadersHeight = 20;
            GridViewCustomerSearch.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewCustomerSearch.Columns.AddRange(new DataGridViewColumn[] { PatientName, PatientAddress, PatientDateOfBirth, Column1 });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            GridViewCustomerSearch.DefaultCellStyle = dataGridViewCellStyle3;
            GridViewCustomerSearch.EditMode = DataGridViewEditMode.EditProgrammatically;
            GridViewCustomerSearch.EnableHeadersVisualStyles = false;
            GridViewCustomerSearch.Location = new Point(4, 34);
            GridViewCustomerSearch.MultiSelect = false;
            GridViewCustomerSearch.Name = "GridViewCustomerSearch";
            GridViewCustomerSearch.ReadOnly = true;
            GridViewCustomerSearch.RowHeadersVisible = false;
            GridViewCustomerSearch.RowTemplate.Height = 20;
            GridViewCustomerSearch.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewCustomerSearch.ShowCellToolTips = false;
            GridViewCustomerSearch.ShowEditingIcon = false;
            GridViewCustomerSearch.Size = new Size(621, 232);
            GridViewCustomerSearch.TabIndex = 36;
            GridViewCustomerSearch.CellDoubleClick += GridViewCustomerSearch_CellDoubleClick;
            GridViewCustomerSearch.Enter += GridViewCustomerSearch_Enter;
            GridViewCustomerSearch.KeyDown += GridViewCustomerSearch_KeyDown;
            GridViewCustomerSearch.Leave += GridViewCustomerSearch_Leave;
            // 
            // BtnCancel
            // 
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(426, 272);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(88, 23);
            BtnCancel.TabIndex = 38;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            // 
            // BtnSelect
            // 
            BtnSelect.Enabled = false;
            BtnSelect.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSelect.Location = new Point(520, 272);
            BtnSelect.Name = "BtnSelect";
            BtnSelect.Size = new Size(88, 23);
            BtnSelect.TabIndex = 37;
            BtnSelect.Text = "Select [F8]";
            BtnSelect.UseVisualStyleBackColor = true;
            BtnSelect.Click += BtnSelect_Click;
            BtnSelect.PreviewKeyDown += BtnSelect_PreviewKeyDown;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { SearchErrorMsg });
            statusStrip1.Location = new Point(0, 301);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(630, 22);
            statusStrip1.TabIndex = 39;
            statusStrip1.Text = "statusStrip1";
            // 
            // SearchErrorMsg
            // 
            SearchErrorMsg.Name = "SearchErrorMsg";
            SearchErrorMsg.Size = new Size(49, 17);
            SearchErrorMsg.Text = "              ";
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
            PatientAddress.Width = 273;
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
            // FormSearchCustomer
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnCancel;
            ClientSize = new Size(630, 323);
            Controls.Add(statusStrip1);
            Controls.Add(BtnCancel);
            Controls.Add(BtnSelect);
            Controls.Add(GridViewCustomerSearch);
            Controls.Add(BtnNewCustomer);
            Controls.Add(toolStrip);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSearchCustomer";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Search Customer";
            Load += FormSearchCustomer_Load;
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewCustomerSearch).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button BtnNewCustomer;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox TextBoxSearchCustomer;
        private System.Windows.Forms.ToolStripButton BtnSearchCustomer;
        private controls.DataViewVerticalScroll GridViewCustomerSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn PatientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PatientAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn PatientDateOfBirth;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnSelect;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel SearchErrorMsg;
    }
}