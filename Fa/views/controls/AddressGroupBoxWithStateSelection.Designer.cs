namespace fa.views.controls
{
    partial class AddressGroupBoxWithStateSelection
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
            groupBox1 = new GroupBox();
            ComboBoxSwapTextBoxState = new ComboBoxSwapTextBox();
            TextBoxAddress2 = new TextBox();
            LabelCustomerBillingAddress2 = new Label();
            TextBoxDistrict = new TextBox();
            TextBoxPin = new MaskedTextBox();
            TextBoxCity = new TextBox();
            LabelCustomerBillingPincode = new Label();
            LabelCustomerBillingState = new Label();
            LabelCustomerBillingDistrict = new Label();
            LabelCustomerBillingCityTown = new Label();
            TextBoxAddressStreet = new TextBox();
            LabelCustomerBillingAddress1 = new Label();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            groupBox1.BackColor = Color.White;
            groupBox1.Controls.Add(ComboBoxSwapTextBoxState);
            groupBox1.Controls.Add(TextBoxAddress2);
            groupBox1.Controls.Add(LabelCustomerBillingAddress2);
            groupBox1.Controls.Add(TextBoxDistrict);
            groupBox1.Controls.Add(TextBoxPin);
            groupBox1.Controls.Add(TextBoxCity);
            groupBox1.Controls.Add(LabelCustomerBillingPincode);
            groupBox1.Controls.Add(LabelCustomerBillingState);
            groupBox1.Controls.Add(LabelCustomerBillingDistrict);
            groupBox1.Controls.Add(LabelCustomerBillingCityTown);
            groupBox1.Controls.Add(TextBoxAddressStreet);
            groupBox1.Controls.Add(LabelCustomerBillingAddress1);
            groupBox1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            groupBox1.Location = new Point(5, 0);
            groupBox1.Margin = new Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 3, 4, 3);
            groupBox1.Size = new Size(527, 220);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            // 
            // ComboBoxSwapTextBoxState
            // 
            ComboBoxSwapTextBoxState.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxSwapTextBoxState.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxSwapTextBoxState.FormattingEnabled = true;
            ComboBoxSwapTextBoxState.Location = new Point(12, 176);
            ComboBoxSwapTextBoxState.Name = "ComboBoxSwapTextBoxState";
            ComboBoxSwapTextBoxState.Size = new Size(238, 21);
            ComboBoxSwapTextBoxState.TabIndex = 4;
            ComboBoxSwapTextBoxState.TxtVisible = true;
            ComboBoxSwapTextBoxState.Visible = false;
            ComboBoxSwapTextBoxState.SelectedIndexChanged += ComboBoxSwapTextBoxState_SelectedIndexChanged;
            // 
            // TextBoxAddress2
            // 
            TextBoxAddress2.BackColor = SystemColors.Window;
            TextBoxAddress2.Location = new Point(12, 83);
            TextBoxAddress2.Margin = new Padding(4, 3, 4, 3);
            TextBoxAddress2.MaxLength = 50;
            TextBoxAddress2.Multiline = true;
            TextBoxAddress2.Name = "TextBoxAddress2";
            TextBoxAddress2.ReadOnly = true;
            TextBoxAddress2.Size = new Size(492, 25);
            TextBoxAddress2.TabIndex = 1;
            TextBoxAddress2.Enter += TextBoxAddress2_Enter;
            TextBoxAddress2.KeyDown += TextBoxAddressStreet_KeyDown;
            TextBoxAddress2.KeyPress += TextBoxAddress2_KeyPress;
            TextBoxAddress2.MouseDown += TextBoxAddress2_MouseDown;
            // 
            // LabelCustomerBillingAddress2
            // 
            LabelCustomerBillingAddress2.AutoSize = true;
            LabelCustomerBillingAddress2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelCustomerBillingAddress2.Location = new Point(9, 63);
            LabelCustomerBillingAddress2.Margin = new Padding(4, 0, 4, 0);
            LabelCustomerBillingAddress2.Name = "LabelCustomerBillingAddress2";
            LabelCustomerBillingAddress2.Size = new Size(146, 13);
            LabelCustomerBillingAddress2.TabIndex = 23;
            LabelCustomerBillingAddress2.Text = "Landmark/Additional Address";
            // 
            // TextBoxDistrict
            // 
            TextBoxDistrict.BackColor = SystemColors.Window;
            TextBoxDistrict.Location = new Point(265, 132);
            TextBoxDistrict.Margin = new Padding(4, 3, 4, 3);
            TextBoxDistrict.MaxLength = 35;
            TextBoxDistrict.Name = "TextBoxDistrict";
            TextBoxDistrict.ReadOnly = true;
            TextBoxDistrict.Size = new Size(238, 21);
            TextBoxDistrict.TabIndex = 3;
            TextBoxDistrict.Enter += TextBoxDistrict_Enter;
            TextBoxDistrict.KeyDown += TextBoxAddressStreet_KeyDown;
            TextBoxDistrict.KeyPress += TextBoxAddress2_KeyPress;
            TextBoxDistrict.MouseDown += TextBoxDistrict_MouseDown;
            // 
            // TextBoxPin
            // 
            TextBoxPin.BackColor = SystemColors.Window;
            TextBoxPin.Location = new Point(265, 176);
            TextBoxPin.Margin = new Padding(4, 3, 4, 3);
            TextBoxPin.Mask = "999999";
            TextBoxPin.Name = "TextBoxPin";
            TextBoxPin.ReadOnly = true;
            TextBoxPin.Size = new Size(63, 21);
            TextBoxPin.TabIndex = 5;
            TextBoxPin.Enter += TextBoxPin_Enter;
            TextBoxPin.PreviewKeyDown += TextBoxPin_PreviewKeyDown;
            // 
            // TextBoxCity
            // 
            TextBoxCity.BackColor = SystemColors.Window;
            TextBoxCity.Location = new Point(12, 132);
            TextBoxCity.Margin = new Padding(4, 3, 4, 3);
            TextBoxCity.MaxLength = 35;
            TextBoxCity.Name = "TextBoxCity";
            TextBoxCity.ReadOnly = true;
            TextBoxCity.Size = new Size(238, 21);
            TextBoxCity.TabIndex = 2;
            TextBoxCity.TextChanged += TextBoxCity_TextChanged;
            TextBoxCity.Enter += TextBoxCity_Enter;
            TextBoxCity.KeyDown += TextBoxAddressStreet_KeyDown;
            TextBoxCity.KeyPress += TextBoxAddress2_KeyPress;
            TextBoxCity.MouseDown += TextBoxCity_MouseDown;
            // 
            // LabelCustomerBillingPincode
            // 
            LabelCustomerBillingPincode.AutoSize = true;
            LabelCustomerBillingPincode.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelCustomerBillingPincode.Location = new Point(262, 160);
            LabelCustomerBillingPincode.Margin = new Padding(4, 0, 4, 0);
            LabelCustomerBillingPincode.Name = "LabelCustomerBillingPincode";
            LabelCustomerBillingPincode.Size = new Size(44, 13);
            LabelCustomerBillingPincode.TabIndex = 21;
            LabelCustomerBillingPincode.Text = "PIN/ZIP";
            // 
            // LabelCustomerBillingState
            // 
            LabelCustomerBillingState.AutoSize = true;
            LabelCustomerBillingState.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelCustomerBillingState.Location = new Point(9, 160);
            LabelCustomerBillingState.Margin = new Padding(4, 0, 4, 0);
            LabelCustomerBillingState.Name = "LabelCustomerBillingState";
            LabelCustomerBillingState.Size = new Size(38, 13);
            LabelCustomerBillingState.TabIndex = 20;
            LabelCustomerBillingState.Text = "State";
            // 
            // LabelCustomerBillingDistrict
            // 
            LabelCustomerBillingDistrict.AutoSize = true;
            LabelCustomerBillingDistrict.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelCustomerBillingDistrict.Location = new Point(262, 112);
            LabelCustomerBillingDistrict.Margin = new Padding(4, 0, 4, 0);
            LabelCustomerBillingDistrict.Name = "LabelCustomerBillingDistrict";
            LabelCustomerBillingDistrict.Size = new Size(40, 13);
            LabelCustomerBillingDistrict.TabIndex = 19;
            LabelCustomerBillingDistrict.Text = "District";
            // 
            // LabelCustomerBillingCityTown
            // 
            LabelCustomerBillingCityTown.AutoSize = true;
            LabelCustomerBillingCityTown.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelCustomerBillingCityTown.Location = new Point(9, 112);
            LabelCustomerBillingCityTown.Margin = new Padding(4, 0, 4, 0);
            LabelCustomerBillingCityTown.Name = "LabelCustomerBillingCityTown";
            LabelCustomerBillingCityTown.Size = new Size(56, 13);
            LabelCustomerBillingCityTown.TabIndex = 18;
            LabelCustomerBillingCityTown.Text = "City/Town";
            // 
            // TextBoxAddressStreet
            // 
            TextBoxAddressStreet.BackColor = SystemColors.Window;
            TextBoxAddressStreet.Location = new Point(12, 33);
            TextBoxAddressStreet.Margin = new Padding(4, 3, 4, 3);
            TextBoxAddressStreet.MaxLength = 50;
            TextBoxAddressStreet.Multiline = true;
            TextBoxAddressStreet.Name = "TextBoxAddressStreet";
            TextBoxAddressStreet.ReadOnly = true;
            TextBoxAddressStreet.Size = new Size(492, 25);
            TextBoxAddressStreet.TabIndex = 0;
            TextBoxAddressStreet.Enter += TextBoxAddressStreet_Enter;
            TextBoxAddressStreet.KeyDown += TextBoxAddressStreet_KeyDown;
            TextBoxAddressStreet.KeyPress += TextBoxAddress1_KeyPress;
            TextBoxAddressStreet.MouseDown += TextBoxAddress1_MouseDown;
            TextBoxAddressStreet.PreviewKeyDown += TextBoxAddress1_PreviewKeyDown;
            // 
            // LabelCustomerBillingAddress1
            // 
            LabelCustomerBillingAddress1.AutoSize = true;
            LabelCustomerBillingAddress1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelCustomerBillingAddress1.Location = new Point(9, 14);
            LabelCustomerBillingAddress1.Margin = new Padding(4, 0, 4, 0);
            LabelCustomerBillingAddress1.Name = "LabelCustomerBillingAddress1";
            LabelCustomerBillingAddress1.Size = new Size(37, 13);
            LabelCustomerBillingAddress1.TabIndex = 17;
            LabelCustomerBillingAddress1.Text = "Street";
            // 
            // AddressGroupBoxWithStateSelection
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupBox1);
            Name = "AddressGroupBoxWithStateSelection";
            Size = new Size(536, 220);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private fa.views.controls.ComboBoxSwapTextBox ComboBoxSwapTextBoxState;
        private TextBox TextBoxAddress2;
        private Label LabelCustomerBillingAddress2;
        private TextBox TextBoxDistrict;
        private MaskedTextBox TextBoxPin;
        private TextBox TextBoxCity;
        private Label LabelCustomerBillingPincode;
        private Label LabelCustomerBillingState;
        private Label LabelCustomerBillingDistrict;
        private Label LabelCustomerBillingCityTown;
        private TextBox TextBoxAddressStreet;
        private Label LabelCustomerBillingAddress1;
    }
}
