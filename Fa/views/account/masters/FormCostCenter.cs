using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using fa.api.Accounting;
using fa.model.Accounting.Masters;
using fa.libraries.Validation;
using fa.libraries.utils;
using fa.api.UserProfile;

namespace fa.views.account.masters
{
    public partial class FormCostCenter : FormBase
    {
        public static string SaveSuccessText = "Saved success...";
        public static string DeleteConfirmText = "Do you want to delete the Cost Center {0}?";
        public static string DeleteErrorText = "Error Deleting the Cost Center!, Please retry";
        public static string NotAllowDeleteErrorText = "Do not delete cost center, It used in some ware else";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";

        public static string UniqueCostCenterNameErrorMsg = "CostCenter {0} already exists in Company {1}";
        public static string EnterCostCenterNameErrorMsg = "Please enter CostCenter name";
        public static string ChooseParentErrorMsg = "Please choose parent Company";
        Container parent;
        UserManager UserManager = null;
        CompanyManager CompanyManager = null;
        CostCenterManager CostcenterManager = null;
        KeypressValidation Keypress_Validation = null;
        public FormCostCenter(object sender)
        {
            parent = (Container)sender;
            UserManager =  UserManager.Instance;
            CompanyManager = CompanyManager.Instance;
            CostcenterManager = CostCenterManager.Instance;
            Keypress_Validation =  KeypressValidation.Instance;
            InitializeComponent();
            excludedObjects = new string[] { "TextBoxCostCenterSearch" };

        }
        private void FormCostCenter_Load(object sender, EventArgs e)
        {
            try
            {
                WaitCursor(Cursors.WaitCursor);
                ResetForm();
                EnableForm(false);
                ComboUtils.InitializeCompanyCombo(ComboBoxCostCenterCompany, CompanyManager.GetAccessibleCompanies(Global.User));
                LoadCostCentersWithFilter();
                this.formIsDirty = false;
            }
            finally
            {
                WaitCursor(Cursors.Default);
            }

           
        }

        private void LoadCostCentersWithFilter()
        {
            Cursor.Current = Cursors.WaitCursor;
            string FilterString = TextBoxCostCenterSearch.Text.Trim();
            TreeViewCostCenter.Nodes.Clear();
            IList<CostCenter> CostCenter = CostcenterManager.GetAllCostCenter();
            if (CostCenter != null)
            {
                Company lCompany = null;
                foreach (var lCostCenter in CostCenter)
                {
                    if (FilterString == null || string.IsNullOrEmpty(FilterString.Trim()) || lCostCenter.Name.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        lCompany = CompanyManager.GetCompany(lCostCenter.ParentCompanyId);
                    TreeNode[] treeNodes = TreeViewCostCenter.Nodes.Cast<TreeNode>().Where(r => r.Text == lCompany.Name).ToArray();
                    if (treeNodes.Length == 0)
                    {
                        TreeViewCostCenter.Nodes.Add(lCompany.CompanyId.ToString(), lCompany.Name);
                        TreeViewCostCenter.Nodes[lCompany.CompanyId.ToString()].ImageIndex = 0;
                        foreach (var llCostCenter in CostCenter)
                        {
                            if (FilterString == null || string.IsNullOrEmpty(FilterString.Trim()) || llCostCenter.Name.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1)
                            {
                                if (lCostCenter.ParentCompanyId == llCostCenter.ParentCompanyId)
                                {
                                    TreeViewCostCenter.Nodes[lCompany.CompanyId.ToString()].Nodes.Add(llCostCenter.CostCenterId.ToString(), llCostCenter.Name);
                                    TreeViewCostCenter.Nodes[lCompany.CompanyId.ToString()].Nodes[llCostCenter.CostCenterId.ToString()].ImageIndex = 1;
                                }
                            }
                        }                    
                    }
                    }                            
                }
            }
            if (TreeViewCostCenter.Nodes.Count > 0)
            {               
                TreeViewCostCenter.ExpandAll();               
                TreeViewCostCenter.SelectedNode = TreeViewCostCenter.Nodes[0].Nodes[0];
            }
        }

        private CostCenter GetCostCenterInfo()
        {
            if (TreeViewCostCenter.SelectedNode!=null)
            {
                
                CostCenter lCostCenter = CostcenterManager.GetCostCenterById(TreeViewCostCenter.SelectedNode.Parent==null?long.Parse(TreeViewCostCenter.SelectedNode.Nodes[0].Name) :long.Parse(TreeViewCostCenter.SelectedNode.Name));
                if (lCostCenter != null)
                {
                    CostCenter CostCenterFromDB = CostcenterManager.GetCostCenterById(lCostCenter.CostCenterId);
                    return CostCenterFromDB;
                }
            }
            return null;
        }

