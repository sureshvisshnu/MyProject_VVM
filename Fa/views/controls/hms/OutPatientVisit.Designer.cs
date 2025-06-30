namespace fa.views.controls.hms
{
    partial class OutPatientVisit
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
            label11 = new Label();
            GridViewOutPatient = new DataViewVerticalScroll();
            OPVisitDate = new DataGridViewTextBoxColumn();
            OPVisitReason = new DataGridViewTextBoxColumn();
            OPConsultant = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)GridViewOutPatient).BeginInit();
            SuspendLayout();
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label11.Location = new Point(1, 4);
            label11.Name = "label11";
            label11.Size = new Size(89, 13);
            label11.TabIndex = 18;
            label11.Text = "Out Patient Visits";
            // 
            // GridViewOutPatient
            // 
            GridViewOutPatient.AllowUserToAddRows = false;
            GridViewOutPatient.AllowUserToDeleteRows = false;
            GridViewOutPatient.AllowUserToResizeColumns = false;
            GridViewOutPatient.AllowUserToResizeRows = false;
            GridViewOutPatient.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewOutPatient.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewOutPatient.ColumnHeadersHeight = 20;
            GridViewOutPatient.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewOutPatient.Columns.AddRange(new DataGridViewColumn[] { OPVisitDate, OPVisitReason, OPConsultant });
            GridViewOutPatient.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewOutPatient.EnableHeadersVisualStyles = false;
            GridViewOutPatient.Location = new Point(4, 22);
            GridViewOutPatient.Name = "GridViewOutPatient";
            GridViewOutPatient.ReadOnly = true;
            GridViewOutPatient.RowHeadersVisible = false;
            GridViewOutPatient.RowTemplate.Height = 20;
            GridViewOutPatient.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewOutPatient.ShowCellToolTips = false;
            GridViewOutPatient.Size = new Size(574, 175);
            GridViewOutPatient.TabIndex = 19;
            GridViewOutPatient.CellClick += GridViewOutPatient_CellClick;
            // 
            // OPVisitDate
            // 
            OPVisitDate.HeaderText = "Visit Date";
            OPVisitDate.Name = "OPVisitDate";
            OPVisitDate.ReadOnly = true;
            OPVisitDate.Resizable = DataGridViewTriState.False;
            OPVisitDate.SortMode = DataGridViewColumnSortMode.NotSortable;
            OPVisitDate.Width = 120;
            // 
            // OPVisitReason
            // 
            OPVisitReason.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            OPVisitReason.HeaderText = "Reason";
            OPVisitReason.Name = "OPVisitReason";
            OPVisitReason.ReadOnly = true;
            OPVisitReason.Resizable = DataGridViewTriState.False;
            OPVisitReason.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // OPConsultant
            // 
            OPConsultant.HeaderText = "OP Consultant";
            OPConsultant.Name = "OPConsultant";
            OPConsultant.ReadOnly = true;
            OPConsultant.Resizable = DataGridViewTriState.False;
            OPConsultant.SortMode = DataGridViewColumnSortMode.NotSortable;
            OPConsultant.Width = 200;
            // 
            // OutPatientVisit
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(label11);
            Controls.Add(GridViewOutPatient);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "OutPatientVisit";
            Size = new Size(581, 200);
            SizeChanged += OutPatientVisit_SizeChanged;
            Enter += OutPatientVisit_Enter;
            Resize += OutPatientVisit_Resize;
            ((System.ComponentModel.ISupportInitialize)GridViewOutPatient).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label11;
        private DataViewVerticalScroll GridViewOutPatient;
        private DataGridViewTextBoxColumn OPVisitDate;
        private DataGridViewTextBoxColumn OPVisitReason;
        private DataGridViewTextBoxColumn OPConsultant;
    }
}
