using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.api.Hms;
using fa.Data;
using Fa.api.Hms;
using fa.model.OrderManagement;
using fa.libraries.utils;
using static fa.libraries.utils.ComboUtils;

namespace fa.views.hms.inventory
{
    public partial class FormInventoryLocations : FormBase
    {
        public static string SelectedLocationNotValidErrorMsg = "Somthing went wrong, the selected location is not valid.";
        public static string EnterNameErrorMsg = "Name could not be empty, Please enter name.";
        public static string SelectTypeEmptyErrorMsg = "Type could not be empty, Please select type.";
        public static string SelectTypeErrorMsg = "Please select valid type.";
        public static string ErrorDeleteLocationMsg = "Error deleting the location!, Please retry";
        public static string SaveSuccessMsg = "Saved success.";
        public static string EnterNameRepeatErrorMsg = "Name {0} already exists";
        public static string ErrorLocation = "Already this Inventory Location asigned, Please Try for another Name";
        HospitalInventoryManager HospitalInventoryManager = null;
        long LocationId = 0L;
        public FormInventoryLocations()
        {
            HospitalInventoryManager = HospitalInventoryManager.Instance;
            InitializeComponent();
        }
        private void ResetForm()
        {
            LocationId = 0L;
            InventoryLocationstextBoxname.ResetText();
            InventoryLocationstextBoxdescription.ResetText();
            LoadInventoryLocationType();
            InventoryLocationsComboBoxType.SelectedIndex = -1;
            InventoryLocationsComboBoxType.ResetText();
            LocationErrorMsg.Text = string.Empty;
        }
        
        private void LoadInventoryLocationType()
        {
            ComboUtils.InitializeInventoryLocationType(InventoryLocationsComboBoxType);
        }
        private void LoadLocationWithFilter()
        {
            string FilterString = InventoryLocationstextBoxsearch.Text.Trim();
            ListBoxInventoryLocation.Items.Clear();
            IList<InventoryLocation> locations = HospitalInventoryManager.ListAllInventoryLocation(Global.Company.CompanyId);
            foreach (var llocation in locations)
            {
                if (FilterString == null || string.IsNullOrEmpty(FilterString.Trim())
                    || llocation.Name.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    ListBoxInventoryLocation.Items.Add(llocation);
                }
            }
            if (ListBoxInventoryLocation.Items.Count > 0)
            {
                ListBoxInventoryLocation.SelectedIndex = 0;
            }
            else
            {
                BtnInventoryLocationsNew.Select();
            }
        }
       
