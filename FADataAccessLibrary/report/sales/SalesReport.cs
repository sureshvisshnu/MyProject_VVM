using fa.api.Accounting;
using fa.api.catalog;
using fa.api.UserProfile;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transactions;
using fa.model.Catalog;
using fa.model.Common;
using fa.model.OrderManagement;
using fa.model.UserProfile;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using System.Text.RegularExpressions;
using static fa.report.sales.SalesReportByProductFamily;

namespace fa.report.sales
{
    public enum SalesReportType
    {
        BY_INVOICE = 1, BY_CUSTOMER = 2, BY_CATEGORY = 3, BY_ITEM = 4, BY_SERIAL = 5, BY_REFERER = 6, BY_SOLD = 7, BY_TAX = 8, BY_Date = 9, By_PFamily = 10, BY_AREA = 11, BY_USER = 12
    }
    public class SalesReportNames
    {
        static string[] ReportNames = {"Invoice Wise Sales Report",
                                "Customer Wise Sales Report",
                                "Category Wise Sales Report",
                                "Item Wise Sales Report",
                                "Schedule Wise Sales Report",
                                "Referer Wise Sales Report",
                                "Sold Wise Sales Report",
                                "Tax Wise Sales Report",
                                "Margin Wise Sales Report",
                                "Product Family Wise Sales",
                                "Area Wise Sale Report",
                                "User Wise Sale Report"
                                };
        static string[] ReturnReportNames = {"Invoice Wise Sales Return Report",
                                "Customer Wise Sales Return Report"
                                };

        public static string getSalesReportName(SalesReportType SalesReport)
        {
            return ReportNames[(int)SalesReport - 1];
        }
        public static string getSalesReturnReportName(SalesReportType SalesReport)
        {
            return ReturnReportNames[(int)SalesReport - 1];
        }

    }

    public abstract class SalesReport : Report
    {

    }

    //By Customer wise sales report
    public class SalesReportByCustomer : SalesReport
    {
        public Entrytype entrytype;
        public long[] CustomerIds { get; set; }
        public string[] Customers { get; set; }
        public string[] AllType { get; set; }
        SalesReportType Type = SalesReportType.BY_CUSTOMER;
        IList<SalesReportByCustomerLineItem> _LineItems = new List<SalesReportByCustomerLineItem>();
        public IList<SalesReportByCustomerLineItem> LineItems
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
            string reportTitle = entrytype == Entrytype.SALE ? SalesReportNames.getSalesReportName(Type) : SalesReportNames.getSalesReturnReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
            return null;
        }

