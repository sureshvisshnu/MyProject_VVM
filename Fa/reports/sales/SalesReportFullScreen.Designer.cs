namespace fa.reports.sales
{
    partial class SalesReportFullScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SalesReportFullScreen));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.BottomPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.MiddlePanel = new System.Windows.Forms.FlowLayoutPanel();
            this.DataGrid = new fa.views.controls.DataViewVerticalScroll();
            this.ab2ToolStrip1 = new fa.views.controls.Ab2ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.ComboBoxReportType = new System.Windows.Forms.ToolStripComboBox();
            this.LabelCategory = new System.Windows.Forms.ToolStripLabel();
            this.ComboBoxCategory = new fa.views.controls.ToolStripComboTree();
            this.LabelCustomer = new System.Windows.Forms.ToolStripLabel();
            this.ComboBoxCustomer = new fa.views.controls.ToolStripComboTree();
            this.LabelItem = new System.Windows.Forms.ToolStripLabel();
            this.ComboBoxItem = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.FromDate = new fa.views.controls.ToolStripCalendar();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.ToDate = new fa.views.controls.ToolStripCalendar();
            this.BtnGo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.miniToolStrip = new fa.views.controls.Ab2ToolStrip();
            this.BottomPanel.SuspendLayout();
            this.MiddlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid)).BeginInit();
            this.ab2ToolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 617);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1174, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.button1);
            this.BottomPanel.Controls.Add(this.button2);
            this.BottomPanel.Controls.Add(this.button3);
            this.BottomPanel.Controls.Add(this.button4);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.BottomPanel.Location = new System.Drawing.Point(0, 577);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Padding = new System.Windows.Forms.Padding(5);
            this.BottomPanel.Size = new System.Drawing.Size(1174, 40);
            this.BottomPanel.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1086, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1005, 8);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(924, 8);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(843, 8);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // MiddlePanel
            // 
            this.MiddlePanel.Controls.Add(this.DataGrid);
            this.MiddlePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MiddlePanel.Location = new System.Drawing.Point(0, 38);
            this.MiddlePanel.Name = "MiddlePanel";
            this.MiddlePanel.Size = new System.Drawing.Size(1174, 539);
            this.MiddlePanel.TabIndex = 6;
            // 
            // DataGrid
            // 
            this.DataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGrid.Location = new System.Drawing.Point(3, 3);
            this.DataGrid.Name = "DataGrid";
            this.DataGrid.Size = new System.Drawing.Size(1167, 536);
            this.DataGrid.TabIndex = 0;
            // 
            // ab2ToolStrip1
            // 
            this.ab2ToolStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ab2ToolStrip1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ab2ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ab2ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.ComboBoxReportType,
            this.LabelCategory,
            this.ComboBoxCategory,
            this.LabelCustomer,
            this.ComboBoxCustomer,
            this.LabelItem,
            this.ComboBoxItem,
            this.toolStripLabel2,
            this.FromDate,
            this.toolStripLabel3,
            this.ToDate,
            this.BtnGo,
            this.toolStripSeparator1,
            this.toolStripButton3,
            this.toolStripButton2});
            this.ab2ToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.ab2ToolStrip1.Name = "ab2ToolStrip1";
            this.ab2ToolStrip1.Padding = new System.Windows.Forms.Padding(5);
            this.ab2ToolStrip1.Size = new System.Drawing.Size(1174, 38);
            this.ab2ToolStrip1.TabIndex = 5;
            this.ab2ToolStrip1.Text = "ab2ToolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(35, 25);
            this.toolStripLabel1.Text = "Type";
            // 
            // ComboBoxReportType
            // 
            this.ComboBoxReportType.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.ComboBoxReportType.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ComboBoxReportType.Items.AddRange(new object[] {
            "By Invoice",
            "By Category",
            "By Customer",
            "By Item",
            "By Serial",
            "Tax Report"});
            this.ComboBoxReportType.Name = "ComboBoxReportType";
            this.ComboBoxReportType.Size = new System.Drawing.Size(121, 28);
            this.ComboBoxReportType.Text = "By Invoice";
            // 
            // LabelCategory
            // 
            this.LabelCategory.Name = "LabelCategory";
            this.LabelCategory.Size = new System.Drawing.Size(56, 25);
            this.LabelCategory.Text = "Category";
            this.LabelCategory.Visible = false;
            // 
            // ComboBoxCategory
            // 
            this.ComboBoxCategory.BackColor = System.Drawing.Color.White;
            this.ComboBoxCategory.Name = "ComboBoxCategory";
            this.ComboBoxCategory.SelectedNode = null;
            this.ComboBoxCategory.Size = new System.Drawing.Size(175, 25);
            this.ComboBoxCategory.Visible = false;
            // 
            // LabelCustomer
            // 
            this.LabelCustomer.Name = "LabelCustomer";
            this.LabelCustomer.Size = new System.Drawing.Size(59, 25);
            this.LabelCustomer.Text = "Customer";
            this.LabelCustomer.Visible = false;
            // 
            // ComboBoxCustomer
            // 
            this.ComboBoxCustomer.BackColor = System.Drawing.Color.White;
            this.ComboBoxCustomer.Name = "ComboBoxCustomer";
            this.ComboBoxCustomer.SelectedNode = null;
            this.ComboBoxCustomer.Size = new System.Drawing.Size(175, 25);
            this.ComboBoxCustomer.Visible = false;
            // 
            // LabelItem
            // 
            this.LabelItem.Name = "LabelItem";
            this.LabelItem.Size = new System.Drawing.Size(33, 25);
            this.LabelItem.Text = "Item";
            this.LabelItem.Visible = false;
            // 
            // ComboBoxItem
            // 
            this.ComboBoxItem.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.ComboBoxItem.Name = "ComboBoxItem";
            this.ComboBoxItem.Size = new System.Drawing.Size(121, 28);
            this.ComboBoxItem.Visible = false;
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(34, 25);
            this.toolStripLabel2.Text = "From";
            // 
            // FromDate
            // 
            this.FromDate.BackColor = System.Drawing.Color.White;
            this.FromDate.Date = null;
            this.FromDate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FromDate.Format = "MM/dd/yyyy";
            this.FromDate.MaxDate = new System.DateTime(9997, 12, 31, 9, 37, 17, 0);
            this.FromDate.MinDate = new System.DateTime(1900, 1, 1, 17, 45, 40, 0);
            this.FromDate.Name = "FromDate";
            this.FromDate.Size = new System.Drawing.Size(97, 25);
            this.FromDate.Text = "toolStripCalendar1";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(22, 25);
            this.toolStripLabel3.Text = "To";
            // 
            // ToDate
            // 
            this.ToDate.BackColor = System.Drawing.Color.White;
            this.ToDate.Date = null;
            this.ToDate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ToDate.Format = "MM/dd/yyyy";
            this.ToDate.MaxDate = new System.DateTime(9997, 12, 31, 9, 37, 17, 0);
            this.ToDate.MinDate = new System.DateTime(1900, 1, 1, 17, 45, 40, 0);
            this.ToDate.Name = "ToDate";
            this.ToDate.Size = new System.Drawing.Size(97, 25);
            this.ToDate.Text = "toolStripCalendar2";
            // 
            // BtnGo
            // 
            this.BtnGo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BtnGo.Image = ((System.Drawing.Image)(resources.GetObject("BtnGo.Image")));
            this.BtnGo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnGo.Name = "BtnGo";
            this.BtnGo.Size = new System.Drawing.Size(26, 25);
            this.BtnGo.Text = "Go";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Black;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 25);
            this.toolStripButton3.Text = "Save";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Black;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 25);
            this.toolStripButton2.Text = "Print";
            // 
            // miniToolStrip
            // 
            this.miniToolStrip.AutoSize = false;
            this.miniToolStrip.CanOverflow = false;
            this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.miniToolStrip.Location = new System.Drawing.Point(9, 3);
            this.miniToolStrip.Name = "miniToolStrip";
            this.miniToolStrip.Padding = new System.Windows.Forms.Padding(5);
            this.miniToolStrip.Size = new System.Drawing.Size(111, 25);
            this.miniToolStrip.TabIndex = 0;
            // 
            // SalesReportFullScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1174, 639);
            this.Controls.Add(this.MiddlePanel);
            this.Controls.Add(this.ab2ToolStrip1);
            this.Controls.Add(this.BottomPanel);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SalesReportFullScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SalesReportFullScreen";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.SalesReportFullScreen_Load);
            this.Resize += new System.EventHandler(this.SalesReportFullScreen_Resize);
            this.BottomPanel.ResumeLayout(false);
            this.MiddlePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid)).EndInit();
            this.ab2ToolStrip1.ResumeLayout(false);
            this.ab2ToolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private views.controls.Ab2ToolStrip miniToolStrip;
        private System.Windows.Forms.FlowLayoutPanel BottomPanel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private views.controls.Ab2ToolStrip ab2ToolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox ComboBoxReportType;
        private System.Windows.Forms.ToolStripLabel LabelCategory;
        private views.controls.ToolStripComboTree ComboBoxCategory;
        private System.Windows.Forms.ToolStripLabel LabelCustomer;
        private views.controls.ToolStripComboTree ComboBoxCustomer;
        private System.Windows.Forms.ToolStripLabel LabelItem;
        private System.Windows.Forms.ToolStripComboBox ComboBoxItem;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private views.controls.ToolStripCalendar FromDate;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private views.controls.ToolStripCalendar ToDate;
        private System.Windows.Forms.ToolStripButton BtnGo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.FlowLayoutPanel MiddlePanel;
        private views.controls.DataViewVerticalScroll DataGrid;
    }
}