using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.reports.sales
{
    public partial class SalesReportFullScreen : Form
    {
        public SalesReportFullScreen()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void SalesReportFullScreen_Load(object sender, EventArgs e)
        {
            ResizeTheDataGrid();
        }

        private void SalesReportFullScreen_Resize(object sender, EventArgs e)
        {
            ResizeTheDataGrid();
        }

        private void ResizeTheDataGrid()
        {
            DataGrid.Height = MiddlePanel.Height;
            DataGrid.Width = MiddlePanel.Width;
        }
    }
}
