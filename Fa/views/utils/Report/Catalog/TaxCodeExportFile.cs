using DocumentFormat.OpenXml.Wordprocessing;
using fa.api.utils;
using fa.model.catalog;
using fa.model.Hms.Master;
using fa.views.utils.Common;
using Fa.api.catalog;
using Microsoft.Office.Interop.Excel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BorderStyle = NPOI.SS.UserModel.BorderStyle;
using HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using DocumentFormat.OpenXml;
using System.Data;
using VisioForge.Libs.NAudio.CoreAudioApi;
using NPOI.SS.Formula.Functions;

namespace fa.views.utils.Report.Catalog
{
    internal class TaxCodeExportFile
    {
        public void GenerateFile()
        {
            Cursor.Current = Cursors.WaitCursor;
            int i = 0;
            int j = 1;
            int k = 1;

            HSSFWorkbook workbook = new HSSFWorkbook();

            ISheet sheet = workbook.CreateSheet("HSNTaxCodeDetails");

            HSSFCellStyle hStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            hStyle.FillPattern = FillPattern.SolidForeground;
            hStyle.FillForegroundColor = HSSFColor.Grey25Percent.Index;

            HSSFFont font1 = (HSSFFont)workbook.CreateFont();
            font1.Boldweight = (short)FontBoldWeight.Bold;
            hStyle.SetFont(font1);
            hStyle.Alignment = HorizontalAlignment.Left;

            HSSFCellStyle leftAlignedStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            leftAlignedStyle.Alignment = HorizontalAlignment.Left;

            HSSFCellStyle rightAlignedStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            rightAlignedStyle.Alignment = HorizontalAlignment.Right;


            IRow headerRow = sheet.CreateRow(i++);
            headerRow.CreateCell(0).SetCellValue("Schedule");
            headerRow.CreateCell(1).SetCellValue("Sl. No.");
            headerRow.CreateCell(2).SetCellValue("TaxCode");
            headerRow.CreateCell(3).SetCellValue("Description");
            headerRow.CreateCell(4).SetCellValue("IGST");
            headerRow.CreateCell(5).SetCellValue("CGST");
            headerRow.CreateCell(6).SetCellValue("SGST / UGST");
            headerRow.CreateCell(7).SetCellValue("EffectiveFromDate");
            headerRow.CreateCell(8).SetCellValue("EffectiveToDate");
            foreach (var cell in headerRow.Cells)
            {
                cell.CellStyle = hStyle;
            }

            IList<ItemTax> ItemTaxInfo = new List<ItemTax>();
            ItemTaxInfo = ItemTaxManager.Instance.GetItemTaxs(fa.Global.Company.CompanyId);
            if (ItemTaxInfo != null)
            {
                foreach (ItemTax itemTax in ItemTaxInfo)
                {
                    if (itemTax.SalesTaxMapLocal.Count > 0) 
                    {
                        // Group ItemSalesTaxMap entries by EffectiveFromDate
                        var groupedByDate = itemTax.SalesTaxMapLocal
                            .GroupBy(s => s.EffectiveFromDate.Date); 

                        foreach (var group in groupedByDate)
                        {
                            DateTime fromDate = group.Key; 

                            // Create a new row for each group (EffectiveFromDate)
                            IRow dataRow = sheet.CreateRow(i++);
                            dataRow.CreateCell(0).SetCellValue(j++); 
                            dataRow.CreateCell(1).SetCellValue(k++); 
                            dataRow.CreateCell(2).SetCellValue(itemTax.Code); 
                            dataRow.CreateCell(3).SetCellValue(itemTax.Description); 

                            // Iterate through items in the current group (same EffectiveFromDate)
                            foreach (ItemSalesTaxMap itemSalesTaxMap in group)
                            {
                                int countrySaleTaxId = (int)(Global.Company.SalesTaxAccountMaps
                                    .FirstOrDefault(x => x.MapId == itemSalesTaxMap.SalesTaxMapId)?.CountrySaleTaxId ?? 0);

                                double taxPercentage = itemSalesTaxMap.TaxPercentage;

                                if (countrySaleTaxId > 0)
                                {
                                    // Determine the appropriate column index based on CountrySaleTaxId
                                    int columnIndex = 0;
                                    switch (countrySaleTaxId)
                                    {
                                        case 1:
                                            columnIndex = 4; // IGST column
                                            break;
                                        case 2:
                                            columnIndex = 5; // CGST column
                                            break;
                                        case 3:
                                            columnIndex = 6; // SGST/UGST column
                                            break;
                                        default:
                                            continue; // Skip if CountrySaleTaxId is not recognized
                                    }

                                    // Set tax percentage in the corresponding column
                                    ICell taxCell = dataRow.GetCell(columnIndex) ?? dataRow.CreateCell(columnIndex);
                                    taxCell.SetCellValue(taxPercentage);
                                    taxCell.CellStyle = rightAlignedStyle; 
                                }
                            }

                            // Set EffectiveFromDate and EffectiveToDate in separate columns
                            ICell fromDateCell = dataRow.GetCell(7) ?? dataRow.CreateCell(7);
                            fromDateCell.SetCellValue(fromDate.ToString(DateUtils.FormatDate(fromDate, Global.Company.DateFormat)));
                            fromDateCell.CellStyle = rightAlignedStyle; 

                            DateTime toDate = group.Max(s => s.EffectiveToDate); 

                            ICell toDateCell = dataRow.GetCell(8) ?? dataRow.CreateCell(8);
                            toDateCell.SetCellValue(toDate.ToString(DateUtils.FormatDate(toDate, Global.Company.DateFormat)));
                            toDateCell.CellStyle = rightAlignedStyle; 

                            for (int columnIndex = 0; columnIndex < 4; columnIndex++)
                            {
                                ICell cell = dataRow.GetCell(columnIndex) ?? dataRow.CreateCell(columnIndex);
                                cell.CellStyle = leftAlignedStyle; 
                            }
                        }
                    }
                }
            }

            for (int columnIndex = 0; columnIndex < 9; columnIndex++)
            {
                sheet.AutoSizeColumn(columnIndex);
            }

            SaveFileDialog sfDlg = new SaveFileDialog();
            sfDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfDlg.Filter = "Excel Workbook (*.xls;*.xlsx)|*.xls;*.xlsx|All Files (*.*)|*.*";
            sfDlg.RestoreDirectory = true;
            sfDlg.FileName = "HSNTaxCodeDetails_" + DateTime.Now.ToShortDateString().Replace("/", "-");

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

        public void GenerateExcelFileViaEPPlus()
        {
            ExcelPackage excel = new ExcelPackage();

            var workSheet = excel.Workbook.Worksheets.Add("HSNTaxCodeDetails");

            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;

            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;

            workSheet.Cells[1, 0].Value = "Schedule";
            workSheet.Cells[1, 1].Value = "Sl. No.";
            workSheet.Cells[1, 2].Value = "TaxCode";
            workSheet.Cells[1, 3].Value = "Description";
            workSheet.Cells[1, 4].Value = "IGST";
            workSheet.Cells[1, 5].Value = "CGST";
            workSheet.Cells[1, 6].Value = "SGST / UGST";
            workSheet.Cells[1, 7].Value = "EffectiveFromDate";
            workSheet.Cells[1, 8].Value = "EffectiveToDate";

            int recordIndex = 2;

            IList<ItemTax> ItemTaxInfo = new List<ItemTax>();
            ItemTaxInfo = ItemTaxManager.Instance.GetItemTaxs(fa.Global.Company.CompanyId);
            if (ItemTaxInfo != null)
            {
                foreach (ItemTax itemTax in ItemTaxInfo)
                {
                    workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                    workSheet.Cells[recordIndex, 2].Value = itemTax.Code;
                    workSheet.Cells[recordIndex, 3].Value = itemTax.Description;

                    if (itemTax.SalesTaxMapLocal.Count > 1)
                    {
                        foreach (ItemSalesTaxMap itemSalesTaxMap in itemTax.SalesTaxMapLocal)
                        {
                            if (Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == itemSalesTaxMap.SalesTaxMapId).CountrySaleTaxId == 1)
                            {
                                workSheet.Cells[recordIndex, 4].Value = itemSalesTaxMap.TaxPercentage;
                            }
                            if (Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == itemSalesTaxMap.SalesTaxMapId).CountrySaleTaxId == 2)
                            {
                                workSheet.Cells[recordIndex, 5].Value = itemSalesTaxMap.TaxPercentage;
                            }
                            if (Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == itemSalesTaxMap.SalesTaxMapId).CountrySaleTaxId == 3)
                            {
                                workSheet.Cells[recordIndex, 6].Value = itemSalesTaxMap.TaxPercentage;
                            }

                            DateTime FromDate = itemSalesTaxMap.EffectiveFromDate;
                            workSheet.Cells[recordIndex, 7].Value = FromDate.ToString(DateUtils.FormatDate(FromDate, Global.Company.DateFormat));

                            DateTime FromDate1 = itemSalesTaxMap.EffectiveToDate;
                            workSheet.Cells[recordIndex, 8].Value = FromDate.ToString(DateUtils.FormatDate(FromDate1, Global.Company.DateFormat));

                        }
                    }
                    recordIndex++;
                }

