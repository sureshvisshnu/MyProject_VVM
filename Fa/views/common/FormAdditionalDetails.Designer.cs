namespace fa.views.sales
{
    partial class FormAdditionalDetails
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
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAdditionalDetails));
            GridViewAdditionalDetails = new controls.DataViewVerticalScroll();
            Detail = new DataGridViewComboBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Description = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewButtonColumn();
            BtnSave = new Button();
            BtnCancel = new Button();
            statusStrip1 = new StatusStrip();
            ErrorMsg = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)GridViewAdditionalDetails).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
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
            // GridViewAdditionalDetails
            // 
            GridViewAdditionalDetails.AllowUserToDeleteRows = false;
            GridViewAdditionalDetails.AllowUserToResizeColumns = false;
            GridViewAdditionalDetails.AllowUserToResizeRows = false;
            GridViewAdditionalDetails.BackgroundColor = Color.White;
            GridViewAdditionalDetails.ColumnHeadersHeight = 20;
            GridViewAdditionalDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewAdditionalDetails.Columns.AddRange(new DataGridViewColumn[] { Detail, Column2, Description, Column1 });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = Color.White;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            GridViewAdditionalDetails.DefaultCellStyle = dataGridViewCellStyle3;
            GridViewAdditionalDetails.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewAdditionalDetails.EnableHeadersVisualStyles = false;
            GridViewAdditionalDetails.Location = new Point(0, 0);
            GridViewAdditionalDetails.Name = "GridViewAdditionalDetails";
            GridViewAdditionalDetails.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.ForeColor = Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = Color.White;
            dataGridViewCellStyle4.SelectionForeColor = Color.Black;
            GridViewAdditionalDetails.RowsDefaultCellStyle = dataGridViewCellStyle4;
            GridViewAdditionalDetails.RowTemplate.Height = 20;
            GridViewAdditionalDetails.ScrollBars = ScrollBars.Vertical;
            GridViewAdditionalDetails.ShowCellToolTips = false;
            GridViewAdditionalDetails.Size = new Size(471, 251);
            GridViewAdditionalDetails.TabIndex = 0;
            GridViewAdditionalDetails.CellBeginEdit += GridViewAdditionalDetails_CellBeginEdit;
            GridViewAdditionalDetails.CellClick += GridViewAdditionalDetails_CellClick;
            GridViewAdditionalDetails.CellLeave += GridViewAdditionalDetails_CellLeave;
            GridViewAdditionalDetails.DataError += GridViewAdditionalDetails_DataError;
            GridViewAdditionalDetails.EditingControlShowing += GridViewAdditionalDetails_EditingControlShowing;
            GridViewAdditionalDetails.RowsAdded += GridViewAdditionalDetails_RowsAdded;
            // 
            // Detail
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridViewCellStyle1.ForeColor = Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = Color.White;
            dataGridViewCellStyle1.SelectionForeColor = Color.Black;
            Detail.DefaultCellStyle = dataGridViewCellStyle1;
            Detail.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            Detail.FlatStyle = FlatStyle.Popup;
            Detail.HeaderText = "Detail";
            Detail.Name = "Detail";
            Detail.Resizable = DataGridViewTriState.False;
            Detail.Width = 150;
            // 
            // Column2
            // 
            Column2.HeaderText = "HDetail";
            Column2.Name = "Column2";
            Column2.Visible = false;
            Column2.Width = 150;
            // 
            // Description
            // 
            Description.HeaderText = "Description";
            Description.MaxInputLength = 100;
            Description.Name = "Description";
            Description.Resizable = DataGridViewTriState.False;
            Description.SortMode = DataGridViewColumnSortMode.NotSortable;
            Description.Width = 275;
            // 
            // Column1
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.NullValue = "X";
            dataGridViewCellStyle2.SelectionBackColor = Color.White;
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            Column1.DefaultCellStyle = dataGridViewCellStyle2;
            Column1.HeaderText = "...";
            Column1.Name = "Column1";
            Column1.Resizable = DataGridViewTriState.False;
            Column1.Width = 25;
            // 
            // BtnSave
            // 
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(384, 264);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 23);
            BtnSave.TabIndex = 1;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            BtnSave.PreviewKeyDown += BtnSave_PreviewKeyDown;
            // 
            // BtnCancel
            // 
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(293, 264);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(85, 23);
            BtnCancel.TabIndex = 2;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            BtnCancel.PreviewKeyDown += BtnCancel_PreviewKeyDown;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsg });
            statusStrip1.Location = new Point(0, 300);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(472, 22);
            statusStrip1.TabIndex = 4;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(46, 17);
            ErrorMsg.Text = "             ";
            // 
            // FormAdditionalDetails
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(472, 322);
            Controls.Add(statusStrip1);
            Controls.Add(BtnCancel);
            Controls.Add(BtnSave);
            Controls.Add(GridViewAdditionalDetails);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormAdditionalDetails";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Additional Details";
            Load += FormAdditionalDetails_Load;
            Controls.SetChildIndex(GridViewAdditionalDetails, 0);
            Controls.SetChildIndex(BtnSave, 0);
            Controls.SetChildIndex(BtnCancel, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            ((System.ComponentModel.ISupportInitialize)GridViewAdditionalDetails).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private controls.DataViewVerticalScroll GridViewAdditionalDetails;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ErrorMsg;
        private System.Windows.Forms.DataGridViewComboBoxColumn Detail;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewButtonColumn Column1;
    }
}