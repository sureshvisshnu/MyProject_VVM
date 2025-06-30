namespace fa.views.controls
{
    partial class QRCodeControl
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
            this.LabelQRCode = new System.Windows.Forms.Label();
            this.PictureBoxQRCode = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxQRCode)).BeginInit();
            this.SuspendLayout();
            // 
            // LabelQRCode
            // 
            this.LabelQRCode.AutoSize = true;
            this.LabelQRCode.Location = new System.Drawing.Point(18, 1);
            this.LabelQRCode.Name = "LabelQRCode";
            this.LabelQRCode.Size = new System.Drawing.Size(51, 13);
            this.LabelQRCode.TabIndex = 0;
            this.LabelQRCode.Text = "QR Code";
            // 
            // PictureBoxQRCode
            // 
            this.PictureBoxQRCode.BackgroundImage = global::fa.Properties.Resources.QRCode;
            this.PictureBoxQRCode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PictureBoxQRCode.Location = new System.Drawing.Point(6, 16);
            this.PictureBoxQRCode.Name = "PictureBoxQRCode";
            this.PictureBoxQRCode.Size = new System.Drawing.Size(78, 71);
            this.PictureBoxQRCode.TabIndex = 43;
            this.PictureBoxQRCode.TabStop = false;
            // 
            // QRCodeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PictureBoxQRCode);
            this.Controls.Add(this.LabelQRCode);
            this.Name = "QRCodeControl";
            this.Size = new System.Drawing.Size(91, 94);
            this.Load += new System.EventHandler(this.QRCodeControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxQRCode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelQRCode;
        private System.Windows.Forms.PictureBox PictureBoxQRCode;
    }
}
