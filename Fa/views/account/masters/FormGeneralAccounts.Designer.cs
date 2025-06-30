namespace fa.views.account.masters
{
    partial class FormGeneralAccounts
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGeneralAccounts));
            groupBox1 = new GroupBox();
            ComboBoxGeneralAccountParentAccount = new controls.ComboBoxSwapTextBox();
            ComboBoxGeneralAccountGroup = new controls.ComboBoxSwapTextBox();
            ComboBoxGeneralAccountDetailGroup = new controls.ComboBoxSwapTextBox();
            ComboBoxBalanceType = new controls.ComboBoxSwapTextBox();
            TextBoxGeneralAccountBalance = new controls.text.CurrencyTextBox();
            DateTimePickerGeneralAccount = new controls.text.DateWithCalendar();
            TextBoxGeneralAccountDisplayAs = new TextBox();
            TextBoxGeneralAccountDescription = new TextBox();
            TextBoxGeneralAccountName = new TextBox();
            label1 = new Label();
            CheckBoxGeneralAccount = new CheckBox();
            LabelGeneralAccountParentAccount = new Label();
            LabelGeneralAccountDescription = new Label();
            LabelGeneralAccountName = new Label();
            LabelGeneralAccountBalance = new Label();
            LabelGeneralAccountAsof = new Label();
            LabelGeneralAccountDetailType = new Label();
            LabelGeneralAccountType = new Label();
            groupBox2 = new GroupBox();
            WebBrowserGeneralAccount = new WebBrowser();
            BtnGeneralAccountSave = new Button();
            BtnGeneralAccountCancel = new Button();
            BtnGeneralAccountEdit = new Button();
            BtnGeneralAccountNew = new Button();
            BtnGeneralAccountDelete = new Button();
            TextBoxGeneralAccountId = new MaskedTextBox();
            TreeViewGeneralAccount = new TreeView();
            ImageListGeneralAccount = new ImageList(components);
            BtnCurrencyExit = new Button();
            statusStrip1 = new StatusStrip();
            ToolStripStatusLabelErrorGeneralAccount = new ToolStripStatusLabel();
            TextBoxGeneralAccountSearch = new controls.text.DelayedTextChangeTextBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            statusStrip1.SuspendLayout();
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
            // groupBox1
            // 
            groupBox1.BackColor = SystemColors.Window;
            groupBox1.Controls.Add(ComboBoxGeneralAccountParentAccount);
            groupBox1.Controls.Add(ComboBoxGeneralAccountGroup);
            groupBox1.Controls.Add(ComboBoxGeneralAccountDetailGroup);
            groupBox1.Controls.Add(ComboBoxBalanceType);
            groupBox1.Controls.Add(TextBoxGeneralAccountBalance);
            groupBox1.Controls.Add(DateTimePickerGeneralAccount);
            groupBox1.Controls.Add(TextBoxGeneralAccountDisplayAs);
            groupBox1.Controls.Add(TextBoxGeneralAccountDescription);
            groupBox1.Controls.Add(TextBoxGeneralAccountName);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(CheckBoxGeneralAccount);
            groupBox1.Controls.Add(LabelGeneralAccountParentAccount);
            groupBox1.Controls.Add(LabelGeneralAccountDescription);
            groupBox1.Controls.Add(LabelGeneralAccountName);
            groupBox1.Controls.Add(LabelGeneralAccountBalance);
            groupBox1.Controls.Add(LabelGeneralAccountAsof);
            groupBox1.Controls.Add(LabelGeneralAccountDetailType);
            groupBox1.Controls.Add(LabelGeneralAccountType);
            groupBox1.Controls.Add(groupBox2);
            groupBox1.Location = new Point(257, 13);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(593, 370);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Account Detail";
            // 
            // ComboBoxGeneralAccountParentAccount
            // 
            ComboBoxGeneralAccountParentAccount.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxGeneralAccountParentAccount.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxGeneralAccountParentAccount.FormattingEnabled = true;
            ComboBoxGeneralAccountParentAccount.Location = new Point(18, 206);
            ComboBoxGeneralAccountParentAccount.Name = "ComboBoxGeneralAccountParentAccount";
            ComboBoxGeneralAccountParentAccount.Size = new Size(243, 21);
            ComboBoxGeneralAccountParentAccount.TabIndex = 9;
            ComboBoxGeneralAccountParentAccount.TxtVisible = true;
            ComboBoxGeneralAccountParentAccount.KeyPress += ComboBoxGeneralAccountParentAccount_KeyPress;
            // 
            // ComboBoxGeneralAccountGroup
            // 
            ComboBoxGeneralAccountGroup.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxGeneralAccountGroup.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxGeneralAccountGroup.FormattingEnabled = true;
            ComboBoxGeneralAccountGroup.Location = new Point(18, 251);
            ComboBoxGeneralAccountGroup.Name = "ComboBoxGeneralAccountGroup";
            ComboBoxGeneralAccountGroup.Size = new Size(243, 21);
            ComboBoxGeneralAccountGroup.TabIndex = 10;
            ComboBoxGeneralAccountGroup.TxtVisible = true;
            ComboBoxGeneralAccountGroup.SelectedIndexChanged += ComboBoxGeneralAccountGroup_TextChanged;
            ComboBoxGeneralAccountGroup.TextChanged += ComboBoxGeneralAccountGroup_TextChanged;
            ComboBoxGeneralAccountGroup.KeyPress += ComboBoxGeneralAccountGroup_KeyPress;
            // 
            // ComboBoxGeneralAccountDetailGroup
            // 
            ComboBoxGeneralAccountDetailGroup.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxGeneralAccountDetailGroup.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxGeneralAccountDetailGroup.FormattingEnabled = true;
            ComboBoxGeneralAccountDetailGroup.Location = new Point(19, 294);
            ComboBoxGeneralAccountDetailGroup.Name = "ComboBoxGeneralAccountDetailGroup";
            ComboBoxGeneralAccountDetailGroup.Size = new Size(243, 21);
            ComboBoxGeneralAccountDetailGroup.TabIndex = 11;
            ComboBoxGeneralAccountDetailGroup.TxtVisible = true;
            ComboBoxGeneralAccountDetailGroup.SelectedIndexChanged += ComboBoxGeneralAccountDetailType_SelectedIndexChanged;
            ComboBoxGeneralAccountDetailGroup.KeyPress += ComboBoxGeneralAccountDetailType_KeyPress;
            // 
            // ComboBoxBalanceType
            // 
            ComboBoxBalanceType.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxBalanceType.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxBalanceType.FormattingEnabled = true;
            ComboBoxBalanceType.Items.AddRange(new object[] { "DR", "CR" });
            ComboBoxBalanceType.Location = new Point(122, 335);
            ComboBoxBalanceType.Name = "ComboBoxBalanceType";
            ComboBoxBalanceType.Size = new Size(40, 21);
            ComboBoxBalanceType.TabIndex = 13;
            ComboBoxBalanceType.TxtVisible = true;
            // 
            // TextBoxGeneralAccountBalance
            // 
            TextBoxGeneralAccountBalance.BackColor = SystemColors.Window;
            TextBoxGeneralAccountBalance.Decimals = 2;
            TextBoxGeneralAccountBalance.Length = 10;
            TextBoxGeneralAccountBalance.Location = new Point(18, 335);
            TextBoxGeneralAccountBalance.Name = "TextBoxGeneralAccountBalance";
            TextBoxGeneralAccountBalance.ReadOnly = true;
            TextBoxGeneralAccountBalance.Size = new Size(100, 21);
            TextBoxGeneralAccountBalance.TabIndex = 12;
            TextBoxGeneralAccountBalance.Text = "0.00";
            TextBoxGeneralAccountBalance.TextAlign = HorizontalAlignment.Right;
            // 
            // DateTimePickerGeneralAccount
            // 
            DateTimePickerGeneralAccount.BackColor = Color.White;
            DateTimePickerGeneralAccount.BorderStyle = BorderStyle.FixedSingle;
            DateTimePickerGeneralAccount.Date = null;
            DateTimePickerGeneralAccount.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DateTimePickerGeneralAccount.Format = "MM/dd/yyyy";
            DateTimePickerGeneralAccount.Location = new Point(169, 335);
            DateTimePickerGeneralAccount.MaxDate = new DateTime(9997, 12, 31, 7, 41, 36, 0);
            DateTimePickerGeneralAccount.MinDate = new DateTime(1900, 1, 1, 19, 57, 16, 0);
            DateTimePickerGeneralAccount.Name = "DateTimePickerGeneralAccount";
            DateTimePickerGeneralAccount.ReadOnly = false;
            DateTimePickerGeneralAccount.Size = new Size(93, 21);
            DateTimePickerGeneralAccount.TabIndex = 14;
            // 
            // TextBoxGeneralAccountDisplayAs
            // 
            TextBoxGeneralAccountDisplayAs.BackColor = SystemColors.Window;
            TextBoxGeneralAccountDisplayAs.Location = new Point(19, 78);
            TextBoxGeneralAccountDisplayAs.MaxLength = 100;
            TextBoxGeneralAccountDisplayAs.Name = "TextBoxGeneralAccountDisplayAs";
            TextBoxGeneralAccountDisplayAs.ReadOnly = true;
            TextBoxGeneralAccountDisplayAs.Size = new Size(554, 21);
            TextBoxGeneralAccountDisplayAs.TabIndex = 6;
            TextBoxGeneralAccountDisplayAs.KeyDown += TextBoxGeneralAccountName_KeyDown;
            TextBoxGeneralAccountDisplayAs.KeyPress += TextBoxGeneralAccountName_KeyPress;
            TextBoxGeneralAccountDisplayAs.MouseDown += TextBoxGeneralAccountDisplayAs_MouseDown;
            // 
            // TextBoxGeneralAccountDescription
            // 
            TextBoxGeneralAccountDescription.BackColor = SystemColors.Window;
            TextBoxGeneralAccountDescription.Location = new Point(19, 122);
            TextBoxGeneralAccountDescription.MaxLength = 250;
            TextBoxGeneralAccountDescription.Multiline = true;
            TextBoxGeneralAccountDescription.Name = "TextBoxGeneralAccountDescription";
            TextBoxGeneralAccountDescription.ReadOnly = true;
            TextBoxGeneralAccountDescription.Size = new Size(554, 38);
            TextBoxGeneralAccountDescription.TabIndex = 7;
            // 
            // TextBoxGeneralAccountName
            // 
            TextBoxGeneralAccountName.AccessibleDescription = "";
            TextBoxGeneralAccountName.AccessibleName = "ffff";
            TextBoxGeneralAccountName.BackColor = SystemColors.Window;
            TextBoxGeneralAccountName.Location = new Point(19, 35);
            TextBoxGeneralAccountName.MaxLength = 30;
            TextBoxGeneralAccountName.Name = "TextBoxGeneralAccountName";
            TextBoxGeneralAccountName.ReadOnly = true;
            TextBoxGeneralAccountName.Size = new Size(554, 21);
            TextBoxGeneralAccountName.TabIndex = 5;
            TextBoxGeneralAccountName.KeyDown += TextBoxGeneralAccountName_KeyDown;
            TextBoxGeneralAccountName.KeyPress += TextBoxGeneralAccountName_KeyPress;
            TextBoxGeneralAccountName.MouseDown += TextBoxGeneralAccountName_MouseDown;
            TextBoxGeneralAccountName.PreviewKeyDown += TextBoxGeneralAccountName_PreviewKeyDown;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(16, 60);
            label1.Name = "label1";
            label1.Size = new Size(56, 13);
            label1.TabIndex = 0;
            label1.Text = "Display As";
            // 
            // CheckBoxGeneralAccount
            // 
            CheckBoxGeneralAccount.AutoSize = true;
            CheckBoxGeneralAccount.Enabled = false;
            CheckBoxGeneralAccount.Location = new Point(19, 166);
            CheckBoxGeneralAccount.Name = "CheckBoxGeneralAccount";
            CheckBoxGeneralAccount.Size = new Size(86, 17);
            CheckBoxGeneralAccount.TabIndex = 8;
            CheckBoxGeneralAccount.Text = "Sub Account";
            CheckBoxGeneralAccount.UseVisualStyleBackColor = true;
            CheckBoxGeneralAccount.CheckedChanged += CheckBoxGeneralAccount_CheckedChanged;
            // 
            // LabelGeneralAccountParentAccount
            // 
            LabelGeneralAccountParentAccount.AutoSize = true;
            LabelGeneralAccountParentAccount.Location = new Point(16, 188);
            LabelGeneralAccountParentAccount.Name = "LabelGeneralAccountParentAccount";
            LabelGeneralAccountParentAccount.Size = new Size(81, 13);
            LabelGeneralAccountParentAccount.TabIndex = 3;
            LabelGeneralAccountParentAccount.Text = "Parent Account";
            // 
            // LabelGeneralAccountDescription
            // 
            LabelGeneralAccountDescription.AutoSize = true;
            LabelGeneralAccountDescription.Location = new Point(16, 103);
            LabelGeneralAccountDescription.Name = "LabelGeneralAccountDescription";
            LabelGeneralAccountDescription.Size = new Size(60, 13);
            LabelGeneralAccountDescription.TabIndex = 1;
            LabelGeneralAccountDescription.Text = "Description";
            // 
            // LabelGeneralAccountName
            // 
            LabelGeneralAccountName.AutoSize = true;
            LabelGeneralAccountName.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelGeneralAccountName.Location = new Point(16, 18);
            LabelGeneralAccountName.Name = "LabelGeneralAccountName";
            LabelGeneralAccountName.Size = new Size(39, 13);
            LabelGeneralAccountName.TabIndex = 0;
            LabelGeneralAccountName.Text = "Name";
            // 
            // LabelGeneralAccountBalance
            // 
            LabelGeneralAccountBalance.AutoSize = true;
            LabelGeneralAccountBalance.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelGeneralAccountBalance.Location = new Point(16, 319);
            LabelGeneralAccountBalance.Name = "LabelGeneralAccountBalance";
            LabelGeneralAccountBalance.Size = new Size(44, 13);
            LabelGeneralAccountBalance.TabIndex = 0;
            LabelGeneralAccountBalance.Text = "Balance";
            // 
            // LabelGeneralAccountAsof
            // 
            LabelGeneralAccountAsof.AutoSize = true;
            LabelGeneralAccountAsof.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelGeneralAccountAsof.Location = new Point(166, 319);
            LabelGeneralAccountAsof.Name = "LabelGeneralAccountAsof";
            LabelGeneralAccountAsof.Size = new Size(36, 13);
            LabelGeneralAccountAsof.TabIndex = 0;
            LabelGeneralAccountAsof.Text = "As Of";
            // 
            // LabelGeneralAccountDetailType
            // 
            LabelGeneralAccountDetailType.AutoSize = true;
            LabelGeneralAccountDetailType.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelGeneralAccountDetailType.Location = new Point(16, 276);
            LabelGeneralAccountDetailType.Name = "LabelGeneralAccountDetailType";
            LabelGeneralAccountDetailType.Size = new Size(71, 13);
            LabelGeneralAccountDetailType.TabIndex = 0;
            LabelGeneralAccountDetailType.Text = "Detail Type";
            // 
            // LabelGeneralAccountType
            // 
            LabelGeneralAccountType.AutoSize = true;
            LabelGeneralAccountType.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelGeneralAccountType.Location = new Point(16, 231);
            LabelGeneralAccountType.Name = "LabelGeneralAccountType";
            LabelGeneralAccountType.Size = new Size(35, 13);
            LabelGeneralAccountType.TabIndex = 0;
            LabelGeneralAccountType.Text = "Type";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(WebBrowserGeneralAccount);
            groupBox2.Location = new Point(279, 188);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(294, 168);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Type Description";
            // 
            // WebBrowserGeneralAccount
            // 
            WebBrowserGeneralAccount.Location = new Point(6, 14);
            WebBrowserGeneralAccount.MinimumSize = new Size(20, 20);
            WebBrowserGeneralAccount.Name = "WebBrowserGeneralAccount";
            WebBrowserGeneralAccount.Size = new Size(282, 145);
            WebBrowserGeneralAccount.TabIndex = 23;
            WebBrowserGeneralAccount.TabStop = false;
            // 
            // BtnGeneralAccountSave
            // 
            BtnGeneralAccountSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnGeneralAccountSave.Location = new Point(670, 397);
            BtnGeneralAccountSave.Name = "BtnGeneralAccountSave";
            BtnGeneralAccountSave.Size = new Size(83, 23);
            BtnGeneralAccountSave.TabIndex = 15;
            BtnGeneralAccountSave.Text = "Save [F8]";
            BtnGeneralAccountSave.UseVisualStyleBackColor = true;
            BtnGeneralAccountSave.Click += BtnGeneralAccountSave_Click;
            // 
            // BtnGeneralAccountCancel
            // 
            BtnGeneralAccountCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnGeneralAccountCancel.Location = new Point(581, 397);
            BtnGeneralAccountCancel.Name = "BtnGeneralAccountCancel";
            BtnGeneralAccountCancel.Size = new Size(83, 23);
            BtnGeneralAccountCancel.TabIndex = 0;
            BtnGeneralAccountCancel.Text = "Cancel [Esc]";
            BtnGeneralAccountCancel.UseVisualStyleBackColor = true;
            BtnGeneralAccountCancel.Click += BtnGeneralAccountCancel_Click;
            // 
            // BtnGeneralAccountEdit
            // 
            BtnGeneralAccountEdit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnGeneralAccountEdit.Location = new Point(195, 397);
            BtnGeneralAccountEdit.Name = "BtnGeneralAccountEdit";
            BtnGeneralAccountEdit.Size = new Size(83, 23);
            BtnGeneralAccountEdit.TabIndex = 4;
            BtnGeneralAccountEdit.Text = "Edit [F7]";
            BtnGeneralAccountEdit.UseVisualStyleBackColor = true;
            BtnGeneralAccountEdit.Click += BtnGeneralAccountEdit_Click;
            // 
            // BtnGeneralAccountNew
            // 
            BtnGeneralAccountNew.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnGeneralAccountNew.Location = new Point(17, 397);
            BtnGeneralAccountNew.Name = "BtnGeneralAccountNew";
            BtnGeneralAccountNew.Size = new Size(83, 23);
            BtnGeneralAccountNew.TabIndex = 2;
            BtnGeneralAccountNew.Text = "New [F3]";
            BtnGeneralAccountNew.UseVisualStyleBackColor = true;
            BtnGeneralAccountNew.Click += BtnGeneralAccountNew_Click;
            // 
            // BtnGeneralAccountDelete
            // 
            BtnGeneralAccountDelete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnGeneralAccountDelete.Location = new Point(106, 397);
            BtnGeneralAccountDelete.Name = "BtnGeneralAccountDelete";
            BtnGeneralAccountDelete.Size = new Size(83, 23);
            BtnGeneralAccountDelete.TabIndex = 3;
            BtnGeneralAccountDelete.Text = "Delete [F4]";
            BtnGeneralAccountDelete.UseVisualStyleBackColor = true;
            BtnGeneralAccountDelete.Click += BtnGeneralAccountDelete_Click;
            // 
            // TextBoxGeneralAccountId
            // 
            TextBoxGeneralAccountId.Location = new Point(437, 402);
            TextBoxGeneralAccountId.Name = "TextBoxGeneralAccountId";
            TextBoxGeneralAccountId.Size = new Size(100, 21);
            TextBoxGeneralAccountId.TabIndex = 18;
            TextBoxGeneralAccountId.TabStop = false;
            TextBoxGeneralAccountId.Visible = false;
            // 
            // TreeViewGeneralAccount
            // 
            TreeViewGeneralAccount.Enabled = false;
            TreeViewGeneralAccount.HideSelection = false;
            TreeViewGeneralAccount.Location = new Point(12, 39);
            TreeViewGeneralAccount.Name = "TreeViewGeneralAccount";
            TreeViewGeneralAccount.Size = new Size(239, 342);
            TreeViewGeneralAccount.TabIndex = 1;
            TreeViewGeneralAccount.AfterSelect += TreeViewGeneralAccount_AfterSelect;
            // 
            // ImageListGeneralAccount
            // 
            ImageListGeneralAccount.ColorDepth = ColorDepth.Depth8Bit;
            ImageListGeneralAccount.ImageStream = (ImageListStreamer)resources.GetObject("ImageListGeneralAccount.ImageStream");
            ImageListGeneralAccount.TransparentColor = Color.Transparent;
            ImageListGeneralAccount.Images.SetKeyName(0, "accounts1.png");
            // 
            // BtnCurrencyExit
            // 
            BtnCurrencyExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCurrencyExit.Location = new Point(759, 397);
            BtnCurrencyExit.Name = "BtnCurrencyExit";
            BtnCurrencyExit.Size = new Size(83, 23);
            BtnCurrencyExit.TabIndex = 0;
            BtnCurrencyExit.Text = "Exit [F10]";
            BtnCurrencyExit.UseVisualStyleBackColor = true;
            BtnCurrencyExit.Click += BtnCurrencyExit_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ToolStripStatusLabelErrorGeneralAccount });
            statusStrip1.Location = new Point(0, 432);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(860, 22);
            statusStrip1.TabIndex = 56;
            statusStrip1.Text = "sdfdsf sdf sdf";
            // 
            // ToolStripStatusLabelErrorGeneralAccount
            // 
            ToolStripStatusLabelErrorGeneralAccount.Name = "ToolStripStatusLabelErrorGeneralAccount";
            ToolStripStatusLabelErrorGeneralAccount.Size = new Size(151, 17);
            ToolStripStatusLabelErrorGeneralAccount.Text = "                                                ";
            // 
            // TextBoxGeneralAccountSearch
            // 
            TextBoxGeneralAccountSearch.BackColor = SystemColors.Window;
            TextBoxGeneralAccountSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxGeneralAccountSearch.Delay = true;
            TextBoxGeneralAccountSearch.DelayTime = 1000;
            TextBoxGeneralAccountSearch.Location = new Point(12, 13);
            TextBoxGeneralAccountSearch.MaxLength = 35;
            TextBoxGeneralAccountSearch.Name = "TextBoxGeneralAccountSearch";
            TextBoxGeneralAccountSearch.Searchstartfrom = 2;
            TextBoxGeneralAccountSearch.Size = new Size(239, 21);
            TextBoxGeneralAccountSearch.TabIndex = 0;
            TextBoxGeneralAccountSearch.TextChanged += TextBoxGeneralAccountSearch_TextChanged;
            TextBoxGeneralAccountSearch.KeyDown += TextBoxGeneralAccountSearch_KeyDown;
            // 
            // FormGeneralAccounts
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(860, 454);
            Controls.Add(TextBoxGeneralAccountSearch);
            Controls.Add(statusStrip1);
            Controls.Add(BtnCurrencyExit);
            Controls.Add(TreeViewGeneralAccount);
            Controls.Add(TextBoxGeneralAccountId);
            Controls.Add(BtnGeneralAccountDelete);
            Controls.Add(BtnGeneralAccountNew);
            Controls.Add(BtnGeneralAccountCancel);
            Controls.Add(BtnGeneralAccountSave);
            Controls.Add(BtnGeneralAccountEdit);
            Controls.Add(groupBox1);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormGeneralAccounts";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "General Account";
            FormClosing += FormGeneralAccounts_FormClosing;
            Load += FormGeneralAccounts_Load;
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(groupBox1, 0);
            Controls.SetChildIndex(BtnGeneralAccountEdit, 0);
            Controls.SetChildIndex(BtnGeneralAccountSave, 0);
            Controls.SetChildIndex(BtnGeneralAccountCancel, 0);
            Controls.SetChildIndex(BtnGeneralAccountNew, 0);
            Controls.SetChildIndex(BtnGeneralAccountDelete, 0);
            Controls.SetChildIndex(TextBoxGeneralAccountId, 0);
            Controls.SetChildIndex(TreeViewGeneralAccount, 0);
            Controls.SetChildIndex(BtnCurrencyExit, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(TextBoxGeneralAccountSearch, 0);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private GroupBox groupBox1;
        private Button BtnGeneralAccountNew;
        private Button BtnGeneralAccountEdit;
        private Button BtnGeneralAccountDelete;
        private Button BtnGeneralAccountSave;
        private Button BtnGeneralAccountCancel;
        private Label LabelGeneralAccountName;
        private Label LabelGeneralAccountDescription;
        private Label LabelGeneralAccountParentAccount;
        private TextBox TextBoxGeneralAccountName;
        private TextBox TextBoxGeneralAccountDescription;
        private CheckBox CheckBoxGeneralAccount;
        private Label LabelGeneralAccountBalance;
        private Label LabelGeneralAccountAsof;
        private Label LabelGeneralAccountDetailType;
        private Label LabelGeneralAccountType;
        private WebBrowser WebBrowserGeneralAccount;
        private MaskedTextBox TextBoxGeneralAccountId;
        private Button BtnCurrencyExit;
        private Label label1;
        private TextBox TextBoxGeneralAccountDisplayAs;
        private ImageList ImageListGeneralAccount;
        public TreeView TreeViewGeneralAccount;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel ToolStripStatusLabelErrorGeneralAccount;
        private controls.ComboBoxSwapTextBox ComboBoxGeneralAccountDetailGroup;
        private controls.ComboBoxSwapTextBox ComboBoxGeneralAccountGroup;
        private controls.ComboBoxSwapTextBox ComboBoxGeneralAccountParentAccount;
        private controls.text.DateWithCalendar DateTimePickerGeneralAccount;
        private controls.text.CurrencyTextBox TextBoxGeneralAccountBalance;
        private controls.ComboBoxSwapTextBox ComboBoxBalanceType;
        private GroupBox groupBox2;
        private controls.text.DelayedTextChangeTextBox TextBoxGeneralAccountSearch;
    }
}