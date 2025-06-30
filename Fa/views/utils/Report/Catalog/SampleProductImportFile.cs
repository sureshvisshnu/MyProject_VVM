using ClosedXML.Excel;
using fa.api.catalog;
using fa.api.Log;
using fa.api.OrderManagement;
using fa.api.utils;
using fa.model.catalog;
using fa.model.Catalog;
using fa.model.OrderManagement;
using fa.views.utils.Common;
using Fa.report.catalog;
using FADataAccessLibrary.Model.Common;
using Microsoft.Office.Interop.Excel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioForge.Libs.NAudio.CoreAudioApi;
using VisioForge.MediaFramework.FFMPEGCore.Instance;
using VisioForge.MediaFramework.ONVIF;
using BorderStyle = System.Windows.Forms.BorderStyle;
using DataTable = System.Data.DataTable;
using DateTime = System.DateTime;
using HorizontalAlignment = System.Windows.Forms.HorizontalAlignment;


namespace fa.views.utils.Report.Catalog
{
    public class SampleProductImportFile
    {
        public void GenerateFile()
        {
            SaveFileDialog sfDlg = new SaveFileDialog();
            Cursor.Current = Cursors.WaitCursor;
            int i = 0;

            HSSFWorkbook workbook = new HSSFWorkbook();

            ISheet sheet = workbook.CreateSheet("Item");

            HSSFCellStyle hStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            hStyle.FillPattern = FillPattern.SolidForeground;
            hStyle.FillForegroundColor = HSSFColor.Grey25Percent.Index;

            HSSFFont font1 = (HSSFFont)workbook.CreateFont();
            font1.Boldweight = (short)FontBoldWeight.Bold;
            hStyle.SetFont(font1);
            hStyle.Alignment = (NPOI.SS.UserModel.HorizontalAlignment)HorizontalAlignment.Left;

            HSSFCellStyle leftAlignedStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            leftAlignedStyle.Alignment = (NPOI.SS.UserModel.HorizontalAlignment)HorizontalAlignment.Left;

            HSSFCellStyle rightAlignedStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            rightAlignedStyle.Alignment = (NPOI.SS.UserModel.HorizontalAlignment)HorizontalAlignment.Right;

            IRow headerRow1 = sheet.CreateRow(i++);
            headerRow1.CreateCell(0).SetCellValue("Product Code[*]");
            headerRow1.CreateCell(1).SetCellValue("HSN Code[*]");
            headerRow1.CreateCell(2).SetCellValue("SKU Name[*]");
            headerRow1.CreateCell(3).SetCellValue("Product Family Name[*]");
            headerRow1.CreateCell(4).SetCellValue("Product Category Name[*]");
            headerRow1.CreateCell(5).SetCellValue("Product Description");
            headerRow1.CreateCell(6).SetCellValue("Purchase UOM[*]");
            headerRow1.CreateCell(7).SetCellValue("Retail UOM[*]");
            headerRow1.CreateCell(8).SetCellValue("Retail X-Factor[*]");
            headerRow1.CreateCell(9).SetCellValue("Wholesale UOM[*]");
            headerRow1.CreateCell(10).SetCellValue("Wholesale X-Factor[*]");
            headerRow1.CreateCell(11).SetCellValue("Purchase Price[*]");
            headerRow1.CreateCell(12).SetCellValue("Retail Price[*]");
            headerRow1.CreateCell(13).SetCellValue("Wholesale Price[*]");
            headerRow1.CreateCell(14).SetCellValue("MSRP[*]");
            headerRow1.CreateCell(15).SetCellValue("Cost[*]");
            headerRow1.CreateCell(16).SetCellValue("Is Batch[*]");
            headerRow1.CreateCell(17).SetCellValue("Sales Account");
            headerRow1.CreateCell(18).SetCellValue("Purchase Account");
            headerRow1.CreateCell(19).SetCellValue("Inventory Account");
            headerRow1.CreateCell(20).SetCellValue("Manufacture");
            headerRow1.CreateCell(21).SetCellValue("Supplier");
            headerRow1.CreateCell(22).SetCellValue("Use HSN Code");
            headerRow1.CreateCell(23).SetCellValue("GST");
            headerRow1.CreateCell(24).SetCellValue("CGST");
            headerRow1.CreateCell(25).SetCellValue("SGST");
            headerRow1.CreateCell(26).SetCellValue("IGST");
            headerRow1.CreateCell(27).SetCellValue("Discount");
            headerRow1.CreateCell(28).SetCellValue("EffectiveFromDate");
            headerRow1.CreateCell(29).SetCellValue("EffectiveToDate");
            foreach (var cell in headerRow1.Cells)
            {
                cell.CellStyle = hStyle;
            }

            IList<Product> lProduct = CatalogProductManager.Instance.ListProductWithDetailsByCompanyId(Global.Company.CompanyId);
            if (lProduct != null && lProduct.Count > 0)
            {
                foreach (Product Product in lProduct)
                {
                    IRow Row1 = sheet.CreateRow(i++);
                    Row1.CreateCell(0).SetCellValue(Product.MaterialId);
                    Row1.CreateCell(1).SetCellValue(Product.HSNCode);
                    Row1.CreateCell(2).SetCellValue(Product.Name);
                    Row1.CreateCell(3).SetCellValue(Product.ProductFamily.Name);
                    Row1.CreateCell(4).SetCellValue(Product.ProductFamily.Parent.Name);
                    Row1.CreateCell(5).SetCellValue(Product.Description);
                    Row1.CreateCell(6).SetCellValue(Product.UOM);
                    Row1.CreateCell(7).SetCellValue(Product.RetailUOM);
                    Row1.CreateCell(8).SetCellValue(Product.RetailXFactor);
                    Row1.CreateCell(9).SetCellValue(Product.WholesaleUOM);
                    Row1.CreateCell(10).SetCellValue(Product.WholesaleXFactor);

                    ICell dateCell1 = Row1.CreateCell(11);
                    dateCell1.SetCellValue(Product.PurchasePrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));

                    ICell dateCell2 = Row1.CreateCell(12);
                    dateCell2.SetCellValue(Product.RetailPrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));

                    ICell dateCell3 = Row1.CreateCell(13);
                    dateCell3.SetCellValue(Product.WholdSalePrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));

