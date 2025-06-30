namespace fa.views.controls.text
{
    partial class CheckedTreeComboBox
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
            ComboTreeBox = new ComboTreeView.ComboTreeBox();
            TextBox = new TextBox();
            SuspendLayout();
            // 
            // ComboTreeBox
            // 
            ComboTreeBox.BackColor = Color.White;
            ComboTreeBox.DroppedDown = false;
            ComboTreeBox.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ComboTreeBox.Location = new Point(0, 0);
            ComboTreeBox.Margin = new Padding(4, 3, 4, 3);
            ComboTreeBox.Name = "ComboTreeBox";
            ComboTreeBox.SelectedNode = null;
            ComboTreeBox.ShowCheckBoxes = true;
            ComboTreeBox.Size = new Size(272, 24);
            ComboTreeBox.TabIndex = 0;
            ComboTreeBox.TabStop = false;
            ComboTreeBox.AfterCheck += ComboTreeBox_AfterCheck;
            ComboTreeBox.Click += TextBox_Click;
            ComboTreeBox.KeyPress += TextBox_KeyPress;
            ComboTreeBox.PreviewKeyDown += ComboTreeBox_PreviewKeyDown;
            // 
            // TextBox
            // 
            TextBox.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBox.Location = new Point(0, 0);
            TextBox.Margin = new Padding(4, 3, 4, 3);
            TextBox.Name = "TextBox";
            TextBox.Size = new Size(251, 21);
            TextBox.TabIndex = 0;
            TextBox.Click += TextBox_Click;
            TextBox.TextChanged += TextBox_TextChanged;
            TextBox.PreviewKeyDown += TextBox_PreviewKeyDown;
            // 
            // CheckedTreeComboBox
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(TextBox);
            Controls.Add(ComboTreeBox);
            Margin = new Padding(4, 3, 4, 3);
            Name = "CheckedTreeComboBox";
            Size = new Size(270, 22);
            ClientSizeChanged += CheckedTreeComboBox_ClientSizeChanged;
            MouseClick += CheckedTreeComboBox_MouseClick;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboTreeView.ComboTreeBox ComboTreeBox;
        private TextBox TextBox;
    }
}
