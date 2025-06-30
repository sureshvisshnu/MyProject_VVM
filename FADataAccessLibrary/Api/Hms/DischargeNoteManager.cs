using fa.api.Accounting;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.hms.common;
using fa.model.Hms.common;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using FaData.Utils;
using FADataAccessLibrary.Api.Hms;
using FADataAccessLibrary.Model.Hms.common;
using Microsoft.EntityFrameworkCore;
using NPOI.HPSF;
using Org.BouncyCastle.Asn1.IsisMtt.X509;

namespace fa.api.Hms
{
    public class DischargeNoteManager
    {
        private static volatile DischargeNoteManager instance;
        private static object syncRoot = new Object();

        private static string NOTE_DESCRIPTION_FOR_DISCHARGED = "Patient got Discharged"; 
        private static string NOTE_DESCRIPTION_FOR_DECEASED_N_DISCHARGED = "Patient got Discharged (Deceased)";
        DischargeNoteManager()
        {

        }
        public static DischargeNoteManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DischargeNoteManager();
                    }
                }
                return instance;
            }
        }

        public IList<DischargePrescription> ListDischargePrescriptionByPatientIpId(long PatientIpId)
        {
            IList<DischargePrescription> prescriptionInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    var NoteIds = from Note in Context.DischargeNotes where Note.InPatientAdmissionId == PatientIpId select Note.Id;
                    prescriptionInfo = Context.DischargePrescriptions.Include("Prescription").Where(x => NoteIds.Contains(x.DischargeNoteId)).ToList();
                }
                catch (Exception ex) 
                { 
                   Logger.LogError(ex);
                }    
                    return prescriptionInfo;
            }
        }
        public IList<DischargeNote> ListDischargeNoteByPatientId(long PatientId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<DischargeNote> Note = (from Consult in Context.DischargeNotes.Include("Patient") where Consult.PatientId == PatientId select Consult).OrderBy(x => x.Date).ToList();
                return Note;
            }
        }
        public DischargeNote GetLatestDischargeNoteByPatientId(long PatientId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                DischargeNote note = Context.DischargeNotes
                    .Include("Patient")
                    .Where(consult => consult.PatientId == PatientId)
                    .OrderByDescending(consult => consult.Date)
                    .FirstOrDefault();

                return note;
            }
        }
        public DischargeNote GetDischargeStatusByAdmissionId(long AdmissionId)
        {
            DischargeNote DischargeNote = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                DischargeNote = Context.DischargeNotes.Include("InPatientAdmission").Include("Employee").FirstOrDefault(x => x.InPatientAdmissionId == AdmissionId);
                return DischargeNote;
            }
        }
        public DischargeNote GetDischargeNoteByIpId(long IpId)
        {
            DischargeNote DischargeNote = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                DischargeNote = Context.DischargeNotes.Include("InPatientAdmission").Include("Employee").FirstOrDefault(x => x.InPatientAdmissionId == IpId);
                return DischargeNote;
            }
        }
        public DischargeNote GetDischargeNoteByIpId(long IpId, DateTime FromDate, DateTime Todate)
        {
            DischargeNote DischargeNote = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                DischargeNote = Context.DischargeNotes.Include("InPatientAdmission").Include("Employee").FirstOrDefault(x => x.InPatientAdmissionId == IpId && x.DischargeOn >= FromDate && x.DischargeOn <= Todate);
                return DischargeNote;
            }
        }
        public IList<DischargePrescription> ListPrescriptionByDischargeNoteId(long DischargeNoteId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<DischargePrescription> prescriptionInfo = Context.DischargePrescriptions.Include("Prescription").Where(x => x.DischargeNoteId == DischargeNoteId).ToList();
                return prescriptionInfo;
            }
        }
        public IList<DischargePrescription> AddPrescription(ICollection<DischargePrescription> lDischargePrescription, AccountMasterContext Context)
        {
            IList<DischargePrescription> lDischargePrescriptionFromPres = new List<DischargePrescription>();
            foreach (DischargePrescription ConsPres in lDischargePrescription)
            {
                Prescription prescription = ConsPres.Prescription;
                if (prescription != null)
                {
                    Context.Prescriptions.Add(prescription);
                    Context.SaveChanges();

                    ConsPres.PrescriptionId = prescription.Id;
                    lDischargePrescriptionFromPres.Add(ConsPres);
                }
            }
            return lDischargePrescriptionFromPres;
        }
        public DischargeNote UpdateDischargePrescription(AccountMasterContext context, DischargeNote dischargeNote)
        {
            IList<DischargePrescription> dischargePrescriptionFromDB = context.DischargePrescriptions.Include("Prescription").Where(x => x.DischargeNoteId == dischargeNote.Id).ToList();
            foreach (var oldDischargePrescription in dischargePrescriptionFromDB)
            {
                var updatedPrescription = dischargeNote.DischargePrescription.FirstOrDefault(x => x.Prescription.ProductId == oldDischargePrescription.Prescription.ProductId);
                if (updatedPrescription == null)
                {
                    context.DischargePrescriptions.Remove(oldDischargePrescription);
                    context.Prescriptions.Remove(oldDischargePrescription.Prescription);
                }
                else
                {
                    oldDischargePrescription.Prescription.Total = updatedPrescription.Prescription.Total;
                    oldDischargePrescription.Prescription.Days = updatedPrescription.Prescription.Days;
                    oldDischargePrescription.Prescription.Morning = updatedPrescription.Prescription.Morning;
                    oldDischargePrescription.Prescription.Afternoon = updatedPrescription.Prescription.Afternoon;
                    oldDischargePrescription.Prescription.Evening = updatedPrescription.Prescription.Evening;
                    oldDischargePrescription.Prescription.Night = updatedPrescription.Prescription.Night;
                    oldDischargePrescription.Prescription.Hours = updatedPrescription.Prescription.Hours;
                    oldDischargePrescription.Prescription.TakeDosage = updatedPrescription.Prescription.TakeDosage;

                    oldDischargePrescription.IsCustomPrescription = updatedPrescription.IsCustomPrescription;
                    oldDischargePrescription.CustomPrescription = updatedPrescription.CustomPrescription;

                    dischargeNote.DischargePrescription.Remove(updatedPrescription);
                }
            }
            foreach (var newDischargePrescription in dischargeNote.DischargePrescription)
            {
                if (newDischargePrescription.Prescription != null)
                {
                    context.Prescriptions.Add(newDischargePrescription.Prescription);
                    context.SaveChanges();

                    newDischargePrescription.PrescriptionId = newDischargePrescription.Prescription.Id;
                    newDischargePrescription.DischargeNoteId = dischargeNote.Id;
                    context.DischargePrescriptions.Add(newDischargePrescription);
                }
            }
            context.SaveChanges();
            return dischargeNote;
        }
        private void AddNoteInDischarge(DischargeNote DischargeNote,AccountMasterContext Context)
        {
            ConsultationNote ConsultationNote = new ConsultationNote();
            ConsultationNote.InPatientAdmissionId = DischargeNote.InPatientAdmissionId;
            ConsultationNote.PatientId = DischargeNote.PatientId;
            ConsultationNote.Note = (DischargeNote.IsDeceased? NOTE_DESCRIPTION_FOR_DECEASED_N_DISCHARGED: NOTE_DESCRIPTION_FOR_DISCHARGED);
            ConsultationNote.Date = (DateTime)DischargeNote.DischargeOn;
            ConsultationNote.ConsultantId = DischargeNote.UserId;
            ConsultationNote.CompanyId = DischargeNote.CompanyId;
            ConsultationNote.IsDischarged = true;
            ConsultationNote.IsPrescriptionDone = true;
            Context.ConsultationNotes.Add(ConsultationNote);
            Context.SaveChanges();
        }
        public DischargeNote AddDischargeNote(DischargeNote dischargeNote,bool IsDischarged)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        //Add prescription
                        if (dischargeNote.DischargePrescription != null && dischargeNote.DischargePrescription.Count > 0)
                        {
                            dischargeNote.DischargePrescription = AddPrescription(dischargeNote.DischargePrescription, Context);
                        }
                        Context.DischargeNotes.Add(dischargeNote);
                        Context.SaveChanges();

                        if (dischargeNote.Waive)
                        {
                            UpdateLadgerFromDischarge(dischargeNote, Context);                            
                        }
                        Patient PatientInfo = Context.Patients.Find(dischargeNote.PatientId);
                        if (PatientInfo != null)
                        {
                            PatientInfo.IsDeceased = dischargeNote.IsDeceased;
                            PatientManager.Instance.UpdatePatient(PatientInfo, Context);
                        }

                        InPatientLocation InPatientLocationInfo = Context.InPatientLocations.FirstOrDefault(x => x.AdmissionId == dischargeNote.InPatientAdmissionId && x.Active == true);
                        if (InPatientLocationInfo != null)
                        {
                            DateTime startDate;
                            DateTime endDate = DateTime.Now;
                            InPatientAdmission admission = Context.InPatientAdmissions.Find(InPatientLocationInfo.AdmissionId);
                            PatientRoomRent lastRentRecord = Context.PatientRoomRents.Where(x => x.InPatientAdmissionId == admission.Id).OrderByDescending(x => x.DatePosted).FirstOrDefault();

                            if (lastRentRecord != null)
                            {
                                startDate = lastRentRecord.DatePosted;
                            }
                            else
                            {
                                startDate = admission.DateOfAdmission;
                            }
                            while (startDate < endDate)
                            {
                                DateTime nextDay = startDate.Date.AddDays(1);

                                if (nextDay > endDate)
                                {
                                    nextDay = endDate;
                                }
                                PatientRoomRentManager.Instance.CalculateRoomRent(admission, startDate, nextDay, Context);
                                startDate = nextDay;
                            }

                            InPatientLocationInfo.Active = IsDischarged ? false : true;
                            InPatientLocationInfo.DateMovedOut = dischargeNote.Date;
                            Context.Entry(InPatientLocationInfo).CurrentValues.SetValues(InPatientLocationInfo);
                            Context.SaveChanges();
                        }
                        if (IsDischarged)
                        {
                            UpdateDischargeStatus(dischargeNote, Context, IsDischarged);
                            AddNoteInDischarge(dischargeNote, Context);                            
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        dischargeNote = null;
                        dbContextTransaction.Rollback();
                    }
                }
            }
            return dischargeNote;
        }
        public DischargeNote UpdateNotes(DischargeNote DischargeNote, bool IsDischarged)
        {
            DischargeNote NotesInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        NotesInfo = Context.DischargeNotes.Find(DischargeNote.Id);
                        if (NotesInfo != null)
                        {
                            //Update Discharge Prescription
                            DischargeNote = UpdateDischargePrescription(Context, DischargeNote);

                            //add prescription
                            if (DischargeNote.DischargePrescription != null && DischargeNote.DischargePrescription.Count > 0)
                            {
                                DischargeNote.DischargePrescription = AddPrescription(DischargeNote.DischargePrescription, Context);
                                //add new Discharge Prescription
                                foreach (DischargePrescription DischargePrescription in DischargeNote.DischargePrescription)
                                {
                                    DischargePrescription.DischargeNoteId = DischargeNote.Id;
                                    Context.DischargePrescriptions.Add(DischargePrescription);
                                    Context.SaveChanges();
                                }
                            }
                            //update Note
                            DischargeNote.DischargePrescription = null;
                            DischargeNote.InPatientAdmission = null;
                            DischargeNote.Patient = null;
                            DischargeNote.Date = NotesInfo.Date;
                            Context.Entry(Context.DischargeNotes.Find(NotesInfo.Id)).CurrentValues.SetValues(DischargeNote);
                            Context.SaveChanges();

                            if (DischargeNote.Waive)
                            {
                                PatientLedger lPatientLedger = Context.PatientLedgers.FirstOrDefault(x => x.InPatientAdmissionId == DischargeNote.InPatientAdmissionId && x.Type == TransactionType.WAIVER);
                                if (lPatientLedger != null)
                                {
                                    lPatientLedger.Amount = DischargeNote.Amount;
                                    PatientLedgerManager.Instance.UpdateReceivePayment(lPatientLedger, Context);
                                }
                                else
                                {
                                    UpdateLadgerFromDischarge(DischargeNote, Context);                                    
                                }
                            }
                            else
                            {
                                PatientLedger lPatientLedger = Context.PatientLedgers.FirstOrDefault(x => x.InPatientAdmissionId == DischargeNote.InPatientAdmissionId && x.Type == TransactionType.WAIVER);
                                if(lPatientLedger!=null)
                                {
                                    PatientLedgerManager.Instance.DeleteReceivePayment(lPatientLedger, Context);
                                }
                            }
                        }
                        Patient PatientInfo = Context.Patients.Find(DischargeNote.PatientId);
                        if (PatientInfo != null)
                        {
                            PatientInfo.IsDeceased = DischargeNote.IsDeceased;
                            PatientManager.Instance.UpdatePatient(PatientInfo, Context);
                        }
                        if (IsDischarged)
                        {
                            UpdateDischargeStatus(DischargeNote, Context, IsDischarged);
                            AddNoteInDischarge(DischargeNote, Context);
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        NotesInfo = null;
                        dbContextTransaction.Rollback();
                    }
                    return DischargeNote;
                }
                
            }
        }
        private void UpdateDischargeStatus(DischargeNote DischargeNote, AccountMasterContext Context, bool IsDischarged)
        {
            InPatientAdmission InPatientAdmissionInfo = Context.InPatientAdmissions.Find(DischargeNote.InPatientAdmissionId);
            if (InPatientAdmissionInfo != null)
            {
                InPatientAdmissionInfo.Status = InPatientStatus.DISCHARGED;
                IpManager.Instance.UpdateInPatientAdmission(InPatientAdmissionInfo, Context);

                Registration Registration = Context.Registrationes.Find(InPatientAdmissionInfo.OpRegistrationId);
                if (Registration != null)
                {
                    OpManager.Instance.UpdateOpRegistrationFromIP(Registration.Id, Status.COMPLETED, Context);
                }
                InPatientLocation lLocation = Context.InPatientLocations.Where(x => x.AdmissionId == InPatientAdmissionInfo.Id).ToList().Last();
                if (lLocation != null)
                {
                    lLocation.Active = IsDischarged ? false : true;
                    lLocation.DateMovedOut = DischargeNote.Date;
                    Context.Entry(lLocation).CurrentValues.SetValues(lLocation);
                    Context.SaveChanges();

                    Bed Bed = Context.Beds.Find(lLocation.BedId);
                    if (Bed != null)
                    {
                        Bed.BedStatus = BedStatus.AVAILABLE;
                        Context.Entry(Context.Beds.Find(Bed.Id)).CurrentValues.SetValues(Bed);
                        Context.SaveChanges();
                    }
                }
            }
        }
        private void UpdateLadgerFromDischarge(DischargeNote DischargeNote, AccountMasterContext Context)
        {
            Company Company = Context.Companies.Find(DischargeNote.CompanyId);
            if (Company != null)
            {
                string RefNum = CompanyManager.Instance.GetIdSpace(Company, EntryType.PATIENT_FEE_RECEIPT, DischargeNote.Date);
                if (!string.IsNullOrEmpty(RefNum))
                {
                    PatientLedger PatientLedger = new PatientLedger();
                    PatientLedger.RefNumber = RefNum;
                    PatientLedger.Date = DischargeNote.Date;
                    PatientLedger.PatientId = DischargeNote.PatientId;
                    PatientLedger.InPatientAdmissionId = DischargeNote.InPatientAdmissionId;
                    PatientLedger.Amount = DischargeNote.Amount;
                    PatientLedger.Description = "Payment waived in discharge as per ref #" + RefNum;
                    PatientLedger.CompanyId = DischargeNote.CompanyId;
                    PatientLedger.Type = TransactionType.WAIVER;
                    Context.PatientLedgers.Add(PatientLedger);
                    Context.SaveChanges();
                    //update invoice
                    PatientLedgerManager.Instance.UpdateInvoiceByPayment(Context, PatientLedger, 0);
                }
            }
        }

    }
}
