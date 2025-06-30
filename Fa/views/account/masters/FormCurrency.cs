using fa.model.Common;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using fa.api.Accounting;
using fa.libraries.Validation;

namespace fa.views.account.masters
{
    public partial class FormCurrency : FormBase
    {
        fa.api.Accounting.CurrencyManager CurrencyManager = null;
        KeypressValidation KeypressValidation = null;
        public FormCurrency()
        {
            CurrencyManager = fa.api.Accounting.CurrencyManager.Instance;
            KeypressValidation =   KeypressValidation.Instance;
            InitializeComponent();
           
        }
        private void FormCurrency_Load(object sender, EventArgs e)
        {
            ResetForm();
            EnableForm(false);
            LoadCurrenciesWithFilter();
            this.formIsDirty = false;
        }

        private void LoadCurrenciesWithFilter()
        {
            string FilterString = TextBoxCurrencySearch.Text.Trim();
            ListBoxCurrency.Items.Clear();            
            IList<Currency> Currencies = CurrencyManager.GetAllCurrencies();
            foreach (var lCurrency in Currencies)
            {
                if(FilterString==null || string.IsNullOrEmpty(FilterString.Trim()) || lCurrency.Name.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase)>-1 )
                {
                    ListBoxCurrency.Items.Add(lCurrency);
                }                    
            }

