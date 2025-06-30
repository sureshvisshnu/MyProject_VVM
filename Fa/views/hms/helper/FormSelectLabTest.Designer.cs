namespace fa.views.hms.helper
{
    partial class FormSelectLabTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectLabTest));
            BtnRemoveLabtest = new Button();
            BtnSelectLabtest = new Button();
            BtnLabtestCancel = new Button();
            BtnLabtestDone = new Button();
            TreeViewSelectedLabTest = new TreeView();
            TextBoxLabtestSearch = new controls.text.NameTextBoxAllowSpace(components);
            TreeViewLabtest = new TreeView();
            TextBoxSelectedLabtestSearch = new controls.text.NameTextBoxAllowSpace(components);
            statusStrip1 = new StatusStrip();
            BtnNewLabTest = new Button();
            SuspendLayout();
            // 
            // PatientIdTransport
            // 
            PatientIdTransport.Location = new Point(12, 812);
            PatientIdTransport.Size = new Size(100, 21);
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
            // BtnRemoveLabtest
            // 
            BtnRemoveLabtest.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnRemoveLabtest.Location = new Point(256, 260);
            BtnRemoveLabtest.Name = "BtnRemoveLabtest";
            BtnRemoveLabtest.Size = new Size(84, 23);
            BtnRemoveLabtest.TabIndex = 22;
            BtnRemoveLabtest.Text = "<-- Remove";
            BtnRemoveLabtest.UseVisualStyleBackColor = true;
            BtnRemoveLabtest.Click += BtnRemoveLabtest_Click;
            // 
            // BtnSelectLabtest
            // 
            BtnSelectLabtest.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSelectLabtest.Location = new Point(256, 133);
            BtnSelectLabtest.Name = "BtnSelectLabtest";
            BtnSelectLabtest.Size = new Size(84, 23);
            BtnSelectLabtest.TabIndex = 21;
            BtnSelectLabtest.Text = "Select -->";
            BtnSelectLabtest.UseVisualStyleBackColor = true;
            BtnSelectLabtest.Click += BtnSelectLabtest_Click;
            // 
            // BtnLabtestCancel
            // 
            BtnLabtestCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnLabtestCancel.Location = new Point(396, 397);
            BtnLabtestCancel.Name = "BtnLabtestCancel";
            BtnLabtestCancel.Size = new Size(93, 23);
            BtnLabtestCancel.TabIndex = 20;
            BtnLabtestCancel.Text = "Reset [Esc]";
            BtnLabtestCancel.UseVisualStyleBackColor = true;
            BtnLabtestCancel.Click += BtnLabtestCancel_Click;
            // 
            // BtnLabtestDone
            // 
            BtnLabtestDone.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnLabtestDone.Location = new Point(495, 397);
            BtnLabtestDone.Name = "BtnLabtestDone";
            BtnLabtestDone.Size = new Size(93, 23);
            BtnLabtestDone.TabIndex = 19;
            BtnLabtestDone.Text = "Done [F8]";
            BtnLabtestDone.UseVisualStyleBackColor = true;
            BtnLabtestDone.Click += BtnLabtestDone_Click;
            // 
            // TreeViewSelectedLabTest
            // 
            TreeViewSelectedLabTest.CheckBoxes = true;
            TreeViewSelectedLabTest.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TreeViewSelectedLabTest.HideSelection = false;
            TreeViewSelectedLabTest.Location = new Point(349, 41);
            TreeViewSelectedLabTest.Name = "TreeViewSelectedLabTest";
            TreeViewSelectedLabTest.Size = new Size(239, 341);
            TreeViewSelectedLabTest.TabIndex = 18;
            TreeViewSelectedLabTest.AfterCheck += TreeViewSelectedLabTest_AfterCheck;
            TreeViewSelectedLabTest.AfterSelect += TreeViewSelectedLabTest_AfterSelect;
            // 
            // TextBoxLabtestSearch
            // 
            TextBoxLabtestSearch.Location = new Point(11, 14);
            TextBoxLabtestSearch.MaxLength = 50;
            TextBoxLabtestSearch.Name = "TextBoxLabtestSearch";
            TextBoxLabtestSearch.Size = new Size(239, 21);
            TextBoxLabtestSearch.TabIndex = 16;
            TextBoxLabtestSearch.TextChanged += TextBoxLabtestSearch_TextChanged;
            // 
            // TreeViewLabtest
            // 
            TreeViewLabtest.CheckBoxes = true;
            TreeViewLabtest.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TreeViewLabtest.HideSelection = false;
            TreeViewLabtest.Location = new Point(11, 41);
            TreeViewLabtest.Name = "TreeViewLabtest";
            TreeViewLabtest.Size = new Size(239, 341);
            TreeViewLabtest.TabIndex = 17;
            TreeViewLabtest.AfterCheck += TreeViewLabtest_AfterCheck;
            TreeViewLabtest.AfterSelect += TreeViewLabtest_AfterSelect;
            // 
            // TextBoxSelectedLabtestSearch
            // 
            TextBoxSelectedLabtestSearch.Location = new Point(349, 14);
            TextBoxSelectedLabtestSearch.MaxLength = 50;
            TextBoxSelectedLabtestSearch.Name = "TextBoxSelectedLabtestSearch";
            TextBoxSelectedLabtestSearch.Size = new Size(239, 21);
            TextBoxSelectedLabtestSearch.TabIndex = 23;
            TextBoxSelectedLabtestSearch.TextChanged += TextBoxSelectedLabtestSearch_TextChanged;
            // 
            // statusStrip1
            // 
            statusStrip1.Location = new Point(0, 434);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(600, 22);
            statusStrip1.TabIndex = 24;
            statusStrip1.Text = "statusStrip1";
            // 
            // BtnNewLabTest
            // 
            BtnNewLabTest.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnNewLabTest.Location = new Point(11, 397);
            BtnNewLabTest.Name = "BtnNewLabTest";
            BtnNewLabTest.Size = new Size(93, 23);
            BtnNewLabTest.TabIndex = 67;
            BtnNewLabTest.Text = "New [F3]";
            BtnNewLabTest.UseVisualStyleBackColor = true;
            BtnNewLabTest.Click += BtnNewLabTest_Click;
            // 
            // FormSelectLabTest
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 456);
            Controls.Add(BtnNewLabTest);
            Controls.Add(statusStrip1);
            Controls.Add(TextBoxSelectedLabtestSearch);
            Controls.Add(BtnRemoveLabtest);
            Controls.Add(BtnSelectLabtest);
            Controls.Add(BtnLabtestCancel);
            Controls.Add(BtnLabtestDone);
            Controls.Add(TreeViewSelectedLabTest);
            Controls.Add(TextBoxLabtestSearch);
            Controls.Add(TreeViewLabtest);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSelectLabTest";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select Lab/Tests";
            Load += FormSelectLabTest_Load;
            Controls.SetChildIndex(TreeViewLabtest, 0);
            Controls.SetChildIndex(TextBoxLabtestSearch, 0);
            Controls.SetChildIndex(TreeViewSelectedLabTest, 0);
            Controls.SetChildIndex(BtnLabtestDone, 0);
            Controls.SetChildIndex(BtnLabtestCancel, 0);
            Controls.SetChildIndex(BtnSelectLabtest, 0);
            Controls.SetChildIndex(BtnRemoveLabtest, 0);
            Controls.SetChildIndex(TextBoxSelectedLabtestSearch, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(BtnNewLabTest, 0);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button BtnRemoveLabtest;
        private System.Windows.Forms.Button BtnSelectLabtest;
        private System.Windows.Forms.Button BtnLabtestCancel;
        private System.Windows.Forms.Button BtnLabtestDone;
        public System.Windows.Forms.TreeView TreeViewSelectedLabTest;
        private controls.text.NameTextBoxAllowSpace TextBoxLabtestSearch;
        public System.Windows.Forms.TreeView TreeViewLabtest;
        private controls.text.NameTextBoxAllowSpace TextBoxSelectedLabtestSearch;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private Button BtnNewLabTest;
    }
}