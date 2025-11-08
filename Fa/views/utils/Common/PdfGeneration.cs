using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using fa.model.Accounting.Masters;
using fa.api.utils;
using Font = iTextSharp.text.Font;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace fa.views.utils
{
    public class PdfGeneration
    {
        public string FileName;
        public bool IsPrint;
        public byte[] PdfFile;
        public string? ConsultingFileName;
        public string? ConsultingFilePath;
        public void SavePdfFile()
        {
            bool wasFileSaved = false;
            System.Windows.Forms.Cursor.Current = Cursors.Default;
            SaveFileDialog sfDlg = new SaveFileDialog();
            try
            {
                sfDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                sfDlg.Filter = string.Format("{1} files|*.{0}", "pdf", "pdf");
                sfDlg.RestoreDirectory = true;
                sfDlg.FileName = FileName;
                var windowsTempPath = Path.GetTempPath();
                Directory.CreateDirectory(windowsTempPath + "");
                var printFilePath = String.Format("{0}", windowsTempPath);
                var printFileName = String.Format("{0}.pdf", sfDlg.FileName);
                var tempFilePath = String.Format("{0}\\{1}.pdf", Path.GetTempPath(), sfDlg.FileName);
                System.IO.File.WriteAllBytes(tempFilePath, PdfFile);

                if (IsPrint)
                {
                    try
                    {
                        File.WriteAllBytes(printFilePath + "\\" + printFileName, PdfFile);
                        PdfPrinter.printPDFInIron(Global.getDefaultPrinter(), printFilePath, printFileName);
                    }
                    catch (Exception e)
                    {
                        if (e.Message.Contains("The process cannot access the file"))
                        {
                            Random random = new Random();
                            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                            string randomFileName = new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray());
                            var printFilePathCatch = String.Format("{0}", windowsTempPath);

                            printFileName = String.Format("{0}Print{1}.pdf", sfDlg.FileName, randomFileName);

                            File.WriteAllBytes(printFilePathCatch + "\\" + printFileName, PdfFile);
                            PdfPrinter.printPDFInIron(Global.getDefaultPrinter(), printFilePathCatch, printFileName);
                        }
                    }
                }
                else if (FileName == "Prescription")
                {
                    Random r = new Random();
                    int Filecount = r.Next(1, 999);

                    printFileName = String.Format("{0}.pdf", sfDlg.FileName + "Preview");
                    if (!FileIsOpen(printFilePath + "\\" + printFileName))
                    {
                        File.WriteAllBytes(printFilePath + "\\" + printFileName, PdfFile);
                        System.Diagnostics.Process.Start(new ProcessStartInfo { FileName = @printFilePath + "\\" + printFileName, UseShellExecute = true });
                    }
                    else
                    {
                        printFileName = String.Format("{0}.pdf", sfDlg.FileName + "Preview" + Filecount);
                        File.WriteAllBytes(printFilePath + "\\" + printFileName, PdfFile);
                        System.Diagnostics.Process.Start(new ProcessStartInfo { FileName = @printFilePath + "\\" + printFileName, UseShellExecute = true });
                    }
                }
                else if (FileName == "PatientChart")
                {
                    Random r = new Random();
                    int Filecount = r.Next(1, 999);

                    printFileName = String.Format("{0}Preview.pdf", FileName);
                    string fullPath = Path.Combine(printFilePath, printFileName);

                    if (!FileIsOpen(fullPath))
                    {
                        File.WriteAllBytes(fullPath, PdfFile);
                        ConsultingFileName = printFileName;
                        ConsultingFilePath = printFilePath;
                        //Process.Start(new ProcessStartInfo { FileName = fullPath, UseShellExecute = true });
                    }
                    else
                    {
                        printFileName = String.Format("{0}Preview{1}.pdf", FileName, Filecount);
                        fullPath = Path.Combine(printFilePath, printFileName);
                        File.WriteAllBytes(fullPath, PdfFile);
                        ConsultingFileName = printFileName;
                        ConsultingFilePath = printFilePath;
                    }
                }
                else
                {
                    if (sfDlg.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(sfDlg.FileName, PdfFile);
                        wasFileSaved = true;

                        if (MessageBox.Show(
                                "Do you want to open the file?",
                                "Confirmation",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(new ProcessStartInfo { FileName = @sfDlg.FileName, UseShellExecute = true });
                        }
                    }
                }
            }
            finally
            {
                sfDlg.Dispose();
            }
        }

        private bool FileIsOpen(string file)
        {
            bool retVal = false;
            try
            {
                
                using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    try
                    {
                        stream.ReadByte();
                    }
                    catch (IOException)
                    {
                        retVal = true;
                       
                    }
                    finally
                    {
                        stream.Close();
                        stream.Dispose();
                    }

                }

            }
            catch (IOException e)
            {
               
                retVal = true;
            }
            catch (UnauthorizedAccessException e)
            {
               
            }
            return retVal;
        }

        //barcode
        public static bool SaveMemoryStreamBarcode(string PrinterName, MemoryStream ms, string defaultFileName, string extension, bool isPrint, PaperTypes PaperType)
        {
            bool wasFileSaved = false;
            try
            {
                SaveFileDialog sfDlg = new SaveFileDialog();
                try
                {
                    sfDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    switch (extension.ToLower())
                    {
                        case "xls":
                            sfDlg.Filter = "Microsoft Office Excel Workbook (*.xls)|*.xls";
                            break;
                        case "pdf":
                            sfDlg.Filter = "Adobe Portable Document Format (*.pdf)|*.pdf";
                            break;
                        default:
                            sfDlg.Filter = string.Format("{1} files|*.{0}", extension, extension.ToUpper());
                            break;
                    }
                        sfDlg.RestoreDirectory = true;
                                        sfDlg.FileName = defaultFileName;
                                        var windowsTempPath = Path.GetTempPath();
                        Directory.CreateDirectory(windowsTempPath + "");
                                        var printFilePath = String.Format("{0}", windowsTempPath);
                        var printFileName = String.Format("{0}.pdf", sfDlg.FileName);
                        var tempFilePath = String.Format("{0}\\{1}.pdf", Path.GetTempPath(), sfDlg.FileName);
                        byte[] bytes = ms.ToArray();
                        System.IO.File.WriteAllBytes(tempFilePath, bytes);
                   
                    if (isPrint)
                    {                    
                        try
                        {
                            PdfPrinter.printPDFInIron(PrinterName, printFilePath, printFileName);
                        }
                        catch (Exception e)
                        {
                            if (e.Message.Contains("The process cannot access the file"))
                            {
                                Random random = new Random();
                                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                                string randomFileName = new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray());
                                var printFilePathCatch = String.Format("{0}", windowsTempPath);
                                printFileName = String.Format("{0}Print{1}.pdf", sfDlg.FileName, randomFileName);
                                File.WriteAllBytes(printFilePathCatch + "\\" + printFileName, bytes);
                                PdfPrinter.printPDFInIron(PrinterName, printFilePathCatch, printFileName);
                            }
                        }
                    }
                    else if(defaultFileName == "Prescription")
                    {
                        printFileName = String.Format("{0}.pdf", sfDlg.FileName+"Preview");
                        File.WriteAllBytes(printFilePath + "\\" + printFileName, bytes);                       
                        System.Diagnostics.Process.Start(new ProcessStartInfo { FileName = @printFilePath + "\\" + printFileName, UseShellExecute = true });
                    }
                    else if (sfDlg.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(sfDlg.FileName, bytes);
                        wasFileSaved = true;
                        if (DialogResult.Yes == MessageBox.Show(
                                        "Do you want to open file?",
                                        "Confirmation",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question))
                        {
                            System.Diagnostics.Process.Start(new ProcessStartInfo { FileName = @sfDlg.FileName, UseShellExecute = true });
                        }
                    }
                }
                finally
                {
                    sfDlg.Dispose();
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
            return wasFileSaved;
        }

        static string PaperFormat = (Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate.ToString(Global.Company.DateFormat)! == Global.getCurrentFiscalYearStartDate().ToString(Global.Company.DateFormat)! && x.YearEndDate.ToString(Global.Company.DateFormat) == Global.getCurrentFiscalYearEndDate().ToString(Global.Company.DateFormat)! && x.EntryType == EntryType.SALES)!).PrintPaperFormat.Name;
        private static readonly Font FBI8B = new Font(PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black"));
        public static bool SaveMemoryStream(MemoryStream ms, string defaultFileName, string extension, bool isPrint, PaperTypes PaperType)
        {
            
            bool wasFileSaved = false;
            try
            {
                SaveFileDialog sfDlg = new SaveFileDialog();
                try
                {
                    sfDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    switch (extension.ToLower())
                    {
                        case "xls":
                            sfDlg.Filter = "Microsoft Office Excel Workbook (*.xls)|*.xls";
                            break;
                        case "pdf":
                            sfDlg.Filter = "Adobe Portable Document Format (*.pdf)|*.pdf";
                            break;
                        default:
                            sfDlg.Filter = string.Format("{1} files|*.{0}", extension, extension.ToUpper());
                            break;
                    }
                    sfDlg.RestoreDirectory = true;
                    sfDlg.FileName = defaultFileName + " " + DateTime.Now.ToString("dd-MM-yyyy").Replace("/", "-");
                    var windowsTempPath = Path.GetTempPath(); 
                    Directory.CreateDirectory(windowsTempPath + "");
                    var printFilePath = String.Format("{0}", windowsTempPath);
                    var printFileName = String.Format("{0}.pdf", sfDlg.FileName);
                    var tempFilePath = String.Format("{0}\\{1}.pdf", Path.GetTempPath(), sfDlg.FileName);
                    byte[] bytes = ms.ToArray();
                    System.IO.File.WriteAllBytes(tempFilePath, bytes);
                    try
                    {
                        File.ReadAllBytes(tempFilePath);
                        iTextSharp.text.Font blackFont = FontFactory.GetFont("Tahoma", 9, iTextSharp.text.Font.ITALIC, BaseColor.BLACK);
                        iTextSharp.text.Font blackFontBold = FontFactory.GetFont("Tahoma", 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                        if (PaperFormat == "105 MM ROLL" || PaperFormat == "80 MM ROLL")
                        {
                            blackFont = FontFactory.GetFont("Tahoma", 6, iTextSharp.text.Font.ITALIC, BaseColor.BLACK);
                            blackFontBold = FontFactory.GetFont("Tahoma", 6, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                        }                    
                        using (MemoryStream stream = new MemoryStream())
                        {
                            PdfReader reader = new PdfReader(bytes);
                            float[] A2 = { 126f, 425f, 550f, 1050f };
                            float[] A3 = { 86f, 260f, 390f, 730f };
                            float[] A4_PORTRAID = { 35f, 0f, 0f, 540f };
                            float[] A4_LANDSCAPE = { 38f, 370f, 400f, 780f };
                            float[] FooterX = (PaperType == PaperTypes.A2) ? A2 : (PaperType == PaperTypes.A3) ? A3 : (PaperType == PaperTypes.A4_PORTRAIT) ? A4_PORTRAID : (PaperType == PaperTypes.A4_LANDSCAPE) ? A4_LANDSCAPE : (PaperType == PaperTypes.A5_LANDSCAPE) ? A4_PORTRAID : null!;
                            using (PdfStamper stamper = new PdfStamper(reader, stream))
                            {
                                if (PaperFormat != "105 MM ROLL" && PaperFormat != "80 MM ROLL")
                                {
                                    int pages = reader.NumberOfPages;
                                    for (int i = 1; i <= pages; i++)
                                    {
                                        if (PaperFormat == "105 MM ROLL" || PaperFormat == "80 MM ROLL")
                                        {
                                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                            @Element.ALIGN_LEFT, new Phrase("Printed On: " + DateTime.Now.Date.ToString(Global.Company.DateFormat), FBI8B), 10f, 10f, 0);

                                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                             @Element.ALIGN_LEFT, new Phrase("Thank you for your business! ", FBI8B), 120f, 10f, 0);
                                        }
                                        else
                                        {
                                            if (FooterX != null)
                                            {
                                                ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                                 @Element.ALIGN_LEFT, new Phrase("Printed On: " + DateTime.Now.ToString(Global.Company.DateFormat), FBI8B), FooterX[0], 20f, 0);

                                                ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                                @Element.ALIGN_LEFT, new Phrase(""/*" Make all checks payable to " + Global.Company.Name*/, FBI8B), FooterX[2], 20f, 0);

                                                ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                                 @Element.ALIGN_MIDDLE, new Phrase(i.ToString() + " of " + pages, FBI8B), FooterX[3], 20f, 0);
                                            }
                                        }
                                    }
                                }
                            }
                            bytes = stream.ToArray();
                        }
                        ms.Close();
                    }
                    catch (DocumentException exe)
                    {
                        MessageBox.Show("There has been an error generating the file. Please try again. Error: " + exe);
                    }
                    if (isPrint)
                    {                    
                        try
                        {
                            File.WriteAllBytes(printFilePath + "\\" + printFileName, bytes);
                            PdfPrinter.printPDFInIron(Global.getDefaultPrinter(), printFilePath, printFileName);                           
                        }
                        catch (Exception e)
                        {
                            if (e.Message.Contains("The process cannot access the file"))
                            {
                                Random random = new Random();
                                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                                string randomFileName = new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray());
                                var printFilePathCatch = String.Format("{0}", windowsTempPath);
                                printFileName = String.Format("{0}Print{1}.pdf", sfDlg.FileName, randomFileName);
                                File.WriteAllBytes(printFilePathCatch + "\\" + printFileName, bytes);
                                 PdfPrinter.printPDFInIron(Global.getDefaultPrinter(), printFilePathCatch, printFileName);
                            }
                        }
                    }
                    else if (sfDlg.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(sfDlg.FileName, bytes);
                        wasFileSaved = true;
                        if (DialogResult.Yes == MessageBox.Show(
                                        "Do you want to open file?",
                                        "Confirmation",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question))
                        {
                            Process.Start(new ProcessStartInfo { FileName = @sfDlg.FileName, UseShellExecute = true });
                        }
                    }
                }
                finally
                {
                    sfDlg.Dispose();
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
            return wasFileSaved;
        }
        // ItemReport
        public static bool SaveMemoryStreams(MemoryStream ms, string defaultFileName, string userPrinter, string extension, bool isPrint, PaperTypes PaperType)
        {
            bool wasFileSaved = false;
            try
            {
                SaveFileDialog sfDlg = new SaveFileDialog();
                try
                {
                    sfDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    switch (extension.ToLower())
                    {
                        case "xls":
                            sfDlg.Filter = "Microsoft Office Excel Workbook (*.xls)|*.xls";
                            break;
                        case "pdf":
                            sfDlg.Filter = "Adobe Portable Document Format (*.pdf)|*.pdf";
                            break;
                        default:
                            sfDlg.Filter = string.Format("{1} files|*.{0}", extension, extension.ToUpper());
                            break;
                    }
                    sfDlg.RestoreDirectory = true;
                    sfDlg.FileName = defaultFileName + " " + DateTime.Now.ToString("dd-MM-yyyy").Replace("/", "-");
                    var windowsTempPath = Path.GetTempPath();
                    Directory.CreateDirectory(windowsTempPath + "");
                    var printFilePath = String.Format("{0}", windowsTempPath);
                    var printFileName = String.Format("{0}.pdf", sfDlg.FileName);
                    var tempFilePath = String.Format("{0}\\{1}.pdf", Path.GetTempPath(), sfDlg.FileName);
                    byte[] bytes = ms.ToArray();
                    System.IO.File.WriteAllBytes(tempFilePath, bytes);
                    try
                    {
                        File.ReadAllBytes(tempFilePath);                       
                        using (MemoryStream stream = new MemoryStream())
                        {
                            PdfReader reader = new PdfReader(bytes);
                            float[] A2 = { 126f, 425f, 550f, 1050f };
                            float[] A3 = { 86f, 260f, 390f, 730f };
                            float[] A4_PORTRAID = { 35f, 270f, 295f, 530f };
                            float[] A4_LANDSCAPE = { 38f, 370f, 400f, 780f };
                            float[] FooterX = (PaperType == PaperTypes.A2) ? A2 : (PaperType == PaperTypes.A3) ? A3 : (PaperType == PaperTypes.A4_PORTRAIT) ? A4_PORTRAID : (PaperType == PaperTypes.A4_LANDSCAPE) ? A4_LANDSCAPE : (PaperType == PaperTypes.A5_LANDSCAPE) ? A4_PORTRAID : A4_PORTRAID;
                            using (PdfStamper stamper = new PdfStamper(reader, stream))
                            {
                                int pages = reader.NumberOfPages;
                                for (int i = 1; i <= pages; i++)
                                {
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                        @Element.ALIGN_LEFT, new Phrase("Printed On: " + DateTime.Now.ToString(Global.Company.DateFormat), FBI8B), FooterX[0], 30f, 0);

                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                    @Element.ALIGN_LEFT, new Phrase(""/*" Make all checks payable to " + Global.Company.Name*/, FBI8B), FooterX[2], 30f, 0);

                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                        @Element.ALIGN_MIDDLE, new Phrase(i.ToString() + " of " + pages, FBI8B), FooterX[3], 30f, 0);
                                }
                            }
                            bytes = stream.ToArray();
                        }
                        ms.Close();
                    }
                    catch (DocumentException exe)
                    {
                        MessageBox.Show("There has been an error generating the file. Please try again. Error: " + exe);
                    }
                    if (isPrint)
                    {
                        try
                        {
                            File.WriteAllBytes(printFilePath + "\\" + printFileName, bytes);
                            PdfPrinter.printPDFInIron(userPrinter, printFilePath, printFileName);
                        }
                        catch (Exception e)
                        {
                            if (e.Message.Contains("The process cannot access the file"))
                            {
                                Random random = new Random();
                                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                                string randomFileName = new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray());
                                var printFilePathCatch = String.Format("{0}", windowsTempPath);
                                printFileName = String.Format("{0}Print{1}.pdf", sfDlg.FileName, randomFileName);
                                File.WriteAllBytes(printFilePathCatch + "\\" + printFileName, bytes);
                                PdfPrinter.printPDFInIron(userPrinter, printFilePathCatch, printFileName);
                            }
                        }
                    }
                    else if (sfDlg.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(sfDlg.FileName, bytes);
                        wasFileSaved = true;
                        if (DialogResult.Yes == MessageBox.Show(
                                        "Do you want to open file?",
                                        "Confirmation",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question))
                        {
                            Process.Start(new ProcessStartInfo { FileName = @sfDlg.FileName, UseShellExecute = true });
                        }
                    }
                }
                finally
                {
                    sfDlg.Dispose();
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
            return wasFileSaved;
        }

        //Daybook
        public static bool SaveMemoryStreamLedger(MemoryStream ms, string defaultFileName, string extension, bool isPrint, PaperTypes PaperType, string[] PageNumberToPage)
        {
            bool wasFileSaved = false;
            try
            {
                SaveFileDialog sfDlg = new SaveFileDialog();
                try
                {
                    sfDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    switch (extension.ToLower())
                    {
                        case "xls":
                            sfDlg.Filter = "Microsoft Office Excel Workbook (*.xls)|*.xls";
                            break;
                        case "pdf":
                            sfDlg.Filter = "Adobe Portable Document Format (*.pdf)|*.pdf";
                            break;
                        default:
                            sfDlg.Filter = string.Format("{1} files|*.{0}", extension, extension.ToUpper());
                            break;
                    }
                    sfDlg.RestoreDirectory = true;
                    sfDlg.FileName = defaultFileName;
                    var windowsTempPath = Path.GetTempPath();
                    Directory.CreateDirectory(windowsTempPath + "");
                    var printFilePath = String.Format("{0}", windowsTempPath);
                    var printFileName = String.Format("{0}.pdf", sfDlg.FileName);
                    var tempFilePath = String.Format("{0}\\{1}.pdf", Path.GetTempPath(), sfDlg.FileName);
                    byte[] bytes = ms.ToArray();
                    System.IO.File.WriteAllBytes(tempFilePath, bytes);
                    try
                    {
                        // For not split by account
                        File.ReadAllBytes(tempFilePath);
                        iTextSharp.text.Font blackFont = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.ITALIC, BaseColor.BLACK);
                        iTextSharp.text.Font blackFontBold = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                        if (PaperFormat == "105mm")
                        {
                            blackFont = FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.ITALIC, BaseColor.BLACK);
                            blackFontBold = FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                        }
                        using (MemoryStream stream = new MemoryStream())
                        {
                            PdfReader reader = new PdfReader(bytes);
                            float[] A2 = { 126f, 425f, 550f, 1050f };
                            float[] A3 = { 86f, 260f, 390f, 730f };
                            float[] A4_PORTRAID = { 50f, 0f, 0f, 520f };
                            float[] A4_LANDSCAPE = { 38f, 270f, 400f, 780f };
                            float[] FooterX = (PaperType == PaperTypes.A2) ? A2 : (PaperType == PaperTypes.A3) ? A3 : (PaperType == PaperTypes.A4_PORTRAIT) ? A4_PORTRAID : (PaperType == PaperTypes.A4_LANDSCAPE) ? A4_LANDSCAPE : (PaperType == PaperTypes.A5_LANDSCAPE) ? A4_PORTRAID : null;
                            using (PdfStamper stamper = new PdfStamper(reader, stream))
                            {
                                int pages = reader.NumberOfPages;
                                for (int i = 1; i <= pages; i++)
                                {
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                         @Element.ALIGN_LEFT, new Phrase("Printed On: " + DateTime.Now.ToString(Global.Company.DateFormat) + " " + DateTime.Now.ToShortTimeString(), blackFont), 25, 10f, 0);

                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                     @Element.ALIGN_LEFT, new Phrase("", blackFontBold), 30, 10f, 0);

                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                    @Element.ALIGN_LEFT, new Phrase("", blackFont), 30, 10f, 0);

                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                        @Element.ALIGN_LEFT, new Phrase(i.ToString() + " of " + pages, blackFont), 550, 10f, 0);
                                }
                            }
                            bytes = stream.ToArray();
                        }
                        ms.Close();
                    }
                    catch (DocumentException exe)
                    {
                        MessageBox.Show("There has been an error generating the file. Please try again. Error: " + exe);
                    }
                    if (isPrint)
                    {
                        try
                        {
                            File.WriteAllBytes(printFilePath + "\\" + printFileName, bytes);
                            PdfPrinter.printPDFInIron(Global.getDefaultPrinter(), printFilePath, printFileName);
                        }
                        catch (Exception e)
                        {
                            if (e.Message.Contains("The process cannot access the file"))
                            {
                                Random random = new Random();
                                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                                string randomFileName = new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray());
                                var printFilePathCatch = String.Format("{0}", windowsTempPath);
                                printFileName = String.Format("{0}Print{1}.pdf", sfDlg.FileName, randomFileName);
                                File.WriteAllBytes(printFilePathCatch + "\\" + printFileName, bytes);
                                PdfPrinter.printPDFInIron(Global.getDefaultPrinter(), printFilePathCatch, printFileName);
                            }
                        }
                    }
                    else if (sfDlg.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(sfDlg.FileName, bytes);
                        wasFileSaved = true;
                        if (DialogResult.Yes == MessageBox.Show(
                                        "Do you want to open file?",
                                        "Confirmation",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question))
                        {
                            System.Diagnostics.Process.Start(new ProcessStartInfo { FileName = @sfDlg.FileName, UseShellExecute = true });
                        }
                    }
                }
                finally
                {
                    sfDlg.Dispose();
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
            return wasFileSaved;
        }        
    }
}
