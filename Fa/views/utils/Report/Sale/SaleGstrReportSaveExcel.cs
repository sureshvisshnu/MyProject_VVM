using fa.api.utils;
using fa.views.utils.Common;
using Fa.report.sales;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;
using System.Diagnostics;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;
using System.Reflection.Metadata;
using NPOI.HSSF.Util;
using BorderStyle = NPOI.SS.UserModel.BorderStyle;
using HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment;

namespace fa.views.utils.Report.Sale
{
    enum GstrReportTypeTableColumn
    {
        HSN, BTOB, BTOCL, BTOCS, BTOBA
    }
    public class SaleGstrReportSaveExcel
    {
        private HSSFWorkbook workbook;
        private ISheet sheet;
        private ISheet sheet1;
        private ISheet sheet2;
        private ISheet sheet3;
        private ComboBox ComboBoxReportType;


        public void SaveGstrReportToExcel(SalesGstrReportHsn lSalesGstrReportHsn, SalesGstrReportBtoB lSalesGstrReportBtoB, SalesGstrReportBtoCL lSalesGstrReportBtoCL, SalesGstrReportBtoCS lSalesGstrReportBtoCS, SalesGstrReportBtoBA lSalesGstrReportBtoBA, int index)
        {
            
            SaveFileDialog sfDlg = new SaveFileDialog();
            Cursor.Current = Cursors.WaitCursor;
            HSSFWorkbook workbook = new HSSFWorkbook();

            int i = 0;
            ISheet sheet = workbook.CreateSheet("Hsn Summary");

            HSSFCellStyle hStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            hStyle.FillPattern = FillPattern.SolidForeground;
            hStyle.FillForegroundColor = HSSFColor.SkyBlue.Index;
            hStyle.BorderBottom = BorderStyle.Medium;
            HSSFFont font1 = (HSSFFont)workbook.CreateFont();
            font1.Boldweight = (short)FontBoldWeight.Bold;
            hStyle.SetFont(font1);
            

            HSSFCellStyle kStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            kStyle.FillPattern = FillPattern.SolidForeground;
            kStyle.FillForegroundColor = HSSFColor.Tan.Index;
            HSSFFont font = (HSSFFont)workbook.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;
            kStyle.SetFont(font);

            HSSFCellStyle leftAlignedStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            leftAlignedStyle.Alignment = HorizontalAlignment.Left;

            HSSFCellStyle rightAlignedStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            rightAlignedStyle.Alignment = HorizontalAlignment.Right;


            for (i = 1; i < 5; i++)
            {
                if (i == 1)
                {
                        IRow headerRow = sheet.CreateRow(i++);
                        headerRow.CreateCell(0).SetCellValue("Summary For HSN");
                        headerRow.GetCell(0).CellStyle = hStyle;
                }
                if (i == 2)
                {
                        IRow headerRow = sheet.CreateRow(i++);
                        headerRow.CreateCell(0).SetCellValue("No. of HSN");
                        headerRow.CreateCell(1).SetCellValue("");
                        headerRow.CreateCell(2).SetCellValue("");
                        headerRow.CreateCell(3).SetCellValue("");
                        headerRow.CreateCell(4).SetCellValue("Total Value");
                        headerRow.CreateCell(5).SetCellValue("Total Taxable Value");
                        headerRow.CreateCell(6).SetCellValue("Total Integrated Tax");
                        headerRow.CreateCell(7).SetCellValue("Total Central Tax");
                        headerRow.CreateCell(8).SetCellValue("Total State/UT Tax");
                        headerRow.CreateCell(9).SetCellValue("Total Cess");
                            foreach (var cell in headerRow.Cells)
                            {
                              cell.CellStyle = hStyle;
                            }               
                }
                if (i == 3 && lSalesGstrReportHsn.LineItems.Count > 0)
                {     
                        IRow Row = sheet.CreateRow(i++);
                        Row.CreateCell(0).SetCellValue(lSalesGstrReportHsn.LineItems.Count);
                        Row.CreateCell(4).SetCellValue(lSalesGstrReportHsn.LineItems.Sum(x => x.TotalValue).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                        Row.CreateCell(5).SetCellValue(lSalesGstrReportHsn.LineItems.Sum(x => x.TaxableValue).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                        Row.CreateCell(6).SetCellValue(0.00.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                        Row.CreateCell(7).SetCellValue(lSalesGstrReportHsn.LineItems.Sum(x => x.CentralTaxAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                        Row.CreateCell(8).SetCellValue(lSalesGstrReportHsn.LineItems.Sum(x => x.StateTaxAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                        Row.CreateCell(9).SetCellValue(0.00.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                    for (int columnIndex = 0; columnIndex < 10; columnIndex++)
                    {
                        ICell cell = Row.GetCell(columnIndex);
                        if (cell != null)
                        {
                            if (columnIndex == 0 || columnIndex == 4 || columnIndex == 5 || columnIndex == 6 || columnIndex == 7 || columnIndex == 8 || columnIndex == 9)
                            {
                                cell.CellStyle = rightAlignedStyle;
                            }
                            else
                            {
                                cell.CellStyle = leftAlignedStyle;
                            }
                        }
                    }

                }
                    if (i == 4)
                    {
                        IRow headerRow = sheet.CreateRow(i++);
                        headerRow.CreateCell(0).SetCellValue("HSN");
                        headerRow.CreateCell(1).SetCellValue("Description");
                        headerRow.CreateCell(2).SetCellValue("UQC");
                        headerRow.CreateCell(3).SetCellValue("Total Quantity");
                        headerRow.CreateCell(4).SetCellValue("Total Value");
                        headerRow.CreateCell(5).SetCellValue("Taxable Value");
                        headerRow.CreateCell(6).SetCellValue("Integrated Tax Amount");
                        headerRow.CreateCell(7).SetCellValue("Central Tax Amount");
                        headerRow.CreateCell(8).SetCellValue("State/UT Tax Amount");
                        headerRow.CreateCell(9).SetCellValue("Cess Amount");
                           foreach (var cell in headerRow.Cells)
                           {
                              cell.CellStyle = kStyle;
                           }
                    }
            }
            foreach (SalesGstrReportHsnLineItem LineItem in lSalesGstrReportHsn.LineItems)
            {
                    IRow Row = sheet.CreateRow(i++);
                    Row.CreateCell(0).SetCellValue(LineItem.Hsn);
                    Row.CreateCell(1).SetCellValue(LineItem.Description);
                    Row.CreateCell(2).SetCellValue(LineItem.Uqc);
                    Row.CreateCell(3).SetCellValue(LineItem.TotalQuantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision)));
                    Row.CreateCell(4).SetCellValue(LineItem.TotalValue.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                    Row.CreateCell(5).SetCellValue(LineItem.TaxableValue.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                    Row.CreateCell(6).SetCellValue(0.00.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                    Row.CreateCell(7).SetCellValue(LineItem.CentralTaxAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                    Row.CreateCell(8).SetCellValue(LineItem.StateTaxAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                    Row.CreateCell(9).SetCellValue(LineItem.CessAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                for (int columnIndex = 0; columnIndex < 10; columnIndex++)
                {
                    ICell cell = Row.GetCell(columnIndex);
                    if (cell != null)
                    {
                        if (columnIndex == 3 || columnIndex == 4 || columnIndex == 5 || columnIndex == 6 || columnIndex == 7 || columnIndex == 8 || columnIndex == 9)
                        {
                            cell.CellStyle = rightAlignedStyle;
                        }
                        else
                        {
                            cell.CellStyle = leftAlignedStyle;
                        }
                    }
                }

            }

            for (int columnIndex = 0; columnIndex < 10; columnIndex++)
            {
                sheet.AutoSizeColumn(columnIndex);
            }

            // Start a new worksheet for B2B data

            ISheet sheet1 = workbook.CreateSheet("Business To Business");
          
                for (i = 1; i < 5; i++)
                {
                IRow headerRow1 = sheet1.CreateRow(i);
                if (i == 1)
                    {
                        headerRow1.CreateCell(0).SetCellValue("Summary For B2B");
                    }
                if (i == 2)
                {
                       
                        headerRow1.CreateCell(0).SetCellValue("No. of Recipients");
                        headerRow1.CreateCell(1).SetCellValue("");
                        headerRow1.CreateCell(2).SetCellValue("No. of Invoices");
                        headerRow1.CreateCell(3).SetCellValue("");
                        headerRow1.CreateCell(4).SetCellValue("Total Invoice Value");
                        headerRow1.CreateCell(5).SetCellValue("");
                        headerRow1.CreateCell(6).SetCellValue("");
                        headerRow1.CreateCell(7).SetCellValue("");
                        headerRow1.CreateCell(8).SetCellValue("");
                        headerRow1.CreateCell(9).SetCellValue("");
                        headerRow1.CreateCell(10).SetCellValue("Total Taxable Value");
                        headerRow1.CreateCell(11).SetCellValue("Total Cess");
                     
                }
                foreach (var cell in headerRow1.Cells)
                {
                    cell.CellStyle = hStyle;
                }
                if (i==3 && lSalesGstrReportBtoB.LineItems.Count > 0)
                {
                        IRow Row1 = sheet1.CreateRow(i);
                        Row1.CreateCell(0).SetCellValue(lSalesGstrReportBtoB.ReceipientCount);
                        Row1.CreateCell(2).SetCellValue(lSalesGstrReportBtoB.InvoiceCount);
                        Row1.CreateCell(4).SetCellValue(lSalesGstrReportBtoB.LineItems.Sum(x => x.value).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                        Row1.CreateCell(10).SetCellValue(lSalesGstrReportBtoB.LineItems.Sum(x => x.TaxableValue).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                        Row1.CreateCell(11).SetCellValue(lSalesGstrReportBtoB.LineItems.Sum(x => x.CessAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                    for (int columnIndex = 0; columnIndex < 12; columnIndex++)
                    {
                        ICell cell = Row1.GetCell(columnIndex);
                        if (cell != null)
                        {
                            if ( columnIndex == 4 || columnIndex == 10 || columnIndex == 11)
                            {
                                cell.CellStyle = rightAlignedStyle;
                            }
                            else
                            {
                                cell.CellStyle = leftAlignedStyle;
                            }
                        }
                    }


                }
                if (i == 4)
                {
                     
                        headerRow1.CreateCell(0).SetCellValue("GSTIN/UIN of Recipient");
                        headerRow1.CreateCell(1).SetCellValue("Customer Name");
                        headerRow1.CreateCell(2).SetCellValue("Invoice Number");
                        headerRow1.CreateCell(3).SetCellValue("Invoice date");
                        headerRow1.CreateCell(4).SetCellValue("Invoice Value");
                        headerRow1.CreateCell(5).SetCellValue("Place Of Supply");
                        headerRow1.CreateCell(6).SetCellValue("Reverse Charge");
                        headerRow1.CreateCell(7).SetCellValue("Invoice Type");
                        headerRow1.CreateCell(8).SetCellValue("E-Commerce GSTIN");
                        headerRow1.CreateCell(9).SetCellValue("Rate");
                        headerRow1.CreateCell(10).SetCellValue("Taxable Value");
                        headerRow1.CreateCell(11).SetCellValue("Cess Amount");
                       foreach (var cell in headerRow1.Cells)
                       {
                        cell.CellStyle = kStyle;
                       }
                    }
                }
                foreach (SalesGstrReportBtoBLineItem LineItem in lSalesGstrReportBtoB.LineItems)
                {
                    IRow Row1 = sheet1.CreateRow(i++);
                    Row1.CreateCell(0).SetCellValue(LineItem.Gstn);
                    Row1.CreateCell(1).SetCellValue(LineItem.Name);
                    Row1.CreateCell(2).SetCellValue(LineItem.Invoice);
                    Row1.CreateCell(3).SetCellValue(LineItem.Date.ToString(DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat)));
                    Row1.CreateCell(4).SetCellValue(LineItem.value.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                    Row1.CreateCell(5).SetCellValue(Global.Company.Address.FullAddressInSingleLine);
                    Row1.CreateCell(6).SetCellValue(LineItem.RevCharge);
                    Row1.CreateCell(7).SetCellValue(LineItem.Type);
                    Row1.CreateCell(8).SetCellValue("");
                    Row1.CreateCell(9).SetCellValue(LineItem.Rate.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                    Row1.CreateCell(10).SetCellValue(LineItem.TaxableValue.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                    Row1.CreateCell(11).SetCellValue(LineItem.CessAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                for (int columnIndex = 0; columnIndex < 12; columnIndex++)
                {
                    ICell cell = Row1.GetCell(columnIndex);
                    if (cell != null)
                    {
                        if (columnIndex == 3 || columnIndex == 4 || columnIndex == 9 || columnIndex == 10 || columnIndex == 11 )
                        {
                            cell.CellStyle = rightAlignedStyle;
                        }
                        else
                        {
                            cell.CellStyle = leftAlignedStyle;
                        }
                    }
                }

            }
            for (int columnIndex = 0; columnIndex < 12; columnIndex++)
            {
                sheet1.AutoSizeColumn(columnIndex);
            }


            ISheet sheet2 = workbook.CreateSheet("Business To Cust Large");
           
                for (i = 1; i < 5; i++)
                {
                    if (i == 1)
                    {
                        IRow headerRow2 = sheet2.CreateRow(i++);
                        headerRow2.CreateCell(0).SetCellValue("Summary For B2CL");
                        foreach (var cell in headerRow2.Cells)
                        {
                            cell.CellStyle = hStyle;
                        }
                    }
                    if (i == 2)
                    {
                        IRow headerRow2 = sheet2.CreateRow(i++);
                        headerRow2.CreateCell(0).SetCellValue("No. of Invoices");
                        headerRow2.CreateCell(1).SetCellValue("");
                        headerRow2.CreateCell(2).SetCellValue("Total Inv Value");
                        headerRow2.CreateCell(3).SetCellValue("");
                        headerRow2.CreateCell(4).SetCellValue("");
                        headerRow2.CreateCell(5).SetCellValue("Total Taxable Value");
                        headerRow2.CreateCell(6).SetCellValue("Total Cess");
                        headerRow2.CreateCell(7).SetCellValue("");
                    foreach (var cell in headerRow2.Cells)
                        {
                        cell.CellStyle = hStyle;
                        }
                    }
                    if (i == 3)
                    {
                        IRow Row2 = sheet2.CreateRow(i++);
                        Row2.CreateCell(0).SetCellValue(lSalesGstrReportBtoCL.InvoiceCount);
                        Row2.CreateCell(2).SetCellValue(lSalesGstrReportBtoCL.LineItems.Sum(x => x.value).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                        Row2.CreateCell(5).SetCellValue(lSalesGstrReportBtoCL.LineItems.Sum(x => x.TaxableValue).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                        Row2.CreateCell(6).SetCellValue(lSalesGstrReportBtoCL.LineItems.Sum(x => x.CessAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                    for (int columnIndex = 0; columnIndex < 8; columnIndex++)
                    {
                        ICell cell = Row2.GetCell(columnIndex);
                        if (cell != null)
                        {
                            if ( columnIndex == 2 || columnIndex == 5 || columnIndex == 6 )
                            {
                                cell.CellStyle = rightAlignedStyle;
                            }
                            else
                            {
                                cell.CellStyle = leftAlignedStyle;
                            }
                        }
                    }

                }
                    if (i == 4)
                    {
                        IRow headerRow2 = sheet2.CreateRow(i++);
                        headerRow2.CreateCell(0).SetCellValue("Invoice Number");
                        headerRow2.CreateCell(1).SetCellValue("Invoice date");
                        headerRow2.CreateCell(2).SetCellValue("Invoice Value");
                        headerRow2.CreateCell(3).SetCellValue("Place Of Supply");
                        headerRow2.CreateCell(4).SetCellValue("Rate");
                        headerRow2.CreateCell(5).SetCellValue("Taxable Value");
                        headerRow2.CreateCell(6).SetCellValue("Cess Amount");
                        headerRow2.CreateCell(7).SetCellValue("E-Commerce GSTIN");
                          foreach (var cell in headerRow2.Cells)
                          {
                               cell.CellStyle = kStyle;
                          }
                    }
                }
                foreach (SalesGstrReportBtoBLineItem LineItem in lSalesGstrReportBtoCL.LineItems)
                {
                    IRow Row2 = sheet2.CreateRow(i++);
                    Row2.CreateCell(0).SetCellValue(LineItem.Invoice);
                    Row2.CreateCell(1).SetCellValue(LineItem.Date.ToString(DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat)));
                    Row2.CreateCell(2).SetCellValue(LineItem.value.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                    Row2.CreateCell(3).SetCellValue(Global.Company.Address.FullAddressInSingleLine);
                    Row2.CreateCell(4).SetCellValue(LineItem.Rate.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                    Row2.CreateCell(5).SetCellValue(LineItem.TaxableValue.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                    Row2.CreateCell(6).SetCellValue(LineItem.CessAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                    Row2.CreateCell(7).SetCellValue(LineItem.EcomGstn);
                for (int columnIndex = 0; columnIndex < 8; columnIndex++)
                {
                    ICell cell = Row2.GetCell(columnIndex);
                    if (cell != null)
                    {
                        if (columnIndex == 1 || columnIndex == 2 || columnIndex == 4 || columnIndex == 5 || columnIndex == 6 )
                        {
                            cell.CellStyle = rightAlignedStyle;
                        }
                        else
                        {
                            cell.CellStyle = leftAlignedStyle;
                        }
                    }
                }

            }
            for (int columnIndex = 0; columnIndex < 8; columnIndex++)
            {
                sheet2.AutoSizeColumn(columnIndex);
            }

            ISheet sheet3 = workbook.CreateSheet("Business To Cust Small");
          
                for (i = 1; i < 5; i++)
                {
                    if (i == 1)
                    {
                        IRow headerRow3 = sheet3.CreateRow(i++);
                        headerRow3.CreateCell(0).SetCellValue("Summary For B2CS");
                       foreach (var cell in headerRow3.Cells)
                       {
                        cell.CellStyle = hStyle;
                       }
                    }
                    if (i == 2)
                    {
                        IRow headerRow3 = sheet3.CreateRow(i++);
                        headerRow3.CreateCell(0).SetCellValue("");
                        headerRow3.CreateCell(1).SetCellValue("");
                        headerRow3.CreateCell(2).SetCellValue("");
                        headerRow3.CreateCell(3).SetCellValue("Total Taxable  Value");
                        headerRow3.CreateCell(4).SetCellValue("Total Cess");
                        headerRow3.CreateCell(5).SetCellValue("");
                    foreach (var cell in headerRow3.Cells)
                        {
                        cell.CellStyle = hStyle;
                        }
                    }
                    if (i == 3)
                    {
                        IRow Row3 = sheet3.CreateRow(i++);
                        Row3.CreateCell(3).SetCellValue(lSalesGstrReportBtoCS.LineItems.Sum(x => x.TaxableValue).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                        Row3.CreateCell(4).SetCellValue(lSalesGstrReportBtoCS.LineItems.Sum(x => x.CessAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                    for (int columnIndex = 0; columnIndex < 5; columnIndex++)
                    {
                        ICell cell = Row3.GetCell(columnIndex);
                        if (cell != null)
                        {
                            if (columnIndex == 3 || columnIndex == 4)
                            {
                                cell.CellStyle = rightAlignedStyle;
                            }
                            else
                            {
                                cell.CellStyle = leftAlignedStyle;
                            }
                        }
                    }

                }
                    if (i == 4)
                    {
                        IRow headerRow3 = sheet3.CreateRow(i++);
                        headerRow3.CreateCell(0).SetCellValue("Type");
                        headerRow3.CreateCell(1).SetCellValue("Place Of Supply");
                        headerRow3.CreateCell(2).SetCellValue("Rate");
                        headerRow3.CreateCell(3).SetCellValue("Taxable Value");
                        headerRow3.CreateCell(4).SetCellValue("Cess Amount");
                        headerRow3.CreateCell(5).SetCellValue("E-Commerce GSTIN");
                           foreach (var cell in headerRow3.Cells)
                           {
                                   cell.CellStyle = kStyle;
                           }
                    }
                }
                foreach (SalesGstrReportBtoBLineItem LineItem in lSalesGstrReportBtoCS.LineItems)
                {
                    IRow Row3 = sheet3.CreateRow(i++);
                    Row3.CreateCell(0).SetCellValue(LineItem.Type);
                    Row3.CreateCell(1).SetCellValue(Global.Company.Address.FullAddressInSingleLine);
                    Row3.CreateCell(2).SetCellValue(LineItem.Rate.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                    Row3.CreateCell(3).SetCellValue(LineItem.TaxableValue.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                    Row3.CreateCell(4).SetCellValue(LineItem.CessAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                    Row3.CreateCell(5).SetCellValue(LineItem.EcomGstn);
                for (int columnIndex = 0; columnIndex < 5; columnIndex++)
                {
                    ICell cell = Row3.GetCell(columnIndex);
                    if (cell != null)
                    {
                        if (columnIndex == 2 || columnIndex == 3 || columnIndex == 4)
                        {
                            cell.CellStyle = rightAlignedStyle;
                        }
                        else
                        {
                            cell.CellStyle = leftAlignedStyle;
                        }
                    }
                }

            }
            for (int columnIndex = 0; columnIndex < 6; columnIndex++)
            {
                sheet3.AutoSizeColumn(columnIndex);
            }


            workbook.SetActiveSheet(index);
            sfDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfDlg.Filter = "Excel Workbook (*.xls;*.xlsx)|*.xls;*.xlsx|All Files (*.*)|*.*";
            sfDlg.RestoreDirectory = true;
            sfDlg.FileName = "Sale Gstr Report_" + DateTime.Now.ToShortDateString().Replace("/", "-");

            if (sfDlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream fs = new FileStream(sfDlg.FileName, FileMode.Create))
                    {
                        workbook.Write(fs);
                    }
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

        }
    }
}
