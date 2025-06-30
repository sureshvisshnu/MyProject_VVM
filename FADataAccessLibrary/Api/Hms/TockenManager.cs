using fa.context;
using fa.model.Hms.Master;
using System;
using System.Linq;

namespace fa.api.Hms
{
    public class TockenManager
    {
        private static volatile TockenManager instance;
        private static object syncRoot = new Object();
        TockenManager()
        {

        }
        public static TockenManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new TockenManager();
                    }
                }
                return instance;
            }
        }
    }
    }
