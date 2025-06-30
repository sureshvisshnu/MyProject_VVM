namespace fa.views.hms.inventory
{
    partial class FormInventoryLocations
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInventoryLocations));
            this.BtnInventoryLocationsExit = new System.Windows.Forms.Button();
            this.BtnInventoryLocationsDelete = new System.Windows.Forms.Button();
            this.BtnInventoryLocationsNew = new System.Windows.Forms.Button();
            this.BtnInventoryLocationsSave = new System.Windows.Forms.Button();
            this.BtnInventoryLocationsCancel = new System.Windows.Forms.Button();
            this.BtnInventoryLocationsEdit = new System.Windows.Forms.Button();
            this.Details = new System.Windows.Forms.TabPage();
            this.InventoryLocationsComboBoxType = new fa.views.controls.ComboBoxSwapTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.InventoryLocationstextBoxdescription = new System.Windows.Forms.TextBox();
            this.InventoryLocationstextBoxname = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.ListBoxInventoryLocation = new System.Windows.Forms.ListBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.LocationErrorMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.InventoryLocationstextBoxsearch = new fa.views.controls.text.DelayedTextChangeTextBox();
            this.Details.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnInventoryLocationsExit
            // 
            this.BtnInventoryLocationsExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnInventoryLocationsExit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.BtnInventoryLocationsExit.Location = new System.Drawing.Point(582, 378);
            this.BtnInventoryLocationsExit.Margin = new System.Windows.Forms.Padding(2);
            this.BtnInventoryLocationsExit.Name = "BtnInventoryLocationsExit";
            this.BtnInventoryLocationsExit.Size = new System.Drawing.Size(83, 22);
            this.BtnInventoryLocationsExit.TabIndex = 9;
            this.BtnInventoryLocationsExit.Text = "Exit [F10]";
            this.BtnInventoryLocationsExit.UseVisualStyleBackColor = true;
            this.BtnInventoryLocationsExit.Click += new System.EventHandler(this.BtnInventoryLocationsExit_Click);
            // 
            // BtnInventoryLocationsDelete
            // 
            this.BtnInventoryLocationsDelete.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.BtnInventoryLocationsDelete.Location = new System.Drawing.Point(96, 378);
            this.BtnInventoryLocationsDelete.Margin = new System.Windows.Forms.Padding(2);
            this.BtnInventoryLocationsDelete.Name = "BtnInventoryLocationsDelete";
            this.BtnInventoryLocationsDelete.Size = new System.Drawing.Size(83, 22);
            this.BtnInventoryLocationsDelete.TabIndex = 2;
            this.BtnInventoryLocationsDelete.Text = "Delete [F4]";
            this.BtnInventoryLocationsDelete.UseVisualStyleBackColor = true;
            this.BtnInventoryLocationsDelete.Click += new System.EventHandler(this.BtnInventoryLocationsDelete_Click);
            // 
            // BtnInventoryLocationsNew
            // 
            this.BtnInventoryLocationsNew.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.BtnInventoryLocationsNew.Location = new System.Drawing.Point(11, 378);
            this.BtnInventoryLocationsNew.Margin = new System.Windows.Forms.Padding(2);
            this.BtnInventoryLocationsNew.Name = "BtnInventoryLocationsNew";
            this.BtnInventoryLocationsNew.Size = new System.Drawing.Size(83, 22);
            this.BtnInventoryLocationsNew.TabIndex = 1;
            this.BtnInventoryLocationsNew.Text = "New [F3]";
            this.BtnInventoryLocationsNew.UseVisualStyleBackColor = true;
            this.BtnInventoryLocationsNew.Click += new System.EventHandler(this.BtnInventoryLocationsNew_Click);
            // 
            // BtnInventoryLocationsSave
            // 
            this.BtnInventoryLocationsSave.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.BtnInventoryLocationsSave.Location = new System.Drawing.Point(496, 378);
            this.BtnInventoryLocationsSave.Margin = new System.Windows.Forms.Padding(2);
            this.BtnInventoryLocationsSave.Name = "BtnInventoryLocationsSave";
            this.BtnInventoryLocationsSave.Size = new System.Drawing.Size(83, 22);
            this.BtnInventoryLocationsSave.TabIndex = 7;
            this.BtnInventoryLocationsSave.Text = "Save [F8]";
            this.BtnInventoryLocationsSave.UseVisualStyleBackColor = true;
            this.BtnInventoryLocationsSave.Click += new System.EventHandler(this.BtnInventoryLocationsSave_Click);
            this.BtnInventoryLocationsSave.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.BtnInventoryLocationsSave_PreviewKeyDown);
            // 
            // BtnInventoryLocationsCancel
            // 
            this.BtnInventoryLocationsCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.BtnInventoryLocationsCancel.Location = new System.Drawing.Point(410, 378);
            this.BtnInventoryLocationsCancel.Margin = new System.Windows.Forms.Padding(2);
            this.BtnInventoryLocationsCancel.Name = "BtnInventoryLocationsCancel";
            this.BtnInventoryLocationsCancel.Size = new System.Drawing.Size(83, 22);
            this.BtnInventoryLocationsCancel.TabIndex = 8;
            this.BtnInventoryLocationsCancel.Text = "Cancel [Esc]";
            this.BtnInventoryLocationsCancel.UseVisualStyleBackColor = true;
            this.BtnInventoryLocationsCancel.Click += new System.EventHandler(this.BtnInventoryLocationsCancel_Click);
            // 
            // BtnInventoryLocationsEdit
            // 
            this.BtnInventoryLocationsEdit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.BtnInventoryLocationsEdit.Location = new System.Drawing.Point(181, 378);
            this.BtnInventoryLocationsEdit.Margin = new System.Windows.Forms.Padding(2);
            this.BtnInventoryLocationsEdit.Name = "BtnInventoryLocationsEdit";
            this.BtnInventoryLocationsEdit.Size = new System.Drawing.Size(83, 22);
            this.BtnInventoryLocationsEdit.TabIndex = 3;
            this.BtnInventoryLocationsEdit.Text = "Edit [F7]";
            this.BtnInventoryLocationsEdit.UseVisualStyleBackColor = true;
            this.BtnInventoryLocationsEdit.Click += new System.EventHandler(this.BtnInventoryLocationsEdit_Click);
            // 
            // Details
            // 
            this.Details.Controls.Add(this.InventoryLocationsComboBoxType);
            this.Details.Controls.Add(this.label3);
            this.Details.Controls.Add(this.InventoryLocationstextBoxdescription);
            this.Details.Controls.Add(this.InventoryLocationstextBoxname);
            this.Details.Controls.Add(this.label2);
            this.Details.Controls.Add(this.label1);
            this.Details.Location = new System.Drawing.Point(4, 22);
            this.Details.Margin = new System.Windows.Forms.Padding(2);
            this.Details.Name = "Details";
            this.Details.Padding = new System.Windows.Forms.Padding(2);
            this.Details.Size = new System.Drawing.Size(399, 328);
            this.Details.TabIndex = 0;
            this.Details.Text = "Details";
            this.Details.UseVisualStyleBackColor = true;
            // 
            // InventoryLocationsComboBoxType
            // 
            this.InventoryLocationsComboBoxType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.InventoryLocationsComboBoxType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.InventoryLocationsComboBoxType.FormattingEnabled = true;
            this.InventoryLocationsComboBoxType.Items.AddRange(new object[] {
            "Ward",
            "Store",
            "Godown",
            "Nurshing Station",
            "Pharmacy"});
            this.InventoryLocationsComboBoxType.Location = new System.Drawing.Point(14, 192);
            this.InventoryLocationsComboBoxType.Name = "InventoryLocationsComboBoxType";
            this.InventoryLocationsComboBoxType.Size = new System.Drawing.Size(164, 21);
            this.InventoryLocationsComboBoxType.TabIndex = 6;
            this.InventoryLocationsComboBoxType.TxtVisible = true;
            this.InventoryLocationsComboBoxType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.InventoryLocationsComboBoxType_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(11, 176);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Type";
            // 
            // InventoryLocationstextBoxdescription
            // 
            this.InventoryLocationstextBoxdescription.BackColor = System.Drawing.Color.White;
            this.InventoryLocationstextBoxdescription.Location = new System.Drawing.Point(14, 74);
            this.InventoryLocationstextBoxdescription.Margin = new System.Windows.Forms.Padding(2);
            this.InventoryLocationstextBoxdescription.MaxLength = 250;
            this.InventoryLocationstextBoxdescription.Multiline = true;
            this.InventoryLocationstextBoxdescription.Name = "InventoryLocationstextBoxdescription";
            this.InventoryLocationstextBoxdescription.ReadOnly = true;
            this.InventoryLocationstextBoxdescription.Size = new System.Drawing.Size(301, 99);
            this.InventoryLocationstextBoxdescription.TabIndex = 5;
            // 
            // InventoryLocationstextBoxname
            // 
            this.InventoryLocationstextBoxname.BackColor = System.Drawing.Color.White;
            this.InventoryLocationstextBoxname.Location = new System.Drawing.Point(14, 32);
            this.InventoryLocationstextBoxname.Margin = new System.Windows.Forms.Padding(2);
            this.InventoryLocationstextBoxname.MaxLength = 20;
            this.InventoryLocationstextBoxname.Name = "InventoryLocationstextBoxname";
            this.InventoryLocationstextBoxname.ReadOnly = true;
            this.InventoryLocationstextBoxname.Size = new System.Drawing.Size(301, 21);
            this.InventoryLocationstextBoxname.TabIndex = 4;
            this.InventoryLocationstextBoxname.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.InventoryLocationstextBoxname_PreviewKeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 57);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Description";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(11, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Details);
            this.tabControl1.Location = new System.Drawing.Point(271, 11);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(407, 354);
            this.tabControl1.TabIndex = 1;
            // 
            // ListBoxInventoryLocation
            // 
            this.ListBoxInventoryLocation.FormattingEnabled = true;
            this.ListBoxInventoryLocation.Location = new System.Drawing.Point(11, 34);
            this.ListBoxInventoryLocation.Name = "ListBoxInventoryLocation";
            this.ListBoxInventoryLocation.Size = new System.Drawing.Size(253, 329);
            this.ListBoxInventoryLocation.TabIndex = 0;
            this.ListBoxInventoryLocation.SelectedIndexChanged += new System.EventHandler(this.ListBoxInventoryLocation_SelectedIndexChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LocationErrorMsg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 414);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(689, 22);
            this.statusStrip1.TabIndex = 78;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // LocationErrorMsg
            // 
            this.LocationErrorMsg.BackColor = System.Drawing.SystemColors.MenuBar;
            this.LocationErrorMsg.Name = "LocationErrorMsg";
            this.LocationErrorMsg.Size = new System.Drawing.Size(49, 17);
            this.LocationErrorMsg.Text = "              ";
            // 
            // InventoryLocationstextBoxsearch
            // 
            this.InventoryLocationstextBoxsearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InventoryLocationstextBoxsearch.Delay = true;
            this.InventoryLocationstextBoxsearch.DelayTime = 1000;
            this.InventoryLocationstextBoxsearch.Location = new System.Drawing.Point(11, 11);
            this.InventoryLocationstextBoxsearch.MaxLength = 35;
            this.InventoryLocationstextBoxsearch.Name = "InventoryLocationstextBoxsearch";
            this.InventoryLocationstextBoxsearch.Searchstartfrom = 2;
            this.InventoryLocationstextBoxsearch.Size = new System.Drawing.Size(253, 21);
            this.InventoryLocationstextBoxsearch.TabIndex = 132;
            this.InventoryLocationstextBoxsearch.TextChanged += new System.EventHandler(this.InventoryLocationstextBoxsearch_TextChanged);
            this.InventoryLocationstextBoxsearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InventoryLocationstextBoxsearch_KeyDown);
            // 
            // FormInventoryLocations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 436);
            this.Controls.Add(this.InventoryLocationstextBoxsearch);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.ListBoxInventoryLocation);
            this.Controls.Add(this.BtnInventoryLocationsExit);
            this.Controls.Add(this.BtnInventoryLocationsDelete);
            this.Controls.Add(this.BtnInventoryLocationsNew);
            this.Controls.Add(this.BtnInventoryLocationsSave);
            this.Controls.Add(this.BtnInventoryLocationsCancel);
            this.Controls.Add(this.BtnInventoryLocationsEdit);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormInventoryLocations";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Inventory Location";
            this.Load += new System.EventHandler(this.FormInventoryLocations_Load);
            this.Details.ResumeLayout(false);
            this.Details.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button BtnInventoryLocationsExit;
        private System.Windows.Forms.Button BtnInventoryLocationsDelete;
        private System.Windows.Forms.Button BtnInventoryLocationsNew;
        private System.Windows.Forms.Button BtnInventoryLocationsSave;
        private System.Windows.Forms.Button BtnInventoryLocationsCancel;
        private System.Windows.Forms.Button BtnInventoryLocationsEdit;
        private System.Windows.Forms.TabPage Details;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TextBox InventoryLocationstextBoxdescription;
        private System.Windows.Forms.TextBox InventoryLocationstextBoxname;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox ListBoxInventoryLocation;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel LocationErrorMsg;
        private controls.text.DelayedTextChangeTextBox InventoryLocationstextBoxsearch;
        private controls.ComboBoxSwapTextBox InventoryLocationsComboBoxType;
    }
}