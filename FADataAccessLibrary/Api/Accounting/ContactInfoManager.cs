using fa.model.Common;
using fa.context;
namespace fa.api.Accounting
{
    public class ContactInfoManager
    {
        private static volatile ContactInfoManager instance;
        private static object syncRoot = new Object();
        ContactInfoManager()
        {

        }
        public static ContactInfoManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ContactInfoManager();
                    }
                }

                return instance;
            }
        }
        public ContactInfo GetContactInfoId(long ContactInfoId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ContactInfo ContactInfoFromDB = Context.ContactInfos.Find(ContactInfoId);
                if (ContactInfoFromDB != null)
                {
                    return ContactInfoFromDB;
                }
            }
            return null;
        }
        public ContactInfo AddContactInfo(ContactInfo contactInfo)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Context.ContactInfos.Add(contactInfo);
                Context.SaveChanges();
            }
            return contactInfo;
        }
        public ContactInfo UpdateContactInfo(ContactInfo Contact)
        {
            ContactInfo ContactInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ContactInfo = Context.ContactInfos.Find(Contact.Id);
                if (ContactInfo != null)
                {
                    Context.Entry(ContactInfo).CurrentValues.SetValues(Contact);
                    Context.SaveChanges();
                    return ContactInfo;
                }
            }
            return ContactInfo;
        }
        public Boolean DeleteContactInfo(long ContactId)
        {
            Boolean Delete = false;
            using (AccountMasterContext context = new AccountMasterContext())
            {
                ContactInfo ContactInfo = context.ContactInfos.Find(ContactId);
                if (ContactInfo != null)
                {
                    context.ContactInfos.Remove(ContactInfo);
                    context.SaveChanges();
                    Delete = true;
                }
            }
            return Delete;
        }
    }
}