                    ICell dateCell4 = Row1.CreateCell(14);
                    dateCell4.SetCellValue(Product.Msrp.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));

                    ICell dateCell5 = Row1.CreateCell(15);
                    dateCell5.SetCellValue(Product.CostPrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));

                    Row1.CreateCell(16).SetCellValue(Product.isInventoryAtBatch != null && (bool)Product.isInventoryAtBatch ? "yes" : "no");
                    Row1.CreateCell(17).SetCellValue(Product.SalesAccount != null ? Product.SalesAccount.Name : String.Empty);
                    Row1.CreateCell(18).SetCellValue(Product.PurchaseAccount != null ? Product.PurchaseAccount.Name : String.Empty);
                    Row1.CreateCell(19).SetCellValue(Product.InventoryAccount != null ? Product.InventoryAccount.Name : String.Empty);
                    Row1.CreateCell(20).SetCellValue(Product.Manufacturer);
                    Row1.CreateCell(21).SetCellValue(Product.Supplier != null ? Product.Supplier.Name : String.Empty);
                    Row1.CreateCell(22).SetCellValue(Product.UseHsnTax ? "yes" : "no");
                    int salesTaxCount = Global.Company.SalesTaxAccountMaps.Count;
                    DateTime EffectiveStartDate = new DateTime(2017, 07, 01);
                    DateTime EffectiveEndDate = new DateTime(2400, 12, 31);
                    int TaxRow = Product?.SalesTaxMapLocal?.Count ?? 0;
                    int TaxRowCount = (TaxRow / salesTaxCount);
                    TaxRowCount += (TaxRowCount % 10 == 0 ? 0 : 1);
                    int TaxCount = 1;
                    if (Product?.SalesTaxMapLocal?.Any() ?? false)
                    {
                        foreach (CatalogItemSalesTaxMap itemSalesTaxMap in Product.SalesTaxMapLocal)
                        {                        
                            var countrySaleTaxId = Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == itemSalesTaxMap.SalesTaxMapId)?.CountrySaleTaxId;
                            if (countrySaleTaxId.HasValue && countrySaleTaxId == 4)
                            {
                                Row1.CreateCell(23).SetCellValue(itemSalesTaxMap.TaxPercentage);
                            }
                            if (countrySaleTaxId.HasValue && countrySaleTaxId == 3)
                            {
                                Row1.CreateCell(25).SetCellValue(itemSalesTaxMap.TaxPercentage);
                            }
                            if (countrySaleTaxId.HasValue && countrySaleTaxId == 2)
                            {
                                Row1.CreateCell(24).SetCellValue(itemSalesTaxMap.TaxPercentage);
                            }
                            if (countrySaleTaxId.HasValue && countrySaleTaxId == 1)
                            {
                                Row1.CreateCell(26).SetCellValue(itemSalesTaxMap.TaxPercentage);
                            }

                            Row1.CreateCell(27).SetCellValue(Product.DefaultDiscount ?? 0.0f);

                            DateTime FromDate = itemSalesTaxMap.EffectiveFrom;
                            ICell dateCell = Row1.CreateCell(28);
                            dateCell.SetCellValue(FromDate.ToString(DateUtils.FormatDate(FromDate, Global.Company.DateFormat)));

                            DateTime FromDate1 = itemSalesTaxMap.EffectiveTo;
                            ICell dateCell11 = Row1.CreateCell(29);
                            dateCell11.SetCellValue(FromDate1.ToString(DateUtils.FormatDate(FromDate1, Global.Company.DateFormat)));

                            TaxCount++;
                        }
                    }
                    else
                    {
                        Row1.CreateCell(23).SetCellValue(0);
                        Row1.CreateCell(24).SetCellValue(0);
                        Row1.CreateCell(25).SetCellValue(0);
                        Row1.CreateCell(26).SetCellValue(0);
                        Row1.CreateCell(27).SetCellValue(Product?.DefaultDiscount ?? 0.0f);
                        ICell dateCell = Row1.CreateCell(28);
                        dateCell.SetCellValue(EffectiveStartDate.ToString(DateUtils.FormatDate(EffectiveStartDate, Global.Company.DateFormat)));

                        ICell dateCell11 = Row1.CreateCell(29);
                        dateCell11.SetCellValue(EffectiveEndDate.ToString(DateUtils.FormatDate(EffectiveEndDate, Global.Company.DateFormat)));
                    }

                    for (int columnIndex = 0; columnIndex < 30; columnIndex++)
                    {
                        ICell cell = Row1.GetCell(columnIndex);
                        if (cell != null)
                        {
                            if (columnIndex == 1 || columnIndex == 8 || columnIndex == 10 || columnIndex >= 11 && columnIndex <= 15 || columnIndex >= 23)
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
            for (int columnIndex = 0; columnIndex < 30; columnIndex++)
            {
                sheet.AutoSizeColumn(columnIndex);
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


        public void GenerateCatalogExcelFileViaClosedXML()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int recordcount = 0;
            SaveFileDialog sfDlg = new SaveFileDialog();
            sfDlg.FileName = null;
            sfDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfDlg.Filter = "Excel Workbook (*.xlsx)|*.xlsx|All Files (*.*)|*.*";
            sfDlg.RestoreDirectory = true;
            sfDlg.FileName = "CatalogProduct_" + DateTime.Now.ToShortDateString().Replace("/", "-");
            sfDlg.Title = "Catalog Item Download Process";
            if (sfDlg.ShowDialog() == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    DataTable resultTable = CatalogProductManager.Instance.ExecuteStoredProcedure(Global.Company.CompanyId);
                    try
                    {
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("ItemDetails");
                            var headers = new[] {   "Product Code[*]", "HSN Code[*]", "SKU Name[*]", "Product Family Name[*]", "Product Category Name[*]", "Product Description",
                                                    "Purchase UOM[*]", "Retail UOM[*]", "Retail X-Factor[*]", "Wholesale UOM[*]", "Wholesale X-Factor[*]", "Purchase Price[*]", "Retail Price[*]",
                                                    "Wholesale Price[*]", "MSRP[*]", "Cost[*]", "Is Batch[*]", "Sales Account", "Purchase Account", "Inventory Account", "Manufacture", "Supplier",
                                                    "Use HSN Code", "GST", "CGST", "SGST", "IGST", "Discount", "EffectiveFromDate", "EffectiveToDate" };
                            for (int i = 0; i < headers.Length; i++)
                            {
                                var cell = worksheet.Cell(1, i + 1);
                                cell.Value = headers[i];
                                cell.Style.Fill.BackgroundColor = XLColor.DarkGray;
                            }
                            for (int i = 0; i < resultTable.Rows.Count; i++)
                            {
                                var ItemList = resultTable.Rows[i];
                                    worksheet.Cell(i + 2, 1).Value = ItemList["MaterialId"] != DBNull.Value ? ItemList["MaterialId"].ToString() : string.Empty;
                                    worksheet.Cell(i + 2, 2).Value = ItemList["HSNCode"] != DBNull.Value ? ItemList["HSNCode"].ToString() : string.Empty;
                                    worksheet.Cell(i + 2, 3).Value = ItemList["ProductName"] != DBNull.Value ? ItemList["ProductName"].ToString() : string.Empty;
                                    worksheet.Cell(i + 2, 4).Value = ItemList["SubCategoryName"] != DBNull.Value ? ItemList["SubCategoryName"].ToString() : string.Empty;
                                    worksheet.Cell(i + 2, 5).Value = ItemList["CategoryName"] != DBNull.Value ? ItemList["CategoryName"].ToString() : string.Empty;
                                    worksheet.Cell(i + 2, 6).Value = ItemList["Description"] != DBNull.Value ? ItemList["Description"].ToString() : string.Empty;
                                    worksheet.Cell(i + 2, 7).Value = ItemList["UOM"] != DBNull.Value ? ItemList["UOM"].ToString() : string.Empty;
                                    worksheet.Cell(i + 2, 8).Value = ItemList["RetailUOM"] != DBNull.Value ? ItemList["RetailUOM"].ToString() : string.Empty;
                                    worksheet.Cell(i + 2, 9).Value = ItemList["RetailXFactor"] != DBNull.Value ? Convert.ToInt32(ItemList["RetailXFactor"]) : 1;
                                    worksheet.Cell(i + 2, 10).Value = ItemList["WholesaleUOM"] != DBNull.Value ? ItemList["WholesaleUOM"].ToString() : string.Empty;
                                    worksheet.Cell(i + 2, 11).Value = ItemList["WholesaleXFactor"] != DBNull.Value ? Convert.ToInt32(ItemList["WholesaleXFactor"]) : 1;
                                    worksheet.Cell(i + 2, 12).Value = ItemList["CostPrice"] != DBNull.Value ? Convert.ToSingle(ItemList["CostPrice"]) : 0.0f;
                                    worksheet.Cell(i + 2, 13).Value = ItemList["RetailPrice"] != DBNull.Value ? Convert.ToSingle(ItemList["RetailPrice"]) : 0.0f;
                                    worksheet.Cell(i + 2, 14).Value = ItemList["WholdSalePrice"] != DBNull.Value ? Convert.ToSingle(ItemList["WholdSalePrice"]) : 0.0f;
                                    worksheet.Cell(i + 2, 15).Value = ItemList["Msrp"] != DBNull.Value ? Convert.ToSingle(ItemList["Msrp"]) : 0.0f;
                                    worksheet.Cell(i + 2, 16).Value = ItemList["CostPrice"] != DBNull.Value ? Convert.ToSingle(ItemList["CostPrice"]) : 0.0f;
                                    worksheet.Cell(i + 2, 17).Value = ItemList["IsInventoryAtBatch"] != DBNull.Value ? (bool)ItemList["IsInventoryAtBatch"] ? "YES" : "NO" : "NO";
                                    worksheet.Cell(i + 2, 18).Value = ItemList["SalesAccount"] != DBNull.Value ? ItemList["Supplier"].ToString() : string.Empty;
                                    worksheet.Cell(i + 2, 19).Value = ItemList["PurchaseAccount"] != DBNull.Value ? ItemList["PurchaseAccount"].ToString() : string.Empty;
                                    worksheet.Cell(i + 2, 20).Value = ItemList["InventoryAccount"] != DBNull.Value ? ItemList["InventoryAccount"].ToString() : string.Empty;
                                    worksheet.Cell(i + 2, 21).Value = ItemList["Manufacturer"] != DBNull.Value ? ItemList["Manufacturer"].ToString() : string.Empty;
                                    worksheet.Cell(i + 2, 22).Value = ItemList["Supplier"] != DBNull.Value ? ItemList["Supplier"].ToString() : string.Empty;
                                    worksheet.Cell(i + 2, 23).Value = ItemList["UseHsnTax"] != DBNull.Value ? (bool)ItemList["UseHsnTax"] ? "YES" : "NO" : "NO";
                                    worksheet.Cell(i + 2, 24).Value = (ItemList["CGST"] != DBNull.Value ? Convert.ToInt32(ItemList["CGST"]) : 0)+(ItemList["SGST"] != DBNull.Value ? Convert.ToInt32(ItemList["SGST"]) : 0);
                                    worksheet.Cell(i + 2, 25).Value = ItemList["CGST"] != DBNull.Value ? Convert.ToInt32(ItemList["CGST"]) : 0;
                                    worksheet.Cell(i + 2, 26).Value = ItemList["SGST"] != DBNull.Value ? Convert.ToInt32(ItemList["SGST"]) : 0;
                                    worksheet.Cell(i + 2, 27).Value = ItemList["IGST"] != DBNull.Value ? Convert.ToInt32(ItemList["IGST"]) : 0;
                                    worksheet.Cell(i + 2, 28).Value = ItemList["DefaultDiscountLocal"] != DBNull.Value ? Convert.ToSingle(ItemList["DefaultDiscountLocal"]) : 0.0f;
                                    DateTime EffectiveStartDate = new DateTime(2017, 07, 01);
                                    DateTime EffectiveEndDate = new DateTime(2400, 12, 31);
                                    worksheet.Cell(i + 2, 29).Value = ItemList["ItemTaxEffectiveFromDate"] != DBNull.Value ? Convert.ToDateTime(ItemList["ItemTaxEffectiveFromDate"]) : EffectiveStartDate;
                                    worksheet.Cell(i + 2, 30).Value = ItemList["ItemTaxEffectiveToDate"] != DBNull.Value ? Convert.ToDateTime(ItemList["ItemTaxEffectiveToDate"]) : EffectiveEndDate;
                                    recordcount = i;
                            }
                            workbook.SaveAs(sfDlg.FileName);
                            Cursor.Current = Cursors.Default;
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("The excisting excel file is in open, \n please close the file or save with another name");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception " + ex.Message+"\n Please contact administration");
                    Logger.LogError(ex);
                }
                sw.Stop();
            }
        }
    }
}
