namespace fa.views.hms.config
{
    partial class FormInvoiceGrouping
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInvoiceGrouping));
            this.GroupBoxInvoiceGroup = new System.Windows.Forms.GroupBox();
            this.CheckedListBoxInvoiceGroup = new System.Windows.Forms.CheckedListBox();
            this.TextBoxInvoiceGroupName = new fa.views.controls.text.NameTextBoxAllowSpace(this.components);
            this.TextBoxInvoiceGroupDescription = new System.Windows.Forms.TextBox();
            this.LabelCostCenterDescription = new System.Windows.Forms.Label();
            this.LabelCostCenterName = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.ListBoxInvoiceGroup = new System.Windows.Forms.ListBox();
            this.BtnInvoiceGroupSave = new System.Windows.Forms.Button();
            this.BtnInvoiceGroupCancel = new System.Windows.Forms.Button();
            this.BtnInvoiceGroupDelete = new System.Windows.Forms.Button();
            this.BtnInvoiceGroupEdit = new System.Windows.Forms.Button();
            this.BtnInvoiceGroupNew = new System.Windows.Forms.Button();
            this.TextBoxInvoiceGroupId = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.InvoiceGroupErrorMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.BtnInvoiceGroupExit = new System.Windows.Forms.Button();
            this.GroupBoxInvoiceGroup.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PatientIdTransport
            // 
            this.PatientIdTransport.Location = new System.Drawing.Point(12, 812);
            this.PatientIdTransport.Size = new System.Drawing.Size(100, 21);
            // 
            // ProductIdTransport
            // 
            this.ProductIdTransport.Location = new System.Drawing.Point(172, 215);
            this.ProductIdTransport.Size = new System.Drawing.Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            this.ProductBatchIdTransport.Location = new System.Drawing.Point(172, 189);
            this.ProductBatchIdTransport.Size = new System.Drawing.Size(100, 21);
            // 
            // AccountIdTransport
            // 
            this.AccountIdTransport.Location = new System.Drawing.Point(172, 163);
            this.AccountIdTransport.Size = new System.Drawing.Size(100, 21);
            // 
            // GroupBoxInvoiceGroup
            // 
            this.GroupBoxInvoiceGroup.Controls.Add(this.CheckedListBoxInvoiceGroup);
            this.GroupBoxInvoiceGroup.Controls.Add(this.TextBoxInvoiceGroupName);
            this.GroupBoxInvoiceGroup.Controls.Add(this.TextBoxInvoiceGroupDescription);
            this.GroupBoxInvoiceGroup.Controls.Add(this.LabelCostCenterDescription);
            this.GroupBoxInvoiceGroup.Controls.Add(this.LabelCostCenterName);
            this.GroupBoxInvoiceGroup.Controls.Add(this.label8);
            this.GroupBoxInvoiceGroup.Location = new System.Drawing.Point(214, 6);
            this.GroupBoxInvoiceGroup.Name = "GroupBoxInvoiceGroup";
            this.GroupBoxInvoiceGroup.Size = new System.Drawing.Size(542, 270);
            this.GroupBoxInvoiceGroup.TabIndex = 36;
            this.GroupBoxInvoiceGroup.TabStop = false;
            this.GroupBoxInvoiceGroup.Text = "Details";
            // 
            // CheckedListBoxInvoiceGroup
            // 
            this.CheckedListBoxInvoiceGroup.CheckOnClick = true;
            this.CheckedListBoxInvoiceGroup.FormattingEnabled = true;
            this.CheckedListBoxInvoiceGroup.Location = new System.Drawing.Point(17, 157);
            this.CheckedListBoxInvoiceGroup.Name = "CheckedListBoxInvoiceGroup";
            this.CheckedListBoxInvoiceGroup.ScrollAlwaysVisible = true;
            this.CheckedListBoxInvoiceGroup.Size = new System.Drawing.Size(275, 100);
            this.CheckedListBoxInvoiceGroup.TabIndex = 28;
            // 
            // TextBoxInvoiceGroupName
            // 
            this.TextBoxInvoiceGroupName.BackColor = System.Drawing.Color.White;
            this.TextBoxInvoiceGroupName.Location = new System.Drawing.Point(17, 38);
            this.TextBoxInvoiceGroupName.MaxLength = 50;
            this.TextBoxInvoiceGroupName.Name = "TextBoxInvoiceGroupName";
            this.TextBoxInvoiceGroupName.Size = new System.Drawing.Size(459, 21);
            this.TextBoxInvoiceGroupName.TabIndex = 26;
            this.TextBoxInvoiceGroupName.TextChanged += new System.EventHandler(this.TextBoxInvoiceGroupName_TextChanged);
            // 
            // TextBoxInvoiceGroupDescription
            // 
            this.TextBoxInvoiceGroupDescription.BackColor = System.Drawing.Color.White;
            this.TextBoxInvoiceGroupDescription.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TextBoxInvoiceGroupDescription.Location = new System.Drawing.Point(17, 78);
            this.TextBoxInvoiceGroupDescription.MaxLength = 250;
            this.TextBoxInvoiceGroupDescription.Multiline = true;
            this.TextBoxInvoiceGroupDescription.Name = "TextBoxInvoiceGroupDescription";
            this.TextBoxInvoiceGroupDescription.Size = new System.Drawing.Size(459, 59);
            this.TextBoxInvoiceGroupDescription.TabIndex = 27;
            this.TextBoxInvoiceGroupDescription.TextChanged += new System.EventHandler(this.TextBoxInvoiceGroupDescription_TextChanged);
            // 
            // LabelCostCenterDescription
            // 
            this.LabelCostCenterDescription.AutoSize = true;
            this.LabelCostCenterDescription.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LabelCostCenterDescription.Location = new System.Drawing.Point(14, 62);
            this.LabelCostCenterDescription.Name = "LabelCostCenterDescription";
            this.LabelCostCenterDescription.Size = new System.Drawing.Size(60, 13);
            this.LabelCostCenterDescription.TabIndex = 16;
            this.LabelCostCenterDescription.Text = "Description";
            // 
            // LabelCostCenterName
            // 
            this.LabelCostCenterName.AutoSize = true;
            this.LabelCostCenterName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LabelCostCenterName.Location = new System.Drawing.Point(14, 22);
            this.LabelCostCenterName.Name = "LabelCostCenterName";
            this.LabelCostCenterName.Size = new System.Drawing.Size(39, 13);
            this.LabelCostCenterName.TabIndex = 15;
            this.LabelCostCenterName.Text = "Name";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label8.Location = new System.Drawing.Point(14, 140);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(90, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Transaction Type";
            // 
            // ListBoxInvoiceGroup
            // 
            this.ListBoxInvoiceGroup.FormattingEnabled = true;
            this.ListBoxInvoiceGroup.Location = new System.Drawing.Point(12, 12);
            this.ListBoxInvoiceGroup.Name = "ListBoxInvoiceGroup";
            this.ListBoxInvoiceGroup.Size = new System.Drawing.Size(196, 264);
            this.ListBoxInvoiceGroup.TabIndex = 32;
            this.ListBoxInvoiceGroup.SelectedIndexChanged += new System.EventHandler(this.ListBoxInvoiceGroup_SelectedIndexChanged);
            // 
            // BtnInvoiceGroupSave
            // 
            this.BtnInvoiceGroupSave.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.BtnInvoiceGroupSave.Location = new System.Drawing.Point(592, 297);
            this.BtnInvoiceGroupSave.Name = "BtnInvoiceGroupSave";
            this.BtnInvoiceGroupSave.Size = new System.Drawing.Size(75, 23);
            this.BtnInvoiceGroupSave.TabIndex = 37;
            this.BtnInvoiceGroupSave.Text = "Save [F8]";
            this.BtnInvoiceGroupSave.UseVisualStyleBackColor = true;
            this.BtnInvoiceGroupSave.Click += new System.EventHandler(this.BtnInvoiceGroupSave_Click);
            // 
            // BtnInvoiceGroupCancel
            // 
            this.BtnInvoiceGroupCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.BtnInvoiceGroupCancel.Location = new System.Drawing.Point(495, 297);
            this.BtnInvoiceGroupCancel.Name = "BtnInvoiceGroupCancel";
            this.BtnInvoiceGroupCancel.Size = new System.Drawing.Size(91, 23);
            this.BtnInvoiceGroupCancel.TabIndex = 38;
            this.BtnInvoiceGroupCancel.Text = "Cancel [Esc]";
            this.BtnInvoiceGroupCancel.UseVisualStyleBackColor = true;
            this.BtnInvoiceGroupCancel.Click += new System.EventHandler(this.BtnInvoiceGroupCancel_Click);
            // 
            // BtnInvoiceGroupDelete
            // 
            this.BtnInvoiceGroupDelete.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.BtnInvoiceGroupDelete.Location = new System.Drawing.Point(104, 295);
            this.BtnInvoiceGroupDelete.Name = "BtnInvoiceGroupDelete";
            this.BtnInvoiceGroupDelete.Size = new System.Drawing.Size(83, 23);
            this.BtnInvoiceGroupDelete.TabIndex = 34;
            this.BtnInvoiceGroupDelete.Text = "Delete [F4]";
            this.BtnInvoiceGroupDelete.UseVisualStyleBackColor = true;
            this.BtnInvoiceGroupDelete.Click += new System.EventHandler(this.BtnInvoiceGroupDelete_Click);
            // 
            // BtnInvoiceGroupEdit
            // 
            this.BtnInvoiceGroupEdit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.BtnInvoiceGroupEdit.Location = new System.Drawing.Point(194, 295);
            this.BtnInvoiceGroupEdit.Name = "BtnInvoiceGroupEdit";
            this.BtnInvoiceGroupEdit.Size = new System.Drawing.Size(83, 23);
            this.BtnInvoiceGroupEdit.TabIndex = 35;
            this.BtnInvoiceGroupEdit.Text = "Edit [F7]";
            this.BtnInvoiceGroupEdit.UseVisualStyleBackColor = true;
            this.BtnInvoiceGroupEdit.Click += new System.EventHandler(this.BtnInvoiceGroupEdit_Click);
            // 
            // BtnInvoiceGroupNew
            // 
            this.BtnInvoiceGroupNew.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.BtnInvoiceGroupNew.Location = new System.Drawing.Point(14, 295);
            this.BtnInvoiceGroupNew.Name = "BtnInvoiceGroupNew";
            this.BtnInvoiceGroupNew.Size = new System.Drawing.Size(83, 23);
            this.BtnInvoiceGroupNew.TabIndex = 33;
            this.BtnInvoiceGroupNew.Text = "New [F3]";
            this.BtnInvoiceGroupNew.UseVisualStyleBackColor = true;
            this.BtnInvoiceGroupNew.Click += new System.EventHandler(this.BtnInvoiceGroupNew_Click);
            // 
            // TextBoxInvoiceGroupId
            // 
            this.TextBoxInvoiceGroupId.Location = new System.Drawing.Point(349, 297);
            this.TextBoxInvoiceGroupId.Name = "TextBoxInvoiceGroupId";
            this.TextBoxInvoiceGroupId.Size = new System.Drawing.Size(127, 21);
            this.TextBoxInvoiceGroupId.TabIndex = 31;
            this.TextBoxInvoiceGroupId.Visible = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.InvoiceGroupErrorMsg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 330);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(765, 22);
            this.statusStrip1.TabIndex = 39;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // InvoiceGroupErrorMsg
            // 
            this.InvoiceGroupErrorMsg.Name = "InvoiceGroupErrorMsg";
            this.InvoiceGroupErrorMsg.Size = new System.Drawing.Size(19, 17);
            this.InvoiceGroupErrorMsg.Text = "    ";
            // 
            // BtnInvoiceGroupExit
            // 
            this.BtnInvoiceGroupExit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.BtnInvoiceGroupExit.Location = new System.Drawing.Point(673, 297);
            this.BtnInvoiceGroupExit.Name = "BtnInvoiceGroupExit";
            this.BtnInvoiceGroupExit.Size = new System.Drawing.Size(75, 23);
            this.BtnInvoiceGroupExit.TabIndex = 67;
            this.BtnInvoiceGroupExit.Text = "Exit [F10]";
            this.BtnInvoiceGroupExit.UseVisualStyleBackColor = true;
            this.BtnInvoiceGroupExit.Click += new System.EventHandler(this.BtnInvoiceGroupExit_Click);
            // 
            // FormInvoiceGrouping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 352);
            this.Controls.Add(this.BtnInvoiceGroupExit);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.GroupBoxInvoiceGroup);
            this.Controls.Add(this.ListBoxInvoiceGroup);
            this.Controls.Add(this.BtnInvoiceGroupSave);
            this.Controls.Add(this.BtnInvoiceGroupCancel);
            this.Controls.Add(this.BtnInvoiceGroupDelete);
            this.Controls.Add(this.BtnInvoiceGroupEdit);
            this.Controls.Add(this.BtnInvoiceGroupNew);
            this.Controls.Add(this.TextBoxInvoiceGroupId);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormInvoiceGrouping";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Invoice Grouping";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormInvoiceGrouping_FormClosing);
            this.Load += new System.EventHandler(this.FormInvoiceGrouping_Load);
            this.Controls.SetChildIndex(this.TextBoxInvoiceGroupId, 0);
            this.Controls.SetChildIndex(this.BtnInvoiceGroupNew, 0);
            this.Controls.SetChildIndex(this.BtnInvoiceGroupEdit, 0);
            this.Controls.SetChildIndex(this.BtnInvoiceGroupDelete, 0);
            this.Controls.SetChildIndex(this.BtnInvoiceGroupCancel, 0);
            this.Controls.SetChildIndex(this.BtnInvoiceGroupSave, 0);
            this.Controls.SetChildIndex(this.ListBoxInvoiceGroup, 0);
            this.Controls.SetChildIndex(this.GroupBoxInvoiceGroup, 0);
            this.Controls.SetChildIndex(this.statusStrip1, 0);
            this.Controls.SetChildIndex(this.PatientIdTransport, 0);
            this.Controls.SetChildIndex(this.ProductIdTransport, 0);
            this.Controls.SetChildIndex(this.ProductBatchIdTransport, 0);
            this.Controls.SetChildIndex(this.AccountIdTransport, 0);
            this.Controls.SetChildIndex(this.BtnInvoiceGroupExit, 0);
            this.GroupBoxInvoiceGroup.ResumeLayout(false);
            this.GroupBoxInvoiceGroup.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox GroupBoxInvoiceGroup;
        private System.Windows.Forms.CheckedListBox CheckedListBoxInvoiceGroup;
        private controls.text.NameTextBoxAllowSpace TextBoxInvoiceGroupName;
        private System.Windows.Forms.TextBox TextBoxInvoiceGroupDescription;
        private System.Windows.Forms.Label LabelCostCenterDescription;
        private System.Windows.Forms.Label LabelCostCenterName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ListBox ListBoxInvoiceGroup;
        private System.Windows.Forms.Button BtnInvoiceGroupSave;
        private System.Windows.Forms.Button BtnInvoiceGroupCancel;
        private System.Windows.Forms.Button BtnInvoiceGroupDelete;
        private System.Windows.Forms.Button BtnInvoiceGroupEdit;
        private System.Windows.Forms.Button BtnInvoiceGroupNew;
        private System.Windows.Forms.TextBox TextBoxInvoiceGroupId;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel InvoiceGroupErrorMsg;
        private System.Windows.Forms.Button BtnInvoiceGroupExit;
    }
}