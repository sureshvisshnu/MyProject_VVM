using fa.api.utils;
using fa.model.OrderManagement;
using fa.views.utils.Common;
using fa.views.utils.Report.Sale;
using Fa.report.Purchase;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioForge.Libs.NAudio.CoreAudioApi;
using BorderStyle = NPOI.SS.UserModel.BorderStyle;
using HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment;

namespace fa.views.utils.Report.Purchase
{
    class PurchaseGstrReportSaveExcel
    {
        public void SaveGstrReportToExcel(PurchaseGstrReportBtoB lPurchaseGstrReportBtoB)
        {
            SaveFileDialog sfDlg = new SaveFileDialog();
            Cursor.Current = Cursors.WaitCursor;
            int i = 0;

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Purchase Gstr Report Data");

            HSSFCellStyle hStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            hStyle.FillPattern = FillPattern.SolidForeground;
            hStyle.FillForegroundColor = HSSFColor.SkyBlue.Index;
            hStyle.BorderBottom = BorderStyle.Medium;
            HSSFFont font1 = (HSSFFont)workbook.CreateFont();
            font1.Boldweight = (short)FontBoldWeight.Bold;
            hStyle.SetFont(font1);
            hStyle.Alignment = HorizontalAlignment.Left;

            HSSFCellStyle kStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            kStyle.FillPattern = FillPattern.SolidForeground;
            kStyle.FillForegroundColor = HSSFColor.Tan.Index;
            HSSFFont font = (HSSFFont)workbook.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;
            kStyle.SetFont(font);
            kStyle.Alignment = HorizontalAlignment.Left;

            HSSFCellStyle rStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            rStyle.Alignment = HorizontalAlignment.Center;

            HSSFCellStyle leftAlignedStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            leftAlignedStyle.Alignment = HorizontalAlignment.Left;

            HSSFCellStyle rightAlignedStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            rightAlignedStyle.Alignment = HorizontalAlignment.Right;

            for (i = 1; i < 5; i++)
            {
                if (i == 1)
                {
                    IRow headerRow = sheet.CreateRow(i++);
                    headerRow.CreateCell(0).SetCellValue("Summary For B2B(4)");
                    headerRow.GetCell(0).CellStyle = hStyle;
                }
                if (i == 2)
                {
                    IRow headerRow = sheet.CreateRow(i++);
                    headerRow.CreateCell(0).SetCellValue("No. of Recipients");
                    headerRow.CreateCell(1).SetCellValue("");
                    headerRow.CreateCell(2).SetCellValue("No. of Invoices");
                    headerRow.CreateCell(3).SetCellValue("");
                    headerRow.CreateCell(4).SetCellValue("Total Invoice Value");
                    headerRow.CreateCell(5).SetCellValue("");
                    headerRow.CreateCell(6).SetCellValue("");
                    headerRow.CreateCell(7).SetCellValue("");
                    headerRow.CreateCell(8).SetCellValue("");
                    headerRow.CreateCell(9).SetCellValue("");
                    headerRow.CreateCell(10).SetCellValue("Total Taxable Value");
                    headerRow.CreateCell(11).SetCellValue("Total Cess");
                    for (int columnIndex = 0; columnIndex <= 11; columnIndex++)
                    {
                        var cell = headerRow.GetCell(columnIndex);
                        if (cell != null)
                        {
                            cell.CellStyle = hStyle;
                        }
                    }
                }
                if (i == 3 && lPurchaseGstrReportBtoB.LineItems.Count > 0)
                {
                    IRow Row = sheet.CreateRow(i++);
                    Row.CreateCell(0).SetCellValue(lPurchaseGstrReportBtoB.ReceipientCount);
                    Row.CreateCell(2).SetCellValue(lPurchaseGstrReportBtoB.InvoiceCount);

                    ICell dateCell = Row.CreateCell(4);
                    dateCell.SetCellValue(lPurchaseGstrReportBtoB.LineItems.Sum(x => x.value).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));

                    ICell dateCell1 = Row.CreateCell(10);
                    dateCell1.SetCellValue(lPurchaseGstrReportBtoB.LineItems.Sum(x => x.TaxableValue).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));

                    ICell dateCell2 = Row.CreateCell(11);
                    dateCell2.SetCellValue(lPurchaseGstrReportBtoB.LineItems.Sum(x => x.CessAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                   
                    foreach (var cell in Row.Cells)
                    {
                        cell.CellStyle = rStyle;
                    }
                    Row.GetCell(0).CellStyle = leftAlignedStyle;
                    Row.GetCell(2).CellStyle = rightAlignedStyle;
                    Row.GetCell(4).CellStyle = rightAlignedStyle;
                    Row.GetCell(10).CellStyle = rightAlignedStyle;
                    Row.GetCell(11).CellStyle = rightAlignedStyle;
                }
                if (i == 4)
                {
                    IRow headerRow = sheet.CreateRow(i++);
                    headerRow.CreateCell(0).SetCellValue("GSTIN/UIN of Recipient");
                    headerRow.CreateCell(1).SetCellValue("Supplier Name");
                    headerRow.CreateCell(2).SetCellValue("Invoice Number");
                    headerRow.CreateCell(3).SetCellValue("Invoice date");
                    headerRow.CreateCell(4).SetCellValue("Invoice Value");
                    headerRow.CreateCell(5).SetCellValue("Place Of Supply");
                    headerRow.CreateCell(6).SetCellValue("Reverse Charge");
                    headerRow.CreateCell(7).SetCellValue("Invoice Type");
                    headerRow.CreateCell(8).SetCellValue("E-Commerce GSTIN");
                    headerRow.CreateCell(9).SetCellValue("Rate");
                    headerRow.CreateCell(10).SetCellValue("Taxable Value");
                    headerRow.CreateCell(11).SetCellValue("Cess Amount");
                    foreach (var cell in headerRow.Cells)
                    {
                        cell.CellStyle = kStyle;
                    }
                }
            }

            int rowIndex = 5;

            foreach (PurchaseGstrReportBtoBLineItem LineItem in lPurchaseGstrReportBtoB.LineItems)
            {
                IRow dataRow = sheet.CreateRow(rowIndex++);
                dataRow.CreateCell(0).SetCellValue(LineItem.Gstn);
                dataRow.CreateCell(1).SetCellValue(LineItem.Name); 
                dataRow.CreateCell(2).SetCellValue(LineItem.Invoice);
                DateTime FromDate = LineItem.Date;
                ICell dateCell = dataRow.CreateCell(3);
                dateCell.SetCellValue(FromDate.ToString(DateUtils.FormatDate(FromDate, Global.Company.DateFormat)));

                ICell dateCell0 = dataRow.CreateCell(4);
                dateCell0.SetCellValue(LineItem.value.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));

                dataRow.CreateCell(5).SetCellValue(Global.Company.Address.FullAddressInSingleLine);
                dataRow.CreateCell(6).SetCellValue(LineItem.RevCharge);
                dataRow.CreateCell(7).SetCellValue(LineItem.Type);
                dataRow.CreateCell(8).SetCellValue("");
                dataRow.CreateCell(9).SetCellValue(LineItem.Rate.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
            
                ICell dateCell1 = dataRow.CreateCell(10);
                dateCell1.SetCellValue(LineItem.TaxableValue.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));

                ICell dateCell2 = dataRow.CreateCell(11);
                dateCell2.SetCellValue(LineItem.CessAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));


