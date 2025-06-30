namespace Fa.views.controls.ComboBoxSearchable
{
    partial class ComboBoxWithSearchBox
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
            ComboBox = new ComboBox();
            TextBox = new TextBox();
            SuspendLayout();
            // 
            // ComboBox
            // 
            ComboBox.FormattingEnabled = true;
            ComboBox.Location = new Point(5, 5);
            ComboBox.Name = "ComboBox";
            ComboBox.Size = new Size(240, 23);
            ComboBox.TabIndex = 0;
            // 
            // TextBox
            // 
            TextBox.Location = new Point(5, 5);
            TextBox.Name = "TextBox";
            TextBox.Size = new Size(222, 23);
            TextBox.TabIndex = 1;
            // 
            // ComboBoxWithSearchBox
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(TextBox);
            Controls.Add(ComboBox);
            Name = "ComboBoxWithSearchBox";
            Size = new Size(248, 35);
            Load += ComboBoxWithSearchBox_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox ComboBox;
        private TextBox TextBox;
    }
}
