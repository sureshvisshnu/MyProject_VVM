using fa.api.Accounting;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.model.Common;
using fa.model.Employee;
using fa.model.OrderManagement;
using fa.views.utils.Receipts;
using fa.views.utils.Transaction;
using Fa.model.Accounting.Masters;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace fa.views.utils.Invoices
{
    class InvoiceSavePrintA4 : PrintBase
    {
        Supplier Supplier = null!;
        Customer Customer = null!;
        Employee Employee = null!;

        public bool ExportToFileOrPrint(long invoiceId, string fileExtension, bool isPrint)
        {
            Cursor.Current = Cursors.WaitCursor;

            InvoiceManager InvoiceManager = InvoiceManager.Instance;
            AddressManager AddressManager = AddressManager.Instance;

            Invoice InvoiceInfo = InvoiceManager.GetInvoice(invoiceId);
            Account lCustomer = InvoiceInfo.Customer;

            string InvoiceMemo = InvoiceInfo.Memo;
            string License = string.Empty;
            string laddress = string.Empty;

            List<string> headingTest = new List<string>();
            headingTest.Add("INVOICE");

            if (lCustomer.AccountType == AccountType.CUSTOMER)
            {
                Customer = CustomerManager.Instance.GetCustomerById(lCustomer.Id);
            }
            else if (lCustomer.AccountType == AccountType.SUPPLIER)
            {
                Supplier = SupplierManager.Instance.GetSupplierById(lCustomer.Id);
            }
            else if (lCustomer.AccountType == AccountType.EMPLOYEE)
            {
                Employee = EmployeeManager.Instance.GetEmployeeInfoById(lCustomer.Id);
            }
            long AddressId = lCustomer.AccountType == AccountType.CUSTOMER ? (long)Customer.BillingAddressId! :
                            lCustomer.AccountType == AccountType.SUPPLIER ? (long)Supplier.AddressId! :
                            lCustomer.AccountType == AccountType.EMPLOYEE ? (long)Employee.AddressId! :
                            0L;

            if (AddressId != 0L)
            {
                laddress = AddressManager.GetAddressById(AddressId).FullAddressInSingleLine;
            }
            if (lCustomer.AccountType == AccountType.SUPPLIER)
            {
                headingTest.Add(Supplier.DisplayAs + "\n" + (string.IsNullOrEmpty(laddress) ? "" : (laddress + "\n")) + "Supplier ID: " + Supplier.Id + "\n\n");
                if (Supplier.SupplierLicenceDetail.Count > 0)
                {
                    foreach (SupplierLicenceDetail Licence in Supplier.SupplierLicenceDetail)
                    {
                        if (Licence.CompanySupplierLicenseMaster.IncludeInReport)
                        {
                            License = (string.IsNullOrEmpty(License) ? License : License + "\n") + (Licence.CompanySupplierLicenseMaster.Name + ": " + Licence.Value);
                        }
                    }
                }
            }
            else if (lCustomer.AccountType == AccountType.CUSTOMER)
            {
                headingTest.Add(Customer.DisplayAs + "\n" + (string.IsNullOrEmpty(laddress) ? "" : (laddress + "\n")));
                if (Customer.CustomerLicenceDetail.Count > 0)
                {
                    foreach (CustomerLicenceDetail Licence in Customer.CustomerLicenceDetail)
                    {
                        if (Licence.CompanyCustomerLicenseMaster.IncludeInReport)
                        {
                            License = (string.IsNullOrEmpty(License) ? License : License + "\n") + (Licence.CompanyCustomerLicenseMaster.Name + ": " + Licence.Value);
                        }
                    }
                }
            }
            else if (lCustomer.AccountType == AccountType.EMPLOYEE)
            {
                headingTest.Add(Employee.DisplayAs + "\n" + (string.IsNullOrEmpty(laddress) ? "" : (laddress + "\n")));
            }
            else
            {
                Account Account = AccountManager.Instance.GetAccountById((long)InvoiceInfo.CustomerId!);
                headingTest.Add(Account.DisplayAs);
            }

            headingTest.Add(License);
            headingTest.Add(InvoiceMemo);

            List<string> invoiceContent = new List<string>();
            //Table Heading
            invoiceContent.Add("Invoice No");
            invoiceContent.Add("Invoice Date");
            invoiceContent.Add("Payment Terms");
            invoiceContent.Add("Due Date");
            
            //Table Content
            invoiceContent.Add(InvoiceInfo.ReferenceNumber);
            invoiceContent.Add(InvoiceInfo.InvoiceDate.Date.ToString(Global.Company.DateFormat));
            invoiceContent.Add(InvoiceInfo.Term.ToString());
            invoiceContent.Add(InvoiceInfo.DueDate.ToString(Global.Company.DateFormat));
            


            DataGridView dgvTotal = new DataGridView();
            dgvTotal.ColumnCount = 4;
            dgvTotal.Columns[0].Name = "0 0";
            dgvTotal.Columns[1].Name = "1";
            dgvTotal.Columns[2].Name = "2";
            dgvTotal.Columns[3].Name = "3";
            //float Tax = 0;
            //dgvTotal.Rows.Add("", "Subtotal", InvoiceInfo.Total.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), Tax.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
            //dgvTotal.Rows.Add("", "Tax", Tax.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), "");
            //dgvTotal.Rows.Add("","", "Total", InvoiceInfo.Total.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
            dgvTotal.AllowUserToAddRows = false;

            DataGridView tableColumnSwap = new DataGridView();
            tableColumnSwap.ColumnCount = 6;


            DataTable dt = new DataTable();
            if (InvoiceInfo.InvoiceDetails.Count != 0)
            {
                try
                {
                    dt.Columns.Add("#", typeof(string));
                    dt.Columns["#"]!.Caption = "#";

                    dt.Columns.Add("dec", typeof(string));
                    dt.Columns["dec"]!.Caption = "Description";

                    dt.Columns.Add("uPrice", typeof(string));
                    dt.Columns["uPrice"]!.Caption = "Unit Price";

                    dt.Columns.Add("qty", typeof(string));
                    dt.Columns["qty"]!.Caption = "QTY";

                    dt.Columns.Add("total", typeof(string));
                    dt.Columns["total"]!.Caption = "Line Total";

                    //dt.Columns.Add("tax", typeof(string));
                    //dt.Columns["tax"].Caption = "Tax";

                    int count = 0;
                    double InvoiceTotal = 0;
                    foreach (InvoiceDetail invDetails in InvoiceInfo.InvoiceDetails)
                    {
                        count++;
                        DataRow ldrNewRow = dt.NewRow();

                        ldrNewRow["#"] = count.ToString();
                        ldrNewRow["dec"] = invDetails.Description.ToString();
                        ldrNewRow["uPrice"] = invDetails.Rate.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        ldrNewRow["qty"] = invDetails.Quantity.ToString();
                        ldrNewRow["total"] = invDetails.Total.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        InvoiceTotal += invDetails.Total;
                        dt.Rows.Add(ldrNewRow);
                    }
                    DataRow drNewRow = dt.NewRow();

                    drNewRow["dec"] = "Grand Total";
                    drNewRow["uPrice"] = "";
                    drNewRow["total"] = InvoiceTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    dt.Rows.Add(drNewRow);
                    if (InvoiceInfo.InvoiceAdditionalTransactions != null && InvoiceInfo.InvoiceAdditionalTransactions.Count > 0)
                    {
                        foreach (InvoiceAdditionalTransaction AdditionalTransaction in InvoiceInfo.InvoiceAdditionalTransactions)
                        {
                            drNewRow = dt.NewRow();
                            drNewRow["dec"] = AdditionalTransaction.DisplayName.ToString();
                            AdditionalTransaction.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            if (AdditionalTransaction.Action == AdditionalTransactionAction.CR)
                            {
                                InvoiceTotal -= AdditionalTransaction.Amount;
                                drNewRow["uPrice"] = "( - )  " + AdditionalTransaction.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                drNewRow["total"] = InvoiceTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            }
                            else
                            {
                                InvoiceTotal += AdditionalTransaction.Amount;
                                drNewRow["uPrice"] = "( + )  " + AdditionalTransaction.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                drNewRow["total"] = InvoiceTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            }
                            dt.Rows.Add(drNewRow);
                        }
                        drNewRow = dt.NewRow();
                        drNewRow["dec"] = "Net Amount";
                        drNewRow["total"] = InvoiceInfo.Total.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        dt.Rows.Add(drNewRow);
                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show("File Error Please Contact System Admin");
                    Console.WriteLine(ee.ToString());
                }
            }
            if (dt.Rows.Count != 0)
            {

                try
                {
                    if (Global.Company.IdSpaces!=null && Global.Company.IdSpaces.FirstOrDefault(x=>x.EntryType==EntryType.INVOICE)!.IsDotMatrix)
                    {
                        fileName = "Invoice";
                        TransactionDotmatrix TransactionDotmatrix = new TransactionDotmatrix(this);
                        TransactionDotmatrix.GenerateTXT(dt, PdfDataAlignment.DataGridViewAsDataTableAc(dgvTotal)!, headingTest, invoiceContent, fileExtension, isPrint);
                    }
                    else
                    {
                        TransactionLaser.GeneratePDF(dt, PdfDataAlignment.DataGridViewAsDataTableAc(dgvTotal)!, headingTest, invoiceContent, "Invoice", fileExtension, isPrint, "");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("File Error Please Contact System Admin");
                    Console.WriteLine(e.ToString());
                }
            }
            Cursor.Current = Cursors.Default;
            return true;
        }
    }
}
