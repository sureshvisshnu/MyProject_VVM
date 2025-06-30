namespace fa.views.hms.Masters
{
    partial class FormConsultations
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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConsultations));
            label1 = new Label();
            label2 = new Label();
            TextBoxConsultationName = new controls.text.NameTextBoxAllowSpace(components);
            TextBoxConsultationDescription = new controls.text.NameTextBoxAllowSpace(components);
            BtnConsultationExit = new Button();
            BtnConsultationCancel = new Button();
            BtnConsultationSave = new Button();
            TextBoxConsultationId = new MaskedTextBox();
            TreeViewConsultation = new TreeView();
            BtnConsultationDelete = new Button();
            BtnConsultationEdit = new Button();
            BtnConsultationNew = new Button();
            statusStrip1 = new StatusStrip();
            ErrorMsgConsultation = new ToolStripStatusLabel();
            TabControlConsultation = new TabControl();
            TabConsultationTypeDetail = new TabPage();
            GridViewServiceProvider = new controls.DataViewVerticalScroll();
            Sno = new DataGridViewTextBoxColumn();
            ConsultingDoctors = new DataGridViewTextBoxColumn();
            ConsultingFees = new controls.grid.DataGridViewCurrencyColumn();
            EmployeeId = new DataGridViewTextBoxColumn();
            label4 = new Label();
            TextBoxConsultationDisplayAs = new controls.text.NameTextBoxAllowSpace(components);
            BtnImport = new Button();
            BtnExport = new Button();
            TextBoxConsultationSearch = new controls.text.DelayedTextChangeTextBox();
            statusStrip1.SuspendLayout();
            TabControlConsultation.SuspendLayout();
            TabConsultationTypeDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewServiceProvider).BeginInit();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(158, 215);
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(158, 189);
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(158, 163);
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(15, 12);
            label1.Name = "label1";
            label1.Size = new Size(39, 13);
            label1.TabIndex = 0;
            label1.Text = "Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(15, 99);
            label2.Name = "label2";
            label2.Size = new Size(60, 13);
            label2.TabIndex = 1;
            label2.Text = "Description";
            // 
            // TextBoxConsultationName
            // 
            TextBoxConsultationName.BackColor = Color.White;
            TextBoxConsultationName.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxConsultationName.Location = new Point(18, 30);
            TextBoxConsultationName.MaxLength = 30;
            TextBoxConsultationName.Name = "TextBoxConsultationName";
            TextBoxConsultationName.Size = new Size(372, 21);
            TextBoxConsultationName.TabIndex = 5;
            TextBoxConsultationName.PreviewKeyDown += TextBoxConsultationName_PreviewKeyDown;
            // 
            // TextBoxConsultationDescription
            // 
            TextBoxConsultationDescription.BackColor = Color.White;
            TextBoxConsultationDescription.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxConsultationDescription.Location = new Point(18, 117);
            TextBoxConsultationDescription.MaxLength = 250;
            TextBoxConsultationDescription.Multiline = true;
            TextBoxConsultationDescription.Name = "TextBoxConsultationDescription";
            TextBoxConsultationDescription.Size = new Size(478, 72);
            TextBoxConsultationDescription.TabIndex = 8;
            // 
            // BtnConsultationExit
            // 
            BtnConsultationExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnConsultationExit.Location = new Point(736, 477);
            BtnConsultationExit.Name = "BtnConsultationExit";
            BtnConsultationExit.Size = new Size(83, 24);
            BtnConsultationExit.TabIndex = 11;
            BtnConsultationExit.Text = "Exit [F10]";
            BtnConsultationExit.UseVisualStyleBackColor = true;
            BtnConsultationExit.Click += BtnConsultationExit_Click;
            // 
            // BtnConsultationCancel
            // 
            BtnConsultationCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnConsultationCancel.Location = new Point(558, 477);
            BtnConsultationCancel.Name = "BtnConsultationCancel";
            BtnConsultationCancel.Size = new Size(83, 24);
            BtnConsultationCancel.TabIndex = 10;
            BtnConsultationCancel.Text = "Cancel [Esc]";
            BtnConsultationCancel.UseVisualStyleBackColor = true;
            BtnConsultationCancel.Click += BtnConsultationCancel_Click;
            // 
            // BtnConsultationSave
            // 
            BtnConsultationSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnConsultationSave.Location = new Point(647, 477);
            BtnConsultationSave.Name = "BtnConsultationSave";
            BtnConsultationSave.Size = new Size(83, 24);
            BtnConsultationSave.TabIndex = 9;
            BtnConsultationSave.Text = "Save [F8]";
            BtnConsultationSave.UseVisualStyleBackColor = true;
            BtnConsultationSave.Click += BtnConsultationSave_Click;
            BtnConsultationSave.PreviewKeyDown += BtnConsultationSave_PreviewKeyDown;
            // 
            // TextBoxConsultationId
            // 
            TextBoxConsultationId.Location = new Point(381, 480);
            TextBoxConsultationId.Name = "TextBoxConsultationId";
            TextBoxConsultationId.Size = new Size(100, 21);
            TextBoxConsultationId.TabIndex = 31;
            TextBoxConsultationId.Visible = false;
            // 
            // TreeViewConsultation
            // 
            TreeViewConsultation.Enabled = false;
            TreeViewConsultation.HideSelection = false;
            TreeViewConsultation.Location = new Point(19, 46);
            TreeViewConsultation.Name = "TreeViewConsultation";
            TreeViewConsultation.Size = new Size(279, 417);
            TreeViewConsultation.TabIndex = 1;
            TreeViewConsultation.AfterSelect += TreeViewConsultation_AfterSelect;
            // 
            // BtnConsultationDelete
            // 
            BtnConsultationDelete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnConsultationDelete.Location = new Point(110, 477);
            BtnConsultationDelete.Name = "BtnConsultationDelete";
            BtnConsultationDelete.Size = new Size(83, 24);
            BtnConsultationDelete.TabIndex = 3;
            BtnConsultationDelete.Text = "Delete [F4]";
            BtnConsultationDelete.UseVisualStyleBackColor = true;
            BtnConsultationDelete.Click += BtnConsultationDelete_Click;
            // 
            // BtnConsultationEdit
            // 
            BtnConsultationEdit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnConsultationEdit.Location = new Point(199, 477);
            BtnConsultationEdit.Name = "BtnConsultationEdit";
            BtnConsultationEdit.Size = new Size(83, 24);
            BtnConsultationEdit.TabIndex = 4;
            BtnConsultationEdit.Text = "Edit [F7]";
            BtnConsultationEdit.UseVisualStyleBackColor = true;
            BtnConsultationEdit.Click += BtnConsultationEdit_Click;
            // 
            // BtnConsultationNew
            // 
            BtnConsultationNew.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnConsultationNew.Location = new Point(21, 477);
            BtnConsultationNew.Name = "BtnConsultationNew";
            BtnConsultationNew.Size = new Size(83, 24);
            BtnConsultationNew.TabIndex = 2;
            BtnConsultationNew.Text = "New [F3]";
            BtnConsultationNew.UseVisualStyleBackColor = true;
            BtnConsultationNew.Click += BtnConsultationNew_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsgConsultation });
            statusStrip1.Location = new Point(0, 514);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(847, 22);
            statusStrip1.TabIndex = 34;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsgConsultation
            // 
            ErrorMsgConsultation.Name = "ErrorMsgConsultation";
            ErrorMsgConsultation.Size = new Size(25, 17);
            ErrorMsgConsultation.Text = "      ";
            // 
            // TabControlConsultation
            // 
            TabControlConsultation.Controls.Add(TabConsultationTypeDetail);
            TabControlConsultation.Location = new Point(304, 19);
            TabControlConsultation.Name = "TabControlConsultation";
            TabControlConsultation.SelectedIndex = 0;
            TabControlConsultation.Size = new Size(528, 444);
            TabControlConsultation.TabIndex = 5;
            // 
            // TabConsultationTypeDetail
            // 
            TabConsultationTypeDetail.Controls.Add(GridViewServiceProvider);
            TabConsultationTypeDetail.Controls.Add(label4);
            TabConsultationTypeDetail.Controls.Add(TextBoxConsultationDisplayAs);
            TabConsultationTypeDetail.Controls.Add(label1);
            TabConsultationTypeDetail.Controls.Add(label2);
            TabConsultationTypeDetail.Controls.Add(TextBoxConsultationName);
            TabConsultationTypeDetail.Controls.Add(TextBoxConsultationDescription);
            TabConsultationTypeDetail.Location = new Point(4, 22);
            TabConsultationTypeDetail.Name = "TabConsultationTypeDetail";
            TabConsultationTypeDetail.Padding = new Padding(3);
            TabConsultationTypeDetail.Size = new Size(520, 418);
            TabConsultationTypeDetail.TabIndex = 0;
            TabConsultationTypeDetail.Text = "Details";
            TabConsultationTypeDetail.UseVisualStyleBackColor = true;
            // 
            // GridViewServiceProvider
            // 
            GridViewServiceProvider.AllowUserToAddRows = false;
            GridViewServiceProvider.AllowUserToResizeColumns = false;
            GridViewServiceProvider.AllowUserToResizeRows = false;
            GridViewServiceProvider.BackgroundColor = SystemColors.ControlLight;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            GridViewServiceProvider.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewServiceProvider.ColumnHeadersHeight = 20;
            GridViewServiceProvider.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewServiceProvider.Columns.AddRange(new DataGridViewColumn[] { Sno, ConsultingDoctors, ConsultingFees, EmployeeId });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            GridViewServiceProvider.DefaultCellStyle = dataGridViewCellStyle3;
            GridViewServiceProvider.EnableHeadersVisualStyles = false;
            GridViewServiceProvider.Location = new Point(18, 199);
            GridViewServiceProvider.MultiSelect = false;
            GridViewServiceProvider.Name = "GridViewServiceProvider";
            GridViewServiceProvider.RowHeadersVisible = false;
            GridViewServiceProvider.RowTemplate.Height = 20;
            GridViewServiceProvider.ScrollBars = ScrollBars.Vertical;
            GridViewServiceProvider.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewServiceProvider.ShowCellToolTips = false;
            GridViewServiceProvider.Size = new Size(478, 207);
            GridViewServiceProvider.TabIndex = 9;
            GridViewServiceProvider.CellEnter += GridViewServiceProvider_CellEnter;
            GridViewServiceProvider.EditingControlShowing += GridViewServiceProvider_EditingControlShowing;
            // 
            // Sno
            // 
            Sno.HeaderText = "Sno";
            Sno.Name = "Sno";
            Sno.ReadOnly = true;
            Sno.Resizable = DataGridViewTriState.False;
            Sno.SortMode = DataGridViewColumnSortMode.NotSortable;
            Sno.Width = 40;
            // 
            // ConsultingDoctors
            // 
            ConsultingDoctors.HeaderText = "Service Provider's Name";
            ConsultingDoctors.Name = "ConsultingDoctors";
            ConsultingDoctors.ReadOnly = true;
            ConsultingDoctors.Resizable = DataGridViewTriState.False;
            ConsultingDoctors.SortMode = DataGridViewColumnSortMode.NotSortable;
            ConsultingDoctors.Width = 310;
            // 
            // ConsultingFees
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopRight;
            ConsultingFees.DefaultCellStyle = dataGridViewCellStyle2;
            ConsultingFees.HeaderText = "Fees";
            ConsultingFees.Name = "ConsultingFees";
            ConsultingFees.Width = 110;
            // 
            // EmployeeId
            // 
            EmployeeId.HeaderText = "EmployeeId";
            EmployeeId.Name = "EmployeeId";
            EmployeeId.ReadOnly = true;
            EmployeeId.Resizable = DataGridViewTriState.False;
            EmployeeId.SortMode = DataGridViewColumnSortMode.NotSortable;
            EmployeeId.Visible = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(15, 56);
            label4.Name = "label4";
            label4.Size = new Size(56, 13);
            label4.TabIndex = 6;
            label4.Text = "Display As";
            // 
            // TextBoxConsultationDisplayAs
            // 
            TextBoxConsultationDisplayAs.BackColor = Color.White;
            TextBoxConsultationDisplayAs.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxConsultationDisplayAs.Location = new Point(18, 74);
            TextBoxConsultationDisplayAs.MaxLength = 30;
            TextBoxConsultationDisplayAs.Name = "TextBoxConsultationDisplayAs";
            TextBoxConsultationDisplayAs.Size = new Size(372, 21);
            TextBoxConsultationDisplayAs.TabIndex = 6;
            // 
            // BtnImport
            // 
            BtnImport.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnImport.Location = new Point(381, 478);
            BtnImport.Name = "BtnImport";
            BtnImport.Size = new Size(83, 23);
            BtnImport.TabIndex = 40;
            BtnImport.Text = "Import";
            BtnImport.UseVisualStyleBackColor = true;
            BtnImport.Click += BtnImport_Click;
            // 
            // BtnExport
            // 
            BtnExport.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExport.Location = new Point(470, 478);
            BtnExport.Name = "BtnExport";
            BtnExport.Size = new Size(83, 23);
            BtnExport.TabIndex = 39;
            BtnExport.Text = "Export";
            BtnExport.UseVisualStyleBackColor = true;
            BtnExport.Click += BtnExport_Click;
            // 
            // TextBoxConsultationSearch
            // 
            TextBoxConsultationSearch.BackColor = SystemColors.Window;
            TextBoxConsultationSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxConsultationSearch.Delay = true;
            TextBoxConsultationSearch.DelayTime = 1000;
            TextBoxConsultationSearch.Location = new Point(19, 19);
            TextBoxConsultationSearch.MaxLength = 35;
            TextBoxConsultationSearch.Name = "TextBoxConsultationSearch";
            TextBoxConsultationSearch.Searchstartfrom = 2;
            TextBoxConsultationSearch.Size = new Size(279, 21);
            TextBoxConsultationSearch.TabIndex = 0;
            TextBoxConsultationSearch.TextChanged += TextBoxConsultationSearch_TextChanged;
            TextBoxConsultationSearch.KeyDown += TextBoxConsultationSearch_KeyDown;
            // 
            // FormConsultations
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(847, 536);
            Controls.Add(TextBoxConsultationSearch);
            Controls.Add(BtnImport);
            Controls.Add(BtnExport);
            Controls.Add(TabControlConsultation);
            Controls.Add(statusStrip1);
            Controls.Add(BtnConsultationExit);
            Controls.Add(BtnConsultationCancel);
            Controls.Add(BtnConsultationSave);
            Controls.Add(TextBoxConsultationId);
            Controls.Add(TreeViewConsultation);
            Controls.Add(BtnConsultationDelete);
            Controls.Add(BtnConsultationEdit);
            Controls.Add(BtnConsultationNew);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormConsultations";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Consultations Details & Fees";
            FormClosing += FormConsultations_FormClosing;
            Load += FormConsultationType_Load;
            Controls.SetChildIndex(BtnConsultationNew, 0);
            Controls.SetChildIndex(BtnConsultationEdit, 0);
            Controls.SetChildIndex(BtnConsultationDelete, 0);
            Controls.SetChildIndex(TreeViewConsultation, 0);
            Controls.SetChildIndex(TextBoxConsultationId, 0);
            Controls.SetChildIndex(BtnConsultationSave, 0);
            Controls.SetChildIndex(BtnConsultationCancel, 0);
            Controls.SetChildIndex(BtnConsultationExit, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(TabControlConsultation, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(BtnExport, 0);
            Controls.SetChildIndex(BtnImport, 0);
            Controls.SetChildIndex(TextBoxConsultationSearch, 0);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            TabControlConsultation.ResumeLayout(false);
            TabConsultationTypeDetail.ResumeLayout(false);
            TabConsultationTypeDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewServiceProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private controls.text.NameTextBoxAllowSpace TextBoxConsultationName;
        private controls.text.NameTextBoxAllowSpace TextBoxConsultationDescription;
        private System.Windows.Forms.Button BtnConsultationExit;
        private System.Windows.Forms.Button BtnConsultationCancel;
        private System.Windows.Forms.Button BtnConsultationSave;
        private System.Windows.Forms.MaskedTextBox TextBoxConsultationId;
        private System.Windows.Forms.TreeView TreeViewConsultation;
        private System.Windows.Forms.Button BtnConsultationDelete;
        private System.Windows.Forms.Button BtnConsultationEdit;
        private System.Windows.Forms.Button BtnConsultationNew;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TabControl TabControlConsultation;
        private System.Windows.Forms.TabPage TabConsultationTypeDetail;
        private System.Windows.Forms.ToolStripStatusLabel ErrorMsgConsultation;
        private System.Windows.Forms.Label label4;
        private controls.text.NameTextBoxAllowSpace TextBoxConsultationDisplayAs;
        private System.Windows.Forms.Button BtnImport;
        private System.Windows.Forms.Button BtnExport;
        private controls.text.DelayedTextChangeTextBox TextBoxConsultationSearch;
        private controls.DataViewVerticalScroll GridViewServiceProvider;
        private DataGridViewTextBoxColumn Sno;
        private DataGridViewTextBoxColumn ConsultingDoctors;
        private controls.grid.DataGridViewCurrencyColumn ConsultingFees;
        private DataGridViewTextBoxColumn EmployeeId;
    }
}