namespace fa.views.controls.accounting
{
    partial class DiscountAdditinalChargeGrid
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
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            Header = new Label();
            GridView = new DataViewVerticalScroll();
            SNO = new DataGridViewTextBoxColumn();
            TransName = new DataGridViewComboBoxColumn();
            CustomName = new DataGridViewTextBoxColumn();
            TransType = new DataGridViewTextBoxColumn();
            TranValue = new DataGridViewTextBoxColumn();
            TypeAmount = new DataGridViewTextBoxColumn();
            CreditDebit = new DataGridViewTextBoxColumn();
            TranTotal = new DataGridViewTextBoxColumn();
            trandelete = new DataGridViewButtonColumn();
            oldindex = new DataGridViewTextBoxColumn();
            AccountId = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)GridView).BeginInit();
            SuspendLayout();
            // 
            // Header
            // 
            Header.AutoSize = true;
            Header.Location = new Point(1, 1);
            Header.Margin = new Padding(4, 0, 4, 0);
            Header.Name = "Header";
            Header.Size = new Size(157, 15);
            Header.TabIndex = 165;
            Header.Text = "Discount/Additonal Charges";
            // 
            // GridView
            // 
            GridView.AllowUserToDeleteRows = false;
            GridView.AllowUserToResizeColumns = false;
            GridView.AllowUserToResizeRows = false;
            GridView.BackgroundColor = SystemColors.Control;
            GridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GridView.Columns.AddRange(new DataGridViewColumn[] { SNO, TransName, CustomName, TransType, TranValue, TypeAmount, CreditDebit, TranTotal, trandelete, oldindex, AccountId });
            GridView.EditMode = DataGridViewEditMode.EditOnEnter;
            GridView.EnableHeadersVisualStyles = false;
            GridView.Location = new Point(0, 22);
            GridView.Margin = new Padding(4, 3, 4, 3);
            GridView.MultiSelect = false;
            GridView.Name = "GridView";
            GridView.RowHeadersVisible = false;
            dataGridViewCellStyle6.BackColor = Color.White;
            dataGridViewCellStyle6.ForeColor = Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = Color.White;
            dataGridViewCellStyle6.SelectionForeColor = Color.Black;
            GridView.RowsDefaultCellStyle = dataGridViewCellStyle6;
            GridView.RowTemplate.Height = 20;
            GridView.ScrollBars = ScrollBars.Vertical;
            GridView.ShowCellToolTips = false;
            GridView.Size = new Size(833, 156);
            GridView.TabIndex = 164;
            GridView.CellClick += GridView_CellClick;
            GridView.CellContentClick += GridView_CellContentClick;
            GridView.CellEndEdit += GridView_CellEndEdit;
            GridView.CellEnter += GridView_CellEnter;
            GridView.CellLeave += GridView_CellLeave;
            GridView.CellValueChanged += GridView_CellValueChanged;
            GridView.DataError += GridView_DataError;
            GridView.EditingControlShowing += GridView_EditingControlShowing;
            GridView.RowsAdded += GridView_RowsAdded;
            GridView.SizeChanged += GridView_SizeChanged;
            GridView.PreviewKeyDown += GridView_PreviewKeyDown;
            GridView.Resize += GridView_Resize;
            // 
            // SNO
            // 
            SNO.HeaderText = "#";
            SNO.Name = "SNO";
            SNO.Resizable = DataGridViewTriState.False;
            SNO.SortMode = DataGridViewColumnSortMode.NotSortable;
            SNO.Width = 25;
            // 
            // TransName
            // 
            TransName.AutoComplete = false;
            TransName.FlatStyle = FlatStyle.Flat;
            TransName.HeaderText = "Name";
            TransName.Name = "TransName";
            TransName.Resizable = DataGridViewTriState.False;
            // 
            // CustomName
            // 
            CustomName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            CustomName.HeaderText = "Custom Description";
            CustomName.MaxInputLength = 250;
            CustomName.Name = "CustomName";
            CustomName.Resizable = DataGridViewTriState.False;
            CustomName.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // TransType
            // 
            TransType.HeaderText = "Type";
            TransType.Name = "TransType";
            TransType.Resizable = DataGridViewTriState.False;
            TransType.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // TranValue
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.NullValue = "0.00";
            TranValue.DefaultCellStyle = dataGridViewCellStyle1;
            TranValue.HeaderText = "Value";
            TranValue.Name = "TranValue";
            TranValue.Resizable = DataGridViewTriState.False;
            TranValue.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // TypeAmount
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.NullValue = "0.00";
            TypeAmount.DefaultCellStyle = dataGridViewCellStyle2;
            TypeAmount.HeaderText = "Amount";
            TypeAmount.Name = "TypeAmount";
            TypeAmount.Resizable = DataGridViewTriState.False;
            TypeAmount.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // CreditDebit
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            CreditDebit.DefaultCellStyle = dataGridViewCellStyle3;
            CreditDebit.HeaderText = "+/-";
            CreditDebit.Name = "CreditDebit";
            CreditDebit.Resizable = DataGridViewTriState.False;
            CreditDebit.SortMode = DataGridViewColumnSortMode.NotSortable;
            CreditDebit.Width = 50;
            // 
            // TranTotal
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.NullValue = "0.00";
            TranTotal.DefaultCellStyle = dataGridViewCellStyle4;
            TranTotal.HeaderText = "Total";
            TranTotal.Name = "TranTotal";
            TranTotal.Resizable = DataGridViewTriState.False;
            TranTotal.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // trandelete
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.NullValue = "X";
            trandelete.DefaultCellStyle = dataGridViewCellStyle5;
            trandelete.HeaderText = " ";
            trandelete.Name = "trandelete";
            trandelete.Resizable = DataGridViewTriState.False;
            trandelete.Width = 25;
            // 
            // oldindex
            // 
            oldindex.HeaderText = "OldValue";
            oldindex.Name = "oldindex";
            oldindex.Resizable = DataGridViewTriState.False;
            oldindex.Visible = false;
            oldindex.Width = 5;
            // 
            // AccountId
            // 
            AccountId.HeaderText = "AccountId";
            AccountId.Name = "AccountId";
            AccountId.Visible = false;
            // 
            // DiscountAdditinalChargeGrid
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(Header);
            Controls.Add(GridView);
            Margin = new Padding(4, 3, 4, 3);
            Name = "DiscountAdditinalChargeGrid";
            Size = new Size(1066, 181);
            SizeChanged += DiscountAdditinalChargeGrid_SizeChanged;
            Enter += DiscountAdditinalChargeGrid_Enter;
            ((System.ComponentModel.ISupportInitialize)GridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Header;
        private DataViewVerticalScroll GridView;
        private DataGridViewTextBoxColumn SNO;
        private DataGridViewComboBoxColumn TransName;
        private DataGridViewTextBoxColumn CustomName;
        private DataGridViewTextBoxColumn TransType;
        private DataGridViewTextBoxColumn CreditDebit;
        private DataGridViewButtonColumn trandelete;
        private DataGridViewTextBoxColumn oldindex;
        private DataGridViewTextBoxColumn AccountId;
        private DataGridViewTextBoxColumn TranValue;
        private DataGridViewTextBoxColumn TypeAmount;
        private DataGridViewTextBoxColumn TranTotal;
    }
}
