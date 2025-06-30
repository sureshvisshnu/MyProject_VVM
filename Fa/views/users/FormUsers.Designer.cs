namespace fa.views.users
{
    partial class FormUsers
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUsers));
            ImageListUserTreeViewAccess = new ImageList(components);
            statusStrip1 = new StatusStrip();
            UserFormErrorMsg = new ToolStripStatusLabel();
            TextBoxUserId = new TextBox();
            BtnUserCancel = new Button();
            TabControlUser = new TabControl();
            TabUserDetails = new TabPage();
            CheckBoxUserLockUser = new CheckBox();
            CheckedListBoxUser = new CheckedListBox();
            TextBoxUserReenterPassword = new TextBox();
            TextBoxUserPassword = new TextBox();
            TextBoxUserLogin = new TextBox();
            TextBoxUserLastName = new TextBox();
            CheckBoxUserResetPassword = new CheckBox();
            TextBoxUserFirstName = new TextBox();
            label7 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            TabContactInfo = new TabPage();
            AddressGroupBoxUser = new controls.AddressGroupBoxWithStateSelection();
            label12 = new Label();
            TextBoxUserMobile = new controls.text.PhoneTextBox();
            TextBoxUserPhone = new controls.text.PhoneTextBox();
            TextBoxUserFax = new MaskedTextBox();
            TextBoxUserEmail = new TextBox();
            label11 = new Label();
            label10 = new Label();
            label9 = new Label();
            label8 = new Label();
            TabTaxDetails = new TabPage();
            TextBoxUserPan = new TextBox();
            label17 = new Label();
            TabAccess = new TabPage();
            TreeViewUserAccess = new TreeView();
            label6 = new Label();
            TabEmployee = new TabPage();
            TextBoxSearchEmployee = new TextBox();
            TreeViewEmployeeUser = new TreeView();
            BtnUserSave = new Button();
            BtnUserEdit = new Button();
            BtnUserExit = new Button();
            BtnUserDelete = new Button();
            BtnUserNew = new Button();
            ListBoxUser = new ListBox();
            TextBoxUserSearch = new controls.text.DelayedTextChangeTextBox();
            statusStrip1.SuspendLayout();
            TabControlUser.SuspendLayout();
            TabUserDetails.SuspendLayout();
            TabContactInfo.SuspendLayout();
            TabTaxDetails.SuspendLayout();
            TabAccess.SuspendLayout();
            TabEmployee.SuspendLayout();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(295, 416);
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(387, 431);
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // ImageListUserTreeViewAccess
            // 
            ImageListUserTreeViewAccess.ColorDepth = ColorDepth.Depth8Bit;
            ImageListUserTreeViewAccess.ImageStream = (ImageListStreamer)resources.GetObject("ImageListUserTreeViewAccess.ImageStream");
            ImageListUserTreeViewAccess.TransparentColor = Color.Transparent;
            ImageListUserTreeViewAccess.Images.SetKeyName(0, "company2.ico");
            ImageListUserTreeViewAccess.Images.SetKeyName(1, "CostCenter.ico");
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { UserFormErrorMsg });
            statusStrip1.Location = new Point(0, 468);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(803, 22);
            statusStrip1.TabIndex = 39;
            statusStrip1.Text = "statusStrip1";
            // 
            // UserFormErrorMsg
            // 
            UserFormErrorMsg.Name = "UserFormErrorMsg";
            UserFormErrorMsg.Size = new Size(0, 17);
            // 
            // TextBoxUserId
            // 
            TextBoxUserId.Location = new Point(295, 433);
            TextBoxUserId.Name = "TextBoxUserId";
            TextBoxUserId.Size = new Size(127, 21);
            TextBoxUserId.TabIndex = 18;
            TextBoxUserId.Visible = false;
            // 
            // BtnUserCancel
            // 
            BtnUserCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnUserCancel.Location = new Point(526, 431);
            BtnUserCancel.Name = "BtnUserCancel";
            BtnUserCancel.Size = new Size(83, 23);
            BtnUserCancel.TabIndex = 29;
            BtnUserCancel.Text = "Cancel [Esc]";
            BtnUserCancel.UseVisualStyleBackColor = true;
            BtnUserCancel.Click += BtnUserCancel_Click;
            // 
            // TabControlUser
            // 
            TabControlUser.Controls.Add(TabUserDetails);
            TabControlUser.Controls.Add(TabContactInfo);
            TabControlUser.Controls.Add(TabTaxDetails);
            TabControlUser.Controls.Add(TabAccess);
            TabControlUser.Controls.Add(TabEmployee);
            TabControlUser.Location = new Point(262, 12);
            TabControlUser.Name = "TabControlUser";
            TabControlUser.SelectedIndex = 0;
            TabControlUser.Size = new Size(530, 401);
            TabControlUser.TabIndex = 5;
            TabControlUser.TabStop = false;
            // 
            // TabUserDetails
            // 
            TabUserDetails.BackColor = SystemColors.Window;
            TabUserDetails.Controls.Add(CheckBoxUserLockUser);
            TabUserDetails.Controls.Add(CheckedListBoxUser);
            TabUserDetails.Controls.Add(TextBoxUserReenterPassword);
            TabUserDetails.Controls.Add(TextBoxUserPassword);
            TabUserDetails.Controls.Add(TextBoxUserLogin);
            TabUserDetails.Controls.Add(TextBoxUserLastName);
            TabUserDetails.Controls.Add(CheckBoxUserResetPassword);
            TabUserDetails.Controls.Add(TextBoxUserFirstName);
            TabUserDetails.Controls.Add(label7);
            TabUserDetails.Controls.Add(label5);
            TabUserDetails.Controls.Add(label4);
            TabUserDetails.Controls.Add(label3);
            TabUserDetails.Controls.Add(label2);
            TabUserDetails.Controls.Add(label1);
            TabUserDetails.Location = new Point(4, 22);
            TabUserDetails.Name = "TabUserDetails";
            TabUserDetails.Padding = new Padding(3);
            TabUserDetails.Size = new Size(522, 375);
            TabUserDetails.TabIndex = 0;
            TabUserDetails.Text = "User Details";
            // 
            // CheckBoxUserLockUser
            // 
            CheckBoxUserLockUser.AutoSize = true;
            CheckBoxUserLockUser.Location = new Point(188, 210);
            CheckBoxUserLockUser.Name = "CheckBoxUserLockUser";
            CheckBoxUserLockUser.Size = new Size(72, 17);
            CheckBoxUserLockUser.TabIndex = 13;
            CheckBoxUserLockUser.Text = "Lock User";
            CheckBoxUserLockUser.UseVisualStyleBackColor = true;
            // 
            // CheckedListBoxUser
            // 
            CheckedListBoxUser.CheckOnClick = true;
            CheckedListBoxUser.FormattingEnabled = true;
            CheckedListBoxUser.Location = new Point(17, 246);
            CheckedListBoxUser.Name = "CheckedListBoxUser";
            CheckedListBoxUser.Size = new Size(365, 84);
            CheckedListBoxUser.TabIndex = 14;
            CheckedListBoxUser.ItemCheck += CheckedListBoxUser_ItemCheck;
            CheckedListBoxUser.PreviewKeyDown += CheckedListBoxUser_PreviewKeyDown;
            // 
            // TextBoxUserReenterPassword
            // 
            TextBoxUserReenterPassword.BackColor = SystemColors.Window;
            TextBoxUserReenterPassword.Location = new Point(17, 183);
            TextBoxUserReenterPassword.MaxLength = 50;
            TextBoxUserReenterPassword.Name = "TextBoxUserReenterPassword";
            TextBoxUserReenterPassword.ReadOnly = true;
            TextBoxUserReenterPassword.Size = new Size(177, 21);
            TextBoxUserReenterPassword.TabIndex = 11;
            TextBoxUserReenterPassword.UseSystemPasswordChar = true;
            TextBoxUserReenterPassword.KeyDown += TextBoxUserFirstName_KeyDown;
            TextBoxUserReenterPassword.KeyPress += TextBoxUserReenterPassword_KeyPress;
            TextBoxUserReenterPassword.MouseDown += TextBoxUserReenterPassword_MouseDown;
            // 
            // TextBoxUserPassword
            // 
            TextBoxUserPassword.BackColor = SystemColors.Window;
            TextBoxUserPassword.Location = new Point(17, 143);
            TextBoxUserPassword.MaxLength = 50;
            TextBoxUserPassword.Name = "TextBoxUserPassword";
            TextBoxUserPassword.ReadOnly = true;
            TextBoxUserPassword.Size = new Size(177, 21);
            TextBoxUserPassword.TabIndex = 10;
            TextBoxUserPassword.UseSystemPasswordChar = true;
            TextBoxUserPassword.KeyDown += TextBoxUserFirstName_KeyDown;
            TextBoxUserPassword.KeyPress += TextBoxUserPassword_KeyPress;
            TextBoxUserPassword.MouseDown += TextBoxUserPassword_MouseDown;
            // 
            // TextBoxUserLogin
            // 
            TextBoxUserLogin.BackColor = SystemColors.Window;
            TextBoxUserLogin.Location = new Point(17, 103);
            TextBoxUserLogin.MaxLength = 30;
            TextBoxUserLogin.Name = "TextBoxUserLogin";
            TextBoxUserLogin.ReadOnly = true;
            TextBoxUserLogin.Size = new Size(258, 21);
            TextBoxUserLogin.TabIndex = 9;
            TextBoxUserLogin.KeyDown += TextBoxUserLogin_KeyDown;
            TextBoxUserLogin.KeyPress += TextBoxUserLogin_KeyPress;
            TextBoxUserLogin.MouseDown += TextBoxUserLogin_MouseDown;
            // 
            // TextBoxUserLastName
            // 
            TextBoxUserLastName.BackColor = SystemColors.Window;
            TextBoxUserLastName.Location = new Point(17, 63);
            TextBoxUserLastName.MaxLength = 30;
            TextBoxUserLastName.Name = "TextBoxUserLastName";
            TextBoxUserLastName.ReadOnly = true;
            TextBoxUserLastName.Size = new Size(258, 21);
            TextBoxUserLastName.TabIndex = 8;
            TextBoxUserLastName.KeyDown += TextBoxUserFirstName_KeyDown;
            TextBoxUserLastName.KeyPress += TextBoxUserLastName_KeyPress;
            TextBoxUserLastName.MouseDown += TextBoxUserLastName_MouseDown;
            // 
            // CheckBoxUserResetPassword
            // 
            CheckBoxUserResetPassword.AutoSize = true;
            CheckBoxUserResetPassword.Location = new Point(17, 210);
            CheckBoxUserResetPassword.Name = "CheckBoxUserResetPassword";
            CheckBoxUserResetPassword.Size = new Size(168, 17);
            CheckBoxUserResetPassword.TabIndex = 12;
            CheckBoxUserResetPassword.Text = "Reset password on next login";
            CheckBoxUserResetPassword.UseVisualStyleBackColor = true;
            // 
            // TextBoxUserFirstName
            // 
            TextBoxUserFirstName.AllowDrop = true;
            TextBoxUserFirstName.BackColor = SystemColors.Window;
            TextBoxUserFirstName.Location = new Point(17, 23);
            TextBoxUserFirstName.MaxLength = 30;
            TextBoxUserFirstName.Name = "TextBoxUserFirstName";
            TextBoxUserFirstName.ReadOnly = true;
            TextBoxUserFirstName.Size = new Size(258, 21);
            TextBoxUserFirstName.TabIndex = 7;
            TextBoxUserFirstName.KeyDown += TextBoxUserFirstName_KeyDown;
            TextBoxUserFirstName.KeyPress += TextBoxUserFirstName_KeyPress;
            TextBoxUserFirstName.MouseDown += TextBoxUserFirstName_MouseDown;
            TextBoxUserFirstName.PreviewKeyDown += TextBoxUserFirstName_PreviewKeyDown;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label7.Location = new Point(14, 230);
            label7.Name = "label7";
            label7.Size = new Size(38, 13);
            label7.TabIndex = 26;
            label7.Text = "Roles";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label5.Location = new Point(14, 167);
            label5.Name = "label5";
            label5.Size = new Size(110, 13);
            label5.TabIndex = 19;
            label5.Text = "Reenter Password";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(14, 127);
            label4.Name = "label4";
            label4.Size = new Size(61, 13);
            label4.TabIndex = 18;
            label4.Text = "Password";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(14, 87);
            label3.Name = "label3";
            label3.Size = new Size(37, 13);
            label3.TabIndex = 17;
            label3.Text = "Login";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(14, 47);
            label2.Name = "label2";
            label2.Size = new Size(66, 13);
            label2.TabIndex = 16;
            label2.Text = "Last Name";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(14, 7);
            label1.Name = "label1";
            label1.Size = new Size(67, 13);
            label1.TabIndex = 15;
            label1.Text = "First Name";
            // 
            // TabContactInfo
            // 
            TabContactInfo.BackColor = SystemColors.Window;
            TabContactInfo.Controls.Add(AddressGroupBoxUser);
            TabContactInfo.Controls.Add(label12);
            TabContactInfo.Controls.Add(TextBoxUserMobile);
            TabContactInfo.Controls.Add(TextBoxUserPhone);
            TabContactInfo.Controls.Add(TextBoxUserFax);
            TabContactInfo.Controls.Add(TextBoxUserEmail);
            TabContactInfo.Controls.Add(label11);
            TabContactInfo.Controls.Add(label10);
            TabContactInfo.Controls.Add(label9);
            TabContactInfo.Controls.Add(label8);
            TabContactInfo.Location = new Point(4, 24);
            TabContactInfo.Name = "TabContactInfo";
            TabContactInfo.Padding = new Padding(3);
            TabContactInfo.Size = new Size(522, 373);
            TabContactInfo.TabIndex = 1;
            TabContactInfo.Text = "Contact Info";
            // 
            // AddressGroupBoxUser
            // 
            AddressGroupBoxUser.AddressLine1 = "";
            AddressGroupBoxUser.AddressLine2 = "";
            AddressGroupBoxUser.CityName = "";
            AddressGroupBoxUser.CountryId = 0L;
            AddressGroupBoxUser.DistrictName = "";
            AddressGroupBoxUser.GroupName = "";
            AddressGroupBoxUser.Location = new Point(9, 18);
            AddressGroupBoxUser.Name = "AddressGroupBoxUser";
            AddressGroupBoxUser.PinCode = "";
            AddressGroupBoxUser.ReadOnly = true;
            AddressGroupBoxUser.Size = new Size(459, 194);
            AddressGroupBoxUser.StateId = 0L;
            AddressGroupBoxUser.StateName = "";
            AddressGroupBoxUser.TabIndex = 15;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(12, 7);
            label12.Name = "label12";
            label12.Size = new Size(46, 13);
            label12.TabIndex = 56;
            label12.Text = "Address";
            // 
            // TextBoxUserMobile
            // 
            TextBoxUserMobile.AreaCodeLength = 5;
            TextBoxUserMobile.BackColor = SystemColors.Window;
            TextBoxUserMobile.Length = 13;
            TextBoxUserMobile.Location = new Point(15, 268);
            TextBoxUserMobile.Mask = "#####-#######";
            TextBoxUserMobile.Name = "TextBoxUserMobile";
            TextBoxUserMobile.ReadOnly = true;
            TextBoxUserMobile.Size = new Size(100, 21);
            TextBoxUserMobile.TabIndex = 21;
            // 
            // TextBoxUserPhone
            // 
            TextBoxUserPhone.AreaCodeLength = 5;
            TextBoxUserPhone.BackColor = SystemColors.Window;
            TextBoxUserPhone.Length = 13;
            TextBoxUserPhone.Location = new Point(15, 228);
            TextBoxUserPhone.Mask = "99999-9999999";
            TextBoxUserPhone.Name = "TextBoxUserPhone";
            TextBoxUserPhone.ReadOnly = true;
            TextBoxUserPhone.Size = new Size(100, 21);
            TextBoxUserPhone.TabIndex = 20;
            TextBoxUserPhone.PreviewKeyDown += TextBoxUserPhone_PreviewKeyDown;
            // 
            // TextBoxUserFax
            // 
            TextBoxUserFax.BackColor = SystemColors.Window;
            TextBoxUserFax.Location = new Point(15, 308);
            TextBoxUserFax.Mask = "99999999999999999999";
            TextBoxUserFax.Name = "TextBoxUserFax";
            TextBoxUserFax.ReadOnly = true;
            TextBoxUserFax.Size = new Size(165, 21);
            TextBoxUserFax.TabIndex = 22;
            // 
            // TextBoxUserEmail
            // 
            TextBoxUserEmail.BackColor = SystemColors.Window;
            TextBoxUserEmail.Location = new Point(15, 348);
            TextBoxUserEmail.MaxLength = 50;
            TextBoxUserEmail.Name = "TextBoxUserEmail";
            TextBoxUserEmail.ReadOnly = true;
            TextBoxUserEmail.Size = new Size(265, 21);
            TextBoxUserEmail.TabIndex = 23;
            TextBoxUserEmail.KeyDown += TextBoxUserEmail_KeyDown;
            TextBoxUserEmail.KeyPress += TextBoxUserEmail_KeyPress;
            TextBoxUserEmail.MouseDown += TextBoxUserEmail_MouseDown;
            TextBoxUserEmail.PreviewKeyDown += TextBoxUserEmail_PreviewKeyDown;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(12, 332);
            label11.Name = "label11";
            label11.Size = new Size(31, 13);
            label11.TabIndex = 7;
            label11.Text = "Email";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(12, 292);
            label10.Name = "label10";
            label10.Size = new Size(25, 13);
            label10.TabIndex = 3;
            label10.Text = "Fax";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(12, 252);
            label9.Name = "label9";
            label9.Size = new Size(37, 13);
            label9.TabIndex = 2;
            label9.Text = "Mobile";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(12, 212);
            label8.Name = "label8";
            label8.Size = new Size(37, 13);
            label8.TabIndex = 1;
            label8.Text = "Phone";
            // 
            // TabTaxDetails
            // 
            TabTaxDetails.BackColor = SystemColors.Window;
            TabTaxDetails.Controls.Add(TextBoxUserPan);
            TabTaxDetails.Controls.Add(label17);
            TabTaxDetails.Location = new Point(4, 24);
            TabTaxDetails.Name = "TabTaxDetails";
            TabTaxDetails.Size = new Size(522, 373);
            TabTaxDetails.TabIndex = 2;
            TabTaxDetails.Text = "Tax Info";
            // 
            // TextBoxUserPan
            // 
            TextBoxUserPan.BackColor = SystemColors.Window;
            TextBoxUserPan.Location = new Point(16, 25);
            TextBoxUserPan.MaxLength = 20;
            TextBoxUserPan.Name = "TextBoxUserPan";
            TextBoxUserPan.ReadOnly = true;
            TextBoxUserPan.Size = new Size(218, 21);
            TextBoxUserPan.TabIndex = 24;
            TextBoxUserPan.KeyDown += TextBoxUserPan_KeyDown;
            TextBoxUserPan.KeyPress += TextBoxUserPan_KeyPress;
            TextBoxUserPan.MouseDown += TextBoxUserPan_MouseDown;
            TextBoxUserPan.PreviewKeyDown += TextBoxUserPan_PreviewKeyDown;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(13, 8);
            label17.Name = "label17";
            label17.Size = new Size(27, 13);
            label17.TabIndex = 1;
            label17.Text = "PAN";
            // 
            // TabAccess
            // 
            TabAccess.BackColor = SystemColors.Window;
            TabAccess.Controls.Add(TreeViewUserAccess);
            TabAccess.Controls.Add(label6);
            TabAccess.Location = new Point(4, 24);
            TabAccess.Margin = new Padding(2);
            TabAccess.Name = "TabAccess";
            TabAccess.Size = new Size(522, 373);
            TabAccess.TabIndex = 3;
            TabAccess.Text = "Access";
            // 
            // TreeViewUserAccess
            // 
            TreeViewUserAccess.CheckBoxes = true;
            TreeViewUserAccess.ImageIndex = 0;
            TreeViewUserAccess.ImageList = ImageListUserTreeViewAccess;
            TreeViewUserAccess.Location = new Point(15, 24);
            TreeViewUserAccess.Margin = new Padding(2);
            TreeViewUserAccess.Name = "TreeViewUserAccess";
            TreeViewUserAccess.SelectedImageIndex = 0;
            TreeViewUserAccess.Size = new Size(490, 306);
            TreeViewUserAccess.TabIndex = 25;
            TreeViewUserAccess.AfterCheck += TreeViewUserAccess_AfterCheck;
            TreeViewUserAccess.AfterSelect += TreeViewUserAccess_AfterSelect;
            TreeViewUserAccess.PreviewKeyDown += TreeViewUserAccess_PreviewKeyDown;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 6);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(40, 13);
            label6.TabIndex = 0;
            label6.Text = "Access";
            // 
            // TabEmployee
            // 
            TabEmployee.Controls.Add(TextBoxSearchEmployee);
            TabEmployee.Controls.Add(TreeViewEmployeeUser);
            TabEmployee.Location = new Point(4, 24);
            TabEmployee.Name = "TabEmployee";
            TabEmployee.Padding = new Padding(3);
            TabEmployee.Size = new Size(522, 373);
            TabEmployee.TabIndex = 4;
            TabEmployee.Text = "Link Employee";
            TabEmployee.UseVisualStyleBackColor = true;
            // 
            // TextBoxSearchEmployee
            // 
            TextBoxSearchEmployee.BackColor = Color.White;
            TextBoxSearchEmployee.Location = new Point(18, 16);
            TextBoxSearchEmployee.Name = "TextBoxSearchEmployee";
            TextBoxSearchEmployee.Size = new Size(485, 21);
            TextBoxSearchEmployee.TabIndex = 26;
            TextBoxSearchEmployee.TextChanged += TextBoxSearchEmployee_TextChanged;
            TextBoxSearchEmployee.PreviewKeyDown += TextBoxSearchEmployee_PreviewKeyDown;
            // 
            // TreeViewEmployeeUser
            // 
            TreeViewEmployeeUser.CheckBoxes = true;
            TreeViewEmployeeUser.Location = new Point(18, 54);
            TreeViewEmployeeUser.Name = "TreeViewEmployeeUser";
            TreeViewEmployeeUser.Size = new Size(485, 304);
            TreeViewEmployeeUser.TabIndex = 27;
            TreeViewEmployeeUser.AfterCheck += TreeViewEmployeeUser_AfterCheck;
            TreeViewEmployeeUser.PreviewKeyDown += TreeViewEmployeeUser_PreviewKeyDown;
            // 
            // BtnUserSave
            // 
            BtnUserSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnUserSave.Location = new Point(615, 431);
            BtnUserSave.Name = "BtnUserSave";
            BtnUserSave.Size = new Size(83, 23);
            BtnUserSave.TabIndex = 28;
            BtnUserSave.Text = "Save [F8]";
            BtnUserSave.UseVisualStyleBackColor = true;
            BtnUserSave.Click += BtnUserSave_Click;
            BtnUserSave.PreviewKeyDown += BtnUserSave_PreviewKeyDown;
            // 
            // BtnUserEdit
            // 
            BtnUserEdit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnUserEdit.Location = new Point(192, 431);
            BtnUserEdit.Name = "BtnUserEdit";
            BtnUserEdit.Size = new Size(83, 23);
            BtnUserEdit.TabIndex = 5;
            BtnUserEdit.Text = "Edit [F7]";
            BtnUserEdit.UseVisualStyleBackColor = true;
            BtnUserEdit.Click += BtnUserEdit_Click;
            // 
            // BtnUserExit
            // 
            BtnUserExit.DialogResult = DialogResult.Cancel;
            BtnUserExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnUserExit.Location = new Point(704, 431);
            BtnUserExit.Name = "BtnUserExit";
            BtnUserExit.Size = new Size(83, 23);
            BtnUserExit.TabIndex = 6;
            BtnUserExit.Text = "Exit [F10]";
            BtnUserExit.UseVisualStyleBackColor = true;
            BtnUserExit.Click += BtnUserExit_Click;
            // 
            // BtnUserDelete
            // 
            BtnUserDelete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnUserDelete.Location = new Point(103, 431);
            BtnUserDelete.Name = "BtnUserDelete";
            BtnUserDelete.Size = new Size(83, 23);
            BtnUserDelete.TabIndex = 4;
            BtnUserDelete.Text = "Delete [F4]";
            BtnUserDelete.UseVisualStyleBackColor = true;
            BtnUserDelete.Click += BtnUserDelete_Click;
            // 
            // BtnUserNew
            // 
            BtnUserNew.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnUserNew.Location = new Point(14, 431);
            BtnUserNew.Name = "BtnUserNew";
            BtnUserNew.Size = new Size(83, 23);
            BtnUserNew.TabIndex = 3;
            BtnUserNew.Text = "New [F3]";
            BtnUserNew.UseVisualStyleBackColor = true;
            BtnUserNew.Click += BtnUserNew_Click;
            // 
            // ListBoxUser
            // 
            ListBoxUser.FormattingEnabled = true;
            ListBoxUser.Location = new Point(14, 40);
            ListBoxUser.Name = "ListBoxUser";
            ListBoxUser.Size = new Size(239, 368);
            ListBoxUser.TabIndex = 2;
            ListBoxUser.Click += ListBoxUser_Click;
            ListBoxUser.SelectedIndexChanged += ListBoxUser_SelectedIndexChanged;
            // 
            // TextBoxUserSearch
            // 
            TextBoxUserSearch.BackColor = SystemColors.Window;
            TextBoxUserSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxUserSearch.Delay = true;
            TextBoxUserSearch.DelayTime = 1000;
            TextBoxUserSearch.Location = new Point(14, 13);
            TextBoxUserSearch.MaxLength = 30;
            TextBoxUserSearch.Name = "TextBoxUserSearch";
            TextBoxUserSearch.Searchstartfrom = 2;
            TextBoxUserSearch.Size = new Size(239, 21);
            TextBoxUserSearch.TabIndex = 1;
            TextBoxUserSearch.TextChanged += TextBoxUserSearch_TextChanged;
            TextBoxUserSearch.KeyDown += TextBoxUserSearch_KeyDown;
            TextBoxUserSearch.KeyPress += TextBoxUserSearch_KeyPress;
            // 
            // FormUsers
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = SystemColors.Control;
            ClientSize = new Size(803, 490);
            Controls.Add(TextBoxUserSearch);
            Controls.Add(statusStrip1);
            Controls.Add(TextBoxUserId);
            Controls.Add(BtnUserCancel);
            Controls.Add(TabControlUser);
            Controls.Add(BtnUserSave);
            Controls.Add(BtnUserEdit);
            Controls.Add(BtnUserExit);
            Controls.Add(BtnUserDelete);
            Controls.Add(BtnUserNew);
            Controls.Add(ListBoxUser);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormUsers";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Users";
            FormClosing += FormUsers_FormClosing;
            Load += FormUsers_Load;
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(ListBoxUser, 0);
            Controls.SetChildIndex(BtnUserNew, 0);
            Controls.SetChildIndex(BtnUserDelete, 0);
            Controls.SetChildIndex(BtnUserExit, 0);
            Controls.SetChildIndex(BtnUserEdit, 0);
            Controls.SetChildIndex(BtnUserSave, 0);
            Controls.SetChildIndex(TabControlUser, 0);
            Controls.SetChildIndex(BtnUserCancel, 0);
            Controls.SetChildIndex(TextBoxUserId, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(TextBoxUserSearch, 0);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            TabControlUser.ResumeLayout(false);
            TabUserDetails.ResumeLayout(false);
            TabUserDetails.PerformLayout();
            TabContactInfo.ResumeLayout(false);
            TabContactInfo.PerformLayout();
            TabTaxDetails.ResumeLayout(false);
            TabTaxDetails.PerformLayout();
            TabAccess.ResumeLayout(false);
            TabAccess.PerformLayout();
            TabEmployee.ResumeLayout(false);
            TabEmployee.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox ListBoxUser;
        private Button BtnUserNew;
        private Button BtnUserDelete;
        private Button BtnUserExit;
        private TabControl TabControlUser;
        private TabPage TabUserDetails;
        private TabPage TabContactInfo;
        private TabPage TabTaxDetails;
        private CheckedListBox CheckedListBoxUser;
        private Label label7;
        private TextBox TextBoxUserReenterPassword;
        private TextBox TextBoxUserPassword;
        private TextBox TextBoxUserLogin;
        private TextBox TextBoxUserLastName;
        private CheckBox CheckBoxUserResetPassword;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox TextBoxUserFirstName;
        private Button BtnUserEdit;
        private Button BtnUserSave;
        private Button BtnUserCancel;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
        private TextBox TextBoxUserEmail;
        private TextBox TextBoxUserPan;
        private Label label17;
        private TextBox TextBoxUserId;
        private CheckBox CheckBoxUserLockUser;
        private MaskedTextBox TextBoxUserFax;
        private TabPage TabAccess;
        private Label label6;
        private TreeView TreeViewUserAccess;
        private ImageList ImageListUserTreeViewAccess;
        private controls.text.PhoneTextBox TextBoxUserMobile;
        private controls.text.PhoneTextBox TextBoxUserPhone;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel UserFormErrorMsg;
        private Label label12;
        private TabPage TabEmployee;
        private TextBox TextBoxSearchEmployee;
        private TreeView TreeViewEmployeeUser;
        private controls.text.DelayedTextChangeTextBox TextBoxUserSearch;
        private controls.AddressGroupBoxWithStateSelection AddressGroupBoxUser;
    }
}