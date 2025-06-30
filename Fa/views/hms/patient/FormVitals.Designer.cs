namespace fa.views.hms.patient
{
    partial class FormVitals
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormVitals));
            BtnVitalReset = new Button();
            BtnVitalSave = new Button();
            VitalVitalEntry = new controls.hms.PatientVitalEntry();
            VitalsPatientInfoMin = new controls.hms.PatientInfoMin();
            GraphChartControlVital = new controls.graphic.GraphChartControl();
            groupBox2 = new GroupBox();
            PatientVitalHistory = new controls.hms.PatientVitalHistory();
            statusStripVitals = new StatusStrip();
            toolStripVitalsErrMsg = new ToolStripStatusLabel();
            groupBox2.SuspendLayout();
            statusStripVitals.SuspendLayout();
            SuspendLayout();
            // 
            // BtnVitalReset
            // 
            BtnVitalReset.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            BtnVitalReset.Location = new Point(230, 553);
            BtnVitalReset.Name = "BtnVitalReset";
            BtnVitalReset.Size = new Size(79, 23);
            BtnVitalReset.TabIndex = 5;
            BtnVitalReset.Text = "Reset [Esc]";
            BtnVitalReset.UseVisualStyleBackColor = true;
            BtnVitalReset.Click += BtnVitalReset_Click;
            // 
            // BtnVitalSave
            // 
            BtnVitalSave.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            BtnVitalSave.Location = new Point(315, 553);
            BtnVitalSave.Name = "BtnVitalSave";
            BtnVitalSave.Size = new Size(75, 23);
            BtnVitalSave.TabIndex = 4;
            BtnVitalSave.Text = "Save [F8]";
            BtnVitalSave.UseVisualStyleBackColor = true;
            BtnVitalSave.Click += BtnVitalSave_Click;
            // 
            // VitalVitalEntry
            // 
            VitalVitalEntry.BackColor = SystemColors.Window;
            VitalVitalEntry.FocusIndex = 0;
            VitalVitalEntry.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            VitalVitalEntry.Location = new Point(199, 12);
            VitalVitalEntry.Margin = new Padding(4, 3, 4, 3);
            VitalVitalEntry.Name = "VitalVitalEntry";
            VitalVitalEntry.PatientId = null;
            VitalVitalEntry.Size = new Size(191, 531);
            VitalVitalEntry.TabIndex = 3;
            VitalVitalEntry.VitalId = 0L;
            // 
            // VitalsPatientInfoMin
            // 
            VitalsPatientInfoMin.AutoSize = true;
            VitalsPatientInfoMin.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            VitalsPatientInfoMin.Location = new Point(4, 2);
            VitalsPatientInfoMin.Margin = new Padding(4, 3, 4, 3);
            VitalsPatientInfoMin.Name = "VitalsPatientInfoMin";
            VitalsPatientInfoMin.PatientChild = Global.SelectGender.Transgender;
            VitalsPatientInfoMin.PatientGender = Global.SelectGender.Transgender;
            VitalsPatientInfoMin.PatientId = null;
            VitalsPatientInfoMin.Short = false;
            VitalsPatientInfoMin.Size = new Size(197, 587);
            VitalsPatientInfoMin.TabIndex = 0;
            // 
            // GraphChartControlVital
            // 
            GraphChartControlVital.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            GraphChartControlVital.Location = new Point(403, 325);
            GraphChartControlVital.Name = "GraphChartControlVital";
            GraphChartControlVital.PatientId = null;
            GraphChartControlVital.Size = new Size(807, 251);
            GraphChartControlVital.TabIndex = 6;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(PatientVitalHistory);
            groupBox2.Location = new Point(397, 8);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(821, 311);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Trend";
            // 
            // PatientVitalHistory
            // 
            PatientVitalHistory.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            PatientVitalHistory.Location = new Point(6, 19);
            PatientVitalHistory.Margin = new Padding(4, 3, 4, 3);
            PatientVitalHistory.Name = "PatientVitalHistory";
            PatientVitalHistory.PatientId = null;
            PatientVitalHistory.Size = new Size(807, 298);
            PatientVitalHistory.TabIndex = 5;
            PatientVitalHistory.VitalsId = 0L;
            PatientVitalHistory.Click += PatientVitalHistory_Click;
            PatientVitalHistory.Enter += PatientVitalHistory_Enter;
            // 
            // statusStripVitals
            // 
            statusStripVitals.Items.AddRange(new ToolStripItem[] { toolStripVitalsErrMsg });
            statusStripVitals.Location = new Point(0, 599);
            statusStripVitals.Name = "statusStripVitals";
            statusStripVitals.Size = new Size(1223, 22);
            statusStripVitals.TabIndex = 7;
            statusStripVitals.Text = "statusStripVitals";
            // 
            // toolStripVitalsErrMsg
            // 
            toolStripVitalsErrMsg.Name = "toolStripVitalsErrMsg";
            toolStripVitalsErrMsg.Size = new Size(118, 17);
            toolStripVitalsErrMsg.Text = "toolStripStatusLabel1";
            // 
            // FormVitals
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1223, 621);
            Controls.Add(statusStripVitals);
            Controls.Add(GraphChartControlVital);
            Controls.Add(BtnVitalReset);
            Controls.Add(BtnVitalSave);
            Controls.Add(VitalVitalEntry);
            Controls.Add(VitalsPatientInfoMin);
            Controls.Add(groupBox2);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormVitals";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Vitals";
            Load += FormVitals_Load;
            groupBox2.ResumeLayout(false);
            statusStripVitals.ResumeLayout(false);
            statusStripVitals.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private controls.hms.PatientInfoMin VitalsPatientInfoMin;
        private controls.hms.PatientVitalEntry VitalVitalEntry;
        private System.Windows.Forms.Button BtnVitalReset;
        private System.Windows.Forms.Button BtnVitalSave;
        private controls.graphic.GraphChartControl GraphChartControlVital;
        private GroupBox groupBox2;
        private controls.hms.PatientVitalHistory PatientVitalHistory;
        private StatusStrip statusStripVitals;
        private ToolStripStatusLabel toolStripVitalsErrMsg;
    }
}