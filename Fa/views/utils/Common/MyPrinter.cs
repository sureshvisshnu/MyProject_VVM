using Syncfusion.Pdf;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace fa.views.utils
{
    class MyPrinter
    {
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Name);
    }
}
