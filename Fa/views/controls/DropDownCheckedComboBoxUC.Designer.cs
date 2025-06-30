
namespace fa.views.controls
{
    partial class DropDownCheckedComboBoxUC
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
            this.CheckedItemListBox = new System.Windows.Forms.CheckedListBox();
            this.TextBoxItemSearch = new System.Windows.Forms.TextBox();
            this.ButtonDropDown = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CheckedItemListBox
            // 
            this.CheckedItemListBox.FormattingEnabled = true;
            this.CheckedItemListBox.Location = new System.Drawing.Point(1, 21);
            this.CheckedItemListBox.Name = "CheckedItemListBox";
            this.CheckedItemListBox.Size = new System.Drawing.Size(150, 19);
            this.CheckedItemListBox.TabIndex = 0;
            this.CheckedItemListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CheckedItemListBox_ItemCheck);
            this.CheckedItemListBox.SelectedIndexChanged += new System.EventHandler(this.CheckedItemListBox_SelectedIndexChanged);
            // 
            // TextBoxItemSearch
            // 
            this.TextBoxItemSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxItemSearch.Location = new System.Drawing.Point(1, 0);
            this.TextBoxItemSearch.Name = "TextBoxItemSearch";
            this.TextBoxItemSearch.Size = new System.Drawing.Size(150, 20);
            this.TextBoxItemSearch.TabIndex = 1;
            this.TextBoxItemSearch.TextChanged += new System.EventHandler(this.TextBoxItemSearch_TextChanged);
            // 
            // ButtonDropDown
            // 
            this.ButtonDropDown.FlatAppearance.BorderSize = 0;
            this.ButtonDropDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonDropDown.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.ButtonDropDown.Location = new System.Drawing.Point(130, 1);
            this.ButtonDropDown.Name = "ButtonDropDown";
            this.ButtonDropDown.Size = new System.Drawing.Size(19, 19);
            this.ButtonDropDown.TabIndex = 2;
            this.ButtonDropDown.Text = "V";
            this.ButtonDropDown.UseVisualStyleBackColor = true;
            this.ButtonDropDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ButtonDropDown_MouseDown);
            this.ButtonDropDown.MouseHover += new System.EventHandler(this.ButtonDropDown_MouseHover);
            this.ButtonDropDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ButtonDropDown_MouseUp);
            // 
            // DropDownCheckedComboBoxUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ButtonDropDown);
            this.Controls.Add(this.TextBoxItemSearch);
            this.Controls.Add(this.CheckedItemListBox);
            this.Name = "DropDownCheckedComboBoxUC";
            this.Size = new System.Drawing.Size(150, 44);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox CheckedItemListBox;
        private System.Windows.Forms.TextBox TextBoxItemSearch;
        private System.Windows.Forms.Button ButtonDropDown;
    }
}
