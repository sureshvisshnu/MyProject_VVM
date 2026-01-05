 using fa.api.OrderManagement;
using fa.model.Accounting.Masters;
using fa.model.OrderManagement;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static fa.views.utils.PrinterSetup;
using fa.api.Accounting;
using fa.api.utils;
using Rectangle = iTextSharp.text.Rectangle;
using fa.api.catalog;
using fa.model.Catalog;


namespace fa.views.utils.Sale
{
    public class SalePrintSaveA5A4
    {
        public string fileName;
        // Dot Matrix Print // Laser Print //
        readonly String[] SaleDetailsTableColumnName = new String[]
        {
        "#", "Item Description","UOM", "HSN/SAC Code","Batch No", "Exp Date","MRP", "Rate", "Qty","Free", "Dis %", "Dis Amount", "Sub Total", "%", "Amount","Line Total"
        };

        readonly String[] SaleDetailsColumnA5Lands = new String[]
        {
        "#", "Item Description","UOM", "HSN Code","Bat.No", "Exp.Date", "MRP", "Rate", "Qty", "Dis %", "Tax %", "Total"
        };

        private static readonly string[] Units = {
            "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
            "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen",
            "seventeen", "eighteen", "nineteen"
        };

        private static readonly string[] Tens = {
            "", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"
        };

        public enum SaleDetailsTableColumn
        {
            SNO, DESC,UOM, HSN, BATCH, BATCHEXPDATE, MSRP, RATE, QTY, FREE, DISPER, DISAMOUNT, SUNTOTAL, TAXPER, TAXAMOUNT, LTOTAL
        }
        public enum SaleDetailsColumnForA5Landscape
        {
            SNO, DESC, UOM, HSN, BATCH, BATCHEXPDATE, MSRP, RATE, QTY, DISPER, TAXPER, LTOTAL
        }

