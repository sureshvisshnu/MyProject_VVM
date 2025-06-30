using fa.model.Accounting.Masters;
using fa.context;
using Microsoft.EntityFrameworkCore;

namespace fa.api.Accounting
{
    public class CustomerManager
    {
        private static volatile CustomerManager instance;
        private static object syncRoot = new Object();
        CustomerManager()
        {

        }
        public static CustomerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CustomerManager();
                    }
                }

                return instance;
            }
        }
        public Customer GetCustomerById(long CustomerId)
        {
            Customer CustomerInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CustomerInfo = Context.Customers.Include("AccountGroup").Include("BillingAddress").Include("ContactInfo").Include("CustomerLicenceDetail").Include("ShippingAddress").Include("CustomerLicenceDetail.CompanyCustomerLicenseMaster").Include("PaymentTerm").FirstOrDefault(x => x.Id == CustomerId);
                return CustomerInfo;
            }
        }
        public Customer GetCustomerByName(String CustomerName, long CompanyId)
        {
            Customer CustomerInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CustomerInfo = Context.Customers.Include("PaymentTerm").FirstOrDefault(x => x.Name == CustomerName && x.CompanyId==CompanyId);
                return CustomerInfo;
            }
        }

        public Customer CheckCustomerNameInUpdate(String CustomerName, long CustomerId, long CompanyId)
        {
            Customer CustomerInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CustomerInfo = Context.Customers.FirstOrDefault(x => x.Name == CustomerName && x.CompanyId==CompanyId && !x.Id.Equals(CustomerId));
                return CustomerInfo;
            }
        }
        public Customer CheckCustomerSubCustomers(long CustomerId)
        {
            Customer CustomerInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CustomerInfo = Context.Customers.FirstOrDefault(x => x.ParentAccountId == CustomerId);
            }
            return CustomerInfo;
        }
        public Customer CheckCompanyHaveCustomer(long CompanyId)
        {
            Customer CustomerInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CustomerInfo = Context.Customers.FirstOrDefault(x => x.CompanyId == CompanyId);
            }
            return CustomerInfo;
        }
        public IList<Customer> ListParentCustomerByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Customer> CustomerInfo = (from Customer in Context.Customers where Customer.CompanyId==CompanyId where Customer.ParentAccountId.Equals(null) select Customer).ToList();
                return CustomerInfo;
            }
        }
        public IList<Customer> ListCustomerByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Customer> CustomerInfo = (from Customer in Context.Customers.Include("BillingAddress").Include("ContactInfo").Include("ShippingAddress").Include("CustomerLicenceDetail") where Customer.CompanyId == CompanyId select Customer).ToList();
                return CustomerInfo;
            }
        }
        public List<Customer> ListCustomerByName(String CustomerName, long CompanyId)
        {
            List<Customer> CustomerInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CustomerInfo = Context.Customers.Include("AccountGroup").Include("BillingAddress").Include("ContactInfo").Include("ShippingAddress").Where(x => (x.Name.Contains(CustomerName) || x.ContactInfo.Phone.Contains(CustomerName) || x.ContactInfo.Mobile.Contains(CustomerName)) && x.CompanyId == CompanyId).ToList<Customer>();
                return CustomerInfo;
            }
        }
        public IList<Customer> GetAllCustomer(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Customer> CustomerInfo = (from c in Context.Customers.Include("AccountGroup").Include("BillingAddress").Include("ContactInfo").Include("ShippingAddress") where c.CompanyId== CompanyId select c).ToList();
                return CustomerInfo;
            }
        }
      
        public bool DeleteCustomer(long CustomerId)
        {

            bool deleted = false;
            using (AccountMasterContext context = new AccountMasterContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Customer Customer = context.Customers.Include("BillingAddress").Include("ContactInfo").Include("ShippingAddress").Include("CustomerLicenceDetail").Where(p => p.Id == CustomerId).First<Customer>();

                        if (Customer != null)
                        {
                            if (Customer.BillingAddress != null)
                            {
                                context.Addresses.Where(Badd => Badd.AddressId == Customer.BillingAddress.AddressId).ToList().ForEach(Badd => context.Addresses.Remove(Badd));
                            }
                            if (Customer.ContactInfo != null)
                            {
                                context.ContactInfos.Where(ci => ci.Id == Customer.ContactInfo.Id).ToList().ForEach(ci => context.ContactInfos.Remove(ci));
                            }
                            if (Customer.ShippingAddress != null)
                            {
                                context.Addresses.Where(Sadd => Sadd.AddressId == Customer.ShippingAddress.AddressId).ToList().ForEach(Sadd => context.Addresses.Remove(Sadd));
                            }

                            if (Customer.CustomerLicenceDetail.Count > 0)
                            {
                                context.CustomerLicenceDetails.Where(p => p.CustomerId == Customer.Id).ToList().ForEach(p => context.CustomerLicenceDetails.Remove(p));
                            }

                        }
                        context.Customers.Remove(Customer);
                        context.SaveChanges();
                        dbContextTransaction.Commit();
                        deleted = true;
                    }
                    #pragma warning disable 0168 // variable declared but not used.
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        deleted = false;
                    }
                    #pragma warning restore 0168
                }
            }
            return deleted;
        }
        public Customer AddCustomer(Customer customer)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {                                
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.Customers.Add(customer);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        customer = null;
                        dbContextTransaction.Rollback();
                    }
                }
            }
            return customer;
        }

        public Customer UpdateCustomer(Customer Customer)
        {
            Customer CustomerInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CustomerInfo = Context.Customers.Find(Customer.Id);
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (CustomerInfo != null)
                        {
                            Context.Entry(CustomerInfo).CurrentValues.SetValues(Customer);
                            Context.SaveChanges();

                            Customer CustomerInfoFromDB = GetCustomerById(Customer.Id);
                            foreach (CustomerLicenceDetail OldInfo in CustomerInfoFromDB.CustomerLicenceDetail)
                            {
                                CustomerLicenceDetail NewInfo = Customer.CustomerLicenceDetail.FirstOrDefault(x => x.CustomerLicenceId == OldInfo.CustomerLicenceId);
                                if (NewInfo == null)
                                {
                                    Context.CustomerLicenceDetails.Remove(OldInfo);
                                }
                                else
                                {
                                    Customer.CustomerLicenceDetail.Remove(NewInfo);
                                    NewInfo.CustomerId = Customer.Id;
                                    Context.Entry(NewInfo).State = EntityState.Modified;
                                }
                                Context.SaveChanges();
                            }
                            foreach (CustomerLicenceDetail LInfo in Customer.CustomerLicenceDetail)
                            {
                                LInfo.CustomerId = Customer.Id;
                                LInfo.CompanyId = Customer.CompanyId;
                                Context.CustomerLicenceDetails.Add(LInfo);
                                Context.SaveChanges();
                            }
                            dbContextTransaction.Commit();
                            
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        CustomerInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return CustomerInfo;
        }

    }
}
