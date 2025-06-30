using fa.context;
using fa.Data;
using fa.model.hms.common;
using fa.model.Hms.common;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using Fa.api.Hms;
using Fa.report.accounting.master;
using FADataAccessLibrary.Api.Accounting.DoubleEntry;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;

namespace fa.api.Hms
{
    public class PatientLedgerManager
    {
        private static volatile PatientLedgerManager instance;
        private static object syncRoot = new Object();
        PatientLedgerManager()
        {

        }
        public static PatientLedgerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PatientLedgerManager();
                    }
                }
                return instance;
            }
        }
        public PatientLedger GetPatientByOpIdType(long OPId,TransactionType Type)
        {
            PatientLedger PatientLedgerInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientLedgerInfo = Context.PatientLedgers.Include("Patient").FirstOrDefault(x => x.OpRegistrationId == OPId && x.Type== Type);
                return PatientLedgerInfo;
            }
        }
        public PatientLedger GetPatientLedgerById(long Id)
        {
            PatientLedger PatientLedgerInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientLedgerInfo = Context.PatientLedgers.Include("Patient").Include("PatientPaymentDetails").Include("PatientPaymentDetails.Account").FirstOrDefault(x => x.Id == Id);
                return PatientLedgerInfo;
            }
        }
        public class ConsultantInfo
        {
            public long? ConsultantId { get; set; }
            public string ConsultantName { get; set; }
        }
        public IList<ConsultantInfo> GetConsultantListByPatientId(long patientId)
        {
            using (AccountMasterContext context = new AccountMasterContext())
            {
                var consultantList = context.PatientLedgers
                    .Where(x => x.PatientId == patientId)
                    .Select(x => new ConsultantInfo
                    {
                        ConsultantId = x.ConsultantId,
                        ConsultantName = x.ConsultantName()
                    })
                    .ToList();

                return consultantList;
            }
        }
        public PatientLedger FindPatientLedgerById(long Id)
        {
            PatientLedger PatientLedgerInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientLedgerInfo = Context.PatientLedgers.Find(Id);
                return PatientLedgerInfo;
            }
        }
        public PatientPaymentDetail GetPatientPatientPaymentDetailById(long LedgId)
        {
            PatientPaymentDetail PatientPaymentDetail = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientPaymentDetail = Context.PatientPaymentDetails.Include("Account").FirstOrDefault(x => x.PatientLedgerId == LedgId);
                return PatientPaymentDetail;
            }
        }
        public IList<PatientLedger> ListAllEntryByPatientId(long PatientId)
        {
            IList<PatientLedger> PatientLedgerInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientLedgerInfo = (from Ledger in Context.PatientLedgers.Include("InPatientAdmission") where Ledger.PatientId == PatientId  select Ledger).OrderBy(x=>x.Date).ToList();
                return PatientLedgerInfo;
            }
        }
        public IList<PatientLedger> ListAllEntryConsultantId(long CompanyId)
        {
            IList<PatientLedger> PatientLedgerInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientLedgerInfo = (from Ledger in Context.PatientLedgers.Include("Consultant") where Ledger.CompanyId == CompanyId select Ledger).ToList();
                return PatientLedgerInfo;
            }
        }
        public IList<PatientLedger> ListAllEntryByInvoiceId(long InvoiceId)
        {
            IList<PatientLedger> PatientInvoiceInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientInvoiceInfo = (from Ledger in Context.PatientLedgers where Ledger.PatientInvoiceId == InvoiceId select Ledger).OrderBy(x => x.Date.Date).ThenBy(x => x.Id).ToList();
                return PatientInvoiceInfo;
            }
        }
        public PatientLedger GetLedgerByInvoiceId(long InvoiceId)
        {
            PatientLedger PatientInvoiceInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientInvoiceInfo = (from Ledger in Context.PatientLedgers where Ledger.PatientInvoiceId == InvoiceId select Ledger).OrderBy(x => x.Id).Last();
                return PatientInvoiceInfo;
            }
        }
        public IList<PatientLedger> ListPaymentReceivedFromPatientLedgerByPatientId(long PatientId)
        {
            IList<PatientLedger> PatientLedgerInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientLedgerInfo = (from Ledger in Context.PatientLedgers where (Ledger.PatientId == PatientId && (Ledger.Type == TransactionType.PAYMENT || Ledger.Type == TransactionType.WAIVER))  select Ledger).OrderBy(x => x.Date).ToList();
                return PatientLedgerInfo;
            }
        }
        public IList<PatientLedger> ListFeeChargeFromPatientLedgerByPatientId(long PatientId)
        {
            IList<PatientLedger> PatientLedgerInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientLedgerInfo = (from Ledger in Context.PatientLedgers.Include("ConsultedConsultationFee").Include("ConsultedProcedure").Include("ConsultedLabTest") where (Ledger.PatientId == PatientId /*&& !Ledger.Invoiced*/ && (Ledger.Type!=TransactionType.PAYMENT && Ledger.Type != TransactionType.WAIVER)) select Ledger).OrderBy(x => x.Date).ToList();
                return PatientLedgerInfo;
            }
        }
        public void UpdateReceivePayment(PatientLedger lPatientLedger, AccountMasterContext Context)
        {
            PatientLedger PatientLedgerFromDB = Context.PatientLedgers.Include("PatientPaymentDetails").FirstOrDefault(x => x.Id == lPatientLedger.Id);
            if (PatientLedgerFromDB != null)
            {
                if (PatientLedgerFromDB.Amount == lPatientLedger.Amount)
                {
                    foreach (PatientPaymentDetail PDetail in lPatientLedger.PatientPaymentDetails)
                    {
                        PatientPaymentDetail OldPDetail = PatientLedgerFromDB.PatientPaymentDetails.FirstOrDefault(x => x.PatientLedgerId == lPatientLedger.Id);
                        if (OldPDetail != null)
                        {
                            PDetail.Id = OldPDetail.Id;
                            Context.Entry(Context.PatientPaymentDetails.Find(PDetail.Id)).CurrentValues.SetValues(PDetail);
                            Context.SaveChanges();
                        }
                    }
                    Context.Entry(Context.PatientLedgers.Find(PatientLedgerFromDB.Id)).CurrentValues.SetValues(lPatientLedger);
                    Context.SaveChanges();

                }
                else if (PatientLedgerFromDB.Amount < lPatientLedger.Amount)
                {
                    UpdateInvoiceByPayment(Context, lPatientLedger, PatientLedgerFromDB.Amount);
                }
                else
                {
                    List<PatientInvoicePayment> lPatientInvoicePayment = Context.PatientInvoicePayments.Where(x => x.LedgerId == PatientLedgerFromDB.Id).ToList();
                    if (lPatientInvoicePayment != null && lPatientInvoicePayment.Count > 0)
                    {
                        double LedPayment = lPatientLedger.Amount;
                        foreach (PatientInvoicePayment Payment in lPatientInvoicePayment.OrderByDescending(a => a.Amount))
                        {
                            if (Payment.Amount > LedPayment)
                            {
                                PatientInvoice lPatientInvoice = Context.PatientInvoices.Find(Payment.InvoiceId);
                                if (lPatientInvoice != null)
                                {
                                    lPatientInvoice.Paid -= Payment.Amount;
                                    lPatientInvoice.Balance += Payment.Amount;
                                    Context.Entry(Context.PatientInvoices.Find(Payment.InvoiceId)).CurrentValues.SetValues(lPatientInvoice);
                                    Context.SaveChanges();

                                    Context.PatientInvoicePayments.Remove(Payment);
                                    Context.SaveChanges();
                                }
                            }
                            else
                            {
                                LedPayment -= Payment.Amount;
                            }
                        }
                        lPatientLedger.InvoiceAmount = LedPayment;
                        lPatientLedger.Invoiced = LedPayment == 0 ? true : false;
                    }
                    Context.Entry(Context.PatientLedgers.Find(lPatientLedger.Id)).CurrentValues.SetValues(lPatientLedger);
                    Context.SaveChanges();
                    //daybook
                    HmsPaymentDoubleEntryManager.Instance.RecordPayment(lPatientLedger, Context);
                }
            }
        }
        public void UpdatePatientLedger(PatientLedger lPatientLedger)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (lPatientLedger != null)
                        {
                            if (lPatientLedger.Id == 0L)
                            {
                                Context.PatientLedgers.Add(lPatientLedger);
                                Context.SaveChanges();
                                UpdateInvoiceByPayment(Context, lPatientLedger,0);
                            }
                            else
                            {
                                UpdateReceivePayment(lPatientLedger,Context);
                            }
                        }                        
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }
        public void UpdateInvoiceByPayment(AccountMasterContext Context, PatientLedger lPatientLedger,double InvoicedAmount)
        {
            IList<PatientInvoice> lPatientInvoice = Context.PatientInvoices.Where(x => x.PatientId == lPatientLedger.PatientId && x.Balance > 0).ToList();
            if (lPatientInvoice != null && lPatientInvoice.Count>0)
            {
                double InvoiceBalance = 0.00;
                double ReceivedAmount = lPatientLedger.Amount- InvoicedAmount;
                foreach (PatientInvoice PatientInvoice in lPatientInvoice)
                {
                    InvoiceBalance = PatientInvoice.Balance;
                    if (ReceivedAmount <= InvoiceBalance)
                    {
                        PatientInvoice.Paid += ReceivedAmount;
                        PatientInvoice.Balance = PatientInvoice.Total - PatientInvoice.Paid;
                        Context.Entry(Context.PatientInvoices.Find(PatientInvoice.InvoiceId)).CurrentValues.SetValues(PatientInvoice);

                        PatientInvoicePayment PatientInvoicePayment = new PatientInvoicePayment();
                        PatientInvoicePayment.InvoiceId = PatientInvoice.InvoiceId;
                        PatientInvoicePayment.LedgerId = lPatientLedger.Id;
                        PatientInvoicePayment.Date = lPatientLedger.Date;
                        PatientInvoicePayment.Description = "Amount received from Payment " + lPatientLedger.RefNumber;
                        PatientInvoicePayment.Amount = ReceivedAmount;
                        Context.PatientInvoicePayments.Add(PatientInvoicePayment);
                        Context.SaveChanges();
                        ReceivedAmount = 0;
                        break;
                    }
                    else
                    {
                        ReceivedAmount -= InvoiceBalance;
                        PatientInvoice.Paid += InvoiceBalance;
                        PatientInvoice.Balance = 0;
                        Context.Entry(Context.PatientInvoices.Find(PatientInvoice.InvoiceId)).CurrentValues.SetValues(PatientInvoice);

                        PatientInvoicePayment PatientInvoicePayment = new PatientInvoicePayment();
                        PatientInvoicePayment.InvoiceId = PatientInvoice.InvoiceId;
                        PatientInvoicePayment.LedgerId = lPatientLedger.Id;
                        PatientInvoicePayment.Date = lPatientLedger.Date;
                        PatientInvoicePayment.Description = "Amount received from Payment " + lPatientLedger.RefNumber;
                        PatientInvoicePayment.Amount = InvoiceBalance;
                        Context.PatientInvoicePayments.Add(PatientInvoicePayment);
                        Context.SaveChanges();
                    }
                }
                lPatientLedger.InvoiceAmount = lPatientLedger.Amount - ReceivedAmount;
                lPatientLedger.Invoiced = ReceivedAmount == 0 ? true : false;
            }
            Context.Entry(Context.PatientLedgers.Find(lPatientLedger.Id)).CurrentValues.SetValues(lPatientLedger);
            Context.SaveChanges();
            //daybook
            HmsPaymentDoubleEntryManager.Instance.RecordPayment(lPatientLedger, Context);
        }
        public void UpdatePatientLedgerForDeletePayment(long PaymentLedgerId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        PatientLedger Ledger = Context.PatientLedgers.Find(PaymentLedgerId);
                        if (Ledger != null)
                        {
                            DeleteReceivePayment(Ledger, Context);
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }
        public void DeleteReceivePayment(PatientLedger Ledger, AccountMasterContext Context)
        {
            Context.PatientPaymentDetails.Where(P => P.PatientLedgerId == Ledger.Id).ToList().ForEach(P => Context.PatientPaymentDetails.Remove(P));
            Context.SaveChanges();
            //remove inoiced payment
            PatientInvoiceManager.Instance.RemovepaymentForRegistrationFee(Ledger, Context);
            //daybook
            HmsPaymentDoubleEntryManager.Instance.DeletePayments(Ledger, Context);
            //remove payment
            Context.PatientLedgers.Remove(Ledger);
            Context.SaveChanges();
        }
        private string GetLedgerDescription(long ConsultationId,AccountMasterContext Context)
        {           
            Consultation Consultation = Context.Consultations.FirstOrDefault(x => x.Id == ConsultationId);
            if (Consultation != null)
            {
                return Consultation.DisplayAs;
            }
            return string.Empty;
        }
        private double GetLastUpdatedFee(long ConsultationId, long ConsultantId, AccountMasterContext Context)
        {
            double Fee = 0.00;
            IList<ConsultedDoctorConsultationFee> DoctorConsultation = (from DocCons in Context.ConsultedDoctorConsultationFees where (DocCons.ConsultationId == ConsultationId && DocCons.ConsultantId == ConsultantId) select DocCons).OrderByDescending(x => x.LastModifiedDate).ToList();
            if (DoctorConsultation != null && DoctorConsultation.Count>0)
            {
                Fee = DoctorConsultation.First().Fee;
            }
            else
            {
                Consultation Consultation = Context.Consultations.FirstOrDefault(x => x.Id == ConsultationId);
                if (Consultation != null)
                {
                    Fee = Consultation.Fee;
                }
            }
            return Fee;
        }
        public double GetDischargeBalanceAmount(InPatientAdmission Admission)
        {
            double BalanceAmount = 0;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<PatientLedger> PatientLedgerPatientLedger = (from Leger in Context.PatientLedgers where Leger.PatientId == Admission.PatientId && Leger.Type!=TransactionType.PAYMENT && Leger.Type!=TransactionType.WAIVER select Leger).Distinct().ToList();
                if (PatientLedgerPatientLedger.Count > 0)
                {
                    BalanceAmount+= PatientLedgerPatientLedger.Sum(x=>x.Amount);
                }
                IList<PatientLedger> PatientLedgerPayment = (from Leger in Context.PatientLedgers where Leger.PatientId == Admission.PatientId && (Leger.Type == TransactionType.PAYMENT || Leger.Type == TransactionType.WAIVER) select Leger).Distinct().ToList();
                if(PatientLedgerPayment.Count>0)
                {
                    BalanceAmount -= PatientLedgerPayment.Sum(x=>x.Amount);
                }
            }
            return BalanceAmount<0?0: BalanceAmount;
        }
        public void AddPatientLedgerFromConsulting(ConsultationNote ConsultationNote, AccountMasterContext Context)
        {
            foreach (ConsultedConsultationFee ConsultedConsultation in ConsultationNote.ConsultedConsultationFee)
            {
                AddPatientLedgerForConsulting(ConsultationNote, ConsultedConsultation, Context);
            }
        }
        public void AddPatientLedgerFromProcedure(ConsultationNote ConsultationNote, AccountMasterContext Context)
        {
            foreach (ConsultedProcedure ConsultedProcedure in ConsultationNote.ConsultedProcedure)
            {
                AddPatientLedgerForProcedure(ConsultationNote, ConsultedProcedure, Context);
            }
        }
        public void AddPatientLedgerFromLabTest(ConsultationNote ConsultationNote, AccountMasterContext Context)
        {
            foreach (ConsultedLabTest ConsultedLabTest in ConsultationNote.ConsultedLabTest)
            {
                AddPatientLedgerForLabTest(ConsultationNote, ConsultedLabTest, Context);
            }
        }
        public void AddPatientLedgerForLabTest(ConsultationNote ConsultationNote, ConsultedLabTest ConsultedLabTest, AccountMasterContext Context)
        {
            PatientLedger PatientLedgerInfo = new PatientLedger();
            PatientLedgerInfo.Date = ConsultedLabTest.RequestedOn;
            PatientLedgerInfo.ConsultationNoteId = ConsultationNote.Id;
            PatientLedgerInfo.ConsultantId = ConsultationNote.ConsultantId;
            PatientLedgerInfo.OpRegistrationId = ConsultationNote.OpRegistrationId != null ? ConsultationNote.OpRegistrationId : null;
            PatientLedgerInfo.InPatientAdmissionId = ConsultationNote.InPatientAdmissionId != null ? ConsultationNote.InPatientAdmissionId : null;
            PatientLedgerInfo.PatientId = ConsultationNote.PatientId;
            PatientLedgerInfo.Amount = ConsultedLabTest.Fees;
            PatientLedgerInfo.ConsultedLabTestId = ConsultedLabTest.ConsultedLabTestId;
            PatientLedgerInfo.CompanyId = ConsultationNote.CompanyId;
            //PatientLedgerInfo.Description = "Charge for " + ConsultedLabTest.Name + " by " + Context.Users.FirstOrDefault(X => X.UserId == ConsultationNote.ConsultantId).Name;
            PatientLedgerInfo.Description = ConsultedLabTest.Name;
            PatientLedgerInfo.Type = TransactionType.LAB_FEE;
            Context.PatientLedgers.Add(PatientLedgerInfo);
            Context.SaveChanges();
        }
        public void AddPatientLedgerForProcedure(ConsultationNote ConsultationNote, ConsultedProcedure ConsultedProcedureFee, AccountMasterContext Context)
        {
            PatientLedger PatientLedgerInfo = new PatientLedger();
            PatientLedgerInfo.Date = ConsultedProcedureFee.Date;
            PatientLedgerInfo.ConsultationNoteId = ConsultationNote.Id;
            PatientLedgerInfo.ConsultantId = ConsultationNote.ConsultantId;
            PatientLedgerInfo.OpRegistrationId = ConsultationNote.OpRegistrationId != null ? ConsultationNote.OpRegistrationId : null;
            PatientLedgerInfo.InPatientAdmissionId = ConsultationNote.InPatientAdmissionId != null ? ConsultationNote.InPatientAdmissionId : null;
            PatientLedgerInfo.PatientId = ConsultationNote.PatientId;
            PatientLedgerInfo.Amount = ConsultedProcedureFee.Fees; 
            PatientLedgerInfo.ConsultedProcedureId = ConsultedProcedureFee.ConsultedProcedureId; 
            PatientLedgerInfo.CompanyId = ConsultationNote.CompanyId;
            //PatientLedgerInfo.Description = "Charge for "+ ConsultedProcedureFee.Name +" by "+Context.Users.FirstOrDefault(X=>X.UserId== ConsultationNote.ConsultantId).Name;
            PatientLedgerInfo.Description = ConsultedProcedureFee.Name;
            PatientLedgerInfo.Type = TransactionType.MEDICALPROCEDURE_FEE;
            Context.PatientLedgers.Add(PatientLedgerInfo);
            Context.SaveChanges();
        }
        public void AddPatientLedgerForConsulting(ConsultationNote ConsultationNote, ConsultedConsultationFee ConsultedConsultationFee, AccountMasterContext Context)
        {           
            PatientLedger PatientLedgerInfo = new PatientLedger();
            PatientLedgerInfo.Date = ConsultedConsultationFee.Date;
            PatientLedgerInfo.ConsultationNoteId = ConsultationNote.Id;
            PatientLedgerInfo.ConsultantId = ConsultationNote.ConsultantId;
            PatientLedgerInfo.OpRegistrationId = ConsultationNote.OpRegistrationId != null ? ConsultationNote.OpRegistrationId : null;
            PatientLedgerInfo.InPatientAdmissionId = ConsultationNote.InPatientAdmissionId != null ? ConsultationNote.InPatientAdmissionId : null;
            PatientLedgerInfo.PatientId = ConsultationNote.PatientId;
            PatientLedgerInfo.Amount = GetLastUpdatedFee(ConsultedConsultationFee.ConsultationId, ConsultationNote.ConsultantId, Context);
            PatientLedgerInfo.ConsultedConsultationId = ConsultedConsultationFee.ConsultedConsultationId;
            PatientLedgerInfo.CompanyId = ConsultationNote.CompanyId;
            PatientLedgerInfo.Description = GetLedgerDescription(ConsultedConsultationFee.ConsultationId, Context) /* + " by " + Context.Users.FirstOrDefault(X => X.UserId == ConsultationNote.ConsultantId).Name */;
            PatientLedgerInfo.Type = TransactionType.CONSULTATION_FEE;
            Context.PatientLedgers.Add(PatientLedgerInfo);
            Context.SaveChanges();
        }
        public void UpdatePatientLedgerFromConsulting(ConsultationNote ConsultationNote, ConsultedConsultationFee ConsultedConsultationFee, AccountMasterContext Context)
        {
            PatientLedger PatientLedgerInfo = Context.PatientLedgers.FirstOrDefault(x=>x.ConsultationNoteId==ConsultationNote.Id && x.ConsultedConsultationId==ConsultedConsultationFee.ConsultedConsultationId);
            if (PatientLedgerInfo != null)
            {
                PatientLedgerInfo.Amount = GetLastUpdatedFee(ConsultedConsultationFee.ConsultationId, ConsultationNote.ConsultantId, Context);
                Context.Entry(Context.PatientLedgers.Find(PatientLedgerInfo.Id)).CurrentValues.SetValues(PatientLedgerInfo);
                Context.SaveChanges();
            }
             
        }
        public void UpdatePatientLedgerFromProcedure(ConsultationNote ConsultationNote, ConsultedProcedure ConsultedProcedureFee, AccountMasterContext Context)
        {
            PatientLedger PatientLedgerInfo = Context.PatientLedgers.FirstOrDefault(x => x.ConsultationNoteId == ConsultationNote.Id && x.ConsultedProcedureId == ConsultedProcedureFee.ConsultedProcedureId);
            if (PatientLedgerInfo != null)
            {
                PatientLedgerInfo.Amount = ConsultedProcedureFee.Fees;
                PatientLedgerInfo.Date = ConsultedProcedureFee.Date;
                Context.Entry(Context.PatientLedgers.Find(PatientLedgerInfo.Id)).CurrentValues.SetValues(PatientLedgerInfo);
                Context.SaveChanges();
            }
        }
        public void UpdatePatientLedgerFromLabTest(ConsultationNote ConsultationNote, ConsultedLabTest ConsultedLabTest, AccountMasterContext Context)
        {
            PatientLedger PatientLedgerInfo = Context.PatientLedgers.FirstOrDefault(x => x.ConsultationNoteId == ConsultationNote.Id && x.ConsultedLabTestId == ConsultedLabTest.ConsultedLabTestId);
            if (PatientLedgerInfo != null)
            {
                PatientLedgerInfo.Amount = ConsultedLabTest.Fees;
                PatientLedgerInfo.Date = ConsultedLabTest.RequestedOn;
                Context.Entry(Context.PatientLedgers.Find(PatientLedgerInfo.Id)).CurrentValues.SetValues(PatientLedgerInfo);
                Context.SaveChanges();
            }
        }
        public void DeletePatientLedgerFromConsulting(ConsultationNote ConsultationNote, ConsultedConsultationFee ConsultedConsultationFee, AccountMasterContext Context)
        {
            PatientLedger PatientLedgerInfo = Context.PatientLedgers.FirstOrDefault(x => x.ConsultationNoteId == ConsultationNote.Id && x.ConsultedConsultationId == ConsultedConsultationFee.ConsultedConsultationId);
            if (PatientLedgerInfo != null)
            {
                Context.PatientLedgers.Remove(PatientLedgerInfo);
                Context.SaveChanges();
            }
        }
        public void DeletePatientLedgerFromProcedure(ConsultationNote ConsultationNote, ConsultedProcedure ConsultedProcedureFee, AccountMasterContext Context)
        {
            PatientLedger PatientLedgerInfo = Context.PatientLedgers.FirstOrDefault(x => x.ConsultationNoteId == ConsultationNote.Id && x.ConsultedProcedureId == ConsultedProcedureFee.ConsultedProcedureId);
            if (PatientLedgerInfo != null)
            {
                Context.PatientLedgers.Remove(PatientLedgerInfo);
                Context.SaveChanges();
            }
        }
        public void DeletePatientLedgerFromLabTest(ConsultationNote ConsultationNote, ConsultedLabTest ConsultedLabTest, AccountMasterContext Context)
        {
            PatientLedger PatientLedgerInfo = Context.PatientLedgers.FirstOrDefault(x => x.ConsultationNoteId == ConsultationNote.Id && x.ConsultedLabTestId == ConsultedLabTest.ConsultedLabTestId);
            if (PatientLedgerInfo != null)
            {
                Context.PatientLedgers.Remove(PatientLedgerInfo);
                Context.SaveChanges();
            }
        }
        public void DeletePatientLedgerFromNote(ConsultationNote ConsultationNote, AccountMasterContext Context)
        {
            Context.PatientLedgers.Where(C => C.ConsultationNoteId == ConsultationNote.Id).ToList().ForEach(C => Context.PatientLedgers.Remove(C));
            Context.SaveChanges();
        }
        
        public void DeleteFeeFromPatientLedger(PatientLedger PatientLedger)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.PatientLedgers.Remove(Context.PatientLedgers.Find(PatientLedger.Id));
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }
        
        public IList<PatientLedger> ListPatientLedgerByPatientIdDate(long PatientId)
        {
            IList<PatientLedger> PatientLedgerInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientLedgerInfo = Context.PatientLedgers.Where(x => x.PatientId == PatientId && x.Type != TransactionType.PAYMENT && x.Type != TransactionType.WAIVER && !x.Invoiced).ToList<PatientLedger>();
            }
            return PatientLedgerInfo;
        }
        public IList<PatientLedger> ListPaymentsFromPatientLedgerByPatientId(long PatientId)
        {
            IList<PatientLedger> PatientLedgerInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientLedgerInfo = (from Ledger in Context.PatientLedgers where (Ledger.PatientId == PatientId && !Ledger.Invoiced  && (Ledger.Type == TransactionType.WAIVER || Ledger.Type == TransactionType.PAYMENT)) select Ledger).OrderBy(x => x.Date).ToList();
                return PatientLedgerInfo;
            }
        }
        public IList<PatientLedger> ListPatientLedgerByTransactionType(long CompanyId, long PatientInvoiceId, TransactionType GetTransactionType)
        {
            IList<PatientLedger> PatientLedgerInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientLedgerInfo = Context.PatientLedgers.Where(x =>  x.PatientInvoiceId == PatientInvoiceId && x.Type == GetTransactionType).ToList<PatientLedger>();
            }
            return PatientLedgerInfo;
        }
        public void UpdatePatientFeesInLedger(IList<PatientLedger> lPatientLedger)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach(PatientLedger ledger in lPatientLedger)
                        {
                            if(ledger.Id==0L)
                            {
                                if (ledger.ConsultedConsultationId != null)
                                {
                                    Consultation Consultation = Context.Consultations.Find((long)ledger.ConsultedConsultationId);
                                    if(Consultation != null)
                                    {
                                        ledger.ConsultedConsultationId = null;

                                        ConsultedConsultationFee ConsultedConsultation = new ConsultedConsultationFee();
                                        ConsultedConsultation.CompanyId = ledger.CompanyId;
                                        ConsultedConsultation.ConsultationId = Consultation.Id;
                                        ConsultedConsultation.Description = ledger.Description;
                                        ConsultedConsultation.Name = Consultation.Name;
                                        ConsultedConsultation.Fee = Consultation.Fee;
                                        ConsultedConsultation.ConsultantId = Global.User.UserId;
                                        ConsultedConsultation.Date = ledger.Date;

                                        Context.ConsultedConsultationFees.Add(ConsultedConsultation);
                                        Context.SaveChanges();

                                        ledger.ConsultedConsultationId = ConsultedConsultation.ConsultedConsultationId;
                                        Context.PatientLedgers.Add(ledger);
                                        Context.SaveChanges();
                                    }

                                }
                            }
                            else
                            {
                                Context.Entry(Context.PatientLedgers.Find(ledger.Id)).CurrentValues.SetValues(ledger);
                                Context.SaveChanges();
                            }
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }
        public void UpdateProcedureInLedger(List<ConsultedProcedure> lprocedures)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (ConsultedProcedure procedures in lprocedures)
                        {
                            if (procedures.ConsultedProcedureId != 0)
                            {
                                Context.Entry(Context.ConsultedProcedures.Find(procedures.ConsultedProcedureId)).CurrentValues.SetValues(procedures);
                                Context.SaveChanges();
                            }
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }
        public void UpdateConsultationFeeInLedger(List<ConsultedConsultationFee> lConsultationFees)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (ConsultedConsultationFee ConsultationFee in lConsultationFees)
                        {
                            if (ConsultationFee.ConsultedConsultationId != 0)
                            {
                                Context.Entry(Context.ConsultedConsultationFees.Find(ConsultationFee.ConsultedConsultationId)).CurrentValues.SetValues(ConsultationFee);
                                Context.SaveChanges();
                            }
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }    
    }
}
