namespace fa.views.utils
{
    partial class FormTransactionDate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTransactionDate));
            this.MonthCalendarTransactionDate = new System.Windows.Forms.MonthCalendar();
            this.BtnChangeTransactionDate = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MonthCalendarTransactionDate
            // 
            this.MonthCalendarTransactionDate.Location = new System.Drawing.Point(7, 9);
            this.MonthCalendarTransactionDate.Name = "MonthCalendarTransactionDate";
            this.MonthCalendarTransactionDate.TabIndex = 0;
            // 
            // BtnChangeTransactionDate
            // 
            this.BtnChangeTransactionDate.Location = new System.Drawing.Point(159, 183);
            this.BtnChangeTransactionDate.Name = "BtnChangeTransactionDate";
            this.BtnChangeTransactionDate.Size = new System.Drawing.Size(75, 23);
            this.BtnChangeTransactionDate.TabIndex = 1;
            this.BtnChangeTransactionDate.Text = "Change";
            this.BtnChangeTransactionDate.UseVisualStyleBackColor = true;
            this.BtnChangeTransactionDate.Click += new System.EventHandler(this.BtnChangeTransactionDate_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(74, 183);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 2;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // FormTransactionDate
            // 
            this.AcceptButton = this.BtnChangeTransactionDate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnCancel;
            this.ClientSize = new System.Drawing.Size(243, 217);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnChangeTransactionDate);
            this.Controls.Add(this.MonthCalendarTransactionDate);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTransactionDate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transaction Date";
            this.Load += new System.EventHandler(this.FormTransactionDate_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MonthCalendar MonthCalendarTransactionDate;
        private System.Windows.Forms.Button BtnChangeTransactionDate;
        private System.Windows.Forms.Button BtnCancel;
    }
}