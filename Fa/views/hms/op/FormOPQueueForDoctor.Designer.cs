namespace fa.views.hms.op
{
    partial class FormOPQueueForDoctor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOPQueueForDoctor));
            statusStrip1 = new StatusStrip();
            ErrorMsg = new ToolStripStatusLabel();
            toolStrip1 = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            TextBoxOpSearch = new ToolStripTextBox();
            BtnQueueDoctorSearch = new ToolStripButton();
            OpQueueGrid = new controls.hms.OPQueueGrid();
            patientInfoMin1 = new controls.hms.PatientInfoMin();
            BtnConsult = new Button();
            checkBoxMyQueue = new CheckBox();
            statusStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // PatientIdTransport
            // 
            PatientIdTransport.Size = new Size(100, 21);
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsg });
            statusStrip1.Location = new Point(0, 609);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(757, 22);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(49, 17);
            ErrorMsg.Text = "              ";
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = SystemColors.ControlLight;
            toolStrip1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, TextBoxOpSearch, BtnQueueDoctorSearch });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new Padding(5);
            toolStrip1.Size = new Size(757, 33);
            toolStrip1.TabIndex = 2;
            toolStrip1.Text = "toolStrip1";
            toolStrip1.PreviewKeyDown += toolStrip1_PreviewKeyDown;
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(81, 20);
            toolStripLabel1.Text = "  Search Queue";
            // 
            // TextBoxOpSearch
            // 
            TextBoxOpSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxOpSearch.MaxLength = 33;
            TextBoxOpSearch.Name = "TextBoxOpSearch";
            TextBoxOpSearch.Size = new Size(200, 23);
            TextBoxOpSearch.KeyDown += TextBoxOpSearch_KeyDown;
            // 
            // BtnQueueDoctorSearch
            // 
            BtnQueueDoctorSearch.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnQueueDoctorSearch.Image = (Image)resources.GetObject("BtnQueueDoctorSearch.Image");
            BtnQueueDoctorSearch.ImageTransparentColor = Color.Magenta;
            BtnQueueDoctorSearch.Name = "BtnQueueDoctorSearch";
            BtnQueueDoctorSearch.Size = new Size(24, 20);
            BtnQueueDoctorSearch.Text = "Go";
            BtnQueueDoctorSearch.Click += BtnQueueDoctorSearch_Click;
            // 
            // OpQueueGrid
            // 
            OpQueueGrid.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            OpQueueGrid.IsDoctor = true;
            OpQueueGrid.IsMyQueue = true;
            OpQueueGrid.Location = new Point(216, 49);
            OpQueueGrid.Margin = new Padding(4);
            OpQueueGrid.Name = "OpQueueGrid";
            OpQueueGrid.OpId = null;
            OpQueueGrid.OPStatus = model.Hms.Op.Status.ALL;
            OpQueueGrid.SearchString = "";
            OpQueueGrid.SingleClickSelection = true;
            OpQueueGrid.Size = new Size(530, 514);
            OpQueueGrid.TabIndex = 0;
            OpQueueGrid.Load += opQueueGrid1_Load;
            OpQueueGrid.Click += OpQueueGrid_Click;
            OpQueueGrid.KeyDown += OpQueueGrid_KeyDown;
            OpQueueGrid.KeyUp += OpQueueGrid_KeyUp;
            OpQueueGrid.PreviewKeyDown += OpQueueGrid_PreviewKeyDown;
            // 
            // patientInfoMin1
            // 
            patientInfoMin1.AutoSize = true;
            patientInfoMin1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            patientInfoMin1.Location = new Point(7, 44);
            patientInfoMin1.Margin = new Padding(4);
            patientInfoMin1.MaximumSize = new Size(197, 600);
            patientInfoMin1.MinimumSize = new Size(197, 505);
            patientInfoMin1.Name = "patientInfoMin1";
            patientInfoMin1.PatientChild = Global.SelectGender.Transgender;
            patientInfoMin1.PatientGender = Global.SelectGender.Transgender;
            patientInfoMin1.PatientId = null;
            patientInfoMin1.Short = false;
            patientInfoMin1.Size = new Size(197, 522);
            patientInfoMin1.TabIndex = 67;
            // 
            // BtnConsult
            // 
            BtnConsult.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnConsult.Location = new Point(646, 570);
            BtnConsult.Name = "BtnConsult";
            BtnConsult.Size = new Size(102, 23);
            BtnConsult.TabIndex = 68;
            BtnConsult.Text = "Consult [F8]";
            BtnConsult.UseVisualStyleBackColor = true;
            BtnConsult.Click += Consult;
            BtnConsult.KeyDown += BtnConsult_KeyDown;
            BtnConsult.PreviewKeyDown += BtnConsult_PreviewKeyDown_1;
            // 
            // checkBoxMyQueue
            // 
            checkBoxMyQueue.AutoSize = true;
            checkBoxMyQueue.Checked = true;
            checkBoxMyQueue.CheckState = CheckState.Checked;
            checkBoxMyQueue.Location = new Point(216, 574);
            checkBoxMyQueue.Name = "checkBoxMyQueue";
            checkBoxMyQueue.Size = new Size(100, 17);
            checkBoxMyQueue.TabIndex = 70;
            checkBoxMyQueue.Text = "My Queue Only";
            checkBoxMyQueue.UseVisualStyleBackColor = true;
            checkBoxMyQueue.CheckedChanged += checkBoxMyQueue_CheckedChanged;
            checkBoxMyQueue.KeyDown += checkBoxMyQueue_KeyDown;
            checkBoxMyQueue.PreviewKeyDown += checkBoxMyQueue_PreviewKeyDown;
            // 
            // FormOPQueueForDoctor
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(757, 631);
            Controls.Add(checkBoxMyQueue);
            Controls.Add(BtnConsult);
            Controls.Add(patientInfoMin1);
            Controls.Add(OpQueueGrid);
            Controls.Add(toolStrip1);
            Controls.Add(statusStrip1);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormOPQueueForDoctor";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Out Patient Queue - Doctor's View";
            Load += FormOPQueue_Load;
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(toolStrip1, 0);
            Controls.SetChildIndex(OpQueueGrid, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(patientInfoMin1, 0);
            Controls.SetChildIndex(BtnConsult, 0);
            Controls.SetChildIndex(checkBoxMyQueue, 0);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip statusStrip1;
        private ToolStrip toolStrip1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripButton BtnQueueDoctorSearch;
        private ToolStripTextBox TextBoxOpSearch;
        private controls.hms.OPQueueGrid OpQueueGrid;
        private ToolStripStatusLabel ErrorMsg;
        private controls.hms.PatientInfoMin patientInfoMin1;
        private Button BtnConsult;
        private CheckBox checkBox1;
        private CheckBox checkBoxMyQueue;
        private ToolStripLabel toolStripLabel2;
        private controls.ToolStripCheckBox toolStripCheckBox1;
    }
}