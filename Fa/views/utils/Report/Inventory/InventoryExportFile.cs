using DocumentFormat.OpenXml.Spreadsheet;
using fa.api.catalog;
using fa.api.OrderManagement;
using fa.api.utils;
using fa.model.catalog;
using fa.model.Catalog;
using fa.model.OrderManagement;
using fa.views.utils.Common;
using Microsoft.Office.Interop.Excel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BorderStyle = System.Windows.Forms.BorderStyle;
using HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment;

namespace fa.views.utils.Report.Catalog
{
    public class InventoryExportFile
    {
        public void GenerateFile()
        {
            SaveFileDialog sfDlg = new SaveFileDialog();
            Cursor.Current = Cursors.WaitCursor;
            int i = 0;

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet2 = workbook.CreateSheet("Stock");

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

            IRow headerRow = sheet2.CreateRow(i++);
            headerRow.CreateCell(0).SetCellValue("Location[*]");
            headerRow.CreateCell(1).SetCellValue("Product Code[*]");
            headerRow.CreateCell(2).SetCellValue("Batch_No");
            headerRow.CreateCell(3).SetCellValue("Exp_Date");
            headerRow.CreateCell(4).SetCellValue("Cost");
            headerRow.CreateCell(5).SetCellValue("PPrice");
            headerRow.CreateCell(6).SetCellValue("RPrice");
            headerRow.CreateCell(7).SetCellValue("Wprice");
            headerRow.CreateCell(8).SetCellValue("MRP");
            headerRow.CreateCell(9).SetCellValue("Stock UOM[*]");
            headerRow.CreateCell(10).SetCellValue("Stock[*]");
            headerRow.CreateCell(11).SetCellValue("Stock_Date[*]");
            foreach (var cell in headerRow.Cells)
            {
                cell.CellStyle = hStyle;
            }


            IList<InventoryBatch> lInventoryBatch = InventoryLocationManager.Instance.GetInventoryBatchbyCompanyId(Global.Company.CompanyId);
            if (lInventoryBatch != null && lInventoryBatch.Count > 0)
            {
                foreach (InventoryBatch InventoryBatch in lInventoryBatch)
                {
                    IRow Row = sheet2.CreateRow(i++);
                    Row.CreateCell(0).SetCellValue(InventoryBatch.Inventory.InventoryLocation.Name);
                    Row.CreateCell(1).SetCellValue(InventoryBatch.Product.MaterialId);
                    Row.CreateCell(2).SetCellValue(InventoryBatch.BatchNo);
                   
                    ICell dateCell = Row.CreateCell(3);
                    dateCell.SetCellValue(InventoryBatch.ExpDate.ToString(DateUtils.FormatDate(InventoryBatch.ExpDate, Global.Company.DateFormat))); 
                  
                    ICell dateCell1 = Row.CreateCell(4);
                    dateCell1.SetCellValue(InventoryBatch.Cost.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));

                    ICell dateCell2 = Row.CreateCell(5);
                    dateCell2.SetCellValue(InventoryBatch.PurchasePrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));

                    ICell dateCell3 = Row.CreateCell(6);
                    dateCell3.SetCellValue(InventoryBatch.RetailSalePrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));

                    ICell dateCell4 = Row.CreateCell(7);
                    dateCell4.SetCellValue(InventoryBatch.WholeSalePrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));

                    ICell dateCell5 = Row.CreateCell(8);
                    dateCell5.SetCellValue(InventoryBatch.MaxRetailPrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                    
                    Row.CreateCell(9).SetCellValue(InventoryBatch.StockUOM);
                    Row.CreateCell(10).SetCellValue(InventoryBatch.OpeningStock);
                    ICell dateCell11 = Row.CreateCell(11);
                    dateCell11.SetCellValue(InventoryBatch.StockDate!=null ?((DateTime)InventoryBatch.StockDate).ToString(DateUtils.FormatDate((DateTime)InventoryBatch.StockDate, Global.Company.DateFormat)):string.Empty);

                    for (int columnIndex = 0; columnIndex < 30; columnIndex++)
                    {
                        ICell cell = Row.GetCell(columnIndex);
                        if (cell != null)
                        {
                            if (columnIndex == 4 || columnIndex == 5 || columnIndex == 6 || columnIndex == 7 || columnIndex == 8 || columnIndex == 10)
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
            IList<fa.model.OrderManagement.Inventory> lInventory = InventoryLocationManager.Instance.ListInventoryByCompanyId(Global.Company.CompanyId);
            if (lInventory != null && lInventory.Where(x => x.Product.isInventoryAtBatch == null || x.Product.isInventoryAtBatch == false).ToList().Count > 0)
            {
                foreach (fa.model.OrderManagement.Inventory Inventory in lInventory.Where(x => x.Product.isInventoryAtBatch == null || x.Product.isInventoryAtBatch == false))
                {
                    IRow Row = sheet2.CreateRow(i++);
                    Row.CreateCell(0).SetCellValue(Inventory.InventoryLocation.Name);
                    Row.CreateCell(1).SetCellValue(Inventory.Product.MaterialId);
                    Row.CreateCell(2).SetCellValue(string.Empty);
                    Row.CreateCell(3).SetCellValue(string.Empty);

                    ICell dateCell = Row.CreateCell(4);
                    dateCell.SetCellValue(Inventory.Product.CostPrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));

                    ICell dateCell1 = Row.CreateCell(5);
                    dateCell1.SetCellValue(Inventory.Product.PurchasePrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));

                    ICell dateCell2 = Row.CreateCell(6);
                    dateCell2.SetCellValue(Inventory.Product.RetailPrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));

                    ICell dateCell3 = Row.CreateCell(7);
                    dateCell3.SetCellValue(Inventory.Product.WholdSalePrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));

                    ICell dateCell4 = Row.CreateCell(8);
                    dateCell4.SetCellValue(Inventory.Product.Msrp.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                    
                    Row.CreateCell(9).SetCellValue(Inventory.StockUOM);
                    Row.CreateCell(10).SetCellValue(Inventory.OpeningStock);
                    ICell dateCell11 = Row.CreateCell(11);
                    dateCell11.SetCellValue(Inventory.StockDate != null ? ((DateTime)Inventory.StockDate).ToString(DateUtils.FormatDate((DateTime)Inventory.StockDate, Global.Company.DateFormat)) : string.Empty);


                    for (int columnIndex = 0; columnIndex < 30; columnIndex++)
                    {
                        ICell cell = Row.GetCell(columnIndex);
                        if (cell != null)
                        {
                            if (columnIndex == 4 || columnIndex == 5 || columnIndex == 6 || columnIndex == 7 || columnIndex == 8 || columnIndex == 10)
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

            for (int columnIndex = 0; columnIndex < 12; columnIndex++)
            {
                sheet2.AutoSizeColumn(columnIndex);
            }

            sfDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfDlg.Filter = "Excel Workbook (*.xls;*.xlsx)|*.xls;*.xlsx|All Files (*.*)|*.*";
            sfDlg.RestoreDirectory = true;
            sfDlg.FileName = "Sample Product Report " + DateTime.Now.ToString("yyyy-MM-dd").Replace("/", "-");

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

