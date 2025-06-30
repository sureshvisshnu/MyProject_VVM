using fa.context;
using System;
using System.Linq;
using fa.model.Catalog;


namespace Fa.api.catalog
{
    public class CatalogSalesTaxMapManager
    {
        private static volatile CatalogSalesTaxMapManager instance;
        private static object syncRoot = new Object();
        CatalogSalesTaxMapManager()
        {

        }
        public static CatalogSalesTaxMapManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CatalogSalesTaxMapManager();
                    }
                }

                return instance;
            }
        }

        public bool CatalogSalesTaxMapCompanySalesTaxMap(long MapId)
        {
            CatalogItemSalesTaxMap CatalogItemSalesTaxMap = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CatalogItemSalesTaxMap = Context.CatalogItemSalesTaxMaps.FirstOrDefault(x=>x.SalesTaxMapId== MapId);
            }
            return CatalogItemSalesTaxMap != null ? true : false;
        }
    }
}
