using fa.api.Accounting;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.model.Common;
using fa.views.utils.Receipts;
using fa.views.utils.Transaction;
using Fa.model.Accounting.Masters;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace fa.views.utils.Bills
{
    class BillSavePrintA4 : PrintBase
    {
        public bool ExportToFileOrPrint(long BillId, string fileExtension, bool isPrint)
        {
            BillManager BillManager = BillManager.Instance;
            AddressManager AddressManager = AddressManager.Instance;
            Bill BillInfo = BillManager.GetBill(BillId);
            Account lSupplier = (Account)BillInfo.Vendor;
            string laddress = string.Empty;
            string License = string.Empty;
            string memo = BillInfo.Memo;

            if (lSupplier.AccountType == AccountType.SUPPLIER)
            {
                Supplier llSupplier = SupplierManager.Instance.GetSupplierById(lSupplier.Id);
                if (llSupplier.Address != null)
                {
                    laddress = llSupplier.Address.FullAddressInSingleLine;
                }
                if (llSupplier.SupplierLicenceDetail.Count > 0)
                {
                    foreach (SupplierLicenceDetail Licence in llSupplier.SupplierLicenceDetail)
                    {
                        if (Licence.CompanySupplierLicenseMaster.IncludeInReport)
                        {
                            License = (string.IsNullOrEmpty(License) ? License : License + "\n") + (Licence.CompanySupplierLicenseMaster.Name + ": " + Licence.Value);
                        }
                    }
                }
            }
            else if (lSupplier.AccountType == AccountType.CUSTOMER)
            {
                Customer Customer = CustomerManager.Instance.GetCustomerById(lSupplier.Id);
                if (Customer.BillingAddress != null)
                {
                    laddress = Customer.BillingAddress.FullAddressInSingleLine;
                }
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

            List<string> headingTest = new List<string>
            {
                "BILL"
            };
            string supplierInfo = lSupplier.DisplayAs + "\n" + (string.IsNullOrEmpty(laddress) ? "" : (laddress + "\n"));
            headingTest.Add(supplierInfo);
            headingTest.Add(License);

            List<string> invoiceContent = new List<string>
            {
                //Table Heading
                "Bill No",
                "Bill Date",
                "Payment Terms",
                "Due Date",
                //Table Content
                BillInfo.ReferenceNumber,
                BillInfo.BillDate.ToString(Global.Company.DateFormat),
                BillInfo.Term.ToString(),
                BillInfo.DueDate.ToString(Global.Company.DateFormat)
            };

            DataGridView dgvTotal = new DataGridView();
            dgvTotal.ColumnCount = 3;
            dgvTotal.Columns[0].Name = "0";
            dgvTotal.Columns[1].Name = "1";
            dgvTotal.Columns[2].Name = "2";

            dgvTotal.Rows.Add("", "Total", BillInfo.Total.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
            dgvTotal.AllowUserToAddRows = false;

            DataGridView tableColumnSwap = new DataGridView();
            tableColumnSwap.ColumnCount = 3;

            DataTable dt = new DataTable();
            if (BillInfo.BillDetails.Count != 0)
            {
                try
                {
                    dt.Columns.Add("#", typeof(string));
                    dt.Columns["#"]!.Caption = "#";

                    dt.Columns.Add("dec", typeof(string));
                    dt.Columns["dec"]!.Caption = "Description";

                    dt.Columns.Add("amount", typeof(string));
                    dt.Columns["amount"]!.Caption = "Amount";

                    int count = 0;
                    foreach (BillDetail billDetails in BillInfo.BillDetails)
                    {
                        count++;
                        DataRow drNewRow = dt.NewRow();
                        drNewRow["#"] = count;
                        drNewRow["dec"] = billDetails.Description.ToString();
                        drNewRow["amount"] = billDetails.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
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
                    if (dt != null)
                    {
                        if (Global.Company.IdSpaces != null && Global.Company.IdSpaces.FirstOrDefault(x => x.EntryType == EntryType.BILL)!.IsDotMatrix)
                        {
                            fileName = "Bill";
                            TransactionDotmatrix TransactionDotmatrix = new TransactionDotmatrix(this);
                            TransactionDotmatrix.GenerateTXT(dt, PdfDataAlignment.DataGridViewAsDataTableAc(dgvTotal)!, headingTest, invoiceContent, fileExtension, isPrint);
                        }
                        else
                        {
                            TransactionLaser.GeneratePDF(dt, PdfDataAlignment.DataGridViewAsDataTableAc(dgvTotal)!, headingTest, invoiceContent, "Bill", fileExtension, isPrint, memo);
                        }
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
