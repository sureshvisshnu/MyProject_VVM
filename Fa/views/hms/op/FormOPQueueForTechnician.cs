using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.views.hms.patient;
using fa.model.Hms.Op;
using fa.api.Hms;
using fa.common;

namespace fa.views.hms.op
{
    public partial class FormOPQueue : FormPatientBase
    {
        public static string TechnicianOpSearchMsg = "No entry found!";

        long PatientId = 0L;
        public FormOPQueue()
        {
            InitializeComponent();
        }

        private void opQueueGrid1_Load(object sender, EventArgs e)
        {
            BtnOpQueueVitalEntry.Enabled = false;
            BtnOpQueueLabResult.Enabled = false;
            OpQueueGrid.IsMyQueue = false;
        }

        private void FormOPQueue_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                BtnOpSearch_Click(sender, e);
                if (OpQueueGrid.SingleClickSelection)
                {
                    if (OpQueueGrid.OpId != null)
                    {
                        Registration OpRegistration = OpManager.Instance.GetOpRegistrationById((long)OpQueueGrid.OpId);
                        if (OpRegistration != null)
                        {
                            PatientInfoMiniHorizontal.Type = PatientTypes.OutPatient;
                            PatientInfoMiniHorizontal.PatientId = OpRegistration.PatientId;
                            PatientId = (long)OpRegistration.PatientId!;
                            BtnOpQueueVitalEntry.Enabled = true;
                            BtnOpQueueLabResult.Enabled = true;
                            BtnCompleteVisit.Enabled = true;
                        }
                    }
                    else
                    {
                        PatientInfoMiniHorizontal.Clear();
                        PatientId = 0L;
                        BtnOpQueueVitalEntry.Enabled = false;
                        BtnOpQueueLabResult.Enabled = false;
                        BtnCompleteVisit.Enabled = false;
                    }
                }
                this.BeginInvoke((MethodInvoker)delegate
                {
                    TextBoxOpSearch.Focus();
                });
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnOpQueueVitalEntry_Click(object sender, EventArgs e)
        {
            if (PatientId != 0)
            {
                this.PatientIdTransport.Text = OpManager.Instance.GetOpRegistrationById((long)OpQueueGrid.OpId!).PatientId.ToString();
                FormVitals FormVitals = new FormVitals(this);
                FormVitals.OpId = (long)OpQueueGrid.OpId;
                FormVitals.ShowDialog();

            }
        }

        private void BtnOpQueueLabResult_Click(object sender, EventArgs e)
        {
            if (PatientId != 0)
            {
                this.PatientIdTransport.Text = PatientId.ToString();
                FormLabTestResults FormLabTestResults = new FormLabTestResults(this);
                FormLabTestResults.ShowDialog();
            }
        }


        private void LoadOpQueue()
        {
            BtnOpQueueVitalEntry.Enabled = false;
            BtnOpQueueLabResult.Enabled = false;
            BtnCompleteVisit.Enabled = false;
            btnPerformProcedures.Enabled = false;
            OpQueueGrid.SearchString = TextBoxOpSearch.Text;
            OpQueueGrid.OPStatus = Status.OPEN;
            OpQueueGrid.IsMyQueue = false;
        }

