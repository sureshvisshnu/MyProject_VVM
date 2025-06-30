using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.api.Hms;
using fa.api.UserProfile;
using fa.common;
using fa.model.Hms.Op;
using fa.model.UserProfile;
using fa.views.hms.patient;

namespace fa.views.hms.op
{
    public partial class FormOPQueueForDoctor : FormPatientBase
    {
        public static string DoctorOpSearchMsg = "No entry found!";
        public bool RefreshRecord = false;
        public FormOPQueueForDoctor()
        {
            InitializeComponent();
        }

        private void opQueueGrid1_Load(object sender, EventArgs e)
        {
            BtnConsult.Enabled = !OpQueueGrid.IsEmpty;
        }

        private void Consult(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.PatientIdTransport.Text = OpManager.Instance.GetOpRegistrationById((long)OpQueueGrid.OpId).PatientId.ToString();
            FormConsulting formConsulting = new FormConsulting(this);
            formConsulting.PatientOpId = (long)OpQueueGrid.OpId;
            formConsulting.ShowDialog();
            RefreshRecord = formConsulting.RecordEnter;
            if (RefreshRecord)
            {
                BtnQueueDoctorSearch_Click(sender, e);
                RefreshRecord = false;
            }
            Cursor.Current = Cursors.Default;
        }

        private void FormOPQueue_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                BtnQueueDoctorSearch_Click(sender, e);
                PatientInformation();
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

        private void BtnQueueDoctorSearch_Click(object sender, EventArgs e)
        {
            ErrorMsg.Text = "";
            LoadOpQueue(checkBoxMyQueue.Checked);
            if (OpQueueGrid.IsEmpty)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    TextBoxOpSearch.Focus();
                });
                ErrorMsg.Text = DoctorOpSearchMsg;
            }
            else
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    TextBoxOpSearch.Focus();
                });
                PatientInformation();
            }
        }
        private void LoadOpQueue(bool IsMyQueue)
        {
            OpQueueGrid.SearchString = TextBoxOpSearch.Text;
            OpQueueGrid.IsMyQueue = IsMyQueue;
            OpQueueGrid.OPStatus = Status.OPEN;
            BtnConsult.Enabled = !OpQueueGrid.IsEmpty;
        }

        private void TextBoxOpSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnQueueDoctorSearch_Click(sender, e);
            }
        }

        public void PatientInformation()
        {
            if (OpQueueGrid.OpId != null)
            {
                Registration OpRegistration = OpManager.Instance.GetOpRegistrationById((long)OpQueueGrid.OpId);
                if (OpRegistration != null)
                {
                    patientInfoMin1.Type = (int)PatientTypes.OutPatient;
                    patientInfoMin1.PatientId = OpRegistration.PatientId;
                    BtnConsult.Enabled = !OpQueueGrid.IsEmpty;
                }
            }
            else
            {
                patientInfoMin1.Clear();
                BtnConsult.Enabled = !OpQueueGrid.IsEmpty;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F8)
            {
                BtnConsult.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void OpQueueGrid_KeyDown(object sender, KeyEventArgs e)
        {
            PatientInformation();
        }

        private void OpQueueGrid_KeyUp(object sender, KeyEventArgs e)
        {
            PatientInformation();
        }

        private void OpQueueGrid_Click(object sender, EventArgs e)
        {
            PatientInformation();
        }

        private void OpQueueGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                checkBoxMyQueue.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                ActiveControl = toolStrip1;
                toolStrip1.Select();
                TextBoxOpSearch.Focus();
                e.IsInputKey = true;
            }
        }

        private void BtnConsult_PreviewKeyDown_1(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                ActiveControl = toolStrip1;
                toolStrip1.Select();
                TextBoxOpSearch.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                checkBoxMyQueue.Focus();
            }
        }

        private void toolStrip1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                if (!OpQueueGrid.IsEmpty)
                {
                    OpQueueGrid.Focus();
                    e.IsInputKey = true;
                }
                else
                {
                    ActiveControl = toolStrip1;
                    toolStrip1.Select();
                    TextBoxOpSearch.Focus();
                    e.IsInputKey = true;
                }

            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnConsult.Focus();
            }
        }

        private void checkBoxMyQueue_CheckedChanged(object sender, EventArgs e)
        {
            LoadOpQueue(checkBoxMyQueue.Checked);
            if (!OpQueueGrid.IsEmpty)
            {
                PatientInformation();
            }
        }
        private void checkBoxMyQueue_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                BtnConsult.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                OpQueueGrid.Focus();
            }
        }

        private void BtnConsult_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F8)
            {
                BtnConsult.PerformClick();
            }
        }

        private void checkBoxMyQueue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F8)
            {
                BtnConsult.PerformClick();
            }
        }
    }
}
