using fa.api.Log;
using fa.model.Accounting.Masters;
using Microsoft.Win32.SafeHandles;
using PDFtoPrinter;
using System;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Management;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace fa.views.utils
{
    public static class PdfPrinter
    {
        public static void printPDFInIron(string PrinterName, string printFilePath, string pdfFileName)
        {
            if (string.IsNullOrEmpty(PrinterName))
            {
               
                PrintDialog pdi = new PrintDialog();
                if (pdi.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    PrinterSettings PrinterSettings = new PrinterSettings();
                    string DefaultPrinter = PrinterSettings.PrinterName;
                    if (IsOnline(pdi.PrinterSettings.PrinterName))
                    {
                        MyPrinter.SetDefaultPrinter(pdi.PrinterSettings.PrinterName);

                        var printer = new PDFtoPrinterPrinter();
                        printer.Print(new PrintingOptions(pdi.PrinterSettings.PrinterName, printFilePath+ pdfFileName));
                        MyPrinter.SetDefaultPrinter(DefaultPrinter);
                    }
                    else
                    {
                        MessageBox.Show("Printer " + pdi.PrinterSettings.PrinterName + " is offline");
                    }
                }
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                if (IsOnline(PrinterName))
                {
                    MyPrinter.SetDefaultPrinter(PrinterName);
                    var printer = new PDFtoPrinterPrinter();
                    printer.Print(new PrintingOptions(PrinterName, printFilePath + pdfFileName));
                }
                else
                {
                    MessageBox.Show("Printer " + PrinterName + " is offline");
                }
            }
            Cursor.Current = Cursors.Default;
        }

        public static void printerToPDF(string userPrinter, string printFilePath, string pdfFileName)
        {
            Cursor.Current = Cursors.WaitCursor;
            PrinterSettings PrinterSettings = new PrinterSettings();
            string DefaultPrinter = PrinterSettings.PrinterName;
            if (IsOnline(userPrinter))
            {
                MyPrinter.SetDefaultPrinter(userPrinter);
                var printer = new PDFtoPrinterPrinter();
                printer.Print(new PrintingOptions(userPrinter, printFilePath + pdfFileName));
                MyPrinter.SetDefaultPrinter(DefaultPrinter);
            }
            else
            {
                MessageBox.Show("Printer " + userPrinter + " is offline");
            }
        }
        public static bool IsOnline(string printerName)
        {
            try
            {
                LocalPrintServer printServer = new LocalPrintServer();

                PrintQueue queue = printServer
                    .GetPrintQueues()
                    .FirstOrDefault(p =>
                        p.Name.Equals(printerName, StringComparison.OrdinalIgnoreCase))!;

                if (queue == null)
                    return false;

                queue.Refresh();

                // If any bad status flag is set → printer is NOT online
                if (queue.QueueStatus.HasFlag(PrintQueueStatus.Offline) ||
                    queue.QueueStatus.HasFlag(PrintQueueStatus.Error) ||
                    queue.QueueStatus.HasFlag(PrintQueueStatus.PaperOut) ||
                    queue.QueueStatus.HasFlag(PrintQueueStatus.Paused) ||
                    queue.QueueStatus.HasFlag(PrintQueueStatus.NotAvailable) ||
                    queue.QueueStatus.HasFlag(PrintQueueStatus.DoorOpen))
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsOnlinexx(string PrinterName)
        {
            ManagementScope scope = new ManagementScope(@"\root\cimv2");
            scope.Connect();
            ManagementObjectSearcher searcher = new
             ManagementObjectSearcher("SELECT * FROM Win32_Printer");
            string printerName = "";
            foreach (ManagementObject printer in searcher.Get())
            {
                printerName = printer["Name"].ToString().ToLower();
                if (printerName.Equals(PrinterName.ToLower()))
                {
                    if (printer["WorkOffline"].ToString().ToLower().Equals("true"))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return true;
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        public static bool Print(bool IsDotmatrix)
        {
            try
            {
                SafeFileHandle fh = CreateFile((IsDotmatrix?"LPT1":"USB001"), FileAccess.Write, 0, IntPtr.Zero, FileMode.OpenOrCreate, 0, IntPtr.Zero);
                return fh.IsInvalid;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return true;
        }
        static string PaperFormat = (Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate.ToString(Global.Company.DateFormat) == Global.getCurrentFiscalYearStartDate().ToString(Global.Company.DateFormat) && x.YearEndDate.ToString(Global.Company.DateFormat) == Global.getCurrentFiscalYearEndDate().ToString(Global.Company.DateFormat) && x.EntryType == EntryType.SALES)).PrintPaperFormat.Name;
       
    }
}
