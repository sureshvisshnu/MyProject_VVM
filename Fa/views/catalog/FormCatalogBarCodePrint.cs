using DocumentFormat.OpenXml.Drawing.Diagrams;
using fa.api.catalog;
using fa.context;
using fa.libraries.utils;
using fa.libraries.Validation;
using fa.views.hms;
using fa.views.sales;
using fa.views.utils;
using Fa.views.catalog;
using FADataAccessLibrary.Api.BarCodeLabel;
using FADataAccessLibrary.Model.Catalog;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows.Forms;

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

                            // Common action after printing
                            //Action<int> handlePrintComplete = (qtyPrinted) =>
                            //{
                            //    int wastedLabels = CalculateWastedLabels(qtyPrinted);
                            //    int totalLabelsUsed = qtyPrinted + wastedLabels;

                            //    // Update DB stock
                            //    UpdateLabelUsage(qtyPrinted, wastedLabels);

                            //    // Refresh label stock info
                            //    var stockInfo = BarCodeLabelManager.Instance
                            //        .GetLabelStockInfo(ComboBoxLabelSize.Text);

                            //    if (stockInfo != null)
                            //    {
                            //        TextBoxlblTodayPrinted.Text = (stockInfo.LabelsPrintedToday).ToString();
                            //        TextBoxlblTotalBalance.Text = (stockInfo.TotalLabelCount - stockInfo.RunningCount).ToString();
                            //    }
                            //    LoadLabelStockInfo();
                            //    // Optional: message to confirm
                            //    //MessageBox.Show($"Printed: {qtyPrinted}, Wasted: {wastedLabels}, Remaining: {TextBoxlblTotalBalance.Text}");
                            //};
                            Action<int> handlePrintComplete = (qtyPrinted) =>
                            {
                                int wastedLabels = CalculateWastedLabels(qtyPrinted);

                                // Update DB stock
                                BarCodeLabelManager.Instance.UpdateLabelUsage(
                                    ComboBoxLabelSize.Text,
                                    qtyPrinted,
                                    wastedLabels,
                                    ProductId.ToString()
                                );

                                // Now reload fresh info from DB
                                var stockInfo = BarCodeLabelManager.Instance.GetLabelStockInfo(ComboBoxLabelSize.Text);

                                if (stockInfo != null)
                                {
                                    TextBoxlblTodayPrinted.Text = (stockInfo.LabelsPrintedToday).ToString();
                                    TextBoxlblTotalBalance.Text = (stockInfo.TotalLabelCount - stockInfo.RunningCount).ToString();

                                    // Check if roll finished
                                    if ((stockInfo.TotalLabelCount - stockInfo.RunningCount) <= 0)
                                    {
                                        DialogResult result = MessageBox.Show(
                                            $"The {ComboBoxLabelSize.Text} roll is finished.\nDo you want to load a new roll now?",
                                            "Label Roll Finished",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Question);

                                        if (result == DialogResult.Yes)
                                        {
                                            FormBarCodeCounterSetUp newRollForm = new FormBarCodeCounterSetUp();
                                            newRollForm.ShowDialog();
                                        }
                                    }
                                }
                                LoadLabelStockInfo();
                            };
                            
                            // Print based on size
                            if (ComboBoxLabelSize.Text == "35 mm * 25 mm")
                            {
                                savePrint.GenerateSpecialBarcodeLabel(ProductId, quantity, ComboBoxDefaultPrinter.Text);
                                handlePrintComplete(quantity);
                            }
                            else if (ComboBoxLabelSize.Text == "25 mm * 20 mm")
                            {
                                savePrint.GenerateCompactBarcodeLabel25x20_4UP(ProductId, quantity, ComboBoxDefaultPrinter.Text);
                                handlePrintComplete(quantity);
                            }
                            else if (ComboBoxLabelSize.Text == "50 mm * 25 mm")
                            {
                                savePrint.GenerateBarcodeLabel(ProductId, LabelSize.TWO, quantity, ComboBoxDefaultPrinter.Text);
                                handlePrintComplete(quantity);
                            }
                            else if (ComboBoxLabelSize.Text == "100 mm * 23 mm")
                            {
                                savePrint.GenerateBarcodeLabel(ProductId, LabelSize.ONE, quantity, ComboBoxDefaultPrinter.Text);
                                handlePrintComplete(quantity);
                            }
                            else
                            {
                                MessageBox.Show("Please select a valid label size");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error generating barcode: {ex.Message}");
                            Console.WriteLine(ex);
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

            RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\VVMApp");
            if (key != null)
            {
                // ✅ Load default printer
                string barcodePrinter = key.GetValue("BarCodePrinter")?.ToString() ?? "";
                string defaultPrinter = key.GetValue("DefaultPrinter")?.ToString() ?? "";

                string printerToUse = !string.IsNullOrEmpty(barcodePrinter)
                    ? barcodePrinter
                    : defaultPrinter;

                if (!string.IsNullOrEmpty(printerToUse) &&
                    ComboBoxDefaultPrinter.Items.Contains(printerToUse))
                {
                    ComboBoxDefaultPrinter.SelectedIndex =
                        ComboBoxDefaultPrinter.FindStringExact(printerToUse);
                }

                // ✅ Load saved barcode label size
                string savedLabelSize = key.GetValue("BarcodeLabelSize")?.ToString() ?? "";
                if (!string.IsNullOrEmpty(savedLabelSize) &&
                    ComboBoxLabelSize.Items.Contains(savedLabelSize))
                {
                    ComboBoxLabelSize.SelectedIndex =
                        ComboBoxLabelSize.FindStringExact(savedLabelSize);
                }

                key.Close();
            }

            //YesNoRadioPaperSize.Checked = IsOP || IsIP;
            //YesNoRadioPaperSize.Enabled = !(IsOP || IsIP);

            YesNoRadioPaperSize.Checked = true;
            YesNoRadioPaperSize.Enabled = !(IsOP || IsIP);
        }

        private void FormCatalogBarCodePrint_Load(object sender, EventArgs e)
        {
            ResetForm();
            LoadLabelStockInfo();
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
                // ❌ Removed: ComboBoxLabelSize.SelectedIndex = -1;
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

        private void LoadLabelStockInfo()
        {
            if (string.IsNullOrEmpty(ComboBoxLabelSize.Text))
                return;

            using (var context = new AccountMasterContext())
            {
                var stock = context.LabelStockMasters
                    .Where(ls => ls.LabelType == ComboBoxLabelSize.Text && ls.IsActive)
                    .OrderByDescending(ls => ls.DateLoaded)
                    .FirstOrDefault();

                if (stock != null)
                {
                    TextBoxlblTotalBalance.Text = $"Balance: {stock.RemainingCount} labels";

                    var todayPrinted = context.LabelStockUsages
                        .Where(u => u.LabelStockId == stock.Id && u.DatePrinted.Date == DateTime.Today)
                        .Sum(u => (int?)u.PrintedCount) ?? 0;

                    TextBoxlblTodayPrinted.Text = $"Today printed: {todayPrinted}";

                    if (stock.RemainingCount <= stock.ThresholdWarning)
                    {
                        lblWarning.Text = "⚠ Low stock!";
                        lblWarning.Visible = true;
                    }
                    else
                    {
                        lblWarning.Visible = false;
                    }
                }
            }
        }
        private void UpdateLabelUsage(int qtyPrinted, int wastedLabels = 0)
        {
            using (var context = new AccountMasterContext())
            {
                var stock = context.LabelStockMasters
                    .Where(ls => ls.LabelType == ComboBoxLabelSize.Text && ls.IsActive)
                    .OrderByDescending(ls => ls.DateLoaded)
                    .FirstOrDefault();

                if (stock != null)
                {
                    stock.LabelsUsed += qtyPrinted;
                    stock.WastedLabelCount += wastedLabels;
                    stock.RemainingCount = stock.TotalLabelCount - stock.LabelsUsed - stock.WastedLabelCount;

                    context.LabelStockUsages.Add(new LabelStockUsage
                    {
                        LabelStockId = stock.Id,
                        DatePrinted = DateTime.Now,
                        PrintedCount = qtyPrinted,
                        WastedCount = wastedLabels,
                        ReferenceId = ProductId.ToString(),
                        PrintedBy = Environment.UserName
                    });

                    context.SaveChanges();
                }
            }

            LoadLabelStockInfo();
        }
        private int CalculateWastedLabels(int qtyPrinted)
        {
            int labelsPerRow = 4; // or get from LabelStockMaster.LabelsPerRow
            int remainder = qtyPrinted % labelsPerRow;
            return remainder == 0 ? 0 : labelsPerRow - remainder;
        }
        private void OnPrintSuccess(int qtyPrinted)
        {
            int wastedLabels = CalculateWastedLabels(qtyPrinted);

            var updatedStock = BarCodeLabelManager.Instance.UpdateLabelUsage(
                labelType: ComboBoxLabelSize.Text,
                qtyPrinted: qtyPrinted,
                wastedLabels: wastedLabels,
                referenceId: ProductId.ToString()
            );

            // Update UI
            TextBoxlblTotalBalance.Text = $"Balance: {updatedStock.RemainingCount}";
            string labelType = ComboBoxLabelSize.Text; // or wherever you store it
            var stockInfo = BarCodeLabelManager.Instance.GetLabelStockInfo(labelType);

            TextBoxlblTodayPrinted.Text = $"Today printed: {BarCodeLabelManager.Instance.GetLabelStockInfo(labelType)}";
        }

    }


}
