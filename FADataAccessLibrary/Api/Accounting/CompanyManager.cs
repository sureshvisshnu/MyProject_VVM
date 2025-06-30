using fa.context;
using fa.model.UserProfile;
using fa.model.Accounting.Masters;
using fa.model.System;
using fa.model.Catalog;
using Fa.api.Accounting;
using fa.model.Hms.Master;
using fa.api.System;
using fa.model.OrderManagement;
using fa.api.Hms;
using fa.model.Accounting.Transaction;
using Microsoft.EntityFrameworkCore;
using fa.model.Common;
using System.Security.Cryptography.Xml;
using System.ComponentModel.Design;
using fa.model.Accounting.Transactions;

namespace fa.api.Accounting
{
    public class CompanyManager
    {
        private static bool isLoaded = false;
        private static volatile CompanyManager instance;
        private static object syncRoot = new Object();
        CompanyManager()
        {
            if (!isLoaded)
            {

                GetCompanies();
                isLoaded = true;

            }
        }
        public static CompanyManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CompanyManager();
                    }
                }

                return instance;
            }
        }
        public string GetIdSpace(Company Company, EntryType EntryType, DateTime Date)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (Company != null)
                {
                    DateTime Start = getFiscalYearStartDate(Company, Date);
                    DateTime End = getFiscalYearEndDate(Company, Date);
                    IdSpace IdSpace = Context.IdSpaces.FirstOrDefault(x => x.CompanyId == Company.CompanyId && x.EntryType == EntryType && x.YearStartDate == Start.Date && x.YearEndDate == End.Date);
                    if (IdSpace != null)
                    {
                        if (IdSpace.EntryType == EntryType.PATIENT_ID || IdSpace.EntryType == EntryType.OP_ID)
                        {
                            return IdGenerator.IdSpaceCompanyGetNextPatientId(IdSpace, Date);
                        }
                        return IdGenerator.IdSpaceCompanyGetNextId(IdSpace, Date);
                    }
                }
            }
            return string.Empty;
        }
        public string GetMovementPrevRef(Company Company, InventoryJournalType lEntryType, DateTime Date)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (Company != null)
                {
                    DateTime Start = getFiscalYearStartDate(Company, Date);
                    DateTime End = getFiscalYearEndDate(Company, Date).AddDays(1);
                    IList<StockMovement> lStockMovement = Context.StockMovement.Where(x => x.CompanyId == Company.CompanyId && x.Type == lEntryType && x.MovementDate >= Start.Date && x.MovementDate < End.Date).ToList();
                    if (lStockMovement != null && lStockMovement.Count > 0)
                    {
                        return lStockMovement.ToList().OrderBy(x => x.CreatedDate).Last().RefNumber;
                    }
                }
            }
            return "000000";
        }
        public string GetSalePrevRef(Company Company, Entrytype lEntryType, DateTime Date)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (Company != null)
                {
                    DateTime Start = getFiscalYearStartDate(Company, Date);
                    DateTime End = getFiscalYearEndDate(Company, Date).AddDays(1);
                    if (lEntryType == Entrytype.SALE || lEntryType == Entrytype.RETURN || lEntryType == Entrytype.QUOTE)
                    {
                        IList<SaleEntry> lSaleEntry = Context.SaleEntry.Where(x => x.CompanyId == Company.CompanyId && x.EntryType == lEntryType && x.SaleDate >= Start.Date && x.SaleDate < End.Date).ToList();
                        if (lSaleEntry != null && lSaleEntry.Count > 0)
                        {
                            return lSaleEntry.ToList().OrderBy(x => x.CreatedDate).Last().RefNumber;
                        }
                    }
                }
            }
            return "000000";
        }
        public string GetPurchasePrevRef(Company Company, PurchaseEntrytype lEntryType, DateTime Date)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (Company != null)
                {
                    DateTime Start = getFiscalYearStartDate(Company, Date);
                    DateTime End = getFiscalYearEndDate(Company, Date).AddDays(1);
                    if (lEntryType == PurchaseEntrytype.PURCHASE || lEntryType == PurchaseEntrytype.RETURN)
                    {
                        IList<PurchaseEntry> lPurchaseEntry = Context.PurchaseEntry.Where(x => x.CompanyId == Company.CompanyId && x.PurchaseEntrytype == lEntryType && x.PurchaseInvDate >= Start.Date && x.PurchaseInvDate < End.Date).ToList();
                        if (lPurchaseEntry != null && lPurchaseEntry.Count > 0)
                        {
                            return lPurchaseEntry.ToList().OrderBy(x => x.CreatedDate).Last().RefNumber;
                        }
                    }
                    else if (lEntryType == PurchaseEntrytype.ORDER)
                    {
                        IList<PurchaseEntry> lPurchaseEntry = Context.PurchaseEntry.Where(x => x.CompanyId == Company.CompanyId && x.PurchaseEntrytype == lEntryType && x.RefDate >= Start.Date && x.RefDate < End.Date).ToList();
                        if (lPurchaseEntry != null && lPurchaseEntry.Count > 0)
                        {
                            return lPurchaseEntry.ToList().OrderBy(x => x.CreatedDate).Last().RefNumber;
                        }
                    }
                }
            }
            return "000000";
        }
        public string GetAccountPrevRef(Company Company, EntryType lEntryType, DateTime Date)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (Company != null)
                {
                    DateTime Start = getFiscalYearStartDate(Company, Date);
                    DateTime End = getFiscalYearEndDate(Company, Date).AddDays(1);
                    if (lEntryType == EntryType.INVOICE)
                    {
                        IList<Invoice> lInvoice = Context.Invoices.Where(x => x.CompanyId == Company.CompanyId && x.InvoiceDate >= Start.Date && x.InvoiceDate < End.Date).ToList();
                        if (lInvoice != null && lInvoice.Count > 0)
                        {
                            return lInvoice.ToList().OrderBy(x => x.CreatedDate).Last().ReferenceNumber;
                        }
                    }
                    else if (lEntryType == EntryType.RECEIPT)
                    {
                        IList<Receipt> lReceipt = Context.Receipts.Where(x => x.CompanyId == Company.CompanyId && x.TransactionDate >= Start.Date && x.TransactionDate < End.Date).ToList();
                        if (lReceipt != null && lReceipt.Count > 0)
                        {
                            return lReceipt.ToList().OrderBy(x => x.CreatedDate).Last().Reference;
                        }
                    }
                    else if (lEntryType == EntryType.BILL)
                    {
                        IList<Bill> lBill = Context.Bills.Where(x => x.CompanyId == Company.CompanyId && x.BillDate >= Start.Date && x.BillDate < End.Date).ToList();
                        if (lBill != null && lBill.Count > 0)
                        {
                            return lBill.ToList().OrderBy(x => x.CreatedDate).Last().ReferenceNumber;
                        }
                    }
                    else if (lEntryType == EntryType.PAYMENT)
                    {
                        IList<Payment> lPayment = Context.Payments.Where(x => x.CompanyId == Company.CompanyId && x.TransactionDate >= Start.Date && x.TransactionDate < End.Date).ToList();
                        if (lPayment != null && lPayment.Count > 0)
                        {
                            return lPayment.ToList().OrderBy(x => x.CreatedDate).Last().Reference;
                        }
                    }
                    else if (lEntryType == EntryType.EXPENSE)
                    {
                        IList<Expense> lExpense = Context.Expenses.Where(x => x.CompanyId == Company.CompanyId && x.TransactionDate >= Start.Date && x.TransactionDate < End.Date).ToList();
                        if (lExpense != null && lExpense.Count > 0)
                        {
                            return lExpense.ToList().OrderBy(x => x.CreatedDate).Last().Reference;
                        }
                    }
                    else if (lEntryType == EntryType.JOURNAL)
                    {
                        IList<Journal> lJournal = Context.Journals.Where(x => x.CompanyId == Company.CompanyId && x.TransactionDate >= Start.Date && x.TransactionDate < End.Date).ToList();
                        if (lJournal != null && lJournal.Count > 0)
                        {
                            return lJournal.ToList().OrderBy(x => x.CreatedDate).Last().ReferenceNumber;
                        }
                    }
                    else if (lEntryType == EntryType.CREDIT_NOTE)
                    {
                        IList<CreditNote> lCreditNotes = Context.CreditNotes.Where(x => x.CompanyId == Company.CompanyId && x.TransactionDate >= Start.Date && x.TransactionDate < End.Date).ToList();
                        if (lCreditNotes != null && lCreditNotes.Count > 0)
                        {
                            return lCreditNotes.ToList().OrderBy(x => x.CreatedDate).Last().ReferenceNumber;
                        }
                    }
                    else if (lEntryType == EntryType.DEBIT_NOTE)
                    {
                        IList<DebitNote> lDebitNotes = Context.DebitNotes.Where(x => x.CompanyId == Company.CompanyId && x.TransactionDate >= Start.Date && x.TransactionDate < End.Date).ToList();
                        if (lDebitNotes != null && lDebitNotes.Count > 0)
                        {
                            return lDebitNotes.ToList().OrderBy(x => x.CreatedDate).Last().ReferenceNumber;
                        }
                    }
                }
            }
            return "000000";
        }
        public static void GenerateIdSpace(Company Company)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                GenerateIdSpace(Company, Context);
            }
        }
        public static long CompaniesCount()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                return Context.Companies.ToList().Count;
            }
        }
        public static void GenerateIndianCompanySaleTaxAccount(Company Company, List<CompanySalesTaxAccountMap> lcompanySalesTaxAccountMaps, AccountMasterContext Context)
        {
            if (lcompanySalesTaxAccountMaps.Count > 0)
            {
                foreach (CompanySalesTaxAccountMap Map in lcompanySalesTaxAccountMaps)
                {
                    String AccountName = Map.Name == "IGST" ? "Integrated Sales Tax Payable" : Map.Name == "CGST" ? "Central Sales Tax Payable" : Map.Name == "SGST" ? "State Sales Tax Payable" : "Tax at Source Payable";
                    Account Account = Context.Accounts.FirstOrDefault(x => x.CompanyId == Company.CompanyId && x.Name == AccountName);
                    if (Account != null)
                    {
                        Map.AccountId = Account.Id;
                        Map.CompanyId = Company.CompanyId;
                        AddCompanySalesTaxAccountMaps(Map, Context);
                    }
                }
            }
        }
        public static void GeneratemMaldivesCompanySaleTaxAccount(Company Company, List<CompanySalesTaxAccountMap> lcompanySalesTaxAccountMaps, AccountMasterContext Context)
        {
            if (lcompanySalesTaxAccountMaps.Count > 0)
            {
                foreach (CompanySalesTaxAccountMap Map in lcompanySalesTaxAccountMaps)
                {
                    String AccountName = "Sales Tax Payable";
                    Account Account = Context.Accounts.FirstOrDefault(x => x.CompanyId == Company.CompanyId && x.Name == AccountName);
                    if (Account != null)
                    {
                        Map.AccountId = Account.Id;
                        Map.CompanyId = Company.CompanyId;
                        AddCompanySalesTaxAccountMaps(Map, Context);
                    }
                }
            }
        }
        private static void AddCompanySalesTaxAccountMaps(CompanySalesTaxAccountMap Map, AccountMasterContext Context)
        {
            Context.CompanySalesTaxAccountMaps.Add(Map);
            Context.SaveChanges();
        }
        public static void GenerateIdSpace(Company Company, AccountMasterContext Context)
        {
            if (Company != null)
            {
                List<IdSpace> LIdSpace = null;
                long Count = CompaniesCount();
                DateTime Start = getFiscalYearStartDate(Company);
                DateTime End = getFiscalYearEndDate(Company);
                DateTime StartDate = Start.AddYears(-1);
                DateTime EndDate = End.AddYears(-1);
                List<IdSpaceEntryTypeDetail> LIdSpaceEntryTypeDetail = Context.IdSpaceEntryTypeDetails.ToList();
                if (Count > 0)
                {
                    LIdSpace = Context.IdSpaces.Where(x => x.CompanyId == Company.CompanyId && x.YearStartDate == Start && x.YearEndDate == End).ToList();
                }
                if (LIdSpace == null || LIdSpace.Count < LIdSpaceEntryTypeDetail.Count - 4)
                {
                    for (int i = 0; i <= LIdSpaceEntryTypeDetail.Count; i++)
                    {
                        IdSpace IdSpaceFromDB = null;
                        if (Count > 0)
                        {
                            IdSpaceFromDB = Context.IdSpaces.FirstOrDefault(x => x.CompanyId == Company.CompanyId && x.EntryType == (EntryType)i && x.YearStartDate == StartDate && x.YearEndDate == EndDate);
                        }
                        if (IdSpaceFromDB != null)
                        {
                            IdSpaceEntryTypeDetail IdSpaceEntryTypeDetail = Context.IdSpaceEntryTypeDetails.FirstOrDefault(x => x.EntryType == (EntryType)i);
                            if (IdSpaceEntryTypeDetail != null)
                            {
                                IdSpace IdSpace = new IdSpace();
                                IdSpace.EntryType = IdSpaceFromDB.EntryType;
                                IdSpace.HasDotMatrix = IdSpaceFromDB.HasDotMatrix;
                                IdSpace.HasPrinterSetup = IdSpaceFromDB.HasPrinterSetup;
                                IdSpace.HasRoundOff = IdSpaceFromDB.HasRoundOff;
                                IdSpace.IsResetDaily = IdSpaceFromDB.IsResetDaily;
                                IdSpace.IsDotMatrix = IdSpaceFromDB.IsDotMatrix;
                                IdSpace.RoundOff = IdSpaceFromDB.RoundOff;
                                IdSpace.Seed = 1;
                                IdSpace.Date = fa.Data.Global.TransactionDate;
                                IdSpace.Prefix = IdSpaceFromDB.Prefix;
                                IdSpace.CompanyId = Company.CompanyId;
                                IdSpace.RunningSeed = 1;
                                IdSpace.YearStartDate = Start;
                                IdSpace.YearEndDate = End;
                                IdSpace.PrintPaperFormat_Id = IdSpaceFromDB.PrintPaperFormat_Id;
                                Context.IdSpaces.Add(IdSpace);
                            }
                        }
                        else
                        {
                            IdSpaceEntryTypeDetail IdSpaceEntryTypeDetail = Context.IdSpaceEntryTypeDetails.FirstOrDefault(x => x.EntryType == (EntryType)i);
                            if (IdSpaceEntryTypeDetail != null)
                            {
                                IdSpace IdSpace = new IdSpace();
                                IdSpace.EntryType = IdSpaceEntryTypeDetail.EntryType;
                                IdSpace.HasDotMatrix = IdSpaceEntryTypeDetail.HasDotMatrix;
                                IdSpace.HasPrinterSetup = IdSpaceEntryTypeDetail.HasPrinterSetup;
                                IdSpace.HasRoundOff = IdSpaceEntryTypeDetail.HasRoundOff;
                                IdSpace.IsResetDaily = IdSpace.EntryType == EntryType.OP_TOKEN ? true : IdSpaceEntryTypeDetail.IsResetDaily;
                                IdSpace.IsDotMatrix = IdSpaceEntryTypeDetail.IsDotMatrix;
                                IdSpace.RoundOff = IdSpaceEntryTypeDetail.RoundOff;
                                IdSpace.Seed = 1;
                                IdSpace.Date = fa.Data.Global.TransactionDate;
                                IdSpace.Prefix = IdSpaceEntryTypeDetail.Prefix;
                                IdSpace.CompanyId = Company.CompanyId;
                                IdSpace.RunningSeed = 1;
                                IdSpace.YearStartDate = Start;
                                IdSpace.YearEndDate = End;
                                IdSpace.PrintPaperFormat_Id = (IdSpace.EntryType == EntryType.PRESCRIPTION || IdSpace.EntryType == EntryType.PATIENT_INVOICE) ? 2 : (IdSpace.EntryType == EntryType.OP_TOKEN || IdSpace.EntryType == EntryType.PATIENT_FEE_RECEIPT) ? 6 : 5;
                                Context.IdSpaces.Add(IdSpace);
                            }
                        }
                        Context.SaveChanges();
                    }
                }
            }
        }
        public static DateTime getFiscalYearStartDate(Company Company)
        {
            DateTime forDate = fa.Data.Global.TransactionDate;
            int yearNumber;
            if (fa.Data.Global.TransactionDate.Month < Company.AccountingStartDate)
            {
                yearNumber = forDate.Year - 1;
            }
            else
            {
                yearNumber = forDate.Year;
            }
            return new DateTime(yearNumber, Company.AccountingStartDate, 1);
        }
        public static DateTime getFiscalYearEndDate(Company Company)
        {
            DateTime forDate = getFiscalYearStartDate(Company);
            forDate = forDate.AddMonths(12);
            forDate = forDate.AddDays(-1);
            return forDate;
        }
        public static DateTime getFiscalYearStartDate(Company Company, DateTime forDate)
        {
            int yearNumber;
            if (forDate.Month < Company.AccountingStartDate)
            {
                yearNumber = forDate.Year - 1;
            }
            else
            {
                yearNumber = forDate.Year;
            }
            return new DateTime(yearNumber, Company.AccountingStartDate, 1);
        }

        public static DateTime getFiscalYearEndDate(Company Company, DateTime forDate)
        {
            DateTime StartDate = getFiscalYearStartDate(Company, forDate);
            StartDate = StartDate.AddMonths(12);
            StartDate = StartDate.AddDays(-1);
            return StartDate;
        }
        public List<IdSpace> ListIdSpace(BuisnessType Type, long CompanyId, DateTime Start, DateTime End)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Company Company = Context.Companies.FirstOrDefault(x => x.CompanyId == CompanyId);
                if (Company != null)
                {
                    List<IdSpace> LIdSpace = null;
                    if (Type == BuisnessType.Hospital)
                    {
                        LIdSpace = Context.IdSpaces.Where(x => x.CompanyId == CompanyId && x.EntryType != EntryType.OP_TOKEN && x.EntryType != EntryType.PATIENT_FEE_RECEIPT && x.EntryType != EntryType.PATIENT_INVOICE && x.YearStartDate == Start && x.YearEndDate == End).AsEnumerable().GroupBy(x => x.EntryType).Select(g => g.First()).OrderBy(x => x.EntryType).ToList();
                    }
                    else
                    {
                        LIdSpace = Context.IdSpaces.Where(x => x.CompanyId == CompanyId && x.EntryType != EntryType.PRESCRIPTION && x.EntryType != EntryType.OP_TOKEN && x.EntryType != EntryType.PATIENT_FEE_RECEIPT && x.EntryType != EntryType.PATIENT_INVOICE && x.YearStartDate == Start && x.YearEndDate == End).AsEnumerable().GroupBy(x => x.EntryType).Select(g => g.First()).OrderBy(x => x.EntryType).ToList();
                    }
                    return LIdSpace;
                }
                return null;
            }
        }
        public List<IdSpaceEntryTypeDetail> GetIdSpaceEntryTypeDetails()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                List<IdSpaceEntryTypeDetail> idSpaceEntryTypesDetails = null!;
                idSpaceEntryTypesDetails = Context.IdSpaceEntryTypeDetails.ToList();
                return idSpaceEntryTypesDetails;
            }
        }
        public List<IdSpace> ListIdSpaceForHospital(long CompanyId, DateTime Start, DateTime End)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Company Company = Context.Companies.FirstOrDefault(x => x.CompanyId == CompanyId);
                if (Company != null)
                {
                    List<IdSpace> LIdSpace = LIdSpace = Context.IdSpaces.Where(x => x.CompanyId == CompanyId && (x.EntryType == EntryType.OP_TOKEN || x.EntryType == EntryType.PATIENT_FEE_RECEIPT || x.EntryType == EntryType.PATIENT_INVOICE) && x.YearStartDate == Start && x.YearEndDate == End).AsEnumerable().GroupBy(x => x.EntryType).Select(g => g.First()).OrderBy(x => x.EntryType).ToList();
                    return LIdSpace;
                }
                return null;
            }
        }
        public List<IdSpace> ListIdSpaceByCompanyForFiscalYear(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Company Company = Context.Companies.FirstOrDefault(x => x.CompanyId == CompanyId);
                if (Company != null)
                {
                    List<IdSpace> LIdSpace = LIdSpace = Context.IdSpaces.Where(x => x.CompanyId == CompanyId && x.EntryType == EntryType.INVOICE).ToList();
                    return LIdSpace;
                }
                return null;
            }
        }
        public static void GenerateIdSpace(long CompanyId, DateTime Start, DateTime End)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                List<IdSpaceEntryTypeDetail> LIdSpaceEntryTypeDetail = Context.IdSpaceEntryTypeDetails.ToList();
                List<IdSpace> LIdSpace = Context.IdSpaces.Where(x => x.CompanyId == CompanyId && x.YearStartDate == Start && x.YearEndDate == End).ToList();
                if (LIdSpace == null || LIdSpace.Count < LIdSpaceEntryTypeDetail.Count)
                {
                    for (int i = 0; i <= LIdSpaceEntryTypeDetail.Count; i++)
                    {
                        IdSpace IdSpaceFromDB = Context.IdSpaces.FirstOrDefault(x => x.CompanyId == CompanyId && x.EntryType == (EntryType)i && x.YearStartDate == Start && x.YearEndDate == End);
                        if (IdSpaceFromDB == null)
                        {
                            IdSpaceEntryTypeDetail IdSpaceEntryTypeDetail = Context.IdSpaceEntryTypeDetails.FirstOrDefault(x => x.EntryType == (EntryType)i);
                            if (IdSpaceEntryTypeDetail != null)
                            {
                                IdSpace IdSpace = new IdSpace();
                                IdSpace.EntryType = IdSpaceEntryTypeDetail.EntryType;
                                IdSpace.HasDotMatrix = IdSpaceEntryTypeDetail.HasDotMatrix;
                                IdSpace.HasPrinterSetup = IdSpaceEntryTypeDetail.HasPrinterSetup;
                                IdSpace.HasRoundOff = IdSpaceEntryTypeDetail.HasRoundOff;
                                IdSpace.IsResetDaily = IdSpace.EntryType == EntryType.OP_TOKEN ? true : IdSpaceEntryTypeDetail.IsResetDaily;
                                IdSpace.IsDotMatrix = IdSpaceEntryTypeDetail.IsDotMatrix;
                                IdSpace.RoundOff = IdSpaceEntryTypeDetail.RoundOff;
                                IdSpace.Seed = 1;
                                IdSpace.Date = fa.Data.Global.TransactionDate;
                                IdSpace.Prefix = string.Empty;
                                IdSpace.CompanyId = CompanyId;
                                IdSpace.RunningSeed = 1;
                                IdSpace.YearStartDate = Start;
                                IdSpace.YearEndDate = End;
                                IdSpace.PrintPaperFormat_Id = (IdSpace.EntryType == EntryType.PRESCRIPTION || IdSpace.EntryType == EntryType.PATIENT_INVOICE) ? 2 : (IdSpace.EntryType == EntryType.OP_TOKEN || IdSpace.EntryType == EntryType.PATIENT_FEE_RECEIPT) ? 6 : 5;
                                Context.IdSpaces.Add(IdSpace);
                            }
                        }
                        Context.SaveChanges();
                    }
                }
            }

        }
        public bool IsCompanyHaveIdSpaceEntries(long CompanyId)
        {
            List<IdSpace> IdSpaces = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IdSpaces = (from IdSpace in Context.IdSpaces
                            where (IdSpace.CompanyId == CompanyId)
                            select IdSpace).OrderBy(x => x.Date).ToList<IdSpace>();
                if (IdSpaces != null && IdSpaces.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public IdSpace GetCompanyIdSpaceEntries(long CompanyId)
        {
            IdSpace IdSpace = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IdSpace = Context.IdSpaces.Where(x => x.CompanyId == CompanyId).OrderBy(x => x.YearStartDate).First();
                if (IdSpace != null)
                {
                    return IdSpace;
                }
            }
            return IdSpace;
        }
        public bool IsCompanyHaveInventory(long CompanyId)
        {
            List<InventoryLocation> Location = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Location = (from Inv in Context.InventoryLocation
                            where Inv.CompanyId == CompanyId
                            select Inv).ToList<InventoryLocation>();
                if (Location != null && Location.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public Company GetCompanyForModel(long CompanyId)
        {
            Company CompanyInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CompanyInfo = Context.Companies.Where(p => p.CompanyId == CompanyId).First<Company>();
            }
            return CompanyInfo;
        }
        public static Company GetCompanyForModelForPrint(long companyId)
        {
            Company CompanyInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CompanyInfo = Context.Companies.Where(p => p.CompanyId == companyId).First<Company>();
            }
            return CompanyInfo;
        }
        public Company GetCompany(long CompanyId)
        {
            Company CompanyInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CompanyInfo = Context.Companies.Where(p => p.CompanyId == CompanyId).First<Company>();
                if (CompanyInfo != null)
                {
                    if (CompanyInfo.IdSpaces == null || CompanyInfo.IdSpaces.Count == 0)
                    {
                        CompanyInfo.IdSpaces = Context.IdSpaces.Include("PrintPaperFormat").Where(x => x.CompanyId == CompanyId).ToList();
                    }
                    if (CompanyInfo.CompanyPurchaseSetupId > 0)
                    {
                        CompanyInfo.CompanyPurchaseSetup = Context.CompanyPurchaseSetups.Where(x => x.Id == CompanyInfo.CompanyPurchaseSetupId).First();
                    }

                    if (CompanyInfo.CompanySalesSetupId > 0)
                    {
                        CompanyInfo.CompanySalesSetup = Context.CompanySalesSetups.Where(x => x.Id == CompanyInfo.CompanySalesSetupId).First();
                    }
                    if (CompanyInfo.CompanyStockMovementSetupId > 0)
                    {
                        CompanyInfo.CompanyStockMovementSetup = Context.CompanyStockMovementSetups.Where(x => x.Id == CompanyInfo.CompanyStockMovementSetupId).First();
                    }
                    if (CompanyInfo.AddressId > 0)
                    {
                        CompanyInfo.Address = Context.Addresses.Where(x => x.AddressId == CompanyInfo.AddressId).First();
                    }

                    if (CompanyInfo.CountryId > 0)
                    {
                        CompanyInfo.Country = Context.Countries.Where(x => x.Id == CompanyInfo.CountryId).First();
                    }
                    if (CompanyInfo.StateId > 0)
                    {
                        CompanyInfo.State = Context.States.Where(x => x.Id == CompanyInfo.StateId).First();
                    }

                    if (CompanyInfo.TaxInfoId > 0)
                    {
                        CompanyInfo.TaxInfo = Context.TaxInfos.Where(x => x.Id == CompanyInfo.TaxInfoId).First();
                    }

                    if (CompanyInfo.ContactInfoId > 0)
                    {
                        CompanyInfo.ContactInfo = Context.ContactInfos.Where(x => x.Id == CompanyInfo.ContactInfoId).First();
                    }
                    if (CompanyInfo.Narration != null && CompanyInfo.Narration.Count == 0)
                    {
                        CompanyInfo.Narration = Context.Narrations.Where(x => x.CompanyId == CompanyInfo.CompanyId).ToList();
                    }

                    CompanyInfo.CostCenters = Context.CostCenters.Where(x => x.ParentCompanyId == CompanyInfo.CompanyId).ToList();
                    if (CompanyInfo.CompanyTypeId > 0)
                    {
                        CompanyInfo.CompanyType = Context.CompanyTypes.Where(x => x.Id == CompanyInfo.CompanyTypeId).First();
                    }
                    if (CompanyInfo.AccountingMethodId > 0)
                    {
                        CompanyInfo.AccountingMethod = Context.AccountingMethods.Where(x => x.AccountingMethodId == CompanyInfo.AccountingMethodId).First();
                    }
                    if (CompanyInfo.PrimaryCurrencyId > 0)
                    {
                        CompanyInfo.PrimaryCurrency = Context.Currencies.Where(x => x.CurrencyId == CompanyInfo.PrimaryCurrencyId).First();
                    }

                    if (CompanyInfo.SalesTaxAccountMaps != null && CompanyInfo.SalesTaxAccountMaps.Count == 0)
                    {
                        CompanyInfo.SalesTaxAccountMaps = Context.CompanySalesTaxAccountMaps.Where(x => x.CompanyId == CompanyInfo.CompanyId).ToList();
                    }

                    if (CompanyInfo.SalesTaxAccountMaps != null && CompanyInfo.SalesTaxAccountMaps.Count == 0)
                    {
                        CompanyInfo.SalesTaxAccountMaps = Context.CompanySalesTaxAccountMaps.Include("CountrySaleTax").Where(x => x.CompanyId == CompanyInfo.CompanyId).ToList();
                    }

                    if (CompanyInfo.CashOnHandAccountId != null && CompanyInfo.CashOnHandAccountId > 0)
                    {
                        CompanyInfo.CashOnHandAccount = Context.Accounts.Where(x => x.Id == CompanyInfo.CashOnHandAccountId).First();
                    }

                    if (CompanyInfo.UndepositedFundAccountId != null && CompanyInfo.UndepositedFundAccountId > 0)
                    {
                        CompanyInfo.UndepositedFundAccount = Context.Accounts.Where(x => x.Id == CompanyInfo.UndepositedFundAccountId).First();
                    }
                    if (CompanyInfo.RoundOffAccountId != null && CompanyInfo.RoundOffAccountId > 0)
                    {
                        CompanyInfo.RoundOffAccount = Context.Accounts.Where(x => x.Id == CompanyInfo.RoundOffAccountId).First();
                    }

                    if (CompanyInfo.PurchaseAccountId != null && CompanyInfo.PurchaseAccountId > 0)
                    {
                        CompanyInfo.PurchaseAccount = Context.Accounts.Where(x => x.Id == CompanyInfo.PurchaseAccountId).First();
                    }

                    if (CompanyInfo.SalesAccountId != null && CompanyInfo.SalesAccountId > 0)
                    {
                        CompanyInfo.SalesAccount = Context.Accounts.Where(x => x.Id == CompanyInfo.SalesAccountId).First();
                    }

                    if (CompanyInfo.AccountRecivableId != null && CompanyInfo.AccountRecivableId > 0)
                    {
                        CompanyInfo.AccountRecivable = Context.Accounts.Where(x => x.Id == CompanyInfo.AccountRecivableId).First();
                    }

                    if (CompanyInfo.AccountPayableId != null && CompanyInfo.AccountPayableId > 0)
                    {
                        CompanyInfo.AccountPayable = Context.Accounts.Where(x => x.Id == CompanyInfo.AccountPayableId).First();
                    }
                    if (CompanyInfo.IncomceAccountId != null && CompanyInfo.IncomceAccountId > 0)
                    {
                        CompanyInfo.IncomceAccount = Context.Accounts.Where(x => x.Id == CompanyInfo.IncomceAccountId).First();
                    }

                    if (CompanyInfo.ExpenseAccountId != null && CompanyInfo.ExpenseAccountId > 0)
                    {
                        CompanyInfo.ExpenseAccount = Context.Accounts.Where(x => x.Id == CompanyInfo.ExpenseAccountId).First();
                    }
                    if (CompanyInfo.PatientPurchaseAccountId != null && CompanyInfo.PatientPurchaseAccountId > 0)
                    {
                        CompanyInfo.PatientPurchaseAccount = Context.Customers.Where(x => x.Id == CompanyInfo.PatientPurchaseAccountId).First();
                    }
                    if (CompanyInfo.CompanyLicence != null && CompanyInfo.CompanyLicence.Count == 0)
                    {
                        CompanyInfo.CompanyLicence = Context.CompanyLicences.Where(x => x.CompanyId == CompanyInfo.CompanyId).ToList();
                    }

                    if (CompanyInfo.CompanySupplierLicenseMaster != null && CompanyInfo.CompanySupplierLicenseMaster.Count == 0)
                    {
                        CompanyInfo.CompanySupplierLicenseMaster = Context.CompanySupplierLicenseMasters.Where(x => x.CompanyId == CompanyInfo.CompanyId).ToList();

                    }
                    if (CompanyInfo.CompanyCustomerLicenseMaster != null && CompanyInfo.CompanyCustomerLicenseMaster.Count == 0)
                    {
                        CompanyInfo.CompanyCustomerLicenseMaster = Context.CompanyCustomerLicenseMasters.Where(x => x.CompanyId == CompanyInfo.CompanyId).ToList();

                    }
                }
            }
            return CompanyInfo;
        }
        public IList<Company> GetCompanies()
        {

            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Company> CompanyInfo = Context.Companies.ToList<Company>();
                return CompanyInfo;
            }

        }
        public IList<Company> GetCompaniesBySoftwareType(SoftwareType SType)
        {

            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Company> CompanyInfo = Context.Companies.Where(t => t.BusinessType != BuisnessType.Hospital).ToList<Company>();
                if (SoftwareType.MEDICARE == SType)
                {
                    CompanyInfo = null;
                    CompanyInfo = Context.Companies.Where(t => t.BusinessType == BuisnessType.Hospital).ToList<Company>();
                }
                return CompanyInfo;
            }

        }
        public IList<Company> GetCompaniesByBusinessType(BuisnessType Type)
        {

            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Company> CompanyInfo = Context.Companies.Where(t => t.BusinessType == Type).ToList<Company>();
                return CompanyInfo;
            }

        }
        /*
         *Returns List of companies that are matching the Keys 
         */
        public IList<Company> GetCompanies(long[] Keys)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Company> CompanyInfo = (from company in Context.Companies
                                              where Keys.Contains(company.CompanyId)
                                              select company).ToList();
                return CompanyInfo;
            }
        }

        public IList<Company> GetRootCompanies(long[] Keys)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Company> CompanyInfo = (from company in Context.Companies
                                              where Keys.Contains(company.CompanyId)
                                              where company.ParentCompanyId.Equals(null)
                                              select company).ToList();
                return CompanyInfo;
            }
        }
        public void UpdateCompany(AccountMasterContext Context, Customer Customer)
        {
            try
            {
                Company CompanyInfo = Context.Companies.Find(Customer.CompanyId);
                if (CompanyInfo != null)
                {
                    CompanyInfo.PatientPurchaseAccount = null;
                    if (Customer == null)
                    {
                        CompanyInfo.PatientPurchaseAccountId = null;
                    }
                    else
                    {
                        CompanyInfo.PatientPurchaseAccountId = Customer.Id;
                    }
                    Context.Entry(Context.Companies.Find(Customer.CompanyId)).CurrentValues.SetValues(CompanyInfo);
                    Context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
        }
        public Company UpdateCompany(Company Company)
        {
            Company CompanyInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        CompanyInfo = Context.Companies.Find(Company.CompanyId);
                        if (CompanyInfo != null)
                        {
                            IList<DocumentCategory> PatientDocumentCategorys = DocumentManager.Instance.ListPatientDocumentCategoryByCompanyId(Company.CompanyId).ToList();
                            if (PatientDocumentCategorys != null)
                                foreach (var PatientDocumentCategory in PatientDocumentCategorys)
                                {
                                    var documentCategory = new DocumentCategory
                                    {
                                        Id = PatientDocumentCategory.Id,
                                        Name = PatientDocumentCategory.Name,
                                        DisplayAs = PatientDocumentCategory.DisplayAs,
                                        Discription = PatientDocumentCategory.Discription,
                                        CompanyId = Company.CompanyId
                                    };
                                    Context.DocumentCategories.Update(documentCategory);
                                    Context.SaveChanges();
                                }
                            Company.PatientPurchaseAccountId = CompanyInfo.PatientPurchaseAccountId;
                            Context.Entry(CompanyInfo).CurrentValues.SetValues(Company);
                            Context.SaveChanges();
                            Company CompanyInfoFromDB = GetCompany(Company.CompanyId);
                            DateTime Start = Company.IdSpaces.First().YearStartDate;
                            DateTime End = Company.IdSpaces.First().YearEndDate;
                            if (Company.BusinessType != BuisnessType.Hospital)
                            {
                                CompanyInfoFromDB.IdSpaces = Context.IdSpaces.Where(x => x.CompanyId == Company.CompanyId && x.YearStartDate == Start && x.YearEndDate == End && (x.EntryType != EntryType.OP_TOKEN && x.EntryType != EntryType.PATIENT_FEE_RECEIPT && x.EntryType != EntryType.PATIENT_INVOICE && x.EntryType != EntryType.PRESCRIPTION)).ToList();
                            }
                            else
                            {
                                CompanyInfoFromDB.IdSpaces = Context.IdSpaces.Where(x => x.CompanyId == Company.CompanyId && x.YearStartDate == Start && x.YearEndDate == End && (x.EntryType != EntryType.OP_TOKEN && x.EntryType != EntryType.PATIENT_FEE_RECEIPT && x.EntryType != EntryType.PATIENT_INVOICE)).ToList();
                                if (CompanyInfoFromDB.IdSpaces != null)
                                {
                                    //update op token daily update
                                    IdSpace OldInfo = Context.IdSpaces.FirstOrDefault(x => x.CompanyId == Company.CompanyId && x.YearStartDate == Start && x.YearEndDate == End && x.EntryType == EntryType.OP_TOKEN);
                                    if (OldInfo != null)
                                    {
                                        OldInfo.IsResetDaily = true;
                                        Context.Entry(Context.IdSpaces.Find(OldInfo.Id)).CurrentValues.SetValues(OldInfo);
                                        Context.SaveChanges();
                                    }
                                }
                            }


                            //license
                            foreach (CompanyLicence OldInfo in CompanyInfoFromDB.CompanyLicence)
                            {
                                CompanyLicence NewInfo = Company.CompanyLicence.FirstOrDefault(x => x.Id == OldInfo.Id);
                                if (NewInfo == null)
                                {
                                    Context.CompanyLicences.Remove(Context.CompanyLicences.FirstOrDefault(x => x.Id == OldInfo.Id));
                                }
                                else
                                {
                                    Company.CompanyLicence.Remove(NewInfo);
                                    NewInfo.CompanyId = Company.CompanyId;
                                    Context.Entry(NewInfo).State = EntityState.Modified;
                                }
                                Context.SaveChanges();
                            }
                            foreach (CompanyLicence Info in Company.CompanyLicence)
                            {
                                Info.CompanyId = Company.CompanyId;
                                Context.CompanyLicences.Add(Info);
                                Context.SaveChanges();
                            }
                            //customer
                            foreach (CompanyCustomerLicenseMaster OldInfo in CompanyInfoFromDB.CompanyCustomerLicenseMaster)
                            {
                                CompanyCustomerLicenseMaster NewInfo = Company.CompanyCustomerLicenseMaster.FirstOrDefault(x => x.Id == OldInfo.Id);
                                if (NewInfo == null)
                                {
                                    Context.CompanyCustomerLicenseMasters.Remove(Context.CompanyCustomerLicenseMasters.FirstOrDefault(x => x.Id == OldInfo.Id));
                                }
                                else
                                {
                                    Company.CompanyCustomerLicenseMaster.Remove(NewInfo);
                                    NewInfo.CompanyId = Company.CompanyId;
                                    Context.Entry(NewInfo).State = EntityState.Modified;
                                }
                                Context.SaveChanges();
                            }
                            foreach (CompanyCustomerLicenseMaster Info in Company.CompanyCustomerLicenseMaster)
                            {
                                Info.CompanyId = Company.CompanyId;
                                Context.CompanyCustomerLicenseMasters.Add(Info);
                                Context.SaveChanges();
                            }
                            //supplier
                            foreach (CompanySupplierLicenseMaster OldInfo in CompanyInfoFromDB.CompanySupplierLicenseMaster)
                            {
                                CompanySupplierLicenseMaster NewInfo = Company.CompanySupplierLicenseMaster.FirstOrDefault(x => x.Id == OldInfo.Id);
                                if (NewInfo == null)
                                {
                                    Context.CompanySupplierLicenseMasters.Remove(Context.CompanySupplierLicenseMasters.FirstOrDefault(x => x.Id == OldInfo.Id));
                                }
                                else
                                {
                                    Company.CompanySupplierLicenseMaster.Remove(NewInfo);
                                    NewInfo.CompanyId = Company.CompanyId;
                                    Context.Entry(NewInfo).State = EntityState.Modified;
                                }
                                Context.SaveChanges();
                            }
                            foreach (CompanySupplierLicenseMaster Info in Company.CompanySupplierLicenseMaster)
                            {
                                Info.CompanyId = Company.CompanyId;
                                Context.CompanySupplierLicenseMasters.Add(Info);
                                Context.SaveChanges();
                            }
                            //Reference Setup
                            foreach (IdSpace OldInfo in CompanyInfoFromDB.IdSpaces)
                            {
                                IdSpace NewInfo = Company.IdSpaces.FirstOrDefault(x => x.EntryType == OldInfo.EntryType);
                                if (NewInfo == null)
                                {
                                    Context.IdSpaces.Remove(Context.IdSpaces.FirstOrDefault(x => x.Id == OldInfo.Id));
                                }
                                else
                                {
                                    //Remove
                                    Company.IdSpaces.Remove(NewInfo);
                                    //Update                                    
                                    NewInfo.CompanyId = OldInfo.CompanyId;
                                    NewInfo.Id = OldInfo.Id;
                                    NewInfo.HasDotMatrix = OldInfo.HasDotMatrix;
                                    NewInfo.HasRoundOff = OldInfo.HasRoundOff;
                                    NewInfo.RunningSeed = OldInfo.RunningSeed;
                                    NewInfo.HasPrinterSetup = OldInfo.HasPrinterSetup;
                                    Context.Entry(Context.IdSpaces.Find(OldInfo.Id)).CurrentValues.SetValues(NewInfo);
                                }
                                Context.SaveChanges();
                            }
                            foreach (IdSpace Info in Company.IdSpaces)
                            {
                                IdSpaceEntryTypeDetail Detail = Context.IdSpaceEntryTypeDetails.FirstOrDefault(x => x.EntryType == Info.EntryType);
                                if (Detail != null)
                                {
                                    Info.CompanyId = Company.CompanyId;
                                    Info.HasPrinterSetup = Detail.HasPrinterSetup;
                                    Info.HasRoundOff = Detail.HasRoundOff;
                                    Info.HasDotMatrix = Detail.HasDotMatrix;
                                    Context.IdSpaces.Add(Info);
                                    Context.SaveChanges();
                                }
                            }
                            if (CompanyInfoFromDB.CompanyPurchaseSetup != null)
                            {
                                CompanyPurchaseSetup PurchaseSetupFromDB = Context.CompanyPurchaseSetups.Include("AdditionalTransactions").Where(p => p.Id == CompanyInfoFromDB.CompanyPurchaseSetupId).First<CompanyPurchaseSetup>();
                                CompanyPurchaseSetup lCompanyPurchaseSetup = new CompanyPurchaseSetup();
                                lCompanyPurchaseSetup = Company.CompanyPurchaseSetup;
                                CompanyPurchaseSetup llCompanyPurchaseSetup = Context.CompanyPurchaseSetups.Find(PurchaseSetupFromDB.Id);
                                Context.Entry(llCompanyPurchaseSetup).CurrentValues.SetValues(lCompanyPurchaseSetup);
                                Context.SaveChanges();
                                //AdditionalTransaction
                                foreach (CompanyAdditionalTransactionSetup OldTransaction in PurchaseSetupFromDB.AdditionalTransactions)
                                {
                                    CompanyAdditionalTransactionSetup NewTransaction = Company.CompanyPurchaseSetup.AdditionalTransactions.FirstOrDefault(x => x.Id == OldTransaction.Id);
                                    if (NewTransaction == null)
                                    {
                                        Context.CompanyAdditionalTransactionSetups.Where(p => p.CompanyPurchaseSetup.Any(x => x.Id == PurchaseSetupFromDB.Id) && p.Id == OldTransaction.Id).ToList().ForEach(p => Context.CompanyAdditionalTransactionSetups.Remove(p));
                                    }
                                    else
                                    {
                                        Company.CompanyPurchaseSetup.AdditionalTransactions.Remove(NewTransaction);
                                        //Context.Entry(NewTransaction).State = EntityState.Modified;
                                        Context.Entry(OldTransaction).CurrentValues.SetValues(NewTransaction);

                                    }
                                    Context.SaveChanges();
                                }
                                foreach (CompanyAdditionalTransactionSetup Transaction in Company.CompanyPurchaseSetup.AdditionalTransactions)
                                {
                                    Context.CompanyPurchaseSetups.Include("AdditionalTransactions").FirstOrDefault(x => x.Id == PurchaseSetupFromDB.Id).AdditionalTransactions.Add(Transaction);
                                    Context.SaveChanges();
                                }
                            }

                            if (CompanyInfoFromDB.CompanySalesSetup != null)
                            {
                                CompanySalesSetup CompanySalesSetupFromDB = GetCompanySalesSetupWithInclude(CompanyInfoFromDB);
                                CompanySalesSetup lCompanySalesSetup = new CompanySalesSetup();
                                lCompanySalesSetup = Company.CompanySalesSetup;
                                CompanySalesSetup llCompanySalesSetup = Context.CompanySalesSetups.Find(CompanySalesSetupFromDB.Id);
                                Context.Entry(llCompanySalesSetup).CurrentValues.SetValues(lCompanySalesSetup);
                                Context.SaveChanges();

                                foreach (CompanyAdditionalTransactionSetup OldTransaction in CompanySalesSetupFromDB.AdditionalTransactions)
                                {
                                    CompanyAdditionalTransactionSetup NewTransaction = Company.CompanySalesSetup.AdditionalTransactions.FirstOrDefault(x => x.Id == OldTransaction.Id);
                                    if (NewTransaction == null)
                                    {
                                        Context.CompanyAdditionalTransactionSetups.Where(p => p.CompanySalesSetup.Any(x => x.Id == CompanySalesSetupFromDB.Id) && p.Id == OldTransaction.Id).ToList().ForEach(p => Context.CompanyAdditionalTransactionSetups.Remove(p));
                                    }
                                    else
                                    {
                                        Company.CompanySalesSetup.AdditionalTransactions.Remove(NewTransaction);
                                        Context.Entry(NewTransaction).State = EntityState.Modified;
                                    }
                                    Context.SaveChanges();
                                }
                                foreach (CompanyAdditionalTransactionSetup Transaction in Company.CompanySalesSetup.AdditionalTransactions)
                                {
                                    Context.CompanySalesSetups.Include("AdditionalTransactions").FirstOrDefault(x => x.Id == CompanySalesSetupFromDB.Id).AdditionalTransactions.Add(Transaction);
                                    Context.SaveChanges();
                                }
                            }


                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        CompanyInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return CompanyInfo;
        }


        public Company AddCompany(Company company)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    //try
                    //{
                    List<CompanySalesTaxAccountMap> lcompanySalesTaxAccountMaps = company.SalesTaxAccountMaps.ToList();
                    company.SalesTaxAccountMaps = null;
                    Context.Companies.Add(company);
                    Context.SaveChanges();
                    CompanyInitializer Initializer = new CompanyInitializer();
                    Initializer.InitializeCompany(company.CompanyId, Context);
                    if (company.CountryId != null)
                    {
                        Country Country = Context.Countries.Find(company.CountryId);
                        if (Country != null && Country.Name == "India")
                        {
                            GenerateIndianCompanySaleTaxAccount(company, lcompanySalesTaxAccountMaps, Context);
                        }
                        else if (Country != null && Country.Name == "Maldives")
                        {
                            GeneratemMaldivesCompanySaleTaxAccount(company, lcompanySalesTaxAccountMaps, Context);
                        }
                    }
                    GenerateIdSpace(company, Context);
                    dbContextTransaction.Commit();
                    //}
                    //catch (Exception e)
                    //{
                    //    Console.WriteLine(e.Message);
                    //    company = null;
                    //    dbContextTransaction.Rollback();
                    //}
                }

            }
            return company;
        }

        public Boolean DeleteCompany(long CompanyId)
        {
            Boolean Deleted = false;
            using (AccountMasterContext context = new AccountMasterContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        Company Company = GetCompany(CompanyId);
                        if (Company != null)
                        {

                            Company CompanyInfo = context.Companies.Find(Company.CompanyId);
                            CompanyInfo.CashOnHandAccountId = null;
                            CompanyInfo.UndepositedFundAccountId = null;
                            CompanyInfo.PurchaseAccountId = null;
                            CompanyInfo.SalesAccountId = null;
                            CompanyInfo.SalesReturnFeeAccountId = null;
                            CompanyInfo.AccountRecivableId = null;
                            CompanyInfo.AccountPayableId = null;
                            CompanyInfo.IncomceAccountId = null;
                            CompanyInfo.ExpenseAccountId = null;
                            CompanyInfo.RoundOffAccountId = null;
                            context.Entry(CompanyInfo).CurrentValues.SetValues(CompanyInfo);
                            context.SaveChanges();

                            //double entry
                            context.DoubleEntries.Where(x => x.CompanyId == CompanyId).ToList().ForEach(p => context.DoubleEntries.Remove(p));
                            context.SaveChanges();

                            //transaction
                            context.ReceiptDetails.Where(x => x.Receipt.CompanyId == CompanyId).ToList().ForEach(p => context.ReceiptDetails.Remove(p));
                            context.SaveChanges();

                            context.Receipts.Where(x => x.CompanyId == CompanyId).ToList().ForEach(p => context.Receipts.Remove(p));
                            context.SaveChanges();

                            context.PaymentDetails.Where(x => x.Payment.CompanyId == CompanyId).ToList().ForEach(p => context.PaymentDetails.Remove(p));
                            context.SaveChanges();

                            context.Payments.Where(x => x.CompanyId == CompanyId).ToList().ForEach(p => context.Payments.Remove(p));
                            context.SaveChanges();

                            context.ExpenseDetails.Where(x => x.Expense.CompanyId == CompanyId).ToList().ForEach(p => context.ExpenseDetails.Remove(p));
                            context.SaveChanges();

                            context.Expenses.Where(x => x.CompanyId == CompanyId).ToList().ForEach(p => context.Expenses.Remove(p));
                            context.SaveChanges();

                            context.DebitNoteDetails.Where(x => x.DebitNote.CompanyId == CompanyId).ToList().ForEach(p => context.DebitNoteDetails.Remove(p));
                            context.SaveChanges();

                            context.DebitNotes.Where(x => x.CompanyId == CompanyId).ToList().ForEach(p => context.DebitNotes.Remove(p));
                            context.SaveChanges();

                            context.CreditNoteDetails.Where(x => x.CreditNote.CompanyId == CompanyId).ToList().ForEach(p => context.CreditNoteDetails.Remove(p));
                            context.SaveChanges();

                            context.CreditNotes.Where(x => x.CompanyId == CompanyId).ToList().ForEach(p => context.CreditNotes.Remove(p));
                            context.SaveChanges();

                            context.InvoiceDetails.Where(x => x.Invoice.CompanyId == CompanyId).ToList().ForEach(p => context.InvoiceDetails.Remove(p));
                            context.SaveChanges();

                            context.Invoices.Where(x => x.CompanyId == CompanyId).ToList().ForEach(p => context.Invoices.Remove(p));
                            context.SaveChanges();

                            context.BillDetails.Where(x => x.Bill.CompanyId == CompanyId).ToList().ForEach(p => context.BillDetails.Remove(p));
                            context.SaveChanges();

                            context.BillAttachments.Where(x => x.Bill.CompanyId == CompanyId).ToList().ForEach(p => context.BillAttachments.Remove(p));
                            context.SaveChanges();

                            context.Bills.Where(x => x.CompanyId == CompanyId).ToList().ForEach(p => context.Bills.Remove(p));
                            context.SaveChanges();

                            //sale
                            context.SaleDetail.Include("Product").Include("Discounts").Include("TaxDetails").Where(p => p.CompanyId == CompanyId).ToList().ForEach(p => context.SaleDetail.Remove(p));
                            context.SaveChanges();

                            context.SaleAdditionalTransactions.Where(p => p.SaleEntry.CompanyId == CompanyId).ToList().ForEach(p => context.SaleAdditionalTransactions.Remove(p));
                            context.SaveChanges();

                            context.OrderLevelSaleDiscounts.Where(p => p.SaleEntry.CompanyId == CompanyId).ToList().ForEach(p => context.OrderLevelSaleDiscounts.Remove(p));
                            context.SaveChanges();

                            context.OrderLevelSaleTaxDetails.Where(p => p.SaleEntry.CompanyId == CompanyId).ToList().ForEach(p => context.OrderLevelSaleTaxDetails.Remove(p));
                            context.SaveChanges();

                            context.SaleEntry.Where(x => x.CompanyId == CompanyId).ToList().ForEach(p => context.SaleEntry.Remove(p));
                            context.SaveChanges();
                            //purchase
                            context.PurchaseDetails.Include("Product").Include("Discounts").Include("TaxDetails").Where(p => p.CompanyId == CompanyId).ToList().ForEach(p => context.PurchaseDetails.Remove(p));
                            context.SaveChanges();

                            context.PurchaseAdditionalTransactionses.Where(p => p.PurchaseEntry.CompanyId == CompanyId).ToList().ForEach(p => context.PurchaseAdditionalTransactionses.Remove(p));
                            context.SaveChanges();

                            context.OrderLevelPurchaseDiscounts.Where(p => p.PurchaseEntry.CompanyId == CompanyId).ToList().ForEach(p => context.OrderLevelPurchaseDiscounts.Remove(p));
                            context.SaveChanges();

                            context.OrderLevelPurchaseTaxDetails.Where(p => p.PurchaseEntry.CompanyId == CompanyId).ToList().ForEach(p => context.OrderLevelPurchaseTaxDetails.Remove(p));
                            context.SaveChanges();

                            context.PurchaseAttachments.Where(p => p.PurchaseEntry.CompanyId == CompanyId).ToList().ForEach(p => context.PurchaseAttachments.Remove(p));
                            context.SaveChanges();

                            context.PurchaseEntry.Where(x => x.CompanyId == CompanyId).ToList().ForEach(p => context.PurchaseEntry.Remove(p));
                            context.SaveChanges();

                            //inventory
                            context.InventoryBatches.Where(x => x.CompanyId == CompanyId).ToList().ForEach(p => context.InventoryBatches.Remove(p));
                            context.SaveChanges();

                            context.Inventories.Where(x => x.CompanyId == CompanyId).ToList().ForEach(p => context.Inventories.Remove(p));
                            context.SaveChanges();

                            //account

                            context.Employees.Where(x => x.CompanyId == CompanyId).ToList().ForEach(p => context.Employees.Remove(p));
                            context.SaveChanges();

                            context.SupplierLicenceDetails.Where(x => x.CompanyId == CompanyId).ToList().ForEach(x => context.SupplierLicenceDetails.Remove(x));
                            context.SaveChanges();

                            Supplier lSupplier = context.Suppliers.FirstOrDefault(x => x.CompanyId == Company.CompanyId && x.AccountType == AccountType.SUPPLIER);
                            if (lSupplier != null)
                            {
                                context.Suppliers.Where(x => x.CompanyId == Company.CompanyId && x.AccountType == AccountType.SUPPLIER).ToList().ForEach(x => context.Suppliers.Remove(x));
                                context.SaveChanges();
                            }

                            context.CustomerLicenceDetails.Where(x => x.CompanyId == CompanyId).ToList().ForEach(x => context.CustomerLicenceDetails.Remove(x));
                            context.SaveChanges();

                            Customer lCustomer = context.Customers.FirstOrDefault(x => x.CompanyId == Company.CompanyId && x.AccountType == AccountType.CUSTOMER);
                            if (lCustomer != null)
                            {
                                context.Customers.Where(x => x.CompanyId == Company.CompanyId && x.AccountType == AccountType.CUSTOMER).ToList().ForEach(x => context.Customers.Remove(x));
                                context.SaveChanges();
                            }

                            context.Titles.Where(x => x.CompanyId == CompanyId).ToList().ForEach(p => context.Titles.Remove(p));
                            context.SaveChanges();

                            context.Departments.Where(x => x.CompanyId == CompanyId).ToList().ForEach(p => context.Departments.Remove(p));
                            context.SaveChanges();

                            PaymentTerm PaymentTerm = context.PaymentTerms.FirstOrDefault(x => x.CompanyId == Company.CompanyId);
                            if (PaymentTerm != null)
                            {
                                context.PaymentTerms.Where(p => p.CompanyId == Company.CompanyId).ToList().ForEach(p => context.PaymentTerms.Remove(p));
                                context.SaveChanges();
                            }

                            PaymentMethod PaymentMethod = context.PaymentMethods.FirstOrDefault(x => x.CompanyId == Company.CompanyId);
                            if (PaymentMethod != null)
                            {
                                context.PaymentMethods.Where(p => p.CompanyId == Company.CompanyId).ToList().ForEach(p => context.PaymentMethods.Remove(p));
                                context.SaveChanges();
                            }

                            Access Access = context.Accesses.FirstOrDefault(x => x.ResourceId == Company.CompanyId);
                            if (Access != null)
                            {
                                context.Accesses.Where(p => p.ResourceId == Company.CompanyId).ToList().ForEach(p => context.Accesses.Remove(p));
                                context.SaveChanges();
                            }
                            CompanyFinancialPeriods CompanyFinancialPeriods = context.CompanyFinancialPeriods.FirstOrDefault(x => x.CompanyId == Company.CompanyId);
                            if (CompanyFinancialPeriods != null)
                            {
                                context.CompanyFinancialPeriods.Where(p => p.CompanyId == Company.CompanyId).ToList().ForEach(p => context.CompanyFinancialPeriods.Remove(p));
                                context.SaveChanges();
                            }
                            if (Company.CostCenters != null)
                            {
                                context.CostCenters.Where(CostCenters => CostCenters.ParentCompanyId == Company.CompanyId).ToList().ForEach(CostCenters => context.CostCenters.Remove(CostCenters));
                                context.SaveChanges();
                            }

                            //catalog
                            IList<CatalogItem> ParentCatalogItems = context.CatalogItems.Where(x => x.CompanyId == Company.CompanyId && x.ParentId == null).ToList();
                            foreach (CatalogItem CatalogItem in ParentCatalogItems)
                            {
                                IList<CatalogItem> lCatalogItems = new List<CatalogItem>();
                                IList<CatalogItem> llCatalogItems = GetAllChildOfSelected(CatalogItem.Id, lCatalogItems, context).Reverse().ToList();
                                foreach (CatalogItem cat in llCatalogItems)
                                {
                                    CatalogItem CategoryInfo = context.CatalogItems.Find(cat.Id);
                                    context.CatalogItems.Remove(CategoryInfo);
                                    context.SaveChanges();
                                }
                            }

                            //License
                            if (Company.CompanyLicence.Count > 0)
                            {
                                context.CompanyLicences.Where(p => p.CompanyId == Company.CompanyId).ToList().ForEach(p => context.CompanyLicences.Remove(p));
                            }
                            if (Company.CompanyCustomerLicenseMaster.Count > 0)
                            {
                                context.CompanyCustomerLicenseMasters.Where(p => p.CompanyId == Company.CompanyId).ToList().ForEach(p => context.CompanyCustomerLicenseMasters.Remove(p));
                            }
                            if (Company.CompanySupplierLicenseMaster.Count > 0)
                            {
                                context.CompanySupplierLicenseMasters.Where(p => p.CompanyId == Company.CompanyId).ToList().ForEach(p => context.CompanySupplierLicenseMasters.Remove(p));
                            }
                            if (Company.Narration != null)
                            {
                                context.Narrations.Where(p => p.CompanyId == Company.CompanyId).ToList().ForEach(p => context.Narrations.Remove(p));
                            }
                            context.SaveChanges();
                        }

                        //additional transaction
                        context.CompanyAdditionalTransactionSetups.Where(p => p.CompanyPurchaseSetup.Any(x => x.Id == Company.CompanyPurchaseSetup.Id)).ToList().ForEach(p => context.CompanyAdditionalTransactionSetups.Remove(p));
                        context.CompanyAdditionalTransactionSetups.Where(p => p.CompanySalesSetup.Any(x => x.Id == Company.CompanySalesSetup.Id)).ToList().ForEach(p => context.CompanyAdditionalTransactionSetups.Remove(p));

                        //sale tax map
                        context.CompanySalesTaxAccountMaps.Where(p => p.CompanyId == Company.CompanyId).ToList().ForEach(p => context.CompanySalesTaxAccountMaps.Remove(p));

                        //general account
                        Account lAccount = context.Accounts.FirstOrDefault(x => x.CompanyId == Company.CompanyId && x.AccountType == AccountType.ACCOUNT);
                        if (lAccount != null)
                        {
                            context.Accounts.Where(x => x.CompanyId == Company.CompanyId && x.AccountType == AccountType.ACCOUNT).ToList().ForEach(x => context.Accounts.Remove(x));
                            context.SaveChanges();
                        }
                        //company
                        context.Companies.Remove(context.Companies.Find(Company.CompanyId));
                        context.SaveChanges();
                        //common
                        if (Company.Address != null)
                        {
                            context.Addresses.Where(add => add.AddressId == Company.AddressId).ToList().ForEach(add => context.Addresses.Remove(add));
                            context.SaveChanges();
                        }
                        if (Company.ContactInfo != null)
                        {
                            context.ContactInfos.Where(ci => ci.Id == Company.ContactInfo.Id).ToList().ForEach(ci => context.ContactInfos.Remove(ci));
                            context.SaveChanges();
                        }
                        if (Company.TaxInfo != null)
                        {
                            context.TaxInfos.Where(ti => ti.Id == Company.TaxInfo.Id).ToList().ForEach(ti => context.TaxInfos.Remove(ti));
                            context.SaveChanges();
                        }
                        //purchase setup
                        if (Company.CompanyPurchaseSetup != null)
                        {
                            context.CompanyPurchaseSetups.Include("AdditionalTransactions").Where(psi => psi.Id == Company.CompanyPurchaseSetup.Id).ToList().ForEach(psi => context.CompanyPurchaseSetups.Remove(psi));
                            context.SaveChanges();
                        }
                        //sale setup
                        if (Company.CompanySalesSetup != null)
                        {
                            context.CompanySalesSetups.Include("AdditionalTransactions").Where(ssi => ssi.Id == Company.CompanySalesSetup.Id).ToList().ForEach(ssi => context.CompanySalesSetups.Remove(ssi));
                            context.SaveChanges();
                        }
                        //stock movement setup
                        if (Company.CompanyStockMovementSetup != null)
                        {
                            context.CompanyStockMovementSetups.Where(ssi => ssi.Id == Company.CompanyStockMovementSetupId).ToList().ForEach(ssi => context.CompanyStockMovementSetups.Remove(ssi));
                            context.SaveChanges();
                        }
                        IList<IdSpace> lCompanyEntryConfiguration = context.IdSpaces.Where(x => x.CompanyId == Company.CompanyId).ToList();
                        if (lCompanyEntryConfiguration != null)
                        {
                            context.IdSpaces.Where(p => p.CompanyId == CompanyId).ToList().ForEach(p => context.IdSpaces.Remove(p));
                            context.SaveChanges();
                        }

                        context.SaveChanges();
                        dbContextTransaction.Commit();
                        Deleted = true;
                    }
#pragma warning disable 0168 // variable declared but not used.
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        Deleted = false;
                    }
