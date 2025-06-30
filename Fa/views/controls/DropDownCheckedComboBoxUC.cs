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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
//using Microsoft.VisualBasic;

namespace fa.views.controls
{

    public partial class DropDownCheckedComboBoxUC : UserControl
    {
        private const int T_DisplayListSize = 6;
        private const string SelectNoneText = "(None Selected)";
        private const string SelectAllText = "(All Selected)";
        private const string SelectSomeText = "(Some Selected...)";
        private Form Frm;
        private bool LostFocused;
        private string CodeValue;
        private bool T_MustFill;
        private static string m_ChkItemsString;
        private event DropDownEventHandler DropDown;
        public delegate void DropDownEventHandler();
        public new event TextChangedEventHandler TextChanged;

        public new delegate void TextChangedEventHandler();
        public DropDownCheckedComboBoxUC()
        {
            InitializeComponent();
            InitializeNew();

            List<string> lItemArray = new List<string>();
            var lItemArrays = new[]
            { 
                string.Empty            
            };
            foreach (var lItem in Global.ProductDetailList.Where(x => x.CompanyId == Global.Company.CompanyId))
            {
                lItemArray.Add(lItem.Name);                
            }

            var dt = new DataTable();

            dt.Columns.Add("Item", typeof(string));
            dt.Columns.Add("Checked", typeof(bool));

            foreach (var item in lItemArray)
            { 
                dt.Rows.Add(item, false);                
            }

            dt.AcceptChanges();

            CheckedItemListBox.DataSource = dt.DefaultView;
            CheckedItemListBox.DisplayMember = "Item";
            CheckedItemListBox.ValueMember = "Item";

            CheckedItemListBox.ItemCheck += CheckedItemListBox_ItemCheck;
            CheckedItemListBox.Hide();
            //SetSize();
        }

        private void InitializeNew()
        {
            //string strTemp;
            //strTemp = string.Empty;
            ListSize = T_DisplayListSize;
            T_DroppedDown = false;
            //T_ListText = "";
            //T_MustFill = false;
            //TextBoxItemSearch.Text = strTemp;
            //CheckedItemListBox.Hide();
            //Frm = new Form();
            //{
            //    var withBlock = Frm;
            //    withBlock.ShowInTaskbar = false;
            //    withBlock.FormBorderStyle = FormBorderStyle.None;
            //    withBlock.ControlBox = false;
            //    withBlock.StartPosition = FormStartPosition.Manual;
            //    withBlock.TopMost = true;
            //    withBlock.Location = CheckedItemListBox.Location;
            //    withBlock.Width = CheckedItemListBox.Width;
            //    withBlock.Controls.Add(CheckedItemListBox);
            //}
            SetSize();
        }

        private int ListSize;
        public int DisplayListSize
        {
            get
            {
                return ListSize;
            }
            set
            {
                ListSize = value;
                //SetList();
            }
        }

        private bool T_DroppedDown;
        public bool DroppedDown
        {
            get
            {
                return T_DroppedDown;
            }
        }

        private string T_ListText;
        public string ListText
        {
            get
            {
                return T_ListText;
            }
        }
        private void SetList()
        {
            Form oFrm;
            Rectangle oRect;
            Point oPt;

            if(Frm != null)
            {
                Frm.Height = (ListSize * CheckedItemListBox.ItemHeight) + 3;
                CheckedItemListBox.Height = Frm.Height;
                CheckedItemListBox.Top = 0;
                oFrm = this.ParentForm;
                if(oFrm != null)
                {
                    oPt = this.ParentForm.PointToClient(this.PointToScreen(Point.Empty));
                    oPt.Y = oPt.Y + this.TextBoxItemSearch.Height;
                    oRect = oFrm.RectangleToScreen(oFrm.ClientRectangle);
                    oPt.X = oPt.X + oRect.Left;
                    oPt.Y = oPt.Y + oRect.Top;
                    Frm.Location = oPt;
                }
                Frm.Width = CheckedItemListBox.Width;
            }
        }
        private void SetSize()
        {
            LostFocused = false;
            this.Left = 1;
            TextBoxItemSearch.Width = this.Width;
            ButtonDropDown.Left = TextBoxItemSearch.Width - ButtonDropDown.Width - 2;
            CheckedItemListBox.Width = this.Width;
            CheckedItemListBox.Height = this.Height + TextBoxItemSearch.Height;
            
            //CheckedItemListBox.Height = CheckedItemListBox.Items.Count * CheckedItemListBox.ItemHeight;
            this.Height = TextBoxItemSearch.Height + CheckedItemListBox.Height;

            if (T_DroppedDown)
            {
                CheckedItemListBox.Show();
            }
            else
            {
                CheckedItemListBox.Hide();
            }
            this.Controls.Add(CheckedItemListBox);
            SetList();
        }

