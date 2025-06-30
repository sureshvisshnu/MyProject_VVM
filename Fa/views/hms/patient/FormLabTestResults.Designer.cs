namespace fa.views.hms.patient
{
    partial class FormLabTestResults
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
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle13 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle14 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle15 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle16 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle17 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle18 = new DataGridViewCellStyle();
            Syncfusion.Windows.Forms.PdfViewer.MessageBoxSettings messageBoxSettings1 = new Syncfusion.Windows.Forms.PdfViewer.MessageBoxSettings();
            Syncfusion.Windows.PdfViewer.PdfViewerPrinterSettings pdfViewerPrinterSettings1 = new Syncfusion.Windows.PdfViewer.PdfViewerPrinterSettings();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLabTestResults));
            Syncfusion.Windows.Forms.PdfViewer.TextSearchSettings textSearchSettings1 = new Syncfusion.Windows.Forms.PdfViewer.TextSearchSettings();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            statusStrip1 = new StatusStrip();
            StatusLabelLabTestResultErrorMsg = new ToolStripStatusLabel();
            GridViewLabTestResultLabTestHistory = new controls.hms.LabTestHistory();
            TabControlLabtestResult = new TabControl();
            TabLabtestResultLabtestDetails = new TabPage();
            BtnLabTestResultLabTestPrintRequisition = new Button();
            BtnLabTestResultLabTestElementSave = new Button();
            BtnLabTestResultLabTestElementCancel = new Button();
            GridViewLabTestResultLabtestElementInfo = new controls.DataViewVerticalScroll();
            TabLabtestResultLabtestImageDocument = new TabPage();
            BtnLabTestResultLabTestImgSave = new Button();
            BtnLabTestResultLabTestImgCancel = new Button();
            GridViewLabTestResultLabTestImage = new controls.DataViewVerticalScroll();
            Column10 = new DataGridViewTextBoxColumn();
            Column11 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column12 = new DataGridViewTextBoxColumn();
            Column13 = new DataGridViewTextBoxColumn();
            Column30 = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            PictureBoxLabTestResultImage = new PictureBox();
            PdfDocumentViewLabtestDocument = new Syncfusion.Windows.Forms.PdfViewer.PdfDocumentView();
            DocBrowserLabtestDocument = new controls.DocBrowser();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn9 = new controls.grid.DataGridViewNameColumn();
            dataGridViewComboBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewCurrencyColumn1 = new DataGridViewComboBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewComboBoxColumn();
            ResultDescription = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            Column27 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn10 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            statusStrip1.SuspendLayout();
            TabControlLabtestResult.SuspendLayout();
            TabLabtestResultLabtestDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewLabTestResultLabtestElementInfo).BeginInit();
            TabLabtestResultLabtestImageDocument.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewLabTestResultLabTestImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PictureBoxLabTestResultImage).BeginInit();
            SuspendLayout();
            // 
            // PatientIdTransport
            // 
            PatientIdTransport.Size = new Size(116, 21);
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(59, 468);
            ProductIdTransport.Size = new Size(116, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(59, 438);
            ProductBatchIdTransport.Size = new Size(116, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(59, 408);
            AccountIdTransport.Size = new Size(116, 21);
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { StatusLabelLabTestResultErrorMsg });
            statusStrip1.Location = new Point(0, 507);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1161, 22);
            statusStrip1.TabIndex = 18;
            statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabelLabTestResultErrorMsg
            // 
            StatusLabelLabTestResultErrorMsg.Name = "StatusLabelLabTestResultErrorMsg";
            StatusLabelLabTestResultErrorMsg.Size = new Size(46, 17);
            StatusLabelLabTestResultErrorMsg.Text = "             ";
            // 
            // GridViewLabTestResultLabTestHistory
            // 
            GridViewLabTestResultLabTestHistory.HiddenNoteId = 0L;
            GridViewLabTestResultLabTestHistory.LoadAllLabTest = true;
            GridViewLabTestResultLabTestHistory.Location = new Point(2, 7);
            GridViewLabTestResultLabTestHistory.Margin = new Padding(4, 3, 4, 3);
            GridViewLabTestResultLabTestHistory.Name = "GridViewLabTestResultLabTestHistory";
            GridViewLabTestResultLabTestHistory.PatientId = 0L;
            GridViewLabTestResultLabTestHistory.Size = new Size(232, 498);
            GridViewLabTestResultLabTestHistory.TabIndex = 20;
            GridViewLabTestResultLabTestHistory.Load += GridViewLabTestHistory_Load;
            // 
            // TabControlLabtestResult
            // 
            TabControlLabtestResult.Controls.Add(TabLabtestResultLabtestDetails);
            TabControlLabtestResult.Controls.Add(TabLabtestResultLabtestImageDocument);
            TabControlLabtestResult.Location = new Point(241, 7);
            TabControlLabtestResult.Name = "TabControlLabtestResult";
            TabControlLabtestResult.SelectedIndex = 0;
            TabControlLabtestResult.Size = new Size(919, 498);
            TabControlLabtestResult.TabIndex = 21;
            // 
            // TabLabtestResultLabtestDetails
            // 
            TabLabtestResultLabtestDetails.Controls.Add(BtnLabTestResultLabTestPrintRequisition);
            TabLabtestResultLabtestDetails.Controls.Add(BtnLabTestResultLabTestElementSave);
            TabLabtestResultLabtestDetails.Controls.Add(BtnLabTestResultLabTestElementCancel);
            TabLabtestResultLabtestDetails.Controls.Add(GridViewLabTestResultLabtestElementInfo);
            TabLabtestResultLabtestDetails.Location = new Point(4, 22);
            TabLabtestResultLabtestDetails.Name = "TabLabtestResultLabtestDetails";
            TabLabtestResultLabtestDetails.Padding = new Padding(3);
            TabLabtestResultLabtestDetails.Size = new Size(911, 472);
            TabLabtestResultLabtestDetails.TabIndex = 0;
            TabLabtestResultLabtestDetails.Text = "Details";
            TabLabtestResultLabtestDetails.UseVisualStyleBackColor = true;
            // 
            // BtnLabTestResultLabTestPrintRequisition
            // 
            BtnLabTestResultLabTestPrintRequisition.Enabled = false;
            BtnLabTestResultLabTestPrintRequisition.Location = new Point(19, 435);
            BtnLabTestResultLabTestPrintRequisition.Name = "BtnLabTestResultLabTestPrintRequisition";
            BtnLabTestResultLabTestPrintRequisition.Size = new Size(119, 23);
            BtnLabTestResultLabTestPrintRequisition.TabIndex = 17;
            BtnLabTestResultLabTestPrintRequisition.Text = "Print Requisition [F9]";
            BtnLabTestResultLabTestPrintRequisition.UseVisualStyleBackColor = true;
            BtnLabTestResultLabTestPrintRequisition.Click += BtnLabTestPrintRequisition_Click;
            // 
            // BtnLabTestResultLabTestElementSave
            // 
            BtnLabTestResultLabTestElementSave.Location = new Point(799, 435);
            BtnLabTestResultLabTestElementSave.Name = "BtnLabTestResultLabTestElementSave";
            BtnLabTestResultLabTestElementSave.Size = new Size(75, 23);
            BtnLabTestResultLabTestElementSave.TabIndex = 16;
            BtnLabTestResultLabTestElementSave.Text = "Save [F8]";
            BtnLabTestResultLabTestElementSave.UseVisualStyleBackColor = true;
            BtnLabTestResultLabTestElementSave.Click += BtnSaveConsLabTestElement_Click;
            // 
            // BtnLabTestResultLabTestElementCancel
            // 
            BtnLabTestResultLabTestElementCancel.Location = new Point(718, 435);
            BtnLabTestResultLabTestElementCancel.Name = "BtnLabTestResultLabTestElementCancel";
            BtnLabTestResultLabTestElementCancel.Size = new Size(75, 23);
            BtnLabTestResultLabTestElementCancel.TabIndex = 17;
            BtnLabTestResultLabTestElementCancel.Text = "Cancel [Esc]";
            BtnLabTestResultLabTestElementCancel.UseVisualStyleBackColor = true;
            BtnLabTestResultLabTestElementCancel.Click += BtnCancelConsLabTestElement_Click;
            // 
            // GridViewLabTestResultLabtestElementInfo
            // 
            GridViewLabTestResultLabtestElementInfo.AllowUserToAddRows = false;
            GridViewLabTestResultLabtestElementInfo.AllowUserToDeleteRows = false;
            GridViewLabTestResultLabtestElementInfo.AllowUserToResizeColumns = false;
            GridViewLabTestResultLabtestElementInfo.AllowUserToResizeRows = false;
            GridViewLabTestResultLabtestElementInfo.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            GridViewLabTestResultLabtestElementInfo.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewLabTestResultLabtestElementInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewLabTestResultLabtestElementInfo.ColumnHeadersHeight = 20;
            GridViewLabTestResultLabtestElementInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewLabTestResultLabtestElementInfo.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn9, dataGridViewComboBoxColumn1, dataGridViewCurrencyColumn1, dataGridViewTextBoxColumn4, ResultDescription, dataGridViewTextBoxColumn5, Column27, dataGridViewTextBoxColumn6, dataGridViewTextBoxColumn10, Column4 });
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = SystemColors.Window;
            dataGridViewCellStyle11.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle11.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.True;
            GridViewLabTestResultLabtestElementInfo.DefaultCellStyle = dataGridViewCellStyle11;
            GridViewLabTestResultLabtestElementInfo.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewLabTestResultLabtestElementInfo.EnableHeadersVisualStyles = false;
            GridViewLabTestResultLabtestElementInfo.Location = new Point(7, 6);
            GridViewLabTestResultLabtestElementInfo.Name = "GridViewLabTestResultLabtestElementInfo";
            GridViewLabTestResultLabtestElementInfo.RowHeadersVisible = false;
            dataGridViewCellStyle12.BackColor = Color.White;
            dataGridViewCellStyle12.ForeColor = Color.Black;
            dataGridViewCellStyle12.SelectionBackColor = Color.White;
            dataGridViewCellStyle12.SelectionForeColor = Color.Black;
            dataGridViewCellStyle12.WrapMode = DataGridViewTriState.True;
            GridViewLabTestResultLabtestElementInfo.RowsDefaultCellStyle = dataGridViewCellStyle12;
            GridViewLabTestResultLabtestElementInfo.RowTemplate.Height = 20;
            GridViewLabTestResultLabtestElementInfo.ScrollBars = ScrollBars.Vertical;
            GridViewLabTestResultLabtestElementInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewLabTestResultLabtestElementInfo.ShowCellToolTips = false;
            GridViewLabTestResultLabtestElementInfo.Size = new Size(894, 415);
            GridViewLabTestResultLabtestElementInfo.TabIndex = 15;
            GridViewLabTestResultLabtestElementInfo.CellEndEdit += GridViewLabTestResultLabtestElementInfo_CellEndEdit;
            GridViewLabTestResultLabtestElementInfo.CellEnter += GridViewElementInfo_CellEnter;
            GridViewLabTestResultLabtestElementInfo.CellFormatting += GridViewLabTestResultLabtestElementInfo_CellFormatting;
            GridViewLabTestResultLabtestElementInfo.CellLeave += GridViewLabTestResultLabtestElementInfo_CellLeave;
            GridViewLabTestResultLabtestElementInfo.DataError += GridViewLabTestResultLabtestElementInfo_DataError;
            GridViewLabTestResultLabtestElementInfo.EditingControlShowing += GridViewLabTestResultLabtestElementInfo_EditingControlShowing;
            // 
            // TabLabtestResultLabtestImageDocument
            // 
            TabLabtestResultLabtestImageDocument.Controls.Add(BtnLabTestResultLabTestImgSave);
            TabLabtestResultLabtestImageDocument.Controls.Add(BtnLabTestResultLabTestImgCancel);
            TabLabtestResultLabtestImageDocument.Controls.Add(GridViewLabTestResultLabTestImage);
            TabLabtestResultLabtestImageDocument.Controls.Add(PictureBoxLabTestResultImage);
            TabLabtestResultLabtestImageDocument.Controls.Add(PdfDocumentViewLabtestDocument);
            TabLabtestResultLabtestImageDocument.Controls.Add(DocBrowserLabtestDocument);
            TabLabtestResultLabtestImageDocument.Location = new Point(4, 24);
            TabLabtestResultLabtestImageDocument.Name = "TabLabtestResultLabtestImageDocument";
            TabLabtestResultLabtestImageDocument.Size = new Size(911, 470);
            TabLabtestResultLabtestImageDocument.TabIndex = 1;
            TabLabtestResultLabtestImageDocument.Text = "Documents / Image";
            TabLabtestResultLabtestImageDocument.UseVisualStyleBackColor = true;
            // 
            // BtnLabTestResultLabTestImgSave
            // 
            BtnLabTestResultLabTestImgSave.Location = new Point(813, 437);
            BtnLabTestResultLabTestImgSave.Name = "BtnLabTestResultLabTestImgSave";
            BtnLabTestResultLabTestImgSave.Size = new Size(75, 23);
            BtnLabTestResultLabTestImgSave.TabIndex = 18;
            BtnLabTestResultLabTestImgSave.Text = "Save [F8]";
            BtnLabTestResultLabTestImgSave.UseVisualStyleBackColor = true;
            BtnLabTestResultLabTestImgSave.Click += BtnLabTestImgSave_Click;
            // 
            // BtnLabTestResultLabTestImgCancel
            // 
            BtnLabTestResultLabTestImgCancel.Location = new Point(732, 437);
            BtnLabTestResultLabTestImgCancel.Name = "BtnLabTestResultLabTestImgCancel";
            BtnLabTestResultLabTestImgCancel.Size = new Size(75, 23);
            BtnLabTestResultLabTestImgCancel.TabIndex = 19;
            BtnLabTestResultLabTestImgCancel.Text = "Cancel [Esc]";
            BtnLabTestResultLabTestImgCancel.UseVisualStyleBackColor = true;
            BtnLabTestResultLabTestImgCancel.Click += BtnLabTestImgCancel_Click;
            // 
            // GridViewLabTestResultLabTestImage
            // 
            GridViewLabTestResultLabTestImage.AllowUserToAddRows = false;
            GridViewLabTestResultLabTestImage.AllowUserToDeleteRows = false;
            GridViewLabTestResultLabTestImage.AllowUserToResizeColumns = false;
            GridViewLabTestResultLabTestImage.AllowUserToResizeRows = false;
            GridViewLabTestResultLabTestImage.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = SystemColors.Control;
            dataGridViewCellStyle13.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle13.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle13.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle13.WrapMode = DataGridViewTriState.True;
            GridViewLabTestResultLabTestImage.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            GridViewLabTestResultLabTestImage.ColumnHeadersHeight = 20;
            GridViewLabTestResultLabTestImage.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewLabTestResultLabTestImage.Columns.AddRange(new DataGridViewColumn[] { Column10, Column11, Column3, Column12, Column13, Column30, Column1, Column2 });
            GridViewLabTestResultLabTestImage.EnableHeadersVisualStyles = false;
            GridViewLabTestResultLabTestImage.GridColor = SystemColors.ButtonShadow;
            GridViewLabTestResultLabTestImage.Location = new Point(11, 13);
            GridViewLabTestResultLabTestImage.MultiSelect = false;
            GridViewLabTestResultLabTestImage.Name = "GridViewLabTestResultLabTestImage";
            GridViewLabTestResultLabTestImage.RowHeadersVisible = false;
            GridViewLabTestResultLabTestImage.RowTemplate.Height = 20;
            GridViewLabTestResultLabTestImage.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewLabTestResultLabTestImage.ShowCellToolTips = false;
            GridViewLabTestResultLabTestImage.Size = new Size(891, 98);
            GridViewLabTestResultLabTestImage.TabIndex = 16;
            GridViewLabTestResultLabTestImage.CellClick += GridViewLabTestImage_CellClick;
            GridViewLabTestResultLabTestImage.CellEnter += GridViewLabTestImage_CellEnter;
            GridViewLabTestResultLabTestImage.RowsAdded += GridViewLabTestImage_RowsAdded;
            // 
            // Column10
            // 
            dataGridViewCellStyle14.BackColor = Color.White;
            dataGridViewCellStyle14.ForeColor = Color.Black;
            dataGridViewCellStyle14.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = Color.White;
            Column10.DefaultCellStyle = dataGridViewCellStyle14;
            Column10.HeaderText = "#";
            Column10.Name = "Column10";
            Column10.Resizable = DataGridViewTriState.False;
            Column10.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column10.Width = 25;
            // 
            // Column11
            // 
            dataGridViewCellStyle15.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle15.BackColor = Color.White;
            dataGridViewCellStyle15.ForeColor = Color.Black;
            dataGridViewCellStyle15.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = Color.White;
            Column11.DefaultCellStyle = dataGridViewCellStyle15;
            Column11.HeaderText = "Name";
            Column11.Name = "Column11";
            Column11.Resizable = DataGridViewTriState.False;
            Column11.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column11.Width = 200;
            // 
            // Column3
            // 
            Column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle16.Alignment = DataGridViewContentAlignment.TopLeft;
            Column3.DefaultCellStyle = dataGridViewCellStyle16;
            Column3.HeaderText = "Description";
            Column3.Name = "Column3";
            Column3.Resizable = DataGridViewTriState.False;
            Column3.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column12
            // 
            dataGridViewCellStyle17.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle17.BackColor = Color.White;
            dataGridViewCellStyle17.ForeColor = Color.Black;
            dataGridViewCellStyle17.NullValue = "+";
            dataGridViewCellStyle17.SelectionBackColor = Color.White;
            dataGridViewCellStyle17.SelectionForeColor = Color.Black;
            Column12.DefaultCellStyle = dataGridViewCellStyle17;
            Column12.HeaderText = "  ";
            Column12.Name = "Column12";
            Column12.Resizable = DataGridViewTriState.False;
            Column12.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column12.Width = 25;
            // 
            // Column13
            // 
            dataGridViewCellStyle18.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle18.BackColor = Color.White;
            dataGridViewCellStyle18.ForeColor = Color.Black;
            dataGridViewCellStyle18.NullValue = "X";
            dataGridViewCellStyle18.SelectionBackColor = Color.White;
            dataGridViewCellStyle18.SelectionForeColor = Color.Black;
            Column13.DefaultCellStyle = dataGridViewCellStyle18;
            Column13.HeaderText = "  ";
            Column13.Name = "Column13";
            Column13.Resizable = DataGridViewTriState.False;
            Column13.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column13.Width = 25;
            // 
            // Column30
            // 
            Column30.HeaderText = "Path";
            Column30.Name = "Column30";
            Column30.Resizable = DataGridViewTriState.False;
            Column30.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column30.Visible = false;
            // 
            // Column1
            // 
            Column1.HeaderText = "Type";
            Column1.Name = "Column1";
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Visible = false;
            // 
            // Column2
            // 
            Column2.HeaderText = "Id";
            Column2.Name = "Column2";
            Column2.Resizable = DataGridViewTriState.False;
            Column2.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column2.Visible = false;
            // 
            // PictureBoxLabTestResultImage
            // 
            PictureBoxLabTestResultImage.BackColor = SystemColors.Control;
            PictureBoxLabTestResultImage.BackgroundImageLayout = ImageLayout.Stretch;
            PictureBoxLabTestResultImage.Location = new Point(11, 117);
            PictureBoxLabTestResultImage.Name = "PictureBoxLabTestResultImage";
            PictureBoxLabTestResultImage.Size = new Size(891, 313);
            PictureBoxLabTestResultImage.TabIndex = 1;
            PictureBoxLabTestResultImage.TabStop = false;
            // 
            // PdfDocumentViewLabtestDocument
            // 
            PdfDocumentViewLabtestDocument.AutoScroll = true;
            PdfDocumentViewLabtestDocument.BackColor = Color.FromArgb(237, 237, 237);
            PdfDocumentViewLabtestDocument.BorderStyle = BorderStyle.FixedSingle;
            PdfDocumentViewLabtestDocument.CursorMode = Syncfusion.Windows.Forms.PdfViewer.PdfViewerCursorMode.SelectTool;
            PdfDocumentViewLabtestDocument.EnableContextMenu = true;
            PdfDocumentViewLabtestDocument.HorizontalScrollOffset = 0;
            PdfDocumentViewLabtestDocument.IsTextSearchEnabled = true;
            PdfDocumentViewLabtestDocument.IsTextSelectionEnabled = true;
            PdfDocumentViewLabtestDocument.Location = new Point(15, 117);
            messageBoxSettings1.EnableNotification = true;
            PdfDocumentViewLabtestDocument.MessageBoxSettings = messageBoxSettings1;
            PdfDocumentViewLabtestDocument.MinimumZoomPercentage = 50;
            PdfDocumentViewLabtestDocument.Name = "PdfDocumentViewLabtestDocument";
            PdfDocumentViewLabtestDocument.PageBorderThickness = 1;
            pdfViewerPrinterSettings1.Copies = 1;
            pdfViewerPrinterSettings1.PageOrientation = Syncfusion.Windows.PdfViewer.PdfViewerPrintOrientation.Auto;
            pdfViewerPrinterSettings1.PageSize = Syncfusion.Windows.PdfViewer.PdfViewerPrintSize.ActualSize;
            pdfViewerPrinterSettings1.PrintLocation = (PointF)resources.GetObject("pdfViewerPrinterSettings1.PrintLocation");
            pdfViewerPrinterSettings1.ShowPrintStatusDialog = true;
            PdfDocumentViewLabtestDocument.PrinterSettings = pdfViewerPrinterSettings1;
            PdfDocumentViewLabtestDocument.ReferencePath = null;
            PdfDocumentViewLabtestDocument.ScrollDisplacementValue = 0;
            PdfDocumentViewLabtestDocument.ShowHorizontalScrollBar = true;
            PdfDocumentViewLabtestDocument.ShowVerticalScrollBar = true;
            PdfDocumentViewLabtestDocument.Size = new Size(858, 301);
            PdfDocumentViewLabtestDocument.SpaceBetweenPages = 8;
            PdfDocumentViewLabtestDocument.TabIndex = 43;
            textSearchSettings1.CurrentInstanceColor = Color.FromArgb(127, 255, 171, 64);
            textSearchSettings1.HighlightAllInstance = true;
            textSearchSettings1.OtherInstanceColor = Color.FromArgb(127, 254, 255, 0);
            PdfDocumentViewLabtestDocument.TextSearchSettings = textSearchSettings1;
            PdfDocumentViewLabtestDocument.ThemeName = "Default";
            PdfDocumentViewLabtestDocument.VerticalScrollOffset = 0;
            PdfDocumentViewLabtestDocument.VisualStyle = Syncfusion.Windows.Forms.PdfViewer.VisualStyle.Default;
            PdfDocumentViewLabtestDocument.ZoomMode = Syncfusion.Windows.Forms.PdfViewer.ZoomMode.Default;
            // 
            // DocBrowserLabtestDocument
            // 
            DocBrowserLabtestDocument.BorderStyle = BorderStyle.FixedSingle;
            DocBrowserLabtestDocument.Location = new Point(15, 117);
            DocBrowserLabtestDocument.Name = "DocBrowserLabtestDocument";
            DocBrowserLabtestDocument.Size = new Size(858, 301);
            DocBrowserLabtestDocument.TabIndex = 42;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewTextBoxColumn3.HeaderText = "#";
            dataGridViewTextBoxColumn3.MaxInputLength = 30;
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            dataGridViewTextBoxColumn3.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn3.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn3.Width = 25;
            // 
            // dataGridViewTextBoxColumn9
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewTextBoxColumn9.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewTextBoxColumn9.HeaderText = "Element Name";
            dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            dataGridViewTextBoxColumn9.ReadOnly = true;
            dataGridViewTextBoxColumn9.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn9.Width = 180;
            // 
            // dataGridViewComboBoxColumn1
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.SelectionBackColor = Color.White;
            dataGridViewCellStyle4.SelectionForeColor = Color.Black;
            dataGridViewComboBoxColumn1.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewComboBoxColumn1.HeaderText = "UOM";
            dataGridViewComboBoxColumn1.Name = "dataGridViewComboBoxColumn1";
            dataGridViewComboBoxColumn1.ReadOnly = true;
            dataGridViewComboBoxColumn1.Resizable = DataGridViewTriState.False;
            dataGridViewComboBoxColumn1.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewComboBoxColumn1.Width = 95;
            // 
            // dataGridViewCurrencyColumn1
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.Control;
            dataGridViewCellStyle5.ForeColor = Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle5.SelectionForeColor = Color.Black;
            dataGridViewCurrencyColumn1.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCurrencyColumn1.FlatStyle = FlatStyle.Flat;
            dataGridViewCurrencyColumn1.HeaderText = "Class";
            dataGridViewCurrencyColumn1.Name = "dataGridViewCurrencyColumn1";
            dataGridViewCurrencyColumn1.Resizable = DataGridViewTriState.False;
            dataGridViewCurrencyColumn1.Width = 120;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle6.BackColor = SystemColors.Control;
            dataGridViewCellStyle6.ForeColor = Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle6.SelectionForeColor = Color.Black;
            dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewTextBoxColumn4.FlatStyle = FlatStyle.Flat;
            dataGridViewTextBoxColumn4.HeaderText = "SubClass";
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn4.Width = 130;
            // 
            // ResultDescription
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.TopLeft;
            ResultDescription.DefaultCellStyle = dataGridViewCellStyle7;
            ResultDescription.HeaderText = "Result";
            ResultDescription.Name = "ResultDescription";
            ResultDescription.Resizable = DataGridViewTriState.False;
            ResultDescription.SortMode = DataGridViewColumnSortMode.NotSortable;
            ResultDescription.Width = 84;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle8.BackColor = SystemColors.Control;
            dataGridViewCellStyle8.ForeColor = Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle8.SelectionForeColor = Color.Black;
            dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewTextBoxColumn5.HeaderText = "Single Value";
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.ReadOnly = true;
            dataGridViewTextBoxColumn5.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn5.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn5.Width = 80;
            // 
            // Column27
            // 
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.TopRight;
            Column27.DefaultCellStyle = dataGridViewCellStyle9;
            Column27.HeaderText = "Range From";
            Column27.Name = "Column27";
            Column27.ReadOnly = true;
            Column27.Resizable = DataGridViewTriState.False;
            Column27.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column27.Width = 80;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle10.BackColor = SystemColors.Control;
            dataGridViewCellStyle10.ForeColor = Color.Black;
            dataGridViewCellStyle10.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle10.SelectionForeColor = Color.Black;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.True;
            dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewTextBoxColumn6.HeaderText = "Range To";
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.ReadOnly = true;
            dataGridViewTextBoxColumn6.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn6.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn6.Width = 80;
            // 
            // dataGridViewTextBoxColumn10
            // 
            dataGridViewTextBoxColumn10.HeaderText = "ID";
            dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            dataGridViewTextBoxColumn10.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn10.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn10.Visible = false;
            // 
            // Column4
            // 
            Column4.HeaderText = "TestID";
            Column4.Name = "Column4";
            Column4.Resizable = DataGridViewTriState.False;
            Column4.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column4.Visible = false;
            // 
            // FormLabTestResults
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1161, 529);
            Controls.Add(GridViewLabTestResultLabTestHistory);
            Controls.Add(TabControlLabtestResult);
            Controls.Add(statusStrip1);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormLabTestResults";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Lab Test Results";
            Load += FormLabTestResults_Load;
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(TabControlLabtestResult, 0);
            Controls.SetChildIndex(GridViewLabTestResultLabTestHistory, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            TabControlLabtestResult.ResumeLayout(false);
            TabLabtestResultLabtestDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)GridViewLabTestResultLabtestElementInfo).EndInit();
            TabLabtestResultLabtestImageDocument.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)GridViewLabTestResultLabTestImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)PictureBoxLabTestResultImage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel StatusLabelLabTestResultErrorMsg;
        private controls.hms.LabTestHistory GridViewLabTestResultLabTestHistory;
        private TabControl TabControlLabtestResult;
        private TabPage TabLabtestResultLabtestDetails;
        private Button BtnLabTestResultLabTestPrintRequisition;
        private Button BtnLabTestResultLabTestElementSave;
        private Button BtnLabTestResultLabTestElementCancel;
        private controls.DataViewVerticalScroll GridViewLabTestResultLabtestElementInfo;
        private TabPage TabLabtestResultLabtestImageDocument;
        private Button BtnLabTestResultLabTestImgSave;
        private Button BtnLabTestResultLabTestImgCancel;
        private PictureBox PictureBoxLabTestResultImage;
        private controls.DataViewVerticalScroll GridViewLabTestResultLabTestImage;
        private controls.DocBrowser DocBrowserLabtestDocument;
        private Syncfusion.Windows.Forms.PdfViewer.PdfDocumentView PdfDocumentViewLabtestDocument;
        private DataGridViewTextBoxColumn Column10;
        private DataGridViewTextBoxColumn Column11;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column12;
        private DataGridViewTextBoxColumn Column13;
        private DataGridViewTextBoxColumn Column30;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private controls.grid.DataGridViewNameColumn dataGridViewTextBoxColumn9;
        private DataGridViewTextBoxColumn dataGridViewComboBoxColumn1;
        private DataGridViewComboBoxColumn dataGridViewCurrencyColumn1;
        private DataGridViewComboBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn ResultDescription;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn Column27;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private DataGridViewTextBoxColumn Column4;
    }
}