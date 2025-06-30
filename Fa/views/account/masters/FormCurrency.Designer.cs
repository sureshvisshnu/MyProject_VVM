namespace fa.views.account.masters
{
    partial class FormCurrency
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCurrency));
            this.ListBoxCurrency = new System.Windows.Forms.ListBox();
            this.TextBoxCurrencySearch = new System.Windows.Forms.TextBox();
            this.GroupBoxCurrency = new System.Windows.Forms.GroupBox();
            this.ComboBoxCurrencyPricision = new fa.views.controls.ComboBoxSwapTextBox();
            this.TextBoxCurrencyFormat = new System.Windows.Forms.TextBox();
            this.TextBoxISOCode = new System.Windows.Forms.TextBox();
            this.TextBoxCurrencyDisplayAs = new System.Windows.Forms.TextBox();
            this.TextBoxCurrencyName = new System.Windows.Forms.TextBox();
            this.LabelCurrencyDisplayas = new System.Windows.Forms.Label();
            this.LabelCurrencyName = new System.Windows.Forms.Label();
            this.LabelCurrencyIsocode = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.LabelCurrencyPricision = new System.Windows.Forms.Label();
            this.BtnCurrencyCancelEdit = new System.Windows.Forms.Button();
            this.BtnCurrencySave = new System.Windows.Forms.Button();
            this.BtnCurrencyEdit = new System.Windows.Forms.Button();
            this.TextBoxCurrencyId = new System.Windows.Forms.TextBox();
            this.BtnNewCurrency = new System.Windows.Forms.Button();
            this.BtnCurrencyDelete = new System.Windows.Forms.Button();
            this.BtnCurrencyExit = new System.Windows.Forms.Button();
            this.GroupBoxCurrency.SuspendLayout();
            this.SuspendLayout();
            // 
            // ListBoxCurrency
            // 
            this.ListBoxCurrency.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ListBoxCurrency.FormattingEnabled = true;
            this.ListBoxCurrency.Location = new System.Drawing.Point(12, 34);
            this.ListBoxCurrency.Name = "ListBoxCurrency";
            this.ListBoxCurrency.Size = new System.Drawing.Size(223, 277);
            this.ListBoxCurrency.Sorted = true;
            this.ListBoxCurrency.TabIndex = 2;
            this.ListBoxCurrency.SelectedIndexChanged += new System.EventHandler(this.ListBoxCurrency_SelectedIndexChanged);
            // 
            // TextBoxCurrencySearch
            // 
            this.TextBoxCurrencySearch.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxCurrencySearch.Location = new System.Drawing.Point(13, 11);
            this.TextBoxCurrencySearch.MaxLength = 50;
            this.TextBoxCurrencySearch.Name = "TextBoxCurrencySearch";
            this.TextBoxCurrencySearch.Size = new System.Drawing.Size(222, 21);
            this.TextBoxCurrencySearch.TabIndex = 1;
            this.TextBoxCurrencySearch.TextChanged += new System.EventHandler(this.TextBoxCurrencySearch_TextChanged);
            this.TextBoxCurrencySearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxCurrencySearch_KeyDown);
            this.TextBoxCurrencySearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxCurrencySearch_KeyPress);
            // 
            // GroupBoxCurrency
            // 
            this.GroupBoxCurrency.BackColor = System.Drawing.SystemColors.Window;
            this.GroupBoxCurrency.Controls.Add(this.ComboBoxCurrencyPricision);
            this.GroupBoxCurrency.Controls.Add(this.TextBoxCurrencyFormat);
            this.GroupBoxCurrency.Controls.Add(this.TextBoxISOCode);
            this.GroupBoxCurrency.Controls.Add(this.TextBoxCurrencyDisplayAs);
            this.GroupBoxCurrency.Controls.Add(this.TextBoxCurrencyName);
            this.GroupBoxCurrency.Controls.Add(this.LabelCurrencyDisplayas);
            this.GroupBoxCurrency.Controls.Add(this.LabelCurrencyName);
            this.GroupBoxCurrency.Controls.Add(this.LabelCurrencyIsocode);
            this.GroupBoxCurrency.Controls.Add(this.label1);
            this.GroupBoxCurrency.Controls.Add(this.LabelCurrencyPricision);
            this.GroupBoxCurrency.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBoxCurrency.Location = new System.Drawing.Point(242, 11);
            this.GroupBoxCurrency.Name = "GroupBoxCurrency";
            this.GroupBoxCurrency.Size = new System.Drawing.Size(475, 302);
            this.GroupBoxCurrency.TabIndex = 6;
            this.GroupBoxCurrency.TabStop = false;
            this.GroupBoxCurrency.Text = "Currency Details";
            // 
            // ComboBoxCurrencyPricision
            // 
            this.ComboBoxCurrencyPricision.FormattingEnabled = true;
            this.ComboBoxCurrencyPricision.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.ComboBoxCurrencyPricision.Location = new System.Drawing.Point(17, 198);
            this.ComboBoxCurrencyPricision.Name = "ComboBoxCurrencyPricision";
            this.ComboBoxCurrencyPricision.Size = new System.Drawing.Size(121, 21);
            this.ComboBoxCurrencyPricision.TabIndex = 10;
            // 
            // TextBoxCurrencyFormat
            // 
            this.TextBoxCurrencyFormat.Location = new System.Drawing.Point(17, 158);
            this.TextBoxCurrencyFormat.MaxLength = 20;
            this.TextBoxCurrencyFormat.Name = "TextBoxCurrencyFormat";
            this.TextBoxCurrencyFormat.ShortcutsEnabled = false;
            this.TextBoxCurrencyFormat.Size = new System.Drawing.Size(209, 21);
            this.TextBoxCurrencyFormat.TabIndex = 9;
            this.TextBoxCurrencyFormat.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TextBoxCurrencyFormat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxCurrencyFormat_KeyPress);
            // 
            // TextBoxISOCode
            // 
            this.TextBoxISOCode.Location = new System.Drawing.Point(17, 118);
            this.TextBoxISOCode.MaxLength = 20;
            this.TextBoxISOCode.Name = "TextBoxISOCode";
            this.TextBoxISOCode.Size = new System.Drawing.Size(209, 21);
            this.TextBoxISOCode.TabIndex = 8;
            this.TextBoxISOCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxISOCode_KeyDown);
            this.TextBoxISOCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxISOCode_KeyPress);
            this.TextBoxISOCode.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TextBoxISOCode_MouseDown);
            // 
            // TextBoxCurrencyDisplayAs
            // 
            this.TextBoxCurrencyDisplayAs.Location = new System.Drawing.Point(17, 78);
            this.TextBoxCurrencyDisplayAs.MaxLength = 50;
            this.TextBoxCurrencyDisplayAs.Name = "TextBoxCurrencyDisplayAs";
            this.TextBoxCurrencyDisplayAs.Size = new System.Drawing.Size(328, 21);
            this.TextBoxCurrencyDisplayAs.TabIndex = 7;
            this.TextBoxCurrencyDisplayAs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxCurrencyDisplayAs_KeyDown);
            this.TextBoxCurrencyDisplayAs.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxCurrencyDisplayAs_KeyPress);
            this.TextBoxCurrencyDisplayAs.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TextBoxCurrencyDisplayAs_MouseDown);
            // 
            // TextBoxCurrencyName
            // 
            this.TextBoxCurrencyName.Location = new System.Drawing.Point(17, 38);
            this.TextBoxCurrencyName.MaxLength = 50;
            this.TextBoxCurrencyName.Name = "TextBoxCurrencyName";
            this.TextBoxCurrencyName.Size = new System.Drawing.Size(328, 21);
            this.TextBoxCurrencyName.TabIndex = 6;
            this.TextBoxCurrencyName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxCurrencyName_KeyDown);
            this.TextBoxCurrencyName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxCurrencyName_KeyPress);
            this.TextBoxCurrencyName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TextBoxCurrencyName_MouseDown);
            this.TextBoxCurrencyName.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TextBoxCurrencyName_PreviewKeyDown);
            // 
            // LabelCurrencyDisplayas
            // 
            this.LabelCurrencyDisplayas.AutoSize = true;
            this.LabelCurrencyDisplayas.Location = new System.Drawing.Point(14, 62);
            this.LabelCurrencyDisplayas.Name = "LabelCurrencyDisplayas";
            this.LabelCurrencyDisplayas.Size = new System.Drawing.Size(56, 13);
            this.LabelCurrencyDisplayas.TabIndex = 2;
            this.LabelCurrencyDisplayas.Text = "Display As";
            // 
            // LabelCurrencyName
            // 
            this.LabelCurrencyName.AutoSize = true;
            this.LabelCurrencyName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCurrencyName.Location = new System.Drawing.Point(14, 22);
            this.LabelCurrencyName.Name = "LabelCurrencyName";
            this.LabelCurrencyName.Size = new System.Drawing.Size(39, 13);
            this.LabelCurrencyName.TabIndex = 1;
            this.LabelCurrencyName.Text = "Name";
            // 
            // LabelCurrencyIsocode
            // 
            this.LabelCurrencyIsocode.AutoSize = true;
            this.LabelCurrencyIsocode.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCurrencyIsocode.Location = new System.Drawing.Point(14, 102);
            this.LabelCurrencyIsocode.Name = "LabelCurrencyIsocode";
            this.LabelCurrencyIsocode.Size = new System.Drawing.Size(58, 13);
            this.LabelCurrencyIsocode.TabIndex = 3;
            this.LabelCurrencyIsocode.Text = "ISO Code";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 142);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Format";
            // 
            // LabelCurrencyPricision
            // 
            this.LabelCurrencyPricision.AutoSize = true;
            this.LabelCurrencyPricision.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCurrencyPricision.Location = new System.Drawing.Point(14, 182);
            this.LabelCurrencyPricision.Name = "LabelCurrencyPricision";
            this.LabelCurrencyPricision.Size = new System.Drawing.Size(54, 13);
            this.LabelCurrencyPricision.TabIndex = 4;
            this.LabelCurrencyPricision.Text = "Pricision";
            // 
            // BtnCurrencyCancelEdit
            // 
            this.BtnCurrencyCancelEdit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCurrencyCancelEdit.Location = new System.Drawing.Point(456, 316);
            this.BtnCurrencyCancelEdit.Name = "BtnCurrencyCancelEdit";
            this.BtnCurrencyCancelEdit.Size = new System.Drawing.Size(83, 23);
            this.BtnCurrencyCancelEdit.TabIndex = 12;
            this.BtnCurrencyCancelEdit.Text = "Cancel [Esc]";
            this.BtnCurrencyCancelEdit.UseVisualStyleBackColor = true;
            this.BtnCurrencyCancelEdit.Click += new System.EventHandler(this.BtnCurrencyCancelEdit_Click);
            // 
            // BtnCurrencySave
            // 
            this.BtnCurrencySave.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCurrencySave.Location = new System.Drawing.Point(545, 316);
            this.BtnCurrencySave.Name = "BtnCurrencySave";
            this.BtnCurrencySave.Size = new System.Drawing.Size(83, 23);
            this.BtnCurrencySave.TabIndex = 11;
            this.BtnCurrencySave.Text = "Save [F8]";
            this.BtnCurrencySave.UseVisualStyleBackColor = true;
            this.BtnCurrencySave.Click += new System.EventHandler(this.BtnCurrencySave_Click);
            this.BtnCurrencySave.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.BtnCurrencySave_PreviewKeyDown);
            // 
            // BtnCurrencyEdit
            // 
            this.BtnCurrencyEdit.Enabled = false;
            this.BtnCurrencyEdit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCurrencyEdit.Location = new System.Drawing.Point(191, 317);
            this.BtnCurrencyEdit.Name = "BtnCurrencyEdit";
            this.BtnCurrencyEdit.Size = new System.Drawing.Size(83, 23);
            this.BtnCurrencyEdit.TabIndex = 5;
            this.BtnCurrencyEdit.Text = "Edit [F7]";
            this.BtnCurrencyEdit.UseVisualStyleBackColor = true;
            this.BtnCurrencyEdit.Click += new System.EventHandler(this.BtnCurrencyEdit_Click);
            // 
            // TextBoxCurrencyId
            // 
            this.TextBoxCurrencyId.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxCurrencyId.Location = new System.Drawing.Point(322, 319);
            this.TextBoxCurrencyId.Name = "TextBoxCurrencyId";
            this.TextBoxCurrencyId.Size = new System.Drawing.Size(67, 21);
            this.TextBoxCurrencyId.TabIndex = 5;
            this.TextBoxCurrencyId.Visible = false;
            // 
            // BtnNewCurrency
            // 
            this.BtnNewCurrency.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnNewCurrency.Location = new System.Drawing.Point(13, 317);
            this.BtnNewCurrency.Name = "BtnNewCurrency";
            this.BtnNewCurrency.Size = new System.Drawing.Size(83, 23);
            this.BtnNewCurrency.TabIndex = 3;
            this.BtnNewCurrency.Text = "New [F3]";
            this.BtnNewCurrency.UseVisualStyleBackColor = true;
            this.BtnNewCurrency.Click += new System.EventHandler(this.BtnNewCurrency_Click);
            // 
            // BtnCurrencyDelete
            // 
            this.BtnCurrencyDelete.Enabled = false;
            this.BtnCurrencyDelete.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCurrencyDelete.Location = new System.Drawing.Point(102, 317);
            this.BtnCurrencyDelete.Name = "BtnCurrencyDelete";
            this.BtnCurrencyDelete.Size = new System.Drawing.Size(83, 23);
            this.BtnCurrencyDelete.TabIndex = 4;
            this.BtnCurrencyDelete.Text = "Delete [F4]";
            this.BtnCurrencyDelete.UseVisualStyleBackColor = true;
            this.BtnCurrencyDelete.Click += new System.EventHandler(this.BtnCurrencyDelete_Click);
            // 
            // BtnCurrencyExit
            // 
            this.BtnCurrencyExit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCurrencyExit.Location = new System.Drawing.Point(634, 316);
            this.BtnCurrencyExit.Name = "BtnCurrencyExit";
            this.BtnCurrencyExit.Size = new System.Drawing.Size(83, 23);
            this.BtnCurrencyExit.TabIndex = 13;
            this.BtnCurrencyExit.Text = "Exit [F10]";
            this.BtnCurrencyExit.UseVisualStyleBackColor = true;
            this.BtnCurrencyExit.Click += new System.EventHandler(this.BtnCurrencyExit_Click);
            // 
            // FormCurrency
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(729, 345);
            this.Controls.Add(this.TextBoxCurrencyId);
            this.Controls.Add(this.BtnCurrencyExit);
            this.Controls.Add(this.BtnCurrencyDelete);
            this.Controls.Add(this.BtnNewCurrency);
            this.Controls.Add(this.GroupBoxCurrency);
            this.Controls.Add(this.TextBoxCurrencySearch);
            this.Controls.Add(this.ListBoxCurrency);
            this.Controls.Add(this.BtnCurrencySave);
            this.Controls.Add(this.BtnCurrencyCancelEdit);
            this.Controls.Add(this.BtnCurrencyEdit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCurrency";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Currency";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCurrency_FormClosing);
            this.Load += new System.EventHandler(this.FormCurrency_Load);
            this.Controls.SetChildIndex(this.ProductIdTransport, 0);
            this.Controls.SetChildIndex(this.ProductBatchIdTransport, 0);
            this.Controls.SetChildIndex(this.AccountIdTransport, 0);
            this.Controls.SetChildIndex(this.BtnCurrencyEdit, 0);
            this.Controls.SetChildIndex(this.BtnCurrencyCancelEdit, 0);
            this.Controls.SetChildIndex(this.BtnCurrencySave, 0);
            this.Controls.SetChildIndex(this.ListBoxCurrency, 0);
            this.Controls.SetChildIndex(this.TextBoxCurrencySearch, 0);
            this.Controls.SetChildIndex(this.GroupBoxCurrency, 0);
            this.Controls.SetChildIndex(this.BtnNewCurrency, 0);
            this.Controls.SetChildIndex(this.BtnCurrencyDelete, 0);
            this.Controls.SetChildIndex(this.BtnCurrencyExit, 0);
            this.Controls.SetChildIndex(this.TextBoxCurrencyId, 0);
            this.GroupBoxCurrency.ResumeLayout(false);
            this.GroupBoxCurrency.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox ListBoxCurrency;
        private System.Windows.Forms.TextBox TextBoxCurrencySearch;
        private System.Windows.Forms.GroupBox GroupBoxCurrency;
        private System.Windows.Forms.Label LabelCurrencyName;
        private System.Windows.Forms.Label LabelCurrencyDisplayas;
        private System.Windows.Forms.TextBox TextBoxCurrencyDisplayAs;
        private System.Windows.Forms.TextBox TextBoxCurrencyName;
        private System.Windows.Forms.Button BtnCurrencySave;
        private System.Windows.Forms.Button BtnCurrencyEdit;
        private System.Windows.Forms.Button BtnNewCurrency;
        private System.Windows.Forms.Button BtnCurrencyDelete;
        private System.Windows.Forms.Button BtnCurrencyExit;
        private System.Windows.Forms.Button BtnCurrencyCancelEdit;
        private System.Windows.Forms.TextBox TextBoxCurrencyId;
        private System.Windows.Forms.TextBox TextBoxISOCode;
        private System.Windows.Forms.Label LabelCurrencyPricision;
        private System.Windows.Forms.Label LabelCurrencyIsocode;
        private System.Windows.Forms.TextBox TextBoxCurrencyFormat;
        private System.Windows.Forms.Label label1;
        private controls.ComboBoxSwapTextBox ComboBoxCurrencyPricision;
    }
}