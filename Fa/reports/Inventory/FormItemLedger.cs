using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using fa.api.catalog;
using fa.api.Hms;
using fa.api.utils;
using fa.libraries.utils;
using fa.model.Catalog;
using fa.model.Common;
using fa.model.OrderManagement;
using fa.report.Inventory;
using fa.views.controls;
using fa.views.controls.ComboListView;
using fa.views.controls.ComboTreeView;
using fa.views.utils.Report.Inventory;
using Fa.reports.sales;
using NPOI.POIFS.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static fa.views.controls.ComboListView.CheckedComboBox;

namespace fa.reports.Inventory
{
    enum ItemLedgerTableColumn
    {
        DATE, TRANSTYPE, FROM, TO, RACKNO, DESCRIPTION, BATCH, EXPDATE, UOM, QTY, PRICE, VALUE, STOCK, PRODUCT_NAME, LID
    }

    public partial class FormItemLedger : Form
    {
        RptItemLedger RptItemLedger = null!;

        public static string ItemLedgerValidItemErrorMsg = "Please select atleast one Item.";
        public static string ItemLedgerNoRecordErrorMsg = "No Record Found..";
        public static string EnterValidDateErrorMsg = "Please enter current date to future date.";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";
        public FormItemLedger()
        {
            InitializeComponent();
            //CheckedComboBoxItem.CheckedItemsChanged += CheckedComboBoxItem_CheckedItemsChanged!;
        }
        private void FormItemLedger_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            LoadCombo();
            ResetForm();
            CheckedComboBoxItem.Select();
            //CheckedComboBoxItem.CheckedItemsChanged += (s, e) => DisplayCheckedInformation();
            Cursor.Current = Cursors.Default;
        }
        private void LoadCombo()
        {
            List<Product> lProduct = CatalogProductManager.Instance.ListProductByCompanyId(Global.Company.CompanyId);
            if (lProduct != null && lProduct.Count > 0)
            {
                //Parallel.ForEach(lProduct, new ParallelOptions { MaxDegreeOfParallelism = 2 }, lProducts =>
                //{
                //    ComboTreeNode parent = new ComboTreeNode();
                //    //CheckedTreeComboBoxProduct.Add(p.Name, p.Id);
                //    parent.Name = lProducts.Id.ToString();
                //    parent.Text = lProducts.Name + " " + "(" + lProducts.MaterialId + ")";
                //    CheckedTreeComboBoxProduct.TreeNodes = parent;
                //});
                foreach (Product p in lProduct.ToList())
                {
                    CheckedComboBoxItem.Add(p.Name + " " + "(" + p.MaterialId + ")", p.Id);
                    //CheckedComboBoxItem.Add(p.Name + " " + "(" + p.MaterialId + ")", p.Id);
                    //ComboTreeNode parent = new ComboTreeNode();
                    //parent.Name = p.Id.ToString();
                    //parent.Text = p.Name + " " + "(" + p.MaterialId + ")";
                    //CheckedTreeComboBoxProduct.TreeNodes = parent;
                }
            }
            ComboUtils.InitializeStockLocationCombo(CheckedTreeComboLocation, Global.Company.CompanyId);
            // ComboUtils.InitializeProductCombo(CheckedTreeComboBoxProduct, Global.Company.CompanyId);           
        }
        private void ResetForm()
        {
            this.Text = "Item Ledger (Stock)";
            GridViewItemLedger.Rows.Clear();
            if (!Global.Company.MaintainRackNumber)
            {
                GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.RACKNO].Visible = false;
            }
            else
            {
                GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.RACKNO].Visible = true;
            }
            ErrorMsgItemLedger.Text = "";
            EnableButtons(false);
            CheckedComboBoxItem.Reset();
            CheckedComboBoxItem.Text = string.Empty;
            CheckBoxLabeledBatchWise.Checked = false;
            CheckBoxAddValue.Checked = false;
            if (!CheckBoxAddValue.Checked && !CheckBoxLabeledBatchWise.Checked)
            {
                if (Global.Company.MaintainRackNumber)
                {
                    GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.DATE].Width = 105;
                    GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.TRANSTYPE].Width = 180;
                    GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.FROM].Width = 180;
                    GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.TO].Width = 180;
                    GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.DESCRIPTION].Width = 180;
                    GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.QTY].Width = 110;
                    GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.STOCK].Width = 110;
                    GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.RACKNO].Width = 150;
                }
                else
                {
                    GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.DATE].Width = 150;
                    GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.TRANSTYPE].Width = 195;
                    GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.FROM].Width = 210;
                    GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.TO].Width = 210;
                    GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.DESCRIPTION].Width = 210;
                    GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.QTY].Width = 110;
                    GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.STOCK].Width = 110;
                }
            }
            if (!CheckBoxAddValue.Checked)
            {
                GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.PRICE].Visible = false;
                GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.VALUE].Visible = false;

            }
            if (!CheckBoxLabeledBatchWise.Checked)
            {
                GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.BATCH].Visible = false;
                GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.EXPDATE].Visible = false;
            }
            CheckedTreeComboLocation.SelectedNode = null;
            foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboLocation.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            FromDate.Format = Global.Company.DateFormat;
            FromDate.Date = Global.getTransactionDate().AddDays(-30);
            ToDate.Format = Global.Company.DateFormat;
            TimeSpan time = new TimeSpan(24, 00, 00);
            ToDate.Date = Global.getTransactionDate();
        }
        private void EnableButtons(bool Enable)
        {
            BtnToolStripSave.Enabled = Enable;
            BtnToolStripPrint.Enabled = Enable;
            BtnSave.Enabled = Enable;
            BtnPrint.Enabled = Enable;
        }
        private void BtnGo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ErrorMsgItemLedger.Text = "";
                GridViewItemLedger.Rows.Clear();
                EnableButtons(false);
                if (FormValidate())
                {
                    LoadItemLedger();
                }
            }
            catch (Exception ex)
            {
                ErrorMsgItemLedger.Text = "Errod fetching ledger (Error:" + ex.InnerException.Message + ")";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private string SelectedNodesText(ToolstripCheckedTreeComboBox ComboTreeBox)
        {
            int i = 0;
            string Name = string.Empty;
            if (ComboTreeBox.Nodes.Count > 0)
            {
                foreach (ComboTreeNode ComboTreeNode in ComboTreeBox.Nodes)
                {
                    if (ComboTreeNode != null)
                    {
                        if (ComboTreeNode.Checked == true)
                        {
                            if (ComboTreeNode.Name == "All")
                            {
                                Name = string.Empty;
                                Name = "All Location";
                                break;
                            }
                            else
                            {
                                Name += string.IsNullOrEmpty(Name) ? ComboTreeNode.Text : (", " + ComboTreeNode.Text);
                            }
                            i++;
                        }
                    }
                }
            }
            return Name;
        }
        private void LoadItemLedger()
        {
            DisplayCheckedInformation();
            RptItemLedger = new RptItemLedger();
            RptItemLedger.FromDate = (DateTime)FromDate.Date!;
            RptItemLedger.ToDate = (DateTime)ToDate.Date!;
            //RptItemLedger.ReportHeader = CheckedComboBoxItem.GetCheckedItemsString + " @ " + SelectedNodesText(CheckedTreeComboLocation);
            RptItemLedger.Company = Global.Company;
            RptItemLedger.ItemIds = CheckedTreeUtils.SelectedItems(CheckedComboBoxItem).ToArray();
            RptItemLedger.LocationIds = CheckedTreeUtils.SelectedNodes(CheckedTreeComboLocation).ToArray();
            RptItemLedger.LabeledBatchWise = CheckBoxLabeledBatchWise.Checked;
            RptItemLedger.IsShowValue = CheckBoxAddValue.Checked;
            RptItemLedger.IsAllLocation = false;
            RptItemLedger.GenerateReport();
            string NewLoctn = string.Empty;
            if (RptItemLedger.ItemLedger != null && RptItemLedger.ItemLedger.Count > 0)
            {
                Currency Currency = api.Accounting.CurrencyManager.Instance.GetCurrencyById((long)Global.Company.PrimaryCurrencyId);
                GridViewItemLedger.Rows.Clear();
                int i = 0;
                int j = 2;
                Color[] RowColor = new Color[2];
                RowColor[0] = Color.White;
                RowColor[1] = Color.WhiteSmoke;
                DateTime? lDate = null;
                string ItemName = "";
                bool NoOpeningStock = true;
                double ClosingStock = 0.00;
                int row = -1;
                if (RptItemLedger.IsAllLocation)
                {
                    EnableButtons(true);
                    ClosingStock = RptItemLedger.ItemLedger.Sum(x => x.OpeningStock);
                    row = GridViewItemLedger.Rows.Add();
                    GridViewItemLedger.Rows[row].DefaultCellStyle.BackColor = RowColor[j % 2];
                    GridViewItemLedger.Rows[row].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                    j++;
                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.DATE].Value = DateUtils.FormatDate((DateTime)FromDate.Date, Global.Company.DateFormat);
                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.TRANSTYPE].Value = "Opening Stock";
                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.QTY].Value = Math.Abs(RptItemLedger.ItemLedger.Sum(x => x.OpeningStock)).ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.STOCK].Value = Math.Abs(RptItemLedger.ItemLedger.Sum(x => x.OpeningStock)).ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                }
                foreach (ItemLedger Ledger in RptItemLedger.ItemLedger.OrderBy(x => x.Location.Id))
                {
                    if (CheckBoxAddValue != null && CheckBoxAddValue.Checked)
                    {
                        GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.PRICE].Visible = true;
                        GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.VALUE].Visible = true;
                    }
                    else
                    {
                        GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.PRICE].Visible = false;
                        GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.VALUE].Visible = false;
                    }
                    if (CheckBoxLabeledBatchWise.Checked)
                    {
                        GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.BATCH].Visible = true;
                        GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.EXPDATE].Visible = true;
                    }
                    else
                    {
                        GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.BATCH].Visible = false;
                        GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.EXPDATE].Visible = false;
                    }
                    if (CheckBoxLabeledBatchWise.Checked && CheckBoxAddValue != null && CheckBoxAddValue.Checked)
                    {
                        if (Global.Company.MaintainRackNumber)
                        {
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.DATE].Width = 100;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.TRANSTYPE].Width = 130;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.FROM].Width = 130;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.TO].Width = 130;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.DESCRIPTION].Width = 180;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.QTY].Width = 75;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.STOCK].Width = 75;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.BATCH].Width = 75;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.EXPDATE].Width = 75;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.PRICE].Width = 75;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.VALUE].Width = 75;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.RACKNO].Width = 75;
                        }
                        else
                        {
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.DATE].Width = 85;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.TRANSTYPE].Width = 140;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.FROM].Width = 140;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.TO].Width = 140;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.DESCRIPTION].Width = 180;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.QTY].Width = 70;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.STOCK].Width = 70;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.BATCH].Width = 125;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.EXPDATE].Width = 85;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.PRICE].Width = 80;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.VALUE].Width = 80;
                        }
                    }
                    if (!CheckBoxLabeledBatchWise.Checked && CheckBoxAddValue != null && !CheckBoxAddValue.Checked)
                    {
                        if (Global.Company.MaintainRackNumber)
                        {
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.DATE].Width = 105;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.TRANSTYPE].Width = 180;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.FROM].Width = 180;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.TO].Width = 180;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.DESCRIPTION].Width = 180;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.QTY].Width = 110;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.STOCK].Width = 110;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.RACKNO].Width = 150;
                        }
                        else
                        {
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.DATE].Width = 150;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.TRANSTYPE].Width = 195;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.FROM].Width = 210;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.TO].Width = 210;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.DESCRIPTION].Width = 210;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.QTY].Width = 110;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.STOCK].Width = 110;
                        }
                    }
                    if (!CheckBoxLabeledBatchWise.Checked && CheckBoxAddValue != null && CheckBoxAddValue.Checked)
                    {
                        if (Global.Company.MaintainRackNumber)
                        {
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.DATE].Width = 100;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.TRANSTYPE].Width = 170;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.FROM].Width = 150;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.TO].Width = 150;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.DESCRIPTION].Width = 190;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.QTY].Width = 80;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.STOCK].Width = 85;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.PRICE].Width = 80;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.VALUE].Width = 90;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.RACKNO].Width = 100;
                        }
                        else
                        {
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.DATE].Width = 110;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.TRANSTYPE].Width = 165;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.FROM].Width = 180;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.TO].Width = 180;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.DESCRIPTION].Width = 200;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.QTY].Width = 90;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.STOCK].Width = 90;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.PRICE].Width = 90;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.VALUE].Width = 90;
                        }
                    }
                    if (CheckBoxLabeledBatchWise.Checked && CheckBoxAddValue != null && !CheckBoxAddValue.Checked)
                    {
                        if (Global.Company.MaintainRackNumber)
                        {
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.DATE].Width = 100;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.TRANSTYPE].Width = 155;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.FROM].Width = 155;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.TO].Width = 155;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.DESCRIPTION].Width = 180;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.QTY].Width = 95;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.STOCK].Width = 85;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.BATCH].Width = 95;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.EXPDATE].Width = 95;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.RACKNO].Width = 80;
                        }
                        else
                        {
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.DATE].Width = 100;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.TRANSTYPE].Width = 165;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.FROM].Width = 165;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.TO].Width = 165;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.DESCRIPTION].Width = 200;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.QTY].Width = 90;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.STOCK].Width = 95;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.BATCH].Width = 115;
                            GridViewItemLedger.Columns[(int)ItemLedgerTableColumn.EXPDATE].Width = 100;
                        }
                    }
                    string Loctn = Ledger.Location.ToString();
                    double price = Ledger.CostPrice;

                    if (Ledger.LineItems.Count > 0 || Ledger.OpeningStock > 0)
                    {
                        NoOpeningStock = false;

                        if (!RptItemLedger.IsAllLocation)
                        {
                            row = -1;
                            if (RptItemLedger.LabeledBatchWise && Ledger.OpeningStockBatchWise.Count > 0)
                            {
                                EnableButtons(true);
                                if (Loctn.ToString() != NewLoctn.ToString())
                                {
                                    row = GridViewItemLedger.Rows.Add();
                                    GridViewItemLedger.Rows[row].DefaultCellStyle.BackColor = Color.LightGray;
                                    GridViewItemLedger.Rows[row].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.PRODUCT_NAME].Value = Ledger.Location;
                                    row = GridViewItemLedger.Rows.Add();
                                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.PRODUCT_NAME].Value = Ledger.ItemName;
                                    j++;
                                    NewLoctn = Ledger.Location.ToString();
                                    ItemName = Ledger.ItemName;
                                }
                                if (Loctn.ToString() == NewLoctn.ToString() && ItemName != Ledger.ItemName)
                                {
                                    row = GridViewItemLedger.Rows.Add();
                                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.PRODUCT_NAME].Value = Ledger.ItemName;
                                }
                                double BatchwiseOpeningStock = Ledger.OpeningStockBatchWise.Sum(x => x.OpeningStock);

                                if (BatchwiseOpeningStock == 0)
                                {
                                    row = GridViewItemLedger.Rows.Add();
                                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.PRODUCT_NAME].Value = Ledger.ItemName;
                                    row = GridViewItemLedger.Rows.Add();
                                    GridViewItemLedger.Rows[row].DefaultCellStyle.BackColor = RowColor[j % 2];
                                    GridViewItemLedger.Rows[row].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                                    j++;
                                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.DATE].Value = DateUtils.FormatDate((DateTime)FromDate.Date, Global.Company.DateFormat);
                                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.TRANSTYPE].Value = "Opening Stock";
                                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.QTY].Value = BatchwiseOpeningStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.STOCK].Value = BatchwiseOpeningStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                                    if (CheckBoxAddValue != null && CheckBoxAddValue.Checked)
                                    {
                                        // calculate value
                                        GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.PRICE].Value = price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                        GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.VALUE].Value = RptItemLedger.CalculateValue(double.Parse(BatchwiseOpeningStock.ToString()), price).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                    }

                                }
                                BatchwiseOpeningStock = 0;
                                string Type = "Opening Stock";
                                string Date = DateUtils.FormatDate((DateTime)FromDate.Date, Global.Company.DateFormat);
                                foreach (BatchWiseOpeningStock BatchWiseOpeningStock in Ledger.OpeningStockBatchWise)
                                {
                                    if (BatchWiseOpeningStock.OpeningStock > 0)
                                    {
                                        row = GridViewItemLedger.Rows.Add();
                                        GridViewItemLedger.Rows[row].DefaultCellStyle.BackColor = RowColor[j % 2];
                                        GridViewItemLedger.Rows[row].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                                        j++;
                                        GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.BATCH].Value = BatchWiseOpeningStock.BatchNo;
                                        GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.EXPDATE].Value = DateUtils.FormatDate(BatchWiseOpeningStock.ExpDate, Global.Company.DateFormat);
                                        GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.DATE].Value = Date;
                                        GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.TRANSTYPE].Value = Type;
                                        GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.UOM].Value = BatchWiseOpeningStock.Uom.ToLower();
                                        GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.QTY].Value = BatchWiseOpeningStock.OpeningStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                                        BatchwiseOpeningStock = RptItemLedger.LoadClosingStock(BatchwiseOpeningStock, BatchWiseOpeningStock.OpeningStock, "Opening Stock");
                                        GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.STOCK].Value = BatchwiseOpeningStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                                        if (CheckBoxAddValue != null && CheckBoxAddValue.Checked)
                                        {
                                            //calculate value
                                            GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.PRICE].Value = price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                            GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.VALUE].Value = RptItemLedger.CalculateValue(BatchWiseOpeningStock.OpeningStock, price).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                        }
                                        Type = string.Empty;
                                        Date = string.Empty;
                                    }
                                }
                            }
                            else
                            {
                                EnableButtons(true);
                                j++;
                                if (Loctn.ToString() != NewLoctn.ToString())
                                {
                                    row = GridViewItemLedger.Rows.Add();
                                    GridViewItemLedger.Rows[row].DefaultCellStyle.BackColor = Color.LightGray;
                                    GridViewItemLedger.Rows[row].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.PRODUCT_NAME].Value = Ledger.Location;
                                    NewLoctn = Ledger.Location.ToString();
                                }
                                row = GridViewItemLedger.Rows.Add();
                                GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.PRODUCT_NAME].Value = Ledger.ItemName;
                                row = GridViewItemLedger.Rows.Add();
                                GridViewItemLedger.Rows[row].DefaultCellStyle.BackColor = RowColor[j % 2];
                                GridViewItemLedger.Rows[row].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                                j++;
                                GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.DATE].Value = DateUtils.FormatDate((DateTime)FromDate.Date, Global.Company.DateFormat);
                                GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.TRANSTYPE].Value = "Opening Stock";
                                GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.UOM].Value = Ledger.Uom.ToLower();
                                GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.QTY].Value = Math.Abs(Ledger.OpeningStock).ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                                GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.STOCK].Value = Math.Abs(Ledger.OpeningStock).ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                                if (CheckBoxAddValue != null && CheckBoxAddValue.Checked)
                                {
                                    // calculate value
                                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.PRICE].Value = price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.VALUE].Value = RptItemLedger.CalculateValue(Ledger.OpeningStock, price).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                }
                            }
                        }

                        string BatchNo = string.Empty;
                        lDate = RptItemLedger.IsAllLocation ? lDate : null;
                        ClosingStock = RptItemLedger.IsAllLocation ? ClosingStock : Ledger.OpeningStock;
                        foreach (ItemLedgerLineItem LineItem in Ledger.LineItems.OrderBy(x => x.Date))
                        {
                            EnableButtons(true);
                            row = GridViewItemLedger.Rows.Add();
                            GridViewItemLedger.Rows[row].DefaultCellStyle.BackColor = RowColor[j % 2];
                            GridViewItemLedger.Rows[row].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            j++;
                            if (lDate != LineItem.Date.Date)
                            {
                                GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.DATE].Value = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                                lDate = LineItem.Date.Date;
                            }
                            GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.DESCRIPTION].Value = LineItem.Description;
                            GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.BATCH].Value = LineItem.BatchDetail;
                            GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.UOM].Value = LineItem.Uom.ToLower();
                            GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.EXPDATE].Value = string.IsNullOrEmpty(LineItem.BatchDetail) ? "" : LineItem.ExpDate.ToString(Global.Company.DateFormat);
                            GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.QTY].Value = RptItemLedger.Sign(LineItem.Qty, LineItem.TransactionType, Global.Company.QuantityPricision);
                            GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.TRANSTYPE].Value = (string.IsNullOrEmpty(LineItem.FromLocation) && string.IsNullOrEmpty(LineItem.ToLocation)) ? null : LineItem.TransactionType;
                            GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.FROM].Value = LineItem.FromLocation;
                            GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.TO].Value = LineItem.ToLocation;
                            GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.RACKNO].Value = LineItem.RackNumber;
                            ClosingStock = RptItemLedger.LoadClosingStock(ClosingStock, LineItem.Qty, LineItem.TransactionType);
                            GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.STOCK].Value = RptItemLedger.Sign(ClosingStock, Global.Company.QuantityPricision);
                            if (CheckBoxAddValue != null && CheckBoxAddValue.Checked)
                            {
                                // calculate value                                
                                GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.PRICE].Value = price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.VALUE].Value = RptItemLedger.CalculateValue(LineItem.Qty, price).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            }

                        }
                        if (!RptItemLedger.IsAllLocation)
                        {
                            if (RptItemLedger.LabeledBatchWise && Ledger.OpeningStockBatchWise.Count > 0)
                            {
                                double BatchwiseClosingStock = Ledger.OpeningStockBatchWise.Sum(x => x.ClosingStock);
                                if (BatchwiseClosingStock == 0)
                                {
                                    row = GridViewItemLedger.Rows.Add();
                                    GridViewItemLedger.Rows[row].DefaultCellStyle.BackColor = RowColor[j % 2];
                                    GridViewItemLedger.Rows[row].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                                    j++;
                                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.DATE].Value = DateUtils.FormatDate((DateTime)ToDate.Date, Global.Company.DateFormat);
                                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.TRANSTYPE].Value = "Closing Stock";
                                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.QTY].Value = BatchwiseClosingStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.STOCK].Value = BatchwiseClosingStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                                    if (CheckBoxAddValue != null && CheckBoxAddValue.Checked)
                                    {
                                        // calculate value
                                        GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.PRICE].Value = price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                        GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.VALUE].Value = RptItemLedger.CalculateValue(double.Parse(BatchwiseClosingStock.ToString()), price).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                    }

                                }
                                BatchwiseClosingStock = 0;
                                string Type = "Closing Stock";
                                string Date = DateUtils.FormatDate((DateTime)ToDate.Date, Global.Company.DateFormat);
                                foreach (BatchWiseOpeningStock BatchWiseOpeningStock in Ledger.OpeningStockBatchWise)
                                {
                                    if (BatchWiseOpeningStock.ClosingStock > 0)
                                    {
                                        row = GridViewItemLedger.Rows.Add();
                                        GridViewItemLedger.Rows[row].DefaultCellStyle.BackColor = RowColor[j % 2];
                                        GridViewItemLedger.Rows[row].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                                        j++;
                                        GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.BATCH].Value = BatchWiseOpeningStock.BatchNo + " " + DateUtils.FormatDate(BatchWiseOpeningStock.ExpDate, Global.Company.DateFormat);
                                        GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.DATE].Value = Date;
                                        GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.TRANSTYPE].Value = Type;
                                        GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.UOM].Value = BatchWiseOpeningStock.Uom.ToLower();
                                        GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.QTY].Value = RptItemLedger.Sign(BatchWiseOpeningStock.ClosingStock, Global.Company.QuantityPricision);
                                        GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.STOCK].Value = RptItemLedger.Sign(ClosingStock, Global.Company.QuantityPricision);
                                        if (CheckBoxAddValue != null && CheckBoxAddValue.Checked)
                                        {
                                            // calculate
                                            GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.PRICE].Value = price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                            GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.VALUE].Value = RptItemLedger.CalculateValue(BatchWiseOpeningStock.ClosingStock, price).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                        }
                                        Type = string.Empty;
                                        Date = string.Empty;
                                    }
                                }
                            }
                            else
                            {
                                row = GridViewItemLedger.Rows.Add();
                                GridViewItemLedger.Rows[row].DefaultCellStyle.BackColor = RowColor[j % 2];
                                GridViewItemLedger.Rows[row].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                                j++;
                                GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.DATE].Value = DateUtils.FormatDate((DateTime)ToDate.Date, Global.Company.DateFormat);
                                GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.TRANSTYPE].Value = "Closing Stock";
                                GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.UOM].Value = Ledger.Uom.ToLower();
                                GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.QTY].Value = RptItemLedger.Sign(ClosingStock, Global.Company.QuantityPricision);
                                GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.STOCK].Value = RptItemLedger.Sign(ClosingStock, Global.Company.QuantityPricision);
                                if (CheckBoxAddValue != null && CheckBoxAddValue.Checked)
                                {
                                    // calculate value
                                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.PRICE].Value = price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.VALUE].Value = RptItemLedger.CalculateValue(ClosingStock, price).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                }
                            }
                        }

                        i++;
                    }
                }
                if (RptItemLedger.IsAllLocation)
                {
                    row = GridViewItemLedger.Rows.Add();
                    GridViewItemLedger.Rows[row].DefaultCellStyle.BackColor = RowColor[j % 2];
                    GridViewItemLedger.Rows[row].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                    j++;
                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.DATE].Value = DateUtils.FormatDate((DateTime)ToDate.Date, Global.Company.DateFormat);
                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.TRANSTYPE].Value = "Closing Stock";
                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.QTY].Value = RptItemLedger.Sign(ClosingStock, Global.Company.QuantityPricision);
                    GridViewItemLedger.Rows[row].Cells[(int)ItemLedgerTableColumn.STOCK].Value = RptItemLedger.Sign(ClosingStock, Global.Company.QuantityPricision);
                }

                if (NoOpeningStock)
                {
                    ErrorMsgItemLedger.Text = ItemLedgerNoRecordErrorMsg;
                }

            }
            else
            {
                ErrorMsgItemLedger.Text = ItemLedgerNoRecordErrorMsg;
            }
        }
        private bool FormValidate()
        {
            ErrorMsgItemLedger.Text = "";

            if (CheckedComboBoxItem.CheckedItems.Count < 1)
            {
                ErrorMsgItemLedger.Text = ItemLedgerValidItemErrorMsg;
                CheckedComboBoxItem.Focus();
                return false;
            }
            if (CheckedTreeUtils.SelectedNodes(CheckedTreeComboLocation).Count < 1)
            {
                ErrorMsgItemLedger.Text = "Please select location";
                CheckedTreeComboLocation.Focus();
                return false;
            }

            if (FromDate.Date == null || !DateUtils.ValidDate(((DateTime)FromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsgItemLedger.Text = EnterValidDateErrorMsg;
                FromDate.Focus();
                return false;
            }
            if (ToDate.Date == null || !DateUtils.ValidDate(((DateTime)ToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsgItemLedger.Text = EnterValidDateErrorMsg;
                ToDate.Focus();
                return false;
            }
            return true;
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ItemLedgerSavePrint ItemLedgerSavePrint = new ItemLedgerSavePrint();
            ItemLedgerSavePrint.ExportToFileOrPrint(RptItemLedger, false);
            Cursor.Current = Cursors.Default;
        }
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ItemLedgerSavePrint ItemLedgerSavePrint = new ItemLedgerSavePrint();
            ItemLedgerSavePrint.ExportToFileOrPrint(RptItemLedger, true);
            Cursor.Current = Cursors.Default;
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void GridViewItemLedger_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewItemLedger.Rows[e.RowIndex].Cells[(int)ItemLedgerTableColumn.STOCK].Value == null)
            {
                if (e.ColumnIndex == (int)ItemLedgerTableColumn.DATE)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)ItemLedgerTableColumn.STOCK)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
        }
        
        private void CheckedTreeComboLocation_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedInformation();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSave.PerformClick();
            }
            else if (keyData == (Keys.F9))
            {
                BtnPrint.PerformClick();
            }
            else if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                btnReset.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
            btnReset.Focus();
        }

        private void CheckedComboBoxItem_ItemCheckedEvent(object sender, ItemCheckEventArgs e)
        {
            DisplayCheckedInformation();
        }
        private ComboBox comboBox;
        private ToolTip ToolTips;

        private void GridViewItemLedger_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (GridViewItemLedger.Rows[e.RowIndex].Cells[(int)ItemLedgerTableColumn.STOCK].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
                    0, e.RowBounds.Top,
                    this.GridViewItemLedger.Columns.GetColumnsWidth(
                        DataGridViewElementStates.Visible) -
                    this.GridViewItemLedger.HorizontalScrollingOffset,
                    e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = GridViewItemLedger.Rows[e.RowIndex].Cells[(int)ItemLedgerTableColumn.PRODUCT_NAME].Value != null ? GridViewItemLedger.Rows[e.RowIndex].Cells[(int)ItemLedgerTableColumn.PRODUCT_NAME].Value.ToString() : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Near;

                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds, stringFormat);
            }
        }

        private void DisplayCheckedInformation()
        {
            string locationNodes = GetCheckedNodes(CheckedTreeComboLocation, "@ All Location");
            string displayFor = CheckedComboBoxItem.GetCheckedItemsString + " @ " + SelectedNodesText(CheckedTreeComboLocation);
            string displayLocation = "";
            string title = "Item Ledger (Stock)";

            if (!string.IsNullOrEmpty(locationNodes) && locationNodes != "@ All Location")
            {
                displayLocation = " For " + displayFor + " @ Location ";
                title += AppendToTitleIfNotEmpty(locationNodes, displayLocation);
            }
            else
            {
                title += " For " + displayFor;
            }

            this.Text = title;
        }

        private string GetCheckedNodes(ToolstripCheckedTreeComboBox comboBox, string nodeName)
        {
            if (comboBox == null || comboBox.CheckedNodes == null || comboBox.CheckedNodes.Count == 0)
            {
                return string.Empty;
            }

            bool isAllSelected = comboBox.CheckedNodes.Any(node => node.Name == "All");

            if (isAllSelected)
            {
                return nodeName;
            }

            string checkedNodes = string.Join(", ", comboBox.CheckedNodes.Select(node => node.Text));
            return checkedNodes;
        }
        private string AppendToTitleIfNotEmpty(string nodes, string prefix)
        {
            return string.IsNullOrEmpty(nodes) ? "" : $"{prefix} {nodes}";
        }

        private void CheckedComboBoxItem_CheckedItemsChanged(object sender, EventArgs e)
        {
            DisplayCheckedInformation();
        }
    }
}
