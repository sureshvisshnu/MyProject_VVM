using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Employee;
using fa.model.hms.config;
using fa.model.Hms.common;
using fa.model.Hms.Master;
using fa.model.System;

namespace Fa.api.Accounting
{
    class InitialAccount
    {
        public string Name { get; set; }
        public int AccountGroup { get; set; }
        public string Discription { get; set; } 
    }

    public class CompanyInitializer
    {
        InitialAccount[] InitialAccounts = {

            //sale tax
            new InitialAccount() { Name = "Integrated Sales Tax Payable", AccountGroup = 810, Discription = "Sales Tax Payable Account, where all sales tax payables goes into" },
            new InitialAccount() { Name = "Central Sales Tax Payable", AccountGroup = 810, Discription = "Sales Tax Payable Account, where all sales tax payables goes into" },
            new InitialAccount() { Name = "State Sales Tax Payable", AccountGroup = 810, Discription = "Sales Tax Payable Account, where all sales tax payables goes into" },
            new InitialAccount() { Name = "Tax at Source Payable", AccountGroup = 810, Discription = "Sales Tax Payable Account, where all sales tax payables goes into" },
                
            //Cash Accounts
            new InitialAccount() {Name="Cash On Hand",AccountGroup=301,Discription="Cash On Hand Account" },

            //Bank Accounts
            new InitialAccount() {Name = "Undeposited Fund",AccountGroup=307,Discription="Undeposited Fund Account, where all undeposited check goes into" },
            new InitialAccount() {Name="Checking/Current Bank",AccountGroup= 302, Discription="Default Checking/Current Bank Account" },
            new InitialAccount() {Name="Saving Bank",AccountGroup= 305, Discription="Default Saving Bank Account" },
            //Credit Card Accounts
            new InitialAccount() {Name="Bank Account for Credit Cards",AccountGroup=701,Discription="Bank account for Credit Cards" },

            //Income Accounts
            new InitialAccount() {Name = "Sales (Proudct)",AccountGroup=1104,Discription="Income Account for product sales, where all undeposited check goes into" },
            new InitialAccount() {Name = "Sales (Service)", AccountGroup = 1105, Discription = "Income Account for service, where all service related transactions are recorded"},
            new InitialAccount() {Name = "Other Income",AccountGroup = 1103, Discription = "income Account, where all miscelleneous incomes are recorded" },

            //Discount Accounts
            new InitialAccount() {Name = "Sales Discount (Product)",AccountGroup=1104,Discription="Discount Account for product" },
            new InitialAccount() {Name = "Sales Discount (Service)",AccountGroup=1105,Discription="Discount Account for service" },
            new InitialAccount() {Name = "Purchase Discount (Product)",AccountGroup=1104,Discription="Purcahse Discount Account for service" },
            new InitialAccount() {Name = "Purchase Discount (Service)",AccountGroup=1105,Discription="Purcahse Discount Account for service" },

            //Accounts Receivable/Payable
            new InitialAccount() {Name = "Accounts Receivable", AccountGroup = 101, Discription = "Account Receivable Account, where all account receivable goes into"},
            new InitialAccount() {Name = "Accounts Payable",AccountGroup = 601, Discription = "Accounts Payable Account, where all payables goes into"},
            new InitialAccount() {Name = "Payroll/Salary Payable",AccountGroup = 1417, Discription = "Payroll/Salary Expense Payable Account, where all saralries and perks payables goes into"},
            new InitialAccount() {Name = "Interest Payable",AccountGroup = 805, Discription = "Interest Payable Account, where all interest payables goes into"},
            new InitialAccount() {Name = "Accrued expenses payable",AccountGroup = 805, Discription = "Interest Payable Account, where all interest payables goes into"},
            new InitialAccount() {Name = "Unearned Income",AccountGroup = 805, Discription = "Unearned income, which need to be returned"},

            //Inventory Account
            new InitialAccount() {Name = "Inventory", AccountGroup = 204, Discription = "Inventory Account"},

            //Prepayments
            new InitialAccount() {Name = "Prepayments", AccountGroup = 204, Discription = "Prepayment/Prepaid Accounts"},

            //Properties/Plants/Building
            new InitialAccount() {Name = "Properties/Buildings", AccountGroup = 404, Discription = "Properties and Building Accounts"},
            new InitialAccount() {Name = "Plant and Factories", AccountGroup = 404, Discription = "Plants/Factories account"},
            new InitialAccount() {Name = "Equipments and Machinaries", AccountGroup = 404, Discription = "Equipments and Machinaries account"},
            //Depreciation
            new InitialAccount() {Name = "Properties/Bldgs Depriciation", AccountGroup = 1502, Discription = "Depriciation account for Properties and Building"},
            new InitialAccount() {Name = "Plant and Fact Depriciation", AccountGroup = 1502, Discription = "Depriciation account for Plants and Factories "},
            new InitialAccount() {Name = "Equip & Mach Depriciation", AccountGroup = 1502, Discription = "Depriciation account for Equipments and Machinaries"},

            //Bad Debts Account            
            new InitialAccount() {Name = "Bad debts", AccountGroup = 201, Discription = "Allowance for doubtful debts account"},

            

            //Tax Payable Accounts
            new InitialAccount() {Name = "Sales Tax Payable",AccountGroup = 810,Discription = "Sales Tax Payable Account, where all sales tax payables goes into"},
            new InitialAccount() {Name = "Payroll Tax Payable", AccountGroup = 807, Discription = "Payroll Tax Payable Account, where all payroll tax payables goes into" },
            new InitialAccount() {Name = "Purchase Tax Payable",AccountGroup = 805, Discription = "Purchase Tax Payable Account, where all purchase tax payables goes into" },
            new InitialAccount() {Name = "Income Tax Payable (Fed/Cntr)", AccountGroup = 801, Discription = "Fed/Central Income Tax Payable Account, where all Income tax payables goes into"},
            new InitialAccount() {Name = "Income Tax Payable (State)", AccountGroup = 811, Discription = "State Income Tax Payable Account, where all Income tax payables goes into"},

            //Loans
            new InitialAccount() {Name = "Mortgage loan", AccountGroup = 205, Discription = "Investment/Mortgage/Real Estate Loans Account"},
            new InitialAccount() {Name = "Other Loans", AccountGroup = 205, Discription = "Investment/Mortgage/Real Estate Loans Account"},

            //Owner Contributions
            new InitialAccount() {Name = "Owner Contribution", AccountGroup = 1004, Discription = "Owners contribution/equity"},
            new InitialAccount() {Name = "Owner Widthdrawals", AccountGroup = 1004, Discription = "Owners widthdrawal/equity"},

            //Retained Earning            
            new InitialAccount() {Name = "Retained earnings", AccountGroup = 1010, Discription = "Retained earnings accounts"},

            //Cost of Goods
            new InitialAccount() {Name = "Purchase (Cost of Goods)", AccountGroup = 1305, Discription = "Cost of goods/materials/supplier purchased account "},
            new InitialAccount() {Name = "Purchase (Raw Materials)", AccountGroup = 1422, Discription = "Cost of raw goods/materials/supplier purchased account "},
            new InitialAccount() {Name = "Import Duties and Levys", AccountGroup = 1303, Discription = "Import duties added to the cost of raw goods/materials/supplier"},
            new InitialAccount() {Name = "Labor cost (Product)", AccountGroup = 1406, Discription = "Labour charges added to the cost of raw goods/materials/supplier"},
            new InitialAccount() {Name = "Labor cost (Service)", AccountGroup = 1303, Discription = "Labour charges added to the cost of service"},

            //Shipping & Packaging Accounts
            new InitialAccount() {Name = "Packaging & Shipping (Purc)", AccountGroup = 1303, Discription = "Shipping and handling charges for goods purchase account "},
            new InitialAccount() {Name = "Packaging & Shipping (Sales)", AccountGroup = 1301, Discription = "Shipping and handling charges for goods sold account "},

            //Expense Accounts
            new InitialAccount() {Name = "Research and development", AccountGroup = 1428, Discription = "Research and Development expenses"},
            new InitialAccount() {Name = "Sales Commisions", AccountGroup = 1429, Discription = "Sales commisions expenses by marketing team"},
            new InitialAccount() {Name = "Sales Promotions and Ad", AccountGroup = 1401, Discription = "Sales promotions expenses by marketing team"},
            new InitialAccount() {Name = "Gift & Samples", AccountGroup = 1401, Discription = "Sales promotions expenses by marketing team"},
            new InitialAccount() {Name = "Marketing Expenses", AccountGroup = 1401, Discription = "Sales promotions expenses by marketing team"},
            new InitialAccount() {Name = "Payroll/Salary", AccountGroup = 1406, Discription = "Payroll Account"},            
            new InitialAccount() {Name = "Contract Labor (not service)", AccountGroup = 1406, Discription = "Contract Labour Payout"},
            new InitialAccount() {Name = "Payroll Expenses", AccountGroup = 1406, Discription = "Sales promotions expenses by marketing team"},
            new InitialAccount() {Name = "Payroll Fees", AccountGroup = 1414, Discription = "Fees for payroll running"},
            new InitialAccount() {Name = "Payroll Benefits", AccountGroup = 1417, Discription = "Payroll benefir expenses"},
            new InitialAccount() {Name = "Payroll Taxes", AccountGroup = 1423, Discription = "Payroll Taxes paid"},
            new InitialAccount() {Name = "Computer and internet", AccountGroup = 1415, Discription = "Computer and Internet expenses"},
            new InitialAccount() {Name = "Software", AccountGroup = 1415, Discription = "Software purchase and licensing expenses "},
            new InitialAccount() {Name = "Rent and Lease", AccountGroup = 1419, Discription = "Rent and Lease expenses"},
            new InitialAccount() {Name = "Property Taxes", AccountGroup = 1423, Discription = "Property taxes paid expenses"},
            new InitialAccount() {Name = "Utilities", AccountGroup = 1427, Discription = "Utilities expenses"},
            new InitialAccount() {Name = "Travelling", AccountGroup = 1424, Discription = "Traveling expenses"},
            new InitialAccount() {Name = "Travelling Meals", AccountGroup = 1425, Discription = "Traveling Meals expenses"},
            new InitialAccount() {Name = "Entertainment", AccountGroup = 1408, Discription = "Entertainment expenses"},
            new InitialAccount() {Name = "Entertainment Meals", AccountGroup = 1409, Discription = "Entertainment and Meals expenses"},
            new InitialAccount() {Name = "Printing and Stationaries", AccountGroup = 1415, Discription = "Printing and Stationaries expenses"},
            new InitialAccount() {Name = "Postage and Courier", AccountGroup = 1415, Discription = "Postage and Courier expenses"},
            new InitialAccount() {Name = "Telephone", AccountGroup = 1415, Discription = "Telephone expenses"},
            new InitialAccount() {Name = "Office Supplier", AccountGroup = 1415, Discription = "Office supplier expenses"},
            new InitialAccount() {Name = "Professional & Legal Fee", AccountGroup = 1414, Discription = "Professional and Legal expenses"},
            new InitialAccount() {Name = "Equipment Rental", AccountGroup = 1410, Discription = "Equipment rental expenses"},
            new InitialAccount() {Name = "Repairs and Maintenance", AccountGroup = 1420, Discription = "Repairs and Maintenance expenses"},
            new InitialAccount() {Name = "Dues and membership fees", AccountGroup = 1407, Discription = "Dues and membership fees expenses"},
            new InitialAccount() {Name = "Insurance", AccountGroup = 1412, Discription = "Insurance expenses"},
            new InitialAccount() {Name = "Interest Paid", AccountGroup = 1413, Discription = "Interest paid expenses"},
            new InitialAccount() {Name = "Bank Fees", AccountGroup = 1404, Discription = "Bank Fee expenses"},
            new InitialAccount() {Name = "Suspense Account", AccountGroup = 1416, Discription = "Misellenous expenses"},

            //other expense
            new InitialAccount() {Name = "Indirect Expense", AccountGroup = 1503, Discription = "To track gains or losses"}

        };

