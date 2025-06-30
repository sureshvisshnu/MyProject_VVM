using fa.api.catalog;
using fa.api.Hms;
using fa.api.OrderManagement;
using fa.model.Catalog;
using fa.model.hms.common;
using fa.model.Hms.Op;
using fa.model.OrderManagement;
using fa.views.sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.views.hms.helper
{

    public enum RecentPrescriptionGridColumn
    {
        DATE, TOKEN, NAME, ID, PRESCRIPTION, ISPAYMENTRECEIVED, ISDELIVER, NOTEID
    }
    public partial class FormRecentPrescription : FormBase
    {
        public static string SelectPrescriptionErrorMsg = "Please select prescription.";

        public IList<ConsultationNote> Notes = null;
        FormBase parent = null;
        public FormRecentPrescription(object sender)
        {
            if (sender is FormItembasedSales)
            {
                parent = (FormItembasedSales)sender;
            }
            InitializeComponent();
        }

        private void FormRecentPrescription_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            LoadConsPrescriptionEntry();
            GridViewRecentPrescription.Select();
            Cursor.Current = Cursors.Default;
        }
        public void LoadConsPrescriptionEntry()
        {
            if (Notes.Count > 0)
            {
                int i = 0;
                foreach (ConsultationNote Note in Notes)
                {
                    IList<ConsultedPrescription> lConsultedPrescription = ConsultationNoteManager.Instance.ListPrescriptionByNoteId(Note.Id);
                    if (lConsultedPrescription != null && lConsultedPrescription.Count > 0)
                    {
                        GridViewRecentPrescription.Rows.Add(1);
                        string PrescriptionDetails = string.Empty;
                        if (Note.SaleEntryId != null)
                        {
                            SaleEntry SaleEntry = SalesManager.Instance.GetSaleEntry((long)Note.SaleEntryId);
                            if (SaleEntry != null)
                            {
                                GridViewRecentPrescription.Rows[i].Cells[(int)RecentPrescriptionGridColumn.ISPAYMENTRECEIVED].Value = SaleEntry.isPaymentReceived;
                                GridViewRecentPrescription.Rows[i].Cells[(int)RecentPrescriptionGridColumn.ISDELIVER].Value = SaleEntry.hasDelivered;

                            }
                        }
                        foreach (ConsultedPrescription ConsPres in lConsultedPrescription)
                        {
                            Product Product = ConsPres.Prescription.ProductId!=null? CatalogProductManager.Instance.GetProductInfoById((long)ConsPres.Prescription.ProductId):null;
                            PrescriptionDetails = string.IsNullOrEmpty(PrescriptionDetails) ? (Product!=null?Product.Name: ConsPres.CustomPrescription) : PrescriptionDetails + ", " + (Product != null ? Product.Name : ConsPres.CustomPrescription);
                        }
                        if (Note.OpRegistrationId != null)
                        {
                            Registration Registration = OpManager.Instance.GetOpRegistrationById((long)Note.OpRegistrationId);
                            if (Registration != null)
                            {
                                GridViewRecentPrescription.Rows[i].Cells[(int)RecentPrescriptionGridColumn.TOKEN].Value = Registration.TockenNo;
                                GridViewRecentPrescription.Rows[i].Cells[(int)RecentPrescriptionGridColumn.NAME].Value = Registration.Patient.Name;
                                GridViewRecentPrescription.Rows[i].Cells[(int)RecentPrescriptionGridColumn.ID].Value = Registration.Patient.PatientNumber;
                            }
                        }
                        else
                        {
                            Registration Registration = OpManager.Instance.GetOpRegistrationByNote(Note);
                            if (Registration != null)
                            {
                                GridViewRecentPrescription.Rows[i].Cells[(int)RecentPrescriptionGridColumn.TOKEN].Value = Registration.TockenNo;
                                GridViewRecentPrescription.Rows[i].Cells[(int)RecentPrescriptionGridColumn.NAME].Value = Registration.Patient.Name;
                                GridViewRecentPrescription.Rows[i].Cells[(int)RecentPrescriptionGridColumn.ID].Value = Registration.Patient.PatientNumber;
                            }
                        }
                        GridViewRecentPrescription.Rows[i].Cells[(int)RecentPrescriptionGridColumn.DATE].Value = Note.Date.ToShortDateString() + " " + Note.Date.ToShortTimeString();
                        GridViewRecentPrescription.Rows[i].Cells[(int)RecentPrescriptionGridColumn.NOTEID].Value = Note.Id;
                        GridViewRecentPrescription.Rows[i].Cells[(int)RecentPrescriptionGridColumn.PRESCRIPTION].Value = PrescriptionDetails;
                        i++;
                    }

                }

            }
        }

        private void GridViewRecentPrescription_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                BtnPrescrptionSelect_Click(sender, e);
            }
        }

        private void GridViewRecentPrescription_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == Convert.ToChar(Keys.Enter))
            {
                BtnPrescrptionSelect_Click(sender, e);
            }
        }

        private void BtnPrescrptionSelect_Click(object sender, EventArgs e)
        {
            if (GridViewRecentPrescription.Rows.Count > 0)
            {
                if (GridViewRecentPrescription.CurrentRow.Index > -1)
                {
                    if (parent is FormItembasedSales)
                    {
                        ((FormItembasedSales)parent).NoteId = (long)GridViewRecentPrescription.CurrentRow.Cells[(int)RecentPrescriptionGridColumn.NOTEID].Value;
                    }
                    this.Close();
                }
                else
                {
                    RecentPrescriptionErrorMsg.Text = SelectPrescriptionErrorMsg;
                }
            }
        }

        private void BtnPrescriptionCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnPrescrptionSelect_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                if (GridViewRecentPrescription.Rows.Count > 0)
                {
                    GridViewRecentPrescription.Focus();
                    GridViewRecentPrescription.CurrentCell = GridViewRecentPrescription[0, 0];

                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewRecentPrescription.Rows.Count > 0)
                {
                    GridViewRecentPrescription.Focus();
                    GridViewRecentPrescription.CurrentCell = GridViewRecentPrescription[0, GridViewRecentPrescription.Rows.Count - 1];

                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnPrescrptionSelect.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnPrescriptionCancel.PerformClick();
                return true;
            }
            try
            {

                if (keyData == (Keys.Tab) && GridViewRecentPrescription.CurrentRow.Index > -1)
                {
                    if (GridViewRecentPrescription.CurrentCell.RowIndex != GridViewRecentPrescription.Rows.Count - 1)
                    {
                        GridViewRecentPrescription.CurrentCell = GridViewRecentPrescription[0, GridViewRecentPrescription.CurrentCell.RowIndex + 1];
                    }
                    else
                    {
                        BtnPrescrptionSelect.Select();
                    }

                }
                if (keyData == (Keys.Tab | Keys.Shift) && GridViewRecentPrescription.CurrentRow.Index > -1)
                {
                    if (GridViewRecentPrescription.CurrentRow.Index != 0)
                    {
                        GridViewRecentPrescription.CurrentCell = GridViewRecentPrescription[0, GridViewRecentPrescription.CurrentCell.RowIndex];
                    }
                    else
                    {
                        BtnPrescrptionSelect.Select();

                    }
                }

            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
