namespace fa.views.account.transactions
{
    partial class FormJournal
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
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle14 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle15 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle13 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJournal));
            label1 = new Label();
            GridViewJournal = new controls.DataViewVerticalScroll();
            SerialNumber = new DataGridViewTextBoxColumn();
            Account = new DataGridViewTextBoxColumn();
            Debits = new controls.grid.DataGridViewCurrencyColumn();
            Credits = new controls.grid.DataGridViewCurrencyColumn();
            Description = new DataGridViewTextBoxColumn();
            CreditorOrDebtorName = new DataGridViewTextBoxColumn();
            Delete = new DataGridViewButtonColumn();
            TextBoxJournalMemo = new TextBox();
            label2 = new Label();
            TextBoxJournalId = new TextBox();
            GridViewJournalTotal = new controls.DataViewVerticalScroll();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
            DatePickerJournal = new controls.text.DateWithCalendar();
            BtnJournalExit = new Button();
            BtnJournalSave = new Button();
            BtnJournalCancel = new Button();
            BtnJournalPrint = new Button();
            BtnJournalDelete = new Button();
            BtnJournalNew = new Button();
            statusStrip1 = new StatusStrip();
            JournalErrorMsg = new ToolStripStatusLabel();
            toolStrip1 = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            TextBoxJournalSearch = new ToolStripTextBox();
            JournalSearchGo = new ToolStripButton();
            JournalRefNo = new Label();
            label3 = new Label();
            TimerJournal = new System.Windows.Forms.Timer(components);
            BtnJournalNewAccounts = new Dropdown_Button.UserControlButtonWithMenu();
            ImageListJournal = new ImageList(components);
            label4 = new Label();
            LastJournalRefNo = new Label();
            ((System.ComponentModel.ISupportInitialize)GridViewJournal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)GridViewJournalTotal).BeginInit();
            statusStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Size = new Size(100, 21);
            ProductBatchIdTransport.TabIndex = 0;
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Size = new Size(100, 21);
            AccountIdTransport.TabIndex = 0;
            // 
            // checkBoxIsPatient
            // 
            checkBoxIsPatient.Size = new Size(69, 17);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(9, 41);
            label1.Name = "label1";
            label1.Size = new Size(30, 13);
            label1.TabIndex = 0;
            label1.Text = "Date";
            // 
            // GridViewJournal
            // 
            GridViewJournal.AllowUserToDeleteRows = false;
            GridViewJournal.AllowUserToResizeColumns = false;
            GridViewJournal.AllowUserToResizeRows = false;
            GridViewJournal.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            GridViewJournal.BackgroundColor = SystemColors.ControlLight;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.ControlLight;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewJournal.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewJournal.ColumnHeadersHeight = 20;
            GridViewJournal.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewJournal.Columns.AddRange(new DataGridViewColumn[] { SerialNumber, Account, Debits, Credits, Description, CreditorOrDebtorName, Delete });
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = SystemColors.Window;
            dataGridViewCellStyle9.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle9.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            GridViewJournal.DefaultCellStyle = dataGridViewCellStyle9;
            GridViewJournal.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewJournal.EnableHeadersVisualStyles = false;
            GridViewJournal.Location = new Point(13, 122);
            GridViewJournal.Name = "GridViewJournal";
            GridViewJournal.RowHeadersVisible = false;
            dataGridViewCellStyle10.BackColor = Color.White;
            dataGridViewCellStyle10.ForeColor = Color.Black;
            dataGridViewCellStyle10.SelectionBackColor = Color.White;
            dataGridViewCellStyle10.SelectionForeColor = Color.Black;
            GridViewJournal.RowsDefaultCellStyle = dataGridViewCellStyle10;
            GridViewJournal.RowTemplate.Height = 20;
            GridViewJournal.ScrollBars = ScrollBars.Vertical;
            GridViewJournal.ShowCellToolTips = false;
            GridViewJournal.Size = new Size(1026, 286);
            GridViewJournal.TabIndex = 4;
            GridViewJournal.CellClick += GridViewJournal_CellClick;
            GridViewJournal.CellEndEdit += GridViewJournal_CellEndEdit;
            GridViewJournal.CellEnter += GridViewJournal_CellEnter;
            GridViewJournal.CellFormatting += GridViewJournal_CellFormatting;
            GridViewJournal.CellLeave += GridViewJournal_CellLeave;
            GridViewJournal.CellValueChanged += GridViewJournal_CellValueChanged;
            GridViewJournal.DataError += GridViewJournal_DataError;
            GridViewJournal.EditingControlShowing += GridViewJournal_EditingControlShowing;
            GridViewJournal.RowsAdded += GridViewJournal_RowsAdded;
            GridViewJournal.PreviewKeyDown += GridViewJournal_PreviewKeyDown;
            // 
            // SerialNumber
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            SerialNumber.DefaultCellStyle = dataGridViewCellStyle2;
            SerialNumber.HeaderText = "#";
            SerialNumber.Name = "SerialNumber";
            SerialNumber.Resizable = DataGridViewTriState.False;
            SerialNumber.SortMode = DataGridViewColumnSortMode.NotSortable;
            SerialNumber.Width = 25;
            // 
            // Account
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            Account.DefaultCellStyle = dataGridViewCellStyle3;
            Account.HeaderText = "Account";
            Account.Name = "Account";
            Account.Resizable = DataGridViewTriState.False;
            Account.SortMode = DataGridViewColumnSortMode.NotSortable;
            Account.Width = 200;
            // 
            // Debits
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle4.NullValue = "0.00";
            Debits.DefaultCellStyle = dataGridViewCellStyle4;
            Debits.HeaderText = "Debits";
            Debits.Name = "Debits";
            Debits.Resizable = DataGridViewTriState.False;
            // 
            // Credits
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle5.NullValue = "0.00";
            Credits.DefaultCellStyle = dataGridViewCellStyle5;
            Credits.HeaderText = "Credits";
            Credits.Name = "Credits";
            Credits.Resizable = DataGridViewTriState.False;
            // 
            // Description
            // 
            Description.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            Description.DefaultCellStyle = dataGridViewCellStyle6;
            Description.HeaderText = "Description";
            Description.MaxInputLength = 500;
            Description.Name = "Description";
            Description.Resizable = DataGridViewTriState.False;
            Description.SortMode = DataGridViewColumnSortMode.NotSortable;
            Description.Width = 353;
            // 
            // CreditorOrDebtorName
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.TopLeft;
            CreditorOrDebtorName.DefaultCellStyle = dataGridViewCellStyle7;
            CreditorOrDebtorName.HeaderText = "Name";
            CreditorOrDebtorName.Name = "CreditorOrDebtorName";
            CreditorOrDebtorName.Resizable = DataGridViewTriState.False;
            CreditorOrDebtorName.SortMode = DataGridViewColumnSortMode.NotSortable;
            CreditorOrDebtorName.Width = 200;
            // 
            // Delete
            // 
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.NullValue = "X";
            Delete.DefaultCellStyle = dataGridViewCellStyle8;
            Delete.HeaderText = "...";
            Delete.Name = "Delete";
            Delete.Resizable = DataGridViewTriState.False;
            Delete.Width = 25;
            // 
            // TextBoxJournalMemo
            // 
            TextBoxJournalMemo.Location = new Point(349, 60);
            TextBoxJournalMemo.MaxLength = 500;
            TextBoxJournalMemo.Multiline = true;
            TextBoxJournalMemo.Name = "TextBoxJournalMemo";
            TextBoxJournalMemo.Size = new Size(690, 44);
            TextBoxJournalMemo.TabIndex = 3;
            TextBoxJournalMemo.KeyPress += TextBoxJournalMemo_KeyPress;
            TextBoxJournalMemo.PreviewKeyDown += TextBoxJournalMemo_PreviewKeyDown;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(346, 41);
            label2.Name = "label2";
            label2.Size = new Size(35, 13);
            label2.TabIndex = 0;
            label2.Text = "Memo";
            // 
            // TextBoxJournalId
            // 
            TextBoxJournalId.Location = new Point(308, 449);
            TextBoxJournalId.Margin = new Padding(2);
            TextBoxJournalId.Name = "TextBoxJournalId";
            TextBoxJournalId.Size = new Size(79, 21);
            TextBoxJournalId.TabIndex = 58;
            TextBoxJournalId.Visible = false;
            // 
            // GridViewJournalTotal
            // 
            GridViewJournalTotal.AllowUserToDeleteRows = false;
            GridViewJournalTotal.AllowUserToResizeColumns = false;
            GridViewJournalTotal.AllowUserToResizeRows = false;
            GridViewJournalTotal.BackgroundColor = Color.White;
            GridViewJournalTotal.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GridViewJournalTotal.ColumnHeadersVisible = false;
            GridViewJournalTotal.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, dataGridViewTextBoxColumn7 });
            dataGridViewCellStyle14.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle14.BackColor = SystemColors.Window;
            dataGridViewCellStyle14.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle14.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = DataGridViewTriState.False;
            GridViewJournalTotal.DefaultCellStyle = dataGridViewCellStyle14;
            GridViewJournalTotal.Location = new Point(13, 407);
            GridViewJournalTotal.Name = "GridViewJournalTotal";
            GridViewJournalTotal.ReadOnly = true;
            GridViewJournalTotal.RowHeadersVisible = false;
            dataGridViewCellStyle15.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle15.BackColor = Color.White;
            dataGridViewCellStyle15.ForeColor = Color.Black;
            dataGridViewCellStyle15.SelectionBackColor = Color.White;
            dataGridViewCellStyle15.SelectionForeColor = Color.Black;
            GridViewJournalTotal.RowsDefaultCellStyle = dataGridViewCellStyle15;
            GridViewJournalTotal.ShowCellToolTips = false;
            GridViewJournalTotal.Size = new Size(1026, 24);
            GridViewJournalTotal.TabIndex = 59;
            GridViewJournalTotal.TabStop = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "#";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            dataGridViewTextBoxColumn1.Width = 25;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle11;
            dataGridViewTextBoxColumn2.HeaderText = "Account";
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            dataGridViewTextBoxColumn2.Width = 200;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle12.NullValue = "0.00";
            dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle12;
            dataGridViewTextBoxColumn3.HeaderText = "Debits";
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle13.NullValue = "0.00";
            dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle13;
            dataGridViewTextBoxColumn4.HeaderText = "Credits";
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "Description";
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.ReadOnly = true;
            dataGridViewTextBoxColumn5.Visible = false;
            dataGridViewTextBoxColumn5.Width = 336;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.HeaderText = "Name";
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.ReadOnly = true;
            dataGridViewTextBoxColumn6.Visible = false;
            dataGridViewTextBoxColumn6.Width = 200;
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewTextBoxColumn7.HeaderText = "";
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            dataGridViewTextBoxColumn7.ReadOnly = true;
            dataGridViewTextBoxColumn7.Visible = false;
            dataGridViewTextBoxColumn7.Width = 25;
            // 
            // DatePickerJournal
            // 
            DatePickerJournal.BackColor = Color.White;
            DatePickerJournal.BorderStyle = BorderStyle.FixedSingle;
            DatePickerJournal.Date = null;
            DatePickerJournal.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DatePickerJournal.Format = "MM/dd/yy";
            DatePickerJournal.Location = new Point(13, 60);
            DatePickerJournal.MaxDate = new DateTime(9997, 12, 31, 8, 13, 58, 0);
            DatePickerJournal.MinDate = new DateTime(1900, 1, 1, 23, 5, 35, 0);
            DatePickerJournal.Name = "DatePickerJournal";
            DatePickerJournal.ReadOnly = false;
            DatePickerJournal.Size = new Size(93, 21);
            DatePickerJournal.TabIndex = 2;
            DatePickerJournal.PreviewKeyDown += DatePickerJournal_PreviewKeyDown;
            // 
            // BtnJournalExit
            // 
            BtnJournalExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnJournalExit.Location = new Point(955, 447);
            BtnJournalExit.Name = "BtnJournalExit";
            BtnJournalExit.Size = new Size(75, 23);
            BtnJournalExit.TabIndex = 8;
            BtnJournalExit.Text = "Exit [F10]";
            BtnJournalExit.UseVisualStyleBackColor = true;
            BtnJournalExit.Click += BtnJournalExit_Click;
            // 
            // BtnJournalSave
            // 
            BtnJournalSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnJournalSave.Location = new Point(865, 447);
            BtnJournalSave.Name = "BtnJournalSave";
            BtnJournalSave.Size = new Size(83, 23);
            BtnJournalSave.TabIndex = 5;
            BtnJournalSave.Text = "Save [F8]";
            BtnJournalSave.UseVisualStyleBackColor = true;
            BtnJournalSave.Click += BtnJournalSave_Click;
            BtnJournalSave.PreviewKeyDown += BtnJournalSave_PreviewKeyDown;
            // 
            // BtnJournalCancel
            // 
            BtnJournalCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnJournalCancel.Location = new Point(776, 446);
            BtnJournalCancel.Name = "BtnJournalCancel";
            BtnJournalCancel.Size = new Size(83, 23);
            BtnJournalCancel.TabIndex = 0;
            BtnJournalCancel.Text = "Cancel [Esc]";
            BtnJournalCancel.UseVisualStyleBackColor = true;
            BtnJournalCancel.Click += BtnJournalCancel_Click;
            // 
            // BtnJournalPrint
            // 
            BtnJournalPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnJournalPrint.Location = new Point(687, 446);
            BtnJournalPrint.Name = "BtnJournalPrint";
            BtnJournalPrint.Size = new Size(83, 23);
            BtnJournalPrint.TabIndex = 5;
            BtnJournalPrint.Text = "Print [F9]";
            BtnJournalPrint.UseVisualStyleBackColor = true;
            BtnJournalPrint.Click += BtnJournalPrint_Click;
            // 
            // BtnJournalDelete
            // 
            BtnJournalDelete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnJournalDelete.Location = new Point(101, 446);
            BtnJournalDelete.Name = "BtnJournalDelete";
            BtnJournalDelete.Size = new Size(83, 23);
            BtnJournalDelete.TabIndex = 6;
            BtnJournalDelete.Text = "Delete [F4]";
            BtnJournalDelete.UseVisualStyleBackColor = true;
            BtnJournalDelete.Click += BtnJournalDelete_Click;
            // 
            // BtnJournalNew
            // 
            BtnJournalNew.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnJournalNew.Location = new Point(12, 446);
            BtnJournalNew.Name = "BtnJournalNew";
            BtnJournalNew.Size = new Size(83, 23);
            BtnJournalNew.TabIndex = 7;
            BtnJournalNew.Text = "New [F3]";
            BtnJournalNew.UseVisualStyleBackColor = true;
            BtnJournalNew.Click += BtnJournalNew_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { JournalErrorMsg });
            statusStrip1.Location = new Point(0, 480);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1055, 22);
            statusStrip1.TabIndex = 68;
            statusStrip1.Text = "sdfdsf sdf sdf";
            // 
            // JournalErrorMsg
            // 
            JournalErrorMsg.Name = "JournalErrorMsg";
            JournalErrorMsg.Size = new Size(151, 17);
            JournalErrorMsg.Text = "                                                ";
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = SystemColors.ControlLight;
            toolStrip1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, TextBoxJournalSearch, JournalSearchGo });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new Padding(5);
            toolStrip1.Size = new Size(1055, 31);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(78, 18);
            toolStripLabel1.Text = "Search Journal";
            // 
            // TextBoxJournalSearch
            // 
            TextBoxJournalSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxJournalSearch.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxJournalSearch.MaxLength = 30;
            TextBoxJournalSearch.Name = "TextBoxJournalSearch";
            TextBoxJournalSearch.Size = new Size(200, 21);
            TextBoxJournalSearch.KeyDown += TextBoxJournalSearch_KeyDown;
            // 
            // JournalSearchGo
            // 
            JournalSearchGo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            JournalSearchGo.Image = (Image)resources.GetObject("JournalSearchGo.Image");
            JournalSearchGo.ImageTransparentColor = Color.Magenta;
            JournalSearchGo.Name = "JournalSearchGo";
            JournalSearchGo.Size = new Size(24, 18);
            JournalSearchGo.Text = "Go";
            JournalSearchGo.Click += JournalSearchGo_Click;
            // 
            // JournalRefNo
            // 
            JournalRefNo.AutoSize = true;
            JournalRefNo.Font = new Font("Tahoma", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            JournalRefNo.Location = new Point(176, 77);
            JournalRefNo.Name = "JournalRefNo";
            JournalRefNo.Size = new Size(55, 16);
            JournalRefNo.TabIndex = 0;
            JournalRefNo.Text = "000000";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(177, 56);
            label3.Name = "label3";
            label3.Size = new Size(53, 13);
            label3.TabIndex = 70;
            label3.Text = "Journal #";
            // 
            // TimerJournal
            // 
            TimerJournal.Interval = 400;
            // 
            // BtnJournalNewAccounts
            // 
            BtnJournalNewAccounts.ButtonText = "Add new accounts [F3]";
            BtnJournalNewAccounts.ImageList = ImageListJournal;
            BtnJournalNewAccounts.Items = (System.Collections.ObjectModel.Collection<string>)resources.GetObject("BtnJournalNewAccounts.Items");
            BtnJournalNewAccounts.Location = new Point(8, 87);
            BtnJournalNewAccounts.Margin = new Padding(4, 3, 4, 3);
            BtnJournalNewAccounts.Name = "BtnJournalNewAccounts";
            BtnJournalNewAccounts.Size = new Size(171, 29);
            BtnJournalNewAccounts.TabIndex = 197;
            BtnJournalNewAccounts.TabStop = false;
            BtnJournalNewAccounts.ItemClickedEvent += BtnJournalNewAccounts_ItemClickedEvent;
            // 
            // ImageListJournal
            // 
            ImageListJournal.ColorDepth = ColorDepth.Depth8Bit;
            ImageListJournal.ImageStream = (ImageListStreamer)resources.GetObject("ImageListJournal.ImageStream");
            ImageListJournal.TransparentColor = Color.Transparent;
            ImageListJournal.Images.SetKeyName(0, "accounts1.png");
            ImageListJournal.Images.SetKeyName(1, "Supplier3.ico");
            ImageListJournal.Images.SetKeyName(2, "customers.ico");
            ImageListJournal.Images.SetKeyName(3, "employee.png");
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(251, 56);
            label4.Name = "label4";
            label4.Size = new Size(76, 13);
            label4.TabIndex = 70;
            label4.Text = "Last Journal #";
            // 
            // LastJournalRefNo
            // 
            LastJournalRefNo.AutoSize = true;
            LastJournalRefNo.Font = new Font("Tahoma", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            LastJournalRefNo.Location = new Point(249, 77);
            LastJournalRefNo.Name = "LastJournalRefNo";
            LastJournalRefNo.Size = new Size(55, 16);
            LastJournalRefNo.TabIndex = 0;
            LastJournalRefNo.Text = "000000";
            // 
            // FormJournal
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1055, 502);
            Controls.Add(BtnJournalNewAccounts);
            Controls.Add(LastJournalRefNo);
            Controls.Add(label4);
            Controls.Add(JournalRefNo);
            Controls.Add(label3);
            Controls.Add(toolStrip1);
            Controls.Add(statusStrip1);
            Controls.Add(BtnJournalDelete);
            Controls.Add(BtnJournalNew);
            Controls.Add(BtnJournalExit);
            Controls.Add(BtnJournalSave);
            Controls.Add(BtnJournalCancel);
            Controls.Add(BtnJournalPrint);
            Controls.Add(DatePickerJournal);
            Controls.Add(GridViewJournalTotal);
            Controls.Add(TextBoxJournalId);
            Controls.Add(label2);
            Controls.Add(TextBoxJournalMemo);
            Controls.Add(GridViewJournal);
            Controls.Add(label1);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormJournal";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Journal";
            FormClosing += FormJournal_FormClosing;
            Load += FormJournal_Load;
            Controls.SetChildIndex(checkBoxIsPatient, 0);
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(GridViewJournal, 0);
            Controls.SetChildIndex(TextBoxJournalMemo, 0);
            Controls.SetChildIndex(label2, 0);
            Controls.SetChildIndex(TextBoxJournalId, 0);
            Controls.SetChildIndex(GridViewJournalTotal, 0);
            Controls.SetChildIndex(DatePickerJournal, 0);
            Controls.SetChildIndex(BtnJournalPrint, 0);
            Controls.SetChildIndex(BtnJournalCancel, 0);
            Controls.SetChildIndex(BtnJournalSave, 0);
            Controls.SetChildIndex(BtnJournalExit, 0);
            Controls.SetChildIndex(BtnJournalNew, 0);
            Controls.SetChildIndex(BtnJournalDelete, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(toolStrip1, 0);
            Controls.SetChildIndex(label3, 0);
            Controls.SetChildIndex(JournalRefNo, 0);
            Controls.SetChildIndex(label4, 0);
            Controls.SetChildIndex(LastJournalRefNo, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(BtnJournalNewAccounts, 0);
            ((System.ComponentModel.ISupportInitialize)GridViewJournal).EndInit();
            ((System.ComponentModel.ISupportInitialize)GridViewJournalTotal).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label label1;
        private fa.views.controls.DataViewVerticalScroll GridViewJournal;
        private System.Windows.Forms.TextBox TextBoxJournalMemo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TextBoxJournalId;
        private fa.views.controls.DataViewVerticalScroll GridViewJournalTotal;
        private controls.text.DateWithCalendar DatePickerJournal;
        private System.Windows.Forms.Button BtnJournalExit;
        private System.Windows.Forms.Button BtnJournalSave;
        private System.Windows.Forms.Button BtnJournalCancel;
        private System.Windows.Forms.Button BtnJournalPrint;
        private System.Windows.Forms.Button BtnJournalDelete;
        private System.Windows.Forms.Button BtnJournalNew;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel JournalErrorMsg;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox TextBoxJournalSearch;
        private System.Windows.Forms.ToolStripButton JournalSearchGo;
        private System.Windows.Forms.Label JournalRefNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer TimerJournal;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private Dropdown_Button.UserControlButtonWithMenu BtnJournalNewAccounts;
        private ImageList ImageListJournal;
        private Label label4;
        private Label LastJournalRefNo;
        private DataGridViewTextBoxColumn SerialNumber;
        private DataGridViewTextBoxColumn Account;
        private controls.grid.DataGridViewCurrencyColumn Debits;
        private controls.grid.DataGridViewCurrencyColumn Credits;
        private DataGridViewTextBoxColumn Description;
        private DataGridViewTextBoxColumn CreditorOrDebtorName;
        private DataGridViewButtonColumn Delete;
    }
}