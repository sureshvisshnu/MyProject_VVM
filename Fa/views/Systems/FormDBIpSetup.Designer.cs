namespace Fa.views.Systems
{
    partial class FormDBIpSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDBIpSetup));
            TextBoxIpAddress = new fa.views.controls.text.NameTextBoxAllowSpace(components);
            panel1 = new Panel();
            label1 = new Label();
            label6 = new Label();
            BerklySoftstatusStrip = new StatusStrip();
            DBIPSetupErrorMsg = new ToolStripStatusLabel();
            BtnIpIdSetupExit = new Button();
            BtnIpIdSetupCancel = new Button();
            BtnIpIdSetupSave = new Button();
            panel1.SuspendLayout();
            BerklySoftstatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // TextBoxIpAddress
            // 
            TextBoxIpAddress.Location = new Point(10, 31);
            TextBoxIpAddress.MaxLength = 150;
            TextBoxIpAddress.Name = "TextBoxIpAddress";
            TextBoxIpAddress.Size = new Size(173, 23);
            TextBoxIpAddress.TabIndex = 5;
            TextBoxIpAddress.KeyPress += TextBoxIpAddress_KeyPress;
            // 
            // panel1
            // 
            panel1.Controls.Add(label1);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(TextBoxIpAddress);
            panel1.Location = new Point(5, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(356, 62);
            panel1.TabIndex = 22;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.DarkGray;
            label1.Location = new Point(189, 34);
            label1.Name = "label1";
            label1.Size = new Size(155, 15);
            label1.TabIndex = 7;
            label1.Text = "Example DESKTOP-XXXXX";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(10, 9);
            label6.Name = "label6";
            label6.Size = new Size(117, 15);
            label6.TabIndex = 6;
            label6.Text = "Server/System Name";
            // 
            // BerklySoftstatusStrip
            // 
            BerklySoftstatusStrip.BackColor = SystemColors.Control;
            BerklySoftstatusStrip.Items.AddRange(new ToolStripItem[] { DBIPSetupErrorMsg });
            BerklySoftstatusStrip.Location = new Point(0, 131);
            BerklySoftstatusStrip.Name = "BerklySoftstatusStrip";
            BerklySoftstatusStrip.Size = new Size(366, 22);
            BerklySoftstatusStrip.TabIndex = 23;
            BerklySoftstatusStrip.Text = "BerklySoft StatusStrip";
            // 
            // DBIPSetupErrorMsg
            // 
            DBIPSetupErrorMsg.Name = "DBIPSetupErrorMsg";
            DBIPSetupErrorMsg.Size = new Size(34, 17);
            DBIPSetupErrorMsg.Text = "         ";
            // 
            // BtnIpIdSetupExit
            // 
            BtnIpIdSetupExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnIpIdSetupExit.Location = new Point(275, 89);
            BtnIpIdSetupExit.Name = "BtnIpIdSetupExit";
            BtnIpIdSetupExit.Size = new Size(75, 23);
            BtnIpIdSetupExit.TabIndex = 26;
            BtnIpIdSetupExit.Text = "Exit [F10]";
            BtnIpIdSetupExit.UseVisualStyleBackColor = true;
            BtnIpIdSetupExit.Click += BtnIpIdSetupExit_Click;
            // 
            // BtnIpIdSetupCancel
            // 
            BtnIpIdSetupCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnIpIdSetupCancel.Location = new Point(97, 89);
            BtnIpIdSetupCancel.Name = "BtnIpIdSetupCancel";
            BtnIpIdSetupCancel.Size = new Size(83, 23);
            BtnIpIdSetupCancel.TabIndex = 25;
            BtnIpIdSetupCancel.Text = "Cancel [Esc]";
            BtnIpIdSetupCancel.UseVisualStyleBackColor = true;
            BtnIpIdSetupCancel.Click += BtnWorkStationCancel_Click;
            // 
            // BtnIpIdSetupSave
            // 
            BtnIpIdSetupSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnIpIdSetupSave.Location = new Point(186, 89);
            BtnIpIdSetupSave.Name = "BtnIpIdSetupSave";
            BtnIpIdSetupSave.Size = new Size(83, 23);
            BtnIpIdSetupSave.TabIndex = 24;
            BtnIpIdSetupSave.Text = "Save [F8]";
            BtnIpIdSetupSave.UseVisualStyleBackColor = true;
            BtnIpIdSetupSave.Click += BtnWorkStationSave_Click;
            // 
            // FormDBIpSetup
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(366, 153);
            Controls.Add(BtnIpIdSetupExit);
            Controls.Add(BtnIpIdSetupCancel);
            Controls.Add(BtnIpIdSetupSave);
            Controls.Add(BerklySoftstatusStrip);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormDBIpSetup";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Host Name Setting For Client Machine";
            Load += FormDBIpSetup_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            BerklySoftstatusStrip.ResumeLayout(false);
            BerklySoftstatusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private fa.views.controls.text.NameTextBoxAllowSpace TextBoxIpAddress;
        private Panel panel1;
        private Label label6;
        private StatusStrip BerklySoftstatusStrip;
        private ToolStripStatusLabel DBIPSetupErrorMsg;
        private Button BtnIpIdSetupExit;
        private Button BtnIpIdSetupCancel;
        private Button BtnIpIdSetupSave;
        private Label label1;
    }
}