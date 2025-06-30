namespace Fa.views.common
{
    partial class FormSelectAccount
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectAccount));
            AccountsListBox = new ListBox();
            AccountSearchTextBox = new fa.views.controls.text.DelayedTextChangeTextBox();
            BtnSelectAccountCancel = new Button();
            BtnSelectAccountDone = new Button();
            SuspendLayout();
            // 
            // AccountsListBox
            // 
            AccountsListBox.BorderStyle = BorderStyle.FixedSingle;
            AccountsListBox.FormattingEnabled = true;
            AccountsListBox.Location = new Point(3, 27);
            AccountsListBox.Name = "AccountsListBox";
            AccountsListBox.Size = new Size(190, 327);
            AccountsListBox.TabIndex = 3;
            AccountsListBox.SelectedIndexChanged += AccountsListBox_SelectedIndexChanged_1;
            // 
            // AccountSearchTextBox
            // 
            AccountSearchTextBox.BorderStyle = BorderStyle.FixedSingle;
            AccountSearchTextBox.Delay = false;
            AccountSearchTextBox.DelayTime = 2000;
            AccountSearchTextBox.Location = new Point(3, 4);
            AccountSearchTextBox.MaxLength = 35;
            AccountSearchTextBox.Name = "AccountSearchTextBox";
            AccountSearchTextBox.Searchstartfrom = 2;
            AccountSearchTextBox.Size = new Size(190, 21);
            AccountSearchTextBox.TabIndex = 2;
            // 
            // BtnSelectAccountCancel
            // 
            BtnSelectAccountCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSelectAccountCancel.Location = new Point(26, 358);
            BtnSelectAccountCancel.Name = "BtnSelectAccountCancel";
            BtnSelectAccountCancel.Size = new Size(80, 20);
            BtnSelectAccountCancel.TabIndex = 6;
            BtnSelectAccountCancel.Text = "Reset [Esc]";
            BtnSelectAccountCancel.UseVisualStyleBackColor = true;
            // 
            // BtnSelectAccountDone
            // 
            BtnSelectAccountDone.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSelectAccountDone.Location = new Point(111, 358);
            BtnSelectAccountDone.Name = "BtnSelectAccountDone";
            BtnSelectAccountDone.Size = new Size(80, 20);
            BtnSelectAccountDone.TabIndex = 5;
            BtnSelectAccountDone.Text = "Done [F8]";
            BtnSelectAccountDone.UseVisualStyleBackColor = true;
            // 
            // FormSelectAccount
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(198, 390);
            Controls.Add(BtnSelectAccountCancel);
            Controls.Add(BtnSelectAccountDone);
            Controls.Add(AccountsListBox);
            Controls.Add(AccountSearchTextBox);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSelectAccount";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select Account";
            Load += FormSelectAccount_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox AccountsListBox;
        private fa.views.controls.text.DelayedTextChangeTextBox AccountSearchTextBox;
        private Button BtnSelectAccountCancel;
        private Button BtnSelectAccountDone;
    }
}