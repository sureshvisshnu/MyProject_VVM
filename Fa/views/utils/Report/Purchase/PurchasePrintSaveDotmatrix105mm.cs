using fa;
using fa.model.Accounting.Masters;
using fa.model.OrderManagement;
using fa.views.utils;
using fa.views.utils.Common;
using fa.model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using fa.api.Accounting;
using iTextSharp.text;
using fa.api.OrderManagement;

namespace Fa.views.utils.Report.Purchase
{
    class PurchasePrintSaveDotmatrix105mm
    {
        DotmatrixPrint DotmatrixPrint = new DotmatrixPrint();
        Entrytype Entrytype = Entrytype.SALE;
        System.IO.StreamWriter rdr;

        public string tran_type;
        public string Discount;
        public string bill_amt;
        public string NetAmount;
        public string reciept_amount;
        public decimal MRPTotal = 0, SavedTotal = 0;
        public int count;
        private int Line_Char = 70;

        public PurchasePrintSaveDotmatrix105mm()
        {
            var windowsTempPath = System.IO.Path.GetTempPath();
            Directory.CreateDirectory(windowsTempPath + "");
            rdr = new System.IO.StreamWriter(windowsTempPath + "PurchasePrint" + ".txt");
        }

        public void Close()
        {
            rdr.Close();
        }
        private void PrintBill(DataTable dataTable, DataTable totalTable, List<string> heading, string CusAddress, List<string> invoiceContent, string BarcodeString, string fileName, string fileExtension, bool isPrint)
        {
            PrintHeader(heading, CusAddress);
            PrintDetails(dataTable, invoiceContent);
            if (Global.Company.CompanySalesSetup.IsBankDetailDisplayOnInvoice
                    && !string.IsNullOrEmpty(Global.Company.CompanySalesSetup.BankDetails))
            {
                rdr.WriteLine(Global.Company.CompanySalesSetup.BankDetails.Replace(",", System.Environment.NewLine));
            }
            if (Global.Company.CompanySalesSetup.IsDeclarationDisplayOnInvoice
                   && !string.IsNullOrEmpty(Global.Company.CompanySalesSetup.Declarations))
            {
                rdr.WriteLine(Global.Company.CompanySalesSetup.Declarations.Replace(",", System.Environment.NewLine));
            }
            //PrintFooter();
            SkipLine(3);
            Close();
            DotmatrixPrint.DoPrint(fileName, isPrint);
        }
        public void PrintHeader(List<string> heading, string CusAddress)
        {
            string CompanyName = Global.Company.DisplayAs;
            string CompanySlogan = Global.Company.Slogan;
            string blankSpace = "";
            if (CompanyName.Count() < Line_Char)
            {
                int ct = CompanyName.Count();
                for (var i = 0; i < (ct - Line_Char) / 2; i++)
                {
                    blankSpace += " ";
                }
            }
            CompanyName = blankSpace + CompanyName;
            string Address = string.Empty;
            string Phone = string.Empty;
            string Email = string.Empty;
            string Web = string.Empty;
            string contactNo = Global.Company.ContactInfo != null && !string.IsNullOrEmpty(Global.Company.ContactInfo.Phone.ToString()) ? "Contact : " + Global.Company.ContactInfo.Phone.ToString() : "";
            Address CAddress = Global.Company.Address;
            if (CAddress != null)
            {
                Address = CAddress.FullAddressInSingleLine;
            }
            rdr.Write(DotmatrixPrint.compressed_On);
            rdr.WriteLine(DotmatrixPrint.Bold_On + GetCenterText(CompanyName) + DotmatrixPrint.Bold_Off);
            if (!string.IsNullOrEmpty(CompanySlogan))
            {
                if (CompanySlogan.Count() < Line_Char)
                {
                    int ct = CompanySlogan.Count();
                    for (var i = 0; i < (ct - Line_Char) / 2; i++)
                    {
                        blankSpace += " ";
                    }
                }
                CompanySlogan = blankSpace + CompanyName[1];
                rdr.WriteLine(DotmatrixPrint.Bold_On + GetCenterText(CompanySlogan) + DotmatrixPrint.Bold_Off);
            }
            rdr.WriteLine();
            rdr.WriteLine(GetCenterText(Address));

            int rLoc = Line_Char - Address.Length;
            if (rLoc < 0)
            {
                rdr.WriteLine(GetCenterText(Address.Substring(71, Math.Abs(rLoc) - 1)));
            }
            if (Global.Company.CompanyLicence != null && Global.Company.CompanyLicence.Count > 0)
            {
                int i = 0;
                foreach (CompanyLicence licence in Global.Company.CompanyLicence)
                {
                    if (i > 1) { break; }
                    if (i == 0)
                    {
                        rdr.WriteLine(GetFormatedText(contactNo, (Line_Char / 2), AlignmentTypes.Suffix) + GetFormatedText(licence.DisplayName + ": " + licence.Value, (Line_Char / 2), AlignmentTypes.Prefix));
                    }
                    else
                    {
                        rdr.WriteLine(GetFormatedText("", (Line_Char / 2), AlignmentTypes.Suffix) + GetFormatedText(licence.DisplayName + ": " + licence.Value, (Line_Char / 2), AlignmentTypes.Prefix));
                    }
                    i++;
                }
            }
            else
            {
                rdr.WriteLine(GetFormatedText(contactNo, (Line_Char / 2), AlignmentTypes.Suffix) + GetFormatedText("", (Line_Char / 2), AlignmentTypes.Prefix));
            }
            rdr.WriteLine(DotmatrixPrint.Bold_On + GetCenterText(Entrytype == Entrytype.SALE ? "TAX INVOICE" : "QUOTATION") + DotmatrixPrint.Bold_Off);
            PrintLine();
            rdr.WriteLine(GetFormatedText(heading[0], (Line_Char / 2), AlignmentTypes.Suffix) + GetFormatedText(heading[1], (Line_Char / 2), AlignmentTypes.Prefix));
            rdr.WriteLine(GetFormatedText(heading[2], (Line_Char / 2), AlignmentTypes.Suffix) + GetFormatedText(heading[3], (Line_Char / 2), AlignmentTypes.Prefix));
            //cus address
            rdr.WriteLine(GetFormatedText(heading[4], (Line_Char / 2), AlignmentTypes.Suffix));
            rdr.WriteLine(GetCenterText(CusAddress));
            rLoc = Line_Char - CusAddress.Length;
            if (rLoc < 0)
            {
                rdr.WriteLine(GetCenterText(CusAddress.Substring(71, Math.Abs(rLoc) - 1)));
            }
            PrintLine();
        }
        public void PrintDetails(DataTable dataTable, List<string> invoiceContent)
        {
            int cols = dataTable.Columns.Count;
            int rows = dataTable.Rows.Count;
            int[] columnSize = { 4, 24, 10, 7, 7, 7, 10 };
            var lineString = "";
            count = 0;
            foreach (DataColumn column in dataTable.Columns)
            {
                if (count == 0 || count == 1)
                {
                    lineString += GetFormatedText(column.Caption, columnSize[count], AlignmentTypes.Suffix);
                }
                else
                {
                    lineString += GetFormatedText(column.Caption, columnSize[count], AlignmentTypes.Prefix);
                }
                count++;
            }
            rdr.WriteLine(lineString);
            PrintLine();
            for (int i = 0; i < rows; i++)
            {
                string[] Item = dataTable.Rows[i][1].ToString().Split("///");
                var productDetails = "";
                for (int j = 0; j < cols; j++)
                {
                    var temp = j == 1 && Item.Length == 2 ? Item[0] : dataTable.Rows[i][j].ToString();
                    if (j == 0 || j == 1)
                    {
                        productDetails += GetFormatedText(temp, columnSize[j], AlignmentTypes.Suffix);
                    }
                    else
                    {
                        productDetails += GetFormatedText(j == 5 && Global.Company.CompanySalesSetup.PriceType == PriceType.MaxRetailPrice ? "0.00" : temp, columnSize[j], AlignmentTypes.Prefix);
                    }
                }
                rdr.WriteLine(productDetails);
                if (Item.Length == 2)
                {
                    rdr.WriteLine(Item[1]);
                }
                if (Global.Company.CompanySalesSetup.PriceType == PriceType.MaxRetailPrice)
                {
                    rdr.WriteLine(dataTable.Rows[i][5].ToString());
                }
            }
            double totalInvoiceAmount = 0;
            var productTotal = "";
            for (int k = 0; k < invoiceContent.Count; k++)
            {
                var temp = invoiceContent[k].ToString();
                if (k == invoiceContent.Count - 2)
                {
                    totalInvoiceAmount = double.Parse(invoiceContent[k]);
                }
                if (k == invoiceContent.Count - 1)
                {
                    PrintLine();
                    rdr.WriteLine(productTotal);
                    PrintLine();
                    productTotal = "";
                    productTotal += GetFormatedText(temp, Line_Char, AlignmentTypes.Suffix);
                    rdr.WriteLine(productTotal);
                    productTotal = "";
                }
                else
                {
                    productTotal += GetFormatedText(temp, columnSize[k], AlignmentTypes.Prefix);
                }
            }
        }
        public void PrintLine()
        {
            int i;
            string Lstr = "";
            for (i = 1; i <= Line_Char; i++)
            {
                Lstr = Lstr + "-";
            }
            rdr.WriteLine(Lstr);
        }

