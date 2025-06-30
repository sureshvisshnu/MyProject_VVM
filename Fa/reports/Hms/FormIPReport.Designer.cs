namespace fa.reports.Hms
{
    partial class FormIPReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormIPReport));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            ErrorMsg = new StatusStrip();
            IpReportErrorMsg = new ToolStripStatusLabel();
            BtnSave = new Button();
            BtnPrint = new Button();
            BtnExit = new Button();
            BtnReset = new Button();
            toolStripLabel1 = new ToolStripLabel();
            toolStripLabel2 = new ToolStripLabel();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripLabel3 = new ToolStripLabel();
            ComboIpReportWards = new ToolStripComboBox();
            toolStripSeparator5 = new ToolStripSeparator();
            toolStripLabel4 = new ToolStripLabel();
            ComboIpReportStatuss = new ToolStripComboBox();
            toolStripSeparator6 = new ToolStripSeparator();
            toolStripLabel5 = new ToolStripLabel();
            ComboIpReportOrders = new ToolStripComboBox();
            toolStripButton1 = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            ToolStripBtnSaves = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            ToolStripBtnPrints = new ToolStripButton();
            toolStripSeparator4 = new ToolStripSeparator();
            ab2ToolStrip1 = new views.controls.Ab2ToolStrip();
            toolStripLabel11 = new ToolStripLabel();
            ComboBoxReportType = new ToolStripComboBox();
            LabelType = new ToolStripLabel();
            ComboBoxInsurance = new views.controls.ToolstripCheckedTreeComboBox();
            ComboBoxWard = new views.controls.ToolstripCheckedTreeComboBox();
            ComboBoxDepartment = new views.controls.ToolstripCheckedTreeComboBox();
            ComboBoxConsultant = new views.controls.ToolstripCheckedTreeComboBox();
            toolStripSeparator12 = new ToolStripSeparator();
            toolStripLabel6 = new ToolStripLabel();
            IpReportFromDate = new views.controls.ToolStripCalendar();
            toolStripLabel7 = new ToolStripLabel();
            IpReportToDate = new views.controls.ToolStripCalendar();
            toolStripSeparator7 = new ToolStripSeparator();
            toolStripLabel9 = new ToolStripLabel();
            ComboIpReportStatus = new ToolStripComboBox();
            toolStripSeparator9 = new ToolStripSeparator();
            toolStripLabel10 = new ToolStripLabel();
            ComboIpReportOrder = new ToolStripComboBox();
            ToolStripBtnGo = new ToolStripButton();
            toolStripSeparator10 = new ToolStripSeparator();
            ToolStripBtnSave = new ToolStripButton();
            toolStripSeparator11 = new ToolStripSeparator();
            ToolStripBtnPrint = new ToolStripButton();
            IpReportDataGridView = new views.controls.DataViewVerticalScroll();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn10 = new DataGridViewTextBoxColumn();
            RowHead = new DataGridViewTextBoxColumn();
            Ward = new DataGridViewTextBoxColumn();
            Bed = new DataGridViewTextBoxColumn();
            PatientId = new DataGridViewTextBoxColumn();
            PatientDetails = new DataGridViewTextBoxColumn();
            DOB = new DataGridViewTextBoxColumn();
            Age = new DataGridViewTextBoxColumn();
            AdmitedOn = new DataGridViewTextBoxColumn();
            PrimaryDoctor = new DataGridViewTextBoxColumn();
            PrmaryNurse = new DataGridViewTextBoxColumn();
            DischargedOn = new DataGridViewTextBoxColumn();
            IpReportFromDates = new views.controls.ToolStripCalendar();
            IpReportToDates = new views.controls.ToolStripCalendar();
            ErrorMsg.SuspendLayout();
            ab2ToolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)IpReportDataGridView).BeginInit();
            SuspendLayout();
            // 
            // ErrorMsg
            // 
            ErrorMsg.Items.AddRange(new ToolStripItem[] { IpReportErrorMsg });
            ErrorMsg.Location = new Point(0, 587);
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(1343, 22);
            ErrorMsg.TabIndex = 1;
            ErrorMsg.Text = "statusStrip1";
            // 
            // IpReportErrorMsg
            // 
            IpReportErrorMsg.Name = "IpReportErrorMsg";
            IpReportErrorMsg.Size = new Size(13, 17);
            IpReportErrorMsg.Text = "  ";
            // 
            // BtnSave
            // 
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(1076, 548);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 23);
            BtnSave.TabIndex = 4;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(1157, 548);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(75, 23);
            BtnPrint.TabIndex = 5;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(1238, 548);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(75, 23);
            BtnExit.TabIndex = 6;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnReset
            // 
            BtnReset.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnReset.Location = new Point(988, 548);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new Size(82, 23);
            BtnReset.TabIndex = 24;
            BtnReset.Text = "Reset [Esc]";
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(35, 25);
            toolStripLabel1.Text = "From";
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(21, 25);
            toolStripLabel2.Text = "To";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 28);
            // 
            // toolStripLabel3
            // 
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new Size(35, 25);
            toolStripLabel3.Text = "Ward";
            // 
            // ComboIpReportWards
            // 
            ComboIpReportWards.Name = "ComboIpReportWards";
            ComboIpReportWards.Size = new Size(121, 23);
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(6, 28);
            // 
            // toolStripLabel4
            // 
            toolStripLabel4.Name = "toolStripLabel4";
            toolStripLabel4.Size = new Size(39, 25);
            toolStripLabel4.Text = "Status";
            // 
            // ComboIpReportStatuss
            // 
            ComboIpReportStatuss.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboIpReportStatuss.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboIpReportStatuss.FlatStyle = FlatStyle.Standard;
            ComboIpReportStatuss.Items.AddRange(new object[] { "Current", "All" });
            ComboIpReportStatuss.Name = "ComboIpReportStatuss";
            ComboIpReportStatuss.Size = new Size(121, 23);
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(6, 28);
            // 
            // toolStripLabel5
            // 
            toolStripLabel5.Name = "toolStripLabel5";
            toolStripLabel5.Size = new Size(53, 25);
            toolStripLabel5.Text = "Order By";
            // 
            // ComboIpReportOrders
            // 
            ComboIpReportOrders.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboIpReportOrders.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboIpReportOrders.FlatStyle = FlatStyle.Standard;
            ComboIpReportOrders.Items.AddRange(new object[] { "Ward", "Admission Date", "Discharge Date" });
            ComboIpReportOrders.Name = "ComboIpReportOrders";
            ComboIpReportOrders.Size = new Size(121, 23);
            // 
            // toolStripButton1
            // 
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(23, 23);
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 28);
            // 
            // ToolStripBtnSaves
            // 
            ToolStripBtnSaves.Name = "ToolStripBtnSaves";
            ToolStripBtnSaves.Size = new Size(23, 23);
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 28);
            // 
            // ToolStripBtnPrints
            // 
            ToolStripBtnPrints.Name = "ToolStripBtnPrints";
            ToolStripBtnPrints.Size = new Size(23, 23);
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 28);
            // 
            // ab2ToolStrip1
            // 
            ab2ToolStrip1.BackColor = SystemColors.ControlLight;
            ab2ToolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            ab2ToolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel11, ComboBoxReportType, LabelType, ComboBoxInsurance, ComboBoxWard, ComboBoxDepartment, ComboBoxConsultant, toolStripSeparator12, toolStripLabel6, IpReportFromDate, toolStripLabel7, IpReportToDate, toolStripSeparator7, toolStripLabel9, ComboIpReportStatus, toolStripSeparator9, toolStripLabel10, ComboIpReportOrder, ToolStripBtnGo, toolStripSeparator10, ToolStripBtnSave, toolStripSeparator11, ToolStripBtnPrint });
            ab2ToolStrip1.Location = new Point(0, 0);
            ab2ToolStrip1.Name = "ab2ToolStrip1";
            ab2ToolStrip1.Padding = new Padding(5);
            ab2ToolStrip1.Size = new Size(1343, 38);
            ab2ToolStrip1.TabIndex = 25;
            ab2ToolStrip1.Text = "ab2ToolStrip1";
            // 
            // toolStripLabel11
            // 
            toolStripLabel11.Name = "toolStripLabel11";
            toolStripLabel11.Size = new Size(32, 25);
            toolStripLabel11.Text = "Type";
            // 
            // ComboBoxReportType
            // 
            ComboBoxReportType.FlatStyle = FlatStyle.Standard;
            ComboBoxReportType.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ComboBoxReportType.Items.AddRange(new object[] { "By Date", "By Consultant", "By Department", "By Insurance", "By Ward" });
            ComboBoxReportType.Name = "ComboBoxReportType";
            ComboBoxReportType.Size = new Size(120, 28);
            ComboBoxReportType.Text = "By Date";
            ComboBoxReportType.SelectedIndexChanged += ComboBoxReportType_SelectedIndexChanged;
            // 
            // LabelType
            // 
            LabelType.Name = "LabelType";
            LabelType.Size = new Size(65, 25);
            LabelType.Text = "Consultant";
            LabelType.TextAlign = ContentAlignment.MiddleLeft;
            LabelType.Visible = false;
            // 
            // ComboBoxInsurance
            // 
            ComboBoxInsurance.AutoSize = false;
            ComboBoxInsurance.Name = "ComboBoxInsurance";
            ComboBoxInsurance.SelectedNode = null;
            ComboBoxInsurance.Size = new Size(3, 21);
            ComboBoxInsurance.Visible = false;
            ComboBoxInsurance.NodeClickedEvent += ComboBoxInsurance_NodeClickedEvent;
            // 
            // ComboBoxWard
            // 
            ComboBoxWard.AutoSize = false;
            ComboBoxWard.Name = "ComboBoxWard";
            ComboBoxWard.SelectedNode = null;
            ComboBoxWard.Size = new Size(93, 21);
            ComboBoxWard.Visible = false;
            ComboBoxWard.NodeClickedEvent += ComboBoxWard_NodeClickedEvent;
            // 
            // ComboBoxDepartment
            // 
            ComboBoxDepartment.AutoSize = false;
            ComboBoxDepartment.Name = "ComboBoxDepartment";
            ComboBoxDepartment.SelectedNode = null;
            ComboBoxDepartment.Size = new Size(93, 21);
            ComboBoxDepartment.Visible = false;
            ComboBoxDepartment.NodeClickedEvent += ComboBoxDepartment_NodeClickedEvent;
            // 
            // ComboBoxConsultant
            // 
            ComboBoxConsultant.AutoSize = false;
            ComboBoxConsultant.Name = "ComboBoxConsultant";
            ComboBoxConsultant.SelectedNode = null;
            ComboBoxConsultant.Size = new Size(3, 21);
            ComboBoxConsultant.Visible = false;
            ComboBoxConsultant.NodeClickedEvent += ComboBoxConsultant_NodeClickedEvent;
            // 
            // toolStripSeparator12
            // 
            toolStripSeparator12.Name = "toolStripSeparator12";
            toolStripSeparator12.Size = new Size(6, 28);
            // 
            // toolStripLabel6
            // 
            toolStripLabel6.Name = "toolStripLabel6";
            toolStripLabel6.Size = new Size(35, 25);
            toolStripLabel6.Text = "From";
            // 
            // IpReportFromDate
            // 
            IpReportFromDate.BackColor = Color.White;
            IpReportFromDate.Date = null;
            IpReportFromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            IpReportFromDate.Format = "MM/dd/yyyy";
            IpReportFromDate.MaxDate = new DateTime(9997, 12, 31, 9, 8, 46, 0);
            IpReportFromDate.MinDate = new DateTime(1900, 1, 1, 23, 1, 20, 0);
            IpReportFromDate.Name = "IpReportFromDate";
            IpReportFromDate.Size = new Size(97, 25);
            IpReportFromDate.Text = "Calender";
            // 
            // toolStripLabel7
            // 
            toolStripLabel7.Name = "toolStripLabel7";
            toolStripLabel7.Size = new Size(20, 25);
            toolStripLabel7.Text = "To";
            // 
            // IpReportToDate
            // 
            IpReportToDate.BackColor = Color.White;
            IpReportToDate.Date = null;
            IpReportToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            IpReportToDate.Format = "MM/dd/yyyy";
            IpReportToDate.MaxDate = new DateTime(9997, 12, 31, 9, 8, 46, 0);
            IpReportToDate.MinDate = new DateTime(1900, 1, 1, 23, 1, 20, 0);
            IpReportToDate.Name = "IpReportToDate";
            IpReportToDate.Size = new Size(97, 25);
            IpReportToDate.Text = "Calender";
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new Size(6, 28);
            // 
            // toolStripLabel9
            // 
            toolStripLabel9.Name = "toolStripLabel9";
            toolStripLabel9.Size = new Size(39, 25);
            toolStripLabel9.Text = "Status";
            // 
            // ComboIpReportStatus
            // 
            ComboIpReportStatus.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboIpReportStatus.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboIpReportStatus.FlatStyle = FlatStyle.Standard;
            ComboIpReportStatus.Items.AddRange(new object[] { "Current", "All" });
            ComboIpReportStatus.Name = "ComboIpReportStatus";
            ComboIpReportStatus.Size = new Size(121, 28);
            // 
            // toolStripSeparator9
            // 
            toolStripSeparator9.Name = "toolStripSeparator9";
            toolStripSeparator9.Size = new Size(6, 28);
            // 
            // toolStripLabel10
            // 
            toolStripLabel10.Name = "toolStripLabel10";
            toolStripLabel10.Size = new Size(53, 25);
            toolStripLabel10.Text = "Order By";
            // 
            // ComboIpReportOrder
            // 
            ComboIpReportOrder.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboIpReportOrder.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboIpReportOrder.FlatStyle = FlatStyle.Standard;
            ComboIpReportOrder.Items.AddRange(new object[] { "Ward", "Admission Date", "Discharge Date" });
            ComboIpReportOrder.Name = "ComboIpReportOrder";
            ComboIpReportOrder.Size = new Size(121, 28);
            // 
            // ToolStripBtnGo
            // 
            ToolStripBtnGo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            ToolStripBtnGo.Image = (Image)resources.GetObject("ToolStripBtnGo.Image");
            ToolStripBtnGo.ImageTransparentColor = Color.Magenta;
            ToolStripBtnGo.Name = "ToolStripBtnGo";
            ToolStripBtnGo.Size = new Size(26, 25);
            ToolStripBtnGo.Text = "Go";
            ToolStripBtnGo.Click += ToolStripBtnGo_Click;
            // 
            // toolStripSeparator10
            // 
            toolStripSeparator10.Name = "toolStripSeparator10";
            toolStripSeparator10.Size = new Size(6, 28);
            // 
            // ToolStripBtnSave
            // 
            ToolStripBtnSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnSave.Image = (Image)resources.GetObject("ToolStripBtnSave.Image");
            ToolStripBtnSave.ImageTransparentColor = Color.Black;
            ToolStripBtnSave.Name = "ToolStripBtnSave";
            ToolStripBtnSave.Size = new Size(23, 25);
            ToolStripBtnSave.Text = "Save";
            ToolStripBtnSave.Click += BtnSave_Click;
            // 
            // toolStripSeparator11
            // 
            toolStripSeparator11.Name = "toolStripSeparator11";
            toolStripSeparator11.Size = new Size(6, 28);
            // 
            // ToolStripBtnPrint
            // 
            ToolStripBtnPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnPrint.Image = (Image)resources.GetObject("ToolStripBtnPrint.Image");
            ToolStripBtnPrint.ImageTransparentColor = Color.Black;
            ToolStripBtnPrint.Name = "ToolStripBtnPrint";
            ToolStripBtnPrint.Size = new Size(23, 25);
            ToolStripBtnPrint.Text = "Print";
            ToolStripBtnPrint.Click += BtnPrint_Click;
            // 
            // IpReportDataGridView
            // 
            IpReportDataGridView.AllowUserToAddRows = false;
            IpReportDataGridView.AllowUserToDeleteRows = false;
            IpReportDataGridView.AllowUserToResizeColumns = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            IpReportDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            IpReportDataGridView.ColumnHeadersHeight = 20;
            IpReportDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            IpReportDataGridView.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, dataGridViewTextBoxColumn7, dataGridViewTextBoxColumn8, dataGridViewTextBoxColumn9, dataGridViewTextBoxColumn10, RowHead });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            IpReportDataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            IpReportDataGridView.EnableHeadersVisualStyles = false;
            IpReportDataGridView.Location = new Point(12, 45);
            IpReportDataGridView.Name = "IpReportDataGridView";
            IpReportDataGridView.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            IpReportDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            IpReportDataGridView.RowHeadersVisible = false;
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.ForeColor = Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = Color.White;
            dataGridViewCellStyle5.SelectionForeColor = Color.Black;
            IpReportDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle5;
            IpReportDataGridView.RowTemplate.Height = 20;
            IpReportDataGridView.ScrollBars = ScrollBars.Vertical;
            IpReportDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            IpReportDataGridView.ShowCellToolTips = false;
            IpReportDataGridView.Size = new Size(1320, 488);
            IpReportDataGridView.TabIndex = 3;
            IpReportDataGridView.CellFormatting += IpReportDataGridView_CellFormatting;
            IpReportDataGridView.CellPainting += IpReportDataGridView_CellPainting;
            IpReportDataGridView.RowPostPaint += IpReportDataGridView_RowPostPaint;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "Ward";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            dataGridViewTextBoxColumn1.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn1.Width = 132;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Bed";
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            dataGridViewTextBoxColumn2.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn2.Width = 131;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Patient Id";
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            dataGridViewTextBoxColumn3.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn3.Width = 132;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewTextBoxColumn4.HeaderText = "Name & Address";
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            dataGridViewTextBoxColumn4.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn4.Width = 132;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "DOB";
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.ReadOnly = true;
            dataGridViewTextBoxColumn5.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn5.Width = 131;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.HeaderText = "Age";
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.ReadOnly = true;
            dataGridViewTextBoxColumn6.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn6.Width = 132;
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewTextBoxColumn7.HeaderText = "Admitted On";
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            dataGridViewTextBoxColumn7.ReadOnly = true;
            dataGridViewTextBoxColumn7.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn7.Width = 132;
            // 
            // dataGridViewTextBoxColumn8
            // 
            dataGridViewTextBoxColumn8.HeaderText = "Primary Doctor";
            dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            dataGridViewTextBoxColumn8.ReadOnly = true;
            dataGridViewTextBoxColumn8.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn8.Width = 132;
            // 
            // dataGridViewTextBoxColumn9
            // 
            dataGridViewTextBoxColumn9.HeaderText = "Primary Nurse";
            dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            dataGridViewTextBoxColumn9.ReadOnly = true;
            dataGridViewTextBoxColumn9.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn9.Width = 131;
            // 
            // dataGridViewTextBoxColumn10
            // 
            dataGridViewTextBoxColumn10.HeaderText = "Discharged On";
            dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            dataGridViewTextBoxColumn10.ReadOnly = true;
            dataGridViewTextBoxColumn10.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn10.Width = 132;
            // 
            // RowHead
            // 
            RowHead.HeaderText = "Row Heading";
            RowHead.MinimumWidth = 2;
            RowHead.Name = "RowHead";
            RowHead.ReadOnly = true;
            RowHead.Visible = false;
            // 
            // Ward
            // 
            Ward.HeaderText = "Ward";
            Ward.Name = "Ward";
            Ward.ReadOnly = true;
            Ward.Width = 150;
            // 
            // Bed
            // 
            Bed.HeaderText = "Bed";
            Bed.Name = "Bed";
            Bed.ReadOnly = true;
            // 
            // PatientId
            // 
            PatientId.HeaderText = "Patient Id";
            PatientId.Name = "PatientId";
            PatientId.ReadOnly = true;
            // 
            // PatientDetails
            // 
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            PatientDetails.DefaultCellStyle = dataGridViewCellStyle6;
            PatientDetails.HeaderText = "Name & Address";
            PatientDetails.Name = "PatientDetails";
            PatientDetails.ReadOnly = true;
            PatientDetails.Width = 300;
            // 
            // DOB
            // 
            DOB.HeaderText = "DOB";
            DOB.Name = "DOB";
            DOB.ReadOnly = true;
            // 
            // Age
            // 
            Age.HeaderText = "Age";
            Age.Name = "Age";
            Age.ReadOnly = true;
            Age.Width = 50;
            // 
            // AdmitedOn
            // 
            AdmitedOn.HeaderText = "Admitted On";
            AdmitedOn.Name = "AdmitedOn";
            AdmitedOn.ReadOnly = true;
            // 
            // PrimaryDoctor
            // 
            PrimaryDoctor.HeaderText = "Primary Doctor";
            PrimaryDoctor.Name = "PrimaryDoctor";
            PrimaryDoctor.ReadOnly = true;
            PrimaryDoctor.Width = 150;
            // 
            // PrmaryNurse
            // 
            PrmaryNurse.HeaderText = "Primary Nurse";
            PrmaryNurse.Name = "PrmaryNurse";
            PrmaryNurse.ReadOnly = true;
            PrmaryNurse.Width = 150;
            // 
            // DischargedOn
            // 
            DischargedOn.HeaderText = "Discharged On";
            DischargedOn.Name = "DischargedOn";
            DischargedOn.ReadOnly = true;
            // 
            // IpReportFromDates
            // 
            IpReportFromDates.BackColor = Color.White;
            IpReportFromDates.Date = null;
            IpReportFromDates.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            IpReportFromDates.Format = "MM/dd/yyyy";
            IpReportFromDates.MaxDate = new DateTime(9997, 12, 31, 10, 55, 26, 0);
            IpReportFromDates.MinDate = new DateTime(1900, 1, 1, 18, 24, 45, 0);
            IpReportFromDates.Name = "IpReportFromDates";
            IpReportFromDates.Size = new Size(91, 19);
            // 
            // IpReportToDates
            // 
            IpReportToDates.BackColor = Color.White;
            IpReportToDates.Date = null;
            IpReportToDates.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            IpReportToDates.Format = "MM/dd/yyyy";
            IpReportToDates.MaxDate = new DateTime(9997, 12, 31, 10, 55, 26, 0);
            IpReportToDates.MinDate = new DateTime(1900, 1, 1, 18, 24, 45, 0);
            IpReportToDates.Name = "IpReportToDates";
            IpReportToDates.Size = new Size(91, 19);
            // 
            // FormIPReport
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1343, 609);
            Controls.Add(ab2ToolStrip1);
            Controls.Add(BtnReset);
            Controls.Add(BtnExit);
            Controls.Add(BtnPrint);
            Controls.Add(BtnSave);
            Controls.Add(ErrorMsg);
            Controls.Add(IpReportDataGridView);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormIPReport";
            StartPosition = FormStartPosition.CenterParent;
            Text = " In Patient Report";
            Load += FormIPReport_Load;
            ErrorMsg.ResumeLayout(false);
            ErrorMsg.PerformLayout();
            ab2ToolStrip1.ResumeLayout(false);
            ab2ToolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)IpReportDataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private StatusStrip ErrorMsg;
        private ToolStripLabel toolStripLabel1;
        private views.controls.ToolStripCalendar IpReportFromDates;
        private ToolStripLabel toolStripLabel2;
        private views.controls.ToolStripCalendar IpReportToDates;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton toolStripButton1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton ToolStripBtnSaves;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton ToolStripBtnPrints;
        private ToolStripLabel toolStripLabel3;
        private views.controls.DataViewVerticalScroll IpReportDataGridView;
        private Button BtnSave;
        private Button BtnPrint;
        private Button BtnExit;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripLabel toolStripLabel4;
        private ToolStripLabel toolStripLabel5;
        private ToolStripComboBox ComboIpReportOrders;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripComboBox ComboIpReportWards;
        private ToolStripComboBox ComboIpReportStatuss;
        private DataGridViewTextBoxColumn Ward;
        private DataGridViewTextBoxColumn Bed;
        private DataGridViewTextBoxColumn PatientId;
        private DataGridViewTextBoxColumn PatientDetails;
        private DataGridViewTextBoxColumn DOB;
        private DataGridViewTextBoxColumn Age;
        private DataGridViewTextBoxColumn AdmitedOn;
        private DataGridViewTextBoxColumn PrimaryDoctor;
        private DataGridViewTextBoxColumn PrmaryNurse;
        private DataGridViewTextBoxColumn DischargedOn;
        private ToolStripStatusLabel IpReportErrorMsg;
        private Button BtnReset;
        private views.controls.Ab2ToolStrip ab2ToolStrip1;
        private ToolStripLabel toolStripLabel6;
        private views.controls.ToolStripCalendar IpReportFromDate;
        private ToolStripLabel toolStripLabel7;
        private views.controls.ToolStripCalendar IpReportToDate;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripLabel toolStripLabel9;
        private ToolStripComboBox ComboIpReportStatus;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripLabel toolStripLabel10;
        private ToolStripComboBox ComboIpReportOrder;
        private ToolStripButton ToolStripBtnGo;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripButton ToolStripBtnSave;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripButton ToolStripBtnPrint;
        private ToolStripSeparator toolStripSeparator12;
        private views.controls.ToolstripCheckedTreeComboBox ComboBoxDepartment;
        private ToolStripLabel LabelType;
        private ToolStripComboBox ComboBoxReportType;
        private ToolStripLabel toolStripLabel11;
        private views.controls.ToolstripCheckedTreeComboBox ComboBoxConsultant;
        private views.controls.ToolstripCheckedTreeComboBox ComboBoxInsurance;
        private views.controls.ToolstripCheckedTreeComboBox ComboBoxWard;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private DataGridViewTextBoxColumn RowHead;
    }
}