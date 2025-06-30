using fa.context;
using fa.model.Hms.Master;
using fa.report;
using fa.report.common;
using fa.model.Hms.common;
using FaData.Utils;
using Microsoft.EntityFrameworkCore;
using Fa.report.accounting.master;
using fa.model.OrderManagement;
using fa.model.Accounting.Transactions;
using fa.model.Accounting.Masters;

namespace Fa.report.Hms
{
    public class RptPatientLedger:Report
    {
        public long PatientId { get; set; }
        public IList<PatientLedgerLineItem> LineItems { get; } = new List<PatientLedgerLineItem>();
        public double OpeningBalance = 0;
        public double ClosingBalance = 0;
        public long PatientIPId { get; set; }
        public CrDr CreditOrDebit(Double lAmount)
        {
            return (lAmount >= 0 ? CrDr.DR : CrDr.CR);
        }
        public override string ReportTitle()
        {
            return String.Format("Patient Ledger");
        }
        public  string ReportSubTitle()
        {
            return String.Format("From: {0} To: {1}", DateUtils.FormatDate(this.FromDate, Company.DateFormat), DateUtils.FormatDate(this.ToDate, Company.DateFormat));
        }
        public override string ReportName()
        {
            string CurrentDate = DateTime.Now.ToString(Company.DateFormat);
            char Separator = CurrentDate.Contains("-") ? '-' : CurrentDate.Contains("/") ? '/' : '.';
            string[] Date = CurrentDate.Split(Separator);
            return String.Format("Patient Ledger {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        public override void GenerateReport()
        {
            if (PatientId != 0L)
            {
                IList<PatientLedger> PatientLedgerInfo = null;
                using (AccountMasterContext Context = new AccountMasterContext())
                {
                    using (var dbContextTransaction = Context.Database.BeginTransaction())
                    {
                        IList<PatientLedger> PatientLedgerFromDB= Context.PatientLedgers.Where(x => x.PatientId == PatientId && x.Date < this.FromDate).ToList();
                        if (PatientLedgerFromDB != null && PatientLedgerFromDB.Count > 0)
                        {
                            OpeningBalance += PatientLedgerFromDB.Where(x => x.Type != TransactionType.PAYMENT && x.Type != TransactionType.WAIVER).Sum(x => x.Amount);
                            OpeningBalance -= PatientLedgerFromDB.Where(x => x.Type == TransactionType.PAYMENT || x.Type == TransactionType.WAIVER).Sum(x => x.Amount);
                        }
                        List<SaleEntry> SaleEntryFromDB = Context.SaleEntry.Where(x => x.PatientId == PatientId && x.SaleDate.Date < this.FromDate).ToList();
                        if(SaleEntryFromDB!=null && SaleEntryFromDB.Count >0)
                        {
                            OpeningBalance += SaleEntryFromDB.Where(x=> x.EntryType == Entrytype.SALE).Sum(x => x.NetAmount);
                            OpeningBalance -= SaleEntryFromDB.Where(x=>x.SaleMethod==SaleMethod.Cash && x.EntryType==Entrytype.SALE).Sum(x => x.NetAmount);
                        }
                        List<ReceiptDetail> ReceiptDetailFromDB = Context.ReceiptDetails.Where(x => x.InvoiceType == InvoiceType.ItemBased && x.Receipt.TransactionDate < this.FromDate).ToList();
                        if (ReceiptDetailFromDB != null && ReceiptDetailFromDB.Count > 0)
                        {
                            OpeningBalance -= (double)ReceiptDetailFromDB.Where(x => (Context.SaleEntry.Find(long.Parse(x.ReferenceTrasnactionId)).PatientId == PatientId)).Sum(x=>x.Amount);
                        }
                        PatientLedgerInfo = (from Ledger in Context.PatientLedgers.Include("Patient").Include("ConsultedConsultationFee").Include("ConsultedLabTest").Include("ConsultedProcedure") where Ledger.PatientId == PatientId && Ledger.Date >= this.FromDate && Ledger.Date <= ToDate  && Ledger.CompanyId == this.Company.CompanyId select Ledger).ToList().OrderBy(x => x.Date.Date).ThenBy(x=>x.Id).ToList();
                        PatientLedgerLineItem PatientLedgerLineItem = null;
                        
                        List<SequenceLedger> lSequenceLedger = new List<SequenceLedger>();
                        SequenceLedger sequenceLedger = new SequenceLedger
                        {
                            InvId = null,
                            TotalLines = PatientLedgerInfo.Where(x => x.Type != TransactionType.PAYMENT && x.Type != TransactionType.WAIVER && x.PatientInvoiceId == null).ToList().Count
                        };
                        lSequenceLedger.Add(sequenceLedger);
                        long InvId = 0L;
                        foreach (PatientLedger Ledger in PatientLedgerInfo.Where(x=> x.Type != TransactionType.PAYMENT && x.Type != TransactionType.WAIVER && x.PatientInvoiceId!=null).OrderBy(x=>x.PatientInvoiceId).ThenBy(x=>x.Date))
                        {
                            if (Ledger.PatientInvoiceId != InvId)
                            {
                                sequenceLedger = new SequenceLedger
                                {
                                    InvId = (long)Ledger.PatientInvoiceId,
                                    TotalLines = PatientLedgerInfo.Where(x => x.Type != TransactionType.PAYMENT && x.Type != TransactionType.WAIVER && x.PatientInvoiceId == Ledger.PatientInvoiceId).ToList().Count
                                };
                                lSequenceLedger.Add(sequenceLedger);
                                InvId = (long)Ledger.PatientInvoiceId;
                                    
                            }
                        }
                        ClosingBalance = OpeningBalance;
                        int Count = 1;
                        DateTime Date = FromDate;
                        int dayCount = 0;
                        double Amount = 0;
                        string bed = "yyy";
                        int rowCount = 0;
                        foreach (PatientLedger Ledger in PatientLedgerInfo)
                        {
                            int LineCount = PatientLedgerInfo.Where(x => x.RentTimeDuration == "1 day" && x.Bed == Ledger.Bed).Count();
                            if (Ledger.Type != TransactionType.PAYMENT &&
                                Ledger.Type != TransactionType.WAIVER)
                            {
                                if (Ledger.PatientInvoiceId != null)
                                {
                                    SequenceLedger Sequence = lSequenceLedger.FirstOrDefault(x => x.InvId == (long)Ledger.PatientInvoiceId);
                                    if (Sequence != null)
                                    {
                                        Sequence.RunningLines += 1;
                                        if (Ledger.RentTimeDuration == "1 day")
                                        {
                                            rowCount++;
                                            if (bed == "yyy" || Ledger.Bed == bed)
                                            {
                                                dayCount++;
                                                Amount += Ledger.Amount;
                                                bed = Ledger.Bed;
                                                if (rowCount == LineCount)
                                                {
                                                    string Description = "Room rent for staying " + Ledger.Bed + " (" + dayCount + " Days)";
                                                    PatientLedgerLineItem = new PatientLedgerLineItem(Ledger, Context, Description, Amount);
                                                    LineItems.Add(PatientLedgerLineItem);
                                                    Amount = 0;
                                                    rowCount = 0;
                                                }
                                            }
                                            else
                                            {
                                                string Description = "Room rent for staying " + Ledger.Bed + " (" + dayCount + " Days)";
                                                PatientLedgerLineItem = new PatientLedgerLineItem(Ledger, Context, Description, Amount);
                                                LineItems.Add(PatientLedgerLineItem);
                                                bed = Ledger.Bed;
                                                Amount = 0;
                                            }
                                        }
                                        else
                                        {
                                            PatientLedgerLineItem = new PatientLedgerLineItem(Ledger, Context);
                                            LineItems.Add(PatientLedgerLineItem);
                                        }
                                        if (Sequence.TotalLines == Sequence.RunningLines)
                                        {
                                            dayCount++;
                                            Amount += Ledger.Amount;
                                            bed = Ledger.Bed;
                                            rowCount++;
                                            if (rowCount == LineCount)
                                            {
                                                string Description = "Room rent for staying " + Ledger.Bed + " (" + dayCount + " Days)";
                                                PatientLedgerLineItem = new PatientLedgerLineItem(Ledger, Sequence, Context, Description, Amount);
                                                LineItems.Add(PatientLedgerLineItem);
                                                Amount = 0;
                                                rowCount = 0;
                                            }
                                            else
                                            {
                                                PatientLedgerLineItem = new PatientLedgerLineItem(Ledger, Sequence, Context);
                                                LineItems.Add(PatientLedgerLineItem);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    SequenceLedger Sequence = lSequenceLedger.FirstOrDefault(x => x.InvId == null);
                                    if (Sequence != null)
                                    {
                                        Sequence.RunningLines += 1;
                                        if (Sequence.TotalLines == Sequence.RunningLines)
                                        {
                                            dayCount++;
                                            Amount += Ledger.Amount;
                                            bed = Ledger.Bed;
                                            rowCount++;
                                            if (rowCount == LineCount)
                                            {
                                                string Description = "Room rent for staying " + Ledger.Bed + " (" + dayCount + " Days)";
                                                PatientLedgerLineItem = new PatientLedgerLineItem(Ledger, Sequence, Context, Description, Amount);
                                                LineItems.Add(PatientLedgerLineItem);
                                                Amount = 0;
                                                rowCount = 0;
                                            }
                                            else
                                            {
                                                PatientLedgerLineItem = new PatientLedgerLineItem(Ledger, Sequence, Context);
                                                LineItems.Add(PatientLedgerLineItem);
                                            }
                                        }
                                        else
                                        {
                                            if (Ledger.RentTimeDuration == "1 day")
                                            {
                                                rowCount++;
                                                if (bed == "yyy" || Ledger.Bed == bed)
                                                {
                                                    dayCount++;
                                                    Amount += Ledger.Amount;
                                                    bed = Ledger.Bed;
                                                    if (rowCount == LineCount)
                                                    {
                                                        string Description = "Room rent for staying " + Ledger.Bed + " (" + dayCount + " Days)";
                                                        PatientLedgerLineItem = new PatientLedgerLineItem(Ledger, Context, Description, Amount);
                                                        LineItems.Add(PatientLedgerLineItem);
                                                        Amount = 0;
                                                        rowCount = 0;
                                                    }
                                                }
                                                else
                                                {
                                                    string Description = "Room rent for staying " + Ledger.Bed + " (" + dayCount + " Days)";
                                                    PatientLedgerLineItem = new PatientLedgerLineItem(Ledger, Context, Description, Amount);
                                                    LineItems.Add(PatientLedgerLineItem);
                                                    bed = Ledger.Bed;
                                                    Amount = 0;
                                                }
                                            }
                                            else
                                            {
                                                PatientLedgerLineItem = new PatientLedgerLineItem(Ledger, Context);
                                                LineItems.Add(PatientLedgerLineItem);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                PatientLedgerLineItem = new PatientLedgerLineItem(Ledger, Context);
                                LineItems.Add(PatientLedgerLineItem);
                            }
                            ClosingBalance += (Ledger.Type == TransactionType.PAYMENT || Ledger.Type == TransactionType.WAIVER) ? -Ledger.Amount : Ledger.Amount;
                            if(Date!= Ledger.Date.Date)
                            {
                                List<SaleEntry> lSaleEntry = Context.SaleEntry.Where(x => x.PatientId == Ledger.PatientId && x.SaleDate.Date>= Date && x.SaleDate.Date <= (PatientLedgerInfo.Count==Count?ToDate.Date:Ledger.Date.Date)).ToList();
                                if(lSaleEntry!=null && lSaleEntry.Count>0)
                                {
                                    foreach (SaleEntry Entry in lSaleEntry)
                                    {
                                        PatientLedgerLineItem lPatientLedgerLineItem = new PatientLedgerLineItem()
                                        { 
                                            Id = Entry.Id,
                                            Patient = Ledger.Patient,
                                            Date = Entry.SaleDate,
                                            Amount = Entry.EntryType == Entrytype.SALE ? Entry.NetAmount : -Entry.NetAmount,
                                            Description = (Entry.EntryType == Entrytype.SALE ? "To Pharma Invoice #": "To Pharma Return #") + Entry.RefNumber,
                                            Type= Entry.EntryType == Entrytype.SALE ? TransactionType.PHARMACY_FEE: TransactionType.PAYMENT,
                                        };
                                        LineItems.Add(lPatientLedgerLineItem);
                                        ClosingBalance +=  lPatientLedgerLineItem.Amount;

                                        if (Entry.EntryType == Entrytype.SALE && Entry.SaleMethod==SaleMethod.Cash)
                                        {
                                            lPatientLedgerLineItem = new PatientLedgerLineItem()
                                            {
                                                Id = Entry.Id,
                                                Patient = Ledger.Patient,
                                                Date = Entry.SaleDate,
                                                Amount = Entry.EntryType == Entrytype.SALE ? -Entry.NetAmount : Entry.NetAmount,
                                                Description = (Entry.EntryType == Entrytype.SALE ? "Cash payment received\nAgainst pharma invoice #" : "Cash paid\nAgainst pharma Return #") + Entry.RefNumber,
                                                Type = Entry.EntryType == Entrytype.SALE ? TransactionType.PAYMENT: TransactionType.PHARMACY_FEE,
                                            };
                                            LineItems.Add(lPatientLedgerLineItem);
                                            ClosingBalance += lPatientLedgerLineItem.Amount;
                                        }
                                    }
                                }
                                List<ReceiptDetail> lReceiptDetail = Context.ReceiptDetails.Include("Receipt").Where(x =>x.InvoiceType==InvoiceType.ItemBased && x.Receipt.TransactionDate >= Date && x.Receipt.TransactionDate <= (PatientLedgerInfo.Count == Count ? ToDate : Ledger.Date.Date)).ToList();
                                if (lReceiptDetail != null && lReceiptDetail.Count > 0)
                                {
                                    foreach (ReceiptDetail Detail in lReceiptDetail)
                                    {
                                        SaleEntry lEntry = Context.SaleEntry.FirstOrDefault(x =>x.Id == long.Parse(Detail.ReferenceTrasnactionId) && x.PatientId == Ledger.PatientId);
                                        if (lEntry != null)
                                        {
                                            PatientLedgerLineItem lPatientLedgerLineItem = new PatientLedgerLineItem()
                                            {
                                                Id = Detail.ReceiptDetailId,
                                                Patient = Ledger.Patient,
                                                Date = Detail.Receipt.TransactionDate,
                                                Amount = (double)-Detail.Amount,
                                                Description = "Payment received as per ref #" + Detail.Receipt.Reference + "\nAgainst pharma invoice #" + lEntry.RefNumber,
                                                Type = TransactionType.PAYMENT,
                                            };
                                            LineItems.Add(lPatientLedgerLineItem);
                                            ClosingBalance +=  lPatientLedgerLineItem.Amount;

                                        }
                                    }
                                }
                                Date = Ledger.Date.Date.AddDays(1);
                            }
                            Count++;
                        }
                    }
                }
            }

        }
    }
    public class SequenceLedger
    {
        public long? InvId { get; set; }
        public int TotalLines { get; set; }
        public int RunningLines { get; set; }

    }
    public class PatientLedgerLineItem
    {
        public TransactionType Type { get; set; }
        public Patient Patient { get; set; }
        public DateTime Date { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public string RefNumber { get; set; }
        public double Amount { get; set; }
        public bool IsPntInv { get; set; }
        public bool IsCrtInv { get; set; }
        public long? InvId { get; set; }
        public long? OPId { get; set; }
        public long? IPId { get; set; }
        public long Id { get; set; }

        public PatientLedgerLineItem()
        {

        }
        public PatientLedgerLineItem(PatientLedger Ledger, AccountMasterContext Context)
        {
            this.Id = Ledger.Id;
            this.Patient = Ledger.Patient;
            this.Date = Ledger.Date;
            string AddDesc = string.Empty;
            if(Ledger.Type == TransactionType.PAYMENT)
            {
                AddDesc = AdditionalDescription(Ledger.Id, Context);
            }
            this.Description = Ledger.Type==TransactionType.PAYMENT ? Ledger.Description+(string.IsNullOrEmpty(AddDesc) ?"": ("\n"+ AddDesc)) : (Ledger.ConsultedConsultationId != null ? Ledger.ConsultedConsultationFee.Description : Ledger.ConsultedProcedureId != null ? Ledger.ConsultedProcedure.Description : Ledger.Description);
            this.Name = Ledger.ConsultedConsultationFee != null ? Ledger.ConsultedConsultationFee.Name : Ledger.ConsultedLabTest != null ? Ledger.ConsultedLabTest.Name : Ledger.ConsultedProcedure != null ? Ledger.ConsultedProcedure.Name : string.Empty;
            this.Amount = (Ledger.Type == TransactionType.PAYMENT || Ledger.Type == TransactionType.WAIVER) ? -Ledger.Amount : Ledger.Amount;
            this.RefNumber = Ledger.RefNumber;
            this.Type = Ledger.Type;
            this.InvId = Ledger.PatientInvoiceId;
            this.OPId = Ledger.OpRegistrationId;
            this.IPId = Ledger.InPatientAdmissionId;

        }
        public PatientLedgerLineItem(PatientLedger Ledger, AccountMasterContext Context, string Description, double Amount)
        {
            this.Id = Ledger.Id;
            this.Patient = Ledger.Patient;
            this.Date = Ledger.Date;
            string AddDesc = string.Empty;
            if(Ledger.Type == TransactionType.PAYMENT)
            {
                AddDesc = AdditionalDescription(Ledger.Id, Context);
            }
            this.Description = Description;
            this.Name = Ledger.ConsultedConsultationFee != null ? Ledger.ConsultedConsultationFee.Name : Ledger.ConsultedLabTest != null ? Ledger.ConsultedLabTest.Name : Ledger.ConsultedProcedure != null ? Ledger.ConsultedProcedure.Name : string.Empty;
            this.Amount = Amount;
            this.RefNumber = Ledger.RefNumber;
            this.Type = Ledger.Type;
            this.InvId = Ledger.PatientInvoiceId;
            this.OPId = Ledger.OpRegistrationId;
            this.IPId = Ledger.InPatientAdmissionId;

        }
        public PatientLedgerLineItem(PatientLedger Ledger, SequenceLedger Sequence, AccountMasterContext Context)
        {
            if (Sequence.InvId == null)
            {
                this.Id = Ledger.Id;
                this.Patient = Ledger.Patient;
                this.Date = Ledger.Date;
                this.Description = Ledger.ConsultedConsultationId != null ? Ledger.ConsultedConsultationFee.Description : Ledger.ConsultedProcedureId != null ? Ledger.ConsultedProcedure.Description : Ledger.Description;
                this.Name = Ledger.ConsultedConsultationFee != null ? Ledger.ConsultedConsultationFee.Name : Ledger.ConsultedLabTest != null ? Ledger.ConsultedLabTest.Name : Ledger.ConsultedProcedure != null ? Ledger.ConsultedProcedure.Name : string.Empty;
                this.Amount = Ledger.Amount;
                this.RefNumber = Ledger.RefNumber;
                this.Type = Ledger.Type;
                this.IsCrtInv = true;
                this.InvId = Ledger.PatientInvoiceId;
                this.OPId = Ledger.OpRegistrationId;
                this.IPId = Ledger.InPatientAdmissionId;
            }
            else
            {
                PatientInvoice PatientInvoiceInfo = Context.PatientInvoices.FirstOrDefault(x => x.InvoiceId == Sequence.InvId);
                if (PatientInvoiceInfo != null)
                {
                    this.Id = Ledger.Id;
                    this.Patient = Ledger.Patient;
                    this.Date = Ledger.Date;
                    this.Description = "To Invoice #" + PatientInvoiceInfo.ReferenceNumber;
                    this.Amount = PatientInvoiceInfo.Total;
                    this.RefNumber = PatientInvoiceInfo.ReferenceNumber;
                    this.Type = Ledger.Type;
                    this.IsPntInv = true;
                    this.InvId = Ledger.PatientInvoiceId;
                    this.OPId = Ledger.OpRegistrationId;
                    this.IPId = Ledger.InPatientAdmissionId;
                }
            }

        }
        public PatientLedgerLineItem(PatientLedger Ledger, SequenceLedger Sequence, AccountMasterContext Context, string Description, double Amount)
        {
            if (Sequence.InvId == null)
            {
                this.Id = Ledger.Id;
                this.Patient = Ledger.Patient;
                this.Date = Ledger.Date;
                this.Description = Description;
                this.Name = Ledger.ConsultedConsultationFee != null ? Ledger.ConsultedConsultationFee.Name : Ledger.ConsultedLabTest != null ? Ledger.ConsultedLabTest.Name : Ledger.ConsultedProcedure != null ? Ledger.ConsultedProcedure.Name : string.Empty;
                this.Amount = Amount;
                this.RefNumber = Ledger.RefNumber;
                this.Type = Ledger.Type;
                this.IsCrtInv = true;
                this.InvId = Ledger.PatientInvoiceId;
                this.OPId = Ledger.OpRegistrationId;
                this.IPId = Ledger.InPatientAdmissionId;
            }
            else
            {
                PatientInvoice PatientInvoiceInfo = Context.PatientInvoices.FirstOrDefault(x => x.InvoiceId == Sequence.InvId);
                if (PatientInvoiceInfo != null)
                {
                    this.Id = Ledger.Id;
                    this.Patient = Ledger.Patient;
                    this.Date = Ledger.Date;
                    this.Description = "To Invoice #" + PatientInvoiceInfo.ReferenceNumber;
                    this.Amount = PatientInvoiceInfo.Total;
                    this.RefNumber = PatientInvoiceInfo.ReferenceNumber;
                    this.Type = Ledger.Type;
                    this.IsPntInv = true;
                    this.InvId = Ledger.PatientInvoiceId;
                    this.OPId = Ledger.OpRegistrationId;
                    this.IPId = Ledger.InPatientAdmissionId;
                }
            }

        }
        private string AdditionalDescription(long LedgId,AccountMasterContext Context)
        {
            string Desc = string.Empty;
            IList<PatientInvoicePayment> lPatientInvoicePayment = Context.PatientInvoicePayments.Include("PatientInvoice").Where(x=>x.LedgerId==LedgId).ToList();
            if(lPatientInvoicePayment!=null)
            {
                foreach(PatientInvoicePayment Payment in lPatientInvoicePayment)
                {
                    Desc += string.IsNullOrEmpty(Desc) ? "Against invoice #"+ Payment.PatientInvoice.ReferenceNumber : ", #"+ Payment.PatientInvoice.ReferenceNumber;
                }
            }
            return Desc;
        }
        public CrDr CreditOrDebit()
        {
            return (this.Amount  >= 0 ? CrDr.DR : CrDr.CR);
        }
    }
}

