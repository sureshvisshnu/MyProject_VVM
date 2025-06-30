namespace fa.views.controls.text
{
    partial class UserControlPoint
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
            this.components = new System.ComponentModel.Container();
            this.Row = new fa.views.controls.text.NumberTextBox(this.components);
            this.Separator = new fa.views.controls.text.NumberTextBox(this.components);
            this.Column = new fa.views.controls.text.NumberTextBox(this.components);
            this.SuspendLayout();
            // 
            // Row
            // 
            this.Row.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Row.Location = new System.Drawing.Point(1, 4);
            this.Row.MaxLength = 2;
            this.Row.Name = "Row";
            this.Row.Size = new System.Drawing.Size(18, 13);
            this.Row.TabIndex = 0;
            this.Row.Text = "1";
            this.Row.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Row.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Row_KeyPress);
            this.Row.Leave += new System.EventHandler(this.Row_Leave);
            this.Row.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Row_PreviewKeyDown);
            // 
            // Separator
            // 
            this.Separator.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Separator.Enabled = false;
            this.Separator.Location = new System.Drawing.Point(14, 4);
            this.Separator.MaxLength = 1;
            this.Separator.Name = "Separator";
            this.Separator.Size = new System.Drawing.Size(10, 13);
            this.Separator.TabIndex = 2;
            this.Separator.TabStop = false;
            this.Separator.Text = ",";
            this.Separator.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Column
            // 
            this.Column.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Column.Location = new System.Drawing.Point(26, 4);
            this.Column.MaxLength = 1;
            this.Column.Name = "Column";
            this.Column.Size = new System.Drawing.Size(10, 13);
            this.Column.TabIndex = 1;
            this.Column.Text = "1";
            this.Column.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Column.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Column_KeyPress);
            this.Column.Leave += new System.EventHandler(this.Column_Leave);
            this.Column.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Column_PreviewKeyDown);
            // 
            // UserControlPoint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.Row);
            this.Controls.Add(this.Separator);
            this.Controls.Add(this.Column);
            this.Name = "UserControlPoint";
            this.Size = new System.Drawing.Size(136, 21);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.UserControlPoint_KeyPress);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.UserControlPoint_PreviewKeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private NumberTextBox Row;
        private NumberTextBox Separator;
        private NumberTextBox Column;
    }
}
