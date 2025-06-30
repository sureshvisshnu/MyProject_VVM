namespace fa.views.hms.patient
{
    partial class FormUploadDocumentWithPreview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUploadDocumentWithPreview));
            Syncfusion.Windows.Forms.PdfViewer.MessageBoxSettings messageBoxSettings1 = new Syncfusion.Windows.Forms.PdfViewer.MessageBoxSettings();
            Syncfusion.Windows.PdfViewer.PdfViewerPrinterSettings pdfViewerPrinterSettings1 = new Syncfusion.Windows.PdfViewer.PdfViewerPrinterSettings();
            Syncfusion.Windows.Forms.PdfViewer.TextSearchSettings textSearchSettings1 = new Syncfusion.Windows.Forms.PdfViewer.TextSearchSettings();
            PictureBoxPath = new PictureBox();
            BtnCancel = new Button();
            BtnSave = new Button();
            BtnChooseFile = new Button();
            statusStrip1 = new StatusStrip();
            ErrorMsg = new ToolStripStatusLabel();
            TextBoxDescription = new TextBox();
            LabelDescription = new Label();
            TextBoxFileName = new TextBox();
            label2 = new Label();
            label1 = new Label();
            LapTestPictureBox = new PictureBox();
            pdfDocumentView = new Syncfusion.Windows.Forms.PdfViewer.PdfDocumentView();
            DocBrowserPatient = new controls.DocBrowser();
            LabelPath = new Label();
            ((System.ComponentModel.ISupportInitialize)PictureBoxPath).BeginInit();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)LapTestPictureBox).BeginInit();
            SuspendLayout();
            // 
            // PictureBoxPath
            // 
            PictureBoxPath.BackgroundImage = (Image)resources.GetObject("PictureBoxPath.BackgroundImage");
            PictureBoxPath.BackgroundImageLayout = ImageLayout.Zoom;
            PictureBoxPath.Location = new Point(15, 49);
            PictureBoxPath.Name = "PictureBoxPath";
            PictureBoxPath.Size = new Size(23, 21);
            PictureBoxPath.TabIndex = 18;
            PictureBoxPath.TabStop = false;
            PictureBoxPath.Visible = false;
            // 
            // BtnCancel
            // 
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(339, 425);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(87, 23);
            BtnCancel.TabIndex = 17;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // BtnSave
            // 
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(432, 425);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(87, 23);
            BtnSave.TabIndex = 16;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // BtnChooseFile
            // 
            BtnChooseFile.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnChooseFile.Location = new Point(432, 22);
            BtnChooseFile.Name = "BtnChooseFile";
            BtnChooseFile.Size = new Size(104, 21);
            BtnChooseFile.TabIndex = 15;
            BtnChooseFile.Text = "Choose File [F2]";
            BtnChooseFile.UseVisualStyleBackColor = true;
            BtnChooseFile.Click += BtnChooseFile_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsg });
            statusStrip1.Location = new Point(0, 465);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(545, 22);
            statusStrip1.TabIndex = 14;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(25, 17);
            ErrorMsg.Text = "      ";
            // 
            // TextBoxDescription
            // 
            TextBoxDescription.Location = new Point(15, 334);
            TextBoxDescription.MaxLength = 500;
            TextBoxDescription.Multiline = true;
            TextBoxDescription.Name = "TextBoxDescription";
            TextBoxDescription.Size = new Size(521, 73);
            TextBoxDescription.TabIndex = 13;
            // 
            // LabelDescription
            // 
            LabelDescription.AutoSize = true;
            LabelDescription.Location = new Point(12, 318);
            LabelDescription.Name = "LabelDescription";
            LabelDescription.Size = new Size(67, 15);
            LabelDescription.TabIndex = 12;
            LabelDescription.Text = "Description";
            // 
            // TextBoxFileName
            // 
            TextBoxFileName.BackColor = Color.White;
            TextBoxFileName.Location = new Point(15, 23);
            TextBoxFileName.MaxLength = 50;
            TextBoxFileName.Name = "TextBoxFileName";
            TextBoxFileName.Size = new Size(411, 23);
            TextBoxFileName.TabIndex = 11;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(12, 7);
            label2.Name = "label2";
            label2.Size = new Size(61, 13);
            label2.TabIndex = 10;
            label2.Text = "File Name";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 77);
            label1.Name = "label1";
            label1.Size = new Size(48, 15);
            label1.TabIndex = 19;
            label1.Text = "Preview";
            // 
            // LapTestPictureBox
            // 
            LapTestPictureBox.BackColor = SystemColors.Control;
            LapTestPictureBox.BackgroundImageLayout = ImageLayout.Stretch;
            LapTestPictureBox.Location = new Point(15, 95);
            LapTestPictureBox.Name = "LapTestPictureBox";
            LapTestPictureBox.Size = new Size(521, 220);
            LapTestPictureBox.TabIndex = 20;
            LapTestPictureBox.TabStop = false;
            // 
            // pdfDocumentView
            // 
            pdfDocumentView.AutoScroll = true;
            pdfDocumentView.BackColor = Color.FromArgb(237, 237, 237);
            pdfDocumentView.BorderStyle = BorderStyle.FixedSingle;
            pdfDocumentView.CursorMode = Syncfusion.Windows.Forms.PdfViewer.PdfViewerCursorMode.SelectTool;
            pdfDocumentView.EnableContextMenu = true;
            pdfDocumentView.HorizontalScrollOffset = 0;
            pdfDocumentView.IsTextSearchEnabled = true;
            pdfDocumentView.IsTextSelectionEnabled = true;
            pdfDocumentView.Location = new Point(15, 95);
            messageBoxSettings1.EnableNotification = true;
            pdfDocumentView.MessageBoxSettings = messageBoxSettings1;
            pdfDocumentView.MinimumZoomPercentage = 50;
            pdfDocumentView.Name = "pdfDocumentView";
            pdfDocumentView.PageBorderThickness = 1;
            pdfViewerPrinterSettings1.Copies = 1;
            pdfViewerPrinterSettings1.PageOrientation = Syncfusion.Windows.PdfViewer.PdfViewerPrintOrientation.Auto;
            pdfViewerPrinterSettings1.PageSize = Syncfusion.Windows.PdfViewer.PdfViewerPrintSize.ActualSize;
            pdfViewerPrinterSettings1.PrintLocation = (PointF)resources.GetObject("pdfViewerPrinterSettings1.PrintLocation");
            pdfViewerPrinterSettings1.ShowPrintStatusDialog = true;
            pdfDocumentView.PrinterSettings = pdfViewerPrinterSettings1;
            pdfDocumentView.ReferencePath = null;
            pdfDocumentView.ScrollDisplacementValue = 0;
            pdfDocumentView.ShowHorizontalScrollBar = true;
            pdfDocumentView.ShowVerticalScrollBar = true;
            pdfDocumentView.Size = new Size(521, 220);
            pdfDocumentView.SpaceBetweenPages = 8;
            pdfDocumentView.TabIndex = 44;
            textSearchSettings1.CurrentInstanceColor = Color.FromArgb(127, 255, 171, 64);
            textSearchSettings1.HighlightAllInstance = true;
            textSearchSettings1.OtherInstanceColor = Color.FromArgb(127, 254, 255, 0);
            pdfDocumentView.TextSearchSettings = textSearchSettings1;
            pdfDocumentView.ThemeName = "Default";
            pdfDocumentView.VerticalScrollOffset = 0;
            pdfDocumentView.VisualStyle = Syncfusion.Windows.Forms.PdfViewer.VisualStyle.Default;
            pdfDocumentView.ZoomMode = Syncfusion.Windows.Forms.PdfViewer.ZoomMode.Default;
            // 
            // DocBrowserPatient
            // 
            DocBrowserPatient.BorderStyle = BorderStyle.FixedSingle;
            DocBrowserPatient.Location = new Point(15, 95);
            DocBrowserPatient.Name = "DocBrowserPatient";
            DocBrowserPatient.Size = new Size(521, 220);
            DocBrowserPatient.TabIndex = 45;
            // 
            // LabelPath
            // 
            LabelPath.AutoSize = true;
            LabelPath.Location = new Point(49, 53);
            LabelPath.Name = "LabelPath";
            LabelPath.Size = new Size(0, 15);
            LabelPath.TabIndex = 46;
            // 
            // FormUploadDocumentWithPreview
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(545, 487);
            Controls.Add(LabelPath);
            Controls.Add(DocBrowserPatient);
            Controls.Add(pdfDocumentView);
            Controls.Add(LapTestPictureBox);
            Controls.Add(label1);
            Controls.Add(PictureBoxPath);
            Controls.Add(BtnCancel);
            Controls.Add(BtnSave);
            Controls.Add(BtnChooseFile);
            Controls.Add(statusStrip1);
            Controls.Add(TextBoxDescription);
            Controls.Add(LabelDescription);
            Controls.Add(TextBoxFileName);
            Controls.Add(label2);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormUploadDocumentWithPreview";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Upload Document";
            Load += FormUploadDocumentWithPreview_Load;
            Controls.SetChildIndex(label2, 0);
            Controls.SetChildIndex(TextBoxFileName, 0);
            Controls.SetChildIndex(LabelDescription, 0);
            Controls.SetChildIndex(TextBoxDescription, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(BtnChooseFile, 0);
            Controls.SetChildIndex(BtnSave, 0);
            Controls.SetChildIndex(BtnCancel, 0);
            Controls.SetChildIndex(PictureBoxPath, 0);
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(LapTestPictureBox, 0);
            Controls.SetChildIndex(pdfDocumentView, 0);
            Controls.SetChildIndex(DocBrowserPatient, 0);
            Controls.SetChildIndex(LabelPath, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            ((System.ComponentModel.ISupportInitialize)PictureBoxPath).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)LapTestPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox PictureBoxPath;
        private Button BtnCancel;
        private Button BtnSave;
        private Button BtnChooseFile;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel ErrorMsg;
        private TextBox TextBoxDescription;
        private Label LabelDescription;
        private TextBox TextBoxFileName;
        private Label label2;
        private Label label1;
        private PictureBox LapTestPictureBox;
        private Syncfusion.Windows.Forms.PdfViewer.PdfDocumentView pdfDocumentView;
        private fa.views.controls.DocBrowser DocBrowserPatient;
        private Label LabelPath;
    }
}