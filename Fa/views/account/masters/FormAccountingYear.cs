using fa.api.Accounting;
using fa.libraries.utils;
using fa.model.Accounting.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.views.account.masters
{
    public partial class FormFiscalYear : FormBase
    {
        public long CompanyId = 0L;
        public static string SelectYearErrorMsg = "Please select year.";
        public static string SaveSuccessText = "Saved success...";
        public FormFiscalYear()
        {
            InitializeComponent();
        }
        private void FormFiscalYear_Load(object sender, EventArgs e)
        {
            ResetForm();
        }
        private void ResetForm()
        {
            ErrorMsg.Text = string.Empty;
            ComboUtils.InitializeFiscalYearCombo(ComboBoxFiscalYear, CompanyId);
        }
        private void BtnFiscalYearSave_Click(object sender, EventArgs e)
        {
            if(Validate())
            {
                Company Company = CompanyManager.Instance.GetCompanyForModel(CompanyId);
                if (Company != null)
                {
                    String Year = (string)ComboBoxFiscalYear.Items[ComboBoxFiscalYear.SelectedIndex];
                    string[] Years = Year.Split('-');
                    DateTime Start = DateTime.ParseExact(new DateTime(int.Parse(Years[0]), Company.AccountingStartDate, 1).ToString(Global.Company.DateFormat), Global.Company.DateFormat, null);
                    DateTime End = DateTime.ParseExact(new DateTime(int.Parse(Years[0]), Company.AccountingStartDate, 1).AddYears(1).AddDays(-1).ToString(Global.Company.DateFormat), Global.Company.DateFormat, null);
                    CompanyManager.GenerateIdSpace(CompanyId, Start, End);
                    ResetForm();
                    ErrorMsg.Text = SaveSuccessText;
                }
            }
        }
        private bool Validate()
        {
            ErrorMsg.Text = string.Empty;
            if (ComboBoxFiscalYear.SelectedIndex<0)
            {
                ComboBoxFiscalYear.Select();
                ErrorMsg.Text = SelectYearErrorMsg;
                return false;
            }
            return true;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnFiscalYearSave.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
