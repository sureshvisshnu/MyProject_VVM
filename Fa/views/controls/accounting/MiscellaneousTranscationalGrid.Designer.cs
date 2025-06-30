namespace fa.views.controls.accounting
{
    partial class MiscellaneousTranscationalGrid
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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            Header = new Label();
            GridView = new DataViewVerticalScroll();
            PurchaseMissChargeName = new grid.DataGridViewNameColumn();
            TranDisplayName = new DataGridViewTextBoxColumn();
            Account = new DataGridViewComboBoxColumn();
            Type = new DataGridViewComboBoxColumn();
            TransactionType = new DataGridViewComboBoxColumn();
            Close = new DataGridViewButtonColumn();
            Id = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)GridView).BeginInit();
            SuspendLayout();
            // 
            // Header
            // 
            Header.AutoSize = true;
            Header.Location = new Point(-3, 5);
            Header.Name = "Header";
            Header.Size = new Size(169, 13);
            Header.TabIndex = 43;
            Header.Text = "Miscellaneous Transcational Items";
            // 
            // GridView
            // 
            GridView.AllowUserToDeleteRows = false;
            GridView.AllowUserToResizeColumns = false;
            GridView.AllowUserToResizeRows = false;
            GridView.BackgroundColor = SystemColors.Control;
            GridView.ColumnHeadersHeight = 20;
            GridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridView.Columns.AddRange(new DataGridViewColumn[] { PurchaseMissChargeName, TranDisplayName, Account, Type, TransactionType, Close, Id });
            GridView.EditMode = DataGridViewEditMode.EditOnEnter;
            GridView.EnableHeadersVisualStyles = false;
            GridView.Location = new Point(0, 23);
            GridView.MultiSelect = false;
            GridView.Name = "GridView";
            GridView.RowHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = Color.White;
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            GridView.RowsDefaultCellStyle = dataGridViewCellStyle2;
            GridView.RowTemplate.Height = 20;
            GridView.ScrollBars = ScrollBars.Vertical;
            GridView.ShowCellToolTips = false;
            GridView.Size = new Size(746, 216);
            GridView.TabIndex = 44;
            GridView.CellClick += GridView_CellClick;
            GridView.CellEnter += GridView_CellEnter;
            GridView.CellLeave += GridView_CellLeave;
            GridView.DataError += GridView_DataError;
            GridView.EditingControlShowing += GridView_EditingControlShowing;
            GridView.RowsAdded += GridView_RowsAdded;
            GridView.PreviewKeyDown += GridView_PreviewKeyDown;
            // 
            // PurchaseMissChargeName
            // 
            PurchaseMissChargeName.HeaderText = "Name";
            PurchaseMissChargeName.Name = "PurchaseMissChargeName";
            PurchaseMissChargeName.NameLength = 25;
            PurchaseMissChargeName.Resizable = DataGridViewTriState.True;
            PurchaseMissChargeName.Width = 200;
            // 
            // TranDisplayName
            // 
            TranDisplayName.HeaderText = "Display Name";
            TranDisplayName.MaxInputLength = 25;
            TranDisplayName.Name = "TranDisplayName";
            TranDisplayName.SortMode = DataGridViewColumnSortMode.NotSortable;
            TranDisplayName.Width = 200;
            // 
            // Account
            // 
            Account.FlatStyle = FlatStyle.Flat;
            Account.HeaderText = "Account";
            Account.Name = "Account";
            // 
            // Type
            // 
            Type.FlatStyle = FlatStyle.Flat;
            Type.HeaderText = "Transaction Type";
            Type.Items.AddRange(new object[] { "Percent", "Value" });
            Type.Name = "Type";
            // 
            // TransactionType
            // 
            TransactionType.FlatStyle = FlatStyle.Flat;
            TransactionType.HeaderText = "Transaction Action";
            TransactionType.Items.AddRange(new object[] { "Credit", "Debit" });
            TransactionType.Name = "TransactionType";
            // 
            // Close
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "X";
            Close.DefaultCellStyle = dataGridViewCellStyle1;
            Close.HeaderText = "...";
            Close.Name = "Close";
            Close.Resizable = DataGridViewTriState.True;
            Close.Width = 25;
            // 
            // Id
            // 
            Id.HeaderText = "Column1";
            Id.Name = "Id";
            Id.Visible = false;
            // 
            // MiscellaneousTranscationalGrid
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(Header);
            Controls.Add(GridView);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "MiscellaneousTranscationalGrid";
            Size = new Size(753, 244);
            SizeChanged += MiscellaneousTranscationalGrid_SizeChanged;
            Enter += MiscellaneousTranscationalGrid_Enter;
            Resize += MiscellaneousTranscationalGrid_Resize;
            ((System.ComponentModel.ISupportInitialize)GridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label Header;
        private DataViewVerticalScroll GridView;
        private grid.DataGridViewNameColumn PurchaseMissChargeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TranDisplayName;
        private System.Windows.Forms.DataGridViewComboBoxColumn Account;
        private System.Windows.Forms.DataGridViewComboBoxColumn Type;
        private System.Windows.Forms.DataGridViewComboBoxColumn TransactionType;
        private System.Windows.Forms.DataGridViewButtonColumn Close;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
    }
}
