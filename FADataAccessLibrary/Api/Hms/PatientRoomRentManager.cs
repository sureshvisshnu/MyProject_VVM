using fa.api.Hms;
using fa.context;
using fa.Data;
using fa.model.Hms.common;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using FADataAccessLibrary.Model.Hms.common;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FADataAccessLibrary.Api.Hms
{
    public class PatientRoomRentManager
    {
        private static volatile PatientRoomRentManager instance;
        private static object syncRoot = new Object();
        PatientRoomRentManager()
        {

        }
        public static PatientRoomRentManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PatientRoomRentManager();
                    }
                }
                return instance;
            }
        }
        public PatientRoomRent GetPatientRoomRentByIpIdDate(long Id,DateTime date)
        {
            PatientRoomRent PatientRoomRentInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientRoomRentInfo = Context.PatientRoomRents.FirstOrDefault(x => x.InPatientAdmissionId == Id);
                return PatientRoomRentInfo;
            }
        }

        public void generatePatientRoomRent()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                List<InPatientAdmission> inPatientAdmissions = Context.InPatientAdmissions.Where(x => x.Status != InPatientStatus.DISCHARGED).ToList();
                foreach (InPatientAdmission admission in inPatientAdmissions)
                {
                    DateTime startDate;
                    DateTime endDate = DateTime.Now;

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
                        CalculateRoomRent(admission, startDate, nextDay, Context);
                        startDate = nextDay;
                    }
                }
            }
        }
        public void CalculateRoomRent(InPatientAdmission admission, DateTime startDate, DateTime nextDay, AccountMasterContext Context)
        {
            int Count = 1;
            List<InPatientLocation> inPatientLocation = Context.InPatientLocations.Where(x => x.AdmissionId == admission.Id && (x.DateMovedOut.Date.ToString() == "0001-01-01" || x.DateMovedOut >= startDate)).ToList();
            foreach (InPatientLocation location in inPatientLocation.Where(x => x.Active).OrderBy(x => x.DateMovedIn))
            {
                double amount = 0;
                int ldays = 0;
                int lhours = 0;
                TimeSpan span = nextDay.Subtract(startDate);
                ldays = span.Days;
                lhours = span.Hours + (span.Minutes >= 30 ? 1 : 0);

                if (lhours > 12)
                {
                    ldays += 1;
                    lhours = 0;
                }
                else if (lhours > 4)
                {
                    lhours = 12;
                }
                else if (lhours > 1)
                {
                    lhours = 4;
                }

                if (ldays > 0 || lhours > 0)
                {
                    string timeDescription = string.Empty;
                    string timeDuration = string.Empty;
                    if (ldays > 0)
                    {
                        timeDescription = ldays == 1 ? " (" + ldays + " day)" : " (" + ldays + " days)";
                        timeDuration = ldays == 1 ? ldays + " day" : ldays + " days";
                    }
                    if (lhours > 0)
                    {
                        timeDescription += string.IsNullOrEmpty(timeDescription) ? "" : " and ";
                        timeDescription += lhours == 1 ? " (" + lhours + " hour)": " (" + lhours + " hours)";
                    }
                    Bed bed = Context.Beds.Include("Ward").Include("BedType").FirstOrDefault(x => x.Id == location.BedId);
                    if (bed != null)
                    {
                        List<Rent> rents = Context.Rents.Where(x => x.BedTypeId == bed.BedTypeId).ToList();
                        if (rents != null && rents.Count > 0)
                        {
                            var prioritizedRentPeriods = new List<RentPeriod>
                            {
                                RentPeriod.MONTHLY,
                                RentPeriod.WEEKLY,
                                RentPeriod.DAILY,
                                RentPeriod.TWELVEHOURS,
                                RentPeriod.FOURHOURS,
                                RentPeriod.HOURLY
                            };

                            rents = rents.OrderBy(rent => prioritizedRentPeriods.IndexOf(rent.RentPeriod)).ToList();

                            foreach (var rent in rents)
                            {
                                switch (rent.RentPeriod)
                                {
                                    case RentPeriod.MONTHLY:
                                        if (ldays >= 30)
                                        {
                                            amount += rent.Amount * (ldays / 30);
                                            ldays = ldays % 30;
                                        }
                                        break;

                                    case RentPeriod.WEEKLY:
                                        if (ldays >= 7)
                                        {
                                            amount += rent.Amount * (ldays / 7);
                                            ldays = ldays % 7;
                                        }
                                        break;

                                    case RentPeriod.DAILY:
                                        if (ldays == 1)
                                        {
                                            amount += rent.Amount * ldays;
                                            ldays = 0;
                                        }
                                        break;

                                    case RentPeriod.TWELVEHOURS:
                                        if (lhours == 12)
                                        {
                                            amount += rent.Amount * (lhours / 12);
                                            lhours = lhours % 12;
                                        }
                                        break;

                                    case RentPeriod.FOURHOURS:
                                        if (lhours == 4)
                                        {
                                            amount += rent.Amount * (lhours / 4);
                                            lhours = lhours % 4;
                                        }
                                        break;

                                    case RentPeriod.HOURLY:
                                        if (lhours == 1)
                                        {
                                            amount += rent.Amount * lhours;
                                            lhours = 0;
                                        }
                                        break;
                                }
                            }

                            if (ldays > 0 || lhours > 0)
                            {
                                var lastRent = rents.LastOrDefault();
                                if (lastRent != null)
                                {
                                    if (ldays > 0)
                                    {
                                        lhours += ldays * 24;
                                        ldays = 0;
                                    }

                                    if (lhours > 0)
                                    {
                                        switch (lastRent.RentPeriod)
                                        {
                                            case RentPeriod.TWELVEHOURS:
                                                amount += lastRent.Amount * ((double)lhours / 12);
                                                lhours = lhours % 12;
                                                break;

                                            case RentPeriod.FOURHOURS:
                                                amount += lastRent.Amount * ((double)lhours / 4);
                                                lhours = lhours % 4;
                                                break;

                                            case RentPeriod.HOURLY:
                                                amount += lastRent.Amount * lhours;
                                                lhours = 0;
                                                break;

                                            case RentPeriod.DAILY:
                                                amount += lastRent.Amount * ((double)lhours / 24);
                                                lhours = lhours % 24;
                                                break;

                                            case RentPeriod.WEEKLY:
                                                amount += lastRent.Amount * (((double)lhours / 24) / 7);
                                                lhours = 0;
                                                break;

                                            case RentPeriod.MONTHLY:
                                                amount += lastRent.Amount * (((double)lhours / 24) / 30);
                                                lhours = 0;
                                                break;
                                        }
                                    }
                                }
                            }

                            //Update in room rent table
                            PatientRoomRent patientRoomRent = new PatientRoomRent();
                            {
                                patientRoomRent.DatePosted = DateTime.Now;
                                patientRoomRent.DateOfRental = startDate.Date;
                                patientRoomRent.Ward = location?.Ward?.Name ?? "";
                                patientRoomRent.Bed = location?.Bed?.Name ?? "";
                                patientRoomRent.PatientId = (long)admission.PatientId;
                                patientRoomRent.InPatientAdmissionId = admission.Id;
                                patientRoomRent.Description = "Room rent for " + startDate.Date.ToString() + timeDescription;
                                patientRoomRent.Amount = amount;
                            }
                            Context.PatientRoomRents.Add(patientRoomRent);
                            Context.SaveChanges();

                            //Update in patient ledger
                            PatientLedger patientLedger = new PatientLedger();
                            {
                                patientLedger.Date = DateTime.Now;
                                patientLedger.PatientId = (long)admission.PatientId;
                                patientLedger.InPatientAdmissionId = admission.Id;
                                patientLedger.OpRegistrationId = admission.OpRegistrationId;
                                patientLedger.Amount = amount;
                                patientLedger.Description = "Room rent for " + startDate.Date.ToString() + timeDescription;
                                patientLedger.CompanyId = admission.CompanyId;
                                patientLedger.Type = TransactionType.ROOM_RENT;
                                if (timeDuration == "1 day")
                                {
                                    patientLedger.Bed = location?.Bed?.Name ?? "";
                                    patientLedger.RentTimeDuration = timeDuration;
                                }
                            }
                            Context.PatientLedgers.Add(patientLedger);
                            Context.SaveChanges();
                        }
                    }
                }
                startDate = location.DateMovedOut;
                Count++;
            }
        }
    }
}
