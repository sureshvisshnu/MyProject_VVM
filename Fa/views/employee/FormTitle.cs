using fa.libraries.Validation;
using fa.api.Accounting;
using fa.model.Employee;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace fa.views.employee
{
    public partial class FormJobTitle : FormBase
    {
        public static string SelectedTitleNotValidErrorMsg = "Somthing went wrong, the selected title is not valid.";
        public static string RemoveSuccess = "{0} romove successful";
        public static string SaveSuccess = "Save successful...";
        public static string DeleteTitleConfirmText = "Do you want to delete the Title {0}?";
        public static string DeleteNotAllowErrorText = "Cannot delete title {0} \r\n [This Title data might have been used in other entries like employee, Please verify.]";
        public static string DeleteErrorText = "Error Deleting the title !, Please retry";
        public static string UniquetitleErrorMsg = "Title {0} already exists";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string EntertitleErrorMsg = "Please enter title";

        public bool CreateTitleOnLoad = false;
        TitleManager TitleManager = null;
        KeypressValidation KeypressValidation = null;
        public FormJobTitle()
        {
            TitleManager = TitleManager.Instance;
            KeypressValidation = KeypressValidation.Instance;
            InitializeComponent();

        }
        private void FormJobTitle_Load(object sender, EventArgs e)
        {
            ResetForm();
            EnableForm(false);
            LoadTitleWithFilter();
            if (CreateTitleOnLoad)
            {
                this.Text = "Create New Job Title";
                this.BtnTitleEdit.Visible = false;
                BtnTitleDelete.Visible = false;
                BtnTitleNew.Visible = false;
                BtnTitleCancel.Visible = false;
                ListBoxTitle.Visible = false;
                GroupBoxTitle.Location = new Point(12, 12);
                BtnTitleSave.Location = new Point(GroupBoxTitle.Width - 78, GroupBoxTitle.Height + 15);
                this.Size = new Size(GroupBoxTitle.Right + 27, GroupBoxTitle.Bottom + 90);
                this.CenterToParent();
                BtnTitleNew_Click(this, null);
            }
        }
        private void LoadTitleWithFilter()
        {
            string FilterString = TextBoxTitleSearch.Text.Trim();
            ListBoxTitle.Items.Clear();
            IList<Title> Title = TitleManager.ListTitleByCompanyId(Global.Company.CompanyId, Global.softwareType);
            foreach (var lTitle in Title)
            {
                if (FilterString == null || string.IsNullOrEmpty(FilterString.Trim()) || lTitle.Name.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    ListBoxTitle.Items.Add(lTitle);
                }
            }
            if (ListBoxTitle.Items.Count > 0)
            {
                ListBoxTitle.SelectedIndex = 0;
            }
        }
        private Title GetTitleInfo()
        {
            int Index = ListBoxTitle.SelectedIndex;
            if (Index > -1)
            {
                Title lTitle = (Title)ListBoxTitle.Items[Index];
                if (lTitle != null)
                {
                    Title PaymentMethodInfo = TitleManager.GetTitleInfoById(lTitle.Id);
                    return PaymentMethodInfo;
                }
            }
            return null;
        }
        private void LoadTitleInfo()
        {
            Title TitleFromDB = GetTitleInfo();
            if (TitleFromDB != null)
            {
                TextBoxTitleId.Text = TitleFromDB.Id.ToString();
                TextBoxTltleJobTitle.Text = TitleFromDB.Name;
                TextBoxTitleDisplayAs.Text = TitleFromDB.DisplayAs;
                TextBoxTitleDescription.Text = TitleFromDB.Discription;
            }
            else
            {
                DisplaySystemError(SelectedTitleNotValidErrorMsg);
                return;
            }
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            this.formIsDirty = false;
            LoadTitleWithFilter();
            return;
        }
        private Title GetTitleFromForm()
        {
            Title lTitle = new Title();
            if (!string.IsNullOrEmpty(TextBoxTitleId.Text))
            {
                lTitle.Id = Convert.ToInt64(TextBoxTitleId.Text);
            }
            else
            {
                lTitle.Id = 0L;
            }
            lTitle.Name = TextBoxTltleJobTitle.Text.Trim();
            lTitle.Discription = TextBoxTitleDescription.Text.Trim();
            lTitle.DisplayAs = TextBoxTitleDisplayAs.Text.Trim();
            lTitle.CompanyId = Global.Company.CompanyId;

            return lTitle;
        }
        private void ListBoxTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetForm();
            LoadTitleInfo();
            EnableForm(false);
        }

        private void TextBoxTitleSearch_TextChanged(object sender, EventArgs e)
        {
            ResetForm();
            LoadTitleWithFilter();
            if (ListBoxTitle.Items.Count > 0) { BtnTitleEdit.Enabled = true; BtnTitleDelete.Enabled = true; }
            else { BtnTitleEdit.Enabled = false; BtnTitleDelete.Enabled = false; }
        }
        private void BtnTitleNew_Click(object sender, EventArgs e)
        {
            ResetForm();
            EnableForm(true);
            //if (ListBoxTitle.Items.Count > 0) { ListBoxTitle.ClearSelected(); }
            TextBoxTltleJobTitle.Select();
        }

        private void BtnTitleDelete_Click(object sender, EventArgs e)
        {
            ToolStripStatusLabelErrorTitle.Text = "";
            Title TitleInfo = GetTitleInfo();
            if (TitleInfo != null)
            {
                DialogResult Result = MessageBox.Show(string.Format(DeleteTitleConfirmText, TitleInfo.Name), "Delete Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.Yes)
                {
                    if (TitleManager.DeleteTitle(TitleInfo.Id))
                    {
                        ResetForm();
                        LoadTitleWithFilter();
                        EnableForm(false);
                    }
                    else
                    {
                        ToolStripStatusLabelErrorTitle.Text = string.Format(DeleteNotAllowErrorText, TitleInfo.Name);

                    }
                }
            }
            else
            {
                DisplaySystemError(SelectedTitleNotValidErrorMsg);
                return;
            }
        }

        private void BtnTitleEdit_Click(object sender, EventArgs e)
        {
            ToolStripStatusLabelErrorTitle.Text = "";
            LoadTitleInfo();
            EnableForm(true);
            TextBoxTltleJobTitle.Select();
        }

        private void BtnTitleCancel_Click(object sender, EventArgs e)
        {
            ToolStripStatusLabelErrorTitle.Text = "";
            if (CreateTitleOnLoad)
            {
                this.Close();
            }
            else
            {
                ResetForm();
                LoadTitleInfo();
                EnableForm(false);
            }
        }


        private void BtnTitleSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Title TitleInfo = GetTitleFromForm();
                if (TitleInfo.Id == 0)
                {
                    Title lTitleName = TitleManager.CheckTitleNameInAdd(TitleInfo.Name, Global.Company.CompanyId);
                    if (lTitleName == null)
                    {
                        TitleManager.AddTitle(TitleInfo);
                        LoadTitleWithFilter();
                        ListBoxTitle.SelectedIndex = ListBoxTitle.FindStringExact(TitleInfo.Name);
                        EnableForm(false);
                        ToolStripStatusLabelErrorTitle.Text = SaveSuccess;
                        if (CreateTitleOnLoad)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        ToolStripStatusLabelErrorTitle.Text = string.Format(UniquetitleErrorMsg, TitleInfo.Name);
                        TextBoxTltleJobTitle.Select();
                        return;
                    }
                }
                else
                {
                    Title lTitleById = TitleManager.GetTitleInfoById(TitleInfo.Id);
                    if (lTitleById != null)
                    {
                        Title lTitleName = TitleManager.CheckTitleNameInUpdate(TitleInfo.Name, lTitleById.Id, Global.Company.CompanyId);
                        if (lTitleName == null)
                        {
                            Title lTitleFromDB = TitleManager.UpdateTitle(TitleInfo);
                            ResetForm();
                            LoadTitleWithFilter();
                            ListBoxTitle.SelectedIndex = ListBoxTitle.FindStringExact(lTitleFromDB.Name);
                            EnableForm(false);
                            ToolStripStatusLabelErrorTitle.Text = SaveSuccess;
                        }
                        else
                        {
                            ToolStripStatusLabelErrorTitle.Text = string.Format(UniquetitleErrorMsg, TitleInfo.Name);
                            TextBoxTltleJobTitle.Select();
                            return;
                        }
                    }
                    else
                    {
                        DisplaySystemError(SelectedTitleNotValidErrorMsg);
                        return;
                    }
                }
            }
        }

        private void BtnTitleExit_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
        }

        private Boolean ValidateForm()
        {
            ToolStripStatusLabelErrorTitle.Text = "";
            if (string.IsNullOrEmpty(TextBoxTltleJobTitle.Text.Trim()))
            {
                ToolStripStatusLabelErrorTitle.Text = EntertitleErrorMsg;
                TextBoxTltleJobTitle.Select();
                return false;
            }

            return true;
        }
        private void ResetForm()
        {
            ToolStripStatusLabelErrorTitle.Text = "";
            TextBoxTitleDescription.ResetText();
            TextBoxTitleDisplayAs.ResetText();
            TextBoxTitleId.ResetText();
            TextBoxTltleJobTitle.ResetText();
        }
        private void EnableForm(Boolean enable)
        {
            if (ListBoxTitle.Items.Count > 0)
            {
                TextBoxTitleSearch.ReadOnly = enable;
                TextBoxTitleSearch.TabStop = !enable;
                ListBoxTitle.Enabled = !enable;
            }
            else
            {
                ListBoxTitle.Enabled = false;
                TextBoxTitleSearch.ReadOnly = true;
                TextBoxTitleSearch.TabStop = false;
                BtnTitleNew.Select();
            }
            TextBoxTitleDescription.ReadOnly = !enable;
            TextBoxTitleDisplayAs.ReadOnly = !enable;
            TextBoxTltleJobTitle.ReadOnly = !enable;
            TextBoxTitleDescription.TabStop = enable;
            TextBoxTitleDisplayAs.TabStop = enable;
            TextBoxTltleJobTitle.TabStop = enable;

            if (!enable)
            {
                BtnTitleCancel.Enabled = enable;
                if (ListBoxTitle.SelectedIndex < 0)
                {
                    BtnTitleDelete.Enabled = enable;
                    BtnTitleEdit.Enabled = enable;
                }
                else
                {
                    BtnTitleDelete.Enabled = !enable;
                    BtnTitleEdit.Enabled = !enable;
                }
                BtnTitleNew.Enabled = !enable;
                BtnTitleSave.Enabled = enable;
            }
            else
            {
                BtnTitleNew.Enabled = !enable;
                BtnTitleDelete.Enabled = !enable;
                BtnTitleEdit.Enabled = !enable;
                BtnTitleSave.Enabled = enable;
                BtnTitleCancel.Enabled = enable;
            }
        }

        private void TextBoxTitleSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (ListBoxTitle.Items.Count > 1)
                {
                    ListBoxTitle.Select();
                    if ((ListBoxTitle.SelectedIndex + 1) == ListBoxTitle.Items.Count)
                    {
                        ListBoxTitle.SelectedIndex = 0;
                    }
                    else
                    {
                        ListBoxTitle.SelectedIndex = ListBoxTitle.SelectedIndex + 1;
                    }
                }
                else
                {
                    ListBoxTitle.Select();
                }
            }
        }

        private void TextBoxTitleSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }

        private void TextBoxTltleJobTitle_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }

        private void TextBoxTitleDisplayAs_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }

        private void TextBoxTltleJobTitle_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "NameChecking");
        }

        private void TextBoxTltleJobTitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxTltleJobTitle, "NameChecking");
            }
        }

        private void TextBoxTitleDisplayAs_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxTitleDisplayAs, "NameChecking");
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnTitleNew.PerformClick();
            }
            else if (keyData == (Keys.F4))
            {
                BtnTitleDelete.PerformClick();
            }
            else if (keyData == (Keys.F7))
            {
                BtnTitleEdit.PerformClick();
            }
            else if (keyData == (Keys.F8))
            {
                BtnTitleSave.PerformClick();
            }
            else if (keyData == (Keys.F10))
            {
                BtnTitleExit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnTitleCancel.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void TextBoxTitleDescription_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                BtnTitleSave.Select();
            }
        }

        private void BtnTitleSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxTltleJobTitle.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxTitleDescription.Select();
            }
        }

        private void TextBoxTltleJobTitle_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxTitleDisplayAs.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnTitleSave.Select();
            }
        }

        private void TextBoxTitleDescription_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
