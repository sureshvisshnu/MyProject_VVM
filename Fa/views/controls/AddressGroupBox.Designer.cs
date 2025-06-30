namespace fa.views.controls
{
    partial class AddressGroupBox
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TextBoxAddress2 = new System.Windows.Forms.TextBox();
            this.LabelCustomerBillingAddress2 = new System.Windows.Forms.Label();
            this.TextBoxState = new System.Windows.Forms.TextBox();
            this.TextBoxDistrict = new System.Windows.Forms.TextBox();
            this.TextBoxPin = new System.Windows.Forms.MaskedTextBox();
            this.TextBoxCity = new System.Windows.Forms.TextBox();
            this.LabelCustomerBillingPincode = new System.Windows.Forms.Label();
            this.LabelCustomerBillingState = new System.Windows.Forms.Label();
            this.LabelCustomerBillingDistrict = new System.Windows.Forms.Label();
            this.LabelCustomerBillingCityTown = new System.Windows.Forms.Label();
            this.TextBoxAddress1 = new System.Windows.Forms.TextBox();
            this.LabelCustomerBillingAddress1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.TextBoxAddress2);
            this.groupBox1.Controls.Add(this.LabelCustomerBillingAddress2);
            this.groupBox1.Controls.Add(this.TextBoxState);
            this.groupBox1.Controls.Add(this.TextBoxDistrict);
            this.groupBox1.Controls.Add(this.TextBoxPin);
            this.groupBox1.Controls.Add(this.TextBoxCity);
            this.groupBox1.Controls.Add(this.LabelCustomerBillingPincode);
            this.groupBox1.Controls.Add(this.LabelCustomerBillingState);
            this.groupBox1.Controls.Add(this.LabelCustomerBillingDistrict);
            this.groupBox1.Controls.Add(this.LabelCustomerBillingCityTown);
            this.groupBox1.Controls.Add(this.TextBoxAddress1);
            this.groupBox1.Controls.Add(this.LabelCustomerBillingAddress1);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(4, -3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(452, 191);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // TextBoxAddress2
            // 
            this.TextBoxAddress2.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxAddress2.Location = new System.Drawing.Point(10, 72);
            this.TextBoxAddress2.MaxLength = 50;
            this.TextBoxAddress2.Multiline = true;
            this.TextBoxAddress2.Name = "TextBoxAddress2";
            this.TextBoxAddress2.ReadOnly = true;
            this.TextBoxAddress2.Size = new System.Drawing.Size(422, 22);
            this.TextBoxAddress2.TabIndex = 1;
            this.TextBoxAddress2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxStreet_KeyDown);
            this.TextBoxAddress2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxState_KeyPress);
            this.TextBoxAddress2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TextBoxAddress2_MouseDown);
            // 
            // LabelCustomerBillingAddress2
            // 
            this.LabelCustomerBillingAddress2.AutoSize = true;
            this.LabelCustomerBillingAddress2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCustomerBillingAddress2.Location = new System.Drawing.Point(7, 55);
            this.LabelCustomerBillingAddress2.Name = "LabelCustomerBillingAddress2";
            this.LabelCustomerBillingAddress2.Size = new System.Drawing.Size(146, 13);
            this.LabelCustomerBillingAddress2.TabIndex = 23;
            this.LabelCustomerBillingAddress2.Text = "Landmark/Additional Address";
            // 
            // TextBoxState
            // 
            this.TextBoxState.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxState.Location = new System.Drawing.Point(10, 156);
            this.TextBoxState.MaxLength = 35;
            this.TextBoxState.Name = "TextBoxState";
            this.TextBoxState.ReadOnly = true;
            this.TextBoxState.Size = new System.Drawing.Size(205, 21);
            this.TextBoxState.TabIndex = 4;
            this.TextBoxState.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxStreet_KeyDown);
            this.TextBoxState.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxState_KeyPress);
            this.TextBoxState.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TextBoxState_MouseDown);
            // 
            // TextBoxDistrict
            // 
            this.TextBoxDistrict.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxDistrict.Location = new System.Drawing.Point(227, 114);
            this.TextBoxDistrict.MaxLength = 35;
            this.TextBoxDistrict.Name = "TextBoxDistrict";
            this.TextBoxDistrict.ReadOnly = true;
            this.TextBoxDistrict.Size = new System.Drawing.Size(205, 21);
            this.TextBoxDistrict.TabIndex = 3;
            this.TextBoxDistrict.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxStreet_KeyDown);
            this.TextBoxDistrict.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxState_KeyPress);
            this.TextBoxDistrict.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TextBoxDistrict_MouseDown);
            // 
            // TextBoxPin
            // 
            this.TextBoxPin.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxPin.Location = new System.Drawing.Point(227, 156);
            this.TextBoxPin.Mask = "999999";
            this.TextBoxPin.Name = "TextBoxPin";
            this.TextBoxPin.ReadOnly = true;
            this.TextBoxPin.Size = new System.Drawing.Size(55, 21);
            this.TextBoxPin.TabIndex = 5;
            this.TextBoxPin.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TextBoxPin_PreviewKeyDown);
            // 
            // TextBoxCity
            // 
            this.TextBoxCity.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxCity.Location = new System.Drawing.Point(10, 114);
            this.TextBoxCity.MaxLength = 35;
            this.TextBoxCity.Name = "TextBoxCity";
            this.TextBoxCity.ReadOnly = true;
            this.TextBoxCity.Size = new System.Drawing.Size(205, 21);
            this.TextBoxCity.TabIndex = 2;
            this.TextBoxCity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxStreet_KeyDown);
            this.TextBoxCity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxState_KeyPress);
            this.TextBoxCity.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TextBoxCity_MouseDown);
            // 
            // LabelCustomerBillingPincode
            // 
            this.LabelCustomerBillingPincode.AutoSize = true;
            this.LabelCustomerBillingPincode.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCustomerBillingPincode.Location = new System.Drawing.Point(224, 139);
            this.LabelCustomerBillingPincode.Name = "LabelCustomerBillingPincode";
            this.LabelCustomerBillingPincode.Size = new System.Drawing.Size(44, 13);
            this.LabelCustomerBillingPincode.TabIndex = 21;
            this.LabelCustomerBillingPincode.Text = "PIN/ZIP";
            // 
            // LabelCustomerBillingState
            // 
            this.LabelCustomerBillingState.AutoSize = true;
            this.LabelCustomerBillingState.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCustomerBillingState.Location = new System.Drawing.Point(7, 139);
            this.LabelCustomerBillingState.Name = "LabelCustomerBillingState";
            this.LabelCustomerBillingState.Size = new System.Drawing.Size(33, 13);
            this.LabelCustomerBillingState.TabIndex = 20;
            this.LabelCustomerBillingState.Text = "State";
            // 
            // LabelCustomerBillingDistrict
            // 
            this.LabelCustomerBillingDistrict.AutoSize = true;
            this.LabelCustomerBillingDistrict.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCustomerBillingDistrict.Location = new System.Drawing.Point(224, 97);
            this.LabelCustomerBillingDistrict.Name = "LabelCustomerBillingDistrict";
            this.LabelCustomerBillingDistrict.Size = new System.Drawing.Size(40, 13);
            this.LabelCustomerBillingDistrict.TabIndex = 19;
            this.LabelCustomerBillingDistrict.Text = "District";
            // 
            // LabelCustomerBillingCityTown
            // 
            this.LabelCustomerBillingCityTown.AutoSize = true;
            this.LabelCustomerBillingCityTown.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCustomerBillingCityTown.Location = new System.Drawing.Point(7, 97);
            this.LabelCustomerBillingCityTown.Name = "LabelCustomerBillingCityTown";
            this.LabelCustomerBillingCityTown.Size = new System.Drawing.Size(56, 13);
            this.LabelCustomerBillingCityTown.TabIndex = 18;
            this.LabelCustomerBillingCityTown.Text = "City/Town";
            // 
            // TextBoxAddress1
            // 
            this.TextBoxAddress1.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxAddress1.Location = new System.Drawing.Point(10, 29);
            this.TextBoxAddress1.MaxLength = 50;
            this.TextBoxAddress1.Multiline = true;
            this.TextBoxAddress1.Name = "TextBoxAddress1";
            this.TextBoxAddress1.ReadOnly = true;
            this.TextBoxAddress1.Size = new System.Drawing.Size(422, 22);
            this.TextBoxAddress1.TabIndex = 0;
            this.TextBoxAddress1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxStreet_KeyDown);
            this.TextBoxAddress1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxStreet_KeyPress);
            this.TextBoxAddress1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TextBoxStreet_MouseDown);
            this.TextBoxAddress1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TextBoxStreet_PreviewKeyDown);
            // 
            // LabelCustomerBillingAddress1
            // 
            this.LabelCustomerBillingAddress1.AutoSize = true;
            this.LabelCustomerBillingAddress1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCustomerBillingAddress1.Location = new System.Drawing.Point(7, 12);
            this.LabelCustomerBillingAddress1.Name = "LabelCustomerBillingAddress1";
            this.LabelCustomerBillingAddress1.Size = new System.Drawing.Size(37, 13);
            this.LabelCustomerBillingAddress1.TabIndex = 17;
            this.LabelCustomerBillingAddress1.Text = "Street";
            // 
            // AddressGroupBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "AddressGroupBox";
            this.Size = new System.Drawing.Size(459, 191);
            this.BackColorChanged += new System.EventHandler(this.AddressGroupBox_BackColorChanged);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.MaskedTextBox TextBoxPin;
        private System.Windows.Forms.TextBox TextBoxCity;
        private System.Windows.Forms.Label LabelCustomerBillingPincode;
        private System.Windows.Forms.Label LabelCustomerBillingState;
        private System.Windows.Forms.Label LabelCustomerBillingDistrict;
        private System.Windows.Forms.Label LabelCustomerBillingCityTown;
        private System.Windows.Forms.Label LabelCustomerBillingAddress1;
        private System.Windows.Forms.TextBox TextBoxAddress1;
        private System.Windows.Forms.TextBox TextBoxDistrict;
        private System.Windows.Forms.TextBox TextBoxState;
        private System.Windows.Forms.TextBox TextBoxAddress2;
        private System.Windows.Forms.Label LabelCustomerBillingAddress2;
    }
}
