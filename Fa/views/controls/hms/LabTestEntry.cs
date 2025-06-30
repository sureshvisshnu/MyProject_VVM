using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.views.controls.hms
{
    public partial class LabTestEntry : UserControl
    {
        protected static int MIN_HEIGHT = 174;
        protected static int MIN_WIDTH = 305;
        protected static int DATAGRID_WIDTH_DIFF = 9;
        protected static int DATAGRID_HEIGHT_DIFF = 24;
        protected static int DATAGRID_DESC_COLUMN_DIFF = 102;
        public LabTestEntry()
        {
            InitializeComponent();
        }

        private void LabTestEntry_ClientSizeChanged(object sender, EventArgs e)
        {
            if (this.Width - DATAGRID_WIDTH_DIFF < MIN_WIDTH)
            {
                this.Width = MIN_WIDTH;
            }
            else
            {
                DataGrid.Width = this.Width - DATAGRID_WIDTH_DIFF;
                //increase the widht of the grid
                DataGrid.Columns[1].Width = DataGrid.Width - DATAGRID_DESC_COLUMN_DIFF;
            }
            if (this.Height - DATAGRID_HEIGHT_DIFF < MIN_HEIGHT)
            {
                this.Height = MIN_HEIGHT;
            }
            else
            {
                DataGrid.Height = this.Height - DATAGRID_HEIGHT_DIFF;
            }
        }

        private void DataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
