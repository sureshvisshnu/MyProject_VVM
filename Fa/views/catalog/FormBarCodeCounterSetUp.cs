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
    public partial class FormBarCodeCounterSetUp : Form
    {
        public FormBarCodeCounterSetUp()
        {
            InitializeComponent();
        }

        private void BtnPriceCalculatorSave_Click(object sender, EventArgs e)
        {

        }

        private void GetBarCodeLabelInfoFromForm()
        {
            LabelStockMaster labelStockMaster = new LabelStockMaster();
            labelStockMaster.LabelSizeCode = ComboBoxLabelSize.Text.ToString();
            labelStockMaster.LabelsPerRow = 1;
            //labelStockMaster.TotalLabelCount = (int)TextBoxXFactorRetail.Text.ToString();
        }
    }
}
