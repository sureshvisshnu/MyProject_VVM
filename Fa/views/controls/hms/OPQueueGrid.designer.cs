namespace fa.views.controls.hms
{
    partial class OPQueueGrid
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
            GridViewOpRegistrationToken = new DataViewVerticalScroll();
            TokenNumber = new DataGridViewTextBoxColumn();
            PatientName = new DataGridViewTextBoxColumn();
            PatientAge = new DataGridViewTextBoxColumn();
            DateOfVisit = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)GridViewOpRegistrationToken).BeginInit();
            SuspendLayout();
            // 
            // GridViewOpRegistrationToken
            // 
            GridViewOpRegistrationToken.AllowUserToAddRows = false;
            GridViewOpRegistrationToken.AllowUserToDeleteRows = false;
            GridViewOpRegistrationToken.AllowUserToResizeColumns = false;
            GridViewOpRegistrationToken.AllowUserToResizeRows = false;
            GridViewOpRegistrationToken.BackgroundColor = SystemColors.Control;
            GridViewOpRegistrationToken.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewOpRegistrationToken.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewOpRegistrationToken.ColumnHeadersHeight = 20;
            GridViewOpRegistrationToken.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewOpRegistrationToken.Columns.AddRange(new DataGridViewColumn[] { TokenNumber, PatientName, PatientAge, DateOfVisit, Column2, Column1 });
            GridViewOpRegistrationToken.EnableHeadersVisualStyles = false;
            GridViewOpRegistrationToken.Location = new Point(0, 0);
            GridViewOpRegistrationToken.MultiSelect = false;
            GridViewOpRegistrationToken.Name = "GridViewOpRegistrationToken";
            GridViewOpRegistrationToken.ReadOnly = true;
            GridViewOpRegistrationToken.RowHeadersVisible = false;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            GridViewOpRegistrationToken.RowsDefaultCellStyle = dataGridViewCellStyle3;
            GridViewOpRegistrationToken.RowTemplate.Height = 20;
            GridViewOpRegistrationToken.ScrollBars = ScrollBars.Vertical;
            GridViewOpRegistrationToken.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewOpRegistrationToken.ShowCellToolTips = false;
            GridViewOpRegistrationToken.Size = new Size(530, 510);
            GridViewOpRegistrationToken.TabIndex = 3;
            GridViewOpRegistrationToken.CellClick += GridViewOpRegistrationToken_CellClick;
            GridViewOpRegistrationToken.CellDoubleClick += GridViewOpRegistrationToken_CellDoubleClick;
            GridViewOpRegistrationToken.DataError += GridViewOpRegistrationToken_DataError;
            GridViewOpRegistrationToken.RowEnter += GridViewOpRegistrationToken_RowEnter;
            GridViewOpRegistrationToken.SizeChanged += GridViewOpRegistrationToken_SizeChanged;
            GridViewOpRegistrationToken.Enter += GridViewOpRegistrationToken_Enter;
            GridViewOpRegistrationToken.KeyDown += GridViewOpRegistrationToken_KeyDown;
            GridViewOpRegistrationToken.KeyUp += GridViewOpRegistrationToken_KeyUp;
            GridViewOpRegistrationToken.PreviewKeyDown += GridViewOpRegistrationToken_PreviewKeyDown;
            // 
            // TokenNumber
            // 
            TokenNumber.HeaderText = "Token";
            TokenNumber.Name = "TokenNumber";
            TokenNumber.ReadOnly = true;
            TokenNumber.Resizable = DataGridViewTriState.False;
            TokenNumber.SortMode = DataGridViewColumnSortMode.NotSortable;
            TokenNumber.Width = 40;
            // 
            // PatientName
            // 
            PatientName.HeaderText = "Name";
            PatientName.Name = "PatientName";
            PatientName.ReadOnly = true;
            PatientName.Resizable = DataGridViewTriState.False;
            PatientName.SortMode = DataGridViewColumnSortMode.NotSortable;
            PatientName.Width = 170;
            // 
            // PatientAge
            // 
            PatientAge.HeaderText = "Age";
            PatientAge.Name = "PatientAge";
            PatientAge.ReadOnly = true;
            PatientAge.Resizable = DataGridViewTriState.False;
            PatientAge.SortMode = DataGridViewColumnSortMode.NotSortable;
            PatientAge.Width = 33;
            // 
            // DateOfVisit
            // 
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            DateOfVisit.DefaultCellStyle = dataGridViewCellStyle2;
            DateOfVisit.HeaderText = "Date/Time";
            DateOfVisit.Name = "DateOfVisit";
            DateOfVisit.ReadOnly = true;
            DateOfVisit.Resizable = DataGridViewTriState.False;
            DateOfVisit.SortMode = DataGridViewColumnSortMode.NotSortable;
            DateOfVisit.Width = 115;
            // 
            // Column2
            // 
            Column2.HeaderText = "Doctor / Consultant";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column2.Width = 150;
            // 
            // Column1
            // 
            Column1.HeaderText = "ID";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Visible = false;
            // 
            // OPQueueGrid
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(GridViewOpRegistrationToken);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "OPQueueGrid";
            Size = new Size(530, 527);
            ClientSizeChanged += OPQueueGrid_ClientSizeChanged;
            SizeChanged += OPQueueGrid_SizeChanged;
            Resize += OPQueueGrid_Resize;
            ((System.ComponentModel.ISupportInitialize)GridViewOpRegistrationToken).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataViewVerticalScroll GridViewOpRegistrationToken;
        private DataGridViewTextBoxColumn TokenNumber;
        private DataGridViewTextBoxColumn PatientName;
        private DataGridViewTextBoxColumn PatientAge;
        private DataGridViewTextBoxColumn DateOfVisit;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column1;
    }
}
