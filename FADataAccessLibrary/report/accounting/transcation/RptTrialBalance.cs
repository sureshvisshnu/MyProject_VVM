using fa.api.Accounting;
using fa.api.catalog;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.model.Catalog;
using fa.model.Hms.common;
using fa.model.OrderManagement;
using fa.report;
using fa.report.common;
using fa.report.Inventory;
using FaData.Utils;
using FADataAccessLibrary.report.Inventory;
using FADataAccessLibrary.report.Stock;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using NPOI.SS.Formula.Functions;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Fa.report.accounting.master
{
    public class RptTrialBalance : Report
    {
        public long? CostcenterId { get; set; }
        public double TotalOpeningStockValue { get; set; }
        public IList<RptTrialBalanceLineItem> LineItems { get; } = new List<RptTrialBalanceLineItem>();
        public override string ReportName()
        {
            string CurrentDate = DateTime.Now.ToString(Company.DateFormat);
            char Separator = CurrentDate.Contains("-") ? '-' : CurrentDate.Contains("/") ? '/' : '.';
            string[] Date = CurrentDate.Split(Separator);
            return String.Format("Trail Balance {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        public override string ReportTitle()
        {
            return "TRIAL BALANCE";
        }
        public string ReportSubTitle()
        {
            return DateUtils.FormatDate(this.FromDate, Company.DateFormat) + "-" + DateUtils.FormatDate(this.ToDate, Company.DateFormat);
        }
        
        public override void GenerateReport()
        {
            TotalOpeningStockValue = 0;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var products = Context.Products.Include(p => p.Inventorys).Include(p => p.ProductFamily).Include(p => p.ProductFamily.Parent).Where(p => p.CompanyId == this.Company.CompanyId).ToList();
                foreach (Product product in products)
                {
                    double openingStockbyDate = 0;
                    double openingStockAmount = 0;
                    if (product._isInventoryAtBatch != null && product._isInventoryAtBatch == true)
                    {
                        var items = Context.InventoryBatches.Include(b => b.Inventory).Where(b => b.Inventory != null && b.CompanyId == this.Company.CompanyId && b.ProductId == product.Id).ToList();
                        foreach (var inventoryBatch in items)
                        {
                            var StockMovement = Context.StockMovementDetail.Include(p => p.StockMovement).Where(p => p.CompanyId == this.Company.CompanyId && p.ProductId == product.Id && p.BatchNo == inventoryBatch.BatchNo && p.StockMovement.MovementDate < this.ToDate.Date).ToList();
                            var openingstock = Context.StockMovementDetail.Include(p => p.StockMovement).Where(p => p.CompanyId == this.Company.CompanyId && p.ProductId == product.Id && p.BatchNo == inventoryBatch.BatchNo &&  p.StockMovement.Type == InventoryJournalType.OPEN_STOCK && p.StockMovement.MovementDate < this.ToDate.Date)
                        .Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                            var purchaseReturns = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.PURCASE_RETURN).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                            var salesReturns = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.SALES_RETURN).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                            var purchase = StockMovement.Where(p=> p.StockMovement.Type == InventoryJournalType.PURCHASE).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                            var sales = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.SALES).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                            var stockIn = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.STOCK_IN).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                            var stockOut = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.STOCK_OUT).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                            var toPatient = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.PATIENT_USE).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                            var damage = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.DAMAGED).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                            var adjust = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.ADJUSTMENT).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                            openingStockbyDate = ((purchase + stockIn + salesReturns + openingstock + adjust) - (sales + stockOut + damage + toPatient + purchaseReturns));
                            openingStockAmount += inventoryBatch.Cost * openingStockbyDate;

                        }
                    }
                    else
                    {
                        var items = Context.Inventories.Include(b => b.InventoryBatchs).Where(b => b.CompanyId == this.Company.CompanyId && b.ProductId == product.Id).ToList();
                        foreach (var InventoryItem in items)
                        {
                            var StockMovement = Context.StockMovementDetail.Include(p => p.StockMovement).Where(p => p.CompanyId == this.Company.CompanyId && p.ProductId == product.Id && p.StockMovement.MovementDate < this.ToDate.Date).ToList();
                            var openingstock = Context.StockMovementDetail.Include(p => p.StockMovement).Where(p => p.CompanyId == this.Company.CompanyId && p.ProductId == product.Id && p.StockMovement.Type == InventoryJournalType.OPEN_STOCK && p.StockMovement.MovementDate < this.ToDate.Date)
                                .Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                            var purchaseReturns = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.PURCASE_RETURN).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                            var salesReturns = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.SALES_RETURN).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                            var purchase = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.PURCHASE).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                            var sales = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.SALES).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                            var stockIn = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.STOCK_IN).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                            var stockOut = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.STOCK_OUT).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                            var toPatient = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.PATIENT_USE).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                            var damage = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.DAMAGED).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                            var adjust = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.ADJUSTMENT).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                            openingStockbyDate = ((purchase + stockIn + salesReturns + openingstock + adjust) - (sales + stockOut + damage + toPatient + purchaseReturns));
                            openingStockAmount += InventoryItem.InventoryBatchs.Sum(batch => batch.Cost * openingStockbyDate);                            
                        }
                    }
                    TotalOpeningStockValue += openingStockAmount;
                }
                IList<Account> lAccounts = (from account in Context.Accounts.Include("AccountGroup")/*.Include("AccountGroup.AccountGroupClassification")*/
                                            where (account.CompanyId == Company.CompanyId)
                                            select account).ToList();
                foreach (Account lAccount in lAccounts.OrderBy(x=>x.AccountGroupId))
                {
                    double OpBal = 0;
                    List<DayBook> OpeningBalance = (from daybook in Context.DoubleEntries
                                                    where (daybook.AccountId == lAccount.Id && daybook.CompanyId==Company.CompanyId
                                                    && daybook.Date < this.ToDate.Date)
                                                    select daybook).ToList();                 
                    foreach (DayBook Balance in OpeningBalance)
                    {
                        OpBal += Balance.Amount * (Balance.TransactionType == DaybookTransactionType.Journal ? 1 : (long)lAccount.AccountGroup.DebitMultiplier);
                    }
                    OpBal = OpBal + (lAccount.BalanceAsOf < ToDate ? lAccount.Balance : 0);
                    const double epsilon = 1e-2;
                    if (Math.Abs(OpBal) > epsilon)
                    {
                        RptTrialBalanceLineItem LineItem = new RptTrialBalanceLineItem(lAccount, OpBal);
                        LineItems.Add(LineItem);  
                    }
                }
            }
        }
    }
    public class RptTrialBalanceLineItem
    {
        public string Name { get; set; }
        public AccountType AccountType { get; set; }
        public string GroupName { get; set; }
        public double Amount { get; set; }
        public Account Account { get; set; }
        public CrDr Nature { get; set; }
        public RptTrialBalanceLineItem(Account account, double Amount)
        {
            this.Name = account.Name;
            this.AccountType = account.AccountType;
            this.GroupName = account.AccountGroup.Name;
            this.Amount = Amount;
            this.Account = account;
        }
        public CrDr CreditOrDebit()
        {
            if (Account.Name!= "Cash On Hand" && Account.AccountGroup.CreditMultiplier == 1)
            {
                return (this.Amount >= 0 ? CrDr.DR : CrDr.CR);
            }
            else if (Account.Name != "Cash On Hand" && Account.AccountGroup.DebitMultiplier == 1)
            {
                return (this.Amount >= 0 ? CrDr.DR : CrDr.CR);
            }
            else
            {
                return (this.Amount >= 0 ? CrDr.DR : CrDr.CR);
            }
        }

    }
}


 