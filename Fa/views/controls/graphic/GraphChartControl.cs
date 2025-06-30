using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.model.Hms.common;
using fa.api.Hms;
using System.Runtime.InteropServices;
using System.Reflection;
using Standard;
using ScottPlot;
using fa.views.controls.graphic;
namespace fa.views.controls.graphic
{
    public partial class GraphChartControl : UserControl
    {
        static int _x, _y;
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);
        bool dragging = false;
        private int beginX, beginY;
        public GraphChartControl()
        {
            InitializeComponent();
        }

        long? _PatientId;
        public long? PatientId
        {
            get
            {
                return _PatientId;
            }
            set
            {
                _PatientId = value;
                LoadChart();
            }
        }
        public struct POINT
        {
            public int X;
            public int Y;
        }

        static void ShowMousePosition()
        {
            POINT point;
            if (GetCursorPos(out point) && point.X != _x && point.Y != _y)
            {
                _x = point.X;
                _y = point.Y;
            }
        }
        public void LoadChart()
        {
            if (PatientId != null)
            {
                IList<Vital> ListVital = VitalEntryManager.Instance.ListVitalEntryByPatientId((long)PatientId);
                if (ListVital != null && ListVital.Count > 0)
                {
                    DateTime[] dates = new DateTime[ListVital.Count];
                    var vDate = new ScottPlot.Plottable.VLineVector();
                    vDate.Xs = new double[ListVital.Count];
                    vDate.Color = Color.Black;
                    vDate.PositionLabel = true;
                    vDate.PositionLabelBackground = vDate.Color;

                    var hHeight = new ScottPlot.Plottable.HLineVector();
                    hHeight.Ys = new double[ListVital.Count];
                    hHeight.Color = Color.Red;
                    hHeight.PositionLabel = true;
                    hHeight.PositionLabelBackground = hHeight.Color;
                    hHeight.DragEnabled = true;

                    var hWeight = new ScottPlot.Plottable.HLineVector();
                    hWeight.Ys = new double[ListVital.Count];
                    hWeight.Color = Color.Green;
                    hWeight.PositionLabel = true;
                    hWeight.PositionLabelBackground = hWeight.Color;
                    hWeight.DragEnabled = true;

                    var hBMI = new ScottPlot.Plottable.HLineVector();
                    hBMI.Ys = new double[ListVital.Count];
                    hBMI.Color = Color.Yellow;
                    hBMI.PositionLabel = true;
                    hBMI.PositionLabelBackground = hBMI.Color;
                    hBMI.DragEnabled = true;

                    var hTemp = new ScottPlot.Plottable.HLineVector();
                    hTemp.Ys = new double[ListVital.Count];
                    hTemp.Color = Color.DarkBlue;
                    hTemp.PositionLabel = true;
                    hTemp.PositionLabelBackground = hTemp.Color;
                    hTemp.DragEnabled = true;

                    var hPulse = new ScottPlot.Plottable.HLineVector();
                    hPulse.Ys = new double[ListVital.Count];
                    hPulse.Color = Color.Violet;
                    hPulse.PositionLabel = true;
                    hPulse.PositionLabelBackground = hPulse.Color;
                    hPulse.DragEnabled = true;

                    var hRepRate = new ScottPlot.Plottable.HLineVector();
                    hRepRate.Ys = new double[ListVital.Count];
                    hRepRate.Color = Color.LightGreen;
                    hRepRate.PositionLabel = true;
                    hRepRate.PositionLabelBackground = hRepRate.Color;
                    hRepRate.DragEnabled = true;

                    var hOxgLevel = new ScottPlot.Plottable.HLineVector();
                    hOxgLevel.Ys = new double[ListVital.Count];
                    hOxgLevel.Color = Color.Orange;
                    hOxgLevel.PositionLabel = true;
                    hOxgLevel.PositionLabelBackground = hOxgLevel.Color;
                    hOxgLevel.DragEnabled = true;

                    int i = 0;
                    foreach (Vital v in ListVital)
                    {
                        dates[i] = DateTime.ParseExact(v.DisDate, Global.Company.DateFormat, null).AddHours(v.Date.Hour).AddMinutes(v.Date.Minute).AddSeconds(v.Date.Second);
                        if (ChartCheckboxHeight.Checked == false)
                        {
                            v.Height = 0;
                        }
                        else
                        {
                            hHeight.Ys[i] = v.Height;
                        }
                        if (ChartCheckboxWeight.Checked == false)
                        {
                            v.Weight = 0;
                        }
                        else
                        {
                            hWeight.Ys[i] = (double)v.Weight;
                        }
                        if (ChartCheckboxBMI.Checked == false)
                        {
                            v.BMI = 0;
                        }
                        else
                        {
                            hBMI.Ys[i] = v.BMI;
                        }
                        if (ChartCheckboxTemperature.Checked == false)
                        {
                            v.Temperature = 0;
                        }
                        else
                        {
                            hTemp.Ys[i] = v.Temperature;
                        }
                        if (ChartCheckboxPulse.Checked == false)
                        {
                            v.Pulse = 0;
                        }
                        else
                        {
                            hPulse.Ys[i] = v.Pulse;
                        }
                        if (ChartCheckboxRespiratory.Checked == false)
                        {
                            v.RespRate = 0;
                        }
                        else
                        {
                            hRepRate.Ys[i] = v.RespRate;
                        }
                        if (ChartCheckboxOxygen.Checked == false)
                        {
                            v.BOxyLevel = 0;
                        }
                        else
                        {
                            hOxgLevel.Ys[i] = v.BOxyLevel;
                        }
                        i++;
                    }
                    vDate.Xs = dates.Select(x => x.ToOADate()).ToArray();
                    vDate.PositionLabel = false;
                    GraphChart.Reset();
                    GraphChart.plt.Add(vDate);
                    if (ChartCheckboxHeight.Checked == true)
                    {
                        GraphChart.plt.AddScatter(vDate.Xs, hHeight.Ys,Color.Red);
                    }
                    if (ChartCheckboxWeight.Checked == true)
                    {
                        GraphChart.plt.AddScatter(vDate.Xs, hWeight.Ys, Color.Green);
                    }
                    if (ChartCheckboxBMI.Checked == true)
                    {
                        GraphChart.plt.AddScatter(vDate.Xs, hBMI.Ys, Color.Yellow);
                    }
                    if (ChartCheckboxTemperature.Checked == true)
                    {
                        GraphChart.plt.AddScatter(vDate.Xs, hTemp.Ys, Color.DarkBlue);
                    }
                    if (ChartCheckboxPulse.Checked == true)
                    {
                        GraphChart.plt.AddScatter(vDate.Xs, hPulse.Ys, Color.Violet);
                    }
                    if (ChartCheckboxRespiratory.Checked == true)
                    {
                        GraphChart.plt.AddScatter(vDate.Xs, hRepRate.Ys, Color.LightGreen);
                    }
                    if (ChartCheckboxOxygen.Checked == true)
                    {
                        GraphChart.plt.AddScatter(vDate.Xs, hOxgLevel.Ys, Color.Orange);
                    }
                    GraphChart.plt.XAxis.TickLabelFormat(Global.Company.DateFormat, dateTimeFormat: true);
                    GraphChart.Refresh();
                }
            }
        }
        private void ChartCheckboxHeight_CheckedChanged(object sender, EventArgs e)
        {
            if (ChartCheckboxHeight.Checked == true)
            {
                LoadChart();
                if (ChartCheckboxHeight.Checked == true && ChartCheckboxWeight.Checked == true &&
                    ChartCheckboxBMI.Checked == true && ChartCheckboxTemperature.Checked == true &&
                    ChartCheckboxPulse.Checked == true && ChartCheckboxRespiratory.Checked == true &&
                   ChartCheckboxOxygen.Checked == true)
                {
                    ChartCheckboxAll.CheckState = CheckState.Checked;
                }
            }
            if (ChartCheckboxHeight.Checked == false)
            {
                LoadChart();
                ChartCheckboxAll.CheckState = CheckState.Unchecked;
            }
        }
        private void ChartCheckboxWeight_CheckedChanged(object sender, EventArgs e)
        {
            if (ChartCheckboxWeight.Checked == true)
            {
                LoadChart();
                if (ChartCheckboxHeight.Checked == true && ChartCheckboxWeight.Checked == true &&
                   ChartCheckboxBMI.Checked == true && ChartCheckboxTemperature.Checked == true &&
                   ChartCheckboxPulse.Checked == true && ChartCheckboxRespiratory.Checked == true &&
                  ChartCheckboxOxygen.Checked == true)
                {
                    ChartCheckboxAll.CheckState = CheckState.Checked;
                }
            }
            if (ChartCheckboxWeight.Checked == false)
            {
                LoadChart();
                ChartCheckboxAll.CheckState = CheckState.Unchecked;
            }
        }
        private void ChartCheckboxBMI_CheckedChanged(object sender, EventArgs e)
        {
            if (ChartCheckboxBMI.Checked == true)
            {
                LoadChart();
                if (ChartCheckboxHeight.Checked == true && ChartCheckboxWeight.Checked == true &&
                   ChartCheckboxBMI.Checked == true && ChartCheckboxTemperature.Checked == true &&
                   ChartCheckboxPulse.Checked == true && ChartCheckboxRespiratory.Checked == true &&
                  ChartCheckboxOxygen.Checked == true)
                {
                    ChartCheckboxAll.CheckState = CheckState.Checked;
                }
            }
            if (ChartCheckboxBMI.Checked == false)
            {
                LoadChart();
                ChartCheckboxAll.CheckState = CheckState.Unchecked;
            }
        }
        private void ChartCheckboxTemperature_CheckedChanged(object sender, EventArgs e)
        {
            if (ChartCheckboxTemperature.Checked == true)
            {
                LoadChart();
                if (ChartCheckboxHeight.Checked == true && ChartCheckboxWeight.Checked == true &&
                   ChartCheckboxBMI.Checked == true && ChartCheckboxTemperature.Checked == true &&
                   ChartCheckboxPulse.Checked == true && ChartCheckboxRespiratory.Checked == true &&
                  ChartCheckboxOxygen.Checked == true)
                {
                    ChartCheckboxAll.CheckState = CheckState.Checked;
                }
            }
            if (ChartCheckboxTemperature.Checked == false)
            {
                LoadChart();
                ChartCheckboxAll.CheckState = CheckState.Unchecked;
            }
        }
        private void ChartCheckboxPulse_CheckedChanged(object sender, EventArgs e)
        {
            if (ChartCheckboxPulse.Checked == true)
            {
                LoadChart();
                if (ChartCheckboxHeight.Checked == true && ChartCheckboxWeight.Checked == true &&
                   ChartCheckboxBMI.Checked == true && ChartCheckboxTemperature.Checked == true &&
                   ChartCheckboxPulse.Checked == true && ChartCheckboxRespiratory.Checked == true &&
                  ChartCheckboxOxygen.Checked == true)
                {
                    ChartCheckboxAll.CheckState = CheckState.Checked;
                }
            }
            if (ChartCheckboxPulse.Checked == false)
            {
                LoadChart();
                ChartCheckboxAll.CheckState = CheckState.Unchecked;
            }
        }
        private void ChartCheckboxRespiratory_CheckedChanged(object sender, EventArgs e)
        {
            if (ChartCheckboxRespiratory.Checked == true)
            {
                LoadChart();
                if (ChartCheckboxHeight.Checked == true && ChartCheckboxWeight.Checked == true &&
                   ChartCheckboxBMI.Checked == true && ChartCheckboxTemperature.Checked == true &&
                   ChartCheckboxPulse.Checked == true && ChartCheckboxRespiratory.Checked == true &&
                  ChartCheckboxOxygen.Checked == true)
                {
                    ChartCheckboxAll.CheckState = CheckState.Checked;
                }
            }
            if (ChartCheckboxRespiratory.Checked == false)
            {
                LoadChart();
                ChartCheckboxAll.CheckState = CheckState.Unchecked;
            }
        }
        private void ChartCheckboxOxygen_CheckedChanged(object sender, EventArgs e)
        {
            if (ChartCheckboxOxygen.Checked == true)
            {
                LoadChart();
                if (ChartCheckboxHeight.Checked == true && ChartCheckboxWeight.Checked == true &&
                   ChartCheckboxBMI.Checked == true && ChartCheckboxTemperature.Checked == true &&
                   ChartCheckboxPulse.Checked == true && ChartCheckboxRespiratory.Checked == true &&
                  ChartCheckboxOxygen.Checked == true)
                {
                    ChartCheckboxAll.CheckState = CheckState.Checked;
                }
            }
            if (ChartCheckboxOxygen.Checked == false)
            {
                LoadChart();
                ChartCheckboxAll.CheckState = CheckState.Unchecked;
            }
        }
        private void ChartCheckboxAll_CheckedChanged(object sender, EventArgs e)
        {
            if (ChartCheckboxAll.Checked == true)
            {
                ChartCheckboxBMI.CheckState = CheckState.Checked;
                ChartCheckboxHeight.CheckState = CheckState.Checked;
                ChartCheckboxWeight.CheckState = CheckState.Checked;
                ChartCheckboxTemperature.CheckState = CheckState.Checked;
                ChartCheckboxPulse.CheckState = CheckState.Checked;
                ChartCheckboxOxygen.CheckState = CheckState.Checked;
                ChartCheckboxRespiratory.CheckState = CheckState.Checked;
            }
            if (ChartCheckboxAll.Checked == false)
            {
                if (ChartCheckboxHeight.Checked == false || ChartCheckboxWeight.Checked == false ||
                    ChartCheckboxBMI.Checked == false || ChartCheckboxTemperature.Checked == false ||
                    ChartCheckboxPulse.Checked == false || ChartCheckboxRespiratory.Checked == false ||
                   ChartCheckboxOxygen.Checked == false)
                {
                    ChartCheckboxAll.CheckState = CheckState.Unchecked;
                }
                else
                {
                    ChartCheckboxBMI.CheckState = CheckState.Unchecked;
                    ChartCheckboxHeight.CheckState = CheckState.Unchecked;
                    ChartCheckboxWeight.CheckState = CheckState.Unchecked;
                    ChartCheckboxTemperature.CheckState = CheckState.Unchecked;
                    ChartCheckboxPulse.CheckState = CheckState.Unchecked;
                    ChartCheckboxOxygen.CheckState = CheckState.Unchecked;
                    ChartCheckboxRespiratory.CheckState = CheckState.Unchecked;
                    ChartCheckboxAll.CheckState = CheckState.Unchecked;
                    GraphChart.Reset();
                }
            }
        }
        private void GraphChartControl_Resize(object sender, EventArgs e)
        {
            if (DesignMode)
            {               
                if (dragging)
                {
                    GraphChart.Size = new Size(this.Width - 40, this.Height - 15);
                }
            }
        }
        private void GraphChartControl_SizeChanged(object sender, EventArgs e)
        {
            
        }
        private void GraphChartControl_ClientSizeChanged(object sender, EventArgs e)
        {
            dragging = true;
        }
        private void GraphChartControl_Load(object sender, EventArgs e)
        {
            foreach (Control cntrl in this.Controls)
            {
                cntrl.MouseDown += Control_MouseDown;
                cntrl.MouseMove += Control_MouseMove;
                cntrl.MouseUp += Control_MouseUp;
            }
        }

        private void Control_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }

        private void Control_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }

        private void Control_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
           
        }

        private void GraphChartControl_MouseDown(object sender, MouseEventArgs e)
        {
 
        }

        private void GraphChartControl_MouseClick(object sender, MouseEventArgs e)
        {
            dragging = true;
            beginX = e.X;
            beginY = e.Y;
        }

        private void GraphChartControl_MouseMove(object sender, MouseEventArgs e)
        {
            dragging = true;
            beginX = e.X;
            beginY = e.Y;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void GraphChart_Load(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }
    }
}
