using fa.api.Accounting;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transactions;
using fa.model.Common;
using fa.views.utils.Receipts;
using fa.views.utils.Transaction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.views.utils.Debitnote
{
    public class DebitnoteSavePrintA4 : PrintBase
    {       
        public bool ExportToFileOrPrint(long Id, string fileExtension, bool isPrint)
        {
            Cursor.Current = Cursors.WaitCursor;
            DebitNoteManager DebitNoteManager = DebitNoteManager.Instance;
            AddressManager AddressManager = AddressManager.Instance;
            CustomerManager CustomerManager = CustomerManager.Instance;
            SupplierManager supplierManager = SupplierManager.Instance;
            DebitNote DebitnoteInfo = DebitNoteManager.GetDebitNote(Id);
            //Supplier lSupplier = DebitnoteInfo.Supplier;
            string InvoiceMemo = DebitnoteInfo.Note;
            string License = string.Empty;
            List<string> headingTest = new List<string>();
            headingTest.Add("DEBITNOTE");
            //address
            if (DebitnoteInfo.Account.AccountType == AccountType.CUSTOMER)
            {
                Customer lCustomer = CustomerManager.GetCustomerById((long)DebitnoteInfo.AccountId);
                Address address = AddressManager.GetAddressById((long)lCustomer.BillingAddressId);
                string laddress = address.FullAddressInSingleLine;
                headingTest.Add(lCustomer.DisplayAs + "\n" + (string.IsNullOrEmpty(laddress) ? "" : (laddress + "\n")) + (string.IsNullOrEmpty(lCustomer.ContactInfo.Phone) ? "" : (lCustomer.ContactInfo.Phone + "\n")) + "Customer ID: " + lCustomer.Id + "\n\n");

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
            else if(DebitnoteInfo.Account.AccountType == AccountType.SUPPLIER)
            {
                Supplier lSupplier = supplierManager.GetSupplierById((long)DebitnoteInfo.AccountId);
                Address address = AddressManager.GetAddressById((long)lSupplier.AddressId);
                string laddress = address.FullAddressInSingleLine;
                headingTest.Add(lSupplier.DisplayAs + "\n" + (string.IsNullOrEmpty(laddress) ? "" : (laddress + "\n")) + (string.IsNullOrEmpty(lSupplier.ContactInfo.Phone) ? "" : (lSupplier.ContactInfo.Phone + "\n")) + "Customer ID: " + lSupplier.Id + "\n\n");
                
                if(lSupplier.SupplierLicenceDetail.Count > 0)
                {
                    foreach (SupplierLicenceDetail licenceDetail in lSupplier.SupplierLicenceDetail)
                    {
                        if(licenceDetail.CompanySupplierLicenseMaster.IncludeInReport)
                        {
                            License = (string.IsNullOrEmpty(License) ? License : License + "\n") + (licenceDetail.CompanySupplierLicenseMaster.Name + ": " + licenceDetail.Value);
                        }
                    }
                }
            }
            else
            {
                Account lAccount = AccountManager.Instance.GetAccountById((long)DebitnoteInfo.AccountId);
                headingTest.Add(lAccount.DisplayAs);
            }
            
            headingTest.Add(License);
            headingTest.Add(InvoiceMemo);
            List<string> invoiceContent = new List<string>();
            invoiceContent.Add("DebitNote No");
            invoiceContent.Add("DebitNote Date");
            invoiceContent.Add(DebitnoteInfo.ReferenceNumber);
            invoiceContent.Add(DebitnoteInfo.TransactionDate.Date.ToString(Global.Company.DateFormat));
            DataGridView dgvTotal = new DataGridView();
            dgvTotal.ColumnCount = 3;
            dgvTotal.Columns[0].Name = "0";
            dgvTotal.Columns[1].Name = "1";
            dgvTotal.Columns[2].Name = "2";
            dgvTotal.Rows.Add("Total", DebitnoteInfo.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
            dgvTotal.AllowUserToAddRows = false;
            DataGridView tableColumnSwap = new DataGridView();
            tableColumnSwap.ColumnCount = 4;
            DataTable dt = new DataTable();
            if (DebitnoteInfo.DebitNoteDetails.Count != 0)
            {
                try
                {
                    dt.Columns.Add("#", typeof(string));
                    dt.Columns["#"].Caption = "#";
                    dt.Columns.Add("account", typeof(string));
                    dt.Columns["account"].Caption = "Account";
                    dt.Columns.Add("dec", typeof(string));
                    dt.Columns["dec"].Caption = "Description";
                    dt.Columns.Add("amount", typeof(string));
                    dt.Columns["amount"].Caption = "Amount";
                    int count = 0;
                    foreach(DebitNoteDetail Details in DebitnoteInfo.DebitNoteDetails)
                    {
                        count++;
                        DataRow drNewRow = dt.NewRow();
                        drNewRow["#"] = count;
                        drNewRow["account"] = Details.Account.Name.ToString();
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
                    if (Global.Company.IdSpaces != null && Global.Company.IdSpaces.FirstOrDefault(x => x.EntryType == EntryType.DEBIT_NOTE).IsDotMatrix)
                    {
                        fileName = "DebitNote";
                        TransactionDotmatrix TransactionDotmatrix = new TransactionDotmatrix(this);
                        TransactionDotmatrix.GenerateTXT(dt, PdfDataAlignment.DataGridViewAsDataTableAc(dgvTotal), headingTest, invoiceContent, fileExtension, isPrint);
                    }
                    else
                    {
                        TransactionLaser.GeneratePDF(dt, PdfDataAlignment.DataGridViewAsDataTableAc(dgvTotal), headingTest, invoiceContent, "DebitNote", fileExtension, isPrint, "");
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
