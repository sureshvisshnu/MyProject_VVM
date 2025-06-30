using fa.api.Accounting;
using fa.api.OrderManagement;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.model.OrderManagement;
using Fa.Utils.utils;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VisioForge.MediaFramework.Helpers;
using Rectangle = iTextSharp.text.Rectangle;

namespace fa.views.utils.Sale
{
    class SalePrintSave105mm
    {
        string PaperFormat = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == Global.getCurrentFiscalYearStartDate() && x.YearEndDate == Global.getCurrentFiscalYearEndDate() && x.EntryType == EntryType.SALES).PrintPaperFormat.Name;
        public bool ExportToFileOrPrint(long SalesId, string fileExtension, bool isPrint)
        {
            SalesManager SalesManager = SalesManager.Instance;
            SaleEntry SaleEntry = SalesManager.GetSaleEntry(SalesId);
            List<CompanySalesTaxAccountMap> CompanySalesTaxAccountMap = (List<CompanySalesTaxAccountMap>)Global.Company.SalesTaxAccountMaps;
            List<string> headingText = new List<string>();
            string customerId = SaleEntry.AccountsId != null ? SaleEntry.AccountsId.ToString() : "";
            headingText.Add((SaleEntry.EntryType == Entrytype.RETURN ? "Return No : ": SaleEntry.EntryType == Entrytype.SALE ? "Invoice No : " : "Quote No : ")+ SaleEntry.RefNumber);
            headingText.Add("Date : " + SaleEntry.SaleDate.ToString(Global.Company.DateFormat));
            if (SaleEntry.AccountsId != null)
            {
                if (Global.Company.BusinessType == BuisnessType.Pharmacy || Global.Company.BusinessType == BuisnessType.Hospital)
                {
                    headingText.Add("Patient Name : " + SaleEntry.Account.DisplayAs);
                }
                else
                {
                    headingText.Add("Customer Name : " + SaleEntry.Account.DisplayAs);
                }
            }
            else
            {
                headingText.Add(SaleEntry.CustomerName);
            }
            if (SaleEntry.AccountsId != null)
            {
                Customer Customer = CustomerManager.Instance.GetCustomerById((long)SaleEntry.AccountsId);
                if (Customer != null && Customer.CustomerLicenceDetail.Count > 0)
                {
                    if (Customer.CustomerLicenceDetail != null && Customer.CustomerLicenceDetail.Count > 0)
                    {
                        headingText.Add(Customer.CustomerLicenceDetail.First().DisplayName+": " + Customer.CustomerLicenceDetail.First().Value);
                    }
                    else
                    {
                        headingText.Add(" ");
                    }
                }
                else
                {
                    Supplier Supplier = SupplierManager.Instance.GetSupplierById((long)SaleEntry.AccountsId);
                    if (Supplier != null && Supplier.SupplierLicenceDetail.Count > 0)
                    {
                        headingText.Add(Supplier.SupplierLicenceDetail.First().DisplayName + ": " + Supplier.SupplierLicenceDetail.First().Value);
                    }
                    else
                    {
                        headingText.Add(" ");
                    }
                }
            }
            else
            {
                headingText.Add(" ");
            }
            string BarcodeString = SaleEntry.RefNumber + "%" + SaleEntry.SaleDate.ToString(Global.Company.DateFormat);            
            DataGridView dgvTotal = new DataGridView();
            DataTable dt = new DataTable();
            string taxRow = "";
            List<string> InvoiceTotal = new List<string>();
            if (SaleEntry.SaleDetails.Count != 0)
            {
                try
                {
                    dt.Columns.Add("#", typeof(string));
                    dt.Columns["#"].Caption = "#";
                    dt.Columns.Add("particulars", typeof(string));
                    dt.Columns["particulars"].Caption = "Particulars";
                    dt.Columns.Add("cost", typeof(string));
                    dt.Columns["cost"].Caption = "Rate";
                    dt.Columns.Add("MRP", typeof(string));
                    dt.Columns["MRP"].Caption = "MRP";
                    dt.Columns.Add("qty", typeof(string));
                    dt.Columns["qty"].Caption = "Qty";
                    if (PaperFormat == "105 MM ROLL")
                    {
                        dt.Columns.Add("BatExp", typeof(string));
                        dt.Columns["BatExp"].Caption = "Bat/Exp";
                    }
                    else
                    {
                        dt.Columns.Add("dis", typeof(string));
                        dt.Columns["dis"].Caption = "Dis %";
                    }
                    dt.Columns.Add("tax", typeof(string));
                    dt.Columns["tax"].Caption = "Tax %";
                    dt.Columns.Add("amount", typeof(string));
                    dt.Columns["amount"].Caption = "Amount";
                    int count = 0;
                    List<string> OverAllTaxAmount = new List<string>();
                    List<TaxDetail> OverAllTaxByPer = new List<TaxDetail>();
                    double salesTotal = 0;
                    double taxTotal = 0;
                    double discountTotal = 0;
                    double qtyTotal = 0;
                    foreach (SaleDetail SaleDetails in SaleEntry.SaleDetails.OrderBy(x=>x.Id))
                    {
                        SaleDetail lSaleDetail = SalesManager.GetSaleDetail(SaleDetails.Id);
                        string salesTax = "";
                        double salesTaxPer = 0.00;
                        string salesDiscount = "0.00";
                        foreach (CompanySalesTaxAccountMap SalesTax in CompanySalesTaxAccountMap)
                        {
                            foreach (TaxDetail Taxes in lSaleDetail.TaxDetails)
                            {
                                if (Taxes.TaxAccountId == SalesTax.AccountId)
                                {
                                    salesTax = Taxes.TaxRate.ToString();
                                    salesTaxPer += Taxes.TaxRate;

                                    bool updateStatus = true;
                                    if (OverAllTaxByPer.Count != 0)
                                    {
                                        for (int i = 0; i < OverAllTaxByPer.Count; i++)
                                        {
                                            if (OverAllTaxByPer[i].TaxAccountId == Taxes.TaxAccountId && OverAllTaxByPer[i].TaxRate == Taxes.TaxRate)
                                            {
                                                OverAllTaxByPer[i].Amount += Taxes.Amount;
                                                updateStatus = false;
                                            }
                                        }
                                        if (updateStatus)
                                        {
                                            OverAllTaxByPer.Add(Taxes);
                                        }
                                    }
                                    else
                                    {
                                        OverAllTaxByPer.Add(Taxes);
                                    }
                                }
                            }
                        }
                        if (lSaleDetail.Discounts.Count > 0)
                        {
                            salesDiscount = lSaleDetail.Discounts.First().DiscountAmount.ToString("F");
                            discountTotal += lSaleDetail.Discounts.First().DiscountAmount;
                        }                        
                        count++;
                        DataRow drNewRow = dt.NewRow();
                        double TaxAmount = PdfDataAlignment.ProductTaxPercentage(lSaleDetail.TaxDetails, SaleEntry.SaleTaxType);
                        drNewRow["#"] = count.ToString();
                        if (PaperFormat == "80 MM ROLL")
                        {
                            drNewRow["particulars"] = SaleDetails.Product.Name.ToString()+" @"+ SaleDetails.TaxDetails.Sum(X=>X.TaxRate) + "%" + " @UOM " + SaleDetails.Uom.ToString();
                        }
                        else
                        {
                            //drNewRow["particulars"] = SaleDetails.Product.Name.ToString() + " @UOM " + SaleDetails.Uom.ToString() + (SaleDetails.isBatch?"\n Bat.No: "+SaleDetails.BatchNo+", Exp.: "+SaleDetails.ExpDate.Month.ToString()+"/"+ SaleDetails.ExpDate.Year.ToString() : "");
                            drNewRow["particulars"] = SaleDetails.Product.Name.ToString() + "\nUOM : " + SaleDetails.Uom.ToString() + (double.Parse(salesDiscount) != 0.00 ? "\nDis : " + salesDiscount + "Rs @" + lSaleDetail.Discounts.First().Discount + "%" : "");
                        }
                        
                        drNewRow["cost"] = SaleDetails.Price.ToString("F");
                        drNewRow["MRP"] = SaleDetails.Msrp.ToString("F");
                        drNewRow["qty"] = SaleDetails.Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        if (PaperFormat == "105 MM ROLL")
                        {
                            drNewRow["BatExp"] = SaleDetails.isBatch ? SaleDetails.BatchNo + "\n" + SaleDetails.ExpDate.Month.ToString() + "/" + SaleDetails.ExpDate.Year.ToString() : "";
                        }
                        else
                        {
                            drNewRow["dis"] = salesDiscount;
                        }
                        drNewRow["tax"] = salesTaxPer.ToString("F");
                        drNewRow["amount"] = Math.Round(SaleDetails.Amount, 2).ToString("F");
                        salesTotal += SaleDetails.Amount;
                        qtyTotal += SaleDetails.Quantity;
                        dt.Rows.Add(drNewRow);
                    }
                    //OverAllTaxAmount
                    for (int i = 0; i < OverAllTaxByPer.Count; i++)
                    {
                        var Comma = i == OverAllTaxByPer.Count - 1 ? "" : ", ";
                        string TypeName = CompanySalesTaxAccountMap.Where(t => t.AccountId == OverAllTaxByPer[i].TaxAccountId).Select(t => t.Name).FirstOrDefault().ToString();
                        taxRow += TypeName.ToString() + " " + OverAllTaxByPer[i].TaxRate.ToString() + " % : Rs. " + OverAllTaxByPer[i].Amount.ToString("F") + Comma;
                        taxTotal += OverAllTaxByPer[i].Amount;
                    }
                    OverAllTaxAmount.Add(taxRow);
                    InvoiceTotal.Add("");
                    InvoiceTotal.Add("");
                    InvoiceTotal.Add("");
                    InvoiceTotal.Add("");
                    InvoiceTotal.Add(qtyTotal.ToString());
                    if (PaperFormat == "105 MM ROLL")
                    {
                        InvoiceTotal.Add("");
                    }
                    else
                    {
                        InvoiceTotal.Add(discountTotal.ToString("F"));
                    }
                    InvoiceTotal.Add(taxTotal.ToString("F"));
                    InvoiceTotal.Add(salesTotal.ToString("F"));
                    InvoiceTotal.Add(taxRow);
                }
                #pragma warning disable 0168 // variable declared but not used.
                catch (DocumentException dex)
                {
                    MessageBox.Show("DocumentException");
                }
                catch (IOException ioex)
                {
                    MessageBox.Show("IOException");
                }
                catch (Exception ee)
                {
                    MessageBox.Show("File Error Please Contact System Admin");
                    Console.WriteLine(ee.ToString());
                }
                #pragma warning restore 0168
            }
            if (true)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var dataTable = dt;
                            if (dataTable != null)
                            {
                                GeneratePDF(SaleEntry,dataTable, PdfDataAlignment.DataGridViewAsDataTable(dgvTotal), headingText, InvoiceTotal, BarcodeString, SaleEntry.EntryType == Entrytype.SALE ? "SaleInvoice" : SaleEntry.EntryType == Entrytype.RETURN ? "ReturnInvoice" : "Quotation", fileExtension, isPrint, SaleEntry.Memo);
                                break;
                            }
                            else
                                break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("File Error Please Contact System Admin");
                    Console.WriteLine(e.ToString());
                }
            }
            return true;
        }
        public PdfPTable MainHeader(SaleEntry SaleEntry)
        {
            PdfPTable Table = new PdfPTable(7);
            float[] widths = new float[] { 5f, 40f, 13f, 10f, 15f, 15f, 17f };
            Table.SetTotalWidth(widths);
            PdfPCell TableCell = new PdfPCell();            
            if (Global.getLogoAsBytes() != null)
            {
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Global.getLogoAsBytes());
                image.ScaleToFit(100f, 20f);
                image.ScaleAbsolute(40, 40);
                var ImgData = image;
                TableCell = new PdfPCell(image);
                TableCell.BorderColor = BaseColor.WHITE;
                //TableCell.MinimumHeight = 20;
                TableCell.Padding = 4;
                TableCell.Colspan = 7;
                TableCell.HorizontalAlignment = Element.ALIGN_CENTER;
                Table.AddCell(TableCell);
            }
            string CompanyName = Global.Company.DisplayAs+(string.IsNullOrEmpty(Global.Company.Slogan) ? "" : "\n" + Global.Company.Slogan);
            TableCell = new PdfPCell(new Phrase(CompanyName, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            TableCell.BorderColor = BaseColor.WHITE;
            TableCell.MinimumHeight = 15;
            TableCell.Padding = 2;
            TableCell.Colspan = 7;
            TableCell.VerticalAlignment = Element.ALIGN_CENTER;
            TableCell.HorizontalAlignment = Element.ALIGN_CENTER;
            Table.AddCell(TableCell);
            if (Global.Company.Address != null && !string.IsNullOrEmpty(Global.Company.Address.FullAddressInSingleLine))
            {
                TableCell = new PdfPCell(new Phrase(Global.Company.Address.FullAddressInSingleLine, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                TableCell.BorderColor = BaseColor.WHITE;
                TableCell.MinimumHeight = 10;
                TableCell.Padding = 2;
                TableCell.Colspan = 7;
                TableCell.HorizontalAlignment = Element.ALIGN_CENTER;
                Table.AddCell(TableCell);
            }
            string contactNo = string.Empty;
            if (Global.Company.ContactInfo != null && !string.IsNullOrEmpty(Global.Company.ContactInfo.Phone.ToString()))
            {
                contactNo = "Contact : " + Global.Company.ContactInfo.Phone.ToString();
            }
            TableCell = new PdfPCell(new Phrase(contactNo, PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            TableCell.BorderColor = BaseColor.WHITE;
            TableCell.MinimumHeight = 10;
            TableCell.Padding = 2;
            TableCell.Colspan = 3;
            TableCell.HorizontalAlignment = Element.ALIGN_LEFT;
            Table.AddCell(TableCell);

            string CompanyGSTNo = "";
            string ReferrerName = string.Empty;
            ReferrerName = SaleEntry.ReferedById != null ? SaleEntry.ReferedBy.Name : string.Empty;
            if (!string.IsNullOrEmpty(ReferrerName) || Global.Company.CompanyLicence.Count > 0)
            {
                CompanyGSTNo = Global.Company.CompanyLicence.Count > 0
                ? "GSTIN: " + Global.Company.CompanyLicence.First().Value + (ReferrerName != string.Empty ? "\nReffered By : " + ReferrerName : "")
                : (ReferrerName != string.Empty ? "Reffered By : " + ReferrerName : "");
            }
            TableCell = new PdfPCell(new Phrase(CompanyGSTNo, PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            TableCell.BorderColor = BaseColor.WHITE;
            TableCell.MinimumHeight = 10;
            TableCell.Padding = 2;
            TableCell.Colspan = 4;
            TableCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            Table.AddCell(TableCell);
            TableCell = new PdfPCell(new Phrase(SaleEntry.EntryType == Entrytype.SALE ? "TAX INVOICE" : SaleEntry.EntryType == Entrytype.RETURN ? "RETURN INVOICE" : "QUOTATION", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            TableCell.UseVariableBorders = true;
            TableCell.BorderColorLeft = BaseColor.WHITE;
            TableCell.BorderColorTop = BaseColor.WHITE;
            TableCell.BorderColorRight = BaseColor.WHITE;
            TableCell.BorderColorBottom = BaseColor.BLACK;
            TableCell.MinimumHeight = 10;
            TableCell.Padding = 4;
            TableCell.Colspan = 7;
            TableCell.HorizontalAlignment = Element.ALIGN_CENTER;
            Table.AddCell(TableCell);
            return Table;
        }
        public PdfPTable SubHeader(List<string> heading)
        {
            PdfPTable Table = new PdfPTable(7);
            float[] widths = new float[] { 5f, 40f, 13f, 10f, 15f, 15f, 17f };
            Table.SetTotalWidth(widths);
            PdfPCell TableCell = new PdfPCell();
            TableCell = new PdfPCell(new Phrase(heading[0], PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            TableCell.UseVariableBorders = true;
            TableCell.BorderColorLeft = BaseColor.WHITE;
            TableCell.BorderColorTop = BaseColor.BLACK;
            TableCell.BorderColorRight = BaseColor.WHITE;
            TableCell.BorderColorBottom = BaseColor.WHITE;
            TableCell.Padding = 4;
            TableCell.Colspan = 3;
            TableCell.HorizontalAlignment = Element.ALIGN_LEFT;
            Table.AddCell(TableCell);
            TableCell = new PdfPCell(new Phrase(heading[1], PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            TableCell.UseVariableBorders = true;
            TableCell.BorderColorLeft = BaseColor.WHITE;
            TableCell.BorderColorTop = BaseColor.BLACK;
            TableCell.BorderColorRight = BaseColor.WHITE;
            TableCell.BorderColorBottom = BaseColor.WHITE;
            TableCell.Padding = 4;
            TableCell.Colspan = 4;
            TableCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            Table.AddCell(TableCell);
            TableCell = new PdfPCell(new Phrase(heading[2], PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            TableCell.UseVariableBorders = true;
            TableCell.BorderColorLeft = BaseColor.WHITE;
            TableCell.BorderColorTop = BaseColor.WHITE;
            TableCell.BorderColorRight = BaseColor.WHITE;
            TableCell.BorderColorBottom = BaseColor.BLACK;
            TableCell.Padding = 4;
            TableCell.Colspan = 3;
            TableCell.HorizontalAlignment = Element.ALIGN_LEFT;
            Table.AddCell(TableCell);
            TableCell = new PdfPCell(new Phrase(heading[3], PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            TableCell.UseVariableBorders = true;
            TableCell.BorderColorLeft = BaseColor.WHITE;
            TableCell.BorderColorTop = BaseColor.WHITE;
            TableCell.BorderColorRight = BaseColor.WHITE;
            TableCell.BorderColorBottom = BaseColor.BLACK;
            TableCell.Padding = 4;
            TableCell.Colspan = 4;
            TableCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            Table.AddCell(TableCell);
            return Table;
        }
        public PdfPTable TableColoumn(DataTable dataTable)
        {
            PdfPTable Table = new PdfPTable(8);
            float[] widths = new float[] { 5f, 30f, 13f, 13f, 10f, 15f, 15f, 17f };
            if (PaperFormat == "80 MM ROLL")
            {
                Table = new PdfPTable(5);
                widths = new float[] { 5f, 40f, 13f,  10f, 17f };
            }
            Table.SetTotalWidth(widths);
            PdfPCell TableCell = new PdfPCell();
            int count = 0;
            foreach (DataColumn column in dataTable.Columns)
            {
                if(PaperFormat == "80 MM ROLL" && 
                    (column.ColumnName == "MRP" || column.ColumnName=="dis" || column.ColumnName == "tax")) { continue; }
                count++;
                TableCell = new PdfPCell(new Phrase(column.Caption, PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                TableCell.UseVariableBorders = true;
                TableCell.BorderColorLeft = BaseColor.WHITE;
                TableCell.BorderColorTop = BaseColor.BLACK;
                TableCell.BorderColorRight = BaseColor.WHITE;
                TableCell.BorderColorBottom = BaseColor.BLACK;
                TableCell.MinimumHeight = 20;
                TableCell.Padding = 4;
                TableCell.Rowspan = 2;
                if (count > 2)
                {
                    TableCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                }
                Table.AddCell(TableCell);
            }
            return Table;
        }
        double Rounds = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == Global.getCurrentFiscalYearStartDate() && x.YearEndDate == Global.getCurrentFiscalYearEndDate() && x.EntryType == EntryType.SALES).RoundOff;
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
                else
                {
                    _roundoff = -mod;
                }
            }
            return _roundoff;
        }
        public void GeneratePDF(SaleEntry SaleEntry, DataTable dataTable, DataTable totalTable, List<string> heading, List<string> invoiceContent, string BarcodeString, string fileName, string fileExtension, bool isPrint,string Memo)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4.Rotate(), -20, -20, 0, 0);
                pdfDoc.SetPageSize(new Rectangle(280, 440));
                if (PaperFormat == "80 MM ROLL")
                {
                    pdfDoc.SetPageSize(new Rectangle(210, 250));
                }
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();
                int cols = dataTable.Columns.Count;
                int rows = dataTable.Rows.Count;
                PdfPTable PatientHeader = !string.IsNullOrEmpty(Memo) ?PdfDataAlignment.PatientDetailHeader105(Memo) : null;
                PdfPTable MHeader = MainHeader(SaleEntry);
                PdfPTable SHeader = SubHeader(heading);
                PdfPTable TColoumn = TableColoumn(dataTable);
                PdfPTable Table = new PdfPTable(8);
                float[] widths = new float[] { 5f, 30f, 13f, 13f, 10f, 15f, 15f, 17f };
                if(PaperFormat == "80 MM ROLL")
                {
                     Table = new PdfPTable(5);
                    widths = new float[] { 5f, 40f, 13f, 10f,17f };
                }
                Table.SetTotalWidth(widths);
                PdfPCell TableCell = new PdfPCell();               
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (PaperFormat == "80 MM ROLL" && 
                            (j == 3 || j == 5 || j == 6)) { continue; }
                        var temp = dataTable.Rows[i][j].ToString();
                        TableCell = new PdfPCell(new Phrase(temp, PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                        TableCell.UseVariableBorders = true;
                        TableCell.BorderColorLeft = BaseColor.WHITE;
                        TableCell.BorderColorTop = BaseColor.WHITE;
                        TableCell.BorderColorRight = BaseColor.WHITE;
                        TableCell.BorderColorBottom = BaseColor.WHITE;
                        if (j > 1)
                        {
                            TableCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else
                        {
                            TableCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        Table.AddCell(TableCell);
                    }
                }
                //double THeight = PdfDataAlignment.CalculatePdfTableHeight(Table) + MHeader.TotalHeight + SHeader.TotalHeight + TColoumn.TotalHeight;                
                //double dummySpace = ((Global.Company.CompanySalesSetup.IsBankDetailDisplayOnInvoice
                //    && !string.IsNullOrEmpty(Global.Company.CompanySalesSetup.BankDetails)) ||
                //    (Global.Company.CompanySalesSetup.IsDeclarationDisplayOnInvoice
                //    && !string.IsNullOrEmpty(Global.Company.CompanySalesSetup.Declarations))?230:330) - THeight;                
                //if (dummySpace > 0)
                //{
                //    TableCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                //    TableCell.UseVariableBorders = true;
                //    TableCell.BorderColorLeft = BaseColor.WHITE;
                //    TableCell.BorderColorTop = BaseColor.WHITE;
                //    TableCell.BorderColorRight = BaseColor.WHITE;
                //    TableCell.BorderColorBottom = BaseColor.WHITE;
                //    TableCell.Colspan = PaperFormat == "80 MM ROLL"?6:8;
                //    TableCell.MinimumHeight = (float)dummySpace;
                //    Table.AddCell(TableCell);
                //}
                double totalInvoiceAmount = 0;
                for (int k = 0; k < invoiceContent.Count; k++)
                {
                    if (PaperFormat == "80 MM ROLL" &&(k==1||k==2)) { continue; }
                    if (PaperFormat == "80 MM ROLL" &&
                            ((k == 1 && double.Parse(invoiceContent[5]) > 0)  || k == 5 || k == 6)) { continue; }

                    if (PaperFormat == "80 MM ROLL" &&
                           (k == 0 && double.Parse(invoiceContent[5])>0))
                    {
                        TableCell = new PdfPCell(new Phrase("Discount: "+ invoiceContent[5], PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                        TableCell.UseVariableBorders = true;
                        TableCell.BorderColorLeft = BaseColor.WHITE;
                        TableCell.BorderColorTop = BaseColor.BLACK;
                        TableCell.BorderColorRight = BaseColor.WHITE;
                        TableCell.BorderColorBottom = BaseColor.BLACK;
                        TableCell.Colspan = 2;
                        Table.AddCell(TableCell);
                        continue;
                    }        
                    if (k == invoiceContent.Count - 2)
                    {
                        totalInvoiceAmount = double.Parse(invoiceContent[k]);
                    }
                    if (k == invoiceContent.Count - 1)
                    {
                        TableCell = new PdfPCell(new Phrase(invoiceContent[k], PdfDataAlignment.GetFont(PaperFormat == "80 MM ROLL" ? "Font_Normal_Italic_6_Black" : "Font_Normal_Italic_6_Black")));
                        TableCell.UseVariableBorders = true;
                        TableCell.BorderColorLeft = BaseColor.WHITE;
                        TableCell.BorderColorTop = BaseColor.WHITE;
                        TableCell.BorderColorRight = BaseColor.WHITE;
                        TableCell.BorderColorBottom = BaseColor.WHITE;
                        TableCell.Colspan = PaperFormat == "80 MM ROLL" ? 5 : 8; 
                        Table.AddCell(TableCell);
                    }
                    else
                    {
                        TableCell = new PdfPCell(new Phrase(invoiceContent[k], PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                        TableCell.UseVariableBorders = true;
                        TableCell.BorderColorLeft = BaseColor.WHITE;
                        TableCell.BorderColorTop = BaseColor.BLACK;
                        TableCell.BorderColorRight = BaseColor.WHITE;
                        TableCell.BorderColorBottom = BaseColor.BLACK;
                        TableCell.Colspan = (PaperFormat == "80 MM ROLL" && k != 0) ? 1 : 1;
                        TableCell.MinimumHeight = 10;
                        if (k > 1)
                        {
                            TableCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        Table.AddCell(TableCell);
                    }
                }
                MemoryStream Barcode = new MemoryStream();
                var BarcodeImg = BarCode.GenerateImageBarcode2(BarcodeString.Trim());
                BarcodeImg.Save(Barcode, System.Drawing.Imaging.ImageFormat.Png);
                iTextSharp.text.Image barcodeImg = iTextSharp.text.Image.GetInstance(Barcode.ToArray());
                barcodeImg.ScaleAbsolute(185, 30);
                var Temp = barcodeImg;
                TableCell = new PdfPCell(new Phrase(""));
                TableCell.UseVariableBorders = true;
                TableCell.BorderColorLeft = BaseColor.WHITE;
                TableCell.BorderColorTop = BaseColor.BLACK;
                TableCell.BorderColorRight = BaseColor.WHITE;
                TableCell.BorderColorBottom = BaseColor.WHITE;
                TableCell.Colspan = PaperFormat == "80 MM ROLL" ? 2 : 5;
                TableCell.Padding = 5;
                TableCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                Table.AddCell(TableCell);

                double RoundoffAmount = Rounds > 0 ? RoundOff(totalInvoiceAmount) : 0;
                double overallTotal = totalInvoiceAmount + RoundoffAmount;

                TableCell = new PdfPCell(new Phrase(overallTotal.ToString("F"), PdfDataAlignment.GetFont((PaperFormat == "80 MM ROLL" ? "Font_Bold_Italic_10_Black" : "Font_Bold_Italic_16_Black"))));
                TableCell.UseVariableBorders = true;
                TableCell.BorderColorLeft = BaseColor.WHITE;
                TableCell.BorderColorTop = BaseColor.BLACK;
                TableCell.BorderColorRight = BaseColor.WHITE;
                TableCell.BorderColorBottom = BaseColor.WHITE;
                TableCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                TableCell.Colspan = 3;
                Table.AddCell(TableCell);
                TableCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont((PaperFormat == "80 MM ROLL" ? "Font_Normal_Italic_5_Black" : "Font_Normal_Italic_6_Black"))));
                TableCell.UseVariableBorders = true;
                TableCell.BorderColorLeft = BaseColor.WHITE;
                TableCell.BorderColorTop = BaseColor.WHITE;
                TableCell.BorderColorRight = BaseColor.WHITE;
                TableCell.BorderColorBottom = BaseColor.WHITE;
                TableCell.Colspan = (PaperFormat == "80 MM ROLL" ? 5 : 8);
                TableCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                Table.AddCell(TableCell);

                bool IsBank = Global.Company.CompanySalesSetup.IsBankDetailDisplayOnInvoice
                    && !string.IsNullOrEmpty(Global.Company.CompanySalesSetup.BankDetails) ? true : false;
                bool IsDeclaration = Global.Company.CompanySalesSetup.IsDeclarationDisplayOnInvoice
                        && !string.IsNullOrEmpty(Global.Company.CompanySalesSetup.Declarations) ? true : false;
                bool IsUPI = Global.Company.CompanySalesSetup.IsPrintQRCode;
                int Cspan = (IsBank && IsDeclaration) ? PaperFormat == "80 MM ROLL" ? 2 : 4 : PaperFormat == "80 MM ROLL" ? 5 : 8;
                if (IsBank)
                {
                    TableCell = new PdfPCell(new Phrase(Global.Company.CompanySalesSetup.BankDetails.Replace(",", System.Environment.NewLine), PdfDataAlignment.GetFont((PaperFormat == "80 MM ROLL" ? "Font_Normal_Italic_4_Black" : "Font_Normal_Italic_5_Black"))));
                    TableCell.UseVariableBorders = true;
                    TableCell.BorderColorLeft = BaseColor.WHITE;
                    TableCell.BorderColorTop = BaseColor.WHITE;
                    TableCell.BorderColorRight = BaseColor.WHITE;
                    TableCell.BorderColorBottom = BaseColor.WHITE;
                    TableCell.Colspan = Cspan;
                    TableCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    Table.AddCell(TableCell);
                }
                if (IsDeclaration)
                {
                    TableCell = new PdfPCell(new Phrase(Global.Company.CompanySalesSetup.Declarations.Replace(",", System.Environment.NewLine), PdfDataAlignment.GetFont((PaperFormat == "80 MM ROLL" ? "Font_Normal_Italic_4_Black" : "Font_Normal_Italic_5_Black"))));
                    TableCell.UseVariableBorders = true;
                    TableCell.BorderColorLeft = BaseColor.WHITE;
                    TableCell.BorderColorTop = BaseColor.WHITE;
                    TableCell.BorderColorRight = BaseColor.WHITE;
                    TableCell.BorderColorBottom = BaseColor.WHITE;
                    TableCell.Colspan = IsBank && IsDeclaration && PaperFormat == "80 MM ROLL" ? 3 : Cspan;
                    TableCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    Table.AddCell(TableCell);
                }
                Cspan = PaperFormat == "80 MM ROLL" ? 5 : 8;
                if (IsUPI)
                {
                    string UpiUrl = Global.Company.CompanySalesSetup.UPIId;
                    UpiUrl = UpiUrl.Replace("&am=100.00", "&am=" + totalInvoiceAmount.ToString(Global.Company.PrimaryCurrency.CurrencyFormat).Replace(",", ""));
                    MemoryStream MStream = new MemoryStream();
                    var Image = QRCode.GenerateQRCode(UpiUrl);
                    Image.Save(MStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(MStream.ToArray());
                    image.ScaleAbsoluteHeight(40);
                    image.ScaleAbsoluteWidth(70);
                    TableCell = new PdfPCell(new Phrase("Scan QR Code To Pay.", PdfDataAlignment.GetFont((PaperFormat == "80 MM ROLL" ? "Font_Normal_Italic_5_Black" : "Font_Normal_Italic_5_Black"))));
                    TableCell.UseVariableBorders = true;
                    TableCell.BorderColorLeft = BaseColor.WHITE;
                    TableCell.BorderColorTop = BaseColor.WHITE;
                    TableCell.BorderColorRight = BaseColor.WHITE;
                    TableCell.BorderColorBottom = BaseColor.WHITE;
                    TableCell.Colspan = Cspan;
                    TableCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    Table.AddCell(TableCell);

                    TableCell = new PdfPCell(new Phrase(" ", PdfDataAlignment.GetFont((PaperFormat == "80 MM ROLL" ? "Font_Normal_Italic_4_Black" : "Font_Normal_Italic_5_Black"))));
                    TableCell.UseVariableBorders = true;
                    TableCell.BorderColorLeft = BaseColor.WHITE;
                    TableCell.BorderColorTop = BaseColor.WHITE;
                    TableCell.BorderColorRight = BaseColor.WHITE;
                    TableCell.BorderColorBottom = BaseColor.WHITE;
                    TableCell.Colspan = Cspan;
                    TableCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    Table.AddCell(TableCell);

                    TableCell=new PdfPCell(image);
                    TableCell.UseVariableBorders = true;
                    TableCell.BorderColorLeft = BaseColor.WHITE;
                    TableCell.BorderColorTop = BaseColor.WHITE;
                    TableCell.BorderColorRight = BaseColor.WHITE;
                    TableCell.BorderColorBottom = BaseColor.WHITE;
                    TableCell.Colspan = Cspan;
                    TableCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    Table.AddCell(TableCell);
                }
                var foot = "Printed On: " + DateTime.Now.Date.ToString(Global.Company.DateFormat);
                TableCell = new PdfPCell(new Phrase(foot, PdfDataAlignment.GetFont((PaperFormat == "80 MM ROLL" ? "Font_Bold_Italic_5_Black" : "Font_Bold_Italic_6_Black"))));
                TableCell.UseVariableBorders = true;
                TableCell.BorderColorLeft = BaseColor.WHITE;
                TableCell.BorderColorTop = BaseColor.WHITE;
                TableCell.BorderColorRight = BaseColor.WHITE;
                TableCell.BorderColorBottom = BaseColor.WHITE;
                TableCell.Colspan = PaperFormat == "80 MM ROLL"?2:3;
                Table.AddCell(TableCell);
                foot = "Thank you for your business! ";
                TableCell = new PdfPCell(new Phrase(foot, PdfDataAlignment.GetFont((PaperFormat == "80 MM ROLL" ? "Font_Bold_Italic_5_Black" : "Font_Bold_Italic_6_Black"))));
                TableCell.UseVariableBorders = true;
                TableCell.BorderColorLeft = BaseColor.WHITE;
                TableCell.BorderColorTop = BaseColor.WHITE;
                TableCell.BorderColorRight = BaseColor.WHITE;
                TableCell.BorderColorBottom = BaseColor.WHITE;
                TableCell.Colspan = Global.Company.IdSpaces.FirstOrDefault(x=>x.YearStartDate==Global.getCurrentFiscalYearStartDate() && x.YearEndDate == Global.getCurrentFiscalYearEndDate() && x.EntryType==EntryType.SALES).PrintPaperFormat.Name == "80 MM ROLL" ? 3 : 5;
                TableCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                Table.AddCell(TableCell);               
                pdfDoc.Add(MHeader);
                if(!string.IsNullOrEmpty(Memo))
                {
                    pdfDoc.Add(PatientHeader);
                }
                pdfDoc.Add(SHeader);
                pdfDoc.Add(TColoumn);
                pdfDoc.Add(Table);
                pdfDoc.Close();
                PdfGeneration.SaveMemoryStream(myMemoryStream, fileName, fileExtension, isPrint,PaperTypes.MM_105);
            }
        }
    }
}
