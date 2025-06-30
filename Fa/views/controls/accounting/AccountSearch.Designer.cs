namespace Fa.views.controls.accounting
{
    partial class AccountSearch
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
            AccountSearchText = new TextBox();
            listView1 = new ListView();
            CloseLabel = new Label();
            SuspendLayout();
            // 
            // AccountSearchText
            // 
            AccountSearchText.BorderStyle = BorderStyle.FixedSingle;
            AccountSearchText.Location = new Point(3, 4);
            AccountSearchText.Name = "AccountSearchText";
            AccountSearchText.Size = new Size(269, 23);
            AccountSearchText.TabIndex = 0;
            // 
            // listView1
            // 
            listView1.BorderStyle = BorderStyle.FixedSingle;
            listView1.Location = new Point(3, 29);
            listView1.Name = "listView1";
            listView1.Size = new Size(300, 332);
            listView1.TabIndex = 2;
            listView1.UseCompatibleStateImageBehavior = false;
            // 
            // CloseLabel
            // 
            CloseLabel.AutoSize = true;
            CloseLabel.FlatStyle = FlatStyle.Flat;
            CloseLabel.Font = new Font("Tahoma", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            CloseLabel.Location = new Point(278, 6);
            CloseLabel.Name = "CloseLabel";
            CloseLabel.Size = new Size(18, 18);
            CloseLabel.TabIndex = 3;
            CloseLabel.Text = "X";
            CloseLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AccountSearch
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(CloseLabel);
            Controls.Add(listView1);
            Controls.Add(AccountSearchText);
            Name = "AccountSearch";
            Size = new Size(300, 367);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox AccountSearchText;
        private ListView listView1;
        private Label CloseLabel;
    }
}
