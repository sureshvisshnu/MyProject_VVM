using fa.model.Accounting.Masters;
using fa.model.Employee;
using System.Collections.Generic;

namespace fa.api.Accounting
{
    public static class AccountHelper
    {

        

        public static List<AccountHelperData> GetHelpData(long HelpId, bool IncludeCustomers, bool IncludeSuppliers, bool IncludeGeneralAccounts, bool IncludeEmployee, string FilterString, Company Company )
        {
            List<AccountHelperData> HelperData = new List<AccountHelperData>();
            if (IncludeGeneralAccounts)
            {
                IList<Account> Accounts = AccountManager.Instance.GetAllGeneralAccountsByCompanyIdSearchTextWithHelpFilter( FilterString, Company, HelpId);
                if (Accounts.Count > 0)
                {
                    foreach (Account Account in Accounts)
                    {
                        AccountHelperData lAccount = new AccountHelperData();
                        lAccount.Id = Account.Id;
                        lAccount.Name = Account.Name;
                        lAccount.Type = Account.AccountType;
                        lAccount.GroupName = Account.AccountGroup.Name;
                        HelperData.Add(lAccount);
                    }
                }
            }

            if (IncludeCustomers)
            {
                IList<Customer> Customers = null;
                if (string.IsNullOrEmpty(FilterString))
                {
                    Customers = CustomerManager.Instance.GetAllCustomer(Company.CompanyId);
                }
                else
                {
                    Customers = CustomerManager.Instance.ListCustomerByName(FilterString, Company.CompanyId);
                }
                if (Customers.Count > 0)
                {

                    foreach (Customer Customer in Customers)
                    {
                        AccountHelperData lAccount = new AccountHelperData();
                        lAccount.Id = Customer.Id;
                        lAccount.Name = Customer.Name;
                        lAccount.Type = Customer.AccountType;
                        lAccount.GroupName = Customer.AccountGroup.Name;
                        if (Customer.BillingAddress != null)
                        {
                            lAccount.Address= Customer.BillingAddress.FullAddressInSingleLine;
                        }
                        if (Customer.ContactInfo != null)
                        {
                            lAccount.Phone= Customer.ContactInfo.Phone;
                        }
                        HelperData.Add(lAccount);
                    }
                }
            }

            if (IncludeSuppliers)
            {
                IList<Supplier> Suppliers = null;
                if (string.IsNullOrEmpty(FilterString))
                {
                    Suppliers = SupplierManager.Instance.GetAllSupplier(Company.CompanyId);
                }
                else
                {
                    Suppliers = SupplierManager.Instance.ListSupplierByName(FilterString, Company.CompanyId);
                }
                if (Suppliers.Count > 0)
                {

                    foreach (Supplier Supplier in Suppliers)
                    {
                        AccountHelperData lAccount = new AccountHelperData();
                        lAccount.Id = Supplier.Id;
                        lAccount.Name = Supplier.Name;
                        lAccount.Type = Supplier.AccountType;
                        lAccount.GroupName = Supplier.AccountGroup.Name;
                        if (Supplier.Address != null)
                        {
                            lAccount.Address = Supplier.Address.FullAddressInSingleLine;
                        }
                        if (Supplier.ContactInfo != null)
                        {
                            lAccount.Phone = Supplier.ContactInfo.Phone;
                        }
                        HelperData.Add(lAccount);
                    }
                }
            }

            if (IncludeEmployee)
            {
                IList<Employee> Employees= null;
                if (string.IsNullOrEmpty(FilterString))
                {
                    Employees = EmployeeManager.Instance.ListEmployeeByCompanyId(Company.CompanyId);
                }
                else
                {
                    Employees = EmployeeManager.Instance.ListEmployeeByCompanyIdWithFilter(FilterString, Company.CompanyId);
                }
                if (Employees.Count > 0)
                {

                    foreach (Employee Employee in Employees)
                    {
                        AccountHelperData lAccount = new AccountHelperData();
                        lAccount.Id = Employee.Id;
                        lAccount.Name = Employee.Name;
                        lAccount.Type = Employee.AccountType;
                        lAccount.GroupName = Employee.AccountGroup.Name;
                        if (Employee.Address != null)
                        {
                            lAccount.Address = Employee.Address.FullAddressInSingleLine;
                        }
                        if (Employee.ContactInfo != null)
                        {
                            lAccount.Phone = Employee.ContactInfo.Phone;
                        }
                        HelperData.Add(lAccount);
                    }
                }
            }

            //sort the data by name
            HelperData.Sort((x,y) => string.Compare(x.Name,y.Name));
            return HelperData;
        }

    }

    public class AccountHelperData
    {
        public long Id { get; set; }
        public AccountType Type { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string GroupName { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
