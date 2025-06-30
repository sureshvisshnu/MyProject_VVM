namespace fa.views.controls.hms
{
    partial class PatientVisitControl
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
            label18 = new Label();
            GridViewIpOpVisit = new DataViewVerticalScroll();
            DateOfVisit = new DataGridViewTextBoxColumn();
            TypeOfVisit = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)GridViewIpOpVisit).BeginInit();
            SuspendLayout();
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(3, 3);
            label18.Name = "label18";
            label18.Size = new Size(63, 13);
            label18.TabIndex = 46;
            label18.Text = "Visit History";
            // 
            // GridViewIpOpVisit
            // 
            GridViewIpOpVisit.AllowUserToAddRows = false;
            GridViewIpOpVisit.AllowUserToDeleteRows = false;
            GridViewIpOpVisit.AllowUserToResizeColumns = false;
            GridViewIpOpVisit.AllowUserToResizeRows = false;
            GridViewIpOpVisit.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewIpOpVisit.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewIpOpVisit.ColumnHeadersHeight = 20;
            GridViewIpOpVisit.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewIpOpVisit.Columns.AddRange(new DataGridViewColumn[] { DateOfVisit, TypeOfVisit });
            GridViewIpOpVisit.EnableHeadersVisualStyles = false;
            GridViewIpOpVisit.Location = new Point(3, 21);
            GridViewIpOpVisit.Name = "GridViewIpOpVisit";
            GridViewIpOpVisit.ReadOnly = true;
            GridViewIpOpVisit.RowHeadersVisible = false;
            GridViewIpOpVisit.RowTemplate.Height = 20;
            GridViewIpOpVisit.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewIpOpVisit.ShowCellToolTips = false;
            GridViewIpOpVisit.Size = new Size(185, 174);
            GridViewIpOpVisit.TabIndex = 45;
            GridViewIpOpVisit.TabStop = false;
            GridViewIpOpVisit.ClientSizeChanged += PatientVisitControl_ClientSizeChanged;
            // 
            // DateOfVisit
            // 
            DateOfVisit.HeaderText = "Date";
            DateOfVisit.Name = "DateOfVisit";
            DateOfVisit.ReadOnly = true;
            DateOfVisit.Resizable = DataGridViewTriState.False;
            DateOfVisit.SortMode = DataGridViewColumnSortMode.NotSortable;
            DateOfVisit.Width = 82;
            // 
            // TypeOfVisit
            // 
            TypeOfVisit.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            TypeOfVisit.HeaderText = "IP/OP";
            TypeOfVisit.Name = "TypeOfVisit";
            TypeOfVisit.ReadOnly = true;
            TypeOfVisit.Resizable = DataGridViewTriState.False;
            TypeOfVisit.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // PatientVisitControl
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(label18);
            Controls.Add(GridViewIpOpVisit);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "PatientVisitControl";
            Size = new Size(190, 209);
            Load += PatientVisitWidget_Load;
            ClientSizeChanged += PatientVisitControl_ClientSizeChanged;
            ((System.ComponentModel.ISupportInitialize)GridViewIpOpVisit).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label18;
        private DataViewVerticalScroll GridViewIpOpVisit;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateOfVisit;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeOfVisit;
    }
}
