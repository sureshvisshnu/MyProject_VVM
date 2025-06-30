using DocumentFormat.OpenXml.Drawing;
using fa;
using fa.api.Hms;
using fa.api.utils;
using fa.model.Accounting.Transactions;
using fa.model.Common;
using fa.model.hms.common;
using fa.model.Hms.common;
using fa.report.Ip;
using fa.report.sales;
using fa.reports.Hms;
using fa.views.utils;
using fa.views.utils.Common;
using Fa.reports.Hms;
using Fa.reports.Inventory;
using FADataAccessLibrary.report.Hms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NPOI.SS.UserModel;
using System;
using System.Data;
using System.IO;
using System.Reflection.Metadata;
using System.Windows.Forms;
using VisioForge.MediaFramework.ONVIF;
using BorderStyle = System.Windows.Forms.BorderStyle;
using Document = iTextSharp.text.Document;
using Rectangle = iTextSharp.text.Rectangle;

namespace Fa.views.utils.Report.Hms
{
    class ReceiveAmountReportSavePrint
    {
        public bool ExportOrPrintToFile(RptReceiveAmount RptReceiveAmount, string ReportName, string fileExtension, bool isPrint)
        {
            if (RptReceiveAmount != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewAsDataTable(RptReceiveAmount);
                            if (DataTable != null)
                            {
                                GeneratePDF(DataTable, RptReceiveAmount, ReportName, fileExtension, isPrint, RptReceiveAmount.FromDate.ToString(Global.Company.DateFormat), RptReceiveAmount.ToDate.ToString(Global.Company.DateFormat));
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
        readonly String[] ReceiveAmountDataTableColumnByDate = new String[]
        {
            "#","Date","Reference","Patient Name","Op/Ip","Consultant","Description","Payment Type","Received Amount",
        };
        readonly String[] ReceiveAmountDataTableColumnByPatient = new String[]
        {
            "#","Patient Name","Date","Reference","Op/Ip","Description","Consultant","Payment Type","Received Amount",
        };
        readonly String[] ReceiveAmountDataTableColumnByAmount = new String[]
        {
            "#","Date","Patient Name","Reference","Op/Ip","Description","Consultant","Payment Type","Received Amount",
        };
        readonly String[] ReceiveAmountDataTableColumnByConsultant = new String[]
        {
            "#","Consultant","Patient Name","Date","Reference","Op/Ip","Description","Payment Type","Received Amount",
        };
        public DataTable DataGridViewAsDataTable(RptReceiveAmount RptReceiveAmount)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (RptReceiveAmount.Type == InventoryReportFilterType.BYDATE)
            {
                DataTable PaymentReceiveTable = new DataTable();
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.SNO], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.DATE], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.REFERENCE], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.PATIENT_NAME], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.OP_IP], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.CONSULTANT], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.DESCRIPTION], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.PAYMENT_TYPE], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.RECEIVED], typeof(string));

                DataRow PaymentReceiveTableRow = null!;
                string dateTime = null!;
                string Patients = string.Empty;
                string Consultant = string.Empty;
                int rn = 0;
                decimal dateGroupTotal = 0;
                decimal grandTotal = 0;
                int i = 1;
                foreach (ReceiveAmountReportLine LineItem in RptReceiveAmount.ReceiveAmountReportLines.OrderBy(x => x.Date))
                {
                    if (LineItem.Received > 0 && LineItem.RefNumber != null)
                    {
                        IList<PatientLedger> lPatientLedger = PatientLedgerManager.Instance.ListAllEntryByPatientId(LineItem.Patient.Id);
                        PaymentReceiveTableRow = PaymentReceiveTable.NewRow();
                        String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);

                        if (dateTime != null && dateTime != stringLineItemDate)
                        {
                            DataRow subtotalRow = PaymentReceiveTable.NewRow();
                            subtotalRow[ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.PAYMENT_TYPE]] = "Sub Total";
                            subtotalRow[ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.RECEIVED]] = dateGroupTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            PaymentReceiveTable.Rows.Add(subtotalRow);
                            i = 1;
                            dateGroupTotal = 0;
                        }
                        PaymentReceiveTableRow[ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.SNO]] = i;
                        if (dateTime == null || dateTime != stringLineItemDate)
                        {
                            PaymentReceiveTableRow[ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.DATE]] = LineItem.Date.ToString(Global.Company.DateFormat);
                            dateTime = stringLineItemDate;
                            Consultant = string.Empty;
                            Patients = string.Empty;
                        }
                        PaymentReceiveTableRow[ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.REFERENCE]] = LineItem.RefNumber;
                        if (Patients == string.Empty || Patients != LineItem.Patient.Name)
                        {
                            PaymentReceiveTableRow[ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.PATIENT_NAME]] = LineItem.Patient;
                            Patients = LineItem.Patient.Name;
                        }
                        foreach (PatientLedger patientLedger in lPatientLedger)
                        {
                            List<PatientLedger> patients = PatientLedgerManager.Instance.ListAllEntryByPatientId(patientLedger.PatientId).ToList();
                            bool hasOP = patients.Any(patient => patient.OpRegistrationId != null);
                            bool hasIP = patients.Any(patient => patient.InPatientAdmissionId != null);
                            PaymentReceiveTableRow[ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.OP_IP]] = hasIP && hasOP ? "IP" : hasOP ? "OP" : "";
                            ConsultationNote ConsultationDoctor = patientLedger.OpRegistrationId.HasValue ? ConsultationNoteManager.Instance.GetConsultationNoteByOPRegisterId((long)patientLedger.OpRegistrationId) : null!;
                            if (ConsultationDoctor != null && (Consultant == string.Empty || Consultant != ConsultationDoctor.Consultant.Name))
                            {
                                PaymentReceiveTableRow[ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.CONSULTANT]] = ConsultationDoctor.Consultant.Name;
                                Consultant = ConsultationDoctor.Consultant.Name;
                            }
                        }
                        PaymentReceiveTableRow[ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.DESCRIPTION]] = LineItem.Description;
                        PaymentReceiveTableRow[ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.PAYMENT_TYPE]] = LineItem.PaymentType;
                        PaymentReceiveTableRow[ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.RECEIVED]] = LineItem.Received.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                        PaymentReceiveTable.Rows.Add(PaymentReceiveTableRow);
                        dateGroupTotal += (decimal)LineItem.Received;
                        grandTotal += (decimal)LineItem.Received;
                        rn++;
                        i++;
                    }
                }
                if (dateGroupTotal > 0)
                {
                    DataRow lastSubtotalRow = PaymentReceiveTable.NewRow();
                    lastSubtotalRow[ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.PAYMENT_TYPE]] = "Sub Total";
                    lastSubtotalRow[ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.RECEIVED]] = dateGroupTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    PaymentReceiveTable.Rows.Add(lastSubtotalRow);
                }
                DataRow grandTotalRow = PaymentReceiveTable.NewRow();
                grandTotalRow[ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.PAYMENT_TYPE]] = "Grand Total";
                grandTotalRow[ReceiveAmountDataTableColumnByDate[(int)PaymentReciveReportByDateTableColumn.RECEIVED]] = grandTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                PaymentReceiveTable.Rows.Add(grandTotalRow);
                return PaymentReceiveTable;
            }
            else if (RptReceiveAmount.Type == InventoryReportFilterType.BYPATIENT)
            {
                DataTable PaymentReceiveTable = new DataTable();
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByPatient[(int)PaymentReciveReportByPatientTableColumn.SNO], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByPatient[(int)PaymentReciveReportByPatientTableColumn.DATE], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByPatient[(int)PaymentReciveReportByPatientTableColumn.REFERENCE], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByPatient[(int)PaymentReciveReportByPatientTableColumn.OP_IP], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByPatient[(int)PaymentReciveReportByPatientTableColumn.DESCRIPTION], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByPatient[(int)PaymentReciveReportByPatientTableColumn.CONSULTANT], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByPatient[(int)PaymentReciveReportByPatientTableColumn.PAYMENT_TYPE], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByPatient[(int)PaymentReciveReportByPatientTableColumn.RECEIVED], typeof(string));

                DataRow PaymentReceiveTableRow = null!;
                string dateTime = null!;
                string Consultant = string.Empty;
                decimal patientSubtotal = 0m;
                decimal grandTotal = 0m;
                string currentPatient = null!;
                int rn = 0;
                int i = 1;
                if (RptReceiveAmount.ReceiveAmountReportLines != null && RptReceiveAmount.ReceiveAmountReportLines.Count > 0)
                {
                    foreach (ReceiveAmountReportLine LineItem in RptReceiveAmount.ReceiveAmountReportLines.OrderBy(x => x.Patient.Name))
                    {
                        if (LineItem.Received > 0 && LineItem.RefNumber != null)
                        {
                            IList<PatientLedger> lPatientLedger = PatientLedgerManager.Instance.ListAllEntryByPatientId(LineItem.Patient.Id);
                            String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                            if (currentPatient != null && currentPatient != LineItem.Patient.Name)
                            {
                                PaymentReceiveTableRow = PaymentReceiveTable.NewRow();
                                PaymentReceiveTableRow[ReceiveAmountDataTableColumnByPatient[(int)PaymentReciveReportByPatientTableColumn.PAYMENT_TYPE]] = "Sub Total";
                                PaymentReceiveTableRow[ReceiveAmountDataTableColumnByPatient[(int)PaymentReciveReportByPatientTableColumn.RECEIVED]] = patientSubtotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                PaymentReceiveTable.Rows.Add(PaymentReceiveTableRow);
                                i = 1;
                                patientSubtotal = 0m;
                            }
                            if (currentPatient == null || currentPatient != LineItem.Patient.Name)
                            {
                                PaymentReceiveTableRow = PaymentReceiveTable.NewRow();
                                PaymentReceiveTableRow[ReceiveAmountDataTableColumnByPatient[(int)PaymentReciveReportByPatientTableColumn.SNO]] = "Patient Name : " + LineItem.Patient.Name;
                                currentPatient = LineItem.Patient.Name;
                                dateTime = null!;
                                Consultant = string.Empty;
                                PaymentReceiveTable.Rows.Add(PaymentReceiveTableRow);
                            }
                            PaymentReceiveTableRow = PaymentReceiveTable.NewRow();
                            PaymentReceiveTableRow[ReceiveAmountDataTableColumnByPatient[(int)PaymentReciveReportByPatientTableColumn.SNO]] = i;
                            if (dateTime == null || dateTime != stringLineItemDate)
                            {
                                PaymentReceiveTableRow[ReceiveAmountDataTableColumnByPatient[(int)PaymentReciveReportByPatientTableColumn.DATE]] = LineItem.Date.ToString(Global.Company.DateFormat);
                                dateTime = stringLineItemDate;
                            }
                            PaymentReceiveTableRow[ReceiveAmountDataTableColumnByPatient[(int)PaymentReciveReportByPatientTableColumn.REFERENCE]] = LineItem.RefNumber;
                            foreach (PatientLedger patientLedger in lPatientLedger)
                            {
                                List<PatientLedger> patients = PatientLedgerManager.Instance.ListAllEntryByPatientId(patientLedger.PatientId).ToList();
                                bool hasOP = patients.Any(patient => patient.OpRegistrationId != null);
                                bool hasIP = patients.Any(patient => patient.InPatientAdmissionId != null);
                                PaymentReceiveTableRow[ReceiveAmountDataTableColumnByPatient[(int)PaymentReciveReportByPatientTableColumn.OP_IP]] = hasIP && hasOP ? "IP" : hasOP ? "OP" : "";

                                ConsultationNote ConsultationDoctor = patientLedger.OpRegistrationId.HasValue ? ConsultationNoteManager.Instance.GetConsultationNoteByOPRegisterId((long)patientLedger.OpRegistrationId) : null!;
                                if (ConsultationDoctor != null && (Consultant == string.Empty || Consultant != ConsultationDoctor.Consultant.Name))
                                {
                                    PaymentReceiveTableRow[ReceiveAmountDataTableColumnByPatient[(int)PaymentReciveReportByPatientTableColumn.CONSULTANT]] = ConsultationDoctor.Consultant.Name;
                                    Consultant = ConsultationDoctor.Consultant.Name;
                                }
                            }
                            PaymentReceiveTableRow[ReceiveAmountDataTableColumnByPatient[(int)PaymentReciveReportByPatientTableColumn.DESCRIPTION]] = LineItem.Description;
                            PaymentReceiveTableRow[ReceiveAmountDataTableColumnByPatient[(int)PaymentReciveReportByPatientTableColumn.PAYMENT_TYPE]] = LineItem.PaymentType;
                            PaymentReceiveTableRow[ReceiveAmountDataTableColumnByPatient[(int)PaymentReciveReportByPatientTableColumn.RECEIVED]] = LineItem.Received.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            patientSubtotal += (decimal)LineItem.Received;
                            grandTotal += (decimal)LineItem.Received;
                            rn++;
                            i++;
                            PaymentReceiveTable.Rows.Add(PaymentReceiveTableRow);
                        }
                    }
                    if (currentPatient != null)
                    {
                        PaymentReceiveTableRow = PaymentReceiveTable.NewRow();
                        PaymentReceiveTableRow[ReceiveAmountDataTableColumnByPatient[(int)PaymentReciveReportByPatientTableColumn.PAYMENT_TYPE]] = "Sub Total";
                        PaymentReceiveTableRow[ReceiveAmountDataTableColumnByPatient[(int)PaymentReciveReportByPatientTableColumn.RECEIVED]] = patientSubtotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        PaymentReceiveTable.Rows.Add(PaymentReceiveTableRow);
                    }
                    PaymentReceiveTableRow = PaymentReceiveTable.NewRow();
                    PaymentReceiveTableRow[ReceiveAmountDataTableColumnByPatient[(int)PaymentReciveReportByPatientTableColumn.PAYMENT_TYPE]] = "Grand Total";
                    PaymentReceiveTableRow[ReceiveAmountDataTableColumnByPatient[(int)PaymentReciveReportByPatientTableColumn.RECEIVED]] = grandTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    PaymentReceiveTable.Rows.Add(PaymentReceiveTableRow);
                }
                return PaymentReceiveTable;
            }
            else if (RptReceiveAmount.Type == InventoryReportFilterType.BYAMOUNT)
            {
                DataTable PaymentReceiveTable = new DataTable();
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.SNO], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.DATE], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.PATIENT_NAME], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.REFERENCE], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.OP_IP], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.DESCRIPTION], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.CONSULTANT], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.PAYMENT_TYPE], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.RECEIVED], typeof(string));

                string Patients = string.Empty;
                string Consultant = string.Empty;
                int rn = 0;
                decimal grandTotal = 0m;
                string previousDate = null!;
                decimal dateSubtotal = 0m;
                int i = 1;
                foreach (ReceiveAmountReportLine LineItem in RptReceiveAmount.ReceiveAmountReportLines.OrderBy(x => x.Date))
                {
                    if (LineItem.Received > 0 && LineItem.RefNumber != null)
                    {
                        IList<PatientLedger> lPatientLedger = PatientLedgerManager.Instance.ListAllEntryByPatientId(LineItem.Patient.Id);
                        String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);

                        if (previousDate != null && previousDate != stringLineItemDate)
                        {
                            DataRow dateSubtotalRow = PaymentReceiveTable.NewRow();
                            dateSubtotalRow[ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.PAYMENT_TYPE]] = "Sub Total";
                            dateSubtotalRow[ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.RECEIVED]] =
                                dateSubtotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            PaymentReceiveTable.Rows.Add(dateSubtotalRow);
                            i = 1;
                            dateSubtotal = 0m;
                        }
                        DataRow AmountReceiveTableRow = PaymentReceiveTable.NewRow();
                        AmountReceiveTableRow[ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.SNO]] = i;
                        if (previousDate == null || previousDate != stringLineItemDate)
                        {
                            AmountReceiveTableRow[ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.DATE]] = LineItem.Date.ToString(Global.Company.DateFormat);
                            previousDate = stringLineItemDate;
                            Patients = string.Empty;
                            Consultant = string.Empty;
                        }
                        AmountReceiveTableRow[ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.REFERENCE]] = LineItem.RefNumber;
                        if (string.IsNullOrEmpty(Patients) || Patients != LineItem.Patient.Name)
                        {
                            AmountReceiveTableRow[ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.PATIENT_NAME]] = LineItem.Patient.Name;
                            Patients = LineItem.Patient.Name;
                        }
                        foreach (PatientLedger patientLedger in lPatientLedger)
                        {
                            List<PatientLedger> patients = PatientLedgerManager.Instance.ListAllEntryByPatientId(patientLedger.PatientId).ToList();
                            bool hasOP = patients.Any(patient => patient.OpRegistrationId != null);
                            bool hasIP = patients.Any(patient => patient.InPatientAdmissionId != null);
                            AmountReceiveTableRow[ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.OP_IP]] =
                                hasIP && hasOP ? "IP" : hasOP ? "OP" : "";

                            ConsultationNote ConsultationDoctor = patientLedger.OpRegistrationId.HasValue ?
                                ConsultationNoteManager.Instance.GetConsultationNoteByOPRegisterId((long)patientLedger.OpRegistrationId) : null!;

                            if (ConsultationDoctor != null && (string.IsNullOrEmpty(Consultant) || Consultant != ConsultationDoctor.Consultant.Name))
                            {
                                AmountReceiveTableRow[ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.CONSULTANT]] = ConsultationDoctor.Consultant.Name;
                                Consultant = ConsultationDoctor.Consultant.Name;
                            }
                        }
                        AmountReceiveTableRow[ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.DESCRIPTION]] = LineItem.Description;
                        AmountReceiveTableRow[ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.PAYMENT_TYPE]] = LineItem.PaymentType;
                        AmountReceiveTableRow[ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.RECEIVED]] =
                        LineItem.Received.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        PaymentReceiveTable.Rows.Add(AmountReceiveTableRow);
                        i++;
                        dateSubtotal += (decimal)LineItem.Received;
                        grandTotal += (decimal)LineItem.Received;
                        rn++;
                    }
                }
                if (dateSubtotal > 0)
                {
                    DataRow lastDateSubtotalRow = PaymentReceiveTable.NewRow();
                    lastDateSubtotalRow[ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.PAYMENT_TYPE]] = "Sub Total";
                    lastDateSubtotalRow[ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.RECEIVED]] =
                        dateSubtotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    PaymentReceiveTable.Rows.Add(lastDateSubtotalRow);
                }
                DataRow grandTotalRow = PaymentReceiveTable.NewRow();
                grandTotalRow[ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.PAYMENT_TYPE]] = "Grand Total";
                grandTotalRow[ReceiveAmountDataTableColumnByAmount[(int)PaymentReciveReportByAmountTableColumn.RECEIVED]] =
                grandTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                PaymentReceiveTable.Rows.Add(grandTotalRow);
                return PaymentReceiveTable;
            }
            else if (RptReceiveAmount.Type == InventoryReportFilterType.BYCONSULTANT)
            {
                DataTable PaymentReceiveTable = new DataTable();
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.SNO], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.CONSULTANT], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.PATIENT_NAME], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.DATE], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.REFERENCE], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.OP_IP], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.DESCRIPTION], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.PAYMENT_TYPE], typeof(string));
                PaymentReceiveTable.Columns.Add(ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.RECEIVED], typeof(string));

                DataRow PaymentReceiveTableRow = null!;
                String dateTime = null!;
                string Patients = string.Empty;
                string Consultant = string.Empty;
                int rn = 0;
                decimal grandTotal = 0m;
                int i = 1;
                decimal dateSubtotal = 0m;
                string previousDate = null!;
                bool isFirstRow = false;
                foreach (ReceiveAmountReportLineByConsult LineItem in RptReceiveAmount.ReceiveAmountReportLineByConsult)
                {
                    if (LineItem.Received > 0 && LineItem.RefNumber != null)
                    {
                        IList<PatientLedger> lPatientLedger = PatientLedgerManager.Instance.ListAllEntryByPatientId(LineItem.Patient.Id);
                        String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                        PaymentReceiveTableRow = PaymentReceiveTable.NewRow();
                        foreach (PatientLedger patientLedger in lPatientLedger)
                        {
                            List<PatientLedger> patients = PatientLedgerManager.Instance.ListAllEntryByPatientId(patientLedger.PatientId).ToList();
                            ConsultationNote ConsultationDoctor = patientLedger.OpRegistrationId.HasValue ?
                            ConsultationNoteManager.Instance.GetConsultationNoteByOPRegisterId((long)patientLedger.OpRegistrationId) : null!;
                            if (ConsultationDoctor != null)
                            {
                                if (Consultant == string.Empty || Consultant != ConsultationDoctor.Consultant.Name)
                                {
                                    if (isFirstRow)
                                    {
                                        DataRow dateSubtotalRow = PaymentReceiveTable.NewRow();
                                        dateSubtotalRow[ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.PAYMENT_TYPE]] = "Sub Total";
                                        dateSubtotalRow[ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.RECEIVED]] =
                                        dateSubtotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                        PaymentReceiveTable.Rows.Add(dateSubtotalRow);
                                        dateSubtotal = 0m;
                                    }
                                    PaymentReceiveTableRow[ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.CONSULTANT]] = ConsultationDoctor.Consultant.Name;
                                    Consultant = ConsultationDoctor.Consultant.Name;
                                    Patients = string.Empty;
                                    isFirstRow = true;
                                    i = 1;
                                }
                            }
                        }
                        PaymentReceiveTableRow[ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.SNO]] = i;
                        if (Patients == string.Empty || Patients != LineItem.Patient.Name)
                        {
                            PaymentReceiveTableRow[ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.PATIENT_NAME]] = LineItem.Patient.Name;
                            Patients = LineItem.Patient.Name;
                        }
                        if (dateTime == null || dateTime != stringLineItemDate)
                        {
                            PaymentReceiveTableRow[ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.DATE]] = LineItem.Date.ToString(Global.Company.DateFormat);
                            dateTime = stringLineItemDate;
                        }
                        PaymentReceiveTableRow[ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.REFERENCE]] = LineItem.RefNumber;

                        foreach (PatientLedger patientLedger in lPatientLedger)
                        {
                            List<PatientLedger> patients = PatientLedgerManager.Instance.ListAllEntryByPatientId(patientLedger.PatientId).ToList();
                            bool hasOP = patients.Any(patient => patient.OpRegistrationId != null);
                            bool hasIP = patients.Any(patient => patient.InPatientAdmissionId != null);
                            PaymentReceiveTableRow[ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.OP_IP]] = hasIP && hasOP ? "IP" : hasOP ? "OP" : "";
                        }
                        PaymentReceiveTableRow[ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.DESCRIPTION]] = LineItem.Description;
                        PaymentReceiveTableRow[ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.PAYMENT_TYPE]] = LineItem.PaymentType;
                        PaymentReceiveTableRow[ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.RECEIVED]] = LineItem.Received.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        PaymentReceiveTable.Rows.Add(PaymentReceiveTableRow);
                        dateSubtotal += (decimal)LineItem.Received;
                        grandTotal += (decimal)LineItem.Received;
                        previousDate = stringLineItemDate;
                        rn++;
                        i++;
                    }
                }
                if (dateSubtotal > 0)
                {
                    DataRow lastDateSubtotalRow = PaymentReceiveTable.NewRow();
                    lastDateSubtotalRow[ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.PAYMENT_TYPE]] = "Sub Total";
                    lastDateSubtotalRow[ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.RECEIVED]] =
                    dateSubtotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    PaymentReceiveTable.Rows.Add(lastDateSubtotalRow);
                }
                DataRow grandTotalRow = PaymentReceiveTable.NewRow();
                grandTotalRow[ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.PAYMENT_TYPE]] = "Grand Total";
                grandTotalRow[ReceiveAmountDataTableColumnByConsultant[(int)PaymentReciveReportByConsultedTableColumn.RECEIVED]] =
                grandTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                PaymentReceiveTable.Rows.Add(grandTotalRow);
                return PaymentReceiveTable;
            }
            return new DataTable();
        }
        public void GeneratePDF(DataTable DataTable, RptReceiveAmount RptReceiveAmount, string ReportName, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            Cursor.Current = Cursors.WaitCursor;
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
                    ReportLine1 = "Payment Received Details",
                    ReportLine2 = RptReceiveAmount.ReportSubTitle()
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
                    ReportLine1 = "Payment Received Details",
                    ReportLine2 = RptReceiveAmount.ReportSubTitle()
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();
                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(DataTable, RptReceiveAmount.ReportTitle());
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = DataTable.Columns.Count;
                int Rows = DataTable.Rows.Count;
                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths = new float[] { 10f, 20f, 20f, 40f, 20f, 40f, 40f, 20f, 30f };
                if (RptReceiveAmount.Type == InventoryReportFilterType.BYPATIENT)
                {
                    widths = new float[] { 10f, 20f, 20f, 20f, 40f, 40f, 20f, 30f };
                }
                else if (RptReceiveAmount.Type == InventoryReportFilterType.BYCONSULTANT)
                {
                    widths = new float[] { 10f, 40f, 40f, 20f, 20f, 20f, 40f, 20f, 30f };
                }
                else if (RptReceiveAmount.Type == InventoryReportFilterType.BYAMOUNT)
                {
                    widths = new float[] { 10f, 20f, 40f, 20f, 20f, 40f, 40f, 20f, 30f };
                }
                else
                {
                    widths = new float[] { 10f, 20f, 20f, 40f, 20f, 40f, 40f, 20f, 30f };
                }
                ReportMainTable.SetWidths(widths);

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
                        ReportMainTable = new PdfPTable(Cols);
                        ReportMainTable.SetWidths(widths);
                    }
                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = DataTable.Rows[i][j].ToString();
                        RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                        RowCell.BorderColor = new BaseColor(160, 160, 160);

                        int columnIndexToCheck = (RptReceiveAmount.Type == InventoryReportFilterType.BYPATIENT) ? 6 : 7;
                        if (DataTable.Rows[i][columnIndexToCheck].ToString()!.Trim().Equals("Sub Total", StringComparison.OrdinalIgnoreCase) || DataTable.Rows[i][columnIndexToCheck].ToString()!.Trim().Equals("Grand Total", StringComparison.OrdinalIgnoreCase))
                        {
                            RowCell.BackgroundColor = new BaseColor(211, 211, 211);

                            if (j == 0)
                            {
                                RowCell.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
                            }
                            else if (j == 8 || (RptReceiveAmount.Type == InventoryReportFilterType.BYPATIENT && j == 7))
                            {
                                RowCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
                            }
                            else
                            {
                                RowCell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
                            }

                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else
                        {
                            RowCell.BackgroundColor = BaseColor.WHITE;

                            if (DataTable.Columns[j].ColumnName == "Received Amount")
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            else
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            }
                        }
                        if (RptReceiveAmount.Type == InventoryReportFilterType.BYPATIENT)
                        {
                            if (j == 0 && string.IsNullOrEmpty(DataTable.Rows[i][7].ToString()))
                            {
                                RowCell.Colspan = DataTable.Columns.Count;
                            }
                            if (j != 0 && string.IsNullOrEmpty(DataTable.Rows[i][7].ToString()))
                            {
                                continue;
                            }
                        }
                        ReportMainTable.AddCell(RowCell);
                    }
                }
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

                PdfGeneration PdfGeneration = new PdfGeneration();
                PdfGeneration.IsPrint = isPrint;
                PdfGeneration.FileName = RptReceiveAmount.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
