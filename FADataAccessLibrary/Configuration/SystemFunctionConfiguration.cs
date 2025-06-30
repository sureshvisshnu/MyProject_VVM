using fa.model.UserProfile;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FADataAccessLibrary.Configuration
{
    public class SystemFunctionConfiguration : IEntityTypeConfiguration<SystemFunction>
    {
        public void Configure(EntityTypeBuilder<SystemFunction> builder)
        {
            builder.HasData(
                new SystemFunction() { FunctionId = 1, Name = "salesMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 2, Name = "invoiceToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 3, Name = "quoteToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 4, Name = "returnToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 5, Name = "saleReportToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 6, Name = "receivePaymentToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 7, Name = "deliveryToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 8, Name = "purchaseMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 9, Name = "purchaseEntryToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 10, Name = "purchaseReturnToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 11, Name = "purchaseOrderToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 12, Name = "purchaseReportsToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 13, Name = "transactionToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 14, Name = "InvoiceMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 15, Name = "BillMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 17, Name = "receiptsMenuItem", Description = "Receipt Entry" },
                new SystemFunction() { FunctionId = 18, Name = "paymentsMenuItem", Description = "Payment Entry" },
                new SystemFunction() { FunctionId = 19, Name = "expenseToolStripMenuItem", Description = "Expense Entry" },
                new SystemFunction() { FunctionId = 20, Name = "journalsMenuItem", Description = "Journal Entry" },
                new SystemFunction() { FunctionId = 21, Name = "accountingReportsMenuItem", Description = "Accounting Report Main Menu" },
                new SystemFunction() { FunctionId = 22, Name = "daybookMenuItem", Description = "Report Daybook" },
                new SystemFunction() { FunctionId = 23, Name = "ledgerMenuItem", Description = "Report Ledger" },
                new SystemFunction() { FunctionId = 24, Name = "trialBalanceMenuItem", Description = "Report Trial balance" },
                new SystemFunction() { FunctionId = 25, Name = "profitLossMenuItem", Description = "Report Profit Loss" },
                new SystemFunction() { FunctionId = 26, Name = "balanceSheetMenuItem", Description = "Report Balance Sheet" },
                new SystemFunction() { FunctionId = 27, Name = "agingReportMenuItem", Description = "Report Aging" },
                new SystemFunction() { FunctionId = 28, Name = "creditNoteToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 29, Name = "debitNoteToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 30, Name = "toolsMenu", Description = "" },
                new SystemFunction() { FunctionId = 31, Name = "usersMenuItem", Description = "Managing Users" },
                new SystemFunction() { FunctionId = 32, Name = "changePasswordMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 33, Name = "companyMenuItem", Description = "Managing Companes" },
                new SystemFunction() { FunctionId = 34, Name = "costCenterMenuItem", Description = "Managing Cost Centers" },
                new SystemFunction() { FunctionId = 35, Name = "accountsMenuItem", Description = "Managing General Accounts" },
                new SystemFunction() { FunctionId = 36, Name = "suppliersMenuItem", Description = "Managing Suppliers" },
                new SystemFunction() { FunctionId = 37, Name = "customersMenuItem", Description = "Managing Customers" },
                new SystemFunction() { FunctionId = 38, Name = "masterReportToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 39, Name = "chartOfAccountsToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 40, Name = "catalogToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 41, Name = "manageCatalogToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 42, Name = "catalogReportToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 43, Name = "importDataToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 44, Name = "employeeToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 45, Name = "SettingsMenu", Description = "" },
                new SystemFunction() { FunctionId = 46, Name = "workStationsetupStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 47, Name = "hospitalToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 48, Name = "registrationToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 49, Name = "patientToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 50, Name = "oPQueueToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 51, Name = "wardToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 52, Name = "bedToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 53, Name = "symptomToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 54, Name = "medicalTestToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 55, Name = "symptomToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 56, Name = "medicalTestToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 57, Name = "consultationsToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 58, Name = "receiveFeeToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 59, Name = "nursesTechToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 60, Name = "nursestechOPQueueToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 61, Name = "inPatientToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 62, Name = "doctorToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 63, Name = "doctorOPQueueToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 64, Name = "iPQueueToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 65, Name = "InventoryToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 66, Name = "InventoryAdjustmentEntryMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 67, Name = "inventoryReportToolStripMenuItem", Description = "" },
                //for super admin
                new SystemFunction() { FunctionId = 68, Name = "testToolStripMenuItem", Description = "" }
            );
        }
    }
}
