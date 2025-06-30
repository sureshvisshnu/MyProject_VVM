namespace fa.views.controls.hms
{
    partial class InPatientVisit
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
            GridViewInPatient = new DataViewVerticalScroll();
            IPVisitDate = new DataGridViewTextBoxColumn();
            IPVisitReason = new DataGridViewTextBoxColumn();
            IPDischargedOn = new DataGridViewTextBoxColumn();
            label12 = new Label();
            ((System.ComponentModel.ISupportInitialize)GridViewInPatient).BeginInit();
            SuspendLayout();
            // 
            // GridViewInPatient
            // 
            GridViewInPatient.AllowUserToAddRows = false;
            GridViewInPatient.AllowUserToDeleteRows = false;
            GridViewInPatient.AllowUserToResizeColumns = false;
            GridViewInPatient.AllowUserToResizeRows = false;
            GridViewInPatient.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewInPatient.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewInPatient.ColumnHeadersHeight = 20;
            GridViewInPatient.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewInPatient.Columns.AddRange(new DataGridViewColumn[] { IPVisitDate, IPVisitReason, IPDischargedOn });
            GridViewInPatient.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewInPatient.EnableHeadersVisualStyles = false;
            GridViewInPatient.Location = new Point(3, 19);
            GridViewInPatient.Name = "GridViewInPatient";
            GridViewInPatient.ReadOnly = true;
            GridViewInPatient.RowHeadersVisible = false;
            GridViewInPatient.RowTemplate.Height = 20;
            GridViewInPatient.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewInPatient.ShowCellToolTips = false;
            GridViewInPatient.Size = new Size(571, 200);
            GridViewInPatient.TabIndex = 20;
            GridViewInPatient.CellClick += GridViewInPatient_CellClick;
            // 
            // IPVisitDate
            // 
            IPVisitDate.HeaderText = "Admitted On";
            IPVisitDate.Name = "IPVisitDate";
            IPVisitDate.ReadOnly = true;
            IPVisitDate.Resizable = DataGridViewTriState.False;
            IPVisitDate.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // IPVisitReason
            // 
            IPVisitReason.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            IPVisitReason.HeaderText = "Summary";
            IPVisitReason.Name = "IPVisitReason";
            IPVisitReason.ReadOnly = true;
            IPVisitReason.Resizable = DataGridViewTriState.False;
            IPVisitReason.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // IPDischargedOn
            // 
            IPDischargedOn.HeaderText = "Discharged On";
            IPDischargedOn.Name = "IPDischargedOn";
            IPDischargedOn.ReadOnly = true;
            IPDischargedOn.Resizable = DataGridViewTriState.False;
            IPDischargedOn.SortMode = DataGridViewColumnSortMode.NotSortable;
            IPDischargedOn.Width = 120;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label12.Location = new Point(3, 2);
            label12.Name = "label12";
            label12.Size = new Size(81, 13);
            label12.TabIndex = 19;
            label12.Text = "In Patient Visits";
            // 
            // InPatientVisit
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(GridViewInPatient);
            Controls.Add(label12);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "InPatientVisit";
            Size = new Size(579, 227);
            SizeChanged += InPatientVisit_SizeChanged;
            Enter += InPatientVisit_Enter;
            Resize += InPatientVisit_Resize;
            ((System.ComponentModel.ISupportInitialize)GridViewInPatient).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataViewVerticalScroll GridViewInPatient;
        private System.Windows.Forms.DataGridViewTextBoxColumn IPVisitDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn IPVisitReason;
        private System.Windows.Forms.DataGridViewTextBoxColumn IPDischargedOn;
        private System.Windows.Forms.Label label12;
    }
}