        private void BtnOpSearch_Click(object sender, EventArgs e)
        {
            ErrorMsg.Text = "";
            LoadOpQueue();
            if (OpQueueGrid.IsEmpty)
            {
                ErrorMsg.Text = TechnicianOpSearchMsg;
                this.BeginInvoke((MethodInvoker)delegate
                {
                    TextBoxOpSearch.Focus();
                });
            }
            else
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    TextBoxOpSearch.Focus();
                });
                OpQueueGrid_Click(sender, e);
            }
        }


        private void OpQueueGrid_Click(object sender, EventArgs e)
        {
            if (OpQueueGrid.SingleClickSelection)
            {
                if (OpQueueGrid.OpId != null)
                {
                    Registration OpRegistration = OpManager.Instance.GetOpRegistrationById((long)OpQueueGrid.OpId);
                    if (OpRegistration != null)
                    {
                        PatientInfoMiniHorizontal.PatientId = OpRegistration.PatientId;
                        PatientId = (long)OpRegistration.PatientId!;
                        BtnOpQueueVitalEntry.Enabled = true;
                        BtnOpQueueLabResult.Enabled = true;
                        BtnCompleteVisit.Enabled = true;
                        btnPerformProcedures.Enabled = true;
                    }
                }
                else
                {
                    PatientInfoMiniHorizontal.Clear();
                    PatientId = 0L;
                    BtnOpQueueVitalEntry.Enabled = false;
                    BtnOpQueueLabResult.Enabled = false;
                    BtnCompleteVisit.Enabled = false;
                    btnPerformProcedures.Enabled = false;
                }
            }
        }

        private void BtnCompleteVisit_Click(object sender, EventArgs e)
        {
            if (PatientId != 0)
            {
                FormOPComplete FormOPComplete = new FormOPComplete(this);
                FormOPComplete.OpId = (long)OpQueueGrid.OpId!;
                FormOPComplete.PatientId = (long)OpManager.Instance.GetOpRegistrationById((long)OpQueueGrid.OpId).PatientId!;
                FormOPComplete.ShowDialog();
                FormOPQueue_Load(sender, EventArgs.Empty);
            }
        }
        private void OpQueueGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (OpQueueGrid.OpId != null)
            {
                Registration OpRegistration = OpManager.Instance.GetOpRegistrationById((long)OpQueueGrid.OpId);
                if (OpRegistration != null)
                {
                    PatientInfoMiniHorizontal.PatientId = OpRegistration.PatientId;
                    PatientId = (long)OpRegistration.PatientId!;
                    BtnOpQueueVitalEntry.Enabled = true;
                    BtnOpQueueLabResult.Enabled = true;
                    BtnCompleteVisit.Enabled = true;
                    btnPerformProcedures.Enabled = true;
                }
            }
            else
            {
                PatientInfoMiniHorizontal.Clear();
                PatientId = 0L;
                BtnOpQueueVitalEntry.Enabled = false;
                BtnOpQueueLabResult.Enabled = false;
                BtnCompleteVisit.Enabled = false;
                btnPerformProcedures.Enabled = false;
            }
        }
        private void OpQueueGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if (OpQueueGrid.OpId != null)
            {
                Registration OpRegistration = OpManager.Instance.GetOpRegistrationById((long)OpQueueGrid.OpId);
                if (OpRegistration != null)
                {
                    PatientInfoMiniHorizontal.PatientId = OpRegistration.PatientId;
                    PatientId = (long)OpRegistration.PatientId!;
                    BtnOpQueueVitalEntry.Enabled = true;
                    BtnOpQueueLabResult.Enabled = true;
                    BtnCompleteVisit.Enabled = true;
                    btnPerformProcedures.Enabled = true;
                }
            }
            else
            {
                PatientInfoMiniHorizontal.Clear();
                PatientId = 0L;
                BtnOpQueueVitalEntry.Enabled = false;
                BtnOpQueueLabResult.Enabled = false;
                BtnCompleteVisit.Enabled = false;
                btnPerformProcedures.Enabled = false;
            }
        }

        private void btnPerformProcedures_Click(object sender, EventArgs e)
        {
            if (OpQueueGrid.OpId != null && OpQueueGrid.OpId != 0L)
            {
                FormPatientProcedures PatientProcedures = new FormPatientProcedures(this);
                this.PatientIdTransport.Text = OpManager.Instance.GetOpRegistrationById((long)OpQueueGrid.OpId).PatientId.ToString();
                Registration Registration = OpManager.Instance.GetRegisterByPatientId(int.Parse(PatientIdTransport.Text!));
                PatientProcedures.ShowDialog();
            }
        }

        private void TextBoxOpSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnOpSearch_Click(sender, e);
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F5))
            {
                btnPerformProcedures.PerformClick();
            }
            else if (keyData == (Keys.F6))
            {
                BtnOpQueueVitalEntry.PerformClick();
            }
            else if (keyData == (Keys.F7))
            {
                BtnOpQueueLabResult.PerformClick();
            }
            else if (keyData == (Keys.F8))
            {
                BtnCompleteVisit.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void OpQueueGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                btnPerformProcedures.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxOpSearch.Focus();
            }
        }

        private void btnPerformProcedures_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnOpQueueVitalEntry.Select();
            }
        }

        private void BtnOpQueueVitalEntry_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnOpQueueLabResult.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                btnPerformProcedures.Select();
            }
        }

        private void BtnOpQueueLabResult_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnCompleteVisit.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnOpQueueVitalEntry.Select();
            }
        }

        private void BtnCompleteVisit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxOpSearch.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnOpQueueLabResult.Select();
            }
        }

        private void toolStrip1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                if (!OpQueueGrid.IsEmpty)
                {
                    e.IsInputKey = true;
                    OpQueueGrid.Focus();
                }
                else
                {
                    e.IsInputKey = true;
                    TextBoxOpSearch.Focus();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnCompleteVisit.Focus();
            }
        }
    }
}
