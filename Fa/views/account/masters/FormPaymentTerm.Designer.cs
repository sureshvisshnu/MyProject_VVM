namespace fa.views.account.masters
{
    partial class FormPaymentTerm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPaymentTerm));
            BtnPaymentTermDelete = new Button();
            BtnPaymentTermNew = new Button();
            TextBoxPaymentTermName = new TextBox();
            LabelPaymentTermName = new Label();
            LabelPaymentTermNoofday = new Label();
            BtnPaymentTermSave = new Button();
            BtnPaymentTermEdit = new Button();
            BtnPaymentTermCancel = new Button();
            ListBoxPaymentTerm = new ListBox();
            TextBoxPaymentTermId = new TextBox();
            BtnPaymentTermExit = new Button();
            StatusStripPayment = new StatusStrip();
            ToolStripStatusLabelErrorPaymentTerm = new ToolStripStatusLabel();
            TabControlPaymentTerm = new TabControl();
            TabDetails = new TabPage();
            TextBoxPaymentTermNoofday = new MaskedTextBox();
            TextBoxPaymentTermSearch = new controls.text.DelayedTextChangeTextBox();
            StatusStripPayment.SuspendLayout();
            TabControlPaymentTerm.SuspendLayout();
            TabDetails.SuspendLayout();
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
            // BtnPaymentTermDelete
            // 
            BtnPaymentTermDelete.Enabled = false;
            BtnPaymentTermDelete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPaymentTermDelete.Location = new Point(109, 337);
            BtnPaymentTermDelete.Name = "BtnPaymentTermDelete";
            BtnPaymentTermDelete.Size = new Size(83, 23);
            BtnPaymentTermDelete.TabIndex = 4;
            BtnPaymentTermDelete.Text = "Delete [F4]";
            BtnPaymentTermDelete.UseVisualStyleBackColor = true;
            BtnPaymentTermDelete.Click += BtnPaymentTermDelete_Click;
            // 
            // BtnPaymentTermNew
            // 
            BtnPaymentTermNew.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPaymentTermNew.Location = new Point(20, 337);
            BtnPaymentTermNew.Name = "BtnPaymentTermNew";
            BtnPaymentTermNew.Size = new Size(83, 23);
            BtnPaymentTermNew.TabIndex = 3;
            BtnPaymentTermNew.Text = "New [F3]";
            BtnPaymentTermNew.UseVisualStyleBackColor = true;
            BtnPaymentTermNew.Click += BtnPaymentTermNew_Click;
            // 
            // TextBoxPaymentTermName
            // 
            TextBoxPaymentTermName.BackColor = SystemColors.Window;
            TextBoxPaymentTermName.Location = new Point(16, 33);
            TextBoxPaymentTermName.MaxLength = 30;
            TextBoxPaymentTermName.Name = "TextBoxPaymentTermName";
            TextBoxPaymentTermName.ReadOnly = true;
            TextBoxPaymentTermName.Size = new Size(355, 21);
            TextBoxPaymentTermName.TabIndex = 6;
            TextBoxPaymentTermName.KeyDown += TextBoxPaymentTermName_KeyDown;
            TextBoxPaymentTermName.KeyPress += TextBoxPaymentTermName_KeyPress;
            TextBoxPaymentTermName.MouseDown += TextBoxPaymentTermName_MouseDown;
            TextBoxPaymentTermName.PreviewKeyDown += TextBoxPaymentTermName_PreviewKeyDown;
            // 
            // LabelPaymentTermName
            // 
            LabelPaymentTermName.AutoSize = true;
            LabelPaymentTermName.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelPaymentTermName.Location = new Point(13, 17);
            LabelPaymentTermName.Name = "LabelPaymentTermName";
            LabelPaymentTermName.Size = new Size(39, 13);
            LabelPaymentTermName.TabIndex = 0;
            LabelPaymentTermName.Text = "Name";
            // 
            // LabelPaymentTermNoofday
            // 
            LabelPaymentTermNoofday.AutoSize = true;
            LabelPaymentTermNoofday.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelPaymentTermNoofday.Location = new Point(13, 59);
            LabelPaymentTermNoofday.Name = "LabelPaymentTermNoofday";
            LabelPaymentTermNoofday.Size = new Size(67, 13);
            LabelPaymentTermNoofday.TabIndex = 14;
            LabelPaymentTermNoofday.Text = "No Of Days";
            // 
            // BtnPaymentTermSave
            // 
            BtnPaymentTermSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPaymentTermSave.Location = new Point(660, 337);
            BtnPaymentTermSave.Name = "BtnPaymentTermSave";
            BtnPaymentTermSave.Size = new Size(83, 23);
            BtnPaymentTermSave.TabIndex = 11;
            BtnPaymentTermSave.Text = "Save [F8]";
            BtnPaymentTermSave.UseVisualStyleBackColor = true;
            BtnPaymentTermSave.Click += BtnPaymentTermSave_Click;
            BtnPaymentTermSave.PreviewKeyDown += BtnPaymentTermSave_PreviewKeyDown;
            // 
            // BtnPaymentTermEdit
            // 
            BtnPaymentTermEdit.Enabled = false;
            BtnPaymentTermEdit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPaymentTermEdit.Location = new Point(198, 337);
            BtnPaymentTermEdit.Name = "BtnPaymentTermEdit";
            BtnPaymentTermEdit.Size = new Size(83, 23);
            BtnPaymentTermEdit.TabIndex = 5;
            BtnPaymentTermEdit.Text = "Edit [F7]";
            BtnPaymentTermEdit.UseVisualStyleBackColor = true;
            BtnPaymentTermEdit.Click += BtnPaymentTermEdit_Click;
            // 
            // BtnPaymentTermCancel
            // 
            BtnPaymentTermCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPaymentTermCancel.Location = new Point(571, 337);
            BtnPaymentTermCancel.Name = "BtnPaymentTermCancel";
            BtnPaymentTermCancel.Size = new Size(83, 23);
            BtnPaymentTermCancel.TabIndex = 12;
            BtnPaymentTermCancel.Text = "Cancel [Esc]";
            BtnPaymentTermCancel.UseVisualStyleBackColor = true;
            BtnPaymentTermCancel.Click += BtnPaymentTermCancel_Click;
            // 
            // ListBoxPaymentTerm
            // 
            ListBoxPaymentTerm.FormattingEnabled = true;
            ListBoxPaymentTerm.Location = new Point(12, 34);
            ListBoxPaymentTerm.Name = "ListBoxPaymentTerm";
            ListBoxPaymentTerm.Size = new Size(239, 290);
            ListBoxPaymentTerm.Sorted = true;
            ListBoxPaymentTerm.TabIndex = 2;
            ListBoxPaymentTerm.SelectedIndexChanged += ListBoxPaymentTerm_SelectedIndexChanged;
            // 
            // TextBoxPaymentTermId
            // 
            TextBoxPaymentTermId.Location = new Point(379, 337);
            TextBoxPaymentTermId.MaxLength = 35;
            TextBoxPaymentTermId.Name = "TextBoxPaymentTermId";
            TextBoxPaymentTermId.Size = new Size(108, 21);
            TextBoxPaymentTermId.TabIndex = 17;
            TextBoxPaymentTermId.Visible = false;
            // 
            // BtnPaymentTermExit
            // 
            BtnPaymentTermExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPaymentTermExit.Location = new Point(749, 337);
            BtnPaymentTermExit.Name = "BtnPaymentTermExit";
            BtnPaymentTermExit.Size = new Size(83, 23);
            BtnPaymentTermExit.TabIndex = 13;
            BtnPaymentTermExit.Text = "Exit [F10]";
            BtnPaymentTermExit.UseVisualStyleBackColor = true;
            BtnPaymentTermExit.Click += BtnPaymentTermExit_Click_1;
            // 
            // StatusStripPayment
            // 
            StatusStripPayment.Items.AddRange(new ToolStripItem[] { ToolStripStatusLabelErrorPaymentTerm });
            StatusStripPayment.Location = new Point(0, 371);
            StatusStripPayment.Name = "StatusStripPayment";
            StatusStripPayment.Size = new Size(846, 22);
            StatusStripPayment.TabIndex = 64;
            StatusStripPayment.Text = "statusStrip1";
            // 
            // ToolStripStatusLabelErrorPaymentTerm
            // 
            ToolStripStatusLabelErrorPaymentTerm.Name = "ToolStripStatusLabelErrorPaymentTerm";
            ToolStripStatusLabelErrorPaymentTerm.Size = new Size(100, 17);
            ToolStripStatusLabelErrorPaymentTerm.Text = "                               ";
            // 
            // TabControlPaymentTerm
            // 
            TabControlPaymentTerm.Controls.Add(TabDetails);
            TabControlPaymentTerm.Location = new Point(257, 11);
            TabControlPaymentTerm.Name = "TabControlPaymentTerm";
            TabControlPaymentTerm.SelectedIndex = 0;
            TabControlPaymentTerm.Size = new Size(583, 314);
            TabControlPaymentTerm.TabIndex = 65;
            // 
            // TabDetails
            // 
            TabDetails.Controls.Add(TextBoxPaymentTermName);
            TabDetails.Controls.Add(LabelPaymentTermNoofday);
            TabDetails.Controls.Add(TextBoxPaymentTermNoofday);
            TabDetails.Controls.Add(LabelPaymentTermName);
            TabDetails.Location = new Point(4, 22);
            TabDetails.Name = "TabDetails";
            TabDetails.Padding = new Padding(3);
            TabDetails.Size = new Size(575, 288);
            TabDetails.TabIndex = 0;
            TabDetails.Text = "Payment Term Details";
            TabDetails.UseVisualStyleBackColor = true;
            // 
            // TextBoxPaymentTermNoofday
            // 
            TextBoxPaymentTermNoofday.BackColor = SystemColors.Window;
            TextBoxPaymentTermNoofday.Location = new Point(16, 78);
            TextBoxPaymentTermNoofday.Name = "TextBoxPaymentTermNoofday";
            TextBoxPaymentTermNoofday.ReadOnly = true;
            TextBoxPaymentTermNoofday.Size = new Size(210, 21);
            TextBoxPaymentTermNoofday.TabIndex = 8;
            TextBoxPaymentTermNoofday.KeyPress += TextBoxPaymentTermNoofday_KeyPress;
            // 
            // TextBoxPaymentTermSearch
            // 
            TextBoxPaymentTermSearch.BackColor = SystemColors.Window;
            TextBoxPaymentTermSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxPaymentTermSearch.Delay = true;
            TextBoxPaymentTermSearch.DelayTime = 1000;
            TextBoxPaymentTermSearch.Location = new Point(12, 11);
            TextBoxPaymentTermSearch.MaxLength = 35;
            TextBoxPaymentTermSearch.Name = "TextBoxPaymentTermSearch";
            TextBoxPaymentTermSearch.Searchstartfrom = 2;
            TextBoxPaymentTermSearch.Size = new Size(239, 21);
            TextBoxPaymentTermSearch.TabIndex = 66;
            TextBoxPaymentTermSearch.TextChanged += TextBoxPaymentTermSearch_TextChanged;
            TextBoxPaymentTermSearch.KeyDown += TextBoxPaymentTermSearch_KeyDown;
            // 
            // FormPaymentTerm
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(846, 393);
            Controls.Add(TextBoxPaymentTermSearch);
            Controls.Add(TabControlPaymentTerm);
            Controls.Add(StatusStripPayment);
            Controls.Add(BtnPaymentTermExit);
            Controls.Add(TextBoxPaymentTermId);
            Controls.Add(BtnPaymentTermDelete);
            Controls.Add(BtnPaymentTermNew);
            Controls.Add(ListBoxPaymentTerm);
            Controls.Add(BtnPaymentTermEdit);
            Controls.Add(BtnPaymentTermCancel);
            Controls.Add(BtnPaymentTermSave);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormPaymentTerm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PaymentTerms";
            Load += FormPaymentTerm_Load;
            Controls.SetChildIndex(BtnPaymentTermSave, 0);
            Controls.SetChildIndex(BtnPaymentTermCancel, 0);
            Controls.SetChildIndex(BtnPaymentTermEdit, 0);
            Controls.SetChildIndex(ListBoxPaymentTerm, 0);
            Controls.SetChildIndex(BtnPaymentTermNew, 0);
            Controls.SetChildIndex(BtnPaymentTermDelete, 0);
            Controls.SetChildIndex(TextBoxPaymentTermId, 0);
            Controls.SetChildIndex(BtnPaymentTermExit, 0);
            Controls.SetChildIndex(StatusStripPayment, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(TabControlPaymentTerm, 0);
            Controls.SetChildIndex(TextBoxPaymentTermSearch, 0);
            StatusStripPayment.ResumeLayout(false);
            StatusStripPayment.PerformLayout();
            TabControlPaymentTerm.ResumeLayout(false);
            TabDetails.ResumeLayout(false);
            TabDetails.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button BtnPaymentTermDelete;
        private System.Windows.Forms.Button BtnPaymentTermNew;
        private System.Windows.Forms.Button BtnPaymentTermCancel;
        private System.Windows.Forms.Button BtnPaymentTermSave;
        private System.Windows.Forms.Button BtnPaymentTermEdit;
        private System.Windows.Forms.TextBox TextBoxPaymentTermName;
        private System.Windows.Forms.Label LabelPaymentTermName;
        private System.Windows.Forms.ListBox ListBoxPaymentTerm;
        private System.Windows.Forms.Label LabelPaymentTermNoofday;
        private System.Windows.Forms.TextBox TextBoxPaymentTermId;
        private System.Windows.Forms.Button BtnPaymentTermExit;
        private System.Windows.Forms.StatusStrip StatusStripPayment;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabelErrorPaymentTerm;
        private System.Windows.Forms.TabControl TabControlPaymentTerm;
        private System.Windows.Forms.TabPage TabDetails;
        private controls.text.DelayedTextChangeTextBox TextBoxPaymentTermSearch;
        private System.Windows.Forms.MaskedTextBox TextBoxPaymentTermNoofday;
    }
}