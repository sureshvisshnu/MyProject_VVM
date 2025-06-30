using fa.api.Accounting;
using fa.model.Accounting.Masters;
using fa.model.catalog;
using fa.views.utils.Report.Catalog;
using Fa.api.catalog;
using Fa.views.utils.Report.Catalog;
using FADataAccessLibrary.Model.Common;
using NPOI.OpenXmlFormats.Dml.Diagram;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.reports.catalog
{

    public partial class FormTaxCodeReport : Form
    {
        public FormTaxCodeReport()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void FormTaxCodeReport_Load(object sender, EventArgs e)
        {
            ResetGrid();
            ListTaxCode();
            EnableButtons(true);
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
        }
        private void ResetGrid()
        {
            int TaxCount = Global.Company.SalesTaxAccountMaps.Count;
            if (TaxCount > 0)
            {
                int i = 6;
                foreach (CompanySalesTaxAccountMap Map in Global.Company.SalesTaxAccountMaps)
                {
                    var Column = new DataGridViewTextBoxColumn();
                    Column.HeaderText = Map.Name.ToUpperInvariant();
                    Column.Name = "Column" + i;
                    Column.Width = 100;
                    Column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    Column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    Column.DefaultCellStyle.Format = "N2";
                    Column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                    i++;
                    TaxCodeGrid.Columns.AddRange(new DataGridViewColumn[] { Column });
                }
                TaxCodeGrid.Width = TaxCodeGrid.Width + (100 * TaxCount);
                this.Width = this.Width + (100 * TaxCount);
                BtnExit.Location = new Point(this.Width - 100, 560);
                BtnPrint.Location = new Point(this.Width - 180, 560);
                BtnSave.Location = new Point(this.Width - 260, 560);
            }
        }
        private void ListTaxCode()
        {
            TaxCodeGrid.Rows.Clear();
            IList<ItemTax> ItemTaxInfo = new List<ItemTax>();
            ItemTaxInfo = ItemTaxManager.Instance.GetItemTaxs(Global.Company.CompanyId);
            if (ItemTaxInfo.Count > 0)
            {
                int i = 0;

                TaxCodeGrid.Rows.Add(ItemTaxInfo.Count);
                foreach (ItemTax Tax in ItemTaxInfo)
                {
                    TaxCodeGrid.Rows[i].Cells[0].Value = i + 1;
                    TaxCodeGrid.Rows[i].Cells[1].Value = Tax.Code;
                    TaxCodeGrid.Rows[i].Cells[2].Value = Tax.Description;

                    int j = 1;
                    int taxcoloumn = 5;
                    try
                    {
                        foreach (ItemSalesTaxMap TaxMap in Tax.SalesTaxMapLocal)
                        {
                            if (j == 1)
                            {
                                TaxCodeGrid.Rows[i].Cells[3].Value = TaxMap.EffectiveFromDate.ToString(Global.Company.DateFormat);
                                TaxCodeGrid.Rows[i].Cells[4].Value = TaxMap.EffectiveToDate.ToString(Global.Company.DateFormat);
                            }
                            if (j > Global.Company.SalesTaxAccountMaps.Count)
                            {
                                TaxCodeGrid.Rows.Add(1);
                                j = 1;
                                taxcoloumn = 5;
                                i++;
                                TaxCodeGrid.Rows[i].Cells[0].Value = i + 1;
                            }
                            else
                            {
                                TaxCodeGrid.Rows[i].Cells[taxcoloumn].Value = Math.Round(TaxMap.TaxPercentage, 2);
                                j++;
                                taxcoloumn++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    TaxCodeGrid.Rows[i].MinimumHeight = TaxCodeGrid.Rows[i].Height;
                    i++;
                }
            }

        }
        private void EnableButtons(bool Enable)
        {
            BtnExit.Enabled = Enable;
            if (TaxCodeGrid.Rows.Count > 0)
            {
                BtnSave.Enabled = Enable;
                BtnPrint.Enabled = Enable;
            }
            else
            {
                BtnSave.Enabled = !Enable;
                BtnPrint.Enabled = !Enable;
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TaxCodeGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                e.CellStyle.BackColor = Color.White;
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.SelectionBackColor = Color.White;
                e.CellStyle.SelectionForeColor = Color.Black;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSave.PerformClick();
            }
            if (keyData == (Keys.F9))
            {
                BtnPrint.PerformClick();
            }
            if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SavePrintTaxCodeReport SavePrintTaxCodeReport = new SavePrintTaxCodeReport();
            SavePrintTaxCodeReport.ExportToFileOrPrint(TaxCodeGrid, "Tax Code report", "Tax Code Report", "pdf", false);
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            this.UseWaitCursor = true;
            BtnPrint.Invoke(new Action(() => BtnPrint.Enabled = false));
            BtnSave.Invoke(new Action(() => BtnSave.Enabled = false));
            BtnExit.Invoke(new Action(() => BtnExit.Enabled = false));
            if (backgroundWorker1.IsBusy != true)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            SavePrintTaxCodeReport SavePrintTaxCodeReport = new SavePrintTaxCodeReport();
            SavePrintTaxCodeReport.ExportToFileOrPrint(TaxCodeGrid, "Tax Code Report", "Tax Code Report", "pdf", true);
            this.UseWaitCursor = false;
            if (BtnPrint.InvokeRequired) BtnPrint.Invoke(new Action(() => BtnPrint.Enabled = true));
            if (BtnSave.InvokeRequired) BtnSave.Invoke(new Action(() => BtnSave.Enabled = true));
            if (BtnExit.InvokeRequired) BtnExit.Invoke(new Action(() => BtnExit.Enabled = true));
            BtnPrint.Select();
        }
    }
}
