namespace fa.views.account.masters
{
    partial class FormPaymentMethod
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPaymentMethod));
            BtnPaymentMethodDelete = new Button();
            BtnPaymentMethodNew = new Button();
            BtnPaymentMethodCancel = new Button();
            BtnPaymentMethodSave = new Button();
            BtnPaymentMethodEdit = new Button();
            ListBoxPaymentMethod = new ListBox();
            BtnPaymentMethodExit = new Button();
            TextBoxPaymentMethodId = new TextBox();
            StatusStripPayment = new StatusStrip();
            ToolStripStatusLabelErrorPayment = new ToolStripStatusLabel();
            TabControlPaymentMethod = new TabControl();
            TabDetails = new TabPage();
            ComboBoxPaymentMethodAccount = new controls.ComboBoxSwapTextBox();
            TextBoxPaymentMethodDisplayAs = new TextBox();
            TextBoxPaymentMethodName = new TextBox();
            LabelCurrencyName = new Label();
            label2 = new Label();
            LabelCurrencyDisplayas = new Label();
            label1 = new Label();
            groupBox1 = new GroupBox();
            RadioButtonPaymentMethodNo = new RadioButton();
            RadioButtonPaymentMethodYes = new RadioButton();
            TextBoxPaymentMethodSearch = new controls.text.DelayedTextChangeTextBox();
            StatusStripPayment.SuspendLayout();
            TabControlPaymentMethod.SuspendLayout();
            TabDetails.SuspendLayout();
            groupBox1.SuspendLayout();
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
            // BtnPaymentMethodDelete
            // 
            BtnPaymentMethodDelete.Enabled = false;
            BtnPaymentMethodDelete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPaymentMethodDelete.Location = new Point(110, 343);
            BtnPaymentMethodDelete.Name = "BtnPaymentMethodDelete";
            BtnPaymentMethodDelete.Size = new Size(83, 23);
            BtnPaymentMethodDelete.TabIndex = 4;
            BtnPaymentMethodDelete.Text = "Delete [F4]";
            BtnPaymentMethodDelete.UseVisualStyleBackColor = true;
            BtnPaymentMethodDelete.Click += BtnPaymentMethodDelete_Click;
            // 
            // BtnPaymentMethodNew
            // 
            BtnPaymentMethodNew.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPaymentMethodNew.Location = new Point(21, 343);
            BtnPaymentMethodNew.Name = "BtnPaymentMethodNew";
            BtnPaymentMethodNew.Size = new Size(83, 23);
            BtnPaymentMethodNew.TabIndex = 3;
            BtnPaymentMethodNew.Text = "New [F3]";
            BtnPaymentMethodNew.UseVisualStyleBackColor = true;
            BtnPaymentMethodNew.Click += BtnPaymentMethodNew_Click;
            // 
            // BtnPaymentMethodCancel
            // 
            BtnPaymentMethodCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPaymentMethodCancel.Location = new Point(570, 341);
            BtnPaymentMethodCancel.Name = "BtnPaymentMethodCancel";
            BtnPaymentMethodCancel.Size = new Size(83, 23);
            BtnPaymentMethodCancel.TabIndex = 11;
            BtnPaymentMethodCancel.Text = "Cancel [Esc]";
            BtnPaymentMethodCancel.UseVisualStyleBackColor = true;
            BtnPaymentMethodCancel.Click += BtnPaymentMethodCancel_Click;
            // 
            // BtnPaymentMethodSave
            // 
            BtnPaymentMethodSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPaymentMethodSave.Location = new Point(659, 340);
            BtnPaymentMethodSave.Name = "BtnPaymentMethodSave";
            BtnPaymentMethodSave.Size = new Size(83, 23);
            BtnPaymentMethodSave.TabIndex = 10;
            BtnPaymentMethodSave.Text = "Save [F8]";
            BtnPaymentMethodSave.UseVisualStyleBackColor = true;
            BtnPaymentMethodSave.Click += BtnPaymentMethodSave_Click;
            BtnPaymentMethodSave.PreviewKeyDown += BtnPaymentMethodSave_PreviewKeyDown;
            // 
            // BtnPaymentMethodEdit
            // 
            BtnPaymentMethodEdit.Enabled = false;
            BtnPaymentMethodEdit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPaymentMethodEdit.Location = new Point(199, 343);
            BtnPaymentMethodEdit.Name = "BtnPaymentMethodEdit";
            BtnPaymentMethodEdit.Size = new Size(83, 23);
            BtnPaymentMethodEdit.TabIndex = 5;
            BtnPaymentMethodEdit.Text = "Edit [F7]";
            BtnPaymentMethodEdit.UseVisualStyleBackColor = true;
            BtnPaymentMethodEdit.Click += BtnPaymentMethodEdit_Click;
            // 
            // ListBoxPaymentMethod
            // 
            ListBoxPaymentMethod.FormattingEnabled = true;
            ListBoxPaymentMethod.Location = new Point(12, 36);
            ListBoxPaymentMethod.Name = "ListBoxPaymentMethod";
            ListBoxPaymentMethod.Size = new Size(239, 290);
            ListBoxPaymentMethod.Sorted = true;
            ListBoxPaymentMethod.TabIndex = 2;
            ListBoxPaymentMethod.SelectedIndexChanged += ListBoxPaymentMethod_SelectedIndexChanged;
            // 
            // BtnPaymentMethodExit
            // 
            BtnPaymentMethodExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPaymentMethodExit.Location = new Point(748, 340);
            BtnPaymentMethodExit.Name = "BtnPaymentMethodExit";
            BtnPaymentMethodExit.Size = new Size(83, 23);
            BtnPaymentMethodExit.TabIndex = 12;
            BtnPaymentMethodExit.Text = "Exit [F10]";
            BtnPaymentMethodExit.UseVisualStyleBackColor = true;
            BtnPaymentMethodExit.Click += BtnPaymentMethodExit_Click;
            // 
            // TextBoxPaymentMethodId
            // 
            TextBoxPaymentMethodId.Location = new Point(349, 343);
            TextBoxPaymentMethodId.MaxLength = 35;
            TextBoxPaymentMethodId.Name = "TextBoxPaymentMethodId";
            TextBoxPaymentMethodId.Size = new Size(108, 21);
            TextBoxPaymentMethodId.TabIndex = 18;
            TextBoxPaymentMethodId.Visible = false;
            // 
            // StatusStripPayment
            // 
            StatusStripPayment.Items.AddRange(new ToolStripItem[] { ToolStripStatusLabelErrorPayment });
            StatusStripPayment.Location = new Point(0, 377);
            StatusStripPayment.Name = "StatusStripPayment";
            StatusStripPayment.Size = new Size(845, 22);
            StatusStripPayment.TabIndex = 63;
            StatusStripPayment.Text = "statusStrip1";
            // 
            // ToolStripStatusLabelErrorPayment
            // 
            ToolStripStatusLabelErrorPayment.Name = "ToolStripStatusLabelErrorPayment";
            ToolStripStatusLabelErrorPayment.Size = new Size(100, 17);
            ToolStripStatusLabelErrorPayment.Text = "                               ";
            // 
            // TabControlPaymentMethod
            // 
            TabControlPaymentMethod.Controls.Add(TabDetails);
            TabControlPaymentMethod.Location = new Point(257, 12);
            TabControlPaymentMethod.Name = "TabControlPaymentMethod";
            TabControlPaymentMethod.SelectedIndex = 0;
            TabControlPaymentMethod.Size = new Size(583, 314);
            TabControlPaymentMethod.TabIndex = 64;
            // 
            // TabDetails
            // 
            TabDetails.Controls.Add(ComboBoxPaymentMethodAccount);
            TabDetails.Controls.Add(TextBoxPaymentMethodDisplayAs);
            TabDetails.Controls.Add(TextBoxPaymentMethodName);
            TabDetails.Controls.Add(LabelCurrencyName);
            TabDetails.Controls.Add(label2);
            TabDetails.Controls.Add(LabelCurrencyDisplayas);
            TabDetails.Controls.Add(label1);
            TabDetails.Controls.Add(groupBox1);
            TabDetails.Location = new Point(4, 22);
            TabDetails.Name = "TabDetails";
            TabDetails.Padding = new Padding(3);
            TabDetails.Size = new Size(575, 288);
            TabDetails.TabIndex = 0;
            TabDetails.Text = "Payment Method Details";
            TabDetails.UseVisualStyleBackColor = true;
            // 
            // ComboBoxPaymentMethodAccount
            // 
            ComboBoxPaymentMethodAccount.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxPaymentMethodAccount.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxPaymentMethodAccount.FormattingEnabled = true;
            ComboBoxPaymentMethodAccount.Location = new Point(18, 124);
            ComboBoxPaymentMethodAccount.Name = "ComboBoxPaymentMethodAccount";
            ComboBoxPaymentMethodAccount.Size = new Size(227, 21);
            ComboBoxPaymentMethodAccount.TabIndex = 8;
            ComboBoxPaymentMethodAccount.TxtVisible = true;
            ComboBoxPaymentMethodAccount.KeyPress += ComboBoxPaymentMethodAccount_KeyPress;
            // 
            // TextBoxPaymentMethodDisplayAs
            // 
            TextBoxPaymentMethodDisplayAs.BackColor = SystemColors.Window;
            TextBoxPaymentMethodDisplayAs.Location = new Point(18, 82);
            TextBoxPaymentMethodDisplayAs.MaxLength = 50;
            TextBoxPaymentMethodDisplayAs.Name = "TextBoxPaymentMethodDisplayAs";
            TextBoxPaymentMethodDisplayAs.ReadOnly = true;
            TextBoxPaymentMethodDisplayAs.Size = new Size(306, 21);
            TextBoxPaymentMethodDisplayAs.TabIndex = 7;
            TextBoxPaymentMethodDisplayAs.KeyDown += TextBoxPaymentMethodName_KeyDown;
            TextBoxPaymentMethodDisplayAs.KeyPress += TextBoxPaymentMethodDisplayAs_KeyPress;
            TextBoxPaymentMethodDisplayAs.MouseDown += TextBoxPaymentMethodDisplayAs_MouseDown;
            // 
            // TextBoxPaymentMethodName
            // 
            TextBoxPaymentMethodName.BackColor = SystemColors.Window;
            TextBoxPaymentMethodName.Location = new Point(18, 40);
            TextBoxPaymentMethodName.MaxLength = 30;
            TextBoxPaymentMethodName.Name = "TextBoxPaymentMethodName";
            TextBoxPaymentMethodName.ReadOnly = true;
            TextBoxPaymentMethodName.Size = new Size(306, 21);
            TextBoxPaymentMethodName.TabIndex = 6;
            TextBoxPaymentMethodName.KeyDown += TextBoxPaymentMethodName_KeyDown;
            TextBoxPaymentMethodName.KeyPress += TextBoxPaymentMethodName_KeyPress;
            TextBoxPaymentMethodName.MouseDown += TextBoxPaymentMethodName_MouseDown;
            TextBoxPaymentMethodName.PreviewKeyDown += TextBoxPaymentMethodName_PreviewKeyDown;
            // 
            // LabelCurrencyName
            // 
            LabelCurrencyName.AutoSize = true;
            LabelCurrencyName.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelCurrencyName.Location = new Point(15, 23);
            LabelCurrencyName.Name = "LabelCurrencyName";
            LabelCurrencyName.Size = new Size(39, 13);
            LabelCurrencyName.TabIndex = 0;
            LabelCurrencyName.Text = "Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(15, 107);
            label2.Name = "label2";
            label2.Size = new Size(53, 13);
            label2.TabIndex = 13;
            label2.Text = "Account";
            // 
            // LabelCurrencyDisplayas
            // 
            LabelCurrencyDisplayas.AutoSize = true;
            LabelCurrencyDisplayas.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelCurrencyDisplayas.Location = new Point(15, 65);
            LabelCurrencyDisplayas.Name = "LabelCurrencyDisplayas";
            LabelCurrencyDisplayas.Size = new Size(56, 13);
            LabelCurrencyDisplayas.TabIndex = 1;
            LabelCurrencyDisplayas.Text = "Display As";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(15, 149);
            label1.Name = "label1";
            label1.Size = new Size(70, 13);
            label1.TabIndex = 12;
            label1.Text = "Credit Card";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(RadioButtonPaymentMethodNo);
            groupBox1.Controls.Add(RadioButtonPaymentMethodYes);
            groupBox1.Location = new Point(16, 160);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(108, 37);
            groupBox1.TabIndex = 9;
            groupBox1.TabStop = false;
            // 
            // RadioButtonPaymentMethodNo
            // 
            RadioButtonPaymentMethodNo.AutoSize = true;
            RadioButtonPaymentMethodNo.Location = new Point(63, 14);
            RadioButtonPaymentMethodNo.Name = "RadioButtonPaymentMethodNo";
            RadioButtonPaymentMethodNo.Size = new Size(38, 17);
            RadioButtonPaymentMethodNo.TabIndex = 9;
            RadioButtonPaymentMethodNo.TabStop = true;
            RadioButtonPaymentMethodNo.Text = "No";
            RadioButtonPaymentMethodNo.UseVisualStyleBackColor = true;
            // 
            // RadioButtonPaymentMethodYes
            // 
            RadioButtonPaymentMethodYes.AutoSize = true;
            RadioButtonPaymentMethodYes.Location = new Point(6, 14);
            RadioButtonPaymentMethodYes.Name = "RadioButtonPaymentMethodYes";
            RadioButtonPaymentMethodYes.Size = new Size(42, 17);
            RadioButtonPaymentMethodYes.TabIndex = 9;
            RadioButtonPaymentMethodYes.TabStop = true;
            RadioButtonPaymentMethodYes.Text = "Yes";
            RadioButtonPaymentMethodYes.UseVisualStyleBackColor = true;
            // 
            // TextBoxPaymentMethodSearch
            // 
            TextBoxPaymentMethodSearch.BackColor = SystemColors.Window;
            TextBoxPaymentMethodSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxPaymentMethodSearch.Delay = true;
            TextBoxPaymentMethodSearch.DelayTime = 1000;
            TextBoxPaymentMethodSearch.Location = new Point(12, 12);
            TextBoxPaymentMethodSearch.MaxLength = 35;
            TextBoxPaymentMethodSearch.Name = "TextBoxPaymentMethodSearch";
            TextBoxPaymentMethodSearch.Searchstartfrom = 2;
            TextBoxPaymentMethodSearch.Size = new Size(239, 21);
            TextBoxPaymentMethodSearch.TabIndex = 65;
            TextBoxPaymentMethodSearch.TextChanged += TextBoxPaymentMethodSearch_TextChanged;
            TextBoxPaymentMethodSearch.KeyDown += TextBoxPaymentMethodSearch_KeyDown;
            // 
            // FormPaymentMethod
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(845, 399);
            Controls.Add(TabControlPaymentMethod);
            Controls.Add(StatusStripPayment);
            Controls.Add(TextBoxPaymentMethodId);
            Controls.Add(BtnPaymentMethodExit);
            Controls.Add(BtnPaymentMethodDelete);
            Controls.Add(BtnPaymentMethodNew);
            Controls.Add(BtnPaymentMethodCancel);
            Controls.Add(BtnPaymentMethodSave);
            Controls.Add(ListBoxPaymentMethod);
            Controls.Add(BtnPaymentMethodEdit);
            Controls.Add(TextBoxPaymentMethodSearch);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormPaymentMethod";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PaymentMethods";
            Load += FormPaymentMethod_Load;
            Controls.SetChildIndex(TextBoxPaymentMethodSearch, 0);
            Controls.SetChildIndex(BtnPaymentMethodEdit, 0);
            Controls.SetChildIndex(ListBoxPaymentMethod, 0);
            Controls.SetChildIndex(BtnPaymentMethodSave, 0);
            Controls.SetChildIndex(BtnPaymentMethodCancel, 0);
            Controls.SetChildIndex(BtnPaymentMethodNew, 0);
            Controls.SetChildIndex(BtnPaymentMethodDelete, 0);
            Controls.SetChildIndex(BtnPaymentMethodExit, 0);
            Controls.SetChildIndex(TextBoxPaymentMethodId, 0);
            Controls.SetChildIndex(StatusStripPayment, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(TabControlPaymentMethod, 0);
            StatusStripPayment.ResumeLayout(false);
            StatusStripPayment.PerformLayout();
            TabControlPaymentMethod.ResumeLayout(false);
            TabDetails.ResumeLayout(false);
            TabDetails.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button BtnPaymentMethodDelete;
        private System.Windows.Forms.Button BtnPaymentMethodNew;
        private System.Windows.Forms.Button BtnPaymentMethodCancel;
        private System.Windows.Forms.Button BtnPaymentMethodSave;
        private System.Windows.Forms.Button BtnPaymentMethodEdit;
        private System.Windows.Forms.ListBox ListBoxPaymentMethod;
        private System.Windows.Forms.Button BtnPaymentMethodExit;
        private System.Windows.Forms.TextBox TextBoxPaymentMethodId;
        private System.Windows.Forms.StatusStrip StatusStripPayment;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabelErrorPayment;
        private System.Windows.Forms.TabControl TabControlPaymentMethod;
        private System.Windows.Forms.TabPage TabDetails;
        private System.Windows.Forms.TextBox TextBoxPaymentMethodName;
        private System.Windows.Forms.Label LabelCurrencyName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label LabelCurrencyDisplayas;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBoxPaymentMethodDisplayAs;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton RadioButtonPaymentMethodNo;
        private System.Windows.Forms.RadioButton RadioButtonPaymentMethodYes;
        private controls.ComboBoxSwapTextBox ComboBoxPaymentMethodAccount;
        private controls.text.DelayedTextChangeTextBox TextBoxPaymentMethodSearch;
    }
}