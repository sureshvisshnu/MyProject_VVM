namespace fa.views.hms.patient
{
    partial class FormPatientProcedures
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
            controls.ComboTreeView.ComboTreeNode comboTreeNode1 = new controls.ComboTreeView.ComboTreeNode();
            controls.ComboTreeView.ComboTreeNode comboTreeNode2 = new controls.ComboTreeView.ComboTreeNode();
            controls.ComboTreeView.ComboTreeNode comboTreeNode3 = new controls.ComboTreeView.ComboTreeNode();
            controls.ComboTreeView.ComboTreeNode comboTreeNode4 = new controls.ComboTreeView.ComboTreeNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPatientProcedures));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            BtnExit = new Button();
            BtnUpdate = new Button();
            statusStrip1 = new StatusStrip();
            ProcedureErrorMsg = new ToolStripStatusLabel();
            ab2ToolStrip1 = new controls.Ab2ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            TextBoxName = new ToolStripTextBox();
            toolStripLabel3 = new ToolStripLabel();
            FromDate = new controls.ToolStripCalendar();
            toolStripLabel4 = new ToolStripLabel();
            ToDate = new controls.ToolStripCalendar();
            toolStripLabel2 = new ToolStripLabel();
            ComboBoxStatus = new controls.ToolStripComboTree();
            toolStripLabel5 = new ToolStripLabel();
            BtnGo = new ToolStripButton();
            GridViewProcedureInfo = new controls.DataViewVerticalScroll();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            ProcedureOrderedDate = new controls.grid.DataGridViewCalendarColumn();
            dataGridViewNameColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewCurrencyColumn5 = new DataGridViewTextBoxColumn();
            Column44 = new DataGridViewTextBoxColumn();
            Status = new DataGridViewTextBoxColumn();
            dataGridViewCurrencyColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewCurrencyColumn7 = new controls.grid.DataGridViewCalendarColumn();
            ProcedureNote = new DataGridViewTextBoxColumn();
            Fees = new controls.grid.DataGridViewCurrencyColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            statusStrip1.SuspendLayout();
            ab2ToolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewProcedureInfo).BeginInit();
            SuspendLayout();
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(1046, 567);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(75, 27);
            BtnExit.TabIndex = 21;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnUpdate
            // 
            BtnUpdate.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnUpdate.Location = new Point(951, 567);
            BtnUpdate.Name = "BtnUpdate";
            BtnUpdate.Size = new Size(89, 27);
            BtnUpdate.TabIndex = 22;
            BtnUpdate.Text = "Update [F3]";
            BtnUpdate.UseVisualStyleBackColor = true;
            BtnUpdate.Click += BtnUpdate_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ProcedureErrorMsg });
            statusStrip1.Location = new Point(0, 607);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1143, 22);
            statusStrip1.TabIndex = 23;
            statusStrip1.Text = "statusStrip1";
            // 
            // ProcedureErrorMsg
            // 
            ProcedureErrorMsg.Name = "ProcedureErrorMsg";
            ProcedureErrorMsg.Size = new Size(37, 17);
            ProcedureErrorMsg.Text = "          ";
            // 
            // ab2ToolStrip1
            // 
            ab2ToolStrip1.AutoSize = false;
            ab2ToolStrip1.BackColor = SystemColors.ControlLight;
            ab2ToolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            ab2ToolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, TextBoxName, toolStripLabel3, FromDate, toolStripLabel4, ToDate, toolStripLabel2, ComboBoxStatus, toolStripLabel5, BtnGo });
            ab2ToolStrip1.Location = new Point(0, 0);
            ab2ToolStrip1.Name = "ab2ToolStrip1";
            ab2ToolStrip1.Padding = new Padding(5);
            ab2ToolStrip1.Size = new Size(1143, 39);
            ab2ToolStrip1.TabIndex = 20;
            ab2ToolStrip1.Text = "ab2ToolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(79, 26);
            toolStripLabel1.Text = "Patient Name";
            // 
            // TextBoxName
            // 
            TextBoxName.AutoSize = false;
            TextBoxName.BackColor = Color.White;
            TextBoxName.BorderStyle = BorderStyle.FixedSingle;
            TextBoxName.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxName.Name = "TextBoxName";
            TextBoxName.ReadOnly = true;
            TextBoxName.Size = new Size(200, 22);
            // 
            // toolStripLabel3
            // 
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new Size(35, 26);
            toolStripLabel3.Text = "From";
            // 
            // FromDate
            // 
            FromDate.BackColor = Color.White;
            FromDate.Date = null;
            FromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FromDate.Format = "MM/dd/yyyy";
            FromDate.MaxDate = new DateTime(9997, 12, 31, 9, 37, 58, 0);
            FromDate.MinDate = new DateTime(1900, 1, 1, 23, 16, 23, 0);
            FromDate.Name = "FromDate";
            FromDate.Size = new Size(97, 26);
            FromDate.Text = "toolStripCalendar2";
            // 
            // toolStripLabel4
            // 
            toolStripLabel4.Name = "toolStripLabel4";
            toolStripLabel4.Size = new Size(20, 26);
            toolStripLabel4.Text = "To";
            // 
            // ToDate
            // 
            ToDate.BackColor = Color.White;
            ToDate.Date = null;
            ToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ToDate.Format = "MM/dd/yyyy";
            ToDate.MaxDate = new DateTime(9997, 12, 31, 9, 37, 58, 0);
            ToDate.MinDate = new DateTime(1900, 1, 1, 23, 16, 23, 0);
            ToDate.Name = "ToDate";
            ToDate.Size = new Size(97, 26);
            ToDate.Text = "toolStripCalendar1";
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(39, 26);
            toolStripLabel2.Text = "Status";
            // 
            // ComboBoxStatus
            // 
            ComboBoxStatus.AutoSize = false;
            ComboBoxStatus.BackColor = Color.White;
            ComboBoxStatus.Name = "ComboBoxStatus";
            comboTreeNode1.Checked = true;
            comboTreeNode1.CheckState = CheckState.Checked;
            comboTreeNode1.Expanded = false;
            comboTreeNode1.Tag = null;
            comboTreeNode1.Text = "Requested";
            comboTreeNode1.ToolTip = null;
            comboTreeNode2.Checked = true;
            comboTreeNode2.CheckState = CheckState.Checked;
            comboTreeNode2.Expanded = false;
            comboTreeNode2.Tag = null;
            comboTreeNode2.Text = "In Progress";
            comboTreeNode2.ToolTip = null;
            comboTreeNode3.Expanded = false;
            comboTreeNode3.Tag = null;
            comboTreeNode3.Text = "Completed";
            comboTreeNode3.ToolTip = null;
            comboTreeNode4.Expanded = false;
            comboTreeNode4.Tag = null;
            comboTreeNode4.Text = "Cancelled";
            comboTreeNode4.ToolTip = null;
            ComboBoxStatus.Nodes.Add(comboTreeNode1);
            ComboBoxStatus.Nodes.Add(comboTreeNode2);
            ComboBoxStatus.Nodes.Add(comboTreeNode3);
            ComboBoxStatus.Nodes.Add(comboTreeNode4);
            ComboBoxStatus.SelectedNode = null;
            ComboBoxStatus.Size = new Size(200, 23);
            // 
            // toolStripLabel5
            // 
            toolStripLabel5.Name = "toolStripLabel5";
            toolStripLabel5.Size = new Size(10, 26);
            toolStripLabel5.Text = " ";
            // 
            // BtnGo
            // 
            BtnGo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnGo.Image = (Image)resources.GetObject("BtnGo.Image");
            BtnGo.ImageTransparentColor = Color.Magenta;
            BtnGo.Name = "BtnGo";
            BtnGo.Size = new Size(26, 26);
            BtnGo.Text = "Go";
            BtnGo.Click += BtnGo_Click;
            // 
            // GridViewProcedureInfo
            // 
            GridViewProcedureInfo.AllowUserToAddRows = false;
            GridViewProcedureInfo.AllowUserToDeleteRows = false;
            GridViewProcedureInfo.AllowUserToResizeColumns = false;
            GridViewProcedureInfo.AllowUserToResizeRows = false;
            GridViewProcedureInfo.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            GridViewProcedureInfo.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewProcedureInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewProcedureInfo.ColumnHeadersHeight = 20;
            GridViewProcedureInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewProcedureInfo.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, ProcedureOrderedDate, dataGridViewNameColumn1, dataGridViewCurrencyColumn5, Column44, Status, dataGridViewCurrencyColumn6, dataGridViewCurrencyColumn7, ProcedureNote, Fees, dataGridViewTextBoxColumn2 });
            GridViewProcedureInfo.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewProcedureInfo.EnableHeadersVisualStyles = false;
            GridViewProcedureInfo.Location = new Point(10, 52);
            GridViewProcedureInfo.MultiSelect = false;
            GridViewProcedureInfo.Name = "GridViewProcedureInfo";
            GridViewProcedureInfo.ReadOnly = true;
            GridViewProcedureInfo.RowHeadersVisible = false;
            GridViewProcedureInfo.RowHeadersWidth = 51;
            GridViewProcedureInfo.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle12.BackColor = Color.White;
            dataGridViewCellStyle12.ForeColor = Color.Black;
            dataGridViewCellStyle12.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = Color.White;
            dataGridViewCellStyle12.WrapMode = DataGridViewTriState.True;
            GridViewProcedureInfo.RowsDefaultCellStyle = dataGridViewCellStyle12;
            GridViewProcedureInfo.RowTemplate.Height = 20;
            GridViewProcedureInfo.ScrollBars = ScrollBars.Vertical;
            GridViewProcedureInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewProcedureInfo.ShowCellToolTips = false;
            GridViewProcedureInfo.Size = new Size(1124, 500);
            GridViewProcedureInfo.TabIndex = 19;
            GridViewProcedureInfo.CellDoubleClick += GridViewProcedureInfo_CellDoubleClick;
            GridViewProcedureInfo.DataError += GridViewProcedureInfo_DataError;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewTextBoxColumn1.HeaderText = "#";
            dataGridViewTextBoxColumn1.MaxInputLength = 30;
            dataGridViewTextBoxColumn1.MinimumWidth = 6;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            dataGridViewTextBoxColumn1.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn1.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn1.Width = 25;
            // 
            // ProcedureOrderedDate
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            ProcedureOrderedDate.DefaultCellStyle = dataGridViewCellStyle3;
            ProcedureOrderedDate.HeaderText = "Date";
            ProcedureOrderedDate.Name = "ProcedureOrderedDate";
            ProcedureOrderedDate.ReadOnly = true;
            ProcedureOrderedDate.Resizable = DataGridViewTriState.False;
            // 
            // dataGridViewNameColumn1
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewNameColumn1.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewNameColumn1.HeaderText = "Name";
            dataGridViewNameColumn1.MinimumWidth = 6;
            dataGridViewNameColumn1.Name = "dataGridViewNameColumn1";
            dataGridViewNameColumn1.ReadOnly = true;
            dataGridViewNameColumn1.Resizable = DataGridViewTriState.False;
            dataGridViewNameColumn1.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewNameColumn1.Width = 150;
            // 
            // dataGridViewCurrencyColumn5
            // 
            dataGridViewCurrencyColumn5.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCurrencyColumn5.DataPropertyName = "Description";
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle5.BackColor = Color.Silver;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            dataGridViewCurrencyColumn5.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCurrencyColumn5.HeaderText = "Description";
            dataGridViewCurrencyColumn5.MinimumWidth = 6;
            dataGridViewCurrencyColumn5.Name = "dataGridViewCurrencyColumn5";
            dataGridViewCurrencyColumn5.ReadOnly = true;
            dataGridViewCurrencyColumn5.Resizable = DataGridViewTriState.False;
            dataGridViewCurrencyColumn5.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column44
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.TopLeft;
            Column44.DefaultCellStyle = dataGridViewCellStyle6;
            Column44.HeaderText = "Requested By";
            Column44.Name = "Column44";
            Column44.ReadOnly = true;
            Column44.Resizable = DataGridViewTriState.False;
            Column44.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Status
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle7.BackColor = Color.Silver;
            Status.DefaultCellStyle = dataGridViewCellStyle7;
            Status.HeaderText = "Status";
            Status.MinimumWidth = 6;
            Status.Name = "Status";
            Status.ReadOnly = true;
            Status.Resizable = DataGridViewTriState.False;
            Status.SortMode = DataGridViewColumnSortMode.NotSortable;
            Status.Width = 70;
            // 
            // dataGridViewCurrencyColumn6
            // 
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
            dataGridViewCurrencyColumn6.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCurrencyColumn6.HeaderText = "Perfromed By";
            dataGridViewCurrencyColumn6.MinimumWidth = 6;
            dataGridViewCurrencyColumn6.Name = "dataGridViewCurrencyColumn6";
            dataGridViewCurrencyColumn6.ReadOnly = true;
            dataGridViewCurrencyColumn6.Resizable = DataGridViewTriState.False;
            dataGridViewCurrencyColumn6.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewCurrencyColumn7
            // 
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCurrencyColumn7.DefaultCellStyle = dataGridViewCellStyle9;
            dataGridViewCurrencyColumn7.HeaderText = "Performed On";
            dataGridViewCurrencyColumn7.MinimumWidth = 6;
            dataGridViewCurrencyColumn7.Name = "dataGridViewCurrencyColumn7";
            dataGridViewCurrencyColumn7.ReadOnly = true;
            dataGridViewCurrencyColumn7.Resizable = DataGridViewTriState.False;
            // 
            // ProcedureNote
            // 
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.TopLeft;
            ProcedureNote.DefaultCellStyle = dataGridViewCellStyle10;
            ProcedureNote.HeaderText = "Note";
            ProcedureNote.Name = "ProcedureNote";
            ProcedureNote.ReadOnly = true;
            ProcedureNote.Resizable = DataGridViewTriState.False;
            ProcedureNote.SortMode = DataGridViewColumnSortMode.NotSortable;
            ProcedureNote.Width = 180;
            // 
            // Fees
            // 
            Fees.DataPropertyName = "Fees";
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.TopRight;
            Fees.DefaultCellStyle = dataGridViewCellStyle11;
            Fees.HeaderText = "Fees";
            Fees.MinimumWidth = 6;
            Fees.Name = "Fees";
            Fees.ReadOnly = true;
            Fees.Resizable = DataGridViewTriState.False;
            Fees.Width = 75;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "ID";
            dataGridViewTextBoxColumn2.MinimumWidth = 6;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            dataGridViewTextBoxColumn2.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn2.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn2.Visible = false;
            dataGridViewTextBoxColumn2.Width = 125;
            // 
            // FormPatientProcedures
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1143, 629);
            Controls.Add(statusStrip1);
            Controls.Add(BtnUpdate);
            Controls.Add(BtnExit);
            Controls.Add(ab2ToolStrip1);
            Controls.Add(GridViewProcedureInfo);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormPatientProcedures";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Patient Procedures In Progress";
            Load += FormPatientProcedures_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ab2ToolStrip1.ResumeLayout(false);
            ab2ToolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewProcedureInfo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private controls.DataViewVerticalScroll GridViewProcedureInfo;
        private controls.Ab2ToolStrip ab2ToolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private controls.ToolStripCalendar FromDate;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private controls.ToolStripCalendar ToDate;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private controls.ToolStripComboTree ComboBoxStatus;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Button BtnUpdate;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripButton BtnGo;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ProcedureErrorMsg;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private controls.grid.DataGridViewCalendarColumn ProcedureOrderedDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewNameColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewCurrencyColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column44;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewCurrencyColumn6;
        private controls.grid.DataGridViewCalendarColumn dataGridViewCurrencyColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProcedureNote;
        private controls.grid.DataGridViewCurrencyColumn dataGridViewCurrencyColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.ToolStripTextBox TextBoxName;
        private controls.grid.DataGridViewCurrencyColumn Fees;
    }
}