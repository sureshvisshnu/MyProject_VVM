using System;
using System.Collections.Generic;
using System.Windows.Forms;
using fa.libraries.Validation;
using fa.api.Accounting;
using fa.api.UserProfile;
using fa.model.Common;
using fa.model.UserProfile;
using fa.model.Accounting.Masters;
using fa.api.security;
using fa.views.controls;
using System.Linq;
using fa.model.Employee;
using System.Text.RegularExpressions;

namespace fa.views.users
{
    public partial class FormUsers : FormBase
    {
        public static string ChooseStateErrorMsg = "Please choose state";
        public static string EnterEmailErrorMsg = "Please enter valid email.";
        public static string ChooseRoleErrorMsg = "Please choose user role";
        public static string EnterFNameErrorMsg = "Please enter first name";
        public static string EnterLNameErrorMsg = "Please enter last name";
        public static string EnterloginErrorMsg = "Please enter login";
        public static string EnterPassErrorMsg = "Please enter password";
        public static string EnterConfirmPassErrorMsg = "Please enter Reenter password";
        public static string PassMismatchErrorMsg = "Password and Reenter password are not match, Please enter Correctly";
        public static string UniqueloginErrorMsg = "Login ID {0} already exists ,Enter New one";
        public static string UniqueNameErrorMsg = "User {0} {1} already exists";
        public static string DeleteAdminErrorMsg = "Admin user cannot be deleted!";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string DeleteConfirmText = "Do you want to delete the user {0}?";
        public static string DeleteErrorText = "Error Deleting the user!, Please retry";
        public static string EmailErrorText = "Please Check the E-mail Address!!";
        public static string SaveSuccessText = "Save success...";

