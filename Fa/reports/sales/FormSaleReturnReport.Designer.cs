namespace fa.reports.sales
{
    partial class FormSaleReturnReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSaleReturnReport));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle17 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle13 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle14 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle15 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle16 = new DataGridViewCellStyle();
            BtnExit = new Button();
            BtnCancel = new Button();
            BtnPrint = new Button();
            BtnSave = new Button();
            Ab2ToolStripForPurchaseReturn = new views.controls.Ab2ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            ComboBoxReportType = new ToolStripComboBox();
            LabelCustomer = new ToolStripLabel();
            ComboBoxCustomerforReturn = new views.controls.ToolstripCheckedTreeComboBox();
            toolStripLabel2 = new ToolStripLabel();
            FromDate = new views.controls.ToolStripCalendar();
            toolStripLabel3 = new ToolStripLabel();
            ToDate = new views.controls.ToolStripCalendar();
            BtnGo = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            ToolStripSave = new ToolStripButton();
            ToolStripPrint = new ToolStripButton();
            GridViewForCustomer = new views.controls.DataViewVerticalScroll();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
            dataGridViewCurrencyColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewCurrencyColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewCurrencyColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewCurrencyColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
            SaleReturnReportErrorMsg = new ToolStripStatusLabel();
            statusStrip1 = new StatusStrip();
            GridViewForInvoice = new views.controls.DataViewVerticalScroll();
            ByInvoiceSnumber = new DataGridViewTextBoxColumn();
            ByInvoiceInvoiceNumber = new DataGridViewTextBoxColumn();
            ByInvoiceInvoiceDate = new DataGridViewTextBoxColumn();
            ByInvoiceCustomerDetails = new DataGridViewTextBoxColumn();
            ByInvoiceInvoiceType = new DataGridViewTextBoxColumn();
            ByInvoiceTaxAmount = new DataGridViewTextBoxColumn();
            ByInvoiceAmount = new DataGridViewTextBoxColumn();
            Ab2ToolStripForPurchaseReturn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewForCustomer).BeginInit();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewForInvoice).BeginInit();
            SuspendLayout();
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(812, 501);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(72, 23);
            BtnExit.TabIndex = 26;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(562, 501);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(82, 23);
            BtnCancel.TabIndex = 25;
            BtnCancel.Text = "Reset [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Enabled = false;
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(730, 501);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(75, 23);
            BtnPrint.TabIndex = 24;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnSave
            // 
            BtnSave.Enabled = false;
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(650, 501);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 23);
            BtnSave.TabIndex = 23;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // Ab2ToolStripForPurchaseReturn
            // 
            Ab2ToolStripForPurchaseReturn.BackColor = SystemColors.ControlLight;
            Ab2ToolStripForPurchaseReturn.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Ab2ToolStripForPurchaseReturn.GripStyle = ToolStripGripStyle.Hidden;
            Ab2ToolStripForPurchaseReturn.Items.AddRange(new ToolStripItem[] { toolStripLabel1, ComboBoxReportType, LabelCustomer, ComboBoxCustomerforReturn, toolStripLabel2, FromDate, toolStripLabel3, ToDate, BtnGo, toolStripSeparator1, ToolStripSave, ToolStripPrint });
            Ab2ToolStripForPurchaseReturn.Location = new Point(0, 0);
            Ab2ToolStripForPurchaseReturn.Name = "Ab2ToolStripForPurchaseReturn";
            Ab2ToolStripForPurchaseReturn.Padding = new Padding(5);
            Ab2ToolStripForPurchaseReturn.Size = new Size(918, 38);
            Ab2ToolStripForPurchaseReturn.TabIndex = 21;
            Ab2ToolStripForPurchaseReturn.Text = "ab2ToolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(35, 25);
            toolStripLabel1.Text = "Type";
            // 
            // ComboBoxReportType
            // 
            ComboBoxReportType.FlatStyle = FlatStyle.Standard;
            ComboBoxReportType.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ComboBoxReportType.Items.AddRange(new object[] { "By Invoice", "By Customer" });
            ComboBoxReportType.Name = "ComboBoxReportType";
            ComboBoxReportType.Size = new Size(121, 28);
            ComboBoxReportType.Text = "By Invoice";
            ComboBoxReportType.SelectedIndexChanged += ComboBoxReportType_SelectedIndexChanged;
            // 
            // LabelCustomer
            // 
            LabelCustomer.Name = "LabelCustomer";
            LabelCustomer.Size = new Size(59, 25);
            LabelCustomer.Text = "Customer";
            LabelCustomer.Visible = false;
            // 
            // ComboBoxCustomerforReturn
            // 
            ComboBoxCustomerforReturn.AutoSize = false;
            ComboBoxCustomerforReturn.Name = "ComboBoxCustomerforReturn";
            ComboBoxCustomerforReturn.SelectedNode = null;
            ComboBoxCustomerforReturn.Size = new Size(175, 25);
            ComboBoxCustomerforReturn.Visible = false;
            ComboBoxCustomerforReturn.NodeClickedEvent += ComboBoxCustomerforReturn_NodeClickedEvent;
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(34, 25);
            toolStripLabel2.Text = "From";
            // 
            // FromDate
            // 
            FromDate.BackColor = Color.White;
            FromDate.Date = null;
            FromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FromDate.Format = "MM/dd/yyyy";
            FromDate.MaxDate = new DateTime(9997, 12, 31, 9, 22, 1, 0);
            FromDate.MinDate = new DateTime(1900, 1, 1, 20, 44, 8, 0);
            FromDate.Name = "FromDate";
            FromDate.Size = new Size(97, 25);
            FromDate.Text = "toolStripCalendar1";
            FromDate.KeyDown += FromDate_KeyDown;
            // 
            // toolStripLabel3
            // 
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new Size(22, 25);
            toolStripLabel3.Text = "To";
            // 
            // ToDate
            // 
            ToDate.BackColor = Color.White;
            ToDate.Date = null;
            ToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ToDate.Format = "MM/dd/yyyy";
            ToDate.MaxDate = new DateTime(9997, 12, 31, 9, 22, 1, 0);
            ToDate.MinDate = new DateTime(1900, 1, 1, 20, 44, 8, 0);
            ToDate.Name = "ToDate";
            ToDate.Size = new Size(97, 25);
            ToDate.Text = "toolStripCalendar2";
            // 
            // BtnGo
            // 
            BtnGo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnGo.Image = (Image)resources.GetObject("BtnGo.Image");
            BtnGo.ImageTransparentColor = Color.Magenta;
            BtnGo.Name = "BtnGo";
            BtnGo.Size = new Size(26, 25);
            BtnGo.Text = "Go";
            BtnGo.Click += BtnGo_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 28);
            // 
            // ToolStripSave
            // 
            ToolStripSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripSave.Image = (Image)resources.GetObject("ToolStripSave.Image");
            ToolStripSave.ImageTransparentColor = Color.Black;
            ToolStripSave.Name = "ToolStripSave";
            ToolStripSave.Size = new Size(23, 25);
            ToolStripSave.Text = "Save";
            ToolStripSave.Click += BtnSave_Click;
            // 
            // ToolStripPrint
            // 
            ToolStripPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripPrint.Image = (Image)resources.GetObject("ToolStripPrint.Image");
            ToolStripPrint.ImageTransparentColor = Color.Black;
            ToolStripPrint.Name = "ToolStripPrint";
            ToolStripPrint.Size = new Size(23, 25);
            ToolStripPrint.Text = "Print";
            ToolStripPrint.Click += BtnPrint_Click;
            // 
            // GridViewForCustomer
            // 
            GridViewForCustomer.AllowUserToAddRows = false;
            GridViewForCustomer.AllowUserToDeleteRows = false;
            GridViewForCustomer.AllowUserToResizeColumns = false;
            GridViewForCustomer.AllowUserToResizeRows = false;
            GridViewForCustomer.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewForCustomer.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewForCustomer.ColumnHeadersHeight = 20;
            GridViewForCustomer.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewForCustomer.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn6, dataGridViewTextBoxColumn7, dataGridViewTextBoxColumn8, dataGridViewCurrencyColumn3, dataGridViewCurrencyColumn4, dataGridViewCurrencyColumn5, dataGridViewCurrencyColumn6, dataGridViewTextBoxColumn9 });
            GridViewForCustomer.EnableHeadersVisualStyles = false;
            GridViewForCustomer.Location = new Point(5, 44);
            GridViewForCustomer.MultiSelect = false;
            GridViewForCustomer.Name = "GridViewForCustomer";
            GridViewForCustomer.ReadOnly = true;
            GridViewForCustomer.RowHeadersVisible = false;
            dataGridViewCellStyle8.BackColor = Color.White;
            dataGridViewCellStyle8.ForeColor = Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = Color.White;
            dataGridViewCellStyle8.SelectionForeColor = Color.Black;
            GridViewForCustomer.RowsDefaultCellStyle = dataGridViewCellStyle8;
            GridViewForCustomer.RowTemplate.Height = 20;
            GridViewForCustomer.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewForCustomer.ShowCellToolTips = false;
            GridViewForCustomer.Size = new Size(908, 452);
            GridViewForCustomer.TabIndex = 27;
            GridViewForCustomer.Visible = false;
            GridViewForCustomer.CellPainting += GridViewForCustomer_CellPainting;
            GridViewForCustomer.RowPostPaint += GridViewForCustomer_RowPostPaint;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewTextBoxColumn6.HeaderText = "#";
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.ReadOnly = true;
            dataGridViewTextBoxColumn6.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn6.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn6.Width = 50;
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewTextBoxColumn7.HeaderText = "Invoice";
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            dataGridViewTextBoxColumn7.ReadOnly = true;
            dataGridViewTextBoxColumn7.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn7.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn7.Width = 205;
            // 
            // dataGridViewTextBoxColumn8
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewTextBoxColumn8.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewTextBoxColumn8.HeaderText = "DateTime";
            dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            dataGridViewTextBoxColumn8.ReadOnly = true;
            dataGridViewTextBoxColumn8.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn8.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn8.Width = 120;
            // 
            // dataGridViewCurrencyColumn3
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCurrencyColumn3.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCurrencyColumn3.HeaderText = "Tax";
            dataGridViewCurrencyColumn3.Name = "dataGridViewCurrencyColumn3";
            dataGridViewCurrencyColumn3.ReadOnly = true;
            dataGridViewCurrencyColumn3.Resizable = DataGridViewTriState.False;
            dataGridViewCurrencyColumn3.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewCurrencyColumn3.Width = 105;
            // 
            // dataGridViewCurrencyColumn4
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCurrencyColumn4.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCurrencyColumn4.HeaderText = "Discount";
            dataGridViewCurrencyColumn4.Name = "dataGridViewCurrencyColumn4";
            dataGridViewCurrencyColumn4.ReadOnly = true;
            dataGridViewCurrencyColumn4.Resizable = DataGridViewTriState.False;
            dataGridViewCurrencyColumn4.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewCurrencyColumn4.Width = 105;
            // 
            // dataGridViewCurrencyColumn5
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCurrencyColumn5.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCurrencyColumn5.HeaderText = "Cash Amount";
            dataGridViewCurrencyColumn5.Name = "dataGridViewCurrencyColumn5";
            dataGridViewCurrencyColumn5.ReadOnly = true;
            dataGridViewCurrencyColumn5.Resizable = DataGridViewTriState.False;
            dataGridViewCurrencyColumn5.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewCurrencyColumn5.Width = 150;
            // 
            // dataGridViewCurrencyColumn6
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCurrencyColumn6.DefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCurrencyColumn6.HeaderText = "Credit Amount";
            dataGridViewCurrencyColumn6.Name = "dataGridViewCurrencyColumn6";
            dataGridViewCurrencyColumn6.ReadOnly = true;
            dataGridViewCurrencyColumn6.Resizable = DataGridViewTriState.False;
            dataGridViewCurrencyColumn6.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewCurrencyColumn6.Width = 150;
            // 
            // dataGridViewTextBoxColumn9
            // 
            dataGridViewTextBoxColumn9.HeaderText = "type";
            dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            dataGridViewTextBoxColumn9.ReadOnly = true;
            dataGridViewTextBoxColumn9.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn9.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn9.Visible = false;
            // 
            // SaleReturnReportErrorMsg
            // 
            SaleReturnReportErrorMsg.Name = "SaleReturnReportErrorMsg";
            SaleReturnReportErrorMsg.Size = new Size(0, 17);
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { SaleReturnReportErrorMsg });
            statusStrip1.Location = new Point(0, 540);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(918, 22);
            statusStrip1.TabIndex = 22;
            statusStrip1.Text = "statusStrip1";
            // 
            // GridViewForInvoice
            // 
            GridViewForInvoice.AllowUserToAddRows = false;
            GridViewForInvoice.AllowUserToDeleteRows = false;
            GridViewForInvoice.AllowUserToResizeColumns = false;
            GridViewForInvoice.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = SystemColors.Control;
            dataGridViewCellStyle9.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle9.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            GridViewForInvoice.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            GridViewForInvoice.ColumnHeadersHeight = 20;
            GridViewForInvoice.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewForInvoice.Columns.AddRange(new DataGridViewColumn[] { ByInvoiceSnumber, ByInvoiceInvoiceNumber, ByInvoiceInvoiceDate, ByInvoiceCustomerDetails, ByInvoiceInvoiceType, ByInvoiceTaxAmount, ByInvoiceAmount });
            GridViewForInvoice.EditMode = DataGridViewEditMode.EditProgrammatically;
            GridViewForInvoice.EnableHeadersVisualStyles = false;
            GridViewForInvoice.Location = new Point(5, 44);
            GridViewForInvoice.MultiSelect = false;
            GridViewForInvoice.Name = "GridViewForInvoice";
            GridViewForInvoice.ReadOnly = true;
            GridViewForInvoice.RowHeadersVisible = false;
            dataGridViewCellStyle17.BackColor = Color.White;
            dataGridViewCellStyle17.ForeColor = Color.Black;
            dataGridViewCellStyle17.SelectionBackColor = Color.White;
            dataGridViewCellStyle17.SelectionForeColor = Color.Black;
            GridViewForInvoice.RowsDefaultCellStyle = dataGridViewCellStyle17;
            GridViewForInvoice.RowTemplate.Height = 20;
            GridViewForInvoice.ShowCellToolTips = false;
            GridViewForInvoice.Size = new Size(908, 451);
            GridViewForInvoice.TabIndex = 28;
            GridViewForInvoice.CellPainting += GridViewForInvoice_CellPainting;
            // 
            // ByInvoiceSnumber
            // 
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.TopLeft;
            ByInvoiceSnumber.DefaultCellStyle = dataGridViewCellStyle10;
            ByInvoiceSnumber.Frozen = true;
            ByInvoiceSnumber.HeaderText = "#";
            ByInvoiceSnumber.Name = "ByInvoiceSnumber";
            ByInvoiceSnumber.ReadOnly = true;
            ByInvoiceSnumber.Resizable = DataGridViewTriState.False;
            ByInvoiceSnumber.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByInvoiceSnumber.Width = 50;
            // 
            // ByInvoiceInvoiceNumber
            // 
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.TopLeft;
            ByInvoiceInvoiceNumber.DefaultCellStyle = dataGridViewCellStyle11;
            ByInvoiceInvoiceNumber.Frozen = true;
            ByInvoiceInvoiceNumber.HeaderText = "Invoice";
            ByInvoiceInvoiceNumber.Name = "ByInvoiceInvoiceNumber";
            ByInvoiceInvoiceNumber.ReadOnly = true;
            ByInvoiceInvoiceNumber.Resizable = DataGridViewTriState.False;
            ByInvoiceInvoiceNumber.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByInvoiceInvoiceNumber.Width = 115;
            // 
            // ByInvoiceInvoiceDate
            // 
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.TopLeft;
            ByInvoiceInvoiceDate.DefaultCellStyle = dataGridViewCellStyle12;
            ByInvoiceInvoiceDate.Frozen = true;
            ByInvoiceInvoiceDate.HeaderText = "Date";
            ByInvoiceInvoiceDate.Name = "ByInvoiceInvoiceDate";
            ByInvoiceInvoiceDate.ReadOnly = true;
            ByInvoiceInvoiceDate.Resizable = DataGridViewTriState.False;
            ByInvoiceInvoiceDate.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // ByInvoiceCustomerDetails
            // 
            dataGridViewCellStyle13.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle13.WrapMode = DataGridViewTriState.True;
            ByInvoiceCustomerDetails.DefaultCellStyle = dataGridViewCellStyle13;
            ByInvoiceCustomerDetails.Frozen = true;
            ByInvoiceCustomerDetails.HeaderText = "Customer Details";
            ByInvoiceCustomerDetails.Name = "ByInvoiceCustomerDetails";
            ByInvoiceCustomerDetails.ReadOnly = true;
            ByInvoiceCustomerDetails.Resizable = DataGridViewTriState.False;
            ByInvoiceCustomerDetails.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByInvoiceCustomerDetails.Width = 350;
            // 
            // ByInvoiceInvoiceType
            // 
            dataGridViewCellStyle14.Alignment = DataGridViewContentAlignment.TopLeft;
            ByInvoiceInvoiceType.DefaultCellStyle = dataGridViewCellStyle14;
            ByInvoiceInvoiceType.HeaderText = "Cash/Credit";
            ByInvoiceInvoiceType.Name = "ByInvoiceInvoiceType";
            ByInvoiceInvoiceType.ReadOnly = true;
            ByInvoiceInvoiceType.Resizable = DataGridViewTriState.False;
            ByInvoiceInvoiceType.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByInvoiceInvoiceType.Width = 85;
            // 
            // ByInvoiceTaxAmount
            // 
            dataGridViewCellStyle15.Alignment = DataGridViewContentAlignment.TopRight;
            ByInvoiceTaxAmount.DefaultCellStyle = dataGridViewCellStyle15;
            ByInvoiceTaxAmount.HeaderText = "Tax";
            ByInvoiceTaxAmount.Name = "ByInvoiceTaxAmount";
            ByInvoiceTaxAmount.ReadOnly = true;
            ByInvoiceTaxAmount.Resizable = DataGridViewTriState.False;
            ByInvoiceTaxAmount.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByInvoiceTaxAmount.Width = 85;
            // 
            // ByInvoiceAmount
            // 
            dataGridViewCellStyle16.Alignment = DataGridViewContentAlignment.TopRight;
            ByInvoiceAmount.DefaultCellStyle = dataGridViewCellStyle16;
            ByInvoiceAmount.HeaderText = "Net Amount";
            ByInvoiceAmount.Name = "ByInvoiceAmount";
            ByInvoiceAmount.ReadOnly = true;
            ByInvoiceAmount.Resizable = DataGridViewTriState.False;
            ByInvoiceAmount.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // FormSaleReturnReport
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(918, 562);
            Controls.Add(BtnExit);
            Controls.Add(BtnCancel);
            Controls.Add(BtnPrint);
            Controls.Add(BtnSave);
            Controls.Add(Ab2ToolStripForPurchaseReturn);
            Controls.Add(statusStrip1);
            Controls.Add(GridViewForCustomer);
            Controls.Add(GridViewForInvoice);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSaleReturnReport";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Sale Return Report";
            Load += FormSaleReturnReport_Load;
            Ab2ToolStripForPurchaseReturn.ResumeLayout(false);
            Ab2ToolStripForPurchaseReturn.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewForCustomer).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewForInvoice).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtnExit;
        private Button BtnCancel;
        private Button BtnPrint;
        private Button BtnSave;
        private views.controls.Ab2ToolStrip Ab2ToolStripForPurchaseReturn;
        private ToolStripLabel toolStripLabel1;
        private ToolStripComboBox ComboBoxReportType;
        private ToolStripLabel LabelCategory;
        private views.controls.ToolStripComboTree ComboBoxCategory;
        private ToolStripLabel LabelItem;
        private ToolStripComboBox ComboBoxItem;
        private ToolStripLabel LabelCustomer;
        private ToolStripLabel toolStripLabel2;
        private views.controls.ToolStripCalendar FromDate;
        private ToolStripLabel toolStripLabel3;
        private views.controls.ToolStripCalendar ToDate;
        private ToolStripButton BtnGo;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton ToolStripSave;
        private ToolStripButton ToolStripPrint;
        private views.controls.DataViewVerticalScroll GridViewForCustomer;
        private ToolStripStatusLabel ErrorMsg;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel SaleReturnReportErrorMsg;
        private views.controls.DataViewVerticalScroll GridViewForInvoice;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DataGridViewTextBoxColumn ByInvoiceSnumber;
        private DataGridViewTextBoxColumn ByInvoiceInvoiceNumber;
        private DataGridViewTextBoxColumn ByInvoiceInvoiceDate;
        private DataGridViewTextBoxColumn ByInvoiceCustomerDetails;
        private DataGridViewTextBoxColumn ByInvoiceInvoiceType;
        //private views.controls.ToolstripCheckedTreeComboBox ComboBoxCustomer;
        private DataGridViewTextBoxColumn dataGridViewCurrencyColumn3;
        private DataGridViewTextBoxColumn dataGridViewCurrencyColumn4;
        private DataGridViewTextBoxColumn dataGridViewCurrencyColumn5;
        private DataGridViewTextBoxColumn dataGridViewCurrencyColumn6;
        private DataGridViewTextBoxColumn ByInvoiceTaxAmount;
        private DataGridViewTextBoxColumn ByInvoiceAmount;
        private views.controls.ToolstripCheckedTreeComboBox ComboBoxCustomerforReturn;
    }
}