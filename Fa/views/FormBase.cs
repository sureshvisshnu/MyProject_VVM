using fa.model.Common;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace fa.views
{
    public partial class FormBase : Form
    {
        public IList<AdditionalDetail> AllAdditionalDetails;
        public string[] excludedObjects;
        protected bool formIsDirty = false;
        public FormBase()
        {
            InitializeComponent();
            excludedObjects = new string[] { };
        }

        private void FormBase_Load(object sender, EventArgs e)
        {
            AddOnChangeHandlerToInputControls(this);
        }

        private void AddOnChangeHandlerToInputControls(Control ctrl)
        {
            foreach (Control subctrl in ctrl.Controls)
            {
                var results = Array.Find(excludedObjects, s => s.Equals(subctrl.Name));
                if (results != null)
                {
                    continue;
                }

                if (subctrl is TextBox)
                    ((TextBox)subctrl).TextChanged +=
                        new EventHandler(InputControls_OnChange!);
                else if (subctrl is MaskedTextBox)
                    ((MaskedTextBox)subctrl).TextChanged +=
                        new EventHandler(InputControls_OnChange!);
                else if (subctrl is CheckBox)
                    ((CheckBox)subctrl).CheckedChanged +=
                        new EventHandler(InputControls_OnChange!);
                else if (subctrl is RadioButton)
                    ((RadioButton)subctrl).CheckedChanged +=
                        new EventHandler(InputControls_OnChange!);
                else if (subctrl is ListBox)
                    ((ListBox)subctrl).SelectedIndexChanged +=
                        new EventHandler(InputControls_OnChange!);
                else if (subctrl is ComboBox)
                    ((ComboBox)subctrl).SelectedIndexChanged +=
                        new EventHandler(InputControls_OnChange!);
                else if (subctrl is DataGridView)
                {
                    ((DataGridView)subctrl).CellValueChanged += new DataGridViewCellEventHandler(InputControls_OnChange!);
                }
                else
                {
                    if (subctrl.Controls.Count > 0)
                        this.AddOnChangeHandlerToInputControls(subctrl);
                }
            }
        }

        public virtual void InputControls_OnChange(object sender, EventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.Name != null && checkBox.Name == "CheckBoxLoadAllNotes")
            {
                return;
            }
            this.formIsDirty = true;            
        }
        
        protected virtual void AccountIdTransportReload(object sender, EventArgs e)
        {

        }
        protected virtual void ProductIdTransportReload(object sender, EventArgs e)
        {

        }
        protected virtual void ProductBatchIdTransportReload(object sender, EventArgs e)
        {

        }

        public System.Windows.Forms.Timer Timer; 
        public void DelayTextChanged(int DELAY)
        {
            if (Timer == null)
            {
                Timer = new System.Windows.Forms.Timer();
                Timer.Interval = DELAY;
                Timer.Tick += new EventHandler(this.handleTypingTimerTimeout!);
            }
            Timer.Stop();
            Timer.Start();
        }
        private void handleTypingTimerTimeout(object sender, EventArgs e)
        {
            var timer = sender as System.Windows.Forms.Timer; 
            if (timer == null)
            {
                return;
            }
            LoadAfterDelay();
            timer.Stop();
        }       
        protected virtual void LoadAfterDelay()
        {
        }
    }
}
