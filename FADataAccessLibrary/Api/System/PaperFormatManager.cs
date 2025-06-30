using fa.context;
using fa.model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fa.api.System
{
    public class PaperFormatManager
    {
        private static volatile PaperFormatManager instance;
        private static object syncRoot = new Object();
        PaperFormatManager()
        {

        }
        public static PaperFormatManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PaperFormatManager();
                    }
                }
                return instance;
            }
        }

        public PrintPaperFormat GetPrintPaperFormatById(long Id)
        {
            PrintPaperFormat PrintPaperFormatInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PrintPaperFormatInfo = Context.PrintPaperFormats.FirstOrDefault(x => x.Id == Id);
                return PrintPaperFormatInfo;
            }
        }

        public IList<PrintPaperFormat> ListPrintPaperFormat()
        {
            IList<PrintPaperFormat> PrintPaperFormatInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PrintPaperFormatInfo = Context.PrintPaperFormats.ToList();
                return PrintPaperFormatInfo;
            }
        }


    }
}
