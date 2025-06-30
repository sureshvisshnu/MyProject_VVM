namespace fa.views.users
{
    partial class FormSelectCompany
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectCompany));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ComboBoxSelectCompanyCompany = new System.Windows.Forms.ComboBox();
            this.BtnSelectCompanyContinue = new System.Windows.Forms.Button();
            this.BtnSelectCompanyCancel = new System.Windows.Forms.Button();
            this.ComboBoxSelectCompanyCostcenter = new System.Windows.Forms.ComboBox();
            this.LabelSelectCompanyCostcente = new System.Windows.Forms.Label();
            this.Print = new System.Windows.Forms.StatusStrip();
            this.SelectCompanyErrorMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.Print.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(223, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "You have access to more than one company!";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Company";
            // 
            // ComboBoxSelectCompanyCompany
            // 
            this.ComboBoxSelectCompanyCompany.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ComboBoxSelectCompanyCompany.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboBoxSelectCompanyCompany.FormattingEnabled = true;
            this.ComboBoxSelectCompanyCompany.Location = new System.Drawing.Point(15, 47);
            this.ComboBoxSelectCompanyCompany.Name = "ComboBoxSelectCompanyCompany";
            this.ComboBoxSelectCompanyCompany.Size = new System.Drawing.Size(309, 21);
            this.ComboBoxSelectCompanyCompany.TabIndex = 1;
            this.ComboBoxSelectCompanyCompany.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSelectCompanyCompany_SelectedIndexChanged);
            this.ComboBoxSelectCompanyCompany.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ComboBoxSelectCompanyCompany_KeyPress);
            // 
            // BtnSelectCompanyContinue
            // 
            this.BtnSelectCompanyContinue.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSelectCompanyContinue.Location = new System.Drawing.Point(233, 120);
            this.BtnSelectCompanyContinue.Name = "BtnSelectCompanyContinue";
            this.BtnSelectCompanyContinue.Size = new System.Drawing.Size(91, 23);
            this.BtnSelectCompanyContinue.TabIndex = 4;
            this.BtnSelectCompanyContinue.Text = "Continue [F8]";
            this.BtnSelectCompanyContinue.UseVisualStyleBackColor = true;
            this.BtnSelectCompanyContinue.Click += new System.EventHandler(this.BtnSelectCompanyContinue_Click);
            // 
            // BtnSelectCompanyCancel
            // 
            this.BtnSelectCompanyCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnSelectCompanyCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSelectCompanyCancel.Location = new System.Drawing.Point(144, 120);
            this.BtnSelectCompanyCancel.Name = "BtnSelectCompanyCancel";
            this.BtnSelectCompanyCancel.Size = new System.Drawing.Size(83, 23);
            this.BtnSelectCompanyCancel.TabIndex = 3;
            this.BtnSelectCompanyCancel.Text = "Cancel [Esc]";
            this.BtnSelectCompanyCancel.UseVisualStyleBackColor = true;
            this.BtnSelectCompanyCancel.Click += new System.EventHandler(this.BtnSelectCompanyCancel_Click);
            // 
            // ComboBoxSelectCompanyCostcenter
            // 
            this.ComboBoxSelectCompanyCostcenter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ComboBoxSelectCompanyCostcenter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboBoxSelectCompanyCostcenter.FormattingEnabled = true;
            this.ComboBoxSelectCompanyCostcenter.Location = new System.Drawing.Point(15, 87);
            this.ComboBoxSelectCompanyCostcenter.Name = "ComboBoxSelectCompanyCostcenter";
            this.ComboBoxSelectCompanyCostcenter.Size = new System.Drawing.Size(309, 21);
            this.ComboBoxSelectCompanyCostcenter.TabIndex = 2;
            this.ComboBoxSelectCompanyCostcenter.Visible = false;
            this.ComboBoxSelectCompanyCostcenter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ComboBoxSelectCompanyCostcenter_KeyPress);
            // 
            // LabelSelectCompanyCostcente
            // 
            this.LabelSelectCompanyCostcente.AutoSize = true;
            this.LabelSelectCompanyCostcente.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelSelectCompanyCostcente.Location = new System.Drawing.Point(12, 71);
            this.LabelSelectCompanyCostcente.Name = "LabelSelectCompanyCostcente";
            this.LabelSelectCompanyCostcente.Size = new System.Drawing.Size(73, 13);
            this.LabelSelectCompanyCostcente.TabIndex = 6;
            this.LabelSelectCompanyCostcente.Text = "Cost Center";
            this.LabelSelectCompanyCostcente.Visible = false;
            // 
            // Print
            // 
            this.Print.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Print.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelectCompanyErrorMsg});
            this.Print.Location = new System.Drawing.Point(0, 155);
            this.Print.Name = "Print";
            this.Print.Size = new System.Drawing.Size(336, 22);
            this.Print.TabIndex = 11;
            // 
            // SelectCompanyErrorMsg
            // 
            this.SelectCompanyErrorMsg.Name = "SelectCompanyErrorMsg";
            this.SelectCompanyErrorMsg.Size = new System.Drawing.Size(31, 17);
            this.SelectCompanyErrorMsg.Text = "        ";
            // 
            // FormSelectCompany
            // 
            this.AcceptButton = this.BtnSelectCompanyContinue;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.BtnSelectCompanyCancel;
            this.ClientSize = new System.Drawing.Size(336, 177);
            this.Controls.Add(this.ComboBoxSelectCompanyCostcenter);
            this.Controls.Add(this.Print);
            this.Controls.Add(this.BtnSelectCompanyCancel);
            this.Controls.Add(this.BtnSelectCompanyContinue);
            this.Controls.Add(this.ComboBoxSelectCompanyCompany);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LabelSelectCompanyCostcente);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelectCompany";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Company";
            this.Load += new System.EventHandler(this.FormSelectCompany_Load);
            this.Print.ResumeLayout(false);
            this.Print.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ComboBoxSelectCompanyCompany;
        private System.Windows.Forms.Button BtnSelectCompanyContinue;
        private System.Windows.Forms.Button BtnSelectCompanyCancel;
        private System.Windows.Forms.ComboBox ComboBoxSelectCompanyCostcenter;
        private System.Windows.Forms.Label LabelSelectCompanyCostcente;
        private System.Windows.Forms.StatusStrip Print;
        private System.Windows.Forms.ToolStripStatusLabel SelectCompanyErrorMsg;
    }
}