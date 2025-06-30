using System;
using System.Collections.Generic;
using System.Windows.Forms;
using fa.api.Hms;
using fa.model.Hms.Op;
using fa.model.Hms.Ip;

namespace fa.views.controls.hms
{
    public partial class PatientVisitControl : UserControl
    {
        OpManager OpManager = null;
        IpManager IpManager = null;
        private string _patientId = string.Empty;
        public string PatientId
        {
            get
            {
                return _patientId;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.GridViewIpOpVisit.Rows.Clear();
                }
                else if (!string.IsNullOrEmpty(value))
                {
                    _patientId = value;
                    loadPatientVisit();
                }
                else
                {
                    //do nothing as patient id did not change.
                }
            }
        }

        public PatientVisitControl()
        {
            InitializeComponent();
        }

        private void PatientVisitWidget_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_patientId))
            {
                loadPatientVisit();
            }
        }

        private void loadPatientVisit()
        {
            GridViewIpOpVisit.Rows.Clear();
            if (!string.IsNullOrEmpty(PatientId))
            {
                OpManager = OpManager.Instance;
                IpManager = IpManager.Instance;
                IList<Registration> OpRegistration = OpManager.ListAllOpByPatientId(long.Parse(PatientId));
                IList<InPatientAdmission> InPatientAdmission = IpManager.ListAllIpByPatientId(long.Parse(PatientId));
                if (OpRegistration.Count + InPatientAdmission.Count > 0)
                {
                    GridViewIpOpVisit.Rows.Add(OpRegistration.Count + InPatientAdmission.Count);
                    int i = 0;
                    if (OpRegistration.Count > 0)
                    {
                        foreach (var lOpRegistration in OpRegistration)
                        {
                            GridViewIpOpVisit.Rows[i].Cells[0].Value = lOpRegistration.DateOfRegistration.ToString(Global.Company.DateFormat);
                            GridViewIpOpVisit.Rows[i].Cells[1].Value = "OP";
                            i++;
                        }
                    }
                    if (InPatientAdmission.Count > 0)
                    {
                        foreach (var lInPatientAdmission in InPatientAdmission)
                        {
                            GridViewIpOpVisit.Rows[i].Cells[0].Value = lInPatientAdmission.DateOfAdmission.ToString(Global.Company.DateFormat);
                            GridViewIpOpVisit.Rows[i].Cells[1].Value = "IP";
                            i++;
                        }
                    }
                }

            }
        }

        private void PatientVisitControl_ClientSizeChanged(object sender, EventArgs e)
        {
            if (this.Height > 25 && this.Width > 25)
            {
                GridViewIpOpVisit.Width = this.Width - 10;
                GridViewIpOpVisit.Height = this.Height - 25;
            }
        }
    }
}
