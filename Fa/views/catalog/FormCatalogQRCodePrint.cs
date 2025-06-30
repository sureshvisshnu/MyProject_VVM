using fa.api.catalog;
using fa.libraries.utils;
using fa.libraries.Validation;
using fa.model.Catalog;
using fa.views.utils;
using fa.views.utils.QRCodes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.views.catalog
{
    public partial class FormCatalogQRCodePrint : Form
    {
        public static string ChooseQRCodeSizeErrorMsg = "Please choose QR Code size";
        public static string ChooseLabelSizeErrorMsg = "Please choose label size";
        public static string EnterQtyErrorMsg = "Please Enter Quantity";
        public static string EnterValidQtyErrorMsg = "Please Enter valid Quantity";
        public long ProductId;
        public string BatchNo;
        
        public FormCatalogQRCodePrint()
        {
            InitializeComponent();
        }
        private void FormCatalogQRCodePrint_Load(object sender, EventArgs e)
        {
            ResetForm();
            TextBoxPrintQuantity.Select();
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (validateFormPrint())
            {
                if (CatalogProductManager.Instance.GetProductInfoById(ProductId) == null)
                {
                    MessageBox.Show("Somthing went wrong, the selected product is not valid.");
                    return;
                }
                Cursor.Current = Cursors.WaitCursor;
                if (!YesNoRadioPaperSize.Checked)
                {
                    SavePrintQRCode SavePrintQRCode = new SavePrintQRCode();
                    SavePrintQRCode.GenerateQRcodeA4(BatchNo,ProductId,ComboBoxQRCodeSize.SelectedIndex.ToString(), "A4SheetQRCode", "pdf", true, TextBoxStartLocation.Text, int.Parse(TextBoxPrintQuantity.Text), ComboBoxDefaultPrinter.Text);
                }
                else
                {
                    SavePrintQRCode SavePrintQRCode = new SavePrintQRCode();
                    if (ComboBoxLabelSize.Text == "35 mm * 25 mm")
                    {
                        SavePrintQRCode.GenerateQRcodeLabel(ProductId, BatchNo, LabelSize.THREE, int.Parse(TextBoxPrintQuantity.Text), ComboBoxDefaultPrinter.Text);
                    }
                    else if (ComboBoxLabelSize.Text == "50 mm * 25 mm")
                    {
                        SavePrintQRCode.GenerateQRcodeLabel(ProductId, BatchNo, LabelSize.TWO, int.Parse(TextBoxPrintQuantity.Text), ComboBoxDefaultPrinter.Text);
                    }
                    else if (ComboBoxLabelSize.Text == "100 mm* 23 mm")
                    {
                        SavePrintQRCode.GenerateQRcodeLabel(ProductId, BatchNo, LabelSize.ONE, int.Parse(TextBoxPrintQuantity.Text), ComboBoxDefaultPrinter.Text);
                    }
                }
                Cursor.Current = Cursors.Default;
            }
        }
        private Boolean validateFormPrint()
        {
            PrintErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxPrintQuantity.Text.Trim()))
            {
                PrintErrorMsg.Text = EnterQtyErrorMsg;
                TextBoxPrintQuantity.Select();
                return false;
            }
            if (int.Parse(TextBoxPrintQuantity.Text) < 1)
            {
                PrintErrorMsg.Text = EnterValidQtyErrorMsg;
                TextBoxPrintQuantity.Select();
                return false;
            }
            if (!YesNoRadioPaperSize.Checked && ComboBoxQRCodeSize.SelectedIndex < 0)
            {
                ComboBoxQRCodeSize.Select();
                PrintErrorMsg.Text = ChooseQRCodeSizeErrorMsg;
                return false;
            }
            if (YesNoRadioPaperSize.Checked && ComboBoxLabelSize.SelectedIndex < 0)
            {
                ComboBoxLabelSize.Select();
                PrintErrorMsg.Text = ChooseLabelSizeErrorMsg;
                return false;
            }
            if (ComboBoxDefaultPrinter.SelectedIndex < 0)
            {
                ComboBoxDefaultPrinter.Select();
                PrintErrorMsg.Text = "Please Choose Printer";
                return false;
            }
            return true;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F9))
            {
                BtnPrint.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                return false;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void ResetForm()
        {
            TextBoxStartLocation.clear();
            TextBoxPrintQuantity.Text = "1";
            YesNoRadioPaperSize.Checked = false;
            Product ProductFromDB = CatalogProductManager.Instance.GetProductInfoById(ProductId);
            BatchQRCode.Text = (ProductFromDB.MaterialId + "#" + BatchNo);
            ComboBoxDefaultPrinter.Items.Clear();
            ComboBoxDefaultPrinter.Items.AddRange(ComboUtils.GetAvailablePrinter().ToArray<string>());
            if (ComboBoxDefaultPrinter.Items != null && ComboBoxDefaultPrinter.Items.Count > 0)
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Ab2App");
                if (key.GetValue("DefaultPrinter") != null && !string.IsNullOrEmpty(key.GetValue("DefaultPrinter").ToString()))
                {
                    ComboBoxDefaultPrinter.SelectedIndex = ComboBoxDefaultPrinter.FindStringExact(key.GetValue("DefaultPrinter").ToString());
                }
            }
        }

        private void TextBoxPrintQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Instance.Keypress_Num(sender, e);
        }
        Point PLabel=new Point(17, 180);
        Point PCombo=new Point(17, 197);
        private void YesNoRadioPaperSize_Load(object sender, EventArgs e)
        {
            
            if (YesNoRadioPaperSize.Checked)
            {
                LabelLabelSize.Location = LabelQRCodeSize.Location;
                ComboBoxLabelSize.Location = ComboBoxQRCodeSize.Location;
                LabelChoosePrinter.Location = LabelStartLocation.Location;
                ComboBoxDefaultPrinter.Location = TextBoxStartLocation.Location;
                LabelLabelSize.Visible = true;
                ComboBoxLabelSize.Visible = true;
                ComboBoxLabelSize.BringToFront();
                ComboBoxLabelSize.SelectedIndex = -1;
                LabelStartLocation.Visible = false;
                TextBoxStartLocation.Visible = false;
                LabelQRCodeSize.Visible = false;
                ComboBoxQRCodeSize.Visible = false;
            }
            else
            {
                LabelChoosePrinter.Location = PLabel;
                ComboBoxDefaultPrinter.Location = PCombo;
                LabelLabelSize.Visible = false;
                ComboBoxLabelSize.Visible = false;
                LabelStartLocation.Visible = true;
                TextBoxStartLocation.Visible = true;
                LabelQRCodeSize.Visible = true;
                ComboBoxQRCodeSize.Visible = true;
            }
        }

        private void ComboBoxQRCodeSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBoxStartLocation.clear();
            if (ComboBoxQRCodeSize.Text=="1 * 1")
            {
                TextBoxStartLocation.NoOfRow = 10;
                TextBoxStartLocation.NoOfColoumns = 8;
            }
            else if (ComboBoxQRCodeSize.Text == "1.5 * 1.5")
            {
                TextBoxStartLocation.NoOfRow = 6;
                TextBoxStartLocation.NoOfColoumns = 4;
            }
            else if (ComboBoxQRCodeSize.Text == "2 * 2")
            {
                TextBoxStartLocation.NoOfRow = 5;
                TextBoxStartLocation.NoOfColoumns = 4;
            }
            
        }

        private void TextBoxStartLocation_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnPrint.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxQRCodeSize.Select();
            }
        }
    }
}
