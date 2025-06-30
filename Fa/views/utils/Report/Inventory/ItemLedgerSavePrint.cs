using fa.api.Accounting;
using fa.api.utils;
using fa.report.Inventory;
using fa.reports.Inventory;
using fa.views.utils.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.IO;
using System.Linq;
using VisioForge.MediaFramework.Helpers;
using Rectangle = iTextSharp.text.Rectangle;

namespace fa.views.utils.Report.Inventory
{
    public class ItemLedgerSavePrint
    {
        public void ExportToFileOrPrint(RptItemLedger RptItemLedger, bool isPrint)
        {
            LaserPrint(RptItemLedger, isPrint);
        }
        readonly String[] ItemLedgerDataTableColumn = new String[]
        {
            "Date", "Transaction Type", "From", "To", "Rack No", "Description", "Batch #", "Exp Date", "UOM", "Quantity", "Cost", "Value", "Stock", "Product Name", "Id"
        };
        private DataTable LedgerAlignment(RptItemLedger RptItemLedger)
        {
            DataTable LedgerTable = new DataTable();
            LedgerTable.Columns.Add(ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.DATE], typeof(string));           
            LedgerTable.Columns.Add(ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.TRANSTYPE], typeof(string));
            LedgerTable.Columns.Add(ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.FROM], typeof(string));
            LedgerTable.Columns.Add(ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.TO], typeof(string));
            if (Global.Company.MaintainRackNumber)
            {
                LedgerTable.Columns.Add(ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.RACKNO], typeof(string));
            }
            LedgerTable.Columns.Add(ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.DESCRIPTION], typeof(string));
            if (RptItemLedger.LabeledBatchWise)
            {
                LedgerTable.Columns.Add(ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.BATCH], typeof(string));
                LedgerTable.Columns.Add(ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.EXPDATE], typeof(string));
            }            
            LedgerTable.Columns.Add(ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.UOM], typeof(string));
            LedgerTable.Columns.Add(ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.QTY], typeof(string));
            if (RptItemLedger.IsShowValue)
            {
                LedgerTable.Columns.Add(ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.PRICE], typeof(string));
                LedgerTable.Columns.Add(ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.VALUE], typeof(string));
            }
            LedgerTable.Columns.Add(ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.STOCK], typeof(string));
            LedgerTable.Columns.Add(ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.LID], typeof(string));
            
            DataRow LedgerTableRow = null;
            int i = 0;
            DateTime? lDate = null;
            double ClosingStock = 0.00;
            string NewLoctn = string.Empty;
            string ItemName = string.Empty;

            if (RptItemLedger.IsAllLocation)
            {
                ClosingStock = RptItemLedger.ItemLedger.Sum(x => x.OpeningStock);
                LedgerTableRow = LedgerTable.NewRow();
                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.DATE]] = DateUtils.FormatDate(RptItemLedger.FromDate.Date, Global.Company.DateFormat);
                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.TRANSTYPE]] = "Opening Stock";
                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.QTY]] = RptItemLedger.ItemLedger.Sum(x => x.OpeningStock).ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.STOCK]] = RptItemLedger.ItemLedger.Sum(x => x.OpeningStock).ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                LedgerTable.Rows.Add(LedgerTableRow);
            }
            foreach (ItemLedger Ledger in RptItemLedger.ItemLedger.OrderBy(x=> x.Location.Id))
            {
                string Loctn = Ledger.Location.ToString();
                double price = Ledger.CostPrice;
                if (Ledger.LineItems.Count > 0 || Ledger.OpeningStock > 0)
                {
                    if (!RptItemLedger.IsAllLocation)
                    {
                        if (RptItemLedger.LabeledBatchWise && Ledger.OpeningStockBatchWise.Count > 0)
                        {                          
                            if (Loctn.ToString() != NewLoctn.ToString())
                            {
                                LedgerTableRow = LedgerTable.NewRow();
                                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.DATE]] = Ledger.Location;
                                LedgerTable.Rows.Add(LedgerTableRow);
                                LedgerTableRow = LedgerTable.NewRow();
                                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.DATE]] = Ledger.ItemName;
                                LedgerTable.Rows.Add(LedgerTableRow);
                                NewLoctn = Ledger.Location.ToString();
                                ItemName = Ledger.ItemName;
                            }
                            if (Loctn.ToString() == NewLoctn.ToString() && ItemName != Ledger.ItemName)
                            {
                                LedgerTableRow = LedgerTable.NewRow();
                                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.DATE]] = Ledger.ItemName;
                                LedgerTable.Rows.Add(LedgerTableRow);
                            }
                            double BatchwiseOpeningStock = Ledger.OpeningStockBatchWise.Sum(x => x.OpeningStock);                            
                            if (BatchwiseOpeningStock == 0)
                            {
                                LedgerTableRow = LedgerTable.NewRow();
                                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.DATE]] = Ledger.ItemName;
                                LedgerTable.Rows.Add(LedgerTableRow);
                                LedgerTableRow = LedgerTable.NewRow();
                                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.DATE]] = DateUtils.FormatDate(RptItemLedger.FromDate.Date, Global.Company.DateFormat);
                                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.TRANSTYPE]] = "Opening Stock";
                                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.UOM]] = Ledger.Uom.ToLower();
                                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.QTY]] = BatchwiseOpeningStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                                if (RptItemLedger.IsShowValue)
                                {
                                    // calculate value
                                    LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.PRICE]] = price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); // Ledger.CostPrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                    LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.VALUE]] = RptItemLedger.CalculateValue(double.Parse(BatchwiseOpeningStock.ToString()), price).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                }
                                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.STOCK]] = BatchwiseOpeningStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.LID]] = Ledger.Location.Id;
                                LedgerTable.Rows.Add(LedgerTableRow);
                            }
                            BatchwiseOpeningStock = 0;
                            string Type = "Opening Stock";
                            string Date = DateUtils.FormatDate(RptItemLedger.FromDate.Date, Global.Company.DateFormat);
                            foreach (BatchWiseOpeningStock BatchWiseOpeningStock in Ledger.OpeningStockBatchWise)
                            {
                                if (BatchWiseOpeningStock.OpeningStock > 0)
                                {
                                    LedgerTableRow = LedgerTable.NewRow();
                                    LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.EXPDATE]] =DateUtils.FormatDate(BatchWiseOpeningStock.ExpDate.Date, Global.Company.DateFormat);
                                    LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.BATCH]] = BatchWiseOpeningStock.BatchNo;
                                    LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.DATE]] = Date;
                                    LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.TRANSTYPE]] = Type;
                                    LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.UOM]] = Ledger.Uom.ToLower();
                                    LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.QTY]] = BatchWiseOpeningStock.OpeningStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                                    BatchwiseOpeningStock = RptItemLedger.LoadClosingStock(BatchwiseOpeningStock, BatchWiseOpeningStock.OpeningStock, "Opening Stock");
                                    if (RptItemLedger.IsShowValue)
                                    {
                                        // calculate value
                                        LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.PRICE]] = price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); // Ledger.CostPrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                        LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.VALUE]] = RptItemLedger.CalculateValue(double.Parse(BatchwiseOpeningStock.ToString()), price).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                    }
                                    LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.STOCK]] = BatchwiseOpeningStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                                    LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.LID]] = Ledger.Location.Id;
                                    LedgerTable.Rows.Add(LedgerTableRow);
                                    Type = string.Empty;
                                    Date = string.Empty;
                                }
                            }
                        }
                        else
                        {
                            if (Loctn.ToString() != NewLoctn.ToString())
                            {
                                LedgerTableRow = LedgerTable.NewRow();
                                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.DATE]] = Ledger.Location;
                                LedgerTable.Rows.Add(LedgerTableRow);
                                NewLoctn = Ledger.Location.ToString();
                            }
                            LedgerTableRow = LedgerTable.NewRow();
                            LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.DATE]] = Ledger.ItemName;
                            LedgerTable.Rows.Add(LedgerTableRow);
                            LedgerTableRow = LedgerTable.NewRow();
                            LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.DATE]] = DateUtils.FormatDate(RptItemLedger.FromDate.Date, Global.Company.DateFormat);
                            LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.TRANSTYPE]] = "Opening Stock";
                            LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.UOM]] = Ledger.Uom.ToLower();
                            LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.QTY]] = Ledger.OpeningStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            if (RptItemLedger.IsShowValue)
                            {
                                // calculate value
                                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.PRICE]] = price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); // Ledger.CostPrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.VALUE]] = RptItemLedger.CalculateValue(double.Parse(Ledger.OpeningStock.ToString()), price).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            }
                            LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.STOCK]] = Ledger.OpeningStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.LID]] = Ledger.Location.Id;
                            LedgerTable.Rows.Add(LedgerTableRow);
                        }
                    }
                    string BatchNo = string.Empty;
                    lDate = RptItemLedger.IsAllLocation ? lDate : null;
                    ClosingStock = RptItemLedger.IsAllLocation ? ClosingStock : Ledger.OpeningStock;
                    foreach (ItemLedgerLineItem LineItem in Ledger.LineItems.OrderBy(x => x.Date))
                    {
                        LedgerTableRow = LedgerTable.NewRow();
                        if (lDate != LineItem.Date.Date)
                        {
                            LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.DATE]] = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                            lDate = LineItem.Date.Date;
                        }
                        LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.DESCRIPTION]] = LineItem.Description;
                        LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.UOM]] = LineItem.Uom.ToLower();
                        LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.QTY]] = RptItemLedger.Sign(LineItem.Qty, LineItem.TransactionType, Global.Company.QuantityPricision);
                        LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.TRANSTYPE]] = (string.IsNullOrEmpty(LineItem.FromLocation) && string.IsNullOrEmpty(LineItem.ToLocation)) ? null : LineItem.TransactionType;
                        LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.FROM]] = LineItem.FromLocation;
                        LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.TO]] = LineItem.ToLocation;
                        if (Global.Company.MaintainRackNumber)
                        {
                            LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.RACKNO]] = LineItem.RackNumber;
                        }
                        if (RptItemLedger.LabeledBatchWise)
                        {
                            LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.EXPDATE]] = LineItem.ExpDate.ToString(Global.Company.DateFormat);
                            LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.BATCH]] = LineItem.BatchDetail;
                        }
                        ClosingStock = RptItemLedger.LoadClosingStock(ClosingStock, LineItem.Qty, LineItem.TransactionType);
                        if (RptItemLedger.IsShowValue)
                        {
                            // calculate value
                            LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.PRICE]] = price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); //LineItem.CostPrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.VALUE]] = RptItemLedger.CalculateValue(double.Parse(LineItem.Qty.ToString()), price).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        }
                        LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.STOCK]] = RptItemLedger.Sign(ClosingStock, Global.Company.QuantityPricision);
                        
                        LedgerTable.Rows.Add(LedgerTableRow);
                    }
                    if (!RptItemLedger.IsAllLocation)
                    {
                        if (RptItemLedger.LabeledBatchWise && Ledger.OpeningStockBatchWise.Count > 0)
                        {
                            double BatchwiseClosingStock = Ledger.OpeningStockBatchWise.Sum(x => x.ClosingStock);
                            if (BatchwiseClosingStock == 0)
                            {
                                LedgerTableRow = LedgerTable.NewRow();
                                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.DATE]] = DateUtils.FormatDate(RptItemLedger.FromDate.Date, Global.Company.DateFormat);
                                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.TRANSTYPE]] = "Closing Stock";
                                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.UOM]] = Ledger.Uom.ToLower();
                                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.QTY]] = BatchwiseClosingStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                                if (RptItemLedger.IsShowValue)
                                {
                                    // calculate value
                                    LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.PRICE]] = price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); // Ledger.CostPrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                    LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.VALUE]] = RptItemLedger.CalculateValue(double.Parse(BatchwiseClosingStock.ToString()), price).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                }
                                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.STOCK]] = BatchwiseClosingStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                                LedgerTable.Rows.Add(LedgerTableRow);
                            }
                            BatchwiseClosingStock = 0;
                            string Type = "Closing Stock";
                            string Date = DateUtils.FormatDate(RptItemLedger.ToDate.Date, Global.Company.DateFormat);
                            foreach (BatchWiseOpeningStock BatchWiseOpeningStock in Ledger.OpeningStockBatchWise)
                            {
                                if (BatchWiseOpeningStock.ClosingStock != 0)
                                {
                                    LedgerTableRow = LedgerTable.NewRow();
                                    LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.BATCH]] = BatchWiseOpeningStock.BatchNo + " " + DateUtils.FormatDate(BatchWiseOpeningStock.ExpDate.Date, Global.Company.DateFormat);
                                    LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.DATE]] = Date;
                                    LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.TRANSTYPE]] = Type;
                                    LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.UOM]] = Ledger.Uom.ToLower();
                                    LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.QTY]] = RptItemLedger.Sign(BatchWiseOpeningStock.ClosingStock, Global.Company.QuantityPricision);
                                    if (RptItemLedger.IsShowValue)
                                    {
                                        // calculate value
                                        LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.PRICE]] = price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); // Ledger.CostPrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                        LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.VALUE]] = RptItemLedger.CalculateValue(BatchWiseOpeningStock.ClosingStock, price).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                    }
                                    LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.STOCK]] = RptItemLedger.Sign(ClosingStock, Global.Company.QuantityPricision);
                                    LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.LID]] = Ledger.Location.Id;
                                    LedgerTable.Rows.Add(LedgerTableRow);
                                    Type = string.Empty;
                                    Date = string.Empty;
                                }
                            }
                        }
                        else
                        {
                            LedgerTableRow = LedgerTable.NewRow();
                            LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.DATE]] = DateUtils.FormatDate(RptItemLedger.ToDate.Date, Global.Company.DateFormat);
                            LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.TRANSTYPE]] = "Closing Stock";
                            LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.UOM]] = Ledger.Uom.ToLower();
                            LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.QTY]] = RptItemLedger.Sign(ClosingStock, Global.Company.QuantityPricision);
                            if (RptItemLedger.IsShowValue)
                            {
                                // calculate value
                                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.PRICE]] = price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); // Ledger.CostPrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.VALUE]] = RptItemLedger.CalculateValue(double.Parse(ClosingStock.ToString()), price).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            }
                            LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.STOCK]] = RptItemLedger.Sign(ClosingStock, Global.Company.QuantityPricision);
                            LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.LID]] = Ledger.Location.Id;
                            LedgerTable.Rows.Add(LedgerTableRow);
                        }
                    }
                    i++;
                }
            }
            if (RptItemLedger.IsAllLocation)
            {
                LedgerTableRow = LedgerTable.NewRow();
                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.DATE]] = DateUtils.FormatDate(RptItemLedger.ToDate.Date, Global.Company.DateFormat);
                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.TRANSTYPE]] = "Closing Stock";
                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.QTY]] = RptItemLedger.Sign(ClosingStock, Global.Company.QuantityPricision);
                
                LedgerTableRow[ItemLedgerDataTableColumn[(int)ItemLedgerTableColumn.STOCK]] = RptItemLedger.Sign(ClosingStock, Global.Company.QuantityPricision);
                LedgerTable.Rows.Add(LedgerTableRow);
            }
            return LedgerTable;
        }
        
        public void LaserPrint(RptItemLedger RptItemLedger, bool isPrint)
        {
            string[] PageNumberToPage = new string[1000];
            DataTable dataTable = LedgerAlignment(RptItemLedger);

            var path = AppDomain.CurrentDomain.BaseDirectory;
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, -30, -30, 30, 20);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);

                pdfDoc.Open();

                double A4Height = 760;
                double ItemTableHeight = A4Height;

                int Cols = dataTable.Columns.Count-1;
                int Rows = dataTable.Rows.Count;

                float[] BodyTableWidths = new float[] { 20f, 37f, 35f, 35f, 40f, 20f, 20f, 22f };
                if (Global.Company.MaintainRackNumber)
                {
                    BodyTableWidths = new float[] { 20f, 37f, 35f, 35f, 30f,25f, 20f, 20f, 22f };
                }
                if (RptItemLedger.IsShowValue)
                {
                    BodyTableWidths = new float[] { 25f, 37f, 35f, 35f, 40f, 17f, 22f, 18f, 20f, 20f };
                    if (Global.Company.MaintainRackNumber)
                    {
                        BodyTableWidths = new float[] { 25f, 37f, 30f, 30f, 30f,25f, 20f, 22f, 20f, 20f, 20f };
                    }
                }
                if (RptItemLedger.LabeledBatchWise)
                {
                    BodyTableWidths = new float[] { 25f, 37f, 35f, 35f, 40f, 20f, 22f, 15f, 20f, 20f };
                    if (Global.Company.MaintainRackNumber)
                    {
                        BodyTableWidths = new float[] { 25f, 37f, 30f, 30f, 30f, 25f, 25f, 22f, 15f, 20f, 20f };
                    }
                    if (RptItemLedger.IsShowValue)
                    {
                        BodyTableWidths = new float[] { 24f, 37f, 30f, 30f, 37f, 30f, 24f, 17f, 22f, 20f, 23f, 20f };
                        if (Global.Company.MaintainRackNumber)
                        {
                            BodyTableWidths = new float[] { 26f, 35f, 32f, 32f, 25f, 30f, 20f, 26f, 15f, 22f, 20f, 26f, 20f };
                        }
                    }
                }
                float[] PageNumberTableWidths = new float[] { 50f, 50f };

                PdfPTable ReportBodyTable = null;

                ReportBodyTable = new PdfPTable(Cols);
                ReportBodyTable.SetWidths(BodyTableWidths);

                //*Add Company Detail table
                PdfPageHeader PdfHeader = new PdfPageHeader
                {
                    IsMainHeader = true,
                    Islogo = true,
                    IsAddress = true,
                    IsPhone = false,
                    IsEmail = false,
                    IsWebsite = false,
                    IsLicenceInfo = false,
                    ReportLine1 = RptItemLedger.ReportTitle(),
                    ReportLine2 = RptItemLedger.ReportDate()
                };
                PdfPTable HTable = PdfHeader.PageHeader(); 
                PdfHeader = new PdfPageHeader
                {
                    IsMainHeader = false,
                    Islogo = true,
                    IsAddress = true,
                    IsPhone = false,
                    IsEmail = false,
                    IsWebsite = false,
                    IsLicenceInfo = false,
                    ReportLine1 = RptItemLedger.ReportTitle(),
                    ReportLine2 = RptItemLedger.ReportHeader + " " + RptItemLedger.ReportDate()
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();

                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(dataTable, "ItemLedger");

                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int k = 2;
                BaseColor[] RowColor = new BaseColor[2];
                RowColor[0] = new BaseColor(255, 255, 255);
                RowColor[1] = new BaseColor(250, 250, 250);

                for (int i = 0; i < Rows; i++)
                {
                    PdfPCell rowCell = new PdfPCell();
                    double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportBodyTable));
                    if (TotalWorkingOnPageH > 760)
                    {
                        pdfDoc.Add(ReportBodyTable);
                        pdfDoc.NewPage();
                        pdfDoc.Add(MiniHTable);
                        pdfDoc.Add(MTable);
                        ReportBodyTable = new PdfPTable(Cols);
                        ReportBodyTable.SetWidths(BodyTableWidths);
                        k = 2;
                    }
                    BaseColor CurRowColor = RowColor[k % 2];
                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = dataTable.Rows[i][j].ToString();                        
                        rowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                        rowCell.BorderColor = new BaseColor(160, 160, 160);
                        rowCell.BackgroundColor = CurRowColor;
                        if (!RptItemLedger.LabeledBatchWise && !RptItemLedger.IsShowValue && (j + 2) != (int)ItemLedgerTableColumn.QTY && (j + 4) != (int)ItemLedgerTableColumn.STOCK) // && (j+3) != (int)ItemLedgerTableColumn.STOCK)
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else if (RptItemLedger.LabeledBatchWise && !RptItemLedger.IsShowValue && j != (int)ItemLedgerTableColumn.QTY && (j + 2) != (int)ItemLedgerTableColumn.STOCK) // && (j+3) != (int)ItemLedgerTableColumn.STOCK)
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else if (!RptItemLedger.LabeledBatchWise && RptItemLedger.IsShowValue && (j + 2) != (int)ItemLedgerTableColumn.QTY && (j + 2) != (int)ItemLedgerTableColumn.PRICE && (j + 2) != (int)ItemLedgerTableColumn.VALUE && (j + 2) != (int)ItemLedgerTableColumn.STOCK) // && (j+3) != (int)ItemLedgerTableColumn.STOCK)
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else if (RptItemLedger.LabeledBatchWise && RptItemLedger.IsShowValue && j != (int)ItemLedgerTableColumn.QTY && j != (int)ItemLedgerTableColumn.PRICE && j != (int)ItemLedgerTableColumn.VALUE && j != (int)ItemLedgerTableColumn.STOCK) // && (j+3) != (int)ItemLedgerTableColumn.STOCK)
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        } 
                        if(RptItemLedger.LabeledBatchWise == true && RptItemLedger.IsShowValue == false && !Global.Company.MaintainRackNumber)
                        {
                            if(dataTable.Rows[i][8].ToString() == "")
                            {
                                if (j != (int)ItemLedgerTableColumn.DATE)
                                {
                                    continue;
                                }
                                else if (j == (int)ItemLedgerTableColumn.DATE)
                                {
                                    int Span = RptItemLedger.LabeledBatchWise ? (Global.Company.MaintainRackNumber ? 11 : 10) : (Global.Company.MaintainRackNumber ? 9 : 8);
                                    rowCell.Colspan = RptItemLedger.IsShowValue ? Span + 2 : Span;
                                }
                            }
                        }
                       else if ((RptItemLedger.LabeledBatchWise && string.IsNullOrEmpty(dataTable.Rows[i][(int)ItemLedgerTableColumn.PRICE].ToString()))
                            || (!RptItemLedger.LabeledBatchWise && string.IsNullOrEmpty(dataTable.Rows[i][(int)ItemLedgerTableColumn.QTY-2].ToString())))
                       {
                            if (j != (int)ItemLedgerTableColumn.DATE)
                            {
                                continue;
                            }
                            else if (j == (int)ItemLedgerTableColumn.DATE)
                            {
                                int Span = RptItemLedger.LabeledBatchWise ? (Global.Company.MaintainRackNumber ? 11 : 10) : (Global.Company.MaintainRackNumber ? 9 : 8);
                                rowCell.Colspan = RptItemLedger.IsShowValue ? Span + 2 : Span;
                            }
                       }
                        if (RptItemLedger.LabeledBatchWise == true && RptItemLedger.IsShowValue == true && j == 8 && !Global.Company.MaintainRackNumber)
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else if(RptItemLedger.LabeledBatchWise == false && RptItemLedger.IsShowValue == true && j == 6 && !Global.Company.MaintainRackNumber)
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else if(RptItemLedger.IsShowValue == false && RptItemLedger.LabeledBatchWise == true && j == 8 && !Global.Company.MaintainRackNumber)
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        if(RptItemLedger.LabeledBatchWise == false && RptItemLedger.IsShowValue == false && j == 6 && !Global.Company.MaintainRackNumber)
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        ReportBodyTable.AddCell(rowCell);
                    }
                    k++;
                }
                pdfDoc.Add(ReportBodyTable);
                pdfDoc.Close();

                PdfFooter PdfFooter = new PdfFooter();
                PdfFooter.IsReport = true;
                PdfFooter.IsDate = true;
                PdfFooter.Text = string.Empty;
                PdfFooter.IsPageNumber = true;
                PdfFooter.PdfFile = myMemoryStream.ToArray();
                byte[] PdfFileWithFooter = PdfFooter.GetPdfFileWithFooter();
                myMemoryStream.Close();

                PdfGeneration PdfGeneration = new PdfGeneration();
                PdfGeneration.IsPrint = isPrint;
                PdfGeneration.FileName = RptItemLedger.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
            }
        }
    }
}
