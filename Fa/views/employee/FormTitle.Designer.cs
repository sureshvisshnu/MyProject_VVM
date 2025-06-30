namespace fa.views.employee
{
    partial class FormJobTitle
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJobTitle));
            label1 = new Label();
            TextBoxTltleJobTitle = new TextBox();
            BtnTitleSave = new Button();
            BtnTitleCancel = new Button();
            label2 = new Label();
            label3 = new Label();
            TextBoxTitleDisplayAs = new TextBox();
            TextBoxTitleDescription = new TextBox();
            TextBoxTitleId = new TextBox();
            BtnTitleExit = new Button();
            BtnTitleDelete = new Button();
            BtnTitleNew = new Button();
            GroupBoxTitle = new GroupBox();
            BtnTitleEdit = new Button();
            TextBoxTitleSearch = new TextBox();
            ListBoxTitle = new ListBox();
            statusStrip1 = new StatusStrip();
            ToolStripStatusLabelErrorTitle = new ToolStripStatusLabel();
            GroupBoxTitle.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(172, 215);
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(172, 189);
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(172, 163);
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(15, 24);
            label1.Name = "label1";
            label1.Size = new Size(55, 13);
            label1.TabIndex = 0;
            label1.Text = "Job Title";
            // 
            // TextBoxTltleJobTitle
            // 
            TextBoxTltleJobTitle.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxTltleJobTitle.Location = new Point(18, 42);
            TextBoxTltleJobTitle.MaxLength = 30;
            TextBoxTltleJobTitle.Name = "TextBoxTltleJobTitle";
            TextBoxTltleJobTitle.Size = new Size(295, 21);
            TextBoxTltleJobTitle.TabIndex = 6;
            TextBoxTltleJobTitle.KeyDown += TextBoxTltleJobTitle_KeyDown;
            TextBoxTltleJobTitle.KeyPress += TextBoxTltleJobTitle_KeyPress;
            TextBoxTltleJobTitle.MouseDown += TextBoxTltleJobTitle_MouseDown;
            TextBoxTltleJobTitle.PreviewKeyDown += TextBoxTltleJobTitle_PreviewKeyDown;
            // 
            // BtnTitleSave
            // 
            BtnTitleSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnTitleSave.Location = new Point(529, 327);
            BtnTitleSave.Name = "BtnTitleSave";
            BtnTitleSave.Size = new Size(83, 23);
            BtnTitleSave.TabIndex = 9;
            BtnTitleSave.Text = "Save [F8]";
            BtnTitleSave.UseVisualStyleBackColor = true;
            BtnTitleSave.Click += BtnTitleSave_Click;
            BtnTitleSave.PreviewKeyDown += BtnTitleSave_PreviewKeyDown;
            // 
            // BtnTitleCancel
            // 
            BtnTitleCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnTitleCancel.Location = new Point(440, 327);
            BtnTitleCancel.Name = "BtnTitleCancel";
            BtnTitleCancel.Size = new Size(83, 23);
            BtnTitleCancel.TabIndex = 10;
            BtnTitleCancel.Text = "Cancel [Esc]";
            BtnTitleCancel.UseVisualStyleBackColor = true;
            BtnTitleCancel.Click += BtnTitleCancel_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(15, 67);
            label2.Name = "label2";
            label2.Size = new Size(56, 13);
            label2.TabIndex = 4;
            label2.Text = "Display As";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(15, 109);
            label3.Name = "label3";
            label3.Size = new Size(60, 13);
            label3.TabIndex = 5;
            label3.Text = "Description";
            // 
            // TextBoxTitleDisplayAs
            // 
            TextBoxTitleDisplayAs.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxTitleDisplayAs.Location = new Point(18, 85);
            TextBoxTitleDisplayAs.MaxLength = 50;
            TextBoxTitleDisplayAs.Name = "TextBoxTitleDisplayAs";
            TextBoxTitleDisplayAs.Size = new Size(295, 21);
            TextBoxTitleDisplayAs.TabIndex = 7;
            TextBoxTitleDisplayAs.KeyDown += TextBoxTltleJobTitle_KeyDown;
            TextBoxTitleDisplayAs.KeyPress += TextBoxTitleDisplayAs_KeyPress;
            TextBoxTitleDisplayAs.MouseDown += TextBoxTitleDisplayAs_MouseDown;
            // 
            // TextBoxTitleDescription
            // 
            TextBoxTitleDescription.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxTitleDescription.Location = new Point(18, 127);
            TextBoxTitleDescription.MaxLength = 250;
            TextBoxTitleDescription.Multiline = true;
            TextBoxTitleDescription.Name = "TextBoxTitleDescription";
            TextBoxTitleDescription.Size = new Size(373, 58);
            TextBoxTitleDescription.TabIndex = 8;
            TextBoxTitleDescription.PreviewKeyDown += TextBoxTitleDescription_PreviewKeyDown;
            // 
            // TextBoxTitleId
            // 
            TextBoxTitleId.Location = new Point(325, 329);
            TextBoxTitleId.MaxLength = 35;
            TextBoxTitleId.Name = "TextBoxTitleId";
            TextBoxTitleId.Size = new Size(73, 21);
            TextBoxTitleId.TabIndex = 25;
            TextBoxTitleId.Visible = false;
            // 
            // BtnTitleExit
            // 
            BtnTitleExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnTitleExit.Location = new Point(618, 327);
            BtnTitleExit.Name = "BtnTitleExit";
            BtnTitleExit.Size = new Size(83, 23);
            BtnTitleExit.TabIndex = 5;
            BtnTitleExit.Text = "Exit [F10]";
            BtnTitleExit.UseVisualStyleBackColor = true;
            BtnTitleExit.Click += BtnTitleExit_Click;
            // 
            // BtnTitleDelete
            // 
            BtnTitleDelete.Enabled = false;
            BtnTitleDelete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnTitleDelete.Location = new Point(101, 327);
            BtnTitleDelete.Name = "BtnTitleDelete";
            BtnTitleDelete.Size = new Size(83, 23);
            BtnTitleDelete.TabIndex = 3;
            BtnTitleDelete.Text = "Delete [F4]";
            BtnTitleDelete.UseVisualStyleBackColor = true;
            BtnTitleDelete.Click += BtnTitleDelete_Click;
            // 
            // BtnTitleNew
            // 
            BtnTitleNew.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnTitleNew.Location = new Point(12, 327);
            BtnTitleNew.Name = "BtnTitleNew";
            BtnTitleNew.Size = new Size(83, 23);
            BtnTitleNew.TabIndex = 2;
            BtnTitleNew.Text = "New [F3]";
            BtnTitleNew.UseVisualStyleBackColor = true;
            BtnTitleNew.Click += BtnTitleNew_Click;
            // 
            // GroupBoxTitle
            // 
            GroupBoxTitle.BackColor = SystemColors.Window;
            GroupBoxTitle.Controls.Add(TextBoxTltleJobTitle);
            GroupBoxTitle.Controls.Add(TextBoxTitleDescription);
            GroupBoxTitle.Controls.Add(TextBoxTitleDisplayAs);
            GroupBoxTitle.Controls.Add(label3);
            GroupBoxTitle.Controls.Add(label2);
            GroupBoxTitle.Controls.Add(label1);
            GroupBoxTitle.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            GroupBoxTitle.Location = new Point(238, 8);
            GroupBoxTitle.Name = "GroupBoxTitle";
            GroupBoxTitle.Size = new Size(463, 308);
            GroupBoxTitle.TabIndex = 4;
            GroupBoxTitle.TabStop = false;
            GroupBoxTitle.Text = "Title Details";
            // 
            // BtnTitleEdit
            // 
            BtnTitleEdit.Enabled = false;
            BtnTitleEdit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnTitleEdit.Location = new Point(190, 327);
            BtnTitleEdit.Name = "BtnTitleEdit";
            BtnTitleEdit.Size = new Size(83, 23);
            BtnTitleEdit.TabIndex = 4;
            BtnTitleEdit.Text = "Edit [F7]";
            BtnTitleEdit.UseVisualStyleBackColor = true;
            BtnTitleEdit.Click += BtnTitleEdit_Click;
            // 
            // TextBoxTitleSearch
            // 
            TextBoxTitleSearch.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxTitleSearch.Location = new Point(12, 12);
            TextBoxTitleSearch.MaxLength = 35;
            TextBoxTitleSearch.Name = "TextBoxTitleSearch";
            TextBoxTitleSearch.Size = new Size(222, 21);
            TextBoxTitleSearch.TabIndex = 0;
            TextBoxTitleSearch.TextChanged += TextBoxTitleSearch_TextChanged;
            TextBoxTitleSearch.KeyDown += TextBoxTitleSearch_KeyDown;
            TextBoxTitleSearch.KeyPress += TextBoxTitleSearch_KeyPress;
            // 
            // ListBoxTitle
            // 
            ListBoxTitle.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ListBoxTitle.FormattingEnabled = true;
            ListBoxTitle.Location = new Point(12, 37);
            ListBoxTitle.Name = "ListBoxTitle";
            ListBoxTitle.Size = new Size(223, 277);
            ListBoxTitle.Sorted = true;
            ListBoxTitle.TabIndex = 1;
            ListBoxTitle.SelectedIndexChanged += ListBoxTitle_SelectedIndexChanged;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ToolStripStatusLabelErrorTitle });
            statusStrip1.Location = new Point(0, 365);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(713, 22);
            statusStrip1.TabIndex = 57;
            statusStrip1.Text = "sdfdsf sdf sdf";
            // 
            // ToolStripStatusLabelErrorTitle
            // 
            ToolStripStatusLabelErrorTitle.Name = "ToolStripStatusLabelErrorTitle";
            ToolStripStatusLabelErrorTitle.Size = new Size(151, 17);
            ToolStripStatusLabelErrorTitle.Text = "                                                ";
            // 
            // FormJobTitle
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(713, 387);
            Controls.Add(statusStrip1);
            Controls.Add(BtnTitleEdit);
            Controls.Add(TextBoxTitleId);
            Controls.Add(BtnTitleCancel);
            Controls.Add(BtnTitleSave);
            Controls.Add(BtnTitleExit);
            Controls.Add(BtnTitleDelete);
            Controls.Add(BtnTitleNew);
            Controls.Add(GroupBoxTitle);
            Controls.Add(TextBoxTitleSearch);
            Controls.Add(ListBoxTitle);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormJobTitle";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Add New Job Title";
            Load += FormJobTitle_Load;
            Controls.SetChildIndex(ListBoxTitle, 0);
            Controls.SetChildIndex(TextBoxTitleSearch, 0);
            Controls.SetChildIndex(GroupBoxTitle, 0);
            Controls.SetChildIndex(BtnTitleNew, 0);
            Controls.SetChildIndex(BtnTitleDelete, 0);
            Controls.SetChildIndex(BtnTitleExit, 0);
            Controls.SetChildIndex(BtnTitleSave, 0);
            Controls.SetChildIndex(BtnTitleCancel, 0);
            Controls.SetChildIndex(TextBoxTitleId, 0);
            Controls.SetChildIndex(BtnTitleEdit, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            GroupBoxTitle.ResumeLayout(false);
            GroupBoxTitle.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox TextBoxTltleJobTitle;
        private Button BtnTitleSave;
        private Button BtnTitleCancel;
        private Label label2;
        private Label label3;
        private TextBox TextBoxTitleDisplayAs;
        private TextBox TextBoxTitleDescription;
        private TextBox TextBoxTitleId;
        private Button BtnTitleExit;
        private Button BtnTitleDelete;
        private Button BtnTitleNew;
        private GroupBox GroupBoxTitle;
        private Button BtnTitleEdit;
        private TextBox TextBoxTitleSearch;
        private ListBox ListBoxTitle;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel ToolStripStatusLabelErrorTitle;
    }
}