using fa.views.Systems;
using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace fa.libraries.Validation
{
    public class WorkStationValidation
    {
        private static volatile WorkStationValidation instance;
        private static object syncRoot = new Object();
        WorkStationValidation()
        {

        }
        public static WorkStationValidation Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new WorkStationValidation();
                    }
                }

                return instance;
            }
        }
        private string ErrorMsg = "";
        public Boolean InitializeWorkstationValidation()
        {           
            if(!ValidateField())
            {
                return WorkStationCreation();
            }
            return true;
        }
        public bool ValidateField()
        {
            ErrorMsg = "";
            RegistryKey key = Global.WorkStation;
            if (key == null)
            {
                ErrorMsg = "WorkStation not available";
                return false;
            }
            else if ((key.GetValue("WorkStationID") == null || key.GetValue("WorkStationID").ToString() == string.Empty)
            && (key.GetValue("WorkStationName") == null || key.GetValue("WorkStationName").ToString() == string.Empty))
            {
                ErrorMsg = "WorkStation not available";
                return false;
            }
            else if ((key.GetValue("WorkStationID") == null || key.GetValue("WorkStationID").ToString() == string.Empty)
            || (key.GetValue("WorkStationName") == null || key.GetValue("WorkStationName").ToString() == string.Empty))
            {
                ErrorMsg = "WorkStation Id/Name not available";
                return false;
            }
            return true;
        }
        private Boolean WorkStationCreation()
        {
            if (Global.User.IsSuperAdmin)
            {
                DialogResult Result = MessageBox.Show(ErrorMsg + " Do you want to create it", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.Yes)
                {
                    FormWorkStationSetup FormWorkStationSetup = new FormWorkStationSetup();
                    FormWorkStationSetup.WorkStationSetupOnLoad = true;
                    FormWorkStationSetup.ShowDialog();
                    if(!ValidateField())
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                MessageBox.Show(ErrorMsg + " Contact Your Admin");
                return false;
            }
            return true;
        }
    }
}
