using fa.api.Accounting;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transactions;
using fa.model.Common;
using fa.views.utils.Receipts;
using fa.views.utils.Transaction;
using Fa.model.Accounting.Masters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace fa.views.utils.Payments
{
    class PaymentSavePrint: PrintBase
    {
        public bool ExportToFileOrPrint(long PaymentId, string fileExtension, bool isPrint)
        {
            PaymentManager PaymentManager = PaymentManager.Instance;
            AddressManager AddressManager = AddressManager.Instance;
            SupplierManager SupplierManager = SupplierManager.Instance;
            List<string> headingTest = new List<string>();
            headingTest.Add("PAYMENT");
            string License = string.Empty;

            Payment PaymentInfo = PaymentManager.GetPayment(PaymentId);
            string memo = PaymentInfo.Description;
            if (PaymentInfo.Account.AccountType == AccountType.SUPPLIER)
            {
                Supplier lSupplier = SupplierManager.GetSupplierById((long)PaymentInfo.AccountId);
                Address address = AddressManager.GetAddressById((long)lSupplier.AddressId);
                string laddress = address.FullAddressInSingleLine;
                headingTest.Add(lSupplier.DisplayAs + "\n" + (string.IsNullOrEmpty(laddress) ? "" : (laddress + "\n")) + (string.IsNullOrEmpty(lSupplier.ContactInfo.Phone) ? "" : (lSupplier.ContactInfo.Phone + "\n")) + "Supplier ID: " + lSupplier.Id + "\n\n");

                if (lSupplier.SupplierLicenceDetail.Count > 0)
                {
                    foreach (SupplierLicenceDetail Licence in lSupplier.SupplierLicenceDetail)
                    {
                        if (Licence.CompanySupplierLicenseMaster.IncludeInReport)
                        {
                            License = (string.IsNullOrEmpty(License) ? License : License + "\n") + (Licence.CompanySupplierLicenseMaster.Name + ": " + Licence.Value);
                        }
                    }
                }

            }
            else
            {
                Account Account = AccountManager.Instance.GetAccountById((long)PaymentInfo.AccountId);
                headingTest.Add(Account.DisplayAs);
            }

            headingTest.Add(License);
            List<string> invoiceContent = new List<string>();
            //Table Heading
            invoiceContent.Add("Transaction No");
            invoiceContent.Add("Payment Date");
            invoiceContent.Add("Payment Type");
            if (PaymentInfo.TransctionType != PaymentType.CASH)
            {
                invoiceContent.Add(Char.ToUpperInvariant(PaymentInfo.TransctionType.ToString()[0]) + PaymentInfo.TransctionType.ToString().Substring(1).ToLower() + " Details");
            }
            //Table Content
            invoiceContent.Add(PaymentInfo.Reference);
            invoiceContent.Add(PaymentInfo.TransactionDate.ToString(Global.Company.DateFormat));
            invoiceContent.Add(PaymentInfo.TransctionType.ToString());

            if (PaymentInfo.TransctionType == PaymentType.CHECK)
            {
                CheckPayment CheckPayment = PaymentManager.GetCheckPayment(PaymentId);
                invoiceContent.Add(CheckPayment.BankAccount.Name + "\n" + CheckPayment.DocumentDate.ToString(Global.Company.DateFormat));
            }
            else if (PaymentInfo.TransctionType == PaymentType.CREDITCARD)
            {
                CreditCardPayment CreditCardPayment = PaymentManager.GetCreditCardPayment(PaymentId);
                invoiceContent.Add(CreditCardPayment.CCAccount.Name + "\n"  + CreditCardPayment.CCTransactionNumber.ToString());
            }
            else if (PaymentInfo.TransctionType == PaymentType.BANKTRANSFER)
            {
                BankTransferPayment BankTransferPayment = PaymentManager.GetBankTransferPayment(PaymentId);
                invoiceContent.Add(BankTransferPayment.BankTransfer.Name + "\n" + BankTransferPayment.TransactionNumber.ToString());
            }
            
            DataGridView dgvTotal = new DataGridView();
            dgvTotal.ColumnCount = 3;
            dgvTotal.Columns[0].Name = "0";
            dgvTotal.Columns[1].Name = "1";
            dgvTotal.Columns[2].Name = "2";

            dgvTotal.Rows.Add("", "Total", PaymentInfo.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));

            dgvTotal.AllowUserToAddRows = false;

            DataGridView tableColumnSwap = new DataGridView();
            tableColumnSwap.ColumnCount = 3;

            DataTable dt = new DataTable();
            if (PaymentInfo.PaymentDetails.Count != 0)
            {
                try
                {
                    dt.Columns.Add("#", typeof(string));
                    dt.Columns["#"].Caption = "#";

                    dt.Columns.Add("dec", typeof(string));
                    dt.Columns["dec"].Caption = "Description";

                    dt.Columns.Add("amount", typeof(string));
                    dt.Columns["amount"].Caption = "Amount";

                    int count = 0;
                    foreach (PaymentDetail paymentDetail in PaymentInfo.PaymentDetails)
                    {
                        count++;
                        DataRow drNewRow = dt.NewRow();
                        drNewRow["#"] = count;
                        drNewRow["dec"] = paymentDetail.Description.ToString();
                        drNewRow["amount"] = paymentDetail.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
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
                        if (Global.Company.IdSpaces != null && Global.Company.IdSpaces.FirstOrDefault(x => x.EntryType == EntryType.PAYMENT).IsDotMatrix)
                        {
                            fileName = "Payment";
                            TransactionDotmatrix TransactionDotmatrix = new TransactionDotmatrix(this);
                            TransactionDotmatrix.GenerateTXT(dt, PdfDataAlignment.DataGridViewAsDataTableAc(dgvTotal), headingTest, invoiceContent, fileExtension, isPrint);
                        }
                        else
                        {
                            TransactionLaser.GeneratePDF(dt, PdfDataAlignment.DataGridViewAsDataTableAc(dgvTotal), headingTest, invoiceContent, "Payment", fileExtension, isPrint, memo);
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
