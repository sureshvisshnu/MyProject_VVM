using System.Collections.Generic;
using System.Linq;
using fa.model.Accounting.Masters;
using fa.context;
using System;

namespace fa.api.Accounting
{
    public class CompanyTypeManager
    {
        private static volatile CompanyTypeManager instance;
        private static object syncRoot = new Object();
        CompanyTypeManager()
        {

        }
        public static CompanyTypeManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CompanyTypeManager();
                    }
                }

                return instance;
            }
        }
        public CompanyType GetCompanyTypeById(long CompanyTypeId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CompanyType CompanyTypeInfo = Context.CompanyTypes.Find(CompanyTypeId);
                if (CompanyTypeInfo != null)
                {
                    return CompanyTypeInfo;
                }
            }
            return null;
        }
        public IList<CompanyType> GetAllCompanyType()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<CompanyType> CompanyTypeInfo = Context.CompanyTypes.ToList<CompanyType>();
                return CompanyTypeInfo;
            }
        }
    }
}
