using fa.api.Accounting;
using fa.context;
using fa.model.Hms.Op;
using fa.model.OrderManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FADataAccessLibrary.Api.Hms
{
    public class PatientAppointmentManager
    {
        private static volatile PatientAppointmentManager instance;
        private static object syncRoot = new Object();

        public static PatientAppointmentManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PatientAppointmentManager();
                    }
                }

                return instance;
            }
        }

        public PatientAppointment AddPatientAppointment(PatientAppointment patientAppointment)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.PatientAppointments.Add(patientAppointment);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        patientAppointment = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return patientAppointment;
        }

        public PatientAppointment UpdatePatientAppointment(PatientAppointment patientAppointment)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        PatientAppointment oldAppointment = Context.PatientAppointments.Find(patientAppointment.Id);
                        if (oldAppointment != null)
                        {
                            Context.Entry(oldAppointment).CurrentValues.SetValues(patientAppointment);
                        }
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                        return patientAppointment;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
        }

        public bool DeletePatientAppointment(long appointmentId)
        {
            using (AccountMasterContext context = new AccountMasterContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var appointment = context.PatientAppointments.FirstOrDefault(a => a.Id == appointmentId);

                        if (appointment == null)
                            return false;

                        context.PatientAppointments.Remove(appointment);
                        context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public List<PatientAppointment> GetAppointmentsByDate(DateTime date)
        {
            try
            {
                using (AccountMasterContext context = new AccountMasterContext())
                {
                    return context.PatientAppointments.Include(a => a.Patient).Include(a => a.Consultant).Where(a => a.FromDateOfAppointment.Date.AddDays(-1) < date.Date && a.ToDateOfAppointment.Date.AddDays(1) > date.Date).OrderBy(a => a.StartingTime).ToList();
                }
            }
            catch (Exception ex)
            {
                return new List<PatientAppointment>();
            }
        }

        public List<PatientAppointment> GetAppointmentsBetweenFromToDate(DateTime Fromdate, DateTime Todate, string startTime, string endTime, long consultantId)
        {
            try
            {
                using (AccountMasterContext context = new AccountMasterContext())
                {
                    return context.PatientAppointments
                        .Include(a => a.Patient)
                        .Include(a => a.Consultant)
                        .Where(a => a.ConsultantId == consultantId && a.FromDateOfAppointment.Date >= Fromdate.Date
                                 && a.ToDateOfAppointment.Date <= Todate.Date).ToList();
                }
            }
            catch (Exception ex)
            {
                return new List<PatientAppointment>();
            }
        }
        public List<PatientAppointment> GetAppointmentsByDateRange(long ConsultantId, DateTime startDate, DateTime endDate)
        {
            try
            {
                using (AccountMasterContext context = new AccountMasterContext())
                {
                    return context.PatientAppointments.Include(a => a.Patient).Include(a => a.Consultant).Where(a => a.ConsultantId == ConsultantId && a.FromDateOfAppointment >= startDate.Date && a.FromDateOfAppointment <= endDate.Date).OrderBy(a => a.FromDateOfAppointment).ThenBy(a => a.StartingTime).ToList();
                }
            }
            catch (Exception ex)
            {
                return new List<PatientAppointment>();
            }
        }
        public PatientAppointment GetAppointmentById(long id)
        {
            using (AccountMasterContext context = new AccountMasterContext())
            {
                return context.PatientAppointments.Include(a => a.Patient).ThenInclude(p => p.Address).FirstOrDefault(a => a.Id == id);
            }
        }
        public List<PatientAppointment> GetAppointmentsByConsultantAndDateRange(long consultantId, DateTime startDate, DateTime endDate)
        {
            using (AccountMasterContext context = new AccountMasterContext())
            {
                return context.PatientAppointments.Include(a => a.Patient).Where(a => a.ConsultantId == consultantId && a.FromDateOfAppointment >= startDate.Date && a.FromDateOfAppointment <= endDate.Date).AsNoTracking().ToList();
            }
        }

        public List<PatientAppointment> GetExistingAppointments(DateTime Fromdate, DateTime Todate, string startTime, string endTime)
        {
            try
            {
                using (AccountMasterContext context = new AccountMasterContext())
                {
                    return context.PatientAppointments.Where(a => a.FromDateOfAppointment.Date == Fromdate.Date && a.ToDateOfAppointment.Date == Todate.Date && a.StartingTime == startTime && a.EndTime == endTime).ToList();
                }
            }
            catch (Exception ex)
            {
                return new List<PatientAppointment>();
            }
        }
    }
}
