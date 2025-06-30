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
using NPOI.HSSF.Util;
using BorderStyle = NPOI.SS.UserModel.BorderStyle;
using HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment;
using System.Data;

namespace Fa.views.utils.Report.Upload
{
    internal class SymptomExportFile
    {
        public void GenerateFile(long CompanyId)
        {
            Cursor.Current = Cursors.WaitCursor;

            int i = 0;
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Symptom Data");

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
            headerRow.CreateCell(0).SetCellValue("SymptomCategory");
            headerRow.CreateCell(1).SetCellValue("CategoryDisplayName");
            headerRow.CreateCell(2).SetCellValue("CategoryDescription");
            headerRow.CreateCell(3).SetCellValue("IsSubSymptomCategory");
            headerRow.CreateCell(4).SetCellValue("ParentCategoryId");
            headerRow.CreateCell(5).SetCellValue("SymptomCode");
            headerRow.CreateCell(6).SetCellValue("Name");
            headerRow.CreateCell(7).SetCellValue("DisplayName");
            headerRow.CreateCell(8).SetCellValue("Description");
            headerRow.CreateCell(9).SetCellValue("Active");
            headerRow.CreateCell(10).SetCellValue("Reason");
            headerRow.CreateCell(11).SetCellValue("Keyword");

            foreach (var cell in headerRow.Cells)
            {
                cell.CellStyle = hStyle;
            }

            IList<Symptom> lSymptoms = SymptomsManager.Instance.ListSymptomByCompanyId(CompanyId);
            if (lSymptoms != null && lSymptoms.Count > 0)
            {
                foreach (Symptom symptom in lSymptoms)
                {

                    IRow dataRow = sheet.CreateRow(i++);
                    dataRow.CreateCell(0).SetCellValue(symptom.SymptomCategory.Name);
                    dataRow.CreateCell(1).SetCellValue(string.IsNullOrEmpty(symptom.SymptomCategory.DisplayAs) ? symptom.SymptomCategory.Name
                        : symptom.SymptomCategory.DisplayAs);
                    dataRow.CreateCell(2).SetCellValue(symptom.SymptomCategory.Discription);
                    dataRow.CreateCell(3).SetCellValue(symptom.SymptomCategory.IsSubSymptomCategory);

                    if (symptom.SymptomCategory.ParentSymptomCategoryId != null)
                    {
                        dataRow.CreateCell(4).SetCellValue((long)symptom.SymptomCategory.ParentSymptomCategoryId);
                    }

                    dataRow.CreateCell(5).SetCellValue(symptom.SymptomCode);
                    dataRow.CreateCell(6).SetCellValue(symptom.Name);
                    dataRow.CreateCell(7).SetCellValue(symptom.DisplayAs != symptom.Name
                        ? symptom.DisplayAs
                        : string.Empty);
                    dataRow.CreateCell(8).SetCellValue(symptom.Discription);
                    dataRow.CreateCell(9).SetCellValue(symptom.IsActive);
                    dataRow.CreateCell(10).SetCellValue(symptom.ReasonForInactive);
                    string keywords = string.Join(",", symptom.Keywords.Select(k => k.Text));
                    dataRow.CreateCell(11).SetCellValue(keywords);

                    for (int columnIndex = 0; columnIndex < 11; columnIndex++)
                    {
                        ICell cell = dataRow.GetCell(columnIndex);
                        if (cell != null)
                        {
                            if (columnIndex == 4)
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
            for (int columnIndex = 0; columnIndex < 11; columnIndex++)
            {
                sheet.AutoSizeColumn(columnIndex);
            }
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SymptomData.xls");
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                workbook.Write(fs);

            }
            SaveFileDialog sfDlg = new SaveFileDialog();
            sfDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfDlg.Filter = "Excel Workbook (*.xls;*.xlsx)|*.xls;*.xlsx|All Files (*.*)|*.*";
            sfDlg.RestoreDirectory = true;
            sfDlg.FileName = "Diagonosis_" + DateTime.Now.ToShortDateString().Replace("/", "-");

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