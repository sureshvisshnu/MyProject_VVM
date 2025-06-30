namespace fa.views.hms.ip
{
    partial class FormTransferPatient
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTransferPatient));
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label5 = new Label();
            GroupBoxTransferToDetail = new GroupBox();
            ComboxAuthor = new ComboBox();
            label7 = new Label();
            TxtboxTransferResn = new TextBox();
            label6 = new Label();
            ComboxBedTo = new ComboBox();
            ComboxWardTo = new ComboBox();
            GroupBoxTransferFromDetail = new GroupBox();
            TxtboxBedFrom = new TextBox();
            TxtboxWardFrom = new TextBox();
            TabControlTransferIp = new TabControl();
            TabPageTransferDetails = new TabPage();
            BtnTransfer = new Button();
            BtnCancel = new Button();
            labelIpLocationHistory = new Label();
            DataViewLocationHist = new controls.DataViewVerticalScroll();
            From = new DataGridViewTextBoxColumn();
            To = new DataGridViewTextBoxColumn();
            Ward = new DataGridViewTextBoxColumn();
            Bed = new DataGridViewTextBoxColumn();
            TransferReason = new DataGridViewTextBoxColumn();
            ab2ToolStripIpTransfer = new controls.Ab2ToolStrip();
            ToolStripLabelSearch = new ToolStripLabel();
            TextBoxPatientSearch = new ToolStripTextBox();
            ToolStripBtnSearch = new ToolStripButton();
            InPatientInfoMin = new controls.hms.PatientInfoMin();
            StatusStripIpTransfer = new StatusStrip();
            IpTransferErrorMsg = new ToolStripStatusLabel();
            GroupBoxTransferToDetail.SuspendLayout();
            GroupBoxTransferFromDetail.SuspendLayout();
            TabControlTransferIp.SuspendLayout();
            TabPageTransferDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DataViewLocationHist).BeginInit();
            ab2ToolStripIpTransfer.SuspendLayout();
            StatusStripIpTransfer.SuspendLayout();
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
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 27);
            label1.Name = "label1";
            label1.Size = new Size(33, 13);
            label1.TabIndex = 0;
            label1.Text = "Ward";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 67);
            label2.Name = "label2";
            label2.Size = new Size(25, 13);
            label2.TabIndex = 1;
            label2.Text = "Bed";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(6, 20);
            label3.Name = "label3";
            label3.Size = new Size(37, 13);
            label3.TabIndex = 2;
            label3.Text = "Ward";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label5.Location = new Point(6, 62);
            label5.Name = "label5";
            label5.Size = new Size(29, 13);
            label5.TabIndex = 4;
            label5.Text = "Bed";
            // 
            // GroupBoxTransferToDetail
            // 
            GroupBoxTransferToDetail.Controls.Add(ComboxAuthor);
            GroupBoxTransferToDetail.Controls.Add(label7);
            GroupBoxTransferToDetail.Controls.Add(TxtboxTransferResn);
            GroupBoxTransferToDetail.Controls.Add(label6);
            GroupBoxTransferToDetail.Controls.Add(ComboxBedTo);
            GroupBoxTransferToDetail.Controls.Add(ComboxWardTo);
            GroupBoxTransferToDetail.Controls.Add(label3);
            GroupBoxTransferToDetail.Controls.Add(label5);
            GroupBoxTransferToDetail.Location = new Point(6, 133);
            GroupBoxTransferToDetail.Name = "GroupBoxTransferToDetail";
            GroupBoxTransferToDetail.Size = new Size(300, 248);
            GroupBoxTransferToDetail.TabIndex = 6;
            GroupBoxTransferToDetail.TabStop = false;
            GroupBoxTransferToDetail.Text = "To";
            // 
            // ComboxAuthor
            // 
            ComboxAuthor.FormattingEnabled = true;
            ComboxAuthor.Location = new Point(9, 213);
            ComboxAuthor.Name = "ComboxAuthor";
            ComboxAuthor.Size = new Size(282, 21);
            ComboxAuthor.TabIndex = 8;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label7.Location = new Point(6, 196);
            label7.Name = "label7";
            label7.Size = new Size(86, 13);
            label7.TabIndex = 0;
            label7.Text = "Authorized By";
            // 
            // TxtboxTransferResn
            // 
            TxtboxTransferResn.Location = new Point(9, 121);
            TxtboxTransferResn.MaxLength = 500;
            TxtboxTransferResn.Multiline = true;
            TxtboxTransferResn.Name = "TxtboxTransferResn";
            TxtboxTransferResn.Size = new Size(282, 71);
            TxtboxTransferResn.TabIndex = 7;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label6.Location = new Point(6, 104);
            label6.Name = "label6";
            label6.Size = new Size(90, 13);
            label6.TabIndex = 0;
            label6.Text = "Transfer Notes";
            // 
            // ComboxBedTo
            // 
            ComboxBedTo.FormattingEnabled = true;
            ComboxBedTo.Location = new Point(9, 79);
            ComboxBedTo.Name = "ComboxBedTo";
            ComboxBedTo.Size = new Size(282, 21);
            ComboxBedTo.TabIndex = 6;
            // 
            // ComboxWardTo
            // 
            ComboxWardTo.FormattingEnabled = true;
            ComboxWardTo.Location = new Point(9, 37);
            ComboxWardTo.Name = "ComboxWardTo";
            ComboxWardTo.Size = new Size(282, 21);
            ComboxWardTo.TabIndex = 5;
            ComboxWardTo.SelectedIndexChanged += ComboxWardTo_SelectedIndexChanged;
            ComboxWardTo.PreviewKeyDown += ComboxWardTo_PreviewKeyDown;
            // 
            // GroupBoxTransferFromDetail
            // 
            GroupBoxTransferFromDetail.Controls.Add(TxtboxBedFrom);
            GroupBoxTransferFromDetail.Controls.Add(TxtboxWardFrom);
            GroupBoxTransferFromDetail.Controls.Add(label1);
            GroupBoxTransferFromDetail.Controls.Add(label2);
            GroupBoxTransferFromDetail.Location = new Point(6, 6);
            GroupBoxTransferFromDetail.Name = "GroupBoxTransferFromDetail";
            GroupBoxTransferFromDetail.Size = new Size(300, 121);
            GroupBoxTransferFromDetail.TabIndex = 0;
            GroupBoxTransferFromDetail.TabStop = false;
            GroupBoxTransferFromDetail.Text = "From";
            // 
            // TxtboxBedFrom
            // 
            TxtboxBedFrom.BackColor = Color.White;
            TxtboxBedFrom.Location = new Point(9, 83);
            TxtboxBedFrom.Name = "TxtboxBedFrom";
            TxtboxBedFrom.ReadOnly = true;
            TxtboxBedFrom.Size = new Size(282, 21);
            TxtboxBedFrom.TabIndex = 5;
            // 
            // TxtboxWardFrom
            // 
            TxtboxWardFrom.BackColor = Color.White;
            TxtboxWardFrom.Location = new Point(9, 43);
            TxtboxWardFrom.Name = "TxtboxWardFrom";
            TxtboxWardFrom.ReadOnly = true;
            TxtboxWardFrom.Size = new Size(282, 21);
            TxtboxWardFrom.TabIndex = 6;
            // 
            // TabControlTransferIp
            // 
            TabControlTransferIp.Controls.Add(TabPageTransferDetails);
            TabControlTransferIp.Location = new Point(209, 37);
            TabControlTransferIp.Name = "TabControlTransferIp";
            TabControlTransferIp.SelectedIndex = 0;
            TabControlTransferIp.Size = new Size(322, 519);
            TabControlTransferIp.TabIndex = 4;
            // 
            // TabPageTransferDetails
            // 
            TabPageTransferDetails.BackColor = SystemColors.Control;
            TabPageTransferDetails.Controls.Add(BtnTransfer);
            TabPageTransferDetails.Controls.Add(BtnCancel);
            TabPageTransferDetails.Controls.Add(GroupBoxTransferFromDetail);
            TabPageTransferDetails.Controls.Add(GroupBoxTransferToDetail);
            TabPageTransferDetails.Location = new Point(4, 22);
            TabPageTransferDetails.Name = "TabPageTransferDetails";
            TabPageTransferDetails.Padding = new Padding(3);
            TabPageTransferDetails.Size = new Size(314, 493);
            TabPageTransferDetails.TabIndex = 0;
            TabPageTransferDetails.Text = "Transfer";
            // 
            // BtnTransfer
            // 
            BtnTransfer.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnTransfer.Location = new Point(199, 437);
            BtnTransfer.Name = "BtnTransfer";
            BtnTransfer.Size = new Size(98, 23);
            BtnTransfer.TabIndex = 9;
            BtnTransfer.Text = "Transfer [F8]";
            BtnTransfer.UseVisualStyleBackColor = true;
            BtnTransfer.Click += BtnTransfer_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(110, 437);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(83, 23);
            BtnCancel.TabIndex = 10;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // labelIpLocationHistory
            // 
            labelIpLocationHistory.AutoSize = true;
            labelIpLocationHistory.Location = new Point(540, 40);
            labelIpLocationHistory.Name = "labelIpLocationHistory";
            labelIpLocationHistory.Size = new Size(84, 13);
            labelIpLocationHistory.TabIndex = 7;
            labelIpLocationHistory.Text = "Location History";
            // 
            // DataViewLocationHist
            // 
            DataViewLocationHist.AllowUserToAddRows = false;
            DataViewLocationHist.AllowUserToDeleteRows = false;
            DataViewLocationHist.AllowUserToResizeColumns = false;
            DataViewLocationHist.AllowUserToResizeRows = false;
            DataViewLocationHist.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            DataViewLocationHist.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            DataViewLocationHist.ColumnHeadersHeight = 20;
            DataViewLocationHist.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DataViewLocationHist.Columns.AddRange(new DataGridViewColumn[] { From, To, Ward, Bed, TransferReason });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.Window;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            DataViewLocationHist.DefaultCellStyle = dataGridViewCellStyle3;
            DataViewLocationHist.EditMode = DataGridViewEditMode.EditOnEnter;
            DataViewLocationHist.EnableHeadersVisualStyles = false;
            DataViewLocationHist.Location = new Point(540, 61);
            DataViewLocationHist.MultiSelect = false;
            DataViewLocationHist.Name = "DataViewLocationHist";
            DataViewLocationHist.ReadOnly = true;
            DataViewLocationHist.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.Window;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            DataViewLocationHist.RowsDefaultCellStyle = dataGridViewCellStyle4;
            DataViewLocationHist.RowTemplate.Height = 20;
            DataViewLocationHist.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DataViewLocationHist.ShowCellToolTips = false;
            DataViewLocationHist.Size = new Size(607, 491);
            DataViewLocationHist.TabIndex = 6;
            // 
            // From
            // 
            From.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            From.HeaderText = "From";
            From.Name = "From";
            From.ReadOnly = true;
            From.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // To
            // 
            To.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            To.HeaderText = "To";
            To.Name = "To";
            To.ReadOnly = true;
            To.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Ward
            // 
            Ward.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Ward.HeaderText = "Ward";
            Ward.Name = "Ward";
            Ward.ReadOnly = true;
            Ward.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Bed
            // 
            Bed.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Bed.HeaderText = "Bed";
            Bed.Name = "Bed";
            Bed.ReadOnly = true;
            Bed.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // TransferReason
            // 
            TransferReason.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            TransferReason.DefaultCellStyle = dataGridViewCellStyle2;
            TransferReason.HeaderText = "Notes";
            TransferReason.Name = "TransferReason";
            TransferReason.ReadOnly = true;
            TransferReason.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // ab2ToolStripIpTransfer
            // 
            ab2ToolStripIpTransfer.BackColor = SystemColors.ControlLight;
            ab2ToolStripIpTransfer.GripStyle = ToolStripGripStyle.Hidden;
            ab2ToolStripIpTransfer.Items.AddRange(new ToolStripItem[] { ToolStripLabelSearch, TextBoxPatientSearch, ToolStripBtnSearch });
            ab2ToolStripIpTransfer.Location = new Point(0, 0);
            ab2ToolStripIpTransfer.Name = "ab2ToolStripIpTransfer";
            ab2ToolStripIpTransfer.Padding = new Padding(5);
            ab2ToolStripIpTransfer.Size = new Size(1158, 33);
            ab2ToolStripIpTransfer.TabIndex = 5;
            ab2ToolStripIpTransfer.Text = "ab2ToolStrip1";
            // 
            // ToolStripLabelSearch
            // 
            ToolStripLabelSearch.Name = "ToolStripLabelSearch";
            ToolStripLabelSearch.Size = new Size(82, 20);
            ToolStripLabelSearch.Text = "Patient Search";
            // 
            // TextBoxPatientSearch
            // 
            TextBoxPatientSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxPatientSearch.Name = "TextBoxPatientSearch";
            TextBoxPatientSearch.Size = new Size(125, 23);
            TextBoxPatientSearch.KeyDown += TextBoxPatientSearch_KeyDown;
            // 
            // ToolStripBtnSearch
            // 
            ToolStripBtnSearch.BackColor = SystemColors.ControlLight;
            ToolStripBtnSearch.DisplayStyle = ToolStripItemDisplayStyle.Text;
            ToolStripBtnSearch.Image = (Image)resources.GetObject("ToolStripBtnSearch.Image");
            ToolStripBtnSearch.ImageTransparentColor = Color.Magenta;
            ToolStripBtnSearch.Name = "ToolStripBtnSearch";
            ToolStripBtnSearch.Size = new Size(26, 20);
            ToolStripBtnSearch.Text = "Go";
            ToolStripBtnSearch.Click += ToolStripBtnSearch_Click_1;
            // 
            // InPatientInfoMin
            // 
            InPatientInfoMin.AutoSize = true;
            InPatientInfoMin.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            InPatientInfoMin.Location = new Point(2, 34);
            InPatientInfoMin.Margin = new Padding(4);
            InPatientInfoMin.MaximumSize = new Size(197, 600);
            InPatientInfoMin.MinimumSize = new Size(197, 505);
            InPatientInfoMin.Name = "InPatientInfoMin";
            InPatientInfoMin.PatientId = null;
            InPatientInfoMin.Short = false;
            InPatientInfoMin.Size = new Size(197, 522);
            InPatientInfoMin.TabIndex = 3;
            // 
            // StatusStripIpTransfer
            // 
            StatusStripIpTransfer.ImageScalingSize = new Size(24, 24);
            StatusStripIpTransfer.Items.AddRange(new ToolStripItem[] { IpTransferErrorMsg });
            StatusStripIpTransfer.Location = new Point(0, 568);
            StatusStripIpTransfer.Name = "StatusStripIpTransfer";
            StatusStripIpTransfer.Size = new Size(1158, 22);
            StatusStripIpTransfer.TabIndex = 67;
            StatusStripIpTransfer.Text = "statusStrip1";
            // 
            // IpTransferErrorMsg
            // 
            IpTransferErrorMsg.Name = "IpTransferErrorMsg";
            IpTransferErrorMsg.Size = new Size(25, 17);
            IpTransferErrorMsg.Text = "      ";
            // 
            // FormTransferPatient
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1158, 590);
            Controls.Add(StatusStripIpTransfer);
            Controls.Add(labelIpLocationHistory);
            Controls.Add(DataViewLocationHist);
            Controls.Add(ab2ToolStripIpTransfer);
            Controls.Add(TabControlTransferIp);
            Controls.Add(InPatientInfoMin);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormTransferPatient";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Transfer/Move Patient";
            Load += FormTransferPatient_Load;
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(InPatientInfoMin, 0);
            Controls.SetChildIndex(TabControlTransferIp, 0);
            Controls.SetChildIndex(ab2ToolStripIpTransfer, 0);
            Controls.SetChildIndex(DataViewLocationHist, 0);
            Controls.SetChildIndex(labelIpLocationHistory, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(StatusStripIpTransfer, 0);
            GroupBoxTransferToDetail.ResumeLayout(false);
            GroupBoxTransferToDetail.PerformLayout();
            GroupBoxTransferFromDetail.ResumeLayout(false);
            GroupBoxTransferFromDetail.PerformLayout();
            TabControlTransferIp.ResumeLayout(false);
            TabPageTransferDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)DataViewLocationHist).EndInit();
            ab2ToolStripIpTransfer.ResumeLayout(false);
            ab2ToolStripIpTransfer.PerformLayout();
            StatusStripIpTransfer.ResumeLayout(false);
            StatusStripIpTransfer.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private controls.hms.PatientInfoMin InPatientInfoMin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox GroupBoxTransferToDetail;
        private System.Windows.Forms.GroupBox GroupBoxTransferFromDetail;
        private System.Windows.Forms.TabControl TabControlTransferIp;
        private System.Windows.Forms.TabPage TabPageTransferDetails;
        private System.Windows.Forms.Button BtnTransfer;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.TextBox TxtboxBedFrom;
        private System.Windows.Forms.TextBox TxtboxWardFrom;
        private System.Windows.Forms.ComboBox ComboxBedTo;
        private System.Windows.Forms.ComboBox ComboxWardTo;
        private controls.Ab2ToolStrip ab2ToolStripIpTransfer;
        private System.Windows.Forms.ToolStripLabel ToolStripLabelSearch;
        private System.Windows.Forms.ToolStripButton ToolStripBtnSearch;
        private controls.DataViewVerticalScroll DataViewLocationHist;
        private System.Windows.Forms.Label labelIpLocationHistory;
        private System.Windows.Forms.TextBox TxtboxTransferResn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.StatusStrip StatusStripIpTransfer;
        private System.Windows.Forms.ToolStripStatusLabel IpTransferErrorMsg;
        private System.Windows.Forms.ComboBox ComboxAuthor;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ToolStripTextBox TextBoxPatientSearch;
        private DataGridViewTextBoxColumn From;
        private DataGridViewTextBoxColumn To;
        private DataGridViewTextBoxColumn Ward;
        private DataGridViewTextBoxColumn Bed;
        private DataGridViewTextBoxColumn TransferReason;
    }
}