using fa.api.catalog;
using fa.libraries.utils;
using fa.libraries.Validation;
using fa.views.utils.Catalog;
using iTextSharp.text.pdf;
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
    public partial class FormCatalogTokenPrint : Form
    {
        public static string EnterQtyErrorMsg = "Please Enter Quantity";
        public long ProductId;
        public FormCatalogTokenPrint()
        {
            InitializeComponent();
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

        private void TextBoxPrintQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Instance.Keypress_Num(sender, e);
        }
        private void ResetForm()
        {
            TextBoxPrintQuantity.Text = "1";
            ComboBoxDefaultPrinter.Items.Clear();
            ComboBoxDefaultPrinter.Items.AddRange(ComboUtils.GetAvailablePrinter().ToArray<string>());
            if (ComboBoxDefaultPrinter.Items != null && ComboBoxDefaultPrinter.Items.Count > 0)
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\VVMApp");
                if (key!=null && key.GetValue("DefaultPrinter") != null && !string.IsNullOrEmpty(key.GetValue("DefaultPrinter").ToString()))
                {
                    ComboBoxDefaultPrinter.SelectedIndex = ComboBoxDefaultPrinter.FindStringExact(key.GetValue("DefaultPrinter").ToString());
                }
            }
        }

        private void FormCatalogTokenPrint_Load(object sender, EventArgs e)
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
                PrintProductDetail.ProductsDetailsPrinting(ProductId,int.Parse(TextBoxPrintQuantity.Text), ComboBoxDefaultPrinter.Text);
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