        private InventoryLocation GetLocationFromForm()
        {
            InventoryLocation llocation = new InventoryLocation();
            {
                llocation.Id = LocationId;
                llocation.CompanyId = Global.Company.CompanyId;
                llocation.Name = InventoryLocationstextBoxname.Text.Trim();
                llocation.Description = InventoryLocationstextBoxdescription.Text.Trim();
                llocation.Type = (InventoryLocationType)((LocationType)InventoryLocationsComboBoxType.Items[InventoryLocationsComboBoxType.SelectedIndex]).Id;
            }
            return llocation;
        }        
        private void EnableForm(Boolean enable)
        {
            if (ListBoxInventoryLocation.Items.Count > 0)
            {
                ListBoxInventoryLocation.Enabled = !enable;
                InventoryLocationstextBoxsearch.ReadOnly = enable;
                InventoryLocationstextBoxsearch.TabStop = !enable;
            }
            else
            {
                ListBoxInventoryLocation.Enabled = false;
                InventoryLocationstextBoxsearch.ReadOnly = true;
                InventoryLocationstextBoxsearch.TabStop = false;
                BtnInventoryLocationsNew.Select();
            }
            InventoryLocationstextBoxname.ReadOnly = !enable;
            InventoryLocationstextBoxdescription.ReadOnly = !enable;
            InventoryLocationstextBoxname.TabStop = enable;
            InventoryLocationstextBoxdescription.TabStop = enable;
            InventoryLocationsComboBoxType.Visible = enable;
            if (!enable)
            {
                BtnInventoryLocationsCancel.Enabled = enable;
                if (ListBoxInventoryLocation.Items.Count == 0)
                {
                    BtnInventoryLocationsDelete.Enabled = enable;
                    BtnInventoryLocationsEdit.Enabled = enable;
                }
                else
                {
                    BtnInventoryLocationsDelete.Enabled = !enable;
                    BtnInventoryLocationsEdit.Enabled = !enable;
                }

                BtnInventoryLocationsNew.Enabled = !enable;
                BtnInventoryLocationsSave.Enabled = enable;
            }
            else
            {
                BtnInventoryLocationsCancel.Enabled = enable;
                BtnInventoryLocationsDelete.Enabled = !enable;
                BtnInventoryLocationsEdit.Enabled = !enable;
                BtnInventoryLocationsNew.Enabled = !enable;
                BtnInventoryLocationsSave.Enabled = enable;
            }
            this.formIsDirty = false;
        }
        private Boolean ValidateForm()
        {
            LocationErrorMsg.Text = "";
            if (string.IsNullOrEmpty(InventoryLocationstextBoxname.Text.Trim()))
            {
                LocationErrorMsg.Text =EnterNameErrorMsg;
                InventoryLocationstextBoxname.Select();
                return false;
            }
            if (string.IsNullOrEmpty(InventoryLocationsComboBoxType.Text.Trim()))
            {
                LocationErrorMsg.Text = SelectTypeEmptyErrorMsg;
                InventoryLocationsComboBoxType.Select();
                return false;
            }
            if (InventoryLocationsComboBoxType.SelectedIndex<0)
            {
                LocationErrorMsg.Text = SelectTypeErrorMsg;
                InventoryLocationsComboBoxType.Select();
                return false;
            }
            return true;
        }
        private void FormInventoryLocations_Load(object sender, EventArgs e)
        {
            ResetForm();
            LoadLocationWithFilter();
            EnableForm(false);
        }
        private void BtnInventoryLocationsExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnInventoryLocationsDelete_Click(object sender, EventArgs e)
        { 
            InventoryLocation LocationInfo = GetLocationInfo();
            if (LocationInfo != null)
            {
                DialogResult Result = MessageBox.Show("Do you want to delete the Inventory Location "+LocationInfo.Name, "Delete Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Boolean Deleted = HospitalInventoryManager.DeleteLocation(LocationId);
                    if (Deleted)
                    {
                        ResetForm();
                        LoadLocationWithFilter();
                        EnableForm(false);
                        InventoryLocationstextBoxsearch.Select();
                    }
                    else
                    {
                        LocationErrorMsg.Text =ErrorDeleteLocationMsg;
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            else
            {
                LocationErrorMsg.Text = (SelectedLocationNotValidErrorMsg);
                return;
            }
            InventoryLocationstextBoxsearch.Clear();
        }
        private void BtnInventoryLocationsCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show("There are unsaved changes, Do you want cancel?", "Confirm",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.No)
                {
                    InventoryLocationstextBoxname.Select();
                    return;
                }
            }
            ResetForm();
            LoadLocationWithFilter();
            InventoryLocationstextBoxsearch.ResetText();
            EnableForm(false);
            BtnInventoryLocationsNew.Select();
        }
        private void BtnInventoryLocationsEdit_Click(object sender, EventArgs e)
        {  
            LoadLocationInfo();
            EnableForm(true);
            InventoryLocationstextBoxname.Select();
            //InventoryLocationstextBoxsearch.Clear();
        }
        private void BtnInventoryLocationsNew_Click(object sender, EventArgs e)
        {
            InventoryLocationstextBoxsearch.Clear();
            ResetForm();
            EnableForm(true);
            InventoryLocationstextBoxname.Select();
        }
        private void InventoryLocationstextBoxsearch_TextChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            LoadLocationWithFilter();
            if (ListBoxInventoryLocation.Items.Count > 0)
            {
                BtnInventoryLocationsEdit.Enabled = true;
                BtnInventoryLocationsDelete.Enabled = true;
            }
            else
            {
                BtnInventoryLocationsEdit.Enabled = false;
                BtnInventoryLocationsDelete.Enabled = false;
            }
            InventoryLocationstextBoxsearch.Select();
            Cursor.Current = Cursors.Default;
        }
        private void ListBoxInventoryLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLocationInfo();
            EnableForm(false);
        }
        private void LoadLocationInfo()
        {
            InventoryLocation LocationFromDB = GetLocationInfo();
            if (LocationFromDB != null)
            {
                InventoryLocationstextBoxname.Text = LocationFromDB.Name;
                InventoryLocationstextBoxdescription.Text = LocationFromDB.Description;
                //InventoryLocationsComboBoxType.SelectedIndex = (int)LocationFromDB.Type;
                InventoryLocationsComboBoxType.SelectedIndex = InventoryLocationsComboBoxType.FindStringExact(/*System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(*/LocationFromDB.Type.ToString()/*)*/);
                LocationId = LocationFromDB.Id;
            }
        }
        private InventoryLocation GetLocationInfo()
        {
            InventoryLocation LocationFromDB = null;
            if (ListBoxInventoryLocation.SelectedIndex > -1)
            {
                LocationFromDB = HospitalInventoryManager.GetLocationById(((InventoryLocation)ListBoxInventoryLocation.Items[ListBoxInventoryLocation.SelectedIndex]).Id);
            }
            return LocationFromDB;
        }
        private void BtnInventoryLocationsSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                InventoryLocation lLocations = GetLocationFromForm();
                if (HospitalInventoryManager.FindLocatioUnique(lLocations))
                {
                    InventoryLocation LocationFromDB = null;
                    if (lLocations.Id == 0)
                    {
                        LocationFromDB = HospitalInventoryManager.AddLocation(lLocations);
                    }
                    else
                    {
                        LocationFromDB = HospitalInventoryManager.UpdateLocation(lLocations);
                    }
                    ResetForm();
                    LoadLocationWithFilter();
                    if(ListBoxInventoryLocation.Items.Count>0)
                    {
                        ListBoxInventoryLocation.SelectedIndex= ListBoxInventoryLocation.FindStringExact(LocationFromDB.Name);
                    }
                    EnableForm(false);
                    LocationErrorMsg.Text = SaveSuccessMsg;
                    InventoryLocationstextBoxsearch.Select();
                }               
            }
        }
        private void InventoryLocationsComboBoxType_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.InventoryLocationsComboBoxType.DroppedDown = false;
        }
        private void InventoryLocationstextBoxsearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                ListBoxInventoryLocation.Select();
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnInventoryLocationsNew.PerformClick();
            }
            if (keyData == (Keys.F4))
            {
                BtnInventoryLocationsDelete.PerformClick();
            }
            if (keyData == (Keys.F7))
            {
                BtnInventoryLocationsEdit.PerformClick();
            }
            if (keyData == (Keys.F8))
            {
                BtnInventoryLocationsSave.PerformClick();
            }
            if (keyData == (Keys.F10))
            {
                BtnInventoryLocationsExit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {                
                BtnInventoryLocationsCancel.PerformClick();
                return true;
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void BtnInventoryLocationsSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                InventoryLocationstextBoxname.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                InventoryLocationsComboBoxType.Select();
            }
        }

        private void InventoryLocationstextBoxname_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                InventoryLocationstextBoxdescription.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                BtnInventoryLocationsSave.Select();
            }
        }
    }
}
