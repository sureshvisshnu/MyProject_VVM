namespace fa.views.controls.text
{
    partial class CurrencyTextBox
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
            this.SuspendLayout();
            // 
            // CurrencyTextBox
            // 
            this.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CurrencyTextBox_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CurrencyTextBox_KeyPress);
            this.Leave += new System.EventHandler(this.CurrencyTextBox_Leave);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CurrencyTextBox_MouseDown);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
