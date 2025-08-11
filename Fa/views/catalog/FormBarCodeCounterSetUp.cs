using DocumentFormat.OpenXml.Office2010.Excel;
using fa;
using fa.model.OrderManagement;
using fa.views;
using FADataAccessLibrary.Api.BarCodeLabel;
using FADataAccessLibrary.Model.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fa.views.catalog
{
    public partial class FormBarCodeCounterSetUp : FormBase
    {
        public FormBarCodeCounterSetUp()
        {
            InitializeComponent();
        }

        private void BtnPriceCalculatorSave_Click(object sender, EventArgs e)
        {
            try
            {
                LabelStockMaster labelStockMasterInfo = GetBarCodeLabelInfoFromForm();

                if (labelStockMasterInfo != null)
                {
                    LabelStockMaster savedLabel = BarCodeLabelManager.Instance
                        .LoadNewLabelRoll(labelStockMasterInfo);

                    MessageBox.Show(
                        $"New label roll for {savedLabel.LabelType} loaded successfully.\n" +
                        $"Total Labels: {savedLabel.TotalLabelCount}, Remaining: {savedLabel.RemainingCount}",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show("No label stock information to save.",
                                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading label roll:\n{ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private LabelStockMaster GetBarCodeLabelInfoFromForm()
        {
            //LabelStockMaster labelStockMaster = new LabelStockMaster();
            //labelStockMaster.LabelSizeCode = ComboBoxLabelSize.Text.ToString();
            //labelStockMaster.LabelsPerRow = 1;
            long BarCodeId = 0; // default value

            if (!string.IsNullOrWhiteSpace(TextBoxBarCodeId.Text) && long.TryParse(TextBoxBarCodeId.Text, out long parsedId))
            {
                BarCodeId = parsedId;
            }

            LabelStockMaster labelStockMaster = new LabelStockMaster
            {
                Id = BarCodeId,
                CompanyId = Global.Company.CompanyId,
                LabelType = ComboBoxLabelType.Text?.Trim(),
                LabelSizeCode = ComboBoxLabelSize.Text?.Trim(),
                LabelsPerRow = (int)NumericUpDownLabelsPerRow.Value, // assuming NumericUpDown control
                TotalLabelCount = (int)NumericUpDownTotalLabelCount.Value,
                RemainingCount = (int)NumericUpDownRunningCount.Value,
                WastedLabelCount = (int)NumericUpDownWastedLabels.Value,
                DateLoaded = DateTimePickerDateLoaded!.Date.Value,
                DateEnded = DateTimePickerDateEnded.Checked ? DateTimePickerDateLoaded.Date.Value : (DateTime?)null,
                ThresholdWarning = (int)NumericUpDownFlagCount.Value
            };
            return labelStockMaster;
            //labelStockMaster.TotalLabelCount = (int)TextBoxXFactorRetail.Text.ToString();
        }
    }
}
