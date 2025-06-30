using fa.api.Accounting;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.views.utils.Receipts;
using fa.views.utils.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.views.utils.Journal
{
    public class JournalSavePrintA4:PrintBase
    {
        public bool ExportToFileOrPrint(long JournalId, string fileExtension, bool isPrint)
        {
            JournalManager JournalManager = JournalManager.Instance;
            AddressManager AddressManager = AddressManager.Instance;

            fa.model.Accounting.Transaction.Journal JournalInfo = JournalManager.GetJournal(JournalId);
            string journalMemo = JournalInfo.Memo;

            List<string> headingTest = new List<string>();
            headingTest.Add("JOURNAL");
            headingTest.Add(journalMemo);

            List<string> invoiceContent = new List<string>();
            //Table Heading
            invoiceContent.Add("Journal No");
            invoiceContent.Add("Journal Date");
            //Table Content
            invoiceContent.Add(JournalInfo.ReferenceNumber);
            invoiceContent.Add(JournalInfo.TransactionDate.ToString(Global.Company.DateFormat));
            
            DataGridView dgvTotal = new DataGridView();
            dgvTotal.ColumnCount = 4;
            dgvTotal.Columns[0].Name = "0";
            dgvTotal.Columns[1].Name = "1";
            dgvTotal.Columns[2].Name = "2";
            dgvTotal.Rows.Add("Total ", JournalInfo.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), JournalInfo.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
            dgvTotal.AllowUserToAddRows = false;

            DataGridView tableColumnSwap = new DataGridView();
            tableColumnSwap.ColumnCount = 6;

            DataTable dt = new DataTable();
            if (JournalInfo.JournalDetails.Count != 0)
            {
                try
                {
                    dt.Columns.Add("#", typeof(string));
                    dt.Columns["#"].Caption = "#";

                    dt.Columns.Add("ac", typeof(string));
                    dt.Columns["ac"].Caption = "Account";

                    dt.Columns.Add("dec", typeof(string));
                    dt.Columns["dec"].Caption = "Description";

                    dt.Columns.Add("name", typeof(string));
                    dt.Columns["name"].Caption = "RefName";

                    dt.Columns.Add("debit", typeof(string));
                    dt.Columns["debit"].Caption = "Debit";

                    dt.Columns.Add("credit", typeof(string));
                    dt.Columns["credit"].Caption = "Credit";

                    int count = 0;
                    foreach (JournalDetail JDetails in JournalInfo.JournalDetails)
                    {
                        count++;
                        DataRow drNewRow = dt.NewRow();
                        Account ToAccount = AccountManager.Instance.GetAccountById((long)JDetails.ToAccountId);
                        Account RefAccount = null;
                        if (JDetails.ReferenceAccountId!=null)
                        {
                            RefAccount = AccountManager.Instance.GetAccountById((long)JDetails.ReferenceAccountId) ;
                        }
                        if (ToAccount != null || RefAccount != null)
                        {
                            drNewRow["#"] = count.ToString();
                            drNewRow["ac"] = ToAccount.Name;
                            drNewRow["dec"] = JDetails.Description.ToString();
                            drNewRow["name"] = RefAccount!=null?RefAccount.Name:"";
                            if (JDetails.Amount < 0)
                            {
                                drNewRow["debit"] = "";
                                drNewRow["credit"] = Math.Abs(JDetails.Amount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            }
                            else
                            {
                                drNewRow["debit"] = Math.Abs(JDetails.Amount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                drNewRow["credit"] = "";
                                
                            }
                            dt.Rows.Add(drNewRow);
                        }
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
                    if (Global.Company.IdSpaces != null && Global.Company.IdSpaces.FirstOrDefault(x => x.EntryType == EntryType.JOURNAL).IsDotMatrix)
                    {
                        fileName = "Journal";
                        TransactionDotmatrix TransactionDotmatrix = new TransactionDotmatrix(this);
                        TransactionDotmatrix.GenerateTXT(dt, PdfDataAlignment.DataGridViewAsDataTableAc(dgvTotal), headingTest, invoiceContent, fileExtension, isPrint);
                    }
                    else
                    {
                        TransactionLaser.GeneratePDF(dt, PdfDataAlignment.DataGridViewAsDataTableAc(dgvTotal), headingTest, invoiceContent, "Journal", fileExtension, isPrint, journalMemo);
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
