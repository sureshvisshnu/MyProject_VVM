using fa.api.Accounting;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transactions;
using fa.model.Common;
using fa.views.utils.Transaction;
using Fa.model.Accounting.Masters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace fa.views.utils.Receipts
{
    class ReceiptSavePrint: PrintBase
    {
        public bool ExportToFileOrPrint(long ReceiptId, string fileExtension, bool isPrint)
        {
            ReceiptManager ReceiptManager = ReceiptManager.Instance;
            AddressManager AddressManager = AddressManager.Instance;
            CustomerManager CustomerManager = CustomerManager.Instance;
            Receipt ReceiptInfo = ReceiptManager.GetReceipt(ReceiptId);

           string ReceiptInfoMemo = ReceiptInfo.Description;

            List<string> headingTest = new List<string>();
            headingTest.Add("RECEIPT");
            string License = string.Empty;

            if (ReceiptInfo.Account.AccountType == AccountType.CUSTOMER)
            {
            Customer lCustomer = CustomerManager.GetCustomerById((long)ReceiptInfo.AccountId);
            Address address = AddressManager.GetAddressById((long)lCustomer.BillingAddressId);
            string laddress = address.FullAddressInSingleLine;
            headingTest.Add(lCustomer.DisplayAs + "\n" +(string.IsNullOrEmpty(laddress)?"":(laddress + "\n"))+ (string.IsNullOrEmpty(lCustomer.ContactInfo.Phone) ? "" : (lCustomer.ContactInfo.Phone + "\n")) + "Customer ID: " + lCustomer.Id + "\n\n");

                if (lCustomer.CustomerLicenceDetail.Count > 0)
                {
                    foreach (CustomerLicenceDetail Licence in lCustomer.CustomerLicenceDetail)
                    {
                        if (Licence.CompanyCustomerLicenseMaster.IncludeInReport)
                        {
                            License = (string.IsNullOrEmpty(License) ? License : License + "\n") + (Licence.CompanyCustomerLicenseMaster.Name + ": " + Licence.Value);
                        }
                    }
                }

            }
            else
            {
                Account lAccount = AccountManager.Instance.GetAccountById((long)ReceiptInfo.AccountId);
                headingTest.Add(lAccount.DisplayAs);
            }
            headingTest.Add(License);
            headingTest.Add(ReceiptInfoMemo);

            List<string> invoiceContent = new List<string>();
            //Table Heading
            invoiceContent.Add("Transaction No");
            invoiceContent.Add("Receipt Date");
            invoiceContent.Add("Receipt Type");
            if (ReceiptInfo.TransactionType != PaymentType.CASH)
            {
                invoiceContent.Add(Char.ToUpperInvariant(ReceiptInfo.TransactionType.ToString()[0]) + ReceiptInfo.TransactionType.ToString().Substring(1).ToLower() + " Details");
            }
            //Table Content
            invoiceContent.Add(ReceiptInfo.Reference);
            invoiceContent.Add(ReceiptInfo.TransactionDate.ToString(Global.Company.DateFormat));
            invoiceContent.Add(ReceiptInfo.TransactionType.ToString());

            if (ReceiptInfo.TransactionType == PaymentType.CHECK)
            {
                CheckReceipt CheckReceipt = ReceiptManager.GetCheckReceipt(ReceiptId);
                invoiceContent.Add(CheckReceipt.DepositedInto.Name + "\n" + CheckReceipt.DocumentDate.ToString(Global.Company.DateFormat));
            }
            else if (ReceiptInfo.TransactionType == PaymentType.CREDITCARD)
            {
                CreditCardReceipt CreditCardReceipt = ReceiptManager.GetCreditCardReceipt(ReceiptId);
                invoiceContent.Add(CreditCardReceipt.CCAccount.Name + "\n" + CreditCardReceipt.CCTransactionNumber.ToString());
            }
            else if (ReceiptInfo.TransactionType == PaymentType.BANKTRANSFER)
            {
                BankTransferReceipt BankTransferReceipt = ReceiptManager.GetBankTransferReceipt(ReceiptId);
                invoiceContent.Add(BankTransferReceipt.BankTransfer.Name + "\n" + BankTransferReceipt.TransactionNumber.ToString());
            }
            
            DataGridView dgvTotal = new DataGridView();
            dgvTotal.ColumnCount = 4;
            dgvTotal.Columns[0].Name = "0";
            dgvTotal.Columns[1].Name = "1";
            dgvTotal.Columns[2].Name = "2";
            dgvTotal.Rows.Add("", "Total ", ReceiptInfo.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
            dgvTotal.AllowUserToAddRows = false;
            DataGridView tableColumnSwap = new DataGridView();
            tableColumnSwap.ColumnCount = 3;
            DataTable dt = new DataTable();
            if (ReceiptInfo.ReceiptDetails.Count != 0)
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
                    foreach (ReceiptDetail receiptDetail in ReceiptInfo.ReceiptDetails)
                    {
                        count++;
                        DataRow drNewRow = dt.NewRow();
                        drNewRow["#"] = count;
                        drNewRow["dec"] = receiptDetail.Description.ToString();
                        drNewRow["amount"] = receiptDetail.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
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
                        if (Global.Company.IdSpaces != null && Global.Company.IdSpaces.FirstOrDefault(x => x.EntryType == EntryType.RECEIPT).IsDotMatrix)
                        {
                            fileName = "Receipt";
                            TransactionDotmatrix TransactionDotmatrix = new TransactionDotmatrix(this);
                            TransactionDotmatrix.GenerateTXT(dt, PdfDataAlignment.DataGridViewAsDataTableAc(dgvTotal), headingTest, invoiceContent, fileExtension, isPrint);
                        }
                        else
                        {
                            TransactionLaser.GeneratePDF(dt, PdfDataAlignment.DataGridViewAsDataTableAc(dgvTotal), headingTest, invoiceContent, "Receipt", fileExtension, isPrint, "");
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
