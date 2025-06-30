using Microsoft.Win32;
using RawPrint;
using RawPrint.NetStd;
using System;
using System.IO;
using System.Windows.Forms;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace fa.views.utils.Common
{
    class DotmatrixPrint
    {
        public static string Enlarged_Bold_On = (char)15 + (char)14 + "";
        public static string Enlarged_Bold_Off = (char)15 + (char)18 + "";
        public static string compressed_On = (char)15 + "";
        public static string compressed_Bold_Off = (char)18 + (char)27 + "F";
        public static string Bold_On = (char)27 + "E";
        public static string Bold_Off = (char)27 + "F";
        public static string Reverse_Paper = (char)27 + "jj";
        public static string Enlarged = (char)14+"";
        public static string Enlargedoff = (char)14 + "";

        public void printTXT(string PDfFilePath, string PDfFileName)
        {
            String Printer = Global.getDefaultPrinter();
            //if (PdfPrinter.IsOnline(Printer))
            //{
                //if (PdfPrinter.Print(true))
                //{
                    string Filepath = @PDfFilePath;
                    string Filename = PDfFileName;
                    string PrinterName = Printer;
                    IPrinter printer = new Printer();
                    printer.PrintRawFile(PrinterName, Filepath, Filename);
                //}
                //else
                //{
                //    MessageBox.Show("Printer " + Printer + " is offline");
                //}
            //}
            //else
            //{
            //    MessageBox.Show("Printer " + Printer + " is offline");
            //}
        }
        public void DoPrint(string fileName,bool isprint)
        {
            SaveFileDialog sfDlg = new SaveFileDialog();
            sfDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfDlg.RestoreDirectory = true;
            sfDlg.FileName = fileName;
            try
            {
                var windowsTempPath = System.IO.Path.GetTempPath();
                Directory.CreateDirectory(windowsTempPath + "");
                var filePath = windowsTempPath + "//" + fileName + ".txt";
                if (isprint)
                {
                    printTXT(filePath, fileName + ".txt");
                }
                else if (sfDlg.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.Copy(filePath, Path.GetFullPath(sfDlg.FileName), true);

                    if (DialogResult.Yes == MessageBox.Show(
                                    "Do you want to open file?",
                                    "Confirmation",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question))
                    {
                        System.Diagnostics.Process.Start("notepad.exe", sfDlg.FileName);
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(
                        exc.Message,
                        "Aborted",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
            finally
            {
                sfDlg.Dispose();
            }

        }
    }
}
