namespace Fa.views.controls.ComboBoxSearchable
{
    partial class ComboBoxWithSearchFilter
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
            SearchTextBox = new TextBox();
            SuspendLayout();
            // 
            // ComboBox
            // 
            ComboBox.FormattingEnabled = true;
            ComboBox.Location = new Point(5, 6);
            ComboBox.Name = "ComboBox";
            ComboBox.Size = new Size(275, 23);
            ComboBox.TabIndex = 0;
            ComboBox.BindingContextChanged += ComboBoxWithSearchFilter_Load;
            // 
            // SearchTextBox
            // 
            SearchTextBox.Location = new Point(5, 6);
            SearchTextBox.Name = "SearchTextBox";
            SearchTextBox.Size = new Size(255, 23);
            SearchTextBox.TabIndex = 1;
            SearchTextBox.TextChanged += SearchTextBox_TextChanged;
            // 
            // ComboBoxWithSearchFilter
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(SearchTextBox);
            Controls.Add(ComboBox);
            Name = "ComboBoxWithSearchFilter";
            Size = new Size(283, 35);
            Load += ComboBoxWithSearchFilter_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox ComboBox;
        private TextBox SearchTextBox;
    }
}
