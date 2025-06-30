namespace fa.views.controls
{
    partial class Test
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripCheckBox2 = new fa.views.controls.ToolStripCheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(346, 80);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(346, 121);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripCheckBox2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(663, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripCheckBox2
            // 
            this.toolStripCheckBox2.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripCheckBox2.Checked = false;
            this.toolStripCheckBox2.CheckState = System.Windows.Forms.CheckState.Unchecked;
            this.toolStripCheckBox2.Name = "toolStripCheckBox2";
            this.toolStripCheckBox2.Size = new System.Drawing.Size(64, 22);
            this.toolStripCheckBox2.Text = "IsBatch";
            this.toolStripCheckBox2.CheckedChanged += new System.EventHandler(this.toolStripCheckBox2_CheckedChanged);
            this.toolStripCheckBox2.CheckStateChanged += new System.EventHandler(this.toolStripCheckBox2_CheckStateChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(152, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 51);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1 dfgdfg dgdfsgsdfg gdfg";
            // 
            // Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 261);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Test";
            this.Text = "Test";
            this.Load += new System.EventHandler(this.Test_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private Ab2ToolStrip ab2ToolStrip1;
        private Dropdown_Button.UserControlButtonWithMenu userControlButtonWithMenu1;
        private ToolstripCheckedTreeComboBox toolstripCheckedTreeComboBox1;
        private ComboTreeView.ComboTreeBox comboTreeBox1;
        private text.CheckedTreeComboBox checkedTreeComboBox1;
        private text.DelayedTextChangeTextBox delayedTextChangeTextBox1;
        private ToolstripDelayedTextBox toolstripDelayedTextBox1;
        private ToolStripCheckBox toolStripCheckBox1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private ToolStripCheckBox toolStripCheckBox2;
        private System.Windows.Forms.Label label1;
    }
}