#pragma warning restore 0168
                }
            }
            return Deleted;
        }
        public IList<CatalogItem> GetAllChildOfSelected(long CategoryId, IList<CatalogItem> CatalogItems, AccountMasterContext Context)
        {

            CatalogItem lCatalogItems = (from CatalogItem in Context.CatalogItems where CatalogItem.Id == CategoryId select CatalogItem).FirstOrDefault();
            if (lCatalogItems != null)
            {
                CatalogItems.Add(lCatalogItems);
                IList<CatalogItem> llCatalogItems = (from CatalogItem in Context.CatalogItems where CatalogItem.ParentId == CategoryId select CatalogItem).ToList();
                if (llCatalogItems != null && llCatalogItems.Count > 0)
                {
                    foreach (CatalogItem CatalogItem in llCatalogItems)
                    {
                        GetAllChildOfSelected(CatalogItem.Id, CatalogItems, Context);
                    }
                }
            }

            return CatalogItems;
        }
        public IList<Company> GetParentCompanies()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Company> CompanyInfo = null;
                try
                {
                    var x = (from company in Context.Companies.Include("CompanyPurchaseSetup").Include("CompanySalesSetup").Include("Address").Include("Country").Include("ContactInfo").Include("TaxInfo").Include("CostCenters").Include("CompanyType").Include("AccountingMethod").Include("PrimaryCurrency")/*.Include("SalesTaxAccountMaps")*/.Include("CashOnHandAccount").Include("UndepositedFundAccount").Include("PurchaseAccount").Include("SalesAccount").Include("AccountRecivable").Include("AccountPayable").Include("IncomceAccount").Include("ExpenseAccount") where company.ParentCompanyId.Equals(null) select company);
                    if (x.Any())
                    {
                        CompanyInfo = x.ToList();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return CompanyInfo;
            }
        }

        public static CompanyFinancialPeriods AccountYearChecking(DateTime Date, long CompanyId)
        {
            CompanyFinancialPeriods CompanyFinancialPeriodsInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CompanyFinancialPeriodsInfo = Context.CompanyFinancialPeriods.FirstOrDefault(x => x.CompanyId == CompanyId && Date >= x.Begin && Date <= x.End);
                return CompanyFinancialPeriodsInfo;
            }
        }

        public static CompanyFinancialPeriods AddAccountingPeriode(CompanyFinancialPeriods companyFinancialPeriods)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Context.CompanyFinancialPeriods.Add(companyFinancialPeriods);
                Context.SaveChanges();
                return companyFinancialPeriods;
            }
        }
        public bool CompanyNameUniqueById(Company Company)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    Company CompanyInfo = null;
                    if (Company.CompanyId == 0)
                    {
                        CompanyInfo = Context.Companies.FirstOrDefault(c => c.Name == Company.Name);
                    }
                    else
                    {
                        CompanyInfo = Context.Companies.FirstOrDefault(c => c.Name == Company.Name && c.CompanyId != Company.CompanyId);
                    }
                    if (CompanyInfo != null)
                    {
                        Status = false;
                    }
                }