        double Rounds = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == Global.getCurrentFiscalYearStartDate() && x.YearEndDate == Global.getCurrentFiscalYearEndDate() && x.EntryType == EntryType.SALES).RoundOff;
        public void ExportToFileOrPrint(long SalesId, string PrintPaper, string fileExtension, bool isPrint)
        {
            IList<TaxTable> lTaxTable = new List<TaxTable>();
            SalesManager SalesManager = SalesManager.Instance;
            SaleEntry SaleEntry = SalesManager.GetSaleEntry(SalesId);
            if (SaleEntry == null)
            {
                MessageBox.Show("Somthing went wrong, the selected sale is not valid.");
                return;
            }
            List<CompanySalesTaxAccountMap> CompanySalesTaxAccountMap = (List<CompanySalesTaxAccountMap>)Global.Company.SalesTaxAccountMaps;
            DataTable SaleDataTableTotal = new DataTable();
            DataTable SaleDetailsTable = new DataTable();
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.SNO], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.DESC], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.UOM], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.HSN], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.BATCH], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.BATCHEXPDATE], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.MSRP], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.RATE], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.QTY], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.FREE], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.DISPER], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.DISAMOUNT], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.SUNTOTAL], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.TAXPER], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.TAXAMOUNT], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.LTOTAL], typeof(string));
            if (SaleEntry.SaleDetails.Count != 0)
            {
                double SubTotalForTax = 0;
                double SubTotal = 0;
                float LintTotal = 0;
                float TaxTotal = 0;
                float Discount = 0;
                int count = 0;
                try
                {
                    foreach (SaleDetail SaleDetails in SaleEntry.SaleDetails.OrderBy(x=>x.Id))
                    {
                        SaleDetail lSaleDetail = SalesManager.GetSaleDetail(SaleDetails.Id);
                        string salesTax = "\n";
                        string subTotal = SaleDetails.OverridePrice == 0 ? (SaleDetails.Price * SaleDetails.Quantity).ToString("F") : (SaleDetails.OverridePrice * SaleDetails.Quantity).ToString("F");
                        SubTotalForTax= SaleDetails.OverridePrice == 0 ? (SaleDetails.Price * SaleDetails.Quantity) : (SaleDetails.OverridePrice * SaleDetails.Quantity);
                        var overAll = SaleDetails.OverridePrice == 0 ? (SaleDetails.Price * SaleDetails.Quantity) : (SaleDetails.OverridePrice * SaleDetails.Quantity);
                        SubTotal = SubTotal + overAll;
                        //Tax map
                        foreach (TaxDetail Taxes in lSaleDetail.TaxDetails)
                        {
                            salesTax +=Global.Company.SalesTaxAccountMaps.FirstOrDefault(x=>x.MapId== (Taxes.CatalogItemSalesTaxMap!=null?Taxes.CatalogItemSalesTaxMap.SalesTaxMapId: Taxes.ItemSalesTaxMap.SalesTaxMapId)).Name + " @" + Taxes.TaxRate.ToString() + "% ";
                        }
                        //Discount
                        if (lSaleDetail.Discounts.Count > 0)
                        {
                            salesTax += "\nDiscount @" + lSaleDetail.Discounts.Sum(X => X.Discount).ToString() + "%";
                            subTotal += "\n(" + Math.Round(lSaleDetail.Discounts.Sum(X => X.DiscountAmount), 2).ToString("F") + ")\n";                            
                            subTotal = SaleDetails.OverridePrice == 0 ? ((SaleDetails.Price * SaleDetails.Quantity) - lSaleDetail.Discounts.Sum(X => X.DiscountAmount)).ToString("F") : ((SaleDetails.OverridePrice * SaleDetails.Quantity) - lSaleDetail.Discounts.Sum(X => X.DiscountAmount)).ToString("F");
                            SubTotalForTax = SaleDetails.OverridePrice == 0 ? ((SaleDetails.Price * SaleDetails.Quantity) - lSaleDetail.Discounts.Sum(X => X.DiscountAmount)) : ((SaleDetails.OverridePrice * SaleDetails.Quantity) - lSaleDetail.Discounts.Sum(X => X.DiscountAmount));
                            Discount = Discount + lSaleDetail.Discounts.Sum(X => X.DiscountAmount);
                        }
                        count++;
                        //Tax details
                        foreach (ItemLevelSaleTaxDetail SalesTax in lSaleDetail.TaxDetails)
                        { 
                                TaxTable TaxTable = null;
                                //TaxDetail Taxes = lSaleDetail.TaxDetails.FirstOrDefault(x => x.CatalogItemSalesTaxMap.SalesTaxMapId == SalesTax.MapId);
                                //if (Taxes == null)
                                //{
                                //    TaxTable = lTaxTable.FirstOrDefault(x => x.TaxPer == 0);
                                //    if (TaxTable == null)
                                //    {
                                //        TaxTable = new TaxTable();
                                //        TaxTable.TaxPer = 0;
                                //        TaxTable.TaxAmount = 0;
                                //        TaxTable.SaleTaxMapId = TaxTable.SaleTaxMapId = SalesTax.CatalogItemSalesTaxMap.SalesTaxMapId;

                                //    TaxTable.SubTotal = SubTotalForTax;
                                //        lTaxTable.Add(TaxTable);
                                //    }
                                //    else
                                //    {
                                //        lTaxTable.Remove(TaxTable);
                                //        TaxTable.TaxAmount += 0;
                                //        TaxTable.SubTotal += SubTotalForTax;
                                //        lTaxTable.Add(TaxTable);
                                //    }
                                //    continue;
                                //}
                                TaxTable = lTaxTable.FirstOrDefault(x => x.TaxPer == SalesTax.TaxRate && x.SaleTaxMapId== (SalesTax.CatalogItemSalesTaxMap!=null?SalesTax.CatalogItemSalesTaxMap.SalesTaxMapId: SalesTax.ItemSalesTaxMap.SalesTaxMapId));
                                if (TaxTable == null)
                                {
                                    TaxTable = new TaxTable();
                                    TaxTable.TaxPer = SalesTax.TaxRate;
                                    TaxTable.TaxAmount = SalesTax.Amount;
                                    TaxTable.SaleTaxMapId = (SalesTax.CatalogItemSalesTaxMap != null ? SalesTax.CatalogItemSalesTaxMap.SalesTaxMapId : SalesTax.ItemSalesTaxMap.SalesTaxMapId);
                                    TaxTable.SubTotal = SubTotalForTax;
                                    lTaxTable.Add(TaxTable);
                                }
                                else
                                {
                                    lTaxTable.Remove(TaxTable);
                                    TaxTable.TaxAmount += SalesTax.Amount;
                                    TaxTable.SubTotal += SubTotalForTax;
                                    lTaxTable.Add(TaxTable);
                                }
                        }
                        LintTotal = LintTotal + SaleDetails.Amount;
                        if (lSaleDetail.TaxDetails.Count > 0)
                        {
                            TaxTotal = TaxTotal + lSaleDetail.TaxDetails.Sum(X => X.Amount);
                        }
                        double TaxAmount = PdfDataAlignment.ProductTaxPercentage(lSaleDetail.TaxDetails, SaleEntry.SaleTaxType);
                        //create new row
                        DataRow SaleDetailsTableNewRow = SaleDetailsTable.NewRow();
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.SNO] = count.ToString();
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.UOM] = SaleDetails.Uom.ToString();
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.DESC] = SaleDetails.Product.Name.ToString();
                        if (PrintPaper == "A4 LANDSCAPE" &&  (Global.Company.BusinessType == BuisnessType.Wholesale || Global.Company.BusinessType == BuisnessType.Pharmacy))
                        {
                            double ProductRetail = 0L;
                            if (SaleDetails.isBatch)
                            {
                                var InventoryBatchProduct = InventoryLocationManager.Instance.GetInventoryBatchDetail((long)SaleDetails.ProductId, SaleDetails.BatchNo);
                                ProductRetail = InventoryBatchProduct.RetailSalePrice;
                            }
                            else
                            {
                                var Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad((long)SaleDetails.ProductId);
                                ProductRetail = Product.RetailPrice;
                            }
                            SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.DESC] = SaleDetails.Product.Name.ToString() + " @PTR : " + string.Format("{0:F2}", Decimal.Parse(ProductRetail.ToString()));
                        }
                        else if (PrintPaper == "A5 LANDSCAPE")
                        {
                            if(SaleDetails.Sale.EntryType == Entrytype.QUOTE)
                            {
                                SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.DESC] = SaleDetails.Product.Name.ToString();
                            }
                            else
                            {
                            SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.DESC] = SaleDetails.Product.Name.ToString() + ", \n" + (SaleDetails.isBatch ? "Bat# : " + SaleDetails.BatchNo + "," + " Exp.: " + SaleDetails.ExpDate.Month.ToString() + "/" + SaleDetails.ExpDate.Year.ToString() : "");
                            }
                        }
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.HSN] = SaleDetails.Product.HSNCode==null?"": SaleDetails.Product.HSNCode.ToString();
                        if (SaleDetails.BatchNo != null)
                        {
                            SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.BATCH] = SaleDetails.BatchNo.ToString();
                            SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.BATCHEXPDATE] = SaleDetails.ExpDate.ToString(Global.Company.DateFormat);
                        }
                        else
                        {
                            SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.BATCH] = "";
                            SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.BATCHEXPDATE] = "";
                        }
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.MSRP] = SaleDetails.Msrp.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.RATE] = SaleDetails.OverridePrice == 0 ? SaleDetails.Price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) : SaleDetails.OverridePrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.QTY] = SaleDetails.Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.FREE] = SaleDetails.FreeQuantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.DISPER] = lSaleDetail.Discounts.Count > 0 ? lSaleDetail.Discounts.Sum(x => x.Discount).ToString("F") : "0.00";
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.DISAMOUNT] = lSaleDetail.Discounts.Count > 0 ? lSaleDetail.Discounts.Sum(x => x.DiscountAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) : TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                        double Stotal = double.Parse(subTotal.ToString());
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.SUNTOTAL] = Stotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.TAXPER] = PdfDataAlignment.ProductTaxPercentage(SaleDetails.TaxDetails, SaleEntry.SaleTaxType).ToString("F");
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.TAXAMOUNT] = lSaleDetail.TaxDetails.Count > 0 ? lSaleDetail.TaxDetails.Sum(x => x.Amount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) : TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.LTOTAL] = Math.Round(SaleDetails.Amount, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        //Add row
                        SaleDetailsTable.Rows.Add(SaleDetailsTableNewRow);
                    }
                    SaleDataTableTotal.Columns.Add("1", typeof(string));
                    SaleDataTableTotal.Columns.Add("2", typeof(string));
                    SaleDataTableTotal.Columns.Add("3", typeof(string));
                    SaleDataTableTotal.Columns.Add("4", typeof(string));
                    SaleDataTableTotal.Columns.Add("5", typeof(string));
                    SaleDataTableTotal.Columns.Add("6", typeof(string));

                    double LintTotalTrans = Math.Round(LintTotal, 2);
                    double RoundedTotal = LintTotalTrans;
                    double RoundoffAmount = Rounds > 0 ? RoundOff(RoundedTotal) : 0;
                    string AmountInWords = Global.Company.CountryId == 99 ? ConvertToINR(double.Parse((RoundedTotal + RoundoffAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)))) : "";

                    //Grand total
                    SaleDataTableTotal.Rows.Add(new object[] { "(Amount in words) " + AmountInWords, "Grand Total", (SubTotal - Discount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), "    ", TaxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), LintTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) });
                    for (int i = 0; i < SaleEntry.SaleAdditionalTransactions.Count; i++)
                    {
                        if (SaleEntry.SaleAdditionalTransactions[i].Action == AdditionalTransactionAction.CR)
                        {
                            LintTotalTrans = LintTotalTrans - SaleEntry.SaleAdditionalTransactions[i].Amount;
                        }
                        else
                        {
                            LintTotalTrans = LintTotalTrans + SaleEntry.SaleAdditionalTransactions[i].Amount;
                        }
                        var TransType = SaleEntry.SaleAdditionalTransactions[i].Type == AdditionalTransactionType.PERCENT ? " @" + SaleEntry.SaleAdditionalTransactions[i].Value + "%" : "";
                        //Additional trans
                        SaleDataTableTotal.Rows.Add(new object[] { SaleEntry.SaleAdditionalTransactions[i].Name + TransType, Math.Round(SaleEntry.SaleAdditionalTransactions[i].Amount, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), "       ", "", Math.Round(LintTotalTrans, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) });
                    }
                    //Round off
                    
                    SaleDataTableTotal.Rows.Add(new object[] { "  ", "Round off", RoundoffAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), "      ", "", (RoundedTotal + RoundoffAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) });
                }
                catch (DocumentException dex)
                {
                    MessageBox.Show(dex.ToString());
                }
                catch (IOException ioex)
                {
                    MessageBox.Show(ioex.ToString());
                }
                catch (Exception ee)
                {
                    MessageBox.Show("File Error Please Contact System Admin");
                    Console.WriteLine(ee.ToString());
                }
            }
            if (SaleDetailsTable.Rows.Count != 0)
            {
                if (fileExtension == "Laser")
                {
                    SalePrintSaveA5A4New SalePrintSaveA5A4New = new SalePrintSaveA5A4New();
                    //Customer need
                    SalePrintSaveA5A4New.GeneratePDF(lTaxTable, SaleDetailsTable, SaleDataTableTotal, SaleEntry, PrintPaper, "pdf", isPrint);
                    //Standrad
                    //GeneratePDF(lTaxTable,SaleDetailsTable, SaleDataTableTotal, SaleEntry, PrintPaper, "pdf", isPrint);
                }
                else if (fileExtension == "Dotmatrix")
                {
                    fileName = SaleEntry.EntryType == Entrytype.SALE ? "SaleInvoice" : SaleEntry.EntryType == Entrytype.RETURN ? "SaleReturnInvoice" :"Quotation";                    
                    SalePrintSaveDotmatrixA5A4 SalePrintSaveDotmatrixA4Portraid = new SalePrintSaveDotmatrixA5A4(this);
                    SalePrintSaveDotmatrixA4Portraid.GenerateDotmarixTxt(lTaxTable, SaleDetailsTable, SaleDataTableTotal, SaleEntry, PrintPaper, isPrint);
                }
            }
        }
        public string LastGeneratedFilePathForWE { get; set; } = string.Empty;
        public void ExportA5LandscapeToFileOrPrint(long SalesId, string PrintPaper, string fileExtension, bool isPrint, bool isForWE = false)
        {
            IList<TaxTable> lTaxTable = new List<TaxTable>();
            SalesManager SalesManager = SalesManager.Instance;
            SaleEntry SaleEntry = SalesManager.GetSaleEntry(SalesId);
            if (SaleEntry == null)
            {
                MessageBox.Show("Something went wrong, the selected sale is not valid.");
                return;
            }
            List<CompanySalesTaxAccountMap> CompanySalesTaxAccountMap = (List<CompanySalesTaxAccountMap>)Global.Company.SalesTaxAccountMaps;
            DataTable SaleDataTableTotal = new DataTable();
            DataTable SaleDetailsTable = new DataTable();
            SaleDetailsTable.Columns.Add(SaleDetailsColumnA5Lands[(int)SaleDetailsColumnForA5Landscape.SNO], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsColumnA5Lands[(int)SaleDetailsColumnForA5Landscape.DESC], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsColumnA5Lands[(int)SaleDetailsColumnForA5Landscape.UOM], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsColumnA5Lands[(int)SaleDetailsColumnForA5Landscape.HSN], typeof(string));
            //if (Global.Company.CompanySalesSetup.ShowBatchOnPrint)
            //{
            //    SaleDetailsTable.Columns.Add(SaleDetailsColumnA5Lands[(int)SaleDetailsColumnForA5Landscape.BATCH], typeof(string));
            //    SaleDetailsTable.Columns.Add(SaleDetailsColumnA5Lands[(int)SaleDetailsColumnForA5Landscape.BATCHEXPDATE], typeof(string));
            //}
            //else
            //{
            //    SaleDetailsTable.Columns.Add("1", typeof(string));
            //    SaleDetailsTable.Columns.Add("2", typeof(string));
            //}
            SaleDetailsTable.Columns.Add("1", typeof(string));
            SaleDetailsTable.Columns.Add("2", typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsColumnA5Lands[(int)SaleDetailsColumnForA5Landscape.MSRP], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsColumnA5Lands[(int)SaleDetailsColumnForA5Landscape.RATE], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsColumnA5Lands[(int)SaleDetailsColumnForA5Landscape.QTY], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsColumnA5Lands[(int)SaleDetailsColumnForA5Landscape.DISPER], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsColumnA5Lands[(int)SaleDetailsColumnForA5Landscape.TAXPER], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsColumnA5Lands[(int)SaleDetailsColumnForA5Landscape.LTOTAL], typeof(string));
            if (SaleEntry.SaleDetails.Count != 0)
            {
                double SubTotalForTax = 0;
                double SubTotal = 0;
                float LintTotal = 0;
                float TaxTotal = 0;
                float Discount = 0;
                int count = 0;
                try
                {
                    foreach (SaleDetail SaleDetails in SaleEntry.SaleDetails.OrderBy(x => x.Id))
                    {
                        SaleDetail lSaleDetail = SalesManager.GetSaleDetail(SaleDetails.Id);
                        string salesTax = "\n";
                        string subTotal = SaleDetails.OverridePrice == 0 ? (SaleDetails.Price * SaleDetails.Quantity).ToString("F") : (SaleDetails.OverridePrice * SaleDetails.Quantity).ToString("F");
                        SubTotalForTax = SaleDetails.OverridePrice == 0 ? (SaleDetails.Price * SaleDetails.Quantity) : (SaleDetails.OverridePrice * SaleDetails.Quantity);
                        var overAll = SaleDetails.OverridePrice == 0 ? (SaleDetails.Price * SaleDetails.Quantity) : (SaleDetails.OverridePrice * SaleDetails.Quantity);
                        SubTotal = SubTotal + overAll;
                        //Tax map
                        foreach (TaxDetail Taxes in lSaleDetail.TaxDetails)
                        {
                            salesTax += Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == (Taxes.CatalogItemSalesTaxMap != null ? Taxes.CatalogItemSalesTaxMap.SalesTaxMapId : Taxes.ItemSalesTaxMap.SalesTaxMapId)).Name + " @" + Taxes.TaxRate.ToString() + "% ";
                        }
                        //Discount
                        if (lSaleDetail.Discounts.Count > 0)
                        {
                            salesTax += "\nDiscount @" + lSaleDetail.Discounts.Sum(X => X.Discount).ToString() + "%";
                            subTotal += "\n(" + Math.Round(lSaleDetail.Discounts.Sum(X => X.DiscountAmount), 2).ToString("F") + ")\n";
                            subTotal = SaleDetails.OverridePrice == 0 ? ((SaleDetails.Price * SaleDetails.Quantity) - lSaleDetail.Discounts.Sum(X => X.DiscountAmount)).ToString("F") : ((SaleDetails.OverridePrice * SaleDetails.Quantity) - lSaleDetail.Discounts.Sum(X => X.DiscountAmount)).ToString("F");
                            SubTotalForTax = SaleDetails.OverridePrice == 0 ? ((SaleDetails.Price * SaleDetails.Quantity) - lSaleDetail.Discounts.Sum(X => X.DiscountAmount)) : ((SaleDetails.OverridePrice * SaleDetails.Quantity) - lSaleDetail.Discounts.Sum(X => X.DiscountAmount));
                            Discount = Discount + lSaleDetail.Discounts.Sum(X => X.DiscountAmount);
                        }
                        count++;
                        //Tax details
                        foreach (ItemLevelSaleTaxDetail SalesTax in lSaleDetail.TaxDetails)
                        {
                            TaxTable TaxTable = null;

                            TaxTable = lTaxTable.FirstOrDefault(x => x.TaxPer == SalesTax.TaxRate && x.SaleTaxMapId == (SalesTax.CatalogItemSalesTaxMap != null ? SalesTax.CatalogItemSalesTaxMap.SalesTaxMapId : SalesTax.ItemSalesTaxMap.SalesTaxMapId));
                            if (TaxTable == null)
                            {
                                TaxTable = new TaxTable();
                                TaxTable.TaxPer = SalesTax.TaxRate;
                                TaxTable.TaxAmount = SalesTax.Amount;
                                TaxTable.SaleTaxMapId = (SalesTax.CatalogItemSalesTaxMap != null ? SalesTax.CatalogItemSalesTaxMap.SalesTaxMapId : SalesTax.ItemSalesTaxMap.SalesTaxMapId);
                                TaxTable.SubTotal = SubTotalForTax;
                                lTaxTable.Add(TaxTable);
                            }
                            else
                            {
                                lTaxTable.Remove(TaxTable);
                                TaxTable.TaxAmount += SalesTax.Amount;
                                TaxTable.SubTotal += SubTotalForTax;
                                lTaxTable.Add(TaxTable);
                            }
                        }
                        LintTotal = LintTotal + SaleDetails.Amount;
                        if (lSaleDetail.TaxDetails.Count > 0)
                        {
                            TaxTotal = TaxTotal + lSaleDetail.TaxDetails.Sum(X => X.Amount);
                        }
                        double TaxAmount = PdfDataAlignment.ProductTaxPercentage(lSaleDetail.TaxDetails, SaleEntry.SaleTaxType);

                        IList<InventoryBatch> Batches = InventoryLocationManager.Instance.GetInventoryBatchbyProductId((long)lSaleDetail.ProductId!, (long)lSaleDetail.Sale.InventoryLocationId!);
                        Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad((long)lSaleDetail.ProductId!);
                        InventoryBatch inventoryBatch = InventoryLocationManager.Instance.GetInventoryBatchDetail((long)lSaleDetail.ProductId!, lSaleDetail.BatchNo, (long)lSaleDetail.Sale.InventoryLocationId!);

                        string format = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                        PriceType priceType = Global.Company.CompanySalesSetup.PriceType;
                        string Rate = string.Empty;

                        if (Batches.Count == 0)
                        {
                            Rate = priceType switch
                            {
                                PriceType.Retail => Product.RetailPrice.ToString(format),
                                PriceType.Wholesale => Product.WholdSalePrice.ToString(format),
                                PriceType.MaxRetailPrice => Product.Msrp.ToString(format),
                                _ => "0"
                            };
                        }
                        else
                        {
                            if (inventoryBatch == null)
                            {
                                Rate = "0.00";
                            }
                            else
                            {
                                Rate = priceType switch
                                {
                                    PriceType.Retail => inventoryBatch.RetailSalePrice.ToString(format),
                                    PriceType.Wholesale => inventoryBatch.WholeSalePrice.ToString(format),
                                    PriceType.MaxRetailPrice => inventoryBatch.MaxRetailPrice.ToString(format),
                                    _ => "0.00"
                                };
                            }
                        }

                        //create new row
                        DataRow SaleDetailsTableNewRow = SaleDetailsTable.NewRow();
                        SaleDetailsTableNewRow[(int)SaleDetailsColumnForA5Landscape.SNO] = count.ToString();
                        SaleDetailsTableNewRow[(int)SaleDetailsColumnForA5Landscape.DESC] = SaleDetails.Product.Name.ToString();
                        SaleDetailsTableNewRow[(int)SaleDetailsColumnForA5Landscape.UOM] = SaleDetails.Uom.ToString();
                        SaleDetailsTableNewRow[(int)SaleDetailsColumnForA5Landscape.HSN] = SaleDetails.Product.HSNCode == null ? "" : SaleDetails.Product.HSNCode.ToString();
                        //if (Global.Company.CompanySalesSetup.ShowBatchOnPrint && SaleDetails.BatchNo != null)
                        //{
                        //    string shortYearFormat = Global.Company.DateFormat.Replace("yyyy", "yy");
                        //    SaleDetailsTableNewRow[(int)SaleDetailsColumnForA5Landscape.BATCH] = SaleDetails.BatchNo.ToString();
                        //    SaleDetailsTableNewRow[(int)SaleDetailsColumnForA5Landscape.BATCHEXPDATE] = SaleDetails.ExpDate.ToString(shortYearFormat);
                        //}
                        //else
                        //{
                        //    SaleDetailsTableNewRow[(int)SaleDetailsColumnForA5Landscape.BATCH] = "";
                        //    SaleDetailsTableNewRow[(int)SaleDetailsColumnForA5Landscape.BATCHEXPDATE] = "";
                        //}
                        SaleDetailsTableNewRow[(int)SaleDetailsColumnForA5Landscape.BATCH] = "";
                        SaleDetailsTableNewRow[(int)SaleDetailsColumnForA5Landscape.BATCHEXPDATE] = "";

                        SaleDetailsTableNewRow[(int)SaleDetailsColumnForA5Landscape.MSRP] = SaleDetails.Msrp.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        SaleDetailsTableNewRow[(int)SaleDetailsColumnForA5Landscape.RATE] = SaleDetails.OverridePrice == 0 ? Rate : SaleDetails.OverridePrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        SaleDetailsTableNewRow[(int)SaleDetailsColumnForA5Landscape.QTY] = SaleDetails.Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        SaleDetailsTableNewRow[(int)SaleDetailsColumnForA5Landscape.DISPER] = lSaleDetail.Discounts.Count > 0 ? lSaleDetail.Discounts.Sum(x => x.Discount).ToString("F") : "0.00";
                        double Stotal = double.Parse(subTotal.ToString());
                        SaleDetailsTableNewRow[(int)SaleDetailsColumnForA5Landscape.TAXPER] = PdfDataAlignment.ProductTaxPercentage(SaleDetails.TaxDetails, SaleEntry.SaleTaxType).ToString("F");
                        SaleDetailsTableNewRow[(int)SaleDetailsColumnForA5Landscape.LTOTAL] = Math.Round(SaleDetails.Amount, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        //Add row
                        SaleDetailsTable.Rows.Add(SaleDetailsTableNewRow);
                    }
                    SaleDataTableTotal.Columns.Add("1", typeof(string));
                    SaleDataTableTotal.Columns.Add("2", typeof(string));
                    SaleDataTableTotal.Columns.Add("3", typeof(string));
                    SaleDataTableTotal.Columns.Add("4", typeof(string));
                    SaleDataTableTotal.Columns.Add("5", typeof(string));
                    SaleDataTableTotal.Columns.Add("6", typeof(string));

                    double LintTotalTrans = Math.Round(LintTotal, 2);
                    double RoundedTotal = LintTotalTrans;
                    double RoundoffAmount = Rounds > 0 ? RoundOff(RoundedTotal) : 0;
                    string AmountInWords = Global.Company.CountryId == 99 ? ConvertToINR(double.Parse((RoundedTotal + RoundoffAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)))) : "";

                    //Grand total
                    SaleDataTableTotal.Rows.Add(new object[] { "     ", "    ", "Grand Total", Discount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), TaxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), LintTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) });
                    for (int i = 0; i < SaleEntry.SaleAdditionalTransactions.Count; i++)
                    {
                        if (SaleEntry.SaleAdditionalTransactions[i].Action == AdditionalTransactionAction.CR)
                        {
                            LintTotalTrans = LintTotalTrans - SaleEntry.SaleAdditionalTransactions[i].Amount;
                        }
                        else
                        {
                            LintTotalTrans = LintTotalTrans + SaleEntry.SaleAdditionalTransactions[i].Amount;
                        }
                        var TransType = SaleEntry.SaleAdditionalTransactions[i].Type == AdditionalTransactionType.PERCENT ? " @" + SaleEntry.SaleAdditionalTransactions[i].Value + "%" : "";
                        RoundoffAmount = Rounds > 0 ? RoundOff(LintTotalTrans) : 0;
                        RoundedTotal = LintTotalTrans;
                        //Additional trans
                        SaleDataTableTotal.Rows.Add(new object[] { "    ", "    ", SaleEntry.SaleAdditionalTransactions[i].Name + TransType, "       ", Math.Round(SaleEntry.SaleAdditionalTransactions[i].Amount, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), Math.Round(LintTotalTrans, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) });
                    }
                    //Round off
                    AmountInWords = Global.Company.CountryId == 99 ? ConvertToINR(double.Parse((RoundedTotal + RoundoffAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)))) : "";
                    SaleDataTableTotal.Rows.Add(new object[] { "(Amount in words) " + AmountInWords, "     ", "Round off", "    ", RoundoffAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), (RoundedTotal + RoundoffAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) });
                }
                catch (DocumentException dex)
                {
                    MessageBox.Show(dex.ToString());
                }
                catch (IOException ioex)
                {
                    MessageBox.Show(ioex.ToString());
                }
                catch (Exception ee)
                {
                    MessageBox.Show("File Error Please Contact System Admin");
                    Console.WriteLine(ee.ToString());
                }
            }
            if (SaleDetailsTable.Rows.Count != 0)
            {
                if (fileExtension == "Laser")
                {
                    SalePrintSaveA5A4New SalePrintSaveA5A4New = new SalePrintSaveA5A4New();
                    SalePrintSaveA5A4New.GenerateA5LandscapPDF(lTaxTable, SaleDetailsTable, SaleDataTableTotal, SaleEntry, PrintPaper, "pdf", isPrint, isForWE);
                    if (isForWE)
                    {
                        LastGeneratedFilePathForWE = SalePrintSaveA5A4New.LastGeneratedFilePath;
                    }
                }
            }
        }

        private double RoundOff(double TotalAmount)
        {
            double Round = (Rounds / 2);
            double _roundoff = 0.00;
            if (Round > 0)
            {
                double mod = TotalAmount % (Round * 2);
                if (mod >= Round)
                {
                    _roundoff = (Round * 2) - mod;
                }
                else if (mod == 0)
                {
                    _roundoff = 0;
                }
                else
                {
                    _roundoff = -mod;
                }
            }
            return _roundoff;
        }

        public static string ConvertToINR(double amount)
        {
            if (amount == 0)
                return "zero rupees only";

            string words = "";

            long rupees = (long)Math.Floor(amount);
            int paise = (int)Math.Round((amount - rupees) * 100);

            if (rupees > 0)
            {
                words += ConvertIntegerToWords(rupees) + " rupee" + (rupees == 1 ? "" : "s");
            }

            if (paise > 0)
            {
                if (rupees > 0)
                    words += " and ";
                words += ConvertIntegerToWords(paise) + " paise" + (paise == 1 ? "" : "");
            }

            return words + " only";
        }

        private static string ConvertIntegerToWords(long number)
        {
            if (number == 0)
                return Units[0];

            if (number < 0)
                return "minus " + ConvertIntegerToWords(Math.Abs(number));

            string words = "";

            if ((number / 10000000) > 0)
            {
                words += ConvertIntegerToWords(number / 10000000) + " crore ";
                number %= 10000000;
            }

            if ((number / 100000) > 0)
            {
                words += ConvertIntegerToWords(number / 100000) + " lakh ";
                number %= 100000;
            }

            if ((number / 1000) > 0)
            {
                words += ConvertIntegerToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += ConvertIntegerToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                if (number < 20)
                    words += Units[number];
                else
                {
                    words += Tens[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + Units[number % 10];
                }
            }

            return words.Trim();
        }
        private PdfPTable ColumnCaption(PdfPTable table, DataTable dataTable, string PrintPaper)
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                PdfPCell headerCell = new PdfPCell();

                headerCell = new PdfPCell(new Phrase(column.Caption, PdfDataAlignment.GetFont("Font_Bold_Italic_9_White")));
                headerCell.BackgroundColor = new BaseColor(160, 160, 160);
                headerCell.BorderColor = BaseColor.BLACK;             
                headerCell.MinimumHeight = 25;
                headerCell.Padding = 4;
                headerCell.Rowspan = 2;
                if ((column.Caption == "Batch No" || column.Caption == "Batch/Exp Date") && PrintPaper != "A4 LANDSCAPE")
                {
                    continue;
                }
                if ((column.Caption == "Dis %" || column.Caption == "Dis Amount"))
                {
                    if (PrintPaper == "A4 LANDSCAPE")
                    {
                        if (column.Caption == "Dis %")
                        {
                            PdfPCell headerCell1 = new PdfPCell();

                            headerCell1 = new PdfPCell(new Phrase("Discount", PdfDataAlignment.GetFont("Font_Bold_Italic_9_White")));
                            headerCell1.Rowspan = 1;
                            headerCell1.Colspan = 2;
                            headerCell1.BackgroundColor = new BaseColor(160, 160, 160);
                            headerCell1.BorderColor = BaseColor.BLACK;
                            headerCell1.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(headerCell1);
                            continue;
                        }
                        if (column.Caption == "Dis Amount")
                        {
                            headerCell.Rowspan = 1;
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                if (column.Caption == "%")
                {
                    PdfPCell headerCell1 = new PdfPCell();

                    headerCell1 = new PdfPCell(new Phrase("Tax", PdfDataAlignment.GetFont("Font_Bold_Italic_9_White")));
                    headerCell1.Rowspan = 1;
                    headerCell1.Colspan = 2;
                    headerCell1.BackgroundColor = new BaseColor(160, 160, 160);
                    headerCell1.BorderColor = BaseColor.BLACK;
                    headerCell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(headerCell1);
                    continue;
                }
                if (column.Caption == "Amount")
                {
                    headerCell.Rowspan = 1;

                    continue;
                }
                if (column.Caption == "Rate" || column.Caption == "Qty" || column.Caption == "Amount" || column.Caption == "Msrp")
                {
                    headerCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                }
                else
                {
                    headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                }
                table.AddCell(headerCell);
            }
            PdfPCell headerCellTaxDis = new PdfPCell();
            headerCellTaxDis = new PdfPCell(new Phrase("%", PdfDataAlignment.GetFont("Font_Bold_Italic_9_White")));
            headerCellTaxDis.BackgroundColor = new BaseColor(160, 160, 160);
            headerCellTaxDis.BorderColor = BaseColor.BLACK;
            headerCellTaxDis.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(headerCellTaxDis);
            headerCellTaxDis = new PdfPCell(new Phrase("Amount", PdfDataAlignment.GetFont("Font_Bold_Italic_9_White")));
            headerCellTaxDis.BackgroundColor = new BaseColor(160, 160, 160);
            headerCellTaxDis.BorderColor = BaseColor.BLACK;
            headerCellTaxDis.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(headerCellTaxDis);
            //discount
            if (PrintPaper == "A4 LANDSCAPE")
            {
                headerCellTaxDis = new PdfPCell(new Phrase("%", PdfDataAlignment.GetFont("Font_Bold_Italic_9_White")));
                headerCellTaxDis.BackgroundColor = new BaseColor(160, 160, 160);
                headerCellTaxDis.BorderColor = BaseColor.BLACK;
                headerCellTaxDis.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(headerCellTaxDis);
                headerCellTaxDis = new PdfPCell(new Phrase("Amount", PdfDataAlignment.GetFont("Font_Bold_Italic_9_White")));
                headerCellTaxDis.BackgroundColor = new BaseColor(160, 160, 160);
                headerCellTaxDis.BorderColor = BaseColor.BLACK;
                headerCellTaxDis.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(headerCellTaxDis);
            }
            return table;
        }
        private PdfPTable RunningTotal(PdfPTable table, string PrintPaper, double PageSubTotal, double PageTaxTotal, double PageLineTotal)
        {
            int Minimumheight = 12;
            PdfPCell rowCell = new PdfPCell();

            rowCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.WHITE;
            rowCell.BorderColorTop = BaseColor.BLACK;
            rowCell.BorderColorRight = BaseColor.WHITE;
            rowCell.BorderColorBottom = BaseColor.BLACK; rowCell.Padding = 4;
            rowCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
            rowCell.MinimumHeight = 4;
            rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 14 : 10;
            table.AddCell(rowCell);

            rowCell = new PdfPCell(new Phrase("To Be Continue...", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.BLACK;
            rowCell.BorderColorTop = BaseColor.GRAY;
            rowCell.BorderColorRight = BaseColor.WHITE;
            rowCell.BorderColorBottom = BaseColor.BLACK;
            rowCell.MinimumHeight = Minimumheight;
            rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 7 : 3;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.Padding = 3;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);

            rowCell = new PdfPCell(new Phrase("Running Total", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.WHITE;
            rowCell.BorderColorTop = BaseColor.GRAY;
            rowCell.BorderColorRight = BaseColor.GRAY;
            rowCell.BorderColorBottom = BaseColor.BLACK;
            rowCell.MinimumHeight = Minimumheight;
            rowCell.Colspan = 3;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);

            rowCell = new PdfPCell(new Phrase(PageSubTotal.ToString("F"), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.GRAY;
            rowCell.BorderColorTop = BaseColor.GRAY;
            rowCell.BorderColorRight = BaseColor.GRAY;
            rowCell.BorderColorBottom = BaseColor.BLACK;
            rowCell.MinimumHeight = Minimumheight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);

            rowCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.GRAY;
            rowCell.BorderColorTop = BaseColor.GRAY;
            rowCell.BorderColorRight = BaseColor.GRAY;
            rowCell.BorderColorBottom = BaseColor.BLACK;
            rowCell.MinimumHeight = Minimumheight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);

            rowCell = new PdfPCell(new Phrase(PageTaxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.GRAY;
            rowCell.BorderColorTop = BaseColor.GRAY;
            rowCell.BorderColorRight = BaseColor.GRAY;
            rowCell.BorderColorBottom = BaseColor.BLACK;
            rowCell.MinimumHeight = Minimumheight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);

            rowCell = new PdfPCell(new Phrase(PageLineTotal.ToString("F"), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.GRAY;
            rowCell.BorderColorTop = BaseColor.GRAY;
            rowCell.BorderColorRight = BaseColor.BLACK;
            rowCell.BorderColorBottom = BaseColor.BLACK;
            rowCell.MinimumHeight = Minimumheight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            return table;
        }
        private PdfPTable ContinueTotal(PdfPTable table, string PrintPaper, string RefNumber, double PageSubTotal, double PageTaxTotal, double PageLineTotal)
        {
            int Minimumheight = 12;
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(RefNumber, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.MinimumHeight = Minimumheight;
            rowCell.Colspan = 3;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.Padding = 3;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase("Continue Total", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.MinimumHeight = Minimumheight;
            rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 7 : 3;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase(PageSubTotal.ToString("F"), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.MinimumHeight = Minimumheight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.MinimumHeight = Minimumheight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase(PageTaxTotal.ToString("F"), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.MinimumHeight = Minimumheight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase(PageLineTotal.ToString("F"), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.MinimumHeight = Minimumheight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.WHITE;
            rowCell.BorderColorTop = BaseColor.BLACK;
            rowCell.BorderColorRight = BaseColor.WHITE;
            rowCell.BorderColorBottom = BaseColor.BLACK;
            rowCell.MinimumHeight = 4;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 14 : 10; ;
            table.AddCell(rowCell);
            return table;
        }        
        private PdfPTable TaxTable(IList<TaxTable> lTaxTable, DataTable totalTable)
        {
            int numtax = Global.Company.SalesTaxAccountMaps.Count;
            int TaxCol = 5 + (numtax * 2);
            PdfPTable taxPdfTable = new PdfPTable(TaxCol);
            float[] column = new float[TaxCol];
            int j = 0;
            column[j] = 20f;
            for (j = 0; j < (numtax * 2); j++)
            {
                column[j + 1] = (j + 1) / 2 == 0 ? 15f : 20f;
            }
            column[j + 1] = 20f;
            column[j + 2] = 30f;
            column[j + 3] = 30f;
            column[j + 4] = 30f;
            int MinimumHeight = 12;
            taxPdfTable.SetWidths(column);
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase("Taxable Value", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.Rowspan = 2;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            taxPdfTable.AddCell(rowCell);
            foreach (CompanySalesTaxAccountMap Map in Global.Company.SalesTaxAccountMaps)
            {
                rowCell = new PdfPCell(new Phrase(Map.Name, PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.Colspan = 2;
                rowCell.HorizontalAlignment = Element.ALIGN_CENTER;
                rowCell.BackgroundColor = new BaseColor(220, 220, 220);
                taxPdfTable.AddCell(rowCell);
            }
            rowCell = new PdfPCell(new Phrase("Total Tax Amount", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.Rowspan = 2;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            taxPdfTable.AddCell(rowCell);
            //signature
            rowCell = new PdfPCell(new Phrase("For " + Global.Company.Name + " \n Authorized Signatory", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.Rowspan = (2 + (lTaxTable.Count > 0 ? lTaxTable.Count / 2 : 1));
            rowCell.Colspan = 3;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.BLACK;
            rowCell.BorderColorTop = BaseColor.WHITE;
            rowCell.BorderColorRight = BaseColor.WHITE;
            rowCell.BorderColorBottom = BaseColor.WHITE;
            taxPdfTable.AddCell(rowCell);
            for (int nt = 0; nt < numtax; nt++)
            {
                rowCell = new PdfPCell(new Phrase("Rate", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                rowCell.BackgroundColor = new BaseColor(220, 220, 220);
                taxPdfTable.AddCell(rowCell);

                rowCell = new PdfPCell(new Phrase("Amount", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                rowCell.BackgroundColor = new BaseColor(220, 220, 220);
                taxPdfTable.AddCell(rowCell);
            }
            if (lTaxTable.Count > 0)
            {
                double taxAmount = 0.00;
                int Count = 1;
                foreach (TaxTable llTaxTable in lTaxTable.OrderBy(x => x.SubTotal))
                {
                    if (Count == 1)
                    {
                        rowCell = new PdfPCell(new Phrase(llTaxTable.SubTotal.ToString(Global.Company.PrimaryCurrency.CurrencyFormat), PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                        rowCell.MinimumHeight = MinimumHeight;
                        rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        taxPdfTable.AddCell(rowCell);
                    }
                    rowCell = new PdfPCell(new Phrase(llTaxTable.TaxPer.ToString(Global.Company.PrimaryCurrency.CurrencyFormat), PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                    rowCell.MinimumHeight = MinimumHeight;
                    rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    taxPdfTable.AddCell(rowCell);

                    rowCell = new PdfPCell(new Phrase(llTaxTable.TaxAmount.ToString(Global.Company.PrimaryCurrency.CurrencyFormat), PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                    rowCell.MinimumHeight = MinimumHeight;
                    rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    taxPdfTable.AddCell(rowCell);
                    taxAmount += llTaxTable.TaxAmount;
                    if (Count == numtax)
                    {
                        rowCell = new PdfPCell(new Phrase(taxAmount.ToString(Global.Company.PrimaryCurrency.CurrencyFormat), PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                        rowCell.MinimumHeight = MinimumHeight;
                        rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        taxPdfTable.AddCell(rowCell);
                        Count = 0;
                        taxAmount = 0.00;
                    }
                    Count++;
                }
            }
            else
            {
                rowCell = new PdfPCell(new Phrase(totalTable.Rows[0][1].ToString(), PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                taxPdfTable.AddCell(rowCell);
                for (int i = 0; i < numtax; i++)
                {
                    rowCell = new PdfPCell(new Phrase("0.00", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                    rowCell.MinimumHeight = MinimumHeight;
                    rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    taxPdfTable.AddCell(rowCell);
                    rowCell = new PdfPCell(new Phrase("0.00", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                    rowCell.MinimumHeight = MinimumHeight;
                    rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    taxPdfTable.AddCell(rowCell);
                }
                rowCell = new PdfPCell(new Phrase("0.00", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                taxPdfTable.AddCell(rowCell);
            }
            return taxPdfTable;
        }
        
        //Standard
        public void GeneratePDF(IList<TaxTable> lTaxTable, DataTable dataTable, DataTable totalTable, SaleEntry SaleEntry, string PrintPaper, string fileExtension, bool isPrint)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                var pageSize = PrintPaper == "A5 LANDSCAPE" ? new Rectangle(595, 421) : PrintPaper == "A4 LANDSCAPE" ? PageSize.A4.Rotate() : PrintPaper == "A4 PORTRAIT" ? PageSize.A4 : new Rectangle(0, 0);
                double A4Height = PrintPaper == "A5 LANDSCAPE" ? 300 : PrintPaper == "A4 LANDSCAPE" ? 500 : PrintPaper == "A4 PORTRAIT" ? 700 : 0;
                int cols = PrintPaper == "A4 LANDSCAPE" ? 14 : 10;
                int rows = dataTable.Rows.Count;

                Document pdfDoc = new Document(pageSize, -30, -30, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);

                pdfDoc.Open();
                float a4Size = pdfDoc.PageSize.Top;
                double lineHeight = 14;
                PdfPTable SaleHeader = PdfDataAlignment.SaleHeader(SaleEntry);
                PdfPTable SaleMiniHeader = PdfDataAlignment.SaleMiniHeader(SaleEntry);
                PdfPTable SaleMainTable = PdfDataAlignment.SaleMainTable(SaleEntry);
                PdfPTable DummyTable = PdfDataAlignment.DummyTable(1, 1, 4);
                PdfPTable MiniDummyTableWithoutBoard = PdfDataAlignment.DummyTableWithoutBoard(1, 1, 2);
                pdfDoc.Add(SaleHeader);
                pdfDoc.Add(SaleMainTable);
                pdfDoc.Add(DummyTable);
                PdfPTable table = new PdfPTable(cols);
                PdfPCell headerCell = new PdfPCell();
                float[] widths = new float[] { 15f, 90f, 60f, 55f, 22f, 25f, 40f, 23f, 35f, 50f };
                if (PrintPaper == "A4 LANDSCAPE")
                {
                    widths = new float[] { 10f, 75f, 30f, 30f, 30f, 35f, 20f, 20f, 15f, 25f, 40f, 15f, 25f, 50f };
                }
                double HeaderTableHeight = SaleHeader.TotalHeight + SaleMainTable.TotalHeight + DummyTable.TotalHeight;
                double TotalWorkingOnPageH = HeaderTableHeight;
                table.SetWidths(widths);
                table = ColumnCaption(table, dataTable, PrintPaper);
                double PageSubTotal = 0;
                double PageTaxTotal = 0;
                double PageLineTotal = 0;
                string RefNumber = (SaleEntry.EntryType == Entrytype.SALE ? "Invoice: " : "Quotes: ") + SaleEntry.RefNumber;

                //Add Item
                for (int i = 0; i < rows; i++)
                {
                    PdfPCell rowCell = new PdfPCell();

                    if (TotalWorkingOnPageH >= A4Height)
                    {
                        //Running Total
                        table = RunningTotal(table, PrintPaper, PageSubTotal, PageTaxTotal, PageLineTotal);

                        pdfDoc.Add(table);
                        pdfDoc.NewPage();
                        table = new PdfPTable(cols);
                        table.SetWidths(widths);
                        pdfDoc.Add(SaleMiniHeader);
                        pdfDoc.Add(MiniDummyTableWithoutBoard);

                        HeaderTableHeight = SaleMiniHeader.TotalHeight + MiniDummyTableWithoutBoard.TotalHeight;
                        TotalWorkingOnPageH = 0;

                        //Continue Total
                        table = ContinueTotal(table, PrintPaper, RefNumber, PageSubTotal, PageTaxTotal, PageLineTotal);
                        //Item Table Header New Page
                        table = ColumnCaption(table, dataTable, PrintPaper);
                    }

                    double TempHeight = lineHeight;
                    //add item detail
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        if ((j == 3 || j == 4 || j == 8 || j == 9) && PrintPaper != "A4 LANDSCAPE")
                        {
                            continue;
                        }
                        var temp = dataTable.Rows[i][j].ToString();
                        rowCell.Padding = 4;
                        rowCell = new PdfPCell(new Phrase(temp, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                        rowCell.UseVariableBorders = true;
                        rowCell.BorderColorLeft = BaseColor.GRAY;
                        rowCell.BorderColorTop = BaseColor.GRAY;
                        rowCell.BorderColorRight = BaseColor.GRAY;
                        rowCell.BorderColorBottom = BaseColor.GRAY;
                        if (i == 0)
                        {
                            rowCell.BorderColorTop = BaseColor.BLACK;
                        }
                        if (j == 0)
                        {
                            rowCell.BorderColorLeft = BaseColor.BLACK;
                        }
                        if (j == 10)
                        {
                            rowCell.BorderColorRight = BaseColor.BLACK;
                        }
                        //Row Odd Even Color
                        if (i % 2 != 0)
                        {
                            rowCell.BackgroundColor = new BaseColor(242, 242, 242);
                        }
                        else
                        {
                            rowCell.BackgroundColor = BaseColor.WHITE;
                        }

                        if (j == 1)
                        {
                            string[] lines = temp.Split(new[] { "\n" }, StringSplitOptions.None);

                            TempHeight += lines.Count() == 1 ? 0 : lines.Count() == 2 ? 10.5 : 20.5;

                            float size = lines[0].Length > 40 ? 6 : lines[0].Length > 30 ? 7 : lines[0].Length > 20 ? 8 : 10;
                            TempHeight += size == 10 ? 0 : 10.5;

                            string FName = size == 6 ? "Font_Normal_Italic_6_Black" : size == 7 ? "Font_Normal_Italic_7_Black" : "Font_Normal_Italic_8_Black";
                            rowCell.AddElement(new Phrase(lines[0], PdfDataAlignment.GetFont(FName)));

                            for (int k = 1; k < lines.Length; k++)
                            {
                                rowCell.AddElement(new Phrase(lines[k], PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                            }
                            rowCell.HorizontalAlignment = Element.ALIGN_BASELINE;
                            rowCell.VerticalAlignment = Element.ALIGN_BASELINE;
                            rowCell.PaddingTop = -3;
                        }
                        else if (j == 2 || j == 3)
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            if (j == 3)
                            {
                                string[] lines = temp.Split(new[] { "\n" },
                                                               StringSplitOptions.None
                                                                );
                                float TH = lines.Count() == 1 ? 14 : 25;

                                if (TH > TempHeight)
                                {
                                    TempHeight = lines.Count() == 1 ? 14 : 25;
                                }
                            }
                        }
                        else
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }

                        if (j == 10)
                        {
                            string[] lines = temp.Split(new[] { "\n" },
                                                          StringSplitOptions.None
                                                      );
                            PageSubTotal += double.Parse(lines[lines.Count() - 1]);
                        }
                        if (j == 12)
                        {
                            PageTaxTotal += double.Parse(temp);
                        }
                        if (j == 13)
                        {
                            PageLineTotal += double.Parse(temp);
                        }

                        table.AddCell(rowCell);
                    }
                    TotalWorkingOnPageH = (HeaderTableHeight + PdfDataAlignment.CalculatePdfTableHeight(table));

                }
                //Total Table
                int rowstotalTable = totalTable.Rows.Count;
                PdfPTable totalPdfTable = new PdfPTable(cols);
                totalPdfTable.SetWidths(widths);
                for (int i = 0; i < rowstotalTable; i++)
                {
                    PdfPCell rowCell = new PdfPCell();
                    for (int j = 0; j < 5; j++)
                    {
                        var temp = totalTable.Rows[i][j].ToString();
                        rowCell = new PdfPCell(new Phrase(temp, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                        if (j == 0)
                        {
                            rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 10 : 6;
                        }
                        rowCell.Padding = 4;
                        rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        rowCell.BorderColor = BaseColor.BLACK;
                        totalPdfTable.AddCell(rowCell);
                    }
                }


                TotalWorkingOnPageH = (HeaderTableHeight + PdfDataAlignment.CalculatePdfTableHeight(table)
                    + PdfDataAlignment.CalculatePdfTableHeight(totalPdfTable));
                if (TotalWorkingOnPageH > A4Height)
                {
                    PdfPCell rowCell = new PdfPCell();
                    //Running Total
                    table = RunningTotal(table, PrintPaper, PageSubTotal, PageTaxTotal, PageLineTotal);
                    pdfDoc.Add(table);
                    pdfDoc.NewPage();
                    pdfDoc.Add(SaleMiniHeader);
                    pdfDoc.Add(MiniDummyTableWithoutBoard);

                    table = new PdfPTable(cols);
                    table.SetWidths(widths);
                    //Continue Total
                    table = ContinueTotal(table, PrintPaper, RefNumber, PageSubTotal, PageTaxTotal, PageLineTotal);

                    TotalWorkingOnPageH = MiniDummyTableWithoutBoard.TotalHeight + SaleMiniHeader.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(totalPdfTable);
                }

                for (int i = 0; i < rowstotalTable; i++)
                {
                    PdfPCell rowCell = new PdfPCell();
                    for (int j = 0; j < 5; j++)
                    {
                        var temp = totalTable.Rows[i][j].ToString();
                        rowCell = new PdfPCell(new Phrase(temp, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                        if (j == 0)
                        {
                            rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 10 : 6;
                        }
                        rowCell.Padding = 4;
                        rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        rowCell.BorderColor = BaseColor.BLACK;
                        table.AddCell(rowCell);
                    }
                }
                pdfDoc.Add(table);


                //tax table
                double TaxTableHeight = 0;
                if (Global.Company.SalesTaxAccountMaps.Count != 0)
                {
                    PdfPTable TaxPdfPTable = TaxTable(lTaxTable, totalTable);
                    TaxTableHeight = PdfDataAlignment.CalculatePdfTableHeight(TaxPdfPTable) + DummyTable.HeaderHeight;

                    TotalWorkingOnPageH = TotalWorkingOnPageH + TaxTableHeight;
                    if (TotalWorkingOnPageH > A4Height)
                    {
                        pdfDoc.NewPage();
                        pdfDoc.Add(SaleMiniHeader);
                        pdfDoc.Add(MiniDummyTableWithoutBoard);
                    }
                    else
                    {
                        pdfDoc.Add(DummyTable);
                    }
                    pdfDoc.Add(TaxPdfPTable);

                }
                //pdfDoc.Add(totalPdfTable);
                //var jAction = PdfAction.JavaScript("this.print(true);\r", writer);
                //writer.AddJavaScript(jAction);
                //var cb = writer.DirectContent;
                //pdfDoc.AddDocListener(writer);
                pdfDoc.Close();
                PdfGeneration.SaveMemoryStream(myMemoryStream, (SaleEntry.EntryType == Entrytype.SALE ? "SaleInvoice" : "SaleQuote"), fileExtension, isPrint, PrintPaper == "A5 LANDSCAPE" ? PaperTypes.A5_LANDSCAPE : PrintPaper == "A4 LANDSCAPE" ? PaperTypes.A4_LANDSCAPE : PaperTypes.A4_PORTRAIT);
            }
        }

        // Dot Matrix Pre Print // Laser Print //
        readonly String[] SalePrePrintDetailsTableColumnName = new String[]
        {
        "#", "Item Description","Mfr" ,"HSN/SAC Code","Batch No","Cgst","Sgst", "Batch/Exp Date", "Rate", "Qty","Free", "Dis %", "Dis Amount", "Sub Total", "%", "Amount","Line Total"
        };
        public enum SalePrePrintDetailsTableColumn
        {
            SNO, DESC, MFR, HSN, BATCH, CGST, SGST, BATCHEXPDATE, RATE, QTY, FREE, DISPER, DISAMOUNT, SUBTOTAL, TAXPER, TAXAMOUNT, LTOTAL
        }
        public void ExportToFileOrPrintForPreprinting(long SalesId, string PrintPaper, string fileExtension, bool isPrint)
        {
            IList<TaxTable> lTaxTable = new List<TaxTable>();
            SalesManager SalesManager = SalesManager.Instance;
            SaleEntry SaleEntry = SalesManager.GetSaleEntry(SalesId);
            if (SaleEntry == null)
            {
                MessageBox.Show("Somthing went wrong, the selected sale is not valid.");
                return;
            }
            List<CompanySalesTaxAccountMap> CompanySalesTaxAccountMap = (List<CompanySalesTaxAccountMap>)Global.Company.SalesTaxAccountMaps;
            DataTable SaleDataTableTotal = new DataTable();
            DataTable SaleDetailsTable = new DataTable();
            SaleDetailsTable.Columns.Add(SalePrePrintDetailsTableColumnName[(int)SalePrePrintDetailsTableColumn.SNO], typeof(string));
            SaleDetailsTable.Columns.Add(SalePrePrintDetailsTableColumnName[(int)SalePrePrintDetailsTableColumn.DESC], typeof(string));
            SaleDetailsTable.Columns.Add(SalePrePrintDetailsTableColumnName[(int)SalePrePrintDetailsTableColumn.MFR], typeof(string));
            SaleDetailsTable.Columns.Add(SalePrePrintDetailsTableColumnName[(int)SalePrePrintDetailsTableColumn.HSN], typeof(string));
            SaleDetailsTable.Columns.Add(SalePrePrintDetailsTableColumnName[(int)SalePrePrintDetailsTableColumn.BATCH], typeof(string));
            SaleDetailsTable.Columns.Add(SalePrePrintDetailsTableColumnName[(int)SalePrePrintDetailsTableColumn.CGST], typeof(string));
            SaleDetailsTable.Columns.Add(SalePrePrintDetailsTableColumnName[(int)SalePrePrintDetailsTableColumn.SGST], typeof(string));
            SaleDetailsTable.Columns.Add(SalePrePrintDetailsTableColumnName[(int)SalePrePrintDetailsTableColumn.BATCHEXPDATE], typeof(string));
            SaleDetailsTable.Columns.Add(SalePrePrintDetailsTableColumnName[(int)SalePrePrintDetailsTableColumn.RATE], typeof(string));
            SaleDetailsTable.Columns.Add(SalePrePrintDetailsTableColumnName[(int)SalePrePrintDetailsTableColumn.QTY], typeof(string));
            SaleDetailsTable.Columns.Add(SalePrePrintDetailsTableColumnName[(int)SalePrePrintDetailsTableColumn.FREE], typeof(string));
            SaleDetailsTable.Columns.Add(SalePrePrintDetailsTableColumnName[(int)SalePrePrintDetailsTableColumn.DISPER], typeof(string));
            SaleDetailsTable.Columns.Add(SalePrePrintDetailsTableColumnName[(int)SalePrePrintDetailsTableColumn.DISAMOUNT], typeof(string));
            SaleDetailsTable.Columns.Add(SalePrePrintDetailsTableColumnName[(int)SalePrePrintDetailsTableColumn.SUBTOTAL], typeof(string));
            SaleDetailsTable.Columns.Add(SalePrePrintDetailsTableColumnName[(int)SalePrePrintDetailsTableColumn.TAXPER], typeof(string));
            SaleDetailsTable.Columns.Add(SalePrePrintDetailsTableColumnName[(int)SalePrePrintDetailsTableColumn.TAXAMOUNT], typeof(string));
            SaleDetailsTable.Columns.Add(SalePrePrintDetailsTableColumnName[(int)SalePrePrintDetailsTableColumn.LTOTAL], typeof(string));

            if (SaleEntry.SaleDetails.Count != 0)
            {
                double SubTotalForTax = 0;
                double SubTotal = 0;
                float LintTotal = 0;
                float TaxTotal = 0;
                float Discount = 0;
                int count = 0;
                float[] TaxWiseAmountTotals = new float[Global.Company.SalesTaxAccountMaps.Count];
                foreach(OrderLevelSaleTaxDetail Detail in SaleEntry.TaxDetails)
                {
                    int i= 0;
                    foreach(CompanySalesTaxAccountMap map in Global.Company.SalesTaxAccountMaps)
                    {
                        if(Detail.TaxAccountId.ToString()== map.AccountId.ToString())
                        {
                            TaxWiseAmountTotals[i] = Detail.Amount;
                        }
                        i++;
                    }
                }

                try
                {
                    foreach (SaleDetail SaleDetails in SaleEntry.SaleDetails)
                    {
                        SaleDetail lSaleDetail = SalesManager.GetSaleDetail(SaleDetails.Id);
                        string salesTax = "\n";
                        string subTotal = SaleDetails.OverridePrice == 0 ? (SaleDetails.Price * SaleDetails.Quantity).ToString("F") : (SaleDetails.OverridePrice * SaleDetails.Quantity).ToString("F");
                        SubTotalForTax = SaleDetails.OverridePrice == 0 ? (SaleDetails.Price * SaleDetails.Quantity) : (SaleDetails.OverridePrice * SaleDetails.Quantity);
                        var overAll = SaleDetails.OverridePrice == 0 ? (SaleDetails.Price * SaleDetails.Quantity) : (SaleDetails.OverridePrice * SaleDetails.Quantity);
                        SubTotal = SubTotal + overAll;
                        //Tax map
                        foreach (CompanySalesTaxAccountMap SalesTax in CompanySalesTaxAccountMap)
                        {
                            foreach (TaxDetail Taxes in lSaleDetail.TaxDetails)
                            {
                                if (Taxes.TaxAccountId == SalesTax.MapId)
                                {
                                    salesTax += SalesTax.Name + " @" + Taxes.TaxRate.ToString() + "% ";
                                }
                            }
                        }
                        //Discount
                        if (lSaleDetail.Discounts.Count > 0)
                        {
                            salesTax += "\nDiscount @" + lSaleDetail.Discounts.Sum(X => X.Discount).ToString() + "%";
                            subTotal += "\n(" + Math.Round(lSaleDetail.Discounts.Sum(X => X.DiscountAmount), 2).ToString("F") + ")\n";
                            subTotal += SaleDetails.OverridePrice == 0 ? ((SaleDetails.Price * SaleDetails.Quantity) - lSaleDetail.Discounts.Sum(X => X.DiscountAmount)).ToString("F") : ((SaleDetails.OverridePrice * SaleDetails.Quantity) - lSaleDetail.Discounts.Sum(X => X.DiscountAmount)).ToString("F");
                            SubTotalForTax = SaleDetails.OverridePrice == 0 ? ((SaleDetails.Price * SaleDetails.Quantity) - lSaleDetail.Discounts.Sum(X => X.DiscountAmount)) : ((SaleDetails.OverridePrice * SaleDetails.Quantity) - lSaleDetail.Discounts.Sum(X => X.DiscountAmount));
                            Discount = Discount + lSaleDetail.Discounts.Sum(X => X.DiscountAmount);
                        }

                        count++;
                        //Tax details
                        foreach (TaxDetail Taxes in lSaleDetail.TaxDetails)
                        {
                            TaxTable TaxTable = null;
                            TaxTable = lTaxTable.FirstOrDefault(x => x.TaxPer == Taxes.TaxRate && x.SaleTaxMapId == Taxes.CatalogItemSalesTaxMap.SalesTaxMapId);
                            if (TaxTable == null)
                            {
                                TaxTable = new TaxTable();
                                TaxTable.TaxPer = Taxes.TaxRate;
                                TaxTable.TaxAmount = Taxes.Amount;
                                TaxTable.Id = Taxes.TaxAccountId;
                                TaxTable.SaleTaxMapId = Taxes.CatalogItemSalesTaxMap.SalesTaxMapId;
                                TaxTable.SubTotal = SubTotalForTax;
                                lTaxTable.Add(TaxTable);
                            }
                            else
                            {
                                lTaxTable.Remove(TaxTable);
                                TaxTable.TaxAmount += Taxes.Amount;
                                TaxTable.SubTotal += SubTotalForTax;
                                lTaxTable.Add(TaxTable);
                            }
                        }

                        //create new row
                        DataRow SaleDetailsTableNewRow = SaleDetailsTable.NewRow();


                        LintTotal = LintTotal + SaleDetails.Amount;
                        if (lSaleDetail.TaxDetails.Count > 0)
                        {
                            TaxTotal = TaxTotal + lSaleDetail.TaxDetails.Sum(X => X.Amount);
                            foreach (ItemLevelSaleTaxDetail Detail in lSaleDetail.TaxDetails)
                            {
                                if (AccountManager.Instance.GetAccountById((long)Detail.TaxAccountId).AccountGroupId == 801)
                                {
                                    SaleDetailsTableNewRow[(int)SalePrePrintDetailsTableColumn.CGST] = Detail.TaxRate.ToString();

                                }
                                else
                                {
                                    SaleDetailsTableNewRow[(int)SalePrePrintDetailsTableColumn.SGST] = Detail.TaxRate.ToString();

                                }
                            }
                        }
                        double TaxAmount = PdfDataAlignment.ProductTaxPercentage(lSaleDetail.TaxDetails, SaleEntry.SaleTaxType);

                        SaleDetailsTableNewRow[(int)SalePrePrintDetailsTableColumn.SNO] = count.ToString();
                        SaleDetailsTableNewRow[(int)SalePrePrintDetailsTableColumn.DESC] = SaleDetails.Product.Name.ToString() + salesTax;
                        SaleDetailsTableNewRow[(int)SalePrePrintDetailsTableColumn.HSN] = SaleDetails.Product.HSNCode == null ? "" : SaleDetails.Product.HSNCode.ToString();
                        if (SaleDetails.BatchNo != null)
                        {
                            SaleDetailsTableNewRow[(int)SalePrePrintDetailsTableColumn.BATCH] = SaleDetails.BatchNo.ToString();
                            SaleDetailsTableNewRow[(int)SalePrePrintDetailsTableColumn.BATCHEXPDATE] = SaleDetails.ExpDate.ToString(Global.Company.DateFormat);
                        }
                        else
                        {
                            SaleDetailsTableNewRow[(int)SalePrePrintDetailsTableColumn.BATCH] = "";
                            SaleDetailsTableNewRow[(int)SalePrePrintDetailsTableColumn.BATCHEXPDATE] = "";
                        }
                        SaleDetailsTableNewRow[(int)SalePrePrintDetailsTableColumn.RATE] = SaleDetails.OverridePrice == 0 ? SaleDetails.Price.ToString("F") : SaleDetails.OverridePrice.ToString("F");
                        SaleDetailsTableNewRow[(int)SalePrePrintDetailsTableColumn.QTY] = SaleDetails.Quantity.ToString();
                        SaleDetailsTableNewRow[(int)SalePrePrintDetailsTableColumn.FREE] = SaleDetails.FreeQuantity.ToString();
                        SaleDetailsTableNewRow[(int)SalePrePrintDetailsTableColumn.DISPER] = lSaleDetail.Discounts.Count > 0 ? lSaleDetail.Discounts.Sum(x => x.Discount).ToString("F") : "0.00"; ;
                        SaleDetailsTableNewRow[(int)SalePrePrintDetailsTableColumn.DISAMOUNT] = lSaleDetail.Discounts.Count > 0 ? lSaleDetail.Discounts.Sum(x => x.DiscountAmount).ToString("F") : "0.00";
                        SaleDetailsTableNewRow[(int)SalePrePrintDetailsTableColumn.SUBTOTAL] = SubTotalForTax;
                        SaleDetailsTableNewRow[(int)SalePrePrintDetailsTableColumn.TAXPER] = PdfDataAlignment.ProductTaxPercentage(SaleDetails.TaxDetails, SaleEntry.SaleTaxType).ToString("F");
                        SaleDetailsTableNewRow[(int)SalePrePrintDetailsTableColumn.TAXAMOUNT] = lSaleDetail.TaxDetails.Count > 0 ? lSaleDetail.TaxDetails.Sum(x => x.Amount).ToString("F") : "0.00";
                        SaleDetailsTableNewRow[(int)SalePrePrintDetailsTableColumn.LTOTAL] = Math.Round(SaleDetails.Amount, 2).ToString("F");

                        //Add row
                        SaleDetailsTable.Rows.Add(SaleDetailsTableNewRow);
                    }

                    SaleDataTableTotal.Columns.Add("1", typeof(string));
                    SaleDataTableTotal.Columns.Add("2", typeof(string));
                    SaleDataTableTotal.Columns.Add("3", typeof(string));
                    SaleDataTableTotal.Columns.Add("4", typeof(string));
                    SaleDataTableTotal.Columns.Add("5", typeof(string));

                    //Grand total
                    SaleDataTableTotal.Rows.Add(new object[] { "Grand Total", (SubTotal - Discount).ToString("F"), TaxWiseAmountTotals[0], TaxWiseAmountTotals[1], LintTotal.ToString("F") });

                    double LintTotalTrans = Math.Round(LintTotal, 2);
                    for (int i = 0; i < SaleEntry.SaleAdditionalTransactions.Count; i++)
                    {
                        if (SaleEntry.SaleAdditionalTransactions[i].Action == AdditionalTransactionAction.CR)
                        {
                            LintTotalTrans = LintTotalTrans + SaleEntry.SaleAdditionalTransactions[i].Amount;
                        }
                        else
                        {
                            LintTotalTrans = LintTotalTrans - SaleEntry.SaleAdditionalTransactions[i].Amount;
                        }
                        var TransType = SaleEntry.SaleAdditionalTransactions[i].Type == AdditionalTransactionType.PERCENT ? " @" + SaleEntry.SaleAdditionalTransactions[i].Value + "%" : "";
                        //Additional trans
                        SaleDataTableTotal.Rows.Add(new object[] { SaleEntry.SaleAdditionalTransactions[i].Name + TransType, Math.Round(SaleEntry.SaleAdditionalTransactions[i].Amount, 2).ToString("F"), "       ", "", Math.Round(LintTotalTrans, 2).ToString("F") });

                    }
                    //Round off
                    SaleDataTableTotal.Rows.Add(new object[] { "Round off", SaleEntry.RoundOff.ToString("F"), "      ", "", SaleEntry.NetAmount.ToString("F") });
                }
                catch (DocumentException dex)
                {
                    Console.WriteLine(dex.ToString());
                }
                catch (IOException ioex)
                {
                    Console.WriteLine(ioex.ToString());
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.ToString());
                }
            }
            if (SaleDetailsTable.Rows.Count != 0)
            {
                if (fileExtension == "Laser")
                {
                    SalePrintSaveA5A4New SalePrintSaveA5A4New = new SalePrintSaveA5A4New();
                    //Customer need
                    SalePrintSaveA5A4New.GeneratePDF(lTaxTable, SaleDetailsTable, SaleDataTableTotal, SaleEntry, PrintPaper, fileExtension, isPrint);
                    //Standrad
                    //GeneratePDF(lTaxTable,SaleDetailsTable, SaleDataTableTotal, SaleEntry, PrintPaper, fileExtension, isPrint);
                }
                else if (fileExtension == "Dotmatrix")
                {
                    fileName = SaleEntry.EntryType == Entrytype.SALE ? "SaleInvoice" : "Quotation";
                    SalePrintSaveDotmatrixPrePrintA5A4 SalePrintSaveDotmatrixPrePrintA5A4 = new SalePrintSaveDotmatrixPrePrintA5A4(this);
                    SalePrintSaveDotmatrixPrePrintA5A4.GenerateDotmarixTxt(SaleDetailsTable, SaleDataTableTotal, SaleEntry, PrintPaper, fileExtension, isPrint);
                }
            }
        }
    }
}

