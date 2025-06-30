namespace fa.views.account.masters
{
    partial class FormCostCenter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCostCenter));
            this.TextBoxCostCenterSearch = new System.Windows.Forms.TextBox();
            this.TreeViewCostCenter = new System.Windows.Forms.TreeView();
            this.ImageListCostCenter = new System.Windows.Forms.ImageList(this.components);
            this.TextBoxCostcenterId = new System.Windows.Forms.TextBox();
            this.BtnCostCenterSave = new System.Windows.Forms.Button();
            this.BtnCostCenterCancel = new System.Windows.Forms.Button();
            this.BtnCostCenterEdit = new System.Windows.Forms.Button();
            this.BtnCostCenterDelete = new System.Windows.Forms.Button();
            this.BtnCostCenterNew = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TextBoxCostCenterParentCompany = new System.Windows.Forms.TextBox();
            this.TextBoxCostCenterDisplayas = new System.Windows.Forms.TextBox();
            this.LabelCostCenterDisplayAs = new System.Windows.Forms.Label();
            this.ComboBoxCostCenterCompany = new System.Windows.Forms.ComboBox();
            this.TextBoxCostCenterDescription = new System.Windows.Forms.TextBox();
            this.TextBoxCostCenterName = new System.Windows.Forms.TextBox();
            this.LabelCostCenterParentCompany = new System.Windows.Forms.Label();
            this.LabelCostCenterDescription = new System.Windows.Forms.Label();
            this.LabelCostCenterName = new System.Windows.Forms.Label();
            this.BtnCostCenterExit = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolStripStatusLabelErrorCostCenter = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProductIdTransport
            // 
            this.ProductIdTransport.Size = new System.Drawing.Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            this.ProductBatchIdTransport.Size = new System.Drawing.Size(100, 21);
            // 
            // CustomerIdTransport
            // 
            // 
            // TextBoxCostCenterSearch
            // 
            this.TextBoxCostCenterSearch.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxCostCenterSearch.Location = new System.Drawing.Point(13, 11);
            this.TextBoxCostCenterSearch.MaxLength = 30;
            this.TextBoxCostCenterSearch.Name = "TextBoxCostCenterSearch";
            this.TextBoxCostCenterSearch.Size = new System.Drawing.Size(239, 21);
            this.TextBoxCostCenterSearch.TabIndex = 1;
            this.TextBoxCostCenterSearch.TextChanged += new System.EventHandler(this.TextBoxCostcenterSearch_TextChanged);
            this.TextBoxCostCenterSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxCostcenterSearch_KeyDown);
            this.TextBoxCostCenterSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxCostCenterSearch_KeyPress);
            // 
            // TreeViewCostCenter
            // 
            this.TreeViewCostCenter.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TreeViewCostCenter.HideSelection = false;
            this.TreeViewCostCenter.ImageIndex = 0;
            this.TreeViewCostCenter.ImageList = this.ImageListCostCenter;
            this.TreeViewCostCenter.Location = new System.Drawing.Point(13, 38);
            this.TreeViewCostCenter.Name = "TreeViewCostCenter";
            this.TreeViewCostCenter.SelectedImageIndex = 0;
            this.TreeViewCostCenter.Size = new System.Drawing.Size(239, 339);
            this.TreeViewCostCenter.TabIndex = 2;
            this.TreeViewCostCenter.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewCostcenter_AfterSelect);
            // 
            // ImageListCostCenter
            // 
            this.ImageListCostCenter.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageListCostCenter.ImageStream")));
            this.ImageListCostCenter.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageListCostCenter.Images.SetKeyName(0, "company2.ico");
            this.ImageListCostCenter.Images.SetKeyName(1, "CostCenter.ico");
            // 
            // TextBoxCostcenterId
            // 
            this.TextBoxCostcenterId.Location = new System.Drawing.Point(434, 7);
            this.TextBoxCostcenterId.Name = "TextBoxCostcenterId";
            this.TextBoxCostcenterId.Size = new System.Drawing.Size(100, 21);
            this.TextBoxCostcenterId.TabIndex = 14;
            this.TextBoxCostcenterId.Visible = false;
            // 
            // BtnCostCenterSave
            // 
            this.BtnCostCenterSave.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCostCenterSave.Location = new System.Drawing.Point(662, 5);
            this.BtnCostCenterSave.Name = "BtnCostCenterSave";
            this.BtnCostCenterSave.Size = new System.Drawing.Size(83, 23);
            this.BtnCostCenterSave.TabIndex = 10;
            this.BtnCostCenterSave.Text = "Save [F8]";
            this.BtnCostCenterSave.UseVisualStyleBackColor = true;
            this.BtnCostCenterSave.Click += new System.EventHandler(this.BtnCostcenterSave_Click);
            this.BtnCostCenterSave.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.BtnCostCenterSave_PreviewKeyDown);
            // 
            // BtnCostCenterCancel
            // 
            this.BtnCostCenterCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCostCenterCancel.Location = new System.Drawing.Point(573, 5);
            this.BtnCostCenterCancel.Name = "BtnCostCenterCancel";
            this.BtnCostCenterCancel.Size = new System.Drawing.Size(83, 23);
            this.BtnCostCenterCancel.TabIndex = 11;
            this.BtnCostCenterCancel.Text = "Cancel [Esc]";
            this.BtnCostCenterCancel.UseVisualStyleBackColor = true;
            this.BtnCostCenterCancel.Click += new System.EventHandler(this.BtnCostcenterCancel_Click);
            // 
            // BtnCostCenterEdit
            // 
            this.BtnCostCenterEdit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCostCenterEdit.Location = new System.Drawing.Point(181, 5);
            this.BtnCostCenterEdit.Name = "BtnCostCenterEdit";
            this.BtnCostCenterEdit.Size = new System.Drawing.Size(83, 23);
            this.BtnCostCenterEdit.TabIndex = 5;
            this.BtnCostCenterEdit.Text = "Edit [F7]";
            this.BtnCostCenterEdit.UseVisualStyleBackColor = true;
            this.BtnCostCenterEdit.Click += new System.EventHandler(this.BtnCostcenterEdit_Click);
            // 
            // BtnCostCenterDelete
            // 
            this.BtnCostCenterDelete.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCostCenterDelete.Location = new System.Drawing.Point(92, 5);
            this.BtnCostCenterDelete.Name = "BtnCostCenterDelete";
            this.BtnCostCenterDelete.Size = new System.Drawing.Size(83, 23);
            this.BtnCostCenterDelete.TabIndex = 4;
            this.BtnCostCenterDelete.Text = "Delete [F4]";
            this.BtnCostCenterDelete.UseVisualStyleBackColor = true;
            this.BtnCostCenterDelete.Click += new System.EventHandler(this.BtnCostcenterDelete_Click);
            // 
            // BtnCostCenterNew
            // 
            this.BtnCostCenterNew.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCostCenterNew.Location = new System.Drawing.Point(3, 5);
            this.BtnCostCenterNew.Name = "BtnCostCenterNew";
            this.BtnCostCenterNew.Size = new System.Drawing.Size(83, 23);
            this.BtnCostCenterNew.TabIndex = 3;
            this.BtnCostCenterNew.Text = "New [F3]";
            this.BtnCostCenterNew.UseVisualStyleBackColor = true;
            this.BtnCostCenterNew.Click += new System.EventHandler(this.BtnCostcenterNew_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox1.Controls.Add(this.TextBoxCostCenterParentCompany);
            this.groupBox1.Controls.Add(this.TextBoxCostCenterDisplayas);
            this.groupBox1.Controls.Add(this.LabelCostCenterDisplayAs);
            this.groupBox1.Controls.Add(this.ComboBoxCostCenterCompany);
            this.groupBox1.Controls.Add(this.TextBoxCostCenterDescription);
            this.groupBox1.Controls.Add(this.TextBoxCostCenterName);
            this.groupBox1.Controls.Add(this.LabelCostCenterParentCompany);
            this.groupBox1.Controls.Add(this.LabelCostCenterDescription);
            this.groupBox1.Controls.Add(this.LabelCostCenterName);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(263, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(587, 370);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cost Center Detail";
            // 
            // TextBoxCostCenterParentCompany
            // 
            this.TextBoxCostCenterParentCompany.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxCostCenterParentCompany.Location = new System.Drawing.Point(19, 202);
            this.TextBoxCostCenterParentCompany.MaxLength = 35;
            this.TextBoxCostCenterParentCompany.Name = "TextBoxCostCenterParentCompany";
            this.TextBoxCostCenterParentCompany.Size = new System.Drawing.Size(321, 21);
            this.TextBoxCostCenterParentCompany.TabIndex = 9;
            // 
            // TextBoxCostCenterDisplayas
            // 
            this.TextBoxCostCenterDisplayas.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxCostCenterDisplayas.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxCostCenterDisplayas.Location = new System.Drawing.Point(19, 80);
            this.TextBoxCostCenterDisplayas.MaxLength = 50;
            this.TextBoxCostCenterDisplayas.Name = "TextBoxCostCenterDisplayas";
            this.TextBoxCostCenterDisplayas.ReadOnly = true;
            this.TextBoxCostCenterDisplayas.Size = new System.Drawing.Size(452, 21);
            this.TextBoxCostCenterDisplayas.TabIndex = 7;
            this.TextBoxCostCenterDisplayas.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxCostCenterDisplayas_KeyDown);
            this.TextBoxCostCenterDisplayas.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxCostcenterDisplayas_KeyPress);
            this.TextBoxCostCenterDisplayas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TextBoxCostCenterDisplayas_MouseDown);
            // 
            // LabelCostCenterDisplayAs
            // 
            this.LabelCostCenterDisplayAs.AutoSize = true;
            this.LabelCostCenterDisplayAs.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCostCenterDisplayAs.Location = new System.Drawing.Point(16, 64);
            this.LabelCostCenterDisplayAs.Name = "LabelCostCenterDisplayAs";
            this.LabelCostCenterDisplayAs.Size = new System.Drawing.Size(56, 13);
            this.LabelCostCenterDisplayAs.TabIndex = 6;
            this.LabelCostCenterDisplayAs.Text = "Display As";
            // 
            // ComboBoxCostCenterCompany
            // 
            this.ComboBoxCostCenterCompany.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ComboBoxCostCenterCompany.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboBoxCostCenterCompany.Enabled = false;
            this.ComboBoxCostCenterCompany.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBoxCostCenterCompany.FormattingEnabled = true;
            this.ComboBoxCostCenterCompany.Location = new System.Drawing.Point(19, 202);
            this.ComboBoxCostCenterCompany.Name = "ComboBoxCostCenterCompany";
            this.ComboBoxCostCenterCompany.Size = new System.Drawing.Size(321, 21);
            this.ComboBoxCostCenterCompany.TabIndex = 10;
            this.ComboBoxCostCenterCompany.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ComboBoxCostCenterCompany_KeyPress);
            // 
            // TextBoxCostCenterDescription
            // 
            this.TextBoxCostCenterDescription.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxCostCenterDescription.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxCostCenterDescription.Location = new System.Drawing.Point(19, 122);
            this.TextBoxCostCenterDescription.MaxLength = 250;
            this.TextBoxCostCenterDescription.Multiline = true;
            this.TextBoxCostCenterDescription.Name = "TextBoxCostCenterDescription";
            this.TextBoxCostCenterDescription.ReadOnly = true;
            this.TextBoxCostCenterDescription.Size = new System.Drawing.Size(452, 59);
            this.TextBoxCostCenterDescription.TabIndex = 8;
            this.TextBoxCostCenterDescription.TabStop = false;
            // 
            // TextBoxCostCenterName
            // 
            this.TextBoxCostCenterName.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxCostCenterName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxCostCenterName.Location = new System.Drawing.Point(19, 39);
            this.TextBoxCostCenterName.MaxLength = 30;
            this.TextBoxCostCenterName.Name = "TextBoxCostCenterName";
            this.TextBoxCostCenterName.ReadOnly = true;
            this.TextBoxCostCenterName.Size = new System.Drawing.Size(452, 21);
            this.TextBoxCostCenterName.TabIndex = 6;
            this.TextBoxCostCenterName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxCostCenterName_KeyDown);
            this.TextBoxCostCenterName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxCostcenterName_KeyPress);
            this.TextBoxCostCenterName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TextBoxCostCenterName_MouseDown);
            this.TextBoxCostCenterName.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TextBoxCostCenterName_PreviewKeyDown);
            // 
            // LabelCostCenterParentCompany
            // 
            this.LabelCostCenterParentCompany.AutoSize = true;
            this.LabelCostCenterParentCompany.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCostCenterParentCompany.Location = new System.Drawing.Point(16, 185);
            this.LabelCostCenterParentCompany.Name = "LabelCostCenterParentCompany";
            this.LabelCostCenterParentCompany.Size = new System.Drawing.Size(101, 13);
            this.LabelCostCenterParentCompany.TabIndex = 2;
            this.LabelCostCenterParentCompany.Text = "Parent Company";
            // 
            // LabelCostCenterDescription
            // 
            this.LabelCostCenterDescription.AutoSize = true;
            this.LabelCostCenterDescription.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCostCenterDescription.Location = new System.Drawing.Point(16, 105);
            this.LabelCostCenterDescription.Name = "LabelCostCenterDescription";
            this.LabelCostCenterDescription.Size = new System.Drawing.Size(60, 13);
            this.LabelCostCenterDescription.TabIndex = 1;
            this.LabelCostCenterDescription.Text = "Description";
            // 
            // LabelCostCenterName
            // 
            this.LabelCostCenterName.AutoSize = true;
            this.LabelCostCenterName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCostCenterName.Location = new System.Drawing.Point(16, 22);
            this.LabelCostCenterName.Name = "LabelCostCenterName";
            this.LabelCostCenterName.Size = new System.Drawing.Size(39, 13);
            this.LabelCostCenterName.TabIndex = 0;
            this.LabelCostCenterName.Text = "Name";
            // 
            // BtnCostCenterExit
            // 
            this.BtnCostCenterExit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCostCenterExit.Location = new System.Drawing.Point(751, 5);
            this.BtnCostCenterExit.Name = "BtnCostCenterExit";
            this.BtnCostCenterExit.Size = new System.Drawing.Size(83, 23);
            this.BtnCostCenterExit.TabIndex = 12;
            this.BtnCostCenterExit.Text = "Exit [F10]";
            this.BtnCostCenterExit.UseVisualStyleBackColor = true;
            this.BtnCostCenterExit.Click += new System.EventHandler(this.BtnCurrencyExit_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripStatusLabelErrorCostCenter});
            this.statusStrip1.Location = new System.Drawing.Point(0, 427);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(862, 22);
            this.statusStrip1.TabIndex = 15;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ToolStripStatusLabelErrorCostCenter
            // 
            this.ToolStripStatusLabelErrorCostCenter.Name = "ToolStripStatusLabelErrorCostCenter";
            this.ToolStripStatusLabelErrorCostCenter.Size = new System.Drawing.Size(49, 17);
            this.ToolStripStatusLabelErrorCostCenter.Text = "              ";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.BtnCostCenterNew);
            this.panel1.Controls.Add(this.BtnCostCenterDelete);
            this.panel1.Controls.Add(this.BtnCostCenterEdit);
            this.panel1.Controls.Add(this.BtnCostCenterExit);
            this.panel1.Controls.Add(this.BtnCostCenterSave);
            this.panel1.Controls.Add(this.BtnCostCenterCancel);
            this.panel1.Controls.Add(this.TextBoxCostcenterId);
            this.panel1.Location = new System.Drawing.Point(13, 384);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(837, 40);
            this.panel1.TabIndex = 11;
            // 
            // FormCostCenter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(862, 449);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.TreeViewCostCenter);
            this.Controls.Add(this.TextBoxCostCenterSearch);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCostCenter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cost Centers";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCostCenter_FormClosing);
            this.Load += new System.EventHandler(this.FormCostCenter_Load);
            this.Controls.SetChildIndex(this.TextBoxCostCenterSearch, 0);
            this.Controls.SetChildIndex(this.TreeViewCostCenter, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.statusStrip1, 0);
            this.Controls.SetChildIndex(this.ProductIdTransport, 0);
            this.Controls.SetChildIndex(this.ProductBatchIdTransport, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBoxCostCenterSearch;
        private System.Windows.Forms.TreeView TreeViewCostCenter;
        private System.Windows.Forms.TextBox TextBoxCostcenterId;
        private System.Windows.Forms.Button BtnCostCenterSave;
        private System.Windows.Forms.Button BtnCostCenterCancel;
        private System.Windows.Forms.Button BtnCostCenterEdit;
        private System.Windows.Forms.Button BtnCostCenterDelete;
        private System.Windows.Forms.Button BtnCostCenterNew;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox TextBoxCostCenterDisplayas;
        private System.Windows.Forms.Label LabelCostCenterDisplayAs;
        private System.Windows.Forms.ComboBox ComboBoxCostCenterCompany;
        private System.Windows.Forms.TextBox TextBoxCostCenterDescription;
        private System.Windows.Forms.TextBox TextBoxCostCenterName;
        private System.Windows.Forms.Label LabelCostCenterParentCompany;
        private System.Windows.Forms.Label LabelCostCenterDescription;
        private System.Windows.Forms.Label LabelCostCenterName;
        private System.Windows.Forms.Button BtnCostCenterExit;
        private System.Windows.Forms.TextBox TextBoxCostCenterParentCompany;
        private System.Windows.Forms.ImageList ImageListCostCenter;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabelErrorCostCenter;
        private System.Windows.Forms.Panel panel1;
    }
}