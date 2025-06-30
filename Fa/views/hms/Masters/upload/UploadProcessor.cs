using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.views.hms.masters.upload
{
    public interface UploadProcessor
    {
        void ProcessData(string FileName, bool HasHeader);
    }
}
