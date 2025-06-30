using fa.context;
using fa.model.Hms.Ip;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using Org.BouncyCastle.Utilities.Collections;
using System;

namespace fa.api.Hms
{
    public class MedicalTeamManager
    {
        private static volatile MedicalTeamManager instance;
        private static object syncRoot = new Object();
        MedicalTeamManager()
        {

        }
        public static MedicalTeamManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new MedicalTeamManager();
                    }
                }
                return instance;
            }
        }

        public MedicalTeam GetInPatientMedicalTeambyAdmissionId(long InPatientAdmissionId)
        {
            MedicalTeam InPatientMedicalTeamInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InPatientMedicalTeamInfo = Context.MedicalTeams.Include("PrimaryDoctor").Include("PrimaryCareGiver").Include("SecondaryDoctor").Include("SecondaryCareGiver").FirstOrDefault(x => x.AdmissionId == InPatientAdmissionId && x.Active == true);
                return InPatientMedicalTeamInfo;
            }
        }
        public List<MedicalTeam> GetInPatientMedicalTeambyAdmissionIdForReport(long inPatientAdmissionId)
        {
            using (AccountMasterContext context = new AccountMasterContext())
            {
                List<MedicalTeam> inPatientMedicalTeams = context.MedicalTeams
                    .Include("PrimaryDoctor")
                    .Include("PrimaryCareGiver")
                    .Include("SecondaryDoctor")
                    .Include("SecondaryCareGiver")
                    .Where(x => x.AdmissionId == inPatientAdmissionId)
                    .OrderBy(x => x.From)
                    .ToList();

                return inPatientMedicalTeams;
            }
        }
        public IList<MedicalTeam> ListMedicalTeambyDrId(long PrimaryDrId, DateTime FromDate)
        {
            IList<MedicalTeam> DrMedicalTeamInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                DrMedicalTeamInfo = Context.MedicalTeams
                    .Include(mt => mt.PrimaryDoctor)
                    .Include(mt => mt.PrimaryCareGiver)
                    .Include(mt => mt.SecondaryDoctor)
                    .Include(mt => mt.SecondaryCareGiver)
                    .Where(mt =>
                        mt.PrimaryDoctorId == PrimaryDrId &&
                        mt.From >= FromDate).ToList();
            }
            return DrMedicalTeamInfo;
        }
        public IList<MedicalTeam> ListMedicalTeambyAdmissionIds(long PrDrId)
        {
            IList<MedicalTeam> InPatientMedicalTeamInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InPatientMedicalTeamInfo = Context.MedicalTeams.Where                    
                    (mt =>mt.PrimaryDoctorId == PrDrId).ToList();
            }
            return InPatientMedicalTeamInfo;
        }
        public IList<MedicalTeam> GetMedicalTeambyAdmissionDateId(long AdmissionId, long PrDrId, DateTime FromDate, DateTime ToDate)
        {
            IList<MedicalTeam> InPatientMedicalTeamInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InPatientMedicalTeamInfo = Context.MedicalTeams
                    .Include(mt => mt.PrimaryDoctor)
                    .Include(mt => mt.PrimaryCareGiver)
                    .Include(mt => mt.SecondaryDoctor)
                    .Include(mt => mt.SecondaryCareGiver)
                    .Where(mt =>
                        mt.AdmissionId == AdmissionId &&
                        mt.PrimaryDoctorId == PrDrId && mt.From < FromDate).ToList();
            }
            return InPatientMedicalTeamInfo;
        }
        public IList<MedicalTeam> ListAllMedicalTeamHistoryByPrimaryId(string AdmissionBy, long PrimaryId, DateTime FromDate)
        {
            IList<MedicalTeam> IpMedicalTeamHistoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if(AdmissionBy == "Doctor")
                {
                    IpMedicalTeamHistoryInfo = (from History in Context.MedicalTeams.Include("PrimaryDoctor").Include("PrimaryCareGiver").Include("SecondaryDoctor").Include("SecondaryCareGiver").Include("AuthorizedByDoctor") where History.PrimaryDoctorId == PrimaryId && History.From < FromDate select History).OrderByDescending(x => x.Id).ToList();
                }
                else if(AdmissionBy == "Nurse")
                {
                    IpMedicalTeamHistoryInfo = (from History in Context.MedicalTeams.Include("PrimaryDoctor").Include("PrimaryCareGiver").Include("SecondaryDoctor").Include("SecondaryCareGiver").Include("AuthorizedByDoctor") where History.PrimaryCareGiverId == PrimaryId && History.From < FromDate select History).OrderByDescending(x => x.Id).ToList();
                }
                
                return IpMedicalTeamHistoryInfo;
            }
        }
        public IList<MedicalTeam> ListAllMedicalTeamHistoryByAdmitionId(long AdmitId)
        {
            IList<MedicalTeam> IpMedicalTeamHistoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {

                IpMedicalTeamHistoryInfo = (from History in Context.MedicalTeams.Include("PrimaryDoctor").Include("PrimaryCareGiver").Include("SecondaryDoctor").Include("SecondaryCareGiver").Include("AuthorizedByDoctor") where History.AdmissionId == AdmitId select History).OrderByDescending(x => x.Id).ToList();
                return IpMedicalTeamHistoryInfo;
            }
        }
        public IList<MedicalTeam> ListAllMedicalTeamHistoryByCareTakerIds(long[] NurseIds, long AdmissionId)
        {
            IList<MedicalTeam> IpMedicalTeamHistoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IpMedicalTeamHistoryInfo = (from History in Context.MedicalTeams
                                            .Include("PrimaryDoctor").Include("PrimaryCareGiver")
                                            .Include("SecondaryDoctor").Include("SecondaryCareGiver")
                                            .Include("AuthorizedByDoctor")
                                            where NurseIds.Contains((long)History.PrimaryCareGiverId)
                                            && History.AdmissionId == AdmissionId
                                            select History)
                                            .OrderByDescending(x => x.Id)
                                            .ToList();
                return IpMedicalTeamHistoryInfo;
            }
        }

        public IList<MedicalTeam> ListAllMedicalTeamHistoryByConsultantIds(long[] ConsultantIds, long AdmissionId)
        {
            IList<MedicalTeam> IpMedicalTeamHistoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IpMedicalTeamHistoryInfo = (from History in Context.MedicalTeams
                                            .Include("PrimaryDoctor").Include("PrimaryCareGiver")
                                            .Include("SecondaryDoctor").Include("SecondaryCareGiver")
                                            .Include("AuthorizedByDoctor")
                                            where ConsultantIds.Contains((long)History.PrimaryDoctorId)
                                            && History.AdmissionId == AdmissionId
                                            select History)
                                            .OrderByDescending(x => x.Id)
                                            .ToList();
                return IpMedicalTeamHistoryInfo;
            }
        }

        public IList<MedicalTeam> ListAllMedicalTeamHistoryByPrDr(long EmployeeId)
        {
            IList<MedicalTeam> MedicalTeamDr = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                MedicalTeamDr = (from History in Context.MedicalTeams.Include("PrimaryDoctor").Include("PrimaryCareGiver").Include("SecondaryDoctor").Include("SecondaryCareGiver").Include("AuthorizedByDoctor") where History.PrimaryDoctorId == EmployeeId select History).OrderByDescending(x => x.Id).ToList();
            }

            return MedicalTeamDr;
        }
        public IList<MedicalTeam> ListAllMedicalTeamHistoryByPrCG(long EmployeeId)
        {
            IList<MedicalTeam> MedicalTeamNr = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                MedicalTeamNr = (from History in Context.MedicalTeams.Include("PrimaryDoctor").Include("PrimaryCareGiver").Include("SecondaryDoctor").Include("SecondaryCareGiver").Include("AuthorizedByDoctor") where History.PrimaryCareGiverId == EmployeeId select History).OrderByDescending(x => x.Id).ToList();
            }

            return MedicalTeamNr;
        }
        public MedicalTeam AddMidicalTeam(MedicalTeam medicalTeam)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        MedicalTeam medicalTeamFromDB =Context.MedicalTeams.FirstOrDefault(x => x.AdmissionId == medicalTeam.AdmissionId && x.Active == true);
                        if(medicalTeamFromDB != null)
                        {                            
                            medicalTeamFromDB.To = medicalTeam.From;
                            medicalTeamFromDB.Active = false;
                            Context.Entry(Context.MedicalTeams.Find(medicalTeamFromDB.Id)).CurrentValues.SetValues(medicalTeamFromDB);
                            Context.SaveChanges();
                        }
                        Context.MedicalTeams.Add(medicalTeam);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        medicalTeam = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return medicalTeam;
        }
    }
}