                dataRow.GetCell(0).CellStyle = leftAlignedStyle;
                dataRow.GetCell(1).CellStyle = leftAlignedStyle;
                dataRow.GetCell(2).CellStyle = leftAlignedStyle;
                dataRow.GetCell(3).CellStyle = rightAlignedStyle;
                dataRow.GetCell(4).CellStyle = rightAlignedStyle;
                dataRow.GetCell(5).CellStyle = leftAlignedStyle;
                dataRow.GetCell(6).CellStyle = leftAlignedStyle;
                dataRow.GetCell(7).CellStyle = leftAlignedStyle;
                dataRow.GetCell(8).CellStyle = rightAlignedStyle;
                dataRow.GetCell(9).CellStyle = rightAlignedStyle;
                dataRow.GetCell(10).CellStyle = rightAlignedStyle;
                dataRow.GetCell(11).CellStyle = rightAlignedStyle;
            }

            for (int columnIndex = 0; columnIndex < 12; columnIndex++)
            {
                sheet.AutoSizeColumn(columnIndex);
            }

            sfDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfDlg.Filter = "Excel Workbook (*.xls;*.xlsx)|*.xls;*.xlsx|All Files (*.*)|*.*";
            sfDlg.RestoreDirectory = true;
            sfDlg.FileName = "Purchase Gstr Report " + DateTime.Now.ToString("yyyy-MM-dd").Replace("/", "-");

            if (sfDlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream fs = new FileStream(sfDlg.FileName, FileMode.Create))
                    {
                        workbook.Write(fs);
                    }
                    MessageBox.Show("File saved successfully!");

                    if (MessageBox.Show("Do you want to open the file?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(new ProcessStartInfo { FileName = sfDlg.FileName, UseShellExecute = true });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to save the file: " + ex.Message);
                }
            }
            Cursor.Current = Cursors.Default;
        }
    }
}