#pragma warning disable 0168
                catch (Exception ex)
                {
                }
#pragma warning restore 0168
            }
            return Status;
        }

        public Company CheckCompanyBranch(long CompanyId)
        {
            Company CompanyInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CompanyInfo = Context.Companies.FirstOrDefault(x => x.ParentCompanyId == CompanyId);
            }
            return CompanyInfo;
        }
        public List<Company> GetAccessibleCompanies(User User)
        {
            CompanyManager CompanyManager = new CompanyManager();
            List<Company> Companies = new List<Company>();
            if (User.IsSuperAdmin)
            {
                Companies = (List<Company>)CompanyManager.GetCompanies();
            }
            else if (User.Accesses != null && User.Accesses.Count > 0)
            {
                List<long> Keys = new List<long>();
                foreach (Access access in User.Accesses)
                {
                    if (access.ResourceType.Equals(ResourceType.Company))
                    {
                        Keys.Add(access.ResourceId);
                    }
                    Companies = (List<Company>)CompanyManager.GetCompanies(Keys.ToArray<long>());
                }
            }
            return Companies;
        }
        public List<Company> GetAccessibleParentCompanies(User User)
        {
            CompanyManager CompanyManager = new CompanyManager();
            List<Company> Companies = new List<Company>();
            if (User.IsSuperAdmin)
            {
                Companies = (List<Company>)CompanyManager.GetParentCompanies();
            }
            else if (User.Accesses != null && User.Accesses.Count > 0)
            {
                List<long> Keys = new List<long>();
                foreach (Access access in User.Accesses)
                {
                    if (access.ResourceType.Equals(ResourceType.Company))
                    {
                        Keys.Add(access.ResourceId);
                    }
                    Companies = (List<Company>)CompanyManager.GetRootCompanies(Keys.ToArray<long>());
                }
            }
            return Companies;
        }
        public Boolean AddSalesTaxAccount(Company Company)
        {
            bool SalesTaxAccount = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SalesTaxAccount = true;
                Company CompanyInfo = GetCompany(Company.CompanyId);
                if (CompanyInfo.SalesTaxAccountMaps.Count > 0)
                {
                    CompanySalesTaxAccountMap CompanySalesTaxAccountMap = Context.CompanySalesTaxAccountMaps.FirstOrDefault(x => x.CompanyId == CompanyInfo.CompanyId);
                    if (CompanySalesTaxAccountMap != null)
                    {
                        Context.CompanySalesTaxAccountMaps.Where(p => p.CompanyId == CompanyInfo.CompanyId).ToList().ForEach(p => Context.CompanySalesTaxAccountMaps.Remove(p));
                        Context.SaveChanges();
                    }
                }
                if (Company.SalesTaxAccountMaps.Count > 0)
                {
                    foreach (var SalesTaxAccountMaps in Company.SalesTaxAccountMaps)
                    {
                        Context.CompanySalesTaxAccountMaps.Add(SalesTaxAccountMaps);
                        Context.SaveChanges();
                    }
                }
                return SalesTaxAccount;
            }
        }
        public CompanySalesTaxAccountMap GetCompanySaleTaxMapById(long Id)
        {
            CompanySalesTaxAccountMap CompanySalesTaxAccountMap = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CompanySalesTaxAccountMap = Context.CompanySalesTaxAccountMaps.Include("CountrySaleTax").Where(p => p.MapId == Id).First<CompanySalesTaxAccountMap>();
            }
            return CompanySalesTaxAccountMap;
        }
        public CompanySalesTaxAccountMap GetCompanySaleTaxMapByAccId(long Id)
        {
            CompanySalesTaxAccountMap CompanySalesTaxAccountMap = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CompanySalesTaxAccountMap = Context.CompanySalesTaxAccountMaps.Include("CountrySaleTax").FirstOrDefault(p => p.AccountId == Id);
            }
            return CompanySalesTaxAccountMap;
        }
        public CompanyPurchaseSetup GetPurchaseSetupById(long? Id)
        {
            CompanyPurchaseSetup CompanyPurchaseSetup = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CompanyPurchaseSetup = Context.CompanyPurchaseSetups.Include("AdditionalTransactions").Where(p => p.Id == Id).First<CompanyPurchaseSetup>();
            }
            return CompanyPurchaseSetup;
        }

        public CompanySalesSetup GetCompanySalesSetupWithInclude(Company Company)
        {
            CompanySalesSetup CompanySalesSetup = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CompanySalesSetup = Context.CompanySalesSetups.Include("AdditionalTransactions").Where(p => p.Id == Company.CompanySalesSetupId).First<CompanySalesSetup>();
            }
            return CompanySalesSetup;
        }
        public CompanySalesSetup GetCompanySalesSetup(Company Company)
        {
            CompanySalesSetup CompanySalesSetup = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CompanySalesSetup = Context.CompanySalesSetups.FirstOrDefault(x => x.Id == Company.CompanySalesSetupId);
            }
            return CompanySalesSetup;
        }
        public CustomerLicenceDetail CustomerLicenseInfo(long CustomerLicenseMasterId)
        {
            CustomerLicenceDetail CustomerLicenceDetail = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CustomerLicenceDetail = Context.CustomerLicenceDetails.FirstOrDefault(x => x.CompanyCustomerLicenseMasterId == CustomerLicenseMasterId);
            }
            return CustomerLicenceDetail;
        }
        public SupplierLicenceDetail SupplierLicenseInfo(long SupplierLicenseMasterId)
        {
            SupplierLicenceDetail SupplierLicenceDetail = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SupplierLicenceDetail = Context.SupplierLicenceDetails.FirstOrDefault(x => x.CompanySupplierLicenseMasterId == SupplierLicenseMasterId);
            }
            return SupplierLicenceDetail;
        }
        public static bool[] HasEntry(long CompanyId, DateTime Start, DateTime End)
        {
            bool[] DaybookHasEntry = new bool[Enum.GetValues(typeof(EntryType)).Length];
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<DayBook> DayBook = Context.DoubleEntries.Where(x => x.CompanyId == CompanyId && x.Date >= Start && x.Date <= End).ToList();
                if (DayBook.Count > 0)
                {
                    DaybookHasEntry[(int)EntryType.INVOICE] = DayBook.Where(x => x.TransactionType == DaybookTransactionType.Invoice).ToList().Count > 0 ? true : false;
                    DaybookHasEntry[(int)EntryType.BILL] = DayBook.Where(x => x.TransactionType == DaybookTransactionType.Bill).ToList().Count > 0 ? true : false;
                    DaybookHasEntry[(int)EntryType.RECEIPT] = DaybookHasEntry[(int)EntryType.DEBIT_NOTE] = DayBook.Where(x => x.TransactionType == DaybookTransactionType.Receipt).ToList().Count > 0 ? true : false;
                    DaybookHasEntry[(int)EntryType.PAYMENT] = DaybookHasEntry[(int)EntryType.CREDIT_NOTE] = DayBook.Where(x => x.TransactionType == DaybookTransactionType.Payment).ToList().Count > 0 ? true : false;
                    DaybookHasEntry[(int)EntryType.EXPENSE] = DayBook.Where(x => x.TransactionType == DaybookTransactionType.Expense).ToList().Count > 0 ? true : false;
                    DaybookHasEntry[(int)EntryType.JOURNAL] = DayBook.Where(x => x.TransactionType == DaybookTransactionType.Journal).ToList().Count > 0 ? true : false;
                    DaybookHasEntry[(int)EntryType.SALES] = DaybookHasEntry[(int)EntryType.SALES_QUOTE] = DayBook.Where(x => x.TransactionType == DaybookTransactionType.Sales).ToList().Count > 0 ? true : false;
                    DaybookHasEntry[(int)EntryType.SALES_RETURN] = DayBook.Where(x => x.TransactionType == DaybookTransactionType.SalesReturn).ToList().Count > 0 ? true : false;
                    DaybookHasEntry[(int)EntryType.PURCHASE] = DaybookHasEntry[(int)EntryType.PURCHASE_ORDER] = DayBook.Where(x => x.TransactionType == DaybookTransactionType.Purchase).ToList().Count > 0 ? true : false;
                    DaybookHasEntry[(int)EntryType.PURCHASE_RETURN] = DayBook.Where(x => x.TransactionType == DaybookTransactionType.PurchaseReturn).ToList().Count > 0 ? true : false;
                }
                else
                {
                    bool Status = false;
                    DaybookHasEntry[(int)EntryType.INVOICE] = Status;
                    DaybookHasEntry[(int)EntryType.BILL] = Status;
                    DaybookHasEntry[(int)EntryType.RECEIPT] = DaybookHasEntry[(int)EntryType.DEBIT_NOTE] = Status;
                    DaybookHasEntry[(int)EntryType.PAYMENT] = DaybookHasEntry[(int)EntryType.CREDIT_NOTE] = Status;
                    DaybookHasEntry[(int)EntryType.EXPENSE] = Status;
                    DaybookHasEntry[(int)EntryType.JOURNAL] = Status;
                    DaybookHasEntry[(int)EntryType.SALES] = DaybookHasEntry[(int)EntryType.SALES_QUOTE] = Status;
                    DaybookHasEntry[(int)EntryType.SALES_RETURN] = Status;
                    DaybookHasEntry[(int)EntryType.PURCHASE] = DaybookHasEntry[(int)EntryType.PURCHASE_ORDER] = Status;
                    DaybookHasEntry[(int)EntryType.PURCHASE_RETURN] = Status;
                }
                IList<StockMovement> StockMovement = Context.StockMovement.Where(x => x.CompanyId == CompanyId && x.MovementDate >= Start && x.MovementDate <= End).ToList();
                if (StockMovement.Count > 0)
                {
                    DaybookHasEntry[(int)EntryType.STOCK_IN] = StockMovement.Where(x => x.Type == InventoryJournalType.STOCK_IN).ToList().Count > 0 ? true : false;
                    DaybookHasEntry[(int)EntryType.STOCK_OUT] = StockMovement.Where(x => x.Type == InventoryJournalType.STOCK_OUT).ToList().Count > 0 ? true : false;
                    DaybookHasEntry[(int)EntryType.STOCK_OPENING] = StockMovement.Where(x => x.Type == InventoryJournalType.OPEN_STOCK).ToList().Count > 0 ? true : false;
                    DaybookHasEntry[(int)EntryType.STOCK_PURCHASE] = StockMovement.Where(x => x.Type == InventoryJournalType.PURCHASE).ToList().Count > 0 ? true : false;
                    DaybookHasEntry[(int)EntryType.STOCK_PURCHASERETURN] = StockMovement.Where(x => x.Type == InventoryJournalType.PURCASE_RETURN).ToList().Count > 0 ? true : false;
                    DaybookHasEntry[(int)EntryType.STOCK_SALE] = StockMovement.Where(x => x.Type == InventoryJournalType.SALES).ToList().Count > 0 ? true : false;
                    DaybookHasEntry[(int)EntryType.STOCK_SALERETURN] = StockMovement.Where(x => x.Type == InventoryJournalType.SALES_RETURN).ToList().Count > 0 ? true : false;
                    DaybookHasEntry[(int)EntryType.STOCK_ADJUSTMENT] = StockMovement.Where(x => x.Type == InventoryJournalType.ADJUSTMENT).ToList().Count > 0 ? true : false;
                    DaybookHasEntry[(int)EntryType.STOCK_DAMAGE] = StockMovement.Where(x => x.Type == InventoryJournalType.DAMAGED).ToList().Count > 0 ? true : false;
                    DaybookHasEntry[(int)EntryType.STOCK_TOPATIENT] = StockMovement.Where(x => x.Type == InventoryJournalType.PATIENT_USE).ToList().Count > 0 ? true : false;
                    DaybookHasEntry[(int)EntryType.STOCK_REQUEST] = StockMovement.Where(x => x.Type == InventoryJournalType.STOCK_REQUEST).ToList().Count > 0 ? true : false;
                }
                else
                {
                    bool Status = false;
                    DaybookHasEntry[(int)EntryType.STOCK_IN] = Status;
                    DaybookHasEntry[(int)EntryType.STOCK_OUT] = Status;
                    DaybookHasEntry[(int)EntryType.STOCK_OUT] = Status;
                    DaybookHasEntry[(int)EntryType.STOCK_OPENING] = Status;
                    DaybookHasEntry[(int)EntryType.STOCK_PURCHASE] = Status;
                    DaybookHasEntry[(int)EntryType.STOCK_PURCHASERETURN] = Status;
                    DaybookHasEntry[(int)EntryType.STOCK_SALE] = Status;
                    DaybookHasEntry[(int)EntryType.STOCK_SALERETURN] = Status;
                    DaybookHasEntry[(int)EntryType.STOCK_ADJUSTMENT] = Status;
                    DaybookHasEntry[(int)EntryType.STOCK_DAMAGE] = Status;
                    DaybookHasEntry[(int)EntryType.STOCK_TOPATIENT] = Status;
                    DaybookHasEntry[(int)EntryType.STOCK_REQUEST] = Status;
                }
            }
            return DaybookHasEntry;
        }
    }
}