        public CompanyInitializer()
        {

        }

        private void InitializePaymentTerms(AccountMasterContext Context, Company Company)
        {
            Context.PaymentTerms.Add(new PaymentTerm() {Name = "Due Upon Receipt", FixedDays = true, NoOfDays = 0, CompanyId = Company.CompanyId });
            Context.PaymentTerms.Add(new PaymentTerm() { Name = "Net 15", FixedDays = true, NoOfDays = 15, CompanyId = Company.CompanyId });
            Context.PaymentTerms.Add(new PaymentTerm() { Name = "Net 30", FixedDays = true, NoOfDays = 30, CompanyId = Company.CompanyId });
            Context.SaveChanges();

        }
        private void SetHospitalconfig(AccountMasterContext Context, Company Company)
        {
            HospitalConfiguration HospitalConfiguration = new HospitalConfiguration();
            HospitalConfiguration.CompanyId = Company.CompanyId;
            HospitalConfiguration.DefaultOPConsultingFee = 0.00;
            HospitalConfiguration.DefaultIPConsultingFee = 0.00;           
            Context.HospitalConfigurations.Add(HospitalConfiguration);
            Context.SaveChanges();
        }
        private void SetPatientLedgerTransactionTypeGroup(AccountMasterContext Context, Company Company)
        {
            IList<PatientLedgerTransactionTypeGroupMapping> Mapping = new List<PatientLedgerTransactionTypeGroupMapping>();
            Mapping.Add(new PatientLedgerTransactionTypeGroupMapping() {TransactionType = TransactionType.CONSULTATION_FEE, CompanyId = Company.CompanyId });
            Mapping.Add(new PatientLedgerTransactionTypeGroupMapping() {TransactionType = TransactionType.REGISTRATION_FEE, CompanyId = Company.CompanyId });
            Context.PatientLedgerTransactionTypeGroups.Add(new PatientLedgerTransactionTypeGroup() { Name = "Fees", Description = "", CompanyId = Company.CompanyId, PatientLedgerTransactionTypeGroupMappings= Mapping });
            Context.SaveChanges();

            Mapping = new List<PatientLedgerTransactionTypeGroupMapping>();
            Mapping.Add(new PatientLedgerTransactionTypeGroupMapping() { TransactionType = TransactionType.MEDICALPROCEDURE_FEE, CompanyId = Company.CompanyId });
            Context.PatientLedgerTransactionTypeGroups.Add(new PatientLedgerTransactionTypeGroup() { Name = "Charges", Description = "", CompanyId = Company.CompanyId, PatientLedgerTransactionTypeGroupMappings = Mapping });
            Context.SaveChanges();


            Mapping = new List<PatientLedgerTransactionTypeGroupMapping>();
            Mapping.Add(new PatientLedgerTransactionTypeGroupMapping() { TransactionType = TransactionType.PHARMACY_FEE, CompanyId = Company.CompanyId });
            Context.PatientLedgerTransactionTypeGroups.Add(new PatientLedgerTransactionTypeGroup() {  Name = "Pharmacy charges", Description = "", CompanyId = Company.CompanyId, PatientLedgerTransactionTypeGroupMappings = Mapping });
            Context.SaveChanges();

            Mapping = new List<PatientLedgerTransactionTypeGroupMapping>();
            Mapping.Add(new PatientLedgerTransactionTypeGroupMapping() {  TransactionType = TransactionType.LAB_FEE, CompanyId = Company.CompanyId });
            Context.PatientLedgerTransactionTypeGroups.Add(new PatientLedgerTransactionTypeGroup() { Name = "Labtest Charges", Description = "", CompanyId = Company.CompanyId, PatientLedgerTransactionTypeGroupMappings = Mapping });
            Context.SaveChanges();

            Mapping = new List<PatientLedgerTransactionTypeGroupMapping>();
            Mapping.Add(new PatientLedgerTransactionTypeGroupMapping() { TransactionType = TransactionType.ROOM_CLEANING_CHARGE, CompanyId = Company.CompanyId });
            Mapping.Add(new PatientLedgerTransactionTypeGroupMapping() { TransactionType = TransactionType.ROOM_RENT, CompanyId = Company.CompanyId });
            Context.PatientLedgerTransactionTypeGroups.Add(new PatientLedgerTransactionTypeGroup() {  Name = "Room Charge", Description = "", CompanyId = Company.CompanyId, PatientLedgerTransactionTypeGroupMappings = Mapping });
            Context.SaveChanges();

            Mapping = new List<PatientLedgerTransactionTypeGroupMapping>();
            Mapping.Add(new PatientLedgerTransactionTypeGroupMapping() {TransactionType = TransactionType.MISCELLENEOUS, CompanyId = Company.CompanyId });
            Context.PatientLedgerTransactionTypeGroups.Add(new PatientLedgerTransactionTypeGroup() {  Name = "Other Charges", Description = "", CompanyId = Company.CompanyId, PatientLedgerTransactionTypeGroupMappings = Mapping });
            Context.SaveChanges();
            
        }
        
