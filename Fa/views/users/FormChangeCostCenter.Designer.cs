namespace fa.views.users
{
    partial class FormChangeCostCenter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChangeCostCenter));
            this.label3 = new System.Windows.Forms.Label();
            this.ComboBoxSelectCompanyCostcenter = new System.Windows.Forms.ComboBox();
            this.BtnSelectCompanyCancel = new System.Windows.Forms.Button();
            this.BtnSelectCompanyContinue = new System.Windows.Forms.Button();
            this.Print = new System.Windows.Forms.StatusStrip();
            this.SelectCostCenterErrorMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.Print.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Select Cost Center";
            // 
            // ComboBoxSelectCompanyCostcenter
            // 
            this.ComboBoxSelectCompanyCostcenter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ComboBoxSelectCompanyCostcenter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboBoxSelectCompanyCostcenter.FormattingEnabled = true;
            this.ComboBoxSelectCompanyCostcenter.Location = new System.Drawing.Point(12, 40);
            this.ComboBoxSelectCompanyCostcenter.Name = "ComboBoxSelectCompanyCostcenter";
            this.ComboBoxSelectCompanyCostcenter.Size = new System.Drawing.Size(312, 21);
            this.ComboBoxSelectCompanyCostcenter.TabIndex = 9;
            this.ComboBoxSelectCompanyCostcenter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ComboBoxSelectCompanyCostcenter_KeyPress);
            // 
            // BtnSelectCompanyCancel
            // 
            this.BtnSelectCompanyCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnSelectCompanyCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSelectCompanyCancel.Location = new System.Drawing.Point(146, 75);
            this.BtnSelectCompanyCancel.Name = "BtnSelectCompanyCancel";
            this.BtnSelectCompanyCancel.Size = new System.Drawing.Size(83, 23);
            this.BtnSelectCompanyCancel.TabIndex = 8;
            this.BtnSelectCompanyCancel.Text = "Cancel [Esc]";
            this.BtnSelectCompanyCancel.UseVisualStyleBackColor = true;
            this.BtnSelectCompanyCancel.Click += new System.EventHandler(this.BtnSelectCompanyCancel_Click);
            // 
            // BtnSelectCompanyContinue
            // 
            this.BtnSelectCompanyContinue.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSelectCompanyContinue.Location = new System.Drawing.Point(233, 75);
            this.BtnSelectCompanyContinue.Name = "BtnSelectCompanyContinue";
            this.BtnSelectCompanyContinue.Size = new System.Drawing.Size(91, 23);
            this.BtnSelectCompanyContinue.TabIndex = 7;
            this.BtnSelectCompanyContinue.Text = "Continue [F8]";
            this.BtnSelectCompanyContinue.UseVisualStyleBackColor = true;
            this.BtnSelectCompanyContinue.Click += new System.EventHandler(this.BtnSelectCompanyContinue_Click);
            // 
            // Print
            // 
            this.Print.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Print.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelectCostCenterErrorMsg});
            this.Print.Location = new System.Drawing.Point(0, 111);
            this.Print.Name = "Print";
            this.Print.Size = new System.Drawing.Size(335, 22);
            this.Print.TabIndex = 13;
            // 
            // SelectCostCenterErrorMsg
            // 
            this.SelectCostCenterErrorMsg.Name = "SelectCostCenterErrorMsg";
            this.SelectCostCenterErrorMsg.Size = new System.Drawing.Size(31, 17);
            this.SelectCostCenterErrorMsg.Text = "        ";
            // 
            // FormChangeCostCenter
            // 
            this.AcceptButton = this.BtnSelectCompanyContinue;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.BtnSelectCompanyCancel;
            this.ClientSize = new System.Drawing.Size(335, 133);
            this.Controls.Add(this.ComboBoxSelectCompanyCostcenter);
            this.Controls.Add(this.Print);
            this.Controls.Add(this.BtnSelectCompanyCancel);
            this.Controls.Add(this.BtnSelectCompanyContinue);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormChangeCostCenter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Change Cost Center";
            this.Load += new System.EventHandler(this.FormChangeCostCenter_Load);
            this.Print.ResumeLayout(false);
            this.Print.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ComboBoxSelectCompanyCostcenter;
        private System.Windows.Forms.Button BtnSelectCompanyCancel;
        private System.Windows.Forms.Button BtnSelectCompanyContinue;
        private System.Windows.Forms.StatusStrip Print;
        private System.Windows.Forms.ToolStripStatusLabel SelectCostCenterErrorMsg;
    }
}