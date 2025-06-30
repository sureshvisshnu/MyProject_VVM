namespace fa.views.controls.hms
{
    partial class IPQueueGrid
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
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            GridViewIp = new DataViewVerticalScroll();
            TokenNumber = new DataGridViewTextBoxColumn();
            PatientName = new DataGridViewTextBoxColumn();
            PatientAge = new DataGridViewTextBoxColumn();
            DateOfVisit = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            primaryConsultant = new DataGridViewTextBoxColumn();
            primaryCareTaker = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            Status = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)GridViewIp).BeginInit();
            SuspendLayout();
            // 
            // GridViewIp
            // 
            GridViewIp.AllowUserToAddRows = false;
            GridViewIp.AllowUserToDeleteRows = false;
            GridViewIp.AllowUserToResizeColumns = false;
            GridViewIp.AllowUserToResizeRows = false;
            GridViewIp.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            GridViewIp.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewIp.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewIp.ColumnHeadersHeight = 20;
            GridViewIp.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewIp.Columns.AddRange(new DataGridViewColumn[] { TokenNumber, PatientName, PatientAge, DateOfVisit, Column2, Column3, primaryConsultant, primaryCareTaker, Column1, Status });
            GridViewIp.EnableHeadersVisualStyles = false;
            GridViewIp.Location = new Point(0, 0);
            GridViewIp.MultiSelect = false;
            GridViewIp.Name = "GridViewIp";
            GridViewIp.ReadOnly = true;
            GridViewIp.RowHeadersVisible = false;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            GridViewIp.RowsDefaultCellStyle = dataGridViewCellStyle3;
            GridViewIp.RowTemplate.Height = 20;
            GridViewIp.ScrollBars = ScrollBars.Vertical;
            GridViewIp.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewIp.ShowCellToolTips = false;
            GridViewIp.Size = new Size(1051, 209);
            GridViewIp.TabIndex = 4;
            GridViewIp.CellClick += GridViewIp_CellClick;
            GridViewIp.DataError += GridViewIp_DataError;
            GridViewIp.RowEnter += GridViewIp_RowEnter;
            GridViewIp.KeyDown += GridViewIp_KeyDown;
            GridViewIp.PreviewKeyDown += GridViewIp_PreviewKeyDown;
            // 
            // TokenNumber
            // 
            TokenNumber.HeaderText = "Patient Id";
            TokenNumber.Name = "TokenNumber";
            TokenNumber.ReadOnly = true;
            TokenNumber.Resizable = DataGridViewTriState.False;
            TokenNumber.SortMode = DataGridViewColumnSortMode.NotSortable;
            TokenNumber.Width = 130;
            // 
            // PatientName
            // 
            PatientName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            PatientName.HeaderText = "Name";
            PatientName.Name = "PatientName";
            PatientName.ReadOnly = true;
            PatientName.Resizable = DataGridViewTriState.False;
            PatientName.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // PatientAge
            // 
            PatientAge.HeaderText = "Age";
            PatientAge.Name = "PatientAge";
            PatientAge.ReadOnly = true;
            PatientAge.Resizable = DataGridViewTriState.False;
            PatientAge.SortMode = DataGridViewColumnSortMode.NotSortable;
            PatientAge.Width = 35;
            // 
            // DateOfVisit
            // 
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            DateOfVisit.DefaultCellStyle = dataGridViewCellStyle2;
            DateOfVisit.HeaderText = "Admitted On";
            DateOfVisit.Name = "DateOfVisit";
            DateOfVisit.ReadOnly = true;
            DateOfVisit.Resizable = DataGridViewTriState.False;
            DateOfVisit.SortMode = DataGridViewColumnSortMode.NotSortable;
            DateOfVisit.Width = 80;
            // 
            // Column2
            // 
            Column2.HeaderText = "Ward";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Resizable = DataGridViewTriState.False;
            Column2.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column3
            // 
            Column3.HeaderText = "Bed";
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            Column3.Resizable = DataGridViewTriState.False;
            Column3.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column3.Width = 95;
            // 
            // primaryConsultant
            // 
            primaryConsultant.HeaderText = "Primary Consultant";
            primaryConsultant.Name = "primaryConsultant";
            primaryConsultant.ReadOnly = true;
            primaryConsultant.Resizable = DataGridViewTriState.False;
            primaryConsultant.SortMode = DataGridViewColumnSortMode.NotSortable;
            primaryConsultant.Width = 150;
            // 
            // primaryCareTaker
            // 
            primaryCareTaker.HeaderText = "Primary Care Taker";
            primaryCareTaker.Name = "primaryCareTaker";
            primaryCareTaker.ReadOnly = true;
            primaryCareTaker.Resizable = DataGridViewTriState.False;
            primaryCareTaker.SortMode = DataGridViewColumnSortMode.NotSortable;
            primaryCareTaker.Width = 150;
            // 
            // Column1
            // 
            Column1.HeaderText = "ID";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Visible = false;
            // 
            // Status
            // 
            Status.HeaderText = "Status";
            Status.Name = "Status";
            Status.ReadOnly = true;
            Status.Resizable = DataGridViewTriState.False;
            Status.SortMode = DataGridViewColumnSortMode.NotSortable;
            Status.Width = 80;
            // 
            // IPQueueGrid
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(GridViewIp);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "IPQueueGrid";
            Size = new Size(1054, 220);
            ClientSizeChanged += IPQueueGrid_ClientSizeChanged;
            ((System.ComponentModel.ISupportInitialize)GridViewIp).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataViewVerticalScroll GridViewIp;
        private DataGridViewTextBoxColumn TokenNumber;
        private DataGridViewTextBoxColumn PatientName;
        private DataGridViewTextBoxColumn PatientAge;
        private DataGridViewTextBoxColumn DateOfVisit;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn primaryConsultant;
        private DataGridViewTextBoxColumn primaryCareTaker;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Status;
    }
}
