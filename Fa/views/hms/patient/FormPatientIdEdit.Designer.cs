namespace fa.views.hms.patient
{
    partial class FormPatientIdEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPatientIdEdit));
            this.label1 = new System.Windows.Forms.Label();
            this.TextBoxPatientId = new System.Windows.Forms.TextBox();
            this.SaveButnPatientId = new System.Windows.Forms.Button();
            this.CancelButnPatientId = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PatientIdTransport
            // 
            this.PatientIdTransport.Location = new System.Drawing.Point(12, 812);
            this.PatientIdTransport.Size = new System.Drawing.Size(100, 21);
            // 
            // ProductIdTransport
            // 
            this.ProductIdTransport.Location = new System.Drawing.Point(172, 215);
            this.ProductIdTransport.Size = new System.Drawing.Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            this.ProductBatchIdTransport.Location = new System.Drawing.Point(172, 189);
            this.ProductBatchIdTransport.Size = new System.Drawing.Size(100, 21);
            // 
            // AccountIdTransport
            // 
            this.AccountIdTransport.Location = new System.Drawing.Point(172, 163);
            this.AccountIdTransport.Size = new System.Drawing.Size(100, 21);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Patient Id";
            // 
            // TextBoxPatientId
            // 
            this.TextBoxPatientId.Location = new System.Drawing.Point(15, 28);
            this.TextBoxPatientId.Name = "TextBoxPatientId";
            this.TextBoxPatientId.Size = new System.Drawing.Size(176, 21);
            this.TextBoxPatientId.TabIndex = 1;
            // 
            // SaveButnPatientId
            // 
            this.SaveButnPatientId.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.SaveButnPatientId.Location = new System.Drawing.Point(22, 74);
            this.SaveButnPatientId.Name = "SaveButnPatientId";
            this.SaveButnPatientId.Size = new System.Drawing.Size(83, 23);
            this.SaveButnPatientId.TabIndex = 2;
            this.SaveButnPatientId.Text = "Save [F8]";
            this.SaveButnPatientId.UseVisualStyleBackColor = true;
            this.SaveButnPatientId.Click += new System.EventHandler(this.SaveButnPatientId_Click);
            // 
            // CancelButnPatientId
            // 
            this.CancelButnPatientId.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.CancelButnPatientId.Location = new System.Drawing.Point(108, 74);
            this.CancelButnPatientId.Name = "CancelButnPatientId";
            this.CancelButnPatientId.Size = new System.Drawing.Size(83, 23);
            this.CancelButnPatientId.TabIndex = 3;
            this.CancelButnPatientId.Text = "Cancel [Esc]";
            this.CancelButnPatientId.UseVisualStyleBackColor = true;
            this.CancelButnPatientId.Click += new System.EventHandler(this.CancelButnPatientId_Click);
            // 
            // FormPatientIdEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(207, 109);
            this.Controls.Add(this.CancelButnPatientId);
            this.Controls.Add(this.SaveButnPatientId);
            this.Controls.Add(this.TextBoxPatientId);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPatientIdEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Patient Id";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.TextBoxPatientId, 0);
            this.Controls.SetChildIndex(this.SaveButnPatientId, 0);
            this.Controls.SetChildIndex(this.CancelButnPatientId, 0);
            this.Controls.SetChildIndex(this.PatientIdTransport, 0);
            this.Controls.SetChildIndex(this.ProductIdTransport, 0);
            this.Controls.SetChildIndex(this.ProductBatchIdTransport, 0);
            this.Controls.SetChildIndex(this.AccountIdTransport, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBoxPatientId;
        private System.Windows.Forms.Button SaveButnPatientId;
        private System.Windows.Forms.Button CancelButnPatientId;
    }
}