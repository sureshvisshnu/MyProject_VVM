using fa;
using fa.api.Accounting;
using fa.api.Hms;
using fa.model.Employee;
using fa.model.Hms.Master;
using fa.views.utils.Common;
using FADataAccessLibrary.Utils;
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
using HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment;

namespace Fa.views.utils.Report.Upload
{
    internal class ConsultationsExportFile
    {
        public void GenerateFile(long CompanyId)
        {
            Cursor.Current = Cursors.WaitCursor;
            int i = 0;

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Consultations Data");

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
            headerRow.CreateCell(0).SetCellValue("Name");
            headerRow.CreateCell(1).SetCellValue("DisplayName");
            headerRow.CreateCell(2).SetCellValue("Description");
            headerRow.CreateCell(3).SetCellValue("ServiceProviders");
            headerRow.CreateCell(4).SetCellValue("Fees");
            headerRow.CreateCell(5).SetCellValue("DOB");
            headerRow.CreateCell(6).SetCellValue("Title");

            foreach (var cell in headerRow.Cells)
            {
                cell.CellStyle = hStyle;
            }
            IList<Consultation> lConsultations = ConsultationManager.Instance.ListConsultationByCompanyId(CompanyId);
            if (lConsultations != null && lConsultations.Count > 0)
            {
                foreach (Consultation Consultation in lConsultations)
                {
                    foreach (ConsultationDetail Consultationdetails in Consultation.ConsultationDetail)
                    {
                        Employee employee = EmployeeManager.Instance.GetEmployeeInfoById((long)Consultationdetails.EmployeeId!);
                        if (Consultationdetails != null && Consultationdetails.Fee != 0)
                        {
                            IRow dataRow = sheet.CreateRow(i++);
                            dataRow.CreateCell(0).SetCellValue(Consultation.Name);

                            if (Consultation.Name != Consultation.DisplayAs)
                            {
                                dataRow.CreateCell(1).SetCellValue(Consultation.DisplayAs);
                            }
                            else
                            {
                                dataRow.CreateCell(1).SetCellValue(Consultation.Name);
                            }
                            dataRow.CreateCell(2).SetCellValue(Consultation.Discription);
                            dataRow.CreateCell(3).SetCellValue(employee.Name);
                            
                            string fees = Consultationdetails != null ? Consultationdetails.Fee.ToString()! : "0";
                            dataRow.CreateCell(4).SetCellValue(double.Parse(fees).ToString(TextUtils.DecimalPlace(fa.Global.Company.PrimaryCurrency.RoundingPrecision)));

                            string DOB = DateTime.Parse(employee.DateOfBirth.ToString()!).ToString(fa.Global.Company.DateFormat);
                            dataRow.CreateCell(5).SetCellValue(DOB);
                            dataRow.CreateCell(6).SetCellValue(employee.Title.Name.ToString());

                            for (int columnIndex = 0; columnIndex < 5; columnIndex++)
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
                }
            }
            for (int columnIndex = 0; columnIndex < 4; columnIndex++)
            {
                sheet.AutoSizeColumn(columnIndex);
            }
            SaveFileDialog sfDlg = new SaveFileDialog();
            sfDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfDlg.Filter = "Excel Workbook (*.xls;*.xlsx)|*.xls;*.xlsx|All Files (*.*)|*.*";
            sfDlg.RestoreDirectory = true;
            sfDlg.FileName = "Consultations_" + DateTime.Now.ToShortDateString().Replace("/", "-");

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
    }
}