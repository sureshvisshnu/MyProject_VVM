using System.Windows.Forms;

namespace fa.views.inventory
{
    partial class FormRecentStockMovement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRecentStockMovement));
            BtnCancel = new Button();
            BtnSelect = new Button();
            statusStrip1 = new StatusStrip();
            RecentStockMovementErrorMsg = new ToolStripStatusLabel();
            GridViewStockMovementRecentInvoice = new controls.DataViewVerticalScroll();
            Date = new DataGridViewTextBoxColumn();
            Supplier = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            Amount = new DataGridViewTextBoxColumn();
            Column10 = new DataGridViewTextBoxColumn();
            GridViewStockAdjustment = new controls.DataViewVerticalScroll();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            ToLoaction = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewStockMovementRecentInvoice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)GridViewStockAdjustment).BeginInit();
            SuspendLayout();
            // 
            // BtnCancel
            // 
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(343, 226);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(88, 23);
            BtnCancel.TabIndex = 152;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // BtnSelect
            // 
            BtnSelect.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSelect.Location = new Point(436, 226);
            BtnSelect.Name = "BtnSelect";
            BtnSelect.Size = new Size(88, 23);
            BtnSelect.TabIndex = 151;
            BtnSelect.Text = "Select [F8]";
            BtnSelect.UseVisualStyleBackColor = true;
            BtnSelect.Click += BtnSelect_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { RecentStockMovementErrorMsg });
            statusStrip1.Location = new Point(0, 261);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(534, 22);
            statusStrip1.TabIndex = 153;
            statusStrip1.Text = "statusStrip1";
            // 
            // RecentStockMovementErrorMsg
            // 
            RecentStockMovementErrorMsg.Name = "RecentStockMovementErrorMsg";
            RecentStockMovementErrorMsg.Size = new Size(49, 17);
            RecentStockMovementErrorMsg.Text = "              ";
            // 
            // GridViewStockMovementRecentInvoice
            // 
            GridViewStockMovementRecentInvoice.AllowUserToAddRows = false;
            GridViewStockMovementRecentInvoice.AllowUserToDeleteRows = false;
            GridViewStockMovementRecentInvoice.AllowUserToResizeColumns = false;
            GridViewStockMovementRecentInvoice.AllowUserToResizeRows = false;
            GridViewStockMovementRecentInvoice.BackgroundColor = SystemColors.Control;
            GridViewStockMovementRecentInvoice.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewStockMovementRecentInvoice.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewStockMovementRecentInvoice.ColumnHeadersHeight = 20;
            GridViewStockMovementRecentInvoice.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewStockMovementRecentInvoice.Columns.AddRange(new DataGridViewColumn[] { Date, Supplier, Column1, Amount, Column10 });
            GridViewStockMovementRecentInvoice.EnableHeadersVisualStyles = false;
            GridViewStockMovementRecentInvoice.Location = new Point(7, 2);
            GridViewStockMovementRecentInvoice.MultiSelect = false;
            GridViewStockMovementRecentInvoice.Name = "GridViewStockMovementRecentInvoice";
            GridViewStockMovementRecentInvoice.ReadOnly = true;
            GridViewStockMovementRecentInvoice.RowHeadersVisible = false;
            GridViewStockMovementRecentInvoice.RowTemplate.Height = 20;
            GridViewStockMovementRecentInvoice.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewStockMovementRecentInvoice.ShowCellToolTips = false;
            GridViewStockMovementRecentInvoice.Size = new Size(521, 219);
            GridViewStockMovementRecentInvoice.TabIndex = 150;
            GridViewStockMovementRecentInvoice.TabStop = false;
            GridViewStockMovementRecentInvoice.CellDoubleClick += GridViewStockMovementRecentInvoice_CellDoubleClick;
            GridViewStockMovementRecentInvoice.KeyDown += GridViewStockMovementRecentInvoice_KeyDown;
            // 
            // Date
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            Date.DefaultCellStyle = dataGridViewCellStyle2;
            Date.HeaderText = "Date";
            Date.Name = "Date";
            Date.ReadOnly = true;
            Date.Resizable = DataGridViewTriState.False;
            Date.SortMode = DataGridViewColumnSortMode.NotSortable;
            Date.Width = 75;
            // 
            // Supplier
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            Supplier.DefaultCellStyle = dataGridViewCellStyle3;
            Supplier.HeaderText = "From Location";
            Supplier.Name = "Supplier";
            Supplier.ReadOnly = true;
            Supplier.Resizable = DataGridViewTriState.False;
            Supplier.SortMode = DataGridViewColumnSortMode.NotSortable;
            Supplier.Width = 200;
            // 
            // Column1
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            Column1.DefaultCellStyle = dataGridViewCellStyle4;
            Column1.HeaderText = "To Location";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 125;
            // 
            // Amount
            // 
            Amount.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            Amount.DefaultCellStyle = dataGridViewCellStyle5;
            Amount.HeaderText = "Ref No";
            Amount.Name = "Amount";
            Amount.ReadOnly = true;
            Amount.Resizable = DataGridViewTriState.False;
            Amount.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column10
            // 
            Column10.HeaderText = "Id";
            Column10.Name = "Column10";
            Column10.ReadOnly = true;
            Column10.Resizable = DataGridViewTriState.False;
            Column10.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column10.Visible = false;
            // 
            // GridViewStockAdjustment
            // 
            GridViewStockAdjustment.AllowUserToAddRows = false;
            GridViewStockAdjustment.AllowUserToDeleteRows = false;
            GridViewStockAdjustment.AllowUserToResizeColumns = false;
            GridViewStockAdjustment.AllowUserToResizeRows = false;
            GridViewStockAdjustment.BackgroundColor = SystemColors.Control;
            GridViewStockAdjustment.BorderStyle = BorderStyle.Fixed3D;
            GridViewStockAdjustment.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewStockAdjustment.ColumnHeadersHeight = 20;
            GridViewStockAdjustment.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewStockAdjustment.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, ToLoaction, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5 });
            GridViewStockAdjustment.EnableHeadersVisualStyles = false;
            GridViewStockAdjustment.Location = new Point(7, 2);
            GridViewStockAdjustment.MultiSelect = false;
            GridViewStockAdjustment.Name = "GridViewStockAdjustment";
            GridViewStockAdjustment.ReadOnly = true;
            GridViewStockAdjustment.RowHeadersVisible = false;
            GridViewStockAdjustment.RowTemplate.Height = 20;
            GridViewStockAdjustment.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewStockAdjustment.ShowCellToolTips = false;
            GridViewStockAdjustment.Size = new Size(521, 219);
            GridViewStockAdjustment.TabIndex = 154;
            GridViewStockAdjustment.TabStop = false;
            GridViewStockAdjustment.CellDoubleClick += GridViewStockMovementRecentInvoice_CellDoubleClick;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewTextBoxColumn1.HeaderText = "Date";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            dataGridViewTextBoxColumn1.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn1.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn1.Width = 80;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewTextBoxColumn2.HeaderText = "Location";
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            dataGridViewTextBoxColumn2.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn2.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn2.Width = 270;
            // 
            // ToLoaction
            // 
            ToLoaction.HeaderText = "ToLoaction";
            ToLoaction.Name = "ToLoaction";
            ToLoaction.ReadOnly = true;
            ToLoaction.Visible = false;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewTextBoxColumn4.HeaderText = "Ref No";
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            dataGridViewTextBoxColumn4.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn4.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "Id";
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.ReadOnly = true;
            dataGridViewTextBoxColumn5.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn5.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn5.Visible = false;
            // 
            // FormRecentStockMovement
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(534, 283);
            Controls.Add(BtnCancel);
            Controls.Add(BtnSelect);
            Controls.Add(statusStrip1);
            Controls.Add(GridViewStockMovementRecentInvoice);
            Controls.Add(GridViewStockAdjustment);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormRecentStockMovement";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Recent Stock Movement";
            Load += FormRecentStockMovement_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewStockMovementRecentInvoice).EndInit();
            ((System.ComponentModel.ISupportInitialize)GridViewStockAdjustment).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnSelect;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel RecentStockMovementErrorMsg;
        private controls.DataViewVerticalScroll GridViewStockMovementRecentInvoice;
        private controls.DataViewVerticalScroll GridViewStockAdjustment;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ToLoaction;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn Date;
        private DataGridViewTextBoxColumn Supplier;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Amount;
        private DataGridViewTextBoxColumn Column10;
    }
}