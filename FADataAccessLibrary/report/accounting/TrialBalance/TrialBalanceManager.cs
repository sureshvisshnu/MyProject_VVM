using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa.api.catalog;
using fa.context;
using MySqlConnector;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FaData.Utils;
using fa.model.Catalog;
using fa.model.OrderManagement;
using Fa.model.Purchase;
using NPOI.SS.Formula.Functions;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using MathNet.Numerics.RootFinding;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.model.Accounting.Transactions;
using fa.model.Hms.common;
using MathNet.Numerics.LinearAlgebra.Factorization;
using System.Diagnostics;
using FADataAccessLibrary.Migrations;
using NPOI.SS.Formula.Atp;
using Org.BouncyCastle.Tls.Crypto;
using static NPOI.HSSF.Util.HSSFColor;
using System.ComponentModel.Design;
using FADataAccessLibrary.report.Stock;
using System.Web;
using Microsoft.EntityFrameworkCore.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;
using System.Security.Policy;
using System.Transactions;
using fa.report;
using fa.api.Accounting;
using fa.model.UserProfile;
using fa.api;
namespace FADataAccessLibrary.report.accounting.TrialBalance
{
    public class TrialBalanceManager 
    {
        private static volatile TrialBalanceManager instance;
        private static object syncRoot = new Object();
               
