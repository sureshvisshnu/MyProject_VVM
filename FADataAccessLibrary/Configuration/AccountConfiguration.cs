using fa.model.Accounting.Masters;
using fa.model.Common;
using Fa.api.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FADataAccessLibrary.Configuration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasData(
                new Account() { Name = "Integrated Sales Tax Payable Account", AccountGroupId = 810, Discription = "Sales Tax Payable Account, where all sales tax payables goes into" },
                new Account() { Name = "Central Sales Tax Payable Account", AccountGroupId = 810, Discription = "Sales Tax Payable Account, where all sales tax payables goes into" },
                new Account() { Name = "State  Sales Tax Payable Account", AccountGroupId = 810, Discription = "Sales Tax Payable Account, where all sales tax payables goes into" },
                new Account() { Name = "Tax at Source Payable Account", AccountGroupId = 810, Discription = "Sales Tax Payable Account, where all sales tax payables goes into" }
                );        
        }
    }
}
