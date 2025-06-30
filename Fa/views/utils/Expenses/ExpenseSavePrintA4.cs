using fa.api.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using fa.model.Accounting.Transactions;
using fa.model.Accounting.Masters;
using fa.model.Employee;
using fa.model.Common;
using Fa.model.Accounting.Masters;
using System.Windows.Forms;
using System.Data;
using fa.views.utils.Receipts;
using fa.views.utils.Transaction;
using fa.api.utils;

namespace fa.views.utils.Expenses
{
    public class ExpenseSavePrintA4 : PrintBase
    {
        Supplier Supplier = null;
        Customer Customer = null;
        Employee Employee = null;
        public bool ExportToFileOrPrint(long ExpenseId, string fileExtension, bool isPrint)
        {
            string License = string.Empty;
            string laddress = string.Empty;
            List<string> headingTest = new List<string>();
            headingTest.Add("Expense");

            ExpenseManager ExpenseManager = ExpenseManager.Instance;
            AddressManager AddressManager = AddressManager.Instance;

            Expense ExpenseInfo = ExpenseManager.GetExpense(ExpenseId);
            Account lAccount = ExpenseInfo.Payee;
            string memo = ExpenseInfo.Memo;

            if (lAccount.AccountType == AccountType.CUSTOMER)
            {
                Customer = CustomerManager.Instance.GetCustomerById(lAccount.Id);
            }
            else if (lAccount.AccountType == AccountType.SUPPLIER)
            {
                Supplier = SupplierManager.Instance.GetSupplierById(lAccount.Id);
            }
            else if (lAccount.AccountType == AccountType.EMPLOYEE)
            {
                Employee = EmployeeManager.Instance.GetEmployeeInfoById(lAccount.Id);
            }
            //address
            long AddressId = lAccount.AccountType == AccountType.CUSTOMER ? (long)Customer.BillingAddressId :
                            lAccount.AccountType == AccountType.SUPPLIER ? (long)Supplier.AddressId :
                            lAccount.AccountType == AccountType.EMPLOYEE ? (long)Employee.AddressId : 0L;

            if (AddressId != 0L)
            {
                laddress = AddressManager.GetAddressById(AddressId).FullAddressInSingleLine;
            }
            //supplier/customer/employee details
            if (lAccount.AccountType == AccountType.SUPPLIER)
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
            else if (lAccount.AccountType == AccountType.CUSTOMER)
            {
                headingTest.Add(Customer.DisplayAs + "\n" + (string.IsNullOrEmpty(laddress) ? "" : (laddress + "\n")) + "Customer ID: " + Customer.Id + "\n\n");
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
            else if (lAccount.AccountType == AccountType.EMPLOYEE)
            {
                headingTest.Add(Employee.DisplayAs + "\n" + (string.IsNullOrEmpty(laddress) ? "" : (laddress + "\n")) + "Employee ID: " + Employee.Id + "\n\n");
            }
            else
            {
                headingTest.Add(lAccount.DisplayAs);
            }
            headingTest.Add(License);
            //header table
            List<string> invoiceContent = new List<string>();
            //Table Heading
            invoiceContent.Add("Transaction No");
            invoiceContent.Add("Expense Date");
            invoiceContent.Add("Expense Type");
            if (ExpenseInfo.TransctionType != PaymentType.CASH)
            {
                invoiceContent.Add(Char.ToUpperInvariant(ExpenseInfo.TransctionType.ToString()[0]) + ExpenseInfo.TransctionType.ToString().Substring(1).ToLower() + " Details");
            }
            //Table Content
            invoiceContent.Add(ExpenseInfo.Reference);
            invoiceContent.Add(ExpenseInfo.TransactionDate.ToString(Global.Company.DateFormat));
            invoiceContent.Add(ExpenseInfo.TransctionType.ToString());

            if (ExpenseInfo.TransctionType == PaymentType.CHECK)
            {
                CheckExpense CheckExpense = ExpenseManager.GetCheckExpense(ExpenseId);
                invoiceContent.Add(CheckExpense.BankAccount.Name + "\n" + CheckExpense.DocumentDate.ToString(Global.Company.DateFormat));
            }
            else if (ExpenseInfo.TransctionType == PaymentType.CREDITCARD)
            {
                CreditCardExpense CreditCardExpense = ExpenseManager.GetCreditCardExpense(ExpenseId);
                invoiceContent.Add(CreditCardExpense.CCAccount.Name + "\n" + CreditCardExpense.CCTransactionNumber.ToString());
            }
            else if (ExpenseInfo.TransctionType == PaymentType.BANKTRANSFER)
            {
                BankTransferExpense BankTransferExpense = ExpenseManager.GetBankTransferExpense(ExpenseId);

                invoiceContent.Add(BankTransferExpense.BankTransfer.Name + "\n" + BankTransferExpense.TransactionNumber.ToString());
            }


            DataGridView dgvTotal = new DataGridView();
            dgvTotal.ColumnCount = 3;
            dgvTotal.Columns[0].Name = "0";
            dgvTotal.Columns[1].Name = "1";
            dgvTotal.Columns[2].Name = "2";

            dgvTotal.Rows.Add("", "Total", ExpenseInfo.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
            dgvTotal.AllowUserToAddRows = false;

            DataGridView tableColumnSwap = new DataGridView();
            tableColumnSwap.ColumnCount = 3;

            DataTable dt = new DataTable();
            if (ExpenseInfo.ExpenseDetails.Count != 0)
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
                    foreach (ExpenseDetail Details in ExpenseInfo.ExpenseDetails)
                    {
                        count++;
                        DataRow drNewRow = dt.NewRow();
                        drNewRow["#"] = count;
                        drNewRow["dec"] = Details.Description.ToString();
                        drNewRow["amount"] = Details.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
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
                        if (Global.Company.IdSpaces != null && Global.Company.IdSpaces.FirstOrDefault(x => x.EntryType == EntryType.EXPENSE).IsDotMatrix)
                        {
                            fileName = "Expense";
                            TransactionDotmatrix TransactionDotmatrix = new TransactionDotmatrix(this);
                            TransactionDotmatrix.GenerateTXT(dt, PdfDataAlignment.DataGridViewAsDataTableAc(dgvTotal), headingTest, invoiceContent, fileExtension, isPrint);
                        }
                        else
                        {
                            TransactionLaser.GeneratePDF(dt, PdfDataAlignment.DataGridViewAsDataTableAc(dgvTotal), headingTest, invoiceContent, "Expense", fileExtension, isPrint, memo);
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
