using fa.context;
using fa.model.hms.common;
using fa.model.Hms.Master;
using fa.model.Hms.Ip;
using fa.model.Hms.Op;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using fa.api.Accounting;
using fa.model.Accounting.Masters;
using fa.model.Employee;
using fa.model.Catalog;
using MySqlConnector;
using System.Data;

namespace fa.api.Hms
{
    public class ConsultationNoteManager
    {
        private static volatile ConsultationNoteManager instance;
        private static object syncRoot = new Object();
        ConsultationNoteManager()
        {

        }
        public static ConsultationNoteManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ConsultationNoteManager();
                    }
                }
                return instance;
            }
        }


        public ConsultationNote GetConsultationNoteById(long NoteId)
        {
            ConsultationNote ConsultationNote = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConsultationNote = Context.ConsultationNotes.Include("Consultant").FirstOrDefault(x => x.Id == NoteId);
                return ConsultationNote;
            }
        }
        public ConsultationNote GetConsultationNoteBySaleId(long SaleId)
        {
            ConsultationNote ConsultationNote = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConsultationNote = Context.ConsultationNotes.Include("Consultant").FirstOrDefault(x => x.SaleEntryId == SaleId);
                return ConsultationNote;
            }
        }
        public ConsultationNote GetConsultationNoteByOPRegisterId(long OpRegistrationId)
        {
            ConsultationNote ConsultationNote = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConsultationNote = Context.ConsultationNotes.Include("Consultant").FirstOrDefault(x => x.OpRegistrationId == OpRegistrationId);
                return ConsultationNote;
            }
        }
        public IList<ConsultationNote> ListNotesSuggesionByCompanyId(long CompanyId, string Stxt)
        {
            DataTable dataTable = new DataTable();
            IList<ConsultationNote> lConsultationNoteInfo = new List<ConsultationNote>();

            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    string connectionString = Context.Database.GetDbConnection().ConnectionString;
                    string query = null;
                    if (string.IsNullOrEmpty(Stxt))
                    {
                        // You can add the default query here if needed
                    }
                    else
                    {
                        query = "SELECT * FROM ConsultationNotes WHERE Name LIKE @Filter AND CompanyId = @CompanyIds";
                    }

                    if (!string.IsNullOrEmpty(query))
                    {
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();
                            using (MySqlCommand command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@Filter", "%" + Stxt + "%");
                                command.Parameters.AddWithValue("@CompanyIds", CompanyId);

                                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                                {
                                    adapter.Fill(dataTable);
                                }
                            }
                        }
                    }

                    foreach (DataRow row in dataTable.Rows)
                    {
                        ConsultationNote note = new ConsultationNote
                        {
                            // Map DataRow to ConsultationNote properties here
                            // Example:
                            // Id = Convert.ToInt64(row["Id"]),
                            // Name = row["Name"].ToString(),
                            // CompanyId = Convert.ToInt64(row["CompanyId"]),
                            // Other properties...
                        };
                        lConsultationNoteInfo.Add(note);
                    }
                }
                catch (MySqlException ex)
                {
                    int errorCode = ex.Number;
                    if (ex.HResult == -2147467259)
                    {
                        Console.WriteLine("Query was stopped: " + ex.HResult);
                    }
                }
            }

            return lConsultationNoteInfo;
        }

        public DataTable ListNotesSuggesionsByCompanyId(long CompanyId, string Stxt)
        {
            DataTable dataTable = new DataTable();
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    string connectionString = Context.Database.GetDbConnection().ConnectionString;
                    string query = null;
                    if (!string.IsNullOrEmpty(Stxt))
                    {
                        query = "SELECT Note FROM ConsultationNotes WHERE Note LIKE @Filter AND CompanyId = @CompanyIds";
                    }

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            if (!string.IsNullOrEmpty(Stxt))
                            {
                                command.Parameters.AddWithValue("@Filter", "%" + Stxt + "%");
                            }
                            command.Parameters.AddWithValue("@CompanyIds", CompanyId);

                            using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                            {
                                adapter.Fill(dataTable);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(Stxt))
                    {
                        var distinctNotes = dataTable.AsEnumerable()
                                                     .GroupBy(row => row.Field<string>("Note"))
                                                     .Select(group => group.First())
                                                     .CopyToDataTable();
                        return distinctNotes;
                    }
                }
                catch (MySqlException ex)
                {
                    int errorCode = ex.Number;
                    if (ex.HResult == -2147467259)
                    {
                        Console.WriteLine("Query was stopped: " + ex.HResult);
                    }
                }
            }
            return dataTable;
        }

        public IList<ConsultationNote> ListNotesEntryByCompanyId(long companyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ConsultationNote> Note = (from Consult in Context.ConsultationNotes.Include("Patient") where Consult.CompanyId == companyId select Consult).OrderBy(x => x.Date).ToList();
                Note = Note.GroupBy(x => x.Note).Select(group => group.First()).ToList();
                return Note;
            }
        }
        public IList<ConsultationNote> ListConsultationNoteByOPIdIPId(long OpRegistrationId, long IpRegistrationId)
        {
            IList < ConsultationNote> ConsultationNote = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConsultationNote = Context.ConsultationNotes.Include("Consultant").Where(x => x.OpRegistrationId == OpRegistrationId|| x.InPatientAdmissionId == IpRegistrationId).ToList();
                return ConsultationNote;
            }
        }
        public ConsultationNote GetConsultationNoteByPatientIdDate(long PatientId, DateTime Date)
        {
            ConsultationNote ConsultationNote = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConsultationNote = Context.ConsultationNotes.Include("Consultant").FirstOrDefault(x => x.PatientId == PatientId && x.Date.Day == Date.Date.Day && x.Date.Month == Date.Date.Month && x.Date.Year == Date.Year);
                return ConsultationNote;
            }
        }

        public Prescription GetPrescriptionById(long PresId)
        {
            Prescription Prescription = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Prescription = Context.Prescriptions.Include("Product").FirstOrDefault(x => x.Id == PresId);
                return Prescription;
            }
        }
        public ConsultedProcedure GetConsultedProceduresById(long ConsltProcedureId)
        {
            ConsultedProcedure ConsultedProcedure = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConsultedProcedure = Context.ConsultedProcedures.Include("RequestedBy").Include("MedicalProcedure").Include("ConsultationNote").FirstOrDefault(x => x.ConsultedProcedureId == ConsltProcedureId);
                return ConsultedProcedure;
            }
        }
        public ConsultedProcedure GetConsultedProcedureById(long ConsultProcedureId)
        {
            ConsultedProcedure ConsultedProcedure = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConsultedProcedure = Context.ConsultedProcedures.Include("MedicalProcedure").FirstOrDefault(x => x.ConsultedProcedureId == ConsultProcedureId);
                return ConsultedProcedure;
            }
        }
        public ConsultedConsultationFee GetConsultedConsultationFeeById(long ConsultedConsultationId)
        {
            ConsultedConsultationFee ConsultationFee = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConsultationFee = Context.ConsultedConsultationFees.Include("Consultation").Include("ConsultationNote").FirstOrDefault(x => x.ConsultedConsultationId == ConsultedConsultationId);
                return ConsultationFee;
            }
        }
        public ConsultedLabTest GetConsultedLabTestsById(long ConsLabTestEleId)
        {
            ConsultedLabTest ConsultedLabTest = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConsultedLabTest = Context.ConsultedLabTests.Include("MedicalTest").Include("ConsultedLabTestElements").FirstOrDefault(x => x.ConsultedLabTestId == ConsLabTestEleId);
                return ConsultedLabTest;
            }
        }
        public ConsultedLabTest GetConsultedLabTestsByMedicalTestId(long MedicalTestId)
        {
            ConsultedLabTest ConsultedLabTest = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConsultedLabTest = Context.ConsultedLabTests.Include("MedicalTest").Include("ConsultedLabTestElements").FirstOrDefault(x => x.MedicalTestId == MedicalTestId);
                return ConsultedLabTest;
            }
        }
        public ConsultedLabTest GetMedicalTestsByConsultedLabTestId(long ConsLabTestId)
        {
            ConsultedLabTest ConsultedLabTest = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConsultedLabTest = Context.ConsultedLabTests.Include("MedicalTest").FirstOrDefault(x => x.ConsultedLabTestId == ConsLabTestId);
                return ConsultedLabTest;
            }
        }
        public IList< ConsultedLabTest> ListMedicalTestsByConsultedLabTestId(long NoteId, long  ConsLabTestId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ConsultedLabTest> ConsultedLabTestInfo = Context.ConsultedLabTests.Include("ConsultedLabTestElements").Include("MedicalTest").Where(x => x.ConsultationNoteId == NoteId && x.ConsultedLabTestId == ConsLabTestId).ToList();
                return ConsultedLabTestInfo;
            }
        }

        public ConsultedLabTestElements GetConsultedLabTestElementsById(long ConsLabTestEleId)
        {
            ConsultedLabTestElements ConsultedLabTestElements = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConsultedLabTestElements = Context.ConsultedLabTestElements.Include("Uom").Include("MedicalTestElement").FirstOrDefault(x => x.Id == ConsLabTestEleId);
                return ConsultedLabTestElements;
            }
        }
        public List<ConsultedLabTestElements> GetConsultedLabTestElementsByConsLabTestId(long ConsLabTestEleId)
        {
            List<ConsultedLabTestElements> ConsultedLabTestElements = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConsultedLabTestElements = Context.ConsultedLabTestElements.Include("Uom").Where(x => x.ConsLabTestId == ConsLabTestEleId).ToList();
                return ConsultedLabTestElements;
            }
        }
        public ConsultedLabTestElements GetConsultedLabTestElementsByMedicalTestElementId(long LabTestEleId)
        {
            ConsultedLabTestElements ConsultedLabTestElements = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConsultedLabTestElements = Context.ConsultedLabTestElements.FirstOrDefault(x => x.MedicalTestElementId == LabTestEleId);
                return ConsultedLabTestElements;
            }
        }
        public IList<ConsultedLabTestElements> ListConsultedLabTestElementsByLabtestId(long ConsLabTestId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ConsultedLabTestElements> ConsultedLabTestElementsInfo = Context.ConsultedLabTestElements.Include("Uom").Where(x => x.ConsLabTestId == ConsLabTestId).ToList();
                return ConsultedLabTestElementsInfo;
            }
        }
        public IList<LabTestAttachment> ListConsultedLabTestAttachmentByLabtestId(long ConsLabTestId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<LabTestAttachment> LabTestAttachmentInfo = Context.LabTestAttachments.Where(x => x.ConsLabTestId == ConsLabTestId).ToList();
                return LabTestAttachmentInfo;
            }
        }
        public ConsultedConsultationFee GetConsultationByNoteId(long NoteId, long ConsultedId)
        {
            ConsultedConsultationFee ConsultedConsultationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConsultedConsultationInfo = Context.ConsultedConsultationFees.Include("Consultation").FirstOrDefault(x => x.ConsultationNoteId == NoteId && x.ConsultedConsultationId == ConsultedId);
                return ConsultedConsultationInfo;
            }
        }
        public IList<ConsultedConsultationFee> ListConsultationByNoteId(long NoteId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ConsultedConsultationFee> ConsultedConsultationInfo = Context.ConsultedConsultationFees.Include("Consultation").Include("ConsultationNote").Where(x => x.ConsultationNoteId == NoteId).ToList();
                return ConsultedConsultationInfo;
            }
        }
        public IList<ConsultedProcedure> LisConsultedProcedureByNoteId(long NoteId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ConsultedProcedure> ConsultedProcedureInfo = Context.ConsultedProcedures.Include("RequestedBy").Include("MedicalProcedure").Where(x => x.ConsultationNoteId == NoteId).ToList();
                return ConsultedProcedureInfo;
            }
        }
        public IList<ConsultedLabTest> ListLabTestByNoteId(long NoteId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ConsultedLabTest> ConsultedLabTestInfo = Context.ConsultedLabTests.Include("ConsultedLabTestElements").Include("MedicalTest").Where(x => x.ConsultationNoteId == NoteId).ToList();
                return ConsultedLabTestInfo;
            }
        }
        public IList<ConsultedLabTest> ListLabTestByConsultationLabTestId(long NoteId, long LabTestId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ConsultedLabTest> ConsultedLabTestInfo = Context.ConsultedLabTests.Include("ConsultedLabTestElements").Include("MedicalTest").Where(x => x.ConsultationNoteId == NoteId && x.MedicalTestId == LabTestId).ToList();
                return ConsultedLabTestInfo;
            }
        }
        public IList<ConsultedLabTest> ListConsLabTestByPatientId(long PatientId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var NoteIds = from Note in Context.ConsultationNotes where Note.PatientId == PatientId select Note.Id;
                IList<ConsultedLabTest> ConsultedLabTestInfo = Context.ConsultedLabTests.Include("MedicalTest").Where(x => NoteIds.Contains(x.ConsultationNoteId)).ToList();
                return ConsultedLabTestInfo;
            }
        }
        public IList<ConsultedLabTest> ListConsLabTestByPatientIdForCurrentIp(long PatientId,long IpId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var NoteIds = from Note in Context.ConsultationNotes where Note.PatientId == PatientId && Note.InPatientAdmissionId==IpId select Note.Id;
                IList<ConsultedLabTest> ConsultedLabTestInfo = Context.ConsultedLabTests.Include("ConsultationNote").Include("MedicalTest").Where(x => NoteIds.Contains(x.ConsultationNoteId)).ToList();
                return ConsultedLabTestInfo;
            }
        }
        public IList<ConsultedLabTest> ListConsultedLabTestByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ConsultedLabTest> ConsultedLabTestInfo = (from ConsultedLabTest in Context.ConsultedLabTests where ConsultedLabTest.CompanyId == CompanyId select ConsultedLabTest).ToList();
                return ConsultedLabTestInfo;
            }
        }
        public IList<ConsultedAllergie> ListAllergieByNoteId(long NoteId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ConsultedAllergie> AllergieInfo = Context.ConsultedAllergies.Include("Allergie").Where(x => x.ConsultationNoteId == NoteId).ToList();
                return AllergieInfo;
            }
        }
        public IList<ConsultedSymptom> ListSymptomByNoteId(long NoteId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ConsultedSymptom> SymptomInfo = Context.ConsultedSymptoms.Include("Symptom").Where(x => x.ConsultationNoteId == NoteId).ToList();
                return SymptomInfo;
            }
        }
        public bool UpdateConsultedProcedureDetails(IList<ConsultedProcedureHistory> ConsultedProcedureHistory, long ConsltProcedureId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        IList<ConsultedProcedureHistory> OldConsultedProcedureHistory = Context.ConsultedProcedureHistorys
                            .Include("PerformedBy")
                            .Include("ConsultedProcedure")
                            .Where(x => x.ConsultedProcedureId == ConsltProcedureId)
                            .ToList();
                        if (ConsultedProcedureHistory.Count == 0 && OldConsultedProcedureHistory.Count == 0)
                        {
                            return false;
                        }
                        foreach (ConsultedProcedureHistory oldHistory in OldConsultedProcedureHistory)
                        {
                            var newHistory = ConsultedProcedureHistory.FirstOrDefault(x => x.Id == oldHistory.Id);
                            if (newHistory == null)
                            {
                                Context.ConsultedProcedureHistorys.Remove(oldHistory);
                            }
                            else
                            {
                                newHistory.PerformedById = oldHistory.PerformedById;
                                Context.Entry(oldHistory).CurrentValues.SetValues(newHistory);
                                ConsultedProcedureHistory.Remove(newHistory);
                            }
                        }
                        foreach (ConsultedProcedureHistory newHistory in ConsultedProcedureHistory)
                        {
                            Context.ConsultedProcedureHistorys.Add(newHistory);
                        }
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public IList<ConsultedProcedureHistory> ListConsultedProcedureHistoryById(long ConsltProcedureId, long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ConsultedProcedureHistory> ConsultedProcedureHistory = Context.ConsultedProcedureHistorys.Include("PerformedBy").Include("ConsultedProcedure").Where(x => x.ConsultedProcedureId == ConsltProcedureId && x.CompanyId == CompanyId).ToList();
                return ConsultedProcedureHistory;
            }
        }
        public Prescription GetPrescriptionByProductId(long ProductId)
        {
            Prescription Prescription = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Prescription = Context.Prescriptions.FirstOrDefault(x => x.ProductId == ProductId);
            }
            return Prescription;
        }
        public IList<ConsultedProcedure> ListProcedureByNoteId(long NoteId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ConsultedProcedure> ProcedureInfo = null;
                ProcedureInfo = Context.ConsultedProcedures.Include("MedicalProcedure").Include("RequestedBy").Include("PerformedBy").Where(x => x.ConsultationNoteId == NoteId).ToList();
                return ProcedureInfo;
            }
        }
       
        public IList<ConsultedPrescription> ListPrescriptionByNoteId(long NoteId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ConsultedPrescription> prescriptionInfo = Context.ConsultedPrescriptions.Include("Prescription").Where(x => x.ConsultationNoteId == NoteId).ToList();
                return prescriptionInfo;
            }
        }
        public IList<ConsultedPrescription> ListConsPrescriptionByPatientId(long PatientId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var NoteIds = from Note in Context.ConsultationNotes where Note.PatientId == PatientId select Note.Id;
                IList<ConsultedPrescription> prescriptionInfo = Context.ConsultedPrescriptions.Include("Prescription").Where(x => NoteIds.Contains(x.ConsultationNoteId)).ToList();
                return prescriptionInfo;
            }
        }
        public IList<ConsultedPrescription> ListConsPrescriptionByPatientIdForCurrentIp(long PatientId,long IPId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var NoteIds = from Note in Context.ConsultationNotes where Note.PatientId == PatientId && Note.InPatientAdmissionId==IPId select Note.Id;
                IList<ConsultedPrescription> prescriptionInfo = Context.ConsultedPrescriptions.Include("Prescription").Where(x => NoteIds.Contains(x.ConsultationNoteId)).ToList();
                return prescriptionInfo;
            }
        }
        public IList<ConsultationNote> ListNotesEntryByPatientId(long PatientId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ConsultationNote> Note = (from Consult in Context.ConsultationNotes.Include("Patient") where Consult.PatientId == PatientId select Consult).OrderBy(x => x.Date).ToList();
                return Note;
            }
        }
        public IList<ConsultationNote> ListNotesEntryByMedicalTest(long PatientId, long ConsultationNoteId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ConsultationNote> Note = (from Consult in Context.ConsultationNotes.Include("Patient") where Consult.PatientId == PatientId && Consult.Id == ConsultationNoteId select Consult).OrderBy(x => x.Date).ToList();
                return Note;
            }
        }
        public IList<ConsultationNote> ListUnCompletedNotesEntryByPatientId(long PatientId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ConsultationNote> Note = (from Consult in Context.ConsultationNotes.Include("Patient") where Consult.PatientId == PatientId && !(Consult.InPatientAdmissionId != null && (Context.InPatientAdmissions.FirstOrDefault(n => n.Id == Consult.InPatientAdmissionId).Status == InPatientStatus.DISCHARGED)) && !(Consult.OpRegistrationId != null && (Context.Registrationes.FirstOrDefault(n => n.Id == Consult.OpRegistrationId).Status == Status.COMPLETED)) select Consult).OrderBy(x => x.Date).ToList();
                return Note;
            }
        }
        public IList<ConsultationNote> ListNotesEntryByPatientIdIpId(long PatientId, long PatientIpId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ConsultationNote> Note = (from Consult in Context.ConsultationNotes.Include("Patient") where Consult.PatientId == PatientId && Consult.InPatientAdmissionId == PatientIpId select Consult).OrderBy(x => x.Date).ToList();
                return Note;
            }
        }
        public IList<ConsultationNote> ListNotesEntryByCompany(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ConsultationNote> Note = (from Consult in Context.ConsultationNotes.Include("Patient") where Consult.CompanyId == CompanyId && Consult.IsPrescriptionDispatchedForMedical == true select Consult).OrderBy(x => x.Date).ToList();
                return Note;
            }
        }
        public IList<ConsultationNote> ListNotesEntryBySearchText(string SearchText, long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ConsultationNote> Note = (from Consult in Context.ConsultationNotes.Include("Patient") where (from Patient in Context.Patients where (Patient.FirstName.Contains(SearchText) || Patient.LastName.Contains(SearchText) || Patient.MiddleInitial.Contains(SearchText) || Patient.PatientNumber.Contains(SearchText)) && Consult.CompanyId == CompanyId && Consult.IsPrescriptionDispatchedForMedical == true select Patient.Id).Contains(Consult.PatientId) select Consult).OrderBy(x => x.Date).ToList();
                return Note;
            }
        }
        public IList<ConsultationNote> ListNotesEntryByCurrentDate(DateTime Date, long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ConsultationNote> Note = (from Consult in Context.ConsultationNotes.Include("Patient") where Consult.Date.Day == Date.Date.Day && Consult.Date.Month == Date.Date.Month && Consult.Date.Year == Date.Year && Consult.CompanyId == CompanyId && Consult.IsPrescriptionDispatchedForMedical == true select Consult).OrderBy(x => x.Date).ToList();
                return Note;
            }
        }

        public IList<ConsultedPrescription> AddPrescription(ICollection<ConsultedPrescription> lConsultedPrescription, AccountMasterContext Context)
        {
            IList<ConsultedPrescription> lConsultedPrescriptionFromPres = new List<ConsultedPrescription>();
            foreach (ConsultedPrescription consPres in lConsultedPrescription)
            {
                Prescription prescription = consPres.Prescription;
                if (prescription != null)
                {
                    Context.Prescriptions.Add(prescription);
                    Context.SaveChanges();

                    consPres.PrescriptionId = prescription.Id;
                    lConsultedPrescriptionFromPres.Add(consPres);
                }
            }
            return lConsultedPrescriptionFromPres;
        }
        public ConsultationNote UpdateConsultedPrescription(AccountMasterContext Context, ConsultationNote ConsultationNote)
        {
            //Update consulted prescription from db
            IList<ConsultedPrescription> ConsultedPrescriptionFromDB = Context.ConsultedPrescriptions.Include("Prescription").Where(x => x.ConsultationNoteId == ConsultationNote.Id).ToList();
            foreach (ConsultedPrescription OldConsultedPrescription in ConsultedPrescriptionFromDB)
            {
                ConsultedPrescription NewConsultedPrescription = ConsultationNote.ConsultedPrescription.FirstOrDefault(x => x.Prescription.ProductId == OldConsultedPrescription.Prescription.ProductId);
                if (NewConsultedPrescription == null)
                {
                    Context.ConsultedPrescriptions.Remove(Context.ConsultedPrescriptions.FirstOrDefault(x => x.ConsultedPrescriptionId == OldConsultedPrescription.ConsultedPrescriptionId));
                    Context.Prescriptions.Remove(Context.Prescriptions.FirstOrDefault(x => x.Id == OldConsultedPrescription.PrescriptionId));

                }
                else
                {
                    ConsultationNote.ConsultedPrescription.Remove(NewConsultedPrescription);
                    NewConsultedPrescription.ConsultationNoteId = ConsultationNote.Id;
                    NewConsultedPrescription.PrescriptionId = OldConsultedPrescription.PrescriptionId;
                    NewConsultedPrescription.ConsultedPrescriptionId = OldConsultedPrescription.ConsultedPrescriptionId;
                    NewConsultedPrescription.Prescription = null;
                    ConsultedPrescription ConsultedPrescription = Context.ConsultedPrescriptions.Find(OldConsultedPrescription.ConsultedPrescriptionId);

                    Context.Entry(ConsultedPrescription).CurrentValues.SetValues(NewConsultedPrescription);
                }
            }
            Context.SaveChanges();

            return ConsultationNote;
        }
        public void RemoveConsultedPrescription(AccountMasterContext Context, ConsultationNote ConsultationNote)
        {
            //Remove consulted prescription from db
            IList<ConsultedPrescription> ConsultedPrescriptionFromDB = Context.ConsultedPrescriptions.Include("Prescription").Where(x => x.ConsultationNoteId == ConsultationNote.Id).ToList();
            foreach (ConsultedPrescription OldConsultedPrescription in ConsultedPrescriptionFromDB)
            {
                Context.ConsultedPrescriptions.Remove(Context.ConsultedPrescriptions.FirstOrDefault(x => x.ConsultedPrescriptionId == OldConsultedPrescription.ConsultedPrescriptionId));
                Context.Prescriptions.Remove(Context.Prescriptions.FirstOrDefault(x => x.Id == OldConsultedPrescription.PrescriptionId));
            }
        }

        public void RemoveConsultedProcedure(AccountMasterContext Context, ConsultationNote ConsultationNote)
        {
            //Remove consulted procedure from db
            IList<ConsultedProcedure> ConsultedProcedureFromDB = Context.ConsultedProcedures.Include("MedicalProcedure").Where(x => x.ConsultationNoteId == ConsultationNote.Id).ToList();
            foreach (ConsultedProcedure OldConsultedProcedure in ConsultedProcedureFromDB)
            {
                Context.ConsultedProcedures.Remove(Context.ConsultedProcedures.FirstOrDefault(x => x.ConsultedProcedureId == OldConsultedProcedure.ConsultedProcedureId)); //                Context.MedicalProcedures.Remove(Context.MedicalProcedures.FirstOrDefault(x => x.Id == OldConsultedProcedure.MedicalProcedureId));
            }
        }
        public void UpdateConsultedLabTestElement(ConsultedLabTest ConsLabTest, AccountMasterContext Context)
        {
            if (ConsLabTest.HasElement)
            {
                IList<ConsultedLabTestElements> lMedicalTestElement = ConsLabTest.ConsultedLabTestElements != null ? ConsLabTest.ConsultedLabTestElements.ToList() : new List<ConsultedLabTestElements>(); /*Context.MedicalTestElements.Where(x => x.MedicalTestId == ConsLabTest.MedicalTestId).ToList()*/;
                if (lMedicalTestElement != null)
                {
                    IList<ConsultedLabTestElements> lConsultedLabTestElements = Context.ConsultedLabTestElements.Where(x => x.ConsLabTestId == ConsLabTest.ConsultedLabTestId).ToList();

                    foreach (ConsultedLabTestElements OldConsElement in lConsultedLabTestElements)
                    {
                        ConsultedLabTestElements Element = lMedicalTestElement.FirstOrDefault(x => x.MedicalTestElementId == OldConsElement.MedicalTestElementId);
                        if (Element != null)
                        {
                            //remove new
                            lMedicalTestElement.Remove(Element);
                            //update old
                            ConsultedLabTestElements ConsultedLabTestElements = new ConsultedLabTestElements();
                            ConsultedLabTestElements.Id = OldConsElement.Id;
                            ConsultedLabTestElements.CompanyId = ConsLabTest.CompanyId;
                            ConsultedLabTestElements.ConsLabTestId = ConsLabTest.ConsultedLabTestId;
                            ConsultedLabTestElements.SubClass = Element.SubClass;
                            ConsultedLabTestElements.Class = Element.Class;
                            ConsultedLabTestElements.RangeFrom = Element.RangeFrom;
                            ConsultedLabTestElements.RangeTo = Element.RangeTo;
                            ConsultedLabTestElements.MedicalTestElementId = Element.MedicalTestElementId;
                            ConsultedLabTestElements.ResultDescription = OldConsElement.ResultDescription;
                            //ConsultedLabTestElements.LowCritical = OldConsElement.LowCritical;
                            //ConsultedLabTestElements.LowNormal = OldConsElement.LowNormal;
                            //ConsultedLabTestElements.ObservedLow = OldConsElement.ObservedLow;

                            ConsultedLabTestElements.Name = Element.Name;
                            ConsultedLabTestElements.UomId = Element.UomId;

                            Context.Entry(Context.ConsultedLabTestElements.Find(OldConsElement.Id)).CurrentValues.SetValues(ConsultedLabTestElements);
                            Context.SaveChanges();
                        }
                        else
                        {
                            Context.ConsultedLabTestElements.Where(L => L.ConsLabTestId == OldConsElement.ConsLabTestId).ToList().ForEach(L => Context.ConsultedLabTestElements.Remove(L));
                            Context.SaveChanges();
                        }
                    }
                    foreach (ConsultedLabTestElements Element in lMedicalTestElement)
                    {
                        ConsultedLabTestElements ConsultedLabTestElements = new ConsultedLabTestElements();
                        ConsultedLabTestElements.CompanyId = ConsLabTest.CompanyId;
                        ConsultedLabTestElements.ConsLabTestId = ConsLabTest.ConsultedLabTestId;
                        ConsultedLabTestElements.SubClass = Element.SubClass;
                        ConsultedLabTestElements.Class = Element.Class;
                        ConsultedLabTestElements.RangeFrom = Element.RangeFrom;
                        ConsultedLabTestElements.RangeTo = Element.RangeTo;
                        //ConsultedLabTestElements.LowCritical = Element.LowCritical;
                        //ConsultedLabTestElements.LowNormal = Element.LowNormal;
                        ConsultedLabTestElements.Name = Element.Name;
                        ConsultedLabTestElements.UomId = Element.UomId;
                        ConsultedLabTestElements.MedicalTestElementId = Element.MedicalTestElementId;
                        Context.ConsultedLabTestElements.Add(ConsultedLabTestElements);
                        Context.SaveChanges();

                    }
                }
            }
        }
        public void RemoveConsultedLabTestElement(ConsultationNote ConsultationNote, AccountMasterContext Context)
        {
            //Remove consulted Labtest from db
            IList<ConsultedLabTest> ConsultedLabTestFromDB = Context.ConsultedLabTests.Where(x => x.ConsultationNoteId == ConsultationNote.Id).ToList();
            foreach (ConsultedLabTest OldConsultedLabTest in ConsultedLabTestFromDB)
            {
               Context.ConsultedLabTestElements.Where(L => L.ConsLabTestId == OldConsultedLabTest.ConsultedLabTestId).ToList().ForEach(L => Context.ConsultedLabTestElements.Remove(L));
            }
            Context.ConsultedLabTests.Where(L => L.ConsultationNoteId == ConsultationNote.Id).ToList().ForEach(L => Context.ConsultedLabTests.Remove(L));
        }
        public void ManageConsultationNote(IList<ConsultationNote> lConsultationNote, long PatientId, DateTime GlobalTransactionDate, bool Filtered,bool IsConsults, long? OPId, bool isFromCheckboxEvent)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (isFromCheckboxEvent)
                        {
                            Filtered = !Filtered;
                        }
                        DateTime ConsltdateTime = GlobalTransactionDate.AddMonths(-1);
                        if (!Filtered)
                        {
                            ConsltdateTime = GlobalTransactionDate.AddYears(-100);
                        }
                        IList<ConsultationNote> lConsultationNoteFromDB = ListUnCompletedNotesEntryByPatientId(PatientId);
                        foreach (ConsultationNote OldConsultationNote in lConsultationNoteFromDB.Where(x => x.IsDischarged != true && x.Date > ConsltdateTime))
                        {
                            ConsultationNote NewConsultationNote = lConsultationNote.FirstOrDefault(x => x.Id == OldConsultationNote.Id);
                            if (NewConsultationNote == null)
                            {
                                DeleteNotes(OldConsultationNote.Id, Context);
                            }
                            else
                            {
                                lConsultationNote.Remove(NewConsultationNote);
                                UpdateNotes(NewConsultationNote, Context);
                            }
                        }
                        foreach (ConsultationNote Note in lConsultationNote)
                        {

                            if (Note.OpRegistrationId == null)
                            {
                                if (OPId != null)
                                {
                                    Note.OpRegistrationId = OPId;
                                }
                                else
                                {
                                    Registration lRegistration = new Registration();
                                    lRegistration.Id = 0L;
                                    lRegistration.IsFeePaid = false;
                                    lRegistration.IsNurseActivitiesCompleted = false;
                                    lRegistration.HasRegistrationFeePaid = false;
                                    lRegistration.RegistrationFee = 0.00;
                                    lRegistration.Status = Status.OPEN;
                                    lRegistration.PatientId = Note.PatientId;
                                    lRegistration.TockenNo = CompanyManager.Instance.GetIdSpace(CompanyManager.Instance.GetCompany(Note.CompanyId), EntryType.OP_TOKEN, Note.Date);
                                    lRegistration.CompanyId = Note.CompanyId;
                                    lRegistration.ReasonForTheVisit = "Emergency";
                                    TimeSpan timeSpan = DateTime.Now.TimeOfDay;
                                    lRegistration.DateOfRegistration = Note.Date.Date + timeSpan;
                                    Context.Registrationes.Add(lRegistration);
                                    Context.SaveChanges();
                                    Note.OpRegistrationId = lRegistration.Id;
                                    OPId = lRegistration.Id;
                                }
                            }
                            AddNotes(Note, Context, dbContextTransaction);
                        }
                        if (OPId!=null && IsConsults)
                        {
                            OpManager.Instance.UpdateOpRegistrationConsulting((long)OPId, true, Context);
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

        public ConsultationNote AddNotes(ConsultationNote consultationNote, AccountMasterContext Context, IDbContextTransaction dbContextTransaction)
        {
            try
            {              
                //Add prescription
                if (consultationNote.ConsultedPrescription != null && consultationNote.ConsultedPrescription.Count > 0)
                {
                    consultationNote.ConsultedPrescription = AddPrescription(consultationNote.ConsultedPrescription, Context);
                }

                //add consultation note
                Context.ConsultationNotes.Add(consultationNote);
                Context.SaveChanges();

                //add ledger                
                PatientLedgerManager.Instance.AddPatientLedgerFromConsulting(consultationNote, Context);
                PatientLedgerManager.Instance.AddPatientLedgerFromProcedure(consultationNote, Context);
                PatientLedgerManager.Instance.AddPatientLedgerFromLabTest(consultationNote, Context);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                consultationNote = null;
            }

            return consultationNote;
        }

        public ConsultationNote UpdateNotes(ConsultationNote ConsultationNote, AccountMasterContext Context)
        {
            ConsultationNote NotesInfo = null;
            try
            {
                NotesInfo = Context.ConsultationNotes.Find(ConsultationNote.Id);
                if (NotesInfo != null)
                {
                    //Update Consulted Prescription
                    ConsultationNote = UpdateConsultedPrescription(Context, ConsultationNote);

                    //add prescription
                    if (ConsultationNote.ConsultedPrescription != null && ConsultationNote.ConsultedPrescription.Count > 0)
                    {
                        ConsultationNote.ConsultedPrescription = AddPrescription(ConsultationNote.ConsultedPrescription, Context);
                    }

                    //Update consulted allergie from db
                    IList<ConsultedAllergie> ConsultedAllergieFromDB = Context.ConsultedAllergies.Where(x => x.ConsultationNoteId == ConsultationNote.Id).ToList();
                    foreach (ConsultedAllergie OldConsultedAllergie in ConsultedAllergieFromDB)
                    {
                        ConsultedAllergie NewConsultedAllergie = ConsultationNote.ConsultedAllergie.FirstOrDefault(x => x.ConsultedAllergieId == OldConsultedAllergie.ConsultedAllergieId);
                        if (NewConsultedAllergie == null)
                        {
                            Context.ConsultedAllergies.Remove(Context.ConsultedAllergies.FirstOrDefault(x => x.ConsultedAllergieId == OldConsultedAllergie.ConsultedAllergieId));
                        }
                        else
                        {
                            ConsultationNote.ConsultedAllergie.Remove(NewConsultedAllergie);
                            NewConsultedAllergie.ConsultationNoteId = ConsultationNote.Id;

                            ConsultedAllergie ConsultedAllergie = Context.ConsultedAllergies.Find(NewConsultedAllergie.ConsultedAllergieId);
                            ConsultedAllergie.ConsultationNote = null;

                            Context.Entry(ConsultedAllergie).CurrentValues.SetValues(NewConsultedAllergie);
                            Context.SaveChanges();
                        }
                    }

                    //Update consulted symptom from db
                    IList<ConsultedSymptom> ConsultedSymptomFromDB = Context.ConsultedSymptoms.Where(x => x.ConsultationNoteId == ConsultationNote.Id).ToList();
                    foreach (ConsultedSymptom OldConsultedSymptom in ConsultedSymptomFromDB)
                    {
                        ConsultedSymptom NewConsultedSymptom = ConsultationNote.ConsultedSymptom.FirstOrDefault(x => x.ConsultedSymptomId == OldConsultedSymptom.ConsultedSymptomId);
                        if (NewConsultedSymptom == null)
                        {
                            Context.ConsultedSymptoms.Remove(Context.ConsultedSymptoms.FirstOrDefault(x => x.ConsultedSymptomId == OldConsultedSymptom.ConsultedSymptomId));
                        }
                        else
                        {
                            ConsultationNote.ConsultedSymptom.Remove(NewConsultedSymptom);
                            NewConsultedSymptom.ConsultationNoteId = ConsultationNote.Id;

                            ConsultedSymptom ConsultedSymptom = Context.ConsultedSymptoms.Find(NewConsultedSymptom.ConsultedSymptomId);
                            ConsultedSymptom.ConsultationNote = null;

                            Context.Entry(ConsultedSymptom).CurrentValues.SetValues(NewConsultedSymptom);
                            Context.SaveChanges();
                        }
                    }

                    //Update consulted consultation from db
                    IList<ConsultedConsultationFee> ConsultedConsultationFeeFromDB = Context.ConsultedConsultationFees.Where(x => x.ConsultationNoteId == ConsultationNote.Id).ToList();
                    foreach (ConsultedConsultationFee OldConsultedConsultationFee in ConsultedConsultationFeeFromDB)
                    {
                        ConsultedConsultationFee NewConsultedConsultationFee = ConsultationNote.ConsultedConsultationFee.FirstOrDefault(x => x.ConsultationId == OldConsultedConsultationFee.ConsultationId);
                        if (NewConsultedConsultationFee == null)
                        {
                            Context.ConsultedConsultationFees.Remove(Context.ConsultedConsultationFees.FirstOrDefault(x => x.ConsultedConsultationId == OldConsultedConsultationFee.ConsultedConsultationId));
                            PatientLedgerManager.Instance.DeletePatientLedgerFromConsulting(NotesInfo, OldConsultedConsultationFee, Context);
                        }
                        else
                        {
                            ConsultationNote.ConsultedConsultationFee.Remove(NewConsultedConsultationFee);
                            NewConsultedConsultationFee.ConsultationNoteId = ConsultationNote.Id;
                            NewConsultedConsultationFee.ConsultedConsultationId = OldConsultedConsultationFee.ConsultedConsultationId;
                            Context.Entry(Context.ConsultedConsultationFees.Find(OldConsultedConsultationFee.ConsultedConsultationId)).CurrentValues.SetValues(NewConsultedConsultationFee);
                            Context.SaveChanges();
                            PatientLedgerManager.Instance.UpdatePatientLedgerFromConsulting(NotesInfo, NewConsultedConsultationFee, Context);
                        }
                    }

                    //Update consulted Labtest from db

                    IList<ConsultedLabTest> ConsultedLabTestFromDB = Context.ConsultedLabTests.Where(x => x.ConsultationNoteId == ConsultationNote.Id).ToList();
                    foreach (ConsultedLabTest OldConsultedLabTest in ConsultedLabTestFromDB)
                    {
                        ConsultedLabTest NewConsultedLabTest = ConsultationNote.ConsultedLabTest.FirstOrDefault(x => x.MedicalTestId == OldConsultedLabTest.MedicalTestId);
                        if (NewConsultedLabTest == null)
                        {
                            Context.ConsultedLabTestElements.Where(L => L.ConsLabTestId == OldConsultedLabTest.ConsultedLabTestId).ToList().ForEach(L => Context.ConsultedLabTestElements.Remove(L));
                            Context.ConsultedLabTests.Remove(Context.ConsultedLabTests.FirstOrDefault(x => x.ConsultedLabTestId == OldConsultedLabTest.ConsultedLabTestId));
                            PatientLedgerManager.Instance.DeletePatientLedgerFromLabTest(NotesInfo, OldConsultedLabTest, Context);
                        }
                        else
                        {

                            ConsultationNote.ConsultedLabTest.Remove(NewConsultedLabTest);
                            NewConsultedLabTest.ConsultationNoteId = ConsultationNote.Id;
                            NewConsultedLabTest.ConsultedLabTestId = OldConsultedLabTest.ConsultedLabTestId;

                            //update consulted labtest element
                            UpdateConsultedLabTestElement(NewConsultedLabTest, Context);

                            ConsultedLabTest ConsultedLabTest = Context.ConsultedLabTests.Find(OldConsultedLabTest.ConsultedLabTestId);
                            if (ConsultedLabTest != null)
                            {
                                Context.Entry(ConsultedLabTest).CurrentValues.SetValues(NewConsultedLabTest);
                                PatientLedgerManager.Instance.UpdatePatientLedgerFromLabTest(NotesInfo, NewConsultedLabTest, Context);
                                Context.SaveChanges();
                            }

                        }
                    }
                    // Update Consulted Procedure
                    IList<ConsultedProcedure> ConsultedProcedureFromDB = Context.ConsultedProcedures.Where(x => x.ConsultationNoteId == ConsultationNote.Id).ToList();
                    foreach (ConsultedProcedure OldConsultedProcedure in ConsultedProcedureFromDB)
                    {
                        ConsultedProcedure NewConsultedProcedure = ConsultationNote.ConsultedProcedure.FirstOrDefault(x => x.MedicalProcedureId == OldConsultedProcedure.MedicalProcedureId);
                        if (NewConsultedProcedure == null)
                        {
                            Context.ConsultedProcedures.Remove(Context.ConsultedProcedures.FirstOrDefault(x => x.ConsultedProcedureId == OldConsultedProcedure.ConsultedProcedureId));
                            PatientLedgerManager.Instance.DeletePatientLedgerFromProcedure(NotesInfo, OldConsultedProcedure, Context);

                        }
                        else
                        {
                            ConsultationNote.ConsultedProcedure.Remove(NewConsultedProcedure);
                            NewConsultedProcedure.ConsultationNoteId = ConsultationNote.Id;

                            NewConsultedProcedure.ConsultedProcedureId = OldConsultedProcedure.ConsultedProcedureId;

                            Context.Entry(Context.ConsultedProcedures.Find(NewConsultedProcedure.ConsultedProcedureId)).CurrentValues.SetValues(NewConsultedProcedure);
                            PatientLedgerManager.Instance.UpdatePatientLedgerFromProcedure(NotesInfo, NewConsultedProcedure, Context);
                            Context.SaveChanges();

                        }
                    }

                    //add new Consulted Symptom
                    foreach (ConsultedSymptom ConsultedSymptom in ConsultationNote.ConsultedSymptom)
                    {
                        ConsultedSymptom.ConsultationNoteId = ConsultationNote.Id;
                        Context.ConsultedSymptoms.Add(ConsultedSymptom);
                        Context.SaveChanges();
                    }
                    //add new Consulted Consultation Fee
                    foreach (ConsultedConsultationFee ConsultedConsultation in ConsultationNote.ConsultedConsultationFee)
                    {
                        ConsultedConsultation.ConsultationNoteId = ConsultationNote.Id;
                        Context.ConsultedConsultationFees.Add(ConsultedConsultation);
                        Context.SaveChanges();
                        PatientLedgerManager.Instance.AddPatientLedgerForConsulting(NotesInfo, ConsultedConsultation, Context);
                    }
                    //add new Consulted Prescription
                    foreach (ConsultedPrescription ConsultedPrescription in ConsultationNote.ConsultedPrescription)
                    {
                        ConsultedPrescription.ConsultationNoteId = ConsultationNote.Id;
                        Context.ConsultedPrescriptions.Add(ConsultedPrescription);
                        Context.SaveChanges();
                    }
                    //add new Consulted LabTest
                    foreach (ConsultedLabTest consultedLabTest in ConsultationNote.ConsultedLabTest)
                    {
                        consultedLabTest.ConsultationNoteId = ConsultationNote.Id;
                        Context.ConsultedLabTests.Add(consultedLabTest);
                        Context.SaveChanges();
                        PatientLedgerManager.Instance.AddPatientLedgerForLabTest(NotesInfo, consultedLabTest, Context);
                        //add consulted lab test element
                        UpdateConsultedLabTestElement(consultedLabTest, Context);
                    }
                    // add new Consulted Procedure
                    foreach (ConsultedProcedure ConsultedProcedure in ConsultationNote.ConsultedProcedure)
                    {
                        ConsultedProcedure.ConsultationNoteId = ConsultationNote.Id;
                        Context.ConsultedProcedures.Add(ConsultedProcedure);
                        Context.SaveChanges();
                        PatientLedgerManager.Instance.AddPatientLedgerForProcedure(NotesInfo, ConsultedProcedure, Context);
                    }

                    //update Note
                    ConsultationNote.ConsultedSymptom = null;
                    ConsultationNote.ConsultedPrescription = null;
                    ConsultationNote.ConsultedLabTest = null;
                    ConsultationNote.ConsultedConsultationFee = null;
                    ConsultationNote.SaleRefPrescription = null;
                    ConsultationNote.OpRegistration = null;
                    ConsultationNote.ConsultedProcedure = null;
                    ConsultationNote.ConsultantId = NotesInfo.ConsultantId;
                    ConsultationNote.SaleEntryId = NotesInfo.SaleEntryId;
                    ConsultationNote.OpRegistrationId = NotesInfo.OpRegistrationId;
                    ConsultationNote.Date = NotesInfo.Date;
                    ConsultationNote.IsPrescriptionDone = NotesInfo.IsPrescriptionDone;
                    ConsultationNote.IsPrescriptionDispatchedForMedical = NotesInfo.IsPrescriptionDispatchedForMedical;

                    Context.Entry(Context.ConsultationNotes.Find(NotesInfo.Id)).CurrentValues.SetValues(ConsultationNote);
                    Context.SaveChanges();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                NotesInfo = null;
                throw (e);
            }
            return NotesInfo;
        }

        public Boolean DeleteNotes(long ConsultationNoteId, AccountMasterContext Context)
        {
            bool deleted = false;
            try
            {
                ConsultationNote ConsultationNote = Context.ConsultationNotes.Find(ConsultationNoteId);
                if (ConsultationNote != null)
                {
                    //Remove consulted prescription from db
                    RemoveConsultedPrescription(Context, ConsultationNote);

                    //remove consulted Allergie from db
                    Context.ConsultedAllergies.Where(S => S.ConsultationNoteId == ConsultationNote.Id).ToList().ForEach(S => Context.ConsultedAllergies.Remove(S));

                    //remove consulted symptom from db
                    Context.ConsultedSymptoms.Where(S => S.ConsultationNoteId == ConsultationNote.Id).ToList().ForEach(S => Context.ConsultedSymptoms.Remove(S));

                    //remove consulted consultation from db
                    Context.ConsultedConsultationFees.Where(C => C.ConsultationNoteId == ConsultationNote.Id).ToList().ForEach(C => Context.ConsultedConsultationFees.Remove(C));

                    //remove consulted Labtest from db
                    RemoveConsultedLabTestElement(ConsultationNote, Context);

                    //remove Consulted Procedure from db
                    RemoveConsultedProcedure(Context, ConsultationNote);

                    //remove procedure from ledger
                    PatientLedgerManager.Instance.DeletePatientLedgerFromNote(ConsultationNote, Context);

                    //remove note
                    Context.ConsultationNotes.Remove(ConsultationNote);

                    Context.SaveChanges();

                }
                deleted = true;
            }
#pragma warning disable 0168
            catch (Exception e)
            {
                deleted = false;
            }
#pragma warning restore 0168
            return deleted;

        }

        public void UpdatePrescription(IList<Prescription> lPrescription, long NoteId)
        {
            if (lPrescription.Count > 0)
            {
                using (AccountMasterContext Context = new AccountMasterContext())
                {
                    using (var dbContextTransaction = Context.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (Prescription pres in lPrescription)
                            {
                                Prescription PresDB = Context.Prescriptions.Find(pres.Id);
                                Context.Entry(PresDB).CurrentValues.SetValues(pres);
                                Context.SaveChanges();
                            }

                            ConsultationNote Note = Context.ConsultationNotes.Find(NoteId);
                            if (Note != null)
                            {
                                Note.ConsultedSymptom = null;
                                Note.ConsultedPrescription = null;
                                Note.ConsultedLabTest = null;
                                Note.ConsultedConsultationFee = null;
                                Note.SaleRefPrescription = null;
                                Note.OpRegistration = null;
                                Note.Company = null;
                                Note.Consultant = null;
                                Note.Patient = null;
                                Note.IsPrescriptionDone = true;

                                Context.Entry(Context.ConsultationNotes.Find(Note.Id)).CurrentValues.SetValues(Note);
                                Context.SaveChanges();
                            }

                            dbContextTransaction.Commit();
                        }
#pragma warning disable 0168
                        catch (Exception e)
                        {
                            dbContextTransaction.Rollback();
                        }
#pragma warning restore 0168
                    }
                }
            }
        }
        public void UpdateNoteForPrescription(long NoteId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        ConsultationNote Note = Context.ConsultationNotes.Find(NoteId);
                        if (Note != null)
                        {
                            Note.ConsultedSymptom = null;
                            Note.ConsultedPrescription = null;
                            Note.ConsultedLabTest = null;
                            Note.ConsultedConsultationFee = null;
                            Note.SaleRefPrescription = null;
                            Note.OpRegistration = null;
                            Note.Company = null;
                            Note.Consultant = null;
                            Note.Patient = null;
                            Note.IsPrescriptionDispatchedForMedical = true;

                            Context.Entry(Context.ConsultationNotes.Find(Note.Id)).CurrentValues.SetValues(Note);
                            Context.SaveChanges();
                        }
                        dbContextTransaction.Commit();

                    }
#pragma warning disable 0168
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                    }
#pragma warning restore 0168
                }
            }
        }
        public void UpdateNoteForInvoiceCreation(long NoteId, AccountMasterContext Context,bool IsInvoiced)
        {
            try
            {
                ConsultationNote Note = Context.ConsultationNotes.Find(NoteId);
                if (Note != null)
                {
                    Note.IsInvoiced = IsInvoiced;
                    Context.Entry(Context.ConsultationNotes.Find(Note.Id)).CurrentValues.SetValues(Note);
                    Context.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
        }
        public void UpdateNoteForPrescriptionSaleUpdate(long NoteId, long SaleId, AccountMasterContext Context)
        {
            try
            {
                ConsultationNote Note = Context.ConsultationNotes.Find(NoteId);
                if (Note != null)
                {
                    Note.SaleEntryId = SaleId;
                    Context.Entry(Context.ConsultationNotes.Find(Note.Id)).CurrentValues.SetValues(Note);
                    Context.SaveChanges();
                }
            }
#pragma warning disable 0168
            catch (Exception e)
            {

            }
#pragma warning restore 0168
        }
        public void UpdateNoteForPrescriptionSaleDelete(long NoteId, AccountMasterContext Context)
        {
            try
            {
                ConsultationNote Note = Context.ConsultationNotes.Find(NoteId);
                if (Note != null)
                {
                    Note.SaleEntryId = null;
                    Context.Entry(Context.ConsultationNotes.Find(Note.Id)).CurrentValues.SetValues(Note);
                    Context.SaveChanges();
                }
            }
#pragma warning disable 0168
            catch (Exception e)
            {
            }
#pragma warning restore 0168
        }
        public void UpdateConsultedMedicalProcedure(IList<ConsultedProcedure> lConsultedProcedures)
        {
            if (lConsultedProcedures.Count > 0)
            {
                using (AccountMasterContext Context = new AccountMasterContext())
                {
                    using (var dbContextTransaction = Context.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (ConsultedProcedure ConsltProcedure in lConsultedProcedures)
                            {
                                ConsultedProcedure ConsultedProceduresDB = Context.ConsultedProcedures.Find(ConsltProcedure.ConsultedProcedureId);
                                Context.Entry(ConsultedProceduresDB).CurrentValues.SetValues(ConsltProcedure);
                                Context.SaveChanges();
                            }
                            dbContextTransaction.Commit();
                        }
                        catch (Exception e)
                        {
                            dbContextTransaction.Rollback();
                        }
                    }
                }
            }
        }
        public void UpdateConsultedLabTestElements(IList<ConsultedLabTestElements> lConsultedLabTestElements)
        {
            if (lConsultedLabTestElements.Count > 0)
            {
                using (AccountMasterContext Context = new AccountMasterContext())
                {
                    using (var dbContextTransaction = Context.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (ConsultedLabTestElements ConsLabTestEle in lConsultedLabTestElements)
                            {
                                if (ConsLabTestEle.Id == 0L)
                                {
                                    Context.ConsultedLabTestElements.Add(ConsLabTestEle);
                                }
                                else
                                {
                                    ConsultedLabTestElements ConsultedLabTestElementsDB = Context.ConsultedLabTestElements.Find(ConsLabTestEle.Id);
                                    Context.Entry(ConsultedLabTestElementsDB).CurrentValues.SetValues(ConsLabTestEle);
                                }
                                Context.SaveChanges();
                            }
                            dbContextTransaction.Commit();
                        }
#pragma warning disable 0168
                        catch (Exception e)
                        {
                            dbContextTransaction.Rollback();
                        }
#pragma warning restore 0168
                    }
                }
            }
        }

        public void UpdateConsultedLabTestAttachment(IList<LabTestAttachment> lLabTestAttachments, long labtestId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        var existingAttachments = Context.LabTestAttachments.Where(C => C.ConsLabTestId == labtestId).ToList();
                        Context.LabTestAttachments.RemoveRange(existingAttachments);
                        Context.SaveChanges();

                        foreach (LabTestAttachment Attachment in lLabTestAttachments)
                        {
                            Context.LabTestAttachments.Add(Attachment);
                        }
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
#pragma warning disable 0168
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                    }
#pragma warning restore 0168
                }
            }
        }
        public ConsultedPrescription GetConPrescriptionByConsultationNoteId(long ConsNoteId)
        {
            ConsultedPrescription consultedPrescription = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                consultedPrescription = Context.ConsultedPrescriptions.FirstOrDefault(x => x.ConsultationNoteId == ConsNoteId);
            }
            return consultedPrescription;
        }

        public IList<LabTestAttachment> ListConsLabTestByPatientIdForDocument(long PatientId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var NoteIds = from Note in Context.ConsultationNotes where Note.PatientId == PatientId select Note.Id;
                var ConsultedLabTestIds = from Labtest in Context.ConsultedLabTests where NoteIds.Contains(Labtest.ConsultationNoteId) select Labtest.ConsultedLabTestId;

                IList<LabTestAttachment> LabTestAttachmentInfo = Context.LabTestAttachments.Where(x => ConsultedLabTestIds.Contains(x.ConsLabTestId)).ToList();
                return LabTestAttachmentInfo;
            }
        }
        public LabTestAttachment GetConsLabTestAttachmentById(long Id)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                LabTestAttachment LabTestAttachmentInfo = Context.LabTestAttachments.FirstOrDefault(x => x.Id == Id);
                return LabTestAttachmentInfo;
            }
        }
    }
}
