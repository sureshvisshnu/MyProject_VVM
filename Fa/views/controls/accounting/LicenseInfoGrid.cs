using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using fa.model.Accounting.Masters;

namespace fa.views.controls.accounting
{
    public enum LicenseInfoColumnName
    {
        TYPE, VALUE,ID
    }
    public partial class LicenseInfoGrid : UserControl
    {
        public LicenseInfoGrid()
        {
            InitializeComponent();
        }
        public void Clear()
        {
            DataGridViewLicenseType.Rows.Clear();
        }
        private IList<LicenseInfo> _LicenseInfos = null;
        public IList<LicenseInfo> LicenseInfos
        {
            get
            {
                return ListLicenseInfos();
            }
            set
            {
                _LicenseInfos = value;
                LoadLicenseInfos(_LicenseInfos);
            }
        }
        private void LoadLicenseInfos(IList<LicenseInfo> lLicenseInfos)
        {
            if (lLicenseInfos != null && lLicenseInfos.Count > 0)
            {
                #pragma warning disable 0219
                int i = 0;
                #pragma warning restore 0219
                foreach (LicenseInfo LicenseInfo in lLicenseInfos)
                {
                    /*
                    if (DataGridViewLicenseType.Rows[i].Cells[(int)LicenseInfoColumnName.TYPE].Value.ToString() == LicenseInfo.LicenseType.ToString())
                    {
                        DataGridViewLicenseType.Rows[i].Cells[(int)LicenseInfoColumnName.VALUE].Value = LicenseInfo.LicenseValue;
                        DataGridViewLicenseType.Rows[i].Cells[(int)LicenseInfoColumnName.ID].Value = LicenseInfo.Id;
                    }
                    i++;*/
                }
            }
        }
        private IList<LicenseInfo> ListLicenseInfos()
        {
            IList<LicenseInfo> LicenseInfo = null;
            int Count = DataGridViewLicenseType.Rows.Count;
            if (Count > 1)
            {
                LicenseInfo = new List<LicenseInfo>();
                for (int i = 0; i < DataGridViewLicenseType.Rows.Count; i++)
                {
                    LicenseInfo lLicenseInfos = new LicenseInfo();
                    /*
                    lLicenseInfos.LicenseType = (LicenseType)Enum.Parse(typeof(LicenseType), DataGridViewLicenseType.Rows[i].Cells[(int)LicenseInfoColumnName.TYPE].Value.ToString(), true); 
                    lLicenseInfos.LicenseValue = (string)DataGridViewLicenseType.Rows[i].Cells[(int)LicenseInfoColumnName.VALUE].Value;*/
                    lLicenseInfos.Id =(DataGridViewLicenseType.Rows[i].Cells[(int)LicenseInfoColumnName.ID].Value!=null)?(long)DataGridViewLicenseType.Rows[i].Cells[(int)LicenseInfoColumnName.ID].Value:0L;

                    LicenseInfo.Add(lLicenseInfos);
                }
            }
            return LicenseInfo;
        }
        
        public void LoadGrid()
        {
            Clear();
            /*
            var values = from Enum e in Enum.GetValues(typeof(LicenseType)) select new { ID = e, Name = e.ToString() };
            if (values != null)
            {
                DataGridViewLicenseType.Rows.Add(values.ToList().Count);
                int i = 0;
                foreach (var License in values)
                {
                    DataGridViewLicenseType.Rows[i].Cells[0].Value = License.Name;
                    i++;
                }
            }*/

        }

        private void DataGridViewLicenseType_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void DataGridViewLicenseType_SizeChanged(object sender, EventArgs e)
        {
            SizeChange();
        }

        private void DataGridViewLicenseType_Resize(object sender, EventArgs e)
        {
            SizeChange();
        }
        private void SizeChange()
        {
            DataGridViewLicenseType.Size = new Size(this.Width - 5, this.Height - 24);
            DataGridViewLicenseType.Columns[1].Width = DataGridViewLicenseType.Width - 222;
        }
        private void LicenseInfoGrid_Resize(object sender, EventArgs e)
        {
            SizeChange();
        }
        private void DataGridViewLicenseType_Enter(object sender, EventArgs e)
        {
            DataGridViewLicenseType.BeginInvoke(new MethodInvoker(delegate ()
            {
                DataGridViewLicenseType.CurrentCell = DataGridViewLicenseType[(int)LicenseInfoColumnName.VALUE, 0];
            }));
        }

        private void DataGridViewLicenseType_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab && DataGridViewLicenseType.CurrentCell == DataGridViewLicenseType[0, 0])
            {
                this.OnPreviewKeyDown(e);
            }
           
        }
        private void DataGridViewLicenseType_Leave(object sender, EventArgs e)
        {
            if (DataGridViewLicenseType.CurrentCell == DataGridViewLicenseType[1, DataGridViewLicenseType.RowCount - 1])
            {
                PreviewKeyDownEventArgs PreviewKeyDownEventArgs = new PreviewKeyDownEventArgs(Keys.Tab);
                this.OnPreviewKeyDown(PreviewKeyDownEventArgs);
            }
        }
       
        private void DataGridViewLicenseType_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex>-1)
            {
                DataGridViewLicenseType.CurrentRow.Cells[0].ReadOnly = true;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (DataGridViewLicenseType.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab) && DataGridViewLicenseType.CurrentCell.ColumnIndex == (int)LicenseInfoColumnName.VALUE)
                    {
                        if (DataGridViewLicenseType.CurrentCell.RowIndex != DataGridViewLicenseType.Rows.Count - 1)
                        {
                            SendKeys.Send("{tab}");
                        }
                        
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && DataGridViewLicenseType.CurrentCell.ColumnIndex == (int)LicenseInfoColumnName.VALUE)
                    {
                        SendKeys.Send("{tab}");
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
