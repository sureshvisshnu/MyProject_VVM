namespace fa.views.account.transactions
{
    partial class FormBill
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
            Label label9;
            DataGridViewCellStyle dataGridViewCellStyle26 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle27 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle24 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle25 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBill));
            DataGridViewCellStyle dataGridViewCellStyle28 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle33 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle34 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle29 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle30 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle31 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle32 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle35 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle40 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle36 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle37 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle38 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle39 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle41 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle42 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle43 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle44 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle45 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle46 = new DataGridViewCellStyle();
            OpenFileDialogBill = new OpenFileDialog();
            BtnBillDelete = new Button();
            BtnBillNew = new Button();
            BtnBillPrint = new Button();
            BtnBillCancel = new Button();
            BtnBillSave = new Button();
            DataGridViewBillDetailsTotal = new DataGridView();
            Total = new DataGridViewTextBoxColumn();
            Value = new DataGridViewTextBoxColumn();
            delete1 = new DataGridViewTextBoxColumn();
            TextBoxBillInternalMemo = new TextBox();
            TextBoxBillMemo = new TextBox();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            BillReferenceNumber = new Label();
            label2 = new Label();
            label1 = new Label();
            BtnBillAttachment = new Button();
            TextBoxBillDueDate = new MaskedTextBox();
            TextBoxBillId = new TextBox();
            TextBoxBillFilePath = new TextBox();
            ImageListBill = new ImageList(components);
            BtnBillFileDelete = new Button();
            BtnBillFileDownload = new Button();
            SaveFileDialogBill = new SaveFileDialog();
            StatusStripBill = new StatusStrip();
            ToolStripStatusLabelErrorBill = new ToolStripStatusLabel();
            TimerBill = new System.Windows.Forms.Timer(components);
            label3 = new Label();
            toolStrip1 = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            TextBoxBillSearch = new ToolStripTextBox();
            BillSearchGo = new ToolStripButton();
            ComboBoxBillTerm = new controls.ComboBoxSwapTextBox();
            GridViewBillSearch = new controls.DataViewVerticalScroll();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn10 = new DataGridViewTextBoxColumn();
            DataGridViewBillDetails = new controls.DataViewVerticalScroll();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            Account = new DataGridViewComboBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            Amount = new controls.grid.DataGridViewCurrencyColumn();
            Delete = new DataGridViewButtonColumn();
            DateTimePickerBillDate = new controls.text.DateWithCalendar();
            BtnBillExit = new Button();
            BtnPayablePayment = new Button();
            TextBoxBillSupplier = new controls.text.IDTextBox();
            BtnBillSearchSupplier = new Button();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            OpenInvoiceNumber = new DataGridViewTextBoxColumn();
            OpenInvDescription = new DataGridViewTextBoxColumn();
            OpenInvAmount = new DataGridViewTextBoxColumn();
            BalAmount = new DataGridViewTextBoxColumn();
            Type = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            Description = new DataGridViewTextBoxColumn();
            BtnBillNewSupplier = new Dropdown_Button.UserControlButtonWithMenu();
            LableReferenceNumber = new Label();
            LastBillReferenceNumber = new Label();
            label9 = new Label();
            ((System.ComponentModel.ISupportInitialize)DataGridViewBillDetailsTotal).BeginInit();
            StatusStripBill.SuspendLayout();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewBillSearch).BeginInit();
            ((System.ComponentModel.ISupportInitialize)DataGridViewBillDetails).BeginInit();
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
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(664, 121);
            label9.Name = "label9";
            label9.Size = new Size(28, 13);
            label9.TabIndex = 45;
            label9.Text = "Files";
            label9.Visible = false;
            // 
            // OpenFileDialogBill
            // 
            OpenFileDialogBill.FileName = "openFileDialog1";
            // 
            // BtnBillDelete
            // 
            BtnBillDelete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnBillDelete.Location = new Point(105, 466);
            BtnBillDelete.Name = "BtnBillDelete";
            BtnBillDelete.Size = new Size(83, 23);
            BtnBillDelete.TabIndex = 11;
            BtnBillDelete.Text = "Delete [F4]";
            BtnBillDelete.UseVisualStyleBackColor = true;
            BtnBillDelete.Click += BtnBillDelete_Click;
            // 
            // BtnBillNew
            // 
            BtnBillNew.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnBillNew.Location = new Point(16, 466);
            BtnBillNew.Name = "BtnBillNew";
            BtnBillNew.Size = new Size(83, 23);
            BtnBillNew.TabIndex = 0;
            BtnBillNew.Text = "New [F3]";
            BtnBillNew.UseVisualStyleBackColor = true;
            BtnBillNew.Click += BtnBillNew_Click;
            // 
            // BtnBillPrint
            // 
            BtnBillPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnBillPrint.Location = new Point(882, 466);
            BtnBillPrint.Name = "BtnBillPrint";
            BtnBillPrint.Size = new Size(83, 23);
            BtnBillPrint.TabIndex = 10;
            BtnBillPrint.Text = "Print [F9]";
            BtnBillPrint.UseVisualStyleBackColor = true;
            BtnBillPrint.Click += BtnBillPrint_Click;
            // 
            // BtnBillCancel
            // 
            BtnBillCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnBillCancel.Location = new Point(971, 466);
            BtnBillCancel.Name = "BtnBillCancel";
            BtnBillCancel.Size = new Size(83, 23);
            BtnBillCancel.TabIndex = 9;
            BtnBillCancel.Text = "Cancel [Esc]";
            BtnBillCancel.UseVisualStyleBackColor = true;
            BtnBillCancel.Click += BtnBillCancel_Click;
            BtnBillCancel.PreviewKeyDown += BtnBillCancel_PreviewKeyDown;
            // 
            // BtnBillSave
            // 
            BtnBillSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnBillSave.Location = new Point(1060, 466);
            BtnBillSave.Name = "BtnBillSave";
            BtnBillSave.Size = new Size(83, 23);
            BtnBillSave.TabIndex = 8;
            BtnBillSave.Text = "Save [F8]";
            BtnBillSave.UseVisualStyleBackColor = true;
            BtnBillSave.Click += BtnBillSave_Click;
            BtnBillSave.PreviewKeyDown += BtnBillSave_PreviewKeyDown;
            // 
            // DataGridViewBillDetailsTotal
            // 
            DataGridViewBillDetailsTotal.AllowUserToResizeColumns = false;
            DataGridViewBillDetailsTotal.AllowUserToResizeRows = false;
            DataGridViewBillDetailsTotal.BackgroundColor = SystemColors.Control;
            DataGridViewBillDetailsTotal.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridViewBillDetailsTotal.ColumnHeadersVisible = false;
            DataGridViewBillDetailsTotal.Columns.AddRange(new DataGridViewColumn[] { Total, Value, delete1 });
            dataGridViewCellStyle26.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle26.BackColor = SystemColors.Window;
            dataGridViewCellStyle26.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle26.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle26.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle26.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle26.WrapMode = DataGridViewTriState.False;
            DataGridViewBillDetailsTotal.DefaultCellStyle = dataGridViewCellStyle26;
            DataGridViewBillDetailsTotal.Enabled = false;
            DataGridViewBillDetailsTotal.Location = new Point(12, 435);
            DataGridViewBillDetailsTotal.Name = "DataGridViewBillDetailsTotal";
            DataGridViewBillDetailsTotal.ReadOnly = true;
            DataGridViewBillDetailsTotal.RowHeadersVisible = false;
            dataGridViewCellStyle27.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle27.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle27.SelectionForeColor = SystemColors.ActiveCaptionText;
            DataGridViewBillDetailsTotal.RowsDefaultCellStyle = dataGridViewCellStyle27;
            DataGridViewBillDetailsTotal.Size = new Size(793, 25);
            DataGridViewBillDetailsTotal.TabIndex = 38;
            DataGridViewBillDetailsTotal.TabStop = false;
            // 
            // Total
            // 
            dataGridViewCellStyle24.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle24.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Total.DefaultCellStyle = dataGridViewCellStyle24;
            Total.HeaderText = "Total";
            Total.Name = "Total";
            Total.ReadOnly = true;
            Total.Width = 638;
            // 
            // Value
            // 
            dataGridViewCellStyle25.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle25.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Value.DefaultCellStyle = dataGridViewCellStyle25;
            Value.HeaderText = "Value";
            Value.Name = "Value";
            Value.ReadOnly = true;
            Value.Width = 108;
            // 
            // delete1
            // 
            delete1.HeaderText = "";
            delete1.Name = "delete1";
            delete1.ReadOnly = true;
            delete1.Resizable = DataGridViewTriState.True;
            delete1.Width = 25;
            // 
            // TextBoxBillInternalMemo
            // 
            TextBoxBillInternalMemo.Location = new Point(343, 116);
            TextBoxBillInternalMemo.MaxLength = 250;
            TextBoxBillInternalMemo.Multiline = true;
            TextBoxBillInternalMemo.Name = "TextBoxBillInternalMemo";
            TextBoxBillInternalMemo.Size = new Size(309, 45);
            TextBoxBillInternalMemo.TabIndex = 5;
            // 
            // TextBoxBillMemo
            // 
            TextBoxBillMemo.Location = new Point(343, 54);
            TextBoxBillMemo.MaxLength = 250;
            TextBoxBillMemo.Multiline = true;
            TextBoxBillMemo.Name = "TextBoxBillMemo";
            TextBoxBillMemo.Size = new Size(309, 42);
            TextBoxBillMemo.TabIndex = 4;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(340, 100);
            label8.Name = "label8";
            label8.Size = new Size(76, 13);
            label8.TabIndex = 33;
            label8.Text = "Internal Memo";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(340, 38);
            label7.Name = "label7";
            label7.Size = new Size(35, 13);
            label7.TabIndex = 32;
            label7.Text = "Memo";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label6.Location = new Point(112, 120);
            label6.Name = "label6";
            label6.Size = new Size(59, 13);
            label6.TabIndex = 31;
            label6.Text = "Due Date";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label5.Location = new Point(9, 120);
            label5.Name = "label5";
            label5.Size = new Size(53, 13);
            label5.TabIndex = 30;
            label5.Text = "Bill Date";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(9, 78);
            label4.Name = "label4";
            label4.Size = new Size(43, 13);
            label4.TabIndex = 29;
            label4.Text = "Terms";
            // 
            // BillReferenceNumber
            // 
            BillReferenceNumber.AutoSize = true;
            BillReferenceNumber.Font = new Font("Tahoma", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            BillReferenceNumber.Location = new Point(661, 56);
            BillReferenceNumber.Name = "BillReferenceNumber";
            BillReferenceNumber.Size = new Size(55, 16);
            BillReferenceNumber.TabIndex = 28;
            BillReferenceNumber.Text = "000000";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(12, 37);
            label2.Name = "label2";
            label2.Size = new Size(53, 13);
            label2.TabIndex = 27;
            label2.Text = "Supplier";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(661, 38);
            label1.Name = "label1";
            label1.Size = new Size(68, 13);
            label1.TabIndex = 26;
            label1.Text = "Reference #";
            // 
            // BtnBillAttachment
            // 
            BtnBillAttachment.Location = new Point(698, 116);
            BtnBillAttachment.Name = "BtnBillAttachment";
            BtnBillAttachment.Size = new Size(75, 23);
            BtnBillAttachment.TabIndex = 6;
            BtnBillAttachment.Text = "Select Files";
            BtnBillAttachment.UseVisualStyleBackColor = true;
            BtnBillAttachment.Visible = false;
            BtnBillAttachment.Click += BtnBillAttachment_Click;
            BtnBillAttachment.PreviewKeyDown += BtnBillAttachment_PreviewKeyDown;
            // 
            // TextBoxBillDueDate
            // 
            TextBoxBillDueDate.BackColor = SystemColors.Window;
            TextBoxBillDueDate.Location = new Point(115, 137);
            TextBoxBillDueDate.Name = "TextBoxBillDueDate";
            TextBoxBillDueDate.ReadOnly = true;
            TextBoxBillDueDate.Size = new Size(93, 21);
            TextBoxBillDueDate.TabIndex = 50;
            TextBoxBillDueDate.TabStop = false;
            TextBoxBillDueDate.ValidatingType = typeof(DateTime);
            // 
            // TextBoxBillId
            // 
            TextBoxBillId.Location = new Point(311, 468);
            TextBoxBillId.Margin = new Padding(2);
            TextBoxBillId.Name = "TextBoxBillId";
            TextBoxBillId.Size = new Size(79, 21);
            TextBoxBillId.TabIndex = 52;
            TextBoxBillId.Visible = false;
            // 
            // TextBoxBillFilePath
            // 
            TextBoxBillFilePath.BorderStyle = BorderStyle.None;
            TextBoxBillFilePath.Location = new Point(714, 147);
            TextBoxBillFilePath.Name = "TextBoxBillFilePath";
            TextBoxBillFilePath.ReadOnly = true;
            TextBoxBillFilePath.Size = new Size(77, 14);
            TextBoxBillFilePath.TabIndex = 53;
            TextBoxBillFilePath.TabStop = false;
            TextBoxBillFilePath.Text = "FileName";
            TextBoxBillFilePath.Visible = false;
            // 
            // ImageListBill
            // 
            ImageListBill.ColorDepth = ColorDepth.Depth8Bit;
            ImageListBill.ImageStream = (ImageListStreamer)resources.GetObject("ImageListBill.ImageStream");
            ImageListBill.TransparentColor = Color.Transparent;
            ImageListBill.Images.SetKeyName(0, "accounts1.png");
            ImageListBill.Images.SetKeyName(1, "Supplier3.ico");
            ImageListBill.Images.SetKeyName(2, "customers.ico");
            ImageListBill.Images.SetKeyName(3, "employee.png");
            // 
            // BtnBillFileDelete
            // 
            BtnBillFileDelete.BackgroundImage = (Image)resources.GetObject("BtnBillFileDelete.BackgroundImage");
            BtnBillFileDelete.FlatAppearance.BorderColor = Color.White;
            BtnBillFileDelete.FlatStyle = FlatStyle.Flat;
            BtnBillFileDelete.Location = new Point(685, 144);
            BtnBillFileDelete.Name = "BtnBillFileDelete";
            BtnBillFileDelete.Size = new Size(18, 17);
            BtnBillFileDelete.TabIndex = 55;
            BtnBillFileDelete.Text = "button2";
            BtnBillFileDelete.UseVisualStyleBackColor = true;
            BtnBillFileDelete.Visible = false;
            BtnBillFileDelete.Click += BtnBillFileDelete_Click;
            // 
            // BtnBillFileDownload
            // 
            BtnBillFileDownload.BackgroundImage = (Image)resources.GetObject("BtnBillFileDownload.BackgroundImage");
            BtnBillFileDownload.FlatAppearance.BorderColor = Color.White;
            BtnBillFileDownload.FlatStyle = FlatStyle.Flat;
            BtnBillFileDownload.Location = new Point(666, 144);
            BtnBillFileDownload.Name = "BtnBillFileDownload";
            BtnBillFileDownload.Size = new Size(18, 17);
            BtnBillFileDownload.TabIndex = 54;
            BtnBillFileDownload.UseVisualStyleBackColor = true;
            BtnBillFileDownload.Visible = false;
            BtnBillFileDownload.Click += BtnBillFileDownload_Click;
            // 
            // StatusStripBill
            // 
            StatusStripBill.Items.AddRange(new ToolStripItem[] { ToolStripStatusLabelErrorBill });
            StatusStripBill.Location = new Point(0, 496);
            StatusStripBill.Name = "StatusStripBill";
            StatusStripBill.Size = new Size(1246, 22);
            StatusStripBill.TabIndex = 56;
            StatusStripBill.Text = "statusStrip1";
            // 
            // ToolStripStatusLabelErrorBill
            // 
            ToolStripStatusLabelErrorBill.Name = "ToolStripStatusLabelErrorBill";
            ToolStripStatusLabelErrorBill.Size = new Size(94, 17);
            ToolStripStatusLabelErrorBill.Text = "                             ";
            // 
            // TimerBill
            // 
            TimerBill.Interval = 400;
            TimerBill.Tick += TimerBill_Tick;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(815, 38);
            label3.Name = "label3";
            label3.Size = new Size(61, 13);
            label3.TabIndex = 71;
            label3.Text = "Recent Bills";
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = SystemColors.ControlLight;
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, TextBoxBillSearch, BillSearchGo });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new Padding(5);
            toolStrip1.Size = new Size(1246, 33);
            toolStrip1.TabIndex = 72;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(61, 20);
            toolStripLabel1.Text = "Search Bill";
            // 
            // TextBoxBillSearch
            // 
            TextBoxBillSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxBillSearch.MaxLength = 30;
            TextBoxBillSearch.Name = "TextBoxBillSearch";
            TextBoxBillSearch.Size = new Size(200, 23);
            TextBoxBillSearch.KeyDown += TextBoxBillSearch_KeyDown;
            TextBoxBillSearch.TextChanged += TextBoxBillSearch_TextChanged;
            // 
            // BillSearchGo
            // 
            BillSearchGo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BillSearchGo.ImageTransparentColor = Color.Magenta;
            BillSearchGo.Name = "BillSearchGo";
            BillSearchGo.Size = new Size(26, 20);
            BillSearchGo.Text = "Go";
            BillSearchGo.Click += BillSearchGo_Click;
            // 
            // ComboBoxBillTerm
            // 
            ComboBoxBillTerm.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxBillTerm.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxBillTerm.FormattingEnabled = true;
            ComboBoxBillTerm.Location = new Point(12, 95);
            ComboBoxBillTerm.Name = "ComboBoxBillTerm";
            ComboBoxBillTerm.Size = new Size(180, 21);
            ComboBoxBillTerm.TabIndex = 2;
            ComboBoxBillTerm.TxtVisible = true;
            ComboBoxBillTerm.SelectedIndexChanged += ComboBoxBillTerm_SelectedIndexChanged;
            ComboBoxBillTerm.KeyPress += ComboBoxBillTerm_KeyPress;
            // 
            // GridViewBillSearch
            // 
            GridViewBillSearch.AllowUserToAddRows = false;
            GridViewBillSearch.AllowUserToDeleteRows = false;
            GridViewBillSearch.AllowUserToResizeColumns = false;
            GridViewBillSearch.AllowUserToResizeRows = false;
            GridViewBillSearch.BackgroundColor = SystemColors.ControlLight;
            dataGridViewCellStyle28.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle28.BackColor = SystemColors.ControlLight;
            dataGridViewCellStyle28.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle28.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle28.SelectionBackColor = SystemColors.ControlLight;
            dataGridViewCellStyle28.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle28.WrapMode = DataGridViewTriState.True;
            GridViewBillSearch.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle28;
            GridViewBillSearch.ColumnHeadersHeight = 20;
            GridViewBillSearch.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewBillSearch.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn6, dataGridViewTextBoxColumn7, dataGridViewTextBoxColumn8, dataGridViewTextBoxColumn9, dataGridViewTextBoxColumn10 });
            dataGridViewCellStyle33.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle33.BackColor = SystemColors.Window;
            dataGridViewCellStyle33.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle33.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle33.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle33.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle33.WrapMode = DataGridViewTriState.True;
            GridViewBillSearch.DefaultCellStyle = dataGridViewCellStyle33;
            GridViewBillSearch.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewBillSearch.EnableHeadersVisualStyles = false;
            GridViewBillSearch.Location = new Point(816, 56);
            GridViewBillSearch.MultiSelect = false;
            GridViewBillSearch.Name = "GridViewBillSearch";
            GridViewBillSearch.ReadOnly = true;
            GridViewBillSearch.RowHeadersVisible = false;
            dataGridViewCellStyle34.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle34.SelectionForeColor = Color.White;
            dataGridViewCellStyle34.WrapMode = DataGridViewTriState.True;
            GridViewBillSearch.RowsDefaultCellStyle = dataGridViewCellStyle34;
            GridViewBillSearch.RowTemplate.Height = 20;
            GridViewBillSearch.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewBillSearch.ShowCellToolTips = false;
            GridViewBillSearch.Size = new Size(417, 404);
            GridViewBillSearch.TabIndex = 70;
            GridViewBillSearch.TabStop = false;
            GridViewBillSearch.CellDoubleClick += GridViewBillSearch_CellDoubleClick;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewCellStyle29.WrapMode = DataGridViewTriState.True;
            dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle29;
            dataGridViewTextBoxColumn6.HeaderText = "Bill. #";
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.ReadOnly = true;
            dataGridViewTextBoxColumn6.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn6.Width = 70;
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewCellStyle30.WrapMode = DataGridViewTriState.True;
            dataGridViewTextBoxColumn7.DefaultCellStyle = dataGridViewCellStyle30;
            dataGridViewTextBoxColumn7.HeaderText = "Date";
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            dataGridViewTextBoxColumn7.ReadOnly = true;
            dataGridViewTextBoxColumn7.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn7.Width = 70;
            // 
            // dataGridViewTextBoxColumn8
            // 
            dataGridViewTextBoxColumn8.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle31.WrapMode = DataGridViewTriState.True;
            dataGridViewTextBoxColumn8.DefaultCellStyle = dataGridViewCellStyle31;
            dataGridViewTextBoxColumn8.HeaderText = "Supplier";
            dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            dataGridViewTextBoxColumn8.ReadOnly = true;
            dataGridViewTextBoxColumn8.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn8.Width = 174;
            // 
            // dataGridViewTextBoxColumn9
            // 
            dataGridViewCellStyle32.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle32.WrapMode = DataGridViewTriState.True;
            dataGridViewTextBoxColumn9.DefaultCellStyle = dataGridViewCellStyle32;
            dataGridViewTextBoxColumn9.HeaderText = "Amount";
            dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            dataGridViewTextBoxColumn9.ReadOnly = true;
            dataGridViewTextBoxColumn9.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn9.Width = 80;
            // 
            // dataGridViewTextBoxColumn10
            // 
            dataGridViewTextBoxColumn10.HeaderText = "Id";
            dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            dataGridViewTextBoxColumn10.ReadOnly = true;
            dataGridViewTextBoxColumn10.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn10.Visible = false;
            // 
            // DataGridViewBillDetails
            // 
            DataGridViewBillDetails.AllowUserToResizeColumns = false;
            DataGridViewBillDetails.AllowUserToResizeRows = false;
            DataGridViewBillDetails.BackgroundColor = SystemColors.ControlLight;
            dataGridViewCellStyle35.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle35.BackColor = SystemColors.ControlLight;
            dataGridViewCellStyle35.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle35.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle35.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle35.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle35.WrapMode = DataGridViewTriState.True;
            DataGridViewBillDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle35;
            DataGridViewBillDetails.ColumnHeadersHeight = 20;
            DataGridViewBillDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DataGridViewBillDetails.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, Account, dataGridViewTextBoxColumn2, Amount, Delete });
            DataGridViewBillDetails.EditMode = DataGridViewEditMode.EditOnEnter;
            DataGridViewBillDetails.EnableHeadersVisualStyles = false;
            DataGridViewBillDetails.Location = new Point(12, 178);
            DataGridViewBillDetails.Name = "DataGridViewBillDetails";
            DataGridViewBillDetails.RowHeadersVisible = false;
            dataGridViewCellStyle40.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle40.SelectionForeColor = SystemColors.ActiveCaptionText;
            DataGridViewBillDetails.RowsDefaultCellStyle = dataGridViewCellStyle40;
            DataGridViewBillDetails.RowTemplate.Height = 20;
            DataGridViewBillDetails.ScrollBars = ScrollBars.Vertical;
            DataGridViewBillDetails.ShowCellToolTips = false;
            DataGridViewBillDetails.Size = new Size(793, 259);
            DataGridViewBillDetails.TabIndex = 7;
            DataGridViewBillDetails.CellClick += DataGridViewBillDetails_CellClick;
            DataGridViewBillDetails.CellContentClick += DataGridViewBillDetails_CellContentClick;
            DataGridViewBillDetails.CellEndEdit += DataGridViewBillDetails_CellEndEdit;
            DataGridViewBillDetails.CellEnter += DataGridViewBillDetails_CellEnter;
            DataGridViewBillDetails.CellLeave += DataGridViewBillDetails_CellLeave;
            DataGridViewBillDetails.DataError += DataGridViewBillDetails_DataError;
            DataGridViewBillDetails.EditingControlShowing += DataGridViewBillDetails_EditingControlShowing;
            DataGridViewBillDetails.RowsAdded += DataGridViewBillDetails_RowsAdded;
            DataGridViewBillDetails.KeyDown += DataGridViewBillDetails_KeyDown;
            DataGridViewBillDetails.KeyPress += DataGridViewBillDetails_KeyPress;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewCellStyle36.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle36;
            dataGridViewTextBoxColumn1.HeaderText = "#";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            dataGridViewTextBoxColumn1.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn1.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn1.Width = 30;
            // 
            // Account
            // 
            Account.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            Account.FlatStyle = FlatStyle.Flat;
            Account.HeaderText = "Account";
            Account.Name = "Account";
            Account.Resizable = DataGridViewTriState.False;
            Account.Width = 208;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewCellStyle37.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle37;
            dataGridViewTextBoxColumn2.HeaderText = "Description";
            dataGridViewTextBoxColumn2.MaxInputLength = 250;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            dataGridViewTextBoxColumn2.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn2.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn2.Width = 400;
            // 
            // Amount
            // 
            dataGridViewCellStyle38.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle38.NullValue = "0.00";
            Amount.DefaultCellStyle = dataGridViewCellStyle38;
            Amount.HeaderText = "Amount";
            Amount.Name = "Amount";
            Amount.Resizable = DataGridViewTriState.False;
            Amount.Width = 108;
            // 
            // Delete
            // 
            dataGridViewCellStyle39.Alignment = DataGridViewContentAlignment.TopLeft;
            Delete.DefaultCellStyle = dataGridViewCellStyle39;
            Delete.HeaderText = "...";
            Delete.Name = "Delete";
            Delete.Resizable = DataGridViewTriState.False;
            Delete.Width = 25;
            // 
            // DateTimePickerBillDate
            // 
            DateTimePickerBillDate.BackColor = Color.White;
            DateTimePickerBillDate.BorderStyle = BorderStyle.FixedSingle;
            DateTimePickerBillDate.Date = null;
            DateTimePickerBillDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DateTimePickerBillDate.Format = "MM/dd/yyyy";
            DateTimePickerBillDate.Location = new Point(12, 137);
            DateTimePickerBillDate.MaxDate = new DateTime(9997, 12, 31, 8, 58, 5, 0);
            DateTimePickerBillDate.MinDate = new DateTime(1900, 1, 1, 18, 12, 10, 0);
            DateTimePickerBillDate.Name = "DateTimePickerBillDate";
            DateTimePickerBillDate.ReadOnly = false;
            DateTimePickerBillDate.Size = new Size(93, 21);
            DateTimePickerBillDate.TabIndex = 3;
            DateTimePickerBillDate.Leave += DateTimePickerBillDate_Leave;
            // 
            // BtnBillExit
            // 
            BtnBillExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnBillExit.Location = new Point(1149, 466);
            BtnBillExit.Name = "BtnBillExit";
            BtnBillExit.Size = new Size(75, 23);
            BtnBillExit.TabIndex = 74;
            BtnBillExit.Text = "Exit [F10]";
            BtnBillExit.UseVisualStyleBackColor = true;
            BtnBillExit.Click += BtnBillExit_Click;
            // 
            // BtnPayablePayment
            // 
            BtnPayablePayment.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPayablePayment.Location = new Point(759, 466);
            BtnPayablePayment.Name = "BtnPayablePayment";
            BtnPayablePayment.Size = new Size(117, 23);
            BtnPayablePayment.TabIndex = 73;
            BtnPayablePayment.Text = "Payable Payment";
            BtnPayablePayment.UseVisualStyleBackColor = true;
            BtnPayablePayment.Click += BtnPayablePayment_Click;
            // 
            // TextBoxBillSupplier
            // 
            TextBoxBillSupplier.BackColor = SystemColors.Window;
            TextBoxBillSupplier.Id = null;
            TextBoxBillSupplier.Location = new Point(12, 53);
            TextBoxBillSupplier.MaxLength = 30;
            TextBoxBillSupplier.Name = "TextBoxBillSupplier";
            TextBoxBillSupplier.ReadOnly = true;
            TextBoxBillSupplier.Size = new Size(180, 21);
            TextBoxBillSupplier.TabIndex = 1;
            TextBoxBillSupplier.PreviewKeyDown += TextBoxBillSupplier_PreviewKeyDown;
            // 
            // BtnBillSearchSupplier
            // 
            BtnBillSearchSupplier.Location = new Point(198, 51);
            BtnBillSearchSupplier.Name = "BtnBillSearchSupplier";
            BtnBillSearchSupplier.Size = new Size(71, 23);
            BtnBillSearchSupplier.TabIndex = 166;
            BtnBillSearchSupplier.TabStop = false;
            BtnBillSearchSupplier.Text = "Search [F2]";
            BtnBillSearchSupplier.UseVisualStyleBackColor = true;
            BtnBillSearchSupplier.Click += BtnBillSearchSupplier_Click;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewCellStyle41.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle41.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle41;
            dataGridViewTextBoxColumn3.HeaderText = "";
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            dataGridViewTextBoxColumn3.Resizable = DataGridViewTriState.True;
            dataGridViewTextBoxColumn3.Width = 25;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewCellStyle42.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle42.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle42;
            dataGridViewTextBoxColumn4.HeaderText = "Value";
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.Width = 108;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "";
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.Resizable = DataGridViewTriState.True;
            dataGridViewTextBoxColumn5.Width = 25;
            // 
            // OpenInvoiceNumber
            // 
            dataGridViewCellStyle43.WrapMode = DataGridViewTriState.True;
            OpenInvoiceNumber.DefaultCellStyle = dataGridViewCellStyle43;
            OpenInvoiceNumber.HeaderText = "Bill. #";
            OpenInvoiceNumber.Name = "OpenInvoiceNumber";
            OpenInvoiceNumber.ReadOnly = true;
            OpenInvoiceNumber.SortMode = DataGridViewColumnSortMode.NotSortable;
            OpenInvoiceNumber.Width = 70;
            // 
            // OpenInvDescription
            // 
            dataGridViewCellStyle44.WrapMode = DataGridViewTriState.True;
            OpenInvDescription.DefaultCellStyle = dataGridViewCellStyle44;
            OpenInvDescription.HeaderText = "Date";
            OpenInvDescription.Name = "OpenInvDescription";
            OpenInvDescription.ReadOnly = true;
            OpenInvDescription.SortMode = DataGridViewColumnSortMode.NotSortable;
            OpenInvDescription.Width = 70;
            // 
            // OpenInvAmount
            // 
            dataGridViewCellStyle45.WrapMode = DataGridViewTriState.True;
            OpenInvAmount.DefaultCellStyle = dataGridViewCellStyle45;
            OpenInvAmount.HeaderText = "Supplier";
            OpenInvAmount.Name = "OpenInvAmount";
            OpenInvAmount.ReadOnly = true;
            OpenInvAmount.SortMode = DataGridViewColumnSortMode.NotSortable;
            OpenInvAmount.Width = 175;
            // 
            // BalAmount
            // 
            dataGridViewCellStyle46.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle46.WrapMode = DataGridViewTriState.True;
            BalAmount.DefaultCellStyle = dataGridViewCellStyle46;
            BalAmount.HeaderText = "Amount";
            BalAmount.Name = "BalAmount";
            BalAmount.ReadOnly = true;
            BalAmount.SortMode = DataGridViewColumnSortMode.NotSortable;
            BalAmount.Width = 80;
            // 
            // Type
            // 
            Type.HeaderText = "Id";
            Type.Name = "Type";
            Type.ReadOnly = true;
            Type.SortMode = DataGridViewColumnSortMode.NotSortable;
            Type.Visible = false;
            // 
            // Column1
            // 
            Column1.HeaderText = "#";
            Column1.Name = "Column1";
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 30;
            // 
            // Description
            // 
            Description.HeaderText = "Description";
            Description.MaxInputLength = 250;
            Description.Name = "Description";
            Description.Resizable = DataGridViewTriState.False;
            Description.SortMode = DataGridViewColumnSortMode.NotSortable;
            Description.Width = 410;
            // 
            // BtnBillNewSupplier
            // 
            BtnBillNewSupplier.ButtonText = "New [F3]";
            BtnBillNewSupplier.ImageList = ImageListBill;
            BtnBillNewSupplier.Items = (System.Collections.ObjectModel.Collection<string>)resources.GetObject("BtnBillNewSupplier.Items");
            BtnBillNewSupplier.Location = new Point(271, 51);
            BtnBillNewSupplier.Margin = new Padding(4, 3, 4, 3);
            BtnBillNewSupplier.Name = "BtnBillNewSupplier";
            BtnBillNewSupplier.Size = new Size(77, 27);
            BtnBillNewSupplier.TabIndex = 197;
            BtnBillNewSupplier.TabStop = false;
            BtnBillNewSupplier.ItemClickedEvent += BtnBillNewSupplier_ItemClickedEvent;
            // 
            // LableReferenceNumber
            // 
            LableReferenceNumber.AutoSize = true;
            LableReferenceNumber.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LableReferenceNumber.Location = new Point(661, 77);
            LableReferenceNumber.Name = "LableReferenceNumber";
            LableReferenceNumber.Size = new Size(91, 13);
            LableReferenceNumber.TabIndex = 26;
            LableReferenceNumber.Text = "Last Reference #";
            // 
            // LastBillReferenceNumber
            // 
            LastBillReferenceNumber.AutoSize = true;
            LastBillReferenceNumber.Font = new Font("Tahoma", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            LastBillReferenceNumber.Location = new Point(661, 95);
            LastBillReferenceNumber.Name = "LastBillReferenceNumber";
            LastBillReferenceNumber.Size = new Size(55, 16);
            LastBillReferenceNumber.TabIndex = 28;
            LastBillReferenceNumber.Text = "000000";
            // 
            // FormBill
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1246, 518);
            Controls.Add(ComboBoxBillTerm);
            Controls.Add(TextBoxBillMemo);
            Controls.Add(BtnBillNewSupplier);
            Controls.Add(TextBoxBillSupplier);
            Controls.Add(BtnBillSearchSupplier);
            Controls.Add(BtnBillExit);
            Controls.Add(BtnPayablePayment);
            Controls.Add(DateTimePickerBillDate);
            Controls.Add(toolStrip1);
            Controls.Add(GridViewBillSearch);
            Controls.Add(StatusStripBill);
            Controls.Add(BtnBillFileDelete);
            Controls.Add(BtnBillFileDownload);
            Controls.Add(TextBoxBillFilePath);
            Controls.Add(TextBoxBillId);
            Controls.Add(TextBoxBillDueDate);
            Controls.Add(label9);
            Controls.Add(BtnBillAttachment);
            Controls.Add(BtnBillDelete);
            Controls.Add(BtnBillNew);
            Controls.Add(BtnBillPrint);
            Controls.Add(BtnBillCancel);
            Controls.Add(BtnBillSave);
            Controls.Add(DataGridViewBillDetailsTotal);
            Controls.Add(TextBoxBillInternalMemo);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(LastBillReferenceNumber);
            Controls.Add(BillReferenceNumber);
            Controls.Add(LableReferenceNumber);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(DataGridViewBillDetails);
            Controls.Add(label3);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormBill";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Bill";
            FormClosing += FormBill_FormClosing;
            Load += FormBill_Load;
            Controls.SetChildIndex(label3, 0);
            Controls.SetChildIndex(DataGridViewBillDetails, 0);
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(label2, 0);
            Controls.SetChildIndex(LableReferenceNumber, 0);
            Controls.SetChildIndex(BillReferenceNumber, 0);
            Controls.SetChildIndex(LastBillReferenceNumber, 0);
            Controls.SetChildIndex(label4, 0);
            Controls.SetChildIndex(label5, 0);
            Controls.SetChildIndex(label6, 0);
            Controls.SetChildIndex(label7, 0);
            Controls.SetChildIndex(label8, 0);
            Controls.SetChildIndex(TextBoxBillInternalMemo, 0);
            Controls.SetChildIndex(DataGridViewBillDetailsTotal, 0);
            Controls.SetChildIndex(BtnBillSave, 0);
            Controls.SetChildIndex(BtnBillCancel, 0);
            Controls.SetChildIndex(BtnBillPrint, 0);
            Controls.SetChildIndex(BtnBillNew, 0);
            Controls.SetChildIndex(BtnBillDelete, 0);
            Controls.SetChildIndex(BtnBillAttachment, 0);
            Controls.SetChildIndex(label9, 0);
            Controls.SetChildIndex(TextBoxBillDueDate, 0);
            Controls.SetChildIndex(TextBoxBillId, 0);
            Controls.SetChildIndex(TextBoxBillFilePath, 0);
            Controls.SetChildIndex(BtnBillFileDownload, 0);
            Controls.SetChildIndex(BtnBillFileDelete, 0);
            Controls.SetChildIndex(StatusStripBill, 0);
            Controls.SetChildIndex(GridViewBillSearch, 0);
            Controls.SetChildIndex(toolStrip1, 0);
            Controls.SetChildIndex(DateTimePickerBillDate, 0);
            Controls.SetChildIndex(BtnPayablePayment, 0);
            Controls.SetChildIndex(BtnBillExit, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(BtnBillSearchSupplier, 0);
            Controls.SetChildIndex(TextBoxBillSupplier, 0);
            Controls.SetChildIndex(BtnBillNewSupplier, 0);
            Controls.SetChildIndex(TextBoxBillMemo, 0);
            Controls.SetChildIndex(ComboBoxBillTerm, 0);
            ((System.ComponentModel.ISupportInitialize)DataGridViewBillDetailsTotal).EndInit();
            StatusStripBill.ResumeLayout(false);
            StatusStripBill.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewBillSearch).EndInit();
            ((System.ComponentModel.ISupportInitialize)DataGridViewBillDetails).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.OpenFileDialog OpenFileDialogBill;
        private System.Windows.Forms.Button BtnBillDelete;
        private System.Windows.Forms.Button BtnBillNew;
        private System.Windows.Forms.Button BtnBillPrint;
        private System.Windows.Forms.Button BtnBillCancel;
        private System.Windows.Forms.Button BtnBillSave;
        private System.Windows.Forms.DataGridView DataGridViewBillDetailsTotal;
        private System.Windows.Forms.TextBox TextBoxBillInternalMemo;
        private System.Windows.Forms.TextBox TextBoxBillMemo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label BillReferenceNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private fa.views.controls.DataViewVerticalScroll DataGridViewBillDetails;
        private System.Windows.Forms.Button BtnBillAttachment;
        public System.Windows.Forms.MaskedTextBox TextBoxBillDueDate;
        private System.Windows.Forms.TextBox TextBoxBillId;
        private System.Windows.Forms.TextBox TextBoxBillFilePath;
        private System.Windows.Forms.ImageList ImageListBill;
        private System.Windows.Forms.Button BtnBillFileDownload;
        private System.Windows.Forms.Button BtnBillFileDelete;
        private System.Windows.Forms.SaveFileDialog SaveFileDialogBill;
        private System.Windows.Forms.StatusStrip StatusStripBill;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabelErrorBill;
        private System.Windows.Forms.Timer TimerBill;
        private System.Windows.Forms.Label label3;
        private controls.DataViewVerticalScroll GridViewBillSearch;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox TextBoxBillSearch;
        private System.Windows.Forms.ToolStripButton BillSearchGo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn OpenInvoiceNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn OpenInvDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn OpenInvAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn BalAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private controls.ComboBoxSwapTextBox ComboBoxBillTerm;
        private controls.text.DateWithCalendar DateTimePickerBillDate;
        private System.Windows.Forms.Button BtnBillExit;
        private System.Windows.Forms.Button BtnPayablePayment;
        private controls.text.IDTextBox TextBoxBillSupplier;
        private System.Windows.Forms.Button BtnBillSearchSupplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private Dropdown_Button.UserControlButtonWithMenu BtnBillNewSupplier;
        private DataGridViewTextBoxColumn Total;
        private DataGridViewTextBoxColumn Value;
        private DataGridViewTextBoxColumn delete1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewComboBoxColumn Account;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private controls.grid.DataGridViewCurrencyColumn Amount;
        private DataGridViewButtonColumn Delete;
        private Label LableReferenceNumber;
        private Label LastBillReferenceNumber;
    }
}