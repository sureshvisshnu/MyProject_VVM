using fa.api.Accounting;
using fa.api.Hms;
using fa.context;
using fa.Data;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transactions;
using fa.model.Hms.common;
using fa.model.Hms.Master;
using Fa.report.accounting.master;
using FADataAccessLibrary.Api.Accounting.DoubleEntry;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Fa.api.Hms
{
     public class PatientInvoiceManager
    {
        private static volatile PatientInvoiceManager instance;
        private static object syncRoot = new Object();
        PatientInvoiceManager()
        {

        }
        public static PatientInvoiceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PatientInvoiceManager();
                    }
                }
                return instance;
            }
        }
        public List<PatientInvoice> GetPatientInvoiceByDate(DateTime? Date, long CompanyId)
        {
            List<PatientInvoice> PatientInvoiceInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientInvoiceInfo = Context.PatientInvoices.Include("Patient").Where(x => x.CompanyId==CompanyId && x.InvoiceDate==Date).ToList();
                return PatientInvoiceInfo;
            }
        }
        public List<PatientInvoice> GetPatientInvoiceBySearchText(string Txt,  long CompanyId)
        {
            List<PatientInvoice> PatientInvoiceInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientInvoiceInfo = Context.PatientInvoices.Include("Patient").Where(x => x.CompanyId == CompanyId && (x.ReferenceNumber.Contains(Txt) || x.Patient.Name.Contains(Txt))).ToList();
                return PatientInvoiceInfo;
            }
        }
        public PatientInvoice GetInvoiceById(long Id)
        {
            PatientInvoice PatientInvoiceInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientInvoiceInfo = Context.PatientInvoices.Include("Patient").FirstOrDefault(x => x.InvoiceId == Id);
                return PatientInvoiceInfo;
            }
        }
        public PatientInvoicePayment GetInvoicebyLedgerId(long InvoiceId, long LedgerId)
        {
            PatientInvoicePayment PatientInvoiceInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientInvoiceInfo = Context.PatientInvoicePayments.Include("PatientLedger").FirstOrDefault(x => x.InvoiceId == InvoiceId && x.LedgerId == LedgerId);
                return PatientInvoiceInfo;
            }            
        }
        public IList<PatientInvoicePayment> ListEntryByInvoiceId(long InvoiceId,long CompanyId)
        {
            IList<PatientInvoicePayment> PatientInvoiceInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientInvoiceInfo = (from Invoiced in Context.PatientInvoicePayments.Include("PatientInvoice") where (Invoiced.InvoiceId == InvoiceId && Invoiced.PatientInvoice.CompanyId== CompanyId && Invoiced.Amount!=0) select Invoiced).OrderBy(d=> d.Date).ToList();
                return PatientInvoiceInfo;
            }
        }
        public void RemovepaymentForRegistrationFee(PatientLedger PatientLedger, AccountMasterContext Context)
        {
            List<PatientInvoicePayment> lPatientInvoicePayment = Context.PatientInvoicePayments.Where(x => x.LedgerId == PatientLedger.Id).ToList();
            if (lPatientInvoicePayment != null && lPatientInvoicePayment.Count>0)
            {
                long InvId = (long)lPatientInvoicePayment.First().InvoiceId;
                if (lPatientInvoicePayment.Count == lPatientInvoicePayment.Where(x => x.InvoiceId == InvId).ToList().Count)
                {
                    PatientInvoice PatientInvoice = Context.PatientInvoices.Find(InvId);
                    if (PatientInvoice != null)
                    {
                        PatientInvoice.Paid -= lPatientInvoicePayment.Sum(x => x.Amount);
                        PatientInvoice.Balance += lPatientInvoicePayment.Sum(x => x.Amount);
                        Context.Entry(Context.PatientInvoices.Find(PatientInvoice.InvoiceId)).CurrentValues.SetValues(PatientInvoice);
                    }
                    //remove Inv payment
                    Context.PatientInvoicePayments.Where(P => P.LedgerId == PatientLedger.Id && P.InvoiceId == PatientInvoice.InvoiceId).ToList().ForEach(P => Context.PatientInvoicePayments.Remove(P));
                    Context.SaveChanges();
                }
                else
                {
                    foreach (var lPayment in lPatientInvoicePayment)
                    {
                        PatientInvoice PatientInvoice = Context.PatientInvoices.Find(lPayment.InvoiceId);
                        if (PatientInvoice == null)
                        {
                            PatientInvoice.Paid -= lPayment.Amount;
                            PatientInvoice.Balance += lPayment.Amount;
                            Context.Entry(Context.PatientInvoices.Find(PatientInvoice.InvoiceId)).CurrentValues.SetValues(PatientInvoice);
                        }
                        //remove Inv payment
                        Context.PatientInvoicePayments.Remove(Context.PatientInvoicePayments.Find(lPayment.Id));
                        Context.SaveChanges();
                    }
                }
            }
        }
        public void UpdatepaymentForRegistrationFee(PatientLedger PatientLedger, AccountMasterContext Context)
        {
            List<PatientInvoicePayment> lPatientInvoicePayment = Context.PatientInvoicePayments.Where(x => x.LedgerId == PatientLedger.Id).ToList();
            if (lPatientInvoicePayment != null && lPatientInvoicePayment.Count>0)
            {
                long InvId = (long)lPatientInvoicePayment.First().InvoiceId;
                if (lPatientInvoicePayment.Sum(x=>x.Amount) == PatientLedger.Amount)
                {
                    PatientLedger Ledger = Context.PatientLedgers.Find(PatientLedger.Id);
                    if (Ledger != null)
                    {
                        Ledger.Invoiced = true;
                        Ledger.InvoiceAmount += Ledger.Amount;
                        Context.Entry(Context.PatientLedgers.Find(Ledger.Id)).CurrentValues.SetValues(Ledger);
                        Context.SaveChanges();
                    }
                    return;
                }
                else if (lPatientInvoicePayment.Sum(x => x.Amount) > PatientLedger.Amount)
                {
                    //reves
                    if (lPatientInvoicePayment.Count == lPatientInvoicePayment.Where(x => x.InvoiceId == InvId).ToList().Count)
                    {
                        PatientInvoice PatientInvoice = Context.PatientInvoices.Find(InvId);
                        if (PatientInvoice != null)
                        {
                            PatientInvoice.Paid -= lPatientInvoicePayment.Sum(x => x.Amount);
                            PatientInvoice.Balance += lPatientInvoicePayment.Sum(x => x.Amount);
                            Context.Entry(Context.PatientInvoices.Find(PatientInvoice.InvoiceId)).CurrentValues.SetValues(PatientInvoice);
                        }
                        //remove Inv payment
                        Context.PatientInvoicePayments.Where(P => P.LedgerId == PatientLedger.Id && P.InvoiceId == PatientInvoice.InvoiceId).ToList().ForEach(P => Context.PatientInvoicePayments.Remove(P));
                        Context.SaveChanges();
                    }
                    else
                    {
                        foreach (var lPayment in lPatientInvoicePayment)
                        {
                            PatientInvoice PatientInvoice = Context.PatientInvoices.Find(lPayment.InvoiceId);
                            if (PatientInvoice == null)
                            {
                                PatientInvoice.Paid -= lPayment.Amount;
                                PatientInvoice.Balance += lPayment.Amount;
                                Context.Entry(Context.PatientInvoices.Find(PatientInvoice.InvoiceId)).CurrentValues.SetValues(PatientInvoice);
                            }
                            //remove Inv payment
                            Context.PatientInvoicePayments.Remove(Context.PatientInvoicePayments.Find(lPayment.Id));
                            Context.SaveChanges();
                        }
                    }                    
                    // repayment
                    paymentForRegistrationFee(PatientLedger, Context);
                }
                else if (lPatientInvoicePayment.Sum(x => x.Amount) < PatientLedger.Amount)
                {   
                    //extra payment
                    double Difference = PatientLedger.Amount - lPatientInvoicePayment.Sum(x => x.Amount);
                    PatientLedgerManager.Instance.UpdateInvoiceByPayment(Context, PatientLedger, Difference);
                }
            }
        }
        public void paymentForRegistrationFee(PatientLedger PatientLedger, AccountMasterContext Context)
        {
            PatientLedger PatientLedgerforRegFee = Context.PatientLedgers.FirstOrDefault(x => x.OpRegistrationId == PatientLedger.OpRegistrationId && x.Type == TransactionType.REGISTRATION_FEE);
            if (PatientLedgerforRegFee != null)
            {
                PatientInvoice PatientInvoice = Context.PatientInvoices.Find(PatientLedgerforRegFee.PatientInvoiceId);
                if (PatientInvoice != null && PatientInvoice.Balance<= PatientLedger.Amount)
                {
                    PatientInvoice.Paid += PatientLedger.Amount;
                    PatientInvoice.Balance = PatientInvoice.Total - PatientInvoice.Paid;
                    Context.Entry(Context.PatientInvoices.Find(PatientInvoice.InvoiceId)).CurrentValues.SetValues(PatientInvoice);

                    PatientInvoicePayment PatientInvoicePayment = new PatientInvoicePayment();
                    PatientInvoicePayment.InvoiceId = PatientInvoice.InvoiceId;
                    PatientInvoicePayment.LedgerId = PatientLedger.Id;
                    PatientInvoicePayment.Date = PatientLedger.Date;
                    PatientInvoicePayment.Description = "Amount received from Payment " + PatientLedger.RefNumber;
                    PatientInvoicePayment.Amount = PatientLedger.Amount;
                    Context.PatientInvoicePayments.Add(PatientInvoicePayment);
                    Context.SaveChanges();

                    PatientLedger Ledger = Context.PatientLedgers.Find(PatientLedger.Id);
                    if (Ledger != null)
                    {
                        Ledger.Invoiced =  true;
                        Ledger.InvoiceAmount += Ledger.Amount;
                        Context.Entry(Context.PatientLedgers.Find(Ledger.Id)).CurrentValues.SetValues(Ledger);
                        Context.SaveChanges();
                    }
                }
                else
                {
                    PatientLedgerManager.Instance.UpdateInvoiceByPayment(Context, PatientLedger,0);
                }
            }
        }
        public void RemoveInvoiceForRegistrationFee(PatientLedger PatientLedger, AccountMasterContext Context)
        {
            PatientInvoice PatientInvoice = Context.PatientInvoices.Find(PatientLedger.PatientInvoiceId);        
            if (PatientInvoice.InvoiceId != 0L)
            {
                List<PatientInvoicePayment> lPatientInvoicePayment = Context.PatientInvoicePayments.Where(x => x.InvoiceId == PatientInvoice.InvoiceId).ToList();
                if (lPatientInvoicePayment != null && lPatientInvoicePayment.Count>0)
                {
                    foreach(PatientInvoicePayment Payment in lPatientInvoicePayment)
                    {
                        PatientLedger Ledger = Context.PatientLedgers.Find(Payment.LedgerId);
                        if(Ledger!= null)
                        {
                            Ledger.InvoiceAmount -= Ledger.Amount;
                            Ledger.Invoiced = false;
                            Context.Entry(Context.PatientLedgers.Find(Ledger.Id)).CurrentValues.SetValues(Ledger);
                            Context.SaveChanges();
                        }
                        //remove Inv payment
                        Context.PatientInvoicePayments.Remove(Context.PatientInvoicePayments.Find(Payment.Id));
                        Context.SaveChanges();
                    }
                }
                //daybook
                HmsInvoiceDoubleEntryManager.Instance.DeleteInvoices(PatientInvoice, Context);
                //remove invoice
                Context.PatientInvoices.Remove(PatientInvoice);
                Context.SaveChanges();
            }
        }
        public void CreateInvoiceForRegistrationFee(PatientLedger PatientLedger,double PaymentReceived, AccountMasterContext Context)
        {
            PatientInvoice PatientInvoice = new PatientInvoice();
            string RefNum = CompanyManager.Instance.GetIdSpace(Context.Companies.Find(PatientLedger.CompanyId), EntryType.PATIENT_INVOICE, PatientLedger.Date);
            if (!string.IsNullOrEmpty(RefNum))
            {
                PatientInvoice.CompanyId = PatientLedger.CompanyId;
                PatientInvoice.PatientId = PatientLedger.PatientId;
                PatientInvoice.ReferenceNumber = RefNum;
                PatientInvoice.Description = "Payable";
                PatientInvoice.Total = PatientLedger.Amount;
                PatientInvoice.InvoiceDate = PatientLedger.Date;
                Context.PatientInvoices.Add(PatientInvoice);
                Context.SaveChanges();
                if (PatientInvoice.InvoiceId != 0L)
                {
                    PatientLedger PatientLedgerFee = Context.PatientLedgers.FirstOrDefault(x => x.PatientId == PatientLedger.PatientId && x.OpRegistrationId == PatientLedger.OpRegistrationId && x.Type == TransactionType.REGISTRATION_FEE && !x.Invoiced);
                    if(PatientLedgerFee!= null)
                    {
                        PatientLedgerFee.Invoiced = true;
                        PatientLedgerFee.PatientInvoiceId = PatientInvoice.InvoiceId;
                        Context.Entry(Context.PatientLedgers.Find(PatientLedgerFee.Id)).CurrentValues.SetValues(PatientLedgerFee);
                        Context.SaveChanges();
                    }
                }
                //daybook
                HmsInvoiceDoubleEntryManager.Instance.RecordInvoice(PatientInvoice, Context);
            }

        }
        //add invoice from ledger
        public PatientInvoice AddInvoiceFromLedger(PatientInvoice patientInvoice,DateTime FromDate, DateTime ToDate)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        long? NoteId = 0L;
                        IList<PatientLedger> PatientLedgerInfo = Context.PatientLedgers.Where(x => x.PatientId == patientInvoice.PatientId && x.Type != TransactionType.PAYMENT && x.Type != TransactionType.WAIVER && !x.Invoiced && x.Date >= FromDate && x.Date <= ToDate).ToList();
                        if (PatientLedgerInfo != null && PatientLedgerInfo.Count > 0)
                        {
                            patientInvoice.Total = PatientLedgerInfo.Sum(x => x.Amount);
                            double AmountReceived = 0.00;
                            IList<PatientLedger> lPatientLedgerPayment = PatientLedgerManager.Instance.ListPaymentsFromPatientLedgerByPatientId((long)patientInvoice.PatientId);
                            if (lPatientLedgerPayment != null && lPatientLedgerPayment.Count>0)
                            {
                                //payment
                                foreach (PatientLedger Ledger in lPatientLedgerPayment)
                                {
                                    PatientInvoicePayment PatientInvoicePayment = new PatientInvoicePayment();
                                    PatientInvoicePayment.LedgerId = Ledger.Id;
                                    PatientInvoicePayment.Date = patientInvoice.InvoiceDate;
                                    PatientInvoicePayment.Description = "Amount received from Payment " + Ledger.RefNumber;
                                    if ((AmountReceived + Ledger.GetPendingAmount()) <= patientInvoice.Total)
                                    {
                                        PatientInvoicePayment.Amount = Ledger.GetPendingAmount();
                                        patientInvoice.Paid += Ledger.GetPendingAmount();
                                    }
                                    else
                                    {
                                        double AmountNeed = patientInvoice.Total - AmountReceived;
                                        PatientInvoicePayment.Amount = AmountNeed;
                                        patientInvoice.Paid += AmountNeed;
                                    }
                                    //update payment
                                    PatientLedger lLedger = Context.PatientLedgers.Find(Ledger.Id);
                                    if (lLedger != null)
                                    {
                                        lLedger.Invoiced = Ledger.GetPendingAmount() - PatientInvoicePayment.Amount == 0 ? true : false;
                                        lLedger.InvoiceAmount += PatientInvoicePayment.Amount;

                                        Context.Entry(Context.PatientLedgers.Find(lLedger.Id)).CurrentValues.SetValues(lLedger);
                                        Context.SaveChanges();
                                    }
                                    //add payment in invoice
                                    patientInvoice.PatientInvoicePayments.Add(PatientInvoicePayment);

                                    AmountReceived += Ledger.GetPendingAmount();
                                    if (AmountReceived >= patientInvoice.Total)
                                    {
                                        break;
                                    }
                                }
                            }
                            patientInvoice.Balance = patientInvoice.Total - patientInvoice.Paid;
                            //add invoice
                            Context.PatientInvoices.Add(patientInvoice);
                            Context.SaveChanges();
                            //fee
                            foreach (PatientLedger PatientLedger in PatientLedgerInfo)
                            {
                                PatientLedger.Invoiced = true;
                                PatientLedger.PatientInvoiceId = patientInvoice.InvoiceId;
                                //update fee
                                Context.Entry(Context.PatientLedgers.Find(PatientLedger.Id)).CurrentValues.SetValues(PatientLedger);
                                Context.SaveChanges();
                                if (NoteId != null)
                                {
                                    if (NoteId != PatientLedger.ConsultationNoteId)
                                    {
                                        if (PatientLedger.ConsultationNoteId != null)
                                        {
                                            NoteId = PatientLedger.ConsultationNoteId;
                                            ConsultationNoteManager.Instance.UpdateNoteForInvoiceCreation((long)NoteId, Context, true);
                                        }
                                        else
                                        {
                                            NoteId = 0L;
                                        }
                                    }
                                }
                            }
                            //daybook
                            HmsInvoiceDoubleEntryManager.Instance.RecordInvoice(patientInvoice, Context);

                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        dbContextTransaction.Rollback();
                    }
                }
            }
            return patientInvoice;
        }
        public PatientInvoice AddInvoice(PatientInvoice patientInvoice)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        double AmountReceived = 0.00;
                        //getpayment
                        IList<PatientLedger> lPatientLedgerPayment = (from Ledger in Context.PatientLedgers where (Ledger.PatientId == patientInvoice.PatientId && !Ledger.Invoiced  && (Ledger.Type == TransactionType.WAIVER || Ledger.Type == TransactionType.PAYMENT)) select Ledger).OrderBy(x => x.Date).ToList();
                        if (lPatientLedgerPayment != null && lPatientLedgerPayment.Count>0)
                        {
                            foreach (PatientLedger Ledger in lPatientLedgerPayment)
                            {
                                PatientInvoicePayment PatientInvoicePayment = new PatientInvoicePayment();
                                PatientInvoicePayment.LedgerId = Ledger.Id;
                                PatientInvoicePayment.Date = patientInvoice.InvoiceDate;
                                PatientInvoicePayment.Description = "Amount received from Payment " + Ledger.RefNumber;
                                if ((AmountReceived + Ledger.GetPendingAmount()) <= patientInvoice.Total)
                                {
                                    PatientInvoicePayment.Amount = Ledger.GetPendingAmount();
                                    patientInvoice.Paid += Ledger.GetPendingAmount();
                                }
                                else
                                {
                                    double AmountNeed = patientInvoice.Total - AmountReceived;
                                    PatientInvoicePayment.Amount = AmountNeed;
                                    patientInvoice.Paid += AmountNeed;
                                }
                                //add payment for invoice
                                patientInvoice.PatientInvoicePayments.Add(PatientInvoicePayment);
                                //update payment
                                PatientLedger LedgerDB = Context.PatientLedgers.Find(Ledger.Id);
                                if (LedgerDB != null)
                                {
                                    LedgerDB.Invoiced = LedgerDB.GetPendingAmount() - PatientInvoicePayment.Amount == 0 ? true : false;
                                    LedgerDB.InvoiceAmount += PatientInvoicePayment.Amount;

                                    Context.Entry(Context.PatientLedgers.Find(LedgerDB.Id)).CurrentValues.SetValues(LedgerDB);
                                    Context.SaveChanges();
                                }
                                AmountReceived += Ledger.GetPendingAmount();
                                if (AmountReceived >= patientInvoice.Total)
                                {
                                    break;
                                }
                            }
                        }
                        //invoice fees
                        IList<PatientLedger> PatientLedgerInfo = patientInvoice.PatientLedgers.ToList();
                        patientInvoice.PatientLedgers=new List<PatientLedger>();
                        //add invoice
                        Context.PatientInvoices.Add(patientInvoice);
                        Context.SaveChanges();
                        //daybook
                        HmsInvoiceDoubleEntryManager.Instance.RecordInvoice(patientInvoice, Context);
                        long? NoteId = 0L;
                        if (PatientLedgerInfo != null && PatientLedgerInfo.Count > 0)
                        {
                            foreach (PatientLedger PatientLedger in PatientLedgerInfo)
                            {
                                //update fee
                                PatientLedger.Invoiced = true;
                                PatientLedger.PatientInvoiceId = patientInvoice.InvoiceId;
                                if (PatientLedger.Id == 0L)
                                {
                                    Context.PatientLedgers.Add(PatientLedger);
                                }
                                else
                                {
                                    Context.Entry(Context.PatientLedgers.Find(PatientLedger.Id)).CurrentValues.SetValues(PatientLedger);
                                    Context.SaveChanges();
                                    if (NoteId != null)
                                    {
                                        if (NoteId != PatientLedger.ConsultationNoteId)
                                        {
                                            if (PatientLedger.ConsultationNoteId != null)
                                            {
                                                NoteId = PatientLedger.ConsultationNoteId;
                                                ConsultationNoteManager.Instance.UpdateNoteForInvoiceCreation((long)NoteId, Context,true);
                                            }
                                            else
                                            {
                                                NoteId = 0L;
                                            }
                                        }
                                    }
                                }
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
            return patientInvoice;
        }
        public PatientInvoice UpdateInvoice(PatientInvoice patientInvoice)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        List<PatientInvoicePayment> lPatientInvoicePayment = Context.PatientInvoicePayments.Where(x => x.InvoiceId == patientInvoice.InvoiceId).ToList();
                        if (lPatientInvoicePayment != null && lPatientInvoicePayment.Count > 0)
                        {
                            double OldAmountReceived = lPatientInvoicePayment.Sum(x => x.Amount);
                            if (patientInvoice.Total!= OldAmountReceived)
                            {
                                if(patientInvoice.Total> OldAmountReceived)
                                {
                                    patientInvoice= AddPaymentInInvoiceSave(patientInvoice, Context, OldAmountReceived);
                                }
                                else
                                {
                                    patientInvoice = UpdatePaymentInInvoiceSave(patientInvoice, Context, OldAmountReceived, lPatientInvoicePayment);
                                }
                            }
                        }
                        //invoice fees
                        IList<PatientLedger> PatientLedgerInfo = patientInvoice.PatientLedgers.ToList();
                        //update old fee
                        IList<PatientLedger> OldPatientLedgerInfo =Context.PatientLedgers.Where(x=>x.PatientInvoiceId== patientInvoice.InvoiceId).ToList();
                        if (OldPatientLedgerInfo != null && OldPatientLedgerInfo.Count > 0)
                        {
                            foreach (PatientLedger OldLedger in OldPatientLedgerInfo)
                            {
                                PatientLedger NewLedger = PatientLedgerInfo.FirstOrDefault(x => x.Id == OldLedger.Id);
                                if (NewLedger == null)
                                {
                                    OldLedger.Invoiced = false;
                                    OldLedger.PatientInvoiceId = null;
                                    Context.Entry(Context.PatientLedgers.Find(OldLedger.Id)).CurrentValues.SetValues(OldLedger);
                                    Context.SaveChanges();
                                }
                                else
                                {
                                    OldLedger.Amount = NewLedger.Amount;
                                    Context.Entry(Context.PatientLedgers.Find(OldLedger.Id)).CurrentValues.SetValues(OldLedger);

                                    PatientLedgerInfo.Remove(NewLedger);
                                    Context.SaveChanges();
                                }
                            }
                        }
                        //add new fee
                        long? NoteId = 0L;
                        if (PatientLedgerInfo != null && PatientLedgerInfo.Count > 0)
                        {
                            foreach (PatientLedger PatientLedger in PatientLedgerInfo)
                            {
                                PatientLedger.Invoiced = true;
                                PatientLedger.PatientInvoiceId = patientInvoice.InvoiceId;
                                if (PatientLedger.Id == 0L)
                                {
                                    Context.PatientLedgers.Add(PatientLedger);
                                }
                                else
                                {
                                    Context.Entry(Context.PatientLedgers.Find(PatientLedger.Id)).CurrentValues.SetValues(PatientLedger);
                                    Context.SaveChanges();
                                    if (NoteId != null)
                                    {
                                        if (NoteId != PatientLedger.ConsultationNoteId)
                                        {
                                            if (PatientLedger.ConsultationNoteId != null)
                                            {
                                                NoteId = PatientLedger.ConsultationNoteId;
                                                ConsultationNoteManager.Instance.UpdateNoteForInvoiceCreation((long)NoteId, Context,true);
                                            }
                                            else
                                            {
                                                NoteId = 0L;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        patientInvoice.PatientLedgers = new List<PatientLedger>();
                        //Update invoice
                        Context.Entry(Context.PatientInvoices.Find(patientInvoice.InvoiceId)).CurrentValues.SetValues(patientInvoice);
                        Context.SaveChanges();
                        //daybook
                        HmsInvoiceDoubleEntryManager.Instance.RecordInvoice(patientInvoice, Context);
                        
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        dbContextTransaction.Rollback();
                    }
                }
            }
            return patientInvoice;
        }

        private PatientInvoice AddPaymentInInvoiceSave(PatientInvoice patientInvoice,AccountMasterContext Context,double OldAmountReceived)
        {
            double AmountReceived = OldAmountReceived;
            IList<PatientLedger> lPatientLedgerPayment = (from Ledger in Context.PatientLedgers where (Ledger.PatientId == patientInvoice.PatientId && !Ledger.Invoiced && (Ledger.Type == TransactionType.WAIVER || Ledger.Type == TransactionType.PAYMENT)) select Ledger).OrderBy(x => x.Date).ToList();
            if (lPatientLedgerPayment != null && lPatientLedgerPayment.Count > 0)
            {
                foreach (PatientLedger Ledger in lPatientLedgerPayment)
                {
                    PatientInvoicePayment PatientInvoicePayment = new PatientInvoicePayment();
                    PatientInvoicePayment.LedgerId = Ledger.Id;
                    PatientInvoicePayment.Date = patientInvoice.InvoiceDate;
                    PatientInvoicePayment.Description = "Amount received from Payment " + Ledger.RefNumber;
                    if ((AmountReceived + Ledger.GetPendingAmount()) <= patientInvoice.Total)
                    {
                        PatientInvoicePayment.Amount = Ledger.GetPendingAmount();
                        patientInvoice.Paid += Ledger.GetPendingAmount();
                    }
                    else
                    {
                        double AmountNeed = patientInvoice.Total - AmountReceived;
                        PatientInvoicePayment.Amount = AmountNeed;
                        patientInvoice.Paid += AmountNeed;
                    }
                    //add payment for invoice
                    patientInvoice.PatientInvoicePayments.Add(PatientInvoicePayment);
                    //update payment
                    PatientLedger LedgerDB = Context.PatientLedgers.Find(Ledger.Id);
                    if (LedgerDB != null)
                    {
                        LedgerDB.Invoiced = LedgerDB.GetPendingAmount() - PatientInvoicePayment.Amount == 0 ? true : false;
                        LedgerDB.InvoiceAmount += PatientInvoicePayment.Amount;

                        Context.Entry(Context.PatientLedgers.Find(LedgerDB.Id)).CurrentValues.SetValues(LedgerDB);
                        Context.SaveChanges();
                    }
                    AmountReceived += Ledger.GetPendingAmount();
                    if (AmountReceived >= patientInvoice.Total)
                    {
                        break;
                    }
                }
            }
            return patientInvoice;
        }
        private PatientInvoice UpdatePaymentInInvoiceSave(PatientInvoice patientInvoice, AccountMasterContext Context, double OldAmountReceived, List<PatientInvoicePayment> lPatientInvoicePayment)
        {
            double AmountReceived = OldAmountReceived;
            bool Status = false;
            foreach (PatientInvoicePayment PatientInvoicePayment in lPatientInvoicePayment)
            {
                if (!Status)
                {
                    AmountReceived -= PatientInvoicePayment.Amount;
                    if (AmountReceived < patientInvoice.Total)
                    {
                        //update payment for invoice
                        double Difference = patientInvoice.Total - AmountReceived;
                        UpdatePayment((long)PatientInvoicePayment.LedgerId, Difference, Context);
                        UpdateInvoicePayment(PatientInvoicePayment, Difference, Context);                        
                        Status = true;
                    }
                    else
                    {
                        //remove payment from invoice
                        UpdatePayment((long)PatientInvoicePayment.LedgerId, PatientInvoicePayment.Amount, Context);
                        RemoveInvoicePayment(PatientInvoicePayment.Id, Context);
                        if (AmountReceived == patientInvoice.Total)
                        {
                            Status = true;
                        }
                    }
                }
                else
                {
                    //remove payment from invoice
                    UpdatePayment((long)PatientInvoicePayment.LedgerId, PatientInvoicePayment.Amount,Context);
                    RemoveInvoicePayment(PatientInvoicePayment.Id, Context);
                }
            }
            return patientInvoice;
        }
        private void UpdatePayment(long LedgerId, double Amount, AccountMasterContext Context)
        {
            PatientLedger Ledger = Context.PatientLedgers.Find(LedgerId);
            if (Ledger != null)
            {
                Ledger.Invoiced = false;
                Ledger.InvoiceAmount -= Amount;
                Context.Entry(Context.PatientLedgers.Find(Ledger.Id)).CurrentValues.SetValues(Ledger);
                Context.SaveChanges();
            }
        }
        private void UpdateInvoicePayment(PatientInvoicePayment PatientInvoicePayment, double Amount, AccountMasterContext Context)
        {
            PatientInvoicePayment.Amount -= Amount;
            Context.Entry(Context.PatientInvoicePayments.Find(PatientInvoicePayment.Id)).CurrentValues.SetValues(PatientInvoicePayment);
            Context.SaveChanges();
        }
        private void RemoveInvoicePayment(long InvoicePaymentId, AccountMasterContext Context)
        {
            Context.PatientInvoicePayments.Remove(Context.PatientInvoicePayments.Find(InvoicePaymentId));
            Context.SaveChanges();
        }
        public bool DeleteInvoice(long patientInvoiceId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        PatientInvoice PatientInvoiceFromDB = Context.PatientInvoices.Find(patientInvoiceId);
                        if (PatientInvoiceFromDB != null)
                        {
                            List<PatientInvoicePayment> lPatientInvoicePayment = Context.PatientInvoicePayments.Where(x => x.InvoiceId == patientInvoiceId).ToList();
                            if (lPatientInvoicePayment != null && lPatientInvoicePayment.Count > 0)
                            {
                                foreach (PatientInvoicePayment Payment in lPatientInvoicePayment)
                                {
                                    //update payment
                                    UpdatePayment((long)Payment.LedgerId, Payment.Amount, Context);
                                    //remove invoice payment
                                    RemoveInvoicePayment(Payment.Id, Context);
                                }

                            }
                            long? NoteId = 0L;
                            IList<PatientLedger> OldPatientLedgerInfo = Context.PatientLedgers.Where(x => x.PatientInvoiceId == patientInvoiceId).ToList();
                            if (OldPatientLedgerInfo != null && OldPatientLedgerInfo.Count > 0)
                            {
                                foreach (PatientLedger OldLedger in OldPatientLedgerInfo)
                                {
                                    //remove fee from invoice
                                    OldLedger.Invoiced = false;
                                    OldLedger.PatientInvoiceId = null;
                                    Context.Entry(Context.PatientLedgers.Find(OldLedger.Id)).CurrentValues.SetValues(OldLedger);
                                    Context.SaveChanges();
                                    if (NoteId != null)
                                    {
                                        if (NoteId != OldLedger.ConsultationNoteId)
                                        {
                                            if (OldLedger.ConsultationNoteId != null)
                                            {
                                                NoteId = OldLedger.ConsultationNoteId;
                                                if (Context.PatientLedgers.Where(x => x.ConsultationNoteId == NoteId && x.Invoiced).ToList().Count == 0)
                                                {
                                                    ConsultationNoteManager.Instance.UpdateNoteForInvoiceCreation((long)NoteId, Context, false);
                                                }
                                            }
                                            else
                                            {
                                                NoteId = 0L;
                                            }
                                        }
                                    }
                                }
                            }
                            //daybook
                            HmsInvoiceDoubleEntryManager.Instance.DeleteInvoices(PatientInvoiceFromDB, Context);
                            //Remove invoice
                            Context.PatientInvoices.Remove(Context.PatientInvoices.Find(patientInvoiceId));
                            Context.SaveChanges();

                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        dbContextTransaction.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