        private void LoadCostCenterInfo()
        {
            Cursor.Current = Cursors.WaitCursor;
            CostCenter CostCenterFromDB = GetCostCenterInfo();
            if (CostCenterFromDB != null)
            {
                TextBoxCostcenterId.Text = CostCenterFromDB.CostCenterId.ToString();
                TextBoxCostCenterName.Text = CostCenterFromDB.Name;
                TextBoxCostCenterDisplayas.Text = CostCenterFromDB.DisplayName;
                TextBoxCostCenterDescription.Text = CostCenterFromDB.Description;
                if (!string.IsNullOrEmpty(CostCenterFromDB.ParentCompanyId.ToString()))
                {
                    Company lCompany = CompanyManager.GetCompany(CostCenterFromDB.ParentCompanyId);
                    if (lCompany != null)
                    {
                        ComboBoxCostCenterCompany.SelectedIndex = ComboBoxCostCenterCompany.FindStringExact(lCompany.Name);
                        TextBoxCostCenterParentCompany.Text = lCompany.Name;
                    }
                }
            }
            else
            {
                DisplaySystemError("Somthing went wrong, the selected costcenter is not valid.");
                return;
            }
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            this.formIsDirty = false;
            LoadCostCentersWithFilter();
            return;
        }
        private CostCenter GetCostCenterFromForm()
        {
            CostCenter lCostCenter = new CostCenter();
            if (!string.IsNullOrEmpty(TextBoxCostcenterId.Text))
            {
                lCostCenter.CostCenterId = Convert.ToInt64(TextBoxCostcenterId.Text);
            }
            else
            {
                lCostCenter.CostCenterId = 0L;
            }
            lCostCenter.Name = TextBoxCostCenterName.Text.Trim();
            lCostCenter.DisplayName = TextBoxCostCenterDisplayas.Text.Trim();
            lCostCenter.Description = TextBoxCostCenterDescription.Text.Trim();
            if (ComboBoxCostCenterCompany.SelectedIndex > -1)
            {
                Company lCompany = (Company)ComboBoxCostCenterCompany.Items[ComboBoxCostCenterCompany.SelectedIndex];
                if (lCompany != null)
                {
                    lCompany = CompanyManager.GetCompany(lCompany.CompanyId);
                    if (lCompany != null)
                    {
                        lCostCenter.ParentCompanyId = lCompany.CompanyId;                        
                    }
                }
            }
            return lCostCenter;
        }       
        private void TreeViewCostcenter_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            node.SelectedImageIndex = node.ImageIndex;
            ResetForm();
            ComboUtils.InitializeCompanyCombo(ComboBoxCostCenterCompany, CompanyManager.GetAccessibleCompanies(Global.User));
            LoadCostCenterInfo();
            EnableForm(false);
            this.formIsDirty = false;
        }
        private void TextBoxCostcenterSearch_TextChanged(object sender, EventArgs e)
        {
            ResetForm();
            LoadCostCentersWithFilter();
            if (TreeViewCostCenter.Nodes.Count > 0) { BtnCostCenterEdit.Enabled = true; BtnCostCenterDelete.Enabled = true; }
            else { BtnCostCenterEdit.Enabled = false; BtnCostCenterDelete.Enabled = false; }
            this.formIsDirty = false;
        }
        private void LoadCostCenterInContainer()
        {
            IList<Company> Company = CompanyManager.GetAccessibleCompanies(Global.User);
            if (Company.Count > 0)
            {               
                parent.RenderLoginDetails();
            }
        }
        private void WaitCursor(Cursor lCursor)
        {
            Cursor.Current = lCursor;
        }
        private void BtnCostcenterNew_Click(object sender, EventArgs e)
        {
            WaitCursor(Cursors.WaitCursor);
            ResetForm();
            EnableForm(true);
            ComboUtils.InitializeCompanyCombo(ComboBoxCostCenterCompany, CompanyManager.GetAccessibleCompanies(Global.User));
            TextBoxCostCenterName.Select();
            this.formIsDirty = false;
            WaitCursor(Cursors.Default);
        }
        private void BtnCostcenterDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxCostcenterId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this costcenter is still valid.");
                return;
            }
            CostCenter CostCenterInfo = GetCostCenterInfo();
            if (CostCenterInfo != null)
            {
                DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText,CostCenterInfo.Name), "Delete Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.Yes)
                {
                    WaitCursor(Cursors.WaitCursor);
                    if (CostcenterManager.DeleteCostCenter(CostCenterInfo.CostCenterId))
                    {
                        if (CostCenterInfo.CostCenterId == Global.CostCenter.CostCenterId)
                        {
                            Global.CostCenter = null;
                        }
                        LoadCostCenterInContainer();
                        ResetForm();
                        ComboUtils.InitializeCompanyCombo(ComboBoxCostCenterCompany, CompanyManager.GetAccessibleCompanies(Global.User));
                        LoadCostCentersWithFilter();
                        EnableForm(false);
                        if (TreeViewCostCenter.Nodes.Count > 0)
                        { TextBoxCostCenterSearch.Select(); }
                        else { BtnCostCenterNew.Select(); }
                        this.formIsDirty = false;
                    }
                    else
                    {
                        ToolStripStatusLabelErrorCostCenter.Text = NotAllowDeleteErrorText;
                    }
                    WaitCursor(Cursors.Default);
                }
            }
            else
            {
                DisplaySystemError("Somting went wrong, please check this costcenter is still valid.");
                return;
            }
        }
        private void BtnCostcenterEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxCostcenterId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this costcenter is still valid.");
                return;
            }
            WaitCursor(Cursors.WaitCursor);
            LoadCostCenterInfo();
            EnableForm(true);
            TextBoxCostCenterName.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void BtnCostcenterCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TextBoxCostCenterName.Select();
                    return;
                }
            }
            WaitCursor(Cursors.WaitCursor);
            ResetForm();
            ComboUtils.InitializeCompanyCombo(ComboBoxCostCenterCompany, CompanyManager.GetAccessibleCompanies(Global.User));
            LoadCostCenterInfo();
            EnableForm(false);
            if(TreeViewCostCenter.Nodes.Count>0)
            { TextBoxCostCenterSearch.Select(); }else { BtnCostCenterNew.Select(); }
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void BtnCostcenterSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                WaitCursor(Cursors.WaitCursor);
                CostCenter lCostCenter = GetCostCenterFromForm();
                if (CostcenterManager.CostCenterNameUniqueById(lCostCenter))
                {
                    CostCenter CostCenterFromDB = null;
                    if (lCostCenter.CostCenterId == 0)
                    {                        
                            CostCenterFromDB = CostcenterManager.AddCostCenter(lCostCenter);                     
                    }
                    else
                    {
                        CostCenter lCostCenterById = CostcenterManager.GetCostCenterById(lCostCenter.CostCenterId);
                        if (lCostCenterById != null)
                        {                          
                            CostCenterFromDB = CostcenterManager.UpdateCostCenter(lCostCenter);
                            if (CostCenterFromDB.CostCenterId == Global.CostCenter.CostCenterId)
                            {
                                Global.CostCenter = null;
                            }                           
                        }
                        else
                        {
                            DisplaySystemError("Somting went wrong, please check this costcenter is still valid.");
                            return;
                        }
                    }
                    LoadCostCenterInContainer();
                    ResetForm();
                    LoadCostCentersWithFilter();
                    TreeNode TreeNode = new TreeNode();
                    TreeNode = TreeViewCostCenter.Nodes[CostCenterFromDB.ParentCompanyId.ToString()].Nodes[CostCenterFromDB.CostCenterId.ToString()];
                    TreeViewCostCenter.SelectedNode = TreeNode;
                    TreeViewCostCenter.Focus();
                    EnableForm(false);
                    this.formIsDirty = false;
                }
                else
                {
                    ToolStripStatusLabelErrorCostCenter.Text = string.Format(UniqueCostCenterNameErrorMsg, lCostCenter.Name, ComboBoxCostCenterCompany.SelectedItem); 
                    TextBoxCostCenterName.Select();
                }
                    WaitCursor(Cursors.Default);
            }
        }

        private void BtnCurrencyExit_Click(object sender, EventArgs e)
        {
            WaitCursor(Cursors.WaitCursor);
            this.Close();
            Cursor.Current = Cursors.Default;
        }
        private Boolean ValidateForm()
        {
            ToolStripStatusLabelErrorCostCenter.Text = "";
            if (string.IsNullOrEmpty(TextBoxCostCenterName.Text.Trim()))
            {
                ToolStripStatusLabelErrorCostCenter.Text = EnterCostCenterNameErrorMsg;
                TextBoxCostCenterName.Select();
                return false;
            }
            
            if (ComboBoxCostCenterCompany.SelectedIndex < 0)
            {
                ToolStripStatusLabelErrorCostCenter.Text =ChooseParentErrorMsg;
                ComboBoxCostCenterCompany.Select();
                return false;

            }
            if (ComboBoxCostCenterCompany.SelectedIndex >= 0 && CompanyManager.GetCompany(((Company)ComboBoxCostCenterCompany.Items[ComboBoxCostCenterCompany.SelectedIndex]).CompanyId) == null)
            {
                ToolStripStatusLabelErrorCostCenter.Text = "Somting went wrong, please check this parent company is still valid.";
                ComboBoxCostCenterCompany.Select();
                return false;

            }
            return true;
        }
        private void ResetForm()
        {
            ToolStripStatusLabelErrorCostCenter.Text = "";
            TextBoxCostCenterParentCompany.ResetText();
            TextBoxCostcenterId.ResetText();
            TextBoxCostCenterDescription.ResetText();
            TextBoxCostCenterDisplayas.ResetText();
            TextBoxCostCenterName.ResetText();
            TextBoxCostcenterId.ResetText();
            ComboBoxCostCenterCompany.SelectedIndex = -1;
        }
        private void EnableForm(Boolean enable)
        {
            CostCenter CostCenterInfo = GetCostCenterInfo();
            if (TreeViewCostCenter.Nodes.Count > 0)
            {
                TreeViewCostCenter.Enabled = !enable;
                TextBoxCostCenterSearch.ReadOnly = enable;
                TextBoxCostCenterSearch.TabStop = !enable;
            }
            else
            {
                TreeViewCostCenter.Enabled = false;
                TextBoxCostCenterSearch.ReadOnly = true;
                TextBoxCostCenterSearch.TabStop = false;
                BtnCostCenterNew.Select();
            }
            TextBoxCostCenterParentCompany.Visible = TextBoxCostCenterParentCompany.ReadOnly = !enable;
            TextBoxCostCenterName.ReadOnly = !enable;
            TextBoxCostCenterDisplayas.ReadOnly = !enable;
            TextBoxCostCenterDescription.ReadOnly = !enable;

            TextBoxCostCenterParentCompany.TabStop= enable;
            TextBoxCostCenterName.TabStop = enable;
            TextBoxCostCenterDisplayas.TabStop = enable;
            TextBoxCostCenterDescription.TabStop = enable;

            if (ComboBoxCostCenterCompany.Items.Count > 0)
            {
                ComboBoxCostCenterCompany.Enabled = enable;
            }
            if(!enable)
            {
                BtnCostCenterCancel.Enabled = enable;
                if (TreeViewCostCenter.SelectedNode == null)
                {
                    BtnCostCenterDelete.Enabled = enable;
                    BtnCostCenterEdit.Enabled = enable;
                }
                else
                {
                    BtnCostCenterDelete.Enabled = !enable;
                    BtnCostCenterEdit.Enabled = !enable;
                }
                BtnCostCenterNew.Enabled = !enable;
                BtnCostCenterSave.Enabled = enable;
            }
            else
            {
                BtnCostCenterCancel.Enabled = enable;
                BtnCostCenterDelete.Enabled = !enable;
                BtnCostCenterEdit.Enabled = !enable;
                BtnCostCenterNew.Enabled = !enable;
                BtnCostCenterSave.Enabled = enable;
            }
        }
        private void TextBoxCostCenterSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            Keypress_Validation.Keypress_NameChecking(sender, e);
        }
        private void TextBoxCostcenterName_KeyPress(object sender, KeyPressEventArgs e)
        {
           Keypress_Validation.Keypress_NameChecking(sender, e);
        }

        private void TextBoxCostcenterDisplayas_KeyPress(object sender, KeyPressEventArgs e)
        {
            Keypress_Validation.Keypress_NameChecking(sender, e);
        }

       
        private void TextBoxCostcenterSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                TreeViewCostCenter.Select();
            }
        }

        private void ComboBoxCostCenterCompany_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxCostCenterCompany.DroppedDown = false;
        }

        private void TextBoxCostCenterName_KeyDown(object sender, KeyEventArgs e)
        {
            Keypress_Validation.Keypress_PasteChecking(sender, e, "NameChecking");

        }

        private void TextBoxCostCenterName_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Keypress_Validation.AddContextMenu(TextBoxCostCenterName, "NameChecking");
            }
        }

        private void TextBoxCostCenterDisplayas_KeyDown(object sender, KeyEventArgs e)
        {
            Keypress_Validation.Keypress_PasteChecking(sender, e, "NameChecking");

        }

        private void TextBoxCostCenterDisplayas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Keypress_Validation.AddContextMenu(TextBoxCostCenterDisplayas, "NameChecking");
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnCostCenterNew.PerformClick();
            }
            else if (keyData == (Keys.F4))
            {
                BtnCostCenterDelete.PerformClick();
            }
            else if (keyData == (Keys.F7))
            {
                BtnCostCenterEdit.PerformClick();
            }
            else if (keyData == (Keys.F8))
            {
                BtnCostCenterSave.PerformClick();
            }
            else if (keyData == (Keys.F10))
            {
                BtnCostCenterExit.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCostCenterCancel.PerformClick();
            }          
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void FormCostCenter_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning,MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TextBoxCostCenterName.Select();
                    e.Cancel = true;
                }
            }
        }

        private void BtnCostCenterSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxCostCenterName.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxCostCenterCompany.Focus();
            }
        }

        private void TextBoxCostCenterName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxCostCenterDisplayas.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnCostCenterSave.Focus();
            }
        }
    }
}
