namespace fa.views.hms.patient
{
    partial class FormChartDetails
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
            TreeNode treeNode1 = new TreeNode("Patient Details");
            TreeNode treeNode2 = new TreeNode("Insurance Details");
            TreeNode treeNode3 = new TreeNode("Medical History");
            TreeNode treeNode4 = new TreeNode("Diagnosis History");
            TreeNode treeNode5 = new TreeNode("Prescription History");
            TreeNode treeNode6 = new TreeNode("LabTest History");
            TreeNode treeNode7 = new TreeNode("Procedure History");
            TreeNode treeNode8 = new TreeNode("Consultation", new TreeNode[] { treeNode4, treeNode5, treeNode6, treeNode7 });
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChartDetails));
            BtnPrint = new Button();
            BtnCancel = new Button();
            TextBoxPatientId = new TextBox();
            statusStrip1 = new StatusStrip();
            ErrorMsg = new ToolStripStatusLabel();
            TreeViewPrintingPages = new TreeView();
            PictureBoxMedicalHis = new PictureBox();
            PictureBoxMedHisUnChecked = new PictureBox();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxMedicalHis).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PictureBoxMedHisUnChecked).BeginInit();
            SuspendLayout();
            // 
            // BtnPrint
            // 
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(169, 165);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(75, 23);
            BtnPrint.TabIndex = 1;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(79, 165);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(84, 23);
            BtnCancel.TabIndex = 2;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // TextBoxPatientId
            // 
            TextBoxPatientId.Location = new Point(114, 201);
            TextBoxPatientId.Name = "TextBoxPatientId";
            TextBoxPatientId.Size = new Size(100, 21);
            TextBoxPatientId.TabIndex = 510;
            TextBoxPatientId.TabStop = false;
            TextBoxPatientId.Visible = false;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsg });
            statusStrip1.Location = new Point(0, 200);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(260, 22);
            statusStrip1.TabIndex = 511;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            ErrorMsg.BackColor = SystemColors.Control;
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(37, 17);
            ErrorMsg.Text = "          ";
            // 
            // TreeViewPrintingPages
            // 
            TreeViewPrintingPages.CheckBoxes = true;
            TreeViewPrintingPages.Location = new Point(8, 6);
            TreeViewPrintingPages.Name = "TreeViewPrintingPages";
            treeNode1.Name = "Patient Details";
            treeNode1.Text = "Patient Details";
            treeNode2.Name = "Insurance Details";
            treeNode2.Text = "Insurance Details";
            treeNode3.Name = "Medical History";
            treeNode3.Text = "Medical History";
            treeNode4.Name = "Diagnosis History";
            treeNode4.Text = "Diagnosis History";
            treeNode5.Name = "Prescription History";
            treeNode5.Text = "Prescription History";
            treeNode6.Name = "LabTest History";
            treeNode6.Text = "LabTest History";
            treeNode7.Name = "Procedure History";
            treeNode7.Text = "Procedure History";
            treeNode8.Name = "Consultation";
            treeNode8.Text = "Consultation";
            TreeViewPrintingPages.Nodes.AddRange(new TreeNode[] { treeNode1, treeNode2, treeNode3, treeNode8 });
            TreeViewPrintingPages.Size = new Size(242, 153);
            TreeViewPrintingPages.TabIndex = 512;
            TreeViewPrintingPages.AfterCheck += TreeViewPrintingPages_AfterCheck;
            TreeViewPrintingPages.AfterSelect += TreeViewPrintingPages_AfterSelect;
            // 
            // PictureBoxMedicalHis
            // 
            PictureBoxMedicalHis.Image = (Image)resources.GetObject("PictureBoxMedicalHis.Image");
            PictureBoxMedicalHis.Location = new Point(276, 12);
            PictureBoxMedicalHis.Name = "PictureBoxMedicalHis";
            PictureBoxMedicalHis.Size = new Size(72, 73);
            PictureBoxMedicalHis.SizeMode = PictureBoxSizeMode.StretchImage;
            PictureBoxMedicalHis.TabIndex = 513;
            PictureBoxMedicalHis.TabStop = false;
            // 
            // PictureBoxMedHisUnChecked
            // 
            PictureBoxMedHisUnChecked.Image = (Image)resources.GetObject("PictureBoxMedHisUnChecked.Image");
            PictureBoxMedHisUnChecked.Location = new Point(276, 91);
            PictureBoxMedHisUnChecked.Name = "PictureBoxMedHisUnChecked";
            PictureBoxMedHisUnChecked.Size = new Size(72, 73);
            PictureBoxMedHisUnChecked.SizeMode = PictureBoxSizeMode.StretchImage;
            PictureBoxMedHisUnChecked.TabIndex = 514;
            PictureBoxMedHisUnChecked.TabStop = false;
            // 
            // FormChartDetails
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(260, 222);
            Controls.Add(PictureBoxMedHisUnChecked);
            Controls.Add(PictureBoxMedicalHis);
            Controls.Add(TreeViewPrintingPages);
            Controls.Add(TextBoxPatientId);
            Controls.Add(BtnCancel);
            Controls.Add(BtnPrint);
            Controls.Add(statusStrip1);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormChartDetails";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select To Print";
            Load += FormChartDetails_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxMedicalHis).EndInit();
            ((System.ComponentModel.ISupportInitialize)PictureBoxMedHisUnChecked).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button BtnPrint;
        private Button BtnCancel;
        public TextBox TextBoxPatientId;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel ErrorMsg;
        private TreeView TreeViewPrintingPages;
        private PictureBox PictureBoxMedicalHis;
        private PictureBox PictureBoxMedHisUnChecked;
    }
}