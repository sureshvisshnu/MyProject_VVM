namespace Fa.views.purchase
{
    partial class FormProductResolution
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProductResolution));
            GridViewProductRectification = new fa.views.controls.DataViewVerticalScroll();
            berklysoftToolStripProductResolution = new fa.views.controls.Ab2ToolStrip();
            toolStripSeparator2 = new ToolStripSeparator();
            berklysoftstatusStripProductResolution = new StatusStrip();
            ToolStripStatusLabelErrorProductTemplate = new ToolStripStatusLabel();
            BtnMappingDone = new Button();
            BtnMappingCancel = new Button();
            TextBoxProductMappingId = new TextBox();
            BtnMappingSave = new Button();
            ((System.ComponentModel.ISupportInitialize)GridViewProductRectification).BeginInit();
            berklysoftToolStripProductResolution.SuspendLayout();
            berklysoftstatusStripProductResolution.SuspendLayout();
            SuspendLayout();
            // 
            // GridViewProductRectification
            // 
            GridViewProductRectification.AllowUserToAddRows = false;
            GridViewProductRectification.AllowUserToDeleteRows = false;
            GridViewProductRectification.AllowUserToResizeColumns = false;
            GridViewProductRectification.AllowUserToResizeRows = false;
            GridViewProductRectification.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewProductRectification.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewProductRectification.ColumnHeadersHeight = 20;
            GridViewProductRectification.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            GridViewProductRectification.DefaultCellStyle = dataGridViewCellStyle2;
            GridViewProductRectification.EditMode = DataGridViewEditMode.EditProgrammatically;
            GridViewProductRectification.EnableHeadersVisualStyles = false;
            GridViewProductRectification.Location = new Point(7, 39);
            GridViewProductRectification.MultiSelect = false;
            GridViewProductRectification.Name = "GridViewProductRectification";
            GridViewProductRectification.ReadOnly = true;
            GridViewProductRectification.RowHeadersVisible = false;
            GridViewProductRectification.RowTemplate.Height = 20;
            GridViewProductRectification.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewProductRectification.ShowCellToolTips = false;
            GridViewProductRectification.ShowEditingIcon = false;
            GridViewProductRectification.Size = new Size(850, 638);
            GridViewProductRectification.TabIndex = 29;
            GridViewProductRectification.CellClick += GridViewProductRectification_CellClick;
            GridViewProductRectification.CellFormatting += GridViewProductRectification_CellFormatting;
            GridViewProductRectification.CellValueChanged += GridViewProductRectification_CellValueChanged;
            // 
            // berklysoftToolStripProductResolution
            // 
            berklysoftToolStripProductResolution.GripStyle = ToolStripGripStyle.Hidden;
            berklysoftToolStripProductResolution.Items.AddRange(new ToolStripItem[] { toolStripSeparator2 });
            berklysoftToolStripProductResolution.Location = new Point(0, 0);
            berklysoftToolStripProductResolution.Name = "berklysoftToolStripProductResolution";
            berklysoftToolStripProductResolution.Padding = new Padding(5);
            berklysoftToolStripProductResolution.Size = new Size(864, 33);
            berklysoftToolStripProductResolution.TabIndex = 30;
            berklysoftToolStripProductResolution.Text = "ab2ToolStrip1";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 23);
            // 
            // berklysoftstatusStripProductResolution
            // 
            berklysoftstatusStripProductResolution.Items.AddRange(new ToolStripItem[] { ToolStripStatusLabelErrorProductTemplate });
            berklysoftstatusStripProductResolution.Location = new Point(0, 731);
            berklysoftstatusStripProductResolution.Name = "berklysoftstatusStripProductResolution";
            berklysoftstatusStripProductResolution.Size = new Size(864, 22);
            berklysoftstatusStripProductResolution.TabIndex = 31;
            berklysoftstatusStripProductResolution.Text = "statusStrip1";
            // 
            // ToolStripStatusLabelErrorProductTemplate
            // 
            ToolStripStatusLabelErrorProductTemplate.Name = "ToolStripStatusLabelErrorProductTemplate";
            ToolStripStatusLabelErrorProductTemplate.Size = new Size(0, 17);
            // 
            // BtnMappingDone
            // 
            BtnMappingDone.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnMappingDone.Location = new Point(776, 696);
            BtnMappingDone.Name = "BtnMappingDone";
            BtnMappingDone.Size = new Size(75, 23);
            BtnMappingDone.TabIndex = 60;
            BtnMappingDone.Text = "Done";
            BtnMappingDone.UseVisualStyleBackColor = true;
            BtnMappingDone.Click += BtnMappingDone_Click;
            // 
            // BtnMappingCancel
            // 
            BtnMappingCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnMappingCancel.Location = new Point(681, 696);
            BtnMappingCancel.Name = "BtnMappingCancel";
            BtnMappingCancel.Size = new Size(89, 23);
            BtnMappingCancel.TabIndex = 61;
            BtnMappingCancel.Text = "Cancel [Esc]";
            BtnMappingCancel.UseVisualStyleBackColor = true;
            // 
            // TextBoxProductMappingId
            // 
            TextBoxProductMappingId.Location = new Point(382, 365);
            TextBoxProductMappingId.Name = "TextBoxProductMappingId";
            TextBoxProductMappingId.Size = new Size(100, 23);
            TextBoxProductMappingId.TabIndex = 286;
            TextBoxProductMappingId.Visible = false;
            // 
            // BtnMappingSave
            // 
            BtnMappingSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnMappingSave.Location = new Point(600, 696);
            BtnMappingSave.Name = "BtnMappingSave";
            BtnMappingSave.Size = new Size(75, 23);
            BtnMappingSave.TabIndex = 287;
            BtnMappingSave.Text = "Save [F2]";
            BtnMappingSave.UseVisualStyleBackColor = true;
            BtnMappingSave.Click += BtnMappingSave_Click;
            // 
            // FormProductResolution
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(864, 753);
            Controls.Add(BtnMappingSave);
            Controls.Add(TextBoxProductMappingId);
            Controls.Add(BtnMappingDone);
            Controls.Add(BtnMappingCancel);
            Controls.Add(berklysoftstatusStripProductResolution);
            Controls.Add(berklysoftToolStripProductResolution);
            Controls.Add(GridViewProductRectification);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormProductResolution";
            Text = "Product Resolution";
            Load += FormProductResolution_Load;
            Controls.SetChildIndex(GridViewProductRectification, 0);
            Controls.SetChildIndex(berklysoftToolStripProductResolution, 0);
            Controls.SetChildIndex(berklysoftstatusStripProductResolution, 0);
            Controls.SetChildIndex(BtnMappingCancel, 0);
            Controls.SetChildIndex(BtnMappingDone, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(checkBoxIsPatient, 0);
            Controls.SetChildIndex(TextBoxProductMappingId, 0);
            Controls.SetChildIndex(BtnMappingSave, 0);
            ((System.ComponentModel.ISupportInitialize)GridViewProductRectification).EndInit();
            berklysoftToolStripProductResolution.ResumeLayout(false);
            berklysoftToolStripProductResolution.PerformLayout();
            berklysoftstatusStripProductResolution.ResumeLayout(false);
            berklysoftstatusStripProductResolution.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private fa.views.controls.DataViewVerticalScroll GridViewProductRectification;
        private fa.views.controls.Ab2ToolStrip berklysoftToolStripProductResolution;
        private StatusStrip berklysoftstatusStripProductResolution;
        private Button BtnMappingDone;
        private Button BtnMappingCancel;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripStatusLabel ToolStripStatusLabelErrorProductTemplate;
        private TextBox TextBoxProductMappingId;
        private Button BtnMappingSave;
    }
}