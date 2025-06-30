using fa.api.utils;
using fa.model.Common;
using fa.report.accounting.master;
using fa.report.Inventory;
using fa.reports.Inventory;
using fa.views.utils.Common;
using Fa.reports.Inventory;
using FADataAccessLibrary.report.Inventory;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using VisioForge.Libs.NAudio.CoreAudioApi;
using Font = iTextSharp.text.Font;

namespace fa.views.utils.Report.Inventory
{
    class StockReportSavePrint
    {
        public bool ExportOrPrintToFile(RptStockReport RptStockReport, string ReportName, string fileExtension, bool isPrint,  List<string> selectedRackNumbers, List<string> selectedManufacturers, List<string> selectedSuppliers)
        {
            if (RptStockReport != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewDataTable(RptStockReport, selectedRackNumbers, selectedManufacturers, selectedSuppliers);
                            if (DataTable != null)
                            {
                                GeneratePDFStocReport(DataTable, RptStockReport, "pdf", isPrint);
                                break;
                            }
                            else
                                break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("File Error Please Contact System Admin");
                    Console.WriteLine(e.ToString());
                }
            }
            return true;
        }
        static readonly String[] StockListColumn = new String[]
        {
            "#", "Product Code", "Product Name", "UOM" , "Rack Num", "Batch Expiry", "Opening Stock","Opening Stock Value", "Closing Stock","Closing Stock Value", "Pur", "Pur Return", "Salse", "Salse Return", "Stock In", "Stock Out", "To Patient", "Damage","Adjust","Last Month Sale"
        };
        public static DataTable DataGridViewDataTable(RptStockReport ReportStock, List<string> selectedRackNumbers, List<string> selectedManufacturers, List<string> selectedSuppliers)
        {
            DataTable StockListTable = new DataTable();
            StockListTable.Columns.Add(StockListColumn[(int)StockReportNewTableColumn.SNO], typeof(string));
            StockListTable.Columns.Add(StockListColumn[(int)StockReportNewTableColumn.MID], typeof(string));
            StockListTable.Columns.Add(StockListColumn[(int)StockReportNewTableColumn.PNAME], typeof(string));
            StockListTable.Columns.Add(StockListColumn[(int)StockReportNewTableColumn.UOM], typeof(string));
            if (ReportStock.Type != ComboTypeSelection.BYRACK && Global.Company.MaintainRackNumber)
            {
                StockListTable.Columns.Add(StockListColumn[(int)StockReportNewTableColumn.RACKNO], typeof(string));
            }
            StockListTable.Columns.Add(StockListColumn[(int)StockReportNewTableColumn.BAT], typeof(string));
            StockListTable.Columns.Add(StockListColumn[(int)StockReportNewTableColumn.OPS], typeof(string));
            StockListTable.Columns.Add(StockListColumn[(int)StockReportNewTableColumn.OPA], typeof(string));
            StockListTable.Columns.Add(StockListColumn[(int)StockReportNewTableColumn.CLSTK], typeof(string));
            StockListTable.Columns.Add(StockListColumn[(int)StockReportNewTableColumn.CSA], typeof(string));
            StockListTable.Columns.Add(StockListColumn[(int)StockReportNewTableColumn.P_QTY], typeof(string));
            StockListTable.Columns.Add(StockListColumn[(int)StockReportNewTableColumn.PR_QTY], typeof(string));
            StockListTable.Columns.Add(StockListColumn[(int)StockReportNewTableColumn.S_QTY], typeof(string));
            StockListTable.Columns.Add(StockListColumn[(int)StockReportNewTableColumn.SR_QTY], typeof(string));
            StockListTable.Columns.Add(StockListColumn[(int)StockReportNewTableColumn.STIN_QTY], typeof(string));
            StockListTable.Columns.Add(StockListColumn[(int)StockReportNewTableColumn.STOT_QTY], typeof(string));
            StockListTable.Columns.Add(StockListColumn[(int)StockReportNewTableColumn.PCON], typeof(string));
            StockListTable.Columns.Add(StockListColumn[(int)StockReportNewTableColumn.DAM], typeof(string));
            StockListTable.Columns.Add(StockListColumn[(int)StockReportNewTableColumn.AD_QTY], typeof(string));
            StockListTable.Columns.Add(StockListColumn[(int)StockReportNewTableColumn.LASTMONTHSALE], typeof(string));

            DataRow dataRow = null;
            int rowIndex = 0;

            if (ReportStock.StockReportLineItemData != null && ReportStock.StockReportLineItemData.Any())
            {
                if(ReportStock.Type == ComboTypeSelection.BYDATE)
                {
                    int Sno = 1;
                    string currentLocation = string.Empty;

                    double locationOpeningStock = 0, locationOpeningAmount = 0, locationClosingStock = 0, locationClosingAmount = 0, locationPurchaseStock = 0,
                        locationPurchaseReturnStock = 0, locationSalesStock = 0, locationSalesReturnStock = 0, locationMoveInStock = 0, locationMoveOutStock = 0,
                        locationToPatientStock = 0, locationDamageStock = 0, locationAdjustmentStock = 0, locationLastMonthSaleStock = 0;
                    double grandOpeningStock = 0, grandOpeningAmount = 0, grandClosingStock = 0, grandClosingAmount = 0, grandPurchaseStock = 0,
                        grandPurchaseReturnStock = 0, grandSalesStock = 0, grandSalesReturnStock = 0, grandMoveInStock = 0, grandMoveOutStock = 0,
                        grandToPatientStock = 0, grandDamageStock = 0, grandAdjustmentStock = 0, grandLastMonthSaleStock = 0;
                    foreach (var lineItem in ReportStock.StockReportLineItemData.OrderBy(x => x.LocationName).ThenBy(x => x.Name))
                    {
                        if (currentLocation != lineItem.LocationName)
                        {
                            if (!string.IsNullOrEmpty(currentLocation))
                            {
                                AddSubtotalRow(StockListTable, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0; locationPurchaseStock = 0;
                                locationPurchaseReturnStock = 0; locationSalesStock = 0; locationSalesReturnStock = 0; locationMoveInStock = 0; locationMoveOutStock = 0;
                                locationToPatientStock = 0; locationDamageStock = 0; locationAdjustmentStock = 0; locationLastMonthSaleStock = 0;
                            }
                            dataRow = StockListTable.NewRow();
                            dataRow[StockListColumn[(int)StockReportNewTableColumn.SNO]] = $"Location Name : {lineItem.LocationName}";
                            StockListTable.Rows.Add(dataRow);
                            currentLocation = lineItem.LocationName;
                            Sno = 1;
                        }
                        AddDataRow(StockListTable, lineItem, Sno, ReportStock);
                        Sno++;
                        locationOpeningStock += lineItem.OpeningStock; locationOpeningAmount += lineItem.OpeningStockAmount; locationClosingStock += lineItem.CloseingStock; locationClosingAmount += lineItem.ClosingStockAmount;
                        locationPurchaseStock += lineItem.PurchaseQty; locationPurchaseReturnStock += lineItem.PurchaseReturnQty; locationSalesStock += lineItem.SalesQty; locationSalesReturnStock += lineItem.SalesReturnQty;
                        locationMoveInStock += lineItem.StockInQty; locationMoveOutStock += lineItem.StockOutQty; locationToPatientStock += lineItem.ToPatientQty; locationDamageStock += lineItem.DamageQty;
                        locationAdjustmentStock += lineItem.AdjustQty; locationLastMonthSaleStock += lineItem.LastMonthSale;

                        grandOpeningStock += lineItem.OpeningStock; grandOpeningAmount += lineItem.OpeningStockAmount; grandClosingStock += lineItem.CloseingStock; grandClosingAmount += lineItem.ClosingStockAmount;
                        grandPurchaseStock += lineItem.PurchaseQty; grandPurchaseReturnStock += lineItem.PurchaseReturnQty; grandSalesStock += lineItem.SalesQty; grandSalesReturnStock += lineItem.SalesReturnQty;
                        grandMoveInStock += lineItem.StockInQty; grandMoveOutStock += lineItem.StockOutQty; grandToPatientStock += lineItem.ToPatientQty; grandDamageStock += lineItem.DamageQty;
                        grandAdjustmentStock += lineItem.AdjustQty; grandLastMonthSaleStock += lineItem.LastMonthSale;
                    }
                    if (!string.IsNullOrEmpty(currentLocation))
                    {
                        AddSubtotalRow(StockListTable, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);
                    }
                    AddGrandTotalRow(StockListTable, grandOpeningStock, grandOpeningAmount, grandClosingStock, grandClosingAmount, grandPurchaseStock,
                                                grandPurchaseReturnStock, grandSalesStock, grandSalesReturnStock, grandMoveInStock, grandMoveOutStock,
                                                grandToPatientStock, grandDamageStock, grandAdjustmentStock, grandLastMonthSaleStock);
                }
                else if (ReportStock.Type == ComboTypeSelection.BYCATEGORY)
                {
                    int Sno = 1;
                    string currentLocation = string.Empty;
                    string CategoryName = string.Empty;

                    double locationOpeningStock = 0, locationOpeningAmount = 0, locationClosingStock = 0, locationClosingAmount = 0, locationPurchaseStock = 0,
                        locationPurchaseReturnStock = 0, locationSalesStock = 0, locationSalesReturnStock = 0, locationMoveInStock = 0, locationMoveOutStock = 0,
                        locationToPatientStock = 0, locationDamageStock = 0, locationAdjustmentStock = 0, locationLastMonthSaleStock = 0;
                    double grandOpeningStock = 0, grandOpeningAmount = 0, grandClosingStock = 0, grandClosingAmount = 0, grandPurchaseStock = 0,
                        grandPurchaseReturnStock = 0, grandSalesStock = 0, grandSalesReturnStock = 0, grandMoveInStock = 0, grandMoveOutStock = 0,
                        grandToPatientStock = 0, grandDamageStock = 0, grandAdjustmentStock = 0, grandLastMonthSaleStock = 0;

                    foreach (var lineItem in ReportStock.StockReportLineItemData.OrderBy(x => x.LocationName).ThenBy(x => x.CategoryName).ThenBy(x => x.Name))
                    {
                        if (currentLocation != lineItem.LocationName)
                        {
                            if (!string.IsNullOrEmpty(CategoryName))
                            {
                                AddSubtotalRow(StockListTable, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0; locationPurchaseStock = 0;
                                locationPurchaseReturnStock = 0; locationSalesStock = 0; locationSalesReturnStock = 0; locationMoveInStock = 0; locationMoveOutStock = 0;
                                locationToPatientStock = 0; locationDamageStock = 0; locationAdjustmentStock = 0; locationLastMonthSaleStock = 0;
                                CategoryName = string.Empty;
                            }
                            dataRow = StockListTable.NewRow();
                            dataRow[StockListColumn[(int)StockReportNewTableColumn.SNO]] = $"Location Name : {lineItem.LocationName}";
                            StockListTable.Rows.Add(dataRow);
                            currentLocation = lineItem.LocationName;
                            Sno = 1;
                        }
                        if (CategoryName != lineItem.CategoryName)
                        {
                            if (!string.IsNullOrEmpty(CategoryName))
                            {
                                var subtotalRow = new DataGridViewRow();
                                AddSubtotalRow(StockListTable, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0; locationPurchaseStock = 0;
                                locationPurchaseReturnStock = 0; locationSalesStock = 0; locationSalesReturnStock = 0; locationMoveInStock = 0; locationMoveOutStock = 0;
                                locationToPatientStock = 0; locationDamageStock = 0; locationAdjustmentStock = 0; locationLastMonthSaleStock = 0;
                            }
                            dataRow = StockListTable.NewRow();
                            dataRow[StockListColumn[(int)StockReportNewTableColumn.SNO]] = $"Category Name : {lineItem.CategoryName}";
                            StockListTable.Rows.Add(dataRow);
                            CategoryName = lineItem.CategoryName;
                            Sno = 1;
                        }
                        AddDataRow(StockListTable, lineItem, Sno, ReportStock);
                        Sno++;
                        locationOpeningStock += lineItem.OpeningStock; locationOpeningAmount += lineItem.OpeningStockAmount; locationClosingStock += lineItem.CloseingStock; locationClosingAmount += lineItem.ClosingStockAmount;
                        locationPurchaseStock += lineItem.PurchaseQty; locationPurchaseReturnStock += lineItem.PurchaseReturnQty; locationSalesStock += lineItem.SalesQty; locationSalesReturnStock += lineItem.SalesReturnQty;
                        locationMoveInStock += lineItem.StockInQty; locationMoveOutStock += lineItem.StockOutQty; locationToPatientStock += lineItem.ToPatientQty; locationDamageStock += lineItem.DamageQty;
                        locationAdjustmentStock += lineItem.AdjustQty; locationLastMonthSaleStock += lineItem.LastMonthSale;

                        grandOpeningStock += lineItem.OpeningStock; grandOpeningAmount += lineItem.OpeningStockAmount; grandClosingStock += lineItem.CloseingStock; grandClosingAmount += lineItem.ClosingStockAmount;
                        grandPurchaseStock += lineItem.PurchaseQty; grandPurchaseReturnStock += lineItem.PurchaseReturnQty; grandSalesStock += lineItem.SalesQty; grandSalesReturnStock += lineItem.SalesReturnQty;
                        grandMoveInStock += lineItem.StockInQty; grandMoveOutStock += lineItem.StockOutQty; grandToPatientStock += lineItem.ToPatientQty; grandDamageStock += lineItem.DamageQty;
                        grandAdjustmentStock += lineItem.AdjustQty; grandLastMonthSaleStock += lineItem.LastMonthSale;
                    }
                    if (!string.IsNullOrEmpty(CategoryName))
                    {
                        AddSubtotalRow(StockListTable, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);
                    }
                    AddGrandTotalRow(StockListTable, grandOpeningStock, grandOpeningAmount, grandClosingStock, grandClosingAmount, grandPurchaseStock,
                                                grandPurchaseReturnStock, grandSalesStock, grandSalesReturnStock, grandMoveInStock, grandMoveOutStock,
                                                grandToPatientStock, grandDamageStock, grandAdjustmentStock, grandLastMonthSaleStock);
                }
                else if (ReportStock.Type == ComboTypeSelection.BYPRODUCTFAMILY)
                {
                    int Sno = 1;
                    string currentLocation = string.Empty;
                    string ProductFamilyName = string.Empty;

                    double locationOpeningStock = 0, locationOpeningAmount = 0, locationClosingStock = 0, locationClosingAmount = 0, locationPurchaseStock = 0,
                        locationPurchaseReturnStock = 0, locationSalesStock = 0, locationSalesReturnStock = 0, locationMoveInStock = 0, locationMoveOutStock = 0,
                        locationToPatientStock = 0, locationDamageStock = 0, locationAdjustmentStock = 0, locationLastMonthSaleStock = 0;
                    double grandOpeningStock = 0, grandOpeningAmount = 0, grandClosingStock = 0, grandClosingAmount = 0, grandPurchaseStock = 0,
                        grandPurchaseReturnStock = 0, grandSalesStock = 0, grandSalesReturnStock = 0, grandMoveInStock = 0, grandMoveOutStock = 0,
                        grandToPatientStock = 0, grandDamageStock = 0, grandAdjustmentStock = 0, grandLastMonthSaleStock = 0;
                    foreach (var lineItem in ReportStock.StockReportLineItemData.OrderBy(x => x.LocationName).ThenBy(x => x.ProductFamilyName).ThenBy(x => x.Name))
                    {
                        if (currentLocation != lineItem.LocationName)
                        {
                            if (!string.IsNullOrEmpty(ProductFamilyName))
                            {
                                AddSubtotalRow(StockListTable, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0; locationPurchaseStock = 0;
                                locationPurchaseReturnStock = 0; locationSalesStock = 0; locationSalesReturnStock = 0; locationMoveInStock = 0; locationMoveOutStock = 0;
                                locationToPatientStock = 0; locationDamageStock = 0; locationAdjustmentStock = 0; locationLastMonthSaleStock = 0;
                                ProductFamilyName = string.Empty;
                            }
                            dataRow = StockListTable.NewRow();
                            dataRow[StockListColumn[(int)StockReportNewTableColumn.SNO]] = $"Location Name : {lineItem.LocationName}";
                            StockListTable.Rows.Add(dataRow);
                            currentLocation = lineItem.LocationName;
                            Sno = 1;
                        }
                        if (ProductFamilyName != lineItem.ProductFamilyName)
                        {
                            if (!string.IsNullOrEmpty(ProductFamilyName))
                            {
                                AddSubtotalRow(StockListTable, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0; locationPurchaseStock = 0;
                                locationPurchaseReturnStock = 0; locationSalesStock = 0; locationSalesReturnStock = 0; locationMoveInStock = 0; locationMoveOutStock = 0;
                                locationToPatientStock = 0; locationDamageStock = 0; locationAdjustmentStock = 0; locationLastMonthSaleStock = 0;

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0;
                            }
                            dataRow = StockListTable.NewRow();
                            dataRow[StockListColumn[(int)StockReportNewTableColumn.SNO]] = $"Product Family Name : {lineItem.ProductFamilyName}";
                            StockListTable.Rows.Add(dataRow);
                            ProductFamilyName = lineItem.ProductFamilyName;
                            Sno = 1;
                        }
                        AddDataRow(StockListTable, lineItem, Sno, ReportStock);
                        Sno++;
                        locationOpeningStock += lineItem.OpeningStock; locationOpeningAmount += lineItem.OpeningStockAmount; locationClosingStock += lineItem.CloseingStock; locationClosingAmount += lineItem.ClosingStockAmount;
                        locationPurchaseStock += lineItem.PurchaseQty; locationPurchaseReturnStock += lineItem.PurchaseReturnQty; locationSalesStock += lineItem.SalesQty; locationSalesReturnStock += lineItem.SalesReturnQty;
                        locationMoveInStock += lineItem.StockInQty; locationMoveOutStock += lineItem.StockOutQty; locationToPatientStock += lineItem.ToPatientQty; locationDamageStock += lineItem.DamageQty;
                        locationAdjustmentStock += lineItem.AdjustQty; locationLastMonthSaleStock += lineItem.LastMonthSale;

                        grandOpeningStock += lineItem.OpeningStock; grandOpeningAmount += lineItem.OpeningStockAmount; grandClosingStock += lineItem.CloseingStock; grandClosingAmount += lineItem.ClosingStockAmount;
                        grandPurchaseStock += lineItem.PurchaseQty; grandPurchaseReturnStock += lineItem.PurchaseReturnQty; grandSalesStock += lineItem.SalesQty; grandSalesReturnStock += lineItem.SalesReturnQty;
                        grandMoveInStock += lineItem.StockInQty; grandMoveOutStock += lineItem.StockOutQty; grandToPatientStock += lineItem.ToPatientQty; grandDamageStock += lineItem.DamageQty;
                        grandAdjustmentStock += lineItem.AdjustQty; grandLastMonthSaleStock += lineItem.LastMonthSale;
                    }
                    if (!string.IsNullOrEmpty(ProductFamilyName))
                    {
                        AddSubtotalRow(StockListTable, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);
                    }
                    AddGrandTotalRow(StockListTable, grandOpeningStock, grandOpeningAmount, grandClosingStock, grandClosingAmount, grandPurchaseStock,
                                                grandPurchaseReturnStock, grandSalesStock, grandSalesReturnStock, grandMoveInStock, grandMoveOutStock,
                                                grandToPatientStock, grandDamageStock, grandAdjustmentStock, grandLastMonthSaleStock);
                }
                else if (ReportStock.Type == ComboTypeSelection.BYMANUFACTURER)
                {
                    int Sno = 1;
                    string currentLocation = string.Empty;
                    string ManufactureName = string.Empty;

                    double locationOpeningStock = 0, locationOpeningAmount = 0, locationClosingStock = 0, locationClosingAmount = 0, locationPurchaseStock = 0,
                        locationPurchaseReturnStock = 0, locationSalesStock = 0, locationSalesReturnStock = 0, locationMoveInStock = 0, locationMoveOutStock = 0,
                        locationToPatientStock = 0, locationDamageStock = 0, locationAdjustmentStock = 0, locationLastMonthSaleStock = 0;
                    double grandOpeningStock = 0, grandOpeningAmount = 0, grandClosingStock = 0, grandClosingAmount = 0, grandPurchaseStock = 0,
                        grandPurchaseReturnStock = 0, grandSalesStock = 0, grandSalesReturnStock = 0, grandMoveInStock = 0, grandMoveOutStock = 0,
                        grandToPatientStock = 0, grandDamageStock = 0, grandAdjustmentStock = 0, grandLastMonthSaleStock = 0;

                    foreach (var lineItem in ReportStock.StockReportLineItemData.OrderBy(x => x.LocationName).ThenBy(x => x.ManufactureName).ThenBy(x => x.Name))
                    {
                        if (currentLocation != lineItem.LocationName)
                        {
                            if (!string.IsNullOrEmpty(ManufactureName))
                            {
                                AddSubtotalRow(StockListTable, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0; locationPurchaseStock = 0;
                                locationPurchaseReturnStock = 0; locationSalesStock = 0; locationSalesReturnStock = 0; locationMoveInStock = 0; locationMoveOutStock = 0;
                                locationToPatientStock = 0; locationDamageStock = 0; locationAdjustmentStock = 0; locationLastMonthSaleStock = 0;
                                ManufactureName = string.Empty;
                            }
                            dataRow = StockListTable.NewRow();
                            dataRow[StockListColumn[(int)StockReportNewTableColumn.SNO]] = $"Location Name : {lineItem.LocationName}";
                            StockListTable.Rows.Add(dataRow);

                            currentLocation = lineItem.LocationName;
                            Sno = 1;
                        }
                        if (ManufactureName != lineItem.ManufactureName)
                        {
                            if (!string.IsNullOrEmpty(ManufactureName))
                            {
                                AddSubtotalRow(StockListTable, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0; locationPurchaseStock = 0;
                                locationPurchaseReturnStock = 0; locationSalesStock = 0; locationSalesReturnStock = 0; locationMoveInStock = 0; locationMoveOutStock = 0;
                                locationToPatientStock = 0; locationDamageStock = 0; locationAdjustmentStock = 0; locationLastMonthSaleStock = 0;

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0;
                            }
                            dataRow = StockListTable.NewRow();
                            dataRow[StockListColumn[(int)StockReportNewTableColumn.SNO]] = $"Manufacture Name : {lineItem.ManufactureName}";
                            StockListTable.Rows.Add(dataRow);

                            ManufactureName = lineItem.ManufactureName;
                            Sno = 1;
                        }
                        AddDataRow(StockListTable, lineItem, Sno, ReportStock);
                        Sno++;
                        locationOpeningStock += lineItem.OpeningStock; locationOpeningAmount += lineItem.OpeningStockAmount; locationClosingStock += lineItem.CloseingStock; locationClosingAmount += lineItem.ClosingStockAmount;
                        locationPurchaseStock += lineItem.PurchaseQty; locationPurchaseReturnStock += lineItem.PurchaseReturnQty; locationSalesStock += lineItem.SalesQty; locationSalesReturnStock += lineItem.SalesReturnQty;
                        locationMoveInStock += lineItem.StockInQty; locationMoveOutStock += lineItem.StockOutQty; locationToPatientStock += lineItem.ToPatientQty; locationDamageStock += lineItem.DamageQty;
                        locationAdjustmentStock += lineItem.AdjustQty; locationLastMonthSaleStock += lineItem.LastMonthSale;

                        grandOpeningStock += lineItem.OpeningStock; grandOpeningAmount += lineItem.OpeningStockAmount; grandClosingStock += lineItem.CloseingStock; grandClosingAmount += lineItem.ClosingStockAmount;
                        grandPurchaseStock += lineItem.PurchaseQty; grandPurchaseReturnStock += lineItem.PurchaseReturnQty; grandSalesStock += lineItem.SalesQty; grandSalesReturnStock += lineItem.SalesReturnQty;
                        grandMoveInStock += lineItem.StockInQty; grandMoveOutStock += lineItem.StockOutQty; grandToPatientStock += lineItem.ToPatientQty; grandDamageStock += lineItem.DamageQty;
                        grandAdjustmentStock += lineItem.AdjustQty; grandLastMonthSaleStock += lineItem.LastMonthSale;
                    }
                    if (!string.IsNullOrEmpty(ManufactureName))
                    {
                        AddSubtotalRow(StockListTable, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);
                    }
                    AddGrandTotalRow(StockListTable, grandOpeningStock, grandOpeningAmount, grandClosingStock, grandClosingAmount, grandPurchaseStock,
                                                grandPurchaseReturnStock, grandSalesStock, grandSalesReturnStock, grandMoveInStock, grandMoveOutStock,
                                                grandToPatientStock, grandDamageStock, grandAdjustmentStock, grandLastMonthSaleStock);
                }
                else if (ReportStock.Type == ComboTypeSelection.BYSUPPLIER)
                {
                    int Sno = 1;
                    string currentLocation = string.Empty;
                    string SupplierName = string.Empty;

                    double locationOpeningStock = 0, locationOpeningAmount = 0, locationClosingStock = 0, locationClosingAmount = 0, locationPurchaseStock = 0,
                        locationPurchaseReturnStock = 0, locationSalesStock = 0, locationSalesReturnStock = 0, locationMoveInStock = 0, locationMoveOutStock = 0,
                        locationToPatientStock = 0, locationDamageStock = 0, locationAdjustmentStock = 0, locationLastMonthSaleStock = 0;
                    double grandOpeningStock = 0, grandOpeningAmount = 0, grandClosingStock = 0, grandClosingAmount = 0, grandPurchaseStock = 0,
                        grandPurchaseReturnStock = 0, grandSalesStock = 0, grandSalesReturnStock = 0, grandMoveInStock = 0, grandMoveOutStock = 0,
                        grandToPatientStock = 0, grandDamageStock = 0, grandAdjustmentStock = 0, grandLastMonthSaleStock = 0;

                    foreach (var lineItem in ReportStock.StockReportLineItemData.OrderBy(x => x.LocationName).ThenBy(x => x.SupplierName).ThenBy(x => x.Name))
                    {
                        if (currentLocation != lineItem.LocationName)
                        {
                            if (!string.IsNullOrEmpty(SupplierName))
                            {
                                AddSubtotalRow(StockListTable, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0; locationPurchaseStock = 0;
                                locationPurchaseReturnStock = 0; locationSalesStock = 0; locationSalesReturnStock = 0; locationMoveInStock = 0; locationMoveOutStock = 0;
                                locationToPatientStock = 0; locationDamageStock = 0; locationAdjustmentStock = 0; locationLastMonthSaleStock = 0;
                                SupplierName = string.Empty;
                            }
                            dataRow = StockListTable.NewRow();
                            dataRow[StockListColumn[(int)StockReportNewTableColumn.SNO]] = $"Location Name : {lineItem.LocationName}";
                            StockListTable.Rows.Add(dataRow);

                            currentLocation = lineItem.LocationName;
                            Sno = 1;
                        }
                        if (SupplierName != lineItem.SupplierName)
                        {
                            if (!string.IsNullOrEmpty(SupplierName))
                            {
                                AddSubtotalRow(StockListTable, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0; locationPurchaseStock = 0;
                                locationPurchaseReturnStock = 0; locationSalesStock = 0; locationSalesReturnStock = 0; locationMoveInStock = 0; locationMoveOutStock = 0;
                                locationToPatientStock = 0; locationDamageStock = 0; locationAdjustmentStock = 0; locationLastMonthSaleStock = 0;
                            }
                            dataRow = StockListTable.NewRow();
                            dataRow[StockListColumn[(int)StockReportNewTableColumn.SNO]] = $"Supplier Name : {lineItem.SupplierName}";
                            StockListTable.Rows.Add(dataRow);

                            SupplierName = lineItem.SupplierName;
                            Sno = 1;
                        }
                        AddDataRow(StockListTable, lineItem, Sno, ReportStock);
                        Sno++;
                        locationOpeningStock += lineItem.OpeningStock; locationOpeningAmount += lineItem.OpeningStockAmount; locationClosingStock += lineItem.CloseingStock; locationClosingAmount += lineItem.ClosingStockAmount;
                        locationPurchaseStock += lineItem.PurchaseQty; locationPurchaseReturnStock += lineItem.PurchaseReturnQty; locationSalesStock += lineItem.SalesQty; locationSalesReturnStock += lineItem.SalesReturnQty;
                        locationMoveInStock += lineItem.StockInQty; locationMoveOutStock += lineItem.StockOutQty; locationToPatientStock += lineItem.ToPatientQty; locationDamageStock += lineItem.DamageQty;
                        locationAdjustmentStock += lineItem.AdjustQty; locationLastMonthSaleStock += lineItem.LastMonthSale;

                        grandOpeningStock += lineItem.OpeningStock; grandOpeningAmount += lineItem.OpeningStockAmount; grandClosingStock += lineItem.CloseingStock; grandClosingAmount += lineItem.ClosingStockAmount;
                        grandPurchaseStock += lineItem.PurchaseQty; grandPurchaseReturnStock += lineItem.PurchaseReturnQty; grandSalesStock += lineItem.SalesQty; grandSalesReturnStock += lineItem.SalesReturnQty;
                        grandMoveInStock += lineItem.StockInQty; grandMoveOutStock += lineItem.StockOutQty; grandToPatientStock += lineItem.ToPatientQty; grandDamageStock += lineItem.DamageQty;
                        grandAdjustmentStock += lineItem.AdjustQty; grandLastMonthSaleStock += lineItem.LastMonthSale;
                    }
                    if (!string.IsNullOrEmpty(SupplierName))
                    {
                        AddSubtotalRow(StockListTable, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);
                    }
                    AddGrandTotalRow(StockListTable, grandOpeningStock, grandOpeningAmount, grandClosingStock, grandClosingAmount, grandPurchaseStock,
                                                grandPurchaseReturnStock, grandSalesStock, grandSalesReturnStock, grandMoveInStock, grandMoveOutStock,
                                                grandToPatientStock, grandDamageStock, grandAdjustmentStock, grandLastMonthSaleStock);
                }
                else if (ReportStock.Type == ComboTypeSelection.BYRACK)
                {
                    int Sno = 1;
                    string currentLocation = string.Empty;
                    string RackNumber = string.Empty;

                    double locationOpeningStock = 0, locationOpeningAmount = 0, locationClosingStock = 0, locationClosingAmount = 0, locationPurchaseStock = 0,
                        locationPurchaseReturnStock = 0, locationSalesStock = 0, locationSalesReturnStock = 0, locationMoveInStock = 0, locationMoveOutStock = 0,
                        locationToPatientStock = 0, locationDamageStock = 0, locationAdjustmentStock = 0, locationLastMonthSaleStock = 0;
                    double grandOpeningStock = 0, grandOpeningAmount = 0, grandClosingStock = 0, grandClosingAmount = 0, grandPurchaseStock = 0,
                        grandPurchaseReturnStock = 0, grandSalesStock = 0, grandSalesReturnStock = 0, grandMoveInStock = 0, grandMoveOutStock = 0,
                        grandToPatientStock = 0, grandDamageStock = 0, grandAdjustmentStock = 0, grandLastMonthSaleStock = 0;

                    foreach (var lineItem in ReportStock.StockReportLineItemData.OrderBy(x => x.LocationName).ThenBy(x => x.RackNumber).ThenBy(x => x.Name))
                    {
                        if (currentLocation != lineItem.LocationName)
                        {
                            if (!string.IsNullOrEmpty(RackNumber))
                            {
                                AddSubtotalRow(StockListTable, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0; locationPurchaseStock = 0;
                                locationPurchaseReturnStock = 0; locationSalesStock = 0; locationSalesReturnStock = 0; locationMoveInStock = 0; locationMoveOutStock = 0;
                                locationToPatientStock = 0; locationDamageStock = 0; locationAdjustmentStock = 0; locationLastMonthSaleStock = 0;
                                RackNumber = string.Empty;
                            }
                            dataRow = StockListTable.NewRow();
                            dataRow[StockListColumn[(int)StockReportNewTableColumn.SNO]] = $"Location Name : {lineItem.LocationName}";
                            StockListTable.Rows.Add(dataRow);

                            currentLocation = lineItem.LocationName;
                            Sno = 1;
                        }
                        if (RackNumber != lineItem.RackNumber)
                        {
                            if (!string.IsNullOrEmpty(RackNumber))
                            {
                                AddSubtotalRow(StockListTable, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0; locationPurchaseStock = 0;
                                locationPurchaseReturnStock = 0; locationSalesStock = 0; locationSalesReturnStock = 0; locationMoveInStock = 0; locationMoveOutStock = 0;
                                locationToPatientStock = 0; locationDamageStock = 0; locationAdjustmentStock = 0; locationLastMonthSaleStock = 0;
                            }
                            dataRow = StockListTable.NewRow();
                            dataRow[StockListColumn[(int)StockReportNewTableColumn.SNO]] = $"Rack Number : {lineItem.RackNumber}";
                            StockListTable.Rows.Add(dataRow);

                            RackNumber = lineItem.RackNumber;
                            Sno = 1;
                        }
                        AddDataRow(StockListTable, lineItem, Sno, ReportStock);
                        Sno++;
                        locationOpeningStock += lineItem.OpeningStock; locationOpeningAmount += lineItem.OpeningStockAmount; locationClosingStock += lineItem.CloseingStock; locationClosingAmount += lineItem.ClosingStockAmount;
                        locationPurchaseStock += lineItem.PurchaseQty; locationPurchaseReturnStock += lineItem.PurchaseReturnQty; locationSalesStock += lineItem.SalesQty; locationSalesReturnStock += lineItem.SalesReturnQty;
                        locationMoveInStock += lineItem.StockInQty; locationMoveOutStock += lineItem.StockOutQty; locationToPatientStock += lineItem.ToPatientQty; locationDamageStock += lineItem.DamageQty;
                        locationAdjustmentStock += lineItem.AdjustQty; locationLastMonthSaleStock += lineItem.LastMonthSale;

                        grandOpeningStock += lineItem.OpeningStock; grandOpeningAmount += lineItem.OpeningStockAmount; grandClosingStock += lineItem.CloseingStock; grandClosingAmount += lineItem.ClosingStockAmount;
                        grandPurchaseStock += lineItem.PurchaseQty; grandPurchaseReturnStock += lineItem.PurchaseReturnQty; grandSalesStock += lineItem.SalesQty; grandSalesReturnStock += lineItem.SalesReturnQty;
                        grandMoveInStock += lineItem.StockInQty; grandMoveOutStock += lineItem.StockOutQty; grandToPatientStock += lineItem.ToPatientQty; grandDamageStock += lineItem.DamageQty;
                        grandAdjustmentStock += lineItem.AdjustQty; grandLastMonthSaleStock += lineItem.LastMonthSale;
                    }
                    if (!string.IsNullOrEmpty(RackNumber))
                    {
                        AddSubtotalRow(StockListTable, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);
                    }
                    AddGrandTotalRow(StockListTable, grandOpeningStock, grandOpeningAmount, grandClosingStock, grandClosingAmount, grandPurchaseStock,
                                                grandPurchaseReturnStock, grandSalesStock, grandSalesReturnStock, grandMoveInStock, grandMoveOutStock,
                                                grandToPatientStock, grandDamageStock, grandAdjustmentStock, grandLastMonthSaleStock);
                }
            }
            return StockListTable;
        }
        private static void AddDataRow(DataTable table, StockReportLineItemsData lineItem, int Sno, RptStockReport rptStockReport)
        {
            DataRow dataRow = table.NewRow();
            dataRow[StockListColumn[(int)StockReportNewTableColumn.SNO]] = Sno;
            dataRow[StockListColumn[(int)StockReportNewTableColumn.MID]] = lineItem.MaterialId;
            dataRow[StockListColumn[(int)StockReportNewTableColumn.PNAME]] = lineItem.Name;
            dataRow[StockListColumn[(int)StockReportNewTableColumn.UOM]] = lineItem.Uom;
            if (rptStockReport.Type != ComboTypeSelection.BYRACK && Global.Company.MaintainRackNumber)
            {
                dataRow[StockListColumn[(int)StockReportNewTableColumn.RACKNO]] = lineItem.RackNumber;
            }
            if(lineItem.Batch != string.Empty)
            {
                dataRow[StockListColumn[(int)StockReportNewTableColumn.BAT]] = lineItem.Batch + Environment.NewLine + String.Format("{0:d}", lineItem.BatchExpDate);
            }
            else
            {
                dataRow[StockListColumn[(int)StockReportNewTableColumn.BAT]] = "";
            }
            dataRow[StockListColumn[(int)StockReportNewTableColumn.OPS]] = lineItem.OpeningStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            dataRow[StockListColumn[(int)StockReportNewTableColumn.OPA]] = lineItem.OpeningStockAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            dataRow[StockListColumn[(int)StockReportNewTableColumn.CLSTK]] = lineItem.CloseingStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            dataRow[StockListColumn[(int)StockReportNewTableColumn.CSA]] = lineItem.ClosingStockAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            dataRow[StockListColumn[(int)StockReportNewTableColumn.P_QTY]] = lineItem.PurchaseQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            dataRow[StockListColumn[(int)StockReportNewTableColumn.PR_QTY]] = lineItem.PurchaseReturnQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            dataRow[StockListColumn[(int)StockReportNewTableColumn.S_QTY]] = lineItem.SalesQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            dataRow[StockListColumn[(int)StockReportNewTableColumn.SR_QTY]] = lineItem.SalesReturnQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            dataRow[StockListColumn[(int)StockReportNewTableColumn.STIN_QTY]] = lineItem.StockInQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            dataRow[StockListColumn[(int)StockReportNewTableColumn.STOT_QTY]] = lineItem.StockOutQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            dataRow[StockListColumn[(int)StockReportNewTableColumn.PCON]] = lineItem.ToPatientQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            dataRow[StockListColumn[(int)StockReportNewTableColumn.DAM]] = lineItem.DamageQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            dataRow[StockListColumn[(int)StockReportNewTableColumn.AD_QTY]] = lineItem.AdjustQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            dataRow[StockListColumn[(int)StockReportNewTableColumn.LASTMONTHSALE]] = lineItem.LastMonthSale.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            table.Rows.Add(dataRow);
        }
        private static void AddSubtotalRow(DataTable table, double locationOpeningStock, double locationOpeningAmount, double locationClosingStock, double locationClosingAmount,
                                                                double locationPurchaseStock, double locationPurchaseReturnStock, double locationSalesStock, double locationSalesReturnStock,
                                                                double locationMoveInStock, double locationMoveOutStock, double locationToPatientStock, double locationDamageStock,
                                                                double locationAdjustmentStock, double locationLastMonthSaleStock)
        {
            DataRow subtotalRow = table.NewRow();
            subtotalRow[StockListColumn[(int)StockReportNewTableColumn.BAT]] = "Subtotal";
            subtotalRow[StockListColumn[(int)StockReportNewTableColumn.OPS]] = locationOpeningStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            subtotalRow[StockListColumn[(int)StockReportNewTableColumn.OPA]] = locationOpeningAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            subtotalRow[StockListColumn[(int)StockReportNewTableColumn.CLSTK]] = locationClosingStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            subtotalRow[StockListColumn[(int)StockReportNewTableColumn.CSA]] = locationClosingAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            subtotalRow[StockListColumn[(int)StockReportNewTableColumn.P_QTY]] = locationPurchaseStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            subtotalRow[StockListColumn[(int)StockReportNewTableColumn.PR_QTY]] = locationPurchaseReturnStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            subtotalRow[StockListColumn[(int)StockReportNewTableColumn.S_QTY]] = locationSalesStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            subtotalRow[StockListColumn[(int)StockReportNewTableColumn.SR_QTY]] = locationSalesReturnStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            subtotalRow[StockListColumn[(int)StockReportNewTableColumn.STIN_QTY]] = locationMoveInStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            subtotalRow[StockListColumn[(int)StockReportNewTableColumn.STOT_QTY]] = locationMoveOutStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            subtotalRow[StockListColumn[(int)StockReportNewTableColumn.PCON]] = locationToPatientStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            subtotalRow[StockListColumn[(int)StockReportNewTableColumn.DAM]] = locationDamageStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            subtotalRow[StockListColumn[(int)StockReportNewTableColumn.AD_QTY]] = locationAdjustmentStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            subtotalRow[StockListColumn[(int)StockReportNewTableColumn.LASTMONTHSALE]] = locationLastMonthSaleStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            table.Rows.Add(subtotalRow);
        }
        private static void AddGrandTotalRow(DataTable table, double grandOpeningStock, double grandOpeningAmount, double grandClosingStock, double grandClosingAmount,
                                                                  double grandPurchaseStock, double grandPurchaseReturnStock, double grandSalesStock, double grandSalesReturnStock,
                                                                  double grandMoveInStock, double grandMoveOutStock, double grandToPatientStock, double grandDamageStock,
                                                                  double grandAdjustmentStock, double grandLastMonthSaleStock)
        {
            DataRow grandTotalRow = table.NewRow();
            grandTotalRow[StockListColumn[(int)StockReportNewTableColumn.BAT]] = "Grand Total";
            grandTotalRow[StockListColumn[(int)StockReportNewTableColumn.OPS]] = grandOpeningStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            grandTotalRow[StockListColumn[(int)StockReportNewTableColumn.OPA]] = grandOpeningAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            grandTotalRow[StockListColumn[(int)StockReportNewTableColumn.CLSTK]] = grandClosingStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            grandTotalRow[StockListColumn[(int)StockReportNewTableColumn.CSA]] = grandClosingAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            grandTotalRow[StockListColumn[(int)StockReportNewTableColumn.P_QTY]] = grandPurchaseStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            grandTotalRow[StockListColumn[(int)StockReportNewTableColumn.PR_QTY]] = grandPurchaseReturnStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            grandTotalRow[StockListColumn[(int)StockReportNewTableColumn.S_QTY]] = grandSalesStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            grandTotalRow[StockListColumn[(int)StockReportNewTableColumn.SR_QTY]] = grandSalesReturnStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            grandTotalRow[StockListColumn[(int)StockReportNewTableColumn.STIN_QTY]] = grandMoveInStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            grandTotalRow[StockListColumn[(int)StockReportNewTableColumn.STOT_QTY]] = grandMoveOutStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            grandTotalRow[StockListColumn[(int)StockReportNewTableColumn.PCON]] = grandToPatientStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            grandTotalRow[StockListColumn[(int)StockReportNewTableColumn.DAM]] = grandDamageStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            grandTotalRow[StockListColumn[(int)StockReportNewTableColumn.AD_QTY]] = grandAdjustmentStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            grandTotalRow[StockListColumn[(int)StockReportNewTableColumn.LASTMONTHSALE]] = grandLastMonthSaleStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            table.Rows.Add(grandTotalRow);
        }

        private static readonly Font FNIB7Font = new Font(PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black"));
        public void GeneratePDFStocReport(DataTable DataTable, RptStockReport ReportStock, string fileExtension, bool isPrint)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                double A4Height = 540;
                Document pdfDoc = new Document(PageSize.A4.Rotate(), -45, -45, 20, 20);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();

                PdfPageHeader PdfHeader = new PdfPageHeader()
                {
                    PaperTypes = PaperTypes.A4_LANDSCAPE,
                    IsMainHeader = true,
                    Islogo = true,
                    IsAddress = true,
                    IsPhone = true,
                    IsEmail = true,
                    IsWebsite = true,
                    IsLicenceInfo = true,
                    ReportLine1 = ReportStock.ReportTitle(),
                    ReportLine2 = ReportStock.ReportHeader
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
                    ReportLine1 = ReportStock.ReportTitle(),
                    ReportLine2 = ReportStock.ReportHeader
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();
                PdfTableHeader PdfTableHeader = new PdfTableHeader();

                string headerType = (ReportStock.Type == ComboTypeSelection.BYRACK) ? "StockReportRackWise" : !Global.Company.MaintainRackNumber ? "StockReportRackWise" : "StockReport";
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(DataTable, headerType);
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = DataTable.Columns.Count;
                int Rows = DataTable.Rows.Count;

                PdfPTable ReportMainTable = new PdfPTable((Global.Company.MaintainRackNumber && ReportStock.Type != ComboTypeSelection.BYRACK) ? DataTable.Columns.Count : !Global.Company.MaintainRackNumber ? DataTable.Columns.Count : DataTable.Columns.Count);
                //PdfPTable ReportMainTable = new PdfPTable(Global.Company.MaintainRackNumber ? DataTable.Columns.Count : DataTable.Columns.Count - 1);
                float[] widths = new float[] { 10f, 25f, 50f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f };
                if (Global.Company.MaintainRackNumber && ReportStock.Type != ComboTypeSelection.BYRACK)
                {
                    widths = new float[] { 10f, 25f, 50f, 20f, 18f, 22f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f };
                }
                ReportMainTable.SetWidths(widths);

                int k = 2;
                BaseColor[] RowColor = new BaseColor[2];
                RowColor[0] = new BaseColor(255, 255, 255);
                RowColor[1] = new BaseColor(250, 250, 250);
                Cursor.Current = Cursors.WaitCursor;

                for (int i = 0; i < Rows; i++)
                {
                    PdfPCell RowCell = new PdfPCell();
                    double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportMainTable));
                    if (TotalWorkingOnPageH > A4Height)
                    {
                        pdfDoc.Add(ReportMainTable);
                        pdfDoc.NewPage();
                        pdfDoc.Add(MiniHTable);
                        pdfDoc.Add(MTable);
                        ReportMainTable = new PdfPTable(Global.Company.MaintainRackNumber ? DataTable.Columns.Count : DataTable.Columns.Count);
                        ReportMainTable.SetWidths(widths);
                        k = 2;
                    }
                    BaseColor CurRowColor = RowColor[k % 2];
                    bool removeBorderConditionForSubTotal = string.IsNullOrEmpty(DataTable.Rows[i][0].ToString())&& !string.IsNullOrEmpty(DataTable.Rows[i][6].ToString());
                    bool removeBorderConditionForSubTitle = !string.IsNullOrEmpty(DataTable.Rows[i][0].ToString()) && string.IsNullOrEmpty(DataTable.Rows[i][7].ToString());
                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = DataTable.Rows[i][j].ToString();
                        RowCell = new PdfPCell(new Phrase(Temp, FNIB7Font));
                        RowCell.BorderColor = new BaseColor(160, 160, 160);
                        RowCell.BackgroundColor = CurRowColor;
                        if (DataTable.Columns[j].ColumnName == "#" || DataTable.Columns[j].ColumnName == "Product Code" || DataTable.Columns[j].ColumnName == "Product Name" || DataTable.Columns[j].ColumnName == "UOM" || DataTable.Columns[j].ColumnName == "Batch Expiry")
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        if (removeBorderConditionForSubTotal)
                        {
                            if(Cols == 20)
                            {
                                if (j >= 1 && j <= 4)
                                {
                                    RowCell.BorderWidthRight = 0f;
                                    RowCell.BorderWidthLeft = 0f;
                                }
                                if (j == 0)
                                {
                                    RowCell.BorderWidthRight = 0f;
                                }
                            }
                            else
                            {
                                if (j >= 1 && j <= 3)
                                {
                                    RowCell.BorderWidthRight = 0f;
                                    RowCell.BorderWidthLeft = 0f;
                                }
                                if (j == 0)
                                {
                                    RowCell.BorderWidthRight = 0f;
                                }
                            }
                        }
                        if ((removeBorderConditionForSubTotal) || (removeBorderConditionForSubTitle))
                        {
                            RowCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        }
                        if (j == 0 && string.IsNullOrEmpty(DataTable.Rows[i][6].ToString()))
                        {
                            RowCell.Colspan = Global.Company.MaintainRackNumber ? DataTable.Columns.Count : DataTable.Columns.Count;
                        }
                        if (j != 0 && string.IsNullOrEmpty(DataTable.Rows[i][6].ToString()))
                        {
                            continue;
                        }
                        ReportMainTable.AddCell(RowCell);
                    }
                    k++;
                }
                Cursor.Current = Cursors.Default;

                pdfDoc.Add(ReportMainTable);
                pdfDoc.Close();

                PdfFooter PdfFooter = new PdfFooter();
                PdfFooter.IsReport = true;
                PdfFooter.IsDate = true;
                PdfFooter.Text = string.Empty;
                PdfFooter.IsPageNumber = true;
                PdfFooter.IsLandScape = true;
                PdfFooter.PdfFile = myMemoryStream.ToArray();
                byte[] PdfFileWithFooter = PdfFooter.GetPdfFileWithFooter();
                myMemoryStream.Close();

                Cursor.Current = Cursors.WaitCursor;
                PdfGeneration PdfGeneration = new PdfGeneration();
                PdfGeneration.IsPrint = isPrint;
                PdfGeneration.FileName = ReportStock.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
            }
        }

    }
}
