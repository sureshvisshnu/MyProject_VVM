using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using fa.model.OrderManagement;

namespace fa.views.controls
{
    public partial class FileUploader : UserControl
    {

        public enum AddFileUploadGridColumn
        {
            SNO, NAME, CHOOSE, REMOVE, PATH
        }

        public FileUploader()
        {
            InitializeComponent();
        }
        private void FileUploader_Load(object sender, EventArgs e)
        {
        }
        public void Clear()
        {
            DataGridViewFileUpload.Rows.Clear();
            DataGridViewFileUpload.Rows.Add();
        }
        private IList<PurchaseAttachment> _PurchaseAttachment = null;
        public IList<PurchaseAttachment> PurchaseAttachment
        {
            get
            {
                return ListPurchaseAttachment();
            }
            set
            {
                _PurchaseAttachment = value;
                LoadPurchaseAttachment(_PurchaseAttachment);
            }
        }
        private IList<PurchaseAttachment> ListPurchaseAttachment()
        {
            IList<PurchaseAttachment> PurchaseAttachment = null;
            int Count = DataGridViewFileUpload.Rows.Count;
            if (Count > 1)
            {
                PurchaseAttachment = new List<PurchaseAttachment>();
                for (int i = 0; i < DataGridViewFileUpload.Rows.Count - 1; i++)
                {
                    PurchaseAttachment lPurchaseAttachment = new PurchaseAttachment();
                    lPurchaseAttachment.FileName = DataGridViewFileUpload.Rows[i].Cells[(int)AddFileUploadGridColumn.NAME].Value.ToString();
                    lPurchaseAttachment.Attachment = (byte[])DataGridViewFileUpload.Rows[i].Cells[(int)AddFileUploadGridColumn.PATH].Value;

                    PurchaseAttachment.Add(lPurchaseAttachment);
                }
            }
            return PurchaseAttachment;
        }
        private void LoadPurchaseAttachment(IList<PurchaseAttachment> lPurchaseAttachment)
        {
            if (lPurchaseAttachment != null)
            {
                DataGridViewFileUpload.Rows.Clear();
                DataGridViewFileUpload.Rows.Add(lPurchaseAttachment.Count);
                int i = 0;
                foreach (PurchaseAttachment PurchAttachment in lPurchaseAttachment)
                {
                    DataGridViewFileUpload.Rows[i].Cells[(int)AddFileUploadGridColumn.SNO].Value = i + 1;
                    DataGridViewFileUpload.Rows[i].Cells[(int)AddFileUploadGridColumn.NAME].Value = PurchAttachment.FileName;

                    ButtonToggle(true, i);

                    DataGridViewFileUpload.Rows[i].Cells[(int)AddFileUploadGridColumn.PATH].Value = PurchAttachment.Attachment;
                    i++;
                }
                DataGridViewFileUpload.Rows.Add();
            }
        }
        private void ButtonToggle(bool btnStatus, int cellIndex)
        {
            if (btnStatus)
            {
                DataGridViewFileUpload.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].Value = "\u2B73";
                DataGridViewFileUpload.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].Style.Font = new Font("Verdana", 14, FontStyle.Bold);
                DataGridViewFileUpload.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].ToolTipText = "Download";
            }
            else
            {
                DataGridViewFileUpload.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].Value = "+";
                DataGridViewFileUpload.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].Style.Font = new Font("Verdana", 14, FontStyle.Bold);
                DataGridViewFileUpload.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].ToolTipText = "";
            }
        }
        private void DataGridViewFileUpload_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 2)
                {
                    UploadDownload();
                }
                else if (e.ColumnIndex == 3 && e.RowIndex != DataGridViewFileUpload.Rows.Count - 1)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete " + DataGridViewFileUpload.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.NAME].Value.ToString() + " File ?", "Delete Confirm",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        if (e.RowIndex == 0 && DataGridViewFileUpload.Rows.Count <= 1)
                        {
                            DataGridViewFileUpload.Rows.RemoveAt(e.RowIndex);
                            DataGridViewFileUpload.Rows.Add();
                        }
                        else
                        {
                            DataGridViewFileUpload.Rows.RemoveAt(e.RowIndex);
                            for (int i = 0; i < DataGridViewFileUpload.Rows.Count - 1; i++)
                            {
                                DataGridViewFileUpload.Rows[i].Cells[(int)AddFileUploadGridColumn.SNO].Value = i + 1;
                            }
                        }
                        ReSequence();
                    }
                }
            }
        }

        private void UploadDownload()
        {
            if (DataGridViewFileUpload.CurrentCell.ColumnIndex == 2 && DataGridViewFileUpload.CurrentRow.Cells[(int)AddFileUploadGridColumn.CHOOSE].ToolTipText != "Download")
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.CheckFileExists = true;
                openFileDialog.AddExtension = true;
                openFileDialog.Multiselect = true;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Filter = "Document Files (*.doc;*docs;*pdf;*.xls;*.xlsx;*.txt)|*.txt;*.doc;*docs;*pdf;*.xls;*.xlsx; |Image Files(*.jpeg;*.bmp;*.png;*.jpg)|*.jpeg;*.bmp;*.png;*.jpg|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var FileSize = new FileInfo(openFileDialog.FileName).Length;
                    if (FileSize < 5242880)
                    {
                        var name = Path.GetFileNameWithoutExtension(openFileDialog.FileName) + Path.GetExtension(openFileDialog.FileName);
                        DataGridViewFileUpload.CurrentRow.Cells[(int)AddFileUploadGridColumn.SNO].Value = DataGridViewFileUpload.Rows.Count;
                        DataGridViewFileUpload.CurrentRow.Cells[(int)AddFileUploadGridColumn.NAME].Value = name;
                        DataGridViewFileUpload.CurrentRow.Cells[(int)AddFileUploadGridColumn.PATH].Value = File.ReadAllBytes(openFileDialog.FileName);
                        ButtonToggle(true, DataGridViewFileUpload.CurrentRow.Index);
                        DataGridViewFileUpload.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        DataGridViewFileUpload.Rows.Add();
                    }
                    else
                    {
                        MessageBox.Show("Please Upload less than 5 mb");
                        return;
                    }
                }
            }
            else if (DataGridViewFileUpload.CurrentCell.ColumnIndex == 2 && DataGridViewFileUpload.CurrentRow.Cells[(int)AddFileUploadGridColumn.CHOOSE].ToolTipText == "Download")
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.FileName = DataGridViewFileUpload.CurrentRow.Cells[(int)AddFileUploadGridColumn.NAME].Value.ToString();
                    saveFileDialog.DefaultExt = Path.GetExtension(DataGridViewFileUpload.CurrentRow.Cells[(int)AddFileUploadGridColumn.NAME].Value.ToString());
                    //saveFileDialog.Filter = "Document Files (*.doc;*docs;*pdf;*.xls;*.xlsx;*.txt)|*.txt;*.doc;*docs;*pdf;*.xls;*.xlsx; |Image Files(*.jpeg;*.bmp;*.png;*.jpg)|*.jpeg;*.bmp;*.png;*.jpg|All files (*.*)|*.*";
                    //saveFileDialog.AddExtension = true;
                    if (DialogResult.OK == saveFileDialog.ShowDialog())
                    {
                        byte[] array = (byte[])DataGridViewFileUpload.CurrentRow.Cells[(int)AddFileUploadGridColumn.PATH].Value;
                        File.WriteAllBytes(path: saveFileDialog.FileName, array);
                    }
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (DataGridViewFileUpload.CurrentCell != null)
            {
                if (keyData == (Keys.Tab) && DataGridViewFileUpload.CurrentCell.ColumnIndex == (int)AddFileUploadGridColumn.REMOVE)
                {
                    if (DataGridViewFileUpload.CurrentRow.Index != DataGridViewFileUpload.Rows.Count - 1)
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                    else
                    {
                        SendKeys.Send("{tab}");
                    }
                }
                if (keyData == (Keys.Tab | Keys.Shift) && DataGridViewFileUpload.CurrentCell.ColumnIndex == (int)AddFileUploadGridColumn.CHOOSE)
                {
                    if (DataGridViewFileUpload.CurrentRow.Index != 0)
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                    else
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                }
            }
            try
            {
                if (keyData == (Keys.Enter) && DataGridViewFileUpload.CurrentCell.ColumnIndex == 2)
                {
                    UploadDownload();
                    return true;
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void DataGridViewFileUpload_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //if (DataGridViewFileUpload.CurrentRow.Cells[e.ColumnIndex].ReadOnly )
            //{
            //    this.BeginInvoke(new MethodInvoker(() =>
            //    {
            //        DoFocus(DataGridViewFileUpload, e.RowIndex, 2);
            //    }));
            //}
        }
        private void DoFocus(DataGridView dgv, int RowIndex, int CellIndex)
        {
            dgv.CurrentCell = dgv.Rows[RowIndex].Cells[CellIndex];
            dgv.CurrentCell.Selected = true;
            dgv.BeginEdit(true);
        }

        private void FileUploader_SizeChanged(object sender, EventArgs e)
        {
            SizeChange();
        }

        private void FileUploader_Resize(object sender, EventArgs e)
        {
            SizeChange();
        }
        private void SizeChange()
        {
            DataGridViewFileUpload.Size = new Size(this.Width - 5, this.Height - 24);
            DataGridViewFileUpload.Columns[1].Width = DataGridViewFileUpload.Width - 98;
        }
        private void DataGridViewFileUpload_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewFileUpload.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.SNO].Value = DataGridViewFileUpload.Rows.Count;
        }
        private void ReSequence()
        {
            for (int i = 0; i < DataGridViewFileUpload.Rows.Count; i++)
            {
                DataGridViewFileUpload.Rows[i].Cells[(int)AddFileUploadGridColumn.SNO].Value = i + 1;
            }
        }
    }
}
