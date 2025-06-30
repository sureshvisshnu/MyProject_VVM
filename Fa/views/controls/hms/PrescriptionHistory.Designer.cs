namespace fa.views.controls.hms
{
    partial class PrescriptionHistory
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
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            label2 = new Label();
            DataGrid = new DataViewVerticalScroll();
            Column1 = new DataGridViewTextBoxColumn();
            PrescDate = new DataGridViewTextBoxColumn();
            refnumber = new DataGridViewTextBoxColumn();
            authorizer = new DataGridViewTextBoxColumn();
            Details = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)DataGrid).BeginInit();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(1, 3);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(111, 15);
            label2.TabIndex = 2;
            label2.Text = "Prescription History";
            // 
            // DataGrid
            // 
            DataGrid.AllowUserToAddRows = false;
            DataGrid.AllowUserToResizeRows = false;
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
            DataGrid.Columns.AddRange(new DataGridViewColumn[] { Column1, PrescDate, refnumber, authorizer, Details, Column2, Column4, Column3 });
            DataGrid.EnableHeadersVisualStyles = false;
            DataGrid.Location = new Point(4, 23);
            DataGrid.Margin = new Padding(4, 3, 4, 3);
            DataGrid.MultiSelect = false;
            DataGrid.Name = "DataGrid";
            DataGrid.ReadOnly = true;
            DataGrid.RowHeadersVisible = false;
            DataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DataGrid.ShowCellToolTips = false;
            DataGrid.Size = new Size(439, 173);
            DataGrid.TabIndex = 3;
            DataGrid.CellClick += DataGrid_CellClick;
            DataGrid.CellEnter += DataGrid_CellEnter;
            DataGrid.CellMouseClick += DataGrid_CellMouseClick;
            DataGrid.DataError += DataGrid_DataError;
            // 
            // Column1
            // 
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.ForeColor = Color.Black;
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
            PrescDate.HeaderText = "Date";
            PrescDate.Name = "PrescDate";
            PrescDate.ReadOnly = true;
            PrescDate.Resizable = DataGridViewTriState.False;
            PrescDate.SortMode = DataGridViewColumnSortMode.NotSortable;
            PrescDate.Width = 120;
            // 
            // refnumber
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopRight;
            refnumber.DefaultCellStyle = dataGridViewCellStyle3;
            refnumber.HeaderText = "Prescription Number";
            refnumber.Name = "refnumber";
            refnumber.ReadOnly = true;
            refnumber.Resizable = DataGridViewTriState.False;
            refnumber.SortMode = DataGridViewColumnSortMode.NotSortable;
            refnumber.Width = 110;
            // 
            // authorizer
            // 
            authorizer.HeaderText = "Prescribed By";
            authorizer.Name = "authorizer";
            authorizer.ReadOnly = true;
            authorizer.Resizable = DataGridViewTriState.False;
            authorizer.SortMode = DataGridViewColumnSortMode.NotSortable;
            authorizer.Width = 140;
            // 
            // Details
            // 
            Details.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Details.HeaderText = "Details";
            Details.Name = "Details";
            Details.ReadOnly = true;
            Details.Resizable = DataGridViewTriState.False;
            Details.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            Column2.HeaderText = "NoteID";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Resizable = DataGridViewTriState.False;
            Column2.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column2.Visible = false;
            // 
            // Column4
            // 
            Column4.HeaderText = "EmployeeId";
            Column4.Name = "Column4";
            Column4.ReadOnly = true;
            Column4.Resizable = DataGridViewTriState.False;
            Column4.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column4.Visible = false;
            // 
            // Column3
            // 
            Column3.HeaderText = "IsDischarge";
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            Column3.Resizable = DataGridViewTriState.False;
            Column3.Visible = false;
            // 
            // PrescriptionHistory
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(DataGrid);
            Controls.Add(label2);
            Margin = new Padding(4, 3, 4, 3);
            Name = "PrescriptionHistory";
            Size = new Size(444, 198);
            Enter += PrescriptionHistory_Enter;
            Resize += PrescriptionHistory_ClientSizeChanged;
            ((System.ComponentModel.ISupportInitialize)DataGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataViewVerticalScroll DataGrid;
        private System.Windows.Forms.Label label2;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn PrescDate;
        private DataGridViewTextBoxColumn refnumber;
        private DataGridViewTextBoxColumn authorizer;
        private DataGridViewTextBoxColumn Details;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewCheckBoxColumn Column3;
    }
}
