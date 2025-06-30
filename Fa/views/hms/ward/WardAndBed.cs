using fa.libraries.utils;
using fa.libraries.Validation;
using fa.api.Hms;
using fa.model.Hms.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using fa.model.OrderManagement;
using System.Reactive;

namespace fa.views.hms.ward
{
    public partial class FormWardAndBed : FormBase
    {
        public static string DoNotAllowToSaveWardNamExisteMsg = "Ward '{0}' already exists";
        public static string DoNotAllowToSaveBedNamExisteMsg = "Bed '{0}' already exists";
        public static string ErrorDeleteWardMsg = "Error deleting the ward!, please retry";
        public static string DoNotAllowToDeleteWardMsg = "Ward '{0}' is occupied and cannot be deleted";
        public static string ErrorDeleteBedMsg = "Error deleting the bed!, please retry";
        public static string DoNotAllowToDeleteBedMsg = "Bed '{0}' is occupied and cannot be deleted";
        public static string EnterNameErrorMsg = "Name could not be empty, please enter Name";
        public static string EnterBedErrorMsg = "Please enter the bed '{0}'";
        public static string EnterBedNameRepeatErrorMsg = "Duplicate bed name '{0}'";
        public static string SaveSuccessMsg = "Saved successfully";
        public static string DeleteSuccessMsg = "Deleted successfully";
        public static string SelectBedTypeErrorMsg = "Please select the bed type";
        public static string SelectWardErrorMsg = "Please select the ward";
        public static string DoNotAllowToDeleteBedWithoutNameMsg = "Bed '{0}' is occupied and cannot be deleted";
        BedManager BedManager = null;
        WardManager WardManager = null;
        HospitalInventoryManager HospitalInventoryManager = null;
        KeypressValidation KeypressValidation = null;
        public FormWardAndBed()
        {
            BedManager = BedManager.Instance;
            WardManager = WardManager.Instance;
            HospitalInventoryManager = HospitalInventoryManager.Instance;
            KeypressValidation = KeypressValidation.Instance;
            InitializeComponent();
        }
        private void FormWardAndBed_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                EnableForm(true);
                LoadCombo();
                LoadWardsWithFilter();
                GridViewBedInfo.RowTemplate.Height = 20;
                this.formIsDirty = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void LoadWardsWithFilter()
        {
            string FilterString = TextBoxWardSearch.Text.Trim();
            TreeViewWardBed.Nodes.Clear();
            IList<Ward> Ward = WardManager.ListWardByCompanyIdFilter(Global.Company.CompanyId, FilterString);
            IList<Bed> Bed = WardManager.ListAllWardBedByCompanyIdFilter(Global.Company.CompanyId, FilterString);
            if (Ward != null)
            {
                foreach (var lWard in Ward)
                {
                    TreeNode[] treeNodes = TreeViewWardBed.Nodes.Cast<TreeNode>().Where(r => r.Text == lWard.Name).ToArray();
                    if (treeNodes.Length == 0)
                    {
                        TreeViewWardBed.Nodes.Add(lWard.Id.ToString(), lWard.Name);
                        TreeViewWardBed.Nodes[lWard.Id.ToString()].ImageIndex = 0;
                        TreeViewWardBed.Nodes[lWard.Id.ToString()].ContextMenuStrip = ContextMenuStripWardBed;
                        if (Bed != null && Bed.Where(x => x.WardId == lWard.Id).ToList().Count > 0)
                        {
                            foreach (var lBed in Bed.Where(x => x.WardId == lWard.Id).ToList())
                            {
                                TreeViewWardBed.Nodes[lWard.Id.ToString()].Nodes.Add(lBed.Id.ToString(), lBed.Name);
                                TreeViewWardBed.Nodes[lWard.Id.ToString()].Nodes[lBed.Id.ToString()].ImageIndex = 1;
                            }
                        }
                    }
                }
            }
            else
            {
                GridViewBedInfo.AllowUserToAddRows = true;
                TextBoxWardName.Select();
            }
            if (TreeViewWardBed.Nodes.Count > 0)
            {
                if (!string.IsNullOrEmpty(TextBoxWardSearch.Text))
                {
                    TreeViewWardBed.ExpandAll();
                }
                TreeViewWardBed.SelectedNode = TreeViewWardBed.Nodes[0];
            }
        }
        private Bed GetWardBedInfo()
        {
            Bed BedFromDB = WardManager.GetWardBedById(long.Parse(TreeViewWardBed.SelectedNode.Name));
            if (BedFromDB != null)
            {
                return BedFromDB;
            }
            return null;
        }
        private Ward GetWardInfo()
        {
            Ward lWard = WardManager.GetWardById(long.Parse(TreeViewWardBed.SelectedNode.Name));
            if (lWard != null)
            {
                Ward WardFromDB = WardManager.GetWardById(lWard.Id);
                return WardFromDB;
            }
            return null;
        }
        private void LoadWardBedInfo()
        {
            if (TreeViewWardBed.SelectedNode != null)
            {
                if (TreeViewWardBed.SelectedNode.Parent == null)
                {
                    TabControlBed.Visible = false;
                    TabControlWard.Visible = true;
                    Ward WardFromDB = GetWardInfo();
                    if (WardFromDB != null)
                    {
                        GridViewBedInfo.Rows.Clear();
                        TextBoxWardBedId.Text = WardFromDB.Id.ToString();
                        TextBoxWardName.Text = WardFromDB.Name;
                        InventoryLocationcheckBox.Checked = WardFromDB.MaintainInventory;
                        if (WardFromDB.Beds.Count > 0)
                        {
                            GridViewBedInfo.Rows.Add(WardFromDB.Beds.Count);
                            int i = 0;
                            foreach (Bed Bed in WardFromDB.Beds)
                            {
                                IList<BedType> BedType = BedManager.ListBedTypeByCompanyId(Global.Company.CompanyId);
                                if (BedType != null)
                                {
                                    (GridViewBedInfo.Rows[i].Cells[1] as DataGridViewComboBoxCell).DataSource = null;
                                    (GridViewBedInfo.Rows[i].Cells[1] as DataGridViewComboBoxCell).DataSource = BedType;
                                    (GridViewBedInfo.Rows[i].Cells[1] as DataGridViewComboBoxCell).ValueMember = "Id";
                                    (GridViewBedInfo.Rows[i].Cells[1] as DataGridViewComboBoxCell).DisplayMember = "Name";
                                    GridViewBedInfo.Rows[i].Cells[2].Value = "X";
                                }
                                GridViewBedInfo.Rows[i].Cells[0].Value = Bed.Name;
                                GridViewBedInfo.Rows[i].Cells[1].Value = Bed.BedTypeId;
                                GridViewBedInfo.Rows[i].Cells[3].Value = Bed.Id;
                                i++;
                            }
                        }
                    }
                }
                else
                {
                    TabControlBed.Visible = true;
                    TabControlWard.Visible = false;
                    Bed WardBedFromDB = GetWardBedInfo();
                    if (WardBedFromDB != null)
                    {
                        TextBoxWardBedId.Text = WardBedFromDB.Id.ToString();
                        TextBoxBedName.Text = WardBedFromDB.Name;
                        if (WardBedFromDB.BedType != null)
                        {
                            ComboBoxSwapTextBoxBedType.SelectedIndex = ComboBoxSwapTextBoxBedType.FindStringExact(WardBedFromDB.BedType.Name);
                            ComboBoxSwapTextBoxBedType.Text = WardBedFromDB.BedType.Name;
                        }
                        if (WardBedFromDB.Ward != null)
                        {
                            ComboBoxSwapTextBoxWard.SelectedIndex = ComboBoxSwapTextBoxWard.FindStringExact(WardBedFromDB.Ward.Name);
                            ComboBoxSwapTextBoxWard.Text = WardBedFromDB.Ward.Name;
                        }
                    }
                }
            }
        }
        private Ward GetWardFromForm()
        {
            Ward lWard = new Ward();
            if (!string.IsNullOrEmpty(TextBoxWardBedId.Text))
            {
                lWard.Id = Convert.ToInt64(TextBoxWardBedId.Text);
            }
            else
            {
                lWard.Id = 0L;
            }
            lWard.Name = TextBoxWardName.Text.Trim();
            lWard.CompanyId = Global.Company.CompanyId;
            lWard.MaintainInventory = InventoryLocationcheckBox.Checked;
            if (InventoryLocationcheckBox.Checked)
            {
                string locationName = TextBoxWardName.Text.Trim();
                InventoryLocation llocation = new InventoryLocation();
                InventoryLocation lllocation = null;
                if (lWard.Id == 0L)
                {
                    lllocation = HospitalInventoryManager.GetLocationByName(locationName, Global.Company.CompanyId);
                    if (lllocation == null)
                    {
                        llocation.Name = locationName;
                    }
                    else
                    {
                        string NewLocation = locationName;
                        for (int i = 0; i < 500; i++)
                        {
                            NewLocation += i.ToString();
                            lllocation = HospitalInventoryManager.GetLocationByName(NewLocation, Global.Company.CompanyId);
                            if (lllocation == null)
                            {
                                llocation.Name = NewLocation;
                                break;
                            }
                        }
                    }
                    llocation.CompanyId = Global.Company.CompanyId;
                    if (Global.CostCenter != null) { llocation.CostCenterId = Global.CostCenter.CostCenterId; }
                    llocation.Type = InventoryLocationType.WARD;
                    lWard.InventoryLocation = llocation;
                }
                else
                {
                    Ward OldWard = WardManager.GetWardById(lWard.Id);
                    if (OldWard != null)
                    {
                        if (OldWard.InventoryLocationId != null)
                        {
                            InventoryLocation OldInventoryLocation = HospitalInventoryManager.GetLocationById((long)OldWard.InventoryLocationId);
                            if (OldInventoryLocation != null)
                            {
                                lWard.InventoryLocationId = OldInventoryLocation.Id;
                            }
                        }
                        else
                        {
                            lllocation = HospitalInventoryManager.GetLocationByName(locationName, Global.Company.CompanyId);
                            if (lllocation == null)
                            {
                                llocation.Name = locationName;
                                llocation.CompanyId = Global.Company.CompanyId;
                                if (Global.CostCenter != null) { llocation.CostCenterId = Global.CostCenter.CostCenterId; }
                                llocation.Type = InventoryLocationType.WARD;
                                lWard.InventoryLocation = llocation;
                            }
                            else
                            {
                                OldWard = WardManager.GetWardByLocationId(lllocation.Id);
                                if (OldWard == null)
                                {
                                    lWard.InventoryLocationId = lllocation.Id;
                                }
                                else
                                {
                                    llocation.Name = locationName;
                                    llocation.CompanyId = Global.Company.CompanyId;
                                    if (Global.CostCenter != null) { llocation.CostCenterId = Global.CostCenter.CostCenterId; }
                                    llocation.Type = InventoryLocationType.WARD;
                                    lWard.InventoryLocation = llocation;
                                }
                            }
                        }
                    }
                }
                if (lWard.InventoryLocationId == null)
                {
                    if (lllocation == null)
                    {
                        llocation.Name = locationName;
                    }
                    else
                    {
                        string NewLocation = locationName;
                        for (int i = 0; i < 500; i++)
                        {
                            NewLocation += i.ToString();
                            lllocation = HospitalInventoryManager.GetLocationByName(NewLocation, Global.Company.CompanyId);
                            if (lllocation == null)
                            {
                                llocation.Name = NewLocation;
                                break;
                            }
                        }
                    }
                    llocation.CompanyId = Global.Company.CompanyId;
                    if (Global.CostCenter != null) { llocation.CostCenterId = Global.CostCenter.CostCenterId; }
                    llocation.Type = InventoryLocationType.WARD;
                    lWard.InventoryLocation = llocation;
                }
            }
            if (GridViewBedInfo.Rows.Count > 0)
            {
                lWard.Beds = new List<Bed>() { };
                for (int i = 0; i < GridViewBedInfo.Rows.Count - 1; i++)
                {
                    Bed Bed = new Bed();
                    Bed.Id = (GridViewBedInfo.Rows[i].Cells[3].Value != null) ? (long)GridViewBedInfo.Rows[i].Cells[3].Value : 0L;
                    Bed.Name = GridViewBedInfo.Rows[i].Cells[0].Value.ToString();
                    Bed.BedTypeId = (long)GridViewBedInfo.Rows[i].Cells[1].Value;
                    Bed.CompanyId = Global.Company.CompanyId;
                    Bed.BedStatus = BedStatus.AVAILABLE;
                    lWard.Beds.Add(Bed);
                }
            }
            return lWard;
        }
        private Bed GetWardBedFromForm()
        {
            Bed lBed = new Bed();
            if (!string.IsNullOrEmpty(TextBoxWardBedId.Text))
            {
                lBed.Id = Convert.ToInt64(TextBoxWardBedId.Text);
            }
            else
            {
                lBed.Id = 0L;
            }
            lBed.Name = TextBoxBedName.Text.Trim();
            lBed.BedStatus = BedStatus.AVAILABLE;
            lBed.CompanyId = Global.Company.CompanyId;
            Ward Ward = (Ward)ComboBoxSwapTextBoxWard.Items[ComboBoxSwapTextBoxWard.SelectedIndex];
            if (Ward != null)
            {
                Ward = WardManager.GetWardById(Ward.Id);
                if (Ward != null)
                {
                    lBed.WardId = Ward.Id;
                }
            }
            BedType BedType = (BedType)ComboBoxSwapTextBoxBedType.Items[ComboBoxSwapTextBoxBedType.SelectedIndex];
            if (BedType != null)
            {
                BedType = BedManager.GetBedTypeById(BedType.Id);
                if (BedType != null)
                {
                    lBed.BedTypeId = BedType.Id;
                }
            }
            return lBed;
        }
        private void TextBoxWardSearch_TextChanged(object sender, EventArgs e)
        {
            ResetForm();
            LoadWardsWithFilter();
            if (TreeViewWardBed.Nodes.Count > 0) { BtnWardEdit.Enabled = true; BtnWardDelete.Enabled = true; }
            else { BtnWardEdit.Enabled = false; BtnWardDelete.Enabled = false; }
        }
        private void TreeViewWard_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            node.SelectedImageIndex = node.ImageIndex;
            ResetForm();
            LoadCombo();
            LoadWardBedInfo();
            EnableForm(false);
            this.formIsDirty = false;
        }
        private void BtnWardDelete_Click(object sender, EventArgs e)
        {
            if (TabControlWard.Visible == true)
            {
                Ward WardInfo = GetWardInfo();
                WardErrorMsg.Text = "";
                if (WardInfo != null)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete the Ward " + WardInfo.Name + "?", "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        if (WardManager.DeleteWard(WardInfo.Id))
                        {
                            ResetForm();
                            LoadWardsWithFilter();
                            EnableForm(false);
                            WardErrorMsg.Text = DeleteSuccessMsg;
                        }
                        else
                        {
                            WardErrorMsg.Text = string.Format(DoNotAllowToDeleteWardMsg, WardInfo.Name);
                        }
                        this.formIsDirty = false;
                    }
                }
                else
                {
                    WardErrorMsg.Text = ErrorDeleteWardMsg;
                }
            }
            else if (TabControlBed.Visible == true)
            {
                Bed WardBedInfo = GetWardBedInfo();
                WardErrorMsg.Text = "";
                if (WardBedInfo != null)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete the Bed " + WardBedInfo.Name + "?", "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        if (WardManager.DeleteWardBed(WardBedInfo.Id))
                        {
                            ResetForm();
                            LoadWardsWithFilter();
                            EnableForm(false);
                            WardErrorMsg.Text = DeleteSuccessMsg;
                        }
                        else
                        {
                            WardErrorMsg.Text = string.Format(DoNotAllowToDeleteBedMsg, WardBedInfo.Name);
                        }
                        this.formIsDirty = false;
                    }
                }
                else
                {
                    WardErrorMsg.Text = ErrorDeleteBedMsg;
                }
            }
        }
        private void BtnWardEdit_Click(object sender, EventArgs e)
        {
            WardErrorMsg.Text = string.Empty;
            LoadWardBedInfo();
            EnableForm(true);
            if (TabControlWard.Visible)
            { GridViewBedInfo.AllowUserToAddRows = true; TextBoxWardName.Select(); }
            else
            { TextBoxBedName.Select(); }
            this.formIsDirty = false;
        }
        private void BtnWardCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show("There are unsaved changes, Do you want cancel?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.No)
                {
                    if (TabControlWard.Visible)
                    {
                        TextBoxWardName.Select();
                    }
                    else
                    {
                        TextBoxBedName.Select();
                    }
                    return;
                }
            }
            ResetForm();
            LoadCombo();
            if (string.IsNullOrEmpty(TextBoxWardSearch.Text))
            {
                LoadWardBedInfo();
            }
            else
            {
                TextBoxWardSearch.Clear();
            }
            EnableForm(false);
            if (TreeViewWardBed.Nodes.Count > 0) { TextBoxWardSearch.Select(); }
            else { BtnWardBedNew.Select(); }
            this.formIsDirty = false;
        }
        private void BtnWardSave_Click(object sender, EventArgs e)
        {
            if (TabControlWard.Visible && ValidateWardForm())
            {
                Ward lWard = GetWardFromForm();
                if (WardManager.WardNameUniqueById(lWard))
                {
                    Ward WardFromDB = null;
                    if (lWard.Id == 0)
                    {
                        WardFromDB = WardManager.AddWard(lWard);
                        WardErrorMsg.Text = SaveSuccessMsg;
                    }
                    else
                    {
                        Ward lWardById = WardManager.GetWardById(lWard.Id);
                        if (lWardById != null)
                        {
                            WardFromDB = WardManager.UpdateWard(lWard);
                            WardErrorMsg.Text = SaveSuccessMsg;
                        }
                    }
                    ResetForm();
                    if (string.IsNullOrEmpty(TextBoxWardSearch.Text))
                    {
                        LoadWardsWithFilter();
                    }
                    else
                    {
                        TextBoxWardSearch.Clear();
                    }
                    TextBoxWardBedId.Text = WardFromDB.Id.ToString();
                    TreeNode TreeNode = new TreeNode();
                    TreeNode = TreeViewWardBed.Nodes[WardFromDB.Id.ToString()];
                    TreeViewWardBed.SelectedNode = TreeNode;
                    TreeViewWardBed.Focus();
                    WardErrorMsg.Text = SaveSuccessMsg;
                    this.formIsDirty = false;
                }
                else
                {
                    WardErrorMsg.Text = string.Format(DoNotAllowToSaveWardNamExisteMsg, lWard.Name);
                    TextBoxWardName.Select();
                    return;
                }
            }
            else if (TabControlBed.Visible && ValidateWardBedForm())
            {
                Bed lWardBed = GetWardBedFromForm();
                if (WardManager.BedNameUniqueById(lWardBed))
                {
                    Bed WardBedFromDB = null;
                    if (lWardBed.Id == 0)
                    {
                        WardBedFromDB = WardManager.AddWardBed(lWardBed);
                        WardErrorMsg.Text = SaveSuccessMsg;
                    }
                    else
                    {
                        Bed lWardById = WardManager.GetWardBedById(lWardBed.Id);
                        if (lWardById != null)
                        {
                            WardBedFromDB = WardManager.UpdateWardBed(lWardBed);
                            WardErrorMsg.Text = SaveSuccessMsg;
                        }
                    }
                    ResetForm();
                    if (string.IsNullOrEmpty(TextBoxWardSearch.Text))
                    {
                        LoadWardsWithFilter();
                    }
                    else
                    {
                        TextBoxWardSearch.Clear();
                    }
                    TextBoxWardBedId.Text = WardBedFromDB.Id.ToString();
                    TreeNode TreeNode = new TreeNode();
                    TreeNode = TreeViewWardBed.Nodes[WardBedFromDB.WardId.ToString()].Nodes[WardBedFromDB.Id.ToString()];
                    TreeViewWardBed.SelectedNode = TreeNode;
                    TreeViewWardBed.Focus();
                    WardErrorMsg.Text = SaveSuccessMsg;
                    this.formIsDirty = false;
                }
                else
                {
                    WardErrorMsg.Text = string.Format(DoNotAllowToSaveBedNamExisteMsg, lWardBed.Name);
                    TextBoxBedName.Select();
                    return;
                }
            }
        }
        private Boolean ValidateWardForm()
        {
            WardErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxWardName.Text.Trim()))
            {
                WardErrorMsg.Text = EnterNameErrorMsg;
                TextBoxWardName.Select();
                return false;
            }
            if (GridViewBedInfo.Rows.Count > 1)
            {
                for (int i = 0; i < GridViewBedInfo.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (GridViewBedInfo.Rows[i].Cells[j].Value == null || GridViewBedInfo.Rows[i].Cells[j].Value == string.Empty || string.IsNullOrWhiteSpace(GridViewBedInfo.Rows[i].Cells[j].Value.ToString()))
                        {
                            WardErrorMsg.Text = string.Format(EnterBedErrorMsg, GridViewBedInfo.Columns[j].HeaderText);
                            GridViewBedInfo.Select();
                            GridViewBedInfo.CurrentCell = GridViewBedInfo[j, i];
                            return false;
                        }
                    }
                    int BedNameCount = 0;
                    for (int k = 0; k < GridViewBedInfo.Rows.Count - 1; k++)
                    {
                        if (GridViewBedInfo.Rows[k].Cells[0].Value != null)
                        {
                            if (GridViewBedInfo.Rows[i].Cells[0].Value.ToString() == GridViewBedInfo.Rows[k].Cells[0].Value.ToString())
                            {
                                BedNameCount++;
                            }
                        }
                        if (BedNameCount > 1)
                        {
                            WardErrorMsg.Text = string.Format(EnterBedNameRepeatErrorMsg, GridViewBedInfo.Rows[k].Cells[0].Value.ToString());
                            GridViewBedInfo.Select();
                            GridViewBedInfo.CurrentCell = GridViewBedInfo[0, k];
                            return false;
                        }
                    }
                }
            }
            return true;
        }


        private Boolean ValidateWardBedForm()
        {
            WardErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxBedName.Text.Trim()))
            {
                WardErrorMsg.Text = EnterNameErrorMsg;
                TextBoxBedName.Select();
                return false;
            }
            if (ComboBoxSwapTextBoxBedType.SelectedIndex < 0)
            {
                WardErrorMsg.Text = SelectBedTypeErrorMsg;
                ComboBoxSwapTextBoxBedType.Select();
                return false;
            }
            if (ComboBoxSwapTextBoxWard.SelectedIndex < 0)
            {
                WardErrorMsg.Text = SelectWardErrorMsg;
                ComboBoxSwapTextBoxWard.Select();
                return false;
            }
            return true;
        }
        private void BtnWardExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public FormBedType FormBedType = null;
        private void linkLabelWardBedType_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (FormBedType == null || FormBedType.IsDisposed)
            {
                FormBedType = new FormBedType(this);
            }
            FormBedType.CreateOpBedTypeOnLoad = true;
            FormBedType.ShowDialog();
            RefreshBedGrid();
        }
        private void RefreshBedGrid()
        {
            for (int i = 0; i < GridViewBedInfo.Rows.Count; i++)
            {
                var OldValue = GridViewBedInfo.Rows[i].Cells[1].Value;
                IList<BedType> BedType = BedManager.ListBedTypeByCompanyId(Global.Company.CompanyId);
                if (BedType != null)
                {
                    (GridViewBedInfo.Rows[i].Cells[1] as DataGridViewComboBoxCell).DataSource = null;
                    (GridViewBedInfo.Rows[i].Cells[1] as DataGridViewComboBoxCell).DataSource = BedType;
                    (GridViewBedInfo.Rows[i].Cells[1] as DataGridViewComboBoxCell).ValueMember = "Id";
                    (GridViewBedInfo.Rows[i].Cells[1] as DataGridViewComboBoxCell).DisplayMember = "Name";
                }
                GridViewBedInfo.Rows[i].Cells[1].Value = OldValue;
            }
        }
        private void ResetForm()
        {
            InventoryLocationcheckBox.Checked = false;
            TextBoxWardBedId.ResetText();
            TextBoxWardName.ResetText();
            TextBoxBedName.ResetText();
            ComboBoxSwapTextBoxBedType.SelectedIndex = -1;
            ComboBoxSwapTextBoxWard.SelectedIndex = -1;
            GridViewBedInfo.Rows.Clear();
            GridViewBedInfo.RowTemplate.Height = 20;
            WardErrorMsg.Text = string.Empty;
        }
        private void EnableForm(Boolean enable)
        {
            if (TreeViewWardBed.Nodes.Count > 0)
            {
                TreeViewWardBed.Enabled = !enable;
                TextBoxWardSearch.ReadOnly = enable;
                TextBoxWardSearch.TabStop = !enable;
                GridViewBedInfo.AllowUserToAddRows = false;
                GridViewBedInfo.RowTemplate.Height = 20;

            }
            else
            {
                TreeViewWardBed.Enabled = false;
                TextBoxWardSearch.ReadOnly = true;
                TextBoxWardSearch.TabStop = false;
                TabControlWard.Visible = true;
                TabControlBed.Visible = false;
                GridViewBedInfo.AllowUserToAddRows = true;
                GridViewBedInfo.RowTemplate.Height = 20;

            }
            GridViewBedInfo.Enabled = enable;
            GridViewBedInfo.RowTemplate.Height = 20;
            linkLabelWardBedType.Enabled = enable;
            linkLabelWardBedType.TabStop = false;
            TextBoxWardName.ReadOnly = !enable;
            TextBoxWardName.TabStop = enable;
            TextBoxBedName.ReadOnly = !enable;
            TextBoxBedName.TabStop = enable;
            ComboBoxSwapTextBoxBedType.Visible = enable;
            ComboBoxSwapTextBoxWard.Visible = enable;
            InventoryLocationcheckBox.Enabled = enable;
            if (!string.IsNullOrEmpty(TextBoxWardBedId.Text) && TabControlBed.Visible && enable)
            {
                ComboBoxSwapTextBoxWard.Visible = !IpManager.Instance.GetInPatientLocationbyBedId(long.Parse(TextBoxWardBedId.Text));
            }
            if (!enable)
            {
                BtnWardCancel.Enabled = enable;
                if (TreeViewWardBed.SelectedNode == null)
                {
                    BtnWardDelete.Enabled = enable;
                    BtnWardEdit.Enabled = enable;

                }
                else
                {
                    BtnWardDelete.Enabled = !enable;
                    BtnWardEdit.Enabled = !enable;

                }
                BtnWardBedNew.Enabled = !enable;
                BtnWardSave.Enabled = enable;

            }
            else
            {
                BtnWardCancel.Enabled = enable;
                BtnWardDelete.Enabled = !enable;
                BtnWardEdit.Enabled = !enable;
                BtnWardBedNew.Enabled = !enable;
                BtnWardSave.Enabled = enable;

            }
        }

        private void GridViewBedInfo_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewBedInfo.Rows[e.RowIndex].Cells[1].ReadOnly = true;
            GridViewBedInfo.Rows[e.RowIndex].Cells[2].ReadOnly = true;

            if (GridViewBedInfo.Rows[e.RowIndex].Cells[0].Value != null)
            {
                GridViewBedInfo.Rows[e.RowIndex].Cells[1].ReadOnly = false;
                GridViewBedInfo.Rows[e.RowIndex].Cells[2].ReadOnly = false;
            }
        }

        private void GridViewBedInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                WardErrorMsg.Text = "";
                object cellValue = GridViewBedInfo.Rows[e.RowIndex].Cells[0].Value;
                if (e.ColumnIndex == 2 && (GridViewBedInfo.Rows.Count - 1) != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show($"Do you want to delete {cellValue}?", "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        if (GridViewBedInfo.Rows[e.RowIndex].Cells[3].Value != null)
                        {
                            if (IpManager.Instance.GetInPatientLocationbyBedId((long)GridViewBedInfo.Rows[e.RowIndex].Cells[3].Value))
                            {
                                WardErrorMsg.Text = string.Format(DoNotAllowToDeleteBedWithoutNameMsg, cellValue);
                                return;
                            }
                        }
                        GridViewBedInfo.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewBedInfo.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
        }

        private void GridViewBedInfo_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            IList<BedType> BedType = BedManager.ListBedTypeByCompanyId(Global.Company.CompanyId);
            if (BedType != null)
            {
                (GridViewBedInfo.Rows[e.RowIndex].Cells[1] as DataGridViewComboBoxCell).DataSource = null;
                (GridViewBedInfo.Rows[e.RowIndex].Cells[1] as DataGridViewComboBoxCell).DataSource = BedType;
                (GridViewBedInfo.Rows[e.RowIndex].Cells[1] as DataGridViewComboBoxCell).ValueMember = "Id";
                (GridViewBedInfo.Rows[e.RowIndex].Cells[1] as DataGridViewComboBoxCell).DisplayMember = "Name";
                GridViewBedInfo.Rows[e.RowIndex].Cells[2].Value = "X";
            }
        }

        private void GridViewBedInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void GridViewBedInfo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (GridViewBedInfo.Rows.Count > 0)
            {
                GridViewBedInfo.CommitEdit(DataGridViewDataErrorContexts.Commit);
                if (GridViewBedInfo.CurrentCell.ColumnIndex == 0)
                {
                    KeypressValidation.Keypress_NameChecking(sender, e);
                }

            }
        }

        private void GridViewBedInfo_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                ((ComboBox)e.Control).MaxLength = 30;
                if (GridViewBedInfo.CurrentCell.Value == null)
                {
                    ((ComboBox)e.Control).SelectedIndex = -1;
                }
                e.Control.KeyPress += new KeyPressEventHandler(DataGridViewBed_KeyPress1);
            }
            if (GridViewBedInfo.CurrentCell.ColumnIndex == 0 || GridViewBedInfo.CurrentCell.ColumnIndex == 1)
            {
                e.Control.KeyPress += new KeyPressEventHandler(GridViewBedInfo_KeyPress);
            }
        }
        private void DataGridViewBed_KeyPress1(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)GridViewBedInfo.EditingControl).DroppedDown = false;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnWardBedNew.ShowDropDown();
            }
            if (keyData == (Keys.F4))
            {
                BtnWardDelete.PerformClick();
            }
            if (keyData == (Keys.F7))
            {
                BtnWardEdit.PerformClick();

            }
            if (keyData == (Keys.F8))
            {
                BtnWardSave.PerformClick();
            }
            if (keyData == (Keys.F10))
            {
                BtnWardExit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnWardCancel.PerformClick();
                return true;

            }
            try
            {
                if (GridViewBedInfo.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab) && GridViewBedInfo.CurrentCell.ColumnIndex == 1)
                    {
                        if (GridViewBedInfo.Rows.Count > 0 && GridViewBedInfo.Rows != null)
                        {
                            SendKeys.Send("{tab}");
                        }
                    }
                    if (keyData == (Keys.Shift | Keys.Tab) && GridViewBedInfo.CurrentCell.ColumnIndex == 0 && GridViewBedInfo.CurrentRow.Index != 0)
                    {
                        if (GridViewBedInfo.Rows.Count > 1 && GridViewBedInfo.Rows != null)
                        {
                            SendKeys.Send("{tab}");
                        }
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void BtnWardBedNew_ItemClickedEvent(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Ward")
            {
                newWardToolStripMenuItem_Click(sender, e);
            }
            else if (e.ClickedItem.Text == "Bed")
            {
                newBedToolStripMenuItem_Click(sender, e);
            }
        }

        private void newWardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabControlWard.Visible = true;
            TabControlBed.Visible = false;
            NewBtnClick();
            this.formIsDirty = false;
        }

        private void newBedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabControlBed.Visible = true;
            TabControlWard.Visible = false;
            NewBtnClick();
            if (TreeViewWardBed.SelectedNode != null)
            {
                ComboBoxSwapTextBoxWard.SelectedIndex = ComboBoxSwapTextBoxWard.FindStringExact(TreeViewWardBed.SelectedNode.Text);
            }
            this.formIsDirty = false;
        }
        private void LoadCombo()
        {
            ComboUtils.InitializeBedTypeCombo(ComboBoxSwapTextBoxBedType, Global.Company.CompanyId);
            ComboUtils.InitializeWardCombo(ComboBoxSwapTextBoxWard, Global.Company.CompanyId);
        }
        private void NewBtnClick()
        {
            ResetForm();
            EnableForm(true);
            LoadCombo();
            if (TabControlWard.Visible)
            {
                GridViewBedInfo.AllowUserToAddRows = true;
                TextBoxWardName.Select();
            }
            else { TextBoxBedName.Select(); };
        }
        private void TextBoxWardSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                TreeViewWardBed.Select();
            }
        }
        private void ComboBoxSwapTextBoxBedType_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxSwapTextBoxBedType.DroppedDown = false;
        }
        private void ComboBoxSwapTextBoxWard_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxSwapTextBoxWard.DroppedDown = false;
        }

        private void TextBoxWardName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                GridViewBedInfo.Select();
                GridViewBedInfo.CurrentCell = GridViewBedInfo[0, 0];
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnWardSave.Focus();
            }
        }

        private void BtnWardSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (TabControlWard.Visible)
                {
                    TextBoxWardName.Select();
                }
                else
                {
                    TextBoxBedName.Select();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (TabControlWard.Visible)
                {
                    InventoryLocationcheckBox.Focus();
                }
                else if(ComboBoxSwapTextBoxWard.Visible)
                {
                    ComboBoxSwapTextBoxWard.Select();
                }
                else
                {
                    ComboBoxSwapTextBoxBedType.Focus();
                }
            }
        }

        private void TextBoxBedName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxSwapTextBoxBedType.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnWardSave.Focus();
            }
        }

        private void GridViewBedInfo_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                BedType BedType = BedManager.GetBedTypeByName(GridViewBedInfo.CurrentCell.EditedFormattedValue.ToString(), Global.Company.CompanyId);
                if (BedType != null)
                {
                    GridViewBedInfo.CurrentCell.Value = BedType.Id;
                }
            }
        }

        private void InventoryLocationcheckBox_Click(object sender, EventArgs e)
        {
            if (!InventoryLocationcheckBox.Checked && !string.IsNullOrEmpty(TextBoxWardBedId.Text))
            {
                DialogResult x = MessageBox.Show("Unchecking this box will remove the inventory location. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (x == DialogResult.No)
                {
                    InventoryLocationcheckBox.Checked = true;
                    BtnWardSave.Select();
                    return;
                }
            }
        }
        private void FormWardAndBed_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show("There are unsaved changes, Do you want Exit?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.No)
                {
                    if (TabControlWard.Visible)
                    {
                        TextBoxWardName.Select();
                    }
                    else
                    {
                        TextBoxBedName.Select();
                    }
                    e.Cancel = true;
                }
            }
        }

        private void InventoryLocationcheckBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            int Row = GridViewBedInfo.Rows.Count;
            int Column = GridViewBedInfo.Columns.Count;
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnWardSave.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                GridViewBedInfo.Select();
                GridViewBedInfo.CurrentCell = GridViewBedInfo.Rows[Row -1].Cells[1];
            }
        }
    }
}