        private void ListButtonClick()
        {
            String strTemp;
            strTemp = T_ListText;
            if(T_DroppedDown)
            {
                T_DroppedDown = false;
                TextBoxItemSearch.Text = GetSelectedItems();
                CheckedItemListBox.Hide();
                Frm.Hide();
                TextBoxItemSearch.Focus();
                if(strTemp != T_ListText)
                TextChanged?.Invoke();
            }
            else if(!LostFocused)
            {
                T_DroppedDown = true;
                SetSize();
                Frm.Show();
                CheckedItemListBox.Show();
                CheckedItemListBox.Focus();
                DropDown?.Invoke();
            }
            LostFocused = false;
        }

        private string GetSelectedItems()
        {
            string strLst;            
            bool blnAllSelected = false;
            strLst = "";
            {
                var withBlock = CheckedItemListBox;
                if(withBlock.Items.Count > 0)
                {
                    if(withBlock.CheckedIndices.Count == 0)
                    strLst = SelectNoneText;
                    else if(withBlock.CheckedIndices.Count == withBlock.Items.Count)
                    strLst = SelectAllText;
                    else
                        strLst = withBlock.CheckedIndices.Count + " selected";// SelectSomeText
                }
                else
                    strLst = SelectNoneText;
            }
            return strLst;
        }
        private void LoadItemWithFilter()
        {
            CheckedItemListBox.Items.Clear();
            var dt = new DataTable();

        dt.Columns.Add("Item", typeof(string));
        dt.Columns.Add("Checked", typeof(bool));

        foreach (var lItem in Global.ProductDetailList.Where(x => x.CompanyId == Global.Company.CompanyId))
        {
            dt.AcceptChanges();

            CheckedItemListBox.DataSource = dt.DefaultView;
            CheckedItemListBox.DisplayMember = "Item";
            CheckedItemListBox.ValueMember = "Item";

            CheckedItemListBox.ItemCheck += CheckedItemListBox_ItemCheck;

            //CheckedItemListBox.Items.Add(lItem, lItem.Id != 0 ? true : false);
        }

        }
        private void CheckedItemListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var dv = CheckedItemListBox.DataSource as DataView;
            var drv = dv[e.Index];
            drv["Checked"] = e.NewValue == CheckState.Checked ? true : false;
        }

        private void TextBoxItemSearch_TextChanged(object sender, EventArgs e)
        {
            var dv = CheckedItemListBox.DataSource as DataView;
            var filter = TextBoxItemSearch.Text.Trim().Length > 0
                ? $"Item LIKE '*{TextBoxItemSearch.Text}*'"
                : null;

            dv.RowFilter = filter;

            for (var i = 0; i < CheckedItemListBox.Items.Count; i++)
            {
                var drv = CheckedItemListBox.Items[i] as DataRowView;
                var chk = Convert.ToBoolean(drv["Checked"]);
                CheckedItemListBox.SetItemChecked(i, chk);
            }
        }

        private void ButtonDropDown_MouseHover(object sender, EventArgs e)
        {
            LostFocused = false;
        }

        private void ButtonDropDown_MouseUp(object sender, MouseEventArgs e)
        {
            LostFocused = false;
        }

        private void CheckedItemListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ButtonDropDown_MouseDown(object sender, MouseEventArgs e)
        {
            if(T_DroppedDown)
            {
                T_DroppedDown = false;
                //SetSize();
                CheckedItemListBox.Show();
            }
            else
            {
                T_DroppedDown = true;
                CheckedItemListBox.Hide();
            }
            
            //ListButtonClick();
        }
    }
}
