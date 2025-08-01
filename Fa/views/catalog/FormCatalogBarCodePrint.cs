using fa.libraries.utils;
using fa.libraries.Validation;
using fa.views.utils;
using Microsoft.Win32;
using System;
using System.Windows.Forms;
using System.Linq;
using fa.api.catalog;
using fa.views.sales;
using fa.views.hms;
using DocumentFormat.OpenXml.Drawing.Diagrams;

namespace fa.views.catalog
{
    public partial class FormCatalogBarCodePrint : FormBase
    {
        public static string ChooseLabelSizeErrorMsg = "Please choose label size";
        public static string EnterQtyErrorMsg = "Please Enter Quantity";
        public static string EnterValidQtyErrorMsg = "Please Enter valid Quantity";

        public long ProductId = 0L;
        public long PatientId = 0L;
        public bool IsOP = false;
        public bool IsIP = false;
        FormBase parent = null;
        public FormCatalogBarCodePrint(object sender)
        {
            if (sender is PatientRegistration)
            {
                parent = (PatientRegistration)sender;
            }
            InitializeComponent();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (validateFormPrint())
            {
                if (ProductId != 0L && CatalogProductManager.Instance.GetProductInfoById(ProductId) == null)
                {
                    MessageBox.Show("Somthing went wrong, the selected product is not valid.");
                    return;
                }
                Cursor.Current = Cursors.WaitCursor;
                if (!YesNoRadioPaperSize.Checked)
                {
                    SavePrintBarcode SavePrintBarcode = new SavePrintBarcode();
                    if (ProductId != 0L)
                    {
                        SavePrintBarcode.GenerateBarcodeA4(ProductId, "A4SheetBarCode", "pdf", true, TextBoxStartLocation.Text, int.Parse(TextBoxPrintQuantity.Text), ComboBoxDefaultPrinter.Text);
                        //SavePrintBarcode.GenerateCompactBarcodeLabel(
                        //    ProductId,  // First parameter should be the product ID
                        //    "A4SheetBarCode",
                        //    "pdf",
                        //    true,
                        //    TextBoxStartLocation.Text,
                        //    int.Parse(TextBoxPrintQuantity.Text),
                        //    ComboBoxDefaultPrinter.Text
                        //);
                    }
                    else
                    {
                        SavePrintBarcode.GenerateBarcodeA4ForPatient(PatientId, "A4SheetBarCode", "pdf", true, TextBoxStartLocation.Text, int.Parse(TextBoxPrintQuantity.Text), ComboBoxDefaultPrinter.Text);
                    }
                }
                else
                {
                    if (ProductId != 0L)
                    {
                        try
                        {
                            if (!int.TryParse(TextBoxPrintQuantity.Text, out int quantity) || quantity <= 0)
                            {
                                MessageBox.Show("Please enter a valid quantity (1 or more)");
                                return;
                            }

                            var savePrint = new SavePrintBarcode();

                        if (ComboBoxLabelSize.Text == "35 mm * 25 mm")
                        {
                            //savePrint.GenerateCompactBarcodeLabel(123, "test.pdf", ".pdf", false, "", 5, "PrinterName");
                            //savePrint.GenerateBarcodeA4(ProductId, "A4SheetBarCode", "pdf", true, TextBoxStartLocation.Text, int.Parse(TextBoxPrintQuantity.Text), ComboBoxDefaultPrinter.Text);
                            //savePrint.GenerateCompactBarcodeLabel(
                            //        ProductId,
                            //        "A4SheetBarCode",
                            //        "pdf",
                            //        true,
                            //        TextBoxStartLocation.Text,
                            //        quantity,
                            //        ComboBoxDefaultPrinter.Text
                            //    );
                            savePrint.GenerateSpecialBarcodeLabel(ProductId, long.Parse(TextBoxPrintQuantity.Text), ComboBoxDefaultPrinter.Text);
                        }
                        else if (ComboBoxLabelSize.Text == "50 mm * 25 mm")
                        {
                            savePrint.GenerateBarcodeLabel(ProductId, LabelSize.TWO, quantity, ComboBoxDefaultPrinter.Text);
                        }
                        else if (ComboBoxLabelSize.Text == "100 mm * 23 mm")
                        {
                            savePrint.GenerateBarcodeLabel(ProductId, LabelSize.ONE, quantity, ComboBoxDefaultPrinter.Text);
                        }
                        else
                        {
                            MessageBox.Show("Please select a valid label size");
                        }
                    }
                        catch (Exception ex)
                        {
                        MessageBox.Show($"Error generating barcode: {ex.Message}");
                        Console.WriteLine(ex.ToString());
                    }
                }
                    else if (IsIP || IsOP)
                    {
                        try
                        {
                            if (!int.TryParse(TextBoxPrintQuantity.Text, out int quantity) || quantity <= 0)
                            {
                                MessageBox.Show("Please enter a valid quantity (1 or more)");
                                return;
                            }

                            var savePrint = new SavePrintBarcode();
                            savePrint.GenerateBarcodeLabelForPatientOPIP(
                                PatientId,
                                LabelSize.ONE,
                                quantity,
                                ComboBoxDefaultPrinter.Text,
                                IsOP
                            );
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error generating patient barcode: {ex.Message}");
                            Console.WriteLine(ex.ToString());
                        }
                    }
                    else
                    {
                        try
                        {
                            if (!int.TryParse(TextBoxPrintQuantity.Text, out int quantity) || quantity <= 0)
                            {
                                MessageBox.Show("Please enter a valid quantity (1 or more)");
                                return;
                            }

                            var savePrint = new SavePrintBarcode();
                            savePrint.GenerateBarcodeLabelForPatient(
                                PatientId,
                                LabelSize.ONE,
                                quantity,
                                ComboBoxDefaultPrinter.Text
                            );
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error generating patient barcode: {ex.Message}");
                            Console.WriteLine(ex.ToString());
                        }
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
                return true;
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
            if (parent != null)
            {
                ComboBoxLabelSize.Items.Clear();
                ComboBoxLabelSize.Items.Add("100 mm* 50 mm");
                ComboBoxLabelSize.SelectedIndex = 0;
            }

            TextBoxPrintQuantity.Text = "1";
            YesNoRadioPaperSize.Checked = false;

            ComboBoxDefaultPrinter.Items.Clear();
            ComboBoxDefaultPrinter.Items.AddRange(ComboUtils.GetAvailablePrinter().ToArray());

            if (ComboBoxDefaultPrinter.Items.Count > 0)
            {
                RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Ab2App");
                if (key != null)
                {
                    // Try to get the BarCodePrinter first
                    string barcodePrinter = key.GetValue("BarCodePrinter")?.ToString()!;
                    string defaultPrinter = key.GetValue("DefaultPrinter")?.ToString()!;

                    string printerToUse = !string.IsNullOrEmpty(barcodePrinter)
                        ? barcodePrinter
                        : defaultPrinter;

                    if (!string.IsNullOrEmpty(printerToUse) &&
                        ComboBoxDefaultPrinter.Items.Contains(printerToUse))
                    {
                        ComboBoxDefaultPrinter.SelectedIndex =
                            ComboBoxDefaultPrinter.FindStringExact(printerToUse);
                    }

                    key.Close();
                }
            }

            YesNoRadioPaperSize.Checked = IsOP || IsIP;
            YesNoRadioPaperSize.Enabled = !(IsOP || IsIP);
        }

        private void FormCatalogBarCodePrint_Load(object sender, EventArgs e)
        {
            ResetForm();
            TextBoxPrintQuantity.Select();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void YesNoRadioPaperSize_Load(object sender, EventArgs e)
        {
            if (YesNoRadioPaperSize.Checked)
            {
                LabelLabelSize.Location = LabelStartLocation.Location;
                ComboBoxLabelSize.Location = TextBoxStartLocation.Location;
                LabelLabelSize.Visible = true;
                ComboBoxLabelSize.Visible = true;
                ComboBoxLabelSize.BringToFront();
                ComboBoxLabelSize.SelectedIndex = -1;
                LabelStartLocation.Visible = false;
                TextBoxStartLocation.Visible = false;
            }
            else
            {
                LabelLabelSize.Visible = false;
                ComboBoxLabelSize.Visible = false;
                LabelStartLocation.Visible = true;
                TextBoxStartLocation.Visible = true;
            }
        }
    }
}
