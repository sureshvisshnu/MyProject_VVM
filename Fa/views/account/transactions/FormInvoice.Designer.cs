namespace fa.views.account.transactions
{
    partial class FormInvoice
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
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInvoice));
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle13 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle14 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle20 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle15 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle16 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle17 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle18 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle19 = new DataGridViewCellStyle();
            label1 = new Label();
            label2 = new Label();
            InvoiceRefNo = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            TextBoxInvoiveMemo = new TextBox();
            DataGridViewInvoiceDetailsTotal = new DataGridView();
            Total = new DataGridViewTextBoxColumn();
            Value = new DataGridViewTextBoxColumn();
            delete1 = new DataGridViewTextBoxColumn();
            openFileDialog1 = new OpenFileDialog();
            TextBoxInvoiceDueDate = new MaskedTextBox();
            TextBoxInvoiceId = new TextBox();
            BtnInvoiceSave = new Button();
            BtnInvoiceCancel = new Button();
            BtnInvoicePrint = new Button();
            BtnInvoiceDelete = new Button();
            BtnInvoiceNew = new Button();
            ImageListInvoice = new ImageList(components);
            statusStrip1 = new StatusStrip();
            ToolStripStatusLabelErrorInvoice = new ToolStripStatusLabel();
            TimerInvoice = new System.Windows.Forms.Timer(components);
            toolStrip1 = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            TextBoxInvoiceSearch = new ToolStripTextBox();
            InvoiceSearchGo = new ToolStripButton();
            label9 = new Label();
            BtnReceivePayment = new Button();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            DateTimePickerInvoiceDate = new controls.text.DateWithCalendar();
            ComboBoxInvoiceTerms = new controls.ComboBoxSwapTextBox();
            GridViewInvoiceSearch = new controls.DataViewVerticalScroll();
            OpenInvoiceNumber = new DataGridViewTextBoxColumn();
            OpenInvDescription = new DataGridViewTextBoxColumn();
            OpenInvAmount = new DataGridViewTextBoxColumn();
            BalAmount = new DataGridViewTextBoxColumn();
            Type = new DataGridViewTextBoxColumn();
            DataGridViewInvoiceDetails = new controls.DataViewVerticalScroll();
            InvoiceSerialNo = new DataGridViewTextBoxColumn();
            ProductOrService = new DataGridViewComboBoxColumn();
            Description = new DataGridViewTextBoxColumn();
            Rate = new controls.grid.DataGridViewCurrencyColumn();
            Quantity = new controls.grid.DataGridViewNumberColumn();
            Amount = new controls.grid.DataGridViewCurrencyColumn();
            Delete = new DataGridViewButtonColumn();
            TextBoxInvoiveInternalMemo = new TextBox();
            BtnInvoiceExit = new Button();
            BtnInvoiceSearchCustomer = new Button();
            TextBoxInvoiceCustomer = new controls.text.IDTextBox();
            BtnInvoiceNewCustomers = new Dropdown_Button.UserControlButtonWithMenu();
            DiscountAdditinalChargeGrid = new controls.accounting.DiscountAdditinalChargeGrid();
            LabelInvoiceFinalAmount = new Label();
            label3 = new Label();
            label10 = new Label();
            LastInvoiceRefNo = new Label();
            ((System.ComponentModel.ISupportInitialize)DataGridViewInvoiceDetailsTotal).BeginInit();
            statusStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewInvoiceSearch).BeginInit();
            ((System.ComponentModel.ISupportInitialize)DataGridViewInvoiceDetails).BeginInit();
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
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(650, 35);
            label1.Name = "label1";
            label1.Size = new Size(53, 13);
            label1.TabIndex = 5;
            label1.Text = "Invoice #";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(8, 35);
            label2.Name = "label2";
            label2.Size = new Size(62, 13);
            label2.TabIndex = 6;
            label2.Text = "Customer";
            // 
            // InvoiceRefNo
            // 
            InvoiceRefNo.AutoSize = true;
            InvoiceRefNo.Font = new Font("Tahoma", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            InvoiceRefNo.Location = new Point(650, 49);
            InvoiceRefNo.Name = "InvoiceRefNo";
            InvoiceRefNo.Size = new Size(55, 16);
            InvoiceRefNo.TabIndex = 0;
            InvoiceRefNo.Text = "000000";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(8, 76);
            label4.Name = "label4";
            label4.Size = new Size(43, 13);
            label4.TabIndex = 8;
            label4.Text = "Terms";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label5.Location = new Point(9, 117);
            label5.Name = "label5";
            label5.Size = new Size(79, 13);
            label5.TabIndex = 9;
            label5.Text = "Invoice Date";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label6.Location = new Point(110, 117);
            label6.Name = "label6";
            label6.Size = new Size(52, 13);
            label6.TabIndex = 10;
            label6.Text = "Due Date";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(321, 35);
            label7.Name = "label7";
            label7.Size = new Size(35, 13);
            label7.TabIndex = 11;
            label7.Text = "Memo";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(321, 93);
            label8.Name = "label8";
            label8.Size = new Size(76, 13);
            label8.TabIndex = 12;
            label8.Text = "Internal Memo";
            // 
            // TextBoxInvoiveMemo
            // 
            TextBoxInvoiveMemo.Location = new Point(324, 51);
            TextBoxInvoiveMemo.MaxLength = 250;
            TextBoxInvoiveMemo.Multiline = true;
            TextBoxInvoiveMemo.Name = "TextBoxInvoiveMemo";
            TextBoxInvoiveMemo.Size = new Size(320, 39);
            TextBoxInvoiveMemo.TabIndex = 4;
            // 
            // DataGridViewInvoiceDetailsTotal
            // 
            DataGridViewInvoiceDetailsTotal.BackgroundColor = SystemColors.Control;
            DataGridViewInvoiceDetailsTotal.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridViewInvoiceDetailsTotal.ColumnHeadersVisible = false;
            DataGridViewInvoiceDetailsTotal.Columns.AddRange(new DataGridViewColumn[] { Total, Value, delete1 });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            DataGridViewInvoiceDetailsTotal.DefaultCellStyle = dataGridViewCellStyle3;
            DataGridViewInvoiceDetailsTotal.Enabled = false;
            DataGridViewInvoiceDetailsTotal.Location = new Point(11, 339);
            DataGridViewInvoiceDetailsTotal.Name = "DataGridViewInvoiceDetailsTotal";
            DataGridViewInvoiceDetailsTotal.ReadOnly = true;
            DataGridViewInvoiceDetailsTotal.RowHeadersVisible = false;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.ActiveCaptionText;
            DataGridViewInvoiceDetailsTotal.RowsDefaultCellStyle = dataGridViewCellStyle4;
            DataGridViewInvoiceDetailsTotal.Size = new Size(793, 25);
            DataGridViewInvoiceDetailsTotal.TabIndex = 17;
            DataGridViewInvoiceDetailsTotal.TabStop = false;
            // 
            // Total
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Total.DefaultCellStyle = dataGridViewCellStyle1;
            Total.HeaderText = "Total";
            Total.Name = "Total";
            Total.ReadOnly = true;
            Total.Width = 617;
            // 
            // Value
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Value.DefaultCellStyle = dataGridViewCellStyle2;
            Value.HeaderText = "Value";
            Value.Name = "Value";
            Value.ReadOnly = true;
            Value.Width = 131;
            // 
            // delete1
            // 
            delete1.HeaderText = "";
            delete1.Name = "delete1";
            delete1.ReadOnly = true;
            delete1.Width = 25;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // TextBoxInvoiceDueDate
            // 
            TextBoxInvoiceDueDate.BackColor = SystemColors.Window;
            TextBoxInvoiceDueDate.Location = new Point(111, 134);
            TextBoxInvoiceDueDate.Mask = "99/99/9999";
            TextBoxInvoiceDueDate.Name = "TextBoxInvoiceDueDate";
            TextBoxInvoiceDueDate.ReadOnly = true;
            TextBoxInvoiceDueDate.Size = new Size(93, 21);
            TextBoxInvoiceDueDate.TabIndex = 4;
            TextBoxInvoiceDueDate.TabStop = false;
            TextBoxInvoiceDueDate.ValidatingType = typeof(DateTime);
            // 
            // TextBoxInvoiceId
            // 
            TextBoxInvoiceId.Location = new Point(217, 580);
            TextBoxInvoiceId.Margin = new Padding(2);
            TextBoxInvoiceId.Name = "TextBoxInvoiceId";
            TextBoxInvoiceId.Size = new Size(79, 21);
            TextBoxInvoiceId.TabIndex = 54;
            TextBoxInvoiceId.Visible = false;
            // 
            // BtnInvoiceSave
            // 
            BtnInvoiceSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnInvoiceSave.Location = new Point(1067, 578);
            BtnInvoiceSave.Name = "BtnInvoiceSave";
            BtnInvoiceSave.Size = new Size(83, 23);
            BtnInvoiceSave.TabIndex = 8;
            BtnInvoiceSave.Text = "Save [F8]";
            BtnInvoiceSave.UseVisualStyleBackColor = true;
            BtnInvoiceSave.Click += BtnInvoiceSave_Click;
            BtnInvoiceSave.PreviewKeyDown += BtnInvoiceSave_PreviewKeyDown;
            // 
            // BtnInvoiceCancel
            // 
            BtnInvoiceCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnInvoiceCancel.Location = new Point(975, 578);
            BtnInvoiceCancel.Name = "BtnInvoiceCancel";
            BtnInvoiceCancel.Size = new Size(83, 23);
            BtnInvoiceCancel.TabIndex = 8;
            BtnInvoiceCancel.Text = "Cancel [Esc]";
            BtnInvoiceCancel.UseVisualStyleBackColor = true;
            BtnInvoiceCancel.Click += BtnInvoiceCancel_Click;
            // 
            // BtnInvoicePrint
            // 
            BtnInvoicePrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnInvoicePrint.Location = new Point(886, 578);
            BtnInvoicePrint.Name = "BtnInvoicePrint";
            BtnInvoicePrint.Size = new Size(83, 23);
            BtnInvoicePrint.TabIndex = 9;
            BtnInvoicePrint.Text = "Print [F9]";
            BtnInvoicePrint.UseVisualStyleBackColor = true;
            BtnInvoicePrint.Click += BtnInvoicePrint_Click;
            // 
            // BtnInvoiceDelete
            // 
            BtnInvoiceDelete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnInvoiceDelete.Location = new Point(119, 578);
            BtnInvoiceDelete.Name = "BtnInvoiceDelete";
            BtnInvoiceDelete.Size = new Size(83, 23);
            BtnInvoiceDelete.TabIndex = 11;
            BtnInvoiceDelete.Text = "Delete [F4]";
            BtnInvoiceDelete.UseVisualStyleBackColor = true;
            BtnInvoiceDelete.Click += BtnInvoiceDelete_Click;
            // 
            // BtnInvoiceNew
            // 
            BtnInvoiceNew.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnInvoiceNew.Location = new Point(30, 578);
            BtnInvoiceNew.Name = "BtnInvoiceNew";
            BtnInvoiceNew.Size = new Size(83, 23);
            BtnInvoiceNew.TabIndex = 12;
            BtnInvoiceNew.Text = "New [F3]";
            BtnInvoiceNew.UseVisualStyleBackColor = true;
            BtnInvoiceNew.Click += BtnInvoiceNew_Click;
            // 
            // ImageListInvoice
            // 
            ImageListInvoice.ColorDepth = ColorDepth.Depth8Bit;
            ImageListInvoice.ImageStream = (ImageListStreamer)resources.GetObject("ImageListInvoice.ImageStream");
            ImageListInvoice.TransparentColor = Color.Transparent;
            ImageListInvoice.Images.SetKeyName(0, "accounts1.png");
            ImageListInvoice.Images.SetKeyName(1, "Supplier3.ico");
            ImageListInvoice.Images.SetKeyName(2, "customers.ico");
            ImageListInvoice.Images.SetKeyName(3, "employee.png");
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ToolStripStatusLabelErrorInvoice });
            statusStrip1.Location = new Point(0, 609);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1243, 22);
            statusStrip1.TabIndex = 55;
            statusStrip1.Text = "sdfdsf sdf sdf";
            // 
            // ToolStripStatusLabelErrorInvoice
            // 
            ToolStripStatusLabelErrorInvoice.Name = "ToolStripStatusLabelErrorInvoice";
            ToolStripStatusLabelErrorInvoice.Size = new Size(151, 17);
            ToolStripStatusLabelErrorInvoice.Text = "                                                ";
            // 
            // TimerInvoice
            // 
            TimerInvoice.Interval = 400;
            TimerInvoice.Tick += TimerInvoice_Tick;
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = SystemColors.ControlLight;
            toolStrip1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, TextBoxInvoiceSearch, InvoiceSearchGo });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new Padding(5);
            toolStrip1.Size = new Size(1243, 32);
            toolStrip1.TabIndex = 68;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(78, 19);
            toolStripLabel1.Text = "Search Invoice";
            // 
            // TextBoxInvoiceSearch
            // 
            TextBoxInvoiceSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxInvoiceSearch.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxInvoiceSearch.MaxLength = 30;
            TextBoxInvoiceSearch.Name = "TextBoxInvoiceSearch";
            TextBoxInvoiceSearch.Size = new Size(200, 22);
            TextBoxInvoiceSearch.KeyDown += TextBoxInvoiceSearch_KeyDown;
            TextBoxInvoiceSearch.TextChanged += TextBoxInvoiceSearch_TextChanged;
            // 
            // InvoiceSearchGo
            // 
            InvoiceSearchGo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            InvoiceSearchGo.ImageTransparentColor = Color.Magenta;
            InvoiceSearchGo.Name = "InvoiceSearchGo";
            InvoiceSearchGo.Size = new Size(24, 19);
            InvoiceSearchGo.Text = "Go";
            InvoiceSearchGo.Click += InvoiceSearchGo_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(813, 33);
            label9.Name = "label9";
            label9.Size = new Size(84, 13);
            label9.TabIndex = 69;
            label9.Text = "Recent Invoices";
            // 
            // BtnReceivePayment
            // 
            BtnReceivePayment.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnReceivePayment.Location = new Point(763, 578);
            BtnReceivePayment.Name = "BtnReceivePayment";
            BtnReceivePayment.Size = new Size(117, 23);
            BtnReceivePayment.TabIndex = 10;
            BtnReceivePayment.Text = "Receive Payment";
            BtnReceivePayment.UseVisualStyleBackColor = true;
            BtnReceivePayment.Click += BtnReceivePayment_Click;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewTextBoxColumn1.HeaderText = "Total";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            dataGridViewTextBoxColumn1.Width = 639;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewTextBoxColumn2.HeaderText = "Value";
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            dataGridViewTextBoxColumn2.Width = 108;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "";
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            dataGridViewTextBoxColumn3.Width = 25;
            // 
            // DateTimePickerInvoiceDate
            // 
            DateTimePickerInvoiceDate.BackColor = Color.White;
            DateTimePickerInvoiceDate.BorderStyle = BorderStyle.FixedSingle;
            DateTimePickerInvoiceDate.Date = null;
            DateTimePickerInvoiceDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DateTimePickerInvoiceDate.Format = "MM/dd/yy";
            DateTimePickerInvoiceDate.Location = new Point(12, 134);
            DateTimePickerInvoiceDate.MaxDate = new DateTime(9997, 12, 31, 8, 13, 58, 0);
            DateTimePickerInvoiceDate.MinDate = new DateTime(1900, 1, 1, 23, 5, 35, 0);
            DateTimePickerInvoiceDate.Name = "DateTimePickerInvoiceDate";
            DateTimePickerInvoiceDate.ReadOnly = false;
            DateTimePickerInvoiceDate.Size = new Size(93, 21);
            DateTimePickerInvoiceDate.TabIndex = 3;
            DateTimePickerInvoiceDate.Leave += DateTimePickerInvoiceDate_Leave;
            // 
            // ComboBoxInvoiceTerms
            // 
            ComboBoxInvoiceTerms.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxInvoiceTerms.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxInvoiceTerms.FormattingEnabled = true;
            ComboBoxInvoiceTerms.Location = new Point(11, 92);
            ComboBoxInvoiceTerms.Name = "ComboBoxInvoiceTerms";
            ComboBoxInvoiceTerms.Size = new Size(153, 21);
            ComboBoxInvoiceTerms.TabIndex = 2;
            ComboBoxInvoiceTerms.TxtVisible = true;
            ComboBoxInvoiceTerms.SelectedIndexChanged += ComboBoxInvoiceTerms_SelectedIndexChanged;
            ComboBoxInvoiceTerms.KeyPress += ComboBoxInvoiceTerms_KeyPress;
            // 
            // GridViewInvoiceSearch
            // 
            GridViewInvoiceSearch.AllowUserToAddRows = false;
            GridViewInvoiceSearch.AllowUserToDeleteRows = false;
            GridViewInvoiceSearch.AllowUserToResizeColumns = false;
            GridViewInvoiceSearch.AllowUserToResizeRows = false;
            GridViewInvoiceSearch.BackgroundColor = SystemColors.ControlLight;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = SystemColors.ControlLight;
            dataGridViewCellStyle7.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle7.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = SystemColors.ControlLight;
            dataGridViewCellStyle7.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            GridViewInvoiceSearch.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            GridViewInvoiceSearch.ColumnHeadersHeight = 20;
            GridViewInvoiceSearch.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewInvoiceSearch.Columns.AddRange(new DataGridViewColumn[] { OpenInvoiceNumber, OpenInvDescription, OpenInvAmount, BalAmount, Type });
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = SystemColors.Window;
            dataGridViewCellStyle12.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle12.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle12.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = DataGridViewTriState.True;
            GridViewInvoiceSearch.DefaultCellStyle = dataGridViewCellStyle12;
            GridViewInvoiceSearch.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewInvoiceSearch.EnableHeadersVisualStyles = false;
            GridViewInvoiceSearch.Location = new Point(815, 50);
            GridViewInvoiceSearch.MultiSelect = false;
            GridViewInvoiceSearch.Name = "GridViewInvoiceSearch";
            GridViewInvoiceSearch.ReadOnly = true;
            GridViewInvoiceSearch.RowHeadersVisible = false;
            dataGridViewCellStyle13.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = Color.White;
            dataGridViewCellStyle13.WrapMode = DataGridViewTriState.True;
            GridViewInvoiceSearch.RowsDefaultCellStyle = dataGridViewCellStyle13;
            GridViewInvoiceSearch.RowTemplate.Height = 20;
            GridViewInvoiceSearch.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewInvoiceSearch.ShowCellToolTips = false;
            GridViewInvoiceSearch.Size = new Size(417, 314);
            GridViewInvoiceSearch.TabIndex = 67;
            GridViewInvoiceSearch.TabStop = false;
            GridViewInvoiceSearch.CellDoubleClick += GridViewInvoiceSearch_CellDoubleClick;
            GridViewInvoiceSearch.CellPainting += GridViewInvoiceSearch_CellPainting;
            GridViewInvoiceSearch.KeyDown += GridViewInvoiceSearch_KeyDown;
            // 
            // OpenInvoiceNumber
            // 
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
            OpenInvoiceNumber.DefaultCellStyle = dataGridViewCellStyle8;
            OpenInvoiceNumber.HeaderText = "Inv. #";
            OpenInvoiceNumber.Name = "OpenInvoiceNumber";
            OpenInvoiceNumber.ReadOnly = true;
            OpenInvoiceNumber.SortMode = DataGridViewColumnSortMode.NotSortable;
            OpenInvoiceNumber.Width = 70;
            // 
            // OpenInvDescription
            // 
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            OpenInvDescription.DefaultCellStyle = dataGridViewCellStyle9;
            OpenInvDescription.HeaderText = "Date";
            OpenInvDescription.Name = "OpenInvDescription";
            OpenInvDescription.ReadOnly = true;
            OpenInvDescription.SortMode = DataGridViewColumnSortMode.NotSortable;
            OpenInvDescription.Width = 70;
            // 
            // OpenInvAmount
            // 
            OpenInvAmount.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.True;
            OpenInvAmount.DefaultCellStyle = dataGridViewCellStyle10;
            OpenInvAmount.HeaderText = "Customer";
            OpenInvAmount.Name = "OpenInvAmount";
            OpenInvAmount.ReadOnly = true;
            OpenInvAmount.SortMode = DataGridViewColumnSortMode.NotSortable;
            OpenInvAmount.Width = 174;
            // 
            // BalAmount
            // 
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.True;
            BalAmount.DefaultCellStyle = dataGridViewCellStyle11;
            BalAmount.HeaderText = "Amount";
            BalAmount.Name = "BalAmount";
            BalAmount.ReadOnly = true;
            BalAmount.SortMode = DataGridViewColumnSortMode.NotSortable;
            BalAmount.Width = 85;
            // 
            // Type
            // 
            Type.HeaderText = "Id";
            Type.Name = "Type";
            Type.ReadOnly = true;
            Type.SortMode = DataGridViewColumnSortMode.NotSortable;
            Type.Visible = false;
            // 
            // DataGridViewInvoiceDetails
            // 
            DataGridViewInvoiceDetails.AllowUserToResizeColumns = false;
            DataGridViewInvoiceDetails.AllowUserToResizeRows = false;
            DataGridViewInvoiceDetails.BackgroundColor = SystemColors.ControlLight;
            dataGridViewCellStyle14.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = SystemColors.ControlLight;
            dataGridViewCellStyle14.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle14.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = DataGridViewTriState.True;
            DataGridViewInvoiceDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
            DataGridViewInvoiceDetails.ColumnHeadersHeight = 20;
            DataGridViewInvoiceDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DataGridViewInvoiceDetails.Columns.AddRange(new DataGridViewColumn[] { InvoiceSerialNo, ProductOrService, Description, Rate, Quantity, Amount, Delete });
            DataGridViewInvoiceDetails.EditMode = DataGridViewEditMode.EditOnEnter;
            DataGridViewInvoiceDetails.EnableHeadersVisualStyles = false;
            DataGridViewInvoiceDetails.Location = new Point(11, 160);
            DataGridViewInvoiceDetails.Name = "DataGridViewInvoiceDetails";
            DataGridViewInvoiceDetails.RowHeadersVisible = false;
            dataGridViewCellStyle20.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle20.SelectionForeColor = SystemColors.ActiveCaptionText;
            DataGridViewInvoiceDetails.RowsDefaultCellStyle = dataGridViewCellStyle20;
            DataGridViewInvoiceDetails.RowTemplate.Height = 20;
            DataGridViewInvoiceDetails.ScrollBars = ScrollBars.Vertical;
            DataGridViewInvoiceDetails.ShowCellToolTips = false;
            DataGridViewInvoiceDetails.Size = new Size(793, 180);
            DataGridViewInvoiceDetails.TabIndex = 6;
            DataGridViewInvoiceDetails.CellClick += DataGridViewInvoiceDetails_CellClick;
            DataGridViewInvoiceDetails.CellEndEdit += DataGridViewInvoiceDetails_CellEndEdit;
            DataGridViewInvoiceDetails.CellEnter += DataGridViewInvoiceDetails_CellEnter;
            DataGridViewInvoiceDetails.CellFormatting += DataGridViewInvoiceDetails_CellFormatting;
            DataGridViewInvoiceDetails.CellLeave += DataGridViewInvoiceDetails_CellLeave;
            DataGridViewInvoiceDetails.DataError += DataGridViewInvoiceDetails_DataError;
            DataGridViewInvoiceDetails.EditingControlShowing += DataGridViewInvoiceDetails_EditingControlShowing;
            DataGridViewInvoiceDetails.RowsAdded += DataGridViewInvoiceDetails_RowsAdded;
            // 
            // InvoiceSerialNo
            // 
            InvoiceSerialNo.HeaderText = "#";
            InvoiceSerialNo.Name = "InvoiceSerialNo";
            InvoiceSerialNo.ReadOnly = true;
            InvoiceSerialNo.Resizable = DataGridViewTriState.False;
            InvoiceSerialNo.SortMode = DataGridViewColumnSortMode.NotSortable;
            InvoiceSerialNo.Width = 30;
            // 
            // ProductOrService
            // 
            dataGridViewCellStyle15.WrapMode = DataGridViewTriState.True;
            ProductOrService.DefaultCellStyle = dataGridViewCellStyle15;
            ProductOrService.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            ProductOrService.FlatStyle = FlatStyle.Flat;
            ProductOrService.HeaderText = "Product / Service";
            ProductOrService.Name = "ProductOrService";
            ProductOrService.Resizable = DataGridViewTriState.False;
            ProductOrService.Width = 200;
            // 
            // Description
            // 
            Description.HeaderText = "Description";
            Description.MaxInputLength = 250;
            Description.Name = "Description";
            Description.Resizable = DataGridViewTriState.False;
            Description.SortMode = DataGridViewColumnSortMode.NotSortable;
            Description.Width = 229;
            // 
            // Rate
            // 
            dataGridViewCellStyle16.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle16.NullValue = "0.00";
            Rate.DefaultCellStyle = dataGridViewCellStyle16;
            Rate.HeaderText = "Rate";
            Rate.Name = "Rate";
            Rate.Resizable = DataGridViewTriState.False;
            Rate.Width = 108;
            // 
            // Quantity
            // 
            dataGridViewCellStyle17.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle17.NullValue = "0";
            Quantity.DefaultCellStyle = dataGridViewCellStyle17;
            Quantity.HeaderText = "QTY";
            Quantity.Name = "Quantity";
            Quantity.NumberLength = 6;
            Quantity.Resizable = DataGridViewTriState.False;
            Quantity.Width = 50;
            // 
            // Amount
            // 
            dataGridViewCellStyle18.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle18.NullValue = "0.00";
            Amount.DefaultCellStyle = dataGridViewCellStyle18;
            Amount.HeaderText = "Amount";
            Amount.Name = "Amount";
            Amount.ReadOnly = true;
            Amount.Resizable = DataGridViewTriState.False;
            Amount.Width = 131;
            // 
            // Delete
            // 
            dataGridViewCellStyle19.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle19.NullValue = "X";
            Delete.DefaultCellStyle = dataGridViewCellStyle19;
            Delete.HeaderText = "...";
            Delete.Name = "Delete";
            Delete.Resizable = DataGridViewTriState.False;
            Delete.Width = 25;
            // 
            // TextBoxInvoiveInternalMemo
            // 
            TextBoxInvoiveInternalMemo.Location = new Point(324, 109);
            TextBoxInvoiveInternalMemo.MaxLength = 250;
            TextBoxInvoiveInternalMemo.Multiline = true;
            TextBoxInvoiveInternalMemo.Name = "TextBoxInvoiveInternalMemo";
            TextBoxInvoiveInternalMemo.Size = new Size(320, 46);
            TextBoxInvoiveInternalMemo.TabIndex = 5;
            TextBoxInvoiveInternalMemo.KeyPress += TextBoxInvoiveInternalMemo_KeyPress;
            TextBoxInvoiveInternalMemo.PreviewKeyDown += TextBoxInvoiveInternalMemo_PreviewKeyDown;
            // 
            // BtnInvoiceExit
            // 
            BtnInvoiceExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnInvoiceExit.Location = new Point(1157, 578);
            BtnInvoiceExit.Name = "BtnInvoiceExit";
            BtnInvoiceExit.Size = new Size(75, 23);
            BtnInvoiceExit.TabIndex = 12;
            BtnInvoiceExit.Text = "Exit [F10]";
            BtnInvoiceExit.UseVisualStyleBackColor = true;
            BtnInvoiceExit.Click += BtnInvoiceExit_Click;
            // 
            // BtnInvoiceSearchCustomer
            // 
            BtnInvoiceSearchCustomer.Location = new Point(170, 49);
            BtnInvoiceSearchCustomer.Name = "BtnInvoiceSearchCustomer";
            BtnInvoiceSearchCustomer.Size = new Size(71, 23);
            BtnInvoiceSearchCustomer.TabIndex = 192;
            BtnInvoiceSearchCustomer.TabStop = false;
            BtnInvoiceSearchCustomer.Text = "Search [F2]";
            BtnInvoiceSearchCustomer.UseVisualStyleBackColor = true;
            BtnInvoiceSearchCustomer.Click += BtnInvoiceSearchCustomer_Click;
            // 
            // TextBoxInvoiceCustomer
            // 
            TextBoxInvoiceCustomer.BackColor = SystemColors.Window;
            TextBoxInvoiceCustomer.Id = null;
            TextBoxInvoiceCustomer.Location = new Point(11, 50);
            TextBoxInvoiceCustomer.MaxLength = 30;
            TextBoxInvoiceCustomer.Name = "TextBoxInvoiceCustomer";
            TextBoxInvoiceCustomer.ReadOnly = true;
            TextBoxInvoiceCustomer.Size = new Size(153, 21);
            TextBoxInvoiceCustomer.TabIndex = 1;
            TextBoxInvoiceCustomer.PreviewKeyDown += TextBoxInvoiceCustomer_PreviewKeyDown;
            // 
            // BtnInvoiceNewCustomers
            // 
            BtnInvoiceNewCustomers.ButtonText = "New [F3]";
            BtnInvoiceNewCustomers.ImageList = ImageListInvoice;
            BtnInvoiceNewCustomers.Items = (System.Collections.ObjectModel.Collection<string>)resources.GetObject("BtnInvoiceNewCustomers.Items");
            BtnInvoiceNewCustomers.Location = new Point(244, 49);
            BtnInvoiceNewCustomers.Margin = new Padding(4, 3, 4, 3);
            BtnInvoiceNewCustomers.Name = "BtnInvoiceNewCustomers";
            BtnInvoiceNewCustomers.Size = new Size(83, 27);
            BtnInvoiceNewCustomers.TabIndex = 196;
            BtnInvoiceNewCustomers.TabStop = false;
            BtnInvoiceNewCustomers.ItemClickedEvent += BtnInvoiceNewCustomers_ItemClickedEvent;
            // 
            // DiscountAdditinalChargeGrid
            // 
            DiscountAdditinalChargeGrid.AdditionalTransactions = null;
            DiscountAdditinalChargeGrid.GridType = controls.accounting.GridType.Purchase;
            DiscountAdditinalChargeGrid.HeaderText = "Discount/Additonal Charges";
            DiscountAdditinalChargeGrid.InputAmount = 0D;
            DiscountAdditinalChargeGrid.Location = new Point(10, 366);
            DiscountAdditinalChargeGrid.Margin = new Padding(4, 3, 4, 3);
            DiscountAdditinalChargeGrid.Name = "DiscountAdditinalChargeGrid";
            DiscountAdditinalChargeGrid.OutputAmount = 0D;
            DiscountAdditinalChargeGrid.Quantity = 0D;
            DiscountAdditinalChargeGrid.Size = new Size(1220, 177);
            DiscountAdditinalChargeGrid.TabIndex = 7;
            DiscountAdditinalChargeGrid.Load += DiscountAdditinalChargeGrid_Load;
            DiscountAdditinalChargeGrid.TabIndexChanged += DiscountAdditinalChargeGrid_TabIndexChanged;
            DiscountAdditinalChargeGrid.PreviewKeyDown += DiscountAdditinalChargeGrid_PreviewKeyDown;
            // 
            // LabelInvoiceFinalAmount
            // 
            LabelInvoiceFinalAmount.BorderStyle = BorderStyle.FixedSingle;
            LabelInvoiceFinalAmount.Font = new Font("Tahoma", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelInvoiceFinalAmount.Location = new Point(1109, 545);
            LabelInvoiceFinalAmount.Name = "LabelInvoiceFinalAmount";
            LabelInvoiceFinalAmount.Size = new Size(122, 25);
            LabelInvoiceFinalAmount.TabIndex = 209;
            LabelInvoiceFinalAmount.Text = "0.00";
            LabelInvoiceFinalAmount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.Font = new Font("Tahoma", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(962, 545);
            label3.Name = "label3";
            label3.Size = new Size(141, 23);
            label3.TabIndex = 208;
            label3.Text = "Total Amount";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label10.Location = new Point(648, 76);
            label10.Name = "label10";
            label10.Size = new Size(73, 13);
            label10.TabIndex = 5;
            label10.Text = "last Invoice #";
            // 
            // LastInvoiceRefNo
            // 
            LastInvoiceRefNo.AutoSize = true;
            LastInvoiceRefNo.Font = new Font("Tahoma", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            LastInvoiceRefNo.Location = new Point(648, 90);
            LastInvoiceRefNo.Name = "LastInvoiceRefNo";
            LastInvoiceRefNo.Size = new Size(55, 16);
            LastInvoiceRefNo.TabIndex = 0;
            LastInvoiceRefNo.Text = "000000";
            // 
            // FormInvoice
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1243, 631);
            Controls.Add(ComboBoxInvoiceTerms);
            Controls.Add(LabelInvoiceFinalAmount);
            Controls.Add(label3);
            Controls.Add(DiscountAdditinalChargeGrid);
            Controls.Add(TextBoxInvoiveMemo);
            Controls.Add(BtnInvoiceNewCustomers);
            Controls.Add(BtnInvoiceSearchCustomer);
            Controls.Add(TextBoxInvoiceCustomer);
            Controls.Add(BtnInvoiceExit);
            Controls.Add(TextBoxInvoiveInternalMemo);
            Controls.Add(DateTimePickerInvoiceDate);
            Controls.Add(BtnReceivePayment);
            Controls.Add(toolStrip1);
            Controls.Add(GridViewInvoiceSearch);
            Controls.Add(statusStrip1);
            Controls.Add(TextBoxInvoiceId);
            Controls.Add(BtnInvoiceSave);
            Controls.Add(BtnInvoiceCancel);
            Controls.Add(BtnInvoicePrint);
            Controls.Add(BtnInvoiceDelete);
            Controls.Add(BtnInvoiceNew);
            Controls.Add(TextBoxInvoiceDueDate);
            Controls.Add(DataGridViewInvoiceDetailsTotal);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(LastInvoiceRefNo);
            Controls.Add(InvoiceRefNo);
            Controls.Add(label10);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(DataGridViewInvoiceDetails);
            Controls.Add(label9);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormInvoice";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Invoice";
            FormClosing += FormInvoice_FormClosing;
            Load += FormInvoice_Load;
            Controls.SetChildIndex(label9, 0);
            Controls.SetChildIndex(DataGridViewInvoiceDetails, 0);
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(label2, 0);
            Controls.SetChildIndex(label10, 0);
            Controls.SetChildIndex(InvoiceRefNo, 0);
            Controls.SetChildIndex(LastInvoiceRefNo, 0);
            Controls.SetChildIndex(label4, 0);
            Controls.SetChildIndex(label5, 0);
            Controls.SetChildIndex(label6, 0);
            Controls.SetChildIndex(label7, 0);
            Controls.SetChildIndex(label8, 0);
            Controls.SetChildIndex(DataGridViewInvoiceDetailsTotal, 0);
            Controls.SetChildIndex(TextBoxInvoiceDueDate, 0);
            Controls.SetChildIndex(BtnInvoiceNew, 0);
            Controls.SetChildIndex(BtnInvoiceDelete, 0);
            Controls.SetChildIndex(BtnInvoicePrint, 0);
            Controls.SetChildIndex(BtnInvoiceCancel, 0);
            Controls.SetChildIndex(BtnInvoiceSave, 0);
            Controls.SetChildIndex(TextBoxInvoiceId, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(GridViewInvoiceSearch, 0);
            Controls.SetChildIndex(toolStrip1, 0);
            Controls.SetChildIndex(BtnReceivePayment, 0);
            Controls.SetChildIndex(DateTimePickerInvoiceDate, 0);
            Controls.SetChildIndex(TextBoxInvoiveInternalMemo, 0);
            Controls.SetChildIndex(BtnInvoiceExit, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(TextBoxInvoiceCustomer, 0);
            Controls.SetChildIndex(BtnInvoiceSearchCustomer, 0);
            Controls.SetChildIndex(BtnInvoiceNewCustomers, 0);
            Controls.SetChildIndex(TextBoxInvoiveMemo, 0);
            Controls.SetChildIndex(DiscountAdditinalChargeGrid, 0);
            Controls.SetChildIndex(label3, 0);
            Controls.SetChildIndex(LabelInvoiceFinalAmount, 0);
            Controls.SetChildIndex(ComboBoxInvoiceTerms, 0);
            ((System.ComponentModel.ISupportInitialize)DataGridViewInvoiceDetailsTotal).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewInvoiceSearch).EndInit();
            ((System.ComponentModel.ISupportInitialize)DataGridViewInvoiceDetails).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private fa.views.controls.DataViewVerticalScroll DataGridViewInvoiceDetails;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label InvoiceRefNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TextBoxInvoiveMemo;
        private System.Windows.Forms.DataGridView DataGridViewInvoiceDetailsTotal;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        public System.Windows.Forms.MaskedTextBox TextBoxInvoiceDueDate;
        private System.Windows.Forms.TextBox TextBoxInvoiceId;
        private System.Windows.Forms.Button BtnInvoiceSave;
        private System.Windows.Forms.Button BtnInvoiceCancel;
        private System.Windows.Forms.Button BtnInvoicePrint;
        private System.Windows.Forms.Button BtnInvoiceDelete;
        private System.Windows.Forms.Button BtnInvoiceNew;
        private System.Windows.Forms.ImageList ImageListInvoice;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabelErrorInvoice;
        private System.Windows.Forms.Timer TimerInvoice;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private controls.DataViewVerticalScroll GridViewInvoiceSearch;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox TextBoxInvoiceSearch;
        private System.Windows.Forms.ToolStripButton InvoiceSearchGo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button BtnReceivePayment;
        private controls.ComboBoxSwapTextBox ComboBoxInvoiceTerms;
        private System.Windows.Forms.DataGridViewTextBoxColumn OpenInvoiceNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn OpenInvDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn OpenInvAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn BalAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private controls.text.DateWithCalendar DateTimePickerInvoiceDate;
        private System.Windows.Forms.TextBox TextBoxInvoiveInternalMemo;
        private System.Windows.Forms.Button BtnInvoiceExit;
        private System.Windows.Forms.Button BtnInvoiceSearchCustomer;
        private controls.text.IDTextBox TextBoxInvoiceCustomer;
        private Dropdown_Button.UserControlButtonWithMenu BtnInvoiceNewCustomers;
        private DataGridViewTextBoxColumn Total;
        private DataGridViewTextBoxColumn Value;
        private DataGridViewTextBoxColumn delete1;
        private DataGridViewTextBoxColumn InvoiceSerialNo;
        private DataGridViewComboBoxColumn ProductOrService;
        private DataGridViewTextBoxColumn Description;
        private controls.grid.DataGridViewCurrencyColumn Rate;
        private controls.grid.DataGridViewNumberColumn Quantity;
        private controls.grid.DataGridViewCurrencyColumn Amount;
        private DataGridViewButtonColumn Delete;
        private controls.accounting.DiscountAdditinalChargeGrid DiscountAdditinalChargeGrid;
        private Label LabelInvoiceFinalAmount;
        private Label label3;
        private Label label10;
        private Label label11;
        private Label LastInvoiceRefNo;
    }
}