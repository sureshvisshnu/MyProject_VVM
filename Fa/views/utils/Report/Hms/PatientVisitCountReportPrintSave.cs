using fa;
using fa.views.utils.Common;
using fa.views.utils;
using Fa.reports.Hms;
using Fa.reports.Inventory;
using Fa.reports.sales;
using FADataAccessLibrary.report.Hms;
using FADataAccessLibrary.report.Inventory;
using FADataAccessLibrary.report.sales;
using iTextSharp.text.pdf;
using iTextSharp.text;
using OpenCvSharp.Dnn;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa.api.utils;
using fa.reports.sales;
using Rectangle = iTextSharp.text.Rectangle;
using fa.api.Accounting;
using NPOI.SS.UserModel;
using fa.model.Employee;
using Fa.api.Hms;
using fa.model.Hms.Master;
using BorderStyle = NPOI.SS.UserModel.BorderStyle;


namespace Fa.views.utils.Report.Hms
{
    internal class PatientVisitCountReportPrintSave
    {
        public bool ExportOrPrintToFile(RptPatientVisitCounting RptPatientVisitCounting, string ReportName, string fileExtension, bool isPrint)
        {
            if (RptPatientVisitCounting != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewAsDataTable(RptPatientVisitCounting);
                            if (DataTable != null)
                            {
                                GeneratePDF(DataTable, RptPatientVisitCounting, ReportName, fileExtension, isPrint, RptPatientVisitCounting.FromDate.ToString(Global.Company.DateFormat), RptPatientVisitCounting.ToDate.ToString(Global.Company.DateFormat));
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
        readonly String[] PatientVisitCountingReportByDateTableColumn = new String[]
        {
            "#","Date","Male Adult","Female Adult","Male Child","Female Child","Others","Total",
        };
        readonly String[] PatientVisitCountingReportByDeptTableColumn = new String[]
        {
            "#","Date","Male Adult","Female Adult","Male Child","Female Child","Others","Total",
        };
        readonly String[] PatientVisitCountingReportByDigaTableColumn = new String[]
        {
            "#","Date","Male Adult","Female Adult","Male Child","Female Child","Others","Total",
        };
        public DataTable DataGridViewAsDataTable(RptPatientVisitCounting RptPatientVisitCounting)
        {
            if (RptPatientVisitCounting.Type == PatientVisitCountingType.BYDATE)
            {
                DataTable PatientVisitCountByDateColumn = new DataTable();
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.SNO], typeof(string));
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.DATE], typeof(string));
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.MALE_ADULT], typeof(string));
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.FEMALE_ADULT], typeof(string));
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.MALE_CHILD], typeof(string));
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.FEMALE_CHILD], typeof(string));
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.OTHERS], typeof(string));
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.TOTAL], typeof(string));

                int Sno = 1;
                double MaleAdultTotal = 0, FemaleAdultTotal = 0, MaleChildTotal = 0, FemaleChildTotal = 0, OthersTotal = 0;
                string previousDateTime = null;
                DataRow currentRow = null;

                foreach (PatientVisitCountingLineItem LineItem in RptPatientVisitCounting.PatientVisitCountingLineItems.OrderBy(x => x.DateOfVisit))
                {
                    String stringLineItemDate = DateUtils.FormatDate(LineItem.DateOfVisit, Global.Company.DateFormat);

                    if (previousDateTime == null || previousDateTime != stringLineItemDate)
                    {
                        if (currentRow != null)
                        {
                            int RowTotal = Convert.ToInt32(currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.MALE_ADULT]] ?? 0) +
                                           Convert.ToInt32(currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.FEMALE_ADULT]] ?? 0) +
                                           Convert.ToInt32(currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.MALE_CHILD]] ?? 0) +
                                           Convert.ToInt32(currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.FEMALE_CHILD]] ?? 0) +
                                           Convert.ToInt32(currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.OTHERS]] ?? 0);

                            currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.TOTAL]] = RowTotal;
                            PatientVisitCountByDateColumn.Rows.Add(currentRow);
                        }
                        currentRow = PatientVisitCountByDateColumn.NewRow();
                        currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.SNO]] = Sno;
                        currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.DATE]] = LineItem.DateOfVisit.ToString(Global.Company.DateFormat);
                        previousDateTime = stringLineItemDate;
                        Sno++;
                    }
                    currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.MALE_ADULT]] =
                        (Convert.ToInt32(currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.MALE_ADULT]] == DBNull.Value
                            ? 0
                            : currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.MALE_ADULT]]))
                        + LineItem.MaleAdult;
                    MaleAdultTotal += LineItem.MaleAdult;

                    currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.FEMALE_ADULT]] =
                        (Convert.ToInt32(currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.FEMALE_ADULT]] == DBNull.Value
                            ? 0
                            : currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.FEMALE_ADULT]]))
                        + LineItem.FemaleAdult;
                    FemaleAdultTotal += LineItem.FemaleAdult;

                    currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.MALE_CHILD]] =
                        (Convert.ToInt32(currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.MALE_CHILD]] == DBNull.Value
                            ? 0
                            : currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.MALE_CHILD]]))
                        + LineItem.MaleChild;
                    MaleChildTotal += LineItem.MaleChild;

                    currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.FEMALE_CHILD]] =
                        (Convert.ToInt32(currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.FEMALE_CHILD]] == DBNull.Value
                            ? 0
                            : currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.FEMALE_CHILD]]))
                        + LineItem.FemaleChild;
                    FemaleChildTotal += LineItem.FemaleChild;

                    currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.OTHERS]] =
                        (Convert.ToInt32(currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.OTHERS]] == DBNull.Value
                            ? 0
                            : currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.OTHERS]]))
                        + LineItem.Others;
                    OthersTotal += LineItem.Others;
                }
                if (currentRow != null)
                {
                    int RowTotal = Convert.ToInt32(currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.MALE_ADULT]] ?? 0) +
                                   Convert.ToInt32(currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.FEMALE_ADULT]] ?? 0) +
                                   Convert.ToInt32(currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.MALE_CHILD]] ?? 0) +
                                   Convert.ToInt32(currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.FEMALE_CHILD]] ?? 0) +
                                   Convert.ToInt32(currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.OTHERS]] ?? 0);

                    currentRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.TOTAL]] = RowTotal;
                    PatientVisitCountByDateColumn.Rows.Add(currentRow);
                }

                double GrandTotal = MaleAdultTotal + FemaleAdultTotal + MaleChildTotal + FemaleChildTotal + OthersTotal;

                DataRow grandTotalRow = PatientVisitCountByDateColumn.NewRow();
                grandTotalRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.OTHERS]] = "Grand Total";
                grandTotalRow[PatientVisitCountingReportByDateTableColumn[(int)PatientVisitCountingByDateTableColumn.TOTAL]] = GrandTotal;

                PatientVisitCountByDateColumn.Rows.Add(grandTotalRow);

                return PatientVisitCountByDateColumn;
            }
            else if (RptPatientVisitCounting.Type == PatientVisitCountingType.BYDEPARTMENT)
            {
                DataTable PatientVisitCountByDateColumn = new DataTable();
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.SNO], typeof(string));
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.DATE], typeof(string));
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.MALE_ADULT], typeof(string));
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.FEMALE_ADULT], typeof(string));
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.MALE_CHILD], typeof(string));
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.FEMALE_CHILD], typeof(string));
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.OTHERS], typeof(string));
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.TOTAL], typeof(string));

                int Sno = 1;
                double MaleAdultTotal = 0, FemaleAdultTotal = 0, MaleChildTotal = 0, FemaleChildTotal = 0, OthersTotal = 0;
                string previousDateTime = null;
                DataRow currentRow = null;
                DataRow SubTitleRow = null;
                long DeptId = 0;

                foreach (var group in RptPatientVisitCounting.PatientVisitCountingLineItems.GroupBy(x => x.DeptId))
                {
                    foreach (PatientVisitCountingLineItem LineItem in group)
                    {
                        String stringLineItemDate = DateUtils.FormatDate(LineItem.DateOfVisit, Global.Company.DateFormat);
                        if (DeptId != LineItem.DeptId)
                        {
                            SubTitleRow = PatientVisitCountByDateColumn.NewRow();
                            Department Department = DepartmentManager.GetDepartmentInfoByIdForReport(LineItem.DeptId);
                            if (Department != null)
                            {
                                SubTitleRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.SNO]] = "Department Name : " + Department.Name;
                                DeptId = LineItem.DeptId;
                                PatientVisitCountByDateColumn.Rows.Add(SubTitleRow);
                            }
                        }

                        if (previousDateTime == null || previousDateTime != stringLineItemDate)
                        {
                            if (currentRow != null)
                            {
                                int RowTotal = Convert.ToInt32(currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.MALE_ADULT]] ?? 0) +
                                               Convert.ToInt32(currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.FEMALE_ADULT]] ?? 0) +
                                               Convert.ToInt32(currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.MALE_CHILD]] ?? 0) +
                                               Convert.ToInt32(currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.FEMALE_CHILD]] ?? 0) +
                                               Convert.ToInt32(currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.OTHERS]] ?? 0);

                                currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.TOTAL]] = RowTotal;
                                PatientVisitCountByDateColumn.Rows.Add(currentRow);
                            }
                            currentRow = PatientVisitCountByDateColumn.NewRow();
                            currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.SNO]] = Sno;
                            currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.DATE]] = LineItem.DateOfVisit.ToString(Global.Company.DateFormat);
                            previousDateTime = stringLineItemDate;
                            Sno++;
                        }
                        currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.MALE_ADULT]] =
                            (Convert.ToInt32(currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.MALE_ADULT]] == DBNull.Value
                                ? 0
                                : currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.MALE_ADULT]]))
                            + LineItem.MaleAdult;
                        MaleAdultTotal += LineItem.MaleAdult;

                        currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.FEMALE_ADULT]] =
                            (Convert.ToInt32(currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.FEMALE_ADULT]] == DBNull.Value
                                ? 0
                                : currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.FEMALE_ADULT]]))
                            + LineItem.FemaleAdult;
                        FemaleAdultTotal += LineItem.FemaleAdult;

                        currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.MALE_CHILD]] =
                            (Convert.ToInt32(currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.MALE_CHILD]] == DBNull.Value
                                ? 0
                                : currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.MALE_CHILD]]))
                            + LineItem.MaleChild;
                        MaleChildTotal += LineItem.MaleChild;

                        currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.FEMALE_CHILD]] =
                            (Convert.ToInt32(currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.FEMALE_CHILD]] == DBNull.Value
                                ? 0
                                : currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.FEMALE_CHILD]]))
                            + LineItem.FemaleChild;
                        FemaleChildTotal += LineItem.FemaleChild;

                        currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.OTHERS]] =
                            (Convert.ToInt32(currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.OTHERS]] == DBNull.Value
                                ? 0
                                : currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.OTHERS]]))
                            + LineItem.Others;
                        OthersTotal += LineItem.Others;
                    }
                    if (currentRow != null)
                    {
                        int RowTotal = Convert.ToInt32(currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.MALE_ADULT]] ?? 0) +
                                       Convert.ToInt32(currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.FEMALE_ADULT]] ?? 0) +
                                       Convert.ToInt32(currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.MALE_CHILD]] ?? 0) +
                                       Convert.ToInt32(currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.FEMALE_CHILD]] ?? 0) +
                                       Convert.ToInt32(currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.OTHERS]] ?? 0);

                        currentRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.TOTAL]] = RowTotal;
                        PatientVisitCountByDateColumn.Rows.Add(currentRow);
                    }
                }
                   
                double GrandTotal = MaleAdultTotal + FemaleAdultTotal + MaleChildTotal + FemaleChildTotal + OthersTotal;

                DataRow grandTotalRow = PatientVisitCountByDateColumn.NewRow();
                grandTotalRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.OTHERS]] = "Grand Total";
                grandTotalRow[PatientVisitCountingReportByDeptTableColumn[(int)PatientVisitCountingByDeptTableColumn.TOTAL]] = GrandTotal;

                PatientVisitCountByDateColumn.Rows.Add(grandTotalRow);

                return PatientVisitCountByDateColumn;
            }
            else
            {
                DataTable PatientVisitCountByDateColumn = new DataTable();
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.SNO], typeof(string));
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.DATE], typeof(string));
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.MALE_ADULT], typeof(string));
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.FEMALE_ADULT], typeof(string));
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.MALE_CHILD], typeof(string));
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.FEMALE_CHILD], typeof(string));
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.OTHERS], typeof(string));
                PatientVisitCountByDateColumn.Columns.Add(PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.TOTAL], typeof(string));

                int Sno = 1;
                double MaleAdultTotal = 0, FemaleAdultTotal = 0, MaleChildTotal = 0, FemaleChildTotal = 0, OthersTotal = 0;
                string previousDateTime = null;
                DataRow currentRow = null;
                DataRow SubTitleRow = null;
                long DiagId = 0;

                foreach (var group in RptPatientVisitCounting.PatientVisitCountingLineItemForDiagnosiss.GroupBy(x => x.DiagId))
                {
                    foreach (PatientVisitCountingLineItemForDiagnosis LineItem in group)
                    {
                        String stringLineItemDate = DateUtils.FormatDate(LineItem.DateOfVisit, Global.Company.DateFormat);
                        if (DiagId != LineItem.DiagId)
                        {
                            SubTitleRow = PatientVisitCountByDateColumn.NewRow();
                            Symptom symptom = SymptomsManager.Instance.GetSymptomsById(LineItem.DiagId);
                            if (symptom != null)
                            {
                                SubTitleRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.SNO]] = "Diagnosis Name : " + symptom.Name;
                                DiagId = LineItem.DiagId;
                                PatientVisitCountByDateColumn.Rows.Add(SubTitleRow);
                            }
                        }

                        if (previousDateTime == null || previousDateTime != stringLineItemDate)
                        {
                            if (currentRow != null)
                            {
                                int RowTotal = Convert.ToInt32(currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.MALE_ADULT]] ?? 0) +
                                               Convert.ToInt32(currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.FEMALE_ADULT]] ?? 0) +
                                               Convert.ToInt32(currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.MALE_CHILD]] ?? 0) +
                                               Convert.ToInt32(currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.FEMALE_CHILD]] ?? 0) +
                                               Convert.ToInt32(currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.OTHERS]] ?? 0);

                                currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.TOTAL]] = RowTotal;
                                PatientVisitCountByDateColumn.Rows.Add(currentRow);
                            }
                            currentRow = PatientVisitCountByDateColumn.NewRow();
                            currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.SNO]] = Sno;
                            currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.DATE]] = LineItem.DateOfVisit.ToString(Global.Company.DateFormat);
                            previousDateTime = stringLineItemDate;
                            Sno++;
                        }
                        currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.MALE_ADULT]] =
                            (Convert.ToInt32(currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.MALE_ADULT]] == DBNull.Value
                                ? 0
                                : currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.MALE_ADULT]]))
                            + LineItem.MaleAdult;
                        MaleAdultTotal += LineItem.MaleAdult;

                        currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.FEMALE_ADULT]] =
                            (Convert.ToInt32(currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.FEMALE_ADULT]] == DBNull.Value
                                ? 0
                                : currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.FEMALE_ADULT]]))
                            + LineItem.FemaleAdult;
                        FemaleAdultTotal += LineItem.FemaleAdult;

                        currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.MALE_CHILD]] =
                            (Convert.ToInt32(currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.MALE_CHILD]] == DBNull.Value
                                ? 0
                                : currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.MALE_CHILD]]))
                            + LineItem.MaleChild;
                        MaleChildTotal += LineItem.MaleChild;

                        currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.FEMALE_CHILD]] =
                            (Convert.ToInt32(currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.FEMALE_CHILD]] == DBNull.Value
                                ? 0
                                : currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.FEMALE_CHILD]]))
                            + LineItem.FemaleChild;
                        FemaleChildTotal += LineItem.FemaleChild;

                        currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.OTHERS]] =
                            (Convert.ToInt32(currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.OTHERS]] == DBNull.Value
                                ? 0
                                : currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.OTHERS]]))
                            + LineItem.Others;
                        OthersTotal += LineItem.Others;
                    }
                    if (currentRow != null)
                    {
                        int RowTotal = Convert.ToInt32(currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.MALE_ADULT]] ?? 0) +
                                       Convert.ToInt32(currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.FEMALE_ADULT]] ?? 0) +
                                       Convert.ToInt32(currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.MALE_CHILD]] ?? 0) +
                                       Convert.ToInt32(currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.FEMALE_CHILD]] ?? 0) +
                                       Convert.ToInt32(currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.OTHERS]] ?? 0);

                        currentRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.TOTAL]] = RowTotal;
                        PatientVisitCountByDateColumn.Rows.Add(currentRow);
                    }
                }

                double GrandTotal = MaleAdultTotal + FemaleAdultTotal + MaleChildTotal + FemaleChildTotal + OthersTotal;

                DataRow grandTotalRow = PatientVisitCountByDateColumn.NewRow();
                grandTotalRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.OTHERS]] = "Grand Total";
                grandTotalRow[PatientVisitCountingReportByDigaTableColumn[(int)PatientVisitCountingByDiagnTableColumn.TOTAL]] = GrandTotal;

                PatientVisitCountByDateColumn.Rows.Add(grandTotalRow);

                return PatientVisitCountByDateColumn;
            }
        }
        public void GeneratePDF(DataTable DataTable, RptPatientVisitCounting RptPatientVisitCounting, string ReportName, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                double A4Height = 760;
                Document pdfDoc = new Document(PageSize.A4, -45, -45, 20, 20);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();

                PdfPageHeader PdfHeader = new PdfPageHeader()
                {
                    IsMainHeader = true,
                    Islogo = true,
                    IsAddress = true,
                    IsPhone = true,
                    IsEmail = true,
                    IsWebsite = true,
                    IsLicenceInfo = true,
                    ReportLine1 = "Patient Visit Counting Report",
                    ReportLine2 = RptPatientVisitCounting.ReportSubTitle()

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
                    ReportLine1 = "Patient Visit Counting Report",
                    ReportLine2 = RptPatientVisitCounting.ReportSubTitle()
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();
                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(DataTable, RptPatientVisitCounting.ReportTitle());
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = DataTable.Columns.Count;
                int Rows = DataTable.Rows.Count - 1;
                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths = new float[] { 10f, 30f, 30f, 30f, 30f, 30f, 30f, 30f };
                ReportMainTable.SetWidths(widths);
                int k = 2;
                BaseColor[] RowColor = new BaseColor[2];
                RowColor[0] = new BaseColor(255, 255, 255);
                RowColor[1] = new BaseColor(250, 250, 250);
                Cursor.Current = Cursors.WaitCursor;
                PdfPCell HeaderCell = new PdfPCell();
                if (RptPatientVisitCounting.Type == PatientVisitCountingType.BYDATE)
                {
                    for (int i = 0; i < DataTable.Rows.Count; i++)
                    {
                        PdfPCell RowCell = new PdfPCell();
                        double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportMainTable));
                        if (TotalWorkingOnPageH > A4Height)
                        {
                            pdfDoc.Add(ReportMainTable);
                            pdfDoc.NewPage();
                            pdfDoc.Add(MiniHTable);
                            pdfDoc.Add(MTable);
                            ReportMainTable = new PdfPTable(DataTable.Columns.Count);
                            ReportMainTable.SetWidths(widths);
                            k = 2;
                        }
                        BaseColor CurRowColor = RowColor[k % 2];
                        for (int j = 0; j < DataTable.Columns.Count; j++)
                        {
                            var Temp = DataTable.Rows[i][j].ToString();

                            RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColor = BaseColor.GRAY;
                            if (i != 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthTop = (float)BorderStyle.None;
                            }
                            if (j != 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            }
                            if (i == DataTable.Rows.Count - 1 && j == 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            if (i == DataTable.Rows.Count - 1 && j > 0 && j < 5)
                            {
                                RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            else
                            {
                                if (i == DataTable.Rows.Count - 1)
                                {
                                    RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                                    RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                }
                                else
                                {
                                    if (DataTable.Columns[j].ColumnName == "#" || DataTable.Columns[j].ColumnName == "Date")
                                    {
                                        RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    }
                                    else
                                    {
                                        RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    }
                                }
                            }
                            ReportMainTable.AddCell(RowCell);
                        }
                        k++;
                    }
                }
                else if(RptPatientVisitCounting.Type == PatientVisitCountingType.BYDEPARTMENT || RptPatientVisitCounting.Type == PatientVisitCountingType.BYDIAGONSIS)
                {
                    for (int i = 0; i < DataTable.Rows.Count; i++)
                    {
                        PdfPCell RowCell = new PdfPCell();
                        double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportMainTable));
                        if (TotalWorkingOnPageH > A4Height)
                        {
                            pdfDoc.Add(ReportMainTable);
                            pdfDoc.NewPage();
                            pdfDoc.Add(MiniHTable);
                            pdfDoc.Add(MTable);
                            ReportMainTable = new PdfPTable(DataTable.Columns.Count);
                            ReportMainTable.SetWidths(widths);
                            k = 2;
                        }
                        BaseColor CurRowColor = RowColor[k % 2];
                        for (int j = 0; j < DataTable.Columns.Count; j++)
                        {
                            var Temp = DataTable.Rows[i][j].ToString();

                            RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColor = BaseColor.GRAY;
                            if (i != 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthTop = (float)BorderStyle.None;
                            }
                            if (j != 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            }
                            if (i == DataTable.Rows.Count - 1 && j == 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            if (i == DataTable.Rows.Count - 1 && j > 0 && j < 5)
                            {
                                RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            else
                            {
                                if (i == DataTable.Rows.Count - 1)
                                {
                                    RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                                    RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                }
                                else
                                {
                                    if (DataTable.Columns[j].ColumnName == "#" || DataTable.Columns[j].ColumnName == "Date")
                                    {
                                        RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    }
                                    else
                                    {
                                        RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    }
                                }
                            }
                            if (j == 0 && string.IsNullOrEmpty(DataTable.Rows[i][7].ToString()))
                            {
                                RowCell.Colspan = DataTable.Columns.Count;
                            }
                            if (j != 0 && string.IsNullOrEmpty(DataTable.Rows[i][7].ToString()))
                            {
                                continue;
                            }
                            ReportMainTable.AddCell(RowCell);
                        }
                        k++;
                    }
                }
                Cursor.Current = Cursors.Default;
                pdfDoc.Add(ReportMainTable);
                pdfDoc.Close();
                PdfFooter PdfFooter = new PdfFooter();
                PdfFooter.IsReport = true;
                PdfFooter.IsDate = true;
                PdfFooter.Text = string.Empty;
                PdfFooter.IsPageNumber = true;
                PdfFooter.PdfFile = myMemoryStream.ToArray();
                byte[] PdfFileWithFooter = PdfFooter.GetPdfFileWithFooter();
                myMemoryStream.Close();

                Cursor.Current = Cursors.WaitCursor;
                PdfGeneration PdfGeneration = new PdfGeneration();
                PdfGeneration.IsPrint = isPrint;
                PdfGeneration.FileName = RptPatientVisitCounting.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
