using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Catalog;
using fa.model.OrderManagement;
using fa.model.System;
using fa.model.UserProfile;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using fa.model.Hms.Master;
using System.ComponentModel.Design;
using MySqlConnector;
using fa.model.Common;
using FADataAccessLibrary.Model.Common;
using System.Globalization;
using System.Data;
using fa.api.catalog;
using System.Data.Common;
using fa.api.Accounting;
using System.Security.Cryptography;

namespace FADataAccessLibrary.Configuration
{
    public class UpdateConfiguration
    {
        CultureInfo provider = CultureInfo.InvariantCulture;
        AccountMasterContext Context;
        public UpdateConfiguration()
        {
            Context=AccountMasterContext.Instance;
        }
        public  bool HasSeedRun(string Key)
        {
            SeedDataHistory SeedHistory = null;
            SeedHistory = Context.SeedDataHistories.Find(Key);
            if (SeedHistory != null)
                return true;
            else
                return false;
        }
        // Update Accountant Role System Function
        public void UpdateAccountantRoleSystemFunction()
        {
            if (HasSeedRun("UpdateAccountantRoleSystemFunction"))
                return;
            Role lRolesFromDB = Context.Roles.Include("SystemFunctions").FirstOrDefault(x => x.RoleId == 2L);
            if(lRolesFromDB != null)
            {
                List<SystemFunction> lSystemFunctions= lRolesFromDB.SystemFunctions.ToList();
                lSystemFunctions.Remove(Context.SystemFunctions.Find(33L));

                lRolesFromDB.SystemFunctions = lSystemFunctions;
                Context.Roles.Update(lRolesFromDB);
                Context.SaveChanges();
            }

            Context.SeedDataHistories.Add(new SeedDataHistory() { Key = "UpdateAccountantRoleSystemFunction", DateExecuted = new System.DateTime() });
            Context.SaveChanges();
        }
        //update account group
        public void UpdateAccountGroupEntry()
        {
            if (HasSeedRun("UpdateAccountGroupEntry"))
                return;
            List<AccountGroup> lAccountGroup = Context.AccountGroups.OrderBy(x => x.ParentAccountGroupId).ToList();
            if (lAccountGroup != null)
            {
                foreach (AccountGroup lGroup in lAccountGroup)
                {                  
                    lGroup.CreditMultiplier = lGroup.AccountClassificationId==1 || lGroup.AccountClassificationId == 4 ?1:-1;
                    lGroup.DebitMultiplier = lGroup.AccountClassificationId==1 || lGroup.AccountClassificationId == 4 ?-1:1;

                    Context.AccountGroups.Update(lGroup);
                    Context.SaveChanges();
                }
            }

            Context.SeedDataHistories.Add(new SeedDataHistory() { Key = "UpdateAccountGroupEntry", DateExecuted = new System.DateTime() });
            Context.SaveChanges();
        }




