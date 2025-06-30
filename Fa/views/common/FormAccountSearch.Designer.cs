namespace fa.views.common
{
    partial class FormAccountSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAccountSearch));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            toolStrip = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            TextBoxSearchAccount = new controls.ToolstripDelayedTextBox();
            BtnSearch = new ToolStripButton();
            BtnAccountSearchNew = new ToolStripDropDownButton();
            accountToolStripMenuItem = new ToolStripMenuItem();
            supplierToolStripMenuItem = new ToolStripMenuItem();
            customerToolStripMenuItem = new ToolStripMenuItem();
            employeeToolStripMenuItem = new ToolStripMenuItem();
            GridViewAccountSearch = new controls.DataViewVerticalScroll();
            BtnCancel = new Button();
            BtnSelect = new Button();
            statusStrip1 = new StatusStrip();
            SearchErrorMsg = new ToolStripStatusLabel();
            PatientName = new DataGridViewTextBoxColumn();
            Type = new DataGridViewTextBoxColumn();
            PatientAddress = new DataGridViewTextBoxColumn();
            PatientDateOfBirth = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            patient = new DataGridViewCheckBoxColumn();
            toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewAccountSearch).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip
            // 
            toolStrip.BackColor = SystemColors.ControlLight;
            toolStrip.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStrip.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip.Items.AddRange(new ToolStripItem[] { toolStripLabel1, TextBoxSearchAccount, BtnSearch, BtnAccountSearchNew });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Padding = new Padding(5);
            toolStrip.Size = new Size(779, 34);
            toolStrip.TabIndex = 35;
            toolStrip.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(59, 21);
            toolStripLabel1.Text = "Search For";
            // 
            // TextBoxSearchAccount
            // 
            TextBoxSearchAccount.AutoSize = false;
            TextBoxSearchAccount.Delay = true;
            TextBoxSearchAccount.DelayTime = 1000;
            TextBoxSearchAccount.Name = "TextBoxSearchAccount";
            TextBoxSearchAccount.Size = new Size(350, 21);
            TextBoxSearchAccount.KeyDown += TextBoxSearchAccount_KeyDown;
            TextBoxSearchAccount.TextChanged += TextBoxSearchAccount_TextChanged;
            // 
            // BtnSearch
            // 
            BtnSearch.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnSearch.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSearch.Image = (Image)resources.GetObject("BtnSearch.Image");
            BtnSearch.ImageTransparentColor = Color.Magenta;
            BtnSearch.Name = "BtnSearch";
            BtnSearch.Size = new Size(26, 21);
            BtnSearch.Text = "Go";
            BtnSearch.Click += BtnSearchAccount_Click;
            // 
            // BtnAccountSearchNew
            // 
            BtnAccountSearchNew.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnAccountSearchNew.DropDownItems.AddRange(new ToolStripItem[] { accountToolStripMenuItem, supplierToolStripMenuItem, customerToolStripMenuItem, employeeToolStripMenuItem });
            BtnAccountSearchNew.Image = (Image)resources.GetObject("BtnAccountSearchNew.Image");
            BtnAccountSearchNew.ImageTransparentColor = Color.Magenta;
            BtnAccountSearchNew.Name = "BtnAccountSearchNew";
            BtnAccountSearchNew.Size = new Size(41, 21);
            BtnAccountSearchNew.Text = "New";
            BtnAccountSearchNew.ToolTipText = "New accounts";
            // 
            // accountToolStripMenuItem
            // 
            accountToolStripMenuItem.Image = (Image)resources.GetObject("accountToolStripMenuItem.Image");
            accountToolStripMenuItem.Name = "accountToolStripMenuItem";
            accountToolStripMenuItem.ShortcutKeys = Keys.F3;
            accountToolStripMenuItem.Size = new Size(139, 22);
            accountToolStripMenuItem.Text = "Account";
            accountToolStripMenuItem.Click += accountToolStripMenuItem_Click;
            // 
            // supplierToolStripMenuItem
            // 
            supplierToolStripMenuItem.Image = (Image)resources.GetObject("supplierToolStripMenuItem.Image");
            supplierToolStripMenuItem.Name = "supplierToolStripMenuItem";
            supplierToolStripMenuItem.ShortcutKeys = Keys.F4;
            supplierToolStripMenuItem.Size = new Size(139, 22);
            supplierToolStripMenuItem.Text = "Supplier";
            supplierToolStripMenuItem.Click += supplierToolStripMenuItem_Click;
            // 
            // customerToolStripMenuItem
            // 
            customerToolStripMenuItem.Image = (Image)resources.GetObject("customerToolStripMenuItem.Image");
            customerToolStripMenuItem.Name = "customerToolStripMenuItem";
            customerToolStripMenuItem.ShortcutKeys = Keys.F5;
            customerToolStripMenuItem.Size = new Size(139, 22);
            customerToolStripMenuItem.Text = "Customer";
            customerToolStripMenuItem.Click += customerToolStripMenuItem_Click;
            // 
            // employeeToolStripMenuItem
            // 
            employeeToolStripMenuItem.Image = (Image)resources.GetObject("employeeToolStripMenuItem.Image");
            employeeToolStripMenuItem.Name = "employeeToolStripMenuItem";
            employeeToolStripMenuItem.ShortcutKeys = Keys.F6;
            employeeToolStripMenuItem.Size = new Size(139, 22);
            employeeToolStripMenuItem.Text = "Employee";
            employeeToolStripMenuItem.Click += employeeToolStripMenuItem_Click;
            // 
            // GridViewAccountSearch
            // 
            GridViewAccountSearch.AllowUserToAddRows = false;
            GridViewAccountSearch.AllowUserToDeleteRows = false;
            GridViewAccountSearch.AllowUserToResizeColumns = false;
            GridViewAccountSearch.AllowUserToResizeRows = false;
            GridViewAccountSearch.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            GridViewAccountSearch.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewAccountSearch.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewAccountSearch.ColumnHeadersHeight = 20;
            GridViewAccountSearch.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewAccountSearch.Columns.AddRange(new DataGridViewColumn[] { PatientName, Type, PatientAddress, PatientDateOfBirth, Column1, patient });
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = SystemColors.Window;
            dataGridViewCellStyle6.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle6.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            GridViewAccountSearch.DefaultCellStyle = dataGridViewCellStyle6;
            GridViewAccountSearch.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewAccountSearch.EnableHeadersVisualStyles = false;
            GridViewAccountSearch.Location = new Point(3, 31);
            GridViewAccountSearch.MultiSelect = false;
            GridViewAccountSearch.Name = "GridViewAccountSearch";
            GridViewAccountSearch.ReadOnly = true;
            GridViewAccountSearch.RowHeadersVisible = false;
            dataGridViewCellStyle7.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            GridViewAccountSearch.RowsDefaultCellStyle = dataGridViewCellStyle7;
            GridViewAccountSearch.RowTemplate.Height = 20;
            GridViewAccountSearch.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewAccountSearch.ShowCellToolTips = false;
            GridViewAccountSearch.Size = new Size(773, 274);
            GridViewAccountSearch.TabIndex = 37;
            GridViewAccountSearch.CellContentClick += GridViewAccountSearch_CellContentClick;
            GridViewAccountSearch.CellDoubleClick += GridViewAccountSearch_CellDoubleClick;
            GridViewAccountSearch.Enter += GridViewAccountSearch_Enter;
            GridViewAccountSearch.KeyDown += GridViewAccountSearch_KeyDown;
            GridViewAccountSearch.Leave += GridViewAccountSearch_Leave;
            // 
            // BtnCancel
            // 
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(515, 313);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(88, 23);
            BtnCancel.TabIndex = 40;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // BtnSelect
            // 
            BtnSelect.Enabled = false;
            BtnSelect.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSelect.Location = new Point(609, 313);
            BtnSelect.Name = "BtnSelect";
            BtnSelect.Size = new Size(88, 23);
            BtnSelect.TabIndex = 39;
            BtnSelect.Text = "Select [F8]";
            BtnSelect.UseVisualStyleBackColor = true;
            BtnSelect.Click += BtnSelect_Click;
            BtnSelect.PreviewKeyDown += BtnSelect_PreviewKeyDown;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { SearchErrorMsg });
            statusStrip1.Location = new Point(0, 347);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(779, 22);
            statusStrip1.TabIndex = 41;
            statusStrip1.Text = "statusStrip1";
            // 
            // SearchErrorMsg
            // 
            SearchErrorMsg.Name = "SearchErrorMsg";
            SearchErrorMsg.Size = new Size(34, 17);
            SearchErrorMsg.Text = "         ";
            // 
            // PatientName
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            PatientName.DefaultCellStyle = dataGridViewCellStyle2;
            PatientName.HeaderText = "Name";
            PatientName.Name = "PatientName";
            PatientName.ReadOnly = true;
            PatientName.Resizable = DataGridViewTriState.False;
            PatientName.SortMode = DataGridViewColumnSortMode.NotSortable;
            PatientName.Width = 200;
            // 
            // Type
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            Type.DefaultCellStyle = dataGridViewCellStyle3;
            Type.HeaderText = "Type";
            Type.Name = "Type";
            Type.ReadOnly = true;
            Type.Resizable = DataGridViewTriState.False;
            Type.SortMode = DataGridViewColumnSortMode.NotSortable;
            Type.Width = 150;
            // 
            // PatientAddress
            // 
            PatientAddress.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            PatientAddress.DefaultCellStyle = dataGridViewCellStyle4;
            PatientAddress.HeaderText = "Address";
            PatientAddress.Name = "PatientAddress";
            PatientAddress.ReadOnly = true;
            PatientAddress.Resizable = DataGridViewTriState.False;
            PatientAddress.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // PatientDateOfBirth
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.TopLeft;
            PatientDateOfBirth.DefaultCellStyle = dataGridViewCellStyle5;
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
            // patient
            // 
            patient.HeaderText = "isPatient";
            patient.Name = "patient";
            patient.ReadOnly = true;
            patient.Visible = false;
            // 
            // FormAccountSearch
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(779, 369);
            Controls.Add(statusStrip1);
            Controls.Add(BtnCancel);
            Controls.Add(BtnSelect);
            Controls.Add(GridViewAccountSearch);
            Controls.Add(toolStrip);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormAccountSearch";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Account Search";
            FormClosing += FormAccountSearch_FormClosing;
            Load += FormAccountSearch_Load;
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewAccountSearch).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton BtnSearch;
        private System.Windows.Forms.ToolStripDropDownButton BtnAccountSearchNew;
        private System.Windows.Forms.ToolStripMenuItem accountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem supplierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customerToolStripMenuItem;
        private controls.DataViewVerticalScroll GridViewAccountSearch;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnSelect;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel SearchErrorMsg;
        private System.Windows.Forms.ToolStripMenuItem employeeToolStripMenuItem;
        private controls.ToolstripDelayedTextBox TextBoxSearchAccount;
        private DataGridViewTextBoxColumn PatientName;
        private DataGridViewTextBoxColumn Type;
        private DataGridViewTextBoxColumn PatientAddress;
        private DataGridViewTextBoxColumn PatientDateOfBirth;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewCheckBoxColumn patient;
    }
}