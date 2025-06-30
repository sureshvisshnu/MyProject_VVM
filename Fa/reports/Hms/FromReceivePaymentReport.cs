using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using fa;
using fa.api.Accounting;
using fa.api.catalog;
using fa.api.Hms;
using fa.api.utils;
using fa.libraries.utils;
using fa.model.Employee;
using fa.model.Hms.common;
using fa.model.Hms.Master;
using fa.model.OrderManagement;
using fa.report.Inventory;
using fa.reports.Inventory;
using fa.views;
using fa.views.controls;
using fa.views.controls.ComboTreeView;
using fa.views.hms.Masters;
using fa.views.utils.Report.Inventory;
using Fa.views.utils.Report.Inventory;
using FADataAccessLibrary.report.Hms;
using FADataAccessLibrary.report.Inventory;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using static FADataAccessLibrary.report.Hms.RptReceiveAmount;
using fa.views.controls.hms;
using fa.model.Hms.Op;
using fa.model.Hms.Ip;
using Microsoft.Office.Interop.Excel;
using Global = fa.Global;
using InventoryReportFilterType = FADataAccessLibrary.report.Hms.InventoryReportFilterType;
using fa.model.UserProfile;
using fa.model.hms.common;
using System.Reflection.Metadata;
using Fa.views.utils.Report.Hms;
using System.Globalization;
using fa.reports.Hms;
using static fa.views.utils.Common.DataGridViewColoumnAdjustment;
using Rectangle = System.Drawing.Rectangle;
using fa.reports.sales;
using DocumentFormat.OpenXml.VariantTypes;
using fa.report.sales;
using fa.views.utils.Report.Sale;

namespace Fa.reports.Hms
{
    enum PaymentReciveReportByDateTableColumn
    {
        SNO, DATE, REFERENCE, PATIENT_NAME, OP_IP, CONSULTANT, DESCRIPTION, PAYMENT_TYPE, RECEIVED,
    }
    enum PaymentReciveReportByPatientTableColumn
    {
        SNO, PATIENT_NAME, DATE, REFERENCE, OP_IP, DESCRIPTION, CONSULTANT, PAYMENT_TYPE, RECEIVED, ROWHEADING
    }
    enum PaymentReciveReportByAmountTableColumn
    {
        SNO, DATE, PATIENT_NAME, REFERENCE, OP_IP, DESCRIPTION, CONSULTANT, PAYMENT_TYPE, RECEIVED,
    }
    enum PaymentReciveReportByConsultedTableColumn
    {
        SNO, CONSULTANT, PATIENT_NAME, DATE, REFERENCE, OP_IP, DESCRIPTION, PAYMENT_TYPE, RECEIVED, ROWHEADING
    }
    public partial class FromReceivePaymentReport : Form
    {
        RptReceiveAmount RptReceiveAmount = null;

        public static string InformationMsg = "No Information Found..!";
        public static string SelectferenceErrorMsg = "Please select {0}";
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string CheckValidDateErrorMsg = "From date is greater than To date";
        public FromReceivePaymentReport()
        {
            InitializeComponent();
            TextBoxReceivePaymentAmountFrom.KeyPress += NumericOnlyTextBox_KeyPress;
            TextBoxReceivePaymentAmountTo.KeyPress += NumericOnlyTextBox_KeyPress;
        }

