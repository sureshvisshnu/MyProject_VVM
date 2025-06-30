using fa.model.Common;
using fa.context;
using fa.model.Accounting.Masters;

namespace fa.api.Accounting
{
    public class AddressManager
    {
        private static volatile AddressManager instance;
        private static object syncRoot = new Object();
        AddressManager()
        {

        }
        public static AddressManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AddressManager();
                    }
                }

                return instance;
            }
        }
        public Address GetAddressById(long AddressId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Address AddressInfo = Context.Addresses.Find(AddressId);
                if (AddressInfo != null)
                {
                    return AddressInfo;
                }
            }
            return null;
        }
        public Address GetAddress(Company Company)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Address AddressInfo = Context.Addresses.Find(Company.AddressId);
                if (AddressInfo != null)
                {
                    return AddressInfo;
                }
            }
            return null;
        }

        public List<string> GetCompanyRelatedAddresses(Company company)
        {
            List<string> addresses = new List<string>();

            using (AccountMasterContext context = new AccountMasterContext())
            {
                Address companyAddress = context.Addresses.Find(company.AddressId);
                string formattedCompanyAddress = companyAddress != null ? FormatAddress(companyAddress) : null;
                if (formattedCompanyAddress != null)
                {
                    addresses.Add(formattedCompanyAddress);
                }

                var billingAddressIds = context.Customers
                    .Where(c => c.CompanyId == company.CompanyId)
                    .Select(c => c.BillingAddressId)
                    .Where(id => id.HasValue)
                    .ToList();

                var shippingAddressIds = context.Customers
                    .Where(c => c.CompanyId == company.CompanyId)
                    .Select(c => c.ShippingAddressId)
                    .Where(id => id.HasValue)
                    .ToList();

                var customerAddressIds = billingAddressIds.Concat(shippingAddressIds).Distinct().ToList();

                foreach (var addressId in customerAddressIds)
                {
                    Address address = context.Addresses.Find(addressId);
                    string formattedAddress = address != null ? FormatAddress(address) : null;
                    if (formattedAddress != null)
                    {
                        addresses.Add(formattedAddress);
                    }
                }

                var supplierAddressIds = context.Suppliers
                    .Where(s => s.CompanyId == company.CompanyId)
                    .Select(s => s.AddressId)
                    .Where(id => id.HasValue)
                    .Distinct()
                    .ToList();

                foreach (var addressId in supplierAddressIds)
                {
                    Address address = context.Addresses.Find(addressId);
                    string formattedAddress = address != null ? FormatAddress(address) : null;
                    if (formattedAddress != null)
                    {
                        addresses.Add(formattedAddress);
                    }
                }
            }

            return addresses;
        }
        public List<string> GetCustomerCompanyRelatedAddresses(Company company)
        {
            List<string> addresses = new List<string>();

            using (AccountMasterContext context = new AccountMasterContext())
            {
                Address companyAddress = context.Addresses.Find(company.AddressId);

                var billingAddressIds = context.Customers
                    .Where(c => c.CompanyId == company.CompanyId)
                    .Select(c => c.BillingAddressId)
                    .Where(id => id.HasValue)
                    .ToList();

                var shippingAddressIds = context.Customers
                    .Where(c => c.CompanyId == company.CompanyId)
                    .Select(c => c.ShippingAddressId)
                    .Where(id => id.HasValue)
                    .ToList();

                var customerAddressIds = billingAddressIds.Concat(shippingAddressIds).Distinct().ToList();

                foreach (var addressId in customerAddressIds)
                {
                    Address address = context.Addresses.Find(addressId);
                    string formattedAddress = address != null ? FormatAddress(address) : null;
                    if (formattedAddress != null)
                    {
                        addresses.Add(formattedAddress);
                    }
                }
            }
            return addresses;
        }
        private string FormatAddress(Address address)
        {
            return string.IsNullOrWhiteSpace(address.CityOrTown) ? null : address.CityOrTown;
        }


        public IList<String> ListAddress()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<string> AddressInfo = Context.Addresses.Select(x=>x.CityOrTown).Distinct().ToList();
                if (AddressInfo != null)
                {
                    return AddressInfo;
                }
            }
            return null;
        }
        public Address AddAddress(Address address)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Context.Addresses.Add(address);
                Context.SaveChanges();
            }
            return address;
        }
        public Address UpdateAddress(Address Address)
        {
            Address AddressInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                AddressInfo = Context.Addresses.Find(Address.AddressId);
                if (AddressInfo != null)
                {
                    Context.Entry(AddressInfo).CurrentValues.SetValues(Address);
                    Context.SaveChanges();
                    return AddressInfo;
                }
            }
            return AddressInfo;
        }
        public Boolean DeleteAddress(long AddressId)
        {
            Boolean Delete = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Address AddressInfo = Context.Addresses.Find(AddressId);
                if (AddressInfo != null)
                {
                    Context.Addresses.Remove(AddressInfo);
                    Context.SaveChanges();
                    Delete = true;
                }
            }
            return Delete;
        }
    }
}