        UserManager UserManager = null;
        KeypressValidation KeypressValidation = null;
        AddressManager AddressManager = null;
        ContactInfoManager ContactInfoManager = null;
        TaxinfoManager TaxinfoManager = null;
        StateManager StateManager = null;
        CountryManager CountryManager = null;
        CompanyManager CompanyManager = null;
        CostCenterManager CostCenterManager = null;
        Encryptor Encryptor = null;
        public FormUsers()
        {
            UserManager = UserManager.Instance;
            KeypressValidation = KeypressValidation.Instance;
            AddressManager = AddressManager.Instance;
            ContactInfoManager = ContactInfoManager.Instance;
            TaxinfoManager = TaxinfoManager.Instance;
            StateManager = StateManager.Instance;
            CountryManager = CountryManager.Instance;
            Encryptor = new Encryptor();
            CompanyManager = CompanyManager.Instance;
            CostCenterManager = CostCenterManager.Instance;
            InitializeComponent();
            excludedObjects = new string[] { "TextBoxUserSearch", "ListBoxUser" };
        }
        private void FormUsers_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                AddressGroupBoxUser.StateId = (long)Global.Company.Address.StatesId!;
                EnableForm(false);
                LoadUserRoles();
                LoadUserAccess();
                LoadEmployeeWithFilter();
                LoadUserWithFilter();
                if (ListBoxUser.Items.Count == 0)
                {
                    BtnUserNew.Focus();
                }
                else
                {
                    TextBoxUserSearch.Focus();
                }
                this.formIsDirty = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void LoadUserWithFilter()
        {
            string FilterString = TextBoxUserSearch.Text.Trim();
            ListBoxUser.Items.Clear();
            IList<User> Users = UserManager.ListAllUser(Global.Company.BusinessType);
            foreach (var lUser in Users)
            {
                if (FilterString == null || string.IsNullOrEmpty(FilterString.Trim())
                    || lUser.FirstName.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1
                    || lUser.LastName.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    ListBoxUser.Items.Add(lUser);
                }
            }
            if (ListBoxUser.Items.Count > 0)
            {
                ListBoxUser.SelectedIndex = 0;
            }
        }
        private User GetUserInfo()
        {
            User lUser = (ListBoxUser.SelectedIndex > -1 ? (User)ListBoxUser.Items[ListBoxUser.SelectedIndex] : null);
            return (lUser != null ? UserManager.GetUserById(lUser.UserId) : null);
        }
        private void LoadUserInfo()
        {
            User UserFromDB = GetUserInfo();
            if (UserFromDB != null)
            {
                TextBoxUserFirstName.Text = UserFromDB.FirstName;
                TextBoxUserLastName.Text = UserFromDB.LastName;
                TextBoxUserId.Text = UserFromDB.UserId.ToString();
                TextBoxUserLogin.Text = UserFromDB.Login;
                TextBoxUserPassword.Text = Encryptor.DecryptText(UserFromDB.Password, "");
                TextBoxUserReenterPassword.Text = Encryptor.DecryptText(UserFromDB.Password, "");
                CheckBoxUserResetPassword.Checked = UserFromDB.IsResetPassword;
                CheckBoxUserLockUser.Checked = UserFromDB.IsLocked;
                if (UserFromDB.Roles.Count > 0)
                {
                    foreach (var Role in UserFromDB.Roles)
                    {
                        CheckedListBoxUser.SetItemChecked(CheckedListBoxUser.FindStringExact(Role.Name), true);
                    }
                }

                if (UserFromDB.Accesses.Count > 0)
                {
                    loadUserAccess(UserFromDB, TreeViewUserAccess.Nodes);
                }
                if (UserFromDB.EmployeeId != null)
                {
                    loadEmployee(UserFromDB);
                }

                AddressGroupBoxUser.CountryId = (long)Global.Company.CountryId;
                Address Address = null;
                if (UserFromDB.AddressId != null)
                {
                    Address = AddressManager.GetAddressById((long)UserFromDB.AddressId);
                }
                if (Address != null)
                {
                    AddressGroupBoxUser.AddressLine1 = Address.AddressLine1;
                    AddressGroupBoxUser.AddressLine2 = Address.AddressLine2;
                    AddressGroupBoxUser.CityName = Address.CityOrTown;
                    AddressGroupBoxUser.DistrictName = Address.District;
                    AddressGroupBoxUser.PinCode = Address.PinCode;
                    AddressGroupBoxUser.StateId = Address.StatesId != null ? (long)Address.StatesId : 0L;

                }
                if (UserFromDB.ContactInfo != null)
                {
                    ContactInfo lContactInfo = UserFromDB.ContactInfo;
                    TextBoxUserMobile.Text = lContactInfo.Mobile;
                    TextBoxUserPhone.Text = lContactInfo.Phone;
                    TextBoxUserEmail.Text = lContactInfo.Email;
                    TextBoxUserFax.Text = lContactInfo.Fax;
                }

                TaxInfo TaxInfo = null;
                if (UserFromDB.TaxInfoId != null)
                {
                    TaxInfo = TaxinfoManager.GetTaxInfoById((long)UserFromDB.TaxInfoId);
                }
                if (TaxInfo != null)
                {
                    TextBoxUserPan.Text = TaxInfo.PAN;
                }
            }
            else
            {
                DisplaySystemError("Somthing went wrong, the selected user is not valid.");
                return;
            }

        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            this.formIsDirty = false;
            LoadUserWithFilter();
            return;
        }
        private void loadUserAccess(User UserFromDB, TreeNodeCollection Nodes)
        {
            foreach (TreeNode node in Nodes)
            {
                Access Accesses = UserFromDB.Accesses.ToList().FirstOrDefault(x => x.ResourceId == long.Parse(node.Name.Remove(0, 1)) && x.ResourceType == (node.Name.Contains("X") ? ResourceType.Company : ResourceType.CostCenter));
                if (Accesses != null)
                {
                    if (Accesses.ResourceType == ResourceType.Company)
                    {
                        Company lCompanyById = CompanyManager.GetCompany(Accesses.ResourceId);
                        if (lCompanyById.ParentCompanyId == null)
                        {
                            TreeViewUserAccess.Nodes["X" + Accesses.ResourceId.ToString()].Checked = true;
                        }
                        else
                        {
                            TreeViewUserAccess.Nodes["X" + lCompanyById.ParentCompanyId.ToString()].Nodes["X" + Accesses.ResourceId.ToString()].Checked = true;
                            TreeViewUserAccess.Nodes["X" + lCompanyById.ParentCompanyId.ToString()].Expand();
                        }
                    }
                    else
                    {
                        Company lCompanyById = CompanyManager.GetCompany(CostCenterManager.GetCostCenterById(Accesses.ResourceId).ParentCompanyId);
                        if (lCompanyById.ParentCompanyId == null)
                        {
                            TreeViewUserAccess.Nodes["X" + lCompanyById.CompanyId.ToString()].Nodes["Y" + Accesses.ResourceId.ToString()].Checked = true;
                            TreeViewUserAccess.Nodes["X" + lCompanyById.CompanyId.ToString()].Expand();
                        }
                        else
                        {
                            TreeViewUserAccess.Nodes["X" + lCompanyById.ParentCompanyId.ToString()].Nodes["X" + lCompanyById.CompanyId.ToString()].Nodes["Y" + Accesses.ResourceId.ToString()].Checked = true;
                            TreeViewUserAccess.Nodes["X" + lCompanyById.ParentCompanyId.ToString()].Nodes["X" + lCompanyById.CompanyId.ToString()].Expand();
                        }

                    }
                }
                else
                {
                    node.Checked = false;
                }
                if (node.Nodes.Count > 0)
                {
                    loadUserAccess(UserFromDB, node.Nodes);
                }
            }
        }
        private void loadEmployee(User User)
        {
            if (TreeViewEmployeeUser.Nodes != null && TreeViewEmployeeUser.Nodes.Count > 0)
            {
                foreach (TreeNode node in TreeViewEmployeeUser.Nodes)
                {
                    if (node.Name.ToString() == User.EmployeeId.ToString())
                    {
                        node.Checked = true;
                    }
                }
            }
        }
        private void LoadEmployeeWithFilter()
        {
            string FilterString = TextBoxSearchEmployee.Text.Trim();
            TreeViewEmployeeUser.Nodes.Clear();
            IList<Employee> Employee = EmployeeManager.Instance.ListEmployeeByCompanyId(Global.Company.CompanyId);
            if (Employee.Count > 0)
            {
                foreach (var lEmployee in Employee)
                {
                    TreeNode[] treeNodes = TreeViewEmployeeUser.Nodes.Cast<TreeNode>().Where(r => r.Text == lEmployee.Name).ToArray();
                    if (treeNodes.Length == 0)
                    {
                        if (FilterString == null || string.IsNullOrEmpty(FilterString.Trim()) || lEmployee.Name.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1)
                        {

                            TreeNode treeRoot = new TreeNode();
                            treeRoot.Text = lEmployee.Name;
                            treeRoot.Name = lEmployee.Id.ToString();
                            treeRoot.ImageIndex = 0;
                            TreeViewEmployeeUser.Nodes.Add(treeRoot);
                        }
                    }
                }
            }

        }
        private void LoadUserRoles()
        {
            CheckedListBoxUser.Items.Clear();
            IList<Role> Role = UserManager.ListAllUserRole(Global.Company.BusinessType);
            foreach (var Roles in Role)
            {
                CheckedListBoxUser.Items.Add(Roles);
            }
            if (CheckedListBoxUser.Items.Count > 0)
            {
                CheckedListBoxUser.SelectedIndex = 0;
            }
        }
        private void LoadUserAccess()
        {
            TreeViewUserAccess.Nodes.Clear();
            TreeViewUserAccess.CheckBoxes = true;
            IList<Company> Companies = CompanyManager.GetCompaniesByBusinessType(Global.Company.BusinessType);
            foreach (var lCompany in Companies)
            {
                if (lCompany.ParentCompanyId == null)
                {
                    TreeViewUserAccess.Nodes.Add("X" + lCompany.CompanyId.ToString(), lCompany.Name);
                    TreeViewUserAccess.Nodes["X" + lCompany.CompanyId.ToString()].ImageIndex = 0;

                    foreach (var llCompany in Companies)
                    {
                        if (lCompany.CompanyId == llCompany.ParentCompanyId)
                        {
                            TreeViewUserAccess.Nodes["X" + lCompany.CompanyId.ToString()].Nodes.Add("X" + llCompany.CompanyId.ToString(), llCompany.Name);
                            TreeViewUserAccess.Nodes["X" + lCompany.CompanyId.ToString()].Nodes["X" + llCompany.CompanyId.ToString()].ImageIndex = 0;

                            IList<CostCenter> BranchCostCenters = CostCenterManager.ListCostCenterByCompanyId(llCompany.CompanyId);
                            foreach (var lCostCenter in BranchCostCenters)
                            {
                                TreeViewUserAccess.Nodes["X" + lCompany.CompanyId.ToString()].Nodes["X" + llCompany.CompanyId.ToString()].Nodes.Add("Y" + lCostCenter.CostCenterId.ToString(), lCostCenter.Name);
                                TreeViewUserAccess.Nodes["X" + lCompany.CompanyId.ToString()].Nodes["X" + llCompany.CompanyId.ToString()].Nodes["Y" + lCostCenter.CostCenterId.ToString()].ImageIndex = 1;
                            }
                        }
                    }
                    IList<CostCenter> ParentCostCenters = CostCenterManager.ListCostCenterByCompanyId(lCompany.CompanyId);
                    foreach (var lCostCenter in ParentCostCenters)
                    {
                        TreeViewUserAccess.Nodes["X" + lCompany.CompanyId.ToString()].Nodes.Add("Y" + lCostCenter.CostCenterId.ToString(), lCostCenter.Name);
                        TreeViewUserAccess.Nodes["X" + lCompany.CompanyId.ToString()].Nodes["Y" + lCostCenter.CostCenterId.ToString()].ImageIndex = 1;
                    }
                }

            }
        }
        private User GetUserFromForm()
        {
            User lUser = new User();
            if (!string.IsNullOrEmpty(TextBoxUserId.Text))
            {
                lUser.UserId = Convert.ToInt64(TextBoxUserId.Text);
            }
            else
            {
                lUser.UserId = 0L;
            }
            lUser.FirstName = TextBoxUserFirstName.Text.Trim();
            lUser.LastName = TextBoxUserLastName.Text.Trim();
            lUser.Login = TextBoxUserLogin.Text.Trim();
            lUser.Password = Encryptor.EncryptText(TextBoxUserPassword.Text);
            lUser.IsResetPassword = CheckBoxUserResetPassword.Checked;
            lUser.IsLocked = CheckBoxUserLockUser.Checked;
            return lUser;
        }
        private Address GetAddressFromForm()
        {
            Address lAddress = new Address();
            lAddress.AddressLine1 = AddressGroupBoxUser.AddressLine1.Trim();
            lAddress.AddressLine2 = AddressGroupBoxUser.AddressLine2.Trim();
            lAddress.CityOrTown = AddressGroupBoxUser.CityName.Trim();
            lAddress.District = AddressGroupBoxUser.DistrictName.Trim();
            lAddress.PinCode = AddressGroupBoxUser.PinCode.Trim();
            lAddress.StatesId = AddressGroupBoxUser.StateId;
            return lAddress;
        }
        private TaxInfo GetTaxInfoFromForm()
        {
            TaxInfo lTaxInfo = new TaxInfo();
            lTaxInfo.PAN = TextBoxUserPan.Text.Trim();
            return lTaxInfo;
        }
        private ContactInfo GetContactInfoFromForm()
        {
            ContactInfo lContactInfo = new ContactInfo();
            lContactInfo.Phone = TextBoxUserPhone.Text.Trim();
            lContactInfo.Mobile = TextBoxUserMobile.Text.Trim();
            lContactInfo.Fax = TextBoxUserFax.Text.Trim();
            lContactInfo.Email = TextBoxUserEmail.Text.Trim();
            return lContactInfo;
        }
        private void ListBoxUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            //UserFormErrorMsg.Text = "";
            ResetForm();
            LoadUserInfo();
            EnableForm(false);
            ListBoxUser.Select();
            this.formIsDirty = false;
        }
        private void TextBoxUserSearch_TextChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            LoadUserWithFilter();
            if (ListBoxUser.Items.Count > 0) { BtnUserEdit.Enabled = true; BtnUserDelete.Enabled = true; }
            else { BtnUserEdit.Enabled = false; BtnUserDelete.Enabled = false; }
            TextBoxUserSearch.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void BtnUserNew_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            UserFormErrorMsg.Text = "";
            ResetForm();
            EnableForm(true);
            AddressGroupBoxUser.CountryId = (long)Global.Company.CountryId;
            AddressGroupBoxUser.StateId = (long)Global.Company.Address.StatesId!;
            CheckBoxUserLockUser.Enabled = true;
            TabControlUser.SelectedTab = TabUserDetails;
            TextBoxUserFirstName.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }

        private void BtnUserDelete_Click(object sender, EventArgs e)
        {
            UserFormErrorMsg.Text = "";
            User UserInfo = GetUserInfo();
            if (UserInfo != null)
            {
                if (UserInfo.IsSuperAdmin)
                {
                    UserFormErrorMsg.Text = DeleteAdminErrorMsg;
                    return;
                }
                DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, UserInfo.FirstName + " " + UserInfo.LastName), "Delete Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Boolean Deleted = UserManager.DeleteUser(UserInfo.UserId);
                    if (Deleted)
                    {
                        ResetForm();
                        TabControlUser.SelectedTab = TabUserDetails;
                        LoadUserWithFilter();
                        TextBoxUserSearch.Clear();
                        EnableForm(false);
                        this.formIsDirty = false;
                    }
                    else
                    {
                        UserFormErrorMsg.Text = DeleteErrorText;
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            else
            {
                DisplaySystemError("Somthing went wrong, the selected user is not valid.");
                return;
            }

        }
        private void BtnUserEdit_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            LoadUserInfo();
            EnableForm(true);
            if (GetUserInfo().IsSuperAdmin) { CheckBoxUserLockUser.Enabled = false; }
            TabControlUser.SelectedTab = TabUserDetails;
            TextBoxUserFirstName.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void BtnUserCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TabControlUser.SelectedTab = TabUserDetails;
                    TextBoxUserFirstName.Select();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            UserFormErrorMsg.Text = "";
            ResetForm();
            TabControlUser.SelectedTab = TabUserDetails;
            if (string.IsNullOrEmpty(TextBoxUserSearch.Text))
            {
                if(ListBoxUser.Items.Count > 0)
                {
                    LoadUserInfo();
                }
            }
            else
            {
                TextBoxUserSearch.Clear();
            }
            EnableForm(false);
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void BtnUserSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Cursor.Current = Cursors.WaitCursor;
                User lUser = GetUserFromForm();
                User lUserFromDB = null;
                if (UserManager.FindNameUnique(lUser))
                {
                    if (UserManager.FindLoginIdUnique(lUser))
                    {
                        Address lAddress = GetAddressFromForm();
                        TaxInfo lTaxInfo = GetTaxInfoFromForm();
                        ContactInfo lContactInfo = GetContactInfoFromForm();
                        if (lUser.UserId == 0)
                        {
                            ContactInfo lContactInfoFromDB = ContactInfoManager.AddContactInfo(lContactInfo);
                            Address lAddressFromDB = AddressManager.AddAddress(lAddress);
                            TaxInfo lTaxInfoFromDB = TaxinfoManager.AddTaxInfo(lTaxInfo);
                            lUser.ContactInfoId = lContactInfoFromDB.Id;
                            lUser.AddressId = lAddressFromDB.AddressId;
                            lUser.TaxInfoId = lTaxInfoFromDB.Id;
                            if (TreeViewEmployeeUser.Nodes != null && TreeViewEmployeeUser.Nodes.Count > 0)
                            {
                                foreach (TreeNode node in TreeViewEmployeeUser.Nodes)
                                {
                                    if (node.Checked)
                                    {
                                        lUser.EmployeeId = long.Parse(node.Name);
                                    }
                                }
                            }
                            lUserFromDB = UserManager.AddUser(lUser);
                            if (CheckedListBoxUser.Items.Count > 0)
                            {
                                lUser.Roles = new List<Role>();
                                foreach (Role Role in CheckedListBoxUser.CheckedItems)
                                    lUser.Roles.Add(Role);
                            }
                            if (TreeViewUserAccess.Nodes.Count > 0)
                            {
                                lUser.Accesses = new List<Access>();
                                foreach (TreeNode ParenNodes in TreeViewUserAccess.Nodes)
                                {
                                    List<string> AllNodes = PrintRecursive(ParenNodes);
                                    foreach (var Nodes in AllNodes)
                                    {
                                        Access Access = new Access();
                                        Access.UserId = lUserFromDB.UserId;
                                        Access.ResourceType = (Nodes.Contains("X")) ? ResourceType.Company : ResourceType.CostCenter;
                                        Access.ResourceId = long.Parse(Nodes.Remove(0, 1));
                                        lUser.Accesses.Add(Access);
                                    }
                                    AllNodes.Clear();
                                }
                            }
                            UserManager.AddRoleAccess(lUser);
                        }
                        else
                        {
                            User lUserById = UserManager.GetUserById(lUser.UserId);
                            if (lUserById != null)
                            {
                                if (lUserById.AddressId != null)
                                {
                                    lUser.AddressId = lUserById.AddressId;
                                    lAddress.AddressId = (long)lUser.AddressId;
                                    Address lAddressFromDB = AddressManager.UpdateAddress(lAddress);
                                }
                                else
                                {
                                    Address lAddressFromDB = AddressManager.AddAddress(lAddress);
                                    lUser.AddressId = lAddressFromDB.AddressId;
                                }
                                if (lUserById.ContactInfoId != null)
                                {
                                    lUser.ContactInfoId = lUserById.ContactInfoId;
                                    lContactInfo.Id = (long)lUser.ContactInfoId;
                                    ContactInfo lContactInfoFromDB = ContactInfoManager.UpdateContactInfo(lContactInfo);
                                }
                                else
                                {
                                    ContactInfo lContactInfoFromDB = ContactInfoManager.AddContactInfo(lContactInfo);
                                    lUser.ContactInfoId = lContactInfoFromDB.Id;
                                }
                                if (lUserById.TaxInfoId != null)
                                {
                                    lUser.TaxInfoId = lUserById.TaxInfoId;
                                    lTaxInfo.Id = (long)lUser.TaxInfoId;
                                    TaxInfo lTaxInfoFromDB = TaxinfoManager.UpdateTaxInfo(lTaxInfo);
                                }
                                else
                                {
                                    TaxInfo lTaxInfoFromDB = TaxinfoManager.AddTaxInfo(lTaxInfo);
                                    lUser.TaxInfoId = lTaxInfoFromDB.Id;
                                }
                                lUser.IsSuperAdmin = lUserById.IsSuperAdmin;
                                if (TreeViewEmployeeUser.Nodes != null && TreeViewEmployeeUser.Nodes.Count > 0)
                                {
                                    foreach (TreeNode node in TreeViewEmployeeUser.Nodes)
                                    {
                                        if (node.Checked)
                                        {
                                            lUser.EmployeeId = long.Parse(node.Name);
                                            break;
                                        }
                                    }
                                }
                                lUserFromDB = UserManager.UpdateUser(lUser);
                                if (CheckedListBoxUser.Items.Count > 0)
                                {
                                    lUser.Roles = new List<Role>();
                                    foreach (Role Role in CheckedListBoxUser.CheckedItems)
                                        lUser.Roles.Add(Role);
                                }
                                if (TreeViewUserAccess.Nodes.Count > 0)
                                {
                                    lUser.Accesses = new List<Access>();
                                    foreach (TreeNode ParenNodes in TreeViewUserAccess.Nodes)
                                    {
                                        List<string> AllNodes = PrintRecursive(ParenNodes);
                                        foreach (var Nodes in AllNodes)
                                        {
                                            Access Access = new Access();
                                            Access.UserId = lUserFromDB.UserId;
                                            Access.ResourceType = (Nodes.Contains("X")) ? ResourceType.Company : ResourceType.CostCenter;
                                            Access.ResourceId = long.Parse(Nodes.Remove(0, 1));
                                            lUser.Accesses.Add(Access);
                                        }
                                        AllNodes.Clear();
                                    }
                                }
                                UserManager.AddRoleAccess(lUser);
                            }
                            else
                            {
                                DisplaySystemError("Somthing went wrong, the selected user is not valid.");
                                return;
                            }
                        }
                        ResetForm();
                        TabControlUser.SelectedTab = TabUserDetails;
                        if (string.IsNullOrEmpty(TextBoxUserSearch.Text))
                        {
                            LoadUserWithFilter();
                        }
                        else
                        {
                            TextBoxUserSearch.Clear();
                        }
                        ListBoxUser.SelectedIndex = ListBoxUser.FindStringExact(lUserFromDB.FirstName + " " + lUserFromDB.LastName);
                        UserFormErrorMsg.Text = SaveSuccessText;
                        LoadGlobaUser(lUser);
                        EnableForm(false);
                        this.formIsDirty = false;
                    }
                    else
                    {
                        UserFormErrorMsg.Text = string.Format(UniqueloginErrorMsg, lUser.Login);
                        TabControlUser.SelectedTab = TabUserDetails;
                        TextBoxUserLogin.Select();
                    }
                }
                else
                {
                    UserFormErrorMsg.Text = string.Format(UniqueNameErrorMsg, lUser.FirstName, lUser.LastName);
                    TabControlUser.SelectedTab = TabUserDetails;
                    TextBoxUserFirstName.Select();
                }
                Cursor.Current = Cursors.Default;
            }
        }
        List<string> Nodes = new List<string>();
        private List<string> PrintRecursive(TreeNode Node)
        {
            if (Node.Checked == true)
            {
                Nodes.Add(Node.Name);
            }
            foreach (TreeNode tn in Node.Nodes)
            {
                PrintRecursive(tn);
            }
            return Nodes;
        }

        private void BtnUserExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private Boolean ValidateForm()
        {
            UserFormErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxUserFirstName.Text.Trim()))
            {
                UserFormErrorMsg.Text = EnterFNameErrorMsg;
                TabControlUser.SelectedTab = TabUserDetails;
                TextBoxUserFirstName.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxUserLastName.Text.Trim()))
            {
                UserFormErrorMsg.Text = EnterLNameErrorMsg;
                TabControlUser.SelectedTab = TabUserDetails;
                TextBoxUserLastName.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxUserLogin.Text.Trim()))
            {
                UserFormErrorMsg.Text = EnterloginErrorMsg;
                TabControlUser.SelectedTab = TabUserDetails;
                TextBoxUserLogin.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxUserPassword.Text))
            {
                UserFormErrorMsg.Text = EnterPassErrorMsg;
                TabControlUser.SelectedTab = TabUserDetails;
                TextBoxUserPassword.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxUserReenterPassword.Text))
            {
                UserFormErrorMsg.Text = EnterConfirmPassErrorMsg;
                TabControlUser.SelectedTab = TabUserDetails;
                TextBoxUserReenterPassword.Select();
                return false;
            }
            if (TextBoxUserPassword.Text != TextBoxUserReenterPassword.Text)
            {
                UserFormErrorMsg.Text = PassMismatchErrorMsg;
                TabControlUser.SelectedTab = TabUserDetails;
                TextBoxUserReenterPassword.Select();
                return false;
            }
            if (CheckedListBoxUser.CheckedItems.Count == 0)
            {
                UserFormErrorMsg.Text = ChooseRoleErrorMsg;
                TabControlUser.SelectedTab = TabUserDetails;
                CheckedListBoxUser.Select();
                return false;
            }
            if (AddressGroupBoxUser.StateId == 0L)
            {
                UserFormErrorMsg.Text = ChooseStateErrorMsg;
                TabControlUser.SelectedTab = TabContactInfo;
                AddressGroupBoxUser.selected_field(Fields.state);
                return false;
            }
            if (!string.IsNullOrEmpty(TextBoxUserEmail.Text.Trim()) && !KeypressValidation.EmailValidation(TextBoxUserEmail.Text.Trim()))
            {
                UserFormErrorMsg.Text = EnterEmailErrorMsg;
                TabControlUser.SelectedTab = TabContactInfo;
                TextBoxUserEmail.Select();
                return false;
            }
            return true;
        }
        private void ResetForm()
        {
            AddressGroupBoxUser.Clear();
            AddressGroupBoxUser.StateId = (long)Global.Company.Address.StatesId!;
            TextBoxUserEmail.ResetText();
            TextBoxUserFax.ResetText();
            TextBoxUserFirstName.ResetText();
            TextBoxUserId.ResetText();
            TextBoxUserLastName.ResetText();
            TextBoxUserLogin.ResetText();
            TextBoxUserMobile.ResetText();
            TextBoxUserPan.ResetText();
            TextBoxUserPassword.ResetText();
            TextBoxUserPhone.ResetText();
            TextBoxUserReenterPassword.ResetText();
            TextBoxSearchEmployee.ResetText();
            CheckBoxUserResetPassword.Checked = false;
            CheckBoxUserLockUser.Checked = false;
            LoadUserRoles();
            LoadUserAccess();
            LoadEmployeeWithFilter();
        }

        private void LoadGlobaUser(User luser)
        {
            User user = UserManager.Instance.GetUserByLogin(Global.User.Login);
            if (luser.Login == user.Login)
            {
                Global.User = luser;
            }
        }

        private void EnableForm(Boolean enable)
        {
            //this.CancelButton = (enable ? BtnUserCancel : BtnUserExit);
            User UserInfo = GetUserInfo();
            if (ListBoxUser.Items.Count > 0)
            {
                ListBoxUser.Enabled = !enable;
                TextBoxUserSearch.ReadOnly = enable;
                TextBoxUserSearch.TabStop = !enable;
            }
            else
            {
                ListBoxUser.Enabled = false;
                TextBoxUserSearch.ReadOnly = true;
                TextBoxUserSearch.TabStop = false;
            }
            AddressGroupBoxUser.ReadOnly = !enable;
            TextBoxUserEmail.ReadOnly = !enable;
            TextBoxUserFax.ReadOnly = !enable;
            TextBoxUserFirstName.ReadOnly = !enable;
            TextBoxUserId.ReadOnly = !enable;
            TextBoxUserLastName.ReadOnly = !enable;
            TextBoxUserLogin.ReadOnly = !enable;
            TextBoxUserMobile.ReadOnly = !enable;
            TextBoxUserPan.ReadOnly = !enable;
            TextBoxUserPassword.ReadOnly = !enable;
            TextBoxUserPhone.ReadOnly = !enable;
            TextBoxUserReenterPassword.ReadOnly = !enable;
            TextBoxSearchEmployee.ReadOnly = !enable;

            AddressGroupBoxUser.TabStop = enable;
            TextBoxUserEmail.TabStop = enable;
            TextBoxUserFax.TabStop = enable;
            TextBoxUserFirstName.TabStop = enable;

            TextBoxUserId.TabStop = enable;
            TextBoxUserLastName.TabStop = enable;
            TextBoxUserLogin.TabStop = enable;
            TextBoxUserMobile.TabStop = enable;
            TextBoxUserPan.TabStop = enable;
            TextBoxUserPassword.TabStop = enable;
            TextBoxUserPhone.TabStop = enable;
            TextBoxUserReenterPassword.TabStop = enable;
            TextBoxSearchEmployee.TabStop = enable;

            CheckBoxUserLockUser.Enabled = enable;
            CheckBoxUserResetPassword.Enabled = enable;
            CheckedListBoxUser.Enabled = enable;
            TreeViewUserAccess.Enabled = enable;
            TreeViewEmployeeUser.Enabled = enable;
            if (!enable)
            {
                BtnUserCancel.Enabled = enable;
                if (ListBoxUser.SelectedIndex < 0)
                {
                    BtnUserDelete.Enabled = enable;
                    BtnUserEdit.Enabled = enable;
                }
                else
                {
                    if (UserInfo.IsSuperAdmin)
                    {
                        BtnUserDelete.Enabled = false;
                        CheckBoxUserLockUser.Enabled = false;
                    }
                    else
                    {
                        BtnUserDelete.Enabled = !enable;
                    }
                    BtnUserEdit.Enabled = !enable;
                }
                BtnUserNew.Enabled = !enable;
                BtnUserSave.Enabled = enable;
            }
            else
            {
                BtnUserCancel.Enabled = enable;
                if (UserInfo == null || UserInfo.IsSuperAdmin)
                {
                    BtnUserDelete.Enabled = false;
                    CheckBoxUserLockUser.Enabled = false;
                }
                else
                {
                    BtnUserDelete.Enabled = !enable;
                }
                BtnUserEdit.Enabled = !enable;
                BtnUserNew.Enabled = !enable;
                BtnUserSave.Enabled = enable;
                if (TreeViewEmployeeUser.Nodes == null || TreeViewEmployeeUser.Nodes.Count == 0)
                {
                    TextBoxSearchEmployee.ReadOnly = true;
                }
            }

        }
        private void TextBoxUserSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void TextBoxUserFirstName_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void TextBoxUserLastName_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void TextBoxUserLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_LoginChecking(sender, e);
        }
        private void TextBoxUserPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void TextBoxUserReenterPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }

        private void TextBoxUserEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_EmailChecking(sender, e);
        }
        private void TextBoxUserPan_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_TaxDetailsNumberChecking(sender, e);

        }
        private void CheckedListBoxUser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlUser.SelectedTab = TabContactInfo;
                AddressGroupBoxUser.selected_field(Fields.address1);
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TabControlUser.SelectedTab = TabUserDetails;
                if (CheckBoxUserLockUser.Enabled) { CheckBoxUserLockUser.Select(); } else { CheckBoxUserResetPassword.Select(); }
            }
        }
        private void AddressGroupBoxUser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlUser.SelectedTab = TabContactInfo;
                TextBoxUserPhone.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlUser.SelectedTab = TabUserDetails;
                CheckedListBoxUser.Select();
            }
        }
        private void TextBoxUserPhone_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlUser.SelectedTab = TabContactInfo;
                TextBoxUserMobile.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlUser.SelectedTab = TabContactInfo;
                AddressGroupBoxUser.selected_field(Fields.pin);
            }
        }
        private void TextBoxUserEmail_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlUser.SelectedTab = TabTaxDetails;
                TextBoxUserPan.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TabControlUser.SelectedTab = TabContactInfo;
                TextBoxUserFax.Select();
            }
        }
        private void TextBoxUserPan_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (TreeViewUserAccess.Nodes != null && TreeViewUserAccess.Nodes.Count > 0)
                {
                    TabControlUser.SelectedTab = TabAccess;
                    TreeViewUserAccess.Focus();
                }
                else if (TreeViewEmployeeUser.Nodes != null && TreeViewEmployeeUser.Nodes.Count > 0)
                {
                    TabControlUser.SelectedTab = TabEmployee;
                    TextBoxSearchEmployee.Select();
                }
                else
                {
                    BtnUserSave.Select();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TabControlUser.SelectedTab = TabContactInfo;
                TextBoxUserEmail.Select();
            }
        }

        private void TextBoxUserSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                ListBoxUser.Select();
            }
        }

        private void TreeViewUserAccess_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (TreeViewEmployeeUser.Nodes != null && TreeViewEmployeeUser.Nodes.Count > 0)
                {
                    TabControlUser.SelectedTab = TabEmployee;
                    TextBoxSearchEmployee.Select();
                }
                else
                {
                    BtnUserSave.Select();
                }

            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TabControlUser.SelectedTab = TabTaxDetails;
                TextBoxUserPan.Select();
            }
        }
        private void TextBoxSearchEmployee_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlUser.SelectedTab = TabEmployee;
                TreeViewEmployeeUser.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TabControlUser.SelectedTab = TabAccess;
                TreeViewUserAccess.Select();
            }
        }
        private void TreeViewEmployeeUser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlUser.SelectedTab = TabEmployee;
                BtnUserSave.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TabControlUser.SelectedTab = TabEmployee;
                TextBoxSearchEmployee.Select();
            }
        }
        private void TextBoxUserFirstName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlUser.SelectedTab = TabUserDetails;
                TextBoxUserLastName.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                BtnUserSave.Select();
            }
        }
        private void BtnUserSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlUser.SelectedTab = TabUserDetails;
                TextBoxUserFirstName.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                if (TreeViewEmployeeUser.Nodes != null && TreeViewEmployeeUser.Nodes.Count > 0)
                {
                    TabControlUser.SelectedTab = TabEmployee;
                    TextBoxSearchEmployee.Select();
                }
                else if (TreeViewUserAccess.Nodes != null && TreeViewUserAccess.Nodes.Count > 0)
                {
                    TabControlUser.SelectedTab = TabAccess;
                    TreeViewEmployeeUser.Focus();
                }
                else
                {
                    TabControlUser.SelectedTab = TabTaxDetails;
                    TextBoxUserPan.Select();
                }

            }
        }
        private void TreeViewUserAccess_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            node.SelectedImageIndex = node.ImageIndex;
        }


        private void TextBoxUserFirstName_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "NameChecking");

        }

        private void TextBoxUserFirstName_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxUserFirstName, "NameChecking");
            }
        }

        private void TextBoxUserLastName_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxUserLastName, "NameChecking");
            }
        }

        private void TextBoxUserPassword_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxUserPassword, "NameChecking");
            }

        }

        private void TextBoxUserReenterPassword_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxUserReenterPassword, "NameChecking");
            }
        }

        private void TextBoxUserEmail_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "EmailChecking");

        }

        private void TextBoxUserEmail_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxUserEmail, "EmailChecking");
            }
        }


        private void TextBoxUserLogin_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "LoginChecking");
        }

        private void TextBoxUserLogin_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxUserLogin, "LoginChecking");
            }
        }

        private void TextBoxUserPan_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "TaxDetailsNumberChecking");

        }

        private void TextBoxUserPan_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxUserPan, "TaxDetailsNumberChecking");
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnUserNew.PerformClick();
            }
            if (keyData == (Keys.F4))
            {
                BtnUserDelete.PerformClick();
            }
            if (keyData == (Keys.F7))
            {
                BtnUserEdit.PerformClick();
            }
            if (keyData == (Keys.F8))
            {
                BtnUserSave.PerformClick();
            }
            if (keyData == (Keys.F10))
            {
                BtnUserExit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnUserCancel.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void FormUsers_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TabControlUser.SelectedTab = TabUserDetails;
                    TextBoxUserFirstName.Select();
                    e.Cancel = true;
                }
            }
        }

        bool EnableAfterCheck = true;
        private void TreeViewUserAccess_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (EnableAfterCheck)
            {
                EnableAfterCheck = false;
                if (e.Node.Checked && e.Node.ImageIndex == 1)
                {
                    TreeNode[] node = TreeViewUserAccess.Nodes.Find(e.Node.Parent.Name, true);
                    if (!node[0].Checked)
                    {
                        node[0].Checked = true;
                    }
                }
                TreeNode[] tns = TreeViewUserAccess.Nodes.Find(e.Node.Name, true);
                if (tns[0].Nodes.Count > 0)
                {
                    CheckedChange(e, tns[0]);
                }
                EnableAfterCheck = true;
            }
        }
        private void CheckedChange(TreeViewEventArgs Args, TreeNode TreeNode)
        {
            foreach (TreeNode node in TreeNode.Nodes)
            {
                node.Checked = Args.Node.Checked;
                if (node.Nodes.Count > 0)
                {
                    CheckedChange(Args, node);
                }
            }

        }

        private void TreeViewEmployeeUser_AfterCheck(object sender, TreeViewEventArgs e)
        {
            foreach (TreeNode node in TreeViewEmployeeUser.Nodes)
            {
                if (e.Node.Name != node.Name & e.Node.Checked)
                {
                    node.Checked = false;
                }
            }
        }

        private void TextBoxSearchEmployee_TextChanged(object sender, EventArgs e)
        {
            LoadEmployeeWithFilter();
        }

        private void CheckedListBoxUser_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.Index == 0)
            {
                if (CheckedListBoxUser.Items.Count > 0)
                {
                    IList<Role> Role = UserManager.ListAllUserRole(Global.Company.BusinessType);
                    foreach (Role lRole in Role)
                    {
                        if (lRole.Name != "Admin")
                        {
                            CheckedListBoxUser.SetItemChecked(CheckedListBoxUser.FindStringExact(lRole.Name), false);
                        }
                    }
                }
            }
            else
            {
                if (CheckedListBoxUser.Items.Count > 0)
                {
                    CheckedListBoxUser.SetItemChecked(CheckedListBoxUser.FindStringExact("Admin"), false);
                }
            }
        }

        private void ListBoxUser_Click(object sender, EventArgs e)
        {
            UserFormErrorMsg.Text = "";
        }
    }
}
