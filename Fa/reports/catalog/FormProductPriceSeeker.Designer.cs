namespace Fa.reports.catalog
{
    partial class FormProductPriceSeeker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProductPriceSeeker));
            groupBox1 = new GroupBox();
            TextBoxCatalogSearch = new fa.views.controls.text.DelayedTextChangeTextBox();
            PRODUCTNAME = new Label();
            LabelPurchasePrice = new Label();
            LabelPPP = new Label();
            label2 = new Label();
            LabelCP = new Label();
            label4 = new Label();
            LabelMRP = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            LabelRP = new Label();
            LabelWSP = new Label();
            LabelLP = new Label();
            BtnExit = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(TextBoxCatalogSearch);
            groupBox1.Controls.Add(PRODUCTNAME);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(315, 83);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Search Product";
            // 
            // TextBoxCatalogSearch
            // 
            TextBoxCatalogSearch.BackColor = SystemColors.Window;
            TextBoxCatalogSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxCatalogSearch.Delay = true;
            TextBoxCatalogSearch.DelayTime = 2000;
            TextBoxCatalogSearch.Location = new Point(6, 21);
            TextBoxCatalogSearch.MaxLength = 50;
            TextBoxCatalogSearch.Name = "TextBoxCatalogSearch";
            TextBoxCatalogSearch.Searchstartfrom = 2;
            TextBoxCatalogSearch.Size = new Size(303, 22);
            TextBoxCatalogSearch.TabIndex = 202;
            TextBoxCatalogSearch.TextChanged += TextBoxCatalogSearch_TextChanged;
            // 
            // PRODUCTNAME
            // 
            PRODUCTNAME.AutoSize = true;
            PRODUCTNAME.ForeColor = Color.FromArgb(255, 128, 0);
            PRODUCTNAME.Location = new Point(6, 56);
            PRODUCTNAME.Name = "PRODUCTNAME";
            PRODUCTNAME.Size = new Size(95, 14);
            PRODUCTNAME.TabIndex = 201;
            PRODUCTNAME.Text = "Purchase Price";
            // 
            // LabelPurchasePrice
            // 
            LabelPurchasePrice.AutoSize = true;
            LabelPurchasePrice.ForeColor = SystemColors.ButtonShadow;
            LabelPurchasePrice.Location = new Point(333, 18);
            LabelPurchasePrice.Name = "LabelPurchasePrice";
            LabelPurchasePrice.Size = new Size(95, 14);
            LabelPurchasePrice.TabIndex = 200;
            LabelPurchasePrice.Text = "Purchase Price";
            // 
            // LabelPPP
            // 
            LabelPPP.ForeColor = SystemColors.ActiveCaption;
            LabelPPP.Location = new Point(452, 18);
            LabelPPP.Name = "LabelPPP";
            LabelPPP.Size = new Size(51, 14);
            LabelPPP.TabIndex = 201;
            LabelPPP.Text = "0.00";
            LabelPPP.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = SystemColors.ButtonShadow;
            label2.Location = new Point(333, 52);
            label2.Name = "label2";
            label2.Size = new Size(68, 14);
            label2.TabIndex = 202;
            label2.Text = "Cost Price";
            // 
            // LabelCP
            // 
            LabelCP.ForeColor = SystemColors.ActiveCaption;
            LabelCP.Location = new Point(452, 52);
            LabelCP.Name = "LabelCP";
            LabelCP.Size = new Size(51, 14);
            LabelCP.TabIndex = 203;
            LabelCP.Text = "0.00";
            LabelCP.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = SystemColors.ButtonShadow;
            label4.Location = new Point(333, 88);
            label4.Name = "label4";
            label4.Size = new Size(35, 14);
            label4.TabIndex = 204;
            label4.Text = "MRP";
            // 
            // LabelMRP
            // 
            LabelMRP.ForeColor = SystemColors.ActiveCaption;
            LabelMRP.Location = new Point(452, 88);
            LabelMRP.Name = "LabelMRP";
            LabelMRP.Size = new Size(51, 14);
            LabelMRP.TabIndex = 205;
            LabelMRP.Text = "0.00";
            LabelMRP.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = SystemColors.ButtonShadow;
            label6.Location = new Point(531, 18);
            label6.Name = "label6";
            label6.Size = new Size(75, 14);
            label6.TabIndex = 206;
            label6.Text = "Retail Price";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.ForeColor = SystemColors.ButtonShadow;
            label7.Location = new Point(531, 52);
            label7.Name = "label7";
            label7.Size = new Size(107, 14);
            label7.TabIndex = 207;
            label7.Text = "Whole Sale Price";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.ForeColor = SystemColors.ButtonShadow;
            label8.Location = new Point(531, 88);
            label8.Name = "label8";
            label8.Size = new Size(65, 14);
            label8.TabIndex = 208;
            label8.Text = "Line Price";
            // 
            // LabelRP
            // 
            LabelRP.ForeColor = Color.FromArgb(0, 192, 0);
            LabelRP.Location = new Point(664, 18);
            LabelRP.Name = "LabelRP";
            LabelRP.Size = new Size(51, 14);
            LabelRP.TabIndex = 209;
            LabelRP.Text = "0.00";
            LabelRP.TextAlign = ContentAlignment.MiddleRight;
            // 
            // LabelWSP
            // 
            LabelWSP.ForeColor = Color.FromArgb(0, 192, 0);
            LabelWSP.Location = new Point(664, 52);
            LabelWSP.Name = "LabelWSP";
            LabelWSP.Size = new Size(51, 14);
            LabelWSP.TabIndex = 210;
            LabelWSP.Text = "0.00";
            LabelWSP.TextAlign = ContentAlignment.MiddleRight;
            // 
            // LabelLP
            // 
            LabelLP.ForeColor = Color.FromArgb(0, 192, 0);
            LabelLP.Location = new Point(664, 88);
            LabelLP.Name = "LabelLP";
            LabelLP.Size = new Size(51, 14);
            LabelLP.TabIndex = 211;
            LabelLP.Text = "0.00";
            LabelLP.TextAlign = ContentAlignment.MiddleRight;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(72, 101);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(72, 23);
            BtnExit.TabIndex = 212;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // FormProductPriceSeeker
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(738, 136);
            Controls.Add(BtnExit);
            Controls.Add(LabelLP);
            Controls.Add(LabelWSP);
            Controls.Add(LabelRP);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(LabelMRP);
            Controls.Add(label4);
            Controls.Add(LabelCP);
            Controls.Add(label2);
            Controls.Add(LabelPPP);
            Controls.Add(LabelPurchasePrice);
            Controls.Add(groupBox1);
            Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormProductPriceSeeker";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Product Price View";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBox1;
        private Label LabelPurchasePrice;
        private Label LabelPPP;
        private Label label2;
        private Label LabelCP;
        private Label label4;
        private Label LabelMRP;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label LabelRP;
        private Label LabelWSP;
        private Label LabelLP;
        private Label PRODUCTNAME;
        private Button BtnExit;
        private fa.views.controls.text.DelayedTextChangeTextBox TextBoxCatalogSearch;
    }
}