        private void ReceivePaymentReport_Load(object sender, EventArgs e)
        {
            ResetForm();
            LoadPatientCombo();
            LoadDoctorCombo();
            ToolStripSeparatorReceivePayment1.Visible = false;
            CheckedTreeComboBoxPatient.Visible = false;
            ToolStripLableReceivePaymenPatient.Visible = false;
            ToolStripSeparatorReceivePayment1.Visible = false;
            ToolStripLableReceivePaymentAmount.Visible = false;
            ToolStripSeparatorReceivePayment3.Visible = false;
            ToolStripLableReceivePaymentAmountTo.Visible = false;
            TextBoxReceivePaymentAmountFrom.Visible = false;
            TextBoxReceivePaymentAmountTo.Visible = false;
            ToolStripLableReceivePaymenConsultant.Visible = false;
            CheckedTreeComboBoxConsulted.Visible = false;
        }
        private void EnableButton(bool Enable)
        {
            BtnPrint.Enabled = Enable;
            BtnSave.Enabled = Enable;
            ToolStripBtnSave.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
        }
        private void ResetForm()
        {
            GridViewByDatePaymentRecive.Rows.Clear();
            GridViewByPatientPaymentReceive.Rows.Clear();
            GridViewByAmountPaymentReceive.Rows.Clear();
            GridViewByConsultPaymentReceive.Rows.Clear();
            ErrorMsgPaymentReceiveReport.Text = "";
            EnableButton(false);
            CheckedTreeComboBoxPatient.SelectedNode = null;
            foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxPatient.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            CheckedTreeComboBoxConsulted.SelectedNode = null;
            foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxConsulted.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            PaymentReciveReportFromDate.Format = Global.Company.DateFormat;
            PaymentReciveReportFromDate.Date = Global.getTransactionDate().AddDays(-30);
            PaymentReciveReportToDate.Format = Global.Company.DateFormat;
            PaymentReciveReportToDate.Date = Global.getTransactionDate();
            ComboBoxTypeSelection.SelectedIndex = 0;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSave.PerformClick();
            }
            else if (keyData == (Keys.F9))
            {
                BtnPrint.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ComboTypeSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxTypeSelection.SelectedIndex == 0)
            {
                CheckedTreeComboBoxPatient.Visible = false;
                GridViewByDatePaymentRecive.Visible = true;
                GridViewByPatientPaymentReceive.Visible = false;
                GridViewByAmountPaymentReceive.Visible = false;
                GridViewByConsultPaymentReceive.Visible = false;
                ToolStripLableReceivePaymenPatient.Visible = false;
                ToolStripSeparatorReceivePayment1.Visible = false;
                ToolStripLableReceivePaymentAmount.Visible = false;
                ToolStripLableReceivePaymentAmountTo.Visible = false;
                TextBoxReceivePaymentAmountFrom.Visible = false;
                TextBoxReceivePaymentAmountTo.Visible = false;
                ToolStripLableReceivePaymenConsultant.Visible = false;
                CheckedTreeComboBoxConsulted.Visible = false;
                GridViewByDatePaymentRecive.Rows.Clear();
                GridViewByPatientPaymentReceive.Rows.Clear();
                GridViewByAmountPaymentReceive.Rows.Clear();
                GridViewByConsultPaymentReceive.Rows.Clear();
                EnableButton(false);
                this.Text = "Payment Receive Report";
                ErrorMsgPaymentReceiveReport.Text = "";
            }
            else if (ComboBoxTypeSelection.SelectedIndex == 1)
            {
                CheckedTreeComboBoxPatient.SelectedNode = null;
                foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxPatient.Nodes)
                {
                    ComboTreeNode.Checked = false;
                }
                CheckedTreeComboBoxPatient.Visible = true;
                GridViewByDatePaymentRecive.Visible = false;
                GridViewByPatientPaymentReceive.Visible = true;
                GridViewByAmountPaymentReceive.Visible = false;
                GridViewByConsultPaymentReceive.Visible = false;
                ToolStripLableReceivePaymenPatient.Visible = true;
                ToolStripSeparatorReceivePayment1.Visible = false;
                ToolStripLableReceivePaymentAmount.Visible = false;
                ToolStripLableReceivePaymentAmountTo.Visible = false;
                TextBoxReceivePaymentAmountFrom.Visible = false;
                TextBoxReceivePaymentAmountTo.Visible = false;
                ToolStripLableReceivePaymenConsultant.Visible = false;
                CheckedTreeComboBoxConsulted.Visible = false;
                GridViewByDatePaymentRecive.Rows.Clear();
                GridViewByPatientPaymentReceive.Rows.Clear();
                GridViewByAmountPaymentReceive.Rows.Clear();
                GridViewByConsultPaymentReceive.Rows.Clear();
                EnableButton(false);
                this.Text = "Payment Receive Report";
                ErrorMsgPaymentReceiveReport.Text = "";
            }
            else if (ComboBoxTypeSelection.SelectedIndex == 2)
            {
                CheckedTreeComboBoxPatient.Visible = false;
                GridViewByDatePaymentRecive.Visible = false;
                GridViewByPatientPaymentReceive.Visible = false;
                GridViewByAmountPaymentReceive.Visible = true;
                GridViewByConsultPaymentReceive.Visible = false;
                ToolStripLableReceivePaymenPatient.Visible = false;
                ToolStripSeparatorReceivePayment1.Visible = false;
                ToolStripLableReceivePaymentAmount.Visible = true;
                ToolStripLableReceivePaymentAmountTo.Visible = true;
                TextBoxReceivePaymentAmountFrom.Visible = true;
                TextBoxReceivePaymentAmountTo.Visible = true;
                TextBoxReceivePaymentAmountFrom.Text = string.Empty;
                TextBoxReceivePaymentAmountTo.Text = string.Empty;
                ToolStripLableReceivePaymenConsultant.Visible = false;
                CheckedTreeComboBoxConsulted.Visible = false;
                GridViewByDatePaymentRecive.Rows.Clear();
                GridViewByPatientPaymentReceive.Rows.Clear();
                GridViewByAmountPaymentReceive.Rows.Clear();
                GridViewByConsultPaymentReceive.Rows.Clear();
                EnableButton(false);
                this.Text = "Payment Receive Report";
                ErrorMsgPaymentReceiveReport.Text = "";
            }
            else
            {
                CheckedTreeComboBoxConsulted.SelectedNode = null;
                foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxConsulted.Nodes)
                {
                    ComboTreeNode.Checked = false;
                }
                CheckedTreeComboBoxPatient.Visible = false;
                GridViewByDatePaymentRecive.Visible = false;
                GridViewByPatientPaymentReceive.Visible = false;
                GridViewByAmountPaymentReceive.Visible = false;
                GridViewByConsultPaymentReceive.Visible = true;
                ToolStripLableReceivePaymenPatient.Visible = false;
                ToolStripSeparatorReceivePayment1.Visible = false;
                ToolStripLableReceivePaymentAmount.Visible = false;
                ToolStripLableReceivePaymentAmountTo.Visible = false;
                TextBoxReceivePaymentAmountFrom.Visible = false;
                TextBoxReceivePaymentAmountTo.Visible = false;
                ToolStripLableReceivePaymenConsultant.Visible = true;
                CheckedTreeComboBoxConsulted.Visible = true;
                GridViewByDatePaymentRecive.Rows.Clear();
                GridViewByPatientPaymentReceive.Rows.Clear();
                GridViewByAmountPaymentReceive.Rows.Clear();
                GridViewByConsultPaymentReceive.Rows.Clear();
                EnableButton(false);
                this.Text = "Payment Receive Report";
                ErrorMsgPaymentReceiveReport.Text = "";
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
            this.Text = "Payment Receive Report";
            ToolStripSeparatorReceivePayment1.Visible = false;
            CheckedTreeComboBoxPatient.Visible = false;
            ToolStripLableReceivePaymenPatient.Visible = false;
            ToolStripSeparatorReceivePayment1.Visible = false;
            ToolStripLableReceivePaymentAmount.Visible = false;
            ToolStripLableReceivePaymentAmountTo.Visible = false;
            TextBoxReceivePaymentAmountFrom.Visible = false;
            TextBoxReceivePaymentAmountTo.Visible = false;
            ToolStripLableReceivePaymenConsultant.Visible = false;
            CheckedTreeComboBoxConsulted.Visible = false;
        }

        private void LoadPatientCombo()
        {
            ComboUtils.InitializePatientCombo(CheckedTreeComboBoxPatient, Global.Company.CompanyId);
        }
        private void LoadDoctorCombo()
        {
            ComboUtils.InitializeConsultantTypeCombo(CheckedTreeComboBoxConsulted, Global.Company.CompanyId);
        }
        private void DisplayCheckedAccount()
        {
            string CheckedNodes = string.Empty;
            if (ComboBoxTypeSelection.SelectedIndex == 1)
            {
                if (CheckedTreeComboBoxPatient.CheckedNodes.Count > 0)
                {
                    foreach (ComboTreeNode node in CheckedTreeComboBoxPatient.CheckedNodes)
                    {
                        if (node.Name == "All") { CheckedNodes = "All Patients"; break; }
                        CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                    }
                    this.Text = "Payment Receive Report " + " @ " + CheckedNodes;
                }
                else
                {
                    this.Text = "Payment Receive Report";
                }
            }
            else if (ComboBoxTypeSelection.SelectedIndex == 3)
            {
                if (CheckedTreeComboBoxConsulted.CheckedNodes.Count > 0)
                {
                    foreach (ComboTreeNode node in CheckedTreeComboBoxConsulted.CheckedNodes)
                    {
                        if (node.Name == "All") { CheckedNodes = "All Consultants"; break; }
                        CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                    }
                    this.Text = "Payment Receive Report " + " @ " + CheckedNodes;
                }
                else
                {
                    this.Text = "Payment Receive Report";
                }
            }
            else
            {
                this.Text = "Payment Receive Report";
            }

        }
        private bool FormValidate()
        {
            ErrorMsgPaymentReceiveReport.Text = "";
            if (ComboBoxTypeSelection.SelectedIndex < 0)
            {
                ErrorMsgPaymentReceiveReport.Text = "Please select type";
                ComboBoxTypeSelection.Select();
                return false;
            }
            if (ComboBoxTypeSelection.SelectedIndex == 1 && CheckedTreeUtils.SelectedNodes(CheckedTreeComboBoxPatient).Count < 1)
            {
                ErrorMsgPaymentReceiveReport.Text = "Please select patient";
                CheckedTreeComboBoxPatient.Focus();
                return false;
            }
            if (ComboBoxTypeSelection.SelectedIndex == 3 && CheckedTreeUtils.SelectedNodes(CheckedTreeComboBoxConsulted).Count < 1)
            {
                ErrorMsgPaymentReceiveReport.Text = "Please select consulted";
                CheckedTreeComboBoxConsulted.Focus();
                return false;
            }
            if (ComboBoxTypeSelection.SelectedIndex == 2)
            {
                if (TextBoxReceivePaymentAmountFrom.Text == string.Empty)
                {
                    ErrorMsgPaymentReceiveReport.Text = "Please select from amount";
                    TextBoxReceivePaymentAmountFrom.Focus();
                    return false;
                }
                else if (long.Parse(TextBoxReceivePaymentAmountFrom.Text) >= long.Parse(TextBoxReceivePaymentAmountTo.Text))
                {
                    ErrorMsgPaymentReceiveReport.Text = "Please enter the valid amount";
                    TextBoxReceivePaymentAmountFrom.Focus();
                    return false;
                }
            }
            if (ComboBoxTypeSelection.SelectedIndex == 2 && TextBoxReceivePaymentAmountTo.Text == string.Empty)
            {
                ErrorMsgPaymentReceiveReport.Text = "Please select to amount";
                TextBoxReceivePaymentAmountTo.Focus();
                return false;
            }
            if (PaymentReciveReportFromDate.Date == null || !DateUtils.ValidDate(((DateTime)PaymentReciveReportFromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsgPaymentReceiveReport.Text = EnterValidDateErrorMsg;
                PaymentReciveReportFromDate.Focus();
                return false;
            }
            if (PaymentReciveReportToDate.Date == null || !DateUtils.ValidDate(((DateTime)PaymentReciveReportToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsgPaymentReceiveReport.Text = EnterValidDateErrorMsg;
                PaymentReciveReportToDate.Focus();
                return false;
            }
            return true;
        }

        private void CheckedComboBoxDoctor_ItemCheckedEvent(object sender, ItemCheckEventArgs e)
        {
            DisplayCheckedAccount();
        }

        private void CheckedComboBoxItemPatient_ItemCheckedEvent(object sender, ItemCheckEventArgs e)
        {
            DisplayCheckedAccount();
        }

        public void LoadReceivePaymentReport()
        {
            RptReceiveAmount = new RptReceiveAmount();
            RptReceiveAmount.FromDate = (DateTime)PaymentReciveReportFromDate.Date!;
            RptReceiveAmount.ToDate = (DateTime)PaymentReciveReportToDate.Date!;
            RptReceiveAmount.Company = Global.Company;
            RptReceiveAmount.ReportHeader = "For" + " @ " + SelectedNodesText(CheckedTreeComboBoxPatient);
            RptReceiveAmount.ReportHeader = "For" + " @ " + SelectedNodesText(CheckedTreeComboBoxConsulted);
            RptReceiveAmount.Type = ComboBoxTypeSelection.SelectedIndex == 0 ? FADataAccessLibrary.report.Hms.InventoryReportFilterType.BYDATE : ComboBoxTypeSelection.SelectedIndex == 1 ? FADataAccessLibrary.report.Hms.InventoryReportFilterType.BYPATIENT : ComboBoxTypeSelection.SelectedIndex == 2 ? FADataAccessLibrary.report.Hms.InventoryReportFilterType.BYAMOUNT : FADataAccessLibrary.report.Hms.InventoryReportFilterType.BYCONSULTANT;
            RptReceiveAmount.PatientIds = CheckedTreeUtils.SelectedNodes(CheckedTreeComboBoxPatient).ToArray();
            RptReceiveAmount.DoctorIds = CheckedTreeUtils.SelectedNodes(CheckedTreeComboBoxConsulted).ToArray();
            RptReceiveAmount.IsAllLocation = SelectedNodesText(CheckedTreeComboBoxPatient) == "All Location" ? true : false;
            RptReceiveAmount.IsAllDoctor = SelectedNodesText(CheckedTreeComboBoxConsulted) == "All Location" ? true : false;
            RptReceiveAmount.GenerateReport(); 
            int irow = 0;
            int j = 2;
            Color[] RowColor = new Color[2];
            RowColor[0] = Color.White;
            RowColor[1] = Color.WhiteSmoke;
            if (RptReceiveAmount.Type == InventoryReportFilterType.BYDATE)
            {
                GridViewByDatePaymentRecive.Rows.Clear();
                if (RptReceiveAmount.ReceiveAmountReportLines != null && RptReceiveAmount.ReceiveAmountReportLines.Count > 0)
                {
                    string Patients = string.Empty;
                    string Consultant = string.Empty;
                    EnableButton(true);
                    decimal dateSubtotal = 0;
                    decimal grandTotal = 0;
                    string currentGroupDate = null!;
                    int i = 1;
                    foreach (ReceiveAmountReportLine LineItem in RptReceiveAmount.ReceiveAmountReportLines.OrderBy(x => x.Date))
                    {
                        if (LineItem.Received > 0 && LineItem.RefNumber != null)
                        {
                            IList<PatientLedger> lPatientLedger = PatientLedgerManager.Instance.ListAllEntryByPatientId(LineItem.Patient.Id);
                            String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);

                            if (currentGroupDate != null && currentGroupDate != stringLineItemDate)
                            {
                                irow = GridViewByDatePaymentRecive.Rows.Add();
                                GridViewByDatePaymentRecive.Rows[irow].DefaultCellStyle.BackColor = Color.LightGray;
                                GridViewByDatePaymentRecive.Rows[irow].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                                GridViewByDatePaymentRecive.Rows[irow].Cells[(int)PaymentReciveReportByDateTableColumn.PAYMENT_TYPE].Value = "Sub Total";
                                GridViewByDatePaymentRecive.Rows[irow].Cells[(int)PaymentReciveReportByDateTableColumn.RECEIVED].Value = dateSubtotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                i = 1;
                                dateSubtotal = 0;
                            }

                            irow = GridViewByDatePaymentRecive.Rows.Add();
                            GridViewByDatePaymentRecive.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            GridViewByDatePaymentRecive.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            GridViewByDatePaymentRecive.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            j++;

                            if (currentGroupDate == null || currentGroupDate != stringLineItemDate)
                            {
                                GridViewByDatePaymentRecive.Rows[irow].Cells[(int)PaymentReciveReportByDateTableColumn.DATE].Value = LineItem.Date.ToString(Global.Company.DateFormat);
                                currentGroupDate = stringLineItemDate;
                                Patients = string.Empty;
                                Consultant = string.Empty;
                            }

                            GridViewByDatePaymentRecive.Rows[irow].Cells[(int)PaymentReciveReportByDateTableColumn.SNO].Value = i;
                            GridViewByDatePaymentRecive.Rows[irow].Cells[(int)PaymentReciveReportByDateTableColumn.REFERENCE].Value = LineItem.RefNumber;

                            if (Patients == string.Empty || Patients != LineItem.Patient.Name)
                            {
                                GridViewByDatePaymentRecive.Rows[irow].Cells[(int)PaymentReciveReportByDateTableColumn.PATIENT_NAME].Value = LineItem.Patient.Name;
                                Patients = LineItem.Patient.Name;
                            }

                            foreach (PatientLedger patientLedger in lPatientLedger)
                            {
                                List<PatientLedger> patients = PatientLedgerManager.Instance.ListAllEntryByPatientId(patientLedger.PatientId).ToList();
                                bool hasOP = patients.Any(patient => patient.OpRegistrationId != null);
                                bool hasIP = patients.Any(patient => patient.InPatientAdmissionId != null);
                                GridViewByDatePaymentRecive.Rows[irow].Cells[(int)PaymentReciveReportByDateTableColumn.OP_IP].Value = hasIP && hasOP ? "IP" : hasOP ? "OP" : "";
                                ConsultationNote ConsultationDoctor = patientLedger.OpRegistrationId.HasValue ? ConsultationNoteManager.Instance.GetConsultationNoteByOPRegisterId((long)patientLedger.OpRegistrationId) : null!;
                                if (ConsultationDoctor != null && (Consultant == string.Empty || Consultant != ConsultationDoctor.Consultant.Name))
                                {
                                    GridViewByDatePaymentRecive.Rows[irow].Cells[(int)PaymentReciveReportByDateTableColumn.CONSULTANT].Value = ConsultationDoctor.Consultant.Name;
                                    Consultant = ConsultationDoctor.Consultant.Name;
                                }
                            }

                            GridViewByDatePaymentRecive.Rows[irow].Cells[(int)PaymentReciveReportByDateTableColumn.DESCRIPTION].Value = LineItem.Description;
                            GridViewByDatePaymentRecive.Rows[irow].Cells[(int)PaymentReciveReportByDateTableColumn.PAYMENT_TYPE].Value = LineItem.PaymentType;
                            GridViewByDatePaymentRecive.Rows[irow].Cells[(int)PaymentReciveReportByDateTableColumn.RECEIVED].Value = LineItem.Received.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                            dateSubtotal += (decimal)LineItem.Received;
                            grandTotal += (decimal)LineItem.Received;
                            i++;
                        }
                    }

                    if (currentGroupDate != null)
                    {
                        irow = GridViewByDatePaymentRecive.Rows.Add();
                        GridViewByDatePaymentRecive.Rows[irow].DefaultCellStyle.BackColor = Color.LightGray;
                        GridViewByDatePaymentRecive.Rows[irow].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                        GridViewByDatePaymentRecive.Rows[irow].Cells[(int)PaymentReciveReportByDateTableColumn.PAYMENT_TYPE].Value = "Sub Total";
                        GridViewByDatePaymentRecive.Rows[irow].Cells[(int)PaymentReciveReportByDateTableColumn.RECEIVED].Value = dateSubtotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    }

                    irow = GridViewByDatePaymentRecive.Rows.Add();
                    GridViewByDatePaymentRecive.Rows[irow].DefaultCellStyle.BackColor = Color.LightGray;
                    GridViewByDatePaymentRecive.Rows[irow].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                    GridViewByDatePaymentRecive.Rows[irow].DefaultCellStyle.BackColor = Color.LightGray;
                    GridViewByDatePaymentRecive.Rows[irow].Cells[(int)PaymentReciveReportByDateTableColumn.PAYMENT_TYPE].Value = "Grand Total";
                    GridViewByDatePaymentRecive.Rows[irow].Cells[(int)PaymentReciveReportByDateTableColumn.RECEIVED].Value = grandTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));


                }
                else
                {
                    ErrorMsgPaymentReceiveReport.Text = InformationMsg;
                }
            }
            else if (RptReceiveAmount.Type == InventoryReportFilterType.BYPATIENT)
            {
                GridViewByPatientPaymentReceive.Rows.Clear();
                string dateTime = null!;
                string Patients = string.Empty;
                string Consultant = string.Empty;
                int i = 1;
                if (RptReceiveAmount.ReceiveAmountReportLines != null && RptReceiveAmount.ReceiveAmountReportLines.Count > 0)
                {
                    EnableButton(true);
                    int row = 0;
                    int k = 2;
                    int rn = 0;
                    Color[] RowColor1 = new Color[2];
                    RowColor1[0] = Color.White;
                    RowColor1[1] = Color.WhiteSmoke;

                    decimal patientSubtotal = 0m;
                    decimal grandTotal = 0m;
                    string currentPatient = null!;

                    foreach (ReceiveAmountReportLine LineItem in RptReceiveAmount.ReceiveAmountReportLines.OrderBy(x => x.Patient.Name))
                    {
                        IList<PatientLedger> lPatientLedger = PatientLedgerManager.Instance.ListAllEntryByPatientId(LineItem.Patient.Id);
                        String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);


                        if (LineItem.Received > 0 && LineItem.RefNumber != null)
                        {
                            if (currentPatient != null && currentPatient != LineItem.Patient.Name)
                            {
                                int subtotalRow = GridViewByPatientPaymentReceive.Rows.Add();
                                GridViewByPatientPaymentReceive.Rows[subtotalRow].DefaultCellStyle.BackColor = Color.LightGray;
                                GridViewByPatientPaymentReceive.Rows[subtotalRow].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                                GridViewByPatientPaymentReceive.Rows[subtotalRow].Cells[(int)PaymentReciveReportByPatientTableColumn.PAYMENT_TYPE].Value = "Sub Total";
                                GridViewByPatientPaymentReceive.Rows[subtotalRow].Cells[(int)PaymentReciveReportByPatientTableColumn.RECEIVED].Value = patientSubtotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                i = 1;
                                patientSubtotal = 0m;
                            }
                            if (currentPatient == null || currentPatient != LineItem.Patient.Name)
                            {
                                irow = GridViewByPatientPaymentReceive.Rows.Add();
                                GridViewByPatientPaymentReceive.Rows[irow].Cells[(int)PaymentReciveReportByPatientTableColumn.PATIENT_NAME].Value = "Patient : " + LineItem.Patient.Name;
                                
                                currentPatient = LineItem.Patient.Name;
                                dateTime = null!;
                                Consultant = string.Empty;
                            }
                            row = GridViewByPatientPaymentReceive.Rows.Add();
                            GridViewByPatientPaymentReceive.Rows[row].DefaultCellStyle.BackColor = RowColor1[k % 2];
                            GridViewByPatientPaymentReceive.Rows[row].DefaultCellStyle.SelectionBackColor = RowColor1[k % 2];
                            GridViewByPatientPaymentReceive.Rows[row].DefaultCellStyle.SelectionForeColor = Color.Black;
                            k++;

                            GridViewByPatientPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByPatientTableColumn.SNO].Value = i;

                            if (dateTime == null || dateTime != stringLineItemDate)
                            {
                                GridViewByPatientPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByPatientTableColumn.DATE].Value = LineItem.Date.ToString(Global.Company.DateFormat);
                                dateTime = stringLineItemDate;
                            }

                            GridViewByPatientPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByPatientTableColumn.REFERENCE].Value = LineItem.RefNumber;

                            foreach (PatientLedger patientLedger in lPatientLedger)
                            {
                                List<PatientLedger> patients = PatientLedgerManager.Instance.ListAllEntryByPatientId(patientLedger.PatientId).ToList();
                                bool hasOP = patients.Any(patient => patient.OpRegistrationId != null);
                                bool hasIP = patients.Any(patient => patient.InPatientAdmissionId != null);
                                GridViewByPatientPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByPatientTableColumn.OP_IP].Value = hasIP && hasOP ? "IP" : hasOP ? "OP" : "";

                                ConsultationNote ConsultationDoctor = patientLedger.OpRegistrationId.HasValue ? ConsultationNoteManager.Instance.GetConsultationNoteByOPRegisterId((long)patientLedger.OpRegistrationId) : null!;
                                if (ConsultationDoctor != null && (Consultant == string.Empty || Consultant != ConsultationDoctor.Consultant.Name))
                                {
                                    GridViewByPatientPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByPatientTableColumn.CONSULTANT].Value = ConsultationDoctor.Consultant.Name;
                                    Consultant = ConsultationDoctor.Consultant.Name;
                                }
                            }

                            GridViewByPatientPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByPatientTableColumn.DESCRIPTION].Value = LineItem.Description;
                            GridViewByPatientPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByPatientTableColumn.PAYMENT_TYPE].Value = LineItem.PaymentType;
                            GridViewByPatientPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByPatientTableColumn.RECEIVED].Value = LineItem.Received.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                            patientSubtotal += (decimal)LineItem.Received;
                            grandTotal += (decimal)LineItem.Received;
                            i++;
                            rn++;
                        }
                    }

                    if (currentPatient != null)
                    {
                        int subtotalRow = GridViewByPatientPaymentReceive.Rows.Add();
                        GridViewByPatientPaymentReceive.Rows[subtotalRow].DefaultCellStyle.BackColor = Color.LightGray;
                        GridViewByPatientPaymentReceive.Rows[subtotalRow].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                        GridViewByPatientPaymentReceive.Rows[subtotalRow].Cells[(int)PaymentReciveReportByPatientTableColumn.PAYMENT_TYPE].Value = "Sub Total";
                        GridViewByPatientPaymentReceive.Rows[subtotalRow].Cells[(int)PaymentReciveReportByPatientTableColumn.RECEIVED].Value = patientSubtotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    }
                    int grandTotalRow = GridViewByPatientPaymentReceive.Rows.Add();
                    GridViewByPatientPaymentReceive.Rows[grandTotalRow].DefaultCellStyle.BackColor = Color.LightGray;
                    GridViewByPatientPaymentReceive.Rows[grandTotalRow].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                    GridViewByPatientPaymentReceive.Rows[grandTotalRow].Cells[(int)PaymentReciveReportByPatientTableColumn.PAYMENT_TYPE].Value = "Grand Total";
                    GridViewByPatientPaymentReceive.Rows[grandTotalRow].Cells[(int)PaymentReciveReportByPatientTableColumn.RECEIVED].Value = grandTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                    if (ComboBoxTypeSelection.SelectedIndex == 1)
                    {
                        GridViewByPatientPaymentReceive.Columns[(int)PaymentReciveReportByPatientTableColumn.PATIENT_NAME].Visible = false;
                    }

                    AdjustColumnWidthsAfterHiding(GridViewByPatientPaymentReceive);
                }
                else
                {
                    ErrorMsgPaymentReceiveReport.Text = InformationMsg;
                }
            }
            else if (RptReceiveAmount.Type == InventoryReportFilterType.BYAMOUNT)
            {
                GridViewByAmountPaymentReceive.Rows.Clear();
                decimal amountFrom = 0;
                decimal amountTo = 0;
                String dateTime = null!;
                string Patients = string.Empty;
                string Consultant = string.Empty;
                if (!decimal.TryParse(TextBoxReceivePaymentAmountFrom.Text, out amountFrom))
                {
                    return;
                }

                if (!decimal.TryParse(TextBoxReceivePaymentAmountTo.Text, out amountTo))
                {
                    return;
                }

                if (RptReceiveAmount.ReceiveAmountReportLines != null && RptReceiveAmount.ReceiveAmountReportLines.Count > 0)
                {
                    EnableButton(true);
                    int row = 0;
                    int i = 1;
                    bool hasItems = false;
                    decimal grandTotal = 0;

                    decimal groupSubtotal = 0;
                    string previousGroup = null;

                    var filteredLines = RptReceiveAmount.ReceiveAmountReportLines
                        .Where(x => Math.Round((decimal)x.Received, 2) >= amountFrom && Math.Round((decimal)x.Received, 2) <= amountTo)
                        .OrderBy(x => x.Date.Date) 
                        .ToList();

                    foreach (var LineItem in filteredLines)
                    {
                        hasItems = true;
                        IList<PatientLedger> lPatientLedger = PatientLedgerManager.Instance.ListAllEntryByPatientId(LineItem.Patient.Id);

                        string currentGroup = LineItem.Date.ToString(Global.Company.DateFormat);

                        if (LineItem.Received > 0 && LineItem.RefNumber != null)
                        {
                            if (previousGroup != null && previousGroup != currentGroup)
                            {
                                AddSubtotalRow(GridViewByAmountPaymentReceive, groupSubtotal, previousGroup, j);
                                grandTotal += groupSubtotal;
                                groupSubtotal = 0;
                                j++;
                                i = 1;
                            }

                            row = GridViewByAmountPaymentReceive.Rows.Add();

                            GridViewByAmountPaymentReceive.Rows[row].DefaultCellStyle.BackColor = RowColor[j % 2];
                            GridViewByAmountPaymentReceive.Rows[row].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            GridViewByAmountPaymentReceive.Rows[row].DefaultCellStyle.SelectionForeColor = Color.Black;
                            j++;

                            if (dateTime == null || dateTime != currentGroup)
                            {
                                GridViewByAmountPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByAmountTableColumn.DATE].Value = LineItem.Date.ToString(Global.Company.DateFormat);
                                dateTime = currentGroup;
                                Patients = string.Empty;
                                Consultant = string.Empty;
                            }

                            GridViewByAmountPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByAmountTableColumn.SNO].Value = i;
                            GridViewByAmountPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByAmountTableColumn.REFERENCE].Value = LineItem.RefNumber;

                            if (Patients == string.Empty || Patients != LineItem.Patient.Name)
                            {
                                GridViewByAmountPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByAmountTableColumn.PATIENT_NAME].Value = LineItem.Patient;
                                Patients = LineItem.Patient.Name;
                            }

                            foreach (PatientLedger patientLedger in lPatientLedger)
                            {
                                var patients = PatientLedgerManager.Instance.ListAllEntryByPatientId(patientLedger.PatientId).ToList();
                                bool hasOP = patients.Any(patient => patient.OpRegistrationId != null);
                                bool hasIP = patients.Any(patient => patient.InPatientAdmissionId != null);

                                GridViewByAmountPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByAmountTableColumn.OP_IP].Value = hasIP && hasOP ? "IP" : hasOP ? "OP" : "";

                                ConsultationNote ConsultationDoctor = patientLedger.OpRegistrationId.HasValue
                                    ? ConsultationNoteManager.Instance.GetConsultationNoteByOPRegisterId((long)patientLedger.OpRegistrationId)
                                    : null!;

                                if (ConsultationDoctor != null && (Consultant == string.Empty || Consultant != ConsultationDoctor.Consultant.Name))
                                {
                                    GridViewByAmountPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByAmountTableColumn.CONSULTANT].Value = ConsultationDoctor.Consultant.Name;
                                    Consultant = ConsultationDoctor.Consultant.Name;
                                }
                            }

                            GridViewByAmountPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByAmountTableColumn.DESCRIPTION].Value = LineItem.Description;
                            GridViewByAmountPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByAmountTableColumn.PAYMENT_TYPE].Value = LineItem.PaymentType;
                            GridViewByAmountPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByAmountTableColumn.RECEIVED].Value = LineItem.Received.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                            groupSubtotal += (decimal)LineItem.Received;
                            i++;
                            previousGroup = currentGroup;
                        }
                    }

                    if (groupSubtotal > 0)
                    {
                        AddSubtotalRow(GridViewByAmountPaymentReceive, groupSubtotal, previousGroup, j);
                        grandTotal += groupSubtotal;
                        j++;
                    }

                    AddGrandTotalRow(GridViewByAmountPaymentReceive, grandTotal, j);

                    if (!hasItems)
                    {
                        ErrorMsgPaymentReceiveReport.Text = "No information found..";
                        EnableButton(false);
                    }
                }
                else
                {
                    ErrorMsgPaymentReceiveReport.Text = InformationMsg;
                }
            }
            else if (RptReceiveAmount.Type == InventoryReportFilterType.BYCONSULTANT)
            {
                GridViewByConsultPaymentReceive.Rows.Clear();
                if (RptReceiveAmount.ReceiveAmountReportLineByConsult != null && RptReceiveAmount.ReceiveAmountReportLineByConsult.Count > 0)
                {
                    EnableButton(true);
                    int row = 0;
                    String dateTime = null!;
                    string Patients = string.Empty;
                    string Consultant = string.Empty;
                    decimal dateSubtotal = 0m;
                    decimal grandTotal = 0m;
                    string previousDate = null!;
                    bool isFirstRow = false;
                    int i = 1;
                    foreach (ReceiveAmountReportLineByConsult LineItem in RptReceiveAmount.ReceiveAmountReportLineByConsult)
                    {
                        IList<PatientLedger> lPatientLedger = PatientLedgerManager.Instance.ListAllEntryByPatientId(LineItem.Patient.Id);
                        String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);

                        if (LineItem.Received > 0 && LineItem.RefNumber != null)
                        {
                            row = GridViewByConsultPaymentReceive.Rows.Add();
                            foreach (PatientLedger patientLedger in lPatientLedger)
                            {
                                ConsultationNote ConsultationDoctor = patientLedger.OpRegistrationId.HasValue
                                    ? ConsultationNoteManager.Instance.GetConsultationNoteByOPRegisterId((long)patientLedger.OpRegistrationId)
                                    : null!;

                                if (ConsultationDoctor != null && (Consultant == string.Empty || Consultant != ConsultationDoctor.Consultant.Name))
                                {
                                    if (isFirstRow)
                                    {
                                        GridViewByConsultPaymentReceive.Rows[row].DefaultCellStyle.BackColor = RowColor[j % 2];
                                        GridViewByConsultPaymentReceive.Rows[row].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                                        GridViewByConsultPaymentReceive.Rows[row].DefaultCellStyle.SelectionForeColor = Color.Black;
                                        j++;
                                        GridViewByConsultPaymentReceive.Rows[row].DefaultCellStyle.BackColor = Color.LightGray;
                                        GridViewByConsultPaymentReceive.Rows[row].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                                        GridViewByConsultPaymentReceive.Rows[row].DefaultCellStyle.ForeColor = Color.Black;
                                        GridViewByConsultPaymentReceive.Rows[row].DefaultCellStyle.SelectionForeColor = Color.Black;
                                        GridViewByConsultPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByConsultedTableColumn.PAYMENT_TYPE].Value = "Sub Total";
                                        GridViewByConsultPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByConsultedTableColumn.RECEIVED].Value = dateSubtotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                        dateSubtotal = 0;
                                        row = GridViewByConsultPaymentReceive.Rows.Add();
                                    }
                                    GridViewByConsultPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByConsultedTableColumn.CONSULTANT].Value = ConsultationDoctor.Consultant.Name;
                                    Consultant = ConsultationDoctor.Consultant.Name;
                                    dateTime = null;
                                    Patients = string.Empty;
                                    isFirstRow = true;
                                    i = 1;
                                }
                            }
                            GridViewByConsultPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByConsultedTableColumn.SNO].Value = i;

                            if (Patients == string.Empty || Patients != LineItem.Patient.Name)
                            {
                                GridViewByConsultPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByConsultedTableColumn.PATIENT_NAME].Value = LineItem.Patient;
                                Patients = LineItem.Patient.Name;
                            }

                            if (dateTime == null || dateTime != stringLineItemDate)
                            {
                                GridViewByConsultPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByConsultedTableColumn.DATE].Value = LineItem.Date.ToString(Global.Company.DateFormat);
                                dateTime = stringLineItemDate;
                            }

                            GridViewByConsultPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByConsultedTableColumn.REFERENCE].Value = LineItem.RefNumber;

                            foreach (PatientLedger patientLedger in lPatientLedger)
                            {
                                bool hasOP = lPatientLedger.Any(patient => patient.OpRegistrationId != null);
                                bool hasIP = lPatientLedger.Any(patient => patient.InPatientAdmissionId != null);
                                GridViewByConsultPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByConsultedTableColumn.OP_IP].Value = hasIP && hasOP ? "IP" : hasOP ? "OP" : "";
                            }

                            GridViewByConsultPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByConsultedTableColumn.DESCRIPTION].Value = LineItem.Description;
                            GridViewByConsultPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByConsultedTableColumn.PAYMENT_TYPE].Value = LineItem.PaymentType;
                            GridViewByConsultPaymentReceive.Rows[row].Cells[(int)PaymentReciveReportByConsultedTableColumn.RECEIVED].Value = LineItem.Received.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                            dateSubtotal += (decimal)LineItem.Received;
                            grandTotal += (decimal)LineItem.Received;
                            i++;
                            previousDate = stringLineItemDate;
                            irow++;
                        }
                    }

                    if (previousDate != null)
                    {
                        int subtotalRow = GridViewByConsultPaymentReceive.Rows.Add();
                        GridViewByConsultPaymentReceive.Rows[subtotalRow].DefaultCellStyle.BackColor = Color.LightGray;
                        GridViewByConsultPaymentReceive.Rows[subtotalRow].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                        GridViewByConsultPaymentReceive.Rows[subtotalRow].DefaultCellStyle.ForeColor = Color.Black;
                        GridViewByConsultPaymentReceive.Rows[subtotalRow].DefaultCellStyle.SelectionForeColor = Color.Black;
                        GridViewByConsultPaymentReceive.Rows[subtotalRow].Cells[(int)PaymentReciveReportByConsultedTableColumn.PAYMENT_TYPE].Value = "Sub Total";
                        GridViewByConsultPaymentReceive.Rows[subtotalRow].Cells[(int)PaymentReciveReportByConsultedTableColumn.RECEIVED].Value = dateSubtotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    }

                    int grandTotalRow = GridViewByConsultPaymentReceive.Rows.Add();
                    GridViewByConsultPaymentReceive.Rows[grandTotalRow].DefaultCellStyle.BackColor = Color.LightGray;
                    GridViewByConsultPaymentReceive.Rows[grandTotalRow].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                    GridViewByConsultPaymentReceive.Rows[grandTotalRow].DefaultCellStyle.ForeColor = Color.Black;
                    GridViewByConsultPaymentReceive.Rows[grandTotalRow].DefaultCellStyle.SelectionForeColor = Color.Black;
                    GridViewByConsultPaymentReceive.Rows[grandTotalRow].Cells[(int)PaymentReciveReportByConsultedTableColumn.PAYMENT_TYPE].Value = "Grand Total";
                    GridViewByConsultPaymentReceive.Rows[grandTotalRow].Cells[(int)PaymentReciveReportByConsultedTableColumn.RECEIVED].Value = grandTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                }
                else
                {
                    ErrorMsgPaymentReceiveReport.Text = InformationMsg;
                }
            }
        }
        private void BtnPaymentReciveSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                GridViewByDatePaymentRecive.Rows.Clear();
                GridViewByPatientPaymentReceive.Rows.Clear();
                GridViewByAmountPaymentReceive.Rows.Clear();
                GridViewByConsultPaymentReceive.Rows.Clear();
                EnableButton(false);
                if (FormValidate())
                {
                    LoadReceivePaymentReport();
                }
            }
            catch (Exception ex)
            {
                ErrorMsgPaymentReceiveReport.Text = "Error fetching Stock (Error:" + ex.Message + ")";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            GridViewByPatientPaymentReceive.DefaultCellStyle.SelectionForeColor = GridViewByPatientPaymentReceive.DefaultCellStyle.ForeColor;
            GridViewByPatientPaymentReceive.DefaultCellStyle.SelectionBackColor = GridViewByPatientPaymentReceive.DefaultCellStyle.BackColor;
        }

        private void CheckedTreeComboBoxConsulted_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }

        private void CheckedTreeComboBoxPatient_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }
        private void NumericOnlyTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private string SelectedNodesText(ToolstripCheckedTreeComboBox ComboTreeBox)
        {
            int i = 0;
            string Name = string.Empty;
            if (ComboTreeBox.Nodes.Count > 0)
            {
                foreach (ComboTreeNode ComboTreeNode in ComboTreeBox.Nodes)
                {
                    if (ComboTreeNode != null)
                    {
                        if (ComboTreeNode.Checked == true)
                        {
                            if (ComboTreeNode.Name == "All")
                            {
                                Name = string.Empty;
                                Name = "All Location";
                                break;
                            }
                            else
                            {
                                Name += string.IsNullOrEmpty(Name) ? ComboTreeNode.Text : (", " + ComboTreeNode.Text);
                            }
                            i++;
                        }
                    }
                }
            }
            return Name;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string lFromDate = ((DateTime)PaymentReciveReportFromDate.Date).ToString(Global.Company.DateFormat);
            string lToDate = ((DateTime)PaymentReciveReportToDate.Date).ToString(Global.Company.DateFormat);
            ReceiveAmountReportSavePrint ReceiveAmountReportSavePrint = new ReceiveAmountReportSavePrint();
            ReceiveAmountReportSavePrint.ExportOrPrintToFile(RptReceiveAmount, "ReceiveAmount", "pdf", false);
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            string lFromDate = ((DateTime)PaymentReciveReportFromDate.Date).ToString(Global.Company.DateFormat);
            string lToDate = ((DateTime)PaymentReciveReportToDate.Date).ToString(Global.Company.DateFormat);
            ReceiveAmountReportSavePrint ReceiveAmountReportSavePrint = new ReceiveAmountReportSavePrint();
            ReceiveAmountReportSavePrint.ExportOrPrintToFile(RptReceiveAmount, "ReceiveAmount", "pdf", true);
        }

        private void AddSubtotalRow(DataGridView gridView, decimal subtotal, string date, int rowIndex)
        {
            int row = gridView.Rows.Add();
            gridView.Rows[row].DefaultCellStyle.BackColor = Color.LightGray;
            gridView.Rows[row].DefaultCellStyle.SelectionBackColor = Color.LightGray;
            gridView.Rows[row].DefaultCellStyle.ForeColor = Color.Black;
            gridView.Rows[row].DefaultCellStyle.SelectionForeColor = Color.Black;
            gridView.Rows[row].Cells[(int)PaymentReciveReportByConsultedTableColumn.PAYMENT_TYPE].Value = "Sub Total";
            gridView.Rows[row].Cells[(int)PaymentReciveReportByConsultedTableColumn.RECEIVED].Value = subtotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
        }

        private void AddGrandTotalRow(DataGridView gridView, decimal grandTotal, int rowIndex)
        {
            int row = gridView.Rows.Add();
            gridView.Rows[row].DefaultCellStyle.BackColor = Color.LightGray;
            gridView.Rows[row].DefaultCellStyle.SelectionBackColor = Color.LightGray;
            gridView.Rows[row].DefaultCellStyle.ForeColor = Color.Black;
            gridView.Rows[row].DefaultCellStyle.SelectionForeColor = Color.Black;
            gridView.Rows[row].Cells[(int)PaymentReciveReportByConsultedTableColumn.PAYMENT_TYPE].Value = "Grand Total";
            gridView.Rows[row].Cells[(int)PaymentReciveReportByConsultedTableColumn.RECEIVED].Value = grandTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
        }

        private void GridViewByPatientPaymentReceive_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {

            if (GridViewByPatientPaymentReceive.Rows[e.RowIndex].Cells[(int)SalesReportByCategoryTableColumn.SNO].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
            0, e.RowBounds.Top,
            this.GridViewByPatientPaymentReceive.Columns.GetColumnsWidth(
                DataGridViewElementStates.Visible) -
            this.GridViewByPatientPaymentReceive.HorizontalScrollingOffset,
            e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = GridViewByPatientPaymentReceive.Rows[e.RowIndex].Cells[(int)PaymentReciveReportByPatientTableColumn.PATIENT_NAME].Value != null ? GridViewByPatientPaymentReceive.Rows[e.RowIndex].Cells[(int)PaymentReciveReportByPatientTableColumn.PATIENT_NAME].Value.ToString()! : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds);
            }
        }

        private void GridViewByPatientPaymentReceive_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewByPatientPaymentReceive.Rows[e.RowIndex].Cells[(int)PaymentReciveReportByPatientTableColumn.PATIENT_NAME].Value != null)
            {
                if (e.ColumnIndex == (int)PaymentReciveReportByPatientTableColumn.DATE)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)PaymentReciveReportByPatientTableColumn.OP_IP)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)PaymentReciveReportByPatientTableColumn.CONSULTANT)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
            }

            if (e.RowIndex > -1 && GridViewByPatientPaymentReceive.Rows[e.RowIndex].Cells[(int)PaymentReciveReportByPatientTableColumn.SNO].Value == null)
            {
                if (e.ColumnIndex < (int)PaymentReciveReportByPatientTableColumn.PAYMENT_TYPE && (GridViewByPatientPaymentReceive.Rows[e.RowIndex].Cells[(int)PaymentReciveReportByPatientTableColumn.PAYMENT_TYPE].Value == "Sub Total"
                    || GridViewByPatientPaymentReceive.Rows[e.RowIndex].Cells[(int)PaymentReciveReportByPatientTableColumn.PAYMENT_TYPE].Value == "Grand Total"))
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)PaymentReciveReportByPatientTableColumn.PAYMENT_TYPE && (GridViewByPatientPaymentReceive.Rows[e.RowIndex].Cells[(int)PaymentReciveReportByPatientTableColumn.PAYMENT_TYPE].Value == "Sub Total"
                    || GridViewByPatientPaymentReceive.Rows[e.RowIndex].Cells[(int)PaymentReciveReportByPatientTableColumn.PAYMENT_TYPE].Value == "Grand Total"))
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
            }
        }

        private void GridViewByPatientPaymentReceive_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            
        }

        private void GridViewByDatePaymentRecive_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewByDatePaymentRecive.Rows[e.RowIndex].Cells[(int)PaymentReciveReportByDateTableColumn.SNO].Value == null)
            {
                if (e.ColumnIndex < (int)PaymentReciveReportByDateTableColumn.PAYMENT_TYPE && (GridViewByDatePaymentRecive.Rows[e.RowIndex].Cells[(int)PaymentReciveReportByDateTableColumn.PAYMENT_TYPE].Value == "Sub Total"
                    || GridViewByDatePaymentRecive.Rows[e.RowIndex].Cells[(int)PaymentReciveReportByDateTableColumn.PAYMENT_TYPE].Value == "Grand Total"))
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)PaymentReciveReportByDateTableColumn.PAYMENT_TYPE && (GridViewByDatePaymentRecive.Rows[e.RowIndex].Cells[(int)PaymentReciveReportByDateTableColumn.PAYMENT_TYPE].Value == "Sub Total"
                    || GridViewByDatePaymentRecive.Rows[e.RowIndex].Cells[(int)PaymentReciveReportByDateTableColumn.PAYMENT_TYPE].Value == "Grand Total"))
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
            }
        }

        private void GridViewByAmountPaymentReceive_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewByAmountPaymentReceive.Rows[e.RowIndex].Cells[(int)PaymentReciveReportByAmountTableColumn.SNO].Value == null)
            {
                if (e.ColumnIndex < (int)PaymentReciveReportByAmountTableColumn.PAYMENT_TYPE && (GridViewByAmountPaymentReceive.Rows[e.RowIndex].Cells[(int)PaymentReciveReportByAmountTableColumn.PAYMENT_TYPE].Value == "Sub Total"
                    || GridViewByAmountPaymentReceive.Rows[e.RowIndex].Cells[(int)PaymentReciveReportByAmountTableColumn.PAYMENT_TYPE].Value == "Grand Total"))
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)PaymentReciveReportByAmountTableColumn.PAYMENT_TYPE && (GridViewByAmountPaymentReceive.Rows[e.RowIndex].Cells[(int)PaymentReciveReportByAmountTableColumn.PAYMENT_TYPE].Value == "Sub Total"
                    || GridViewByAmountPaymentReceive.Rows[e.RowIndex].Cells[(int)PaymentReciveReportByAmountTableColumn.PAYMENT_TYPE].Value == "Grand Total"))
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
            }
        }

        private void GridViewByConsultPaymentReceive_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewByConsultPaymentReceive.Rows[e.RowIndex].Cells[(int)PaymentReciveReportByConsultedTableColumn.SNO].Value == null)
            {
                if (e.ColumnIndex < (int)PaymentReciveReportByConsultedTableColumn.PAYMENT_TYPE && (GridViewByConsultPaymentReceive.Rows[e.RowIndex].Cells[(int)PaymentReciveReportByConsultedTableColumn.PAYMENT_TYPE].Value == "Sub Total"
                    || GridViewByConsultPaymentReceive.Rows[e.RowIndex].Cells[(int)PaymentReciveReportByConsultedTableColumn.PAYMENT_TYPE].Value == "Grand Total"))
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)PaymentReciveReportByConsultedTableColumn.PAYMENT_TYPE && (GridViewByConsultPaymentReceive.Rows[e.RowIndex].Cells[(int)PaymentReciveReportByConsultedTableColumn.PAYMENT_TYPE].Value == "Sub Total"
                    || GridViewByConsultPaymentReceive.Rows[e.RowIndex].Cells[(int)PaymentReciveReportByConsultedTableColumn.PAYMENT_TYPE].Value == "Grand Total"))
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
            }
        }
    }
}
