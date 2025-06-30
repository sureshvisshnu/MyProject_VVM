namespace fa.views.controls.hms
{
    partial class LabTestHistory
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGrid = new DataViewVerticalScroll();
            Column1 = new DataGridViewTextBoxColumn();
            PrescDate = new DataGridViewTextBoxColumn();
            Details = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)DataGrid).BeginInit();
            SuspendLayout();
            // 
            // DataGrid
            // 
            DataGrid.AllowUserToAddRows = false;
            DataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            DataGrid.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            DataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            DataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGrid.Columns.AddRange(new DataGridViewColumn[] { Column1, PrescDate, Details, Column2 });
            DataGrid.EnableHeadersVisualStyles = false;
            DataGrid.Location = new Point(7, 25);
            DataGrid.Margin = new Padding(4, 3, 4, 3);
            DataGrid.MultiSelect = false;
            DataGrid.Name = "DataGrid";
            DataGrid.ReadOnly = true;
            DataGrid.RowHeadersVisible = false;
            DataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DataGrid.ShowCellToolTips = false;
            DataGrid.Size = new Size(350, 173);
            DataGrid.TabIndex = 5;
            DataGrid.CellClick += DataGrid_CellClick;
            DataGrid.CellEnter += DataGrid_CellEnter;
            DataGrid.DataError += DataGrid_DataError;
            DataGrid.ClientSizeChanged += DataGrid_ClientSizeChanged;
            // 
            // Column1
            // 
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.Window;
            Column1.DefaultCellStyle = dataGridViewCellStyle2;
            Column1.HeaderText = "...";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 25;
            // 
            // PrescDate
            // 
            PrescDate.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            PrescDate.HeaderText = "Date";
            PrescDate.Name = "PrescDate";
            PrescDate.ReadOnly = true;
            PrescDate.Resizable = DataGridViewTriState.False;
            PrescDate.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Details
            // 
            Details.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Details.HeaderText = "Name";
            Details.Name = "Details";
            Details.ReadOnly = true;
            Details.Resizable = DataGridViewTriState.False;
            Details.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            Column2.HeaderText = "LID";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Resizable = DataGridViewTriState.False;
            Column2.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column2.Visible = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(5, 6);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(87, 15);
            label2.TabIndex = 4;
            label2.Text = "LabTest History";
            // 
            // LabTestHistory
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(DataGrid);
            Controls.Add(label2);
            Margin = new Padding(4, 3, 4, 3);
            Name = "LabTestHistory";
            Size = new Size(365, 216);
            ClientSizeChanged += LabTestHistory_ClientSizeChanged;
            ((System.ComponentModel.ISupportInitialize)DataGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataViewVerticalScroll DataGrid;
        private System.Windows.Forms.Label label2;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn PrescDate;
        private DataGridViewTextBoxColumn Details;
        private DataGridViewTextBoxColumn Column2;
    }
}
