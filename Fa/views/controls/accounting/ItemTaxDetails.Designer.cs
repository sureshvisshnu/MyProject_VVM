namespace fa.views.controls.accounting
{
    partial class ItemTaxDetails
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            GridViewProductTaxDetails = new DataViewVerticalScroll();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new grid.DataGridViewCurrencyColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            labeltitle = new Label();
            ((System.ComponentModel.ISupportInitialize)GridViewProductTaxDetails).BeginInit();
            SuspendLayout();
            // 
            // GridViewProductTaxDetails
            // 
            GridViewProductTaxDetails.AllowUserToAddRows = false;
            GridViewProductTaxDetails.AllowUserToDeleteRows = false;
            GridViewProductTaxDetails.AllowUserToResizeColumns = false;
            GridViewProductTaxDetails.AllowUserToResizeRows = false;
            GridViewProductTaxDetails.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewProductTaxDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewProductTaxDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GridViewProductTaxDetails.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn6, dataGridViewTextBoxColumn5 });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            GridViewProductTaxDetails.DefaultCellStyle = dataGridViewCellStyle3;
            GridViewProductTaxDetails.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewProductTaxDetails.EnableHeadersVisualStyles = false;
            GridViewProductTaxDetails.Location = new Point(4, 21);
            GridViewProductTaxDetails.Margin = new Padding(4, 3, 4, 3);
            GridViewProductTaxDetails.Name = "GridViewProductTaxDetails";
            GridViewProductTaxDetails.ReadOnly = true;
            GridViewProductTaxDetails.RowHeadersVisible = false;
            GridViewProductTaxDetails.ScrollBars = ScrollBars.Vertical;
            GridViewProductTaxDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewProductTaxDetails.ShowCellToolTips = false;
            GridViewProductTaxDetails.Size = new Size(253, 185);
            GridViewProductTaxDetails.TabIndex = 77;
            GridViewProductTaxDetails.DataError += GridViewProductTaxDetails_DataError;
            GridViewProductTaxDetails.SizeChanged += GridViewProductTaxDetails_SizeChanged;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewTextBoxColumn4.HeaderText = "Tax";
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            dataGridViewTextBoxColumn4.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn4.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewTextBoxColumn6.Currencylength = 4;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewTextBoxColumn6.HeaderText = "Percentage";
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.ReadOnly = true;
            dataGridViewTextBoxColumn6.Resizable = DataGridViewTriState.False;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "";
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.ReadOnly = true;
            dataGridViewTextBoxColumn5.Resizable = DataGridViewTriState.True;
            dataGridViewTextBoxColumn5.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn5.Visible = false;
            dataGridViewTextBoxColumn5.Width = 5;
            // 
            // labeltitle
            // 
            labeltitle.AutoSize = true;
            labeltitle.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            labeltitle.Location = new Point(1, 0);
            labeltitle.Margin = new Padding(4, 0, 4, 0);
            labeltitle.Name = "labeltitle";
            labeltitle.Size = new Size(60, 13);
            labeltitle.TabIndex = 78;
            labeltitle.Text = "Tax Details";
            // 
            // ItemTaxDetails
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(GridViewProductTaxDetails);
            Controls.Add(labeltitle);
            Margin = new Padding(4, 3, 4, 3);
            Name = "ItemTaxDetails";
            Size = new Size(261, 210);
            Load += ItemTaxDetails_Load;
            EnabledChanged += ItemTaxDetails_EnabledChanged;
            SizeChanged += ItemTaxDetails_SizeChanged;
            Enter += ItemTaxDetails_Enter;
            ((System.ComponentModel.ISupportInitialize)GridViewProductTaxDetails).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataViewVerticalScroll GridViewProductTaxDetails;
        private Label labeltitle;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private grid.DataGridViewCurrencyColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    }
}
