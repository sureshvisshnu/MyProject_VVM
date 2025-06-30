namespace Fa.views.catalog
{
    partial class FormCatalogTax
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
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCatalogTax));
            DataGridViewCurrentTax = new fa.views.controls.DataViewVerticalScroll();
            TaxName = new DataGridViewComboBoxColumn();
            Percentage = new fa.views.controls.grid.DataGridViewCurrencyColumn();
            EffectiveFrom = new fa.views.controls.grid.DataGridViewCalendarColumn();
            EffectiveTo = new fa.views.controls.grid.DataGridViewCalendarColumn();
            Remove = new DataGridViewTextBoxColumn();
            Mapid = new DataGridViewTextBoxColumn();
            Id = new DataGridViewTextBoxColumn();
            autoLabel1 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            autoLabel2 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            DataGridViewTaxHistory = new fa.views.controls.DataViewVerticalScroll();
            TaxName1 = new DataGridViewTextBoxColumn();
            dataGridViewCurrencyColumn1 = new fa.views.controls.grid.DataGridViewCurrencyColumn();
            dataGridViewCalendarColumn1 = new fa.views.controls.grid.DataGridViewCalendarColumn();
            dataGridViewCalendarColumn2 = new fa.views.controls.grid.DataGridViewCalendarColumn();
            BtnItemTaxSave = new Button();
            BtnItemTaxExit = new Button();
            autoLabel4 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            TextBoxCatalogName = new TextBox();
            statusStrip1 = new StatusStrip();
            ErrorMsgItemTax = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)DataGridViewCurrentTax).BeginInit();
            ((System.ComponentModel.ISupportInitialize)DataGridViewTaxHistory).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Size = new Size(116, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Size = new Size(116, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Size = new Size(116, 21);
            AccountIdTransport.TabIndex = 2;
            // 
            // DataGridViewCurrentTax
            // 
            DataGridViewCurrentTax.AllowUserToDeleteRows = false;
            DataGridViewCurrentTax.AllowUserToResizeColumns = false;
            DataGridViewCurrentTax.AllowUserToResizeRows = false;
            DataGridViewCurrentTax.BackgroundColor = SystemColors.Control;
            DataGridViewCurrentTax.ColumnHeadersHeight = 20;
            DataGridViewCurrentTax.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DataGridViewCurrentTax.Columns.AddRange(new DataGridViewColumn[] { TaxName, Percentage, EffectiveFrom, EffectiveTo, Remove, Mapid, Id });
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = SystemColors.Window;
            dataGridViewCellStyle6.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle6.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            DataGridViewCurrentTax.DefaultCellStyle = dataGridViewCellStyle6;
            DataGridViewCurrentTax.EditMode = DataGridViewEditMode.EditOnEnter;
            DataGridViewCurrentTax.EnableHeadersVisualStyles = false;
            DataGridViewCurrentTax.Location = new Point(12, 79);
            DataGridViewCurrentTax.Name = "DataGridViewCurrentTax";
            DataGridViewCurrentTax.RowHeadersVisible = false;
            DataGridViewCurrentTax.RowTemplate.Height = 20;
            DataGridViewCurrentTax.ShowCellToolTips = false;
            DataGridViewCurrentTax.Size = new Size(522, 130);
            DataGridViewCurrentTax.TabIndex = 4;
            DataGridViewCurrentTax.CellClick += DataGridViewCurrentTax_CellClick;
            DataGridViewCurrentTax.CellEnter += DataGridViewCurrentTax_CellEnter;
            DataGridViewCurrentTax.CellFormatting += DataGridViewCurrentTax_CellFormatting;
            DataGridViewCurrentTax.CellLeave += DataGridViewCurrentTax_CellLeave;
            DataGridViewCurrentTax.DataError += DataGridViewCurrentTax_DataError;
            DataGridViewCurrentTax.EditingControlShowing += DataGridViewCurrentTax_EditingControlShowing;
            DataGridViewCurrentTax.RowsAdded += DataGridViewCurrentTax_RowsAdded;
            // 
            // TaxName
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.TopLeft;
            TaxName.DefaultCellStyle = dataGridViewCellStyle1;
            TaxName.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            TaxName.FlatStyle = FlatStyle.Flat;
            TaxName.HeaderText = "Name";
            TaxName.Name = "TaxName";
            TaxName.Resizable = DataGridViewTriState.True;
            TaxName.Width = 200;
            // 
            // Percentage
            // 
            Percentage.Currencylength = 5;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopRight;
            Percentage.DefaultCellStyle = dataGridViewCellStyle2;
            Percentage.HeaderText = "Percentage";
            Percentage.Name = "Percentage";
            Percentage.Width = 75;
            // 
            // EffectiveFrom
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            EffectiveFrom.DefaultCellStyle = dataGridViewCellStyle3;
            EffectiveFrom.HeaderText = "Effective From";
            EffectiveFrom.Name = "EffectiveFrom";
            // 
            // EffectiveTo
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopLeft;
            EffectiveTo.DefaultCellStyle = dataGridViewCellStyle4;
            EffectiveTo.HeaderText = "To";
            EffectiveTo.Name = "EffectiveTo";
            // 
            // Remove
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.NullValue = "X";
            Remove.DefaultCellStyle = dataGridViewCellStyle5;
            Remove.HeaderText = "...";
            Remove.Name = "Remove";
            Remove.SortMode = DataGridViewColumnSortMode.NotSortable;
            Remove.Width = 25;
            // 
            // Mapid
            // 
            Mapid.HeaderText = "Mapid";
            Mapid.Name = "Mapid";
            Mapid.Visible = false;
            // 
            // Id
            // 
            Id.HeaderText = "Id";
            Id.Name = "Id";
            Id.Visible = false;
            // 
            // autoLabel1
            // 
            autoLabel1.Location = new Point(12, 61);
            autoLabel1.Name = "autoLabel1";
            autoLabel1.Size = new Size(37, 13);
            autoLabel1.TabIndex = 10;
            autoLabel1.Text = "Active";
            // 
            // autoLabel2
            // 
            autoLabel2.Location = new Point(12, 218);
            autoLabel2.Name = "autoLabel2";
            autoLabel2.Size = new Size(50, 13);
            autoLabel2.TabIndex = 13;
            autoLabel2.Text = "Historical";
            // 
            // DataGridViewTaxHistory
            // 
            DataGridViewTaxHistory.AllowUserToAddRows = false;
            DataGridViewTaxHistory.AllowUserToDeleteRows = false;
            DataGridViewTaxHistory.AllowUserToResizeColumns = false;
            DataGridViewTaxHistory.AllowUserToResizeRows = false;
            DataGridViewTaxHistory.BackgroundColor = SystemColors.Control;
            DataGridViewTaxHistory.ColumnHeadersHeight = 20;
            DataGridViewTaxHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DataGridViewTaxHistory.Columns.AddRange(new DataGridViewColumn[] { TaxName1, dataGridViewCurrencyColumn1, dataGridViewCalendarColumn1, dataGridViewCalendarColumn2 });
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = SystemColors.Window;
            dataGridViewCellStyle11.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle11.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle11.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.False;
            DataGridViewTaxHistory.DefaultCellStyle = dataGridViewCellStyle11;
            DataGridViewTaxHistory.EnableHeadersVisualStyles = false;
            DataGridViewTaxHistory.Location = new Point(12, 235);
            DataGridViewTaxHistory.Name = "DataGridViewTaxHistory";
            DataGridViewTaxHistory.ReadOnly = true;
            DataGridViewTaxHistory.RowHeadersVisible = false;
            DataGridViewTaxHistory.RowTemplate.Height = 20;
            DataGridViewTaxHistory.ShowCellToolTips = false;
            DataGridViewTaxHistory.Size = new Size(522, 130);
            DataGridViewTaxHistory.TabIndex = 5;
            DataGridViewTaxHistory.DataError += DataGridViewTaxHistory_DataError;
            // 
            // TaxName1
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.TopLeft;
            TaxName1.DefaultCellStyle = dataGridViewCellStyle7;
            TaxName1.HeaderText = "Name";
            TaxName1.Name = "TaxName1";
            TaxName1.ReadOnly = true;
            TaxName1.SortMode = DataGridViewColumnSortMode.NotSortable;
            TaxName1.Width = 200;
            // 
            // dataGridViewCurrencyColumn1
            // 
            dataGridViewCurrencyColumn1.Currencylength = 5;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCurrencyColumn1.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCurrencyColumn1.HeaderText = "Percentage";
            dataGridViewCurrencyColumn1.Name = "dataGridViewCurrencyColumn1";
            dataGridViewCurrencyColumn1.ReadOnly = true;
            dataGridViewCurrencyColumn1.Width = 75;
            // 
            // dataGridViewCalendarColumn1
            // 
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCalendarColumn1.DefaultCellStyle = dataGridViewCellStyle9;
            dataGridViewCalendarColumn1.HeaderText = "Effective From";
            dataGridViewCalendarColumn1.Name = "dataGridViewCalendarColumn1";
            dataGridViewCalendarColumn1.ReadOnly = true;
            // 
            // dataGridViewCalendarColumn2
            // 
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCalendarColumn2.DefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewCalendarColumn2.HeaderText = "To";
            dataGridViewCalendarColumn2.Name = "dataGridViewCalendarColumn2";
            dataGridViewCalendarColumn2.ReadOnly = true;
            dataGridViewCalendarColumn2.Width = 125;
            // 
            // BtnItemTaxSave
            // 
            BtnItemTaxSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnItemTaxSave.Location = new Point(364, 374);
            BtnItemTaxSave.Name = "BtnItemTaxSave";
            BtnItemTaxSave.Size = new Size(81, 24);
            BtnItemTaxSave.TabIndex = 6;
            BtnItemTaxSave.Text = "Save [F8]";
            BtnItemTaxSave.UseVisualStyleBackColor = true;
            BtnItemTaxSave.Click += BtnItemTaxSave_Click;
            // 
            // BtnItemTaxExit
            // 
            BtnItemTaxExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnItemTaxExit.Location = new Point(451, 374);
            BtnItemTaxExit.Name = "BtnItemTaxExit";
            BtnItemTaxExit.Size = new Size(81, 24);
            BtnItemTaxExit.TabIndex = 7;
            BtnItemTaxExit.Text = "Exit [F10]";
            BtnItemTaxExit.UseVisualStyleBackColor = true;
            BtnItemTaxExit.Click += BtnItemTaxExit_Click;
            // 
            // autoLabel4
            // 
            autoLabel4.Location = new Point(12, 14);
            autoLabel4.Name = "autoLabel4";
            autoLabel4.Size = new Size(34, 13);
            autoLabel4.TabIndex = 9;
            autoLabel4.Text = "Name";
            // 
            // TextBoxCatalogName
            // 
            TextBoxCatalogName.BackColor = Color.White;
            TextBoxCatalogName.Location = new Point(12, 29);
            TextBoxCatalogName.Name = "TextBoxCatalogName";
            TextBoxCatalogName.ReadOnly = true;
            TextBoxCatalogName.Size = new Size(304, 21);
            TextBoxCatalogName.TabIndex = 3;
            TextBoxCatalogName.PreviewKeyDown += TextBoxCatalogName_PreviewKeyDown;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsgItemTax });
            statusStrip1.Location = new Point(0, 409);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(544, 22);
            statusStrip1.TabIndex = 10;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsgItemTax
            // 
            ErrorMsgItemTax.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ErrorMsgItemTax.Name = "ErrorMsgItemTax";
            ErrorMsgItemTax.Size = new Size(11, 17);
            ErrorMsgItemTax.Text = " ";
            // 
            // FormCatalogTax
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(544, 431);
            Controls.Add(statusStrip1);
            Controls.Add(autoLabel4);
            Controls.Add(TextBoxCatalogName);
            Controls.Add(BtnItemTaxExit);
            Controls.Add(BtnItemTaxSave);
            Controls.Add(autoLabel2);
            Controls.Add(DataGridViewTaxHistory);
            Controls.Add(autoLabel1);
            Controls.Add(DataGridViewCurrentTax);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormCatalogTax";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Item Tax";
            FormClosing += FormCatalogTax_FormClosing;
            Load += FormCatalogTax_Load;
            Controls.SetChildIndex(DataGridViewCurrentTax, 0);
            Controls.SetChildIndex(autoLabel1, 0);
            Controls.SetChildIndex(DataGridViewTaxHistory, 0);
            Controls.SetChildIndex(autoLabel2, 0);
            Controls.SetChildIndex(BtnItemTaxSave, 0);
            Controls.SetChildIndex(BtnItemTaxExit, 0);
            Controls.SetChildIndex(TextBoxCatalogName, 0);
            Controls.SetChildIndex(autoLabel4, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            ((System.ComponentModel.ISupportInitialize)DataGridViewCurrentTax).EndInit();
            ((System.ComponentModel.ISupportInitialize)DataGridViewTaxHistory).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private fa.views.controls.DataViewVerticalScroll DataGridViewCurrentTax;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel1;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel2;
        private fa.views.controls.DataViewVerticalScroll DataGridViewTaxHistory;
        private Button BtnItemTaxSave;
        private Button BtnItemTaxExit;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel4;
        private TextBox TextBoxCatalogName;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel ErrorMsgItemTax;
        private DataGridViewTextBoxColumn TaxName1;
        private fa.views.controls.grid.DataGridViewCurrencyColumn dataGridViewCurrencyColumn1;
        private fa.views.controls.grid.DataGridViewCalendarColumn dataGridViewCalendarColumn1;
        private fa.views.controls.grid.DataGridViewCalendarColumn dataGridViewCalendarColumn2;
        private DataGridViewComboBoxColumn TaxName;
        private fa.views.controls.grid.DataGridViewCurrencyColumn Percentage;
        private fa.views.controls.grid.DataGridViewCalendarColumn EffectiveFrom;
        private fa.views.controls.grid.DataGridViewCalendarColumn EffectiveTo;
        private DataGridViewTextBoxColumn Remove;
        private DataGridViewTextBoxColumn Mapid;
        private DataGridViewTextBoxColumn Id;
    }
}