using fa.api.catalog;
using fa.model.Catalog;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.views.utils.Catalog
{
    public class PrintProductDetail
    {
        public static void ProductsDetailsPrinting(long PId,int Qty,string PrinterName)
        {
            Product Product = CatalogProductManager.Instance.GetProductInfoById(PId);
            if (Product != null)
            {
                using (MemoryStream myMemoryStream = new MemoryStream())
                {
                    
                    var pgSize = new iTextSharp.text.Rectangle(150, 300);

                    Document pdfDoc = new Document(pgSize, -10, -10, 0, 0);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                    pdfDoc.Open();

                    PdfPTable Table = new PdfPTable(2);
                    float[] widths = new float[] { 25f, 50f};
                    Table.SetTotalWidth(widths);
                    PdfPCell TableCell = new PdfPCell();
                    
                    TableCell = new PdfPCell(new Phrase(Global.Company.Name, PdfDataAlignment.GetFont("Font_Bold_Italic_12_Black")));
                    TableCell.UseVariableBorders = true;
                    TableCell.BorderColorLeft = BaseColor.WHITE;
                    TableCell.BorderColorTop = BaseColor.WHITE;
                    TableCell.BorderColorRight = BaseColor.WHITE;
                    TableCell.BorderColorBottom = BaseColor.WHITE;
                    TableCell.Padding = 4;
                    TableCell.Colspan = 2;
                    TableCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    Table.AddCell(TableCell);

                    if (Global.Company.CompanyLicence.Count != 0)
                    {
                        Table.AddCell(CreateColSpanCell(Global.Company.CompanyLicence.First().DisplayName + " :" + Global.Company.CompanyLicence.First().Value, PdfDataAlignment.GetFont("Font_Bold_Italic_10_Black")));
                    }

                    Table.AddCell(CreateColSpanCell(Product.Name, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    Table.AddCell(CreateColSpanCell("(Best Before 3 Months)", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));

                    Table.AddCell(CreateCell("PKD", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), true, 1));
                    Table.AddCell(CreateCell(": "+DateTime.Now.ToLongDateString(), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), true, 1));

                    Table.AddCell(CreateCell("Price", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), true, 1));
                    Table.AddCell(CreateCell(": " + ((int)Global.Company.BusinessType == 1?Product.RetailPrice: Product.WholdSalePrice).ToString(), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), true, 1));

                    Table.AddCell(CreateCell("Code", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), true, 1));
                    Table.AddCell(CreateCell(": " + Product.MaterialId, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), true, 1));

                    Table.AddCell(CreateCell("Batch", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), true, 1));
                    Table.AddCell(CreateCell(": " + Product.MaterialId+@"\"+ DateTime.Now.Month + @"\"+ DateTime.Now.Day, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), true, 1));


                    Table.AddCell(CreateCell("MRP", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), true, 1));
                    Table.AddCell(CreateCell(": " + Product.Msrp, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), true, 1));

                 
                    Table.AddCell(CreateColSpanCell("Wish you all the best", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    pdfDoc.Add(Table);
                    pdfDoc.Close();
                    for (int i = 0; i < Qty; i++)
                    {
                        SaveMemoryStream(myMemoryStream, "PrintTocken"+i,PrinterName);
                    }
                }
            }
        }
        
        public static PdfPCell CreateCell(string Txt, iTextSharp.text.Font font, bool left, int cols)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(Txt, font));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.WHITE;
            rowCell.BorderColorTop = BaseColor.WHITE;
            rowCell.BorderColorRight = BaseColor.WHITE;
            rowCell.BorderColorBottom = BaseColor.WHITE;
            rowCell.Padding = 4;
            rowCell.Colspan = cols;
            rowCell.HorizontalAlignment = left ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT;
            return rowCell;
        }
        public static PdfPCell CreateColSpanCell(string Txt, iTextSharp.text.Font font)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(Txt, font));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.WHITE;
            rowCell.BorderColorTop = BaseColor.WHITE;
            rowCell.BorderColorRight = BaseColor.WHITE;
            rowCell.BorderColorBottom = BaseColor.WHITE;
            rowCell.Padding = 4;
            rowCell.Colspan = 2;
            rowCell.HorizontalAlignment = Element.ALIGN_CENTER;
            return rowCell;
        }
        public static bool SaveMemoryStream(MemoryStream ms, string filename,string PrinterName)
        {
            bool wasFileSaved = false;
            try
            {
                SaveFileDialog sfDlg = new SaveFileDialog();
                try
                {
                    MyPrinter.SetDefaultPrinter(PrinterName);

                    sfDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    sfDlg.RestoreDirectory = true;
                    sfDlg.FileName = filename;
                    var windowsTempPath = Path.GetTempPath();
                    Directory.CreateDirectory(windowsTempPath + "");
                    var printFilePath = String.Format("{0}", windowsTempPath);
                    var printFileName = String.Format("{0}.pdf", sfDlg.FileName);
                    var tempFilePath = String.Format("{0}\\{1}.pdf", Path.GetTempPath(), sfDlg.FileName);
                    byte[] bytes = ms.ToArray();
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
                            PdfPrinter.printPDFInIron(Global.getDefaultPrinter(), printFilePath , printFileName);
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
                MessageBox.Show(exc.Message, "Aborted", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return wasFileSaved;
        }
       
    }
}
