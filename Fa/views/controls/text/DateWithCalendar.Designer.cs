namespace fa.views.controls.text
{
    partial class DateWithCalendar
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
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.maskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.SuspendLayout();
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.dateTimePicker.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker.Location = new System.Drawing.Point(75, -1);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(17, 21);
            this.dateTimePicker.TabIndex = 2;
            this.dateTimePicker.TabStop = false;
            this.dateTimePicker.CloseUp += new System.EventHandler(this.dateTimePicker_CloseUp);
            this.dateTimePicker.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            this.dateTimePicker.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dateTimePicker_KeyDown);
            this.dateTimePicker.Leave += new System.EventHandler(this.dateTimePicker_Leave);
            this.dateTimePicker.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dateTimePicker_MouseDown);
            // 
            // maskedTextBox
            // 
            this.maskedTextBox.BackColor = System.Drawing.Color.White;
            this.maskedTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.maskedTextBox.Location = new System.Drawing.Point(2, 2);
            this.maskedTextBox.Mask = "##/##/####";
            this.maskedTextBox.Name = "maskedTextBox";
            this.maskedTextBox.Size = new System.Drawing.Size(71, 14);
            this.maskedTextBox.TabIndex = 1;
            this.maskedTextBox.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.maskedTextBox_MaskInputRejected);
            this.maskedTextBox.TextChanged += new System.EventHandler(this.maskedTextBox_TextChanged);
            this.maskedTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.maskedTextBox_KeyDown);
            this.maskedTextBox.Leave += new System.EventHandler(this.maskedTextBox_Leave);
            this.maskedTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.maskedTextBox_PreviewKeyDown);
            // 
            // DateWithCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.maskedTextBox);
            this.Controls.Add(this.dateTimePicker);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DateWithCalendar";
            this.Size = new System.Drawing.Size(91, 19);
            this.Load += new System.EventHandler(this.DateWithCalendar_Load);
            this.Resize += new System.EventHandler(this.DateWithCalendar_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker;
        public System.Windows.Forms.MaskedTextBox maskedTextBox;
    }
}