        public void SkipLine(int LineNos)
        {
            int i;
            for (i = 1; i <= 5; i++)
            {
                rdr.WriteLine("");
            }
        }

        private string GetCenterText(string Cont)
        {
            string blankSpace = "";
            int rLoc = Line_Char - Cont.Length;

            if (rLoc < 0)
            {
                Cont = Cont.Substring(0, Line_Char);
            }
            else if (Cont.Count() < Line_Char)
            {

                int ct = Cont.Count();// = ;
                int loopCount = (Line_Char - ct) / 2;
                for (var i = 0; i < loopCount; i++)
                {
                    blankSpace += " ";
                }
            }
            return (blankSpace + Cont);
        }
        private string GetFormatedText(string Cont, int Length, AlignmentTypes AlignmentType)
        {
            int rLoc = Length - Cont.Length;

            if (rLoc < 0)
            {
                Cont = Cont.Substring(0, Length);
            }
            else
            {
                int nos;
                for (nos = 0; nos < rLoc; nos++)
                {
                    if (AlignmentType == AlignmentTypes.Prefix)
                    {
                        Cont = " " + Cont;

                    }
                    else
                    {
                        Cont = Cont + " ";

                    }
                }
            }
            return (Cont);
        }
        public bool ExportToFileOrPrint(long PurchaseId, string fileExtension, bool isPrint)
        {
            // Changes made here CustomerId to AccountId and Customer to Account due to purchase mdel change
            //Entrytype = Type;
            PurchaseEntryManager Manager  = PurchaseEntryManager.Instance;

            PurchaseEntry PurchaseEntry = PurchaseEntryManager.Instance.GetPurchaseEntry(PurchaseId);
            List<CompanySalesTaxAccountMap> CompanyPurchaseTaxAccountMap = (List<CompanySalesTaxAccountMap>)Global.Company.SalesTaxAccountMaps;
            List<string> headingText = new List<string>();
            string CusAddress = string.Empty;
            string customerId = PurchaseEntry.AccountId != null ? PurchaseEntry.AccountId.ToString() : "";
            if (PurchaseEntry.AccountId != null)
            {
                Customer Customer = CustomerManager.Instance.GetCustomerById((long)PurchaseEntry.AccountId);
                if (Customer != null)
                {
                    string PaymentTerm = Customer != null ? Customer.PaymentTerm.ToString() : "Immediate Payment";
                    string PaymentDate = Customer != null ? PurchaseEntry.PurchaseInvDate.AddDays(Customer.PaymentTerm.NoOfDays).ToString(Global.Company.DateFormat).ToString() : DateTime.Now.Date.ToString(Global.Company.DateFormat);
                }
            }

            headingText.Add((Entrytype == Entrytype.SALE ? "Invoice No : " : Entrytype == Entrytype.RETURN ? "Return No : " : "Quote No : ") + PurchaseEntry.RefNumber);
            headingText.Add("Date : " + PurchaseEntry.PurchaseInvDate.ToString(Global.Company.DateFormat));


            if (PurchaseEntry.AccountId != null)
            {
                
                Customer Customer = CustomerManager.Instance.GetCustomerById((long)PurchaseEntry.AccountId);
                Supplier Supplier = null;
                if (Customer == null)
                {
                    Supplier = SupplierManager.Instance.GetSupplierById((long)PurchaseEntry.AccountId);
                    if (Supplier.SupplierLicenceDetail.Count > 0)
                    {
                        headingText.Add(Supplier.SupplierLicenceDetail.First().DisplayName + ": " + Supplier.SupplierLicenceDetail.First().Value);
                    }
                    else
                    {
                        headingText.Add("");
                    }
                    if (Supplier.Address != null)
                    {
                        CusAddress = Supplier.Address.FullAddressInSingleLine;
                    }
                }
                else
                {
                    if (Customer.CustomerLicenceDetail.Count > 0)
                    {
                        headingText.Add(Customer.CustomerLicenceDetail.First().DisplayName + ": " + Customer.CustomerLicenceDetail.First().Value);
                    }
                    else
                    {
                        headingText.Add("");
                    }
                    if (Customer.BillingAddress != null)
                    {
                        CusAddress = Customer.BillingAddress.FullAddressInSingleLine;
                    }
                }

            }
            //else
            //{
            //    headingText.Add("Patient Name: " + PurchaseEntry.SupplierName);
            //    if (PurchaseEntry.ReferedBy != null)
            //    {
            //        headingText.Add("Refered By: " + PurchaseEntry.ReferedBy.Name);
            //    }
            //    else
            //    {
            //        headingText.Add(" ");
            //    }
            //    headingText.Add("");
            //}

            string BarcodeString = PurchaseEntry.RefNumber + "%" + PurchaseEntry.PurchaseInvDate.ToString(Global.Company.DateFormat);
            DataGridView dgvTotal = new DataGridView();
            DataTable dt = new DataTable();
            string taxRow = "";
            List<string> InvoiceTotal = new List<string>();

            if (PurchaseEntry.PurchaseDetails.Count != 0)
            {
                try
                {
                    dt.Columns.Add("#", typeof(string));
                    dt.Columns["#"].Caption = "#";

                    dt.Columns.Add("particulars", typeof(string));
                    dt.Columns["particulars"].Caption = "Particulars";

                    dt.Columns.Add("cost", typeof(string));
                    dt.Columns["cost"].Caption = "Rate";

                    dt.Columns.Add("qty", typeof(string));
                    dt.Columns["qty"].Caption = "Qty";

                    dt.Columns.Add("dis", typeof(string));
                    dt.Columns["dis"].Caption = "Dis %";

                    dt.Columns.Add("tax", typeof(string));
                    dt.Columns["tax"].Caption = "Tax %";

                    dt.Columns.Add("amount", typeof(string));
                    dt.Columns["amount"].Caption = "Amount";

                    int count = 0;
                    List<string> OverAllTaxAmount = new List<string>();
                    List<TaxDetail> OverAllTaxByPer = new List<TaxDetail>();
                    double purchaseTotal = 0;
                    double taxTotal = 0;
                    double discountTotal = 0;
                    double qtyTotal = 0;
                    foreach (PurchaseDetails PurchaseDetails in PurchaseEntry.PurchaseDetails)
                    {
                        PurchaseDetails lPurchaseDetail = PurchaseEntryManager.Instance.GetPurchaseDetail(PurchaseDetails.Id);
                        string purchaseTax = "";
                        float purchaseTaxPer = 0;
                        string purchaseDiscount = "";
                        foreach (CompanySalesTaxAccountMap PurchaseTax in CompanyPurchaseTaxAccountMap)
                        {
                            foreach (TaxDetail Taxes in lPurchaseDetail.TaxDetails)
                            {
                                if (Taxes.TaxAccountId == PurchaseTax.MapId)
                                {
                                    purchaseTax = Taxes.TaxRate.ToString();
                                    purchaseTaxPer += Taxes.TaxRate;
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
                        if (lPurchaseDetail.Discounts.Count > 0)
                        {
                            purchaseDiscount = lPurchaseDetail.Discounts.First().Discount.ToString("F");
                            discountTotal += lPurchaseDetail.Discounts.First().DiscountAmount;
                        }
                        else
                        {
                        }

                        count++;
                        DataRow drNewRow = dt.NewRow();

                        double TaxAmount = PdfDataAlignment.ProductTaxPercentagePurchase(lPurchaseDetail.TaxDetails, PurchaseEntry.PurchaseTaxType);

                        drNewRow["#"] = count.ToString();
                        drNewRow["particulars"] = PurchaseDetails.Product.Name.ToString() + " @UOM " + PurchaseDetails.WholesaleUOM.ToString() + (PurchaseDetails.isBatch ? "///Bat.No: " + PurchaseDetails.BatchNo + ", Exp Date: " + PurchaseDetails.ExpDate.Month.ToString() + "/" + PurchaseDetails.ExpDate.Year.ToString() : "");

                        drNewRow["cost"] = PurchaseDetails.PurchasePrice== 0 ? PurchaseDetails.PurchasePrice.ToString("F") : PurchaseDetails.PurchasePrice.ToString("F");
                        drNewRow["qty"] = PurchaseDetails.Quantity.ToString("F");

                        drNewRow["dis"] = purchaseDiscount;
                        drNewRow["tax"] = Global.Company.CompanySalesSetup.PriceType == PriceType.MaxRetailPrice ? "Bat No: " + PurchaseDetails.BatchNo + " Exp: " + PurchaseDetails.ExpDate.ToShortDateString() : purchaseTaxPer.ToString("F");
                        drNewRow["amount"] = Math.Round(PurchaseDetails.Amount, 2).ToString("F");

                        purchaseTotal += PurchaseDetails.Amount;
                        qtyTotal += PurchaseDetails.Quantity;

                        dt.Rows.Add(drNewRow);
                    }

                    for (int i = 0; i < OverAllTaxByPer.Count; i++)
                    {
                        var Comma = i == OverAllTaxByPer.Count - 1 ? "" : ", ";
                        string TypeName = CompanyPurchaseTaxAccountMap.Where(t => t.MapId == OverAllTaxByPer[i].TaxAccountId).Select(t => t.Name).FirstOrDefault().ToString();
                        taxRow += TypeName.ToString() + " " + OverAllTaxByPer[i].TaxRate.ToString() + " % : Rs. " + OverAllTaxByPer[i].Amount.ToString("F") + Comma;
                        taxTotal += OverAllTaxByPer[i].Amount;
                    }
                    OverAllTaxAmount.Add(taxRow);

                    InvoiceTotal.Add("");
                    InvoiceTotal.Add("Total:");
                    InvoiceTotal.Add("");
                    InvoiceTotal.Add(qtyTotal.ToString());
                    InvoiceTotal.Add(discountTotal.ToString("F"));
                    InvoiceTotal.Add(taxTotal.ToString("F"));
                    InvoiceTotal.Add(purchaseTotal.ToString("F"));
                    InvoiceTotal.Add(taxRow);
                }
#pragma warning disable 0168
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
                                PrintBill(dataTable, PdfDataAlignment.DataGridViewAsDataTableAc(dgvTotal), headingText, CusAddress, InvoiceTotal, BarcodeString, "SalePrint", fileExtension, isPrint);
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
    }
}