                workSheet.Column(0).AutoFit();
                workSheet.Column(1).AutoFit();
                workSheet.Column(2).AutoFit();
                workSheet.Column(3).AutoFit();
                workSheet.Column(4).AutoFit();
                workSheet.Column(5).AutoFit();
                workSheet.Column(6).AutoFit();
                workSheet.Column(7).AutoFit();
                workSheet.Column(8).AutoFit();

                SaveFileDialog sfDlg = new SaveFileDialog();
                sfDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                sfDlg.Filter = "Excel Workbook (*.xls;*.xlsx)|*.xls;*.xlsx|All Files (*.*)|*.*";
                sfDlg.RestoreDirectory = true;
                sfDlg.FileName = "HSNTaxCodeDetails_" + DateTime.Now.ToShortDateString().Replace("/", "-");

                if (sfDlg.ShowDialog() == DialogResult.OK)
                {
                    string p_strPath = sfDlg.InitialDirectory + sfDlg.FileName;

                    if (File.Exists(p_strPath))
                        File.Delete(p_strPath);

                    // Create excel file on physical disk  
                    FileStream objFileStrm = File.Create(p_strPath);
                    objFileStrm.Close();

                    // Write content to excel file  
                    File.WriteAllBytes(p_strPath, excel.GetAsByteArray());
                    //Close Excel package 
                    excel.Dispose();
                    Console.ReadKey();
                }
            }
        }
        public DateTime FromDate;
        public DateTime ToDate;
        public void GenerateExcelFileViaEPPlusNoLicence()
        {
            SaveFileDialog sfDlg = new SaveFileDialog();
            sfDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfDlg.Filter = "Excel Workbook (*.xlsx)|*.xlsx|All Files (*.*)|*.*";
            sfDlg.RestoreDirectory = true;
            sfDlg.FileName = "HSNTaxCodeDetails_" + DateTime.Now.ToShortDateString().Replace("/", "-");

            if (sfDlg.ShowDialog() == DialogResult.OK)
            {
                //string p_strPath = sfDlg.InitialDirectory + sfDlg.FileName;

                //if (File.Exists(p_strPath))
                //    File.Delete(p_strPath);

                //var fileInfo = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\EPPlus_PerfTest.xlsx");
                var fileInfo = new FileInfo(sfDlg.FileName);
                if (File.Exists(fileInfo.FullName)) File.Delete(fileInfo.FullName);

                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                var sw = new Stopwatch();
                Cursor.Current = Cursors.WaitCursor;
                sw.Start();
                // since we are in a sync context, wait for the async task to finish...
                WriteToDisk(sw, fileInfo).Wait();
                sw.Stop();
                Cursor.Current = Cursors.Default;
                static async Task WriteToDisk(Stopwatch sw, FileInfo fileInfo)
                {
                    using var package = new ExcelPackage(fileInfo);
                    var sheet = package.Workbook.Worksheets.Add("HSNTaxCodeDetails");

                    IList<ItemTax> ItemTaxInfo = new List<ItemTax>();
                    ItemTaxInfo = ItemTaxManager.Instance.GetItemTaxs(fa.Global.Company.CompanyId);
                    if (ItemTaxInfo != null)
                    {                        
                        var range = sheet.Cells[1, 1, ItemTaxInfo.Count, 9];
                        var toCol = range.End.Column;
                        var toRow = range.End.Row;
                        for (var row = 1; row <= toRow; row++)
                        {
                            DateTime FromDate = new DateTime();
                            DateTime ToDate = new DateTime();
                            for (var col = 1; col <= toCol; col++)
                            {
                                
                                if (col == 1)
                                {
                                    range[row, col].Value = row;
                                }
                                else if (col == 2)
                                {
                                    range[row, col].Value = row;
                                }
                                else if (col == 3)
                                {
                                    range[row, col].Value = ItemTaxInfo[row-1].Code;
                                }
                                else if (col == 4)
                                {
                                    range[row, col].Value = ItemTaxInfo[row-1].Description;
                                }                               
                                else
                                {
                                    foreach (ItemSalesTaxMap itemSalesTaxMap in ItemTaxInfo[row - 1].SalesTaxMapLocal)
                                    {
                                        FromDate = itemSalesTaxMap.EffectiveFromDate;
                                        ToDate = itemSalesTaxMap.EffectiveToDate;
                                        if (col == 5)
                                        {
                                            if (Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == itemSalesTaxMap.SalesTaxMapId).CountrySaleTaxId == 1)
                                            {

                                                range[row, col].Value = itemSalesTaxMap.TaxPercentage;
                                                col++;
                                            }
                                        }
                                        if (col == 6)
                                        {
                                            if (Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == itemSalesTaxMap.SalesTaxMapId).CountrySaleTaxId == 2)
                                            {


                                                range[row, col].Value = itemSalesTaxMap.TaxPercentage;
                                                col++;
                                            }
                                        }
                                        if (col == 7)
                                        {
                                            if (Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == itemSalesTaxMap.SalesTaxMapId).CountrySaleTaxId == 3)
                                            {
                                                range[row, col].Value = itemSalesTaxMap.TaxPercentage;
                                            }
                                        }                                       
                                    }
                                }
                                if (col == 8)
                                {
                                    range[row, col].Value = FromDate.ToString(DateUtils.FormatDate(FromDate, Global.Company.DateFormat));
                                }
                                else if (col == 9)
                                {
                                    range[row, col].Value = ToDate.ToString(DateUtils.FormatDate(ToDate, Global.Company.DateFormat));
                                }
                            }
                            if(row == toRow)
                            {
                                MessageBox.Show("Time taken to  write to excel is : " + sw.Elapsed.TotalSeconds);
                            }
                        }
                        await package.SaveAsync();
                        package.Dispose();
                    }                   
                }                
            }
        }
        public void GenerateEplusTest()
        {
            var fileInfo = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\EPPlus_PerfTest.xlsx");
            if (File.Exists(fileInfo.FullName)) File.Delete(fileInfo.FullName);

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            Console.WriteLine("Creating package");

            var sw = new Stopwatch();
            sw.Start();
            // since we are in a sync context, wait for the async task to finish...
            WriteToDisk(sw, fileInfo).Wait();
            sw.Stop();

            Console.WriteLine(" Total seconds writing data: " + sw.Elapsed.TotalSeconds);
            Console.WriteLine("Loading package...");
            sw.Reset();
            sw.Start();
            LoadFromDisk(fileInfo).Wait();
            sw.Stop();
            Console.WriteLine(" Total seconds loading data: " + sw.Elapsed.TotalSeconds);

            static async Task LoadFromDisk(FileInfo fileInfo)
            {
                using var package = new ExcelPackage();
                await package.LoadAsync(fileInfo);
                Console.WriteLine("Cell GR100000: " + package.Workbook.Worksheets[0].Cells["GR100000"].Value);

            }

            static async Task WriteToDisk(Stopwatch sw, FileInfo fileInfo)
            {
                using var package = new ExcelPackage(fileInfo);
                var sheet = package.Workbook.Worksheets.Add("Test");
                var range = sheet.Cells[1, 1, 100000, 200];
                var toCol = range.End.Column;
                var toRow = range.End.Row;
                for (var row = 1; row <= toRow; row++)
                {
                    for (var col = 1; col <= toCol; col++)
                    {
                        if (col % 2 == 0)
                        {
                            range[row, col].Value = row + col;
                        }
                        else
                        {
                            range[row, col].Value = "abc123abc123abc123abc123abc123abc123";
                        }
                    }
                }
                Console.WriteLine(" Data written to cell store: " + sw.Elapsed.TotalSeconds);
                await package.SaveAsync();
                MessageBox.Show("Work completed to write in disk : " + sw.Elapsed);
            }

            // For Line no 149
            /*
                foreach (ItemTax itemTax in ItemTaxInfo)
                {
                    IRow dataRow = sheet.CreateRow(i++);
                    dataRow.CreateCell(0).SetCellValue(j++);
                    dataRow.CreateCell(1).SetCellValue(k++);
                    dataRow.CreateCell(2).SetCellValue(itemTax.Code);
                    dataRow.CreateCell(3).SetCellValue(itemTax.Description);

                    if (itemTax.SalesTaxMapLocal.Count > 1)
                    {
                        foreach (ItemSalesTaxMap itemSalesTaxMap in itemTax.SalesTaxMapLocal)
                        {
                            //IRow dataRow = sheet.CreateRow(i++);
                            //dataRow.CreateCell(0).SetCellValue(j++);
                            //dataRow.CreateCell(1).SetCellValue(k++);
                            //dataRow.CreateCell(2).SetCellValue(itemTax.Code);
                            //dataRow.CreateCell(3).SetCellValue(itemTax.Description);

                            if (Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == itemSalesTaxMap.SalesTaxMapId).CountrySaleTaxId == 1)
                            {
                                dataRow.CreateCell(4).SetCellValue(itemSalesTaxMap.TaxPercentage);
                            }
                            if (Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == itemSalesTaxMap.SalesTaxMapId).CountrySaleTaxId == 2)
                            {
                                dataRow.CreateCell(5).SetCellValue(itemSalesTaxMap.TaxPercentage);
                            }
                            if (Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == itemSalesTaxMap.SalesTaxMapId).CountrySaleTaxId == 3)
                            {
                                dataRow.CreateCell(6).SetCellValue(itemSalesTaxMap.TaxPercentage);
                            }

                            DateTime FromDate = itemSalesTaxMap.EffectiveFromDate;
                            ICell dateCell = dataRow.CreateCell(7);
                            dateCell.SetCellValue(FromDate.ToString(DateUtils.FormatDate(FromDate, Global.Company.DateFormat)));

                            DateTime FromDate1 = itemSalesTaxMap.EffectiveToDate;
                            ICell dateCell11 = dataRow.CreateCell(8);
                            dateCell11.SetCellValue(FromDate.ToString(DateUtils.FormatDate(FromDate1, Global.Company.DateFormat)));

                            for (int columnIndex = 0; columnIndex < 9; columnIndex++)
                            {
                                ICell cell = dataRow.GetCell(columnIndex);
                                if (cell != null)
                                {
                                    if (columnIndex == 4 || columnIndex == 5 || columnIndex == 6 || columnIndex == 7 || columnIndex == 8)
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
                    }

                } */
        }
    }
}