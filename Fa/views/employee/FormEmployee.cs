using fa.libraries.utils;
using fa.libraries.Validation;
using fa.views.controls;
using fa.api.Accounting;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.Common;
using fa.model.Employee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using fa.api.Hms;
using fa.model.Hms.Master;
using DocumentFormat.OpenXml.Bibliography;
using Department = fa.model.Employee.Department;
using Title = fa.model.Employee.Title;
using Fa.api.Hms;
using Standard;
using System.Text.RegularExpressions;
using fa.model.hms.config;
using FADataAccessLibrary.Api.Hms;

namespace fa.views.employee
{
    public partial class FormEmployee : FormBase
    {
        public static string SelectedDepartmentNotValidErrorMsg = "Somthing went wrong, the selected department is not valid.";
        public static string SelectedEmployeeNotValidErrorMsg = "Somthing went wrong, the selected employee is not valid.";
        public static string SelectedParentNotValidErrorMsg = "Somthing went wrong, the selected parent is not valid.";
        public static string SelectedTitleNotValidErrorMsg = "Somthing went wrong, the selected title is not valid.";
        public static string NoDepartmentFoundErrorMsg = "Before adding employee, Add department";
        public static string RemoveSuccess = "{0} {1} romove successful";
        public static string DeleteEmployeeConfirmText = "Do you want to delete the employee {0}?";
        public static string DeleteDepartmentConfirmText = "Do you want to delete the department {0}?";
        public static string DeleteNotAllowErrorText = "Cannot delete {0} it's used in some where else";
        public static string DeleteErrorText = "Error deleting the {0}!, Please retry";
        public static string UniqueNameErrorMsg = "{0} {1} already exists";
        public static string EmployeeUniqueNameErrorMsg = "{0}  Name {1} already exists for another employee {2}";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want exit?";
        public static string EnterNameErrorMsg = "Please enter {0} name";
        public static string ChooseDepartmentErrorMsg = "Please choose department";
        public static string EnterDateErrorMsg = "Please enter valid date";
        public static string ChooseTitleErrorMsg = "Please choose title";
        public static string ChooseParentErrorMsg = "Please choose parent department";
        public static string CreateEmployeeOnloadText = "New {0}";
        public static string UpdateCustomerOnloadText = "Update employee";
        public static string SaveSuccessText = "Saved success...";
        public static string ChooseStateErrorMsg = "Please choose state";
        public static string EnterEmployeeEmailErrorMsg = "Please enter valid email.";
        public static string EnterPhoneNumberErrorMsg = "Please enter valid phone number.";
        public static string EnterMobileNumberErrorMsg = "Please enter valid mobile number.";

