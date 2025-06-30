namespace fa.views.catalog
{
    partial class FormCatalogParent
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCatalogParent));
            this.TreeViewCatalog = new System.Windows.Forms.TreeView();
            this.ImageListCatalog = new System.Windows.Forms.ImageList(this.components);
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnSelect = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ErrorMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripSearchLabel = new System.Windows.Forms.ToolStripLabel();
            this.ab2ToolStripCatalogParent = new fa.views.controls.Ab2ToolStrip();
            this.ToolStripSearchTextBox = new fa.views.controls.ToolstripDelayedTextBox();
            this.statusStrip1.SuspendLayout();
            this.ab2ToolStripCatalogParent.SuspendLayout();
            this.SuspendLayout();
            // 
            // TreeViewCatalog
            // 
            this.TreeViewCatalog.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TreeViewCatalog.HideSelection = false;
            this.TreeViewCatalog.ImageIndex = 0;
            this.TreeViewCatalog.ImageList = this.ImageListCatalog;
            this.TreeViewCatalog.Location = new System.Drawing.Point(4, 38);
            this.TreeViewCatalog.Name = "TreeViewCatalog";
            this.TreeViewCatalog.SelectedImageIndex = 0;
            this.TreeViewCatalog.Size = new System.Drawing.Size(286, 390);
            this.TreeViewCatalog.StateImageList = this.ImageListCatalog;
            this.TreeViewCatalog.TabIndex = 3;
            this.TreeViewCatalog.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewCatalog_AfterSelect);
            this.TreeViewCatalog.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeViewCatalog_NodeMouseDoubleClick);
            this.TreeViewCatalog.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TreeViewCatalog_KeyDown);
            // 
            // ImageListCatalog
            // 
            this.ImageListCatalog.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ImageListCatalog.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageListCatalog.ImageStream")));
            this.ImageListCatalog.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageListCatalog.Images.SetKeyName(0, "Category.png");
            this.ImageListCatalog.Images.SetKeyName(1, "product.png");
            this.ImageListCatalog.Images.SetKeyName(2, "sku.png");
            // 
            // BtnCancel
            // 
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.BtnCancel.Location = new System.Drawing.Point(108, 433);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(88, 23);
            this.BtnCancel.TabIndex = 42;
            this.BtnCancel.Text = "Cancel [Esc]";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            this.BtnCancel.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.BtnCancel_PreviewKeyDown);
            // 
            // BtnSelect
            // 
            this.BtnSelect.Enabled = false;
            this.BtnSelect.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.BtnSelect.Location = new System.Drawing.Point(202, 433);
            this.BtnSelect.Name = "BtnSelect";
            this.BtnSelect.Size = new System.Drawing.Size(88, 23);
            this.BtnSelect.TabIndex = 41;
            this.BtnSelect.Text = "Select [F8]";
            this.BtnSelect.UseVisualStyleBackColor = true;
            this.BtnSelect.Click += new System.EventHandler(this.BtnSelect_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ErrorMsg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 461);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(295, 22);
            this.statusStrip1.TabIndex = 43;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            this.ErrorMsg.Name = "ErrorMsg";
            this.ErrorMsg.Size = new System.Drawing.Size(28, 17);
            this.ErrorMsg.Text = "       ";
            // 
            // ToolStripSearchLabel
            // 
            this.ToolStripSearchLabel.Name = "ToolStripSearchLabel";
            this.ToolStripSearchLabel.Size = new System.Drawing.Size(42, 22);
            this.ToolStripSearchLabel.Text = "Search";
            // 
            // ab2ToolStripCatalogParent
            // 
            this.ab2ToolStripCatalogParent.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ab2ToolStripCatalogParent.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripSearchLabel,
            this.ToolStripSearchTextBox});
            this.ab2ToolStripCatalogParent.Location = new System.Drawing.Point(0, 0);
            this.ab2ToolStripCatalogParent.Name = "ab2ToolStripCatalogParent";
            this.ab2ToolStripCatalogParent.Padding = new System.Windows.Forms.Padding(5);
            this.ab2ToolStripCatalogParent.Size = new System.Drawing.Size(295, 35);
            this.ab2ToolStripCatalogParent.TabIndex = 4;
            this.ab2ToolStripCatalogParent.Text = "ab2ToolStrip1";
            // 
            // ToolStripSearchTextBox
            // 
            this.ToolStripSearchTextBox.AutoSize = false;
            this.ToolStripSearchTextBox.Delay = true;
            this.ToolStripSearchTextBox.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ToolStripSearchTextBox.Name = "ToolStripSearchTextBox";
            this.ToolStripSearchTextBox.Size = new System.Drawing.Size(239, 22);
            this.ToolStripSearchTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ToolStripSearchTextBox_KeyDown);
            this.ToolStripSearchTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ToolStripSearchTextBox_KeyPress);
            this.ToolStripSearchTextBox.TextChanged += new System.EventHandler(this.toolStripTextBox1_TextChanged);
            // 
            // FormCatalogParent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 483);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnSelect);
            this.Controls.Add(this.ab2ToolStripCatalogParent);
            this.Controls.Add(this.TreeViewCatalog);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCatalogParent";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Search Parent";
            this.Load += new System.EventHandler(this.FormCatalogParent_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ab2ToolStripCatalogParent.ResumeLayout(false);
            this.ab2ToolStripCatalogParent.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TreeView TreeViewCatalog;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnSelect;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ErrorMsg;
        private System.Windows.Forms.ToolStripLabel ToolStripSearchLabel;
        private controls.Ab2ToolStrip ab2ToolStripCatalogParent;
        private controls.ToolstripDelayedTextBox ToolStripSearchTextBox;
        private System.Windows.Forms.ImageList ImageListCatalog;
    }
}