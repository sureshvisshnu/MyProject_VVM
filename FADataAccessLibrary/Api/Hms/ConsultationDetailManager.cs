using fa.api.Accounting;
using fa.context;
using fa.model.Employee;
using fa.model.Hms.Master;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FADataAccessLibrary.Api.Hms
{
    public class ConsultationDetailManager
    {
        private static volatile ConsultationDetailManager instance;
        private static object syncRoot = new Object();
        ConsultationDetailManager()
        {

        }
        public static ConsultationDetailManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ConsultationDetailManager();
                    }
                }

                return instance;
            }
        }

        public void AddConsultationDetail(ConsultationDetail consultationDetail)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (consultationDetail.Id == 0)
                        {
                            Context.ConsultationDetails.Add(consultationDetail);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
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

        public ConsultationDetail GetConsultationDetailByEmployeeId(long companyId, long employeeId, Consultation consultation)
        {
            ConsultationDetail consultationDetail = null!;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                consultationDetail = Context.ConsultationDetails.Include("Employee").FirstOrDefault(x => x.CompanyId == companyId && x.EmployeeId == employeeId && x.ConsultationId == consultation.Id);
                return consultationDetail;
            }
        }

        public ConsultationDetail UpdateConsultationDetail(ConsultationDetail consultationDetail)
        {
            ConsultationDetail ConsultationDetailInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConsultationDetailInfo = Context.ConsultationDetails.Find(consultationDetail.Id);
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (ConsultationDetailInfo != null)
                        {
                            Context.Entry(ConsultationDetailInfo).CurrentValues.SetValues(consultationDetail);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        ConsultationDetailInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return ConsultationDetailInfo;
        }
    }
}
