namespace Fa.views.hms.helper
{
    partial class FormDataModification
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
            BtnExecute = new Button();
            LblDate = new Label();
            LblDateToday = new Label();
            BtnElemenateSales = new Button();
            BtnDeletePurchase = new Button();
            BtnSalesDelete = new Button();
            BtnPurchaseDelete = new Button();
            LblRecordCount = new Label();
            TxtBoxRecordCount = new TextBox();
            LblTimeTaken = new Label();
            LblTotRec = new Label();
            LblTotRecCount = new Label();
            BtnDelRemaingSales = new Button();
            SuspendLayout();
            // 
            // BtnExecute
            // 
            BtnExecute.Location = new Point(30, 86);
            BtnExecute.Name = "BtnExecute";
            BtnExecute.Size = new Size(251, 27);
            BtnExecute.TabIndex = 0;
            BtnExecute.Text = "Execute Delete all Data from All Tabel";
            BtnExecute.UseVisualStyleBackColor = true;
            BtnExecute.Click += BtnExecute_Click;
            // 
            // LblDate
            // 
            LblDate.AutoSize = true;
            LblDate.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point);
            LblDate.ForeColor = SystemColors.ControlDark;
            LblDate.Location = new Point(30, 26);
            LblDate.Name = "LblDate";
            LblDate.Size = new Size(52, 14);
            LblDate.TabIndex = 1;
            LblDate.Text = "Today :";
            // 
            // LblDateToday
            // 
            LblDateToday.AutoSize = true;
            LblDateToday.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point);
            LblDateToday.ForeColor = Color.FromArgb(255, 128, 0);
            LblDateToday.Location = new Point(82, 26);
            LblDateToday.Name = "LblDateToday";
            LblDateToday.Size = new Size(85, 14);
            LblDateToday.TabIndex = 2;
            LblDateToday.Text = "DD-MM-YYYY";
            // 
            // BtnElemenateSales
            // 
            BtnElemenateSales.Location = new Point(30, 130);
            BtnElemenateSales.Name = "BtnElemenateSales";
            BtnElemenateSales.Size = new Size(251, 27);
            BtnElemenateSales.TabIndex = 3;
            BtnElemenateSales.Text = "Execute Delete all Data from Sales Tabel";
            BtnElemenateSales.UseVisualStyleBackColor = true;
            BtnElemenateSales.Click += BtnElemenateSales_Click;
            // 
            // BtnDeletePurchase
            // 
            BtnDeletePurchase.Location = new Point(30, 173);
            BtnDeletePurchase.Name = "BtnDeletePurchase";
            BtnDeletePurchase.Size = new Size(251, 27);
            BtnDeletePurchase.TabIndex = 4;
            BtnDeletePurchase.Text = "Execute Delete all Data from Purchase Tabel";
            BtnDeletePurchase.UseVisualStyleBackColor = true;
            BtnDeletePurchase.Click += BtnDeletePurchase_Click;
            // 
            // BtnSalesDelete
            // 
            BtnSalesDelete.Location = new Point(305, 130);
            BtnSalesDelete.Name = "BtnSalesDelete";
            BtnSalesDelete.Size = new Size(202, 27);
            BtnSalesDelete.TabIndex = 5;
            BtnSalesDelete.Text = "Delete all Data from Sales Tabel";
            BtnSalesDelete.UseVisualStyleBackColor = true;
            BtnSalesDelete.Click += BtnSalesDelete_Click;
            // 
            // BtnPurchaseDelete
            // 
            BtnPurchaseDelete.Location = new Point(305, 173);
            BtnPurchaseDelete.Name = "BtnPurchaseDelete";
            BtnPurchaseDelete.Size = new Size(202, 27);
            BtnPurchaseDelete.TabIndex = 6;
            BtnPurchaseDelete.Text = "Delete all Data from Purchase Tabel";
            BtnPurchaseDelete.UseVisualStyleBackColor = true;
            BtnPurchaseDelete.Click += BtnPurchaseDelete_Click;
            // 
            // LblRecordCount
            // 
            LblRecordCount.AutoSize = true;
            LblRecordCount.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point);
            LblRecordCount.ForeColor = SystemColors.ControlDark;
            LblRecordCount.Location = new Point(30, 56);
            LblRecordCount.Name = "LblRecordCount";
            LblRecordCount.Size = new Size(130, 14);
            LblRecordCount.TabIndex = 7;
            LblRecordCount.Text = "Deleting Record #  :";
            // 
            // TxtBoxRecordCount
            // 
            TxtBoxRecordCount.Font = new Font("Tahoma", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            TxtBoxRecordCount.ForeColor = Color.Blue;
            TxtBoxRecordCount.Location = new Point(206, 52);
            TxtBoxRecordCount.Name = "TxtBoxRecordCount";
            TxtBoxRecordCount.Size = new Size(75, 23);
            TxtBoxRecordCount.TabIndex = 9;
            TxtBoxRecordCount.Text = "0";
            // 
            // LblTimeTaken
            // 
            LblTimeTaken.AutoSize = true;
            LblTimeTaken.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point);
            LblTimeTaken.ForeColor = Color.Green;
            LblTimeTaken.Location = new Point(305, 26);
            LblTimeTaken.Name = "LblTimeTaken";
            LblTimeTaken.Size = new Size(51, 14);
            LblTimeTaken.TabIndex = 10;
            LblTimeTaken.Text = "HH:MM";
            // 
            // LblTotRec
            // 
            LblTotRec.AutoSize = true;
            LblTotRec.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point);
            LblTotRec.ForeColor = SystemColors.ControlDark;
            LblTotRec.Location = new Point(305, 56);
            LblTotRec.Name = "LblTotRec";
            LblTotRec.Size = new Size(103, 14);
            LblTotRec.TabIndex = 12;
            LblTotRec.Text = "Total Records  :";
            LblTotRec.TextAlign = ContentAlignment.TopCenter;
            // 
            // LblTotRecCount
            // 
            LblTotRecCount.AutoSize = true;
            LblTotRecCount.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point);
            LblTotRecCount.ForeColor = Color.Fuchsia;
            LblTotRecCount.Location = new Point(456, 56);
            LblTotRecCount.Name = "LblTotRecCount";
            LblTotRecCount.Size = new Size(55, 14);
            LblTotRecCount.TabIndex = 13;
            LblTotRecCount.Text = "000000";
            // 
            // BtnDelRemaingSales
            // 
            BtnDelRemaingSales.Location = new Point(305, 86);
            BtnDelRemaingSales.Name = "BtnDelRemaingSales";
            BtnDelRemaingSales.Size = new Size(202, 27);
            BtnDelRemaingSales.TabIndex = 14;
            BtnDelRemaingSales.Text = "Delete remaining Data from Sales Tabel";
            BtnDelRemaingSales.UseVisualStyleBackColor = true;
            BtnDelRemaingSales.Click += BtnDelRemaingSales_Click;
            // 
            // FormDataModification
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(519, 215);
            Controls.Add(BtnDelRemaingSales);
            Controls.Add(LblTotRecCount);
            Controls.Add(LblTotRec);
            Controls.Add(LblTimeTaken);
            Controls.Add(TxtBoxRecordCount);
            Controls.Add(LblRecordCount);
            Controls.Add(BtnPurchaseDelete);
            Controls.Add(BtnSalesDelete);
            Controls.Add(BtnDeletePurchase);
            Controls.Add(BtnElemenateSales);
            Controls.Add(LblDateToday);
            Controls.Add(LblDate);
            Controls.Add(BtnExecute);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormDataModification";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Data Modification";
            Load += FormDataModification_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtnExecute;
        private Label LblDate;
        private Label LblDateToday;
        private Button BtnElemenateSales;
        private Button BtnDeletePurchase;
        private Button BtnSalesDelete;
        private Button BtnPurchaseDelete;
        private Label LblRecordCount;
        private TextBox TxtBoxRecordCount;
        private Label LblTimeTaken;
        private Label LblTotRec;
        private Label LblTotRecCount;
        private Button BtnDelRemaingSales;
    }
}