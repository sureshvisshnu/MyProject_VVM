using fa;
using fa.Data;
using fa.model;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.model.Accounting.Transactions;
using fa.model.catalog;
using fa.model.Catalog;
using fa.model.Common;
using fa.model.Employee;
using fa.model.hms.common;
using fa.model.hms.config;
using fa.model.Hms.common;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using fa.model.OrderManagement;
using fa.model.System;
using fa.model.UserProfile;
using Fa.api.Accounting;
using FADataAccessLibrary.Configuration;
using FADataAccessLibrary.Model.Catalog;
using FADataAccessLibrary.Model.Common;
using FADataAccessLibrary.Model.Hms.common;
using FADataAccessLibrary.Model.Hms.Master;
using FADataAccessLibrary.Model.Purchase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MySqlConnector;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Security.Principal;

namespace fa.context
{
    [EntityTypeConfiguration(typeof(MySqlEntityTypeExtensions))]
    public class AccountMasterContext : DbContext
    {
        
        private static volatile AccountMasterContext instance;
        private static object syncRoot = new Object();        
        public static AccountMasterContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AccountMasterContext();

                    }
                }
                return instance;  
            }
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //GetDatabaseConnection(optionsBuilder);
        //    string ipAddress = Global.IpAddressDefault;
        //    string connectionString;
        //    connectionString = $"server={ipAddress};port=3306;database=vv-matrix;user=admin;password=adminpass";
        //    var builder = new MySqlConnectionStringBuilder(connectionString)
        //    {
        //        Pooling = true,
        //        MinimumPoolSize = 5,
        //        MaximumPoolSize = 20,
        //        ConnectionTimeout = 30
        //    };

        //    optionsBuilder.UseMySql(builder.ConnectionString, new MySqlServerVersion(new Version(8, 0, 27)))
        //                      .EnableSensitiveDataLogging()
        //                      .EnableDetailedErrors();
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string systemName = Global.DefaultHostName;
            string connectionString = $"server={systemName};port=3306;database=vv-matrix;user=admin;password=adminpass";

            var builder = new MySqlConnectionStringBuilder(connectionString)
            {
                Pooling = true,
                MinimumPoolSize = 5,
                MaximumPoolSize = 20,
                ConnectionTimeout = 30
            };

            optionsBuilder.UseMySql(builder.ConnectionString, new MySqlServerVersion(new Version(8, 0, 27)))
                          .EnableSensitiveDataLogging()
                          .EnableDetailedErrors();
        }

        private bool IsInNetwork(string ipAddress, string networkAddress)
        {
            string[] networkParts = networkAddress.Split('/');
            int subnetMaskLength = int.Parse(networkParts[1]);

            List<string> LocalipAddresses = GetIPAddresses();            
            List<string> ipBases = GetIpAddressesFromArp();
            ipBases.AddRange(LocalipAddresses);

            return AreInSameNetwork(ipBases, ipAddress, subnetMaskLength);
        }
        private bool AreInSameNetwork(List<string> ipBases, string ipAddress, int subnetMaskLength)
        {
            var ipAddress2 = IPAddress.Parse(ipAddress);
            
            foreach (string ipBase in ipBases)
            {
                var ipAddress1 = IPAddress.Parse(ipBase);

                if (ipAddress2.Equals(ipAddress1))
                {
                    return true;
                }
            }
            return false;
        }        
        static List<string> GetIpAddressesFromArp()
        {
            List<string> ipAddresses = new List<string>();

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "arp",
                Arguments = "-a",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(psi))
            {
                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();

                    string[] lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 2)
                        {
                            string ipAddress = parts[0];
                            if (System.Net.IPAddress.TryParse(ipAddress, out _))
                            {
                                ipAddresses.Add(ipAddress);
                            }
                        }
                    }
                }
            }
            return ipAddresses;
        }
        public static List<string> GetIPAddresses()
        {
            List<string> ipAddresses = new List<string>();
            string hostName = Dns.GetHostName();
            IPAddress[] localIPs = Dns.GetHostAddresses(hostName);
            foreach (IPAddress addr in localIPs)
            {
                // Only add IPv4 addresses
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddresses.Add(addr.ToString());
                }
            }
            return ipAddresses;            
        }
        public void GetDatabaseConnection(DbContextOptionsBuilder optionsBuilder)
        {            
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString;

                bool isInNetwork = IsInNetwork("192.168.100.100", "192.168.100.0/24");

                if (isInNetwork)
                {
                    connectionString = "server=192.168.100.100;port=3306;database=vv-matrix;user=admin;password=adminpass";
                }
                else
                {
                    connectionString = "server=localhost;port=3306;database=vv-matrix;user=admin;password=adminpass";
                }

                var builder = new MySqlConnectionStringBuilder(connectionString)
                {
                    Pooling = true,
                    MinimumPoolSize = 5,
                    MaximumPoolSize = 20,
                    ConnectionTimeout = 30
                };

                optionsBuilder.UseMySql(builder.ConnectionString, new MySqlServerVersion(new Version(8, 0, 27)))
                              .EnableSensitiveDataLogging()
                              .EnableDetailedErrors();

                //optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 27)));
            }
        }
        public DbSet<AdditionalDetail> AdditionalDetails { get; set; }
        public DbSet<Refered> Refereds { get; set; }
        public DbSet<CompanySalesSetup> CompanySalesSetups { get; set; }
        public DbSet<CompanyPurchaseSetup> CompanyPurchaseSetups { get; set; }
        public DbSet<CompanyStockMovementSetup> CompanyStockMovementSetups { get; set; }
        public DbSet<CompanyAdditionalTransactionSetup> CompanyAdditionalTransactionSetups { get; set; }
        public DbSet<CompanySalesTaxAccountMap> CompanySalesTaxAccountMaps { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CostCenter> CostCenters { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<TaxDocumentType> TaxTypes { get; set; }
        public DbSet<CompanyType> CompanyTypes { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<AccountGroup> AccountGroups { get; set; }
        public DbSet<AccountGroupForHelp> AccountGroupForHelp { get; set; }
        public DbSet<AccountGroupClassification> AccountGroupClassification { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PaymentTerm> PaymentTerms { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<TaxInfo> TaxInfos { get; set; }
        public DbSet<Access> Accesses { get; set; }
        public DbSet<AccountingMethod> AccountingMethods { get; set; }
        public DbSet<CompanyFinancialPeriods> CompanyFinancialPeriods { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptDetail> ReceiptDetails { get; set; }
        public DbSet<CreditCardReceipt> CreditCardReceipts { get; set; }
        public DbSet<CheckReceipt> CheckReceipts { get; set; }
        public DbSet<BankTransferReceipt> BankTransferReceipts { get; set; }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; }

        public DbSet<CreditCardPayment> CreditCardPayments { get; set; }
        public DbSet<CheckPayment> CheckPayments { get; set; }
        public DbSet<UpiTransactionPayment> UpiTransactionPayments { get; set; }
        public DbSet<BankTransferPayment> BankTransferPayments { get; set; }
        public DbSet<CreditNote> CreditNotes { get; set; }
        public DbSet<CreditNoteDetail> CreditNoteDetails { get; set; }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseDetail> ExpenseDetails { get; set; }

        public DbSet<CreditCardExpense> CreditCardExpenses { get; set; }
        public DbSet<CashExpense> CashExpenses { get; set; }
        public DbSet<CheckExpense> CheckExpenses { get; set; }
        public DbSet<BankTransferExpense> BankTransferExpenses { get; set; }


        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<InvoiceAdditionalTransaction> InvoiceAdditionalTransactions { get; set; }

        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillAttachment> BillAttachments { get; set; }
        public DbSet<BillDetail> BillDetails { get; set; }

        public DbSet<DebitNote> DebitNotes { get; set; }
        public DbSet<DebitNoteDetail> DebitNoteDetails { get; set; }

        public DbSet<Journal> Journals { get; set; }
        public DbSet<JournalDetail> JournalDetails { get; set; }        
        public DbSet<IdSpace> IdSpaces { get; set; }
        public DbSet<IdSpaceEntryTypeDetail> IdSpaceEntryTypeDetails { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Title> Titles { get; set; }

        //Licence
        public DbSet<LicenseInfo> LicenseInfos { get; set; }
        public DbSet<CompanyLicence> CompanyLicences { get; set; }
        public DbSet<CompanySupplierLicenseMaster> CompanySupplierLicenseMasters { get; set; }
        public DbSet<CompanyCustomerLicenseMaster> CompanyCustomerLicenseMasters { get; set; }

        public DbSet<CustomerLicenceDetail> CustomerLicenceDetails { get; set; }
        public DbSet<SupplierLicenceDetail> SupplierLicenceDetails { get; set; }

        public DbSet<Narration> Narrations { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SystemFunction> SystemFunctions { get; set; }


        // Catalog
        public DbSet<CatalogItem> CatalogItems { get; set; } 
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductFamily> ProductFamilies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CatalogItemSalesTaxMap> CatalogItemSalesTaxMaps { get; set; }
        public DbSet<DayBook> DoubleEntries { get; set; }
        public DbSet<ItemTax> ItemTaxs { get; set; }
        public DbSet<ItemSalesTaxMap> ItemSalesTaxMaps { get; set; }
        public DbSet<ProductPercentage> ProductPercentages { get; set; }
        public DbSet<PercentageStorage> PercentageStorages { get; set; }
        public DbSet<SupplierProduct> SupplierProducts { get; set; }

        // BarCode Label Counter
        public DbSet<LabelStockMaster> LabelStockMasters { get; set; }
        public DbSet<LabelStockUsage> LabelStockUsages { get; set; }
        public DbSet<RibbonUsage> RibbonUsages { get; set; }

        //Purcahse
        public DbSet<PurchaseEntry> PurchaseEntry { get; set; }
        public DbSet<PurchaseAttachment> PurchaseAttachments { get; set; }
        public DbSet<PurchaseAdditionalTransaction> PurchaseAdditionalTransactionses { get; set; }
        public DbSet<PurchaseDetails> PurchaseDetails { get; set; }


        //Sales
        public DbSet<SaleEntry> SaleEntry { get; set; }
        public DbSet<SaleAdditionalTransaction> SaleAdditionalTransactions { get; set; }
        public DbSet<SaleDetail> SaleDetail { get; set; }

        public DbSet<AdditionalTransaction> AdditionalTransactions { get; set; }

        public DbSet<TaxDetail> TaxDetails { get; set; }
        public DbSet<OrderLevelSaleTaxDetail> OrderLevelSaleTaxDetails { get; set; }
        public DbSet<ItemLevelSaleTaxDetail> ItemLevelSaleTaxDetails { get; set; }
        public DbSet<OrderLevelPurchaseTaxDetail> OrderLevelPurchaseTaxDetails { get; set; }
        public DbSet<LineLevelPurchaseTaxDetail> LineLevelPurchaseTaxDetails { get; set; }


        public DbSet<DiscountDetail> DiscountDetails { get; set; }
        public DbSet<OrderLevelSaleDiscount> OrderLevelSaleDiscounts { get; set; }
        public DbSet<ItemLevelSaleDiscount> ItemLevelSaleDiscounts { get; set; }
        public DbSet<OrderLevelPurchaseDiscount> OrderLevelPurchaseDiscounts { get; set; }
        public DbSet<LineLevelPurchaseDiscount> LineLevelPurchaseDiscounts { get; set; }

        //Inventory
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryBatch> InventoryBatches { get; set; }

        //Keyword
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<SymptomKeyword> SymptomsKeywords { get; set; }
        public DbSet<AllergieKeyword> AllergieKeywords { get; set; }
        public DbSet<MedicalTestKeyword> MedicalTestKeywords { get; set; }
        public DbSet<MedicalProcedureKeyword> MedicalProcedureKeywords { get; set; }


        //HMS
        public DbSet<PatientHistoryQuestion> PatientHistoryQuestions { get; set; }
        public DbSet<PatientHistoryQuestionGroup> PatientHistoryQuestionGroups { get; set; }
        public DbSet<Allergie> Allergies { get; set; }
        public DbSet<Symptom> Symptoms { get; set; }
        public DbSet<SymptomCategory> SymptomCategorys { get; set; }
        public DbSet<AllergieCategory> AllergieCategorys { get; set; }
        public DbSet<PatientReportedSymptom> PatientReportedSymptoms { get; set; }
        public DbSet<TestObsorbedSymptom> TestObsorbedSymptoms { get; set; }
        public DbSet<PatientReportedAllergie> PatientReportedAllergies { get; set; }
        public DbSet<TestObsorbedAllergie> TestObsorbedAllergies { get; set; }
        public DbSet<MedicalTest> MedicalTests { get; set; }
        public DbSet<MedicalTestCategory>MedicalTestCategorys { get; set; }
        public DbSet<MedicalTestElement> MedicalTestElements { get; set; }
        public DbSet<MedicalTestResult> MedicalTestResults { get; set; }
        public DbSet<MedicalTestUOM> MedicalTestUOMs { get; set; }
        public DbSet<BedType> BedTypes { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Vital> Vitals { get; set; }
        public DbSet<Guardian> Guardians { get; set; }
        public DbSet<InsuranceInfo> InsuranceInfos { get; set; }
        public DbSet<EmergencyContact> EmergencyContacts { get; set; }
        public DbSet<PatientPreMedicalHistory> PatientPreMedicalHistories { get; set; }
        public DbSet<Registration> Registrationes { get; set; }
        public DbSet<InPatientAdmission> InPatientAdmissions { get; set; }
        public DbSet<InPatientLocation> InPatientLocations { get; set; }
        public DbSet<MedicalTeam> MedicalTeams { get; set; }

        public DbSet<DischargeNote> DischargeNotes { get; set; }
        public DbSet<DischargePrescription> DischargePrescriptions { get; set; }

        public DbSet<Token> Tokens { get; set; }
        public DbSet<PatientId> PatientIds { get; set; }
        public DbSet<ConsultationNote> ConsultationNotes { get; set; }
        public DbSet<ConsultedConsultationFee> ConsultedConsultationFees { get; set; }
        public DbSet<ConsultedLabTest> ConsultedLabTests { get; set; }
        public DbSet<ConsultedPrescription> ConsultedPrescriptions { get; set; }
        public DbSet<ConsultedProcedure> ConsultedProcedures { get; set; }
        public DbSet<ConsultedSymptom> ConsultedSymptoms { get; set; }
        public DbSet<ConsultedAllergie> ConsultedAllergies { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<ConsultedDoctorConsultationFee> ConsultedDoctorConsultationFees { get; set; }
        public DbSet<ConsultedLabTestElements> ConsultedLabTestElements { get; set; }
        public DbSet<LabTestAttachment> LabTestAttachments { get; set; }
        public DbSet<PatientLedger> PatientLedgers { get; set; }
        public DbSet<SeedDataHistory> SeedDataHistories { get; set; }
        public DbSet<InventoryLocation> InventoryLocation { get; set; }
        public DbSet<StockMovement> StockMovement { get; set; }
        public DbSet<StockMovementDetail> StockMovementDetail { get; set; }
        public DbSet<StockMovementIn> StockMovementIn { get; set; }
        public DbSet<StockMovementOut> StockMovementOut { get; set; }
        public DbSet<StockMovementRequest> StockMovementRequest { get; set; }
        public DbSet<StockMovementPatientUse> StockMovementPatientUse { get; set; }
        public DbSet<StockMovementOpeningStock> StockMovementOpeningStock { get; set; }
        public DbSet<StockMovementDamaged> StockMovementDamaged { get; set; }
        public DbSet<StockMovementPurchase> StockMovementPurchase { get; set; }
        public DbSet<StockMovementSales> StockMovementSales { get; set; }
        public DbSet<StockMovementPurchaseReturn> StockMovementPurchaseReturn { get; set; }
        public DbSet<StockMovementSalesReturn> StockMovementSalesReturn { get; set; }
        public DbSet<StockMovementAdjustment> StockMovementAdjustment { get; set; }
        public DbSet<DocumentCategory> DocumentCategories { get; set; }
        public DbSet<PatientDocument> PatientDocuments { get; set; }
        public DbSet<HospitalConfiguration> HospitalConfigurations { get; set; }
        public DbSet<PrintPaperFormat> PrintPaperFormats { get; set; }
        public DbSet<MedicalProcedure> MedicalProcedures { get; set; }
        public DbSet<MedicalProcedureCategory> MedicalProcedureCategory { get; set; }
        public DbSet<MedicalProcedureElement> MedicalProcedureElements { get; set; }
        public DbSet<ConsultedProcedureHistory> ConsultedProcedureHistorys { get; set; }
        public DbSet<PatientInvoice> PatientInvoices { get; set; }
        public DbSet<PatientInvoicePayment> PatientInvoicePayments { get; set; }
        public DbSet<PatientLedgerTransactionTypeGroup> PatientLedgerTransactionTypeGroups { get; set; }
        public DbSet<PatientLedgerTransactionTypeGroupMapping> PatientLedgerTransactionTypeGroupMappings { get; set; }
        public DbSet<PatientPaymentDetail> PatientPaymentDetails { get; set; }
        public DbSet<CountrySaleTax> CountrySaleTaxs { get; set; }
        //public DbSet<RoleSystemFunction> RoleSystemFunctions { get; set; }
        public DbSet<PatientOPId> PatientOPIds { get; set; }
        public DbSet<PatientIPId> PatientIPIds { get; set; }
        public DbSet<ConsultationDetail> ConsultationDetails { get; set; }
        public DbSet<PatientRoomRent> PatientRoomRents { get; set; }
        public DbSet<PatientAppointment> PatientAppointments { get; set; }

        // Invoice Mapping
        public DbSet<InvoiceMappingTemplate> InvoiceMappingTemplates { get; set; }
        public DbSet<ProductMappingTemplate> ProductMappingTemplates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientOPId>().HasKey(p => new { p.CompanyId, p.Date });
            modelBuilder.Entity<PatientIPId>().HasKey(p => new { p.CompanyId, p.Date });
            modelBuilder.Entity<PatientId>().HasKey(p => new { p.CompanyId, p.Date });
            modelBuilder.Entity<Token>().HasKey(t => new { t.CompanyId, t.Date });
            modelBuilder.Entity<Account>().HasOne(acc => acc.Company).WithMany().HasForeignKey(acc => acc.CompanyId);

            modelBuilder.Entity<AccountGroupClassification>().ToTable("accountgroupclassifications");
            modelBuilder.Entity<ConsultedProcedureHistory>().ToTable("consultedprocedurehistories");
            modelBuilder.Entity<ContactInfo>().ToTable("contactinfoes");
            modelBuilder.Entity<DayBook>().ToTable("daybooks");
            modelBuilder.Entity<Employee>().ToTable("employees");
            modelBuilder.Entity<InsuranceInfo>().ToTable("insuranceinfoes");
            modelBuilder.Entity<InventoryLocation>().ToTable("inventorylocations");
            modelBuilder.Entity<ItemTax>().ToTable("itemtaxes");
            modelBuilder.Entity<LicenseInfo>().ToTable("licenseinfoes");
            modelBuilder.Entity<PurchaseEntry>().ToTable("purchaseentries");
            modelBuilder.Entity<Registration>().ToTable("registrations");
            modelBuilder.Entity<SaleDetail>().ToTable("saledetails");
            modelBuilder.Entity<SaleEntry>().ToTable("saleentries");
            modelBuilder.Entity<StockMovement>().ToTable("stockmovements");
            modelBuilder.Entity<StockMovementDetail>().ToTable("stockmovementdetails");
            modelBuilder.Entity<TaxDocumentType>().ToTable("taxdocumenttypes");
            modelBuilder.Entity<TaxInfo>().ToTable("taxinfoes");

            modelBuilder.Entity<CompanyPurchaseSetup>()
                .HasMany<CompanyAdditionalTransactionSetup>(ps => ps.AdditionalTransactions)
                .WithMany(ts => ts.CompanyPurchaseSetup)
                .UsingEntity(at =>
                {
                    at.Property("AdditionalTransactionsId").HasColumnName("AddTransactionSetupId");
                    at.Property("CompanyPurchaseSetupId").HasColumnName("PurchaseSetupId");
                    at.ToTable("purchaseadditionaltransaction");
                });
            modelBuilder.Entity<CompanySalesSetup>()
                .HasMany<CompanyAdditionalTransactionSetup>(ss => ss.AdditionalTransactions)
                .WithMany(ts => ts.CompanySalesSetup)
                .UsingEntity(at =>
                {
                    at.Property("AdditionalTransactionsId").HasColumnName("AddTransactionSetupId");
                    at.Property("CompanySalesSetupId").HasColumnName("SalesSetupId");
                    at.ToTable("salesadditionaltransaction");
                });
            modelBuilder.Entity<Country>()
                .HasMany<TaxDocumentType>(t => t.TaxType)
                .WithMany(c => c.Country)
                .UsingEntity(ct =>
                {
                    ct.Property("CountryId").HasColumnName("Id");
                    ct.ToTable("countrytax");
                });

            modelBuilder.Entity<Role>()
                        .HasMany<SystemFunction>(s => s.SystemFunctions
                        )
                        .WithMany(r => r.Roles)
                        .UsingEntity<Dictionary<string, object>>(
                    "rolesystemfunction",
                    rsf => rsf.HasOne<SystemFunction>().WithMany().HasForeignKey("FunctionId"),
                    rsf => rsf.HasOne<Role>().WithMany().HasForeignKey("RoleId")
                );


            modelBuilder.ApplyConfiguration(new CurrencyConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AccountingMethodConfiguration());
            modelBuilder.ApplyConfiguration(new TaxDocumentTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new StateConfiguration());
            modelBuilder.ApplyConfiguration(new AccountGroupClassificationConfiguration());
            modelBuilder.ApplyConfiguration(new AccountGroupConfiguration());
            modelBuilder.ApplyConfiguration(new AccountGroupForHelpConfiguration());
            //modelBuilder.ApplyConfiguration(new SystemFunctionConfiguration());
            //modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new PatientHistoryQuestionGroupConfiguration());
            modelBuilder.ApplyConfiguration(new PatientHistoryQuestionConfiguration());
            modelBuilder.ApplyConfiguration(new PrintPaperFormatConfiguration());
            modelBuilder.ApplyConfiguration(new IdSpaceEntryTypeDetailConfiguration());
            modelBuilder.ApplyConfiguration(new CountryTaxConfiguration());
        }

        /* Overriding the SaveChanges to update CreatedBy/Date and ModifiedBy/Date automatically, Now all Entities are auditable.
         * These creaed and modified fields are not referenced to the user but it will be updating using the current user. If there is no user set then
         * "System" will be used to represent the system generated values.
         * 
         * We should modify this method if we need to override any system mainained Entites link DBook.
         */
        public override int SaveChanges()
        {
            DateTime currentDateTime = DateTime.Now;
            var userName = "System";
            if (fa.Data.Global.User != null)
            {
                userName = fa.Data.Global.User.FirstName + "[id:" + fa.Data.Global.User.UserId + "]";
            }

            foreach (var auditableEntity in ChangeTracker.Entries<IAuditableEntity>())
            {
                if (auditableEntity.State == EntityState.Added || auditableEntity.State == EntityState.Modified)
                {                    
                    if(isSystemUpdatedEntity(auditableEntity.Entity))
                    {
                        userName = "System"; 
                    }
                    auditableEntity.Entity.LastModifiedDate = currentDateTime;
                    auditableEntity.Entity.LastModifiedBy = userName;
                    if (auditableEntity.State == EntityState.Added)
                    {
                        auditableEntity.Entity.CreatedDate = currentDateTime;
                        auditableEntity.Entity.CreatedBy = userName;
                   
                    }
                    else 
                    if(auditableEntity.State == EntityState.Modified)
                    {
                        auditableEntity.Property(x => x.CreatedBy).IsModified = false;
                        auditableEntity.Property(x => x.CreatedDate).IsModified = false;
                    }
                   
                }
            }
                return base.SaveChanges();
        }
        
        private bool isSystemUpdatedEntity(Object Entity)
        {
            if (Entity is DayBook)
            {
                return true;
            }
            return false;
        }
    }
}
