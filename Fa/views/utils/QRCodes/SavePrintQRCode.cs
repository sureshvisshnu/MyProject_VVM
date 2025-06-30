using fa.api.catalog;
using fa.model.Catalog;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa.model.Accounting.Masters;
using fa.model.OrderManagement;
using fa.api.OrderManagement;
using Fa.Utils.utils;
using fa.views.purchase;

namespace fa.views.utils.QRCodes
{
    public class SavePrintQRCode
    {
        //a4 sheet QRcode by item
        public void GenerateQRcodeA4(string BatchNo, long ProductId, string QRCodeSize, string fileName, string fileExtension, bool isPrint, string Location, int Qty, string PrinterName)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, -60, -60, 35, 25);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();

                PdfPTable ReportMainTable = MainTable(BatchNo, ProductId, QRCodeSize, Location, Qty);

                pdfDoc.Add(ReportMainTable);
                pdfDoc.Close();
                PdfGeneration.SaveMemoryStreamBarcode(PrinterName, myMemoryStream, fileName, fileExtension, isPrint, PaperTypes.A4_PORTRAIT);
            }
        }

        private PdfPTable MainTable(string BatchNo, long ProductId, string QRCodeSize, string Location, int Qty)
        {
            int Cols = 8;
            float[] widths = new float[] { 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f };
            float MinHeight = 50;
            float EmptyMinHeight = 77;
            float RowSpaceMinHeight = 10;
            float ImgHeight = 6;
            float ImgWidth = 32;

            if (QRCodeSize == "2")
            {
                Cols = 4;
                widths = new float[] { 200f, 200f, 200f, 200f };
                MinHeight = 90;
                EmptyMinHeight = 155;
                RowSpaceMinHeight = 15;
                ImgHeight = 15;
                ImgWidth = 78;
            }
            if (QRCodeSize == "1")
            {
                Cols = 4;
                widths = new float[] { 200f, 200f, 200f, 200f };
                MinHeight = 90;
                EmptyMinHeight = 130;
                RowSpaceMinHeight = 12;
                ImgHeight = 10;
                ImgWidth = 55;
            }
            string[] StartCell = Location.Split(',');
            int PrintStartCellCount = (((int.Parse(StartCell[0]) - 1) * Cols) + int.Parse(StartCell[1]));
            int PrintStartCell = (PrintStartCellCount == 0) ? 0 : (PrintStartCellCount - 1);
            int UnCompletedRowCell = (PrintStartCell % Cols);

            PdfPTable ReportMainTable = new PdfPTable(Cols);

            ReportMainTable.SetWidths(widths);

            int Rows = 0;
            int AddColumn = 0;
            int i = 0;
            int jCell = 0;
            int TotalQty = PrintStartCell;
            for (int P = 0; P < PrintStartCell; P++)
            {
                PdfPCell RowCell = new PdfPCell();
                RowCell.MinimumHeight = EmptyMinHeight;
                RowCell.BorderColor = BaseColor.WHITE;
                ReportMainTable.AddCell(RowCell);
            }

            Product ProductFromDB = CatalogProductManager.Instance.GetProductInfoById(ProductId);
            InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryBatchDetail(ProductId, BatchNo);

            int Quantity = Qty;
            TotalQty = TotalQty + Quantity;
            Rows += ((Quantity / Cols) + ((Quantity % Cols) != 0 ? 1 : 0)) + (UnCompletedRowCell != 0 ? 1 : 0);
            if (AddColumn != 0)
            {
                jCell = AddColumn;
                i = i - 1;
            }
            AddColumn = (Quantity % Cols);

            if (UnCompletedRowCell != 0)
            {
                jCell = UnCompletedRowCell;
                if (AddColumn != 0 && ((Cols - jCell) - AddColumn) > -1)
                {
                    Rows -= 1;
                }
            }
            if (jCell != 0)
            {
                int TempAddColumn = (Cols - jCell);
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
                    AddColumn = (Cols - (TempAddColumn - AddColumn));
                }
            }
            UnCompletedRowCell = 0;
            MemoryStream Barcode = new MemoryStream();
            var Image = QRCode.GenerateQRCode(ProductFromDB.MaterialId + "#" + BatchNo + "#");
            Image.Save(Barcode, System.Drawing.Imaging.ImageFormat.Png);
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Barcode.ToArray());
            image.ScaleAbsoluteHeight(ImgHeight);
            image.ScaleAbsoluteWidth(ImgWidth);
            var Temp = image;
            int alength = Global.Company.Name.Length;
            int plength = ProductFromDB.Name.Length;

            var AboveBarcodeName = " " + Global.Company.Name.Substring(0, (alength <= 25) ? alength : 25) + ((alength > 25) ? ".." : "") + "\n" + " " + ProductFromDB.Name.Substring(0, (plength <= 25) ? plength : 25) + ((plength > 25) ? ".." : "");
            var BelowBarcodeName = " " + ProductFromDB.MaterialId;
            var price = "MRP: ₹" + (InventoryBatch != null ? InventoryBatch.MaxRetailPrice : ProductFromDB.Msrp).ToString(Global.Company.PrimaryCurrency.CurrencyFormat) +
               "     Rate: ₹" + (Global.Company.BusinessType == BuisnessType.Wholesale ? (InventoryBatch != null ? InventoryBatch.WholeSalePrice : ProductFromDB.WholdSalePrice).ToString(Global.Company.PrimaryCurrency.CurrencyFormat)
               : (InventoryBatch != null ? InventoryBatch.RetailSalePrice : ProductFromDB.RetailPrice).ToString(Global.Company.PrimaryCurrency.CurrencyFormat));
            var Description = string.Empty;

            string Space = "";
            int blength = (13 - BelowBarcodeName.Length);
            for (int s = 0; s < blength; s++)
            {
                Space = Space + " ";
            }
            BelowBarcodeName = BelowBarcodeName + Space + BatchNo;

            for (i = i; i < Rows; i++)
            {
                PdfPCell RowCell = new PdfPCell();

                for (int j = jCell; j < Cols; j++)
                {
                    if (AddColumn != 0 && AddColumn <= j && i == (Rows - 1))
                    {
                        continue;
                    }
                    RowCell = new PdfPCell(new Phrase(AboveBarcodeName, PdfDataAlignment.GetFont(QRCodeSize == "0" ? "Font_Normal_Italic_6_Black" : "Font_Normal_Italic_10_Black")));
                    RowCell.MinimumHeight = MinHeight;
                    RowCell.PaddingTop = -3;
                    RowCell.UseVariableBorders = true;
                    RowCell.BorderColor = BaseColor.WHITE;
                    RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    RowCell.AddElement(new Phrase(AboveBarcodeName, PdfDataAlignment.GetFont(QRCodeSize == "0" ? "Font_Normal_Italic_6_Black" : "Font_Normal_Italic_10_Black")));

                    RowCell.PaddingTop = -3;
                    RowCell.UseVariableBorders = true;
                    RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    RowCell.AddElement(Temp);

                    RowCell.PaddingTop = -4;
                    RowCell.UseVariableBorders = true;
                    RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    RowCell.VerticalAlignment = Element.ALIGN_TOP;
                    RowCell.AddElement(new Phrase(BelowBarcodeName, PdfDataAlignment.GetFont(QRCodeSize == "0" ? "Font_Normal_Italic_6_Black" : "Font_Normal_Italic_10_Black")));

                    RowCell.PaddingTop = -3;
                    RowCell.UseVariableBorders = true;
                    RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    RowCell.AddElement(new Phrase(price, PdfDataAlignment.GetFont(QRCodeSize == "0" ? "Font_Normal_Italic_6_Black" : "Font_Normal_Italic_10_Black")));
                    ReportMainTable.AddCell(RowCell);
                }
                jCell = 0;

                if (i != (Rows - 1))
                {
                    for (int k = 0; k < Cols; k++)
                    {
                        RowCell = new PdfPCell();
                        RowCell.MinimumHeight = RowSpaceMinHeight;
                        RowCell.BorderColor = BaseColor.WHITE;
                        ReportMainTable.AddCell(RowCell);
                    }
                }
            }
            AddColumn = TotalQty % Cols;
            if (AddColumn > 0)
            {
                for (i = 0; i < (Cols - AddColumn); i++)
                {
                    PdfPCell RowCell = new PdfPCell();
                    RowCell.BorderColor = BaseColor.WHITE;
                    ReportMainTable.AddCell(RowCell);
                }
            }
            return ReportMainTable;
        }
        // a4 sheet QRcode from grid
        public void GenerateQRcodeA4(DataGridView DataGridView, string fileName, string fileExtension, bool isPrint, string Location, string PrinterName, LabelType LabelType)
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
            int Cols = 3;
            string[] StartCell = Location.Split(',');
            int PrintStartCellCount = (((int.Parse(StartCell[0]) - 1) * Cols) + int.Parse(StartCell[1]));
            int PrintStartCell = (PrintStartCellCount == 0) ? 0 : (PrintStartCellCount - 1);
            int UnCompletedRowCell = (PrintStartCell % Cols);

            PdfPTable ReportMainTable = new PdfPTable(Cols);
            float[] widths = null;
            widths = new float[] { 280f, 270f, 260f };
            ReportMainTable.SetWidths(widths);

            int Rows = 0;
            int AddColumn = 0;
            int i = 0;
            int jCell = 0;
            float EmptyMinHeight = 97;
            int TotalQty = PrintStartCell;
            for (int P = 0; P < PrintStartCell; P++)
            {
                PdfPCell RowCell = new PdfPCell();
                RowCell.MinimumHeight = EmptyMinHeight;
                RowCell.BorderColor = BaseColor.WHITE;
                ReportMainTable.AddCell(RowCell);
            }
            foreach (DataGridViewRow Row in DataGridView.Rows)
            {
                if ((bool)Row.Cells[(int)BarcodePrintGridColumn.SELECT_ITEM].Value)
                {
                    int Quantity = int.Parse(Row.Cells[(int)BarcodePrintGridColumn.QTY].Value.ToString()); ;
                    TotalQty = TotalQty + Quantity;
                    Rows += ((Quantity / Cols) + ((Quantity % Cols) != 0 ? 1 : 0)) + (UnCompletedRowCell != 0 ? 1 : 0);
                    if (AddColumn != 0)
                    {
                        jCell = AddColumn;
                        i = i - 1;
                    }
                    AddColumn = (Quantity % Cols);

                    if (UnCompletedRowCell != 0)
                    {
                        jCell = UnCompletedRowCell;
                        if (AddColumn != 0 && ((Cols - jCell) - AddColumn) > -1)
                        {
                            Rows -= 1;
                        }
                    }
                    if (jCell != 0)
                    {
                        int TempAddColumn = (Cols - jCell);
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
                            AddColumn = (Cols - (TempAddColumn - AddColumn));
                        }
                    }
                    UnCompletedRowCell = 0;
                    MemoryStream Barcode = new MemoryStream();
                    var Image = QRCode.GenerateQRCode(Row.Cells[(int)BarcodePrintGridColumn.ITEM_CODE].Value.ToString());
                    Image.Save(Barcode, System.Drawing.Imaging.ImageFormat.Png);
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Barcode.ToArray());
                    image.ScaleAbsoluteHeight(10);
                    image.ScaleAbsoluteWidth(55);
                    var Temp = image;
                    int alength = Global.Company.Name.Length;
                    int plength = Row.Cells[(int)BarcodePrintGridColumn.ITEM_NAME].Value.ToString().Length;

                    var AboveBarcodeName = " " + Global.Company.Name.Substring(0, (alength <= 25) ? alength : 25) + ((alength > 25) ? ".." : "") + "\n" + " " + Row.Cells[(int)BarcodePrintGridColumn.ITEM_NAME].Value.ToString().Substring(0, (plength <= 25) ? plength : 25) + ((plength > 25) ? ".." : "");
                    var BelowBarcodeName = " " + Row.Cells[(int)BarcodePrintGridColumn.ITEM_CODE].Value.ToString();
                    var price = "MRP: ₹" + (float.Parse(Row.Cells[(int)BarcodePrintGridColumn.MRP].Value.ToString())).ToString(Global.Company.PrimaryCurrency.CurrencyFormat) +
                       "     Rate: ₹" + (float.Parse(Row.Cells[(int)BarcodePrintGridColumn.RATE].Value.ToString())).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    var Description = string.Empty;

                    string Space = "";
                    int blength = (13 - BelowBarcodeName.Length);
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
                            RowCell = new PdfPCell(new Phrase(AboveBarcodeName, PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                            RowCell.MinimumHeight = 90;
                            RowCell.PaddingTop = -3;
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColor = BaseColor.WHITE;
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            RowCell.AddElement(new Phrase(AboveBarcodeName, PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));

                            RowCell.PaddingTop = -3;
                            RowCell.UseVariableBorders = true;
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            RowCell.AddElement(Temp);

                            RowCell.PaddingTop = -4;
                            RowCell.UseVariableBorders = true;
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            RowCell.VerticalAlignment = Element.ALIGN_TOP;
                            RowCell.AddElement(new Phrase(BelowBarcodeName, PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));

                            RowCell.PaddingTop = -3;
                            RowCell.UseVariableBorders = true;
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            RowCell.AddElement(new Phrase(price, PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                            ReportMainTable.AddCell(RowCell);
                        }
                        jCell = 0;

                        if (i != (Rows - 1))
                        {
                            for (int k = 0; k < Cols; k++)
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
            AddColumn = TotalQty % Cols;
            if (AddColumn > 0)
            {
                for (i = 0; i < (Cols - AddColumn); i++)
                {
                    PdfPCell RowCell = new PdfPCell();
                    RowCell.BorderColor = BaseColor.WHITE;
                    ReportMainTable.AddCell(RowCell);
                }
            }
            return ReportMainTable;
        }
        // QR code label print for single product
        public void GenerateQRcodeLabel(long ProductId, string BatchNo, LabelSize Size, long Qty, string PrinterName)
        {
            Product ProductFromDB = CatalogProductManager.Instance.GetProductInfoById(ProductId);
            if (ProductFromDB != null)
            {
                InventoryBatch BatchDetails = InventoryLocationManager.Instance.GetInventoryBatchDetail(ProductFromDB.Id, BatchNo);
                if (BatchDetails != null)
                {
                    var length = ProductFromDB.Name.Length;
                    var Product = ProductFromDB.Name.Substring(0, (length <= 15) ? length : 15) + ((length > 15) ? ".." : "");
                    length = Global.Company.Name.Length;
                    var CompanyName = Global.Company.Name.Substring(0, (length <= 15) ? length : 15) + ((length > 15) ? ".." : "");


                    char quote = '"';
                    string Mrp = "Mrp:";
                    string Rate = "Rs:";
                    string PMrp = BatchDetails.MaxRetailPrice.ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    string PRate = (Global.Company.CompanySalesSetup.PriceType == PriceType.Wholesale ? BatchDetails.WholeSalePrice : Global.Company.CompanySalesSetup.PriceType == PriceType.Retail ? BatchDetails.RetailSalePrice : BatchDetails.MaxRetailPrice).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    string MatId = quote + ProductFromDB.MaterialId + "#" + BatchNo + "#" + quote;
                    if (Size == LabelSize.THREE)
                    {
                        long Rows = Qty;
                        long Cols = Qty % 3;
                        string[] Print;
                        for (long i = 0; i < Rows; i++)
                        {
                            Print = new string[] {"I8,A","q812","O","JF","ZT","Q200,25","N","A803,185,2,4,1,1,N," + quote + CompanyName + quote + "",
                            "A803,153,2,2,1,1,N," + quote + Product + quote + "","A803,30,2,2,1,1,N," + quote + Mrp + quote + "","A750,27,2,1,1,1,N," + quote + PMrp + quote + "",
                            "A600,10,1,2,1,1,N," + quote + Rate + quote + "","A600,50,1,1,1,1,N," + quote + PRate + quote + "","b689,43,Q,m2,s4,eL," + MatId+ "",
                            "A801,55,1,1,1,1,N," + quote + ProductFromDB.MaterialId + quote + "","A678,55,1,1,1,1,N," + quote + BatchNo + quote + "",
                            "A526,185,2,4,1,1,N,"+ quote + CompanyName + quote +"",
                            "A526,153,2,2,1,1,N,"+ quote + Product + quote +"","A526,30,2,2,1,1,N,"+quote+Mrp+quote+"","A473,27,2,1,1,1,N,"+ quote + PMrp + quote + "","A320,10,1,2,1,1,N,"+ quote + Rate + quote +"",
                            "A320,50,1,1,1,1,N,"+ quote + PRate + quote +"","b412,43,Q,m2,s4,eL,"+ MatId +"","A524,55,1,1,1,1,N,"+ quote + ProductFromDB.MaterialId + quote +"","A401,55,1,1,1,1,N,"+ quote + BatchNo + quote +"",
                            "A248,185,2,4,1,1,N,"+ quote + CompanyName + quote +"",
                            "A248,153,2,2,1,1,N,"+ quote + Product + quote +"","A248,30,2,2,1,1,N,"+quote+Mrp+quote+"","A195,27,2,1,1,1,N,"+ quote + PMrp + quote + "",
                            "A40,10,1,2,1,1,N,"+ quote + Rate + quote +"","A40,50,1,1,1,1,N,"+ quote + PRate + quote +"","b134,43,Q,m2,s4,eL,"+ MatId +"",
                            "A246,55,1,1,1,1,N,"+ quote + ProductFromDB.MaterialId + quote +"","A123,55,1,1,1,1,N,"+ quote + BatchNo + quote +"","P1"};
                            SavePrintBarcode.PrintLabel(PrinterName, Print, i.ToString());

                        }
                        if (Cols == 1)
                        {
                            Print = new string[] { "I8,A","q812","O","JF","ZT","Q200,25","N","A803,185,2,4,1,1,N," + quote + CompanyName + quote + "",
                            "A803,153,2,2,1,1,N," + quote + Product + quote + "","A803,30,2,2,1,1,N," + quote + Mrp + quote + "","A750,27,2,1,1,1,N," + quote + PMrp + quote + "",
                            "A600,10,1,2,1,1,N," + quote + Rate + quote + "","A600,50,1,1,1,1,N," + quote + PRate + quote + "","b689,43,Q,m2,s4,eL," + MatId+ "",
                            "A801,55,1,1,1,1,N," + quote + ProductFromDB.MaterialId + quote + "","A678,55,1,1,1,1,N," + quote + BatchNo + quote + "","P1"};
                            SavePrintBarcode.PrintLabel(PrinterName, Print, "");

                        }
                        else if (Cols == 2)
                        {
                            Print = new string[] {"I8,A","q812","O","JF","ZT","Q200,25","N","A803,185,2,4,1,1,N," + quote + CompanyName + quote + "",
                            "A803,153,2,2,1,1,N," + quote + Product + quote + "","A803,30,2,2,1,1,N," + quote + Mrp + quote + "","A750,27,2,1,1,1,N," + quote + PMrp + quote + "",
                            "A600,10,1,2,1,1,N," + quote + Rate + quote + "","A600,50,1,1,1,1,N," + quote + PRate + quote + "","b689,43,Q,m2,s4,eL," + MatId+ "",
                            "A801,55,1,1,1,1,N," + quote + ProductFromDB.MaterialId + quote + "","A678,55,1,1,1,1,N," + quote + BatchNo + quote + "","A526,185,2,4,1,1,N,"+ quote + CompanyName + quote +"",
                            "A526,153,2,2,1,1,N,"+ quote + Product + quote +"","A526,30,2,2,1,1,N,"+quote+Mrp+quote+"","A473,27,2,1,1,1,N,"+ quote + PMrp + quote + "","A320,10,1,2,1,1,N,"+ quote + Rate + quote +"",
                            "A320,50,1,1,1,1,N,"+ quote + PRate + quote +"","b412,43,Q,m2,s4,eL,"+ MatId +"","A524,55,1,1,1,1,N,"+ quote + ProductFromDB.MaterialId + quote +"","A401,55,1,1,1,1,N,"+ quote + BatchNo + quote +"","P1"};
                            SavePrintBarcode.PrintLabel(PrinterName, Print, "");
                        }

                    }
                    else if (Size == LabelSize.TWO)
                    {
                        long Rows = Qty / 2;
                        long Cols = Qty % 2;
                        string[] Print;
                        for (long i = 0; i < Rows; i++)
                        {
                            Print = new string[] { "I8,A","q812","O","JF","ZT","Q200,25","N","A796,180,2,4,1,1,N," + quote + CompanyName + quote + "",
                            "A796,151,2,3,1,1,N," + quote + Product + quote + "","A796,27,2,2,1,1,N," + quote + Mrp + quote + "","A740,26,2,1,1,1,N," + quote + PMrp + quote + "",
                            "A655,27,2,2,1,1,N," + quote + Rate + quote + "","A590,26,2,1,1,1,N," + quote + PRate + quote + "","b682,38,Q,m2,s4,eL," + MatId+ "",
                            "A794,50,1,1,1,1,N," + quote + ProductFromDB.MaterialId + quote + "","A671,50,1,1,1,1,N," + quote + BatchNo + quote + "","A380,180,2,4,1,1,N,"+ quote + CompanyName + quote +"",
                            "A380,151,2,3,1,1,N,"+ quote + Product + quote +"","A380,27,2,2,1,1,N,"+ quote + Mrp + quote +"","A324,26,2,1,1,1,N,"+ quote + PMrp + quote +"","A239,27,2,2,1,1,N,"+ quote + Rate + quote +"",
                            "A174,26,2,1,1,1,N,"+ quote + PRate + quote +"","b266,38,Q,m2,s4,eL,"+ MatId +"","A378,50,1,1,1,1,N,"+ quote + ProductFromDB.MaterialId + quote +"",
                            "A255,50,1,1,1,1,N,"+ quote + BatchNo + quote +"","P1"};

                            SavePrintBarcode.PrintLabel(PrinterName, Print, i.ToString());
                        }
                        if (Cols != 0)
                        {
                            Print = new string[] {"I8,A","q812","O","JF","ZT","Q200,25","N","A796,180,2,4,1,1,N," + quote + CompanyName + quote + "",
                            "A796,151,2,3,1,1,N," + quote + Product + quote + "","A796,27,2,2,1,1,N," + quote + Mrp + quote + "","A740,26,2,1,1,1,N," + quote + PMrp + quote + "",
                            "A655,27,2,2,1,1,N," + quote + Rate + quote + "","A590,26,2,1,1,1,N," + quote + PRate + quote + "","b682,38,Q,m2,s4,eL," + MatId + "",
                            "A794,50,1,1,1,1,N," + quote + ProductFromDB.MaterialId + quote + "","A671,50,1,1,1,1,N," + quote + BatchNo + quote + "","P1"};

                            SavePrintBarcode.PrintLabel(PrinterName, Print, "");
                        }
                    }
                    else if (Size == LabelSize.ONE)
                    {
                        for (long i = 0; i < Qty; i++)
                        {
                            string[] Print = {"I8,A","q779","O","JF","ZT","Q184,25","N","A748,175,2,4,1,1,N,"+quote+CompanyName+quote+"",
                            "A748,143,2,3,1,1,N,"+quote+Product+quote+"","A748,29,2,3,1,1,N,"+quote+Mrp+quote+"","A692,26,2,2,1,1,N,"+ quote + PMrp + quote +"",
                            "A602,29,2,3,1,1,N," + quote + Rate + quote + "","A532,26,2,2,1,1,N,"+ quote + PRate + quote +"","b621,32,Q,m2,s4,eL,"+ MatId +"",
                            "A746,44,1,1,1,1,N," + quote + ProductFromDB.MaterialId + quote + "","A599,44,1,1,1,1,N,"+ quote + BatchNo + quote +"","P1"};

                            SavePrintBarcode.PrintLabel(PrinterName, Print, "");
                        }
                    }
                }
            }
        }
        // QR code label print for multiple product
        public void GenerateQRcodeLabel(DataGridView DataGridView, LabelSize Size, string PrinterName, LabelType LabelType)
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
                    string BatchNo = string.Empty;
                    string MeterialId = Row.Cells[(int)BarcodePrintGridColumn.ITEM_CODE].Value.ToString();
                    BatchNo = Row.Cells[(int)BarcodePrintGridColumn.BATCH_NO].Value.ToString();
                    var length = Row.Cells[(int)BarcodePrintGridColumn.ITEM_NAME].Value.ToString().Length;
                    var Product = Row.Cells[(int)BarcodePrintGridColumn.ITEM_NAME].Value.ToString().Substring(0, (length <= 15) ? length : 15) + ((length > 15) ? ".." : "");
                    length = Global.Company.Name.Length;
                    var CompanyName = Global.Company.Name.Substring(0, (length <= 15) ? length : 15) + ((length > 15) ? ".." : "");

                    char quote = '"';
                    string Mrp = "Mrp:";
                    string Rate = "Rs:";
                    string PMrp = double.Parse(Row.Cells[(int)BarcodePrintGridColumn.MRP].Value.ToString()).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    string PRate = double.Parse(Row.Cells[(int)BarcodePrintGridColumn.RATE].Value.ToString()).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    string MatId = quote + MeterialId + quote;
                    string DisMatId = LabelType == LabelType.QRCODE ? quote + MeterialId + quote : quote + MeterialId + quote;
                    long Qty = long.Parse(Row.Cells[(int)BarcodePrintGridColumn.QTY].Value.ToString());
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
                            SavePrintBarcode.PrintLabel(PrinterName, Print, i.ToString());
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
                                SavePrintBarcode.PrintLabel(PrinterName, Print, "");
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
                                SavePrintBarcode.PrintLabel(PrinterName, Print, "");
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
                            SavePrintBarcode.PrintLabel(PrinterName, Print, i.ToString());
                        }
                        if (Cols != 0)
                        {
                            Print = new string[] {"I8,A","q812","O","JF","ZT","Q200,25","N","A796,180,2,4,1,1,N,"+quote+CompanyName+quote+"",
                            "A796,147,2,3,1,1,N,"+quote+Product+quote+"","B770,115,2,1C,4,8,39,N,"+quote+Row.Cells[(int)BarcodePrintGridColumn.ITEM_CODE].Value.ToString()+quote+"","A668,69,2,3,1,1,N,"+quote+Row.Cells[(int)BarcodePrintGridColumn.ITEM_CODE].Value.ToString()+quote+"",
                            "A796,41,2,3,1,1,N,"+quote+Mrp+quote+"","A740,38,2,2,1,1,N," + quote + PMrp + quote + "","A548,41,2,3,1,1,N,"+ quote + Rate + quote +"","A478,38,2,2,1,1,N," + quote + PRate + quote + "",
                            "P1"};
                            SavePrintBarcode.PrintLabel(PrinterName, Print, "");
                        }
                    }
                    else if (Size == LabelSize.ONE)
                    {
                        string[] Print = {"I8,A","q779","O","JF","ZT","Q184,25","N","A748,170,2,4,1,1,N,"+quote+CompanyName+quote+"",
                        "A748,137,2,3,1,1,N,"+quote+Product+quote+"","B722,105,2,1C,5,10,39,N,"+quote+Row.Cells[(int)BarcodePrintGridColumn.ITEM_CODE].Value.ToString()+quote+"","A580,60,2,3,1,1,N,"+quote+Row.Cells[(int)BarcodePrintGridColumn.ITEM_CODE].Value.ToString()+quote+"",
                        "A748,32,2,3,1,1,N,"+quote+Mrp+quote+"","A692,29,2,2,1,1,N,"+ quote + PMrp + quote +"","A420,32,2,3,1,1,N,"+ quote + Rate + quote +"","A350,29,2,2,1,1,N,"+ quote + PRate + quote +"",
                        "P1"};
                        SavePrintBarcode.PrintLabel(PrinterName, Print, "");
                    }
                }
            }
        }
    }
}