            if (ListBoxCurrency.Items.Count > 0)
            {
                ListBoxCurrency.SelectedIndex = 0;
            }
        }
        private Currency GetCurrencyInfo()
        {
            int Index = ListBoxCurrency.SelectedIndex;
            if (Index > -1)
            {
                Currency lCurrency = (Currency)ListBoxCurrency.Items[Index];
                if (lCurrency != null)
                {
                    Currency CurrencyFromDB = CurrencyManager.GetCurrencyById(lCurrency.CurrencyId);
                    return CurrencyFromDB;
                }
            }
            return null;
        }
        private void LoadCurrencyInfo()
        {
            Currency CurrencyFromDB = GetCurrencyInfo();
            if (CurrencyFromDB != null)
            { 
                TextBoxCurrencyName.Text = CurrencyFromDB.Name;
                TextBoxCurrencyDisplayAs.Text = CurrencyFromDB.DisplayAs;
                TextBoxISOCode.Text = CurrencyFromDB.CurrencyCodeISO;
                TextBoxCurrencyId.Text = CurrencyFromDB.CurrencyId.ToString();
                TextBoxCurrencyFormat.Text = CurrencyFromDB.CurrencyFormat;
                var CurrencyPricision = CurrencyFromDB.RoundingPrecision;
                ComboBoxCurrencyPricision.SelectedIndex = ComboBoxCurrencyPricision.FindStringExact("" + CurrencyFromDB.RoundingPrecision);
            }
            else
            {
                DisplaySystemError("Somthing went wrong, the selected currency is not valid.");
                return;
            }
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            this.formIsDirty = false;
            LoadCurrenciesWithFilter();
            return;
        }
        private Currency GetCurrencyFromForm()
        {
            Currency lCurrency = new Currency();
            if (!string.IsNullOrEmpty(TextBoxCurrencyId.Text))
            {
                lCurrency.CurrencyId = Convert.ToInt64(TextBoxCurrencyId.Text);
            }
            else
            {
                lCurrency.CurrencyId = 0L;
            }
            lCurrency.Name = TextBoxCurrencyName.Text.Trim();
            lCurrency.DisplayAs = TextBoxCurrencyDisplayAs.Text.Trim();
            lCurrency.CurrencyCodeISO = TextBoxISOCode.Text.Trim();
            lCurrency.CurrencyFormat = TextBoxCurrencyFormat.Text;
            if (!string.IsNullOrEmpty(ComboBoxCurrencyPricision.Text))
            {
                lCurrency.RoundingPrecision = Convert.ToInt32(ComboBoxCurrencyPricision.Text);
            }
            return lCurrency;
        }
        private void ListBoxCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetForm();
            LoadCurrencyInfo();
            EnableForm(false);
            this.formIsDirty = false;
        }
        private void TextBoxCurrencySearch_TextChanged(object sender, EventArgs e)
        {
            ResetForm();
            LoadCurrenciesWithFilter();
            if (ListBoxCurrency.Items.Count > 0) { BtnCurrencyEdit.Enabled = true; BtnCurrencyDelete.Enabled = true; }
            else { BtnCurrencyEdit.Enabled = false; BtnCurrencyDelete.Enabled = false; }
        }    
        private void BtnNewCurrency_Click(object sender, EventArgs e)
        {
            ResetForm();
            EnableForm(true);
            TextBoxCurrencyName.Select();
            this.formIsDirty = false;

        }
        private void BtnCurrencyDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxCurrencyId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this currency is still valid.");
                return;
            }
            Currency lCurrency = GetCurrencyInfo();
            if (lCurrency != null)
            {
                DialogResult Result = MessageBox.Show("Do you want to delete the currency " + lCurrency.Name + "?", "Delete Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning,MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.Yes)
                {
                    Boolean Deleted = CurrencyManager.DeleteCurrency(lCurrency.CurrencyId);
                    if (Deleted)
                    {
                        ResetForm();
                        LoadCurrenciesWithFilter();
                        TextBoxCurrencySearch.Clear();
                        EnableForm(false);
                        this.formIsDirty = false;

                    }
                    else
                    {
                        MessageBox.Show("Could not delete Currency," + lCurrency.Name + "!, Please retry. \r\n [This currency data might have been used in other entries like Company, Please verify.]", "Error Deleting Currency", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                DisplaySystemError("Somting went wrong, please check this currency is still valid.");
                return;
            }
        }
        private void BtnCurrencyEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxCurrencyId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this currency is still valid.");
                return;
            }
            LoadCurrencyInfo();
            EnableForm(true);
            TextBoxCurrencyName.Select();
            this.formIsDirty = false;

        }
        private void BtnCurrencyCancelEdit_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show("There are unsaved changes, Do you want cancel?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TextBoxCurrencyName.Select();
                    return;
                }
            }
            ResetForm();
            LoadCurrencyInfo();
            EnableForm(false);
            this.formIsDirty = false;
        }
        private void BtnCurrencySave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Currency lCurrency = GetCurrencyFromForm();
                if (lCurrency.CurrencyId == 0)
                {
                    Currency lCurrencyByName = CurrencyManager.GetCurrencyByName(lCurrency.Name);
                    if (lCurrencyByName == null)
                    {
                        CurrencyManager.AddCurrency(lCurrency);
                        ResetForm();
                        LoadCurrenciesWithFilter();
                        ListBoxCurrency.SelectedIndex = ListBoxCurrency.FindStringExact(lCurrency.Name);
                        EnableForm(false);
                        this.formIsDirty = false;

                    }
                    else
                    {
                        MessageBox.Show(lCurrency.Name + " already exists");
                        TextBoxCurrencyName.Select();
                    }
                }
                else
                {
                    Currency lCurrencyById = CurrencyManager.GetCurrencyById(lCurrency.CurrencyId);
                    if (lCurrencyById != null)
                    {
                        Currency lCurrencyByName = CurrencyManager.CheckCurrencyNameInUpdate(lCurrency.Name, lCurrency.CurrencyId);
                        if (lCurrencyByName == null)
                        {
                            Currency CurrencyInfo = CurrencyManager.UpdateCurrency(lCurrency);
                            ResetForm();
                            LoadCurrenciesWithFilter();
                            ListBoxCurrency.SelectedIndex = ListBoxCurrency.FindStringExact(lCurrency.Name);
                            EnableForm(false);
                            this.formIsDirty = false;

                        }
                        else
                        {
                            MessageBox.Show(lCurrency.Name + " already exists");
                            TextBoxCurrencyName.Select();
                        }
                    }
                    else
                    {
                        DisplaySystemError("Somting went wrong, please check this currency is still valid.");
                        return;
                    }
                }

            }
        }
        private void BtnCurrencyExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private Boolean ValidateForm()
        {
            if (string.IsNullOrEmpty(TextBoxCurrencyName.Text.Trim()))
            {
                MessageBox.Show("Currency Name could not be empty, Please enter Currency Name");
                TextBoxCurrencyName.Select();
                return false;
            }

            if (string.IsNullOrEmpty(TextBoxISOCode.Text.Trim()))
            {
                MessageBox.Show("Currency ISO Code could not be empty, Please enter Currency ISO Code");                
                TextBoxISOCode.Select();
                return false;
            }
            if (ComboBoxCurrencyPricision.SelectedIndex < 0)
            {
                MessageBox.Show("Pricision field could not be empty, Please select Pricision");              
                ComboBoxCurrencyPricision.Select();
                return false;
            }
            return true;
        }      
       
        private void ResetForm()
        {
            TextBoxCurrencyName.ResetText();
            TextBoxCurrencyDisplayAs.ResetText();
            TextBoxISOCode.ResetText();
            TextBoxCurrencyId.ResetText();
            ComboBoxCurrencyPricision.SelectedIndex = -1;
        }
        private void EnableForm(Boolean enable)
        {
            if (ListBoxCurrency.Items.Count > 0)
            {
                TextBoxCurrencySearch.ReadOnly = enable;
                TextBoxCurrencySearch.TabStop = !enable;
                ListBoxCurrency.Enabled = !enable;
            }
            else
            {
                ListBoxCurrency.Enabled = false;
                TextBoxCurrencySearch.ReadOnly = true;
                TextBoxCurrencySearch.TabStop = false;
                BtnNewCurrency.Select();
            }
            TextBoxCurrencyFormat.ReadOnly = !enable;
            TextBoxCurrencyName.ReadOnly = !enable;
            TextBoxCurrencyDisplayAs.ReadOnly = !enable;
            TextBoxISOCode.ReadOnly = !enable;
            TextBoxCurrencyName.TabStop = enable;
            TextBoxCurrencyDisplayAs.TabStop = enable;
            TextBoxISOCode.TabStop = enable;
            TextBoxCurrencyFormat.TabStop = enable;
            ComboBoxCurrencyPricision.Visible = enable;
            if (!enable)
            {
                BtnCurrencyCancelEdit.Enabled = enable;
                if (ListBoxCurrency.SelectedIndex < 0)
                {
                    BtnCurrencyDelete.Enabled = enable;
                    BtnCurrencyEdit.Enabled = enable;
                }
                else
                {
                    BtnCurrencyDelete.Enabled = !enable;
                    BtnCurrencyEdit.Enabled = !enable;
                }
                BtnNewCurrency.Enabled = !enable;
                BtnCurrencySave.Enabled = enable;
            }
            else
            {
                BtnNewCurrency.Enabled = !enable;
                BtnCurrencyDelete.Enabled = !enable;
                BtnCurrencyEdit.Enabled = !enable;
                BtnCurrencySave.Enabled = enable;
                BtnCurrencyCancelEdit.Enabled = enable;
            }
        }
        private void TextBoxCurrencySearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void TextBoxCurrencyName_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }

        private void TextBoxCurrencyDisplayAs_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void TextBoxISOCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        string Unselected = "";

 
        private void TextBoxCurrencyFormat_KeyPress(object sender, KeyPressEventArgs e)
        {
            Unselected = TextBoxCurrencyFormat.Text;
            if (TextBoxCurrencyFormat.SelectionLength > 0) { Unselected = TextBoxCurrencyFormat.Text.Remove(TextBoxCurrencyFormat.SelectionStart, TextBoxCurrencyFormat.SelectionLength); }
            KeypressValidation.Keypress_NumberDotBraket(sender, e,Unselected);
        }
        private void TextBoxCurrencySearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                ListBoxCurrency.Select();
            }
        }

        private void ComboBoxCurrencyPricision_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxCurrencyPricision.DroppedDown = false;
        }

        private void TextBoxCurrencyName_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "NameChecking");
        }

        private void TextBoxCurrencyName_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxCurrencyName, "NameChecking");
            }

        }

        private void TextBoxCurrencyDisplayAs_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "NameChecking");

        }

        private void TextBoxCurrencyDisplayAs_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxCurrencyDisplayAs, "NameChecking");
            }
        }

        private void TextBoxISOCode_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "NameChecking");
        }
        private void TextBoxISOCode_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxISOCode, "NameChecking");
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnNewCurrency.PerformClick();
            }
            else if (keyData == (Keys.F4))
            {
                BtnCurrencyDelete.PerformClick();
            }
            else if (keyData == (Keys.F7))
            {
                BtnCurrencyEdit.PerformClick();
            }
            else if (keyData == (Keys.F8))
            {
                BtnCurrencySave.PerformClick();
            }
            else if (keyData == (Keys.F10))
            {
                BtnCurrencyExit.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCurrencyCancelEdit.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void FormCurrency_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show("There are unsaved changes, Do you want Exit?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning,MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TextBoxCurrencyName.Select();
                    e.Cancel = true;
                }
            }
        }

        private void BtnCurrencySave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                TextBoxCurrencyName.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxCurrencyPricision.Select();
            }
        }

        private void TextBoxCurrencyName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                TextBoxCurrencyDisplayAs.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnCurrencySave.Select();
            }
        }
    }
}