        EmployeeManager EmployeeManager = null;
        DepartmentManager DepartmentManager = null;
        TitleManager TitleManager = null;
        AddressManager AddressManager = null;
        StateManager StateManager = null;
        ContactInfoManager ContactInfoManager = null;
        TaxinfoManager TaxinfoManager = null;
        KeypressValidation KeypressValidation = null;
        DateValidation DateValidation = null;
        FormBase parent = null;
        KeypressValidation Keypress_Validation = null;
        public bool CreateEmployeeOnLoad = false;
        public string ParentId;
        public string Id;
        public FormEmployee(object sender)
        {
            if (sender is FormBase)
            {
                parent = (FormBase)sender;
            }
            EmployeeManager = EmployeeManager.Instance;
            DepartmentManager = DepartmentManager.Instance;
            TitleManager = TitleManager.Instance;
            AddressManager = AddressManager.Instance;
            StateManager = StateManager.Instance;
            ContactInfoManager = ContactInfoManager.Instance;
            TaxinfoManager = TaxinfoManager.Instance;
            KeypressValidation = KeypressValidation.Instance;
            DateValidation = DateValidation.Instance;
            InitializeComponent();
            excludedObjects = new string[] { "TextBoxSearch" };
        }
        private void Employee_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                CameraPhotoTaker.SetLabelText("Employee Photo");
                TreeViewEmployeeDepartment.Nodes.Clear();
                ResetForm();
                EnableForm(false);
                loadCombo();
                LoadEmployeeDeparmentWithFilter();
                if (CreateEmployeeOnLoad)
                {
                    TextBoxSearch.Visible = false;
                    BtnDepartmentCancel.Visible = false;
                    BtnNew.Visible = false;
                    BtnDepartmentEdit.Visible = false;
                    BtnDelete.Visible = false;
                    TreeViewEmployeeDepartment.Visible = false;
                    TabControlEmployee.Location = new Point(12, 12);
                    TabControlDepartment.Location = new Point(12, 12);
                    BtnDepartmentSave.Location = new Point(TabControlEmployee.Width - 74, TabControlEmployee.Height + 15);
                    this.Size = new Size(TabControlEmployee.Right + 25, TabControlEmployee.Bottom + 90);
                    this.CenterToParent();
                    if (Id == null)
                    {
                        this.Text = string.Format(CreateEmployeeOnloadText, "Employee");
                        newEmployeeToolStripMenuItem.PerformClick();
                    }
                    else
                    {
                        this.Text = UpdateCustomerOnloadText;
                        PointSaveorUpdatedNode(TreeViewEmployeeDepartment.Nodes, (Id + "@"));
                        BtnDepartmentEdit_Click(this, null);
                    }
                }
                this.formIsDirty = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void GetChildNode(long ParentDepartmentId, TreeNode ParentTreeNode)
        {
            IList<Department> Department = DepartmentManager.ListDepartmentByDepartmentIdCompanyId(ParentDepartmentId, Global.Company.CompanyId);
            IList<Employee> Employee = EmployeeManager.ListEmployeeByDepartmentId(ParentDepartmentId);
            if (Department.Count > 0)
            {
                foreach (var lDepartment in Department)
                {
                    TreeNode childNode = new TreeNode();
                    childNode.Text = lDepartment.Name;
                    childNode.Name = lDepartment.Id.ToString();
                    childNode.ImageIndex = 0;
                    IList<Department> llDepartment = DepartmentManager.ListDepartmentByDepartmentIdCompanyId(lDepartment.Id, Global.Company.CompanyId);
                    IList<Employee> lEmployee = EmployeeManager.ListEmployeeByDepartmentId(lDepartment.Id);
                    if (llDepartment.Count > 0)
                    {
                        GetChildNode(lDepartment.Id, childNode);
                    }
                    if (lEmployee.Count > 0 && (llDepartment.Count == 0))
                    {
                        GetChildNode(lDepartment.Id, childNode);
                    }
                    ParentTreeNode.Nodes.Add(childNode);
                }
            }
            if (Employee.Count > 0)
            {
                foreach (var lEmployee in Employee)
                {
                    TreeNode childNode = new TreeNode();
                    childNode.Text = lEmployee.Name;
                    childNode.Name = lEmployee.Id.ToString() + "@";
                    childNode.ImageIndex = 1;
                    ParentTreeNode.Nodes.Add(childNode);
                }
            }
        }
        private void LoadEmployeeDeparmentWithFilter()
        {
            string FilterString = TextBoxSearch.Text.Trim();
            TreeViewEmployeeDepartment.SelectedNode = null;
            TreeViewEmployeeDepartment.Nodes.Clear();
            IList<Department> Department = DepartmentManager.ListDepartmentByFilterCompanyId(Global.Company.CompanyId, FilterString);
            IList<Employee> Employee = EmployeeManager.ListFilterEmployeeByCompanyId(Global.Company.CompanyId, FilterString);
            if (Department != null)
            {
                if (Department.Count == 0)
                {

                    Department lDepartmentById = new Department();
                    foreach (var dEmployee in Employee)
                    {
                        lDepartmentById = DepartmentManager.GetDepartmentInfoById((long)dEmployee.DepartmentId);
                        Department.Add(lDepartmentById);
                    }
                    foreach (var lDepartments in Department)
                    {
                        TreeNode[] treeNodes = TreeViewEmployeeDepartment.Nodes.Cast<TreeNode>().Where(r => r.Text == lDepartments.Name).ToArray();
                        if (treeNodes.Length == 0)
                        {
                            TreeNode treeRoot = new TreeNode();
                            treeRoot.Text = lDepartments.Name;
                            treeRoot.Name = lDepartments.Id.ToString();
                            treeRoot.ImageIndex = 0;
                            TreeViewEmployeeDepartment.Nodes.Add(treeRoot);
                            GetChildNode(lDepartments.Id, treeRoot, Department, Employee);

                        }
                    }
                }
                else
                {
                    foreach (var lDepartment in Department.Where(d => d.ParentDepartmentId == null))
                    {
                        TreeNode[] treeNodes = TreeViewEmployeeDepartment.Nodes.Cast<TreeNode>().Where(r => r.Text == lDepartment.Name).ToArray();
                        if (treeNodes.Length == 0)
                        {
                            TreeNode treeRoot = new TreeNode();
                            treeRoot.Text = lDepartment.Name;
                            treeRoot.Name = lDepartment.Id.ToString();
                            treeRoot.ImageIndex = 0;
                            TreeViewEmployeeDepartment.Nodes.Add(treeRoot);
                            GetChildNode(lDepartment.Id, treeRoot, Department, Employee);

                        }
                    }
                }
            }
            if (TreeViewEmployeeDepartment.Nodes.Count > 0)
            {
                if (!string.IsNullOrEmpty(TextBoxSearch.Text))
                {
                    TreeViewEmployeeDepartment.ExpandAll();
                }
                TreeViewEmployeeDepartment.SelectedNode = TreeViewEmployeeDepartment.Nodes[0];
            }
        }
        private void GetChildNode(long ParentDepartmentId, TreeNode ParentTreeNode, IList<Department> Department, IList<Employee> Employee)
        {
            foreach (var lDepartment in Department.Where(x => x.ParentDepartmentId == ParentDepartmentId))
            {
                TreeNode childNode = new TreeNode();
                childNode.Text = lDepartment.Name;
                childNode.Name = lDepartment.Id.ToString();
                childNode.ImageIndex = 0;
                GetChildNode(lDepartment.Id, childNode, Department, Employee);
                ParentTreeNode.Nodes.Add(childNode);
            }
            foreach (var lEmployee in Employee.Where(x => x.DepartmentId == ParentDepartmentId).ToList())
            {
                TreeNode childNode = new TreeNode();
                childNode.Text = lEmployee.Name;
                childNode.Name = lEmployee.Id.ToString() + "@";
                childNode.ImageIndex = 1;
                ParentTreeNode.Nodes.Add(childNode);
            }
        }
        private Employee GetEmployeeInfo()
        {
            Employee EmployeeFromDB = EmployeeManager.GetEmployeeInfoById(long.Parse(TreeViewEmployeeDepartment.SelectedNode.Name.Trim('@')));
            if (EmployeeFromDB != null)
            {
                return EmployeeFromDB;
            }
            return null;
        }
        private Department GetDepartmentInfo()
        {
            if (TreeViewEmployeeDepartment.SelectedNode != null)
            {
                Department CostCenterFromDB = DepartmentManager.GetDepartmentInfoById(TreeViewEmployeeDepartment.SelectedNode.Name != null ? long.Parse(TreeViewEmployeeDepartment.SelectedNode.Name) : 0);
                if (CostCenterFromDB != null)
                {
                    Department DepartmentFromDB = DepartmentManager.GetDepartmentInfoById(CostCenterFromDB.Id);
                    return DepartmentFromDB;
                }
            }
            return null;
        }
        private void LoadEmpDepInfo()
        {
            if (TreeViewEmployeeDepartment.SelectedNode != null)
            {
                if (!TreeViewEmployeeDepartment.SelectedNode.Name.Contains('@'))
                {
                    TabControlDepartment.Visible = true;
                    TabControlEmployee.Visible = false;
                    Department DepartmentFromDB = GetDepartmentInfo();
                    if (DepartmentFromDB != null)
                    {
                        TextBoxEmpDepId.Text = DepartmentFromDB.Id.ToString();
                        TextBoxDepartmentName.Text = DepartmentFromDB.Name;
                        TextBoxDepartmentDisplayAs.Text = DepartmentFromDB.DisplayAs;
                        TextBoxDepartmentDescription.Text = DepartmentFromDB.Discription;
                        if (DepartmentFromDB.ParentDepartment != null)
                        {
                            comboBoxSwapTextBoxParentDepartment.SelectedIndex = comboBoxSwapTextBoxParentDepartment.FindStringExact(DepartmentFromDB.ParentDepartment.Name);
                            comboBoxSwapTextBoxParentDepartment.Text = DepartmentFromDB.ParentDepartment.Name;
                        }
                        else
                        {
                            comboBoxSwapTextBoxParentDepartment.SelectedIndex = -1;
                        }
                    }
                    else
                    {
                        DisplaySystemError(SelectedDepartmentNotValidErrorMsg);
                        return;
                    }
                }
                else
                {
                    TabControlEmployee.Visible = true;
                    TabControlEmployee.SelectedTab = TabPageEmployeeDetails;
                    TabControlDepartment.Visible = false;
                    Employee EmployeeFromDB = GetEmployeeInfo();
                    AddressGroupBoxEmployee.CountryId = (long)Global.Company.CountryId!;
                    ClearSignatureDetails();
                    if (EmployeeFromDB != null)
                    {
                        SignTabPageAlignment();
                        TextBoxEmpDepId.Text = EmployeeFromDB.Id.ToString();
                        TextBoxEmployeeName.Text = EmployeeFromDB.Name;
                        TextBoxEmployeeLegalName.Text = EmployeeFromDB.DisplayAs;
                        comboBoxSwapTextBoxEmployeeDepartment.SelectedIndex = comboBoxSwapTextBoxEmployeeDepartment.FindStringExact(EmployeeFromDB.Department.Name);
                        comboBoxSwapTextBoxEmployeeDepartment.Text = EmployeeFromDB.Department.Name;
                        if (EmployeeFromDB.DateOfBirth != null && DateUtils.ValidDate(((DateTime)EmployeeFromDB.DateOfBirth).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
                        {
                            DateTimePickerEmployee.Date = (DateTime)DateUtils.ToDate(((DateTime)EmployeeFromDB.DateOfBirth).Date.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                        }
                        ComboBoxSwapTextBoxEmployeeTitle.SelectedIndex = ComboBoxSwapTextBoxEmployeeTitle.FindStringExact(EmployeeFromDB.Title.Name);
                        ComboBoxSwapTextBoxEmployeeTitle.Text = EmployeeFromDB.Title.Name;
                        checkBoxIsServiceProvider.Checked = EmployeeFromDB.IsServiceProvider;
                        if (EmployeeFromDB.Address != null)
                        {
                            AddressGroupBoxEmployee.AddressLine1 = EmployeeFromDB.Address.AddressLine1;
                            AddressGroupBoxEmployee.AddressLine2 = EmployeeFromDB.Address.AddressLine2;
                            AddressGroupBoxEmployee.CityName = EmployeeFromDB.Address.CityOrTown;
                            AddressGroupBoxEmployee.DistrictName = EmployeeFromDB.Address.District;
                            AddressGroupBoxEmployee.PinCode = EmployeeFromDB.Address.PinCode;
                            AddressGroupBoxEmployee.StateName = EmployeeFromDB.Address.StateName;
                            AddressGroupBoxEmployee.StateId = EmployeeFromDB.Address.StatesId != null ? (long)EmployeeFromDB.Address.StatesId : 0L;
                        }
                        if (EmployeeFromDB.EmployeePhoto != null)
                        {
                            MemoryStream Stream = new MemoryStream(EmployeeFromDB.EmployeePhoto);
                            CameraPhotoTaker.Photo = System.Drawing.Image.FromStream(Stream);
                        }
                        else
                        {
                            CameraPhotoTaker.Clear();
                        }
                        TextBoxEmployeeMobile.Text = EmployeeFromDB.ContactInfo != null ? EmployeeFromDB.ContactInfo.Mobile : "";
                        TextBoxEmployeePhone.Text = EmployeeFromDB.ContactInfo != null ? EmployeeFromDB.ContactInfo.Phone : "";
                        TextBoxEmployeeFax.Text = EmployeeFromDB.ContactInfo != null ? EmployeeFromDB.ContactInfo.Fax : "";
                        TextBoxEmployeeEmail.Text = EmployeeFromDB.ContactInfo != null ? EmployeeFromDB.ContactInfo.Email : "";
                        TextBoxEmployeePan.Text = EmployeeFromDB.TaxInfo != null ? EmployeeFromDB.TaxInfo.PAN : "";
                        LoadSignatureDetails();
                    }
                    else
                    {
                        DisplaySystemError(SelectedEmployeeNotValidErrorMsg);
                        return;
                    }
                }
            }
        }
        private void ClearSignatureDetails()
        {
            CheckBoxRxSymbolPresc.Checked = false;
            CheckBoxAllowDigtalsignature.Checked = false;
            DigitalSignaturePictureBoxPresc.Image = null;
        }
        private void SignTabPageAlignment()
        {
            Employee EmployeeFromDB = GetEmployeeInfo();
            if (EmployeeFromDB != null)
            {
                if (EmployeeFromDB.Title.Name == "Nurse")
                {
                    TabControlEmployee.TabPages.Remove(TabPageSignatureDetails);
                }
                else if (EmployeeFromDB.Title.Name == "Doctor")
                {
                    if (!TabControlEmployee.TabPages.Contains(TabPageSignatureDetails))
                    {
                        TabControlEmployee.TabPages.Add(TabPageSignatureDetails);
                    }
                    CheckBoxRxSymbolPresc.Visible = true;
                    CheckBoxRxSymbolPresc.Location = new Point(18, 12);
                    CheckBoxAllowDigtalsignature.Location = new Point(18, 35);
                    DigitalSignaturePictureBoxPresc.Location = new Point(18, 71);
                    BtnDigitalSignatureDeletePresc.Location = new Point(310, 71);
                    BtnDigitalSignatureImpoortPresc.Location = new Point(310, 117);
                }
                else if (EmployeeFromDB.Title.Name == "Technician")
                {
                    if (!TabControlEmployee.TabPages.Contains(TabPageSignatureDetails))
                    {
                        TabControlEmployee.TabPages.Add(TabPageSignatureDetails);
                    }
                    CheckBoxRxSymbolPresc.Visible = false;
                    CheckBoxAllowDigtalsignature.Location = new Point(18, 12);
                    DigitalSignaturePictureBoxPresc.Location = new Point(18, 35);
                    BtnDigitalSignatureDeletePresc.Location = new Point(310, 35);
                    BtnDigitalSignatureImpoortPresc.Location = new Point(310, 80);
                }
            }
        }
        private void LoadSignatureDetails()
        {
            Employee EmployeeFromDB = GetEmployeeInfo();
            if (EmployeeFromDB != null)
            {
                if (EmployeeFromDB.DigitalSignature != null)
                {
                    MemoryStream Stream = new MemoryStream(EmployeeFromDB.DigitalSignature);
                    DigitalSignaturePictureBoxPresc.Image = System.Drawing.Image.FromStream(Stream);
                }
                if (EmployeeFromDB.IsRxSymbolDisplayOn)
                {
                    CheckBoxRxSymbolPresc.Checked = EmployeeFromDB.IsRxSymbolDisplayOn;
                }
                if (EmployeeFromDB.IsRSignatureDisplayOn)
                {
                    CheckBoxAllowDigtalsignature.Checked = EmployeeFromDB.IsRSignatureDisplayOn;
                }
            }
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            this.formIsDirty = false;
            LoadEmployeeDeparmentWithFilter();
            return;
        }
        private Department GetDepartmentFromForm()
        {
            Department lDepartment = new Department();
            if (!string.IsNullOrEmpty(TextBoxEmpDepId.Text))
            {
                lDepartment.Id = Convert.ToInt64(TextBoxEmpDepId.Text);
            }
            else
            {
                lDepartment.Id = 0L;
            }
            lDepartment.Name = TextBoxDepartmentName.Text.Trim();
            lDepartment.Discription = TextBoxDepartmentDescription.Text.Trim();
            lDepartment.DisplayAs = TextBoxDepartmentDisplayAs.Text.Trim();
            lDepartment.CompanyId = Global.Company.CompanyId;
            if (comboBoxSwapTextBoxParentDepartment.SelectedIndex > -1)
            {
                Department Department = (Department)comboBoxSwapTextBoxParentDepartment.Items[comboBoxSwapTextBoxParentDepartment.SelectedIndex];
                if (Department != null)
                {
                    Department = DepartmentManager.GetDepartmentInfoById(Department.Id);
                    if (Department != null)
                    {
                        lDepartment.ParentDepartmentId = Department.Id;
                        lDepartment.IsSubDepartment = true;
                    }
                }
            }
            else
            {
                lDepartment.IsSubDepartment = false;
            }
            return lDepartment;
        }
        private Employee GetEmployeeFromForm()
        {
            Employee lEmployee = new Employee();
            if (!string.IsNullOrEmpty(TextBoxEmpDepId.Text))
            {
                lEmployee.Id = Convert.ToInt64(TextBoxEmpDepId.Text);
            }
            else
            {
                lEmployee.Id = 0L;
            }
            lEmployee.Name = TextBoxEmployeeName.Text.Trim();
            lEmployee.DisplayAs = TextBoxEmployeeLegalName.Text.Trim();
            if (comboBoxSwapTextBoxEmployeeDepartment.SelectedIndex > -1)
            {
                Department Department = (Department)comboBoxSwapTextBoxEmployeeDepartment.Items[comboBoxSwapTextBoxEmployeeDepartment.SelectedIndex];
                if (Department != null)
                {
                    Department = DepartmentManager.GetDepartmentInfoById(Department.Id);
                    if (Department != null)
                    {
                        lEmployee.DepartmentId = Department.Id;
                    }
                }
            }
            if (DateTimePickerEmployee.Date != null && DateUtils.ValidDate_TillCurrentDate(((DateTime)DateTimePickerEmployee.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                lEmployee.DateOfBirth = DateTimePickerEmployee.Date;
            }
            else
            {
                lEmployee.DateOfBirth = null;
            }
            if (ComboBoxSwapTextBoxEmployeeTitle.SelectedIndex > -1)
            {
                Title Title = (Title)ComboBoxSwapTextBoxEmployeeTitle.Items[ComboBoxSwapTextBoxEmployeeTitle.SelectedIndex];
                if (Title != null)
                {
                    Title = TitleManager.GetTitleInfoById(Title.Id);
                    if (Title != null)
                    {
                        lEmployee.TitleId = Title.Id;
                    }
                }
            }
            if (CameraPhotoTaker.Photo != null)
            {
                MemoryStream Stream = new MemoryStream();
                CameraPhotoTaker.Photo.Save(Stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                lEmployee.EmployeePhoto = Stream.ToArray();
            }
            lEmployee.CompanyId = Global.Company.CompanyId;
            lEmployee.Address = GetEmployeeAddressFromForm();
            lEmployee.ContactInfo = GetEmployeeContactInfoFromForm();
            lEmployee.TaxInfo = GetEmployeeTaxInfoFromForm();
            lEmployee.IsServiceProvider = checkBoxIsServiceProvider.Checked;
            lEmployee.IsRxSymbolDisplayOn = CheckBoxRxSymbolPresc.Checked;
            lEmployee.IsRSignatureDisplayOn = CheckBoxAllowDigtalsignature.Checked;
            if (DigitalSignaturePictureBoxPresc.Image != null)
            {
                MemoryStream Stream = new MemoryStream();
                DigitalSignaturePictureBoxPresc.Image.Save(Stream, System.Drawing.Imaging.ImageFormat.Png);
                lEmployee.DigitalSignature = Stream.ToArray();
            }
            return lEmployee;
        }
        private Address GetEmployeeAddressFromForm()
        {
            Address lAddress = new Address();
            lAddress.AddressLine1 = AddressGroupBoxEmployee.AddressLine1.Trim();
            lAddress.AddressLine2 = AddressGroupBoxEmployee.AddressLine2.Trim();
            lAddress.CityOrTown = AddressGroupBoxEmployee.CityName.Trim();
            lAddress.District = AddressGroupBoxEmployee.DistrictName.Trim();
            lAddress.PinCode = AddressGroupBoxEmployee.PinCode.Trim();
            lAddress.StatesId = AddressGroupBoxEmployee.StateId;
            return lAddress;
        }
        private TaxInfo GetEmployeeTaxInfoFromForm()
        {
            TaxInfo lTaxInfo = new TaxInfo();
            lTaxInfo.PAN = TextBoxEmployeePan.Text.Trim();
            return lTaxInfo;
        }
        private ContactInfo GetEmployeeContactInfoFromForm()
        {
            ContactInfo lContactInfo = new ContactInfo();
            lContactInfo.Phone = TextBoxEmployeePhone.Text.Trim().Replace("-", "");
            lContactInfo.Mobile = TextBoxEmployeeMobile.Text.Trim().Replace("-", "");
            lContactInfo.Fax = TextBoxEmployeeFax.Text;
            lContactInfo.Email = TextBoxEmployeeEmail.Text;
            return lContactInfo;
        }
        private void TreeViewEmployeeDepartment_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            node.SelectedImageIndex = node.ImageIndex;
            ResetForm();
            loadCombo();
            LoadEmpDepInfo();
            EnableForm(false);
            this.formIsDirty = false;
        }
        private void loadCombo()
        {
            ComboUtils.InitializeDepartmentCombo(comboBoxSwapTextBoxEmployeeDepartment, Global.Company.CompanyId);
            ComboUtils.InitializeTitleCombo(ComboBoxSwapTextBoxEmployeeTitle, Global.Company.CompanyId, Global.softwareType);
            ComboUtils.InitializeParentDepartmentCombo(comboBoxSwapTextBoxParentDepartment, Global.Company.CompanyId);
        }
        private void TextBoxSearch_TextChanged(object sender, EventArgs e)
        {
            ResetForm();
            LoadEmployeeDeparmentWithFilter();
            if (TreeViewEmployeeDepartment.Nodes.Count > 0) { BtnDepartmentEdit.Enabled = true; BtnDelete.Enabled = true; }
            else { BtnDepartmentEdit.Enabled = false; BtnDelete.Enabled = false; }
            this.formIsDirty = false;
        }
        private void BtnAddTitle_Click(object sender, EventArgs e)
        {
            FormJobTitle FormJobTitle = new FormJobTitle();
            FormJobTitle.CreateTitleOnLoad = false;
            FormJobTitle.ShowDialog(this);
            ComboUtils.InitializeTitleCombo(ComboBoxSwapTextBoxEmployeeTitle, Global.Company.CompanyId, Global.softwareType);
            ComboBoxSwapTextBoxEmployeeTitle.Select();
        }
        private Boolean validateFormEmployee()
        {
            EmployeeDepartmentErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxEmployeeName.Text.Trim()))
            {
                EmployeeDepartmentErrorMsg.Text = string.Format(EnterNameErrorMsg, "Employee");
                TabControlEmployee.SelectedTab = TabPageEmployeeDetails;
                TextBoxEmployeeName.Select();
                return false;
            }
            if (comboBoxSwapTextBoxEmployeeDepartment.SelectedIndex < 0)
            {
                EmployeeDepartmentErrorMsg.Text = ChooseDepartmentErrorMsg;
                TabControlEmployee.SelectedTab = TabPageEmployeeDetails;
                comboBoxSwapTextBoxEmployeeDepartment.Select();
                return false;
            }
            if (comboBoxSwapTextBoxEmployeeDepartment.SelectedIndex >= 0 && DepartmentManager.GetDepartmentInfoById(((Department)comboBoxSwapTextBoxEmployeeDepartment.Items[comboBoxSwapTextBoxEmployeeDepartment.SelectedIndex]).Id) == null)
            {
                EmployeeDepartmentErrorMsg.Text = SelectedDepartmentNotValidErrorMsg;
                TabControlEmployee.SelectedTab = TabPageEmployeeDetails;
                comboBoxSwapTextBoxEmployeeDepartment.Select();
                return false;
            }
            if (DateTimePickerEmployee.Date == null || !DateUtils.ValidDate_TillCurrentDate(((DateTime)DateTimePickerEmployee.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat) && !DateTimePickerEmployee.Isempty())
            {
                EmployeeDepartmentErrorMsg.Text = EnterDateErrorMsg;
                TabControlEmployee.SelectedTab = TabPageEmployeeDetails;
                DateTimePickerEmployee.Focus();
                return false;
            }
            if (ComboBoxSwapTextBoxEmployeeTitle.SelectedIndex < 0)
            {
                EmployeeDepartmentErrorMsg.Text = ChooseTitleErrorMsg;
                TabControlEmployee.SelectedTab = TabPageEmployeeDetails;
                ComboBoxSwapTextBoxEmployeeTitle.Select();
                return false;
            }
            if (ComboBoxSwapTextBoxEmployeeTitle.SelectedIndex >= 0 && TitleManager.GetTitleInfoById(((Title)ComboBoxSwapTextBoxEmployeeTitle.Items[ComboBoxSwapTextBoxEmployeeTitle.SelectedIndex]).Id) == null)
            {
                EmployeeDepartmentErrorMsg.Text = SelectedTitleNotValidErrorMsg;
                TabControlEmployee.SelectedTab = TabPageEmployeeDetails;
                ComboBoxSwapTextBoxEmployeeTitle.Select();
                return false;
            }
            if (AddressGroupBoxEmployee.StateId == 0L)
            {
                EmployeeDepartmentErrorMsg.Text = ChooseStateErrorMsg;
                TabControlEmployee.SelectedTab = TabPageEmployeeDetails;
                AddressGroupBoxEmployee.selected_field(Fields.state);
                return false;
            }
            //if (!string.IsNullOrEmpty(TextBoxEmployeePhone.Text.Trim()) && !KeypressValidation.PhoneValidation((string)TextBoxEmployeePhone.Text.Trim('-')))
            //{                
            //    EmployeeDepartmentErrorMsg.Text = EnterPhoneNumberErrorMsg;
            //    TabControlEmployee.SelectedTab = TabPageContactDetail;
            //    TextBoxEmployeePhone.Select();
            //    return false;
            //}
            //if (!string.IsNullOrEmpty(TextBoxEmployeeMobile.Text.Trim()) && !KeypressValidation.MobileValidation((string)TextBoxEmployeeMobile.Text.Trim('-')))
            //{
            //    EmployeeDepartmentErrorMsg.Text = EnterMobileNumberErrorMsg;
            //    TabControlEmployee.SelectedTab = TabPageContactDetail;
            //    TextBoxEmployeeMobile.Select();
            //    return false;
            //}
            if (!string.IsNullOrEmpty(TextBoxEmployeeEmail.Text.Trim()) && !KeypressValidation.EmailValidation((string)TextBoxEmployeeEmail.Text.Trim()))
            {
                EmployeeDepartmentErrorMsg.Text = EnterEmployeeEmailErrorMsg;
                TabControlEmployee.SelectedTab = TabPageContactDetail;
                TextBoxEmployeeEmail.Select();
                return false;
            }
            return true;
        }
        private bool isEditMode = false;
        private void BtnDepartmentEdit_Click(object sender, EventArgs e)
        {
            isEditMode = true;
            LoadEmpDepInfo();
            EnableForm(true);
            if (TabControlDepartment.Visible)
            {
                if (comboBoxSwapTextBoxParentDepartment.SelectedIndex < 0)
                {
                    comboBoxSwapTextBoxParentDepartment.Visible = false;
                }
                TabControlDepartment.SelectedTab = TabPageDepartmentDetails;
                TextBoxDepartmentName.Select();
            }
            else if (TabControlEmployee.Visible)
            {
                TabControlEmployee.SelectedTab = TabPageEmployeeDetails;
                CameraPanel.Enabled = true;
                TextBoxEmployeeName.Select();
            }
            this.formIsDirty = false;
        }
        private void FocusEmpDep()
        {
            if (TabControlEmployee.Visible)
            {
                TabControlEmployee.SelectedTab = TabPageEmployeeDetails;
                TextBoxEmployeeName.Select();
            }
            else
            {
                TextBoxDepartmentName.Select();
            }
        }
        private void BtnDepartmentCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    FocusEmpDep();
                    return;
                }
            }
            ResetForm();
            loadCombo();

            if (string.IsNullOrEmpty(TextBoxSearch.Text))
            {
                LoadEmployeeDeparmentWithFilter();
                //LoadEmpDepInfo();
            }
            else
            {
                TextBoxSearch.Clear();
            }
            //TextBoxSearch.Select();
            EnableForm(false);
            SetFocus();
            this.formIsDirty = false;
        }
        private void SetFocus()
        {
            TreeViewEmployeeDepartment.SelectedNode = null;
            if (TreeViewEmployeeDepartment.Nodes.Count > 0)
            {
                TextBoxSearch.Select();
            }
            else
            {
                BtnNew.Focus();
            }
        }
        private void BtnDepartmentSave_Click(object sender, EventArgs e)
        {
            if (TabControlDepartment.Visible && ValidateFormDepartment())
            {
                Department DepartmentInfo = GetDepartmentFromForm();
                if (DepartmentManager.DepartmentNameUniqueById(DepartmentInfo))
                {
                    Department lDepartmentFromDB = null;
                    if (DepartmentInfo.Id == 0)
                    {
                        lDepartmentFromDB = DepartmentManager.AddDepartment(DepartmentInfo);
                    }
                    else
                    {
                        Department lDepartmentById = DepartmentManager.GetDepartmentInfoById(DepartmentInfo.Id);
                        if (lDepartmentById != null)
                        {
                            lDepartmentFromDB = DepartmentManager.UpdateDepartment(DepartmentInfo);
                        }
                        else
                        {
                            DisplaySystemError(SelectedDepartmentNotValidErrorMsg);
                            return;
                        }
                    }
                    if (CreateEmployeeOnLoad)
                    {
                        Employee_Load(sender, e);
                        return;
                    }
                    ResetForm();
                    TabControlDepartment.SelectedTab = TabPageDepartmentDetails;
                    loadCombo();
                    if (string.IsNullOrEmpty(TextBoxSearch.Text))
                    {
                        LoadEmployeeDeparmentWithFilter();
                    }
                    else
                    {
                        TextBoxSearch.Clear();
                    }
                    PointSaveorUpdatedNode(TreeViewEmployeeDepartment.Nodes, lDepartmentFromDB.Id.ToString());
                    EnableForm(false);
                    this.formIsDirty = false;
                }
                else
                {
                    EmployeeDepartmentErrorMsg.Text = string.Format(UniqueNameErrorMsg, "Department", DepartmentInfo.Name);
                    TextBoxDepartmentName.Select();
                }
            }
            else if (TabControlEmployee.Visible && validateFormEmployee())
            {
                Employee lEmployee = GetEmployeeFromForm();
                if (EmployeeManager.EmployeeNameUniqueById(lEmployee))
                {
                    Employee lEmployeeFromDB = null;
                    if (lEmployee.Id == 0)
                    {
                        lEmployeeFromDB = EmployeeManager.AddEmployee(lEmployee);
                    }
                    else
                    {
                        Employee lEmployeeById = EmployeeManager.GetEmployeeInfoById(lEmployee.Id);
                        if (lEmployeeById != null)
                        {
                            lEmployee.ContactInfoId = lEmployee.ContactInfo.Id = (long)lEmployeeById.ContactInfoId;
                            lEmployee.AddressId = lEmployee.Address.AddressId = (long)lEmployeeById.AddressId;
                            lEmployee.TaxInfoId = lEmployee.TaxInfo.Id = (long)lEmployeeById.TaxInfoId;
                            lEmployeeFromDB = EmployeeManager.UpdateEmployee(lEmployee);
                            AddressManager.UpdateAddress(lEmployee.Address);
                            TaxinfoManager.UpdateTaxInfo(lEmployee.TaxInfo);
                            ContactInfoManager.UpdateContactInfo(lEmployee.ContactInfo);
                        }
                        else
                        {
                            DisplaySystemError(SelectedEmployeeNotValidErrorMsg);
                            return;
                        }
                    }
                    if (checkBoxIsServiceProvider.Checked && lEmployee?.Id != null)
                    {
                        IList<Consultation> Consultations = ConsultationManager.Instance.ListConsultationByCompanyId(Global.Company.CompanyId);
                        if (Consultations !=null && Consultations.Count > 0)
                        {
                            foreach (Consultation consultation in Consultations)
                            {
                                ConsultationDetail lconsultationDetail = ConsultationDetailManager.Instance.GetConsultationDetailByEmployeeId(Global.Company.CompanyId, lEmployee.Id, consultation);
                                if (lconsultationDetail == null)
                                {
                                    ConsultationDetail consultationDetail = new ConsultationDetail();
                                    consultationDetail.ConsultationId = consultation.Id;
                                    consultationDetail.EmployeeId = lEmployee.Id;
                                    consultationDetail.Fee = 0.00;
                                    consultationDetail.CompanyId = Global.Company.CompanyId;
                                    ConsultationDetailManager.Instance.AddConsultationDetail(consultationDetail);
                                }
                            }
                        }
                    }
                    if (CreateEmployeeOnLoad)
                    {
                        this.formIsDirty = false;
                        this.Close();
                    }
                    ResetForm();
                    TabControlEmployee.SelectedTab = TabPageEmployeeDetails;
                    loadCombo();
                    if (string.IsNullOrEmpty(TextBoxSearch.Text))
                    {
                        LoadEmployeeDeparmentWithFilter();
                    }
                    else
                    {
                        TextBoxSearch.Clear();
                    }
                    PointSaveorUpdatedNode(TreeViewEmployeeDepartment.Nodes, lEmployeeFromDB.Id.ToString() + "@");
                    EmployeeDepartmentErrorMsg.Text = SaveSuccessText;
                    EnableForm(false);
                    this.formIsDirty = false;
                }
                else
                {
                    EmployeeDepartmentErrorMsg.Text = string.Format(UniqueNameErrorMsg, "Employee", lEmployee.Name);
                    TextBoxEmployeeName.Select();
                }
            }
        }
        private void PointSaveorUpdatedNode(TreeNodeCollection nodes, string findtext)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Name.ToString().Trim() == findtext)
                {
                    node.Expand();
                    node.TreeView.SelectedNode = node.NextNode;
                    TreeViewEmployeeDepartment.SelectedNode = node;
                    node.TreeView.Focus();
                    break;
                }
                PointSaveorUpdatedNode(node.Nodes, findtext);
            }
        }
        private Boolean ValidateFormDepartment()
        {
            EmployeeDepartmentErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxDepartmentName.Text.Trim()))
            {
                EmployeeDepartmentErrorMsg.Text = string.Format(EnterNameErrorMsg, "Department");
                TextBoxDepartmentName.Select();
                return false;
            }

            if (comboBoxSwapTextBoxParentDepartment.SelectedIndex >= 0 && DepartmentManager.GetDepartmentInfoById(((Department)comboBoxSwapTextBoxParentDepartment.Items[comboBoxSwapTextBoxParentDepartment.SelectedIndex]).Id) == null)
            {
                EmployeeDepartmentErrorMsg.Text = SelectedParentNotValidErrorMsg;
                comboBoxSwapTextBoxParentDepartment.Select();
                return false;
            }
            return true;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (TabControlEmployee.Visible == true)
            {
                Employee EmployeeInfo = GetEmployeeInfo();
                EmployeeDepartmentErrorMsg.Text = "";
                if (EmployeeInfo != null)
                {
                    DialogResult Result = MessageBox.Show(string.Format(DeleteEmployeeConfirmText, EmployeeInfo.Name), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        if (EmployeeManager.DeleteEmployee(EmployeeInfo.Id))
                        {
                            ResetForm();
                            TabControlEmployee.SelectedTab = TabPageEmployeeDetails;
                            loadCombo();
                            LoadEmployeeDeparmentWithFilter();
                            TextBoxSearch.Clear();
                            EnableForm(false);
                            SetFocus();
                            EmployeeDepartmentErrorMsg.Text = string.Format(RemoveSuccess, "Employee ", EmployeeInfo.Name);
                            this.formIsDirty = false;
                        }
                        else
                        {
                            EmployeeDepartmentErrorMsg.Text = string.Format(DeleteNotAllowErrorText, "Employee");
                        }
                    }
                }
                else
                {
                    DisplaySystemError(SelectedEmployeeNotValidErrorMsg);
                    return;
                }
            }
            else if (TabControlDepartment.Visible == true)
            {
                Department DepartmentInfo = GetDepartmentInfo();
                EmployeeDepartmentErrorMsg.Text = "";
                if (DepartmentInfo != null)
                {
                    DialogResult Result = MessageBox.Show(string.Format(DeleteDepartmentConfirmText, DepartmentInfo.Name), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        if (DepartmentManager.DeleteDepartment(DepartmentInfo.Id))
                        {
                            ResetForm();
                            TabControlDepartment.SelectedTab = TabPageDepartmentDetails;
                            loadCombo();
                            LoadEmployeeDeparmentWithFilter();
                            TextBoxSearch.Clear();
                            EnableForm(false);
                            SetFocus();
                            EmployeeDepartmentErrorMsg.Text = string.Format(RemoveSuccess, "Department ", DepartmentInfo.Name);
                            this.formIsDirty = false;
                        }
                        else
                        {
                            EmployeeDepartmentErrorMsg.Text = string.Format(DeleteNotAllowErrorText, "Department");
                        }
                    }
                }
                else
                {
                    DisplaySystemError(SelectedDepartmentNotValidErrorMsg);
                    return;
                }
            }
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void newDepartmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextBoxSearch.ResetText();
            TabControlDepartment.Visible = true;
            TabControlEmployee.Visible = false;
            NewClear();
            if (TreeViewEmployeeDepartment != null && TreeViewEmployeeDepartment.SelectedNode != null && !TreeViewEmployeeDepartment.SelectedNode.Name.Contains('@'))
            {
                comboBoxSwapTextBoxParentDepartment.SelectedIndex = comboBoxSwapTextBoxParentDepartment.FindStringExact(TreeViewEmployeeDepartment.SelectedNode.Text);
            }
            this.formIsDirty = false;
        }
        private void newDepartmentToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TabControlDepartment.Visible = true;
            TabControlEmployee.Visible = false;
            NewClear();
            this.formIsDirty = false;
        }
        private void newEmployeeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AddressGroupBoxEmployee.CountryId = (long)Global.Company.CountryId;
            if (DepartmentManager.ListDepartmentByCompanyId(Global.Company.CompanyId).Count > 0)
            {
                TabControlEmployee.Visible = true;
                TabControlEmployee.SelectedTab = TabPageEmployeeDetails;
                TabControlDepartment.Visible = false;
                TextBoxSearch.ResetText();
                NewClear();
                ResetForm();
                EnableForm(true);
                if (TreeViewEmployeeDepartment != null && TreeViewEmployeeDepartment.SelectedNode != null)
                {
                    Employee EmployeeFromDB = EmployeeManager.GetEmployeeInfoById(long.Parse(TreeViewEmployeeDepartment.SelectedNode.Name.Trim('@')));
                    if (EmployeeFromDB != null)
                    {
                        comboBoxSwapTextBoxEmployeeDepartment.Text = EmployeeFromDB.Department.Name;
                    }
                    else
                    {
                        comboBoxSwapTextBoxEmployeeDepartment.SelectedIndex = comboBoxSwapTextBoxEmployeeDepartment.FindStringExact(TreeViewEmployeeDepartment.SelectedNode.Text);
                        //comboBoxSwapTextBoxEmployeeDepartment.Text = TreeViewEmployeeDepartment.SelectedNode.Text;
                    }
                }
                CameraPanel.Enabled = true;
                TextBoxEmployeeName.Select();
                this.formIsDirty = false;
            }
            else
            {
                newDepartmentToolStripMenuItem.PerformClick();
                EmployeeDepartmentErrorMsg.Text = NoDepartmentFoundErrorMsg;
            }
            this.formIsDirty = false;
        }
        private void newEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddressGroupBoxEmployee.CountryId = (long)Global.Company.CountryId;
            if (DepartmentManager.ListDepartmentByCompanyId(Global.Company.CompanyId).Count > 0)
            {
                TabControlEmployee.Visible = true; ;
                TabControlEmployee.SelectedTab = TabPageEmployeeDetails;
                TabControlDepartment.Visible = false;
                NewClear();
                ResetForm();
                EnableForm(true);
                if (TreeViewEmployeeDepartment != null && TreeViewEmployeeDepartment.SelectedNode != null)
                {
                    Employee EmployeeFromDB = EmployeeManager.GetEmployeeInfoById(long.Parse(TreeViewEmployeeDepartment.SelectedNode.Name.Trim('@')));
                    if (EmployeeFromDB != null)
                    {
                        comboBoxSwapTextBoxEmployeeDepartment.Text = EmployeeFromDB.Department.Name;
                    }
                    else
                    {
                        comboBoxSwapTextBoxEmployeeDepartment.SelectedIndex = comboBoxSwapTextBoxEmployeeDepartment.FindStringExact(TreeViewEmployeeDepartment.SelectedNode.Text);
                    }
                }
                CameraPanel.Enabled = true;
                TextBoxEmployeeName.Select();
                this.formIsDirty = false;
            }
            else
            {
                newDepartmentToolStripMenuItem.PerformClick();
                EmployeeDepartmentErrorMsg.Text = NoDepartmentFoundErrorMsg;
            }
            this.formIsDirty = false;
        }

        private void BtnNew_ItemClickedEvent(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Department")
            {
                newDepartmentToolStripMenuItem1_Click(sender, e);
            }
            else if (e.ClickedItem.Text == "Employee")
            {
                ClearSignatureDetails();
                TabControlEmployee.TabPages.Remove(TabPageSignatureDetails);
                newEmployeeToolStripMenuItem1_Click(sender, e);
            }
        }
        private void NewClear()
        {
            ResetForm();
            EnableForm(true);
            loadCombo();
            if (TabControlEmployee.Visible)
            {
                if (TreeViewEmployeeDepartment.SelectedNode != null)
                {
                    comboBoxSwapTextBoxEmployeeDepartment.Text = TreeViewEmployeeDepartment.SelectedNode.Text;
                }
                TextBoxEmployeeName.Select();
            }
            else { TextBoxDepartmentName.Select(); };
        }
        private void ResetForm()
        {
            AddressGroupBoxEmployee.Clear();
            AddressGroupBoxEmployee.StateId = (long)Global.Company.Address.StatesId!;
            EmployeeDepartmentErrorMsg.Text = "";
            TextBoxDepartmentDescription.ResetText();
            TextBoxDepartmentDisplayAs.ResetText();
            TextBoxDepartmentName.ResetText();
            TextBoxEmpDepId.ResetText();
            TextBoxEmployeeEmail.ResetText();
            TextBoxEmployeeFax.ResetText();
            TextBoxEmployeeLegalName.ResetText();
            TextBoxEmployeeMobile.ResetText();
            TextBoxEmployeeName.ResetText();
            TextBoxEmployeePan.ResetText();
            TextBoxEmployeePhone.ResetText();
            checkBoxIsServiceProvider.Checked = false;
            comboBoxSwapTextBoxEmployeeDepartment.SelectedIndex = -1;
            comboBoxSwapTextBoxParentDepartment.SelectedIndex = -1;
            ComboBoxSwapTextBoxEmployeeTitle.SelectedIndex = -1;
            DateTimePickerEmployee.Format = Global.Company.DateFormat;
            DateTimePickerEmployee.MaxDate = (DateTime)DateUtils.ToDate(DateTime.Now.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            DateTimePickerEmployee.Reset();
            CameraPhotoTaker.Clear();
            CameraPanel.Enabled = false;
            LabelParentDepartment.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);
        }
        private void EnableForm(Boolean enable)
        {
            if (DepartmentManager.ListDepartmentByCompanyId(Global.Company.CompanyId).Count > 0)
            {
                TreeViewEmployeeDepartment.Enabled = !enable;
                TextBoxSearch.ReadOnly = enable;
                TextBoxSearch.TabStop = !enable;
            }
            else
            {
                TreeViewEmployeeDepartment.Enabled = false;
                TabControlDepartment.Visible = true;
                TabControlEmployee.Visible = false;
                TextBoxSearch.ReadOnly = true;
                TextBoxSearch.TabStop = false;
            }
            TextBoxDepartmentDescription.ReadOnly = !enable;
            TextBoxDepartmentDisplayAs.ReadOnly = !enable;
            TextBoxDepartmentName.ReadOnly = !enable;
            AddressGroupBoxEmployee.ReadOnly = !enable;
            TextBoxEmployeeEmail.ReadOnly = !enable;
            TextBoxEmployeeFax.ReadOnly = !enable;
            TextBoxEmployeeLegalName.ReadOnly = !enable;
            TextBoxEmployeeMobile.ReadOnly = !enable;
            TextBoxEmployeeName.ReadOnly = !enable;
            TextBoxEmployeePan.ReadOnly = !enable;
            TextBoxEmployeePhone.ReadOnly = !enable;
            checkBoxIsServiceProvider.Enabled = enable;
            TextBoxDepartmentDescription.TabStop = enable;
            TextBoxDepartmentDisplayAs.TabStop = enable;
            TextBoxDepartmentName.TabStop = enable;
            AddressGroupBoxEmployee.TabStop = enable;
            TextBoxEmployeeEmail.TabStop = enable;
            TextBoxEmployeeFax.TabStop = enable;
            TextBoxEmployeeLegalName.TabStop = enable;
            TextBoxEmployeeMobile.TabStop = enable;
            TextBoxEmployeeName.TabStop = enable;
            TextBoxEmployeePan.TabStop = enable;
            TextBoxEmployeePhone.TabStop = enable;

            comboBoxSwapTextBoxEmployeeDepartment.Visible = enable;
            ComboBoxSwapTextBoxEmployeeTitle.Visible = enable;
            DateTimePickerEmployee.Enabled = enable;
            comboBoxSwapTextBoxParentDepartment.Visible = enable;
            comboBoxSwapTextBoxParentDepartment.TabStop = enable;
            BtnAddTitle.Enabled = enable;

            CheckBoxRxSymbolPresc.Enabled = enable;
            CheckBoxAllowDigtalsignature.Enabled = enable;

            BtnDigitalSignatureImpoortPresc.Enabled = enable;
            BtnDigitalSignatureDeletePresc.Enabled = enable;
            if (enable && CheckBoxAllowDigtalsignature.Checked == false)
            {
                BtnDigitalSignatureImpoortPresc.Enabled = !enable;
                BtnDigitalSignatureDeletePresc.Enabled = !enable;
            }
            if (!enable)
            {
                BtnDepartmentCancel.Enabled = enable;
                if (TreeViewEmployeeDepartment.SelectedNode == null)
                {
                    BtnDelete.Enabled = enable;
                    BtnDepartmentEdit.Enabled = enable;

                }
                else
                {
                    BtnDelete.Enabled = !enable;
                    BtnDepartmentEdit.Enabled = !enable;
                }
                BtnNew.Enabled = !enable;
                BtnDepartmentSave.Enabled = enable;
            }
            else
            {
                BtnDepartmentCancel.Enabled = enable;
                BtnDelete.Enabled = !enable;
                BtnDepartmentEdit.Enabled = !enable;
                BtnNew.Enabled = !enable;
                BtnDepartmentSave.Enabled = enable;
            }
        }
        private void AddressGroupBoxEmployee_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                TabControlEmployee.SelectedTab = TabPageContactDetail;
                TextBoxEmployeePhone.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlEmployee.SelectedTab = TabPageEmployeeDetails;
                checkBoxIsServiceProvider.Focus();
            }

        }
        private void TextBoxEmployeePhone_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                TabControlEmployee.SelectedTab = TabPageContactDetail;
                TextBoxEmployeeMobile.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlEmployee.SelectedTab = TabPageEmployeeDetails;
                AddressGroupBoxEmployee.selected_field(Fields.pin);
            }
        }
        private void BtnDepartmentSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                if (TabControlDepartment.Visible)
                {
                    TextBoxDepartmentName.Select();
                }
                else
                {
                    TabControlEmployee.SelectedTab = TabPageEmployeeDetails;
                    TextBoxEmployeeName.Select();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (TabControlDepartment.Visible)
                {
                    if (comboBoxSwapTextBoxParentDepartment.Visible)
                    { comboBoxSwapTextBoxParentDepartment.Select(); }
                    else
                    { TextBoxDepartmentDescription.Select(); }
                }
                else
                {
                    TabControlEmployee.SelectedTab = TabPageContactDetail;
                    TextBoxEmployeePan.Select();
                }
            }
        }

        private void TextBoxDepartmentName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                TextBoxDepartmentDisplayAs.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnDepartmentSave.Select();
            }
        }
        private void TextBoxEmployeeName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                TextBoxEmployeeLegalName.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnDepartmentSave.Select();
            }
        }
        private void TextBoxDepartmentName_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void comboBoxSwapTextBoxParentDepartment_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.comboBoxSwapTextBoxParentDepartment.DroppedDown = false;
        }
        private void TextBoxEmployeeName_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void comboBoxSwapTextBoxEmployeeDepartment_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.comboBoxSwapTextBoxEmployeeDepartment.DroppedDown = false;
        }
        private void TextBoxEmployeeEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_EmailChecking(sender, e);
        }
        private void TextBoxEmployeePan_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_TaxDetailsNumberChecking(sender, e);
        }
        private void TextBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                TreeViewEmployeeDepartment.Select();
            }
        }
        private void TextBoxDepartmentName_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "NameChecking");
        }
        private void TextBoxDepartmentName_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxDepartmentName, "NameChecking");
            }
        }
        private void TextBoxDepartmentDisplayAs_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxDepartmentDisplayAs, "NameChecking");
            }
        }
        private void TextBoxEmployeeName_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "NameChecking");
        }
        private void TextBoxEmployeeEmail_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "EmailChecking");
        }
        private void TextBoxEmployeePan_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "TaxDetailsNumberChecking");
        }
        private void TextBoxEmployeeName_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxEmployeeName, "NameChecking");
            }
        }
        private void TextBoxEmployeeLegalName_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxEmployeeLegalName, "NameChecking");
            }
        }
        private void TextBoxEmployeeEmail_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxEmployeeEmail, "EmailChecking");
            }
        }
        private void TextBoxEmployeePan_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxEmployeePan, "TaxDetailsNumberChecking");
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnNew.ShowDropDown();
            }
            if (keyData == (Keys.F4))
            {
                BtnDelete.PerformClick();
            }
            if (keyData == (Keys.F7))
            {
                BtnDepartmentEdit.PerformClick();
            }
            if (keyData == (Keys.F8))
            {
                BtnDepartmentSave.PerformClick();
            }
            if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                if (CreateEmployeeOnLoad)
                {
                    BtnExit.PerformClick();
                    return true;
                }
                BtnDepartmentCancel.PerformClick();
                return true;
            }
            if (keyData == Keys.Tab && ActiveControl == ComboBoxSwapTextBoxEmployeeTitle)
            {
                BtnAddTitle.Focus();
                return true;
            }
            if (keyData == Keys.Tab && ActiveControl == BtnAddTitle)
            {
                checkBoxIsServiceProvider.Focus();
                return true;
            }
            if (keyData == Keys.Tab && ActiveControl == checkBoxIsServiceProvider)
            {
                AddressGroupBoxEmployee.selected_field(Fields.address1);
                return true;
            }
            if (keyData == (Keys.Shift | Keys.Tab) && ActiveControl == checkBoxIsServiceProvider)
            {
                BtnAddTitle.Focus();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void FormEmployee_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty && !CreateEmployeeOnLoad)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    FocusEmpDep();
                    e.Cancel = true;
                }
            }
            if (CameraPhotoTaker.videocapture != null && !CameraPhotoTaker.videocapture.IsDisposed)
            {
                Application.Idle -= CameraPhotoTaker.Streaming!;
                CameraPhotoTaker.videocapture!.Release();
                CameraPhotoTaker.videocapture.Dispose();
                CameraPhotoTaker.isCameraRunning = false;
            }
        }

        private void comboBoxSwapTextBoxParentDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSwapTextBoxParentDepartment.SelectedIndex < 0) { LabelParentDepartment.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular); }
            else { LabelParentDepartment.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold); }
        }

        private void TreeViewEmployeeDepartment_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeViewEmployeeDepartment.SelectedNode = e.Node;
        }

        private void BtnDigitalSignatureImpoortPresc_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.AddExtension = true;
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Image Files(*.jpeg;*.bmp;*.png;*.jpg)|*.jpeg;*.bmp;*.png;*.jpg";

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DigitalSignaturePictureBoxPresc.Image = new Bitmap(openFileDialog.FileName);
            }
        }

        private void BtnDigitalSignatureDeletePresc_Click(object sender, EventArgs e)
        {
            DigitalSignaturePictureBoxPresc.Image = null;
        }

        private void CheckBoxAllowDigtalsignature_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxAllowDigtalsignature.Checked)
            {
                BtnDigitalSignatureImpoortPresc.Enabled = true;
                BtnDigitalSignatureDeletePresc.Enabled = true;
            }
            else
            {
                BtnDigitalSignatureImpoortPresc.Enabled = false;
                BtnDigitalSignatureDeletePresc.Enabled = false;
            }
        }
    }
}
