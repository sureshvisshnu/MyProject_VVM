using System;
using System.Linq;
using fa.api.Accounting;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.System;

namespace fa.api.System
{
    public class IdGenerator
    {
        /*
         * This method will reutrn the next id for a IdSpace you pass in, it will create the IdSpace record if it does not exist. 
         * TODO: This method should be synced across all clients.
         */
        public static String IdSpaceCompanyGetNextId(IdSpace IdSpace,DateTime Date)
        {
            Boolean gotNew = false;
            do
            {
                using (AccountMasterContext Context = new AccountMasterContext())
                {
                    if (!IdSpace.IsResetDaily)
                    {
                        IdSpace IdSpaceFromDB = Context.IdSpaces.FirstOrDefault(x => x.Id == IdSpace.Id);
                        if (IdSpaceFromDB != null)
                        {
                            long CurrentNumber = IdSpaceFromDB.RunningSeed==1? IdSpaceFromDB.Seed: IdSpaceFromDB.RunningSeed;
                            IdSpaceFromDB.RunningSeed = CurrentNumber + 1;
                            Context.Entry(IdSpaceFromDB).CurrentValues.SetValues(IdSpaceFromDB);
                            Context.SaveChanges();
                            gotNew = true;
                        }
                        return string.Format("{0}{1}", IdSpaceFromDB.Prefix, IdSpaceFromDB.RunningSeed - 1);
                    }
                    else
                    {
                        IdSpace IdSpaceFromDB = Context.IdSpaces.FirstOrDefault(x => x.Id == IdSpace.Id);
                        if (IdSpaceFromDB != null)
                        {
                            long CurrentNumber = IdSpaceFromDB.Seed;
                            if (IdSpaceFromDB.Date == Date.Date)
                            {
                                CurrentNumber = IdSpaceFromDB.RunningSeed;
                                IdSpaceFromDB.RunningSeed = CurrentNumber + 1;
                                Context.Entry(IdSpaceFromDB).CurrentValues.SetValues(IdSpaceFromDB);
                                Context.SaveChanges();
                            }
                            else
                            {
                                IdSpaceFromDB.RunningSeed = CurrentNumber + 1;
                                IdSpaceFromDB.Date = Date.Date;
                                Context.Entry(IdSpaceFromDB).CurrentValues.SetValues(IdSpaceFromDB);
                                Context.SaveChanges();
                            }
                        }
                        return string.Format("{0}{1}", IdSpaceFromDB.Prefix, IdSpaceFromDB.RunningSeed - 1);
                    }
                }
            } while (!gotNew);
        }
        public static String IdSpaceCompanyGetNextPatientId(IdSpace IdSpace, DateTime Date)
        {
            Boolean gotNew = false;
            do
            {
                using (AccountMasterContext Context = new AccountMasterContext())
                {
                    IdSpace IdSpaceFromDB = Context.IdSpaces.FirstOrDefault(x => x.Id == IdSpace.Id);
                    if (IdSpaceFromDB != null)
                    {
                        long CurrentNumber = IdSpaceFromDB.RunningSeed == 1 ? IdSpaceFromDB.Seed : IdSpaceFromDB.RunningSeed;
                        IdSpaceFromDB.RunningSeed = CurrentNumber + 1;
                        Context.Entry(IdSpaceFromDB).CurrentValues.SetValues(IdSpaceFromDB);
                        Context.SaveChanges();
                        gotNew = true;
                    }
                    return string.Format("{0}{1}", IdSpaceFromDB.Prefix, (IdSpaceFromDB.Prefix == string.Empty && IdSpaceFromDB.RunningSeed.ToString().Length < 6) ? (IdSpaceFromDB.RunningSeed - 1).ToString("D6") : IdSpaceFromDB.RunningSeed - 1);
                }
            } while (!gotNew);
        }
        public static void IdSpaceCompanyGetPreviousRunningSeed(Company Company, EntryType EntryType, DateTime Date)
        {
            DateTime Start = CompanyManager.getFiscalYearStartDate(Company, Date);
            DateTime End = CompanyManager.getFiscalYearEndDate(Company, Date);
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IdSpace IdSpace = Context.IdSpaces.FirstOrDefault(x => x.CompanyId == Company.CompanyId && x.EntryType == EntryType && x.YearStartDate == Start.Date && x.YearEndDate == End.Date);
                IdSpace IdSpaceFromDB = Context.IdSpaces.Find(IdSpace.Id);
                if (IdSpaceFromDB != null)
                {
                    IdSpaceFromDB.Id = IdSpace.Id;
                    IdSpaceFromDB.RunningSeed = IdSpaceFromDB.RunningSeed - 1;
                    Context.Entry(IdSpaceFromDB).CurrentValues.SetValues(IdSpaceFromDB);
                    Context.SaveChanges();
                }
            }
        }
    }
}
