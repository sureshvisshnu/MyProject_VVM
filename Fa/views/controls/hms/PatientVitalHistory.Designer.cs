namespace fa.views.controls.hms
{
    partial class PatientVitalHistory
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
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            label17 = new Label();
            GridViewVitalHistory = new DataViewVerticalScroll();
            VitalDateTime = new grid.DataGridViewCalendarColumn();
            VitalHeight = new grid.DataGridViewNumberColumn();
            VitalWeight = new grid.DataGridViewNumberColumn();
            VitalBMI = new grid.DataGridViewNumberColumn();
            VitalTemperature = new grid.DataGridViewNumberColumn();
            VitalPulse = new grid.DataGridViewNumberColumn();
            VitalRespRate = new grid.DataGridViewNumberColumn();
            VitalBPressure = new grid.DataGridViewNumberColumn();
            VitalBOxiLevel = new grid.DataGridViewNumberColumn();
            VitalEnteredBy = new DataGridViewTextBoxColumn();
            VitalRemove = new DataGridViewButtonColumn();
            VitalId = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)GridViewVitalHistory).BeginInit();
            SuspendLayout();
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(0, 3);
            label17.Name = "label17";
            label17.Size = new Size(69, 13);
            label17.TabIndex = 7;
            label17.Text = "Vitals History";
            // 
            // GridViewVitalHistory
            // 
            GridViewVitalHistory.AllowUserToAddRows = false;
            GridViewVitalHistory.AllowUserToDeleteRows = false;
            GridViewVitalHistory.AllowUserToResizeColumns = false;
            GridViewVitalHistory.AllowUserToResizeRows = false;
            GridViewVitalHistory.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewVitalHistory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewVitalHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GridViewVitalHistory.Columns.AddRange(new DataGridViewColumn[] { VitalDateTime, VitalHeight, VitalWeight, VitalBMI, VitalTemperature, VitalPulse, VitalRespRate, VitalBPressure, VitalBOxiLevel, VitalEnteredBy, VitalRemove, VitalId });
            GridViewVitalHistory.EnableHeadersVisualStyles = false;
            GridViewVitalHistory.Location = new Point(3, 25);
            GridViewVitalHistory.MultiSelect = false;
            GridViewVitalHistory.Name = "GridViewVitalHistory";
            GridViewVitalHistory.ReadOnly = true;
            GridViewVitalHistory.RowHeadersVisible = false;
            GridViewVitalHistory.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle11.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.False;
            GridViewVitalHistory.RowsDefaultCellStyle = dataGridViewCellStyle11;
            GridViewVitalHistory.RowTemplate.Height = 20;
            GridViewVitalHistory.ScrollBars = ScrollBars.Vertical;
            GridViewVitalHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewVitalHistory.ShowCellToolTips = false;
            GridViewVitalHistory.Size = new Size(811, 331);
            GridViewVitalHistory.TabIndex = 8;
            GridViewVitalHistory.CellClick += GridViewVitalHistory_CellClick;
            GridViewVitalHistory.CellDoubleClick += GridViewVitalHistory_CellDoubleClick;
            GridViewVitalHistory.ClientSizeChanged += GridViewVitalHistory_ClientSizeChanged;
            // 
            // VitalDateTime
            // 
            VitalDateTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            VitalDateTime.HeaderText = "Date/Time";
            VitalDateTime.Name = "VitalDateTime";
            VitalDateTime.ReadOnly = true;
            VitalDateTime.Resizable = DataGridViewTriState.False;
            // 
            // VitalHeight
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            VitalHeight.DefaultCellStyle = dataGridViewCellStyle2;
            VitalHeight.HeaderText = "Height [cm]";
            VitalHeight.Name = "VitalHeight";
            VitalHeight.NumberLength = 1;
            VitalHeight.ReadOnly = true;
            VitalHeight.Resizable = DataGridViewTriState.False;
            VitalHeight.Width = 65;
            // 
            // VitalWeight
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
            VitalWeight.DefaultCellStyle = dataGridViewCellStyle3;
            VitalWeight.HeaderText = "Weight [Kg]";
            VitalWeight.Name = "VitalWeight";
            VitalWeight.NumberLength = 1;
            VitalWeight.ReadOnly = true;
            VitalWeight.Resizable = DataGridViewTriState.False;
            VitalWeight.Width = 65;
            // 
            // VitalBMI
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
            VitalBMI.DefaultCellStyle = dataGridViewCellStyle4;
            VitalBMI.HeaderText = "BMI";
            VitalBMI.Name = "VitalBMI";
            VitalBMI.NumberLength = 1;
            VitalBMI.ReadOnly = true;
            VitalBMI.Resizable = DataGridViewTriState.False;
            VitalBMI.Width = 70;
            // 
            // VitalTemperature
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
            VitalTemperature.DefaultCellStyle = dataGridViewCellStyle5;
            VitalTemperature.HeaderText = "Temp. [°C]";
            VitalTemperature.Name = "VitalTemperature";
            VitalTemperature.NumberLength = 1;
            VitalTemperature.ReadOnly = true;
            VitalTemperature.Resizable = DataGridViewTriState.False;
            VitalTemperature.Width = 60;
            // 
            // VitalPulse
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
            VitalPulse.DefaultCellStyle = dataGridViewCellStyle6;
            VitalPulse.HeaderText = "Pulse [bpm]";
            VitalPulse.Name = "VitalPulse";
            VitalPulse.NumberLength = 1;
            VitalPulse.ReadOnly = true;
            VitalPulse.Resizable = DataGridViewTriState.False;
            VitalPulse.Width = 60;
            // 
            // VitalRespRate
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight;
            VitalRespRate.DefaultCellStyle = dataGridViewCellStyle7;
            VitalRespRate.HeaderText = "Resp. Rate";
            VitalRespRate.Name = "VitalRespRate";
            VitalRespRate.NumberLength = 1;
            VitalRespRate.ReadOnly = true;
            VitalRespRate.Resizable = DataGridViewTriState.False;
            VitalRespRate.Width = 60;
            // 
            // VitalBPressure
            // 
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
            VitalBPressure.DefaultCellStyle = dataGridViewCellStyle8;
            VitalBPressure.HeaderText = "B.Pressure";
            VitalBPressure.Name = "VitalBPressure";
            VitalBPressure.NumberLength = 1;
            VitalBPressure.ReadOnly = true;
            VitalBPressure.Resizable = DataGridViewTriState.False;
            // 
            // VitalBOxiLevel
            // 
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleRight;
            VitalBOxiLevel.DefaultCellStyle = dataGridViewCellStyle9;
            VitalBOxiLevel.HeaderText = "B.Oxigen Level [%]";
            VitalBOxiLevel.Name = "VitalBOxiLevel";
            VitalBOxiLevel.NumberLength = 1;
            VitalBOxiLevel.ReadOnly = true;
            VitalBOxiLevel.Resizable = DataGridViewTriState.False;
            VitalBOxiLevel.Width = 90;
            // 
            // VitalEnteredBy
            // 
            VitalEnteredBy.HeaderText = "Entered By";
            VitalEnteredBy.Name = "VitalEnteredBy";
            VitalEnteredBy.ReadOnly = true;
            VitalEnteredBy.Resizable = DataGridViewTriState.False;
            VitalEnteredBy.SortMode = DataGridViewColumnSortMode.NotSortable;
            VitalEnteredBy.Width = 90;
            // 
            // VitalRemove
            // 
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.NullValue = "X";
            VitalRemove.DefaultCellStyle = dataGridViewCellStyle10;
            VitalRemove.HeaderText = " ";
            VitalRemove.Name = "VitalRemove";
            VitalRemove.ReadOnly = true;
            VitalRemove.Resizable = DataGridViewTriState.False;
            VitalRemove.Width = 25;
            // 
            // VitalId
            // 
            VitalId.HeaderText = "..";
            VitalId.Name = "VitalId";
            VitalId.ReadOnly = true;
            VitalId.SortMode = DataGridViewColumnSortMode.NotSortable;
            VitalId.Visible = false;
            // 
            // PatientVitalHistory
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(GridViewVitalHistory);
            Controls.Add(label17);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "PatientVitalHistory";
            Size = new Size(824, 370);
            ClientSizeChanged += PatientVitalHistory_ClientSizeChanged;
            ((System.ComponentModel.ISupportInitialize)GridViewVitalHistory).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataViewVerticalScroll GridViewVitalHistory;
        private System.Windows.Forms.Label label17;
        private grid.DataGridViewCalendarColumn VitalDateTime;
        private grid.DataGridViewNumberColumn VitalHeight;
        private grid.DataGridViewNumberColumn VitalWeight;
        private grid.DataGridViewNumberColumn VitalBMI;
        private grid.DataGridViewNumberColumn VitalTemperature;
        private grid.DataGridViewNumberColumn VitalPulse;
        private grid.DataGridViewNumberColumn VitalRespRate;
        private grid.DataGridViewNumberColumn VitalBPressure;
        private grid.DataGridViewNumberColumn VitalBOxiLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn VitalEnteredBy;
        private System.Windows.Forms.DataGridViewButtonColumn VitalRemove;
        private System.Windows.Forms.DataGridViewTextBoxColumn VitalId;
    }
}
