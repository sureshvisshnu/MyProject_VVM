namespace Fa.reports.Purchase
{
    partial class ForrmInvoiceUpload
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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ForrmInvoiceUpload));
            StatusStripInvoiceLoad = new StatusStrip();
            ToolStripStatusLabelErrorInvoice = new ToolStripStatusLabel();
            DataViewInvoiceSelecter = new fa.views.controls.DataViewVerticalScroll();
            lblIvoicedata = new Label();
            BtnUpload = new Button();
            BtnChoose = new Button();
            TextBoxFileName = new TextBox();
            BtnReset = new Button();
            DataViewInvoiceHeaderSelecter = new fa.views.controls.DataViewVerticalScroll();
            checkBoxHeader = new CheckBox();
            groupBox1 = new GroupBox();
            comboBoxSwapTextBoxAvailabelTemplet = new fa.views.controls.ComboBoxSwapTextBox();
            BtnApplyTemplet = new Button();
            label1 = new Label();
            BtnSveTemplet = new Button();
            BtnItemVerify = new Button();
            lblTempleName = new Label();
            TextBoxTemplateName = new TextBox();
            TextBoxInvoiceTemplateId = new TextBox();
            checkBoxFooter = new CheckBox();
            DataViewInvoiceFooterSelecter = new fa.views.controls.DataViewVerticalScroll();
            TextBoxHeaderMappingId = new TextBox();
            StatusStripInvoiceLoad.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DataViewInvoiceSelecter).BeginInit();
            ((System.ComponentModel.ISupportInitialize)DataViewInvoiceHeaderSelecter).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DataViewInvoiceFooterSelecter).BeginInit();
            SuspendLayout();
            // 
            // StatusStripInvoiceLoad
            // 
            StatusStripInvoiceLoad.Items.AddRange(new ToolStripItem[] { ToolStripStatusLabelErrorInvoice });
            StatusStripInvoiceLoad.Location = new Point(0, 725);
            StatusStripInvoiceLoad.Name = "StatusStripInvoiceLoad";
            StatusStripInvoiceLoad.Padding = new Padding(1, 0, 16, 0);
            StatusStripInvoiceLoad.Size = new Size(1379, 22);
            StatusStripInvoiceLoad.TabIndex = 278;
            StatusStripInvoiceLoad.Text = "statusStrip1";
            // 
            // ToolStripStatusLabelErrorInvoice
            // 
            ToolStripStatusLabelErrorInvoice.Name = "ToolStripStatusLabelErrorInvoice";
            ToolStripStatusLabelErrorInvoice.Size = new Size(94, 17);
            ToolStripStatusLabelErrorInvoice.Text = "                             ";
            // 
            // DataViewInvoiceSelecter
            // 
            DataViewInvoiceSelecter.AllowUserToAddRows = false;
            DataViewInvoiceSelecter.AllowUserToDeleteRows = false;
            DataViewInvoiceSelecter.AllowUserToResizeRows = false;
            DataViewInvoiceSelecter.BackgroundColor = SystemColors.Control;
            DataViewInvoiceSelecter.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            DataViewInvoiceSelecter.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            DataViewInvoiceSelecter.ColumnHeadersHeight = 25;
            DataViewInvoiceSelecter.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            DataViewInvoiceSelecter.DefaultCellStyle = dataGridViewCellStyle2;
            DataViewInvoiceSelecter.EditMode = DataGridViewEditMode.EditOnEnter;
            DataViewInvoiceSelecter.EnableHeadersVisualStyles = false;
            DataViewInvoiceSelecter.Location = new Point(14, 199);
            DataViewInvoiceSelecter.MultiSelect = false;
            DataViewInvoiceSelecter.Name = "DataViewInvoiceSelecter";
            DataViewInvoiceSelecter.RowHeadersVisible = false;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.White;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            DataViewInvoiceSelecter.RowsDefaultCellStyle = dataGridViewCellStyle3;
            DataViewInvoiceSelecter.RowTemplate.Height = 25;
            DataViewInvoiceSelecter.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DataViewInvoiceSelecter.ShowCellToolTips = false;
            DataViewInvoiceSelecter.Size = new Size(1352, 399);
            DataViewInvoiceSelecter.TabIndex = 279;
            DataViewInvoiceSelecter.CellMouseClick += DataViewInvoiceSelecter_CellMouseClick;
            DataViewInvoiceSelecter.CellMouseDown += DataViewInvoiceSelecter_CellMouseDown;
            DataViewInvoiceSelecter.ColumnAdded += DataViewInvoiceSelecter_ColumnAdded;
            DataViewInvoiceSelecter.ColumnHeaderMouseClick += DataViewInvoiceSelecter_ColumnHeaderMouseClick;
            DataViewInvoiceSelecter.SelectionChanged += DataViewInvoiceSelecter_SelectionChanged;
            // 
            // lblIvoicedata
            // 
            lblIvoicedata.AutoSize = true;
            lblIvoicedata.Location = new Point(17, 179);
            lblIvoicedata.Name = "lblIvoicedata";
            lblIvoicedata.Size = new Size(68, 13);
            lblIvoicedata.TabIndex = 281;
            lblIvoicedata.Text = "Invoice Data";
            // 
            // BtnUpload
            // 
            BtnUpload.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            BtnUpload.Location = new Point(1298, 17);
            BtnUpload.Name = "BtnUpload";
            BtnUpload.Size = new Size(54, 23);
            BtnUpload.TabIndex = 68;
            BtnUpload.Text = "Upload";
            BtnUpload.UseVisualStyleBackColor = true;
            BtnUpload.Click += BtnUpload_Click;
            // 
            // BtnChoose
            // 
            BtnChoose.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            BtnChoose.Location = new Point(371, 18);
            BtnChoose.Margin = new Padding(2);
            BtnChoose.Name = "BtnChoose";
            BtnChoose.Size = new Size(54, 23);
            BtnChoose.TabIndex = 67;
            BtnChoose.Text = "Browse";
            BtnChoose.UseVisualStyleBackColor = true;
            BtnChoose.Click += BtnChoose_Click;
            // 
            // TextBoxFileName
            // 
            TextBoxFileName.BackColor = SystemColors.Window;
            TextBoxFileName.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxFileName.Location = new Point(6, 19);
            TextBoxFileName.Name = "TextBoxFileName";
            TextBoxFileName.ReadOnly = true;
            TextBoxFileName.Size = new Size(362, 21);
            TextBoxFileName.TabIndex = 69;
            TextBoxFileName.TabStop = false;
            // 
            // BtnReset
            // 
            BtnReset.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            BtnReset.Location = new Point(427, 18);
            BtnReset.Margin = new Padding(2);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new Size(54, 23);
            BtnReset.TabIndex = 71;
            BtnReset.Text = "Reset";
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // DataViewInvoiceHeaderSelecter
            // 
            DataViewInvoiceHeaderSelecter.AllowUserToAddRows = false;
            DataViewInvoiceHeaderSelecter.AllowUserToDeleteRows = false;
            DataViewInvoiceHeaderSelecter.AllowUserToResizeRows = false;
            DataViewInvoiceHeaderSelecter.BackgroundColor = SystemColors.Control;
            DataViewInvoiceHeaderSelecter.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            DataViewInvoiceHeaderSelecter.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            DataViewInvoiceHeaderSelecter.ColumnHeadersHeight = 25;
            DataViewInvoiceHeaderSelecter.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.Window;
            dataGridViewCellStyle5.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle5.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            DataViewInvoiceHeaderSelecter.DefaultCellStyle = dataGridViewCellStyle5;
            DataViewInvoiceHeaderSelecter.EditMode = DataGridViewEditMode.EditOnEnter;
            DataViewInvoiceHeaderSelecter.EnableHeadersVisualStyles = false;
            DataViewInvoiceHeaderSelecter.Location = new Point(6, 67);
            DataViewInvoiceHeaderSelecter.MultiSelect = false;
            DataViewInvoiceHeaderSelecter.Name = "DataViewInvoiceHeaderSelecter";
            DataViewInvoiceHeaderSelecter.RowHeadersVisible = false;
            dataGridViewCellStyle6.BackColor = Color.White;
            dataGridViewCellStyle6.ForeColor = Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = Color.White;
            dataGridViewCellStyle6.SelectionForeColor = Color.Black;
            DataViewInvoiceHeaderSelecter.RowsDefaultCellStyle = dataGridViewCellStyle6;
            DataViewInvoiceHeaderSelecter.RowTemplate.Height = 25;
            DataViewInvoiceHeaderSelecter.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DataViewInvoiceHeaderSelecter.ShowCellToolTips = false;
            DataViewInvoiceHeaderSelecter.Size = new Size(1355, 93);
            DataViewInvoiceHeaderSelecter.TabIndex = 280;
            // 
            // checkBoxHeader
            // 
            checkBoxHeader.AutoSize = true;
            checkBoxHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            checkBoxHeader.Location = new Point(10, 44);
            checkBoxHeader.Name = "checkBoxHeader";
            checkBoxHeader.Size = new Size(97, 17);
            checkBoxHeader.TabIndex = 281;
            checkBoxHeader.Text = "Has Header?";
            checkBoxHeader.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(comboBoxSwapTextBoxAvailabelTemplet);
            groupBox1.Controls.Add(BtnApplyTemplet);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(BtnSveTemplet);
            groupBox1.Controls.Add(BtnItemVerify);
            groupBox1.Controls.Add(lblTempleName);
            groupBox1.Controls.Add(TextBoxTemplateName);
            groupBox1.Controls.Add(checkBoxHeader);
            groupBox1.Controls.Add(DataViewInvoiceHeaderSelecter);
            groupBox1.Controls.Add(BtnReset);
            groupBox1.Controls.Add(TextBoxFileName);
            groupBox1.Controls.Add(BtnChoose);
            groupBox1.Controls.Add(BtnUpload);
            groupBox1.Location = new Point(5, 10);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1370, 166);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "File Name";
            // 
            // comboBoxSwapTextBoxAvailabelTemplet
            // 
            comboBoxSwapTextBoxAvailabelTemplet.FormattingEnabled = true;
            comboBoxSwapTextBoxAvailabelTemplet.Location = new Point(955, 18);
            comboBoxSwapTextBoxAvailabelTemplet.Name = "comboBoxSwapTextBoxAvailabelTemplet";
            comboBoxSwapTextBoxAvailabelTemplet.Size = new Size(176, 21);
            comboBoxSwapTextBoxAvailabelTemplet.TabIndex = 283;
            comboBoxSwapTextBoxAvailabelTemplet.TxtVisible = true;
            comboBoxSwapTextBoxAvailabelTemplet.SelectedIndexChanged += comboBoxSwapTextBoxAvailabelTemplet_SelectedIndexChanged;
            // 
            // BtnApplyTemplet
            // 
            BtnApplyTemplet.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            BtnApplyTemplet.Location = new Point(1132, 17);
            BtnApplyTemplet.Margin = new Padding(2);
            BtnApplyTemplet.Name = "BtnApplyTemplet";
            BtnApplyTemplet.Size = new Size(54, 23);
            BtnApplyTemplet.TabIndex = 290;
            BtnApplyTemplet.Text = "Apply";
            BtnApplyTemplet.UseVisualStyleBackColor = true;
            BtnApplyTemplet.Click += BtnApplyTemplet_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(863, 22);
            label1.Name = "label1";
            label1.Size = new Size(91, 13);
            label1.TabIndex = 287;
            label1.Text = "Availabel Templet";
            // 
            // BtnSveTemplet
            // 
            BtnSveTemplet.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            BtnSveTemplet.Location = new Point(775, 17);
            BtnSveTemplet.Margin = new Padding(2);
            BtnSveTemplet.Name = "BtnSveTemplet";
            BtnSveTemplet.Size = new Size(87, 23);
            BtnSveTemplet.TabIndex = 286;
            BtnSveTemplet.Text = "Save Templet";
            BtnSveTemplet.UseVisualStyleBackColor = true;
            BtnSveTemplet.Click += BtnSveTemplet_Click_1;
            // 
            // BtnItemVerify
            // 
            BtnItemVerify.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            BtnItemVerify.Location = new Point(1187, 17);
            BtnItemVerify.Margin = new Padding(2);
            BtnItemVerify.Name = "BtnItemVerify";
            BtnItemVerify.Size = new Size(110, 23);
            BtnItemVerify.TabIndex = 288;
            BtnItemVerify.Text = "MaterialID Mapping";
            BtnItemVerify.UseVisualStyleBackColor = true;
            BtnItemVerify.Click += BtnItemVerify_Click;
            // 
            // lblTempleName
            // 
            lblTempleName.AutoSize = true;
            lblTempleName.Location = new Point(518, 22);
            lblTempleName.Name = "lblTempleName";
            lblTempleName.Size = new Size(75, 13);
            lblTempleName.TabIndex = 285;
            lblTempleName.Text = "Templet Name";
            // 
            // TextBoxTemplateName
            // 
            TextBoxTemplateName.BackColor = SystemColors.Window;
            TextBoxTemplateName.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxTemplateName.Location = new Point(594, 18);
            TextBoxTemplateName.Name = "TextBoxTemplateName";
            TextBoxTemplateName.Size = new Size(180, 21);
            TextBoxTemplateName.TabIndex = 282;
            TextBoxTemplateName.TabStop = false;
            // 
            // TextBoxInvoiceTemplateId
            // 
            TextBoxInvoiceTemplateId.Location = new Point(309, 303);
            TextBoxInvoiceTemplateId.Name = "TextBoxInvoiceTemplateId";
            TextBoxInvoiceTemplateId.Size = new Size(100, 21);
            TextBoxInvoiceTemplateId.TabIndex = 284;
            TextBoxInvoiceTemplateId.Visible = false;
            // 
            // checkBoxFooter
            // 
            checkBoxFooter.AutoSize = true;
            checkBoxFooter.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            checkBoxFooter.Location = new Point(16, 604);
            checkBoxFooter.Name = "checkBoxFooter";
            checkBoxFooter.Size = new Size(93, 17);
            checkBoxFooter.TabIndex = 283;
            checkBoxFooter.Text = "Has Footer?";
            checkBoxFooter.UseVisualStyleBackColor = true;
            // 
            // DataViewInvoiceFooterSelecter
            // 
            DataViewInvoiceFooterSelecter.AllowUserToAddRows = false;
            DataViewInvoiceFooterSelecter.AllowUserToDeleteRows = false;
            DataViewInvoiceFooterSelecter.AllowUserToResizeRows = false;
            DataViewInvoiceFooterSelecter.BackgroundColor = SystemColors.Control;
            DataViewInvoiceFooterSelecter.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = SystemColors.Control;
            dataGridViewCellStyle7.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle7.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle7.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            DataViewInvoiceFooterSelecter.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            DataViewInvoiceFooterSelecter.ColumnHeadersHeight = 25;
            DataViewInvoiceFooterSelecter.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = SystemColors.Window;
            dataGridViewCellStyle8.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle8.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            DataViewInvoiceFooterSelecter.DefaultCellStyle = dataGridViewCellStyle8;
            DataViewInvoiceFooterSelecter.EditMode = DataGridViewEditMode.EditOnEnter;
            DataViewInvoiceFooterSelecter.EnableHeadersVisualStyles = false;
            DataViewInvoiceFooterSelecter.Location = new Point(12, 627);
            DataViewInvoiceFooterSelecter.MultiSelect = false;
            DataViewInvoiceFooterSelecter.Name = "DataViewInvoiceFooterSelecter";
            DataViewInvoiceFooterSelecter.RowHeadersVisible = false;
            dataGridViewCellStyle9.BackColor = Color.White;
            dataGridViewCellStyle9.ForeColor = Color.Black;
            dataGridViewCellStyle9.SelectionBackColor = Color.White;
            dataGridViewCellStyle9.SelectionForeColor = Color.Black;
            DataViewInvoiceFooterSelecter.RowsDefaultCellStyle = dataGridViewCellStyle9;
            DataViewInvoiceFooterSelecter.RowTemplate.Height = 25;
            DataViewInvoiceFooterSelecter.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DataViewInvoiceFooterSelecter.ShowCellToolTips = false;
            DataViewInvoiceFooterSelecter.Size = new Size(1355, 93);
            DataViewInvoiceFooterSelecter.TabIndex = 282;
            // 
            // TextBoxHeaderMappingId
            // 
            TextBoxHeaderMappingId.Location = new Point(309, 330);
            TextBoxHeaderMappingId.Name = "TextBoxHeaderMappingId";
            TextBoxHeaderMappingId.Size = new Size(100, 21);
            TextBoxHeaderMappingId.TabIndex = 285;
            TextBoxHeaderMappingId.Visible = false;
            // 
            // ForrmInvoiceUpload
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1379, 747);
            Controls.Add(TextBoxHeaderMappingId);
            Controls.Add(checkBoxFooter);
            Controls.Add(lblIvoicedata);
            Controls.Add(TextBoxInvoiceTemplateId);
            Controls.Add(DataViewInvoiceFooterSelecter);
            Controls.Add(DataViewInvoiceSelecter);
            Controls.Add(StatusStripInvoiceLoad);
            Controls.Add(groupBox1);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ForrmInvoiceUpload";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Invoice Upload";
            Load += ForrmInvoiceUpload_Load;
            StatusStripInvoiceLoad.ResumeLayout(false);
            StatusStripInvoiceLoad.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)DataViewInvoiceSelecter).EndInit();
            ((System.ComponentModel.ISupportInitialize)DataViewInvoiceHeaderSelecter).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)DataViewInvoiceFooterSelecter).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private StatusStrip StatusStripInvoiceLoad;
        private ToolStripStatusLabel ToolStripStatusLabelErrorInvoice;
        private fa.views.controls.DataViewVerticalScroll DataViewInvoiceSelecter;
        private Label lblIvoicedata;
        private Button BtnUpload;
        private Button BtnChoose;
        private TextBox TextBoxFileName;
        private Button BtnReset;
        private fa.views.controls.DataViewVerticalScroll DataViewInvoiceHeaderSelecter;
        private CheckBox checkBoxHeader;
        private GroupBox groupBox1;
        private CheckBox checkBoxFooter;
        private fa.views.controls.DataViewVerticalScroll DataViewInvoiceFooterSelecter;
        private Button BtnItemVerify;
        private fa.views.controls.ComboBoxSwapTextBox comboBoxSwapTextBoxAvailabelTemplet;
        private Label label1;
        private Button BtnSveTemplet;
        private Label lblTempleName;
        private TextBox TextBoxTemplateName;
        private Button BtnApplyTemplet;
        private TextBox TextBoxInvoiceTemplateId;
        private TextBox TextBoxHeaderMappingId;
    }
}