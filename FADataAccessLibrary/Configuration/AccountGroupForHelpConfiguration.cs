using fa.model.Accounting.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FADataAccessLibrary.Configuration
{
    public class AccountGroupForHelpConfiguration : IEntityTypeConfiguration<AccountGroupForHelp>
    {
        public void Configure(EntityTypeBuilder<AccountGroupForHelp> builder)
        {
            builder.ToTable("accountgroupforhelps");
            builder.HasData(
                new AccountGroupForHelp() { AccountGroupForHelpId = 1, HelpGroupDescription = "ExcludedAccountInReceipt" },
                new AccountGroupForHelp() { AccountGroupForHelpId = 2,  HelpGroupDescription = "BankAccountInReceipt" },
                new AccountGroupForHelp() { AccountGroupForHelpId = 3, HelpGroupDescription = "IncludedAccountCreditNoteService" },
                new AccountGroupForHelp() { AccountGroupForHelpId = 4, HelpGroupDescription = "IncludedAccountDebitNoteService" },
                new AccountGroupForHelp() { AccountGroupForHelpId = 5, HelpGroupDescription = "IncludedAccountInvoiceService" },
                new AccountGroupForHelp() { AccountGroupForHelpId = 6, HelpGroupDescription = "IncludedAccountBillService" },
                new AccountGroupForHelp() { AccountGroupForHelpId = 7, HelpGroupDescription = "IncludedAccountCompanySalesTaxReceivable" }
            );
            builder.HasMany<AccountGroup>(agh => agh.AccountGroups)
                .WithMany(ag => ag.AccountGroupsForHelp)
                .UsingEntity<Dictionary<string, object>>(
                    "accountgroupandaccountgrouphelp",
                    ag => ag.HasOne<AccountGroup>().WithMany().HasForeignKey("AccountGroupId"),
                    ag => ag.HasOne<AccountGroupForHelp>().WithMany().HasForeignKey("AccountGroupHelpId"),
                    ag => ag.HasData(GetAccountGroupAndHelpSeedData())
                );
        }

        private static Object[] GetAccountGroupAndHelpSeedData()
        {
            List<Object> accountGroupAndHelpList = new();
            accountGroupAndHelpList.AddRange(BuildAccountGroupAndHelp(1L, Enumerable.Range(301, 6)));
            accountGroupAndHelpList.AddRange(BuildAccountGroupAndHelp(2L, Enumerable.Range(302, 6)));
            accountGroupAndHelpList.AddRange(BuildAccountGroupAndHelp(3L, Enumerable.Range(1101, 6)
                .Concat(Enumerable.Range(1201, 5))
                .Concat(Enumerable.Range(1301, 5))
            ));
            accountGroupAndHelpList.AddRange(BuildAccountGroupAndHelp(4L, Enumerable.Range(1401, 27)));
            accountGroupAndHelpList.AddRange(BuildAccountGroupAndHelp(5L, Enumerable.Range(1101, 6)));
            accountGroupAndHelpList.AddRange(BuildAccountGroupAndHelp(7L, Enumerable.Range(801, 12)));
            return accountGroupAndHelpList.ToArray();
        }

        private static List<Object> BuildAccountGroupAndHelp(long accountGroupHelpId, IEnumerable<int> accountGroupIds)
        {
            List<Object> accountGroupAndHelpList = new();
            foreach (int accountGroupId in accountGroupIds)
            {
                accountGroupAndHelpList.Add(new { AccountGroupHelpId = accountGroupHelpId, AccountGroupId = (long)accountGroupId });
            }
            return accountGroupAndHelpList;
        }
    }
}