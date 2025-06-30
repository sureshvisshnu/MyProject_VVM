namespace fa.views.purchase
{
    partial class FormPurchaseBarcodePrint
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPurchaseBarcodePrint));
            BtnExit = new Button();
            BtnCancel = new Button();
            statusStrip1 = new StatusStrip();
            ErrorMsg = new ToolStripStatusLabel();
            ab2ToolStrip1 = new controls.Ab2ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            ComboBoxPaperSize = new ToolStripComboBox();
            LabelLabelSize = new ToolStripLabel();
            ComboBoxLabelSize = new ToolStripComboBox();
            LabelQRCodeSize = new ToolStripLabel();
            ComboBoxQRCodeSize = new ToolStripComboBox();
            LabelStartLocation = new ToolStripLabel();
            TextBoxLocation = new ToolStripTextBox();
            DefaultPrinter = new ToolStripLabel();
            ComboBoxDefaultPrinter = new ToolStripComboBox();
            GridViewPurchaseDetails = new controls.DataViewVerticalScroll();
            Column1 = new DataGridViewTextBoxColumn();
            Column7 = new DataGridViewCheckBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new controls.grid.DataGridViewNameColumn();
            Column11 = new DataGridViewTextBoxColumn();
            Column9 = new DataGridViewTextBoxColumn();
            Column5 = new controls.grid.DataGridViewCurrencyColumn();
            Column6 = new controls.grid.DataGridViewCurrencyColumn();
            Column8 = new DataGridViewCheckBoxColumn();
            Column4 = new controls.grid.DataGridViewNumberColumn();
            Column10 = new DataGridViewCheckBoxColumn();
            ID = new DataGridViewTextBoxColumn();
            ImageList = new ImageList(components);
            BtnPrint = new Dropdown_Button.UserControlButtonWithMenu();
            statusStrip1.SuspendLayout();
            ab2ToolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewPurchaseDetails).BeginInit();
            SuspendLayout();
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(905, 294);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(75, 25);
            BtnExit.TabIndex = 16;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(739, 295);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(83, 25);
            BtnCancel.TabIndex = 14;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsg });
            statusStrip1.Location = new Point(0, 326);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(992, 22);
            statusStrip1.TabIndex = 17;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(34, 17);
            ErrorMsg.Text = "         ";
            // 
            // ab2ToolStrip1
            // 
            ab2ToolStrip1.BackColor = SystemColors.ControlLight;
            ab2ToolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            ab2ToolStrip1.ImageScalingSize = new Size(20, 20);
            ab2ToolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, ComboBoxPaperSize, LabelLabelSize, ComboBoxLabelSize, LabelQRCodeSize, ComboBoxQRCodeSize, LabelStartLocation, TextBoxLocation, DefaultPrinter, ComboBoxDefaultPrinter });
            ab2ToolStrip1.Location = new Point(0, 0);
            ab2ToolStrip1.Name = "ab2ToolStrip1";
            ab2ToolStrip1.Padding = new Padding(5);
            ab2ToolStrip1.Size = new Size(992, 37);
            ab2ToolStrip1.TabIndex = 18;
            ab2ToolStrip1.Text = "ab2ToolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(60, 24);
            toolStripLabel1.Text = "Paper Size";
            // 
            // ComboBoxPaperSize
            // 
            ComboBoxPaperSize.FlatStyle = FlatStyle.Standard;
            ComboBoxPaperSize.Items.AddRange(new object[] { "A4", "Label" });
            ComboBoxPaperSize.Name = "ComboBoxPaperSize";
            ComboBoxPaperSize.Size = new Size(121, 27);
            ComboBoxPaperSize.SelectedIndexChanged += ComboBoxPaperSize_SelectedIndexChanged;
            // 
            // LabelLabelSize
            // 
            LabelLabelSize.Name = "LabelLabelSize";
            LabelLabelSize.Size = new Size(58, 24);
            LabelLabelSize.Text = "Label Size";
            LabelLabelSize.Visible = false;
            // 
            // ComboBoxLabelSize
            // 
            ComboBoxLabelSize.FlatStyle = FlatStyle.Standard;
            ComboBoxLabelSize.Items.AddRange(new object[] { "35 mm * 25 mm", "50 mm * 25 mm", "100 mm* 23 mm" });
            ComboBoxLabelSize.Name = "ComboBoxLabelSize";
            ComboBoxLabelSize.Size = new Size(121, 27);
            ComboBoxLabelSize.Visible = false;
            // 
            // LabelQRCodeSize
            // 
            LabelQRCodeSize.Name = "LabelQRCodeSize";
            LabelQRCodeSize.Size = new Size(77, 24);
            LabelQRCodeSize.Text = "QR Code Size";
            LabelQRCodeSize.Visible = false;
            // 
            // ComboBoxQRCodeSize
            // 
            ComboBoxQRCodeSize.FlatStyle = FlatStyle.Standard;
            ComboBoxQRCodeSize.Items.AddRange(new object[] { "1 * 1", "1.5 * 1.5", "2 * 2" });
            ComboBoxQRCodeSize.Name = "ComboBoxQRCodeSize";
            ComboBoxQRCodeSize.Size = new Size(121, 27);
            ComboBoxQRCodeSize.Visible = false;
            // 
            // LabelStartLocation
            // 
            LabelStartLocation.Name = "LabelStartLocation";
            LabelStartLocation.Size = new Size(80, 24);
            LabelStartLocation.Text = "Start Location";
            LabelStartLocation.Visible = false;
            // 
            // TextBoxLocation
            // 
            TextBoxLocation.BorderStyle = BorderStyle.FixedSingle;
            TextBoxLocation.MaxLength = 3;
            TextBoxLocation.Name = "TextBoxLocation";
            TextBoxLocation.Size = new Size(50, 27);
            TextBoxLocation.Text = "1,1";
            // 
            // DefaultPrinter
            // 
            DefaultPrinter.Name = "DefaultPrinter";
            DefaultPrinter.Size = new Size(85, 24);
            DefaultPrinter.Text = "Choose Printer";
            // 
            // ComboBoxDefaultPrinter
            // 
            ComboBoxDefaultPrinter.FlatStyle = FlatStyle.Standard;
            ComboBoxDefaultPrinter.Name = "ComboBoxDefaultPrinter";
            ComboBoxDefaultPrinter.Size = new Size(200, 23);
            // 
            // GridViewPurchaseDetails
            // 
            GridViewPurchaseDetails.AllowUserToAddRows = false;
            GridViewPurchaseDetails.AllowUserToDeleteRows = false;
            GridViewPurchaseDetails.AllowUserToResizeColumns = false;
            GridViewPurchaseDetails.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewPurchaseDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewPurchaseDetails.ColumnHeadersHeight = 20;
            GridViewPurchaseDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewPurchaseDetails.Columns.AddRange(new DataGridViewColumn[] { Column1, Column7, Column2, Column3, Column11, Column9, Column5, Column6, Column8, Column4, Column10, ID });
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = SystemColors.Window;
            dataGridViewCellStyle10.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle10.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.False;
            GridViewPurchaseDetails.DefaultCellStyle = dataGridViewCellStyle10;
            GridViewPurchaseDetails.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewPurchaseDetails.EnableHeadersVisualStyles = false;
            GridViewPurchaseDetails.Location = new Point(7, 39);
            GridViewPurchaseDetails.Name = "GridViewPurchaseDetails";
            GridViewPurchaseDetails.RowHeadersVisible = false;
            GridViewPurchaseDetails.RowHeadersWidth = 51;
            dataGridViewCellStyle11.BackColor = Color.White;
            dataGridViewCellStyle11.ForeColor = Color.Black;
            dataGridViewCellStyle11.SelectionBackColor = Color.White;
            dataGridViewCellStyle11.SelectionForeColor = Color.Black;
            GridViewPurchaseDetails.RowsDefaultCellStyle = dataGridViewCellStyle11;
            GridViewPurchaseDetails.RowTemplate.Height = 20;
            GridViewPurchaseDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewPurchaseDetails.ShowCellErrors = false;
            GridViewPurchaseDetails.ShowCellToolTips = false;
            GridViewPurchaseDetails.Size = new Size(978, 241);
            GridViewPurchaseDetails.TabIndex = 0;
            GridViewPurchaseDetails.CellClick += GridViewPurchaseDetails_CellClick;
            GridViewPurchaseDetails.RowEnter += GridViewPurchaseDetails_RowEnter;
            // 
            // Column1
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            Column1.DefaultCellStyle = dataGridViewCellStyle2;
            Column1.HeaderText = "#";
            Column1.MinimumWidth = 6;
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 25;
            // 
            // Column7
            // 
            Column7.HeaderText = "Select";
            Column7.MinimumWidth = 6;
            Column7.Name = "Column7";
            Column7.ReadOnly = true;
            Column7.Resizable = DataGridViewTriState.False;
            Column7.Width = 40;
            // 
            // Column2
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            Column2.DefaultCellStyle = dataGridViewCellStyle3;
            Column2.HeaderText = "Item Code";
            Column2.MinimumWidth = 6;
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Resizable = DataGridViewTriState.False;
            Column2.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column2.Width = 125;
            // 
            // Column3
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            Column3.DefaultCellStyle = dataGridViewCellStyle4;
            Column3.HeaderText = "Item Name";
            Column3.MinimumWidth = 6;
            Column3.Name = "Column3";
            Column3.NameLength = 20;
            Column3.Resizable = DataGridViewTriState.False;
            Column3.Width = 175;
            // 
            // Column11
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            Column11.DefaultCellStyle = dataGridViewCellStyle5;
            Column11.HeaderText = "Batch No";
            Column11.MinimumWidth = 6;
            Column11.Name = "Column11";
            Column11.Resizable = DataGridViewTriState.False;
            Column11.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column11.Width = 125;
            // 
            // Column9
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            Column9.DefaultCellStyle = dataGridViewCellStyle6;
            Column9.HeaderText = "Description";
            Column9.MaxInputLength = 12;
            Column9.MinimumWidth = 6;
            Column9.Name = "Column9";
            Column9.Resizable = DataGridViewTriState.False;
            Column9.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column9.Width = 180;
            // 
            // Column5
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            Column5.DefaultCellStyle = dataGridViewCellStyle7;
            Column5.HeaderText = "Rate";
            Column5.MinimumWidth = 6;
            Column5.Name = "Column5";
            Column5.Resizable = DataGridViewTriState.False;
            Column5.Width = 80;
            // 
            // Column6
            // 
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
            Column6.DefaultCellStyle = dataGridViewCellStyle8;
            Column6.HeaderText = "MRP";
            Column6.MinimumWidth = 6;
            Column6.Name = "Column6";
            Column6.Resizable = DataGridViewTriState.False;
            Column6.Width = 65;
            // 
            // Column8
            // 
            Column8.HeaderText = "Print MRP";
            Column8.MinimumWidth = 6;
            Column8.Name = "Column8";
            Column8.ReadOnly = true;
            Column8.Resizable = DataGridViewTriState.False;
            Column8.Width = 70;
            // 
            // Column4
            // 
            Column4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            Column4.DefaultCellStyle = dataGridViewCellStyle9;
            Column4.HeaderText = "Quantity";
            Column4.MinimumWidth = 6;
            Column4.Name = "Column4";
            Column4.NumberLength = 6;
            Column4.Resizable = DataGridViewTriState.False;
            // 
            // Column10
            // 
            Column10.HeaderText = "Isbatch";
            Column10.MinimumWidth = 6;
            Column10.Name = "Column10";
            Column10.Resizable = DataGridViewTriState.False;
            Column10.Visible = false;
            Column10.Width = 125;
            // 
            // ID
            // 
            ID.HeaderText = "ID";
            ID.Name = "ID";
            ID.Resizable = DataGridViewTriState.False;
            ID.SortMode = DataGridViewColumnSortMode.NotSortable;
            ID.Visible = false;
            // 
            // ImageList
            // 
            ImageList.ColorDepth = ColorDepth.Depth8Bit;
            ImageList.ImageSize = new Size(16, 16);
            ImageList.TransparentColor = Color.Transparent;
            // 
            // BtnPrint
            // 
            BtnPrint.ButtonText = "Print [F9]";
            BtnPrint.ImageList = null;
            BtnPrint.Items = (System.Collections.ObjectModel.Collection<string>)resources.GetObject("BtnPrint.Items");
            BtnPrint.Location = new Point(826, 295);
            BtnPrint.Margin = new Padding(4, 3, 4, 3);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(89, 29);
            BtnPrint.TabIndex = 19;
            BtnPrint.ItemClickedEvent += BtnPrint_ItemClickedEvent;
            BtnPrint.PreviewKeyDown += BtnPrint_PreviewKeyDown;
            // 
            // FormPurchaseBarcodePrint
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(992, 348);
            Controls.Add(BtnExit);
            Controls.Add(ab2ToolStrip1);
            Controls.Add(statusStrip1);
            Controls.Add(BtnCancel);
            Controls.Add(GridViewPurchaseDetails);
            Controls.Add(BtnPrint);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormPurchaseBarcodePrint";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Purchase Barcode Print";
            Load += FormPurchaseBarcodePrint_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ab2ToolStrip1.ResumeLayout(false);
            ab2ToolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewPurchaseDetails).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private controls.DataViewVerticalScroll GridViewPurchaseDetails;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ErrorMsg;
        private controls.Ab2ToolStrip ab2ToolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox ComboBoxPaperSize;
        private System.Windows.Forms.ToolStripLabel LabelLabelSize;
        private System.Windows.Forms.ToolStripComboBox ComboBoxLabelSize;
        private System.Windows.Forms.ToolStripLabel LabelStartLocation;
        private System.Windows.Forms.ToolStripLabel LabelQRCodeSize;
        private System.Windows.Forms.ToolStripComboBox ComboBoxQRCodeSize;
        private System.Windows.Forms.ToolStripLabel DefaultPrinter;
        private System.Windows.Forms.ToolStripComboBox ComboBoxDefaultPrinter;
        private System.Windows.Forms.ImageList ImageList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private controls.grid.DataGridViewNameColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private controls.grid.DataGridViewCurrencyColumn Column5;
        private controls.grid.DataGridViewCurrencyColumn Column6;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column8;
        private controls.grid.DataGridViewNumberColumn Column4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private Dropdown_Button.UserControlButtonWithMenu BtnPrint;
        private ToolStripTextBox TextBoxLocation;
    }
}