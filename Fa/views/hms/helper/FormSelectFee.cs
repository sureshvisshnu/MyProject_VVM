using fa;
using fa.api.Hms;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.Hms.common;
using fa.model.Hms.Master;
using fa.model.OrderManagement;
using fa.views;
using fa.views.controls.grid;
using fa.views.hms.Masters;
using fa.views.sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fa.views.hms.helper
{
    public enum FeeGridColumn
    {
        NAME, DESC, AMOUNT, ID
    }
    public partial class FormSelectFee : FormBase
    {
        public string SearchText;
        public static string ChooseFeeErrorMsg = "Please select fee.";
        public static string DeletedFeeErrorMsg = "Your Select fee is remove, Please press go button then select fee.";
        FormLedgerManageLineItems parent = null;
        public FormSelectFee(Object sender)
        {
            parent = (FormLedgerManageLineItems)sender;
            InitializeComponent();
        }

        private void FormSelectGeneratedInvoice_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            TextBoxFeeSearch.Text = SearchText;
            LoadFee();
            GridViewFee.Select();
            Cursor.Current = Cursors.Default;
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)GridViewFee.Columns["Amount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
        }
        private void BtnFeeSearch_Click(object sender, EventArgs e)
        {
            LoadFee();
        }
        public void LoadFee()
        {
            GridViewFee.Rows.Clear();
            IList<Consultation> Consultation = ConsultationManager.Instance.ListConsultationByCompanyId(Global.Company.CompanyId);
            if (Consultation != null && Consultation.Count > 0)
            {
                int i = 0;
                foreach (var FeeInfo in Consultation)
                {
                    if (string.IsNullOrEmpty(TextBoxFeeSearch.Text.Trim()) || FeeInfo.Name.IndexOf(TextBoxFeeSearch.Text.Trim(), StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        GridViewFee.Rows.Add();
                        GridViewFee.Rows[i].Cells[(int)FeeGridColumn.NAME].Value = FeeInfo.Name;
                        GridViewFee.Rows[i].Cells[(int)FeeGridColumn.DESC].Value = FeeInfo.Discription;
                        GridViewFee.Rows[i].Cells[(int)FeeGridColumn.AMOUNT].Value = FeeInfo.Fee.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewFee.Rows[i].Cells[(int)FeeGridColumn.ID].Value = FeeInfo.Id;
                        i++;
                    }
                }
            }
        }
        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (GridViewFee.Rows.Count > 0)
            {
                if (GridViewFee.CurrentRow.Index > -1)
                {
                    parent.FeeId = (long)GridViewFee.CurrentRow.Cells[(int)FeeGridColumn.ID].Value;
                    this.Close();
                }
                else
                {
                    RecentInvoiceErrorMsg.Text = ChooseFeeErrorMsg;
                }
            }
        }
        private void GridViewRecentInvoice_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                BtnSelect_Click(sender, e);
            }
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void GridViewRecentInvoice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == Convert.ToChar(Keys.Enter))
            {
                BtnSelect_Click(sender, e);
            }
        }
        private void BtnSelect_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                if (GridViewFee.Rows.Count > 0)
                {
                    GridViewFee.Focus();
                    GridViewFee.CurrentCell = GridViewFee[0, 0];
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewFee.Rows.Count > 0)
                {
                    GridViewFee.Focus();
                    GridViewFee.CurrentCell = GridViewFee[0, GridViewFee.Rows.Count - 1];
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSelect.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                return true;
            }
            try
            {
                if (keyData == (Keys.Tab) && GridViewFee.CurrentRow.Index > -1)
                {
                    if (GridViewFee.CurrentCell.RowIndex != GridViewFee.Rows.Count - 1)
                    {
                        GridViewFee.CurrentCell = GridViewFee[0, GridViewFee.CurrentCell.RowIndex + 1];
                    }
                    else
                    {
                        BtnSelect.Select();
                    }
                }
                if (keyData == (Keys.Tab | Keys.Shift) && GridViewFee.CurrentRow.Index > -1)
                {
                    if (GridViewFee.CurrentRow.Index != 0)
                    {
                        GridViewFee.CurrentCell = GridViewFee[0, GridViewFee.CurrentCell.RowIndex];
                    }
                    else
                    {
                        BtnSelect.Select();
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


    }
}