        private void CreateCompanyAccounts(AccountMasterContext Context, Company Company)
        {
            var AccountDictionary = new Dictionary<String, long>();

            foreach(InitialAccount initialAccount in InitialAccounts )
            {
                var account = new Account
                {
                    Name = initialAccount.Name,
                    CompanyId = Company.CompanyId,
                    Discription = initialAccount.Discription,
                    BalanceAsOf = DateTime.Today,
                    AccountGroupId = initialAccount.AccountGroup,
                    AccountType = AccountType.ACCOUNT
                };

                Context.Accounts.Add(account);
                Context.SaveChanges();

                if (initialAccount.Name == "Cash On Hand")
                {
                    Company.CashOnHandAccountId = account.Id;
                }
                else if (initialAccount.Name == "Undeposited Fund")
                {
                    Company.UndepositedFundAccountId = account.Id;
                }
                else if (initialAccount.Name == "Sales (Proudct)")
                {
                    Company.SalesAccountId = account.Id;
                    Company.SalesReturnFeeAccountId = account.Id;
                }
                else if (initialAccount.Name == "Purchase (Cost of Goods)")
                {
                    Company.PurchaseAccountId = account.Id;
                }

                else if (initialAccount.Name == "Accounts Receivable")
                {
                    Company.AccountRecivableId = account.Id;
                }
                else if (initialAccount.Name == "Accounts Payable")
                {
                    Company.AccountPayableId = account.Id;
                }
                else if (initialAccount.Name == "Sales Tax Payable")
                {
                    Company.IncomceAccountId = account.Id;
                }
                else if (initialAccount.Name == "Indirect Expense")
                {
                    Company.RoundOffAccountId = account.Id;
                }
                Context.SaveChanges();
            }
           
        }

