using fa.context;
using fa.model.Accounting.Masters;
using fa.report.common;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace fa.report.accounting.master
{
    public enum ChartOfAccReportType
    {
        ALL = 1, BY_GENERAL_ACC = 2, BY_SUPPLIER= 3, BY_CUSTOMER = 4
    }
    public class ChartOfAccountNames
    {
        static string[] ReportNames = {"All Account Reports",
                                "General Account Report",
                                "Supplier Account Report",
                                "Customer Account Report",
                                "Employeer Account Report"
                                };

        public static string getChartOfAccountNames(ChartOfAccReportType ChartOfAccReport)
        {
            return ReportNames[(int)ChartOfAccReport - 1];
        }

    }
    

    public class RptChartOfAccountLineItem
    {
        public String AccName { get; set; }
        public String AccDescription { get; set; }
        public String AccType { get; set; }
        public String AccDetailType { get; set; }
        public Double AccBalance { get; set; }
        public long Id { get; set; }
        public long? ParentAccountId { get; set; }
        public AccountType AccountTypeId { get; set; }
        public CrDr BalanceType { get; set; }

        public RptChartOfAccountLineItem()
        {
        }

        public RptChartOfAccountLineItem(Account Account)
        {
            var newlineString = Account.Discription != "" ? "\n " : "";
            this.AccName = Account.Name;
            this.AccDescription = Account.Discription + newlineString + Account.DisplayAs;
            this.AccType = Account.AccountGroup != null ? Account.AccountGroup.ParentAccountGroup != null ? Account.AccountGroup.ParentAccountGroup.Name : Account.AccountGroup.Name : Account.AccountType.ToString(); 
            this.AccDetailType = Account.AccountGroup != null ? Account.AccountGroup.Name.ToString() : Account.AccountType.ToString();
            this.AccBalance = Account.Balance;
            this.Id = Account.Id;
            this.ParentAccountId = Account.ParentAccountId;
            this.AccountTypeId = Account.AccountType;
            this.BalanceType = Account.BalanceType();
        }
    }

    public class RptChartOfAccount : Report
    {
        public string SearchText { get; set; }
        ChartOfAccReportType Type = ChartOfAccReportType.ALL;
        IList<RptChartOfAccountLineItem> _LineItems = new List<RptChartOfAccountLineItem>();

        public IList<RptChartOfAccountLineItem> LineItems
        {
            get
            {
                return _LineItems;
            }
            
        }

        public override string ReportName()
        {
            return null;
        }

        public override string ReportTitle()
        {
            string reportTitle = ChartOfAccountNames.getChartOfAccountNames(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
            return null;
        }
        //fetch from multiple category
        public override void GenerateReport()
        {
            IList<Account> AccountInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    if(!String.IsNullOrEmpty(SearchText))
                        AccountInfo = (from Account in Context.Accounts.Include("AccountGroup").Include("AccountGroup.ParentAccountGroup").Include("AccountGroup.AccountGroupClassification") where (Account.CompanyId == this.Company.CompanyId && (Account.Name.Contains(SearchText)|| (Account.Discription.Contains(SearchText) ))) select Account).OrderBy(x => x.Name).ToList();
                    else
                        AccountInfo = (from Account in Context.Accounts.Include("AccountGroup").Include("AccountGroup.ParentAccountGroup").Include("AccountGroup.AccountGroupClassification") where (Account.CompanyId == this.Company.CompanyId) select Account).OrderBy(x => x.Name).ToList();
                }
             
            if (AccountInfo != null && AccountInfo.Count > 0)
                {
                    foreach (Account Account in AccountInfo)
                    {
                        if(Account.AccountType == AccountType.EMPLOYEE)
                        {
                            Account.ParentAccountId = (Context.Employees.FirstOrDefault(x => x.Id == Account.Id)).DepartmentId;
                        }
                        /*
                        if(Account.AccountGroup!=null)
                        {
                            AccountGroupManager.Instance.GetAccountGroupById((long) Account.AccountGroup.AccountClassificationId);
                            Account.AccountGroup = AccountGroupManager.Instance.GetAccountGroupById((long)Account.AccountGroup.AccountClassificationId);
                        }*/
                        LineItems.Add(new RptChartOfAccountLineItem(Account));
                    }
                }
            }
        }
    }
}
