using fa.model.Common;
using fa.context;
using System.Collections.Generic;
using System.Linq;
using System;

namespace fa.api.Accounting
{
    public class StateManager
    {
        private static volatile StateManager instance;
        private static object syncRoot = new Object();
        StateManager()
        {

        }
        public static StateManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new StateManager();
                    }
                }

                return instance;
            }
        }
        public State GetStateById(long StateId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                State StateFromDB = Context.States.Find(StateId);
                if (StateFromDB != null)
                {
                    return StateFromDB;
                }
            }
            return null;
        }

        public List<State> GetAllStates(long countryId)
        {
            try
            {
                using var context = new AccountMasterContext();
                return context.States
                            .Where(x => x.CountryId == countryId)
                            .OrderBy(x => x.Name)
                            .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting states: {ex.Message}");
                return new List<State>();
            }
        }
        public List<State> ListAllStatesByCountry(long CountryId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                List<State> State = (from States in Context.States where States.CountryId == CountryId select States).ToList();
                return State;
            }
        }

        public long? GetStateByName(string patientState, long? countryId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                string State = patientState?.Trim().ToLower();
                long stateId = Context.States.Where(x => x.Name.Trim().ToLower() == State && x.CountryId == countryId).Select(x => x.Id).FirstOrDefault();
                if (stateId != 0)
                {
                    return stateId;
                }
            }
            return null;
        }
    }
}
