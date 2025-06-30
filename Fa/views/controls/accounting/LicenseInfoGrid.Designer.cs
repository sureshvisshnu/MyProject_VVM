namespace fa.views.controls.accounting
{
    partial class LicenseInfoGrid
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.DataGridViewLicenseType = new fa.views.controls.DataViewVerticalScroll();
            this.TaxType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Values = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewLicenseType)).BeginInit();
            this.SuspendLayout();
            // 
            // DataGridViewLicenseType
            // 
            this.DataGridViewLicenseType.AllowUserToAddRows = false;
            this.DataGridViewLicenseType.AllowUserToDeleteRows = false;
            this.DataGridViewLicenseType.AllowUserToResizeColumns = false;
            this.DataGridViewLicenseType.AllowUserToResizeRows = false;
            this.DataGridViewLicenseType.BackgroundColor = System.Drawing.SystemColors.Window;
            this.DataGridViewLicenseType.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridViewLicenseType.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TaxType,
            this.Values,
            this.Column2});
            this.DataGridViewLicenseType.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DataGridViewLicenseType.Location = new System.Drawing.Point(0, 0);
            this.DataGridViewLicenseType.Margin = new System.Windows.Forms.Padding(2);
            this.DataGridViewLicenseType.Name = "DataGridViewLicenseType";
            this.DataGridViewLicenseType.RowHeadersVisible = false;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.DataGridViewLicenseType.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.DataGridViewLicenseType.RowTemplate.Height = 24;
            this.DataGridViewLicenseType.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DataGridViewLicenseType.Size = new System.Drawing.Size(472, 215);
            this.DataGridViewLicenseType.TabIndex = 29;
            this.DataGridViewLicenseType.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewLicenseType_CellEnter);
            this.DataGridViewLicenseType.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DataGridViewLicenseType_DataError);
            this.DataGridViewLicenseType.SizeChanged += new System.EventHandler(this.DataGridViewLicenseType_SizeChanged);
            this.DataGridViewLicenseType.Enter += new System.EventHandler(this.DataGridViewLicenseType_Enter);
            this.DataGridViewLicenseType.Leave += new System.EventHandler(this.DataGridViewLicenseType_Leave);
            this.DataGridViewLicenseType.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.DataGridViewLicenseType_PreviewKeyDown);
            this.DataGridViewLicenseType.Resize += new System.EventHandler(this.DataGridViewLicenseType_Resize);
            // 
            // TaxType
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TaxType.DefaultCellStyle = dataGridViewCellStyle1;
            this.TaxType.HeaderText = "Type";
            this.TaxType.Name = "TaxType";
            this.TaxType.ReadOnly = true;
            this.TaxType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TaxType.Width = 200;
            // 
            // Values
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Values.DefaultCellStyle = dataGridViewCellStyle2;
            this.Values.HeaderText = "Values";
            this.Values.MaxInputLength = 20;
            this.Values.Name = "Values";
            this.Values.Width = 250;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "ID";
            this.Column2.Name = "Column2";
            this.Column2.Visible = false;
            // 
            // LicenseInfoGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DataGridViewLicenseType);
            this.Name = "LicenseInfoGrid";
            this.Size = new System.Drawing.Size(479, 220);
            this.Resize += new System.EventHandler(this.LicenseInfoGrid_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewLicenseType)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataViewVerticalScroll DataGridViewLicenseType;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaxType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Values;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
    }
}
