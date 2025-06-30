namespace fa.views.hms.op
{
    partial class FormOPQueue
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOPQueue));
            statusStrip1 = new StatusStrip();
            ErrorMsg = new ToolStripStatusLabel();
            toolStrip1 = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            TextBoxOpSearch = new ToolStripTextBox();
            BtnVitalSearch = new ToolStripButton();
            BtnOpQueueVitalEntry = new Button();
            BtnOpQueueLabResult = new Button();
            OpQueueGrid = new controls.hms.OPQueueGrid();
            PatientInfoMiniHorizontal = new controls.hms.PatientInfoMin();
            BtnCompleteVisit = new Button();
            btnPerformProcedures = new Button();
            statusStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // PatientIdTransport
            // 
            PatientIdTransport.Location = new Point(16, 821);
            PatientIdTransport.Size = new Size(100, 21);
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(176, 224);
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(176, 198);
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(176, 172);
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
            ErrorMsg.BackColor = SystemColors.Control;
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(19, 17);
            ErrorMsg.Text = "    ";
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = SystemColors.ControlLight;
            toolStrip1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, TextBoxOpSearch, BtnVitalSearch });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new Padding(5);
            toolStrip1.Size = new Size(757, 33);
            toolStrip1.TabIndex = 1;
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
            // BtnVitalSearch
            // 
            BtnVitalSearch.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnVitalSearch.Image = (Image)resources.GetObject("BtnVitalSearch.Image");
            BtnVitalSearch.ImageTransparentColor = Color.Magenta;
            BtnVitalSearch.Name = "BtnVitalSearch";
            BtnVitalSearch.Size = new Size(24, 20);
            BtnVitalSearch.Text = "Go";
            BtnVitalSearch.Click += BtnOpSearch_Click;
            // 
            // BtnOpQueueVitalEntry
            // 
            BtnOpQueueVitalEntry.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnOpQueueVitalEntry.Location = new Point(378, 572);
            BtnOpQueueVitalEntry.Name = "BtnOpQueueVitalEntry";
            BtnOpQueueVitalEntry.Size = new Size(102, 23);
            BtnOpQueueVitalEntry.TabIndex = 4;
            BtnOpQueueVitalEntry.Text = "Vital Entry [F6]";
            BtnOpQueueVitalEntry.UseVisualStyleBackColor = true;
            BtnOpQueueVitalEntry.Click += BtnOpQueueVitalEntry_Click;
            BtnOpQueueVitalEntry.PreviewKeyDown += BtnOpQueueVitalEntry_PreviewKeyDown;
            // 
            // BtnOpQueueLabResult
            // 
            BtnOpQueueLabResult.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnOpQueueLabResult.Location = new Point(486, 572);
            BtnOpQueueLabResult.Name = "BtnOpQueueLabResult";
            BtnOpQueueLabResult.Size = new Size(135, 23);
            BtnOpQueueLabResult.TabIndex = 5;
            BtnOpQueueLabResult.Text = "Lab Result Entry [F7]";
            BtnOpQueueLabResult.UseVisualStyleBackColor = true;
            BtnOpQueueLabResult.Click += BtnOpQueueLabResult_Click;
            BtnOpQueueLabResult.PreviewKeyDown += BtnOpQueueLabResult_PreviewKeyDown;
            // 
            // OpQueueGrid
            // 
            OpQueueGrid.BackColor = SystemColors.Control;
            OpQueueGrid.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            OpQueueGrid.IsDoctor = false;
            OpQueueGrid.IsMyQueue = true;
            OpQueueGrid.Location = new Point(216, 51);
            OpQueueGrid.Margin = new Padding(4);
            OpQueueGrid.Name = "OpQueueGrid";
            OpQueueGrid.OpId = null;
            OpQueueGrid.OPStatus = model.Hms.Op.Status.ALL;
            OpQueueGrid.SearchString = "";
            OpQueueGrid.SingleClickSelection = true;
            OpQueueGrid.Size = new Size(530, 514);
            OpQueueGrid.TabIndex = 2;
            OpQueueGrid.Load += opQueueGrid1_Load;
            OpQueueGrid.Click += OpQueueGrid_Click;
            OpQueueGrid.KeyDown += OpQueueGrid_KeyDown;
            OpQueueGrid.KeyUp += OpQueueGrid_KeyUp;
            OpQueueGrid.PreviewKeyDown += OpQueueGrid_PreviewKeyDown;
            // 
            // PatientInfoMiniHorizontal
            // 
            PatientInfoMiniHorizontal.AutoSize = true;
            PatientInfoMiniHorizontal.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            PatientInfoMiniHorizontal.Location = new Point(12, 48);
            PatientInfoMiniHorizontal.Margin = new Padding(4);
            PatientInfoMiniHorizontal.MaximumSize = new Size(197, 600);
            PatientInfoMiniHorizontal.MinimumSize = new Size(197, 505);
            PatientInfoMiniHorizontal.Name = "PatientInfoMiniHorizontal";
            PatientInfoMiniHorizontal.PatientChild = Global.SelectGender.Transgender;
            PatientInfoMiniHorizontal.PatientGender = Global.SelectGender.Transgender;
            PatientInfoMiniHorizontal.PatientId = null;
            PatientInfoMiniHorizontal.Short = false;
            PatientInfoMiniHorizontal.Size = new Size(197, 522);
            PatientInfoMiniHorizontal.TabIndex = 67;
            PatientInfoMiniHorizontal.TabStop = false;
            // 
            // BtnCompleteVisit
            // 
            BtnCompleteVisit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCompleteVisit.Location = new Point(627, 572);
            BtnCompleteVisit.Name = "BtnCompleteVisit";
            BtnCompleteVisit.Size = new Size(122, 23);
            BtnCompleteVisit.TabIndex = 6;
            BtnCompleteVisit.Text = "Complete Visit [F8]";
            BtnCompleteVisit.UseVisualStyleBackColor = true;
            BtnCompleteVisit.Click += BtnCompleteVisit_Click;
            BtnCompleteVisit.PreviewKeyDown += BtnCompleteVisit_PreviewKeyDown;
            // 
            // btnPerformProcedures
            // 
            btnPerformProcedures.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            btnPerformProcedures.Location = new Point(214, 572);
            btnPerformProcedures.Name = "btnPerformProcedures";
            btnPerformProcedures.Size = new Size(158, 23);
            btnPerformProcedures.TabIndex = 3;
            btnPerformProcedures.Text = "Perform Procedures [F5]";
            btnPerformProcedures.UseVisualStyleBackColor = true;
            btnPerformProcedures.Click += btnPerformProcedures_Click;
            btnPerformProcedures.PreviewKeyDown += btnPerformProcedures_PreviewKeyDown;
            // 
            // FormOPQueue
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(757, 631);
            Controls.Add(btnPerformProcedures);
            Controls.Add(OpQueueGrid);
            Controls.Add(BtnCompleteVisit);
            Controls.Add(BtnOpQueueLabResult);
            Controls.Add(BtnOpQueueVitalEntry);
            Controls.Add(toolStrip1);
            Controls.Add(statusStrip1);
            Controls.Add(PatientInfoMiniHorizontal);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormOPQueue";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Out Patient Queue for Nurse / Technician ";
            Load += FormOPQueue_Load;
            Controls.SetChildIndex(PatientInfoMiniHorizontal, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(toolStrip1, 0);
            Controls.SetChildIndex(BtnOpQueueVitalEntry, 0);
            Controls.SetChildIndex(BtnOpQueueLabResult, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(BtnCompleteVisit, 0);
            Controls.SetChildIndex(OpQueueGrid, 0);
            Controls.SetChildIndex(btnPerformProcedures, 0);
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
        private ToolStripButton BtnVitalSearch;
        private ToolStripTextBox TextBoxOpSearch;
        private controls.hms.OPQueueGrid OpQueueGrid;
        private Button BtnOpQueueVitalEntry;
        private Button BtnOpQueueLabResult;
        private ToolStripStatusLabel ErrorMsg;
        private controls.hms.PatientInfoMin PatientInfoMiniHorizontal;
        private Button BtnCompleteVisit;
        //private Button button1;
        private Button btnPerformProcedures;
    }
}