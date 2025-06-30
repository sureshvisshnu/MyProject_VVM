namespace fa.views.hms.helper
{
    partial class FormSelectPrescriptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectPrescriptions));
            DataGridViewCellStyle dataGridViewCellStyle13 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle20 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle14 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle15 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle16 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle17 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle18 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle19 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            GridViewSelectPrescription = new controls.DataViewVerticalScroll();
            BtnSelectPrescriptionCancel = new Button();
            BtnSelectPrescriptionDone = new Button();
            SelectPrescriptionStatusStrip = new StatusStrip();
            ErrorMsgSelectPrescription = new ToolStripStatusLabel();
            BtnRefreshPrescription = new Button();
            PrescriptionSearchTextBox = new controls.text.DelayedTextChangeTextBox();
            BtnSelectPrescriptionAddCustomRow = new Button();
            GridViewSelectPrescriptionBatch = new controls.DataViewVerticalScroll();
            ItemName = new DataGridViewTextBoxColumn();
            PatientName = new DataGridViewTextBoxColumn();
            PatientAddress = new DataGridViewTextBoxColumn();
            Column2 = new controls.grid.DataGridViewQuantityColumn();
            uom = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            BatchMRP = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            PrescriptionListBox = new ListBox();
            Names = new DataGridViewTextBoxColumn();
            Total = new DataGridViewTextBoxColumn();
            Days = new DataGridViewTextBoxColumn();
            BeforeAfter = new DataGridViewComboBoxColumn();
            Interval = new DataGridViewComboBoxColumn();
            Morning = new DataGridViewTextBoxColumn();
            Afternoon = new DataGridViewTextBoxColumn();
            Evening = new DataGridViewTextBoxColumn();
            Night = new DataGridViewTextBoxColumn();
            Notes = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            pid = new DataGridViewTextBoxColumn();
            CUSTOM = new DataGridViewTextBoxColumn();
            id = new DataGridViewTextBoxColumn();
            PriscpName = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)GridViewSelectPrescription).BeginInit();
            SelectPrescriptionStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewSelectPrescriptionBatch).BeginInit();
            SuspendLayout();
            // 
            // PatientIdTransport
            // 
            PatientIdTransport.Location = new Point(12, 812);
            PatientIdTransport.Margin = new Padding(4, 5, 4, 5);
            PatientIdTransport.Size = new Size(100, 21);
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(42, 238);
            ProductIdTransport.Margin = new Padding(4, 5, 4, 5);
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(42, 207);
            ProductBatchIdTransport.Margin = new Padding(4, 5, 4, 5);
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(42, 176);
            AccountIdTransport.Margin = new Padding(4, 5, 4, 5);
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // GridViewSelectPrescription
            // 
            GridViewSelectPrescription.AllowUserToAddRows = false;
            GridViewSelectPrescription.AllowUserToDeleteRows = false;
            GridViewSelectPrescription.AllowUserToResizeColumns = false;
            GridViewSelectPrescription.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            GridViewSelectPrescription.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewSelectPrescription.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewSelectPrescription.ColumnHeadersHeight = 20;
            GridViewSelectPrescription.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewSelectPrescription.Columns.AddRange(new DataGridViewColumn[] { Names, Total, Days, BeforeAfter, Interval, Morning, Afternoon, Evening, Night, Notes, Column5, pid, CUSTOM, id, PriscpName });
            GridViewSelectPrescription.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewSelectPrescription.EnableHeadersVisualStyles = false;
            GridViewSelectPrescription.Location = new Point(233, 12);
            GridViewSelectPrescription.MultiSelect = false;
            GridViewSelectPrescription.Name = "GridViewSelectPrescription";
            GridViewSelectPrescription.RowHeadersVisible = false;
            GridViewSelectPrescription.RowHeadersWidth = 62;
            GridViewSelectPrescription.RowTemplate.Height = 20;
            GridViewSelectPrescription.ScrollBars = ScrollBars.Vertical;
            GridViewSelectPrescription.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewSelectPrescription.ShowCellToolTips = false;
            GridViewSelectPrescription.Size = new Size(888, 224);
            GridViewSelectPrescription.TabIndex = 2;
            GridViewSelectPrescription.CellClick += GridViewSelectPrescription_CellClick;
            GridViewSelectPrescription.CellEndEdit += GridViewSelectPrescription_CellEndEdit;
            GridViewSelectPrescription.CellEnter += GridViewSelectPrescription_CellEnter;
            GridViewSelectPrescription.CellFormatting += GridViewSelectPrescription_CellFormatting;
            GridViewSelectPrescription.DataError += GridViewSelectPrescription_DataError;
            GridViewSelectPrescription.EditingControlShowing += GridViewSelectPrescription_EditingControlShowing;
            GridViewSelectPrescription.RowsAdded += GridViewSelectPrescription_RowsAdded;
            GridViewSelectPrescription.SelectionChanged += GridViewSelectPrescription_SelectionChanged;
            GridViewSelectPrescription.Enter += GridViewSelectPrescription_Enter;
            // 
            // BtnSelectPrescriptionCancel
            // 
            BtnSelectPrescriptionCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSelectPrescriptionCancel.Location = new Point(902, 394);
            BtnSelectPrescriptionCancel.Name = "BtnSelectPrescriptionCancel";
            BtnSelectPrescriptionCancel.Size = new Size(93, 24);
            BtnSelectPrescriptionCancel.TabIndex = 4;
            BtnSelectPrescriptionCancel.Text = "Reset [Esc]";
            BtnSelectPrescriptionCancel.UseVisualStyleBackColor = true;
            BtnSelectPrescriptionCancel.Click += BtnSelectPrescriptionCancel_Click;
            // 
            // BtnSelectPrescriptionDone
            // 
            BtnSelectPrescriptionDone.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSelectPrescriptionDone.Location = new Point(1001, 394);
            BtnSelectPrescriptionDone.Name = "BtnSelectPrescriptionDone";
            BtnSelectPrescriptionDone.Size = new Size(93, 24);
            BtnSelectPrescriptionDone.TabIndex = 3;
            BtnSelectPrescriptionDone.Text = "Done [F8]";
            BtnSelectPrescriptionDone.UseVisualStyleBackColor = true;
            BtnSelectPrescriptionDone.Click += BtnSelectPrescriptionDone_Click;
            BtnSelectPrescriptionDone.PreviewKeyDown += BtnSelectPrescriptionDone_PreviewKeyDown;
            // 
            // SelectPrescriptionStatusStrip
            // 
            SelectPrescriptionStatusStrip.ImageScalingSize = new Size(24, 24);
            SelectPrescriptionStatusStrip.Items.AddRange(new ToolStripItem[] { ErrorMsgSelectPrescription });
            SelectPrescriptionStatusStrip.Location = new Point(0, 428);
            SelectPrescriptionStatusStrip.Name = "SelectPrescriptionStatusStrip";
            SelectPrescriptionStatusStrip.Size = new Size(1127, 22);
            SelectPrescriptionStatusStrip.TabIndex = 78;
            SelectPrescriptionStatusStrip.Text = "statusStrip1";
            // 
            // ErrorMsgSelectPrescription
            // 
            ErrorMsgSelectPrescription.Name = "ErrorMsgSelectPrescription";
            ErrorMsgSelectPrescription.Size = new Size(16, 17);
            ErrorMsgSelectPrescription.Text = "   ";
            // 
            // BtnRefreshPrescription
            // 
            BtnRefreshPrescription.BackgroundImage = (Image)resources.GetObject("BtnRefreshPrescription.BackgroundImage");
            BtnRefreshPrescription.BackgroundImageLayout = ImageLayout.Stretch;
            BtnRefreshPrescription.Image = (Image)resources.GetObject("BtnRefreshPrescription.Image");
            BtnRefreshPrescription.Location = new Point(204, 12);
            BtnRefreshPrescription.Name = "BtnRefreshPrescription";
            BtnRefreshPrescription.Size = new Size(23, 22);
            BtnRefreshPrescription.TabIndex = 79;
            BtnRefreshPrescription.TabStop = false;
            BtnRefreshPrescription.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnRefreshPrescription.UseVisualStyleBackColor = true;
            BtnRefreshPrescription.Click += BtnRefreshPrescription_Click;
            // 
            // PrescriptionSearchTextBox
            // 
            PrescriptionSearchTextBox.BorderStyle = BorderStyle.FixedSingle;
            PrescriptionSearchTextBox.Delay = false;
            PrescriptionSearchTextBox.DelayTime = 2000;
            PrescriptionSearchTextBox.Location = new Point(6, 12);
            PrescriptionSearchTextBox.MaxLength = 35;
            PrescriptionSearchTextBox.Name = "PrescriptionSearchTextBox";
            PrescriptionSearchTextBox.Searchstartfrom = 2;
            PrescriptionSearchTextBox.Size = new Size(192, 21);
            PrescriptionSearchTextBox.TabIndex = 0;
            PrescriptionSearchTextBox.TextChanged += PrescriptionSearchTextBox_TextChanged;
            // 
            // BtnSelectPrescriptionAddCustomRow
            // 
            BtnSelectPrescriptionAddCustomRow.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSelectPrescriptionAddCustomRow.Location = new Point(42, 394);
            BtnSelectPrescriptionAddCustomRow.Name = "BtnSelectPrescriptionAddCustomRow";
            BtnSelectPrescriptionAddCustomRow.Size = new Size(132, 24);
            BtnSelectPrescriptionAddCustomRow.TabIndex = 80;
            BtnSelectPrescriptionAddCustomRow.Text = "Add Custom Row";
            BtnSelectPrescriptionAddCustomRow.UseVisualStyleBackColor = true;
            BtnSelectPrescriptionAddCustomRow.Click += BtnSelectPrescriptionAddCustomRow_Click;
            // 
            // GridViewSelectPrescriptionBatch
            // 
            GridViewSelectPrescriptionBatch.AllowUserToAddRows = false;
            GridViewSelectPrescriptionBatch.AllowUserToDeleteRows = false;
            GridViewSelectPrescriptionBatch.AllowUserToResizeColumns = false;
            GridViewSelectPrescriptionBatch.AllowUserToResizeRows = false;
            GridViewSelectPrescriptionBatch.BackgroundColor = SystemColors.Control;
            GridViewSelectPrescriptionBatch.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = SystemColors.Control;
            dataGridViewCellStyle13.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle13.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle13.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle13.WrapMode = DataGridViewTriState.True;
            GridViewSelectPrescriptionBatch.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            GridViewSelectPrescriptionBatch.ColumnHeadersHeight = 20;
            GridViewSelectPrescriptionBatch.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewSelectPrescriptionBatch.Columns.AddRange(new DataGridViewColumn[] { ItemName, PatientName, PatientAddress, Column2, uom, Column3, BatchMRP, Column1 });
            dataGridViewCellStyle20.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = SystemColors.Window;
            dataGridViewCellStyle20.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle20.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle20.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle20.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle20.WrapMode = DataGridViewTriState.False;
            GridViewSelectPrescriptionBatch.DefaultCellStyle = dataGridViewCellStyle20;
            GridViewSelectPrescriptionBatch.EditMode = DataGridViewEditMode.EditProgrammatically;
            GridViewSelectPrescriptionBatch.EnableHeadersVisualStyles = false;
            GridViewSelectPrescriptionBatch.Location = new Point(233, 242);
            GridViewSelectPrescriptionBatch.MultiSelect = false;
            GridViewSelectPrescriptionBatch.Name = "GridViewSelectPrescriptionBatch";
            GridViewSelectPrescriptionBatch.ReadOnly = true;
            GridViewSelectPrescriptionBatch.RowHeadersVisible = false;
            GridViewSelectPrescriptionBatch.RowTemplate.Height = 20;
            GridViewSelectPrescriptionBatch.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewSelectPrescriptionBatch.ShowCellToolTips = false;
            GridViewSelectPrescriptionBatch.ShowEditingIcon = false;
            GridViewSelectPrescriptionBatch.Size = new Size(888, 139);
            GridViewSelectPrescriptionBatch.TabIndex = 81;
            // 
            // ItemName
            // 
            ItemName.HeaderText = "Item Name";
            ItemName.Name = "ItemName";
            ItemName.ReadOnly = true;
            ItemName.Resizable = DataGridViewTriState.False;
            ItemName.SortMode = DataGridViewColumnSortMode.NotSortable;
            ItemName.Width = 200;
            // 
            // PatientName
            // 
            dataGridViewCellStyle14.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle14.WrapMode = DataGridViewTriState.True;
            PatientName.DefaultCellStyle = dataGridViewCellStyle14;
            PatientName.HeaderText = "Batch No";
            PatientName.Name = "PatientName";
            PatientName.ReadOnly = true;
            PatientName.Resizable = DataGridViewTriState.False;
            PatientName.SortMode = DataGridViewColumnSortMode.NotSortable;
            PatientName.Width = 130;
            // 
            // PatientAddress
            // 
            dataGridViewCellStyle15.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle15.WrapMode = DataGridViewTriState.True;
            PatientAddress.DefaultCellStyle = dataGridViewCellStyle15;
            PatientAddress.HeaderText = "Exp Date";
            PatientAddress.Name = "PatientAddress";
            PatientAddress.ReadOnly = true;
            PatientAddress.Resizable = DataGridViewTriState.False;
            PatientAddress.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            dataGridViewCellStyle16.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle16.WrapMode = DataGridViewTriState.True;
            Column2.DefaultCellStyle = dataGridViewCellStyle16;
            Column2.HeaderText = "QTY";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Resizable = DataGridViewTriState.False;
            Column2.Width = 70;
            // 
            // uom
            // 
            dataGridViewCellStyle17.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle17.WrapMode = DataGridViewTriState.True;
            uom.DefaultCellStyle = dataGridViewCellStyle17;
            uom.HeaderText = "UOM";
            uom.Name = "uom";
            uom.ReadOnly = true;
            uom.Resizable = DataGridViewTriState.False;
            uom.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column3
            // 
            dataGridViewCellStyle18.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle18.WrapMode = DataGridViewTriState.True;
            Column3.DefaultCellStyle = dataGridViewCellStyle18;
            Column3.HeaderText = "Location";
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            Column3.Resizable = DataGridViewTriState.False;
            Column3.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column3.Width = 170;
            // 
            // BatchMRP
            // 
            dataGridViewCellStyle19.Alignment = DataGridViewContentAlignment.TopRight;
            BatchMRP.DefaultCellStyle = dataGridViewCellStyle19;
            BatchMRP.HeaderText = "MRP";
            BatchMRP.Name = "BatchMRP";
            BatchMRP.ReadOnly = true;
            BatchMRP.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column1
            // 
            Column1.HeaderText = "Id";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Visible = false;
            // 
            // PrescriptionListBox
            // 
            PrescriptionListBox.BorderStyle = BorderStyle.FixedSingle;
            PrescriptionListBox.FormattingEnabled = true;
            PrescriptionListBox.Location = new Point(6, 39);
            PrescriptionListBox.Name = "PrescriptionListBox";
            PrescriptionListBox.Size = new Size(221, 340);
            PrescriptionListBox.TabIndex = 1;
            PrescriptionListBox.SelectedIndexChanged += PrescriptionListBox_SelectedIndexChanged;
            PrescriptionListBox.DoubleClick += PrescriptionListBox_DoubleClick;
            // 
            // Names
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            Names.DefaultCellStyle = dataGridViewCellStyle2;
            Names.HeaderText = "Name";
            Names.MinimumWidth = 8;
            Names.Name = "Names";
            Names.Resizable = DataGridViewTriState.False;
            Names.SortMode = DataGridViewColumnSortMode.NotSortable;
            Names.Width = 180;
            // 
            // Total
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            Total.DefaultCellStyle = dataGridViewCellStyle3;
            Total.HeaderText = "Total";
            Total.MaxInputLength = 10;
            Total.MinimumWidth = 8;
            Total.Name = "Total";
            Total.Resizable = DataGridViewTriState.False;
            Total.SortMode = DataGridViewColumnSortMode.NotSortable;
            Total.Width = 75;
            // 
            // Days
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            Days.DefaultCellStyle = dataGridViewCellStyle4;
            Days.HeaderText = "Days";
            Days.MaxInputLength = 10;
            Days.MinimumWidth = 8;
            Days.Name = "Days";
            Days.Resizable = DataGridViewTriState.False;
            Days.SortMode = DataGridViewColumnSortMode.NotSortable;
            Days.Width = 80;
            // 
            // BeforeAfter
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            BeforeAfter.DefaultCellStyle = dataGridViewCellStyle5;
            BeforeAfter.FlatStyle = FlatStyle.Flat;
            BeforeAfter.HeaderText = "Before/After";
            BeforeAfter.MinimumWidth = 8;
            BeforeAfter.Name = "BeforeAfter";
            BeforeAfter.Resizable = DataGridViewTriState.False;
            BeforeAfter.Width = 75;
            // 
            // Interval
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            Interval.DefaultCellStyle = dataGridViewCellStyle6;
            Interval.FlatStyle = FlatStyle.Flat;
            Interval.HeaderText = "Interval";
            Interval.Items.AddRange(new object[] { " ", "Every 2", "Every 4", "Every 6", "Every 8", "Every 12", "Once a Day" });
            Interval.MinimumWidth = 8;
            Interval.Name = "Interval";
            Interval.Resizable = DataGridViewTriState.False;
            Interval.Width = 75;
            // 
            // Morning
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.TopCenter;
            Morning.DefaultCellStyle = dataGridViewCellStyle7;
            Morning.HeaderText = "Morning";
            Morning.MinimumWidth = 8;
            Morning.Name = "Morning";
            Morning.Resizable = DataGridViewTriState.False;
            Morning.SortMode = DataGridViewColumnSortMode.NotSortable;
            Morning.Width = 50;
            // 
            // Afternoon
            // 
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.TopCenter;
            Afternoon.DefaultCellStyle = dataGridViewCellStyle8;
            Afternoon.HeaderText = "Afternoon";
            Afternoon.MinimumWidth = 8;
            Afternoon.Name = "Afternoon";
            Afternoon.Resizable = DataGridViewTriState.False;
            Afternoon.SortMode = DataGridViewColumnSortMode.NotSortable;
            Afternoon.Width = 60;
            // 
            // Evening
            // 
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.TopCenter;
            Evening.DefaultCellStyle = dataGridViewCellStyle9;
            Evening.HeaderText = "Evening";
            Evening.MinimumWidth = 8;
            Evening.Name = "Evening";
            Evening.Resizable = DataGridViewTriState.False;
            Evening.SortMode = DataGridViewColumnSortMode.NotSortable;
            Evening.Width = 50;
            // 
            // Night
            // 
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.TopCenter;
            Night.DefaultCellStyle = dataGridViewCellStyle10;
            Night.HeaderText = "Night";
            Night.MinimumWidth = 8;
            Night.Name = "Night";
            Night.Resizable = DataGridViewTriState.False;
            Night.SortMode = DataGridViewColumnSortMode.NotSortable;
            Night.Width = 50;
            // 
            // Notes
            // 
            Notes.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.True;
            Notes.DefaultCellStyle = dataGridViewCellStyle11;
            Notes.HeaderText = "Additonal Notes";
            Notes.MaxInputLength = 200;
            Notes.MinimumWidth = 8;
            Notes.Name = "Notes";
            Notes.Resizable = DataGridViewTriState.False;
            Notes.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column5
            // 
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle12.BackColor = Color.White;
            dataGridViewCellStyle12.ForeColor = Color.Black;
            dataGridViewCellStyle12.SelectionBackColor = Color.White;
            dataGridViewCellStyle12.SelectionForeColor = Color.Black;
            Column5.DefaultCellStyle = dataGridViewCellStyle12;
            Column5.HeaderText = "...";
            Column5.MinimumWidth = 8;
            Column5.Name = "Column5";
            Column5.Resizable = DataGridViewTriState.False;
            Column5.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column5.Width = 25;
            // 
            // pid
            // 
            pid.HeaderText = "PID";
            pid.MinimumWidth = 8;
            pid.Name = "pid";
            pid.Resizable = DataGridViewTriState.False;
            pid.SortMode = DataGridViewColumnSortMode.NotSortable;
            pid.Visible = false;
            pid.Width = 150;
            // 
            // CUSTOM
            // 
            CUSTOM.HeaderText = "CUSTOM";
            CUSTOM.Name = "CUSTOM";
            CUSTOM.Visible = false;
            // 
            // id
            // 
            id.HeaderText = "ID";
            id.MinimumWidth = 8;
            id.Name = "id";
            id.Resizable = DataGridViewTriState.False;
            id.SortMode = DataGridViewColumnSortMode.NotSortable;
            id.Visible = false;
            id.Width = 150;
            // 
            // PriscpName
            // 
            PriscpName.HeaderText = "PriscpName";
            PriscpName.MinimumWidth = 8;
            PriscpName.Name = "PriscpName";
            PriscpName.Resizable = DataGridViewTriState.False;
            PriscpName.SortMode = DataGridViewColumnSortMode.NotSortable;
            PriscpName.Visible = false;
            PriscpName.Width = 150;
            // 
            // FormSelectPrescriptions
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1127, 450);
            Controls.Add(PrescriptionListBox);
            Controls.Add(GridViewSelectPrescriptionBatch);
            Controls.Add(BtnSelectPrescriptionAddCustomRow);
            Controls.Add(PrescriptionSearchTextBox);
            Controls.Add(BtnRefreshPrescription);
            Controls.Add(SelectPrescriptionStatusStrip);
            Controls.Add(BtnSelectPrescriptionCancel);
            Controls.Add(BtnSelectPrescriptionDone);
            Controls.Add(GridViewSelectPrescription);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(6, 8, 6, 8);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSelectPrescriptions";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select Prescriptions";
            Load += FormSelectPrescriptions_Load;
            Controls.SetChildIndex(GridViewSelectPrescription, 0);
            Controls.SetChildIndex(BtnSelectPrescriptionDone, 0);
            Controls.SetChildIndex(BtnSelectPrescriptionCancel, 0);
            Controls.SetChildIndex(SelectPrescriptionStatusStrip, 0);
            Controls.SetChildIndex(BtnRefreshPrescription, 0);
            Controls.SetChildIndex(PrescriptionSearchTextBox, 0);
            Controls.SetChildIndex(BtnSelectPrescriptionAddCustomRow, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(GridViewSelectPrescriptionBatch, 0);
            Controls.SetChildIndex(PrescriptionListBox, 0);
            ((System.ComponentModel.ISupportInitialize)GridViewSelectPrescription).EndInit();
            SelectPrescriptionStatusStrip.ResumeLayout(false);
            SelectPrescriptionStatusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewSelectPrescriptionBatch).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private controls.DataViewVerticalScroll GridViewSelectPrescription;
        private Button BtnSelectPrescriptionCancel;
        private Button BtnSelectPrescriptionDone;
        private StatusStrip SelectPrescriptionStatusStrip;
        private ToolStripStatusLabel ErrorMsgSelectPrescription;
        private Button BtnRefreshPrescription;
        private controls.text.DelayedTextChangeTextBox PrescriptionSearchTextBox;
        private Button BtnSelectPrescriptionAddCustomRow;
        private controls.DataViewVerticalScroll GridViewSelectPrescriptionBatch;
        private DataGridViewTextBoxColumn ItemName;
        private DataGridViewTextBoxColumn PatientName;
        private DataGridViewTextBoxColumn PatientAddress;
        private controls.grid.DataGridViewQuantityColumn Column2;
        private DataGridViewTextBoxColumn uom;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn BatchMRP;
        private DataGridViewTextBoxColumn Column1;
        private ListBox PrescriptionListBox;
        private DataGridViewTextBoxColumn Names;
        private DataGridViewTextBoxColumn Total;
        private DataGridViewTextBoxColumn Days;
        private DataGridViewComboBoxColumn BeforeAfter;
        private DataGridViewComboBoxColumn Interval;
        private DataGridViewTextBoxColumn Morning;
        private DataGridViewTextBoxColumn Afternoon;
        private DataGridViewTextBoxColumn Evening;
        private DataGridViewTextBoxColumn Night;
        private DataGridViewTextBoxColumn Notes;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn pid;
        private DataGridViewTextBoxColumn CUSTOM;
        private DataGridViewTextBoxColumn id;
        private DataGridViewTextBoxColumn PriscpName;
    }
}