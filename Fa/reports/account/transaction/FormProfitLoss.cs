using fa;
using fa.libraries.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fa.reports.account.transaction
{
    enum ProfitLossGridColumn
    {
        DESCRIPTION, AMOUNT
    }
    public partial class FormProfitLoss : Form
    {
        public FormProfitLoss()
        {
            InitializeComponent();
        }

        private void FormProfitLoss_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ComboUtils.InitializeCostCenterComboByUserAccess(ComboBoxCostCenter, (long)Global.Company.CompanyId);
                ResetForm();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnRunReport_Click(object sender, EventArgs e)
        {

        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            Cursor.Current = Cursors.Default;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ResetForm()
        {
            ProfitLossGrid.Rows.Clear();
            ErrorMsg.Text = "";
            EnableButton(false);
            if (ComboBoxCostCenter.Items.Count > 1)
            {
                LabelCostCenter.Visible = true;
                ComboBoxCostCenter.Visible = true;
                ComboBoxCostCenter.SelectedIndex = 0;
            }
            else
            {
                LabelCostCenter.Visible = false;
                ComboBoxCostCenter.Visible = false;
            }

            FromDate.Format = Global.Company.DateFormat;
            FromDate.Date = Global.getTransactionDate().AddDays(-30);
            ToDate.Format = Global.Company.DateFormat;
            ToDate.Date = Global.getTransactionDate();
        }
        private void EnableButton(bool Enable)
        {
            BtnPrint.Enabled = Enable;
            BtnSave.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
            ToolStripBtnSave.Enabled = Enable;
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {

        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                BtnReset.PerformClick();
                return true;
            }
            if (keyData == Keys.F8)
            {
                BtnSave.PerformClick();
                return true;
            }
            if (keyData == Keys.F9)
            {
                BtnPrint.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
