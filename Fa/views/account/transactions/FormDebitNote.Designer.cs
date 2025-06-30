namespace fa.views.account.transactions
{
    partial class FormDebitNote
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDebitNote));
            DataGridViewCellStyle dataGridViewCellStyle17 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle18 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle19 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle15 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle16 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle20 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle21 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle27 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle28 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle22 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle23 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle24 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle25 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle26 = new DataGridViewCellStyle();
            label8 = new Label();
            DebitNoteRefNo = new Label();
            BtnDebitNoteSave = new Button();
            BtnDebitNoteCancel = new Button();
            BtnDebitNotePrint = new Button();
            BtnDebitNoteDelete = new Button();
            BtnDebitNoteNew = new Button();
            label4 = new Label();
            TextBoxDebitNoteInternalNote = new TextBox();
            TextBoxDebitNoteNote = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            TextBoxDebitNoteId = new TextBox();
            DebitNoteImageList = new ImageList(components);
            DataGridViewDebitNoteDetailsTotal = new DataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            all = new DataGridViewTextBoxColumn();
            DeleteTotal = new DataGridViewTextBoxColumn();
            StatusStripBill = new StatusStrip();
            ToolStripStatusLabelErrorDebitNote = new ToolStripStatusLabel();
            BtnDebitNoteExit = new Button();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            DateTimePickerDebitNote = new controls.text.DateWithCalendar();
            DataGridViewDebitNoteDetails = new controls.DataViewVerticalScroll();
            SerialNo = new DataGridViewTextBoxColumn();
            Account = new DataGridViewComboBoxColumn();
            Description = new DataGridViewTextBoxColumn();
            Amount = new controls.grid.DataGridViewCurrencyColumn();
            Delete = new DataGridViewButtonColumn();
            Column1 = new DataGridViewTextBoxColumn();
            TextBoxDebitNoteSupplier = new controls.text.IDTextBox();
            BtnDebitNoteSearchSupplier = new Button();
            toolStripDebiteNote = new ToolStrip();
            toolStripLabelDebiteNoteSearch = new ToolStripLabel();
            TextBoxSearchDebitNote = new ToolStripTextBox();
            BtnSearchDebitNote = new ToolStripButton();
            BtnDebitNoteNewSupplier = new Dropdown_Button.UserControlButtonWithMenu();
            LastDebitNoteRefNo = new Label();
            label10 = new Label();
            ((System.ComponentModel.ISupportInitialize)DataGridViewDebitNoteDetailsTotal).BeginInit();
            StatusStripBill.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DataGridViewDebitNoteDetails).BeginInit();
            toolStripDebiteNote.SuspendLayout();
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
            // checkBoxIsPatient
            // 
            checkBoxIsPatient.Size = new Size(69, 17);
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label8.Location = new Point(12, 83);
            label8.Name = "label8";
            label8.Size = new Size(34, 13);
            label8.TabIndex = 35;
            label8.Text = "Date";
            // 
            // DebitNoteRefNo
            // 
            DebitNoteRefNo.AutoSize = true;
            DebitNoteRefNo.Font = new Font("Tahoma", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            DebitNoteRefNo.Location = new Point(372, 57);
            DebitNoteRefNo.Name = "DebitNoteRefNo";
            DebitNoteRefNo.Size = new Size(55, 16);
            DebitNoteRefNo.TabIndex = 34;
            DebitNoteRefNo.Text = "000000";
            // 
            // BtnDebitNoteSave
            // 
            BtnDebitNoteSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnDebitNoteSave.Location = new Point(603, 389);
            BtnDebitNoteSave.Name = "BtnDebitNoteSave";
            BtnDebitNoteSave.Size = new Size(83, 23);
            BtnDebitNoteSave.TabIndex = 7;
            BtnDebitNoteSave.Text = "Save [F8]";
            BtnDebitNoteSave.UseVisualStyleBackColor = true;
            BtnDebitNoteSave.Click += BtnDebitNoteSave_Click;
            BtnDebitNoteSave.PreviewKeyDown += BtnDebitNoteSave_PreviewKeyDown;
            // 
            // BtnDebitNoteCancel
            // 
            BtnDebitNoteCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnDebitNoteCancel.Location = new Point(514, 389);
            BtnDebitNoteCancel.Name = "BtnDebitNoteCancel";
            BtnDebitNoteCancel.Size = new Size(83, 23);
            BtnDebitNoteCancel.TabIndex = 8;
            BtnDebitNoteCancel.Text = "Cancel [Esc]";
            BtnDebitNoteCancel.UseVisualStyleBackColor = true;
            BtnDebitNoteCancel.Click += BtnDebitNoteCancel_Click;
            // 
            // BtnDebitNotePrint
            // 
            BtnDebitNotePrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnDebitNotePrint.Location = new Point(425, 389);
            BtnDebitNotePrint.Name = "BtnDebitNotePrint";
            BtnDebitNotePrint.Size = new Size(83, 23);
            BtnDebitNotePrint.TabIndex = 9;
            BtnDebitNotePrint.Text = "Print [F9]";
            BtnDebitNotePrint.UseVisualStyleBackColor = true;
            BtnDebitNotePrint.Click += BtnDebitNotePrint_Click;
            // 
            // BtnDebitNoteDelete
            // 
            BtnDebitNoteDelete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnDebitNoteDelete.Location = new Point(110, 389);
            BtnDebitNoteDelete.Name = "BtnDebitNoteDelete";
            BtnDebitNoteDelete.Size = new Size(83, 23);
            BtnDebitNoteDelete.TabIndex = 10;
            BtnDebitNoteDelete.Text = "Delete [F4]";
            BtnDebitNoteDelete.UseVisualStyleBackColor = true;
            BtnDebitNoteDelete.Click += BtnDebitNoteDelete_Click;
            // 
            // BtnDebitNoteNew
            // 
            BtnDebitNoteNew.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnDebitNoteNew.Location = new Point(18, 389);
            BtnDebitNoteNew.Name = "BtnDebitNoteNew";
            BtnDebitNoteNew.Size = new Size(83, 23);
            BtnDebitNoteNew.TabIndex = 0;
            BtnDebitNoteNew.Text = "New [F3]";
            BtnDebitNoteNew.UseVisualStyleBackColor = true;
            BtnDebitNoteNew.Click += BtnDebitNoteNew_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(489, 96);
            label4.Name = "label4";
            label4.Size = new Size(71, 13);
            label4.TabIndex = 26;
            label4.Text = "Internal Note";
            // 
            // TextBoxDebitNoteInternalNote
            // 
            TextBoxDebitNoteInternalNote.Location = new Point(492, 113);
            TextBoxDebitNoteInternalNote.MaxLength = 250;
            TextBoxDebitNoteInternalNote.Multiline = true;
            TextBoxDebitNoteInternalNote.Name = "TextBoxDebitNoteInternalNote";
            TextBoxDebitNoteInternalNote.Size = new Size(275, 44);
            TextBoxDebitNoteInternalNote.TabIndex = 5;
            TextBoxDebitNoteInternalNote.TextChanged += TextBoxDebitNoteInternalNote_TextChanged;
            TextBoxDebitNoteInternalNote.PreviewKeyDown += TextBoxDebitNoteInternalNote_PreviewKeyDown;
            // 
            // TextBoxDebitNoteNote
            // 
            TextBoxDebitNoteNote.Location = new Point(492, 54);
            TextBoxDebitNoteNote.MaxLength = 250;
            TextBoxDebitNoteNote.Multiline = true;
            TextBoxDebitNoteNote.Name = "TextBoxDebitNoteNote";
            TextBoxDebitNoteNote.Size = new Size(275, 39);
            TextBoxDebitNoteNote.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(489, 37);
            label3.Name = "label3";
            label3.Size = new Size(30, 13);
            label3.TabIndex = 23;
            label3.Text = "Note";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(12, 37);
            label2.Name = "label2";
            label2.Size = new Size(105, 13);
            label2.TabIndex = 21;
            label2.Text = "Vendor / Supplier";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(373, 37);
            label1.Name = "label1";
            label1.Size = new Size(69, 13);
            label1.TabIndex = 20;
            label1.Text = "Debit Note #";
            // 
            // TextBoxDebitNoteId
            // 
            TextBoxDebitNoteId.Location = new Point(316, 395);
            TextBoxDebitNoteId.Margin = new Padding(2);
            TextBoxDebitNoteId.Name = "TextBoxDebitNoteId";
            TextBoxDebitNoteId.Size = new Size(79, 21);
            TextBoxDebitNoteId.TabIndex = 39;
            TextBoxDebitNoteId.Visible = false;
            // 
            // DebitNoteImageList
            // 
            DebitNoteImageList.ColorDepth = ColorDepth.Depth8Bit;
            DebitNoteImageList.ImageStream = (ImageListStreamer)resources.GetObject("DebitNoteImageList.ImageStream");
            DebitNoteImageList.TransparentColor = Color.Transparent;
            DebitNoteImageList.Images.SetKeyName(0, "accounts1.png");
            DebitNoteImageList.Images.SetKeyName(1, "Supplier3.png");
            DebitNoteImageList.Images.SetKeyName(2, "customers.png");
            DebitNoteImageList.Images.SetKeyName(3, "employee.png");
            // 
            // DataGridViewDebitNoteDetailsTotal
            // 
            DataGridViewDebitNoteDetailsTotal.BackgroundColor = SystemColors.Control;
            DataGridViewDebitNoteDetailsTotal.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridViewDebitNoteDetailsTotal.ColumnHeadersVisible = false;
            DataGridViewDebitNoteDetailsTotal.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, all, DeleteTotal });
            dataGridViewCellStyle17.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle17.BackColor = SystemColors.Window;
            dataGridViewCellStyle17.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle17.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle17.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = DataGridViewTriState.False;
            DataGridViewDebitNoteDetailsTotal.DefaultCellStyle = dataGridViewCellStyle17;
            DataGridViewDebitNoteDetailsTotal.Enabled = false;
            DataGridViewDebitNoteDetailsTotal.Location = new Point(15, 354);
            DataGridViewDebitNoteDetailsTotal.Name = "DataGridViewDebitNoteDetailsTotal";
            DataGridViewDebitNoteDetailsTotal.ReadOnly = true;
            dataGridViewCellStyle18.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = SystemColors.ActiveBorder;
            dataGridViewCellStyle18.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle18.ForeColor = SystemColors.Window;
            dataGridViewCellStyle18.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle18.SelectionForeColor = SystemColors.MenuText;
            dataGridViewCellStyle18.WrapMode = DataGridViewTriState.True;
            DataGridViewDebitNoteDetailsTotal.RowHeadersDefaultCellStyle = dataGridViewCellStyle18;
            DataGridViewDebitNoteDetailsTotal.RowHeadersVisible = false;
            dataGridViewCellStyle19.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle19.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle19.SelectionForeColor = SystemColors.ActiveCaptionText;
            dataGridViewCellStyle19.WrapMode = DataGridViewTriState.True;
            DataGridViewDebitNoteDetailsTotal.RowsDefaultCellStyle = dataGridViewCellStyle19;
            DataGridViewDebitNoteDetailsTotal.ScrollBars = ScrollBars.None;
            DataGridViewDebitNoteDetailsTotal.Size = new Size(752, 23);
            DataGridViewDebitNoteDetailsTotal.TabIndex = 43;
            DataGridViewDebitNoteDetailsTotal.TabStop = false;
            DataGridViewDebitNoteDetailsTotal.CellEnter += DataGridViewDebitNoteDetailsTotal_CellEnter_1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewCellStyle15.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle15.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle15;
            dataGridViewTextBoxColumn1.Frozen = true;
            dataGridViewTextBoxColumn1.HeaderText = "Total";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            dataGridViewTextBoxColumn1.Width = 601;
            // 
            // all
            // 
            dataGridViewCellStyle16.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle16.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            all.DefaultCellStyle = dataGridViewCellStyle16;
            all.Frozen = true;
            all.HeaderText = "all";
            all.Name = "all";
            all.ReadOnly = true;
            all.Width = 108;
            // 
            // DeleteTotal
            // 
            DeleteTotal.Frozen = true;
            DeleteTotal.HeaderText = "DeleteTotal";
            DeleteTotal.Name = "DeleteTotal";
            DeleteTotal.ReadOnly = true;
            DeleteTotal.Width = 23;
            // 
            // StatusStripBill
            // 
            StatusStripBill.Items.AddRange(new ToolStripItem[] { ToolStripStatusLabelErrorDebitNote });
            StatusStripBill.Location = new Point(0, 421);
            StatusStripBill.Name = "StatusStripBill";
            StatusStripBill.Size = new Size(788, 22);
            StatusStripBill.TabIndex = 58;
            StatusStripBill.Text = "statusStrip1";
            // 
            // ToolStripStatusLabelErrorDebitNote
            // 
            ToolStripStatusLabelErrorDebitNote.Name = "ToolStripStatusLabelErrorDebitNote";
            ToolStripStatusLabelErrorDebitNote.Size = new Size(94, 17);
            ToolStripStatusLabelErrorDebitNote.Text = "                             ";
            // 
            // BtnDebitNoteExit
            // 
            BtnDebitNoteExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnDebitNoteExit.Location = new Point(692, 389);
            BtnDebitNoteExit.Name = "BtnDebitNoteExit";
            BtnDebitNoteExit.Size = new Size(75, 23);
            BtnDebitNoteExit.TabIndex = 11;
            BtnDebitNoteExit.Text = "Exit [F10]";
            BtnDebitNoteExit.UseVisualStyleBackColor = true;
            BtnDebitNoteExit.Click += BtnDebitNoteExit_Click;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewCellStyle20.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle20.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle20;
            dataGridViewTextBoxColumn2.Frozen = true;
            dataGridViewTextBoxColumn2.HeaderText = "all";
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            dataGridViewTextBoxColumn2.Width = 108;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.Frozen = true;
            dataGridViewTextBoxColumn3.HeaderText = "DeleteTotal";
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            dataGridViewTextBoxColumn3.Width = 25;
            // 
            // DateTimePickerDebitNote
            // 
            DateTimePickerDebitNote.BackColor = Color.White;
            DateTimePickerDebitNote.BorderStyle = BorderStyle.FixedSingle;
            DateTimePickerDebitNote.Date = null;
            DateTimePickerDebitNote.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DateTimePickerDebitNote.Format = "MM/dd/yyyy";
            DateTimePickerDebitNote.Location = new Point(15, 99);
            DateTimePickerDebitNote.MaxDate = new DateTime(9997, 12, 31, 9, 1, 46, 0);
            DateTimePickerDebitNote.MinDate = new DateTime(1900, 1, 1, 21, 39, 9, 0);
            DateTimePickerDebitNote.Name = "DateTimePickerDebitNote";
            DateTimePickerDebitNote.ReadOnly = false;
            DateTimePickerDebitNote.Size = new Size(93, 21);
            DateTimePickerDebitNote.TabIndex = 3;
            // 
            // DataGridViewDebitNoteDetails
            // 
            DataGridViewDebitNoteDetails.AllowUserToDeleteRows = false;
            DataGridViewDebitNoteDetails.AllowUserToResizeColumns = false;
            DataGridViewDebitNoteDetails.AllowUserToResizeRows = false;
            DataGridViewDebitNoteDetails.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            DataGridViewDebitNoteDetails.BackgroundColor = SystemColors.ControlLight;
            dataGridViewCellStyle21.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle21.BackColor = SystemColors.ControlLight;
            dataGridViewCellStyle21.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle21.SelectionBackColor = SystemColors.ControlLight;
            dataGridViewCellStyle21.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle21.WrapMode = DataGridViewTriState.True;
            DataGridViewDebitNoteDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle21;
            DataGridViewDebitNoteDetails.ColumnHeadersHeight = 20;
            DataGridViewDebitNoteDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DataGridViewDebitNoteDetails.Columns.AddRange(new DataGridViewColumn[] { SerialNo, Account, Description, Amount, Delete, Column1 });
            dataGridViewCellStyle27.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle27.BackColor = SystemColors.Window;
            dataGridViewCellStyle27.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle27.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle27.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle27.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle27.WrapMode = DataGridViewTriState.True;
            DataGridViewDebitNoteDetails.DefaultCellStyle = dataGridViewCellStyle27;
            DataGridViewDebitNoteDetails.EditMode = DataGridViewEditMode.EditOnEnter;
            DataGridViewDebitNoteDetails.EnableHeadersVisualStyles = false;
            DataGridViewDebitNoteDetails.Location = new Point(15, 163);
            DataGridViewDebitNoteDetails.Name = "DataGridViewDebitNoteDetails";
            DataGridViewDebitNoteDetails.RowHeadersVisible = false;
            DataGridViewDebitNoteDetails.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle28.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle28.SelectionForeColor = SystemColors.ActiveCaptionText;
            DataGridViewDebitNoteDetails.RowsDefaultCellStyle = dataGridViewCellStyle28;
            DataGridViewDebitNoteDetails.RowTemplate.Height = 20;
            DataGridViewDebitNoteDetails.ScrollBars = ScrollBars.Vertical;
            DataGridViewDebitNoteDetails.ShowCellToolTips = false;
            DataGridViewDebitNoteDetails.Size = new Size(752, 192);
            DataGridViewDebitNoteDetails.TabIndex = 6;
            DataGridViewDebitNoteDetails.CellClick += DataGridViewDebitNoteDetails_CellClick;
            DataGridViewDebitNoteDetails.CellEndEdit += DataGridViewDebitNoteDetails_CellEndEdit;
            DataGridViewDebitNoteDetails.CellEnter += DataGridViewDebitNoteDetails_CellEnter;
            DataGridViewDebitNoteDetails.CellFormatting += DataGridViewDebitNoteDetails_CellFormatting;
            DataGridViewDebitNoteDetails.CellLeave += DataGridViewDebitNoteDetails_CellLeave;
            DataGridViewDebitNoteDetails.DataError += DataGridViewDebitNoteDetails_DataError;
            DataGridViewDebitNoteDetails.EditingControlShowing += DataGridViewDebitNoteDetails_EditingControlShowing;
            DataGridViewDebitNoteDetails.RowsAdded += DataGridViewDebitNoteDetails_RowsAdded;
            DataGridViewDebitNoteDetails.KeyPress += DataGridViewDebitNoteDetails_KeyPress;
            // 
            // SerialNo
            // 
            dataGridViewCellStyle22.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle22.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            SerialNo.DefaultCellStyle = dataGridViewCellStyle22;
            SerialNo.HeaderText = "#";
            SerialNo.Name = "SerialNo";
            SerialNo.Resizable = DataGridViewTriState.False;
            SerialNo.SortMode = DataGridViewColumnSortMode.NotSortable;
            SerialNo.Width = 30;
            // 
            // Account
            // 
            dataGridViewCellStyle23.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle23.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle23.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle23.SelectionForeColor = SystemColors.ActiveCaptionText;
            Account.DefaultCellStyle = dataGridViewCellStyle23;
            Account.FlatStyle = FlatStyle.Flat;
            Account.HeaderText = "Account";
            Account.Name = "Account";
            Account.Resizable = DataGridViewTriState.False;
            Account.Width = 200;
            // 
            // Description
            // 
            dataGridViewCellStyle24.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle24.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle24.WrapMode = DataGridViewTriState.True;
            Description.DefaultCellStyle = dataGridViewCellStyle24;
            Description.HeaderText = "Description";
            Description.MaxInputLength = 250;
            Description.Name = "Description";
            Description.Resizable = DataGridViewTriState.False;
            Description.SortMode = DataGridViewColumnSortMode.NotSortable;
            Description.Width = 371;
            // 
            // Amount
            // 
            dataGridViewCellStyle25.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle25.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle25.NullValue = "0.00";
            Amount.DefaultCellStyle = dataGridViewCellStyle25;
            Amount.HeaderText = "Amount";
            Amount.Name = "Amount";
            Amount.Resizable = DataGridViewTriState.False;
            Amount.Width = 108;
            // 
            // Delete
            // 
            dataGridViewCellStyle26.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle26.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle26.NullValue = "X";
            Delete.DefaultCellStyle = dataGridViewCellStyle26;
            Delete.HeaderText = "...";
            Delete.Name = "Delete";
            Delete.Resizable = DataGridViewTriState.False;
            Delete.Width = 25;
            // 
            // Column1
            // 
            Column1.HeaderText = "Id";
            Column1.Name = "Column1";
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Visible = false;
            // 
            // TextBoxDebitNoteSupplier
            // 
            TextBoxDebitNoteSupplier.BackColor = SystemColors.Window;
            TextBoxDebitNoteSupplier.Id = null;
            TextBoxDebitNoteSupplier.Location = new Point(15, 53);
            TextBoxDebitNoteSupplier.MaxLength = 30;
            TextBoxDebitNoteSupplier.Name = "TextBoxDebitNoteSupplier";
            TextBoxDebitNoteSupplier.ReadOnly = true;
            TextBoxDebitNoteSupplier.Size = new Size(180, 21);
            TextBoxDebitNoteSupplier.TabIndex = 2;
            TextBoxDebitNoteSupplier.PreviewKeyDown += TextBoxDebitNoteSupplier_PreviewKeyDown;
            // 
            // BtnDebitNoteSearchSupplier
            // 
            BtnDebitNoteSearchSupplier.Location = new Point(201, 51);
            BtnDebitNoteSearchSupplier.Name = "BtnDebitNoteSearchSupplier";
            BtnDebitNoteSearchSupplier.Size = new Size(71, 23);
            BtnDebitNoteSearchSupplier.TabIndex = 12;
            BtnDebitNoteSearchSupplier.TabStop = false;
            BtnDebitNoteSearchSupplier.Text = "Search [F2]";
            BtnDebitNoteSearchSupplier.UseVisualStyleBackColor = true;
            BtnDebitNoteSearchSupplier.Click += BtnDebitNoteSearchSupplier_Click;
            // 
            // toolStripDebiteNote
            // 
            toolStripDebiteNote.BackColor = SystemColors.ControlLight;
            toolStripDebiteNote.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripDebiteNote.GripStyle = ToolStripGripStyle.Hidden;
            toolStripDebiteNote.Items.AddRange(new ToolStripItem[] { toolStripLabelDebiteNoteSearch, TextBoxSearchDebitNote, BtnSearchDebitNote });
            toolStripDebiteNote.Location = new Point(0, 0);
            toolStripDebiteNote.Name = "toolStripDebiteNote";
            toolStripDebiteNote.Padding = new Padding(5);
            toolStripDebiteNote.Size = new Size(788, 33);
            toolStripDebiteNote.TabIndex = 1;
            toolStripDebiteNote.Text = "Tool Strip Debite Note";
            // 
            // toolStripLabelDebiteNoteSearch
            // 
            toolStripLabelDebiteNoteSearch.Name = "toolStripLabelDebiteNoteSearch";
            toolStripLabelDebiteNoteSearch.Size = new Size(100, 20);
            toolStripLabelDebiteNoteSearch.Text = "Debite Note Search";
            // 
            // TextBoxSearchDebitNote
            // 
            TextBoxSearchDebitNote.BorderStyle = BorderStyle.FixedSingle;
            TextBoxSearchDebitNote.MaxLength = 30;
            TextBoxSearchDebitNote.Name = "TextBoxSearchDebitNote";
            TextBoxSearchDebitNote.Size = new Size(150, 23);
            TextBoxSearchDebitNote.KeyDown += TextBoxSearchDebitNote_KeyDown;
            // 
            // BtnSearchDebitNote
            // 
            BtnSearchDebitNote.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnSearchDebitNote.Image = (Image)resources.GetObject("BtnSearchDebitNote.Image");
            BtnSearchDebitNote.ImageTransparentColor = Color.Magenta;
            BtnSearchDebitNote.Name = "BtnSearchDebitNote";
            BtnSearchDebitNote.Size = new Size(24, 20);
            BtnSearchDebitNote.Text = "Go";
            BtnSearchDebitNote.Click += BtnSearchDebitNote_Click;
            // 
            // BtnDebitNoteNewSupplier
            // 
            BtnDebitNoteNewSupplier.ButtonText = "New [F3]";
            BtnDebitNoteNewSupplier.ImageList = DebitNoteImageList;
            BtnDebitNoteNewSupplier.Items = (System.Collections.ObjectModel.Collection<string>)resources.GetObject("BtnDebitNoteNewSupplier.Items");
            BtnDebitNoteNewSupplier.Location = new Point(277, 51);
            BtnDebitNoteNewSupplier.Margin = new Padding(4, 3, 4, 3);
            BtnDebitNoteNewSupplier.Name = "BtnDebitNoteNewSupplier";
            BtnDebitNoteNewSupplier.Size = new Size(80, 27);
            BtnDebitNoteNewSupplier.TabIndex = 199;
            BtnDebitNoteNewSupplier.TabStop = false;
            BtnDebitNoteNewSupplier.ItemClickedEvent += BtnExpenseNewSupplier_ItemClickedEvent;
            // 
            // LastDebitNoteRefNo
            // 
            LastDebitNoteRefNo.AutoSize = true;
            LastDebitNoteRefNo.Font = new Font("Tahoma", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            LastDebitNoteRefNo.Location = new Point(373, 97);
            LastDebitNoteRefNo.Name = "LastDebitNoteRefNo";
            LastDebitNoteRefNo.Size = new Size(55, 16);
            LastDebitNoteRefNo.TabIndex = 200;
            LastDebitNoteRefNo.Text = "000000";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label10.Location = new Point(373, 83);
            label10.Name = "label10";
            label10.Size = new Size(89, 13);
            label10.TabIndex = 201;
            label10.Text = "last Debit Note #";
            // 
            // FormDebitNote
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(788, 443);
            Controls.Add(LastDebitNoteRefNo);
            Controls.Add(label10);
            Controls.Add(BtnDebitNoteNewSupplier);
            Controls.Add(toolStripDebiteNote);
            Controls.Add(TextBoxDebitNoteSupplier);
            Controls.Add(BtnDebitNoteSearchSupplier);
            Controls.Add(BtnDebitNoteExit);
            Controls.Add(TextBoxDebitNoteNote);
            Controls.Add(TextBoxDebitNoteInternalNote);
            Controls.Add(DateTimePickerDebitNote);
            Controls.Add(StatusStripBill);
            Controls.Add(DataGridViewDebitNoteDetailsTotal);
            Controls.Add(TextBoxDebitNoteId);
            Controls.Add(label8);
            Controls.Add(DebitNoteRefNo);
            Controls.Add(BtnDebitNoteSave);
            Controls.Add(BtnDebitNoteCancel);
            Controls.Add(BtnDebitNotePrint);
            Controls.Add(BtnDebitNoteDelete);
            Controls.Add(BtnDebitNoteNew);
            Controls.Add(DataGridViewDebitNoteDetails);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormDebitNote";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Debit Note / Vendor Credit";
            FormClosing += FormDebitNote_FormClosing;
            Load += FormDebitNote_Load;
            Controls.SetChildIndex(checkBoxIsPatient, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(label2, 0);
            Controls.SetChildIndex(label3, 0);
            Controls.SetChildIndex(label4, 0);
            Controls.SetChildIndex(DataGridViewDebitNoteDetails, 0);
            Controls.SetChildIndex(BtnDebitNoteNew, 0);
            Controls.SetChildIndex(BtnDebitNoteDelete, 0);
            Controls.SetChildIndex(BtnDebitNotePrint, 0);
            Controls.SetChildIndex(BtnDebitNoteCancel, 0);
            Controls.SetChildIndex(BtnDebitNoteSave, 0);
            Controls.SetChildIndex(DebitNoteRefNo, 0);
            Controls.SetChildIndex(label8, 0);
            Controls.SetChildIndex(TextBoxDebitNoteId, 0);
            Controls.SetChildIndex(DataGridViewDebitNoteDetailsTotal, 0);
            Controls.SetChildIndex(StatusStripBill, 0);
            Controls.SetChildIndex(DateTimePickerDebitNote, 0);
            Controls.SetChildIndex(TextBoxDebitNoteInternalNote, 0);
            Controls.SetChildIndex(TextBoxDebitNoteNote, 0);
            Controls.SetChildIndex(BtnDebitNoteExit, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(BtnDebitNoteSearchSupplier, 0);
            Controls.SetChildIndex(TextBoxDebitNoteSupplier, 0);
            Controls.SetChildIndex(toolStripDebiteNote, 0);
            Controls.SetChildIndex(BtnDebitNoteNewSupplier, 0);
            Controls.SetChildIndex(label10, 0);
            Controls.SetChildIndex(LastDebitNoteRefNo, 0);
            ((System.ComponentModel.ISupportInitialize)DataGridViewDebitNoteDetailsTotal).EndInit();
            StatusStripBill.ResumeLayout(false);
            StatusStripBill.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)DataGridViewDebitNoteDetails).EndInit();
            toolStripDebiteNote.ResumeLayout(false);
            toolStripDebiteNote.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label DebitNoteRefNo;
        private System.Windows.Forms.Button BtnDebitNoteSave;
        private System.Windows.Forms.Button BtnDebitNoteCancel;
        private System.Windows.Forms.Button BtnDebitNotePrint;
        private System.Windows.Forms.Button BtnDebitNoteDelete;
        private System.Windows.Forms.Button BtnDebitNoteNew;
        private fa.views.controls.DataViewVerticalScroll DataGridViewDebitNoteDetails;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TextBoxDebitNoteInternalNote;
        private System.Windows.Forms.TextBox TextBoxDebitNoteNote;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBoxDebitNoteId;
        private System.Windows.Forms.ImageList DebitNoteImageList;
        private System.Windows.Forms.DataGridView DataGridViewDebitNoteDetailsTotal;
        private System.Windows.Forms.StatusStrip StatusStripBill;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabelErrorDebitNote;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private controls.text.DateWithCalendar DateTimePickerDebitNote;
        private System.Windows.Forms.Button BtnDebitNoteExit;
        private controls.text.IDTextBox TextBoxDebitNoteSupplier;
        private System.Windows.Forms.Button BtnDebitNoteSearchSupplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn SerialNo;
        private System.Windows.Forms.DataGridViewComboBoxColumn Account;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private controls.grid.DataGridViewCurrencyColumn Amount;
        private System.Windows.Forms.DataGridViewButtonColumn Delete;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn all;
        private DataGridViewTextBoxColumn DeleteTotal;
        private ToolStrip toolStripDebiteNote;
        private ToolStripLabel toolStripLabelDebiteNoteSearch;
        private ToolStripTextBox TextBoxSearchDebitNote;
        private ToolStripButton BtnSearchDebitNote;
        private Dropdown_Button.UserControlButtonWithMenu BtnDebitNoteNewSupplier;
        private Label LastDebitNoteRefNo;
        private Label label10;
    }
}