        public void UpdateAllergieUpdate()
        {
            if (HasSeedRun("UpdateAllergieUpdate"))
                return;

            PatientHistoryQuestionGroup lPatientHistoryQuestionGroup = new PatientHistoryQuestionGroup() {Id =8L,Name = "Allergie", Order = 1 };
            Context.PatientHistoryQuestionGroups.Add(lPatientHistoryQuestionGroup);
            Context.SaveChanges();

            if (lPatientHistoryQuestionGroup != null)
            {
                long GroupId = lPatientHistoryQuestionGroup.Id;
                PatientHistoryQuestionGroup llPatientHistoryQuestionGroup = new PatientHistoryQuestionGroup() { Id = 9L, Name = "Food Allergie", Order = 1,ParentGroupId= GroupId };
                Context.PatientHistoryQuestionGroups.Add(llPatientHistoryQuestionGroup);
                Context.SaveChanges();
                if (llPatientHistoryQuestionGroup != null)
                {
                    GroupId = llPatientHistoryQuestionGroup.Id;
                    List<PatientHistoryQuestion> lPatientHistoryQuestion = new List<PatientHistoryQuestion>()
                    {
                        new PatientHistoryQuestion() {Id = 14L, GroupId=GroupId,  Name = "buckwheat",ValueType=PatientHistoryQuestionType.TEXT,AdditionalNotes=true, AdditionalNotesCaption = "Asthma" },
                        new PatientHistoryQuestion() {Id = 15L, GroupId=GroupId,  Name = "Celery",ValueType=PatientHistoryQuestionType.TEXT,AdditionalNotes=true, AdditionalNotesCaption = "Abdominal pain" },
                        new PatientHistoryQuestion() {Id = 16L, GroupId=GroupId,  Name = "Egg",ValueType=PatientHistoryQuestionType.TEXT,AdditionalNotes=true, AdditionalNotesCaption = "vomiting"},
                        new PatientHistoryQuestion() {Id = 17L, GroupId=GroupId,  Name = "Fruit",ValueType=PatientHistoryQuestionType.TEXT,AdditionalNotes=true, AdditionalNotesCaption = "mild itching, vomiting" },
                        new PatientHistoryQuestion() {Id = 18L, GroupId=GroupId,  Name = "Garlic",ValueType=PatientHistoryQuestionType.TEXT,AdditionalNotes=true, AdditionalNotesCaption = "Dermatitis" },
                        new PatientHistoryQuestion() {Id = 19L, GroupId=GroupId,  Name = "Oats",ValueType=PatientHistoryQuestionType.TEXT,AdditionalNotes=true, AdditionalNotesCaption = "Stomach pain, migraine"},
                        new PatientHistoryQuestion() {Id = 20L, GroupId=GroupId,  Name = "Milk",ValueType=PatientHistoryQuestionType.TEXT,AdditionalNotes=true, AdditionalNotesCaption = "skin rash, vomiting, stomach pain" },
                        new PatientHistoryQuestion() {Id = 21L, GroupId=GroupId,  Name = "poultry meat",ValueType=PatientHistoryQuestionType.TEXT,AdditionalNotes=true, AdditionalNotesCaption = "Vomiting , swelling" },
                        new PatientHistoryQuestion() {Id = 22L, GroupId=GroupId,  Name = "Rice",ValueType=PatientHistoryQuestionType.TEXT,AdditionalNotes=true, AdditionalNotesCaption = "runny nose, itching"},
                        new PatientHistoryQuestion() {Id = 23L, GroupId=GroupId,  Name = "Sesame",ValueType=PatientHistoryQuestionType.TEXT,AdditionalNotes=true, AdditionalNotesCaption = "Skin problem, gastrointestinal reactions" },
                        new PatientHistoryQuestion() {Id = 24L, GroupId=GroupId,  Name = "Shellfish",ValueType=PatientHistoryQuestionType.TEXT,AdditionalNotes=true, AdditionalNotesCaption = "oral allergy symptoms" },
                        new PatientHistoryQuestion() {Id = 25L, GroupId=GroupId,  Name = "Wheat",ValueType=PatientHistoryQuestionType.TEXT,AdditionalNotes=true, AdditionalNotesCaption = "Asthma, oral allergy syndrome" }
                    };
                    foreach (PatientHistoryQuestion Question in lPatientHistoryQuestion)
                    {
                        Context.PatientHistoryQuestions.Add(Question);
                        Context.SaveChanges();
                    }
                }
            }
            Context.SeedDataHistories.Add(new SeedDataHistory() { Key = "UpdateAllergieUpdate", DateExecuted = new System.DateTime() });
            Context.SaveChanges();
        }
        public void UpdateIdSpaceDetailsWithPatientId()
        {
            long idSpaceEntryTypeDetailsId = 29L;
            IdSpaceEntryTypeDetail detail = Context.IdSpaceEntryTypeDetails.Find(idSpaceEntryTypeDetailsId);
            if (detail == null)
            {
                IList<IdSpaceEntryTypeDetail> idSpaceEntryTypeDetails = new List<IdSpaceEntryTypeDetail>()
                {
                    new IdSpaceEntryTypeDetail() { Id = 29L, EntryType = EntryType.PATIENT_ID, HasDotMatrix = false, HasPrinterSetup = false, HasRoundOff = false, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.0, Prefix = string.Empty }
                };
                foreach (IdSpaceEntryTypeDetail idSpaceEntryTypeDetail in idSpaceEntryTypeDetails)
                {
                    Context.IdSpaceEntryTypeDetails.Add(idSpaceEntryTypeDetail);
                    Context.SaveChanges();
                }
            }
        }
        public void UpdateIdSpaceDetailsWithPatientOpIdAndIpId()
        {
            long[] idsToCheck = { 30L, 31L };

            foreach (long id in idsToCheck)
            {
                var detail = Context.IdSpaceEntryTypeDetails.SingleOrDefault(x => x.Id == id);
                if (detail == null)
                {
                    var newDetail = new IdSpaceEntryTypeDetail()
                    {
                        Id = id,
                        HasDotMatrix = false,
                        HasPrinterSetup = false,
                        HasRoundOff = false,
                        IsDotMatrix = false,
                        IsResetDaily = false,
                        RoundOff = 0.0,
                        Prefix = string.Empty
                    };

                    switch (id)
                    {
                        case 30L:
                            newDetail.EntryType = EntryType.OP_ID;
                            break;

                        case 31L:
                            newDetail.EntryType = EntryType.IP_ID;
                            break;
                    }
                    Context.IdSpaceEntryTypeDetails.Add(newDetail);
                }
            }
            Context.SaveChanges();
        }
        public void UpdatePatientHistoryQuestionsWithAllergies()
        {
            if (HasSeedRun("UpdatePatientHistoryQuestionsWithAllergies"))
                return;

            long[] questionIds = new long[] { 14L, 15L, 16L, 17L, 18L, 19L, 20L, 21L, 22L, 23L, 24L, 25L };

            foreach (var id in questionIds)
            {
                var question = Context.PatientHistoryQuestions.Find(id);
                if (question != null)
                {
                    question.ValueType = PatientHistoryQuestionType.YESNO;
                    question.PosibleValues = new string[] { "Yes", "No" };
                    question.AdditionalNotes = false;
                    question.AdditionalNotesCaption = null;
                    Context.PatientHistoryQuestions.Update(question);
                }
            }
            Context.SaveChanges();

            long GroupId;
            PatientHistoryQuestionGroup lPatientHistoryQuestionGroup = new PatientHistoryQuestionGroup() { Id = 10L, Name = "Drug Allergie", Order = 1, ParentGroupId = 8L };
            Context.PatientHistoryQuestionGroups.Add(lPatientHistoryQuestionGroup);
            Context.SaveChanges();
            if (lPatientHistoryQuestionGroup != null)
            {
                GroupId = lPatientHistoryQuestionGroup.Id;
                List<PatientHistoryQuestion> lPatientHistoryQuestion = new List<PatientHistoryQuestion>()
                {
                    new PatientHistoryQuestion() {Id = 26L, GroupId=GroupId, Order = 1, Name = "Penicillin", ValueType=PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } },
                    new PatientHistoryQuestion() {Id = 27L, GroupId=GroupId, Order = 2, Name = "Tegretol", ValueType=PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } },
                    new PatientHistoryQuestion() {Id = 28L, GroupId=GroupId, Order = 3, Name = "Tetracycline", ValueType=PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } },
                    new PatientHistoryQuestion() {Id = 29L, GroupId=GroupId, Order = 4, Name = "Local anesthetics", ValueType=PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } },
                    new PatientHistoryQuestion() {Id = 30L, GroupId=GroupId, Order = 5, Name = "Dilantin", ValueType=PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } },
                    new PatientHistoryQuestion() {Id = 31L, GroupId=GroupId, Order = 6, Name = "Cephalosporins", ValueType=PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } }
                };
                foreach (PatientHistoryQuestion Question in lPatientHistoryQuestion)
                {
                    Context.PatientHistoryQuestions.Add(Question);
                    Context.SaveChanges();
                }
            }
            PatientHistoryQuestionGroup llPatientHistoryQuestionGroup = new PatientHistoryQuestionGroup() { Id = 11L, Name = "Environmental Allergie", Order = 1, ParentGroupId = 8L };
            Context.PatientHistoryQuestionGroups.Add(llPatientHistoryQuestionGroup);
            Context.SaveChanges();
            if (llPatientHistoryQuestionGroup != null)
            {
                GroupId = llPatientHistoryQuestionGroup.Id;
                List<PatientHistoryQuestion> llPatientHistoryQuestion = new List<PatientHistoryQuestion>()
                {
                    new PatientHistoryQuestion() {Id = 32L, GroupId=GroupId, Order = 1, Name = "Cat", ValueType=PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } },
                    new PatientHistoryQuestion() {Id = 33L, GroupId=GroupId, Order = 2, Name = "Dog", ValueType=PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } },
                    new PatientHistoryQuestion() {Id = 34L, GroupId=GroupId, Order = 3, Name = "Insect sting", ValueType=PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } },
                    new PatientHistoryQuestion() {Id = 35L, GroupId=GroupId, Order = 4, Name = "Perfume", ValueType=PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } },
                    new PatientHistoryQuestion() {Id = 36L, GroupId=GroupId, Order = 5, Name = "Cosmetics", ValueType=PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } },
                    new PatientHistoryQuestion() {Id = 37L, GroupId=GroupId, Order = 6, Name = "House dust mite", ValueType=PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } },
                    new PatientHistoryQuestion() {Id = 38L, GroupId=GroupId, Order = 7, Name = "Chromium", ValueType=PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } },
                    new PatientHistoryQuestion() {Id = 39L, GroupId=GroupId, Order = 8, Name = "Formaldehyde", ValueType=PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } },
                    new PatientHistoryQuestion() {Id = 40L, GroupId=GroupId, Order = 9, Name = "Cold stimuli", ValueType=PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } },
                    new PatientHistoryQuestion() {Id = 41L, GroupId=GroupId, Order = 10, Name = "Nickel", ValueType=PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } }
                };
                foreach (PatientHistoryQuestion Question in llPatientHistoryQuestion)
                {
                    Context.PatientHistoryQuestions.Add(Question);
                    Context.SaveChanges();
                }
            }

            Context.SeedDataHistories.Add(new SeedDataHistory() { Key = "UpdatePatientHistoryQuestionsWithAllergies", DateExecuted = new System.DateTime() });
            Context.SaveChanges();
        }

        //update and add role/system function
        public void UserRoleandSystemfunctionUpdate()
        {
            if (HasSeedRun("UserRoleandSystemfunctionUpdate"))
            return;

            //add new system function
            List<SystemFunction> lSystemFunction = new List<SystemFunction>(){
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
                new SystemFunction() { FunctionId = 68, Name = "testToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 69, Name = "saleReportToolStripMenuItem1", Description = "" },
                new SystemFunction() { FunctionId = 70, Name = "gstReportToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 71, Name = "saleReturnReportToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 72, Name = "purchaseReportToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 73, Name = "gstrReportToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 74, Name = "purchaseReturnReportToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 75, Name = "pOSToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 76, Name = "moveStockToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 77, Name = "intraStockMovementToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 78, Name = "intraStockReceiveToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 79, Name = "intraStockRequestToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 80, Name = "createInvoiceMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 81, Name = "damageEntryToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 82, Name = "inventoryLocationToolStripMenuItem1", Description = "" },
                new SystemFunction() { FunctionId = 83, Name = "inventoryLocationToolStripMenuItem2", Description = "" },
                new SystemFunction() { FunctionId = 84, Name = "reportToolStripMenuItem1", Description = "" },
                new SystemFunction() { FunctionId = 85, Name = "currentStockToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 86, Name = "priceListToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 87, Name = "stockRequestsToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 88, Name = "expeToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 89, Name = "masterToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 90, Name = "wardToolStripMenuItem1", Description = "" },
                new SystemFunction() { FunctionId = 91, Name = "bedTypeToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 92, Name = "symptomToolStripMenuItem1", Description = "" },
                new SystemFunction() { FunctionId = 93, Name = "medicalTestToolStripMenuItem1", Description = "" },
                new SystemFunction() { FunctionId = 94, Name = "medicalProceduresToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 95, Name = "consultationToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 96, Name = "settingsToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 97, Name = "reportToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 98, Name = "opReportToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 99, Name = "iPReportToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 100, Name = "patientLedgerToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 101, Name = "patientDueListToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 102, Name = "inPatientCareToolStripMenuItem1", Description = "" },
                new SystemFunction() { FunctionId = 103, Name = "transferPatientToolStripMenuItem1", Description = "" },
                new SystemFunction() { FunctionId = 104, Name = "reAssignCareTakerToolStripMenuItem1", Description = "" },
                new SystemFunction() { FunctionId = 105, Name = "dischargePatientToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 106, Name = "performProceduresToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 107, Name = "consultingToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 108, Name = "transactionReportToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 109, Name = "manageItemTaxToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 110, Name = "miseleneousToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 111, Name = "paymentTermsToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 112, Name = "paymentMethodToolStripMenuItem", Description = "" },
                new SystemFunction() { FunctionId = 113, Name = "helpMenu", Description = "" },
                new SystemFunction() { FunctionId = 114, Name = "OutPatientCareToolStripMenuItem", Description = "" }
            };
            List<SystemFunction> lSystemFunctionFromDB = Context.SystemFunctions.Where(x => x.FunctionId < 69).ToList();
            foreach (SystemFunction system in (lSystemFunctionFromDB != null && lSystemFunctionFromDB.Count > 0 ? lSystemFunction.Where(x => x.FunctionId > 68) : lSystemFunction))
            {
                Context.SystemFunctions.Add(system);
                Context.SaveChanges();
            }
            //find system function
            var f1 = Context.SystemFunctions.Find(1L);
            var f2 = Context.SystemFunctions.Find(2L);
            var f3 = Context.SystemFunctions.Find(3L);
            var f4 = Context.SystemFunctions.Find(4L);
            var f5 = Context.SystemFunctions.Find(5L);
            var f6 = Context.SystemFunctions.Find(6L);
            var f7 = Context.SystemFunctions.Find(7L);
            var f8 = Context.SystemFunctions.Find(8L);
            var f9 = Context.SystemFunctions.Find(9L);
            var f10 = Context.SystemFunctions.Find(10L);
            var f11 = Context.SystemFunctions.Find(11L);
            var f12 = Context.SystemFunctions.Find(12L);
            var f13 = Context.SystemFunctions.Find(13L);
            var f14 = Context.SystemFunctions.Find(14L);
            var f15 = Context.SystemFunctions.Find(15L);
            var f17 = Context.SystemFunctions.Find(17L);
            var f18 = Context.SystemFunctions.Find(18L);
            var f19 = Context.SystemFunctions.Find(19L);
            var f20 = Context.SystemFunctions.Find(20L);
            var f21 = Context.SystemFunctions.Find(21L);
            var f22 = Context.SystemFunctions.Find(22L);
            var f23 = Context.SystemFunctions.Find(23L);
            var f24 = Context.SystemFunctions.Find(24L);
            var f25 = Context.SystemFunctions.Find(25L);
            var f26 = Context.SystemFunctions.Find(26L);
            var f27 = Context.SystemFunctions.Find(27L);
            var f28 = Context.SystemFunctions.Find(28L);
            var f29 = Context.SystemFunctions.Find(29L);
            var f30 = Context.SystemFunctions.Find(30L);
            var f31 = Context.SystemFunctions.Find(31L);
            var f32 = Context.SystemFunctions.Find(32L);
            var f33 = Context.SystemFunctions.Find(33L);
            var f34 = Context.SystemFunctions.Find(34L);
            var f35 = Context.SystemFunctions.Find(35L);
            var f36 = Context.SystemFunctions.Find(36L);
            var f37 = Context.SystemFunctions.Find(37L);
            var f38 = Context.SystemFunctions.Find(38L);
            var f39 = Context.SystemFunctions.Find(39L);
            var f40 = Context.SystemFunctions.Find(40L);
            var f41 = Context.SystemFunctions.Find(41L);
            var f42 = Context.SystemFunctions.Find(42L);
            var f43 = Context.SystemFunctions.Find(43L);
            var f44 = Context.SystemFunctions.Find(44L);
            var f45 = Context.SystemFunctions.Find(45L);
            var f46 = Context.SystemFunctions.Find(46L);
            var f47 = Context.SystemFunctions.Find(47L);
            var f48 = Context.SystemFunctions.Find(48L);
            var f49 = Context.SystemFunctions.Find(49L);
            var f50 = Context.SystemFunctions.Find(50L);
            var f51 = Context.SystemFunctions.Find(51L);
            var f52 = Context.SystemFunctions.Find(52L);
            var f53 = Context.SystemFunctions.Find(53L);
            var f54 = Context.SystemFunctions.Find(54L);
            var f55 = Context.SystemFunctions.Find(55L);
            var f56 = Context.SystemFunctions.Find(56L);
            var f57 = Context.SystemFunctions.Find(57L);
            var f58 = Context.SystemFunctions.Find(58L);
            var f59 = Context.SystemFunctions.Find(59L);
            var f60 = Context.SystemFunctions.Find(60L);
            var f61 = Context.SystemFunctions.Find(61L);
            var f62 = Context.SystemFunctions.Find(62L);
            var f63 = Context.SystemFunctions.Find(63L);
            var f64 = Context.SystemFunctions.Find(64L);
            var f65 = Context.SystemFunctions.Find(65L);
            var f66 = Context.SystemFunctions.Find(66L);
            var f67 = Context.SystemFunctions.Find(67L);
            var f68 = Context.SystemFunctions.Find(68L);

            var f69 = Context.SystemFunctions.Find(69L);
            var f70 = Context.SystemFunctions.Find(70L);
            var f71 = Context.SystemFunctions.Find(71L);
            var f72 = Context.SystemFunctions.Find(72L);
            var f73 = Context.SystemFunctions.Find(73L);
            var f74 = Context.SystemFunctions.Find(74L);
            var f75 = Context.SystemFunctions.Find(75L);
            var f76 = Context.SystemFunctions.Find(76L);
            var f77 = Context.SystemFunctions.Find(77L);
            var f78 = Context.SystemFunctions.Find(78L);
            var f79 = Context.SystemFunctions.Find(79L);
            var f80 = Context.SystemFunctions.Find(80L);
            var f81 = Context.SystemFunctions.Find(81L);
            var f82 = Context.SystemFunctions.Find(82L);
            var f83 = Context.SystemFunctions.Find(83L);
            var f84 = Context.SystemFunctions.Find(84L);
            var f85 = Context.SystemFunctions.Find(85L);
            var f86 = Context.SystemFunctions.Find(86L);
            var f87 = Context.SystemFunctions.Find(87L);
            var f88 = Context.SystemFunctions.Find(88L);
            var f89 = Context.SystemFunctions.Find(89L);
            var f90 = Context.SystemFunctions.Find(90L);
            var f91 = Context.SystemFunctions.Find(91L);
            var f92 = Context.SystemFunctions.Find(92L);
            var f93 = Context.SystemFunctions.Find(93L);
            var f94 = Context.SystemFunctions.Find(94L);
            var f95 = Context.SystemFunctions.Find(95L);
            var f96 = Context.SystemFunctions.Find(96L);
            var f97 = Context.SystemFunctions.Find(97L);
            var f98 = Context.SystemFunctions.Find(98L);
            var f99 = Context.SystemFunctions.Find(99L);
            var f100 = Context.SystemFunctions.Find(100L);
            var f101 = Context.SystemFunctions.Find(101L);
            var f102 = Context.SystemFunctions.Find(102L);
            var f103 = Context.SystemFunctions.Find(103L);
            var f104 = Context.SystemFunctions.Find(104L);
            var f105 = Context.SystemFunctions.Find(105L);
            var f106 = Context.SystemFunctions.Find(106L);
            var f107 = Context.SystemFunctions.Find(107L);
            var f108 = Context.SystemFunctions.Find(108L);
            var f109 = Context.SystemFunctions.Find(109L);
            var f110 = Context.SystemFunctions.Find(110L);
            var f111 = Context.SystemFunctions.Find(111L);
            var f112 = Context.SystemFunctions.Find(112L);
            var f113 = Context.SystemFunctions.Find(113L);
            var f114 = Context.SystemFunctions.Find(114L);

            SystemFunction[] sf1 = { f1, f2, f3, f4,f5,f6,f7,f8,f9,f10,f11,f12,f13, f14, f15, f17, f18, f19, f20, f21, f22, f23, f24, f25, f26, f27, f28, f29, f30,f31,f32, f33,f34, f35, f36, f37, f38, f39,f40,f41,f42,f43, f44,
                f45,f46,f47,f48,f49,f50,f51,f52,f53,f54,f55,f56,f57,f58,f59,f60,f61,f62,f63,f64,f65,f66,f67,f68,f69,f70,f71,f72,f73,f74, f75, f76, f77, f78, f79, f80, f81, f82, f83, f84, f85, f86, f87, f88,f89,f90,f91,f92,f93,
                f94,f95,f96,f97,f98,f99,f100,f101,f102,f103,f104,f105,f106,f107,f108,f109, f110, f111, f112,f113,f114 };
            SystemFunction[] sf2 = { f13, f14, f15, f17, f18, f19, f20, f21, f22, f23, f24, f25, f26, f27, f28, f29, f30, f35, f36, f37, f38, f39, f44, f108, f110, f111, f112 };
            SystemFunction[] sf3 = { f1, f2, f3, f4, f5, f6, f7, f8, f9, f10, f11, f12, f13, f14, f15, f30, f32, f40, f41, f42, f43, f44 };
            SystemFunction[] sf4 = { f1, f2, f3, f4 };
            SystemFunction[] sf5 = { f8, f9, f10, f11, f67, f75, f76, f77, f78, f79, f80, f81, f82, f83, f84, f85, f86, f87, f88 };
            SystemFunction[] sf6 = { f59, f60, f61, f62, f63, f64, f106, f107 };
            SystemFunction[] sf7 = { f59, f60, f61, f106 };
            SystemFunction[] sf8 = { f1, f6, f13, f17 };
            SystemFunction[] sf9 = { f1, f2, f3, f4 };
            SystemFunction[] sf10 = { f8, f9, f10, f11,f67,f75,f76,f77,f78,f79,f80,f81,f82,f83,f84,f85,f86,f87,f88 };
            SystemFunction[] sf11 = { f47, f48, f49,f59,f60,f61,f97,f98,f99,f100,f101,f102,f103,f104,f105,f106,f114 };
            SystemFunction[] sf12 = { f47, f48, f49,f58, f97, f98, f99, f100, f101, f102,f105,f114 };
            SystemFunction[] sf13 = { f47,f93 };

            //add new roles
            List<Role> lRolesFromDB = Context.Roles.Where(x => x.RoleId < 8).ToList();
            List<Role> lRoles = null;
            if (lRolesFromDB != null && lRolesFromDB.Count > 0)
            {
                lRoles = new List<Role>()
                {
                new Role() { RoleId = 8, Name = "Cashier", Description = "Cashier User, responsible for handling financial transactions", SystemFunctions =sf8.ToList<SystemFunction>() },
                new Role() { RoleId = 9, Name = "Sales Clerk", Description = "Sales Clerk, This user will have access to Point of Sale systems", SystemFunctions = sf9.ToList<SystemFunction>() },
                new Role() { RoleId = 10, Name = "Inventory Manager", Description = "Inventory Manager User, responsible for overseeing the inventory management process in a hospital or Organization", SystemFunctions = sf10.ToList<SystemFunction>() },
                new Role() { RoleId = 11, Name = "Nursing Station Staff", Description = "Hospital User, This user will have access to Point of Hospital", SystemFunctions = sf11.ToList<SystemFunction>() },
                new Role() { RoleId = 12, Name = "Record Section Staff", Description = "Record Section Staff, responsible for the management and maintenance of medical records in a hospital", SystemFunctions = sf12.ToList<SystemFunction>() },
                new Role() { RoleId = 13, Name = "Lab Technician", Description = "Hospital User, This user will have access to Point of Hospital", SystemFunctions = sf13.ToList<SystemFunction>() }
                };
                foreach (Role role in lRoles)
                {
                    Context.Roles.Add(role);
                    Context.SaveChanges();
                }
                //update role system function map
                if (lRolesFromDB != null && lRolesFromDB.Count > 0)
                {
                    foreach (Role role in lRolesFromDB)
                    {
                        role.SystemFunctions = (role.RoleId == 1L ? sf1 : role.RoleId == 2L ? sf2 : role.RoleId == 3L ? sf3 : role.RoleId == 4L ? sf4 : role.RoleId == 5L ? sf5 : role.RoleId == 6L ? sf6 : sf7).ToList<SystemFunction>();
                        Context.Roles.Update(role);
                        Context.SaveChanges();
                    }
                }
            }
            else
            {
                lRoles = new List<Role>()
                {
                new Role() { RoleId = 1, Name = "Admin", Description = "Admin User, This user will have access to the entire system" , SystemFunctions =sf1.ToList<SystemFunction>()},
                new Role() { RoleId = 2, Name = "Accountant", Description = "Accountant User, This user will have access to most of the finacial accounting functionality", SystemFunctions =sf2.ToList<SystemFunction>() },
                new Role() { RoleId = 3, Name = "Standard", Description = "POS User, This user will have access to Point of Sale", SystemFunctions =sf3.ToList<SystemFunction>() },
                new Role() { RoleId = 4, Name = "POSUser", Description = "POS User, This user will have access to Point of Sale" , SystemFunctions =sf4.ToList<SystemFunction>()},
                new Role() { RoleId = 5, Name = "Purchaser", Description = "POS User, This user will have access to Point of Sale" , SystemFunctions =sf5.ToList<SystemFunction>()},
                new Role() { RoleId = 6, Name = "Doctor", Description = "Hospital User, This user will have access to Point of Hospital" , SystemFunctions =sf6.ToList<SystemFunction>()},
                new Role() { RoleId = 7, Name = "Nurse", Description = "Hospital User, This user will have access to Point of Hospital", SystemFunctions =sf7.ToList<SystemFunction>() },
                new Role() { RoleId = 8, Name = "Cashier", Description = "Cashier User, responsible for handling financial transactions", SystemFunctions =sf8.ToList<SystemFunction>() },
                new Role() { RoleId = 9, Name = "Sales Clerk", Description = "Sales Clerk, This user will have access to Point of Sale systems", SystemFunctions = sf9.ToList<SystemFunction>() },
                new Role() { RoleId = 10, Name = "Inventory Manager", Description = "Inventory Manager User, responsible for overseeing the inventory management process in a hospital or Organization", SystemFunctions = sf10.ToList<SystemFunction>() },
                new Role() { RoleId = 11, Name = "Nursing Station Staff", Description = "Hospital User, This user will have access to Point of Hospital", SystemFunctions = sf11.ToList<SystemFunction>() },
                new Role() { RoleId = 12, Name = "Record Section Staff", Description = "Record Section Staff, responsible for the management and maintenance of medical records in a hospital", SystemFunctions = sf12.ToList<SystemFunction>() },
                new Role() { RoleId = 13, Name = "Lab Technician", Description = "Hospital User, This user will have access to Point of Hospital", SystemFunctions = sf13.ToList<SystemFunction>() }
                };
                foreach (Role role in lRoles)
                {
                    Context.Roles.Add(role);
                    Context.SaveChanges();
                }
            }
            Context.SeedDataHistories.Add(new SeedDataHistory() { Key = "UserRoleandSystemfunctionUpdate", DateExecuted = new System.DateTime() });
            Context.SaveChanges();
        }
        //Country Sale Tax Seed Update For Maldives
        public void CountrySaleTaxSeedUpdateForMaldives()
        {
            if (HasSeedRun("CountrySaleTaxSeedUpdateForMaldives"))
                return;
            //add currency
            Currency currency = new Currency() { CurrencyId = 95, CurrencyCodeISO = "MVR", Name = "Rufiyaa", DisplayAs = "MVR", RoundingPrecision = 2, CurrencyFormat = "#,##,##,##0.00" };
            Context.Currencies.Add(currency);
            Context.SaveChanges();
            //activate country
            Country Country = Context.Countries.Find(130L);
            if(Country != null)
            {
                Country.Active = true;
                Country.DefaultCurrencyId = currency.CurrencyId;
                Context.Entry(Country).CurrentValues.SetValues(Country);
                Context.SaveChanges();
            }
            //add country state
            List<State> lState = new List<State>(){
               new State() { Id = 51, Name = "Maale", ISOCode = "MV-ME", DisplayAs = "ME", CountryId =130, Code = "1" },
                new State() { Id = 52, Name = "Seenu", ISOCode = "MV-SU", DisplayAs = "SU", CountryId =130, Code = "2" },
                new State() { Id = 53, Name = "Faafu", ISOCode = "MV-FU", DisplayAs = "FU", CountryId =130, Code = "3" },
                new State() { Id = 54, Name = "Gaafu Alifu", ISOCode = "MV-GA", DisplayAs = "GA", CountryId =130, Code = "4" },
                new State() { Id = 55, Name = "Gaafu Dhaalu", ISOCode = "MV-GD", DisplayAs = "GD", CountryId =130, Code = "5" },
                new State() { Id = 56, Name = "Gnaviyani", ISOCode = "MV-GN", DisplayAs = "GN", CountryId =130, Code = "6" },
                new State() { Id = 57, Name = "Haa Alifu", ISOCode = "MV-HA", DisplayAs = "HA", CountryId =130, Code = "7" },
                new State() { Id = 58, Name = "Haa Dhaalu", ISOCode = "MV-HD", DisplayAs = "HD", CountryId =130, Code = "8" },
                new State() { Id = 59, Name = "Kaafu", ISOCode = "MV-KU", DisplayAs = "KU", CountryId =130, Code = "9" },
                new State() { Id = 60, Name = "Laamu", ISOCode = "MV-LU", DisplayAs = "LU", CountryId =130, Code = "10" },
                new State() { Id = 61, Name = "Lhaviyani", ISOCode = "MV-LH", DisplayAs = "LH", CountryId =130, Code = "11" },
                new State() { Id = 62, Name = "Meemu", ISOCode = "MV-MU", DisplayAs = "MU", CountryId =130, Code = "12" },
                new State() { Id = 63, Name = "Noonu", ISOCode = "MV-NU", DisplayAs = "NU", CountryId =130, Code = "13" },
                new State() { Id = 64, Name = "Raa", ISOCode = "MV-RA", DisplayAs = "RA", CountryId =130, Code = "14" },
                new State() { Id = 65, Name = "Shaviyani", ISOCode = "MV-SH", DisplayAs = "SH", CountryId =130, Code = "15" },
                new State() { Id = 66, Name = "Thaa", ISOCode = "MV-TA", DisplayAs = "TA", CountryId =130, Code = "16" },
                new State() { Id = 67, Name = "Baa", ISOCode = "MV-BA", DisplayAs = "BA", CountryId =130, Code = "17" },
                new State() { Id = 68, Name = "Vaavu", ISOCode = "MV-VU", DisplayAs = "VU", CountryId =130, Code = "18" },
                new State() { Id = 69, Name = "Dhaalu", ISOCode = "MV-DU", DisplayAs = "DU", CountryId = 130, Code = "19" }
            };
            foreach (State State in lState)
            {
                Context.States.Add(State);
                Context.SaveChanges();
            }
            //add country sale tax
            CountrySaleTax countrySaleTax = new CountrySaleTax() { Id = 5, Name = "GST", CountryId = 130, Discription = "Goods and services tax", Rule = "RunGST()", EffectiveFrom = DateTime.ParseExact("01-01-2017", "MM-dd-yyyy", provider), EffectiveTo = DateTime.Now.AddYears(2400 - DateTime.Now.Year) };
            Context.CountrySaleTaxs.Add(countrySaleTax);
            Context.SaveChanges();
            //update seed history
            Context.SeedDataHistories.Add(new SeedDataHistory() { Key = "CountrySaleTaxSeedUpdateForMaldives", DateExecuted = new System.DateTime() });
            Context.SaveChanges();
        }


        public void IdSpaceEntryTypeConfigUpdate()
        {
            if (HasSeedRun("IdSpaceEntryTypeConfigUpdate"))
                return;
            //add Idspace entry type for hms
            List<IdSpaceEntryTypeDetail> lDetail = new List<IdSpaceEntryTypeDetail>(){
                new IdSpaceEntryTypeDetail() { Id = 32, EntryType = EntryType.STOCK_REQUEST, HasPrinterSetup = true, HasDotMatrix = false, HasRoundOff = false, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty }
            };
            foreach (IdSpaceEntryTypeDetail Detail in lDetail)
            {
                Context.IdSpaceEntryTypeDetails.Add(Detail);
                Context.SaveChanges();
            }
            var ListCompany = Context.Companies.ToList();
            if (ListCompany != null && ListCompany.Count > 0)
            {
                foreach (Company company in ListCompany)
                {
                    if (company != null && company.BusinessType == BuisnessType.Hospital)
                    {
                        List<IdSpace> IdSpaceInfo = Context.IdSpaces.Where(c => c.CompanyId == company.CompanyId).ToList();
                        if (IdSpaceInfo != null && IdSpaceInfo.Count > 0)
                        {                            
                            foreach (var IdSpace in IdSpaceInfo.DistinctBy(x=>x.YearStartDate))
                            {
                                if (IdSpace != null)
                                {
                                    IdSpace lIdSpace = Context.IdSpaces.FirstOrDefault(x => x.CompanyId == IdSpace.CompanyId && x.YearStartDate == IdSpace.YearStartDate && x.YearEndDate == IdSpace.YearEndDate && x.EntryType == EntryType.PATIENT_ID);
                                    if (lIdSpace == null)
                                    {
                                        lIdSpace = new IdSpace();
                                        lIdSpace.EntryType = EntryType.PATIENT_ID;
                                        lIdSpace.HasDotMatrix = false;
                                        lIdSpace.HasPrinterSetup = true;
                                        lIdSpace.HasRoundOff = false;
                                        lIdSpace.IsResetDaily = false;
                                        lIdSpace.IsDotMatrix = false;
                                        lIdSpace.RoundOff =0.00;
                                        lIdSpace.Seed = 1;
                                        lIdSpace.Date = fa.Data.Global.TransactionDate;
                                        lIdSpace.Prefix = string.Empty;
                                        lIdSpace.CompanyId = IdSpace.CompanyId;
                                        lIdSpace.RunningSeed = 1;
                                        lIdSpace.YearStartDate = IdSpace.YearStartDate;
                                        lIdSpace.YearEndDate = IdSpace.YearEndDate;
                                        lIdSpace.PrintPaperFormat_Id = (lIdSpace.EntryType == EntryType.PRESCRIPTION || lIdSpace.EntryType == EntryType.PATIENT_INVOICE) ? 2 : (lIdSpace.EntryType == EntryType.OP_TOKEN || lIdSpace.EntryType == EntryType.PATIENT_FEE_RECEIPT) ? 6 : 5;
                                        Context.IdSpaces.Add(lIdSpace);
                                        lIdSpace = null;
                                    }
                                    lIdSpace = Context.IdSpaces.FirstOrDefault(x => x.CompanyId == IdSpace.CompanyId && x.YearStartDate == IdSpace.YearStartDate && x.YearEndDate == IdSpace.YearEndDate && x.EntryType == EntryType.OP_ID);
                                    if (lIdSpace == null)
                                    {
                                        lIdSpace = new IdSpace();
                                        lIdSpace.EntryType = EntryType.OP_ID;
                                        lIdSpace.HasDotMatrix = false;
                                        lIdSpace.HasPrinterSetup = true;
                                        lIdSpace.HasRoundOff = false;
                                        lIdSpace.IsResetDaily = false;
                                        lIdSpace.IsDotMatrix = false;
                                        lIdSpace.RoundOff = 0.00;
                                        lIdSpace.Seed = 1;
                                        lIdSpace.Date = fa.Data.Global.TransactionDate;
                                        lIdSpace.Prefix = string.Empty;
                                        lIdSpace.CompanyId = IdSpace.CompanyId;
                                        lIdSpace.RunningSeed = 1;
                                        lIdSpace.YearStartDate = IdSpace.YearStartDate;
                                        lIdSpace.YearEndDate = IdSpace.YearEndDate;
                                        lIdSpace.PrintPaperFormat_Id = (lIdSpace.EntryType == EntryType.PRESCRIPTION || lIdSpace.EntryType == EntryType.PATIENT_INVOICE) ? 2 : (lIdSpace.EntryType == EntryType.OP_TOKEN || lIdSpace.EntryType == EntryType.PATIENT_FEE_RECEIPT) ? 6 : 5;
                                        Context.IdSpaces.Add(lIdSpace);
                                        lIdSpace = null;
                                    }
                                    lIdSpace = Context.IdSpaces.FirstOrDefault(x => x.CompanyId == IdSpace.CompanyId && x.YearStartDate == IdSpace.YearStartDate && x.YearEndDate == IdSpace.YearEndDate && x.EntryType == EntryType.IP_ID);
                                    if (lIdSpace == null)
                                    {
                                        lIdSpace = new IdSpace();
                                        lIdSpace.EntryType = EntryType.IP_ID;
                                        lIdSpace.HasDotMatrix = false;
                                        lIdSpace.HasPrinterSetup = true;
                                        lIdSpace.HasRoundOff = false;
                                        lIdSpace.IsResetDaily = false;
                                        lIdSpace.IsDotMatrix = false;
                                        lIdSpace.RoundOff = 0.00;
                                        lIdSpace.Seed = 1;
                                        lIdSpace.Date = fa.Data.Global.TransactionDate;
                                        lIdSpace.Prefix = string.Empty;
                                        lIdSpace.CompanyId = IdSpace.CompanyId;
                                        lIdSpace.RunningSeed = 1;
                                        lIdSpace.YearStartDate = IdSpace.YearStartDate;
                                        lIdSpace.YearEndDate = IdSpace.YearEndDate;
                                        lIdSpace.PrintPaperFormat_Id = (lIdSpace.EntryType == EntryType.PRESCRIPTION || lIdSpace.EntryType == EntryType.PATIENT_INVOICE) ? 2 : (lIdSpace.EntryType == EntryType.OP_TOKEN || lIdSpace.EntryType == EntryType.PATIENT_FEE_RECEIPT) ? 6 : 5;
                                        Context.IdSpaces.Add(lIdSpace);
                                        lIdSpace = null;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //update seed history
            Context.SeedDataHistories.Add(new SeedDataHistory() { Key = "IdSpaceEntryTypeConfigUpdate", DateExecuted = new System.DateTime() });
            Context.SaveChanges();
        }







        //Country Sale Tax Seed Update For India
        public void CountrySaleTaxSeedUpdateForIndia()
        {
            if (HasSeedRun("CountrySaleTaxSeedUpdateForIndia"))
                return;
            var ListCompany = Context.Companies.ToList();
            foreach (var company in ListCompany)
            {
                if (company.CountryId == 99)
                {
                    if (Context.CompanySalesTaxAccountMaps.Where(x => x.CompanyId == company.CompanyId && x.CountrySaleTaxId == null).ToList().Count > 0 || Context.CompanySalesTaxAccountMaps.Where(x => x.CompanyId == company.CompanyId).ToList().Count==3)
                    {
                        List<CompanySalesTaxAccountMap> lCompanySalesTaxAccountMap = Context.CompanySalesTaxAccountMaps.Where(x => x.CompanyId == company.CompanyId).ToList();
                        Account accountdb = Context.Accounts.FirstOrDefault(x => x.Name == "Integrated Sales Tax Payable" && x.CompanyId == company.CompanyId);
                        if (accountdb == null)
                        {
                            accountdb = new Account
                            {
                                Name = "Integrated Sales Tax Payable",
                                Discription = "Sales Tax Payable Account, where all sales tax payables goes into",
                                CompanyId = company.CompanyId,
                                BalanceAsOf = DateTime.Today,
                                AccountGroupId = 810,
                                AccountType = AccountType.ACCOUNT
                            };
                            Context.Accounts.Add(accountdb);
                            Context.SaveChanges();
                        }
                        if (accountdb != null)
                        {
                            CompanySalesTaxAccountMap map = Context.CompanySalesTaxAccountMaps.FirstOrDefault(x => x.CompanyId == company.CompanyId && x.Name == "IGST");
                            if (map != null)
                            {    
                                map.CountrySaleTaxId = 1;
                                map.AccountId = accountdb.Id;
                                //sale purchase tax
                                UpdateTaxchangeInPurchaseAndSale(map);
                                //update company tax
                                Context.Entry(map).CurrentValues.SetValues(map);
                                Context.SaveChanges();
                            }
                        }
                        accountdb = null;
                        accountdb = Context.Accounts.FirstOrDefault(x => x.Name == "Central Sales Tax Payable" && x.CompanyId == company.CompanyId);
                        if (accountdb == null)
                        {
                            accountdb = new Account
                            {
                                Name = "Central Sales Tax Payable",
                                Discription = "Sales Tax Payable Account, where all sales tax payables goes into",
                                CompanyId = company.CompanyId,
                                BalanceAsOf = DateTime.Today,
                                AccountGroupId = 810,
                                AccountType = AccountType.ACCOUNT
                            };
                            Context.Accounts.Add(accountdb);
                            Context.SaveChanges();
                        }
                        if (accountdb != null)
                        {
                            CompanySalesTaxAccountMap map = Context.CompanySalesTaxAccountMaps.FirstOrDefault(x => x.CompanyId == company.CompanyId && x.Name == "CGST");
                            if (map != null)
                            {   
                                map.CountrySaleTaxId = 2;
                                map.AccountId = accountdb.Id;
                                //sale purchase tax
                                UpdateTaxchangeInPurchaseAndSale(map);
                                //update company tax
                                Context.Entry(map).CurrentValues.SetValues(map);
                                Context.SaveChanges();
                            }
                        }
                        accountdb = null;
                        accountdb = Context.Accounts.FirstOrDefault(x => x.Name == "State Sales Tax Payable" && x.CompanyId == company.CompanyId);
                        if (accountdb == null)
                        {
                            accountdb = new Account
                            {
                                Name = "State Sales Tax Payable",
                                Discription = "Sales Tax Payable Account, where all sales tax payables goes into",
                                CompanyId = company.CompanyId,
                                BalanceAsOf = DateTime.Today,
                                AccountGroupId = 810,
                                AccountType = AccountType.ACCOUNT
                            };
                            Context.Accounts.Add(accountdb);
                            Context.SaveChanges();
                        }
                        if (accountdb != null)
                        {
                            CompanySalesTaxAccountMap map = Context.CompanySalesTaxAccountMaps.FirstOrDefault(x => x.CompanyId == company.CompanyId && x.Name == "SGST");
                            if (map != null)
                            {
                                map.CountrySaleTaxId = 3;
                                map.AccountId = accountdb.Id;
                                //sale purchase tax
                                UpdateTaxchangeInPurchaseAndSale(map);
                                //update company tax
                                Context.Entry(map).CurrentValues.SetValues(map);
                                Context.SaveChanges();
                            }
                        }
                        accountdb = null;
                        accountdb = Context.Accounts.FirstOrDefault(x => x.Name == "Tax at Source Payable" && x.CompanyId == company.CompanyId);
                        if (accountdb == null)
                        {
                            accountdb = new Account
                            {
                                Name = "Tax at Source Payable",
                                Discription = "Sales Tax Payable Account, where all sales tax payables goes into",
                                CompanyId = company.CompanyId,
                                BalanceAsOf = DateTime.Today,
                                AccountGroupId = 810,
                                AccountType = AccountType.ACCOUNT
                            };
                            Context.Accounts.Add(accountdb);
                            Context.SaveChanges();
                        }
                        if (accountdb != null)
                        {
                            CompanySalesTaxAccountMap map = Context.CompanySalesTaxAccountMaps.FirstOrDefault(x => x.CompanyId == company.CompanyId && x.Name == "TCS");
                            if (map != null)
                            {
                                map.CountrySaleTaxId = 4;
                                map.AccountId = accountdb.Id;
                                //sale purchase tax
                                UpdateTaxchangeInPurchaseAndSale(map);
                                //update company tax
                                Context.Entry(map).CurrentValues.SetValues(map);
                                Context.SaveChanges();
                            }
                            else
                            {
                                CompanySalesTaxAccountMap CompanySalesTaxAccountMap = new CompanySalesTaxAccountMap
                                {
                                    Name = "TCS",
                                    CountrySaleTaxId = 4,
                                    AccountId = accountdb.Id,
                                    CompanyId = company.CompanyId
                                };
                                Context.CompanySalesTaxAccountMaps.Add(CompanySalesTaxAccountMap);
                                Context.SaveChanges();
                            }
                        }
                        
                    }
                }
            }
            Context.SeedDataHistories.Add(new SeedDataHistory() { Key = "CountrySaleTaxSeedUpdateForIndia", DateExecuted = new System.DateTime() });
            Context.SaveChanges();
        }

        private void UpdateTaxchangeInPurchaseAndSale(CompanySalesTaxAccountMap map)
        {
            //saledetailtax
            List<ItemLevelSaleTaxDetail> itemLevelSaleTaxDetails = Context.ItemLevelSaleTaxDetails.Include("SaleDetails").Where(x => x.TaxAccountId == map.AccountId).ToList();
            if (itemLevelSaleTaxDetails!=null && itemLevelSaleTaxDetails.Count > 0)
            {
                foreach (ItemLevelSaleTaxDetail taxDetail in itemLevelSaleTaxDetails)
                {
                    CatalogItemSalesTaxMap itemmap = Context.CatalogItemSalesTaxMaps.FirstOrDefault(x => x.CatalogItemId == taxDetail.SaleDetails.ProductId && x.SalesTaxMapId == map.MapId);
                    if (itemmap != null)
                    {
                        taxDetail.SaleDetails = null;
                        taxDetail.TaxAccountId = map.AccountId;
                        taxDetail.ItemTaxMapId = itemmap.Id;
                        Context.Entry(Context.ItemLevelSaleTaxDetails.Find(taxDetail.Id)).CurrentValues.SetValues(taxDetail);
                        Context.SaveChanges();
                    }
                }
            }
            //purchasedetailtax
            List<LineLevelPurchaseTaxDetail> lineLevelPurchaseTaxDetails = Context.LineLevelPurchaseTaxDetails.Include("PurchaseDetails").Where(x => x.TaxAccountId == map.AccountId).ToList();
            if (lineLevelPurchaseTaxDetails!=null && lineLevelPurchaseTaxDetails.Count > 0)
            {
                foreach (LineLevelPurchaseTaxDetail taxDetail in lineLevelPurchaseTaxDetails)
                {
                    CatalogItemSalesTaxMap itemmap = Context.CatalogItemSalesTaxMaps.FirstOrDefault(x => x.CatalogItemId == taxDetail.PurchaseDetails.ProductId && x.SalesTaxMapId == map.MapId);
                    if (itemmap != null)
                    {
                        taxDetail.PurchaseDetails = null;
                        taxDetail.TaxAccountId = map.AccountId;
                        taxDetail.ItemTaxMapId = itemmap.Id;
                        Context.Entry(Context.LineLevelPurchaseTaxDetails.Find(taxDetail.Id)).CurrentValues.SetValues(taxDetail);
                        Context.SaveChanges();
                    }
                }
            }
        }
        // Update Symptom Category
        public void UpdateSymptomWithSymptomCategory()
        {
            if (HasSeedRun("UpdateSymptomWithSymptomCategory"))
                return;

            var ListCompany = Context.Companies.ToList();
            if (ListCompany!=null && ListCompany.Count > 0)
            {
                foreach (var company in ListCompany)
                {
                    if (company != null && company.BusinessType==BuisnessType.Hospital)
                    {
                        IList<Symptom> SymptomInfo = Context.Symptoms.Where(c => c.CompanyId == company.CompanyId).ToList();
                        if (SymptomInfo!=null && SymptomInfo.Count > 0)
                        {
                            SymptomCategory SymptomCategoryModel = new SymptomCategory();
                            SymptomCategoryModel.CompanyId = company.CompanyId;
                            SymptomCategoryModel.Name = "General";
                            SymptomCategoryModel.Discription = "";
                            SymptomCategoryModel.DisplayAs = "General";
                            SymptomCategoryModel.IsSubSymptomCategory = false;
                            Context.SymptomCategorys.Add(SymptomCategoryModel);
                            Context.SaveChanges();

                            foreach (var symptom in SymptomInfo)
                            {
                                if (symptom.SymptomCategoryId == null)
                                {
                                    symptom.SymptomCategoryId = SymptomCategoryModel.Id;
                                    Context.Entry(Context.Symptoms.Find(symptom.Id)).CurrentValues.SetValues(symptom);
                                    Context.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }

            Context.SeedDataHistories.Add(new SeedDataHistory() { Key = "UpdateSymptomWithSymptomCategory", DateExecuted = new System.DateTime() });
            Context.SaveChanges();
        }

        // Update MedicalTest Category

        public void UpdateMedicalTestWithMedicalTestCategory()
        {
            if (HasSeedRun("UpdateMedicalTestWithMedicalTestCategory"))
                return;

            var ListCompany = Context.Companies.ToList();
            if(ListCompany.Count > 0 && ListCompany != null)
            {
                foreach(var company in ListCompany)
                {
                    if(company != null && company.BusinessType == BuisnessType.Hospital)
                    {
                        IList<MedicalTest> MedicalTestsInfo = Context.MedicalTests.Where(c => c.CompanyId == company.CompanyId).ToList();                      
                        if(MedicalTestsInfo.Count > 0 && MedicalTestsInfo != null)
                        {
                            MedicalTestCategory MedicalTestCategoryData = new MedicalTestCategory();
                            MedicalTestCategoryData.CompanyId = company.CompanyId;
                            MedicalTestCategoryData.Name = "General";
                            MedicalTestCategoryData.Description = "";
                            MedicalTestCategoryData.DisplayAs = "General";
                            MedicalTestCategoryData.IsSubMedicalTestCategory = false;
                            Context.MedicalTestCategorys.Add(MedicalTestCategoryData);
                            Context.SaveChanges();

                            foreach (var test in MedicalTestsInfo)
                            {
                                if (test.MedicalTestCategoryId == null)
                                {
                                    test.MedicalTestCategoryId = MedicalTestCategoryData.Id;
                                    Context.Entry(Context.MedicalTests.Find(test.Id)).CurrentValues.SetValues(test);
                                    Context.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }

            Context.SeedDataHistories.Add(new SeedDataHistory() { Key = "UpdateMedicalTestWithMedicalTestCategory", DateExecuted = new System.DateTime() });
            Context.SaveChanges();
        }

        // Update MedicalProcedure Category
        public void UpdateMedicalProcedureWithMedicalProcedureCategory()
        {
            if (HasSeedRun("UpdateMedicalProcedureWithMedicalProcedureCategory"))
                return;

            var ListCompany = Context.Companies.ToList();
            if (ListCompany.Count > 0 && ListCompany != null)
            {
                foreach (var company in ListCompany)
                {
                    if (company != null && company.BusinessType == BuisnessType.Hospital)
                    {
                        IList<MedicalProcedure> MedicalProceduresInfo = Context.MedicalProcedures.Where(c => c.CompanyId == company.CompanyId).ToList();
                        if (MedicalProceduresInfo.Count > 0 && MedicalProceduresInfo != null)
                        {
                            MedicalProcedureCategory MedicalProcedureCategoryData = new MedicalProcedureCategory();
                            MedicalProcedureCategoryData.CompanyId = company.CompanyId;
                            MedicalProcedureCategoryData.Name = "General";
                            MedicalProcedureCategoryData.Description = "";
                            MedicalProcedureCategoryData.DisplayAs = "General";
                            MedicalProcedureCategoryData.IsSubMedicalProcedureCategory = false;
                            Context.MedicalProcedureCategory.Add(MedicalProcedureCategoryData);
                            Context.SaveChanges();

                            foreach (var test in MedicalProceduresInfo)
                            {
                                if (test.MedicalProcedureCategoryId == null)
                                {
                                    test.MedicalProcedureCategoryId = MedicalProcedureCategoryData.Id;
                                    Context.Entry(Context.MedicalProcedures.Find(test.Id)).CurrentValues.SetValues(test);
                                    Context.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }

            Context.SeedDataHistories.Add(new SeedDataHistory() { Key = "UpdateMedicalProcedureWithMedicalProcedureCategory", DateExecuted = new System.DateTime() });
            Context.SaveChanges();
        }

        // Update for stock report
        public void UpdateForStockReport()
        {
            if (HasSeedRun("UpdateForStockReport"))
                return;
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
            Update(ConString, schemaName);
            Context.SeedDataHistories.Add(new SeedDataHistory() { Key = "UpdateForStockReport", DateExecuted = new System.DateTime() });
            Context.SaveChanges();
        }
        public void Update(string ConString, string schemaName)
        {                     
            bool HasProcedure = IsStoredProcedureExists("GetOpeningStockLastMonthQty", ConString, schemaName);
            if (!HasProcedure)
            {
                CreateStoredProcedure("GetOpeningStockLastMonthQty", ConString);
            }
            HasProcedure = IsStoredProcedureExists("GetOpeningStockByMovement", ConString, schemaName);
            if (!HasProcedure)
            {
                CreateStoredProcedure("GetOpeningStockByMovement", ConString);
            }
            HasProcedure = IsStoredProcedureExists("GetOpeningStockLastMonthQtyByBatch", ConString, schemaName);
            if (!HasProcedure)
            {
                CreateStoredProcedure("GetOpeningStockLastMonthQtyByBatch", ConString);
            }
            HasProcedure = IsStoredProcedureExists("GetOpeningStockByMovementByBatch", ConString, schemaName);
            if (!HasProcedure)
            {
                CreateStoredProcedure("GetOpeningStockByMovementByBatch", ConString);
            }
            HasProcedure = IsStoredProcedureExists("GetCatalogItemsWithSalesTax", ConString, schemaName);
            if (!HasProcedure)
            {
                CreateStoredProcedure("GetCatalogItemsWithSalesTax", ConString);
            }
        }
        public bool IsStoredProcedureExists(string storedProcedureName, string ConString, string schemaName)
        {
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
        public void UpdateCorrectionForStockReport()
        {
            if (HasSeedRun("UpdateCorrectionForStockReport"))
                return;
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
            dropProcedure(ConString, "GetOpeningStockLastMonthQty");
            dropProcedure(ConString, "GetOpeningStockLastMonthQtyByBatch");
            dropProcedure(ConString, "GetOpeningStockByMovement");
            dropProcedure(ConString, "GetOpeningStockByMovementByBatch");
            Update(ConString, schemaName);
            Context.SeedDataHistories.Add(new SeedDataHistory() { Key = "UpdateCorrectionForStockReport", DateExecuted = new System.DateTime() });
            Context.SaveChanges();
        }
        public void CreateDailyRoomRentProcedure()
        {
            if (HasSeedRun("CreateDailyRoomRentProcedure"))
                return;
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
            dropProcedure(ConString, "CreateDailyRoomRentProcedure");

            bool HasProcedure = IsStoredProcedureExists("CreateDailyRoomRentProcedure", ConString, schemaName);
            if (!HasProcedure)
            {
                //GeneratePatientRoomRent("CreateDailyRoomRentProcedure", ConString);
            }
        }
        public void UpdateDailyRoomRentProcedure()
        {
            if (HasSeedRun("UpdateDailyRoomRentProcedure"))
                return;
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
            dropProcedure(ConString, "CreateDailyRoomRentProcedure");

            bool HasProcedure = IsStoredProcedureExists("UpdateDailyRoomRentProcedure", ConString, schemaName);
            if (!HasProcedure)
            {
                //GeneratePatientRoomRent("UpdateDailyRoomRentProcedure", ConString);
            }
            Context.SeedDataHistories.Add(new SeedDataHistory() { Key = "UpdateDailyRoomRentProcedure", DateExecuted = new System.DateTime() });
            Context.SaveChanges();
        }
        
        public void GeneratePatientRoomRent(string Name, string ConString)
        {
            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();
                string createProcedureScript = string.Empty;
                if (Name == "CreateDailyRoomRentProcedure" || Name == "UpdateDailyRoomRentProcedure")
                {
                    createProcedureScript = @"
                    CREATE PROCEDURE " + Name + @"()
                    BEGIN
                        -- Declare all variables
                        DECLARE done INT DEFAULT 0;
                        DECLARE admissionId BIGINT;
                        DECLARE patientId BIGINT;
                        DECLARE DateOfAdmission DATETIME;
                        DECLARE companyId BIGINT;
                        DECLARE OpRegistrationId BIGINT;
                        DECLARE startDate DATETIME;
                        DECLARE endDate DATETIME;
                        DECLARE nextDay DATETIME;
                        DECLARE ldays INT;
                        DECLARE lhours INT;
                        DECLARE amount DOUBLE DEFAULT 0;
                        DECLARE rentAmount DOUBLE;
                        DECLARE rentPeriod INT;
                        DECLARE lastRentDate DATETIME;    
                        DECLARE bedId BIGINT;
                        DECLARE WardId BIGINT;
                        DECLARE bedTypeId BIGINT;
                        DECLARE span_seconds BIGINT;
                        DECLARE BedName varchar(20);
                        DECLARE WardName varchar(20);
                        DECLARE timeDescription TEXT DEFAULT '';

                        -- Declare cursors
                        DECLARE curAdmissions CURSOR FOR 
                            SELECT a.Id, a.PatientId, a.DateOfAdmission, a.CompanyId, a.OpRegistrationId
                            FROM InPatientAdmissions a
                            WHERE a.Status != '2';
                        DECLARE curLocations CURSOR FOR
                            SELECT l.BedId, l.AdmissionId
                            FROM InPatientLocations l
                            WHERE l.AdmissionId = admissionId AND Active = '1'
                            AND (l.DateMovedOut = '0001-01-01 00:00:00.000000' OR l.DateMovedOut >= startDate);
                        DECLARE curRents CURSOR FOR
                            SELECT r.Amount, r.RentPeriod
                            FROM Rents r
                            WHERE r.BedTypeId = (SELECT BedTypeId FROM Beds WHERE Id = bedId)
                            ORDER BY FIELD(r.RentPeriod, '0', '2', '1', '4', '5', '3');

                        -- Declare a CONTINUE handler for NOT FOUND
                        DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = 1;

                        -- Initialize endDate
                        SET endDate = NOW();
                        SET endDate = TIMESTAMP(DATE(endDate), '00:00:00');

                        -- Open curAdmissions cursor
                        OPEN curAdmissions;
                        read_loop: LOOP

                            -- Reset done flag before each fetch
                            SET done = 0;
                            FETCH curAdmissions INTO admissionId, patientId, DateOfAdmission, companyId, OpRegistrationId;
                            IF done THEN
                                LEAVE read_loop;
                            END IF;

                            -- Determine start date for rent calculation
                            SELECT MAX(rr.DatePosted) INTO lastRentDate
                            FROM PatientRoomRents rr
                            WHERE rr.InPatientAdmissionId = admissionId;
                            IF lastRentDate IS NOT NULL THEN
                                SET startDate = lastRentDate;
                            ELSE
                                SET startDate = DateOfAdmission;
                            END IF;
                            IF startDate >= endDate THEN
                                ITERATE read_loop;
                            END IF;

                            -- Process room rents until endDate
                            WHILE startDate < endDate DO
                                SET nextDay = TIMESTAMP(DATE_ADD(DATE(startDate), INTERVAL 1 DAY), '00:00:00');
                                IF nextDay > endDate THEN
                                    SET nextDay = endDate;
                                    SET done = 1;
                                END IF;

                                -- Process locations for the current admission
                                OPEN curLocations;
                                location_loop: LOOP

                                    -- Reset done flag before each fetch
                                    SET done = 0;
                                    FETCH curLocations INTO bedId, admissionId;
                                    IF done THEN
                                        LEAVE location_loop;
                                    END IF;

                                    -- Calculate rental duration
                                    SET span_seconds = TIMESTAMPDIFF(SECOND, startDate, nextDay);
                                    SET ldays = FLOOR(span_seconds / 86400);
                                    SET lhours = FLOOR((span_seconds % 86400) / 3600);
                                    SET lhours = lhours + CASE WHEN (span_seconds % 3600) / 60 >= 30 THEN 1 ELSE 0 END;
                                    IF lhours > 12 THEN
                                        SET ldays = ldays + 1;
                                        SET lhours = 0;
                                    ELSEIF lhours > 4 THEN
                                        SET lhours = 12;
                                    ELSEIF lhours > 1 THEN
                                        SET lhours = 4;
                                    END IF;
                                    IF ldays > 0 THEN
                                        IF ldays = 1 THEN
				                            SET timeDescription = CONCAT(' (', ldays, ' day)');
                                        ELSE
				                            SET timeDescription = CONCAT(' (', ldays, ' days)');
                                        END IF;
                                    END IF;

                                    -- Check if lhours > 0 and construct the hour part of the description
                                    IF lhours > 0 THEN
                                        IF timeDescription != '' THEN
				                            SET timeDescription = CONCAT(timeDescription, ' and ');
                                        END IF;
                                        IF lhours = 1 THEN
				                            SET timeDescription = CONCAT(timeDescription, ' (', lhours, ' hour)');
                                        ELSE
				                            SET timeDescription = CONCAT(timeDescription, ' (', lhours, ' hours)');
                                        END IF;
                                    END IF;

                                    -- Fetch bed type for rent calculation
                                    SELECT b.BedTypeId, b.Name, b.WardId INTO bedTypeId, BedName, WardId
                                    FROM Beds b
                                    WHERE b.Id = bedId;
                                    SELECT w.Name INTO WardName
                                    FROM Wards w 
                                    WHERE w.Id = WardId;

                                    -- Calculate rent based on rent period
                                    OPEN curRents;
                                    rent_loop: LOOP
                                        -- Reset done flag before each fetch
                                        SET done = 0;
                                        FETCH curRents INTO rentAmount, rentPeriod;
                                        IF done THEN
                                            LEAVE rent_loop;
                                        END IF;
                                        CASE rentPeriod
                                            WHEN '0' THEN -- Monthly
                                                IF ldays >= 30 THEN
                                                    SET amount = amount + rentAmount * (ldays / 30);
                                                    SET ldays = MOD(ldays, 30);
                                                END IF;
                                            WHEN '2' THEN -- Weekly
                                                IF ldays >= 7 THEN
                                                    SET amount = amount + rentAmount * (ldays / 7);
                                                    SET ldays = MOD(ldays, 7);
                                                END IF;
                                            WHEN '1' THEN -- Daily
                                                IF ldays = 1 THEN
                                                    SET amount = amount + rentAmount * ldays;
                                                    SET ldays = 0;
                                                END IF;
                                            WHEN '4' THEN -- Half-Day (12 hours)
                                                IF lhours = 12 THEN
                                                    SET amount = amount + rentAmount * (lhours / 12);
                                                    SET lhours = 0;
                                                END IF;
                                            WHEN '5' THEN -- 4-Hour Blocks
                                                IF lhours = 4 THEN
                                                    SET amount = amount + rentAmount * (lhours / 4);
                                                    SET lhours = 0;
                                                END IF;
                                            WHEN '3' THEN -- Hourly
                                                IF lhours = 1 THEN
                                                    SET amount = amount + rentAmount * lhours;
                                                    SET lhours = 0;
                                                END IF;
                                        END CASE;
                                    END LOOP rent_loop;
                                    CLOSE curRents;

                                    IF ldays > 0 OR lhours > 0 THEN

                                    -- Fetch the last rent entry
                                    SELECT r.Amount, r.RentPeriod INTO rentAmount, rentPeriod 
                                    FROM Rents r 
                                    WHERE r.BedTypeId = (SELECT BedTypeId FROM Beds WHERE Id = bedId)
                                    ORDER BY FIELD(r.RentPeriod, '0', '2', '1', '4', '5', '3') DESC LIMIT 1;

                                    -- Convert days to hours if ldays > 0
                                    IF ldays > 0 THEN
                                        SET lhours = lhours + (ldays * 24);
                                        SET ldays = 0;
                                    END IF;

                                    -- Process remaining hours based on RentPeriod
                                    IF lhours > 0 THEN
                                        CASE rentPeriod
				                            WHEN '4' THEN -- TWELVEHOURS
					                            SET amount = amount + (rentAmount * (lhours / 12));
					                            SET lhours = 0;
				                            WHEN '5' THEN -- FOURHOURS
					                            SET amount = amount + (rentAmount * (lhours / 4));
					                            SET lhours = 0;
				                            WHEN '3' THEN -- HOURLY
					                            SET amount = amount + (rentAmount * lhours);
					                            SET lhours = 0;
				                            WHEN '1' THEN -- DAILY
					                            SET amount = amount + (rentAmount * (lhours / 24));
					                            SET lhours = 0;
				                            WHEN '2' THEN -- WEEKLY (7 * 24)
					                            SET amount = amount + (rentAmount * ((lhours / 24) / 7));
					                            SET lhours = 0;
				                            WHEN '0' THEN -- MONTHLY (30 * 24)
					                            SET amount = amount + (rentAmount * ((lhours / 24) / 30));
					                            SET lhours = 0;
                                        END CASE;
                                        END IF;
                                    END IF;
                                    IF amount > 0 THEN

                                        -- Insert calculated rent into PatientRoomRents
                                        INSERT INTO PatientRoomRents
                                        (DatePosted, DateOfRental, PatientId, InPatientAdmissionId, Description, Amount, Bed, Ward)
                                        VALUES (TIMESTAMP(DATE(NOW()), '00:00:00'), DATE(startDate), patientId, admissionId, CONCAT('Room rent for ', startDate, timeDescription), amount, BedName, WardName);

                                        -- Insert calculated rent into PatientLedgers
                                        INSERT INTO PatientLedgers
                                        (Date, PatientId, InPatientAdmissionId, OpRegistrationId, Amount, Description, CompanyId, Type, Invoiced, InvoiceAmount)
                                        VALUES (TIMESTAMP(DATE(NOW()), '00:00:00'), patientId, admissionId, OpRegistrationId, amount, CONCAT('Room rent for ', startDate, timeDescription), companyId, '5', '0', 0);
                                    END IF;

                                    -- Update for the next iteration
                                    SET amount = 0;
                                    SET timeDescription = '';
                                    SET startDate = nextDay;
                                END LOOP location_loop;
                                CLOSE curLocations;
                            END WHILE;
                        END LOOP read_loop;
                        CLOSE curAdmissions;
                    END";
                }
                using (MySqlCommand cmd = new MySqlCommand(createProcedureScript, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void StockReportCorectionWithDecimals()
        {
            if (HasSeedRun("StockReportCorectionWithDecimals"))
                return;
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
            dropProcedure(ConString, "GetOpeningStockLastMonthQty");
            dropProcedure(ConString, "GetOpeningStockLastMonthQtyByBatch");
            dropProcedure(ConString, "GetOpeningStockByMovement");
            dropProcedure(ConString, "GetOpeningStockByMovementByBatch");
            Update(ConString, schemaName);
            Context.SeedDataHistories.Add(new SeedDataHistory() { Key = "StockReportCorectionWithDecimals", DateExecuted = new System.DateTime() });
            Context.SaveChanges();
        }
        private void dropProcedure(string ConString,string storedProcedureName)
        {
            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("DROP PROCEDURE if exists "+ storedProcedureName, connection);
                command.ExecuteReader();
            }
        }
        public void CreateStoredProcedure(string Name, string ConString)
        {
            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();
                string createProcedureScript = string.Empty;
                if (Name == "GetOpeningStockLastMonthQty" || Name == "GetOpeningStockLastMonthQtyByBatch")
                {
                    string BatchFlag = Name == "GetOpeningStockLastMonthQtyByBatch" ? " && sd.BatchNo=BatNo" : "";
                    string BatchParam = Name == "GetOpeningStockLastMonthQtyByBatch" ? ", in BatNo VARCHAR(40)" : "";

                    createProcedureScript = @"
                    CREATE PROCEDURE " + Name + @"(in Uom VARCHAR(40), in PId INT,in ComId INT,in LoId INT,in FDate DATE,in PMDate DATE,in SType INT" + BatchParam + @")
                    BEGIN
                    DECLARE Ostock double;
                    DECLARE lOstock double;
                    set Ostock=(select(
                    (select COALESCE(SUM(sd.Quantity+sd.FreeQuantity),0) as OpeningStock from stockmovementdetails sd 
                    where sd.ProductId=PId " + BatchFlag + @" && sd.Uom=Uom && sd.StockDate<=FDate
                    && exists (select * from stockmovements where Id=sd.StockMovementId && CompanyId=ComId && InventoryStockLocationId=LoId && SType=0))+

                    (select COALESCE(SUM(sd.Quantity+sd.FreeQuantity),0) as OpeningStock from stockmovementdetails sd 
                    where sd.ProductId=PId " + BatchFlag + @" && sd.Uom=Uom 
                    && exists (select * from stockmovements where Id=sd.StockMovementId && CompanyId=ComId && InventoryStockLocationId=LoId && MovementDate<=FDate && (SType=1 || SType=5 || SType=8 || SType=9)))+

                    (select COALESCE(SUM(sd.Quantity+sd.FreeQuantity),0) / (select p.RetailXFactor from catalogitems p where p.id=PId) as OpeningStock from stockmovementdetails sd 
                    where sd.ProductId=PId " + BatchFlag + @" && sd.Uom!=Uom && sd.StockDate<=FDate 
                    && exists (select * from stockmovements where Id=sd.StockMovementId && CompanyId=ComId && InventoryStockLocationId=LoId && SType=0))+

                    (select COALESCE(SUM(sd.Quantity+sd.FreeQuantity),0) / (select p.RetailXFactor from catalogitems p where p.id=PId) as OpeningStock from stockmovementdetails sd 
                    where sd.ProductId=PId " + BatchFlag + @" && sd.Uom!=Uom 
                    && exists (select * from stockmovements where Id=sd.StockMovementId && CompanyId=ComId && InventoryStockLocationId=LoId && MovementDate<=FDate && (SType=1 || SType=5 || SType=8 || SType=9)))-

                    (select COALESCE(SUM(sd.Quantity+sd.FreeQuantity),0) as OpeningStock from stockmovementdetails sd 
                    where sd.ProductId=PId " + BatchFlag + @" && sd.Uom=Uom && exists (select * from stockmovements where Id=sd.StockMovementId && CompanyId=ComId && InventoryStockLocationId=LoId 
                    && MovementDate<=FDate && (SType=2 || SType=6 || SType=7 || SType=3 || SType=4)))-

                    (select COALESCE(SUM(sd.Quantity+sd.FreeQuantity),0) / (select p.RetailXFactor from catalogitems p where p.id=PId) as OpeningStock from stockmovementdetails sd 
                    where sd.ProductId=PId " + BatchFlag + @" && sd.Uom!=Uom && exists (select * from stockmovements where Id=sd.StockMovementId && CompanyId=ComId && InventoryStockLocationId=LoId 
                    && MovementDate<=FDate && (SType=2 || SType=6 || SType=7 || SType=3 || SType=4)))) as Ostock);

                    set lOstock=(SELECT(
                    (select COALESCE(SUM(sd.Quantity+sd.FreeQuantity),0) as LastMonthSaleQty from stockmovementdetails sd 
                    where sd.ProductId=PId " + BatchFlag + @" && sd.Uom=Uom && 
                    exists (select * from stockmovements where Id=sd.StockMovementId && CompanyId=ComId && InventoryStockLocationId=LoId 
                    && date(MovementDate)>=PMDate && date(MovementDate)<=FDate && Type=6))+ 

                    (select COALESCE(SUM(sd.Quantity+sd.FreeQuantity),0) / (select p.RetailXFactor from catalogitems p where p.id=PId) as LastMonthSaleQty from stockmovementdetails sd 
                    where sd.ProductId=PId " + BatchFlag + @" && sd.Uom!=Uom && 
                    exists (select * from stockmovements where Id=sd.StockMovementId && CompanyId=ComId && InventoryStockLocationId=LoId && date(MovementDate)>=PMDate && date(MovementDate)<=FDate && Type=6))) as lOstock);
                    SELECT Ostock, lOstock;
                    END";
                }
                else if (Name == "GetOpeningStockByMovement" || Name == "GetOpeningStockByMovementByBatch")
                {
                    string BatchFlag = Name == "GetOpeningStockByMovementByBatch" ? " && sd.BatchNo=BatNo" : "";
                    string BatchParam = Name == "GetOpeningStockByMovementByBatch" ? ", in BatNo VARCHAR(40)" : "";

                    createProcedureScript = @"
                    CREATE PROCEDURE " + Name + @"(in Uom VARCHAR(40), in PId INT,in ComId INT,in LoId INT,in FDate DATE,in PMDate DATE,IN SType INT" + BatchParam + @")
                    BEGIN
                    DECLARE Movstock double;
                    set Movstock=(select(
                    (select COALESCE(SUM(sd.Quantity+sd.FreeQuantity),0) as Stock from stockmovementdetails sd 
                    where sd.ProductId = PId" + BatchFlag + @" && sd.Uom = Uom && 
                    exists (select *from stockmovements where Id = sd.StockMovementId && CompanyId = ComId && InventoryStockLocationId = LoId && date(MovementDate) <= PMDate && Type = SType))+

                    (select COALESCE(SUM(sd.Quantity+sd.FreeQuantity),0) / (select p.RetailXFactor from catalogitems p where p.id=PId) as Stock from stockmovementdetails sd 
                    where sd.ProductId=PId" + BatchFlag + @" && sd.Uom!=Uom && 
                    exists (select * from stockmovements where Id=sd.StockMovementId && CompanyId=ComId && InventoryStockLocationId=LoId && date(MovementDate)<=PMDate && Type=SType))) as Movstock);
                    SELECT Movstock;
                    END";
                }
                else if (Name == "GetCatalogItemsWithSalesTax")
                {
                     createProcedureScript = @"
                        CREATE PROCEDURE GetCatalogItemsWithSalesTax(IN p_CompanyIds INT)
                        BEGIN
                        CREATE TEMPORARY TABLE IF NOT EXISTS CatalogDetailsTemp (
                            CategoryName VARCHAR(255),
                            SubcategoryName VARCHAR(255),
                            ProductName VARCHAR(255),
                            MaterialId VARCHAR(255), 
                            ParentId INT,
                            Description VARCHAR(255),
                            HSNCode VARCHAR(255),
                            UOM VARCHAR(255),
                            RetailUOM VARCHAR(255),
                            RetailXFactor DECIMAL(10,2),
                            WholesaleUOM VARCHAR(255),
                            WholesaleXFactor DECIMAL(10,2),
                            PurchasePrice DECIMAL(10,2),
                            RetailPrice DECIMAL(10,2),
                            WholdSalePrice DECIMAL(10,2),
                            MSRP DECIMAL(10,2),
                            CostPrice DECIMAL(10,2),
                            IsInventoryAtBatch BOOLEAN,
                            UseHsnTax BOOLEAN,
                            Manufacturer VARCHAR(255),
                            DefaultDiscountLocal DECIMAL(5,2),
                            IGST DECIMAL(5,2),
                            CGST DECIMAL(5,2),
                            SGST DECIMAL(5,2),
                            TCS DECIMAL(5,2),
                            SalesAccount VARCHAR(255),
                            PurchaseAccount VARCHAR(255),
                            InventoryAccount VARCHAR(255),
                            Supplier VARCHAR(255),
                            ItemTaxEffectiveFromDate DATE,
                            ItemTaxEffectiveToDate DATE
                        );

                        INSERT INTO CatalogDetailsTemp (
                            CategoryName,
                            SubcategoryName,
                            ProductName,
                            MaterialId,
                            ParentId,
                            Description,
                            HSNCode,
                            UOM,
                            RetailUOM,
                            RetailXFactor,
                            WholesaleUOM,
                            WholesaleXFactor,
                            PurchasePrice,
                            RetailPrice,
                            WholdSalePrice,
                            MSRP,
                            CostPrice,
                            IsInventoryAtBatch,
                            UseHsnTax,
                            Manufacturer,
                            DefaultDiscountLocal,
                            IGST,
                            CGST,
                            SGST,
                            TCS,
                            SalesAccount,
                            PurchaseAccount,
                            InventoryAccount,
                            Supplier,
                            ItemTaxEffectiveFromDate,
                            ItemTaxEffectiveToDate
                        )
                        SELECT
                            cat1.Name AS CategoryName,
                            cat2.Name AS SubcategoryName,
                            cat3.Name AS ProductName,
                            cat3.MaterialId AS MaterialId,
                            cat3.ParentId AS ParentId,
                            cat3.Description,
                            cat3.HSNCode,
                            cat3.UOM,
                            cat3.RetailUOM,
                            cat3.RetailXFactor,
                            cat3.WholesaleUOM,
                            cat3.WholesaleXFactor,
                            cat3.PurchasePrice,
                            cat3.RetailPrice,
                            cat3.WholdSalePrice,
                            cat3.MSRP,
                            cat3.CostPrice,
                            cat3._isInventoryAtBatch,
                            cat3.UseHsnTax,
                            cat3.Manufacturer,
                            cat3.DefaultDiscountLocal,
                            MAX(CASE WHEN cstam.Name = 'IGST' THEN stam.TaxPercentage END) AS IGST,
                            MAX(CASE WHEN cstam.Name = 'CGST' THEN stam.TaxPercentage END) AS CGST,
                            MAX(CASE WHEN cstam.Name = 'SGST' THEN stam.TaxPercentage END) AS SGST,
                            MAX(CASE WHEN cstam.Name = 'TCS' THEN stam.TaxPercentage END) AS TCS,
                            SalesAccount.Name AS SalesAccount,
                            PurchaseAccount.Name AS PurchaseAccount,
                            InventoryAccount.Name AS InventoryAccount,
                            Supplier.Name AS Supplier,
                            MAX(stam.EffectiveFrom) AS ItemTaxEffectiveFromDate,
                            MAX(stam.EffectiveTo) AS ItemTaxEffectiveToDate
                        FROM catalogitems cat1
                        LEFT JOIN catalogitems cat2 ON cat1.Id = cat2.ParentId AND cat2.Type = 2
                        LEFT JOIN catalogitems cat3 ON cat2.Id = cat3.ParentId AND cat3.Type = 3
                        LEFT JOIN catalogitemsalestaxmaps stam ON cat3.Id = stam.CatalogItemId
                        LEFT JOIN companysalestaxaccountmaps cstam ON stam.SalesTaxMapId = cstam.MapId
                        LEFT JOIN accounts AS SalesAccount ON cat3.SalesAccountLocalId = SalesAccount.Id
                        LEFT JOIN accounts AS PurchaseAccount ON cat3.PurchaseAccountLocalId = PurchaseAccount.Id
                        LEFT JOIN accounts AS InventoryAccount ON cat3.InventoryAccountLocalId = InventoryAccount.Id
                        LEFT JOIN accounts AS Supplier ON cat3.SupplierId = Supplier.Id
                        WHERE cat1.Type = 1 AND cat1.CompanyId = p_CompanyIds
                        GROUP BY
                            cat1.Name,
                            cat2.Name,
                            cat3.Name,
                            cat3.MaterialId, 
                            cat3.ParentId,
                            cat3.Description,
                            cat3.HSNCode,
                            cat3.UOM,
                            cat3.RetailUOM,
                            cat3.RetailXFactor,
                            cat3.WholesaleUOM,
                            cat3.WholesaleXFactor,
                            cat3.PurchasePrice,
                            cat3.RetailPrice,
                            cat3.WholdSalePrice,
                            cat3.MSRP,
                            cat3.CostPrice,
                            cat3._isInventoryAtBatch,
                            cat3.UseHsnTax,
                            cat3.Manufacturer,
                            cat3.DefaultDiscountLocal,
                            SalesAccount.Name,
                            PurchaseAccount.Name,
                            InventoryAccount.Name,
                            Supplier.Name;
                        SELECT * FROM CatalogDetailsTemp  c where MaterialId is not null;
                        DROP TEMPORARY TABLE IF EXISTS CatalogDetailsTemp;
                    END";
                }
                using (MySqlCommand cmd = new MySqlCommand(createProcedureScript, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public DataTable ExecuteStoredProcedure(string Name, string Uom, long PId, long CompanyId, long LocationId, DateTime FDate, DateTime PMDate, int Type, string BatNo)
        {
            DataTable dataTable = new DataTable();
            AccountMasterContext Context = new AccountMasterContext();
            string ConString = Context.Database.GetDbConnection().ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(Name, connection))
                {
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    adapter.SelectCommand.Parameters.AddWithValue("@Uom", Uom);
                    adapter.SelectCommand.Parameters.AddWithValue("@PId", PId);
                    adapter.SelectCommand.Parameters.AddWithValue("@ComId", CompanyId);
                    adapter.SelectCommand.Parameters.AddWithValue("@LoId", LocationId);
                    adapter.SelectCommand.Parameters.AddWithValue("@FDate", FDate);
                    adapter.SelectCommand.Parameters.AddWithValue("@PMDate", PMDate);
                    if (Name == "GetOpeningStockLastMonthQtyByBatch")
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@BatNo", BatNo);
                    }
                    else if (Name == "GetOpeningStockByMovement")
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@SType", Type);
                    }
                    else if (Name == "GetOpeningStockByMovementByBatch")
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@SType", Type);
                        adapter.SelectCommand.Parameters.AddWithValue("@BatNo", BatNo);
                    }
                    adapter.Fill(dataTable);
                }
            }
            Console.WriteLine("Execution completed successfully.");
            return dataTable;
        }
        public void DailyRoomRentProcedure()
        {
            if (HasSeedRun("DailyRoomRentProcedure"))
                return;
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
            dropProcedure(ConString, "UpdateDailyRoomRentProcedure");

            bool HasProcedure = IsStoredProcedureExists("DailyRoomRentProcedure", ConString, schemaName);
            if (!HasProcedure)
            {
                GeneratePatientRoomRentProcedure("DailyRoomRentProcedure", ConString);
            }
            Context.SeedDataHistories.Add(new SeedDataHistory() { Key = "DailyRoomRentProcedure", DateExecuted = new System.DateTime() });
            Context.SaveChanges();
        }
        public void GeneratePatientRoomRentProcedure(string Name, string ConString)
        {
            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();
                string createProcedureScript = string.Empty;
                if (Name == "DailyRoomRentProcedure")
                {
                    createProcedureScript = @"
                    CREATE PROCEDURE " + Name + @"()
                    BEGIN
                        -- Declare all variables
                        DECLARE done INT DEFAULT 0;
                        DECLARE admissionId BIGINT;
                        DECLARE patientId BIGINT;
                        DECLARE DateOfAdmission DATETIME;
                        DECLARE companyId BIGINT;
                        DECLARE OpRegistrationId BIGINT;
                        DECLARE startDate DATETIME;
                        DECLARE endDate DATETIME;
                        DECLARE nextDay DATETIME;
                        DECLARE ldays INT;
                        DECLARE lhours INT;
                        DECLARE amount DOUBLE DEFAULT 0;
                        DECLARE rentAmount DOUBLE;
                        DECLARE rentPeriod INT;
                        DECLARE lastRentDate DATETIME;    
                        DECLARE bedId BIGINT;
                        DECLARE WardId BIGINT;
                        DECLARE bedTypeId BIGINT;
                        DECLARE span_seconds BIGINT;
                        DECLARE BedName varchar(20);
                        DECLARE WardName varchar(20);
                        DECLARE timeDescription TEXT DEFAULT '';
                        DECLARE timeDuration TEXT DEFAULT '';

                        -- Declare cursors
                        DECLARE curAdmissions CURSOR FOR 
                            SELECT a.Id, a.PatientId, a.DateOfAdmission, a.CompanyId, a.OpRegistrationId
                            FROM InPatientAdmissions a
                            WHERE a.Status != '2';
                        DECLARE curLocations CURSOR FOR
                            SELECT l.BedId, l.AdmissionId
                            FROM InPatientLocations l
                            WHERE l.AdmissionId = admissionId AND Active = '1'
                            AND (l.DateMovedOut = '0001-01-01 00:00:00.000000' OR l.DateMovedOut >= startDate);
                        DECLARE curRents CURSOR FOR
                            SELECT r.Amount, r.RentPeriod
                            FROM Rents r
                            WHERE r.BedTypeId = (SELECT BedTypeId FROM Beds WHERE Id = bedId)
                            ORDER BY FIELD(r.RentPeriod, '0', '2', '1', '4', '5', '3');

                        -- Declare a CONTINUE handler for NOT FOUND
                        DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = 1;

                        -- Initialize endDate
                        SET endDate = NOW();
                        SET endDate = TIMESTAMP(DATE(endDate), '00:00:00');

                        -- Open curAdmissions cursor
                        OPEN curAdmissions;
                        read_loop: LOOP

                            -- Reset done flag before each fetch
                            SET done = 0;
                            FETCH curAdmissions INTO admissionId, patientId, DateOfAdmission, companyId, OpRegistrationId;
                            IF done THEN
                                LEAVE read_loop;
                            END IF;

                            -- Determine start date for rent calculation
                            SELECT MAX(rr.DatePosted) INTO lastRentDate
                            FROM PatientRoomRents rr
                            WHERE rr.InPatientAdmissionId = admissionId;
                            IF lastRentDate IS NOT NULL THEN
                                SET startDate = lastRentDate;
                            ELSE
                                SET startDate = DateOfAdmission;
                            END IF;
                            IF startDate >= endDate THEN
                                ITERATE read_loop;
                            END IF;

                            -- Process room rents until endDate
                            WHILE startDate < endDate DO
                                SET nextDay = TIMESTAMP(DATE_ADD(DATE(startDate), INTERVAL 1 DAY), '00:00:00');
                                IF nextDay > endDate THEN
                                    SET nextDay = endDate;
                                    SET done = 1;
                                END IF;

                                -- Process locations for the current admission
                                OPEN curLocations;
                                location_loop: LOOP

                                    -- Reset done flag before each fetch
                                    SET done = 0;
                                    FETCH curLocations INTO bedId, admissionId;
                                    IF done THEN
                                        LEAVE location_loop;
                                    END IF;

                                    -- Calculate rental duration
                                    SET span_seconds = TIMESTAMPDIFF(SECOND, startDate, nextDay);
                                    SET ldays = FLOOR(span_seconds / 86400);
                                    SET lhours = FLOOR((span_seconds % 86400) / 3600);
                                    SET lhours = lhours + CASE WHEN (span_seconds % 3600) / 60 >= 30 THEN 1 ELSE 0 END;
                                    IF lhours > 12 THEN
                                        SET ldays = ldays + 1;
                                        SET lhours = 0;
                                    ELSEIF lhours > 4 THEN
                                        SET lhours = 12;
                                    ELSEIF lhours > 1 THEN
                                        SET lhours = 4;
                                    END IF;
                                    IF ldays > 0 THEN
                                        IF ldays = 1 THEN
				                            SET timeDescription = CONCAT(' (', ldays, ' day)');
                                            SET timeDuration = CONCAT(ldays, ' day');
                                        ELSE
				                            SET timeDescription = CONCAT(' (', ldays, ' days)');
                                            SET timeDuration = CONCAT(ldays, ' day');
                                        END IF;
                                    END IF;

                                    -- Check if lhours > 0 and construct the hour part of the description
                                    IF lhours > 0 THEN
                                        IF timeDescription != '' THEN
				                            SET timeDescription = CONCAT(timeDescription, ' and ');
                                        END IF;
                                        IF lhours = 1 THEN
				                            SET timeDescription = CONCAT(timeDescription, ' (', lhours, ' hour)');
                                            SET timeDuration = CONCAT(lhours, ' hour');
                                        ELSE
				                            SET timeDescription = CONCAT(timeDescription, ' (', lhours, ' hours)');
                                            SET timeDuration = CONCAT(lhours, ' hour');
                                        END IF;
                                    END IF;

                                    -- Fetch bed type for rent calculation
                                    SELECT b.BedTypeId, b.Name, b.WardId INTO bedTypeId, BedName, WardId
                                    FROM Beds b
                                    WHERE b.Id = bedId;
                                    SELECT w.Name INTO WardName
                                    FROM Wards w 
                                    WHERE w.Id = WardId;

                                    -- Calculate rent based on rent period
                                    OPEN curRents;
                                    rent_loop: LOOP
                                        -- Reset done flag before each fetch
                                        SET done = 0;
                                        FETCH curRents INTO rentAmount, rentPeriod;
                                        IF done THEN
                                            LEAVE rent_loop;
                                        END IF;
                                        CASE rentPeriod
                                            WHEN '0' THEN -- Monthly
                                                IF ldays >= 30 THEN
                                                    SET amount = amount + rentAmount * (ldays / 30);
                                                    SET ldays = MOD(ldays, 30);
                                                END IF;
                                            WHEN '2' THEN -- Weekly
                                                IF ldays >= 7 THEN
                                                    SET amount = amount + rentAmount * (ldays / 7);
                                                    SET ldays = MOD(ldays, 7);
                                                END IF;
                                            WHEN '1' THEN -- Daily
                                                IF ldays = 1 THEN
                                                    SET amount = amount + rentAmount * ldays;
                                                    SET ldays = 0;
                                                END IF;
                                            WHEN '4' THEN -- Half-Day (12 hours)
                                                IF lhours = 12 THEN
                                                    SET amount = amount + rentAmount * (lhours / 12);
                                                    SET lhours = 0;
                                                END IF;
                                            WHEN '5' THEN -- 4-Hour Blocks
                                                IF lhours = 4 THEN
                                                    SET amount = amount + rentAmount * (lhours / 4);
                                                    SET lhours = 0;
                                                END IF;
                                            WHEN '3' THEN -- Hourly
                                                IF lhours = 1 THEN
                                                    SET amount = amount + rentAmount * lhours;
                                                    SET lhours = 0;
                                                END IF;
                                        END CASE;
                                    END LOOP rent_loop;
                                    CLOSE curRents;

                                    IF ldays > 0 OR lhours > 0 THEN

                                    -- Fetch the last rent entry
                                    SELECT r.Amount, r.RentPeriod INTO rentAmount, rentPeriod 
                                    FROM Rents r 
                                    WHERE r.BedTypeId = (SELECT BedTypeId FROM Beds WHERE Id = bedId)
                                    ORDER BY FIELD(r.RentPeriod, '0', '2', '1', '4', '5', '3') DESC LIMIT 1;

                                    -- Convert days to hours if ldays > 0
                                    IF ldays > 0 THEN
                                        SET lhours = lhours + (ldays * 24);
                                        SET ldays = 0;
                                    END IF;

                                    -- Process remaining hours based on RentPeriod
                                    IF lhours > 0 THEN
                                        CASE rentPeriod
				                            WHEN '4' THEN -- TWELVEHOURS
					                            SET amount = amount + (rentAmount * (lhours / 12));
					                            SET lhours = 0;
				                            WHEN '5' THEN -- FOURHOURS
					                            SET amount = amount + (rentAmount * (lhours / 4));
					                            SET lhours = 0;
				                            WHEN '3' THEN -- HOURLY
					                            SET amount = amount + (rentAmount * lhours);
					                            SET lhours = 0;
				                            WHEN '1' THEN -- DAILY
					                            SET amount = amount + (rentAmount * (lhours / 24));
					                            SET lhours = 0;
				                            WHEN '2' THEN -- WEEKLY (7 * 24)
					                            SET amount = amount + (rentAmount * ((lhours / 24) / 7));
					                            SET lhours = 0;
				                            WHEN '0' THEN -- MONTHLY (30 * 24)
					                            SET amount = amount + (rentAmount * ((lhours / 24) / 30));
					                            SET lhours = 0;
                                        END CASE;
                                        END IF;
                                    END IF;
                                    IF amount > 0 THEN

                                        -- Insert calculated rent into PatientRoomRents
                                        INSERT INTO PatientRoomRents
                                        (DatePosted, DateOfRental, PatientId, InPatientAdmissionId, Description, Amount, Bed, Ward)
                                        VALUES (TIMESTAMP(DATE(NOW()), '00:00:00'), DATE(startDate), patientId, admissionId, CONCAT('Room rent for ', startDate, timeDescription), amount, BedName, WardName);

                                        -- Insert calculated rent into PatientLedgers
                                        INSERT INTO PatientLedgers
                                        (Date, PatientId, InPatientAdmissionId, OpRegistrationId, Amount, Description, CompanyId, Type, Invoiced, InvoiceAmount, RentTimeDuration, Bed)
                                        VALUES (TIMESTAMP(DATE(NOW()), '00:00:00'), patientId, admissionId, OpRegistrationId, amount, CONCAT('Room rent for ', startDate, timeDescription), companyId, '5', '0', 0, timeDuration, BedName);
                                    END IF;

                                    -- Update for the next iteration
                                    SET amount = 0;
                                    SET timeDescription = '';
                                    SET startDate = nextDay;
                                END LOOP location_loop;
                                CLOSE curLocations;
                            END WHILE;
                        END LOOP read_loop;
                        CLOSE curAdmissions;
                    END";
                }
                using (MySqlCommand cmd = new MySqlCommand(createProcedureScript, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UserRoleAndsystemfunctionUpdatePharmacyAndOPRegister()
        {
            if (HasSeedRun("UserRoleAndsystemfunctionUpdatePharmacyAndOPRegister"))
                return;

            var f1 = Context.SystemFunctions.Find(1L);
            var f2 = Context.SystemFunctions.Find(2L);
            var f3 = Context.SystemFunctions.Find(3L);
            var f4 = Context.SystemFunctions.Find(4L);
            var f5 = Context.SystemFunctions.Find(5L);
            var f6 = Context.SystemFunctions.Find(6L);
            var f7 = Context.SystemFunctions.Find(7L);
            var f8 = Context.SystemFunctions.Find(8L);
            var f9 = Context.SystemFunctions.Find(9L);
            var f10 = Context.SystemFunctions.Find(10L);
            var f11 = Context.SystemFunctions.Find(11L);
            var f12 = Context.SystemFunctions.Find(12L);
            var f13 = Context.SystemFunctions.Find(13L);
            var f14 = Context.SystemFunctions.Find(14L);
            var f15 = Context.SystemFunctions.Find(15L);
            var f17 = Context.SystemFunctions.Find(17L);
            var f18 = Context.SystemFunctions.Find(18L);
            var f19 = Context.SystemFunctions.Find(19L);
            var f20 = Context.SystemFunctions.Find(20L);
            var f21 = Context.SystemFunctions.Find(21L);
            var f22 = Context.SystemFunctions.Find(22L);
            var f23 = Context.SystemFunctions.Find(23L);
            var f24 = Context.SystemFunctions.Find(24L);
            var f25 = Context.SystemFunctions.Find(25L);
            var f26 = Context.SystemFunctions.Find(26L);
            var f27 = Context.SystemFunctions.Find(27L);
            var f28 = Context.SystemFunctions.Find(28L);
            var f29 = Context.SystemFunctions.Find(29L);
            var f30 = Context.SystemFunctions.Find(30L);
            var f31 = Context.SystemFunctions.Find(31L);
            var f32 = Context.SystemFunctions.Find(32L);
            var f33 = Context.SystemFunctions.Find(33L);
            var f34 = Context.SystemFunctions.Find(34L);
            var f35 = Context.SystemFunctions.Find(35L);
            var f36 = Context.SystemFunctions.Find(36L);
            var f37 = Context.SystemFunctions.Find(37L);
            var f38 = Context.SystemFunctions.Find(38L);
            var f39 = Context.SystemFunctions.Find(39L);
            var f40 = Context.SystemFunctions.Find(40L);
            var f41 = Context.SystemFunctions.Find(41L);
            var f42 = Context.SystemFunctions.Find(42L);
            var f43 = Context.SystemFunctions.Find(43L);
            var f44 = Context.SystemFunctions.Find(44L);
            var f45 = Context.SystemFunctions.Find(45L);
            var f46 = Context.SystemFunctions.Find(46L);
            var f47 = Context.SystemFunctions.Find(47L);
            var f48 = Context.SystemFunctions.Find(48L);
            var f49 = Context.SystemFunctions.Find(49L);
            var f50 = Context.SystemFunctions.Find(50L);
            var f51 = Context.SystemFunctions.Find(51L);
            var f52 = Context.SystemFunctions.Find(52L);
            var f53 = Context.SystemFunctions.Find(53L);
            var f54 = Context.SystemFunctions.Find(54L);
            var f55 = Context.SystemFunctions.Find(55L);
            var f56 = Context.SystemFunctions.Find(56L);
            var f57 = Context.SystemFunctions.Find(57L);
            var f58 = Context.SystemFunctions.Find(58L);
            var f59 = Context.SystemFunctions.Find(59L);
            var f60 = Context.SystemFunctions.Find(60L);
            var f61 = Context.SystemFunctions.Find(61L);
            var f62 = Context.SystemFunctions.Find(62L);
            var f63 = Context.SystemFunctions.Find(63L);
            var f64 = Context.SystemFunctions.Find(64L);
            var f65 = Context.SystemFunctions.Find(65L);
            var f66 = Context.SystemFunctions.Find(66L);
            var f67 = Context.SystemFunctions.Find(67L);
            var f68 = Context.SystemFunctions.Find(68L);
            var f69 = Context.SystemFunctions.Find(69L);
            var f70 = Context.SystemFunctions.Find(70L);
            var f71 = Context.SystemFunctions.Find(71L);
            var f72 = Context.SystemFunctions.Find(72L);
            var f73 = Context.SystemFunctions.Find(73L);
            var f74 = Context.SystemFunctions.Find(74L);
            var f75 = Context.SystemFunctions.Find(75L);
            var f76 = Context.SystemFunctions.Find(76L);
            var f77 = Context.SystemFunctions.Find(77L);
            var f78 = Context.SystemFunctions.Find(78L);
            var f79 = Context.SystemFunctions.Find(79L);
            var f80 = Context.SystemFunctions.Find(80L);
            var f81 = Context.SystemFunctions.Find(81L);
            var f82 = Context.SystemFunctions.Find(82L);
            var f83 = Context.SystemFunctions.Find(83L);
            var f84 = Context.SystemFunctions.Find(84L);
            var f85 = Context.SystemFunctions.Find(85L);
            var f86 = Context.SystemFunctions.Find(86L);
            var f87 = Context.SystemFunctions.Find(87L);
            var f88 = Context.SystemFunctions.Find(88L);
            var f89 = Context.SystemFunctions.Find(89L);
            var f90 = Context.SystemFunctions.Find(90L);
            var f91 = Context.SystemFunctions.Find(91L);
            var f92 = Context.SystemFunctions.Find(92L);
            var f93 = Context.SystemFunctions.Find(93L);
            var f94 = Context.SystemFunctions.Find(94L);
            var f95 = Context.SystemFunctions.Find(95L);
            var f96 = Context.SystemFunctions.Find(96L);
            var f97 = Context.SystemFunctions.Find(97L);
            var f98 = Context.SystemFunctions.Find(98L);
            var f99 = Context.SystemFunctions.Find(99L);
            var f100 = Context.SystemFunctions.Find(100L);
            var f101 = Context.SystemFunctions.Find(101L);
            var f102 = Context.SystemFunctions.Find(102L);
            var f103 = Context.SystemFunctions.Find(103L);
            var f104 = Context.SystemFunctions.Find(104L);
            var f105 = Context.SystemFunctions.Find(105L);
            var f106 = Context.SystemFunctions.Find(106L);
            var f107 = Context.SystemFunctions.Find(107L);
            var f108 = Context.SystemFunctions.Find(108L);
            var f109 = Context.SystemFunctions.Find(109L);
            var f110 = Context.SystemFunctions.Find(110L);
            var f111 = Context.SystemFunctions.Find(111L);
            var f112 = Context.SystemFunctions.Find(112L);
            var f113 = Context.SystemFunctions.Find(113L);
            var f114 = Context.SystemFunctions.Find(114L);

            SystemFunction[] sf14 = { f114, f49, f47, f97 };
            SystemFunction[] sf15 = { f1, f2, f3, f4,f5,f6,f7,f8,f9,f10,f11,f12,f13, f14, f15, f17, f18, f19, f20, f21, f22, f23, f24, f25, f26, f27, f28, f29, f30,f31,f32, f33,f34, f35, f36, f37, f38, f39,f40,f41,f42,f43, f44,
                f45,f46,f65,f66,f67,f68,f69,f70,f71,f72,f73,f74, f75, f76, f77, f78, f79, f80, f81, f82, f83, f84, f85, f86, f87, f88,f89,f108,f109, f110, f111, f112,f113,f114 };

            List<Role> lRolesFromDB = Context.Roles.ToList();
            List<Role> lRoles = null;
            if (lRolesFromDB != null && lRolesFromDB.Count > 0)
            {
                lRoles = new List<Role>()
                {
                    new Role() { RoleId = 14, Name = "Register-OP", Description = "Hospital User, This user will have access to OP", SystemFunctions = sf14.ToList<SystemFunction>() },
                    new Role() { RoleId = 15, Name = "Pharmacy", Description = "Hospital User, This user will have access to Pharmacy", SystemFunctions = sf15.ToList<SystemFunction>() }
                };
                foreach (Role role in lRoles)
                {
                    Context.Roles.Add(role);
                    Context.SaveChanges();
                }
            }
            Context.SeedDataHistories.Add(new SeedDataHistory() { Key = "UserRoleAndsystemfunctionUpdatePharmacyAndOPRegister", DateExecuted = new System.DateTime() });
            Context.SaveChanges();
        }
    }
}
