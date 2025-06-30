using fa.model.Hms.Master;
using fa.views.utils.Common;
using Fa.api.Hms;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Data;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using VisioForge.Libs.Accord.Math;
using NPOI.HSSF.Util;
using BorderStyle = NPOI.SS.UserModel.BorderStyle;
using fa.api.utils;
using fa.api;
using fa.Data;

namespace Fa.views.utils.Report.Upload
{
    internal class ProcedureExportFile
    {
        public void GenerateFile(long CompanyId)
        {
            Cursor.Current = Cursors.WaitCursor;
            int i = 0;

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("ProcedureDetails");

            HSSFCellStyle hStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            hStyle.FillPattern = FillPattern.SolidForeground;
            hStyle.FillForegroundColor = HSSFColor.Grey25Percent.Index;
            hStyle.BorderBottom = BorderStyle.Medium;

            IRow headerRow = sheet.CreateRow(i++);
            headerRow.CreateCell(0).SetCellValue("MedicalProcedureCategory");
            headerRow.CreateCell(1).SetCellValue("CategoryDisplayName");
            headerRow.CreateCell(2).SetCellValue("CategoryDescription");
            headerRow.CreateCell(3).SetCellValue("IsSubMedicalProcedureCategory");
            headerRow.CreateCell(4).SetCellValue("ParentCategoryId");

            headerRow.CreateCell(5).SetCellValue("ProcedureCode");
            headerRow.CreateCell(6).SetCellValue("Name");
            headerRow.CreateCell(7).SetCellValue("DisplayName");
            headerRow.CreateCell(8).SetCellValue("Description");
            headerRow.CreateCell(9).SetCellValue("Is Active");
            headerRow.CreateCell(10).SetCellValue("Reason For InActive");
            headerRow.CreateCell(11).SetCellValue("Fee");
            headerRow.CreateCell(12).SetCellValue("Keywords");

            foreach (var cell in headerRow.Cells)
            {
                cell.CellStyle = hStyle;
            }
            IList<MedicalProcedure> MedicalProcedures = MedicalProcedureManager.Instance.ListMedicalProcedureByCompanyId(CompanyId);
            if (MedicalProcedures != null && MedicalProcedures.Count > 0)
            {

                foreach (MedicalProcedure MedicalProcedure in MedicalProcedures)
                {

                    IRow dataRow = sheet.CreateRow(i++);
                    dataRow.CreateCell(0).SetCellValue(MedicalProcedure.MedicalProcedureCategory.ToString());

                    if (string.IsNullOrEmpty(MedicalProcedure.MedicalProcedureCategory.DisplayAs))
                    {
                        dataRow.CreateCell(1).SetCellValue(MedicalProcedure.MedicalProcedureCategory.Name);
                    }
                    else
                    {
                        dataRow.CreateCell(1).SetCellValue(MedicalProcedure.MedicalProcedureCategory.DisplayAs);
                    }
                    dataRow.CreateCell(2).SetCellValue(MedicalProcedure.MedicalProcedureCategory.Description);
                    dataRow.CreateCell(3).SetCellValue(MedicalProcedure.MedicalProcedureCategory.IsSubMedicalProcedureCategory);
                    dataRow.CreateCell(4).SetCellValue(MedicalProcedure.MedicalProcedureCategory.ParentMedicalProcedureCategoryId.HasValue ? (double)MedicalProcedure.MedicalProcedureCategory.ParentMedicalProcedureCategoryId : 0.0);

                    dataRow.CreateCell(5).SetCellValue(MedicalProcedure.ProcedureCode);
                    dataRow.CreateCell(6).SetCellValue(MedicalProcedure.Name);
                    if (MedicalProcedure.Name != MedicalProcedure.DisplayAs)
                    {
                        dataRow.CreateCell(7).SetCellValue(MedicalProcedure.DisplayAs);
                    }
                    else
                    {
                        dataRow.CreateCell(7).SetCellValue("");
                    }
                    dataRow.CreateCell(8).SetCellValue(MedicalProcedure.Description);
                    dataRow.CreateCell(9).SetCellValue(MedicalProcedure.IsActive);
                    dataRow.CreateCell(10).SetCellValue(MedicalProcedure.Reason);
                    dataRow.CreateCell(11).SetCellValue(MedicalProcedure.Fee);

                    ICell dateCell1 = dataRow.CreateCell(11);
                    dateCell1.SetCellValue(MedicalProcedure.Fee.ToString(TextUtils.DecimalPlace(fa.Global.Company.PrimaryCurrency.RoundingPrecision)));

                    string keywords = string.Join(",", MedicalProcedure.Keywords.Select(k => k.Text));
                    dataRow.CreateCell(12).SetCellValue(keywords);
                }
            }

            for (int columnIndex = 0; columnIndex < 13; columnIndex++)
            {
                sheet.AutoSizeColumn(columnIndex);
            }

            SaveFileDialog sfDlg = new SaveFileDialog();
            sfDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfDlg.Filter = "Excel Workbook (*.xls;*.xlsx)|*.xls;*.xlsx|All Files (*.*)|*.*";
            sfDlg.RestoreDirectory = true;
            sfDlg.FileName = "MedicalProcedureMaster_" + DateTime.Now.ToShortDateString().Replace("/", "-");

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
