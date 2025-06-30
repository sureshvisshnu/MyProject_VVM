using fa.api.utils;
using fa.model.OrderManagement;
using fa.views.utils.Common;
using Fa.api.OrderManagement;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rectangle = iTextSharp.text.Rectangle;

namespace fa.views.utils.Inventory
{
    public class DamageEntryPrint
    {
        public bool PrintDamageEntry(long EntryId, string FileName)
        {
            try
            {
                using (MemoryStream myMemoryStream = new MemoryStream())
                {
                    Cursor.Current = Cursors.WaitCursor;
                    var pageSize = new Rectangle(595, 421);
                    Document pdfDoc = new Document(pageSize, -30, -30, 30, 30);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                    pdfDoc.Open();
                    StockMovementDamaged Damaged = StockMovementManager.Instance.GetStockDamaged(EntryId);
                    if (Damaged != null)
                    {
                        PdfPageHeader PdfHeader = new PdfPageHeader()
                        {
                            IsMainHeader = true,
                            Islogo = true,
                            IsAddress = true,
                            IsPhone = true,
                            IsEmail = true,
                            IsWebsite = true,
                            IsLicenceInfo = true,
                            ReportLine1 = FileName,
                        };
                        PdfPTable HTable = PdfHeader.PageHeader();
                        PdfHeader = new PdfPageHeader()
                        {
                            IsMainHeader = false,
                            Islogo = true,
                            IsAddress = true,
                            IsPhone = false,
                            IsEmail = false,
                            IsWebsite = false,
                            IsLicenceInfo = false,
                            ReportLine1 = FileName,
                        };
                        PdfPTable MiniHTable = PdfHeader.PageHeader();
                        PdfPTable EntryHTable = PageEntryHTable(Damaged);
                        PdfPTable EntryDetailHTable = PageEntryDetailHTable(Damaged);
                        pdfDoc.Add(HTable);
                        pdfDoc.Add(EntryHTable);
                        pdfDoc.Add(PdfDataAlignment.DummyTable(1, 5, 10));
                        pdfDoc.Add(EntryDetailHTable);
                        PdfPTable EntryDetailTable = new PdfPTable(7);
                        float[] Widths = new float[] { 15f, 80f, 30f, 30f, 35f, 35f, 75f };
                        EntryDetailTable.SetWidths(Widths);
                        int Count = 1;
                        foreach (StockMovementDetail Detail in Damaged.StockMovementDetails)
                        {
                            EntryDetailTable.AddCell(PdfDataAlignment.CreateCell(Count.ToString(), 1));
                            EntryDetailTable.AddCell(PdfDataAlignment.CreateCell(Detail.Product.Name, 1));
                            EntryDetailTable.AddCell(PdfDataAlignment.CreateCell(Detail.Uom, 1));
                            EntryDetailTable.AddCell(PdfDataAlignment.CreateCellRightAlign(Detail.Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision)), 1));
                            EntryDetailTable.AddCell(PdfDataAlignment.CreateCell(Detail.BatchNo, 1));
                            EntryDetailTable.AddCell(PdfDataAlignment.CreateCell(string.IsNullOrEmpty(Detail.BatchNo) ? "" : Detail.ExpDate.ToString(Global.Company.DateFormat), 1));
                            EntryDetailTable.AddCell(PdfDataAlignment.CreateCell(Detail.Description, 1));
                            Count++;
                        }

                        pdfDoc.Add(EntryDetailTable);
                        pdfDoc.Close();
                        Cursor.Current = Cursors.WaitCursor;
                        PdfFooter PdfFooter = new PdfFooter();
                        PdfFooter.IsReport = true;
                        PdfFooter.IsDate = true;
                        PdfFooter.Text = string.Empty;
                        PdfFooter.IsPageNumber = true;
                        PdfFooter.PdfFile = myMemoryStream.ToArray();
                        byte[] PdfFileWithFooter = PdfFooter.GetPdfFileWithFooter();
                        myMemoryStream.Close();
                        Cursor.Current = Cursors.WaitCursor;
                        PdfGeneration PdfGeneration = new PdfGeneration();
                        PdfGeneration.IsPrint = true;
                        PdfGeneration.FileName = FileName.Replace(" ", "");
                        PdfGeneration.PdfFile = PdfFileWithFooter;
                        PdfGeneration.SavePdfFile();
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            return true;
        }

        private PdfPTable PageEntryHTable(StockMovementDamaged Damaged)
        {
            PdfPTable EntryHTable = new PdfPTable(3);
            EntryHTable.AddCell(PdfDataAlignment.CreateCellWithBackColourandBoldWhiteText("Reference No", 18));
            EntryHTable.AddCell(PdfDataAlignment.CreateCellWithBackColourandBoldWhiteText("Date", 18));
            EntryHTable.AddCell(PdfDataAlignment.CreateCellWithBackColourandBoldWhiteText("Location", 18));
            EntryHTable.AddCell(PdfDataAlignment.CreateCell(Damaged.RefNumber, 1));
            EntryHTable.AddCell(PdfDataAlignment.CreateCell(Damaged.MovementDate.Date.ToString(Global.Company.DateFormat), 1));
            EntryHTable.AddCell(PdfDataAlignment.CreateCell(Damaged.InventoryStockLocation.Name, 1));
            return EntryHTable;
        }

        private PdfPTable PageEntryDetailHTable(StockMovementDamaged Damaged)
        {
            PdfPTable EntryHTable = new PdfPTable(7);
            float[] Widths = new float[] { 15f, 80f, 30f, 30f, 35f, 35f ,75f};
            EntryHTable.SetWidths(Widths);
            EntryHTable.AddCell(PdfDataAlignment.CreateCellWithBackColourandBoldWhiteText("#", 18));
            EntryHTable.AddCell(PdfDataAlignment.CreateCellWithBackColourandBoldWhiteText("Items", 18));
            EntryHTable.AddCell(PdfDataAlignment.CreateCellWithBackColourandBoldWhiteText("UOM", 18));
            EntryHTable.AddCell(PdfDataAlignment.CreateCellWithBackColourandBoldWhiteTextRightAlign("Quantity", 18));
            EntryHTable.AddCell(PdfDataAlignment.CreateCellWithBackColourandBoldWhiteText("Batch No", 18));
            EntryHTable.AddCell(PdfDataAlignment.CreateCellWithBackColourandBoldWhiteText("Exp Date", 18));
            EntryHTable.AddCell(PdfDataAlignment.CreateCellWithBackColourandBoldWhiteText("Description", 18));
            return EntryHTable;
        }
    }
}