        public void InitializeCompany(long lCompanyId, AccountMasterContext context)
        {
                Company lCompany = null;
                lCompany = context.Companies.FirstOrDefault(x => x.CompanyId == lCompanyId);
                if (lCompany != null)
                {
                    CreateCompanyAccounts(context, lCompany);
                    InitializePaymentTerms(context, lCompany);

                    if (lCompany.BusinessType == BuisnessType.Hospital)
                    {
                        SetPatientLedgerTransactionTypeGroup(context, lCompany);
                        SetHospitalconfig(context, lCompany);
                    }

                    context.Titles.Add(new Title() { Name = "Doctor", DisplayAs = "Doctor", Discription = "", CompanyId = lCompanyId });
                    context.Titles.Add(new Title() {  Name = "Nurse", DisplayAs = "Nurse", Discription = "", CompanyId = lCompanyId });
                    context.Titles.Add(new Title() {  Name = "Technician", DisplayAs = "Technician", Discription = "", CompanyId = lCompanyId });
                    context.SaveChanges();

                    context.DocumentCategories.Add(new DocumentCategory() { Name = "Patient Documents", DisplayAs = "Patient Documents", Discription = "", CompanyId = lCompanyId });
                    context.DocumentCategories.Add(new DocumentCategory() { Name = "Medical Documents", DisplayAs = "Medical Documents", Discription = "", CompanyId = lCompanyId });
                    context.DocumentCategories.Add(new DocumentCategory() { Name = "Medical Test Results", DisplayAs = "Medical Test Results", Discription = "", CompanyId = lCompanyId });
                    context.DocumentCategories.Add(new DocumentCategory() { Name = "Insurance Documents", DisplayAs = "Insurance Documents", Discription = "", CompanyId = lCompanyId });
                    context.DocumentCategories.Add(new DocumentCategory() { Name = "Uncategorized", DisplayAs = "Uncategorized", Discription = "", CompanyId = lCompanyId });
                    context.SaveChanges();
                    
            }
        }
        public Account InitializePaymentMethods(long lCompanyId, long lAccountId)
        {
            Account lAccount = null;
            using (AccountMasterContext context = new AccountMasterContext())
            {
                lAccount = context.Accounts.FirstOrDefault(x => x.Id == lAccountId && x.CompanyId == lCompanyId);
                if (lAccount != null)
                {
                    context.PaymentMethods.Add(
                    new PaymentMethod()
                    {
                        Name = "Cash",
                        DisplayAs = "Cash",
                        CreditCard = false,
                        CompanyId = lCompanyId,
                        AccountId = lAccountId
                    });
                    context.SaveChanges();

                    context.PaymentMethods.Add(
                    new PaymentMethod()
                    {
                        Name = "Invoice",
                        DisplayAs = "Invoice",
                        CreditCard = false,
                        CompanyId = lCompanyId,
                        AccountId = lAccountId
                    });
                    context.SaveChanges();
                }
            }
            return lAccount;
        }
    }
}