        public static TrialBalanceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new TrialBalanceManager();
                    }
                }
                return instance;
            }
        }
        public static DateTime FromDate { get; set; }
        public static DateTime ToDate { get; set; }

        public bool IsStoredProcedureExists(string storedProcedureName)
        {
            AccountMasterContext Context = new AccountMasterContext();
            string ConString = Context.Database.GetDbConnection().ConnectionString;
            string schemaName = "";
            if (ConString != null)
            {
                var builder = new DbConnectionStringBuilder { ConnectionString = ConString };

                if (builder.TryGetValue("Database", out var database))
                {
                    schemaName = database?.ToString();
                }
            }
            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand("SHOW PROCEDURE STATUS WHERE Db = @databaseName AND Name = @procedureName", connection);
                command.Parameters.AddWithValue("@databaseName", schemaName);
                command.Parameters.AddWithValue("@procedureName", storedProcedureName);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        public DataTable RunTrialBalanceREPORT(string StoredProcedureName, long CompanyId, long[] LocationIds, long[] CategoryIds, int QueryQ, DateTime FromDate, DateTime ToDate)
        {
            DataTable TrialBalanceDataTable = new DataTable();

            long InventoryAcntId = 0;
            long COGSId = 0;           

            Account InventryAccount = AccountManager.Instance.GetAccountDetailByName("Inventory", CompanyId);
            if (InventryAccount != null)
            {
                InventoryAcntId = InventryAccount.Id;
            }
            Account COGSAccount = AccountManager.Instance.GetAccountDetailByName("Purchase (Cost of Goods)", CompanyId);
            if (COGSAccount != null)
            {
                COGSId = COGSAccount.Id;
            }
            DataTable cbdt = new DataTable();
            cbdt = ExecuteStoredProcedure("StockReportGeneratorClosingPriceByMultiLocation", CompanyId, LocationIds, CategoryIds, null, QueryQ);

            double closingStockValue = 0;

            if (cbdt != null && cbdt.Rows.Count > 0) 
            {
                if (cbdt.Rows[0][1] != DBNull.Value && cbdt.Rows[0][1] != null) 
                {
                    closingStockValue = Convert.ToDouble(cbdt.Rows[0][1]);
                }
            }

            DateTime passTransactionDate = ToDate.AddDays(-1);
            Journal lJournal = null;

            if (closingStockValue > 0)
            {
                if (InventoryAcntId != 0 && COGSId != 0)
                {
                    lJournal = CollectJournalForClosingStock(closingStockValue, passTransactionDate, CompanyId, InventoryAcntId, COGSId);
                }
            }

            TrialBalanceDataTable.Columns.Add("AccountType", typeof(string));
            TrialBalanceDataTable.Columns.Add("AccountName", typeof(string));
            TrialBalanceDataTable.Columns.Add("AccountGroupClassification", typeof(string));            
            TrialBalanceDataTable.Columns.Add("Balance", typeof(double));

            AccountMasterContext Context = new AccountMasterContext();
            string ConString = Context.Database.GetDbConnection().ConnectionString;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConString))
                {
                    connection.Open();

                    DataTable AccountsDataTable = new DataTable();
                    string accountsQuery = @"
                        SELECT a.Id AS AccountId, a.Name as AccountName, a.AccountType, a.BalanceAsOf, a.Balance,
                               ag.Name AS AccountGroupName, agc.Name AS AccountGroupClassification, agc.DebitMultiplier
                        FROM Accounts a
                        INNER JOIN AccountGroups ag ON a.AccountGroupId = ag.Id
                        INNER JOIN accountgroupclassifications agc ON ag.AccountClassificationId = agc.Id
                        WHERE a.CompanyId = @CompanyId
                        ORDER BY agc.Name, a.AccountType;";

                    using (MySqlCommand accountsCommand = new MySqlCommand(accountsQuery, connection))
                    {
                        accountsCommand.Parameters.AddWithValue("@CompanyId", CompanyId);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(accountsCommand))
                        {
                            adapter.Fill(AccountsDataTable);
                        }
                    }

                    foreach (DataRow accountRow in AccountsDataTable.Rows)
                    {
                       
                        int accountId = Convert.ToInt32(accountRow["AccountId"]);
                        int accountType = Convert.ToInt32(accountRow["AccountType"]);
                        DateTime balanceAsOf = Convert.ToDateTime(accountRow["BalanceAsOf"]);
                        double initialBalance = Convert.ToDouble(accountRow["Balance"]);
                        int debitMultiplier = Convert.ToInt32(accountRow["DebitMultiplier"]);
                       
                        double openingBalance = 0;
                        string openingBalanceQuery = @"
                            SELECT SUM(d.Amount * @DebitMultiplier) AS OpeningBalance
                            FROM daybooks d
                            WHERE d.AccountId = @AccountId
                            AND d.Date < @FromDate;";

                        using (MySqlCommand openingBalanceCommand = new MySqlCommand(openingBalanceQuery, connection))
                        {
                            openingBalanceCommand.Parameters.AddWithValue("@AccountId", accountId);
                            openingBalanceCommand.Parameters.AddWithValue("@FromDate", FromDate);
                            openingBalanceCommand.Parameters.AddWithValue("@DebitMultiplier", debitMultiplier);

                            object result = openingBalanceCommand.ExecuteScalar();
                            openingBalance = (result != DBNull.Value) ? Convert.ToDouble(result) : 0.0;
                        }

                        openingBalance += (balanceAsOf < FromDate) ? initialBalance : 0;

                        

                        double closingBalance = openingBalance;
                        string closingBalanceQuery = @"
                            SELECT SUM(d.Amount * @DebitMultiplier) AS ClosingBalance
                            FROM daybooks d
                            WHERE d.AccountId = @AccountId
                            AND d.Date BETWEEN @FromDate AND @ToDate;";

                        using (MySqlCommand closingBalanceCommand = new MySqlCommand(closingBalanceQuery, connection))
                        {
                            closingBalanceCommand.Parameters.AddWithValue("@AccountId", accountId);
                            closingBalanceCommand.Parameters.AddWithValue("@FromDate", FromDate);
                            closingBalanceCommand.Parameters.AddWithValue("@ToDate", ToDate);
                            closingBalanceCommand.Parameters.AddWithValue("@DebitMultiplier", debitMultiplier);

                            object result = closingBalanceCommand.ExecuteScalar();
                            closingBalance = (result != DBNull.Value) ? Convert.ToDouble(result) : 0.0;
                            if (lJournal != null)
                            {
                                foreach (var journalDetail in lJournal.JournalDetails)
                                {
                                    if (journalDetail.ToAccountId == accountId)
                                    {
                                        closingBalance += Convert.ToDouble(journalDetail.Amount);
                                    }
                                }
                            }
                        }

                        closingBalance += (balanceAsOf >= FromDate && balanceAsOf <= ToDate) ? initialBalance : 0;

                        const double epsilon = 1e-2;

                        if (Math.Abs(closingBalance) > epsilon)
                        {
                            DataRow row = TrialBalanceDataTable.NewRow();
                            row["AccountType"] = accountType.ToString();
                            row["AccountName"] = accountRow["AccountName"].ToString();
                            row["AccountGroupClassification"] = accountRow["AccountGroupClassification"].ToString();
                            row["Balance"] = closingBalance;

                            TrialBalanceDataTable.Rows.Add(row);
                        }
                    }
                }                
            }
 
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return TrialBalanceDataTable;
        }
        private Journal CollectJournalForClosingStock(Double clsStkVal, DateTime TransactionDate, long CompanyId, long InvntryId, long CostOfGoodSoldId)
        {
            Journal CSJournal = new Journal();
            CSJournal.JournalId = 0L;
            CSJournal.ReferenceNumber = "0";
            CSJournal.Amount = Convert.ToDecimal(clsStkVal);
            CSJournal.TransactionDate = (DateTime)TransactionDate.Date;
            CSJournal.Memo = "Closing Stock Updation";
            CSJournal.CompanyId = CompanyId;


            CostCenter lCostCenter = CostCenterManager.Instance.GetCostCenterByCompanyId(CompanyId);
            if (lCostCenter != null)
            {
                CSJournal.CostCenterId = lCostCenter.CostCenterId;

            }

            JournalDetail JournalToDetail = new JournalDetail();
            Account ToAccount = AccountManager.Instance.GetAccountById((long)InvntryId);

            JournalToDetail.ToAccountId = ToAccount.Id;
            JournalToDetail.Description = "for Closing Stock Updation";
            if (clsStkVal > 0)
            {
                JournalToDetail.Amount = Convert.ToDecimal(clsStkVal);
            }
            CSJournal.JournalDetails.Add(JournalToDetail);

            JournalDetail JournalByDetail = new JournalDetail();
            Account ByAccount = AccountManager.Instance.GetAccountById((long)CostOfGoodSoldId);

            JournalByDetail.ToAccountId = ByAccount.Id;
            JournalByDetail.Description = "for Closing Stock Balance Adjustment";
            if (clsStkVal > 0)
            {
                JournalByDetail.Amount = Convert.ToDecimal(clsStkVal) * -1;
            }
            CSJournal.JournalDetails.Add(JournalByDetail);
            return CSJournal;
        }
       
        public void CreateSPForTrialBalance()
        {
            AccountMasterContext Context = new AccountMasterContext();
            string ConString = Context.Database.GetDbConnection().ConnectionString;

            int journalTransactionType = (int)DaybookTransactionType.Journal;

            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();

                string createProcedureScript = $@"
                                CREATE PROCEDURE CalculateTrialBalance(
                                    IN p_CompanyId INT,
                                    IN FromDate DATE,
                                    IN ToDate DATE,
                                    IN CostCenterId INT
                                )
                                BEGIN
                                    CREATE TEMPORARY TABLE TempAccounts AS
                                    SELECT
                                        a.Id AS AccountId,
                                        a.Name AS AccountName,
                                        a.AccountType,
                                        a.BalanceAsOf,
                                        a.Balance,
                                        agc.DebitMultiplier,
                                        agc.Name AS Classification
                                    FROM
                                        Accounts a
                                    INNER JOIN accountgroups ag ON a.AccountGroupId = ag.Id
                                    INNER JOIN accountgroupclassifications agc ON ag.AccountClassificationId = agc.Id
                                    WHERE
                                        a.CompanyId = p_CompanyId;

                                    CREATE TEMPORARY TABLE TempOpeningBalance AS
                                    SELECT
                                        da.AccountId,
                                        SUM(
                                            da.Amount *
                                            CASE
                                                WHEN da.TransactionType = {journalTransactionType} THEN 1
                                                ELSE agc.DebitMultiplier
                                            END
                                        ) AS OpeningBalance
                                    FROM
                                        DayBooks da
                                    INNER JOIN TempAccounts ta ON da.AccountId = ta.AccountId
                                    INNER JOIN accountgroups ag ON ta.AccountId = ag.Id
                                    INNER JOIN accountgroupclassifications agc ON ag.AccountClassificationId = agc.Id
                                    WHERE
                                        da.Date < FromDate
                                    GROUP BY
                                        da.AccountId;

                                    UPDATE TempOpeningBalance tob
                                    JOIN TempAccounts ta ON tob.AccountId = ta.AccountId
                                    SET tob.OpeningBalance = tob.OpeningBalance +
                                        CASE
                                            WHEN ta.BalanceAsOf < FromDate THEN ta.Balance
                                            ELSE 0
                                        END;

                                    CREATE TEMPORARY TABLE TempClosingBalance AS
                                    SELECT
                                        da.AccountId,
                                        SUM(
                                            da.Amount *
                                            CASE
                                                WHEN da.TransactionType = {journalTransactionType} THEN 1
                                                ELSE agc.DebitMultiplier
                                            END
                                        ) AS TransactionsTotal
                                    FROM
                                        DayBooks da
                                    INNER JOIN TempAccounts ta ON da.AccountId = ta.AccountId
                                    INNER JOIN accountgroups ag ON ta.AccountId = ag.Id
                                    INNER JOIN accountgroupclassifications agc ON ag.AccountClassificationId = agc.Id
                                    WHERE
                                        da.Date BETWEEN FromDate AND ToDate
                                    GROUP BY
                                        da.AccountId;

                                    UPDATE TempClosingBalance tcb
                                    JOIN TempAccounts ta ON tcb.AccountId = ta.AccountId
                                    SET tcb.TransactionsTotal = tcb.TransactionsTotal +
                                        CASE
                                            WHEN ta.BalanceAsOf >= FromDate AND ta.BalanceAsOf <= ToDate THEN ta.Balance
                                            ELSE 0
                                        END;

                                    CREATE TEMPORARY TABLE TempBalances AS
                                    SELECT
                                        ta.AccountId,
                                        ta.AccountName,
                                        COALESCE(tob.OpeningBalance, 0) AS OpeningBalance,
                                        COALESCE(tob.OpeningBalance, 0) + COALESCE(tcb.TransactionsTotal, 0) AS ClosingBalance
                                    FROM
                                        TempAccounts ta
                                    LEFT JOIN TempOpeningBalance tob ON ta.AccountId = tob.AccountId
                                    LEFT JOIN TempClosingBalance tcb ON ta.AccountId = tcb.AccountId;

                                    SELECT
                                        tb.AccountName,
                                        tb.ClosingBalance,
                                        CASE
                                            WHEN tb.ClosingBalance > 0 THEN 'DR'
                                            ELSE 'CR'
                                        END AS DebitOrCredit
                                    FROM
                                        TempBalances tb
                                    WHERE
                                        tb.ClosingBalance != 0;

                                    SELECT
                                        SUM(CASE WHEN ClosingBalance > 0 THEN ClosingBalance ELSE 0 END) AS TotalDebit,
                                        SUM(CASE WHEN ClosingBalance < 0 THEN ABS(ClosingBalance) ELSE 0 END) AS TotalCredit
                                    FROM
                                        TempBalances;

                                    DROP TEMPORARY TABLE IF EXISTS TempAccounts;
                                    DROP TEMPORARY TABLE IF EXISTS TempOpeningBalance;
                                    DROP TEMPORARY TABLE IF EXISTS TempClosingBalance;
                                    DROP TEMPORARY TABLE IF EXISTS TempBalances;
                                END";

                using (MySqlCommand cmd = new MySqlCommand(createProcedureScript, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("Stored procedure created successfully.");
            }
        }

        public DataTable ExecuteStoredProcedure(string StoredProcedureName, long CompanyId, long[] LocationIds, long[] CategoryIds, long[] CostcenterIds, int QueryQ)
        {
            DataTable dataTable = new DataTable();

            AccountMasterContext Context = new AccountMasterContext();
            string ConString = Context.Database.GetDbConnection().ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(StoredProcedureName, connection))
                {
                     adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                    adapter.SelectCommand.Parameters.AddWithValue("@p_CompanyId", CompanyId);
                    adapter.SelectCommand.Parameters.AddWithValue("@p_InventoryLocationIds", string.Join(",", LocationIds));
                    adapter.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate);
                    adapter.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate);
                    adapter.SelectCommand.Parameters.AddWithValue("@CostCenterId", CostcenterIds);
                    

                    if (QueryQ == 1)
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@p_CategoryIds", string.Join(",", CategoryIds));
                    }

                    adapter.Fill(dataTable);

                    string DropProcedureQuery = $"DROP PROCEDURE IF EXISTS {StoredProcedureName}";

                    using (MySqlCommand dropCommand = new MySqlCommand(DropProcedureQuery, connection))
                    {
                        dropCommand.ExecuteNonQuery();
                    }
                }
            }
            Console.WriteLine("Execution completed successfully.");
            return dataTable;
        }
        
    }
}
