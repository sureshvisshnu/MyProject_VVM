using fa.context;
using fa.model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fa.api.Accounting
{
    public class AdditionalDetailsManager
    {
        private static volatile AdditionalDetailsManager instance;
        private static object syncRoot = new Object();

        AdditionalDetailsManager()
        {

        }
        public static AdditionalDetailsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AdditionalDetailsManager();
                    }
                }

                return instance;
            }
        }


        public IList<AdditionalDetail> GetAllAdditionalDetail(string SId,AdditionalDetailSourceType SourceType,long CompanyId)
        {
            IList<AdditionalDetail> AdditionalDetailInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                AdditionalDetailInfo = (from AdditionalDetail in Context.AdditionalDetails where AdditionalDetail.CompanyId == CompanyId && AdditionalDetail.SourceType== SourceType && AdditionalDetail.SourceId == SId select AdditionalDetail).ToList();
            }
            return AdditionalDetailInfo;
        }

        public void AddAdditionalDetail(IList<AdditionalDetail> lAdditionalDetail,string SId, AdditionalDetailSourceType SType)
        {
            using (AccountMasterContext context = new AccountMasterContext())
            {
                context.AdditionalDetails.Where(x => x.SourceId == SId && x.SourceType== SType).ToList().ForEach(x => context.AdditionalDetails.Remove(x));
                context.SaveChanges();

                if (lAdditionalDetail.Count>0)
                {
                    foreach(AdditionalDetail detail in lAdditionalDetail)
                    {
                        detail.SourceId = SId;
                        detail.SourceType = SType;
                        context.AdditionalDetails.Add(detail);
                        context.SaveChanges();
                    }
                }
            }

        }

        public List<AdditionalDetail> GetAllUniqueDetail(long CompanyId)
        {
            List<AdditionalDetail> Details = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Details = Context.AdditionalDetails.Where(c => c.CompanyId == CompanyId).GroupBy(d=>d.Detail).Select(s=>s.FirstOrDefault()).ToList();
            }
            return Details;
        }




    }
}
