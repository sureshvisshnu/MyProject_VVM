namespace Dropdown_Button {
    partial class UserControlButtonWithMenu {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
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
            btnDropDown = new Button();
            SuspendLayout();
            // 
            // btnDropDown
            // 
            btnDropDown.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnDropDown.FlatStyle = FlatStyle.System;
            btnDropDown.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            btnDropDown.Location = new Point(4, 0);
            btnDropDown.Margin = new Padding(4, 3, 4, 3);
            btnDropDown.Name = "btnDropDown";
            btnDropDown.Size = new Size(204, 27);
            btnDropDown.TabIndex = 0;
            btnDropDown.UseVisualStyleBackColor = true;
            btnDropDown.Click += btnDropDown_Click;
            // 
            // UserControlButtonWithMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnDropDown);
            Margin = new Padding(4, 3, 4, 3);
            Name = "UserControlButtonWithMenu";
            Size = new Size(211, 28);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnDropDown;
    }
}
