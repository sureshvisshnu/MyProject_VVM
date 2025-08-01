//using BarcodeLib;
//using DocumentFormat.OpenXml.Bibliography;
//using DocumentFormat.OpenXml.Drawing;
using fa.api.catalog;
using fa.api.Hms;
using fa.api.OrderManagement;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.Catalog;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using fa.model.OrderManagement;
using fa.views.purchase;
using FADataAccessLibrary.Api.catalog;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
//using Pango;
using RawPrint;
using RawPrint.NetStd;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using VisioForge.Libs.MediaFoundation.OPM;
using Rectangle = iTextSharp.text.Rectangle;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace fa.views.utils
{

    public enum LabelSize
    {
        THREE, TWO, ONE
    }
    public class SavePrintBarcode
    {
        public string? PrinterName;

        // Font size adjustments (add these constants at the top of your class)
        private const int COMPANY_FONT_SIZE = 11; // Reduced from 12
        private const int MRP_UOM_FONT_SIZE = 7;  // Reduced from 8
        private const int SECRET_CODE_FONT_SIZE = 10; // Reduced from 12 (assuming original was same as 

        //a4 sheet barcode by item
        public void GenerateBarcodeA4(long ProductId, string fileName, string fileExtension, bool isPrint, string Location, int Qty, string PrinterName)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, -60, -60, 35, 25);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();

                PdfPTable ReportMainTable = MainTable(ProductId, Location, Qty);

                pdfDoc.Add(ReportMainTable);
                pdfDoc.Close();
                PdfGeneration.SaveMemoryStreamBarcode(PrinterName, myMemoryStream, fileName, fileExtension, isPrint, PaperTypes.A4_PORTRAIT);
            }
        }

        private PdfPTable MainTable(long ProductId, string Location, int Qty)
        {
            string[] StartCell = Location.Split(',');
            int PrintStartCellCount = (((int.Parse(StartCell[0]) - 1) * 3) + int.Parse(StartCell[1]));
            int PrintStartCell = (PrintStartCellCount == 0) ? 0 : (PrintStartCellCount - 1);
            int UnCompletedRowCell = (PrintStartCell % 3);
            int Cols = 3;

            PdfPTable ReportMainTable = new PdfPTable(Cols);

            float[] widths = null!;

            widths = new float[] { 280f, 270f, 260f };
            ReportMainTable.SetWidths(widths);

            int Rows = 0;
            int AddColumn = 0;
            int i = 0;
            int jCell = 0;
            int TotalQty = PrintStartCell;
            for (int P = 0; P < PrintStartCell; P++)
            {
                PdfPCell RowCell = new PdfPCell();
                RowCell.MinimumHeight = 97;
                RowCell.BorderColor = BaseColor.WHITE;
                ReportMainTable.AddCell(RowCell);
            }

            Product ProductFromDB = CatalogProductManager.Instance.GetProductInfoById(ProductId);

            int Quantity = Qty;
            TotalQty = TotalQty + Quantity;
            Rows += ((Quantity / 3) + ((Quantity % 3) != 0 ? 1 : 0)) + (UnCompletedRowCell != 0 ? 1 : 0);
            if (AddColumn != 0)
            {
                jCell = AddColumn;
                i = i - 1;
            }
            AddColumn = (Quantity % 3);

            if (UnCompletedRowCell != 0)
            {
                jCell = UnCompletedRowCell;
                if (AddColumn != 0 && ((3 - jCell) - AddColumn) > -1)
                {
                    Rows -= 1;
                }
            }
            if (jCell != 0)
            {
                int TempAddColumn = (3 - jCell);
                if (TempAddColumn <= AddColumn)
                {
                    if (TempAddColumn == AddColumn && UnCompletedRowCell == 0)
                    {
                        Rows -= 1;
                    }
                    AddColumn -= TempAddColumn;
                }
                else
                {
                    if (AddColumn != 0 && UnCompletedRowCell == 0)
                    {
                        Rows -= 1;
                    }
                    AddColumn = (3 - (TempAddColumn - AddColumn));
                }
            }
            UnCompletedRowCell = 0;
            MemoryStream Barcode = new MemoryStream();
            var Image = BarCode.GenerateImageBarcode1(ProductFromDB.MaterialId);
            Image.Save(Barcode, System.Drawing.Imaging.ImageFormat.Png);
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Barcode.ToArray());
            image.ScaleAbsoluteHeight(15);
            image.ScaleAbsoluteWidth(130);
            var Temp = image;
            int alength = Global.Company.Name.Length;
            int plength = ProductFromDB.Name.Length;

            var AboveBarcodeName = " " + Global.Company.Name.Substring(0, (alength <= 25) ? alength : 25) + ((alength > 25) ? ".." : "") + "\n" + " " + ProductFromDB.Name.Substring(0, (plength <= 25) ? plength : 25) + ((plength > 25) ? ".." : "");
            var BelowBarcodeName = " " + ProductFromDB.MaterialId;
            var price = "MRP: ₹" + ProductFromDB.Msrp.ToString(Global.Company.PrimaryCurrency.CurrencyFormat) +
               "     Rate: ₹" + (Global.Company.BusinessType == BuisnessType.Wholesale ? ProductFromDB.WholdSalePrice.ToString(Global.Company.PrimaryCurrency.CurrencyFormat)
               : ProductFromDB.RetailPrice.ToString(Global.Company.PrimaryCurrency.CurrencyFormat));
            var Description = string.Empty;

            string Space = "";
            int blength = 12 + (13 - BelowBarcodeName.Length);
            for (int s = 0; s < blength; s++)
            {
                Space = Space + " ";
            }
            BelowBarcodeName = Space + BelowBarcodeName;

            for (i = i; i < Rows; i++)
            {
                PdfPCell RowCell = new PdfPCell();

                for (int j = jCell; j < Cols; j++)
                {
                    if (AddColumn != 0 && AddColumn <= j && i == (Rows - 1))
                    {
                        continue;
                    }
                    RowCell = new PdfPCell(new Phrase(AboveBarcodeName, PdfDataAlignment.GetFont("Font_Normal_Italic_10_Black")));
                    RowCell.MinimumHeight = 90;
                    RowCell.PaddingTop = -3;
                    RowCell.UseVariableBorders = true;
                    RowCell.BorderColor = BaseColor.WHITE;
                    RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    RowCell.AddElement(new Phrase(AboveBarcodeName, PdfDataAlignment.GetFont("Font_Normal_Italic_10_Black")));

                    RowCell.PaddingTop = -3;
                    RowCell.UseVariableBorders = true;
                    RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    RowCell.AddElement(Temp);

                    RowCell.PaddingTop = -4;
                    RowCell.UseVariableBorders = true;
                    RowCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    RowCell.VerticalAlignment = Element.ALIGN_TOP;
                    RowCell.AddElement(new Phrase(BelowBarcodeName, PdfDataAlignment.GetFont("Font_Normal_Italic_10_Black")));

                    RowCell.PaddingTop = -3;
                    RowCell.UseVariableBorders = true;
                    RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    RowCell.AddElement(new Phrase(price, PdfDataAlignment.GetFont("Font_Normal_Italic_10_Black")));
                    ReportMainTable.AddCell(RowCell);
                }
                jCell = 0;

                if (i != (Rows - 1))
                {
                    for (int k = 0; k < 3; k++)
                    {
                        RowCell = new PdfPCell();
                        RowCell.MinimumHeight = 7;
                        RowCell.BorderColor = BaseColor.WHITE;
                        ReportMainTable.AddCell(RowCell);
                    }
                }
            }
            AddColumn = TotalQty % 3;
            if (AddColumn > 0)
            {
                for (i = 0; i < (3 - AddColumn); i++)
                {
                    PdfPCell RowCell = new PdfPCell();
                    RowCell.BorderColor = BaseColor.WHITE;
                    ReportMainTable.AddCell(RowCell);
                }
            }
            return ReportMainTable;
        }

        //generate barcode a4 for patient
        public void GenerateBarcodeA4ForPatient(long PatientId, string fileName, string fileExtension, bool isPrint, string Location, int Qty, string PrinterName)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, -63, -60, 35, 25);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();
                PdfPTable ReportMainTable = MainTablePatient(PatientId, Location, Qty);
                pdfDoc.Add(ReportMainTable);
                pdfDoc.Close();
                PdfGeneration.SaveMemoryStreamBarcode(PrinterName, myMemoryStream, fileName, fileExtension, isPrint, PaperTypes.A4_PORTRAIT);
            }
        }
        private PdfPTable MainTablePatient(long PatientId, string Location, int Qty)
        {
            string[] StartCell = Location.Split(',');
            int PrintStartCellCount = (((int.Parse(StartCell[0]) - 1) * 3) + int.Parse(StartCell[1]));
            int PrintStartCell = (PrintStartCellCount == 0) ? 0 : (PrintStartCellCount - 1);
            int UnCompletedRowCell = (PrintStartCell % 3);
            int Cols = 3;
            PdfPTable ReportMainTable = new PdfPTable(Cols);
            float[] widths = null;
            widths = new float[] { 260f, 260f, 260f };
            ReportMainTable.SetWidths(widths);
            int Rows = 0;
            int AddColumn = 0;
            int i = 0;
            int jCell = 0;
            int TotalQty = PrintStartCell;
            for (int P = 0; P < PrintStartCell; P++)
            {
                PdfPCell RowCell = new PdfPCell();
                RowCell.MinimumHeight = 107;
                RowCell.BorderColor = BaseColor.WHITE;
                ReportMainTable.AddCell(RowCell);
            }
            Patient PatientFromDB = PatientManager.Instance.GetPatientById(PatientId);
            int Quantity = Qty;
            TotalQty = TotalQty + Quantity;
            Rows += ((Quantity / 3) + ((Quantity % 3) != 0 ? 1 : 0)) + (UnCompletedRowCell != 0 ? 1 : 0);
            if (AddColumn != 0)
            {
                jCell = AddColumn;
                i = i - 1;
            }
            AddColumn = (Quantity % 3);
            if (UnCompletedRowCell != 0)
            {
                jCell = UnCompletedRowCell;
                if (AddColumn != 0 && ((3 - jCell) - AddColumn) > -1)
                {
                    Rows -= 1;
                }
            }
            if (jCell != 0)
            {
                int TempAddColumn = (3 - jCell);
                if (TempAddColumn <= AddColumn)
                {
                    if (TempAddColumn == AddColumn && UnCompletedRowCell == 0)
                    {
                        Rows -= 1;
                    }
                    AddColumn -= TempAddColumn;
                }
                else
                {
                    if (AddColumn != 0 && UnCompletedRowCell == 0)
                    {
                        Rows -= 1;
                    }
                    AddColumn = (3 - (TempAddColumn - AddColumn));
                }
            }
            UnCompletedRowCell = 0;
            MemoryStream Barcode = new MemoryStream();
            var Image = BarCode.GenerateImageBarcode1(PatientFromDB.PatientNumber);
            Image.Save(Barcode, System.Drawing.Imaging.ImageFormat.Png);
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Barcode.ToArray());
            image.ScaleAbsoluteHeight(15);
            image.ScaleAbsoluteWidth(130);
            var Temp = image;
            int alength = PatientFromDB.Address != null ? PatientFromDB.Address.FullAddressInSingleLine.Length : 0;
            int plength = PatientFromDB.Name.Length;
            var AboveBarcodeName = "Name :" + PatientFromDB.Name.Substring(0, (plength <= 25) ? plength : 25) + ((plength > 25) ? ".." : "") + "\n"
                + "PNO  :" + PatientFromDB.PatientNumber + "    Sex  :" + PatientFromDB.Gender.ToString() + "\n"
                + "DOB  :" + PatientFromDB.DateOfBirth.ToString(Global.Company.DateFormat) + "    Date :" + Global.getTransactionDate().ToString(Global.Company.DateFormat) + "\n"
                + (PatientFromDB.Address != null ? ("Address:" + PatientFromDB.Address.FullAddressInSingleLine.Substring(0, (alength <= 70) ? alength : 70) + ((alength > 70) ? ".." : "")) : "");

            for (i = i; i < Rows; i++)
            {
                PdfPCell RowCell = new PdfPCell();

                for (int j = jCell; j < Cols; j++)
                {
                    if (AddColumn != 0 && AddColumn <= j && i == (Rows - 1))
                    {
                        continue;
                    }
                    RowCell = new PdfPCell(new Phrase(AboveBarcodeName, PdfDataAlignment.GetFont("Font_Normal_Italic_10_Black")));
                    RowCell.MinimumHeight = 107;
                    RowCell.PaddingTop = -2;
                    RowCell.UseVariableBorders = true;
                    RowCell.BorderColor = BaseColor.WHITE;
                    RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    RowCell.AddElement(new Phrase(AboveBarcodeName, PdfDataAlignment.GetFont("Font_Normal_Italic_10_Black")));

                    RowCell.PaddingTop = -3;
                    RowCell.UseVariableBorders = true;
                    RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    RowCell.AddElement(Temp);
                    ReportMainTable.AddCell(RowCell);
                }
                jCell = 0;

                if (i != (Rows - 1))
                {
                    for (int k = 0; k < 3; k++)
                    {
                        RowCell = new PdfPCell();
                        RowCell.MinimumHeight = 7;
                        RowCell.BorderColor = BaseColor.WHITE;
                        ReportMainTable.AddCell(RowCell);
                    }
                }
            }
            AddColumn = TotalQty % 3;
            if (AddColumn > 0)
            {
                for (i = 0; i < (3 - AddColumn); i++)
                {
                    PdfPCell RowCell = new PdfPCell();
                    RowCell.BorderColor = BaseColor.WHITE;
                    ReportMainTable.AddCell(RowCell);
                }
            }
            return ReportMainTable;
        }

        //a4 sheet barcode from grid
        public void GenerateBarcodeA4(DataGridView DataGridView, string fileName, string fileExtension, bool isPrint, string Location, string PrinterName, LabelType LabelType)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, -60, -60, 35, 25);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();
                PdfPTable ReportMainTable = MainTable(DataGridView, Location);
                pdfDoc.Add(ReportMainTable);
                pdfDoc.Close();
                PdfGeneration.SaveMemoryStreamBarcode(PrinterName, myMemoryStream, fileName, fileExtension, isPrint, PaperTypes.A4_PORTRAIT);
            }
        }

        private PdfPTable MainTable(DataGridView DataGridView, string Location)
        {
            string[] StartCell = Location.Split(',');
            int PrintStartCellCount = (((int.Parse(StartCell[0]) - 1) * 3) + int.Parse(StartCell[1]));
            int PrintStartCell = (PrintStartCellCount == 0) ? 0 : (PrintStartCellCount - 1);
            int UnCompletedRowCell = (PrintStartCell % 3);
            int Cols = 3;

            PdfPTable ReportMainTable = new PdfPTable(Cols);

            float[] widths = null;

            widths = new float[] { 280f, 270f, 260f };
            ReportMainTable.SetWidths(widths);

            int Rows = 0;
            int AddColumn = 0;
            int i = 0;
            int jCell = 0;
            int TotalQty = PrintStartCell;
            for (int P = 0; P < PrintStartCell; P++)
            {
                PdfPCell RowCell = new PdfPCell();
                RowCell.MinimumHeight = 97;
                RowCell.BorderColor = BaseColor.WHITE;
                ReportMainTable.AddCell(RowCell);
            }
            foreach (DataGridViewRow Row in DataGridView.Rows)
            {
                if ((bool)Row.Cells[(int)BarcodePrintGridColumn.SELECT_ITEM].Value)
                {
                    int Quantity = int.Parse(Row.Cells[(int)BarcodePrintGridColumn.QTY].Value.ToString()!);
                    TotalQty = TotalQty + Quantity;
                    Rows += ((Quantity / 3) + ((Quantity % 3) != 0 ? 1 : 0)) + (UnCompletedRowCell != 0 ? 1 : 0);
                    if (AddColumn != 0)
                    {
                        jCell = AddColumn;
                        i = i - 1;
                    }
                    AddColumn = (Quantity % 3);

                    if (UnCompletedRowCell != 0)
                    {
                        jCell = UnCompletedRowCell;
                        if (AddColumn != 0 && ((3 - jCell) - AddColumn) > -1)
                        {
                            Rows -= 1;
                        }
                    }
                    if (jCell != 0)
                    {
                        int TempAddColumn = (3 - jCell);
                        if (TempAddColumn <= AddColumn)
                        {
                            if (TempAddColumn == AddColumn && UnCompletedRowCell == 0)
                            {
                                Rows -= 1;
                            }
                            AddColumn -= TempAddColumn;
                        }
                        else
                        {
                            if (AddColumn != 0 && UnCompletedRowCell == 0)
                            {
                                Rows -= 1;
                            }
                            AddColumn = (3 - (TempAddColumn - AddColumn));
                        }
                    }
                    UnCompletedRowCell = 0;
                    MemoryStream Barcode = new MemoryStream();
                    var Image = BarCode.GenerateImageBarcode1(Row.Cells[(int)BarcodePrintGridColumn.ITEM_CODE].Value.ToString());
                    Image.Save(Barcode, System.Drawing.Imaging.ImageFormat.Png);
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Barcode.ToArray());
                    image.ScaleAbsoluteHeight(15);
                    image.ScaleAbsoluteWidth(130);
                    var Temp = image;
                    int alength = Global.Company.Name.Length;
                    int plength = Row.Cells[(int)BarcodePrintGridColumn.ITEM_NAME].Value.ToString().Length;

                    var AboveBarcodeName = " " + Global.Company.Name.Substring(0, (alength <= 25) ? alength : 25) + ((alength > 25) ? ".." : "") + "\n" + " " + Row.Cells[(int)BarcodePrintGridColumn.ITEM_NAME].Value.ToString().Substring(0, (plength <= 25) ? plength : 25) + ((plength > 25) ? ".." : "");
                    var BelowBarcodeName = " " + Row.Cells[(int)BarcodePrintGridColumn.ITEM_CODE].Value.ToString();
                    var price = "MRP: ₹" + (float.Parse(Row.Cells[(int)BarcodePrintGridColumn.MRP].Value.ToString())).ToString(Global.Company.PrimaryCurrency.CurrencyFormat) +
                       "     Rate: ₹" + (float.Parse(Row.Cells[(int)BarcodePrintGridColumn.RATE].Value.ToString())).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    var Description = string.Empty;

                    string Space = "";
                    int blength = 12 + (13 - BelowBarcodeName.Length);
                    for (int s = 0; s < blength; s++)
                    {
                        Space = Space + " ";
                    }
                    BelowBarcodeName = Space + BelowBarcodeName;

                    for (i = i; i < Rows; i++)
                    {
                        PdfPCell RowCell = new PdfPCell();

                        for (int j = jCell; j < Cols; j++)
                        {
                            if (AddColumn != 0 && AddColumn <= j && i == (Rows - 1))
                            {
                                continue;
                            }
                            RowCell = new PdfPCell(new Phrase(AboveBarcodeName, PdfDataAlignment.GetFont("Font_Normal_Italic_10_Black")));
                            RowCell.MinimumHeight = 90;
                            RowCell.PaddingTop = -3;
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColor = BaseColor.WHITE;
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            RowCell.AddElement(new Phrase(AboveBarcodeName, PdfDataAlignment.GetFont("Font_Normal_Italic_10_Black")));

                            RowCell.PaddingTop = -3;
                            RowCell.UseVariableBorders = true;
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            RowCell.AddElement(Temp);

                            RowCell.PaddingTop = -4;
                            RowCell.UseVariableBorders = true;
                            RowCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            RowCell.VerticalAlignment = Element.ALIGN_TOP;
                            RowCell.AddElement(new Phrase(BelowBarcodeName, PdfDataAlignment.GetFont("Font_Normal_Italic_10_Black")));

                            RowCell.PaddingTop = -3;
                            RowCell.UseVariableBorders = true;
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            RowCell.AddElement(new Phrase(price, PdfDataAlignment.GetFont("Font_Normal_Italic_10_Black")));
                            ReportMainTable.AddCell(RowCell);
                        }
                        jCell = 0;

                        if (i != (Rows - 1))
                        {
                            for (int k = 0; k < 3; k++)
                            {
                                RowCell = new PdfPCell();
                                RowCell.MinimumHeight = 7;
                                RowCell.BorderColor = BaseColor.WHITE;
                                ReportMainTable.AddCell(RowCell);
                            }
                        }
                    }
                }
            }
            AddColumn = TotalQty % 3;
            if (AddColumn > 0)
            {
                for (i = 0; i < (3 - AddColumn); i++)
                {
                    PdfPCell RowCell = new PdfPCell();
                    RowCell.BorderColor = BaseColor.WHITE;
                    ReportMainTable.AddCell(RowCell);
                }
            }
            return ReportMainTable;
        }

        //generate barcode label for single product
        public void GenerateBarcodeLabel(long productId, LabelSize size, long quantity, string printerName)
        {
            // Fetch product data
            var product = CatalogProductManager.Instance.GetProductInfoById(productId);

            // Format text fields with ellipsis if too long
            var productName = TruncateWithEllipsis(product.Name, 17);
            var companyName = TruncateWithEllipsis(Global.Company.Name, 17);

            // Prepare pricing information
            var priceInfo = PreparePriceInformation(product);

            // Generate labels based on size
            if (size == LabelSize.THREE)
            {
                PrintThreeColumnLabels(printerName, product, productName, companyName, priceInfo, quantity);
            }
        }

        // Helper method to truncate text with ellipsis
        private string TruncateWithEllipsis(string text, int maxLength)
        {
            if (text.Length <= maxLength) return text;
            return text.Substring(0, maxLength) + "..";
        }

        // Helper method to prepare price information
        private PriceInformation PreparePriceInformation(Product product)
        {
            var currencyFormat = Global.Company.PrimaryCurrency.CurrencyFormat;
            var isWholesale = Global.Company.BusinessType == BuisnessType.Wholesale;

            return new PriceInformation
            {
                MrpText = "sms:",
                RateText = "Rs:",
                FormattedMrp = product.RetailPrice.ToString(currencyFormat).Replace(",", ""),
                FormattedRate = (isWholesale ? product.WholdSalePrice : product.RetailPrice)
                                .ToString(currencyFormat)
                                .Replace(",", "")
            };
        }

        // Helper method to print 3-column labels
        private void PrintThreeColumnLabels(string printerName, Product product,
                                           string productName, string companyName,
                                           PriceInformation priceInfo, long quantity)
        {
            var rows = quantity / 3;
            var remaining = quantity % 3;

            // Print full rows (3 labels each)
            for (long i = 0; i < rows; i++)
            {
                var commands = CreateThreeLabelCommandSet(product, productName, companyName, priceInfo);
                PrintLabel(printerName, commands, i.ToString());
            }

            // Print remaining labels (1 or 2)
            if (remaining > 0)
            {
                var commands = remaining == 1
                    ? CreateSingleLabelCommandSet(product, productName, companyName, priceInfo)
                    : CreateDoubleLabelCommandSet(product, productName, companyName, priceInfo);

                PrintLabel(printerName, commands, "");
            }
        }

        // Command set builders
        private string[] CreateThreeLabelCommandSet(Product product, string productName,
                                                  string companyName, PriceInformation priceInfo)
        {
            return CombineCommands(
                GetPrinterSetupCommands(),
                GetLabelCommand(785, companyName, productName, product, priceInfo, 2, 1),
                GetLabelCommand(506, companyName, productName, product, priceInfo, 2, 1),
                GetLabelCommand(228, companyName, productName, product, priceInfo, 2, 1),
                "P1"
            );
        }

        private string[] CreateDoubleLabelCommandSet(Product product, string productName,
                                                   string companyName, PriceInformation priceInfo)
        {
            return CombineCommands(
                GetPrinterSetupCommands(),
                GetLabelCommand(785, companyName, productName, product, priceInfo, 2, 1),
                GetLabelCommand(506, companyName, productName, product, priceInfo, 2, 1),
                "P1"
            );
        }

        private string[] CreateSingleLabelCommandSet(Product product, string productName,
                                                   string companyName, PriceInformation priceInfo)
        {
            return CombineCommands(
                GetPrinterSetupCommands(),
                GetLabelCommand(785, companyName, productName, product, priceInfo, 2, 1),
                "P1"
            );
        }

        // Helper method to create printer setup commands
        private string[] GetPrinterSetupCommands()
        {
            return new string[]
            {
                "I8,A",    // 203 DPI, font A
                "q812",    // Label height = 812 dots (~4 inches)
                "O",       // Reverse printing (optional)
                "JF",      // Field justification
                "ZT",      // Thermal transfer mode
                "Q200,25", // Label width = 200 dots, gap sensitivity
                "N"        // Normal printing mode
            };
        }

        // Helper method to create label commands
        private string[] GetLabelCommand(int xOffset, string companyName, string productName,
                                        Product product, PriceInformation priceInfo,
                                        int largeFontSize, int smallFontSize)
        {
            var quote = '"';
            return new string[]
            {
                $"A{xOffset},180,2,{largeFontSize},1,1,N,{quote}{companyName}{quote}",
                $"A{xOffset},150,2,{smallFontSize},1,1,N,{quote}{productName}{quote}",
                $"B{xOffset-11},126,2,1,1,4,41,N,{quote}{product.MaterialId}{quote}",
                $"A{xOffset-11},69,2,{smallFontSize},1,1,N,{quote}{product.MaterialId}{quote}",
                $"A{xOffset+5},30,2,{largeFontSize},1,1,N,{quote}{priceInfo.MrpText}{quote}",
                $"A{xOffset-50},27,2,{largeFontSize},1,1,N,{quote}{priceInfo.FormattedMrp}{quote}"
            };
        }

        // Helper method to combine command arrays
        private string[] CombineCommands(params object[] commandSets)
        {
            return commandSets.SelectMany(c =>
                c is string[] array ? array : new[] { c.ToString()! }
            ).ToArray();
        }

        // Supporting class for price information
        private class PriceInformation
        {
            public string? MrpText { get; set; }
            public string? RateText { get; set; }
            public string? FormattedMrp { get; set; }
            public string? FormattedRate { get; set; }
        }
        public void GenerateBarcodeLabelxxx(long ProductId, LabelSize Size, long Qty, string PrinterName/*, LabelType Label*/)
        {
            Product ProductFromDB = CatalogProductManager.Instance.GetProductInfoById(ProductId);
            var length = ProductFromDB.Name.Length;
            var Product = ProductFromDB.Name.Substring(0, (length <= 15) ? length : 15) + ((length > 15) ? ".." : "");
            length = Global.Company.Name.Length;
            var CompanyName = Global.Company.Name.Substring(0, (length <= 15) ? length : 15) + ((length > 15) ? ".." : "");


            char quote = '"';
            string Mrp = "Mrp:";
            string Rate = "Rs:";
            string lMrp = ProductFromDB.Msrp.ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            string PMrp = lMrp.Replace(",", "");
            string lRate = (Global.Company.BusinessType == BuisnessType.Wholesale ? ProductFromDB.WholdSalePrice : ProductFromDB.RetailPrice).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            string PRate = lRate.Replace(",", "");
            if (Size == LabelSize.THREE)
            {
                long Rows = Qty / 3;
                long Cols = Qty % 3;
                string[] Print;
                for (long i = 0; i < Rows; i++)
                {
                    Print = new string[] {"I8,A","q812","O","JF","ZT","Q200,25","N","A785,180,2,4,1,1,N,"+quote+CompanyName+quote+"","A785,150,2,2,1,1,N,"+quote+Product+quote+"",
                    "B774,126,2,1,1,4,41,N,"+quote+ProductFromDB.MaterialId+quote+"","A774,69,2,2,1,1,N,"+quote+ProductFromDB.MaterialId+quote+"","A790,30,2,4,1,1,N,"+quote+Mrp+quote+"",
                    "A735,27,2,4,1,1,N,"+quote+PMrp+quote+"","A506,180,2,4,1,1,N,"+quote+CompanyName+quote+"",
                    "A506,150,2,2,1,1,N,"+quote+Product+quote+"","B487,126,2,1,1,4,41,N,"+quote+ProductFromDB.MaterialId+quote+"","A492,69,2,2,1,1,N,"+quote+ProductFromDB.MaterialId+quote+"",
                    "A509,30,2,4,1,1,N,"+quote+Mrp+quote+"","A453,27,2,4,1,1,N,"+quote+PMrp+quote+"",
                    "A228,180,2,4,1,1,N,"+quote+CompanyName+quote+"","A228,150,2,2,1,1,N,"+quote+Product+quote+"","B209,126,2,1,1,4,41,N,"+quote+ProductFromDB.MaterialId+quote+"",
                    "A214,69,2,2,1,1,N,"+quote+ProductFromDB.MaterialId+quote+"","A231,30,2,4,1,1,N,"+quote+Mrp+quote+"","A175,27,2,4,1,1,N,"+quote+PMrp+quote+"",
                    "P1"};

                    PrintLabel(PrinterName, Print, i.ToString());
                }
                if (Cols == 1)
                {
                    Print = new string[] {"I8,A","q812","O","JF","ZT","Q200,25","N","A785,180,2,4,1,1,N,"+quote+CompanyName+quote+"","A785,150,2,2,1,1,N,"+quote+Product+quote+"",
                    "B774,126,2,1,1,4,41,N,"+quote+ProductFromDB.MaterialId+quote+"","A774,69,2,2,1,1,N,"+quote+ProductFromDB.MaterialId+quote+"","A790,30,2,4,1,1,N,"+quote+Mrp+quote+"",
                    "A735,27,2,4,1,1,N,"+quote+PMrp+quote+"","P1"};

                    PrintLabel(PrinterName, Print, "");
                }
                else if (Cols == 2)
                {
                    Print = new string[] {"I8,A","q812","O","JF","ZT","Q200,25","N","A785,180,2,4,1,1,N,"+quote+CompanyName+quote+"","A785,150,2,2,1,1,N,"+quote+Product+quote+"",
                    "B774,136,2,1,1,4,41,N,"+quote+ProductFromDB.MaterialId+quote+"","A774,69,2,2,1,1,N,"+quote+ProductFromDB.MaterialId+quote+"","A790,30,2,4,1,1,N,"+quote+Mrp+quote+"",
                    "A735,27,2,4,1,1,N,"+quote+PMrp+quote+"","A506,180,2,4,1,1,N,"+quote+CompanyName+quote+"",
                    "A506,150,2,2,1,1,N,"+quote+Product+quote+"","B487,126,2,1,1,4,41,N,"+quote+ProductFromDB.MaterialId+quote+"","A492,69,2,2,1,1,N,"+quote+ProductFromDB.MaterialId+quote+"",
                    "A509,30,2,4,1,1,N,"+quote+Mrp+quote+"","A453,27,2,4,1,1,N,"+quote+PMrp+quote+"","P1"};

                    PrintLabel(PrinterName, Print, "");
                }
            }
            else if (Size == LabelSize.TWO)
            {
                long Rows = Qty / 2;
                long Cols = Qty % 2;
                string[] Print;
                for (long i = 0; i < Rows; i++)
                {
                    Print = new string[] {"I8,A","q812","O","JF","ZT","Q200,25","N","A796,180,2,4,1,1,N,"+quote+CompanyName+quote+"",
                    "A796,147,2,3,1,1,N,"+quote+Product+quote+"","B770,115,2,1C,4,8,39,N,"+quote+ProductFromDB.MaterialId+quote+"","A668,69,2,3,1,1,N,"+quote+ProductFromDB.MaterialId+quote+"",
                    "A796,41,2,3,1,1,N,"+quote+Mrp+quote+"","A740,38,2,2,1,1,N," + quote + PMrp + quote + "","A548,41,2,3,1,1,N,"+ quote + Rate + quote +"","A478,38,2,2,1,1,N," + quote + PRate + quote + "",
                    "A380,180,2,4,1,1,N,"+quote+CompanyName+quote+"","A380,147,2,3,1,1,N,"+quote+Product+quote+"","B354,115,2,1C,4,8,39,N,"+quote+ProductFromDB.MaterialId+quote+"",
                    "A252,69,2,3,1,1,N,"+quote+ProductFromDB.MaterialId+quote+"","A380,41,2,3,1,1,N,"+ quote + Mrp + quote +"","A324,38,2,2,1,1,N,"+ quote + PMrp + quote +"","A132,41,2,3,1,1,N,"+ quote + Rate + quote +"",
                    "A62,38,2,2,1,1,N,"+ quote + PRate + quote +"","P1"};
                    PrintLabel(PrinterName, Print, i.ToString());
                }
                if (Cols != 0)
                {
                    Print = new string[] {"I8,A","q812","O","JF","ZT","Q200,25","N","A796,180,2,4,1,1,N,"+quote+CompanyName+quote+"",
                    "A796,147,2,3,1,1,N,"+quote+Product+quote+"","B770,115,2,1C,4,8,39,N,"+quote+ProductFromDB.MaterialId+quote+"","A668,69,2,3,1,1,N,"+quote+ProductFromDB.MaterialId+quote+"",
                    "A796,41,2,3,1,1,N,"+quote+Mrp+quote+"","A740,38,2,2,1,1,N," + quote + PMrp + quote + "","A548,41,2,3,1,1,N,"+ quote + Rate + quote +"","A478,38,2,2,1,1,N," + quote + PRate + quote + "",
                "P1"};
                    PrintLabel(PrinterName, Print, "");
                }
            }
            else if (Size == LabelSize.ONE)
            {
                string[] Print = {"I8,A","q779","O","JF","ZT","Q184,25","N","A748,170,2,4,1,1,N,"+quote+CompanyName+quote+"",
                    "A748,137,2,3,1,1,N,"+quote+Product+quote+"","B722,105,2,1C,5,10,39,N,"+quote+ProductFromDB.MaterialId+quote+"","A580,60,2,3,1,1,N,"+quote+ProductFromDB.MaterialId+quote+"",
                    "A748,32,2,3,1,1,N,"+quote+Mrp+quote+"","A692,29,2,2,1,1,N,"+ quote + PMrp + quote +"","A420,32,2,3,1,1,N,"+ quote + Rate + quote +"","A350,29,2,2,1,1,N,"+ quote + PRate + quote +"",
                "P1"};
                PrintLabel(PrinterName, Print, "");
            }

        }

        //generate barcode label for multiple product
        public void GenerateBarcodeLabel(DataGridView DataGridView, LabelSize Size, string PrinterName, LabelType LabelType)
        {
            int TotalCount = 0;
            int CurrentCount = 0;
            int BufferCount = 0;
            bool IsBufferString = false;
            string[] BufferPrint = new string[] { };

            foreach (DataGridViewRow Row in DataGridView.Rows)
            {
                if ((bool)Row.Cells[(int)BarcodePrintGridColumn.SELECT_ITEM].Value)
                {
                    TotalCount++;
                }
            }
            foreach (DataGridViewRow Row in DataGridView.Rows)
            {
                if ((bool)Row.Cells[(int)BarcodePrintGridColumn.SELECT_ITEM].Value)
                {
                    CurrentCount++;
                    bool IsBatch = (bool)Row.Cells[(int)BarcodePrintGridColumn.ISBATCH].Value;
                    string BatchNo = string.Empty;
                    string MeterialId = Row.Cells[(int)BarcodePrintGridColumn.ITEM_CODE].Value.ToString()!;
                    if (IsBatch)
                    {
                        BatchNo = Row.Cells[(int)BarcodePrintGridColumn.BATCH_NO].Value.ToString()!;
                    }
                    var length = Row.Cells[(int)BarcodePrintGridColumn.ITEM_NAME].Value.ToString()!.Length;
                    var Product = Row.Cells[(int)BarcodePrintGridColumn.ITEM_NAME].Value.ToString()!.Substring(0, (length <= 15) ? length : 15) + ((length > 15) ? ".." : "");
                    length = Global.Company.Name.Length;
                    var CompanyName = Global.Company.Name.Substring(0, (length <= 15) ? length : 15) + ((length > 15) ? ".." : "");

                    char quote = '"';
                    string Mrp = "Mrp:";
                    string Rate = "Rs:";
                    string PMrp = double.Parse(Row.Cells[(int)BarcodePrintGridColumn.MRP].Value.ToString()!).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    string PRate = double.Parse(Row.Cells[(int)BarcodePrintGridColumn.RATE].Value.ToString()!).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    string MatId = IsBatch ? quote + MeterialId + "#" + BatchNo + "#" + quote : quote + MeterialId + quote;
                    string DisMatId = LabelType == LabelType.QRCODE ? quote + MeterialId + quote : IsBatch ? quote + MeterialId + " " + BatchNo + quote : quote + MeterialId + quote;
                    long Qty = long.Parse(Row.Cells[(int)BarcodePrintGridColumn.QTY].Value.ToString()!);
                    if (Size == LabelSize.THREE)
                    {
                        long Rows = (BufferCount + Qty) / 3;
                        long Cols = (BufferCount + Qty) % 3;
                        string[] Print;
                        for (long i = 0; i < Rows; i++)
                        {
                            if (IsBufferString)
                            {
                                Print = BufferPrint.Concat(BufferCount == 1 ? new string[]{"A506,190,2,4,1,1,N,"+quote+CompanyName+quote+"",
                                    "A506,158,2,2,1,1,N,"+quote+Product+quote+"",
                                    (LabelType == LabelType.QRCODE ? "b412,43,Q,m2,s4,eL," : "B487,126,2,1,1,2,41,N,")+MatId+"",
                                    (LabelType == LabelType.QRCODE ? "A524,55,1,1,1,1,N," : "A492,69,2,2,1,1,N,")+DisMatId+"",
                                    (LabelType == LabelType.QRCODE?"A401,55,1,1,1,1,N," + quote + BatchNo + quote + "":""),"A509,30,2,3,1,1,N,"+quote+Mrp+quote+"",
                                    "A453,27,2,2,1,1,N,"+quote+PMrp+quote+"","A300,10,1,3,1,1,N,"+quote+Rate+quote+"",
                                    "A300,50,1,2,1,1,N,"+quote+PRate+quote+"",
                                    "A228,185,2,4,1,1,N,"+quote+CompanyName+quote+"","A228,153,2,2,1,1,N,"+quote+Product+quote+"",
                                    (LabelType == LabelType.QRCODE ? "b134,43,Q,m2,s4,eL," : "B209,126,2,1,1,2,41,N,")+MatId+"",
                                    (LabelType == LabelType.QRCODE ? "A246,55,1,1,1,1,N," : "A214,69,2,2,1,1,N,")+DisMatId+"",
                                    (LabelType == LabelType.QRCODE?"A123,55,1,1,1,1,N," + quote + BatchNo + quote + "":""),
                                    "A231,30,2,3,1,1,N,"+quote+Mrp+quote+"","A175,27,2,2,1,1,N,"+quote+PMrp+quote+"",
                                    "A25,10,1,3,1,1,N,"+quote+Rate+quote+"",
                                    "A25,50,1,2,1,1,N,"+quote+PRate+quote+"","P1" } : new string[]{
                                    "A228,185,2,4,1,1,N,"+quote+CompanyName+quote+"","A228,153,2,2,1,1,N,"+quote+Product+quote+"",
                                    (LabelType == LabelType.QRCODE ? "b134,43,Q,m2,s4,eL," : "B209,126,2,1,1,2,41,N,")+MatId+"",
                                    (LabelType == LabelType.QRCODE ? "A246,55,1,1,1,1,N," : "A214,69,2,2,1,1,N,")+DisMatId+"",
                                    (LabelType == LabelType.QRCODE?"A123,55,1,1,1,1,N," + quote + BatchNo + quote + "":""),"A231,30,2,3,1,1,N,"+quote+Mrp+quote+"",
                                    "A175,27,2,2,1,1,N,"+quote+PMrp+quote+"","A25,10,1,3,1,1,N,"+quote+Rate+quote+"",
                                    "A25,50,1,2,1,1,N,"+quote+PRate+quote+"","P1" }).ToArray();
                            }
                            else
                            {
                                Print = new string[] {"I8,A", "q812", "O", "JF", "ZT", "Q200,25",
                                    "N","A785,190,2,4,1,1,N,"+quote+CompanyName+quote+"","A785,158,2,2,1,1,N,"+quote+Product+quote+"",
                                    (LabelType==LabelType.QRCODE? "b689,43,Q,m2,s4,eL,":"B774,126,2,1,1,2,41,N,")+MatId+"",
                                    (LabelType == LabelType.QRCODE ? "A801,55,1,1,1,1,N," : "A774,69,2,2,1,1,N,")+DisMatId+"",
                                    (LabelType == LabelType.QRCODE?"A678,55,1,1,1,1,N," + quote + BatchNo + quote + "":""),"A790,30,2,3,1,1,N,"+quote+Mrp+quote+"",
                                    "A735,27,2,2,1,1,N,"+quote+PMrp+quote+"","A580,10,1,3,1,1,N,"+quote+Rate+quote+"",
                                    "A580,50,1,2,1,1,N,"+quote+PRate+quote+"","A506,190,2,4,1,1,N,"+quote+CompanyName+quote+"",
                                    "A506,158,2,2,1,1,N,"+quote+Product+quote+"",
                                    (LabelType == LabelType.QRCODE ? "b412,43,Q,m2,s4,eL," : "B487,126,2,1,1,2,41,N,")+MatId+"",
                                    (LabelType == LabelType.QRCODE ? "A524,55,1,1,1,1,N," : "A492,69,2,2,1,1,N,")+DisMatId+"",
                                    (LabelType == LabelType.QRCODE?"A401,55,1,1,1,1,N," + quote + BatchNo + quote + "":""),
                                    "A509,30,2,3,1,1,N,"+quote+Mrp+quote+"","A453,27,2,2,1,1,N,"+quote+PMrp+quote+"",
                                    "A300,10,1,3,1,1,N,"+quote+Rate+quote+"","A300,50,1,2,1,1,N,"+quote+PRate+quote+"",
                                    "A228,185,2,4,1,1,N,"+quote+CompanyName+quote+"","A228,153,2,2,1,1,N,"+quote+Product+quote+"",
                                    (LabelType == LabelType.QRCODE ? "b134,43,Q,m2,s4,eL," : "B209,126,2,1,1,2,41,N,")+MatId+"",
                                    (LabelType == LabelType.QRCODE ? "A246,55,1,1,1,1,N," : "A214,69,2,2,1,1,N,")+DisMatId+"",
                                    (LabelType == LabelType.QRCODE?"A123,55,1,1,1,1,N," + quote + BatchNo + quote + "":""),
                                    "A231,30,2,3,1,1,N,"+quote+Mrp+quote+"","A175,27,2,2,1,1,N,"+quote+PMrp+quote+"",
                                    "A25,10,1,3,1,1,N,"+quote+Rate+quote+"","A25,50,1,2,1,1,N,"+quote+PRate+quote+"","P1"};
                            }
                            PrintLabel(PrinterName, Print, i.ToString());
                            IsBufferString = false;
                            BufferPrint = new string[] { };
                            BufferCount = 0;
                        }
                        if (Cols == 1)
                        {
                            if (TotalCount == CurrentCount)
                            {

                                Print = new string[] {"I8,A", "q812", "O", "JF", "ZT", "Q200,25", "N",
                                "A785,190,2,4,1,1,N,"+quote+CompanyName+quote+"","A785,158,2,2,1,1,N,"+quote+Product+quote+"",
                                (LabelType==LabelType.QRCODE? "b689,43,Q,m2,s4,eL,":"B774,126,2,1,1,2,41,N,")+MatId+"",
                                (LabelType == LabelType.QRCODE ? "A801,55,1,1,1,1,N," : "A774,69,2,2,1,1,N,")+DisMatId+"",
                                (LabelType == LabelType.QRCODE?"A678,55,1,1,1,1,N," + quote + BatchNo + quote + "":""),"A790,30,2,3,1,1,N,"+quote+Mrp+quote+"",
                                "A735,27,2,2,1,1,N,"+quote+PMrp+quote+"","A580,10,1,3,1,1,N,"+quote+Rate+quote+"",
                                "A580,50,1,2,1,1,N,"+quote+PRate+quote+"","P1"};
                                PrintLabel(PrinterName, Print, "");
                                IsBufferString = false;
                                BufferPrint = new string[] { };
                                BufferCount = 0;
                            }
                            else
                            {
                                IsBufferString = true;
                                BufferCount++;
                                BufferPrint = new string[] {"I8,A","q812","O","JF","ZT","Q200,25","N",
                                "A785,190,2,4,1,1,N,"+quote+CompanyName+quote+"","A785,158,2,2,1,1,N,"+quote+Product+quote+"",
                                (LabelType==LabelType.QRCODE? "b689,43,Q,m2,s4,eL,":"B774,126,2,1,1,2,41,N,")+MatId+"",
                                (LabelType == LabelType.QRCODE ? "A801,55,1,1,1,1,N," : "A774,69,2,2,1,1,N,")+DisMatId+"",
                                (LabelType == LabelType.QRCODE?"A678,55,1,1,1,1,N," + quote + BatchNo + quote + "":""),"A790,30,2,3,1,1,N,"+quote+Mrp+quote+"",
                                "A735,27,2,2,1,1,N,"+quote+PMrp+quote+"","A580,10,1,3,1,1,N,"+quote+Rate+quote+"",
                                "A580,50,1,2,1,1,N,"+quote+PRate+quote+"" };
                            }
                        }
                        else if (Cols == 2)
                        {
                            if (TotalCount == CurrentCount)
                            {
                                if (IsBufferString)
                                {
                                    string[] TempPrint = IsBufferString ? BufferPrint : new string[] { "I8,A", "q812", "O", "JF", "ZT", "Q200,25", "N", };
                                    Print = TempPrint.Concat((BufferCount == 1 ? new string[] {
                                    "A506,190,2,4,1,1,N,"+quote+CompanyName+quote+"",
                                    "A506,158,2,2,1,1,N,"+quote+Product+quote+"",
                                    (LabelType == LabelType.QRCODE ? "b412,43,Q,m2,s4,eL," : "B487,126,2,1,1,2,41,N,")+MatId+"",
                                    (LabelType == LabelType.QRCODE ? "A524,55,1,1,1,1,N," : "A492,69,2,2,1,1,N,")+DisMatId+"",
                                    (LabelType == LabelType.QRCODE?"A401,55,1,1,1,1,N," + quote + BatchNo + quote + "":""),"A509,30,2,3,1,1,N,"+quote+Mrp+quote+"",
                                    "A453,27,2,2,1,1,N,"+quote+PMrp+quote+"","A300,10,1,3,1,1,N,"+quote+Rate+quote+"",
                                    "A300,50,1,2,1,1,N,"+quote+PRate+quote+"","P1"} : new string[] {
                                    "A506,190,2,4,1,1,N,"+quote+CompanyName+quote+"","A506,158,2,2,1,1,N,"+quote+Product+quote+"",
                                    (LabelType == LabelType.QRCODE ? "b412,43,Q,m2,s4,eL," : "B487,126,2,1,1,2,41,N,")+MatId+"",
                                    (LabelType == LabelType.QRCODE ? "A524,55,1,1,1,1,N," : "A492,69,2,2,1,1,N,")+DisMatId+"",
                                    (LabelType == LabelType.QRCODE?"A401,55,1,1,1,1,N," + quote + BatchNo + quote + "":""),"A509,30,2,3,1,1,N,"+quote+Mrp+quote+"",
                                    "A453,27,2,2,1,1,N,"+quote+PMrp+quote+"","A300,10,1,3,1,1,N,"+quote+Rate+quote+"",
                                    "A300,50,1,2,1,1,N,"+quote+PRate+quote+"","P1"})).ToArray();
                                }
                                else
                                {

                                    Print = new string[] {"I8,A", "q812", "O", "JF", "ZT", "Q200,25", "N","A785,190,2,4,1,1,N,"+quote+CompanyName+quote+"",
                                    "A785,158,2,2,1,1,N,"+quote+Product+quote+"",
                                    (LabelType==LabelType.QRCODE? "b689,43,Q,m2,s4,eL,":"B774,126,2,1,1,2,41,N,")+MatId+"",
                                    (LabelType == LabelType.QRCODE ? "A801,55,1,1,1,1,N," : "A774,69,2,2,1,1,N,")+DisMatId+"",
                                    (LabelType == LabelType.QRCODE?"A678,55,1,1,1,1,N," + quote + BatchNo + quote + "":""),"A790,30,2,3,1,1,N,"+quote+Mrp+quote+"",
                                    "A735,27,2,2,1,1,N,"+quote+PMrp+quote+"","A580,10,1,3,1,1,N,"+quote+Rate+quote+"","A580,50,1,2,1,1,N,"+quote+PRate+quote+"",
                                    "A506,190,2,4,1,1,N,"+quote+CompanyName+quote+"","A506,158,2,2,1,1,N,"+quote+Product+quote+"",
                                    (LabelType == LabelType.QRCODE ? "b412,43,Q,m2,s4,eL," : "B487,126,2,1,1,2,41,N,")+MatId+"",
                                    (LabelType == LabelType.QRCODE ? "A524,55,1,1,1,1,N," : "A492,69,2,2,1,1,N,")+DisMatId+"",
                                    (LabelType == LabelType.QRCODE?"A401,55,1,1,1,1,N," + quote + BatchNo + quote + "":""),
                                    "A509,30,2,3,1,1,N,"+quote+Mrp+quote+"","A453,27,2,2,1,1,N,"+quote+PMrp+quote+"","A300,10,1,3,1,1,N,"+quote+Rate+quote+"",
                                    "A300,50,1,2,1,1,N,"+quote+PRate+quote+"","P1"};
                                }
                                PrintLabel(PrinterName, Print, "");
                                IsBufferString = false;
                                BufferPrint = new string[] { };
                                BufferCount = 0;
                            }
                            else
                            {
                                IsBufferString = true;
                                BufferCount += 2;
                                BufferPrint = new string[] {"I8,A","q812","O","JF","ZT","Q200,25","N","A785,190,2,4,1,1,N,"+quote+CompanyName+quote+"","A785,158,2,2,1,1,N,"+quote+Product+quote+"",
                                (LabelType==LabelType.QRCODE? "b689,43,Q,m2,s4,eL,":"B774,126,2,1,1,2,41,N,")+MatId+"",
                                (LabelType == LabelType.QRCODE ? "A801,55,1,1,1,1,N," : "A774,69,2,2,1,1,N,")+DisMatId+"",
                                (LabelType == LabelType.QRCODE?"A678,55,1,1,1,1,N," + quote + BatchNo + quote + "":""),"A790,30,2,3,1,1,N,"+quote+Mrp+quote+"",
                                "A735,27,2,2,1,1,N,"+quote+PMrp+quote+"","A580,10,1,3,1,1,N,"+quote+Rate+quote+"","A580,50,1,2,1,1,N,"+quote+PRate+quote+"",
                                "A506,190,2,4,1,1,N,"+quote+CompanyName+quote+"","A506,158,2,2,1,1,N,"+quote+Product+quote+"",
                                (LabelType == LabelType.QRCODE ? "b412,43,Q,m2,s4,eL," : "B487,126,2,1,1,2,41,N,")+MatId+"",
                                (LabelType == LabelType.QRCODE ? "A524,55,1,1,1,1,N," : "A492,69,2,2,1,1,N,")+DisMatId+"",
                                (LabelType == LabelType.QRCODE?"A401,55,1,1,1,1,N," + quote + BatchNo + quote + "":""),
                                "A509,30,2,3,1,1,N,"+quote+Mrp+quote+"","A453,27,2,2,1,1,N,"+quote+PMrp+quote+"",
                                "A300,10,1,3,1,1,N,"+quote+Rate+quote+"","A300,50,1,2,1,1,N,"+quote+PRate+quote+""};
                            }
                        }
                    }
                    else if (Size == LabelSize.TWO)
                    {
                        long Rows = Qty / 2;
                        long Cols = Qty % 2;
                        string[] Print;
                        for (long i = 0; i < Rows; i++)
                        {
                            Print = new string[] {"I8,A","q812","O","JF","ZT","Q200,25","N","A796,180,2,4,1,1,N,"+quote+CompanyName+quote+"",
                                "A796,147,2,3,1,1,N,"+quote+Product+quote+"","B770,115,2,1C,4,8,39,N,"+MatId+"","A668,69,2,3,1,1,N,"+MatId+"",
                                "A796,41,2,3,1,1,N,"+quote+Mrp+quote+"","A740,38,2,2,1,1,N," + quote + PMrp + quote + "","A548,41,2,3,1,1,N,"+ quote + Rate + quote +"","A478,38,2,2,1,1,N," + quote + PRate + quote + "",
                                "A380,180,2,4,1,1,N,"+quote+CompanyName+quote+"","A380,147,2,3,1,1,N,"+quote+Product+quote+"","B354,115,2,1C,4,8,39,N,"+MatId+"",
                                "A252,69,2,3,1,1,N,"+MatId+"","A380,41,2,3,1,1,N,"+ quote + Mrp + quote +"","A324,38,2,2,1,1,N,"+ quote + PMrp + quote +"","A132,41,2,3,1,1,N,"+ quote + Rate + quote +"",
                                "A62,38,2,2,1,1,N,"+ quote + PRate + quote +"","P1"};
                            PrintLabel(PrinterName, Print, i.ToString());
                        }
                        if (Cols != 0)
                        {
                            Print = new string[] {"I8,A","q812","O","JF","ZT","Q200,25","N","A796,180,2,4,1,1,N,"+quote+CompanyName+quote+"",
                                "A796,147,2,3,1,1,N,"+quote+Product+quote+"","B770,115,2,1C,4,8,39,N,"+quote+Row.Cells[(int)BarcodePrintGridColumn.ITEM_CODE].Value.ToString()+quote+"","A668,69,2,3,1,1,N,"+quote+Row.Cells[(int)BarcodePrintGridColumn.ITEM_CODE].Value.ToString()+quote+"",
                                "A796,41,2,3,1,1,N,"+quote+Mrp+quote+"","A740,38,2,2,1,1,N," + quote + PMrp + quote + "","A548,41,2,3,1,1,N,"+ quote + Rate + quote +"","A478,38,2,2,1,1,N," + quote + PRate + quote + "",
                                "P1"};
                            PrintLabel(PrinterName, Print, "");
                        }
                    }
                    else if (Size == LabelSize.ONE)
                    {
                        string[] Print = {"I8,A","q779","O","JF","ZT","Q184,25","N","A748,170,2,4,1,1,N,"+quote+CompanyName+quote+"",
                            "A748,137,2,3,1,1,N,"+quote+Product+quote+"","B722,105,2,1C,5,10,39,N,"+quote+Row.Cells[(int)BarcodePrintGridColumn.ITEM_CODE].Value.ToString()+quote+"","A580,60,2,3,1,1,N,"+quote+Row.Cells[(int)BarcodePrintGridColumn.ITEM_CODE].Value.ToString()+quote+"",
                            "A748,32,2,3,1,1,N,"+quote+Mrp+quote+"","A692,29,2,2,1,1,N,"+ quote + PMrp + quote +"","A420,32,2,3,1,1,N,"+ quote + Rate + quote +"","A350,29,2,2,1,1,N,"+ quote + PRate + quote +"",
                            "P1"};
                        PrintLabel(PrinterName, Print, "");
                    }
                }
            }
        }

        // generate barcode label for patient
        public void GenerateBarcodeLabelForPatient(long PatientId, LabelSize Size, long Qty, string PrinterName)
        {
            Patient PatientFromDB = PatientManager.Instance.GetPatientById(PatientId);
            int alength = PatientFromDB.Address != null ? PatientFromDB.Address.FullAddressInSingleLine.Length : 0;
            int plength = PatientFromDB.Name.Length;
            var AboveBarcodeName = "Name :" + PatientFromDB.Name.Substring(0, (plength <= 25) ? plength : 25) + ((plength > 25) ? ".." : "") + PatientFromDB.Guardians != null && PatientFromDB.Guardians.Count > 0 ? ("    Parent/Guardian  :" + PatientFromDB.Guardians.First().Name) : "" + "\n"
                + "PNO  :" + PatientFromDB.PatientNumber + "    Sex  :" + PatientFromDB.Gender.ToString() + "\n"
                + "DOB  :" + PatientFromDB.DateOfBirth.ToString(Global.Company.DateFormat) + "    Date :" + Global.getTransactionDate().ToString(Global.Company.DateFormat) + "\n"
                + (PatientFromDB.Address != null ? ("Address:" + PatientFromDB.Address.FullAddressInSingleLine.Substring(0, (alength <= 70) ? alength : 70) + ((alength > 70) ? ".." : "")) : "");

            string[] Address = new string[2];
            Address[0] = PatientFromDB.Address != null ? PatientFromDB.Address.FullAddressInSingleLine.Substring(0, (alength <= 42) ? alength : 42) : "";
            Address[1] = PatientFromDB.Address != null ? (alength > 42 ? PatientFromDB.Address.FullAddressInSingleLine.Substring(42, (alength - 42)) + ((alength > 80) ? ".." : "") : "") : "";
            string Parent = PatientFromDB.Guardians != null && PatientFromDB.Guardians.Count > 0 ? PatientFromDB.Guardians.First().Name : string.Empty;
            string mobile = PatientFromDB.ContactInfo != null && !string.IsNullOrEmpty(PatientFromDB.ContactInfo.Phone) ? PatientFromDB.ContactInfo.Phone : string.Empty;
            char quote = '"';
            if (Size == LabelSize.ONE)
            {
                string[] Print;
                Print = new string[]
                {
                    "I8,A",
                    "q799",
                    "O",
                    "JF",
                    "ZT",
                    "Q400,25",
                    "N",
                    "A781,372,2,4a,1,1,N,"+quote+"Name  :"+PatientFromDB.Name+quote,
                    "A781,334,2,a,1,1,N,"+quote+"PNo      : "+PatientFromDB.PatientNumber+quote,
                    "A347,334,2,a,1,1,N,"+quote+"Parent : "+Parent+quote,
                    "A781,294,2,a,1,1,N,"+quote+"Sex       : "+PatientFromDB.Gender.ToString()+quote,
                    "A347,294,2,a,1,1,N,"+quote+"DOB   : "+PatientFromDB.DateOfBirth.ToString(Global.Company.DateFormat)+quote,
                    "A781,256,2,a,1,1,N,"+quote+"Age       : "+PatientFromDB.Age.ToString()+quote,
                    "A347,256,2,a,1,1,N,"+quote+"Phone : "+mobile+quote,
                    "A781,216,2,a,1,1,N,"+quote+"RegDate: "+Global.getTransactionDate().ToString(Global.Company.DateFormat)+quote,
                    "A781,176,2,a,1,1,N,"+quote+"Address : "+Address[0]+quote,
                    "A781,143,2,a,1,1,N,"+quote+""+Address[1]+quote,
                    "B690,103,2,1C,6,12,55,N,"+quote+""+PatientFromDB.PatientNumber+quote,
                    "A517,40,2,a,1,1,N,"+quote+""+PatientFromDB.PatientNumber+quote,
                    "P1"
                };
                PrintLabel(PrinterName, Print, "");
            }
        }
        public void GenerateBarcodeLabelForPatientOPIP(long PatientId, LabelSize Size, long Qty, string PrinterName, bool IsOP)
        {
            Patient PatientFromDB = PatientManager.Instance.GetPatientById(PatientId);
            int alength = PatientFromDB.Address != null ? PatientFromDB.Address.FullAddressInSingleLine.Length : 0;
            int plength = PatientFromDB.Name.Length;
            var AboveBarcodeName = "Name :" + PatientFromDB.Name.Substring(0, (plength <= 25) ? plength : 25) + ((plength > 25) ? ".." : "") + PatientFromDB.Guardians != null && PatientFromDB.Guardians.Count > 0 ? ("    Parent/Guardian  :" + PatientFromDB.Guardians.First().Name) : "" + "\n"
                + "PNO  :" + PatientFromDB.PatientNumber + "    Sex  :" + PatientFromDB.Gender.ToString() + "\n"
                + "DOB  :" + PatientFromDB.DateOfBirth.ToString(Global.Company.DateFormat) + "    Date :" + Global.getTransactionDate().ToString(Global.Company.DateFormat) + "\n"
                + (PatientFromDB.Address != null ? ("Address:" + PatientFromDB.Address.FullAddressInSingleLine.Substring(0, (alength <= 70) ? alength : 70) + ((alength > 70) ? ".." : "")) : "");

            string[] Address = new string[2];
            Address[0] = PatientFromDB.Address != null ? PatientFromDB.Address.FullAddressInSingleLine.Substring(0, (alength <= 42) ? alength : 42) : "";
            Address[1] = PatientFromDB.Address != null ? (alength > 42 ? PatientFromDB.Address.FullAddressInSingleLine.Substring(42, (alength - 42)) + ((alength > 80) ? ".." : "") : "") : "";
            string Parent = PatientFromDB.Guardians != null && PatientFromDB.Guardians.Count > 0 ? PatientFromDB.Guardians.First().Name : string.Empty;
            string mobile = PatientFromDB.ContactInfo != null && !string.IsNullOrEmpty(PatientFromDB.ContactInfo.Phone) ? PatientFromDB.ContactInfo.Phone : string.Empty;
            char quote = '"';
            string OPIPNO = string.Empty;
            String Date = string.Empty;
            if (IsOP)
            {
                Registration OP = OpManager.Instance.GetOpNumberByPatientId(PatientId, Global.getTransactionDate());
                if (OP != null)
                {
                    OPIPNO = OP.PatientOPNumber;
                    Date = OP.DateOfRegistration.ToString(Global.Company.DateFormat);
                }
                else
                {
                    MessageBox.Show("Something went worng, Contact admin.");
                    return;
                }
            }
            else
            {
                InPatientAdmission IP = IpManager.Instance.GetAdmittedInPatientAdmissionByPatientId(PatientId);
                if (IP != null)
                {
                    OPIPNO = IP.PatientIPNumber;
                    Date = IP.DateOfAdmission.ToString(Global.Company.DateFormat);
                }
                else
                {
                    MessageBox.Show("Something went worng, Contact admin.");
                    return;
                }
            }

            if (Size == LabelSize.ONE)
            {
                string[] Print;
                if (IsOP)
                {
                    Print = new string[]
                    {
                        "I8,A",
                        "q799",
                        "O",
                        "JF",
                        "ZT",
                        "Q400,25",
                        "N",
                        "A778,362,2,4d,1,1,N," + quote + "Name:" + PatientFromDB.Name + quote,
                        "A778,327,2,a,1,1,N," + quote + "PNo   : " + PatientFromDB.PatientNumber + quote,
                        "A384,327,2,a,1,1,N," + quote + "Parent   : " + Parent + quote,
                        "A778,295,2,a,1,1,N," + quote + "Sex    : " + PatientFromDB.Gender.ToString() + quote,
                        "A384,295,2,a,1,1,N," + quote + "DOB     : " + PatientFromDB.DateOfBirth.ToString(Global.Company.DateFormat) + quote,
                        "A778,264,2,a,1,1,N," + quote + "Age    : " + PatientFromDB.Age.ToString() + quote,
                        "A384,264,2,a,1,1,N," + quote + "Phone   : " + mobile + quote,
                        "A778,227,2,a,1,1,N," + quote + "OPNo: " + OPIPNO + quote,
                        "A384,227,2,a,1,1,N," + quote + "RegDate: " + Date + quote,
                        "A773,186,2,a,1,1,N," + quote + "Address: " + Address[0] + quote,
                        "A773,153,2,a,1,1,N," + quote + Address[1] + quote,
                        "B773,103,2,1C,3,6,35,N," + quote + PatientFromDB.PatientNumber + quote,
                        "A717,61,2,a,1,1,N," + quote + PatientFromDB.PatientNumber + quote,
                        "B398,104,2,1,2,4,37,N," + quote + OPIPNO + quote,
                        "A374,60,2,a,1,1,N," + quote + OPIPNO + quote,
                        "P1"
                    };
                }
                else
                {
                    Print = new string[]
                    {
                        "I8,A",
                        "q799",
                        "O",
                        "JF",
                        "ZT",
                        "Q400,25",
                        "N",
                        "A778,362,2,4a,1,1,N,"+quote+"Name:"+PatientFromDB.Name+quote,
                        "A778,327,2,a,1,1,N,"+quote+"PNo  : "+PatientFromDB.PatientNumber+quote,
                        "A384,327,2,a,1,1,N,"+quote+"Parent : "+Parent+quote,
                        "A778,295,2,a,1,1,N,"+quote+"Sex   : "+PatientFromDB.Gender.ToString()+quote,
                        "A384,295,2,a,1,1,N,"+quote+"DOB   : "+PatientFromDB.DateOfBirth.ToString(Global.Company.DateFormat)+quote,
                        "A778,264,2,a,1,1,N,"+quote+"Age   : "+PatientFromDB.Age.ToString()+quote,
                        "A384,264,2,a,1,1,N,"+quote+"Phone : "+mobile+quote,
                        "A778,227,2,a,1,1,N,"+quote+"IPNo : "+OPIPNO+quote,
                        "A384,227,2,a,1,1,N,"+quote+"Admit Date: "+Date+quote,
                        "A773,186,2,a,1,1,N,"+quote+"Address: "+Address[0]+quote,
                        "A773,153,2,a,1,1,N,"+quote+""+Address[1]+quote,
                        "B773,103,2,1C,3,6,35,N,"+quote+""+PatientFromDB.PatientNumber+quote,
                        "A717,61,2,a,1,1,N,"+quote+""+PatientFromDB.PatientNumber+quote,
                        "B398,104,2,1,2,4,37,N,"+quote+""+OPIPNO+quote,
                        "A374,60,2,a,1,1,N,"+quote+""+OPIPNO+quote,
                        "P1"
                    };
                }
                PrintLabel(PrinterName, Print, "");
            }
        }
        public static void PrintLabel(string PrinterName, string[] Label, string sufix)
        {
            MyPrinter.SetDefaultPrinter(PrinterName);

            SaveFileDialog sfDlg = new SaveFileDialog();
            sfDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfDlg.RestoreDirectory = true;
            sfDlg.FileName = "LabelPrint" + sufix;

            var windowsTempPath = Path.GetTempPath();
            Directory.CreateDirectory(windowsTempPath + "");
            var printFilePath = String.Format("{0}\\{1}.prn", windowsTempPath, sfDlg.FileName);
            try
            {
                File.WriteAllLines(printFilePath, Label);
                if (PdfPrinter.IsOnline(PrinterName))
                {
                    //if (PdfPrinter.Print(false))
                    //{
                    string Filepath = @printFilePath;
                    string Filename = sfDlg.FileName + ".prn";
                    IPrinter printer = new Printer();
                    printer.PrintRawFile(PrinterName, Filepath, Filename);
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Printer " + PrinterName + " is offline");
                    //}
                }
                else
                {
                    MessageBox.Show("Printer " + PrinterName + " is offline");
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("The process cannot access the file"))
                {
                    Random random = new Random();
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    string randomFileName = new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray());
                    var printFilePathCatch = String.Format("{0}\\{1}{2}.prn", windowsTempPath, sfDlg.FileName, randomFileName);
                    File.WriteAllLines(printFilePathCatch, Label);

                    if (PdfPrinter.IsOnline(PrinterName))
                    {
                        //if (PdfPrinter.Print(false))
                        //{
                        string Filepath = @printFilePathCatch;
                        string Filename = sfDlg.FileName + randomFileName + ".prn";
                        IPrinter printer = new Printer();
                        printer.PrintRawFile(PrinterName, Filepath, Filename);
                        //}
                        //else
                        //{
                        //    MessageBox.Show("Printer " + PrinterName + " is offline");
                        //}
                    }
                    else
                    {
                        MessageBox.Show("Printer " + PrinterName + " is offline");
                    }
                }
            }
        }
        public void GenerateSampleCollectionBarCodeLabel(string sampleId, long qty, string printerName, int labelColumn)
        {
            char quote = '"';
            string barcodeData = sampleId;
            string displayText = sampleId;

            switch (labelColumn)
            {
                case 1:
                    // 1-column logic
                    for (long i = 0; i < qty; i++)
                    {
                        string[] printCommands = {
                    // 1-column commands
                };
                        PrintLabel(printerName, printCommands, i.ToString());
                    }
                    break;

                case 2:
                    // 2-column logic
                    // ... existing 2-column code ...
                    break;

                case 3:
                    // 3-column logic (if needed)
                    // Implement similar to 2-column but with 3 barcodes
                    break;

                default:
                    throw new ArgumentException("Invalid label column value. Use 1, 2, or 3.");
            }
        }
        // ======= SIMILAR DESIGN ======
        private void PrintThreeColumnLabels35x25mm(string printerName, Product product,
                                        string productName, string companyName,
                                        PriceInformation priceInfo, long quantity)
        {
            var rows = quantity / 3;
            var remaining = quantity % 3;

            // Print full rows (3 labels each)
            for (long i = 0; i < rows; i++)
            {
                var commands = CreateThreeLabelCommandSet35x25mm(product, productName, companyName, priceInfo);
                PrintLabel(printerName, commands, i.ToString());
            }

            // Print remaining labels (1 or 2)
            if (remaining > 0)
            {
                var commands = remaining == 1
                    ? CreateSingleLabelCommandSet35x25mm(product, productName, companyName, priceInfo)
                    : CreateDoubleLabelCommandSet35x25mm(product, productName, companyName, priceInfo);

                PrintLabel(printerName, commands, "");
            }
        }

        private string[] CreateThreeLabelCommandSet35x25mm(Product product, string productName,
                                                         string companyName, PriceInformation priceInfo)
        {
            return CombineCommands(
                GetPrinterSetupCommands35x25mm(),
                GetLabelCommand35x25mm(785, companyName, productName, product, priceInfo, 2, 1),
                GetLabelCommand35x25mm(506, companyName, productName, product, priceInfo, 2, 1),
                GetLabelCommand35x25mm(228, companyName, productName, product, priceInfo, 2, 1),
                "P1"
            );
        }

        private string[] CreateDoubleLabelCommandSet35x25mm(Product product, string productName,
                                                          string companyName, PriceInformation priceInfo)
        {
            return CombineCommands(
                GetPrinterSetupCommands35x25mm(),
                GetLabelCommand35x25mm(785, companyName, productName, product, priceInfo, 2, 1),
                GetLabelCommand35x25mm(506, companyName, productName, product, priceInfo, 2, 1),
                "P1"
            );
        }

        private string[] CreateSingleLabelCommandSet35x25mm(Product product, string productName,
                                                          string companyName, PriceInformation priceInfo)
        {
            return CombineCommands(
                GetPrinterSetupCommands35x25mm(),
                GetLabelCommand35x25mm(785, companyName, productName, product, priceInfo, 2, 1),
                "P1"
            );
        }

        private string[] GetPrinterSetupCommands35x25mm()
        {
            return new string[]
            {
                "I8,A",    // 203 DPI, font A
                "q295",    // Label height = 295 dots (~35mm)
                "O",       // Reverse printing (optional)
                "JF",      // Field justification
                "ZT",      // Thermal transfer mode
                "Q200,25", // Label width = 200 dots (~25mm)
                "N"        // Normal printing mode
            };
        }

        private string[] GetLabelCommand35x25mm(int xOffset, string companyName, string productName,
                                              Product product, PriceInformation priceInfo,
                                              int largeFontSize, int smallFontSize)
        {
            var quote = '"';
            return new string[]
            {
                // Adjusted coordinates for 35x25mm labels
                $"A{xOffset},100,2,{largeFontSize},1,1,N,{quote}{companyName}{quote}",
                $"A{xOffset},80,2,{smallFontSize},1,1,N,{quote}{productName}{quote}",
                $"B{xOffset-11},60,2,1,1,3,30,N,{quote}{product.MaterialId}{quote}",  // Smaller barcode
                $"A{xOffset-11},30,2,{smallFontSize},1,1,N,{quote}{product.MaterialId}{quote}",
                $"A{xOffset+5},20,2,{largeFontSize},1,1,N,{quote}{priceInfo.MrpText}{quote}",
                $"A{xOffset-30},15,2,{largeFontSize},1,1,N,{quote}{priceInfo.FormattedMrp}{quote}"
            };
        }
        // ===== SPECIAL DESIGN ---
        public void GenerateSpecialBarcodeLabel(long ProductId, long Qty, string PrinterName)
        {
            Product ProductFromDB = CatalogProductManager.Instance.GetProductInfoById(ProductId);
            var length = ProductFromDB.Name.Length;
            var Product = ProductFromDB.Name.Substring(0, (length <= 15) ? length : 15) + ((length > 15) ? ".." : "");
            length = Global.Company.Name.Length;
            var CompanyName = Global.Company.Name.Substring(0, (length <= 15) ? length : 15) + ((length > 15) ? ".." : "");

            // Get percentage values (assuming they're stored in ProductFromDB)
            var (retailPercent, wholesalePercent) = GetProductPercentages(ProductId);

            // Format NGL value (use retail percentage)
            // Divide and round to 1 decimal place
            int retailNGL = (int)Math.Floor(retailPercent); // Remove decimal part only
            int wholeNGL = (int)Math.Floor(wholesalePercent); // Remove decimal part only
            string nglValue = "NGL:" + wholeNGL.ToString() + "--";

            //decimal roundedNGL = Math.Round(retailPercent, 1);

            // Convert to string, remove trailing ".0" if it's whole number
            // string nglFormatted = roundedNGL % 1 == 0
            //    ? roundedNGL.ToString("0")   // e.g. 12
            //    : roundedNGL.ToString("0.0"); // e.g. 12.5

            //// Add "NGL " prefix and two trailing spaces
            //string nglValue = "NGL:" + nglFormatted + "-";


            // Get UOM from catalog
            string uom = ProductFromDB.UOM ?? ""; // Replace with actual UOM field

            // Format secret code (convert retail price digits to letters)
            decimal retailPrice = Global.Company.BusinessType == BuisnessType.Wholesale ?
                                (decimal)ProductFromDB.WholdSalePrice : (decimal)ProductFromDB.RetailPrice;
            decimal purchasePrice = (decimal)ProductFromDB.PurchasePrice;
            string secretCode = ConvertToSecretCode(purchasePrice.ToString(""));

            string lMrp = ProductFromDB.Msrp.ToString(Global.Company.PrimaryCurrency.CurrencyFormat).Replace(",", "");
            string lRate = retailPrice.ToString(Global.Company.PrimaryCurrency.CurrencyFormat).Replace(",", "");

            char quote = '"';
            string MrpLabel = "MRP:";
            string NglLabel = nglValue;
            string UomLabel = uom;

            long Rows = Qty / 3;
            long Cols = Qty % 3;
            string[] Print;

            for (long i = 0; i < Rows; i++)
            {
                Print = new string[]
                {
                    "I8,A", "q812", "O", "JF", "ZT", "Q200,25", "N",

                    // 1st Label
                    $"A785,180,2,3,1,1,N,{quote}{CompanyName}       {secretCode}{quote}",
                    $"A785,150,2,2,1,1,N,{quote}{Product}{quote}",
                    $"B774,126,2,1,1,4,41,N,{quote}{ProductFromDB.MaterialId}{quote}",
                    $"A774,69,2,2,1,1,N,{quote}{ProductFromDB.MaterialId}{quote}",
                    $"A825,35,2,2,1,1,N,{quote}{NglLabel}{quote}",
                    $"A740,35,2,2,1,1,N,{quote}{lMrp}{quote}",
                    $"A650,35,2,2,1,1,N,{quote}{UomLabel}{quote}",

                    // 2nd Label
                    $"A506,180,2,3,1,1,N,{quote}{CompanyName}       {secretCode}{quote}",
                    $"A506,150,2,2,1,1,N,{quote}{Product}{quote}",
                    $"B487,126,2,1,1,4,41,N,{quote}{ProductFromDB.MaterialId}{quote}",
                    $"A492,69,2,2,1,1,N,{quote}{ProductFromDB.MaterialId}{quote}",
                    $"A538,35,2,2,1,1,N,{quote}{NglLabel}{quote}",
                    $"A460,35,2,2,1,1,N,{quote}{lMrp}{quote}",
                    $"A370,35,2,2,1,1,N,{quote}{UomLabel}{quote}",

                    // 3rd Label
                    $"A228,180,2,3,1,1,N,{quote}{CompanyName}       {secretCode}{quote}",
                    $"A228,150,2,2,1,1,N,{quote}{Product}{quote}",
                    $"B209,126,2,1,1,4,41,N,{quote}{ProductFromDB.MaterialId}{quote}",
                    $"A214,69,2,2,1,1,N,{quote}{ProductFromDB.MaterialId}{quote}",
                    $"A260,35,2,2,1,1,N,{quote}{NglLabel}{quote}",
                    $"A180,35,2,2,1,1,N,{quote}{lMrp}{quote}",
                    $"A90,35,2,2,1,1,N,{quote}{UomLabel}{quote}",

                    "P1"
                };
                PrintLabel(PrinterName, Print, i.ToString());
            }

            if (Cols > 0)
            {
                List<string> partial = new() { "I8,A", "q812", "O", "JF", "ZT", "Q200,25", "N" };

                if (Cols >= 1)
                {
                    partial.AddRange(new string[]
                    {
                        $"A785,180,2,3,1,1,N,{quote}{CompanyName}       {secretCode}{quote}",
                        $"A785,150,2,2,1,1,N,{quote}{Product}{quote}",
                        $"B774,126,2,1,1,4,41,N,{quote}{ProductFromDB.MaterialId}{quote}",
                        $"A774,69,2,2,1,1,N,{quote}{ProductFromDB.MaterialId}{quote}",
                        $"A825,35,2,2,1,1,N,{quote}{NglLabel}{quote}",
                        $"A740,35,2,2,1,1,N,{quote}{lMrp}{quote}",
                        $"A650,35,2,2,1,1,N,{quote}{UomLabel}{quote}",
                    });
                }

                if (Cols == 2)
                {
                    partial.AddRange(new string[]
                    {
                        $"A228,180,2,3,1,1,N,{quote}{CompanyName}       {secretCode}{quote}",
                        $"A228,150,2,2,1,1,N,{quote}{Product}{quote}",
                        $"B209,126,2,1,1,4,41,N,{quote}{ProductFromDB.MaterialId}{quote}",
                        $"A214,69,2,2,1,1,N,{quote}{ProductFromDB.MaterialId}{quote}",
                        $"A260,35,2,2,1,1,N,{quote}{NglLabel}{quote}",
                        $"A180,35,2,2,1,1,N,{quote}{lMrp}{quote}",
                        $"A90,35,2,2,1,1,N,{quote}{UomLabel}{quote}",
                    });
                }

                partial.Add("P1");
                PrintLabel(PrinterName, partial.ToArray(), "");
            }
        }

        public void GenerateSpecialBarcodeLabel4Ups(long ProductId, long Qty, string PrinterName)
        {
            Product ProductFromDB = CatalogProductManager.Instance.GetProductInfoById(ProductId);
            var length = ProductFromDB.Name.Length;
            var Product = ProductFromDB.Name.Substring(0, (length <= 10) ? length : 10) + ((length > 10) ? ".." : ""); // Shorter for small labels
            length = Global.Company.Name.Length;
            var CompanyName = Global.Company.Name.Substring(0, (length <= 8) ? length : 8) + ((length > 8) ? ".." : ""); // Shorter company name

            // Get percentages and format values (same as before)
            var (retailPercent, wholesalePercent) = GetProductPercentages(ProductId);
            string nglValue = "NGL" + (retailPercent % 10 == 0 ? (retailPercent / 10).ToString("0") : (retailPercent / 10).ToString("0.#"));
            string uom = ProductFromDB.UOM ?? "";
            decimal retailPrice = Global.Company.BusinessType == BuisnessType.Wholesale ?
                                (decimal)ProductFromDB.WholdSalePrice : (decimal)ProductFromDB.RetailPrice;
            string secretCode = ConvertToSecretCode(retailPrice.ToString(""));
            string lMrp = ProductFromDB.Msrp.ToString("0.00"); // Simplified format for small labels

            char quote = '"';
            long Rows = Qty / 4; // 4 labels per row
            long Cols = Qty % 4;
            string[] Print;

            // Label dimensions (25mm x 20mm ≈ 200x160 dots at 8 dots/mm)
            int labelWidth = 200;
            int labelHeight = 160;
            int margin = 15;

            for (long i = 0; i < Rows; i++)
            {
                Print = new string[]
                {
            "^XA", // Start of label
            $"^PW{labelWidth * 4 + margin * 2}", // Total width for 4 labels
            "^LL" + (labelHeight + margin), // Label length
            "^LS0", // Label shift
            "^XZ", // End of label (temporary)

            // 1st Label (position: 0,0)
            $"^FO{margin},{margin}^A0N,15,10^FD{CompanyName}^FS",
            $"^FO{margin + 120},{margin}^A0N,10,10^FD{secretCode}^FS",
            $"^FO{margin},{margin + 20}^A0N,12,10^FD{Product}^FS",
            $"^FO{margin},{margin + 40}^BY1^BEN,30,Y,N^FD{ProductFromDB.MaterialId}^FS",
            //$"^FO{margin},{margin + 75}^A0N,10,10^FD{NglValue}^FS",
            $"^FO{margin + 60},{margin + 75}^A0N,10,10^FD{lMrp}^FS",
            $"^FO{margin + 120},{margin + 75}^A0N,10,10^FD{uom}^FS",

            // 2nd Label (position: 200,0)
            $"^FO{margin + labelWidth},{margin}^A0N,15,10^FD{CompanyName}^FS",
            // ... repeat same elements with X offset by labelWidth ...

            // 3rd Label (position: 400,0)
            // ... repeat with X offset by 2*labelWidth ...

            // 4th Label (position: 600,0)
            // ... repeat with X offset by 3*labelWidth ...

            "^XZ" // End of label
                };
                PrintLabel(PrinterName, Print, i.ToString());
            }

            // Handle partial rows (1-3 remaining labels)
            if (Cols > 0)
            {
                List<string> partial = new() { "^XA", $"^PW{labelWidth * Cols + margin * 2}", "^LL" + (labelHeight + margin), "^LS0" };

                for (int j = 0; j < Cols; j++)
                {
                    partial.AddRange(new string[]
                    {
                $"^FO{margin + j * labelWidth},{margin}^A0N,15,10^FD{CompanyName}^FS",
                        // ... add all other elements with X offset by j*labelWidth ...
                    });
                }
                partial.Add("^XZ");
                PrintLabel(PrinterName, partial.ToArray(), "");
            }
        }
        private string ConvertToSecretCode(string price)
        {
            Dictionary<char, char> secretMap = new()
            {
                {'0', 'O'}, {'1', 'M'}, {'2', 'A'}, {'3', 'R'},
                {'4', 'K'}, {'5', 'E'}, {'6', 'T'}, {'7', 'I'},
                {'8', 'N'}, {'9', 'G'}, {'.', '.'}
            };

            return string.Concat(price.Select(c => secretMap.TryGetValue(c, out var mapped) ? mapped : c));
        }

        private string FormatSecretCode(string code)
        {
            return $"^FD{code}^FS^A0N,{SECRET_CODE_FONT_SIZE},10^FB500,1,0,C^FO15,20";
        }
        
        public void GenerateSpecialBarcodeLabelv1(long ProductId, long Qty, string PrinterName)
        {
            Product ProductFromDB = CatalogProductManager.Instance.GetProductInfoById(ProductId);
            var length = ProductFromDB.Name.Length;
            var Product = ProductFromDB.Name.Substring(0, (length <= 15) ? length : 15) + ((length > 15) ? ".." : "");
            length = Global.Company.Name.Length;
            var CompanyName = Global.Company.Name.Substring(0, (length <= 15) ? length : 15) + ((length > 15) ? ".." : "");

            string lMrp = ProductFromDB.Msrp.ToString(Global.Company.PrimaryCurrency.CurrencyFormat).Replace(",", "");
            string lRate = (Global.Company.BusinessType == BuisnessType.Wholesale
                            ? ProductFromDB.WholdSalePrice
                            : ProductFromDB.RetailPrice)
                            .ToString(Global.Company.PrimaryCurrency.CurrencyFormat)
                            .Replace(",", "");

            char quote = '"';
            string MrpLabel = "MRP:";
            string RateLabel = "Rate:";

            long Rows = Qty / 3;
            long Cols = Qty % 3;
            string[] Print;

            for (long i = 0; i < Rows; i++)
            {
                Print = new string[]
                {
            "I8,A", "q812", "O", "JF", "ZT", "Q200,25", "N",

            // 1st Label
            $"A785,180,2,4,1,1,N,{quote}{CompanyName}{quote}",
            $"A785,150,2,2,1,1,N,{quote}{Product}{quote}",
            $"B774,126,2,1,1,4,41,N,{quote}{ProductFromDB.MaterialId}{quote}",
            $"A774,69,2,2,1,1,N,{quote}{ProductFromDB.MaterialId}{quote}",
            $"A790,30,2,4,1,1,N,{quote}{MrpLabel}{quote}",
            $"A735,27,2,4,1,1,N,{quote}{lMrp}{quote}",

            // 2nd Label
            $"A506,180,2,4,1,1,N,{quote}{CompanyName}{quote}",
            $"A506,150,2,2,1,1,N,{quote}{Product}{quote}",
            $"B487,126,2,1,1,4,41,N,{quote}{ProductFromDB.MaterialId}{quote}",
            $"A492,69,2,2,1,1,N,{quote}{ProductFromDB.MaterialId}{quote}",
            $"A509,30,2,4,1,1,N,{quote}{MrpLabel}{quote}",
            $"A453,27,2,4,1,1,N,{quote}{lMrp}{quote}",

            // 3rd Label
            $"A228,180,2,4,1,1,N,{quote}{CompanyName}{quote}",
            $"A228,150,2,2,1,1,N,{quote}{Product}{quote}",
            $"B209,126,2,1,1,4,41,N,{quote}{ProductFromDB.MaterialId}{quote}",
            $"A214,69,2,2,1,1,N,{quote}{ProductFromDB.MaterialId}{quote}",
            $"A231,30,2,4,1,1,N,{quote}{MrpLabel}{quote}",
            $"A175,27,2,4,1,1,N,{quote}{lMrp}{quote}",

            "P1"
                };
                PrintLabel(PrinterName, Print, i.ToString());
            }

            // Partial label if Qty % 3 != 0
            if (Cols > 0)
            {
                List<string> partial = new() { "I8,A", "q812", "O", "JF", "ZT", "Q200,25", "N" };

                if (Cols >= 1)
                {
                    partial.AddRange(new string[]
                    {
                $"A785,180,2,4,1,1,N,{quote}{CompanyName}{quote}",
                $"A785,150,2,2,1,1,N,{quote}{Product}{quote}",
                $"B774,126,2,1,1,4,41,N,{quote}{ProductFromDB.MaterialId}{quote}",
                $"A774,69,2,2,1,1,N,{quote}{ProductFromDB.MaterialId}{quote}",
                $"A790,30,2,4,1,1,N,{quote}{MrpLabel}{quote}",
                $"A735,27,2,4,1,1,N,{quote}{lMrp}{quote}"
                    });
                }

                if (Cols == 2)
                {
                    partial.AddRange(new string[]
                    {
                $"A506,180,2,4,1,1,N,{quote}{CompanyName}{quote}",
                $"A506,150,2,2,1,1,N,{quote}{Product}{quote}",
                $"B487,126,2,1,1,4,41,N,{quote}{ProductFromDB.MaterialId}{quote}",
                $"A492,69,2,2,1,1,N,{quote}{ProductFromDB.MaterialId}{quote}",
                $"A509,30,2,4,1,1,N,{quote}{MrpLabel}{quote}",
                $"A453,27,2,4,1,1,N,{quote}{lMrp}{quote}"
                    });
                }

                partial.Add("P1");
                PrintLabel(PrinterName, partial.ToArray(), "");
            }
        }

        public void GenerateCompactBarcodeLabel(long productId, string fileName, string fileExtension,
                                      bool isPrint, string location, int qty, string printerName)
        {
            // Use the proven working pattern from GenerateBarcodeA4
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, -60, -60, 35, 25);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();

                // Use your NEW compact table design but with proper null checks
                PdfPTable reportTable = CreateCompactLabelTableSafe(productId, location, qty);

                pdfDoc.Add(reportTable);
                pdfDoc.Close();

                PdfGeneration.SaveMemoryStreamBarcode(printerName, myMemoryStream, fileName, fileExtension, isPrint, PaperTypes.A4_PORTRAIT);
            }
        }

        private PdfPTable CreateCompactLabelTableSafe(long productId, string location, int qty)
        {
            // 1. Initialize a 3-column table (A4 page layout)
            PdfPTable table = new PdfPTable(3);
            table.SetWidths(new float[] { 280f, 270f, 260f }); // Column widths
            table.DefaultCell.Border = Rectangle.NO_BORDER;

            // 2. Fetch product data (with null checks)
            Product product = CatalogProductManager.Instance?.GetProductInfoById(productId);
            if (product == null || qty <= 0)
            {
                // Add a visible error cell instead of returning empty
                PdfPCell errorCell = new PdfPCell(new Phrase("Invalid product/quantity",
                    PdfDataAlignment.GetFont("Font_Normal_10_Red")));
                errorCell.Colspan = 3;
                table.AddCell(errorCell);
                return table;
            }

            // 3. Format content (as per BarCode Sample.png)
            string companyName = (Global.Company?.Name ?? "").Trim();
            string productCode = product.MaterialId ?? "N/A";
            string mrp = product.Msrp.ToString("0.00");
            string rate = (Global.Company?.BusinessType == BuisnessType.Wholesale
                          ? product.WholdSalePrice
                          : product.RetailPrice).ToString("0.00");

            // 4. Generate barcode (with fallback to text)
            iTextSharp.text.Image barcodeImage = null!;
            try
            {
                using (var stream = new MemoryStream())
                {
                    var image = BarCode.GenerateImageBarcode1(productCode);
                    if (image != null)
                    {
                        image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                        barcodeImage = iTextSharp.text.Image.GetInstance(stream.ToArray());
                        barcodeImage.ScaleAbsolute(150, 40); // Match your sample dimensions
                    }
                }
            }
            catch { /* Barcode fails silently -> fallback to text */ }

            // 5. Build labels (qty times)
            for (int i = 0; i < qty; i++)
            {
                PdfPCell cell = new PdfPCell();
                cell.MinimumHeight = 90; // Fixed height (like your sample)
                cell.Border = Rectangle.NO_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;

                // Top: Company Name (bold, single line)
                if (!string.IsNullOrEmpty(companyName))
                    cell.AddElement(new Phrase(companyName, PdfDataAlignment.GetFont("Font_Bold_12_Black")));

                // Middle: Barcode or Product Code
                if (barcodeImage != null)
                    cell.AddElement(barcodeImage);
                else
                    cell.AddElement(new Phrase(productCode, PdfDataAlignment.GetFont("Font_Normal_10_Black")));

                // Bottom: MRP + Rate (compact)
                cell.AddElement(new Phrase($"MRP: {mrp}  RATE: {rate}",
                    PdfDataAlignment.GetFont("Font_Normal_8_Black")));

                table.AddCell(cell);
            }

            return table;
        }

        public void GenerateCompactBarcodeLabelyy(long productId, string fileName, string fileExtension,
                                      bool isPrint, string location, int qty, string printerName)
        {
            // Validate all required parameters first
            if (productId <= 0) throw new ArgumentException("Invalid Product ID");
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("File name cannot be empty");
            if (string.IsNullOrWhiteSpace(fileExtension)) throw new ArgumentException("File extension cannot be empty");
            if (qty <= 0) throw new ArgumentException("Quantity must be positive");
            if (string.IsNullOrWhiteSpace(printerName)) throw new ArgumentException("Printer name cannot be empty");

            Document pdfDoc = null!;
            MemoryStream myMemoryStream = null!;

            try
            {
                myMemoryStream = new MemoryStream();

                // 1. Create document with validation
                pdfDoc = new Document(PageSize.A4, -60, -60, 35, 25);
                if (pdfDoc == null) throw new Exception("Failed to create PDF document");

                // 2. Create writer with validation
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                if (writer == null) throw new Exception("Failed to create PDF writer");

                pdfDoc.Open();

                // 3. Create table with validation
                PdfPTable reportTable = CreateCompactLabelTable(productId, location, qty);
                if (reportTable == null) throw new Exception("Failed to create label table");

                // 4. Add content to document
                pdfDoc.Add(reportTable);

                // 5. Handle printing/saving
                if (isPrint)
                {
                    if (!PdfGeneration.SaveMemoryStreamBarcode(printerName, myMemoryStream, fileName, fileExtension, true, PaperTypes.A4_PORTRAIT))
                    {
                        throw new Exception("Failed to print barcode");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log detailed error
                Debug.WriteLine($"Error in GenerateCompactBarcodeLabel: {ex}");
                throw new Exception($"Barcode generation failed: {ex.Message}", ex);
            }
            finally
            {
                // Ensure proper cleanup
                pdfDoc?.Close();
                myMemoryStream?.Dispose();
            }
        }

        private PdfPTable CreateCompactLabelTable(long productId, string location, int qty)
        {
            try
            {
                // Validate product exists
                Product product = CatalogProductManager.Instance?.GetProductInfoById(productId)!;
                if (product == null) throw new Exception("Product not found");

                // Validate company info
                if (Global.Company == null) throw new Exception("Company information not available");
                if (Global.Company.PrimaryCurrency == null) throw new Exception("Currency not set");

                // Create and configure table
                PdfPTable table = new PdfPTable(3);
                table.SetWidths(new float[] { 280f, 270f, 260f });
                table.DefaultCell.Border = Rectangle.NO_BORDER;

                // [Rest of your table creation logic...]

                return table;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating label table: {ex}");
                return null;
            }
        }
        public void GenerateCompactBarcodeLabelxx(long productId, string fileName, string fileExtension,
                                      bool isPrint, string location, int qty, string printerName)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, -60, -60, 35, 25);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();

                PdfPTable reportTable = CreateCompactLabelTable(productId, location, qty);
                pdfDoc.Add(reportTable);
                pdfDoc.Close();

                PdfGeneration.SaveMemoryStreamBarcode(printerName, myMemoryStream, fileName, fileExtension, isPrint, PaperTypes.A4_PORTRAIT);
            }
        }

        private PdfPTable CreateCompactLabelTablexx(long productId, string location, int qty)
        {
            try
            {
                Product product = CatalogProductManager.Instance.GetProductInfoById(productId);
                if (product == null) return null;

                // Validate location format
                if (string.IsNullOrEmpty(location) || !location.Contains(','))
                {
                    location = "1,1"; // Default starting position
                }

                // Format text fields - shorter for compact display
                string companyName = TruncateWithEllipsis(Global.Company.Name, 15);
                string productCode = product.MaterialId;

                // Get pricing information
                string mrp = product.Msrp.ToString(Global.Company.PrimaryCurrency.CurrencyFormat);

                // Get retail percentage (new modification)
                decimal retailPercentage = GetRetailPercentage(productId); // You'll need to implement this
                string nglCode = "NGL" + Math.Floor(retailPercentage / 10); // Gets first digit (e.g., 40% becomes NGL4)

                // Prepare pricing information
                string rate = (Global.Company.BusinessType == BuisnessType.Wholesale
                             ? product.WholdSalePrice
                             : product.RetailPrice)
                            .ToString(Global.Company.PrimaryCurrency.CurrencyFormat);

                // Parse starting location
                string[] startCell = location.Split(',');
                int printStartCellCount = (((int.Parse(startCell[0]) - 1) * 3) + int.Parse(startCell[1]));
                int printStartCell = (printStartCellCount == 0) ? 0 : (printStartCellCount - 1);

                // Create table with 3 columns
                PdfPTable table = new PdfPTable(3);
                table.SetWidths(new float[] { 280f, 270f, 260f });
                table.DefaultCell.Border = Rectangle.NO_BORDER;


                // Add empty cells for starting position
                for (int p = 0; p < printStartCell; p++)
                {
                    table.AddCell(new PdfPCell() { MinimumHeight = 97, Border = Rectangle.NO_BORDER });
                }

                // Generate barcode image once
                MemoryStream barcodeStream = new MemoryStream();
                var barcodeImage = BarCode.GenerateImageBarcode1(productCode);
                barcodeImage.Save(barcodeStream, System.Drawing.Imaging.ImageFormat.Png);
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(barcodeStream.ToArray());
                image.ScaleAbsoluteHeight(40); // Slightly taller barcode
                image.ScaleAbsoluteWidth(150); // Slightly wider barcode

                // Add label cells
                for (int i = 0; i < qty; i++)
                {
                    PdfPCell cell = new PdfPCell();
                    cell.MinimumHeight = 97;
                    cell.Border = Rectangle.NO_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.PaddingTop = 5f;

                    // Company Name (top)
                    cell.AddElement(new Phrase(companyName, PdfDataAlignment.GetFont("Font_Bold_12_Black")));
                    cell.AddElement(new Phrase("\n"));

                    // Barcode (center)
                    cell.AddElement(image);
                    cell.AddElement(new Phrase("\n"));

                    // Product Code (below barcode)
                    cell.AddElement(new Phrase(productCode, PdfDataAlignment.GetFont("Font_Normal_10_Black")));
                    cell.AddElement(new Phrase("\n"));

                    // Modified pricing line - now shows NGL code and MRP
                    cell.AddElement(new Phrase($"{nglCode}  MRP: {mrp}",
                                             PdfDataAlignment.GetFont("Font_Normal_8_Black")));

                    table.AddCell(cell);
                }

                // Fill remaining cells in last row if needed
                int addColumn = (printStartCell + qty) % 3;
                if (addColumn > 0)
                {
                    for (int i = 0; i < (3 - addColumn); i++)
                    {
                        table.AddCell(new PdfPCell() { Border = Rectangle.NO_BORDER });
                    }
                }

                return table;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating compact label table: {ex}");
                return null!;
            }
        }

        private (decimal RetailPercentage, decimal WholesalePercentage) GetProductPercentages(long productId)
        {
            var percentages = ProductSalePercentageManager.Instance.GetProductSalePercentage(productId);
            return (
                percentages.RetailMarginPercentage,
                percentages.WholesaleMarginPercentage // Assuming this property exists
            );
        }

        private decimal GetRetailPercentage(long productId)
        {
            // Implement your actual percentage lookup logic here
            // This is just a placeholder - replace with your actual database query
            var productpercentage = ProductSalePercentageManager.Instance.GetProductSalePercentage(productId);
            return productpercentage.RetailMarginPercentage; // Assuming your Product model has this property
        }
        private void PrintCompactLabels(string printerName, string companyName, string productName,
                                      string productCode, string mrp, string rate, long quantity)
        {
            var rows = quantity / 3;
            var remaining = quantity % 3;

            // Print full rows (3 labels each)
            for (long i = 0; i < rows; i++)
            {
                var commands = CreateCompactLabelCommandSet(companyName, productName, productCode, mrp, rate);
                PrintLabel(printerName, commands, i.ToString());
            }

            // Print remaining labels (1 or 2)
            if (remaining > 0)
            {
                var commands = remaining == 1
                    ? CreateSingleCompactLabelCommandSet(companyName, productName, productCode, mrp, rate)
                    : CreateDoubleCompactLabelCommandSet(companyName, productName, productCode, mrp, rate);

                PrintLabel(printerName, commands, "");
            }
        }

        private string[] CreateCompactLabelCommandSet(string companyName, string productName,
                                                    string productCode, string mrp, string rate)
        {
            return CombineCommands(
                GetCompactPrinterSetupCommands(),
                GetCompactLabelCommand(785, companyName, productName, productCode, mrp, rate),
                GetCompactLabelCommand(506, companyName, productName, productCode, mrp, rate),
                GetCompactLabelCommand(228, companyName, productName, productCode, mrp, rate),
                "P1"
            );
        }

        private string[] CreateDoubleCompactLabelCommandSet(string companyName, string productName,
                                                          string productCode, string mrp, string rate)
        {
            return CombineCommands(
                GetCompactPrinterSetupCommands(),
                GetCompactLabelCommand(785, companyName, productName, productCode, mrp, rate),
                GetCompactLabelCommand(506, companyName, productName, productCode, mrp, rate),
                "P1"
            );
        }

        private string[] CreateSingleCompactLabelCommandSet(string companyName, string productName,
                                                          string productCode, string mrp, string rate)
        {
            return CombineCommands(
                GetCompactPrinterSetupCommands(),
                GetCompactLabelCommand(785, companyName, productName, productCode, mrp, rate),
                "P1"
            );
        }

        private string[] GetCompactPrinterSetupCommands()
        {
            return new string[]
            {
                "I8,A",    // 203 DPI, font A
                "q295",    // Label height = 295 dots (~35mm)
                "O",       // Reverse printing (optional)
                "JF",      // Field justification
                "ZT",      // Thermal transfer mode
                "Q200,25", // Label width = 200 dots (~25mm)
                "N"        // Normal printing mode
            };
        }

        private string[] GetCompactLabelCommand(int xOffset, string companyName, string productName,
                                              string productCode, string mrp, string rate)
        {
            var quote = '"';
            return new string[]
            {
                // Company Name (top line)
                $"A{xOffset},100,2,2,1,1,N,{quote}{companyName}{quote}",
        
                // Product Name (middle line)
                $"A{xOffset},70,2,1,1,1,N,{quote}{productName}{quote}",
        
                // Barcode (centered)
                $"B{xOffset-30},50,2,1,1,2,30,N,{quote}{productCode}{quote}",
        
                // Product Code (below barcode)
                $"A{xOffset-30},30,2,1,1,1,N,{quote}{productCode}{quote}",
        
                // MRP and Rate (bottom line)
                $"A{xOffset},15,2,1,1,1,N,{quote}MRP:{mrp} RATE:{rate}{quote}"
                    };
        }
    }
}
