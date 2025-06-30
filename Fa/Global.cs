using fa.api.Accounting;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.Catalog;
using fa.model.Hms.Master;
using fa.model.UserProfile;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using FADataAccessLibrary.Model.Hms.Master;

namespace fa
{
    public static class Global
    {
        public static RegistryKey WorkStation = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Ab2App")!;
        public static bool isAuthenticated = false;
        public static bool isDateModification = false;
        public static User User = null!;
        public static String providerName = "VV Matrix, Inc.";
        public static String ApplicationName = "Matrix v1.0";
        public static Company Company = null!;
        public static CostCenter CostCenter = null!;
        private static DateTime TransactionDate;
        public static bool isLocalDB = false;
        public static bool isRemoteDB = false;

        public static CompanyFinancialPeriods CompanyFinancialPeriods = null!;
        public static long?[] ExcludedAccountInReceipt = new long?[] { 301, 302, 303, 304, 305, 306 };
        public static long?[] BankAccountInReceipt = new long?[] { 302, 303, 304, 305, 306, 307 };
        public static long?[] IncludedAccountCreditNoteService = new long?[] { 1101, 1102, 1103, 1104, 1105, 1106, 1201, 1202, 1203, 1204, 1205, 1301, 1302, 1303, 1304, 1305 };
        public static long?[] IncludedAccountDebitNoteService = new long?[] { 1401, 1402, 1403, 1404, 1405, 1406, 1407, 1408, 1409, 1410, 1411, 1412, 1413, 1414, 1415, 1416, 1417, 1418, 1419, 1420, 1421, 1422, 1423, 1424, 1425, 1426, 1427 };
        public static long?[] IncludedAccountInvoiceService = new long?[] { 1101, 1102, 1103, 1104, 1105, 1106 };
        public static long?[] IncludedAccountBillService = new long?[] { };
        public static long?[] IncludedAccountCompanySalesTaxReceivable = new long?[] { 801, 802, 803, 804, 805, 806, 807, 808, 809, 810, 811, 812, 813 };

        public static byte[] getLogoAsBytes()
        {
            byte[] imageBytes = null!;
            imageBytes = (byte[])Company.Logo;

            return imageBytes;
        }
        public static void setTransactionDate(DateTime dt)
        {
            TransactionDate = dt;
            fa.api.Global.TransactionDate = TransactionDate;
            fa.Data.Global.TransactionDate = TransactionDate;
            if (Company != null)
            {
                fa.api.Global.CompanyFinancialPeriodBegin = getCurrentFiscalYearStartDate();
                fa.api.Global.CompanyFinancialPeriodEnd = getCurrentFiscalYearEndDate();
            }
        }
        public static DateTime getTransactionDate()
        {
            return TransactionDate;
        }
        public static string getWorkStationId()
        {
            if (WorkStation != null)
            {
                return WorkStation?.GetValue("WorkStationID") != null ? WorkStation.GetValue("WorkStationID")?.ToString()! : null!;
            }
            return null!;
        }
        public static string getWorkStationName()
        {
            if (WorkStation != null)
            {
                return WorkStation?.GetValue("WorkStationName") != null ? WorkStation.GetValue("WorkStationName")?.ToString()! : null!;
            }
            return null!;
        }
        public static string getDefaultPrinter()
        {
            if (WorkStation != null)
            {
                return WorkStation?.GetValue("DefaultPrinter") != null ? WorkStation.GetValue("DefaultPrinter")?.ToString()! : null!;
            }
            return null!;
        }
        static IList<Consultation> lConsultationDetailList = new List<Consultation>();
        public static IList<Consultation> ConsultationDetailList
        {
            get
            {
                return lConsultationDetailList.Where(x => x.CompanyId == Global.Company.CompanyId).ToList();
            }
            set
            {
                lConsultationDetailList = value;
            }
        }
        static IList<Symptom> lSymptomDetailList = new List<Symptom>();
        public static IList<Symptom> AllSymptomDetailList
        {
            get
            {
                return lSymptomDetailList;
            }
            set
            {
                lSymptomDetailList = value;
            }
        }
        public static IList<Symptom> SymptomDetailList
        {
            get
            {
                return lSymptomDetailList.Where(x => x.IsActive == true && x.CompanyId == Global.Company.CompanyId).ToList();
            }
        }
        static IList<Allergie> lAllergieDetailList = new List<Allergie>();
        public static IList<Allergie> AllAllergieDetailList
        {
            get
            {
                return lAllergieDetailList;
            }
            set
            {
                lAllergieDetailList = value;
            }
        }
        public static IList<Allergie> AllergieDetailList
        {
            get
            {
                return lAllergieDetailList.Where(x => x.IsActive == true && x.CompanyId == Global.Company.CompanyId).ToList();
            }
        }

        static IList<MedicalProcedure> lProcedureDetailList = new List<MedicalProcedure>();
        public static IList<MedicalProcedure> AllProcesdureDetailList
        {
            get
            {
                return lProcedureDetailList.Where(x => x.CompanyId == Global.Company.CompanyId).ToList();
            }
            set
            {
                lProcedureDetailList = value;
            }
        }
        public static IList<MedicalProcedure> ProcedureDetailList
        {
            get
            {
                return lProcedureDetailList.Where(x => x.IsActive == true && x.CompanyId == Global.Company.CompanyId).ToList();
            }
        }
        static IList<Product> lProductDetailList = new List<Product>();
        public static IList<Product> ProductDetailList
        {
            get
            {
                return lProductDetailList;
            }
            set
            {
                lProductDetailList = value;
                ProductList = (from P in value select new SProduct { Id = P.Id, Name = P.Name }).ToList<SProduct>();
            }
        }
        public class SProduct
        {
            public long Id { get; set; }
            public string? Name { get; set; }

        }
        static IList<SProduct> lProductList = new List<SProduct>();
        public static IList<SProduct> ProductList
        {
            get
            {
                return lProductList;
            }
            set
            {
                lProductList = value;
            }
        }

        public static DateTime getCurrentFiscalYearStartDate()
        {
            DateTime forDate = TransactionDate;
            int yearNumber;
            if (TransactionDate.Month < Company.AccountingStartDate)
            {
                yearNumber = forDate.Year - 1;
            }
            else
            {
                yearNumber = forDate.Year;
            }
            return new DateTime(yearNumber, Company.AccountingStartDate, 1);
        }

        public static DateTime getCurrentFiscalYearEndDate()
        {
            DateTime forDate = getCurrentFiscalYearStartDate();
            forDate = forDate.AddMonths(12);
            forDate = forDate.AddDays(-1);
            return forDate;
        }

        public enum SelectGender { Transgender, Male, Female, Child, Adult }

        public static BuisnessType LoginType { get; set; }

        public static SoftwareType softwareType = SoftwareType.VVMATRIX;

    }
}