        public override void GenerateReport()
        {
            List<SaleEntry> Sales = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                //fetch from multiple customer
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    if (entrytype == Entrytype.SALE)
                    {
                        if (AllType != null)
                        {
                            if (AllType.Contains("All"))
                            {
                                Sales = (from SaleEntry in Context.SaleEntry where (SaleEntry.EntryType == entrytype && SaleEntry.CompanyId == this.Company.CompanyId && SaleEntry.SaleDate >= this.FromDate && SaleEntry.SaleDate <= this.ToDate) select SaleEntry).ToList();
                            }
                            else if (CustomerIds.ToList().Count > 0)
                            {
                                Sales = (from SaleEntry in Context.SaleEntry where (SaleEntry.EntryType == entrytype && SaleEntry.CompanyId == this.Company.CompanyId && CustomerIds.Contains((long)SaleEntry.AccountsId) && SaleEntry.SaleDate >= this.FromDate && SaleEntry.SaleDate <= this.ToDate) select SaleEntry).ToList();
                            }
                        }
                        else
                        {
                            Sales = (from SaleEntry in Context.SaleEntry where (SaleEntry.EntryType == entrytype && SaleEntry.CompanyId == this.Company.CompanyId && CustomerIds.Contains((long)SaleEntry.AccountsId) && SaleEntry.SaleDate >= this.FromDate && SaleEntry.SaleDate <= this.ToDate) select SaleEntry).ToList();
                        }
                    }
                    else if (entrytype == Entrytype.RETURN)
                    {
                        if (AllType != null)
                        {
                            if (AllType.Contains("All"))
                            {
                                Sales = (from SaleEntry in Context.SaleEntry where (SaleEntry.EntryType == entrytype && SaleEntry.CompanyId == this.Company.CompanyId && SaleEntry.SaleDate >= this.FromDate && SaleEntry.SaleDate <= this.ToDate) select SaleEntry).ToList();
                            }
                            else if (Customers.ToList().Count > 0)
                            {
                                Sales = (from SaleEntry in Context.SaleEntry where (SaleEntry.EntryType == entrytype && SaleEntry.CompanyId == this.Company.CompanyId && Customers.Contains(SaleEntry.CustomerName) && SaleEntry.SaleDate >= this.FromDate && SaleEntry.SaleDate <= this.ToDate) select SaleEntry).ToList();
                            }
                        }
                        else
                        {
                            Sales = (from SaleEntry in Context.SaleEntry where (SaleEntry.EntryType == entrytype && SaleEntry.CompanyId == this.Company.CompanyId && Customers.Contains(SaleEntry.CustomerName) && SaleEntry.SaleDate >= this.FromDate && SaleEntry.SaleDate <= this.ToDate) select SaleEntry).ToList();
                        }
                    }
                }
            }
            if (Sales != null && Sales.Count > 0)
            {
                foreach (SaleEntry Sale in Sales)
                {
                    SalesReportByCustomerLineItem LineItem = new SalesReportByCustomerLineItem(Sale);
                    LineItems.Add(LineItem);
                }
            }
        }
    }
    public class SalesReportByCustomerLineItem
    {
        public String CustomerInvoiceNumber { get; set; }
        public DateTime CustomerInvoiceDate { get; set; }
        public Double CustomerInvoiceTax { get; set; }
        public Double CustomerInvoiceDiscount { get; set; }
        public Double CustomerInvoiceAmount { get; set; }
        public String InvoiceType { get; set; }
        public String Customer { get; set; }

        public SalesReportByCustomerLineItem()
        {
        }

        public SalesReportByCustomerLineItem(SaleEntry Sale)
        {
            this.CustomerInvoiceNumber = Sale.RefNumber;
            this.CustomerInvoiceDate = Sale.SaleDate;
            this.CustomerInvoiceTax = Sale.TaxAmount;
            this.CustomerInvoiceDiscount = Sale.DisAmount;
            this.InvoiceType = Sale.SaleMethod == SaleMethod.Credit ? "Credit" : "Cash";
            this.CustomerInvoiceAmount = Sale.TotalAmount;
            this.Customer = Sale.CustomerName != string.Empty ? Sale.CustomerName : "Unknown Customer";
        }
    }

    //By Invoice sales report
    public class SalesReportByInvoice : SalesReport
    {
        public Entrytype entrytype;
        SalesReportType Type = SalesReportType.BY_INVOICE;
        IList<SalesReportByInvoiceLineItem> _LineItems = new List<SalesReportByInvoiceLineItem>();

        public IList<SalesReportByInvoiceLineItem> LineItems
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
            string reportTitle = entrytype == Entrytype.SALE ? SalesReportNames.getSalesReportName(Type) : SalesReportNames.getSalesReturnReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
            return null;
        }

        //generate the report
        public override void GenerateReport()
        {
            List<SaleEntry> Sales = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    //Should include the pagination later on when needed.
                    Sales = (from SaleEntry in Context.SaleEntry where (SaleEntry.EntryType == entrytype && SaleEntry.CompanyId == this.Company.CompanyId && SaleEntry.SaleDate >= this.FromDate && SaleEntry.SaleDate <= this.ToDate) select SaleEntry).ToList();
                }
            }
            if (Sales != null && Sales.Count > 0)
            {
                foreach (SaleEntry Sale in Sales)
                {
                    SalesReportByInvoiceLineItem LineItem = new SalesReportByInvoiceLineItem(Sale);
                    LineItems.Add(LineItem);
                }
            }
        }
    }
    public class SalesReportByInvoiceLineItem
    {
        public String InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public String CustomerName { get; set; }
        public String CustomerAddress { get; set; }
        public Double InvoiceAmount { get; set; }
        public Double InvoiceTax { get; set; }
        public Double InvoiceNetAmount { get; set; }
        public String InvoiceType { get; set; }
        public long PaymentType { get; set; }

        public SalesReportByInvoiceLineItem()
        {
            //Default Constructor
        }

        public SalesReportByInvoiceLineItem(SaleEntry Sale)
        {
            this.InvoiceNetAmount = Sale.NetAmount;
            this.InvoiceTax = Sale.TaxAmount;
            this.InvoiceAmount = Sale.TotalAmount;
            this.InvoiceType = Sale.SaleMethod == SaleMethod.Credit ? "Credit" : "Cash";
            this.CustomerName = Sale.CustomerName;
            this.CustomerAddress = Sale.CustomerAddress.Replace("\r", "").Replace("\n", "");
            this.InvoiceNumber = Sale.RefNumber;
            this.InvoiceDate = Sale.SaleDate;
        }
    }

    //By User sales report
    public class SalesReportByUser : SalesReport
    {
        public Entrytype entrytype;
        public string PaymentType { get; set; }

        SalesReportType Type = SalesReportType.BY_USER;
        public long[] UserId { get; set; }
        IList<SalesReportByUserLineItem> _LineItems = new List<SalesReportByUserLineItem>();

        public IList<SalesReportByUserLineItem> LineItems
        {
            get
            {
                return _LineItems;
            }
        }
        public override string ReportName()
        {
            string ReportName = "Sales Report By User Wise";
            return ReportName;
        }
        public override string ReportTitle()
        {
            string reportTitle = entrytype == Entrytype.SALE ? SalesReportNames.getSalesReportName(Type) : SalesReportNames.getSalesReturnReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
            return null;
        }
        public override void GenerateReport()
        {
            List<SaleEntry> Sales = new List<SaleEntry>();
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                List<User> users = new List<User>();
                foreach (long id in UserId)
                {
                    User user = UserManager.Instance.GetUserById(id);
                    if (user != null)
                    {
                        users.Add(user);
                    }
                }

                List<string> userNames = users.Select(u => u.FirstName).ToList();
                if (PaymentType == "All")
                {
                    Sales = Context.SaleEntry.Include("SalePayment").Where(s => s.EntryType == entrytype && s.CompanyId == this.Company.CompanyId && s.SaleDate >= this.FromDate && s.SaleDate <= this.ToDate).AsEnumerable().Where(s => userNames.Any(user => s.CreatedBy.Contains(user))).ToList();
                    if (Sales != null && Sales.Count > 0)
                    {
                        foreach (SaleEntry sale in Sales)
                        {
                            IList<Payment> Payment = PaymentManager.Instance.ListAllUnAppliedPaymentPaymentBySale(sale.Id).ToList();
                            foreach (Payment lPayment in Payment)
                            {
                                SalesReportByUserLineItem LineItem = new SalesReportByUserLineItem(sale, lPayment);
                                LineItems.Add(LineItem);
                            }
                        }
                    }
                }
                else
                {
                    Sales = Context.SaleEntry.Include("SalePayment").Where(s => s.EntryType == entrytype && s.CompanyId == this.Company.CompanyId && s.SaleDate >= this.FromDate && s.SaleDate <= this.ToDate).AsEnumerable().Where(s => userNames.Any(user => s.CreatedBy.Contains(user)) && s.SalePayment != null).ToList();
                    if (Sales != null && Sales.Count > 0)
                    {
                        foreach (SaleEntry sale in Sales)
                        {
                            IList<Payment> Payment = PaymentManager.Instance.ListAllUnAppliedPaymentPaymentBySale(sale.Id).Where(x => x.TransctionType.ToString() == PaymentType).ToList();
                            foreach (Payment lPayment in Payment)
                            {
                                SalesReportByUserLineItem LineItem = new SalesReportByUserLineItem(sale, lPayment);
                                LineItems.Add(LineItem);
                            }
                        }
                    }
                }
            }
        }
    }
    public class SalesReportByUserLineItem
    {
        public String InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public String CustomerName { get; set; }
        public Double InvoiceAmount { get; set; }
        public Double InvoiceTax { get; set; }
        public PaymentType? PaymentType { get; set; }
        public Decimal ReceiveAmount { get; set; }
        public String User { get; set; }
        public SalesReportByUserLineItem()
        {
            //Default Constructor
        }

        public SalesReportByUserLineItem(SaleEntry Sale, Payment lPayment)
        {
            this.InvoiceTax = Sale.TaxAmount;
            this.InvoiceAmount = Sale.TotalAmount;
            this.CustomerName = Sale.CustomerName;
            this.InvoiceNumber = Sale.RefNumber;
            this.InvoiceDate = Sale.SaleDate;
            if (Sale.PaymentId != null)
            {
                this.PaymentType = lPayment.TransctionType;
                this.ReceiveAmount = lPayment.Amount;
            }
            else
            {
                this.PaymentType = null;
                this.ReceiveAmount = 0;
            }
            this.User = Sale.CreatedBy;
        }
    }

    //By Area sales report
    public class SalesReportByArea : SalesReport
    {
        public Entrytype entrytype;
        public string[] Areas { get; set; }
        public bool IsAllArea { get; set; }
        public bool IsUnknownAreas { get; set; }
        SalesReportType Type = SalesReportType.BY_AREA;
        public string[] ProductFamilyNames { get; set; }
        IList<SalesReportByAreaLineItem> _LineItems = new List<SalesReportByAreaLineItem>();

        public IList<SalesReportByAreaLineItem> LineItems
        {
            get
            {
                return _LineItems;
            }
        }
        public override string ReportName()
        {
            string CurrentDate = DateTime.Now.ToString(Company.DateFormat);
            char Separator = CurrentDate.Contains("-") ? '-' : CurrentDate.Contains("/") ? '/' : '.';
            string[] Date = CurrentDate.Split(Separator);
            return String.Format("Area Wise Sales {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }

        public override string ReportTitle()
        {
            string reportTitle = entrytype == Entrytype.SALE ? SalesReportNames.getSalesReportName(Type) : SalesReportNames.getSalesReturnReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
            return null;
        }

        //generate the report
        public override void GenerateReport()
        {
            List<SaleDetail> Sales = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    if (ProductFamilyNames.ToList().Count > 0)
                    {
                        var ProductIds = (from product in Context.Products
                                          join productFamily in Context.ProductFamilies on product.ProductFamilyId equals productFamily.Id
                                          where product.CompanyId == Company.CompanyId
                                                && product.Type == CatalogItemType.PRODUCT
                                                && ProductFamilyNames.Contains(productFamily.Name)
                                          select product.Id).ToList();

                        Sales = (from SaleDetails in Context.SaleDetail
                                 .Include("Sale")
                                 .Include("Product")
                                 .Include("TaxDetails")
                                 where ProductIds.Contains((long)SaleDetails.ProductId)
                                       && SaleDetails.Sale.EntryType == Entrytype.SALE
                                       && SaleDetails.CompanyId == this.Company.CompanyId
                                       && SaleDetails.Sale.SaleDate >= this.FromDate
                                       && SaleDetails.Sale.SaleDate <= this.ToDate
                                 select SaleDetails).ToList();
                    }
                    else
                    {
                        return;
                    }

                    if (Sales != null && Sales.Count > 0)
                    {
                        foreach (SaleDetail Sale in Sales)
                        {
                            SaleEntry saleEntry = Context.SaleEntry
                                .Include("Account")
                                .Include("Patient")
                                .FirstOrDefault(X => X.Id == Sale.SaleId);

                            Address address = GetAddressForSaleEntry(Context, saleEntry);

                            if (address == null || address.CityOrTown == null)
                            {
                                address = new Address { CityOrTown = "UnKnown Area" };
                            }

                            ProductFamily productFamily = CatalogProductFamilyManager.Instance
                                .GetProductFamilyInfoById(Sale.Product.ProductFamilyId, Company.CompanyId);

                            bool isUnknownArea = string.IsNullOrEmpty(address.CityOrTown) || address.CityOrTown == "UnKnown Area";
                            bool includeArea = IsAllArea || isUnknownArea || Areas.Contains(address.CityOrTown);

                            if (includeArea)
                            {
                                SalesReportByAreaLineItem LineItem = new SalesReportByAreaLineItem(Sale, productFamily, address);
                                SalesReportByAreaLineItem OldLineItem = FindExistingLineItem(Sale);

                                if (OldLineItem != null)
                                {
                                    LineItem.Qty += OldLineItem.Qty;
                                    LineItem.Free += OldLineItem.Free;
                                    LineItem.SubTotal += OldLineItem.SubTotal;
                                    LineItem.Tax += OldLineItem.Tax;
                                    LineItem.Total += OldLineItem.Total;

                                    LineItems.Remove(OldLineItem);
                                }

                                LineItems.Add(LineItem);
                            }
                        }
                    }
                }
            }
        }

        // Helper method to fetch address for a sale entry
        private Address GetAddressForSaleEntry(AccountMasterContext context, SaleEntry saleEntry)
        {
            if (saleEntry?.Patient != null)
            {
                return context.Patients
                    .Include("Address")
                    .FirstOrDefault(x => x.Id == saleEntry.Patient.Id)
                    ?.Address;
            }
            else if (saleEntry?.Account != null)
            {
                if (saleEntry.Account.AccountType == AccountType.CUSTOMER)
                {
                    return context.Customers
                        .Include("BillingAddress")
                        .FirstOrDefault(x => x.Id == saleEntry.Account.Id)
                        ?.BillingAddress;
                }
                else if (saleEntry.Account.AccountType == AccountType.SUPPLIER)
                {
                    return context.Suppliers
                        .Include("Address")
                        .FirstOrDefault(x => x.Id == saleEntry.Account.Id)
                        ?.Address;
                }
            }
            return null;
        }

        // Helper method to find an existing line item
        private SalesReportByAreaLineItem FindExistingLineItem(SaleDetail sale)
        {
            if (sale.isBatch)
            {
                return LineItems.FirstOrDefault(x => x.ProductId == sale.ProductId && x.BatchNumber == sale.BatchNo);
            }
            else
            {
                return LineItems.FirstOrDefault(x => x.ProductId == sale.ProductId);
            }
        }
    }

    public class SalesReportByAreaLineItem
    {
        public String Item { get; set; }
        public String ItemName { get; set; }
        public String CustomerName { get; set; }
        public double Qty { get; set; }
        public double Free { get; set; }
        public String BatchNumber { get; set; }
        public DateTime ExpDate { get; set; }
        public Double SubTotal { get; set; }
        public Double Tax { get; set; }
        public Double Total { get; set; }
        public String ProductFamily { get; set; }
        public long ProductId { get; set; }
        public String City { get; set; }

        public SalesReportByAreaLineItem()
        {
        }
        public SalesReportByAreaLineItem(SaleDetail SaleDetail, ProductFamily productFamily, Address address)
        {
            this.Item = SaleDetail.Product.MaterialId;
            this.ItemName = SaleDetail.Product.Name;
            this.CustomerName = SaleDetail.Sale.CustomerName;
            this.Qty = SaleDetail.Quantity;
            this.Free = SaleDetail.FreeQuantity;
            this.BatchNumber = SaleDetail.isBatch ? SaleDetail.BatchNo : "";
            this.ExpDate = SaleDetail.ExpDate;
            this.SubTotal = (SaleDetail.Amount - SaleDetail.TaxDetails.Sum(x => x.Amount));
            this.Tax = SaleDetail.TaxDetails.Sum(x => x.Amount);
            this.Total = SaleDetail.Amount;
            this.ProductFamily = productFamily.ToString();
            this.ProductId = (long)SaleDetail.ProductId;
            this.City = address != null && !string.IsNullOrEmpty(address.CityOrTown) ? address.CityOrTown : "UnKnown Area";
        }
    }

    //By Tax sales report
    public class SalesReportByTax : SalesReport
    {
        SalesReportType Type = SalesReportType.BY_TAX;
        IList<SalesReportByTaxLineItem> _LineItems = new List<SalesReportByTaxLineItem>();

        public IList<SalesReportByTaxLineItem> LineItems
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
            string reportTitle = SalesReportNames.getSalesReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
            return null;
        }

        //generate the report
        public override void GenerateReport()
        {
            List<SaleEntry> Sales = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    //Should include the pagination later on when needed.
                    Sales = (from SaleEntry in Context.SaleEntry.Include("SaleDetails").Include("SaleDetails.TaxDetails").Include("SaleAdditionalTransactions") where (SaleEntry.EntryType == Entrytype.SALE && SaleEntry.CompanyId == this.Company.CompanyId && SaleEntry.SaleDate >= this.FromDate && SaleEntry.SaleDate <= this.ToDate) select SaleEntry).ToList();
                }
            }
            if (Sales != null && Sales.Count > 0)
            {
                foreach (SaleEntry Sale in Sales)
                {
                    Customer Customer = null;
                    Supplier Supplier = null;
                    string Gst = string.Empty;
                    if (Sale.AccountsId != null)
                    {
                        Customer = CustomerManager.Instance.GetCustomerById((long)Sale.AccountsId);
                        if (Customer == null)
                        {
                            Supplier = SupplierManager.Instance.GetSupplierById((long)Sale.AccountsId);
                        }
                        if ((Customer != null && Customer.CustomerLicenceDetail.Count > 0)
                                || (Supplier != null && Supplier.SupplierLicenceDetail.Count > 0))
                        {
                            Gst = Customer != null && Customer.CustomerLicenceDetail.Count > 0 ? Customer.CustomerLicenceDetail.First().Value : Supplier.SupplierLicenceDetail.First().Value;
                        }
                    }
                    SalesReportByTaxLineItem LineItem = new SalesReportByTaxLineItem(Sale, Gst);
                    LineItems.Add(LineItem);
                }
            }
        }
    }

    public class SalesReportByTaxLineItem
    {
        public String InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public String CustomerName { get; set; }
        public String Gstn { get; set; }
        public Double ZeroTaxvalue { get; set; }
        public Double ZeroTaxAmount { get; set; }
        public Double FiveTaxvalue { get; set; }
        public Double FiveTaxAmount { get; set; }
        public Double TwelveTaxvalue { get; set; }
        public Double TwelveTaxAmount { get; set; }
        public Double EighteenTaxvalue { get; set; }
        public Double EighteenTaxAmount { get; set; }
        public Double TwentyeightTaxvalue { get; set; }
        public Double TwentyeightTaxAmount { get; set; }
        public Double OtherTaxvalue { get; set; }
        public Double OtherTaxAmount { get; set; }
        public Double Charges { get; set; }
        public Double Total { get; set; }

        public SalesReportByTaxLineItem()
        {
            //Default Constructor
        }

        public SalesReportByTaxLineItem(SaleEntry Sale, string Gst)
        {
            foreach (SaleDetail Detail in Sale.SaleDetails)
            {
                float TaxRate = Detail.TaxDetails.Sum(s => s.TaxRate);
                if (TaxRate == 0)
                {
                    this.ZeroTaxvalue += Detail.Amount;
                }
                else if (TaxRate == 5)
                {
                    this.FiveTaxvalue += Detail.Amount - Detail.TaxDetails.Sum(s => s.Amount);
                    this.FiveTaxAmount += Detail.TaxDetails.Sum(s => s.Amount);
                }
                else if (TaxRate == 12)
                {
                    this.TwelveTaxvalue += (Detail.Amount - Detail.TaxDetails.Sum(s => s.Amount));
                    this.TwelveTaxAmount += Detail.TaxDetails.Sum(s => s.Amount);
                }
                else if (TaxRate == 18)
                {
                    this.EighteenTaxvalue += Detail.Amount - Detail.TaxDetails.Sum(s => s.Amount);
                    this.EighteenTaxAmount += Detail.TaxDetails.Sum(s => s.Amount);
                }
                else if (TaxRate == 28)
                {
                    this.TwentyeightTaxvalue += Detail.Amount - Detail.TaxDetails.Sum(s => s.Amount);
                    this.TwentyeightTaxAmount += Detail.TaxDetails.Sum(s => s.Amount);
                }
                else
                {
                    this.OtherTaxvalue += Detail.Amount - Detail.TaxDetails.Sum(s => s.Amount);
                    this.OtherTaxAmount += Detail.TaxDetails.Sum(s => s.Amount);
                }
            }
            this.CustomerName = Sale.CustomerName;
            this.Gstn = Gst;
            this.InvoiceNumber = Sale.RefNumber;
            this.InvoiceDate = Sale.SaleDate;
            this.Charges = Sale.SaleAdditionalTransactions.Sum(x => x.Action == AdditionalTransactionAction.DR ? x.Amount : -x.Amount);
            this.Total = Sale.TotalAmount;

        }
    }

    //By Sold sales report
    public class SalesReportBySold : SalesReport
    {
        public long[] SoldIds { get; set; }
        SalesReportType Type = SalesReportType.BY_SOLD;
        IList<SalesReportByRefererLineItem> _LineItems = new List<SalesReportByRefererLineItem>();

        public IList<SalesReportByRefererLineItem> LineItems
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
            string reportTitle = SalesReportNames.getSalesReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
            return null;
        }

        //generate the report
        public override void GenerateReport()
        {
            List<SaleEntry> Sales = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    if (SoldIds != null && SoldIds.ToList().Count > 0)
                    {
                        Sales = (from SaleEntry in Context.SaleEntry.Include("SoldBy") where (SaleEntry.EntryType == Entrytype.SALE && SaleEntry.CompanyId == this.Company.CompanyId && SoldIds.Contains((long)SaleEntry.SoldById) && SaleEntry.SaleDate >= this.FromDate && SaleEntry.SaleDate <= this.ToDate) select SaleEntry).ToList();
                    }
                    else
                    {
                        //Sales = (from SaleEntry in Context.SaleEntry.Include("SoldBy") where (SaleEntry.EntryType == Entrytype.SALE && SaleEntry.CompanyId == this.Company.CompanyId && SaleEntry.SoldById != null && SaleEntry.SaleDate >= this.FromDate && SaleEntry.SaleDate <= this.ToDate) select SaleEntry).ToList();
                        return;
                    }
                }
            }
            if (Sales != null && Sales.Count > 0)
            {
                foreach (SaleEntry Sale in Sales)
                {
                    SalesReportByRefererLineItem LineItem = new SalesReportByRefererLineItem(Sale, Type);
                    LineItems.Add(LineItem);
                }
            }
        }
    }

    //By Referer sales report
    public class SalesReportByReferer : SalesReport
    {
        public long[] RefererIds { get; set; }
        SalesReportType Type = SalesReportType.BY_REFERER;
        IList<SalesReportByRefererLineItem> _LineItems = new List<SalesReportByRefererLineItem>();

        public IList<SalesReportByRefererLineItem> LineItems
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
            string reportTitle = SalesReportNames.getSalesReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
            return null;
        }

        //generate the report
        public override void GenerateReport()
        {
            List<SaleEntry> Sales = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    if (RefererIds != null && RefererIds.ToList().Count > 0)
                    {
                        Sales = (from SaleEntry in Context.SaleEntry.Include("ReferedBy") where (SaleEntry.EntryType == Entrytype.SALE && SaleEntry.CompanyId == this.Company.CompanyId && RefererIds.Contains((long)SaleEntry.ReferedById) && SaleEntry.SaleDate >= this.FromDate && SaleEntry.SaleDate <= this.ToDate) select SaleEntry).ToList();
                    }
                    else
                    {
                        //Sales = (from SaleEntry in Context.SaleEntry.Include("ReferedBy") where (SaleEntry.EntryType == Entrytype.SALE && SaleEntry.CompanyId == this.Company.CompanyId && SaleEntry.ReferedById!=null && SaleEntry.SaleDate >= this.FromDate && SaleEntry.SaleDate <= this.ToDate) select SaleEntry).ToList();
                        return;
                    }
                }
            }
            if (Sales != null && Sales.Count > 0)
            {
                foreach (SaleEntry Sale in Sales)
                {
                    SalesReportByRefererLineItem LineItem = new SalesReportByRefererLineItem(Sale, Type);
                    LineItems.Add(LineItem);
                }
            }
        }
    }

    public class SalesReportByRefererLineItem
    {
        public String InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public String CustomerName { get; set; }
        public String CustomerAddress { get; set; }
        public Double InvoiceAmount { get; set; }
        public string Referer { get; set; }
        public Double InvoiceNetAmount { get; set; }
        public String InvoiceType { get; set; }

        public SalesReportByRefererLineItem()
        {
            //Default Constructor
        }

        public SalesReportByRefererLineItem(SaleEntry Sale, SalesReportType Type)
        {
            this.InvoiceNetAmount = Sale.NetAmount;
            this.Referer = (Type == SalesReportType.BY_REFERER ? Sale.ReferedBy.Name : Sale.SoldBy.Name);
            this.InvoiceAmount = Sale.TotalAmount;
            this.InvoiceType = Sale.SaleMethod == SaleMethod.Credit ? "Credit" : "Cash";
            this.CustomerName = Sale.CustomerName;
            this.CustomerAddress = Sale.CustomerAddress;
            this.InvoiceNumber = Sale.RefNumber;
            this.InvoiceDate = Sale.SaleDate;
        }
    }

    //By Categoery Sales report
    public class SalesReportByCategory : SalesReport
    {
        public long[] CategoryIds { get; set; }
        SalesReportType Type = SalesReportType.BY_CATEGORY;
        IList<SalesReportByCategoryLineItem> _LineItems = new List<SalesReportByCategoryLineItem>();

        public IList<SalesReportByCategoryLineItem> LineItems
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
            string reportTitle = SalesReportNames.getSalesReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
            return null;
        }

        //fetch from multiple category
        public override void GenerateReport()
        {
            List<SaleDetail> Sales = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    if (CategoryIds.ToList().Count > 0)
                    {
                        var ProductIds = (from lCatalogItem in Context.CatalogItems where (((from Pfamily in Context.CatalogItems where CategoryIds.Contains((long)Pfamily.ParentId) select Pfamily.Id).ToList()).Contains((long)lCatalogItem.ParentId)) where (lCatalogItem.Type == model.Catalog.CatalogItemType.PRODUCT) select lCatalogItem.Id).ToList();
                        Sales = (from SaleDetails in Context.SaleDetail.Include("Sale").Include("Product").Include("TaxDetails") where ProductIds.Contains((long)SaleDetails.ProductId) where (SaleDetails.Sale.EntryType == Entrytype.SALE && SaleDetails.CompanyId == this.Company.CompanyId && SaleDetails.Sale.SaleDate >= this.FromDate && SaleDetails.Sale.SaleDate <= this.ToDate) select SaleDetails).ToList();
                    }
                    else
                    {
                        //Sales = (from SaleDetails in Context.SaleDetail.Include("Sale").Include("Product").Include("TaxDetails") where (SaleDetails.Sale.EntryType == Entrytype.SALE && SaleDetails.CompanyId == this.Company.CompanyId && SaleDetails.Sale.SaleDate >= this.FromDate && SaleDetails.Sale.SaleDate <= this.ToDate) select SaleDetails).ToList();
                        return;
                    }
                }
            }
            if (Sales != null && Sales.Count > 0)
            {
                foreach (SaleDetail Sale in Sales)
                {
                    CatalogItem catalogItem = CatalogItemManager.Instance.GetParentInfoById((long)Sale.ProductId);
                    SalesReportByCategoryLineItem LineItem = new SalesReportByCategoryLineItem(Sale, catalogItem);
                    SalesReportByCategoryLineItem OldLineItem = null;
                    if (Sale.isBatch)
                    {
                        OldLineItem = LineItems.FirstOrDefault(x => x.ProductId == Sale.ProductId && x.BatchNumber == Sale.BatchNo);
                    }
                    else
                    {
                        OldLineItem = LineItems.FirstOrDefault(x => x.ProductId == Sale.ProductId);
                    }
                    if (OldLineItem != null)
                    {
                        LineItem.Qty += OldLineItem.Qty;
                        LineItem.Free += OldLineItem.Free;
                        LineItem.SubTotal += OldLineItem.SubTotal;
                        LineItem.Tax += OldLineItem.Tax;
                        LineItem.Total += OldLineItem.Total;

                        LineItems.Remove(OldLineItem);
                    }
                    LineItems.Add(LineItem);
                }
            }
        }
    }
    public class SalesReportByCategoryLineItem
    {
        public String Item { get; set; }
        public String Catagory { get; set; }
        public String ItemName { get; set; }
        public String CustomerAddress { get; set; }
        public double Qty { get; set; }
        public double Free { get; set; }
        public String BatchNumber { get; set; }
        public DateTime ExpDate { get; set; }
        public Double SubTotal { get; set; }
        public Double Tax { get; set; }
        public Double Total { get; set; }
        public long ProductId { get; set; }


        public SalesReportByCategoryLineItem()
        {
        }

        public SalesReportByCategoryLineItem(SaleDetail SaleDetail, CatalogItem catalogItem)
        {
            this.Item = SaleDetail.Product.MaterialId;
            this.ItemName = SaleDetail.Product.Name;
            this.CustomerAddress = SaleDetail.Sale.CustomerAddress;
            this.Qty = SaleDetail.Quantity;
            this.Free = SaleDetail.FreeQuantity;
            this.BatchNumber = SaleDetail.isBatch ? SaleDetail.BatchNo : "";
            this.ExpDate = SaleDetail.ExpDate;
            this.SubTotal = (SaleDetail.Amount - SaleDetail.TaxDetails.Sum(x => x.Amount));
            this.Tax = SaleDetail.TaxDetails.Sum(x => x.Amount);
            this.Total = SaleDetail.Amount;
            this.ProductId = (long)SaleDetail.ProductId;
            this.Catagory = catalogItem.Parent.Parent.Name;
        }
    }
    //By Product family Sales report
    public class SalesReportByProductFamily : SalesReport
    {
        private IList<SalesReportByProductFamilyLineItem> _LineItems = new List<SalesReportByProductFamilyLineItem>();
        SalesReportType Type = SalesReportType.By_PFamily;
        public long[] PFamilyIds { get; set; }
        public string[] ProductFamilyNames { get; set; }

        public IList<SalesReportByProductFamilyLineItem> LineItems
        {
            get
            {
                return _LineItems;
            }
        }
        public override string ReportName()
        {
            string CurrentDate = DateTime.Now.ToString(Company.DateFormat);
            char Separator = CurrentDate.Contains("-") ? '-' : CurrentDate.Contains("/") ? '/' : '.';
            string[] Date = CurrentDate.Split(Separator);
            return String.Format("Product FamilyWise Sales {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        public override string ReportTitle()
        {
            string reportTitle = SalesReportNames.getSalesReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
            return null;
        }
        public override void GenerateReport()
        {
            List<SaleDetail> Sales = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    if (ProductFamilyNames.ToList().Count > 0)
                    {
                        var ProductIds = (from product in Context.Products
                                          join productFamily in Context.ProductFamilies on product.ProductFamilyId equals productFamily.Id
                                          where product.CompanyId == Company.CompanyId
                                                && product.Type == CatalogItemType.PRODUCT
                                                && ProductFamilyNames.Contains(productFamily.Name)
                                          select product.Id).ToList();
                        Sales = (from SaleDetails in Context.SaleDetail.Include("Sale").Include("Product").Include("TaxDetails") where ProductIds.Contains((long)SaleDetails.ProductId) where (SaleDetails.Sale.EntryType == Entrytype.SALE && SaleDetails.CompanyId == this.Company.CompanyId && SaleDetails.Sale.SaleDate >= this.FromDate && SaleDetails.Sale.SaleDate <= this.ToDate) select SaleDetails).ToList();
                    }
                    else
                    {
                        return;
                    }
                }
            }
            if (Sales != null && Sales.Count > 0)
            {
                foreach (SaleDetail Sale in Sales)
                {
                    ProductFamily productFamily = CatalogProductFamilyManager.Instance.GetProductFamilyInfoById(Sale.Product.ProductFamilyId, Company.CompanyId);
                    SalesReportByProductFamilyLineItem LineItem = new SalesReportByProductFamilyLineItem(Sale, productFamily);
                    SalesReportByProductFamilyLineItem OldLineItem = null;
                    if (Sale.isBatch)
                    {
                        OldLineItem = LineItems.FirstOrDefault(x => x.ProductId == Sale.ProductId && x.BatchNumber == Sale.BatchNo);
                    }
                    else
                    {
                        OldLineItem = LineItems.FirstOrDefault(x => x.ProductId == Sale.ProductId);
                    }
                    if (OldLineItem != null)
                    {
                        LineItem.Qty += OldLineItem.Qty;
                        LineItem.Free += OldLineItem.Free;
                        LineItem.SubTotal += OldLineItem.SubTotal;
                        LineItem.Tax += OldLineItem.Tax;
                        LineItem.Total += OldLineItem.Total;

                        LineItems.Remove(OldLineItem);
                    }
                    LineItems.Add(LineItem);
                }
            }
        }
        public class SalesReportByProductFamilyLineItem
        {
            public String Item { get; set; }
            public String ItemName { get; set; }
            public String CustomerAddress { get; set; }
            public double Qty { get; set; }
            public double Free { get; set; }
            public String BatchNumber { get; set; }
            public DateTime ExpDate { get; set; }
            public Double SubTotal { get; set; }
            public Double Tax { get; set; }
            public Double Total { get; set; }
            public String ProductFamily { get; set; }
            public long ProductId { get; set; }

            public SalesReportByProductFamilyLineItem()
            {
            }
            public SalesReportByProductFamilyLineItem(SaleDetail SaleDetail, ProductFamily productFamily)
            {
                this.Item = SaleDetail.Product.MaterialId;
                this.ItemName = SaleDetail.Product.Name;
                this.CustomerAddress = SaleDetail.Sale.CustomerAddress;
                this.Qty = SaleDetail.Quantity;
                this.Free = SaleDetail.FreeQuantity;
                this.BatchNumber = SaleDetail.isBatch ? SaleDetail.BatchNo : "";
                this.ExpDate = SaleDetail.ExpDate;
                this.SubTotal = (SaleDetail.Amount - SaleDetail.TaxDetails.Sum(x => x.Amount));
                this.Tax = SaleDetail.TaxDetails.Sum(x => x.Amount);
                this.Total = SaleDetail.Amount;
                this.ProductFamily = productFamily.ToString();
                this.ProductId = (long)SaleDetail.ProductId;
            }
        }
    }

    //By Item Sales report
    public class SalesReportByItem : SalesReport
    {
        public long[] ItemIds { get; set; }
        SalesReportType Type = SalesReportType.BY_ITEM;
        IList<SalesReportByCategoryLineItem> _LineItems = new List<SalesReportByCategoryLineItem>();

        public IList<SalesReportByCategoryLineItem> LineItems
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
            string reportTitle = SalesReportNames.getSalesReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
            return null;
        }

        //fetch from multiple category
        public override void GenerateReport()
        {
            List<SaleDetail> Sales = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    Sales = (from SaleDetails in Context.SaleDetail.Include("Sale").Include("Product").Include("TaxDetails") where ItemIds.Contains((long)SaleDetails.ProductId) where (SaleDetails.Sale.EntryType == Entrytype.SALE && SaleDetails.CompanyId == this.Company.CompanyId && SaleDetails.Sale.SaleDate >= this.FromDate && SaleDetails.Sale.SaleDate <= this.ToDate) select SaleDetails).ToList();
                }
            }
            if (Sales != null && Sales.Count > 0)
            {
                foreach (SaleDetail Sale in Sales)
                {
                    CatalogItem catalogItem = CatalogItemManager.Instance.GetParentInfoById((long)Sale.ProductId);
                    SalesReportByCategoryLineItem LineItem = new SalesReportByCategoryLineItem(Sale, catalogItem);
                    SalesReportByCategoryLineItem OldLineItem = null;
                    if (Sale.isBatch)
                    {
                        OldLineItem = LineItems.FirstOrDefault(x => x.ProductId == Sale.ProductId && x.BatchNumber == Sale.BatchNo);
                    }
                    else
                    {
                        OldLineItem = LineItems.FirstOrDefault(x => x.ProductId == Sale.ProductId);
                    }
                    if (OldLineItem != null)
                    {
                        LineItem.Qty += OldLineItem.Qty;
                        LineItem.Free += OldLineItem.Free;
                        LineItem.SubTotal += OldLineItem.SubTotal;
                        LineItem.Tax += OldLineItem.Tax;
                        LineItem.Total += OldLineItem.Total;

                        LineItems.Remove(OldLineItem);
                    }
                    LineItems.Add(LineItem);
                }
            }
        }
    }

    //By Serial Sales report
    public class SalesReportBySerial : SalesReport
    {
        SalesReportType Type = SalesReportType.BY_SERIAL;
        IList<SalesReportBySerialLineItem> _LineItems = new List<SalesReportBySerialLineItem>();

        public IList<SalesReportBySerialLineItem> LineItems
        {
            get
            {
                return _LineItems;
            }
        }
        public override string ReportTitle()
        {
            string reportTitle = SalesReportNames.getSalesReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
            return null;
        }
        public override string ReportName()
        {
            return null;
        }
        public override void GenerateReport()
        {
            List<SaleEntry> Sales = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    Sales = (from SaleEntry in Context.SaleEntry.Include("ReferedBy").Include("Company").Include("SaleDetails").Include("SaleDetails.Product") where (SaleEntry.EntryType == Entrytype.SALE && SaleEntry.CompanyId == this.Company.CompanyId && SaleEntry.SaleDate >= this.FromDate && SaleEntry.SaleDate <= this.ToDate) select SaleEntry).OrderByDescending(X => X.SaleDate).ToList();
                }
            }
            if (Sales != null && Sales.Count > 0)
            {
                foreach (SaleEntry Sale in Sales)
                {
                    foreach (SaleDetail detail in Sale.SaleDetails)
                    {
                        if (!string.IsNullOrEmpty(detail.Product.Schedule))
                        {
                            SalesReportBySerialLineItem LineItem = new SalesReportBySerialLineItem(Sale, detail);
                            LineItems.Add(LineItem);
                        }
                    }

                }
            }
        }
    }

    public class SalesReportBySerialLineItem
    {
        public String InvoiceNumber { get; set; }
        public DateTime Date { get; set; }
        public String CustomerName { get; set; }
        public Double SerialNetAmount { get; set; }
        public SaleMethod SaleMethod { get; set; }
        public String Batchno { get; set; }
        public String Refered { get; set; }

        public String ExpDate { get; set; }

        public double Qty { get; set; }
        public String Item { get; set; }
        public String ScheduleName { get; set; }
        public SalesReportBySerialLineItem()
        {
        }

        public SalesReportBySerialLineItem(SaleEntry Sale, SaleDetail detail)
        {
            this.SerialNetAmount = Sale.NetAmount;
            this.SaleMethod = Sale.SaleMethod;
            this.CustomerName = Sale.CustomerName + (string.IsNullOrEmpty(Sale.CustomerAddress) ? "" : "\n" + Sale.CustomerAddress);
            this.Date = Sale.SaleDate;
            this.InvoiceNumber = Sale.RefNumber;
            this.ExpDate = detail.isBatch ? detail.ExpDate.ToString(Sale.Company.DateFormat) : "";
            this.Batchno = detail.isBatch ? detail.BatchNo : "";
            this.Qty = detail.Quantity;
            this.Item = detail.Product.Name + " " + detail.Product.RetailUOM;
            this.Refered = Sale.ReferedBy != null ? Sale.ReferedBy.Name : "";
            this.ScheduleName = detail.Product.Schedule;
        }

        // Sales Report By date
        public class SalesReportByDate : SalesReport
        {
            public Entrytype entrytype;
            SalesReportType Type = SalesReportType.BY_Date;
            IList<SalesReportByDateLineItem> _LineItems = new List<SalesReportByDateLineItem>();

            public IList<SalesReportByDateLineItem> LineItems
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
                string reportTitle = SalesReportNames.getSalesReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
                return null;
            }
            public override void GenerateReport()
            {
                List<SaleEntry> Sales = null;
                using (AccountMasterContext Context = new AccountMasterContext())
                {
                    using (var dbContextTransaction = Context.Database.BeginTransaction())
                    {
                        Sales = (from SaleEntry in Context.SaleEntry.Include("SaleDetails") where (SaleEntry.EntryType == entrytype && SaleEntry.CompanyId == this.Company.CompanyId && SaleEntry.SaleDate >= this.FromDate && SaleEntry.SaleDate <= this.ToDate) select SaleEntry).ToList();
                    }
                }
                if (Sales != null && Sales.Count > 0)
                {
                    IList<SaleDetail> SaleDetails = null;
                    foreach (SaleEntry Sale in Sales)
                    {
                        double TotalSaleAmount = 0.00;
                        double TotalPurchaseAmount = 0.00;
                        double Amount = 0.00;
                        double PurchasAmount = 0.00;
                        double TotalQuantity = 0;
                        double SalesValue = 0;
                        double ActualAmount = 0;
                        SaleDetails = Sale.SaleDetails.Where(s => s.SaleId == Sale.Id).ToList();
                        foreach (SaleDetail SaleDetail in SaleDetails)
                        {
                            TotalQuantity = SaleDetail.Quantity;
                            Amount = SaleDetail.Price;
                            TotalSaleAmount += TotalQuantity * Amount;

                            Product product = null;
                            product = CatalogProductManager.Instance.GetProductInfoBySaleDetailId((long)SaleDetail.ProductId);
                            PurchasAmount = product.CostPrice;
                            TotalPurchaseAmount += TotalQuantity * PurchasAmount;
                        }
                        SalesValue += TotalSaleAmount;
                        ActualAmount += TotalPurchaseAmount;
                        double Profit = TotalSaleAmount - TotalPurchaseAmount;
                        SalesReportByDateLineItem LineItem = new SalesReportByDateLineItem(Sale, SalesValue, ActualAmount, Profit);
                        LineItems.Add(LineItem);
                    }
                }
            }
            public class SalesReportByDateLineItem
            {
                public String InvoiceNumber { get; set; }
                public DateTime InvoiceDate { get; set; }
                public double SalesValue { get; set; }
                public double ActualAmount { get; set; }
                public double Profit { get; set; }

                public SalesReportByDateLineItem()
                {
                    //Default Constructor
                }
                public SalesReportByDateLineItem(SaleEntry Sale, double SalesValue, double ActualAmount, double Profit)
                {
                    this.InvoiceNumber = Sale.RefNumber;
                    this.InvoiceDate = Sale.SaleDate;
                    this.SalesValue = SalesValue;
                    this.ActualAmount = ActualAmount;
                    this.Profit = Profit;
                }
            }
        }
    }
}


