using fa.api.Accounting;
using fa.api.Log;
using fa.model.Common;
using Fa.api.Accounting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.views.sales
{
    public enum AdditinalDetailGridColumn
    {
        DETAIL, HDETAIL, DESC, REMOVE
    }
    public partial class FormAdditionalDetails : FormBase
    {
        FormBase parent = null;
        public FormAdditionalDetails(object sender)
        {
            parent = (FormBase)sender;            
            InitializeComponent();
        }

        private void FormAdditionalDetails_Load(object sender, EventArgs e)
        {
            Reset();
            GridViewAdditionalDetails.BeginInvoke(new MethodInvoker(delegate ()
            {
                GridViewAdditionalDetails.CurrentCell = GridViewAdditionalDetails[2, 0];
                GridViewAdditionalDetails.CurrentCell = GridViewAdditionalDetails[0, 0];
            }));
        }
        private void Reset()
        {

            GridViewAdditionalDetails.Rows.Clear();
            if (this.AllAdditionalDetails != null && this.AllAdditionalDetails.Count > 0)
            {
                GridViewAdditionalDetails.Rows.Add(AllAdditionalDetails.Count);
                int i = 0;
                foreach (AdditionalDetail Detail in this.AllAdditionalDetails)
                {

                    List<AdditionalDetail> AdditionalDetail = new List<AdditionalDetail>();
                    AdditionalDetail EAdditionalDetail = new AdditionalDetail();
                    AdditionalDetail.Add(EAdditionalDetail);
                    AdditionalDetail.AddRange(AdditionalDetailsManager.Instance.GetAllUniqueDetail(Global.Company.CompanyId));

                    foreach(AdditionalDetail lDetail in AllAdditionalDetails)
                    {
                        string xx = GridViewAdditionalDetails.Rows[i].Cells[(int)AdditinalDetailGridColumn.HDETAIL].Value != null ? GridViewAdditionalDetails.Rows[i].Cells[(int)AdditinalDetailGridColumn.HDETAIL].Value.ToString() : string.Empty;
                        if (!String.IsNullOrEmpty(lDetail.Detail) && AdditionalDetail.FirstOrDefault(x=>x.Detail== lDetail.Detail)==null)
                        {
                            AdditionalDetail lAdditionalDetail = new AdditionalDetail();
                            lAdditionalDetail.CompanyId = Global.Company.CompanyId;
                            if (Global.CostCenter != null) { lAdditionalDetail.CostCenterId = Global.CostCenter.CostCenterId; }
                            lAdditionalDetail.Detail = lDetail.Detail;
                            lAdditionalDetail.Description = "";
                            AdditionalDetail.Add(lAdditionalDetail);
                        }
                    }
                    if (AdditionalDetail != null)
                    {
                        (GridViewAdditionalDetails.Rows[i].Cells[(int)AdditinalDetailGridColumn.DETAIL] as DataGridViewComboBoxCell).DataSource = null;
                        (GridViewAdditionalDetails.Rows[i].Cells[(int)AdditinalDetailGridColumn.DETAIL] as DataGridViewComboBoxCell).DataSource = AdditionalDetail;
                        (GridViewAdditionalDetails.Rows[i].Cells[(int)AdditinalDetailGridColumn.DETAIL] as DataGridViewComboBoxCell).ValueMember = "Detail";
                        (GridViewAdditionalDetails.Rows[i].Cells[(int)AdditinalDetailGridColumn.DETAIL] as DataGridViewComboBoxCell).DisplayMember = "Detail";
                        (GridViewAdditionalDetails.Rows[i].Cells[(int)AdditinalDetailGridColumn.DETAIL] as DataGridViewComboBoxCell).AutoComplete = true;
                    }
                    GridViewAdditionalDetails.Rows[i].Cells[(int)AdditinalDetailGridColumn.DETAIL].Value = Detail.Detail;
                    GridViewAdditionalDetails.Rows[i].Cells[(int)AdditinalDetailGridColumn.HDETAIL].Value = Detail.Detail;
                    GridViewAdditionalDetails.Rows[i].Cells[(int)AdditinalDetailGridColumn.DESC].Value = Detail.Description;
                    i++;
                }
            }

            GridViewAdditionalDetails.Select();
            GridViewAdditionalDetails.CurrentCell = GridViewAdditionalDetails[(int)AdditinalDetailGridColumn.DETAIL, 0];
            GridViewAdditionalDetails.BeginEdit(true);
        }


        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                IList<AdditionalDetail> AdditionalDetail = new List<AdditionalDetail>();
                for (int i = 0; i < (GridViewAdditionalDetails.Rows.Count-1); i++)
                {
                    AdditionalDetail lAdditionalDetail = new AdditionalDetail();
                    lAdditionalDetail.CompanyId = Global.Company.CompanyId;
                    if (Global.CostCenter != null) { lAdditionalDetail.CostCenterId = Global.CostCenter.CostCenterId; }
                    lAdditionalDetail.Detail = GridViewAdditionalDetails.Rows[i].Cells[(int)AdditinalDetailGridColumn.DETAIL].Value.ToString();
                    lAdditionalDetail.Description = GridViewAdditionalDetails.Rows[i].Cells[(int)AdditinalDetailGridColumn.DESC].Value.ToString();
                    AdditionalDetail.Add(lAdditionalDetail);
                }

                parent.AllAdditionalDetails = AdditionalDetail;                
                ErrorMsg.Text = "Saved Success..";
            }
        }
        private bool Validation()
        {
            ErrorMsg.Text = string.Empty;
            for (int i=0;i<GridViewAdditionalDetails.Rows.Count-1;i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (j == 1) { continue; }
                    if (GridViewAdditionalDetails.Rows[i].Cells[j].Value == null || string.IsNullOrEmpty(GridViewAdditionalDetails.Rows[i].Cells[j].Value.ToString()))
                    {
                        GridViewAdditionalDetails.Select();
                        GridViewAdditionalDetails.CurrentCell = GridViewAdditionalDetails[j, i];
                        GridViewAdditionalDetails.BeginEdit(true);
                        ErrorMsg.Text = "Please Enter "+ GridViewAdditionalDetails.Columns[j].HeaderText;
                        return false;
                    }
                }
            }

            return true;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSave.PerformClick();
            }            
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                return true;
            }
            try
            {
                if(GridViewAdditionalDetails.CurrentCell!=null)
                {
                    if (keyData == (Keys.Tab) && GridViewAdditionalDetails.CurrentCell.ColumnIndex==2)
                    {
                        SendKeys.Send("{tab}");                         
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewAdditionalDetails.CurrentCell.ColumnIndex == 0 && GridViewAdditionalDetails.CurrentCell.RowIndex!=0)
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

        private void BtnSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {          
            if ((e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)||(e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab))
            {
                e.IsInputKey = true;
                GridViewAdditionalDetails.Select();
                GridViewAdditionalDetails.CurrentCell = GridViewAdditionalDetails[(int)AdditinalDetailGridColumn.DETAIL, 0];
                GridViewAdditionalDetails.BeginEdit(true);
            }
        }
        private void BtnCancel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if ((e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab))
            {
                e.IsInputKey = true;
                GridViewAdditionalDetails.Select();
                GridViewAdditionalDetails.CurrentCell = GridViewAdditionalDetails[(int)AdditinalDetailGridColumn.DETAIL,0];
                GridViewAdditionalDetails.BeginEdit(true);
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnSave.Select();
            }
        }
        private void GridViewAdditionalDetails_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {     
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                if (GridViewAdditionalDetails.CurrentCell.Value == null)
                {
                    ((ComboBox)e.Control).SelectedIndex = -1;
                }
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                ((ComboBox)e.Control).FormattingEnabled = true;               
                e.Control.KeyPress += new KeyPressEventHandler(GridViewAdditionalDetails_KeyPress1);

                ((ComboBox)e.Control).TextChanged -= AdditonalDetailComboTextChanged;
                ((ComboBox)e.Control).TextChanged += AdditonalDetailComboTextChanged;
            }           
        }

        private void AdditonalDetailComboTextChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == -1 /*&& !string.IsNullOrEmpty(((ComboBox)sender).Text)*/)
            {
                try
                {
                        ((ComboBox)GridViewAdditionalDetails.EditingControl).DroppedDown = false;
                        if (GridViewAdditionalDetails.CurrentCell != null && GridViewAdditionalDetails.CurrentCell.ColumnIndex == 0)
                        {
                            var hh = GridViewAdditionalDetails.Rows[GridViewAdditionalDetails.CurrentCell.RowIndex].Cells[(int)AdditinalDetailGridColumn.HDETAIL].Value = /*((temp != 0 ?*/ ((ComboBox)GridViewAdditionalDetails.EditingControl).Text /*: "")*//* + ((e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Delete)) ? e.KeyChar.ToString() : string.Empty)*//*)*/;

                            if (temp == 0 && Index == GridViewAdditionalDetails.Rows.Count - 1)
                            {
                                //CurRow = Index;
                            //((ComboBox)sender).SelectedIndex = 0;
                            GridViewAdditionalDetails.CommitEdit(DataGridViewDataErrorContexts.Commit);
                            GridViewAdditionalDetails.EndEdit();
                            GridViewAdditionalDetails.Rows.Add();


                            //GridViewAdditionalDetails.BeginInvoke(new MethodInvoker(delegate ()
                            //    {
                            //        GridViewAdditionalDetails.CurrentCell = GridViewAdditionalDetails[0, CurRow];
                            //    }));
                            temp++;
                            //Index = CurRow;
                            GridViewAdditionalDetails.BeginInvoke(new MethodInvoker(delegate ()
                            {
                                GridViewAdditionalDetails.CurrentCell = GridViewAdditionalDetails[2, CurRow];
                                GridViewAdditionalDetails.CurrentCell = GridViewAdditionalDetails[0, CurRow];
                                GridViewAdditionalDetails.Rows[CurRow].Cells[(int)AdditinalDetailGridColumn.DETAIL].Value = hh;
                                GridViewAdditionalDetails.CommitEdit(DataGridViewDataErrorContexts.Commit);

                            }));
                            GridViewAdditionalDetails.Rows[CurRow].Cells[(int)AdditinalDetailGridColumn.DETAIL].Value = hh;
                            //((ComboBox)sender).Text = hh;
                            GridViewAdditionalDetails.CommitEdit(DataGridViewDataErrorContexts.Commit);
                            
                        }
                        temp++;
                        }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                }
            }
            
        }


        int temp=0;
        int Index;
        int CurRow;
        private void GridViewAdditionalDetails_KeyPress1(object sender, KeyPressEventArgs e)
        {
            try
            {

                //    if (e.KeyChar != Convert.ToChar(Keys.Tab) /*&& e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Delete)*/)
                //    {
                //        ((ComboBox)GridViewAdditionalDetails.EditingControl).DroppedDown = false;
                //        if (GridViewAdditionalDetails.CurrentCell != null && GridViewAdditionalDetails.CurrentCell.ColumnIndex == 0)
                //        {
                //            var hh = GridViewAdditionalDetails.Rows[GridViewAdditionalDetails.CurrentCell.RowIndex].Cells[(int)AdditinalDetailGridColumn.HDETAIL].Value = ((temp != 0 ? ((ComboBox)GridViewAdditionalDetails.EditingControl).Text : "") + ((e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Delete))? e.KeyChar.ToString():string.Empty));

            if (temp == 0 && Index == GridViewAdditionalDetails.Rows.Count - 1)
            {
                    CurRow = Index;
                    //                GridViewAdditionalDetails.Rows.Add();

                    //                GridViewAdditionalDetails.BeginInvoke(new MethodInvoker(delegate ()
                    //                {
                    //                    GridViewAdditionalDetails.CurrentCell = GridViewAdditionalDetails[0, CurRow];
                    //                }));
                    //                temp++;
                    //                Index = CurRow;
                    //                GridViewAdditionalDetails.BeginInvoke(new MethodInvoker(delegate ()
                    //                {
                    //                    GridViewAdditionalDetails.CurrentCell = GridViewAdditionalDetails[2, CurRow];
                    //                    GridViewAdditionalDetails.CurrentCell = GridViewAdditionalDetails[0, CurRow];
                    //                    GridViewAdditionalDetails.CurrentRow.Cells[(int)AdditinalDetailGridColumn.DETAIL].Value = hh;
                    //                    GridViewAdditionalDetails.CommitEdit(DataGridViewDataErrorContexts.Commit);

                    //                }));
                    //            }
                    //            temp++;
                    //        }
                }
            }
                catch (Exception ex)
            {
                Logger.LogError(ex);
            }

        }

        private void GridViewAdditionalDetails_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                List<AdditionalDetail> AdditionalDetail = new List<AdditionalDetail>();
                AdditionalDetail EAdditionalDetail = new AdditionalDetail();
                AdditionalDetail.Add(EAdditionalDetail);
                AdditionalDetail.AddRange(AdditionalDetailsManager.Instance.GetAllUniqueDetail(Global.Company.CompanyId));

                for (int i = 0; i < GridViewAdditionalDetails.Rows.Count - 1; i++)
                {
                    string xx = GridViewAdditionalDetails.Rows[i].Cells[(int)AdditinalDetailGridColumn.HDETAIL].Value != null ? GridViewAdditionalDetails.Rows[i].Cells[(int)AdditinalDetailGridColumn.HDETAIL].Value.ToString() : string.Empty;
                    if (!String.IsNullOrEmpty(xx))
                    {
                        AdditionalDetail lAdditionalDetail = new AdditionalDetail();
                        lAdditionalDetail.CompanyId = Global.Company.CompanyId;
                        if (Global.CostCenter != null) { lAdditionalDetail.CostCenterId = Global.CostCenter.CostCenterId; }
                        lAdditionalDetail.Detail = GridViewAdditionalDetails.Rows[i].Cells[(int)AdditinalDetailGridColumn.HDETAIL].Value.ToString();
                        lAdditionalDetail.Description = "";
                        AdditionalDetail.Add(lAdditionalDetail);
                    }
                }
                if (AdditionalDetail != null)
                {
                    (GridViewAdditionalDetails.Rows[e.RowIndex].Cells[(int)AdditinalDetailGridColumn.DETAIL] as DataGridViewComboBoxCell).DataSource = null;
                    (GridViewAdditionalDetails.Rows[e.RowIndex].Cells[(int)AdditinalDetailGridColumn.DETAIL] as DataGridViewComboBoxCell).DataSource = AdditionalDetail;
                    (GridViewAdditionalDetails.Rows[e.RowIndex].Cells[(int)AdditinalDetailGridColumn.DETAIL] as DataGridViewComboBoxCell).ValueMember = "Detail";
                    (GridViewAdditionalDetails.Rows[e.RowIndex].Cells[(int)AdditinalDetailGridColumn.DETAIL] as DataGridViewComboBoxCell).DisplayMember = "Detail";
                    (GridViewAdditionalDetails.Rows[e.RowIndex].Cells[(int)AdditinalDetailGridColumn.DETAIL] as DataGridViewComboBoxCell).AutoComplete = true;
                }
            }
            catch(Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        private void GridViewAdditionalDetails_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if(GridViewAdditionalDetails.EditingControl!=null && ((ComboBox)GridViewAdditionalDetails.EditingControl).SelectedIndex==-1)
                {
                    try
                    {
                        List<AdditionalDetail> AdditionalDetail = new List<AdditionalDetail>();
                        AdditionalDetail EAdditionalDetail = new AdditionalDetail();
                        AdditionalDetail.Add(EAdditionalDetail);
                        AdditionalDetail.AddRange(AdditionalDetailsManager.Instance.GetAllUniqueDetail(Global.Company.CompanyId));

                        for (int i = 0; i < GridViewAdditionalDetails.Rows.Count - 1; i++)
                        {
                            string xx = GridViewAdditionalDetails.Rows[i].Cells[(int)AdditinalDetailGridColumn.HDETAIL].Value != null ? GridViewAdditionalDetails.Rows[i].Cells[(int)AdditinalDetailGridColumn.HDETAIL].Value.ToString() : string.Empty;
                            if (!String.IsNullOrEmpty(xx))
                            {
                                AdditionalDetail lAdditionalDetail = new AdditionalDetail();
                                lAdditionalDetail.CompanyId = Global.Company.CompanyId;
                                if (Global.CostCenter != null) { lAdditionalDetail.CostCenterId = Global.CostCenter.CostCenterId; }
                                lAdditionalDetail.Detail = GridViewAdditionalDetails.Rows[i].Cells[(int)AdditinalDetailGridColumn.HDETAIL].Value.ToString();
                                lAdditionalDetail.Description = "";
                                AdditionalDetail.Add(lAdditionalDetail);
                            }
                        }
                        if (AdditionalDetail != null)
                        {
                            for (int i = 0; i < GridViewAdditionalDetails.Rows.Count - 1; i++)
                            {
                                var tt = GridViewAdditionalDetails.Rows[i].Cells[(int)AdditinalDetailGridColumn.DETAIL].Value;
                                (GridViewAdditionalDetails.Rows[i].Cells[(int)AdditinalDetailGridColumn.DETAIL] as DataGridViewComboBoxCell).DataSource = AdditionalDetail;
                                GridViewAdditionalDetails.Rows[i].Cells[(int)AdditinalDetailGridColumn.DETAIL].Value = GridViewAdditionalDetails.CurrentCell.RowIndex == i ? GridViewAdditionalDetails.CurrentRow.Cells[(int)AdditinalDetailGridColumn.HDETAIL].Value : tt;
                                GridViewAdditionalDetails.CommitEdit(DataGridViewDataErrorContexts.Commit);

                            }
                        }                        
                        GridViewAdditionalDetails.CurrentCell.Value = GridViewAdditionalDetails.CurrentRow.Cells[(int)AdditinalDetailGridColumn.HDETAIL].EditedFormattedValue.ToString();
                    }
                    catch(Exception ex)
                    {
                        Logger.LogError(ex);
                    }
                }
                else
                {
                    GridViewAdditionalDetails.CurrentCell.Value = GridViewAdditionalDetails.CurrentCell.EditedFormattedValue.ToString();
                }
            }        
        }
        
        private void GridViewAdditionalDetails_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if(e.ColumnIndex==0)
            {
                temp = 0;
                Index = e.RowIndex;
            }
        }

        private void GridViewAdditionalDetails_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void GridViewAdditionalDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == (int)AdditinalDetailGridColumn.REMOVE && GridViewAdditionalDetails.Rows.Count - 1 != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete row " + (e.RowIndex+1) + "?", "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        GridViewAdditionalDetails.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewAdditionalDetails.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
        }
    }
}
