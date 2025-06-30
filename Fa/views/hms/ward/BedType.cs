using fa.libraries.Validation;
using fa.api.Hms;
using fa.model.Hms.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using fa.api.utils;
using fa.views.controls.grid;
namespace fa.views.hms.ward
{
    public partial class FormBedType : FormBase
    {
        public static string SaveSuccessMsg = "Saved successfully";
        public static string EnterNameErrorMsg = "Name could not be empty, Please enter the Name";
        public static string DoNotAllowToSaveBedTypNamExisteMsg = "Bed type '{0}' already exists";
        public static string ErrorDeleteBedTypeMsg = "Error deleting the Bed type!, Please retry";
        public static string DoNotAllowToDeleteBedTypeMsg = "Duplicate bed type '{0}'";
        public static string DoNotAllowToDeleteBedMsg = "Bed '{0}' is occupied and cannot be deleted";
        public static string EnterRateErrorMsg = "Please enter the bed '{0}'";
        public static string EnterRateRepeatErrorMsg = "Duplicate rent duration '{0}'";
        public static string EnterRentErrorMsg = "Please enter at least one rental details";
        public bool CreateOpBedTypeOnLoad = false;
        BedManager BedManager = null!;
        KeypressValidation KeypressValidation = null!;
        public FormBedType(object sender)
        {
            try
            {
                FormWardAndBed FormWardAndBed = (FormWardAndBed)sender;
            }
#pragma warning disable 0168 // variable declared but not used.
            catch (InvalidCastException ex)
            {
                Container parent = (Container)sender;
            }
#pragma warning restore 0168
            BedManager = BedManager.Instance;
            KeypressValidation = KeypressValidation.Instance;
            InitializeComponent();
        }
        private void BedType_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                EnableForm(true);
                LoadBedTypesWithFilter();
                GridViewRateInfo.RowTemplate.Height = 20;
                if (CreateOpBedTypeOnLoad)
                {
                    this.Text = "Update BedType";
                    this.BtnBedTypeEdit.Visible = false;
                    BtnBedTypeCancel.Visible = false;
                    BtnBedTypeNew.Visible = false;
                    BtnBedTypeDelete.Visible = false;
                    TextBoxBedTypeSearch.Visible = false;
                    TreeViewBedType.Visible = false;
                    TabControlBedType.Location = new Point(12, 12);
                    BtnBedTypeSave.Location = new Point(TabControlBedType.Width - 74, TabControlBedType.Height + 15);
                    this.Size = new Size(TabControlBedType.Right + 25, TabControlBedType.Bottom + 90);
                    this.CenterToParent();
                    BtnBedTypeNew_Click(this, null!);
                }
                this.formIsDirty = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void LoadBedTypesWithFilter()
        {
            string FilterString = TextBoxBedTypeSearch.Text.Trim();
            TreeViewBedType.Nodes.Clear();
            IList<BedType> BedType = BedManager.ListBedTypeByCompanyId(Global.Company.CompanyId);
            if (BedType.Count > 0)
            {
                foreach (var lBedType in BedType)
                {
                    if (FilterString == null || string.IsNullOrEmpty(FilterString.Trim()) || lBedType.Name.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        TreeViewBedType.Nodes.Add(lBedType.Id.ToString(), lBedType.Name);
                        TreeViewBedType.Nodes[lBedType.Id.ToString()].ImageIndex = 0;
                    }
                }
            }
            else
            {
                GridViewRateInfo.AllowUserToAddRows = true;
                TextBoxBedTypeName.Select();
            }
            if (TreeViewBedType.Nodes.Count > 0)
            {
                TreeViewBedType.ExpandAll();
                TreeViewBedType.SelectedNode = TreeViewBedType.Nodes[0];
            }
        }
        private BedType GetBedTypeInfo()
        {
            if (TreeViewBedType.SelectedNode != null)
            {
                BedType lBedType = BedManager.GetBedTypeById(long.Parse(TreeViewBedType.SelectedNode.Name));
                if (lBedType != null)
                {
                    BedType BedTypeFromDB = BedManager.GetBedTypeById(lBedType.Id);
                    return BedTypeFromDB;
                }
            }
            return null!;
        }
        private void LoadBedTypeInfo()
        {
            BedType BedTypeFromDB = GetBedTypeInfo();
            if (BedTypeFromDB != null)
            {
                GridViewRateInfo.Rows.Clear();
                TextBoxBedTypeId.Text = BedTypeFromDB.Id.ToString();
                TextBoxBedTypeName.Text = BedTypeFromDB.Name;
                if (BedTypeFromDB.Rents.Count > 0)
                {
                    GridViewRateInfo.Rows.Add(BedTypeFromDB.Rents.Count);
                    int i = 0;
                    foreach (Rent Rent in BedTypeFromDB.Rents)
                    {
                        var values = from Enum e in Enum.GetValues(typeof(RentPeriod)) select new { ID = e, Name = e.ToString() };
                        (GridViewRateInfo.Rows[i].Cells[0] as DataGridViewComboBoxCell)!.DataSource = null;
                        (GridViewRateInfo.Rows[i].Cells[0] as DataGridViewComboBoxCell)!.DataSource = values.ToList();
                        (GridViewRateInfo.Rows[i].Cells[0] as DataGridViewComboBoxCell)!.ValueMember = "ID";
                        (GridViewRateInfo.Rows[i].Cells[0] as DataGridViewComboBoxCell)!.DisplayMember = "Name";
                        GridViewRateInfo.Rows[i].Cells[0].Value = Rent.RentPeriod;
                        GridViewRateInfo.Rows[i].Cells[1].Value = Rent.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewRateInfo.Rows[i].Cells[2].Value = "X";
                        i++;
                    }
                }
            }
        }
        private BedType GetBedTypeFromForm()
        {
            BedType lBedType = new BedType();
            if (!string.IsNullOrEmpty(TextBoxBedTypeId.Text))
            {
                lBedType.Id = Convert.ToInt64(TextBoxBedTypeId.Text);
            }
            else
            {
                lBedType.Id = 0L;
            }
            lBedType.Name = TextBoxBedTypeName.Text.Trim();
            lBedType.CompanyId = Global.Company.CompanyId;
            return lBedType;
        }
        private void TextBoxBedTypeSearch_TextChanged(object sender, EventArgs e)
        {
            ResetForm();
            LoadBedTypesWithFilter();
            if (TreeViewBedType.Nodes.Count > 0) { BtnBedTypeEdit.Enabled = true; BtnBedTypeDelete.Enabled = true; }
            else { BtnBedTypeEdit.Enabled = false; BtnBedTypeDelete.Enabled = false; }
        }
        private void TreeViewBedType_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node!;
            node.SelectedImageIndex = node.ImageIndex;
            ResetForm();
            LoadBedTypeInfo();
            EnableForm(false);
            this.formIsDirty = false;
        }
        private void BtnBedTypeNew_Click(object sender, EventArgs e)
        {
            ResetForm();
            EnableForm(true);
            GridViewRateInfo.AllowUserToAddRows = true;
            TextBoxBedTypeName.Select();
            this.formIsDirty = false;
        }
        private void BtnBedTypeDelete_Click(object sender, EventArgs e)
        {
            BedType BedTypeInfo = GetBedTypeInfo();
            BedTypeErrorMsg.Text = "";
            if (BedTypeInfo != null)
            {
                DialogResult Result = MessageBox.Show("Do you want to delete the BedType " + BedTypeInfo.Name + "?", "Delete Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.Yes)
                {
                    if (BedManager.DeleteBedType(BedTypeInfo.Id))
                    {
                        ResetForm();
                        LoadBedTypesWithFilter();
                        EnableForm(false);
                    }
                    else
                    {
                        BedTypeErrorMsg.Text = string.Format(DoNotAllowToDeleteBedMsg, BedTypeInfo.Name);
                    }
                    this.formIsDirty = false;
                }
            }
            else
            {
                BedTypeErrorMsg.Text = ErrorDeleteBedTypeMsg;
            }
        }
        private void BtnBedTypeEdit_Click(object sender, EventArgs e)
        {
            LoadBedTypeInfo();
            EnableForm(true);
            GridViewRateInfo.AllowUserToAddRows = true;
            this.formIsDirty = false;
            TextBoxBedTypeName.Select();
        }
        private void BtnBedTypeCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show("There are unsaved changes, Do you want cancel?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.No)
                {
                    TextBoxBedTypeName.Select();
                    return;
                }
            }
            ResetForm();
            if (string.IsNullOrEmpty(TextBoxBedTypeSearch.Text))
            {
                LoadBedTypeInfo();
            }
            else
            {
                TextBoxBedTypeSearch.Clear();
            }
            EnableForm(false);
            TreeViewBedType.Select();
            this.formIsDirty = false;
        }
        private void BtnBedTypeSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                BedType lBedType = GetBedTypeFromForm();
                if (BedManager.BedtypeNameUniqueById(lBedType))
                {
                    BedType BedTypeFromDB = null!;
                    if (lBedType.Id == 0)
                    {
                        BedTypeFromDB = BedManager.AddBedType(lBedType);
                    }
                    else
                    {
                        BedType lBedTypeById = BedManager.GetBedTypeById(lBedType.Id);
                        if (lBedTypeById != null)
                        {
                            BedTypeFromDB = BedManager.UpdateBedType(lBedType);
                        }
                    }
                    BedType BedType = new BedType();
                    BedType.Id = BedTypeFromDB.Id;
                    if (GridViewRateInfo.Rows.Count > 0)
                    {
                        BedType.Rents = new List<Rent>() { };
                        for (int i = 0; i < GridViewRateInfo.Rows.Count - 1; i++)
                        {
                            Rent Rent = new Rent();
                            Rent.RentPeriod = (RentPeriod)Enum.Parse(typeof(RentPeriod), GridViewRateInfo.Rows[i].Cells[0].Value.ToString()!, true);
                            Rent.Amount = float.Parse(GridViewRateInfo.Rows[i].Cells[1].Value.ToString()!);
                            Rent.BedTypeId = BedTypeFromDB.Id;
                            Rent.CompanyId = BedTypeFromDB.CompanyId;
                            BedType.Rents.Add(Rent);
                        }
                    }
                    BedManager.AddBedRentDetail(BedType);
                    if (CreateOpBedTypeOnLoad)
                    {
                        this.formIsDirty = false;
                        this.Close();
                    }
                    ResetForm();
                    if (string.IsNullOrEmpty(TextBoxBedTypeSearch.Text))
                    {
                        LoadBedTypesWithFilter();
                    }
                    else
                    {
                        TextBoxBedTypeSearch.Clear();
                    }
                    TextBoxBedTypeId.Text = BedTypeFromDB.Id.ToString();
                    TreeNode TreeNode = new TreeNode();
                    TreeNode = TreeViewBedType.Nodes[BedTypeFromDB.Id.ToString()];
                    TreeViewBedType.SelectedNode = TreeNode;
                    TreeViewBedType.Focus();
                    BedTypeErrorMsg.Text = SaveSuccessMsg;
                    this.formIsDirty = false;
                }
                else
                {
                    BedTypeErrorMsg.Text = string.Format(DoNotAllowToSaveBedTypNamExisteMsg, lBedType.Name);
                    TextBoxBedTypeName.Select();
                }
            }
        }
        private Boolean ValidateForm()
        {
            BedTypeErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxBedTypeName.Text.Trim()))
            {
                BedTypeErrorMsg.Text = EnterNameErrorMsg;
                TextBoxBedTypeName.Select();
                return false;
            }
            if (GridViewRateInfo.Rows.Count <= 1 && GridViewRateInfo.Rows[0].Cells[0].Value == null)
            {
                BedTypeErrorMsg.Text = EnterRentErrorMsg;
                GridViewRateInfo.Select();
                GridViewRateInfo.CurrentCell = GridViewRateInfo[0, 0];
                return false;
            }
            if (GridViewRateInfo.Rows.Count > 1)
            {
                for (int i = 0; i < GridViewRateInfo.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (GridViewRateInfo.Rows[i].Cells[j].Value == null || GridViewRateInfo.Rows[i].Cells[j].Value.Equals(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)))
                        {
                            BedTypeErrorMsg.Text = string.Format(EnterRateErrorMsg, GridViewRateInfo.Columns[j].HeaderText);
                            GridViewRateInfo.Select();
                            GridViewRateInfo.CurrentCell = GridViewRateInfo[j, i];
                            return false;
                        }
                    }
                    int RentPeriod = 0;
                    for (int k = 0; k < GridViewRateInfo.Rows.Count - 1; k++)
                    {
                        if (GridViewRateInfo.Rows[k].Cells[0].Value != null)
                        {
                            if (GridViewRateInfo.Rows[i].Cells[0].Value.ToString() == GridViewRateInfo.Rows[k].Cells[0].Value.ToString())
                            {
                                RentPeriod++;
                            }
                        }
                        if (RentPeriod > 1)
                        {
                            BedTypeErrorMsg.Text = string.Format(EnterRateRepeatErrorMsg, GridViewRateInfo.Rows[k].Cells[0].Value.ToString());
                            GridViewRateInfo.Select();
                            GridViewRateInfo.CurrentCell = GridViewRateInfo[0, k];
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        private void BtnBedTypeExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ResetForm()
        {
            BedTypeErrorMsg.Text = "";
            TextBoxBedTypeId.ResetText();
            TextBoxBedTypeName.ResetText();
            GridViewRateInfo.Rows.Clear();
            GridViewRateInfo.RowTemplate.Height = 20;
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)GridViewRateInfo.Columns["Rate"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
        }
        private void EnableForm(Boolean enable)
        {
            BedType BedTypeInfo = GetBedTypeInfo();
            if (TreeViewBedType.Nodes.Count > 0)
            {
                TreeViewBedType.Enabled = !enable;
                TextBoxBedTypeSearch.ReadOnly = enable;
                TextBoxBedTypeSearch.TabStop = !enable;
            }
            else
            {
                TreeViewBedType.Enabled = false;
                TextBoxBedTypeSearch.ReadOnly = true;
                TextBoxBedTypeSearch.TabStop = false;
                BtnBedTypeNew.Select();
            }
            GridViewRateInfo.RowTemplate.Height = 20;
            GridViewRateInfo.AllowUserToAddRows = false;
            GridViewRateInfo.Enabled = enable;
            TextBoxBedTypeName.ReadOnly = !enable;
            TextBoxBedTypeName.TabStop = enable;
            if (!enable)
            {
                BtnBedTypeCancel.Enabled = enable;
                if (TreeViewBedType.SelectedNode == null)
                {
                    BtnBedTypeDelete.Enabled = enable;
                    BtnBedTypeEdit.Enabled = enable;
                }
                else
                {
                    BtnBedTypeDelete.Enabled = !enable;
                    BtnBedTypeEdit.Enabled = !enable;
                }
                BtnBedTypeNew.Enabled = !enable;
                BtnBedTypeSave.Enabled = enable;
            }
            else
            {
                BtnBedTypeCancel.Enabled = enable;
                BtnBedTypeDelete.Enabled = !enable;
                BtnBedTypeEdit.Enabled = !enable;
                BtnBedTypeNew.Enabled = !enable;
                BtnBedTypeSave.Enabled = enable;
            }
        }
        private void GridViewRateInfo_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewRateInfo.Rows[e.RowIndex].Cells[1].ReadOnly = true;
            GridViewRateInfo.Rows[e.RowIndex].Cells[2].ReadOnly = true;

            if (GridViewRateInfo.Rows[e.RowIndex].Cells[0].Value != null)
            {
                GridViewRateInfo.Rows[e.RowIndex].Cells[1].ReadOnly = false;
                GridViewRateInfo.Rows[e.RowIndex].Cells[2].ReadOnly = false;
            }
        }
        private void GridViewRateInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 2 && (GridViewRateInfo.Rows.Count - 1) != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete " + GridViewRateInfo.Rows[e.RowIndex].Cells[0].Value.ToString() + "?", "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        GridViewRateInfo.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewRateInfo.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
        }
        private void GridViewRateInfo_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            var values = from Enum en in Enum.GetValues(typeof(RentPeriod)) select new { ID = en, Name = en.ToString() };
            (GridViewRateInfo.Rows[e.RowIndex].Cells[0] as DataGridViewComboBoxCell)!.DataSource = null;
            (GridViewRateInfo.Rows[e.RowIndex].Cells[0] as DataGridViewComboBoxCell)!.DataSource = values.ToList();
            (GridViewRateInfo.Rows[e.RowIndex].Cells[0] as DataGridViewComboBoxCell)!.ValueMember = "ID";
            (GridViewRateInfo.Rows[e.RowIndex].Cells[0] as DataGridViewComboBoxCell)!.DisplayMember = "Name";
            GridViewRateInfo.Rows[e.RowIndex].Cells[1].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            GridViewRateInfo.Rows[e.RowIndex].Cells[2].Value = "X";
        }
        private void GridViewRateInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void GridViewRateInfo_KeyPress1(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                ((ComboBox)GridViewRateInfo.EditingControl).DroppedDown = false;
            }
        }
        private void GridViewRateInfo_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            GridViewRateInfo.CommitEdit(DataGridViewDataErrorContexts.Commit);
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                ((ComboBox)e.Control).MaxLength = 30;
                if (GridViewRateInfo.CurrentCell.Value == null)
                {
                    ((ComboBox)e.Control).SelectedIndex = -1;
                }
                e.Control.KeyPress += new KeyPressEventHandler(GridViewRateInfo_KeyPress1);
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnBedTypeNew.PerformClick();
            }
            if (keyData == (Keys.F4))
            {
                BtnBedTypeDelete.PerformClick();
            }
            if (keyData == (Keys.F7))
            {
                BtnBedTypeEdit.PerformClick();
            }
            if (keyData == (Keys.F8))
            {
                BtnBedTypeSave.PerformClick();
            }
            if (keyData == (Keys.F10))
            {
                BtnBedTypeExit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnBedTypeCancel.PerformClick();
                return true;
            }
            try
            {
                if (keyData == (Keys.Tab) && GridViewRateInfo.CurrentCell.ColumnIndex == 1)
                {
                    SendKeys.Send("{tab}");
                }
                if (keyData == (Keys.Shift | Keys.Tab) && GridViewRateInfo.CurrentCell.ColumnIndex == 0 && GridViewRateInfo.CurrentRow.Index != 0)
                {
                    SendKeys.Send("{tab}");
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void TextBoxBedTypeSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                TreeViewBedType.Select();
            }
        }
        private void TextBoxBedTypeName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                GridViewRateInfo.Select();
                GridViewRateInfo.CurrentCell = GridViewRateInfo[0, 0];
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnBedTypeSave.Focus();
            }
        }
        private void BtnBedTypeSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            int Row = GridViewRateInfo.Rows.Count;
            int Column = GridViewRateInfo.Columns.Count;
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxBedTypeName.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                GridViewRateInfo.Select();
                GridViewRateInfo.CurrentCell = GridViewRateInfo.Rows[Row - 1].Cells[1];
            }
        }
        private void GridViewRateInfo_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                var values = from Enum en in Enum.GetValues(typeof(RentPeriod)) select new { ID = en, Name = en.ToString() };
                var Output = values.ToList().Find(x => x.Name.Equals(GridViewRateInfo.CurrentCell.EditedFormattedValue.ToString()!.ToUpper()));
                if (Output != null)
                {
                    GridViewRateInfo.CurrentCell.Value = Output.ID;
                }
            }
        }
        private void FormBedType_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show("There are unsaved changes, Do you want Exit?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.No)
                {
                    TextBoxBedTypeName.Select();
                    e.Cancel = true;
                }
            }
        }
    }
}
