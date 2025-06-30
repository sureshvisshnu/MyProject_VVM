using fa.model.Common;
using fa.context;
using Microsoft.EntityFrameworkCore;
using FADataAccessLibrary.Model.Common;
using fa.model.Accounting.Masters;

namespace fa.api.Accounting
{
    public class CountryManager
    {
        private static volatile CountryManager instance;
        private static object syncRoot = new Object();
        CountryManager()
        {

        }
        public static CountryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CountryManager();
                    }
                }

                return instance;
            }
        }
        public Country GetCountryInfoById(long CountryId)
        {
            Country CountryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var Query = from Country in Context.Countries.Include("TaxType").Include("AccountingMethod").Include("CompanyType") where (Country.Id == CountryId) select Country;
                foreach (var Result in Query)
                {
                    CountryInfo = Result;
                    CountryInfo.TaxType.ToList();

                    break;
                }
                return CountryInfo;
            }
        }
        public CountrySaleTax CountryTaxById(long Id)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CountrySaleTax CountrySaleTax = Context.CountrySaleTaxs.FirstOrDefault(x=>x.Id == Id);
                return CountrySaleTax;
            }
        }
        public List<CountrySaleTax> ListCountryTaxByCountryId(long CountryId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                List<CountrySaleTax> CountrySaleTax = (from ConTax in Context.CountrySaleTaxs where ConTax.CountryId == CountryId select ConTax).ToList();
                return CountrySaleTax;
            }
        }
        public  bool IncludeTax(CountrySaleTax CountrySaleTax,Company company,DateTime dateTime,long customerId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Supplier supplier = null;
                Customer Customer = null;
                Address address = null;
                if (customerId!=0L)
                {
                     Customer =Context.Customers.Include("BillingAddress").FirstOrDefault(x=>x.Id==customerId);
                    if (Customer != null)
                    {
                        address= Customer.BillingAddress;
                    }
                    else
                    {
                        supplier = Context.Suppliers.Include("Address").FirstOrDefault(x => x.Id == customerId);
                        if (supplier != null)
                        {
                            address= supplier.Address;
                        }
                    }
                }
                if(CountrySaleTax != null && CountrySaleTax.EffectiveFrom <= dateTime && CountrySaleTax.EffectiveTo>= dateTime)
                {
                    if (company.Country.Name == "India")
                    {
                        if (CountrySaleTax.Rule == "RunIGST()")
                        {
                            return address != null && company.Address.StatesId != address.StatesId ? true : false;
                        }
                        else if (CountrySaleTax.Rule == "RunSGST()")
                        {
                            return address == null || company.Address.StatesId == address.StatesId ? true : false;
                        }
                        else if (CountrySaleTax.Rule == "RunCGST()")
                        {
                            return address == null || company.Address.StatesId == address.StatesId ? true : false;
                        }
                        else if (CountrySaleTax.Rule == "RunTCS()")
                        {
                            return (Customer != null && Customer.Balance > 10000000) || (supplier != null && supplier.Balance > 10000000) ? true : false;
                        }
                    }
                    else if (company.Country.Name == "Maldives")
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        


        public List<Country> ListAllCountry()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                List<Country> Country = (from Countries in Context.Countries where Countries.Active==true select Countries).ToList();
                return Country;
            }
        }
        public Country CountryIndia()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Country CountryInfo = Context.Countries.Find(1);
                if (CountryInfo != null)
                {
                    return CountryInfo;
                }
            }
            return null;
        }


    }
}
