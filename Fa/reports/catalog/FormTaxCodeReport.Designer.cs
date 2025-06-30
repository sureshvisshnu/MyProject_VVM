namespace fa.reports.catalog
{
    partial class FormTaxCodeReport
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTaxCodeReport));
            TaxCodeGrid = new views.controls.DataViewVerticalScroll();
            Column1 = new DataGridViewTextBoxColumn();
            code = new DataGridViewTextBoxColumn();
            description = new DataGridViewTextBoxColumn();
            effecrivedatefrom = new DataGridViewTextBoxColumn();
            effectivedateto = new DataGridViewTextBoxColumn();
            BtnExit = new Button();
            BtnPrint = new Button();
            BtnSave = new Button();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)TaxCodeGrid).BeginInit();
            SuspendLayout();
            // 
            // TaxCodeGrid
            // 
            TaxCodeGrid.AllowUserToAddRows = false;
            TaxCodeGrid.AllowUserToDeleteRows = false;
            TaxCodeGrid.AllowUserToResizeColumns = false;
            TaxCodeGrid.AllowUserToResizeRows = false;
            TaxCodeGrid.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            TaxCodeGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            TaxCodeGrid.ColumnHeadersHeight = 20;
            TaxCodeGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            TaxCodeGrid.Columns.AddRange(new DataGridViewColumn[] { Column1, code, description, effecrivedatefrom, effectivedateto });
            TaxCodeGrid.EnableHeadersVisualStyles = false;
            TaxCodeGrid.Location = new Point(5, 7);
            TaxCodeGrid.Name = "TaxCodeGrid";
            TaxCodeGrid.ReadOnly = true;
            TaxCodeGrid.RowHeadersVisible = false;
            TaxCodeGrid.RowTemplate.Height = 20;
            TaxCodeGrid.ScrollBars = ScrollBars.Vertical;
            TaxCodeGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            TaxCodeGrid.ShowCellToolTips = false;
            TaxCodeGrid.Size = new Size(777, 538);
            TaxCodeGrid.TabIndex = 0;
            TaxCodeGrid.CellFormatting += TaxCodeGrid_CellFormatting;
            // 
            // Column1
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            Column1.DefaultCellStyle = dataGridViewCellStyle2;
            Column1.HeaderText = "#";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 50;
            // 
            // code
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            code.DefaultCellStyle = dataGridViewCellStyle3;
            code.HeaderText = "Code";
            code.Name = "code";
            code.ReadOnly = true;
            code.Resizable = DataGridViewTriState.False;
            code.SortMode = DataGridViewColumnSortMode.NotSortable;
            code.Width = 120;
            // 
            // description
            // 
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            description.DefaultCellStyle = dataGridViewCellStyle4;
            description.HeaderText = "Description";
            description.Name = "description";
            description.ReadOnly = true;
            description.Resizable = DataGridViewTriState.False;
            description.SortMode = DataGridViewColumnSortMode.NotSortable;
            description.Width = 350;
            // 
            // effecrivedatefrom
            // 
            effecrivedatefrom.HeaderText = "Effective From Date";
            effecrivedatefrom.Name = "effecrivedatefrom";
            effecrivedatefrom.ReadOnly = true;
            effecrivedatefrom.SortMode = DataGridViewColumnSortMode.NotSortable;
            effecrivedatefrom.Width = 125;
            // 
            // effectivedateto
            // 
            effectivedateto.HeaderText = "Effective To Date";
            effectivedateto.Name = "effectivedateto";
            effectivedateto.ReadOnly = true;
            effectivedateto.SortMode = DataGridViewColumnSortMode.NotSortable;
            effectivedateto.Width = 110;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(548, 560);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(72, 23);
            BtnExit.TabIndex = 16;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Enabled = false;
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(467, 560);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(75, 23);
            BtnPrint.TabIndex = 15;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnSave
            // 
            BtnSave.Enabled = false;
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(386, 560);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 23);
            BtnSave.TabIndex = 14;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // backgroundWorker1
            // 
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            // 
            // FormTaxCodeReport
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 595);
            Controls.Add(BtnExit);
            Controls.Add(BtnPrint);
            Controls.Add(BtnSave);
            Controls.Add(TaxCodeGrid);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormTaxCodeReport";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Tax Code Report";
            Load += FormTaxCodeReport_Load;
            ((System.ComponentModel.ISupportInitialize)TaxCodeGrid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private views.controls.DataViewVerticalScroll TaxCodeGrid;
        private Button BtnExit;
        private Button BtnPrint;
        private Button BtnSave;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn code;
        private DataGridViewTextBoxColumn description;
        private DataGridViewTextBoxColumn effecrivedatefrom;
        private DataGridViewTextBoxColumn effectivedateto;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}