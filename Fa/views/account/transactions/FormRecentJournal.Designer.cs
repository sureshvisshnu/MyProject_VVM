namespace fa.views.account.transactions
{
    partial class FormRecentJournal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRecentJournal));
            BtnCancel = new Button();
            BtnSelect = new Button();
            statusStrip1 = new StatusStrip();
            ErrorMsg = new ToolStripStatusLabel();
            GridViewJournalRecent = new controls.DataViewVerticalScroll();
            Date = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            Debit = new DataGridViewTextBoxColumn();
            Column10 = new DataGridViewTextBoxColumn();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewJournalRecent).BeginInit();
            SuspendLayout();
            // 
            // BtnCancel
            // 
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(327, 258);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(88, 23);
            BtnCancel.TabIndex = 160;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // BtnSelect
            // 
            BtnSelect.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSelect.Location = new Point(421, 258);
            BtnSelect.Name = "BtnSelect";
            BtnSelect.Size = new Size(88, 23);
            BtnSelect.TabIndex = 159;
            BtnSelect.Text = "Select [F8]";
            BtnSelect.UseVisualStyleBackColor = true;
            BtnSelect.Click += BtnSelect_Click;
            BtnSelect.PreviewKeyDown += BtnSelect_PreviewKeyDown;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsg });
            statusStrip1.Location = new Point(0, 289);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(535, 22);
            statusStrip1.TabIndex = 161;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(49, 17);
            ErrorMsg.Text = "              ";
            // 
            // GridViewJournalRecent
            // 
            GridViewJournalRecent.AllowUserToAddRows = false;
            GridViewJournalRecent.AllowUserToDeleteRows = false;
            GridViewJournalRecent.AllowUserToResizeColumns = false;
            GridViewJournalRecent.AllowUserToResizeRows = false;
            GridViewJournalRecent.BackgroundColor = SystemColors.ControlLight;
            GridViewJournalRecent.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewJournalRecent.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewJournalRecent.ColumnHeadersHeight = 20;
            GridViewJournalRecent.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewJournalRecent.Columns.AddRange(new DataGridViewColumn[] { Date, Column1, Debit, Column10 });
            GridViewJournalRecent.EnableHeadersVisualStyles = false;
            GridViewJournalRecent.Location = new Point(7, 4);
            GridViewJournalRecent.MultiSelect = false;
            GridViewJournalRecent.Name = "GridViewJournalRecent";
            GridViewJournalRecent.ReadOnly = true;
            GridViewJournalRecent.RowHeadersVisible = false;
            GridViewJournalRecent.RowTemplate.Height = 20;
            GridViewJournalRecent.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewJournalRecent.ShowCellToolTips = false;
            GridViewJournalRecent.Size = new Size(521, 242);
            GridViewJournalRecent.TabIndex = 158;
            GridViewJournalRecent.TabStop = false;
            GridViewJournalRecent.CellDoubleClick += GridViewJournalRecent_CellDoubleClick;
            GridViewJournalRecent.KeyDown += GridViewJournalRecent_KeyDown;
            // 
            // Date
            // 
            Date.HeaderText = "Date";
            Date.Name = "Date";
            Date.ReadOnly = true;
            Date.Resizable = DataGridViewTriState.False;
            Date.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column1
            // 
            Column1.HeaderText = "Ref No";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 200;
            // 
            // Debit
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            Debit.DefaultCellStyle = dataGridViewCellStyle2;
            Debit.HeaderText = "Amount";
            Debit.Name = "Debit";
            Debit.ReadOnly = true;
            Debit.Resizable = DataGridViewTriState.False;
            Debit.SortMode = DataGridViewColumnSortMode.NotSortable;
            Debit.Width = 200;
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
            // FormRecentJournal
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(535, 311);
            Controls.Add(BtnCancel);
            Controls.Add(BtnSelect);
            Controls.Add(statusStrip1);
            Controls.Add(GridViewJournalRecent);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormRecentJournal";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Recent Journals";
            Load += FormRecentJournal_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewJournalRecent).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnSelect;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ErrorMsg;
        private controls.DataViewVerticalScroll GridViewJournalRecent;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private DataGridViewTextBoxColumn Debit;
    }
}