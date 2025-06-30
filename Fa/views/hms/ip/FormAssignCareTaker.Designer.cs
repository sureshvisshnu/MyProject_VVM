namespace fa.views.hms.ip
{
    partial class FormAssignCareTaker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAssignCareTaker));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            ab2ToolStripIpTransfer = new controls.Ab2ToolStrip();
            ToolStripLabelSearch = new ToolStripLabel();
            TextBoxPatientSearch = new ToolStripTextBox();
            BtnPatientSearch = new ToolStripButton();
            InPatientInfoMin = new controls.hms.PatientInfoMin();
            TabControlReAssignCareTaker = new TabControl();
            TabPageTransferDetails = new TabPage();
            BtnReAssign = new Button();
            BtnCancel = new Button();
            GroupBoxCareTakersFromDetail = new GroupBox();
            TextBoxSecondaryNurse = new TextBox();
            TextBoxPrimaryNurse = new TextBox();
            label4 = new Label();
            label8 = new Label();
            TextBoxSecondaryDoctor = new TextBox();
            TextBoxPrimaryDoctor = new TextBox();
            label1 = new Label();
            label2 = new Label();
            GroupBoxNewCareTakerDetail = new GroupBox();
            ComboBoxNewSecondaryDoctor = new ComboBox();
            ComboBoxNewSecondaryNurse = new ComboBox();
            ComboBoxNewPrimaryNurse = new ComboBox();
            ComboBoxNewPrimaryDoctor = new ComboBox();
            label3 = new Label();
            label5 = new Label();
            label9 = new Label();
            label10 = new Label();
            ComboBoxAuthorizedBy = new ComboBox();
            label7 = new Label();
            TextBoxNote = new TextBox();
            label6 = new Label();
            StatusStripIpTransfer = new StatusStrip();
            CareTakerErrorMsg = new ToolStripStatusLabel();
            labelIpLocationHistory = new Label();
            GridViewCareTakerHistory = new controls.DataViewVerticalScroll();
            From = new DataGridViewTextBoxColumn();
            To = new DataGridViewTextBoxColumn();
            PromaryDoc = new DataGridViewTextBoxColumn();
            Bed = new DataGridViewTextBoxColumn();
            PrimaryNurse = new DataGridViewTextBoxColumn();
            SecondaryNurse = new DataGridViewTextBoxColumn();
            TransferReason = new DataGridViewTextBoxColumn();
            AuthorizedBy = new DataGridViewTextBoxColumn();
            ab2ToolStripIpTransfer.SuspendLayout();
            TabControlReAssignCareTaker.SuspendLayout();
            TabPageTransferDetails.SuspendLayout();
            GroupBoxCareTakersFromDetail.SuspendLayout();
            GroupBoxNewCareTakerDetail.SuspendLayout();
            StatusStripIpTransfer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewCareTakerHistory).BeginInit();
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
            // ab2ToolStripIpTransfer
            // 
            ab2ToolStripIpTransfer.BackColor = SystemColors.ControlLight;
            ab2ToolStripIpTransfer.GripStyle = ToolStripGripStyle.Hidden;
            ab2ToolStripIpTransfer.Items.AddRange(new ToolStripItem[] { ToolStripLabelSearch, TextBoxPatientSearch, BtnPatientSearch });
            ab2ToolStripIpTransfer.Location = new Point(0, 0);
            ab2ToolStripIpTransfer.Name = "ab2ToolStripIpTransfer";
            ab2ToolStripIpTransfer.Padding = new Padding(5);
            ab2ToolStripIpTransfer.Size = new Size(1337, 33);
            ab2ToolStripIpTransfer.TabIndex = 6;
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
            // BtnPatientSearch
            // 
            BtnPatientSearch.BackColor = SystemColors.ControlLight;
            BtnPatientSearch.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnPatientSearch.Image = (Image)resources.GetObject("BtnPatientSearch.Image");
            BtnPatientSearch.ImageTransparentColor = Color.Magenta;
            BtnPatientSearch.Name = "BtnPatientSearch";
            BtnPatientSearch.Size = new Size(26, 20);
            BtnPatientSearch.Text = "Go";
            BtnPatientSearch.Click += BtnPatientSearch_Click;
            // 
            // InPatientInfoMin
            // 
            InPatientInfoMin.AutoSize = true;
            InPatientInfoMin.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            InPatientInfoMin.Location = new Point(3, 36);
            InPatientInfoMin.Margin = new Padding(4);
            InPatientInfoMin.MaximumSize = new Size(197, 750);
            InPatientInfoMin.MinimumSize = new Size(197, 505);
            InPatientInfoMin.Name = "InPatientInfoMin";
            InPatientInfoMin.PatientId = null;
            InPatientInfoMin.Short = false;
            InPatientInfoMin.Size = new Size(197, 530);
            InPatientInfoMin.TabIndex = 7;
            // 
            // TabControlReAssignCareTaker
            // 
            TabControlReAssignCareTaker.Controls.Add(TabPageTransferDetails);
            TabControlReAssignCareTaker.Location = new Point(203, 36);
            TabControlReAssignCareTaker.Name = "TabControlReAssignCareTaker";
            TabControlReAssignCareTaker.SelectedIndex = 0;
            TabControlReAssignCareTaker.Size = new Size(398, 523);
            TabControlReAssignCareTaker.TabIndex = 8;
            // 
            // TabPageTransferDetails
            // 
            TabPageTransferDetails.BackColor = SystemColors.Control;
            TabPageTransferDetails.Controls.Add(BtnReAssign);
            TabPageTransferDetails.Controls.Add(BtnCancel);
            TabPageTransferDetails.Controls.Add(GroupBoxCareTakersFromDetail);
            TabPageTransferDetails.Controls.Add(GroupBoxNewCareTakerDetail);
            TabPageTransferDetails.Location = new Point(4, 22);
            TabPageTransferDetails.Name = "TabPageTransferDetails";
            TabPageTransferDetails.Padding = new Padding(3);
            TabPageTransferDetails.Size = new Size(390, 497);
            TabPageTransferDetails.TabIndex = 0;
            TabPageTransferDetails.Text = "Re-assign Care Takers";
            // 
            // BtnReAssign
            // 
            BtnReAssign.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnReAssign.Location = new Point(279, 438);
            BtnReAssign.Name = "BtnReAssign";
            BtnReAssign.Size = new Size(98, 23);
            BtnReAssign.TabIndex = 6;
            BtnReAssign.Text = "Re-assign [F8]";
            BtnReAssign.UseVisualStyleBackColor = true;
            BtnReAssign.Click += BtnReAssign_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(187, 438);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(83, 23);
            BtnCancel.TabIndex = 7;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            BtnCancel.PreviewKeyDown += BtnCancel_PreviewKeyDown;
            // 
            // GroupBoxCareTakersFromDetail
            // 
            GroupBoxCareTakersFromDetail.Controls.Add(TextBoxSecondaryNurse);
            GroupBoxCareTakersFromDetail.Controls.Add(TextBoxPrimaryNurse);
            GroupBoxCareTakersFromDetail.Controls.Add(label4);
            GroupBoxCareTakersFromDetail.Controls.Add(label8);
            GroupBoxCareTakersFromDetail.Controls.Add(TextBoxSecondaryDoctor);
            GroupBoxCareTakersFromDetail.Controls.Add(TextBoxPrimaryDoctor);
            GroupBoxCareTakersFromDetail.Controls.Add(label1);
            GroupBoxCareTakersFromDetail.Controls.Add(label2);
            GroupBoxCareTakersFromDetail.Location = new Point(6, 6);
            GroupBoxCareTakersFromDetail.Name = "GroupBoxCareTakersFromDetail";
            GroupBoxCareTakersFromDetail.Size = new Size(376, 121);
            GroupBoxCareTakersFromDetail.TabIndex = 0;
            GroupBoxCareTakersFromDetail.TabStop = false;
            GroupBoxCareTakersFromDetail.Text = "From";
            // 
            // TextBoxSecondaryNurse
            // 
            TextBoxSecondaryNurse.BackColor = Color.White;
            TextBoxSecondaryNurse.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxSecondaryNurse.Location = new Point(193, 83);
            TextBoxSecondaryNurse.Name = "TextBoxSecondaryNurse";
            TextBoxSecondaryNurse.ReadOnly = true;
            TextBoxSecondaryNurse.Size = new Size(178, 21);
            TextBoxSecondaryNurse.TabIndex = 9;
            TextBoxSecondaryNurse.TabStop = false;
            // 
            // TextBoxPrimaryNurse
            // 
            TextBoxPrimaryNurse.BackColor = Color.White;
            TextBoxPrimaryNurse.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxPrimaryNurse.Location = new Point(193, 43);
            TextBoxPrimaryNurse.Name = "TextBoxPrimaryNurse";
            TextBoxPrimaryNurse.ReadOnly = true;
            TextBoxPrimaryNurse.Size = new Size(178, 21);
            TextBoxPrimaryNurse.TabIndex = 10;
            TextBoxPrimaryNurse.TabStop = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(190, 27);
            label4.Name = "label4";
            label4.Size = new Size(74, 13);
            label4.TabIndex = 7;
            label4.Text = "Primary Nurse";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(190, 67);
            label8.Name = "label8";
            label8.Size = new Size(89, 13);
            label8.TabIndex = 8;
            label8.Text = "Secondary Nurse";
            // 
            // TextBoxSecondaryDoctor
            // 
            TextBoxSecondaryDoctor.BackColor = Color.White;
            TextBoxSecondaryDoctor.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxSecondaryDoctor.Location = new Point(9, 83);
            TextBoxSecondaryDoctor.Name = "TextBoxSecondaryDoctor";
            TextBoxSecondaryDoctor.ReadOnly = true;
            TextBoxSecondaryDoctor.Size = new Size(178, 21);
            TextBoxSecondaryDoctor.TabIndex = 5;
            TextBoxSecondaryDoctor.TabStop = false;
            // 
            // TextBoxPrimaryDoctor
            // 
            TextBoxPrimaryDoctor.BackColor = Color.White;
            TextBoxPrimaryDoctor.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxPrimaryDoctor.Location = new Point(9, 43);
            TextBoxPrimaryDoctor.Name = "TextBoxPrimaryDoctor";
            TextBoxPrimaryDoctor.ReadOnly = true;
            TextBoxPrimaryDoctor.Size = new Size(178, 21);
            TextBoxPrimaryDoctor.TabIndex = 6;
            TextBoxPrimaryDoctor.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 27);
            label1.Name = "label1";
            label1.Size = new Size(78, 13);
            label1.TabIndex = 0;
            label1.Text = "Primary Doctor";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 67);
            label2.Name = "label2";
            label2.Size = new Size(93, 13);
            label2.TabIndex = 1;
            label2.Text = "Secondary Doctor";
            // 
            // GroupBoxNewCareTakerDetail
            // 
            GroupBoxNewCareTakerDetail.Controls.Add(ComboBoxNewSecondaryDoctor);
            GroupBoxNewCareTakerDetail.Controls.Add(ComboBoxNewSecondaryNurse);
            GroupBoxNewCareTakerDetail.Controls.Add(ComboBoxNewPrimaryNurse);
            GroupBoxNewCareTakerDetail.Controls.Add(ComboBoxNewPrimaryDoctor);
            GroupBoxNewCareTakerDetail.Controls.Add(label3);
            GroupBoxNewCareTakerDetail.Controls.Add(label5);
            GroupBoxNewCareTakerDetail.Controls.Add(label9);
            GroupBoxNewCareTakerDetail.Controls.Add(label10);
            GroupBoxNewCareTakerDetail.Controls.Add(ComboBoxAuthorizedBy);
            GroupBoxNewCareTakerDetail.Controls.Add(label7);
            GroupBoxNewCareTakerDetail.Controls.Add(TextBoxNote);
            GroupBoxNewCareTakerDetail.Controls.Add(label6);
            GroupBoxNewCareTakerDetail.Location = new Point(6, 133);
            GroupBoxNewCareTakerDetail.Name = "GroupBoxNewCareTakerDetail";
            GroupBoxNewCareTakerDetail.Size = new Size(376, 248);
            GroupBoxNewCareTakerDetail.TabIndex = 0;
            GroupBoxNewCareTakerDetail.TabStop = false;
            GroupBoxNewCareTakerDetail.Text = "To";
            // 
            // ComboBoxNewSecondaryDoctor
            // 
            ComboBoxNewSecondaryDoctor.FormattingEnabled = true;
            ComboBoxNewSecondaryDoctor.Location = new Point(9, 81);
            ComboBoxNewSecondaryDoctor.Name = "ComboBoxNewSecondaryDoctor";
            ComboBoxNewSecondaryDoctor.Size = new Size(178, 21);
            ComboBoxNewSecondaryDoctor.TabIndex = 2;
            // 
            // ComboBoxNewSecondaryNurse
            // 
            ComboBoxNewSecondaryNurse.FormattingEnabled = true;
            ComboBoxNewSecondaryNurse.Location = new Point(193, 81);
            ComboBoxNewSecondaryNurse.Name = "ComboBoxNewSecondaryNurse";
            ComboBoxNewSecondaryNurse.Size = new Size(178, 21);
            ComboBoxNewSecondaryNurse.TabIndex = 3;
            // 
            // ComboBoxNewPrimaryNurse
            // 
            ComboBoxNewPrimaryNurse.FormattingEnabled = true;
            ComboBoxNewPrimaryNurse.Location = new Point(193, 40);
            ComboBoxNewPrimaryNurse.Name = "ComboBoxNewPrimaryNurse";
            ComboBoxNewPrimaryNurse.Size = new Size(178, 21);
            ComboBoxNewPrimaryNurse.TabIndex = 1;
            // 
            // ComboBoxNewPrimaryDoctor
            // 
            ComboBoxNewPrimaryDoctor.FormattingEnabled = true;
            ComboBoxNewPrimaryDoctor.Location = new Point(9, 40);
            ComboBoxNewPrimaryDoctor.Name = "ComboBoxNewPrimaryDoctor";
            ComboBoxNewPrimaryDoctor.Size = new Size(178, 21);
            ComboBoxNewPrimaryDoctor.TabIndex = 0;
            ComboBoxNewPrimaryDoctor.PreviewKeyDown += ComboBoxNewPrimaryDoctor_PreviewKeyDown;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(190, 25);
            label3.Name = "label3";
            label3.Size = new Size(87, 13);
            label3.TabIndex = 15;
            label3.Text = "Primary Nurse";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(190, 65);
            label5.Name = "label5";
            label5.Size = new Size(89, 13);
            label5.TabIndex = 16;
            label5.Text = "Secondary Nurse";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label9.Location = new Point(6, 25);
            label9.Name = "label9";
            label9.Size = new Size(93, 13);
            label9.TabIndex = 11;
            label9.Text = "Primary Doctor";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(6, 65);
            label10.Name = "label10";
            label10.Size = new Size(93, 13);
            label10.TabIndex = 12;
            label10.Text = "Secondary Doctor";
            // 
            // ComboBoxAuthorizedBy
            // 
            ComboBoxAuthorizedBy.FormattingEnabled = true;
            ComboBoxAuthorizedBy.Location = new Point(9, 213);
            ComboBoxAuthorizedBy.Name = "ComboBoxAuthorizedBy";
            ComboBoxAuthorizedBy.Size = new Size(282, 21);
            ComboBoxAuthorizedBy.TabIndex = 5;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label7.Location = new Point(6, 196);
            label7.Name = "label7";
            label7.Size = new Size(86, 13);
            label7.TabIndex = 9;
            label7.Text = "Authorized By";
            // 
            // TextBoxNote
            // 
            TextBoxNote.Location = new Point(9, 121);
            TextBoxNote.MaxLength = 500;
            TextBoxNote.Multiline = true;
            TextBoxNote.Name = "TextBoxNote";
            TextBoxNote.Size = new Size(282, 71);
            TextBoxNote.TabIndex = 4;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label6.Location = new Point(6, 104);
            label6.Name = "label6";
            label6.Size = new Size(39, 13);
            label6.TabIndex = 7;
            label6.Text = "Notes";
            // 
            // StatusStripIpTransfer
            // 
            StatusStripIpTransfer.ImageScalingSize = new Size(24, 24);
            StatusStripIpTransfer.Items.AddRange(new ToolStripItem[] { CareTakerErrorMsg });
            StatusStripIpTransfer.Location = new Point(0, 571);
            StatusStripIpTransfer.Name = "StatusStripIpTransfer";
            StatusStripIpTransfer.Size = new Size(1337, 22);
            StatusStripIpTransfer.TabIndex = 68;
            StatusStripIpTransfer.Text = "statusStrip1";
            // 
            // CareTakerErrorMsg
            // 
            CareTakerErrorMsg.Name = "CareTakerErrorMsg";
            CareTakerErrorMsg.Size = new Size(25, 17);
            CareTakerErrorMsg.Text = "      ";
            // 
            // labelIpLocationHistory
            // 
            labelIpLocationHistory.AutoSize = true;
            labelIpLocationHistory.Location = new Point(606, 40);
            labelIpLocationHistory.Name = "labelIpLocationHistory";
            labelIpLocationHistory.Size = new Size(102, 13);
            labelIpLocationHistory.TabIndex = 70;
            labelIpLocationHistory.Text = "Care Takers History";
            // 
            // GridViewCareTakerHistory
            // 
            GridViewCareTakerHistory.AllowUserToAddRows = false;
            GridViewCareTakerHistory.AllowUserToDeleteRows = false;
            GridViewCareTakerHistory.AllowUserToResizeColumns = false;
            GridViewCareTakerHistory.AllowUserToResizeRows = false;
            GridViewCareTakerHistory.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            GridViewCareTakerHistory.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewCareTakerHistory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewCareTakerHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GridViewCareTakerHistory.Columns.AddRange(new DataGridViewColumn[] { From, To, PromaryDoc, Bed, PrimaryNurse, SecondaryNurse, TransferReason, AuthorizedBy });
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle10.BackColor = SystemColors.Window;
            dataGridViewCellStyle10.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle10.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.True;
            GridViewCareTakerHistory.DefaultCellStyle = dataGridViewCellStyle10;
            GridViewCareTakerHistory.EnableHeadersVisualStyles = false;
            GridViewCareTakerHistory.Location = new Point(605, 58);
            GridViewCareTakerHistory.MultiSelect = false;
            GridViewCareTakerHistory.Name = "GridViewCareTakerHistory";
            GridViewCareTakerHistory.ReadOnly = true;
            GridViewCareTakerHistory.RowHeadersVisible = false;
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.True;
            GridViewCareTakerHistory.RowsDefaultCellStyle = dataGridViewCellStyle11;
            GridViewCareTakerHistory.RowTemplate.Height = 20;
            GridViewCareTakerHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewCareTakerHistory.ShowCellToolTips = false;
            GridViewCareTakerHistory.Size = new Size(720, 497);
            GridViewCareTakerHistory.TabIndex = 69;
            // 
            // From
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            From.DefaultCellStyle = dataGridViewCellStyle2;
            From.HeaderText = "From";
            From.Name = "From";
            From.ReadOnly = true;
            From.Resizable = DataGridViewTriState.False;
            From.SortMode = DataGridViewColumnSortMode.NotSortable;
            From.Width = 75;
            // 
            // To
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            To.DefaultCellStyle = dataGridViewCellStyle3;
            To.HeaderText = "To";
            To.Name = "To";
            To.ReadOnly = true;
            To.Resizable = DataGridViewTriState.False;
            To.SortMode = DataGridViewColumnSortMode.NotSortable;
            To.Width = 75;
            // 
            // PromaryDoc
            // 
            PromaryDoc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopLeft;
            PromaryDoc.DefaultCellStyle = dataGridViewCellStyle4;
            PromaryDoc.HeaderText = "Primary Doctor";
            PromaryDoc.Name = "PromaryDoc";
            PromaryDoc.ReadOnly = true;
            PromaryDoc.Resizable = DataGridViewTriState.False;
            PromaryDoc.SortMode = DataGridViewColumnSortMode.NotSortable;
            PromaryDoc.Width = 76;
            // 
            // Bed
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.TopLeft;
            Bed.DefaultCellStyle = dataGridViewCellStyle5;
            Bed.HeaderText = "Secondary Doctor";
            Bed.Name = "Bed";
            Bed.ReadOnly = true;
            Bed.Resizable = DataGridViewTriState.False;
            Bed.SortMode = DataGridViewColumnSortMode.NotSortable;
            Bed.Width = 80;
            // 
            // PrimaryNurse
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.TopLeft;
            PrimaryNurse.DefaultCellStyle = dataGridViewCellStyle6;
            PrimaryNurse.HeaderText = "Primary Nurse";
            PrimaryNurse.Name = "PrimaryNurse";
            PrimaryNurse.ReadOnly = true;
            PrimaryNurse.Resizable = DataGridViewTriState.False;
            PrimaryNurse.SortMode = DataGridViewColumnSortMode.NotSortable;
            PrimaryNurse.Width = 80;
            // 
            // SecondaryNurse
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.TopLeft;
            SecondaryNurse.DefaultCellStyle = dataGridViewCellStyle7;
            SecondaryNurse.HeaderText = "Secondary Nurse";
            SecondaryNurse.Name = "SecondaryNurse";
            SecondaryNurse.ReadOnly = true;
            SecondaryNurse.Resizable = DataGridViewTriState.False;
            SecondaryNurse.SortMode = DataGridViewColumnSortMode.NotSortable;
            SecondaryNurse.Width = 80;
            // 
            // TransferReason
            // 
            TransferReason.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
            TransferReason.DefaultCellStyle = dataGridViewCellStyle8;
            TransferReason.HeaderText = "Notes";
            TransferReason.Name = "TransferReason";
            TransferReason.ReadOnly = true;
            TransferReason.Resizable = DataGridViewTriState.False;
            TransferReason.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // AuthorizedBy
            // 
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.TopLeft;
            AuthorizedBy.DefaultCellStyle = dataGridViewCellStyle9;
            AuthorizedBy.HeaderText = "Aurhorized By";
            AuthorizedBy.Name = "AuthorizedBy";
            AuthorizedBy.ReadOnly = true;
            AuthorizedBy.Resizable = DataGridViewTriState.False;
            AuthorizedBy.SortMode = DataGridViewColumnSortMode.NotSortable;
            AuthorizedBy.Width = 80;
            // 
            // FormAssignCareTaker
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1337, 593);
            Controls.Add(labelIpLocationHistory);
            Controls.Add(GridViewCareTakerHistory);
            Controls.Add(StatusStripIpTransfer);
            Controls.Add(TabControlReAssignCareTaker);
            Controls.Add(InPatientInfoMin);
            Controls.Add(ab2ToolStripIpTransfer);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormAssignCareTaker";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Re Assign Care Taker";
            Load += FormAssignCareTaker_Load;
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(ab2ToolStripIpTransfer, 0);
            Controls.SetChildIndex(InPatientInfoMin, 0);
            Controls.SetChildIndex(TabControlReAssignCareTaker, 0);
            Controls.SetChildIndex(StatusStripIpTransfer, 0);
            Controls.SetChildIndex(GridViewCareTakerHistory, 0);
            Controls.SetChildIndex(labelIpLocationHistory, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            ab2ToolStripIpTransfer.ResumeLayout(false);
            ab2ToolStripIpTransfer.PerformLayout();
            TabControlReAssignCareTaker.ResumeLayout(false);
            TabPageTransferDetails.ResumeLayout(false);
            GroupBoxCareTakersFromDetail.ResumeLayout(false);
            GroupBoxCareTakersFromDetail.PerformLayout();
            GroupBoxNewCareTakerDetail.ResumeLayout(false);
            GroupBoxNewCareTakerDetail.PerformLayout();
            StatusStripIpTransfer.ResumeLayout(false);
            StatusStripIpTransfer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewCareTakerHistory).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private controls.Ab2ToolStrip ab2ToolStripIpTransfer;
        private System.Windows.Forms.ToolStripLabel ToolStripLabelSearch;
        private System.Windows.Forms.ToolStripButton BtnPatientSearch;
        private controls.hms.PatientInfoMin InPatientInfoMin;
        private System.Windows.Forms.TabControl TabControlReAssignCareTaker;
        private System.Windows.Forms.TabPage TabPageTransferDetails;
        private System.Windows.Forms.Button BtnReAssign;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.GroupBox GroupBoxCareTakersFromDetail;
        private System.Windows.Forms.TextBox TextBoxSecondaryNurse;
        private System.Windows.Forms.TextBox TextBoxPrimaryNurse;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TextBoxSecondaryDoctor;
        private System.Windows.Forms.TextBox TextBoxPrimaryDoctor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox GroupBoxNewCareTakerDetail;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox ComboBoxAuthorizedBy;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox TextBoxNote;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.StatusStrip StatusStripIpTransfer;
        private System.Windows.Forms.ToolStripStatusLabel CareTakerErrorMsg;
        private System.Windows.Forms.Label labelIpLocationHistory;
        private controls.DataViewVerticalScroll GridViewCareTakerHistory;
        private System.Windows.Forms.ComboBox ComboBoxNewPrimaryDoctor;
        private System.Windows.Forms.ComboBox ComboBoxNewPrimaryNurse;
        private System.Windows.Forms.ComboBox ComboBoxNewSecondaryDoctor;
        private System.Windows.Forms.ComboBox ComboBoxNewSecondaryNurse;
        private System.Windows.Forms.ToolStripTextBox TextBoxPatientSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn From;
        private System.Windows.Forms.DataGridViewTextBoxColumn To;
        private System.Windows.Forms.DataGridViewTextBoxColumn PromaryDoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bed;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrimaryNurse;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecondaryNurse;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransferReason;
        private System.Windows.Forms.DataGridViewTextBoxColumn AuthorizedBy;
    }
}