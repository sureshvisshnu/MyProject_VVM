namespace fa.views.hms.ip
{
    partial class FormIPQueue
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormIPQueue));
            toolStrip1 = new ToolStrip();
            toolStripLabelStatus = new ToolStripLabel();
            ComboBoxStatus = new ToolStripComboBox();
            StatusSeparator = new ToolStripSeparator();
            LabelWard = new ToolStripLabel();
            ComboBoxFilter = new ToolStripComboBox();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripLabel1 = new ToolStripLabel();
            PatientSearchBox = new controls.ToolstripDelayedTextBox();
            toolStripGoButton = new ToolStripButton();
            statusStrip1 = new StatusStrip();
            ErrorMsg = new ToolStripStatusLabel();
            contextMenuStrip1 = new ContextMenuStrip(components);
            BtnInpatientVisit = new Button();
            IpQueueGrid = new controls.hms.IPQueueGrid();
            BtnPatientWardTransfer = new Button();
            BtnReAssignCareTaker = new Button();
            patientInfoMin1 = new controls.hms.PatientInfoMin();
            BtnDischargePatient = new Button();
            buttonEditDisCharge = new Button();
            toolStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // PatientIdTransport
            // 
            PatientIdTransport.Size = new Size(100, 21);
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(235, 556);
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(127, 551);
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(21, 556);
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = SystemColors.ControlLight;
            toolStrip1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabelStatus, ComboBoxStatus, StatusSeparator, LabelWard, ComboBoxFilter, toolStripSeparator1, toolStripLabel1, PatientSearchBox, toolStripGoButton });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new Padding(5);
            toolStrip1.Size = new Size(1207, 34);
            toolStrip1.TabIndex = 2;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabelStatus
            // 
            toolStripLabelStatus.Name = "toolStripLabelStatus";
            toolStripLabelStatus.Size = new Size(38, 21);
            toolStripLabelStatus.Text = "Status";
            // 
            // ComboBoxStatus
            // 
            ComboBoxStatus.AutoCompleteCustomSource.AddRange(new string[] { "My Patients", "All" });
            ComboBoxStatus.FlatStyle = FlatStyle.Standard;
            ComboBoxStatus.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ComboBoxStatus.Items.AddRange(new object[] { "All", "Open", "Complete" });
            ComboBoxStatus.Name = "ComboBoxStatus";
            ComboBoxStatus.Size = new Size(121, 24);
            ComboBoxStatus.SelectedIndexChanged += ComboBoxStatus_SelectedIndexChanged;
            // 
            // StatusSeparator
            // 
            StatusSeparator.Name = "StatusSeparator";
            StatusSeparator.Size = new Size(6, 24);
            // 
            // LabelWard
            // 
            LabelWard.Name = "LabelWard";
            LabelWard.Size = new Size(33, 21);
            LabelWard.Text = "Ward";
            // 
            // ComboBoxFilter
            // 
            ComboBoxFilter.AutoCompleteCustomSource.AddRange(new string[] { "My Patients", "All" });
            ComboBoxFilter.FlatStyle = FlatStyle.Standard;
            ComboBoxFilter.Name = "ComboBoxFilter";
            ComboBoxFilter.Size = new Size(121, 24);
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 24);
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(40, 21);
            toolStripLabel1.Text = "Search";
            // 
            // PatientSearchBox
            // 
            PatientSearchBox.AutoSize = false;
            PatientSearchBox.Delay = true;
            PatientSearchBox.DelayTime = 1000;
            PatientSearchBox.Name = "PatientSearchBox";
            PatientSearchBox.Size = new Size(150, 21);
            // 
            // toolStripGoButton
            // 
            toolStripGoButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripGoButton.Image = (Image)resources.GetObject("toolStripGoButton.Image");
            toolStripGoButton.ImageTransparentColor = Color.Magenta;
            toolStripGoButton.Name = "toolStripGoButton";
            toolStripGoButton.Size = new Size(24, 21);
            toolStripGoButton.Text = "Go";
            toolStripGoButton.Click += BtnQueueSearch_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsg });
            statusStrip1.Location = new Point(0, 623);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1207, 22);
            statusStrip1.TabIndex = 3;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            ErrorMsg.BackColor = SystemColors.MenuBar;
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(49, 17);
            ErrorMsg.Text = "              ";
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // BtnInpatientVisit
            // 
            BtnInpatientVisit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnInpatientVisit.ImageAlign = ContentAlignment.MiddleLeft;
            BtnInpatientVisit.Location = new Point(997, 585);
            BtnInpatientVisit.Name = "BtnInpatientVisit";
            BtnInpatientVisit.Size = new Size(119, 23);
            BtnInpatientVisit.TabIndex = 6;
            BtnInpatientVisit.Text = "In Room Visit [F8]";
            BtnInpatientVisit.UseVisualStyleBackColor = true;
            BtnInpatientVisit.Click += ButtonInPatientVisit_Click;
            // 
            // IpQueueGrid
            // 
            IpQueueGrid.BackColor = SystemColors.Control;
            IpQueueGrid.FillterBy = controls.hms.IpQueueStatus.ALL;
            IpQueueGrid.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            IpQueueGrid.IpId = null;
            IpQueueGrid.IsDoctor = true;
            IpQueueGrid.Location = new Point(3, 36);
            IpQueueGrid.Margin = new Padding(4);
            IpQueueGrid.Name = "IpQueueGrid";
            IpQueueGrid.SearchString = "";
            IpQueueGrid.SingleClickSelection = true;
            IpQueueGrid.Size = new Size(993, 536);
            IpQueueGrid.StatusIndex = 0;
            IpQueueGrid.TabIndex = 5;
            IpQueueGrid.WardId = 0L;
            IpQueueGrid.Click += IpQueueGrid_Click;
            IpQueueGrid.KeyDown += IpQueueGrid_KeyDown;
            IpQueueGrid.KeyPress += IpQueueGrid_KeyPress;
            IpQueueGrid.KeyUp += IpQueueGrid_KeyUp;
            IpQueueGrid.PreviewKeyDown += IpQueueGrid_PreviewKeyDown;
            // 
            // BtnPatientWardTransfer
            // 
            BtnPatientWardTransfer.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPatientWardTransfer.ImageAlign = ContentAlignment.MiddleLeft;
            BtnPatientWardTransfer.Location = new Point(851, 585);
            BtnPatientWardTransfer.Name = "BtnPatientWardTransfer";
            BtnPatientWardTransfer.Size = new Size(140, 23);
            BtnPatientWardTransfer.TabIndex = 7;
            BtnPatientWardTransfer.Text = "Transfer Patient [F7]";
            BtnPatientWardTransfer.UseVisualStyleBackColor = true;
            BtnPatientWardTransfer.Click += BtnPatientWardTransfer_Click;
            // 
            // BtnReAssignCareTaker
            // 
            BtnReAssignCareTaker.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnReAssignCareTaker.ImageAlign = ContentAlignment.MiddleLeft;
            BtnReAssignCareTaker.Location = new Point(681, 585);
            BtnReAssignCareTaker.Name = "BtnReAssignCareTaker";
            BtnReAssignCareTaker.Size = new Size(164, 23);
            BtnReAssignCareTaker.TabIndex = 8;
            BtnReAssignCareTaker.Text = "Re-assign Care Taker [F6]";
            BtnReAssignCareTaker.UseVisualStyleBackColor = true;
            BtnReAssignCareTaker.Click += BtnReassign_Click;
            // 
            // patientInfoMin1
            // 
            patientInfoMin1.AutoSize = true;
            patientInfoMin1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            patientInfoMin1.Location = new Point(997, 37);
            patientInfoMin1.Margin = new Padding(4);
            patientInfoMin1.MaximumSize = new Size(197, 530);
            patientInfoMin1.MinimumSize = new Size(197, 530);
            patientInfoMin1.Name = "patientInfoMin1";
            patientInfoMin1.PatientChild = Global.SelectGender.Transgender;
            patientInfoMin1.PatientGender = Global.SelectGender.Transgender;
            patientInfoMin1.PatientId = null;
            patientInfoMin1.Short = false;
            patientInfoMin1.Size = new Size(197, 530);
            patientInfoMin1.TabIndex = 70;
            // 
            // BtnDischargePatient
            // 
            BtnDischargePatient.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnDischargePatient.ImageAlign = ContentAlignment.MiddleLeft;
            BtnDischargePatient.Location = new Point(535, 585);
            BtnDischargePatient.Name = "BtnDischargePatient";
            BtnDischargePatient.Size = new Size(140, 23);
            BtnDischargePatient.TabIndex = 9;
            BtnDischargePatient.Text = "Discharge Patient";
            BtnDischargePatient.UseVisualStyleBackColor = true;
            BtnDischargePatient.Click += BtnDischargePatient_Click;
            BtnDischargePatient.PreviewKeyDown += BtnDischargePatient_PreviewKeyDown;
            // 
            // buttonEditDisCharge
            // 
            buttonEditDisCharge.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonEditDisCharge.Location = new Point(418, 585);
            buttonEditDisCharge.Name = "buttonEditDisCharge";
            buttonEditDisCharge.Size = new Size(109, 23);
            buttonEditDisCharge.TabIndex = 71;
            buttonEditDisCharge.Text = "Edit - Discharge";
            buttonEditDisCharge.UseVisualStyleBackColor = true;
            buttonEditDisCharge.Click += buttonDisChargePrint_Click;
            // 
            // FormIPQueue
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1207, 645);
            Controls.Add(buttonEditDisCharge);
            Controls.Add(BtnDischargePatient);
            Controls.Add(patientInfoMin1);
            Controls.Add(BtnReAssignCareTaker);
            Controls.Add(BtnPatientWardTransfer);
            Controls.Add(BtnInpatientVisit);
            Controls.Add(IpQueueGrid);
            Controls.Add(statusStrip1);
            Controls.Add(toolStrip1);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormIPQueue";
            StartPosition = FormStartPosition.CenterParent;
            Text = "In Patient Queue";
            Load += FormIpQueue_Load;
            Controls.SetChildIndex(toolStrip1, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(IpQueueGrid, 0);
            Controls.SetChildIndex(BtnInpatientVisit, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(BtnPatientWardTransfer, 0);
            Controls.SetChildIndex(BtnReAssignCareTaker, 0);
            Controls.SetChildIndex(patientInfoMin1, 0);
            Controls.SetChildIndex(BtnDischargePatient, 0);
            Controls.SetChildIndex(buttonEditDisCharge, 0);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ErrorMsg;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel LabelWard;
        private System.Windows.Forms.ToolStripComboBox ComboBoxFilter;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private controls.hms.IPQueueGrid IpQueueGrid;
        private System.Windows.Forms.Button BtnInpatientVisit;
        private controls.ToolstripDelayedTextBox PatientSearchBox;
        private System.Windows.Forms.ToolStripButton toolStripGoButton;
        private System.Windows.Forms.Button BtnPatientWardTransfer;
        private System.Windows.Forms.Button BtnReAssignCareTaker;
        private controls.hms.PatientInfoMin patientInfoMin1;
        private System.Windows.Forms.Button BtnDischargePatient;
        private ToolStripLabel toolStripLabelStatus;
        private ToolStripComboBox ComboBoxStatus;
        private ToolStripSeparator StatusSeparator;
        private Button buttonEditDisCharge;
    }
}