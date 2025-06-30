using fa.model.Accounting.Masters;
using fa.context;
namespace fa.api.Accounting
{
    public class TaxinfoManager
    {
        private static volatile TaxinfoManager instance;
        private static object syncRoot = new Object();
        TaxinfoManager()
        {

        }
        public static TaxinfoManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new TaxinfoManager();
                    }
                }

                return instance;
            }
        }
        public TaxInfo GetTaxInfoById(long TaxId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                TaxInfo TaxInfo = Context.TaxInfos.Find(TaxId);
                if (TaxInfo != null)
                {
                    return TaxInfo;
                }
            }
            return null;
        }
        public TaxInfo AddTaxInfo(TaxInfo tax)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Context.TaxInfos.Add(tax);
                Context.SaveChanges();
            }
            return tax;
        }
        public TaxInfo UpdateTaxInfo(TaxInfo Tax)
        {
            TaxInfo TaxInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                TaxInfo = Context.TaxInfos.Find(Tax.Id);
                if (TaxInfo != null)
                {
                    Context.Entry(TaxInfo).CurrentValues.SetValues(Tax);
                    Context.SaveChanges();
                    return TaxInfo;
                }
            }
            return TaxInfo;
        }
        public Boolean DeleteTaxInfo(long TaxId)
        {
            Boolean Delete = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                TaxInfo TaxInfo = Context.TaxInfos.Find(TaxId);
                if (TaxInfo != null)
                {
                    Context.TaxInfos.Remove(TaxInfo);
                    Context.SaveChanges();
                    Delete = true;
                }
            }
            return Delete;
        }
    }
}
