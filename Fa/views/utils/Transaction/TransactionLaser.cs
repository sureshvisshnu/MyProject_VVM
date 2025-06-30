using fa.model.Accounting.Masters;
using iTextSharp.text;
using iTextSharp.text.pdf;
using K4os.Compression.LZ4.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using static fa.views.utils.PrinterSetup;

namespace fa.views.utils.Receipts
{
    public class TransactionLaser
    {
        public static void GeneratePDF(DataTable dataTable, DataTable totalTable, List<string> heading, List<string> invoiceContent, string fileName, string fileExtension, bool isPrint, string memo)
        {
            Cursor.Current = Cursors.WaitCursor;
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                var pageSize = new iTextSharp.text.Rectangle(595, 421);
                Document pdfDoc = new Document(pageSize, -30, -30, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                int cols = dataTable.Columns.Count;
                int rows = dataTable.Rows.Count;
                pdfDoc.Open();

                float a4Size = pdfDoc.PageSize.Top;
                string CompanyName = Global.Company.DisplayAs + "\n";
                string Address = Global.Company.Address.FullAddressInSingleLine;
                string Phone = string.Empty;
                string Email = string.Empty;
                string Web = string.Empty;
                string License = string.Empty;

                if (Global.Company.ContactInfo.Phone != "" && Global.Company.ContactInfo.Phone != "-")
                {
                    var fax = Global.Company.ContactInfo.Fax != "" ? "\nFax: " + Global.Company.ContactInfo.Fax : "";
                    Phone = "\nPhone: " + Global.Company.ContactInfo.Phone + fax;
                }
                if (!string.IsNullOrEmpty(Global.Company.ContactInfo.Email))
                {
                    Email = Global.Company.ContactInfo.Email == "" ? "" : "\nEmail: " + (Global.Company.ContactInfo).Email;
                }
                if (!string.IsNullOrEmpty(Global.Company.ContactInfo.WebSite))
                {
                    Web = Global.Company.ContactInfo.WebSite == "" ? "" : "\nWeb: " + (Global.Company.ContactInfo).WebSite;
                }
                if (Global.Company.CompanyLicence.Count > 0)
                {
                    foreach (CompanyLicence Licence in Global.Company.CompanyLicence)
                    {
                        if (Licence.IncludeInInvoice)
                        {
                            License = (string.IsNullOrEmpty(License) ? License : License + ", ") + (Licence.Name + ":" + Licence.Value);
                        }
                    }
                    License = License + "\n";
                }
                int HeadColumns = 3;
                float[] HeadWidths = new float[] { 50f, 30f, 20f };
                if (Global.getLogoAsBytes() != null)
                {
                    HeadColumns = 4;
                    HeadWidths = new float[] { 15f, 40f, 25f, 20f };
                }

                PdfPTable HeadTable = new PdfPTable(HeadColumns);
                PdfPCell HeadCell = new PdfPCell();
                HeadTable.SetWidths(HeadWidths);

                if (Global.getLogoAsBytes() != null)
                {
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Global.getLogoAsBytes());
                    image.ScaleToFit(200f, 20f);
                    image.ScaleAbsolute(70, 70);
                    var ImgData = image;
                    HeadCell = new PdfPCell(image);
                    HeadCell.BorderColor = BaseColor.WHITE;
                    HeadCell.MinimumHeight = 25;
                    HeadCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    HeadCell.Rowspan = 2;
                    HeadTable.AddCell(HeadCell);
                }
                var AddressData = CompanyName;
                HeadCell = new PdfPCell(new Phrase(AddressData, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                HeadCell.AddElement(new Phrase(AddressData, PdfDataAlignment.GetFont("Font_Bold_Italic_10_Black")));
                AddressData = Address + Phone + Email + Web;
                HeadCell.AddElement(new Phrase(AddressData, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                HeadCell.AddElement(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                HeadCell.AddElement(new Phrase(License, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                HeadCell.BorderColor = BaseColor.WHITE;
                HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeadCell.Rowspan = 2;
                HeadCell.MinimumHeight = 0f;
                HeadTable.AddCell(HeadCell);

                if (fileName == "Journal")
                {
                    HeadCell = new PdfPCell(new Phrase(heading[0], PdfDataAlignment.GetFont("Font_Bold_Italic_20_LightGray")));
                    HeadCell.BorderColor = BaseColor.WHITE;
                    HeadCell.Colspan = 2;
                    HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    HeadTable.AddCell(HeadCell);
                }
                else
                {
                    HeadCell = new PdfPCell(new Phrase("To", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    HeadCell.AddElement(new Phrase("To", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    HeadCell.AddElement(new Phrase(heading[1], PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    HeadCell.BorderColor = BaseColor.WHITE;
                    HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    HeadCell.Rowspan = 2;
                    HeadTable.AddCell(HeadCell);

                    HeadCell = new PdfPCell(new Phrase(heading[0], PdfDataAlignment.GetFont("Font_Bold_Italic_15_LightGray")));
                    HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    HeadCell.BorderColor = BaseColor.WHITE;
                    HeadTable.AddCell(HeadCell);

                    HeadCell = new PdfPCell(new Phrase(heading[2], PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    HeadCell.BorderColor = BaseColor.WHITE;
                    HeadTable.AddCell(HeadCell);

                    HeadCell = new PdfPCell(new Phrase(" ", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    HeadCell.BorderColor = BaseColor.WHITE;
                    HeadCell.Colspan = HeadColumns;
                    HeadCell.MinimumHeight = 0f;
                    HeadTable.AddCell(HeadCell);
                }
                pdfDoc.Add(HeadTable);
                PdfPTable invoiceTableContent = new PdfPTable((invoiceContent.Count / 2));
                float[] columnWidths = new float[(invoiceContent.Count / 2)];
                for (int i = 0; i < columnWidths.Length; i++)
                {
                    if (fileName == "Expense" || fileName == "Receipt" || fileName == "Payment")
                    {
                        if (i == 3 || i == 7) // Adjust width for cell 4 and 8
                        {
                            columnWidths[i] = 2f; // Set wider width
                        }
                        else
                        {
                            columnWidths[i] = 1f; // Set default width
                        }
                    }
                    else
                    {
                        columnWidths[i] = 1f;
                    }
                }
                invoiceTableContent.SetWidths(columnWidths);

                PdfPCell invoiceCellContent;

                for (int i = 0; i < invoiceContent.Count; i++)
                {
                    invoiceCellContent = new PdfPCell(new Phrase(invoiceContent[i].Trim(), PdfDataAlignment.GetFont("Font_Bold_Italic_9_White")));
                    invoiceCellContent.BorderColor = BaseColor.LIGHT_GRAY;
                    invoiceCellContent.BackgroundColor = new BaseColor(160, 160, 160);
                    invoiceCellContent.MinimumHeight = 14;
                    invoiceCellContent.Padding = 5;
                    invoiceCellContent.HorizontalAlignment = Element.ALIGN_LEFT;

                    if (i >= (invoiceContent.Count / 2))
                    {
                        invoiceCellContent = new PdfPCell(new Phrase(invoiceContent[i].Trim(), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                        invoiceCellContent.MinimumHeight = 14;
                        invoiceCellContent.BorderColor = BaseColor.LIGHT_GRAY;
                        invoiceCellContent.HorizontalAlignment = Element.ALIGN_LEFT;
                    }

                    invoiceTableContent.AddCell(invoiceCellContent);
                }

                pdfDoc.Add(invoiceTableContent);

                if (fileName == "DebitNote" || fileName == "CreditNote" || fileName == "Invoice" || fileName == "Receipt")
                {
                    if (heading[3] != string.Empty)
                    {
                        float[] MemoHeadWidths = new float[] { 100f };
                        int MemoHeadColumns = 1;
                        PdfPTable MemoContent = new PdfPTable(MemoHeadColumns);
                        PdfPCell MemoCell = new PdfPCell(new Phrase(heading[3], PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                        MemoContent.SetWidths(MemoHeadWidths);
                        MemoCell.MinimumHeight = 14;
                        MemoCell.BorderColor = BaseColor.LIGHT_GRAY;
                        MemoCell.BorderWidthTop = 0f;
                        MemoContent.AddCell(MemoCell);
                        pdfDoc.Add(MemoContent);
                    }
                }
                if (fileName == "Journal" || fileName == "Expense" || fileName == "Bill" || fileName == "Payment")
                {
                    if (!string.IsNullOrEmpty(memo))
                    {
                        float[] MemoHeadWidths = new float[] { 100f };
                        int MemoHeadColumns = 1;
                        PdfPTable MemoContent = new PdfPTable(MemoHeadColumns);
                        PdfPCell MemoCell = new PdfPCell(new Phrase(memo, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                        MemoContent.SetWidths(MemoHeadWidths);
                        MemoCell.MinimumHeight = 14;
                        MemoCell.BorderColor = BaseColor.LIGHT_GRAY;
                        MemoCell.BorderWidthTop = 0f;
                        MemoContent.AddCell(MemoCell);
                        pdfDoc.Add(MemoContent);
                    }
                }
                //dummy space
                PdfPTable DummyTableContent = new PdfPTable(1);
                PdfPCell DummyCell = new PdfPCell(new Phrase(" "));
                DummyCell.MinimumHeight = 14;
                DummyCell.BorderColor = BaseColor.WHITE;
                DummyCell.BorderWidthTop = 0f;
                DummyTableContent.AddCell(DummyCell);
                pdfDoc.Add(DummyTableContent);

                PdfPTable table = new PdfPTable(cols);
                PdfPCell headerCell = new PdfPCell();
                float[] widths = new float[] { 15f, 200f, 50f };
                if (fileName == "Invoice")
                {
                    widths = new float[] { 10f, 100f, 25f, 20f, 30f };
                }
                else if (fileName == "Journal")
                {
                    widths = new float[] { 10f, 35f, 64f, 35f, 23f, 23f };
                }
                else if (fileName == "DebitNote" || fileName == "CreditNote")
                {
                    widths = new float[] { 10f, 40f, 120f, 20f };
                }
                table.SetWidths(widths);
                //Item Table Header
                foreach (DataColumn column in dataTable.Columns)
                {
                    headerCell = new PdfPCell(new Phrase(column.Caption, PdfDataAlignment.GetFont("Font_Bold_Italic_9_White")));
                    headerCell.BackgroundColor = new BaseColor(160, 160, 160);
                    headerCell.BorderColor = BaseColor.LIGHT_GRAY;
                    headerCell.MinimumHeight = 16;
                    headerCell.Padding = 4;
                    headerCell.Rowspan = 2;
                    if (column.Caption == "Description" || column.Caption == "RefName" || column.Caption == "Account" || column.Caption == "#")
                    {
                        headerCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    }
                    else
                    {
                        headerCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    }
                    table.AddCell(headerCell);
                }
                //Item Table Body
                bool isafterGrandTotal = false;
                for (int i = 0; i < rows; i++)
                {
                    PdfPCell rowCell = new PdfPCell();
                    for (int j = 0; j < cols; j++)
                    {
                        var temp = dataTable.Rows[i][j].ToString();
                        rowCell = new PdfPCell(new Phrase(temp, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                        rowCell.BorderColor = BaseColor.LIGHT_GRAY;
                        rowCell.UseVariableBorders = true;
                        if (j == 0 || j == 1 || (fileName == "Journal" && (j == 2 || j == 3))
                            || ((fileName == "DebitNote" || fileName == "CreditNote") && j == 2))
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        //Row Odd Even Color
                        if (i % 2 != 0)
                        {
                            rowCell.BackgroundColor = new BaseColor(242, 242, 242);
                        }
                        else
                        {
                            rowCell.BackgroundColor = BaseColor.WHITE;
                        }
                        if (fileName == "Invoice")
                        {
                            rowCell.BorderWidthBottom = (float)BorderStyle.None;
                            if (j == 1 && (temp == "Grand Total" || temp == "Net Amount" || isafterGrandTotal))
                            {
                                if (temp == "Grand Total")
                                {
                                    isafterGrandTotal = true;
                                }
                                rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                rowCell.BorderWidthLeft = (float)BorderStyle.None;
                            }
                            if (i == rows - 1)
                            {
                                rowCell.BorderWidthBottom = 0.75f;
                            }
                        }
                        table.AddCell(rowCell);
                    }
                }
                pdfDoc.Add(table);

                int colstotalTable = totalTable.Columns.Count;
                int rowstotalTable = totalTable.Rows.Count;
                if (totalTable.Columns.Count > 0)
                {
                    PdfPTable totalPdfTable = new PdfPTable(colstotalTable);
                    if (fileName == "Invoice")
                    {
                        widths = new float[] { 55f, 101f, 23.5f, 34.5f };
                    }
                    else if (fileName == "DebitNote" || fileName == "CreditNote")
                    {
                        widths = new float[] { 150f, 20f, 20f };
                    }
                    else if (fileName == "Journal")
                    {
                        widths = new float[] { 144f, 23f, 23f };
                    }
                    totalPdfTable.SetWidths(widths);
                    totalPdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                    //Total Table
                    if (fileName == "Invoice")
                    {
                        for (int i = 0; i < rowstotalTable; i++)
                        {
                            PdfPCell rowCell = new PdfPCell();
                            for (int j = 0; j < colstotalTable; j++)
                            {
                                var temp = totalTable.Rows[i][j].ToString();
                                rowCell = new PdfPCell(new Phrase(temp, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                                rowCell.BorderColor = BaseColor.GRAY;
                                rowCell.MinimumHeight = 14;
                                rowCell.Padding = 2;
                                if (j == 0 || j == 1)
                                {
                                    rowCell.BorderColor = BaseColor.WHITE;
                                    rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                }
                                else if (i >= 1 && j >= 3)
                                {
                                    rowCell.BorderColor = BaseColor.GRAY;
                                    rowCell.BackgroundColor = BaseColor.GRAY;
                                }
                                else
                                {
                                    rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    rowCell.BorderColor = BaseColor.GRAY;
                                }
                                if (j == 2 || j == 3)
                                {
                                    rowCell.UseVariableBorders = true;
                                    rowCell.BorderColorTop = BaseColor.LIGHT_GRAY;
                                    rowCell.BorderColorRight = BaseColor.LIGHT_GRAY;
                                    rowCell.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                    rowCell.BorderColorBottom = BaseColor.LIGHT_GRAY;
                                    if (j != 2)
                                    {
                                        rowCell.BorderWidthLeft = 0f;
                                    }
                                    rowCell.BorderWidthBottom = 0f;
                                }
                                else if (j == 1 || j == 0)
                                {
                                    rowCell.UseVariableBorders = true;
                                    rowCell.BorderColorTop = BaseColor.LIGHT_GRAY;
                                }
                                totalPdfTable.AddCell(rowCell);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < rowstotalTable; i++)
                        {
                            PdfPCell rowCell = new PdfPCell();
                            for (int j = 0; j < colstotalTable; j++)
                            {
                                var temp = totalTable.Rows[i][j].ToString();
                                rowCell = new PdfPCell(new Phrase(temp, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                                rowCell.MinimumHeight = 14;
                                rowCell.Padding = 2;
                                if (fileName == "DebitNote" || fileName == "CreditNote" || fileName == "Journal")
                                {
                                    rowCell.Colspan = 2;
                                }
                                rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                rowCell.BorderColor = BaseColor.LIGHT_GRAY;
                                totalPdfTable.AddCell(rowCell);
                            }
                        }
                    }
                    pdfDoc.Add(totalPdfTable);

                    //dummy line
                    DummyTableContent = new PdfPTable(colstotalTable);
                    DummyTableContent.SetWidths(widths);
                    for (int j = 0; j < colstotalTable; j++)
                    {
                        DummyCell = new PdfPCell(new Phrase(""));
                        DummyCell.UseVariableBorders = true;
                        DummyCell.BorderColorLeft = BaseColor.WHITE;
                        DummyCell.BorderColorRight = BaseColor.WHITE;
                        DummyCell.BorderColorBottom = BaseColor.WHITE;
                        DummyCell.BorderColorTop = BaseColor.LIGHT_GRAY;
                        if ((j == 0 || j == 1) && fileName == "Invoice")
                        {
                            DummyCell.BorderColorTop = BaseColor.WHITE;
                        }
                        DummyCell.MinimumHeight = 5;
                        DummyTableContent.AddCell(DummyCell);
                    }
                    pdfDoc.Add(DummyTableContent);
                }

                //signature
                PdfPTable BottomTable = new PdfPTable(2);
                PdfPCell BottomCell = new PdfPCell();

                float[] BottomWidths = new float[] { 50f, 50f };
                BottomTable.SetWidths(BottomWidths);
                for (int l = 0; l < 6; l++)
                {
                    BottomCell = new PdfPCell(new Phrase("           ", PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
                    if (fileName == "Payment" || fileName == "Expense")
                    {
                        if (l == 4)
                        {
                            BottomCell = new PdfPCell(new Phrase("Authorized Signatory \n" + "For " + Global.Company.Name, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
                            BottomCell.HorizontalAlignment = Element.ALIGN_LEFT;

                        }
                        if (l == 5)
                        {
                            BottomCell = new PdfPCell(new Phrase("Receiver's Signature", PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
                            BottomCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                    }
                    else
                    {
                        if (l == 4)
                        {
                            BottomCell = new PdfPCell(new Phrase("Authorized Signatory \n" + "For " + Global.Company.Name, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
                            BottomCell.HorizontalAlignment = Element.ALIGN_LEFT;

                        }
                        if (l == 5)
                        {
                            BottomCell = new PdfPCell(new Phrase(" ", PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
                            BottomCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                    }

                    BottomCell.BorderColor = BaseColor.WHITE;
                    BottomTable.AddCell(BottomCell);
                }

                pdfDoc.Add(BottomTable);
                pdfDoc.Close();
                PdfGeneration.SaveMemoryStream(myMemoryStream, fileName, fileExtension, isPrint, PaperTypes.A5_LANDSCAPE);
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
