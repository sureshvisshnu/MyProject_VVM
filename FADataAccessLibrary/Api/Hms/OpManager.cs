using fa.context;
using fa.model.Accounting.Masters;
using fa.model.hms.common;
using fa.model.hms.config;
using fa.model.Hms.Op;
using fa.model.Hms.common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using fa.api.Accounting;
using FADataAccessLibrary.Api.Accounting.DoubleEntry;
using fa.Data;
using fa.model.Hms.Master;
using System;
using Fa.api.Hms;
using fa.model.Hms.Ip;
using System.ComponentModel.Design;
using System.Net.NetworkInformation;
using fa.model.UserProfile;
using System.Linq;
using fa.model.Employee;

namespace fa.api.Hms
{
    public class OpManager
    {
        private static volatile OpManager instance;
        private static object syncRoot = new Object();
        OpManager()
        {

        }
        public static OpManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new OpManager();
                    }
                }
                return instance;
            }
        }
        public static string FeachPatientOPID(long CompanyId, DateTime Date)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientOPId lPatientOPId = Context.PatientOPIds.FirstOrDefault(x => x.CompanyId == CompanyId && x.Date == Date);
                if (lPatientOPId == null)
                {
                    lPatientOPId = new PatientOPId
                    {
                        CompanyId = CompanyId,
                        Date = Date,
                        NextNumber = 1
                    };
                    Context.PatientOPIds.Add(lPatientOPId);
                    Context.SaveChanges();
                }
                return lPatientOPId.PatientOPNo;
            }
        }
        public static void GenerateNextPatientOPID(long CompanyId, DateTime Date)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientOPId lPatientOPId = Context.PatientOPIds.FirstOrDefault(x => x.CompanyId == CompanyId && x.Date == Date);
                if (lPatientOPId != null)
                {
                    int CurrentPatientNo = lPatientOPId.NextNumber;
                    lPatientOPId.NextNumber = CurrentPatientNo + 1;
                    Context.Entry(lPatientOPId).CurrentValues.SetValues(lPatientOPId);
                    Context.SaveChanges();

                }
            }
        }
        public IList<Registration> ListAllOpByPatientId(long PatientId)
        {
            IList<Registration> OpInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                OpInfo = Context.Registrationes.Include("RequestedDoctor").Include("Patient").Where(x => x.PatientId == PatientId).OrderByDescending(x => x.DateOfRegistration).ToList<Registration>();
                return OpInfo;
            }
        }
        public IList<Registration> ListAllOpByPatientIdForReport(long PatientId, DateTime FromDate, DateTime ToDate)
        {
            IList<Registration> OpInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                OpInfo = Context.Registrationes.Include("RequestedDoctor").Include("Patient").Where(x => x.PatientId == PatientId && x.DateOfRegistration >= FromDate.Date && x.DateOfRegistration < ToDate.AddDays(1).Date).ToList<Registration>();
                return OpInfo;
            }
        }
        public IList<Registration> ListAllOpByPatientNumber(string PatientNumber, bool IsDoctor, IList<Registration> Registration)
        {
            IList<Registration> OpInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (IsDoctor)
                {
                    OpInfo = Registration.Where(x => !x.HasConsulted && x.Patient.PatientNumber == PatientNumber ).OrderBy(x => x.TockenNo).ToList<Registration>();
                }
                else
                {
                    OpInfo = Registration.Where(x => x.Patient.PatientNumber == PatientNumber).OrderBy(x => x.TockenNo).ToList<Registration>();
                }
                return OpInfo;
            }
        }
        public IList<Registration> ListAllOpByPatientPhoneNumber(string PhoneNumber, bool IsDoctor, IList<Registration> Registration)
        {
            IList<Registration> OpInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (IsDoctor)
                {
                    OpInfo = Registration.Where(x => !x.HasConsulted && (x.Patient.ContactInfo.Phone.Contains(PhoneNumber) || x.Patient.ContactInfo.Mobile.Contains(PhoneNumber))).OrderBy(x => x.TockenNo).ToList<Registration>();
                }
                else
                {
                    OpInfo = Registration.Where(x => (x.Patient.ContactInfo.Phone.Contains(PhoneNumber) || x.Patient.ContactInfo.Mobile.Contains(PhoneNumber))).OrderBy(x => x.TockenNo).ToList<Registration>();
                }
                return OpInfo;
            }
        }
        public IList<Registration> ListAllOpByDOB(DateTime Dob, bool IsDoctor, IList<Registration> Registration)
        {
            IList<Registration> OpInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (IsDoctor)
                {
                    OpInfo = Registration.Where(x => x.HasConsulted == false && x.Patient.DateOfBirth == Dob ).OrderBy(x => x.TockenNo).ToList<Registration>();
                }
                else
                {
                    OpInfo = Registration.Where(x => x.Patient.DateOfBirth == Dob).OrderBy(x => x.TockenNo).ToList<Registration>();
                }
                return OpInfo;
            }
        }

        public IList<Registration> ListAllOpByToken(string TokenNumber, bool IsDoctor, IList<Registration> Registration)
        {
            IList<Registration> OpInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (IsDoctor)
                {
                    OpInfo = Registration.Where(x => x.HasConsulted == false && x.TockenNo == TokenNumber ).OrderBy(x => x.TockenNo).ToList<Registration>();
                }
                else
                {
                    OpInfo = Registration.Where(x => x.TockenNo == TokenNumber).OrderBy(x => x.TockenNo).ToList<Registration>();
                }
                return OpInfo;
            }
        }
        public IList<Registration> ListAllOpByName(string Name, bool IsDoctor, IList<Registration> Registration)
        {
            IList<Registration> OpInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (IsDoctor)
                {
                    OpInfo = Registration.Where(x => x.HasConsulted == false && x.Patient.Name.Contains(Name)).OrderBy(x => x.TockenNo).ToList<Registration>();
                }
                else
                {
                    OpInfo = Registration.Where(x => x.Patient.Name.Contains(Name)).OrderBy(x => x.TockenNo).ToList<Registration>();
                }
                return OpInfo;
            }
        }
        public IList<Registration> ListAllOpByTransactionDate(IList<Status> OpStatus, long CompantId, DateTime Date, bool IsDoctor)
        {
            IList<Registration> OpInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (IsDoctor)
                {
                    OpInfo = Context.Registrationes.Include("RequestedDoctor").Include("Patient").Where(x => x.DateOfRegistration.Day == Date.Day && x.DateOfRegistration.Month == Date.Month && x.DateOfRegistration.Year == Date.Year && x.CompanyId == CompantId && x.HasConsulted == false && OpStatus.Contains(x.Status) ).OrderBy(x => x.TockenNo).ToList<Registration>();
                }
                else
                {
                    OpInfo = Context.Registrationes.Include("RequestedDoctor").Include("Patient").Where(x => x.DateOfRegistration.Day == Date.Day && x.DateOfRegistration.Month == Date.Month && x.DateOfRegistration.Year == Date.Year && x.CompanyId == CompantId  && OpStatus.Contains(x.Status)).OrderBy(x => x.TockenNo).ToList<Registration>();
                }
                return OpInfo;

            }
        }
        public Registration GetRegisterByRegId(long Id)
        {
            Registration RegInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                RegInfo = Context.Registrationes.Include("Patient").Include("RequestedDoctor").FirstOrDefault(x => x.Id == Id);
                return RegInfo;
            }
        }
        public Registration GetRegisterByPatientId(long Id)
        {
            Registration RegInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                RegInfo = Context.Registrationes.Include("Patient").FirstOrDefault(x => x.PatientId == Id);
                return RegInfo;
            }
        }
        public Registration GetOPRecordIfAdmitted(long PatientId)
        {
            Registration RegInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                RegInfo = Context.Registrationes.Include("Patient").FirstOrDefault(x => x.PatientId == PatientId && x.Status == Status.INPATIENT);
                return RegInfo;
            }
        }
        public Registration GetlastOPRecord(long PatientId , long CompanyId)
        {
            Registration RegInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                RegInfo = Context.Registrationes.Include("Patient").Include("RequestedDoctor").Where(x => x.PatientId == PatientId && x.CompanyId == CompanyId).OrderBy(x=>x.DateOfRegistration).Last();
                return RegInfo;
            }
        }
        public Registration GetPatientInOpQueue(long PatientId, DateTime date)
        {
            Registration RegInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                RegInfo = Context.Registrationes.Include("Patient").FirstOrDefault(x => x.PatientId == PatientId && x.DateOfRegistration.Day == date.Day && x.DateOfRegistration.Year == date.Year && x.DateOfRegistration.Month == date.Month && x.Status == Status.OPEN);
                return RegInfo;
            }
        }
        public Registration GetPatientLastOp(long PatientId, DateTime date)
        {
            Registration RegInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                RegInfo = Context.Registrationes.FirstOrDefault(x => x.PatientId == PatientId && x.DateOfRegistration.Day == date.Day && x.DateOfRegistration.Year == date.Year && x.DateOfRegistration.Month == date.Month && x.Status == Status.OPEN);
                Registration lastRegistration = Context.Registrationes.Where(x => x.PatientId == PatientId).OrderByDescending(x => x.DateOfRegistration).FirstOrDefault();
                if (lastRegistration != null)
                {
                    RegInfo = lastRegistration;
                }
                return RegInfo;
            }
        }
        public Registration GetPatientLastOpByTransactionDate(long PatientId, DateTime date, long CompanyId)
        {
            List<Registration> RegInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                RegInfo = Context.Registrationes.Where(x => x.CompanyId == CompanyId && x.DateOfRegistration.Day == date.Day && x.DateOfRegistration.Year == date.Year && x.DateOfRegistration.Month == date.Month).AsEnumerable().Where(x => int.TryParse(x.TockenNo, out _)).OrderByDescending(x => int.Parse(x.TockenNo)).ToList();
            }
            return RegInfo.Count > 0 ? RegInfo.First() : null;
        }
        public Registration GetOpByPatientId(long PatientId, DateTime date)
        {
            Registration RegInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                RegInfo = Context.Registrationes.Include("Patient").FirstOrDefault(x => x.PatientId == PatientId && x.DateOfRegistration.Day == date.Day && x.DateOfRegistration.Year == date.Year && x.DateOfRegistration.Month == date.Month && x.Status == Status.OPEN && x.HasConsulted == false);
                return RegInfo;
            }
        }
        public Registration GetConsultedOpByPatientId(long PatientId, DateTime date)
        {
            Registration RegInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                RegInfo = Context.Registrationes.Include("RequestedDoctor").Include("Patient").FirstOrDefault(x => x.PatientId == PatientId && x.DateOfRegistration.Day == date.Day && x.DateOfRegistration.Year == date.Year && x.DateOfRegistration.Month == date.Month && x.Status == Status.OPEN);
                return RegInfo;
            }
        }
        public Registration GetOpNumberByPatientId(long PatientId, DateTime date)
        {
            Registration RegInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                RegInfo = Context.Registrationes.Include("RequestedDoctor").Include("Patient").FirstOrDefault(x => x.PatientId == PatientId && x.DateOfRegistration.Day == date.Day && x.DateOfRegistration.Year == date.Year && x.DateOfRegistration.Month == date.Month && (x.Status == Status.INPATIENT || x.Status == Status.OPEN));
                return RegInfo;
            }
        }
        public Registration GetOpRegistrationById(long OpRegistrationId)
        {
            Registration OpRegistrationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                OpRegistrationInfo = Context.Registrationes.Include("Patient").Include("RequestedDoctor").FirstOrDefault(x => x.Id == OpRegistrationId);
                return OpRegistrationInfo;
            }
        }
        public Registration GetOpPrimaryDoctorByOpId(long OpRegistrationId)
        {
            Registration OpRegistrationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                OpRegistrationInfo = Context.Registrationes.Include("RequestedDoctor").Include("Patient").FirstOrDefault(x => x.Id == OpRegistrationId);
                return OpRegistrationInfo;
            }
        }
        public Registration GetOpRegistrationByNote(ConsultationNote ConsultationNote)
        {
            Registration OpRegistrationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                OpRegistrationInfo = Context.Registrationes.Include("Patient").FirstOrDefault(x => x.PatientId == ConsultationNote.PatientId && x.DateOfRegistration.Day == ConsultationNote.Date.Day && x.DateOfRegistration.Month == ConsultationNote.Date.Month && x.DateOfRegistration.Year == ConsultationNote.Date.Year);
                return OpRegistrationInfo;
            }
        }
        public Registration AddOpRegistration(Registration opRegistration)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.Registrationes.Add(opRegistration);
                        Context.SaveChanges();
                        opRegistration = ManageOpRegistrationFee(opRegistration, Context, dbContextTransaction);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        dbContextTransaction.Rollback();
                        //throw (e);
                    }
                }
            }
            return opRegistration;
        }
        public Registration ManageOpRegistrationFee(Registration OpRegistrationInfo, AccountMasterContext Context, IDbContextTransaction dbContextTransaction)
        {
            HospitalConfiguration HospitalConfiguration = Context.HospitalConfigurations.FirstOrDefault(x => x.CompanyId == OpRegistrationInfo.CompanyId);
            if (HospitalConfiguration != null && HospitalConfiguration.DefaultOPConsultingFee > 0)
            {
                PatientLedger PatientLedger = new PatientLedger();
                PatientLedger.Date = (DateTime)OpRegistrationInfo.CreatedDate;
                PatientLedger.PatientId = (long)OpRegistrationInfo.PatientId;
                PatientLedger.OpRegistrationId = OpRegistrationInfo.Id;
                PatientLedger.Amount = HospitalConfiguration.DefaultOPConsultingFee;
                PatientLedger.Description = "Registration fee";
                PatientLedger.CompanyId = OpRegistrationInfo.CompanyId;
                PatientLedger.Type = TransactionType.REGISTRATION_FEE;
                Context.PatientLedgers.Add(PatientLedger);
                Context.SaveChanges();
                //create invoice for registration fee
                PatientInvoiceManager.Instance.CreateInvoiceForRegistrationFee(PatientLedger, OpRegistrationInfo.RegistrationFee, Context);
                Company Company = Context.Companies.Find(OpRegistrationInfo.CompanyId);
                if (Company != null)
                {
                    if (!OpRegistrationInfo.HasRegistrationFeePaid)
                    {
                        string RefNum = CompanyManager.Instance.GetIdSpace(Company, EntryType.PATIENT_FEE_RECEIPT, (DateTime)OpRegistrationInfo.CreatedDate);
                        if (!string.IsNullOrEmpty(RefNum))
                        {
                            PatientLedger = new PatientLedger();
                            PatientLedger.RefNumber = RefNum;
                            PatientLedger.Date = (DateTime)OpRegistrationInfo.CreatedDate;
                            PatientLedger.PatientId = (long)OpRegistrationInfo.PatientId;
                            PatientLedger.OpRegistrationId = OpRegistrationInfo.Id;
                            PatientLedger.Amount = HospitalConfiguration.DefaultOPConsultingFee;
                            PatientLedger.Description = "Payment waived as per ref #" + RefNum;
                            PatientLedger.CompanyId = OpRegistrationInfo.CompanyId;
                            PatientLedger.Type = TransactionType.WAIVER;
                            Context.PatientLedgers.Add(PatientLedger);
                            Context.SaveChanges();
                            //create payment for registration fee
                            PatientInvoiceManager.Instance.paymentForRegistrationFee(PatientLedger, Context);
                            //daybook
                            HmsPaymentDoubleEntryManager.Instance.RecordPayment(PatientLedger, Context);
                        }
                        else
                        {
                            dbContextTransaction.Rollback();
                            return null;
                        }
                    }
                    else if (OpRegistrationInfo.RegistrationFee > 0)
                    {
                        string RefNum = CompanyManager.Instance.GetIdSpace(Company, EntryType.PATIENT_FEE_RECEIPT, (DateTime)OpRegistrationInfo.CreatedDate);
                        if (!string.IsNullOrEmpty(RefNum))
                        {
                            PatientLedger = new PatientLedger();
                            PatientLedger.RefNumber = RefNum;
                            PatientLedger.Date = (DateTime)OpRegistrationInfo.CreatedDate;
                            PatientLedger.PatientId = (long)OpRegistrationInfo.PatientId;
                            PatientLedger.OpRegistrationId = OpRegistrationInfo.Id;
                            PatientLedger.Amount = OpRegistrationInfo.RegistrationFee;
                            PatientLedger.Description = "Payment received as per ref #" + RefNum;
                            PatientLedger.CompanyId = OpRegistrationInfo.CompanyId;
                            PatientLedger.Type = TransactionType.PAYMENT;
                            Context.PatientLedgers.Add(PatientLedger);
                            Context.SaveChanges();
                            //create payment for registration fee
                            PatientInvoiceManager.Instance.paymentForRegistrationFee(PatientLedger, Context);
                            //daybook
                            HmsPaymentDoubleEntryManager.Instance.RecordPayment(PatientLedger, Context);
                        }
                        else
                        {
                            dbContextTransaction.Rollback();
                            return null;
                        }
                    }
                }
            }
            return OpRegistrationInfo;
        }
        public Registration UpdateOpRegistrations(Registration OpRegistration)
        {
            Registration OpRegistrationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        OpRegistrationInfo = Context.Registrationes.Find(OpRegistration.Id);
                        if (OpRegistrationInfo != null)
                        {
                            Context.Entry(OpRegistrationInfo).CurrentValues.SetValues(OpRegistration);
                            Context.SaveChanges();
                            Company Company = Context.Companies.Find(OpRegistrationInfo.CompanyId);
                            if (Company != null)
                            {
                                HospitalConfiguration HospitalConfiguration = Context.HospitalConfigurations.FirstOrDefault(x => x.CompanyId == OpRegistrationInfo.CompanyId);
                                if (OpRegistration.RegistrationFee > 0)
                                {
                                    PatientLedger lPatientLedger = Context.PatientLedgers.FirstOrDefault(x => x.OpRegistrationId == OpRegistration.Id && x.RefNumber != null);
                                    if (lPatientLedger != null)
                                    {
                                        if (lPatientLedger.Type == TransactionType.WAIVER)
                                        {
                                            //Update waived to payment
                                            PatientLedger PatientLedger = new PatientLedger();
                                            PatientLedger.Id = lPatientLedger.Id;
                                            PatientLedger.CreatedBy = lPatientLedger.CreatedBy;
                                            PatientLedger.CreatedDate = lPatientLedger.CreatedDate;
                                            PatientLedger.RefNumber = lPatientLedger.RefNumber;
                                            PatientLedger.Date = lPatientLedger.Date;
                                            PatientLedger.PatientId = (long)OpRegistrationInfo.PatientId;
                                            PatientLedger.OpRegistrationId = OpRegistrationInfo.Id;
                                            PatientLedger.Amount = OpRegistrationInfo.RegistrationFee;
                                            PatientLedger.Description = "Payment received as per ref #" + lPatientLedger.RefNumber;
                                            PatientLedger.CompanyId = OpRegistrationInfo.CompanyId;
                                            PatientLedger.Type = TransactionType.PAYMENT;
                                            Context.Entry(Context.PatientLedgers.Find(lPatientLedger.Id)).CurrentValues.SetValues(PatientLedger);
                                            Context.SaveChanges();
                                            //recreate payment for registration fee
                                            PatientInvoiceManager.Instance.UpdatepaymentForRegistrationFee(PatientLedger, Context);
                                            //daybook
                                            HmsPaymentDoubleEntryManager.Instance.RecordPayment(PatientLedger, Context);
                                        }
                                    }
                                    else 
                                    { 
                                        string RefNum = CompanyManager.Instance.GetIdSpace(Company, EntryType.PATIENT_FEE_RECEIPT, OpRegistrationInfo.DateOfRegistration);
                                        if (!string.IsNullOrEmpty(RefNum))
                                        {
                                            PatientLedger PatientLedger = new PatientLedger();
                                            PatientLedger.RefNumber = RefNum;
                                            PatientLedger.Date = (DateTime)OpRegistrationInfo.CreatedDate;
                                            PatientLedger.PatientId = (long)OpRegistrationInfo.PatientId;
                                            PatientLedger.OpRegistrationId = OpRegistrationInfo.Id;
                                            PatientLedger.Amount = OpRegistrationInfo.RegistrationFee;
                                            PatientLedger.Description = "Payment received as per ref #" + RefNum;
                                            PatientLedger.CompanyId = OpRegistrationInfo.CompanyId;
                                            PatientLedger.Type = TransactionType.PAYMENT;
                                            Context.PatientLedgers.Add(PatientLedger);
                                            Context.SaveChanges();
                                            //create payment for registration fee
                                            PatientInvoiceManager.Instance.paymentForRegistrationFee(PatientLedger, Context);
                                            //daybook
                                            HmsPaymentDoubleEntryManager.Instance.RecordPayment(PatientLedger, Context);
                                        }
                                    }                                   
                                }
                                else if (!OpRegistration.HasRegistrationFeePaid)
                                {
                                    PatientLedger lPatientLedger = Context.PatientLedgers.FirstOrDefault(x => x.OpRegistrationId == OpRegistration.Id && x.RefNumber != null);
                                    if (lPatientLedger != null)
                                    {
                                        if (lPatientLedger.Type == TransactionType.PAYMENT)
                                        {
                                            //Update payment to waived
                                            PatientLedger PatientLedger = new PatientLedger();
                                            PatientLedger.Id = lPatientLedger.Id;
                                            PatientLedger.CreatedBy = lPatientLedger.CreatedBy;
                                            PatientLedger.CreatedDate = lPatientLedger.CreatedDate;
                                            PatientLedger.RefNumber = lPatientLedger.RefNumber;
                                            PatientLedger.Date = lPatientLedger.Date;
                                            PatientLedger.PatientId = (long)OpRegistrationInfo.PatientId;
                                            PatientLedger.OpRegistrationId = OpRegistrationInfo.Id;
                                            PatientLedger.Amount = HospitalConfiguration!=null? HospitalConfiguration.DefaultOPConsultingFee:0;
                                            PatientLedger.Description = "Payment waived as per ref #" + lPatientLedger.RefNumber;
                                            PatientLedger.CompanyId = OpRegistrationInfo.CompanyId;
                                            PatientLedger.Type = TransactionType.WAIVER;
                                            Context.Entry(Context.PatientLedgers.Find(lPatientLedger.Id)).CurrentValues.SetValues(PatientLedger);
                                            Context.SaveChanges();
                                            //create payment for registration fee
                                            PatientInvoiceManager.Instance.UpdatepaymentForRegistrationFee(PatientLedger, Context);
                                            //daybook
                                            HmsPaymentDoubleEntryManager.Instance.RecordPayment(PatientLedger, Context);
                                        }
                                    }
                                    else
                                    {
                                        string RefNum = CompanyManager.Instance.GetIdSpace(Company, EntryType.PATIENT_FEE_RECEIPT, (DateTime)OpRegistrationInfo.CreatedDate);
                                        if (!string.IsNullOrEmpty(RefNum))
                                        {
                                            PatientLedger PatientLedger = new PatientLedger();
                                            PatientLedger.RefNumber = RefNum;
                                            PatientLedger.Date = (DateTime)OpRegistrationInfo.CreatedDate;
                                            PatientLedger.PatientId = (long)OpRegistrationInfo.PatientId;
                                            PatientLedger.OpRegistrationId = OpRegistrationInfo.Id;
                                            PatientLedger.Amount = HospitalConfiguration!=null? HospitalConfiguration.DefaultOPConsultingFee:0.00;
                                            PatientLedger.Description = "Payment waived as per ref #" + RefNum;
                                            PatientLedger.CompanyId = OpRegistrationInfo.CompanyId;
                                            PatientLedger.Type = TransactionType.WAIVER;
                                            Context.PatientLedgers.Add(PatientLedger);
                                            Context.SaveChanges();
                                            //create payment for registration fee
                                            PatientInvoiceManager.Instance.paymentForRegistrationFee(PatientLedger, Context);
                                            //daybook
                                            HmsPaymentDoubleEntryManager.Instance.RecordPayment(PatientLedger, Context);
                                        }
                                        else
                                        {
                                            dbContextTransaction.Rollback();
                                            return null;
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
                        throw (e);
                    }
                }
            }
            return OpRegistrationInfo;
        }

        public Registration UpdateOpRegistration(Registration OpRegistration)
        {
            Registration OpRegistrationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        OpRegistrationInfo = Context.Registrationes.Find(OpRegistration.Id);
                        if (OpRegistrationInfo != null)
                        {
                            Context.Entry(OpRegistrationInfo).CurrentValues.SetValues(OpRegistration);
                            Context.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return OpRegistrationInfo;
        }
        public Registration UpdateOpRegistrationConsulting(long OpRegistrationId, bool HasConsulted)
        {
            Registration OpRegistrationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                OpRegistrationInfo = Context.Registrationes.Find(OpRegistrationId);
                if (OpRegistrationInfo != null)
                {
                    OpRegistrationInfo.HasConsulted = HasConsulted;
                    Context.Entry(Context.Registrationes.Find(OpRegistrationId)).CurrentValues.SetValues(OpRegistrationInfo);
                    Context.SaveChanges();
                }
            }
            return OpRegistrationInfo;
        }
        public Registration UpdateOpRegistrationConsulting(long OpRegistrationId, bool HasConsulted, AccountMasterContext Context)
        {
            Registration OpRegistrationInfo = null;

            OpRegistrationInfo = Context.Registrationes.Find(OpRegistrationId);
            if (OpRegistrationInfo != null)
            {
                OpRegistrationInfo.HasConsulted = HasConsulted;
                Context.Entry(Context.Registrationes.Find(OpRegistrationId)).CurrentValues.SetValues(OpRegistrationInfo);
                Context.SaveChanges();
            }
            return OpRegistrationInfo;
        }

        public Registration UpdateOpRegistrationFromLedger(long OpRegistrationId, bool IsPaid, AccountMasterContext Context)
        {
            Registration OpRegistrationInfo = null;
            OpRegistrationInfo = Context.Registrationes.Find(OpRegistrationId);
            if (OpRegistrationInfo != null)
            {
                OpRegistrationInfo.IsFeePaid = IsPaid;
                Context.Entry(Context.Registrationes.Find(OpRegistrationId)).CurrentValues.SetValues(OpRegistrationInfo);
                Context.SaveChanges();
            }
            return OpRegistrationInfo;
        }
        public Registration UpdateOpRegistrationFromIP(long OpRegistrationId, Status OpStatus, AccountMasterContext Context)
        {
            Registration OpRegistrationInfo = null;
            OpRegistrationInfo = Context.Registrationes.Find(OpRegistrationId);
            if (OpRegistrationInfo != null)
            {
                OpRegistrationInfo.Status = OpStatus;
                Context.Entry(Context.Registrationes.Find(OpRegistrationId)).CurrentValues.SetValues(OpRegistrationInfo);
                Context.SaveChanges();
            }
            return OpRegistrationInfo;
        }
        public Boolean DeleteOpRegister(long OpId)
        {
            bool deleted = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Registration Registration = Context.Registrationes.Find(OpId);
                        if (Registration != null)
                        {
                            PatientLedger Ledger = Context.PatientLedgers.FirstOrDefault(p => p.OpRegistrationId == Registration.Id && p.RefNumber != null);
                            if (Ledger != null)
                            {
                                //daybook
                                HmsPaymentDoubleEntryManager.Instance.DeletePayments(Ledger, Context);
                                //remove inv payment
                                PatientInvoiceManager.Instance.RemovepaymentForRegistrationFee(Ledger, Context);
                                //remove fee payment
                                Context.PatientLedgers.Remove(Ledger);
                                Context.SaveChanges();
                            }
                            PatientLedger lLedger = Context.PatientLedgers.FirstOrDefault(p => p.OpRegistrationId == Registration.Id && p.Type == TransactionType.REGISTRATION_FEE);
                            if (lLedger != null)
                            {
                                //remove invoice
                                PatientInvoiceManager.Instance.RemoveInvoiceForRegistrationFee(lLedger, Context);
                                //remove fee
                                Context.PatientLedgers.Remove(lLedger);
                                Context.SaveChanges();
                            }
                        }
                        Context.Registrationes.Remove(Registration);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                    deleted = true;
                }
            }
            return deleted;
        }

        public IList<Registration> GetOPNumberContainsPrefix(string prefix, long companyId)
        {
            IList<Registration> Opnumber = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Opnumber = Context.Registrationes.Where(x => x.CompanyId == companyId && x.PatientOPNumber.Contains(prefix)).ToList<Registration>();
            }
            return Opnumber;
        }

        public IList<Registration> ListAllOpByEmployeeId(IList<Status> lStatus, long companyId, DateTime Date, bool isDoctor, User user)
        {
            IList<Registration> OpInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (isDoctor)
                {
                    OpInfo = Context.Registrationes.Include("RequestedDoctor").Include("Patient").Where(x => x.CompanyId == companyId && x.DateOfRegistration.Day == Date.Day && x.DateOfRegistration.Month == Date.Month && x.DateOfRegistration.Year == Date.Year && (x.RequestedDoctorId == user.EmployeeId || (x.RequestedDoctorId == null && x.CreatedBy.Contains(user.FirstName))) && x.HasConsulted == false && lStatus.Contains(x.Status)).OrderBy(x => x.TockenNo).ToList<Registration>();
                }
                else
                {
                    OpInfo = Context.Registrationes.Include("RequestedDoctor").Include("Patient").Where(x => x.CompanyId == companyId && x.DateOfRegistration.Day == Date.Day && x.DateOfRegistration.Month == Date.Month && x.DateOfRegistration.Year == Date.Year && (x.RequestedDoctorId == user.EmployeeId || (x.RequestedDoctorId == null && x.CreatedBy.Contains(user.FirstName))) && lStatus.Contains(x.Status)).OrderBy(x => x.TockenNo).ToList<Registration>();
                }
                return OpInfo;
            }
        }
    }
}
