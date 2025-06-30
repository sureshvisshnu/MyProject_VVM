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
using BorderStyle = NPOI.SS.UserModel.BorderStyle;
using NPOI.HSSF.Util;

namespace Fa.views.utils.Report.Upload
{
    internal class MedicalTestExportFile
    {
        public void GenerateFile(long CompanyId)
        {
            Cursor.Current = Cursors.WaitCursor;
            int i = 0;

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Medical Test Data");

            HSSFCellStyle hStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            hStyle.FillPattern = FillPattern.SolidForeground;
            hStyle.FillForegroundColor = HSSFColor.Grey25Percent.Index;
            hStyle.BorderBottom = BorderStyle.Medium;

            IRow headerRow = sheet.CreateRow(i++);
            headerRow.CreateCell(0).SetCellValue("MedicalTestCategory");
            headerRow.CreateCell(1).SetCellValue("CategoryDisplayName");
            headerRow.CreateCell(2).SetCellValue("CategoryDescription");
            headerRow.CreateCell(3).SetCellValue("IsSubMedicalTestCategory");
            headerRow.CreateCell(4).SetCellValue("ParentCategory");
            headerRow.CreateCell(5).SetCellValue("TestName");
            headerRow.CreateCell(6).SetCellValue("DisplayAs");
            headerRow.CreateCell(7).SetCellValue("TestCode");
            headerRow.CreateCell(8).SetCellValue("TestShortName");
            headerRow.CreateCell(9).SetCellValue("TestDescription");
            headerRow.CreateCell(10).SetCellValue("SampleRequirement");
            headerRow.CreateCell(11).SetCellValue("Active");
            headerRow.CreateCell(12).SetCellValue("Reason");
            headerRow.CreateCell(13).SetCellValue("HasElement");
            headerRow.CreateCell(14).SetCellValue("Keywords");
            headerRow.CreateCell(15).SetCellValue("ElementCode");
            headerRow.CreateCell(16).SetCellValue("ElementName");
            headerRow.CreateCell(17).SetCellValue("ElementShortName");
            headerRow.CreateCell(18).SetCellValue("Class");
            headerRow.CreateCell(19).SetCellValue("SubClass");
            headerRow.CreateCell(20).SetCellValue("UOM");
            headerRow.CreateCell(21).SetCellValue("UomDescription");
            headerRow.CreateCell(22).SetCellValue("RangeFrom");
            headerRow.CreateCell(23).SetCellValue("RangeTo");
            headerRow.CreateCell(24).SetCellValue("SingleValue");
            headerRow.CreateCell(25).SetCellValue("TimetoResult");
            headerRow.CreateCell(26).SetCellValue("ElementDescription");

            foreach (var cell in headerRow.Cells)
            {
                cell.CellStyle = hStyle;
            }
            List<MedicalTestCategory> medicalTestCategorys = MedicalTestManager.Instance.GetMedicalTestCatagoryByCompanyId(fa.Global.Company.CompanyId);
            foreach (MedicalTestCategory lCatagory in medicalTestCategorys)
            {
                List<MedicalTestCategory> ChildCategorys = MedicalTestManager.Instance.GetChildMedicalTestCategoryByParentId(lCatagory.Id, fa.Global.Company.CompanyId);
                if (ChildCategorys != null && ChildCategorys.Count > 0)
                {
                    foreach (MedicalTestCategory childCategory in ChildCategorys)
                    {
                        CreateExcellCells(childCategory, sheet, ref i);
                    }
                }
                CreateExcellCells(lCatagory, sheet, ref i);
            }
            for (int columnIndex = 0; columnIndex < 20; columnIndex++)
            {
                //sheet.AutoSizeColumn(columnIndex);
            }
            SaveFileDialog sfDlg = new SaveFileDialog();
            sfDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfDlg.Filter = "Excel Workbook (*.xls;*.xlsx)|*.xls;*.xlsx|All Files (*.*)|*.*";
            sfDlg.RestoreDirectory = true;
            sfDlg.FileName = "MedicalTestMaster_" + DateTime.Now.ToShortDateString().Replace("/", "-");

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
            Cursor.Current = Cursors.Default;
        }
        public void CreateExcellCells(MedicalTestCategory Catagory, ISheet sheet, ref int i)
        {
            List<MedicalTest> lMedicalTest = MedicalTestManager.Instance.GetMedicalTestByCategoryId(Catagory.Id, fa.Global.Company.CompanyId);

            if (lMedicalTest != null && lMedicalTest.Count > 0)
            {
                foreach (MedicalTest MedicalTest in lMedicalTest)
                {
                    if (MedicalTest.HasElement && MedicalTest.TestElements != null && MedicalTest.TestElements.Count > 0)
                    {
                        foreach (MedicalTestElement Element in MedicalTest.TestElements)
                        {
                            IRow dataRow = sheet.CreateRow(i++);

                            dataRow.CreateCell(0).SetCellValue(MedicalTest.MedicalTestCategory.Name ?? "");
                            dataRow.CreateCell(1).SetCellValue(string.IsNullOrEmpty(MedicalTest.MedicalTestCategory.DisplayAs) ? MedicalTest.MedicalTestCategory.Name : MedicalTest.MedicalTestCategory.DisplayAs);
                            dataRow.CreateCell(2).SetCellValue(MedicalTest.MedicalTestCategory.Description);
                            dataRow.CreateCell(3).SetCellValue(MedicalTest.MedicalTestCategory.IsSubMedicalTestCategory);
                            dataRow.CreateCell(4).SetCellValue(Catagory.ParentMedicalTestCategory != null ? Catagory.ParentMedicalTestCategory.Name : "");
                            dataRow.CreateCell(5).SetCellValue(MedicalTest.Name);
                            dataRow.CreateCell(6).SetCellValue(MedicalTest.Name != MedicalTest.DisplayAs ? MedicalTest.DisplayAs : "");
                            dataRow.CreateCell(7).SetCellValue(MedicalTest.TestCode != null ? MedicalTest.TestCode : "");
                            dataRow.CreateCell(8).SetCellValue(MedicalTest.TestShortName != null ? MedicalTest.TestShortName : "");
                            dataRow.CreateCell(9).SetCellValue(MedicalTest.Description);
                            dataRow.CreateCell(10).SetCellValue(MedicalTest.SampleRequirement);
                            dataRow.CreateCell(11).SetCellValue(MedicalTest.IsActive);
                            dataRow.CreateCell(12).SetCellValue(MedicalTest.Reason);
                            dataRow.CreateCell(13).SetCellValue(MedicalTest.HasElement);

                            string keywords = string.Join(", ", MedicalTest.Keywords.Select(k => k.Text));
                            dataRow.CreateCell(14).SetCellValue(keywords);

                            dataRow.CreateCell(15).SetCellValue(Element.ElementCode);
                            dataRow.CreateCell(16).SetCellValue(Element.Name);
                            dataRow.CreateCell(17).SetCellValue(Element.ElementShortName);
                            dataRow.CreateCell(18).SetCellValue(Element.Class);
                            dataRow.CreateCell(19).SetCellValue(Element.SubClass);
                            dataRow.CreateCell(20).SetCellValue(Element.Uom != null ? Element.Uom.Name : "");
                            dataRow.CreateCell(21).SetCellValue(Element.Uom != null ? Element.Uom.Discription : "");
                            dataRow.CreateCell(22).SetCellValue(Element.RangeFrom);
                            dataRow.CreateCell(23).SetCellValue(Element.RangeTo);
                            dataRow.CreateCell(24).SetCellValue(Element.SingleValue);
                            dataRow.CreateCell(25).SetCellValue(Element.ResultDuration);
                            dataRow.CreateCell(26).SetCellValue(Element.Description);
                        }
                    }
                    else
                    {
                        IRow dataRow = sheet.CreateRow(i++);
                        dataRow.CreateCell(0).SetCellValue(MedicalTest.MedicalTestCategory.Name ?? "");
                        dataRow.CreateCell(1).SetCellValue(string.IsNullOrEmpty(MedicalTest.MedicalTestCategory.DisplayAs) ? MedicalTest.MedicalTestCategory.Name : MedicalTest.MedicalTestCategory.DisplayAs);
                        dataRow.CreateCell(2).SetCellValue(MedicalTest.MedicalTestCategory.Description);
                        dataRow.CreateCell(3).SetCellValue(MedicalTest.MedicalTestCategory.IsSubMedicalTestCategory);
                        dataRow.CreateCell(4).SetCellValue(Catagory.ParentMedicalTestCategory != null ? Catagory.ParentMedicalTestCategory.Name : "");
                        dataRow.CreateCell(5).SetCellValue(MedicalTest.Name);
                        dataRow.CreateCell(6).SetCellValue(MedicalTest.Name != MedicalTest.DisplayAs ? MedicalTest.DisplayAs : "");
                        dataRow.CreateCell(7).SetCellValue(MedicalTest.TestCode != null ? MedicalTest.TestCode : "");
                        dataRow.CreateCell(8).SetCellValue(MedicalTest.TestShortName != null ? MedicalTest.TestShortName : "");
                        dataRow.CreateCell(9).SetCellValue(MedicalTest.Description);
                        dataRow.CreateCell(10).SetCellValue(MedicalTest.SampleRequirement);
                        dataRow.CreateCell(11).SetCellValue(MedicalTest.IsActive);
                        dataRow.CreateCell(12).SetCellValue(MedicalTest.Reason);
                        dataRow.CreateCell(13).SetCellValue(MedicalTest.HasElement);

                        string keywords = string.Join(", ", MedicalTest.Keywords.Select(k => k.Text));
                        dataRow.CreateCell(14).SetCellValue(keywords);
                    }
                }
            }
        }
    }
}