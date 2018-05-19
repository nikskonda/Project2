using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Windows.Forms.DataVisualization.Charting;

namespace Cube
{
    public partial class Form2 : Form
    {
        Form1 test;

        List<object[]> dataStress;
        List<object[]> dataDisplacement;

        public Form2()
        {
            InitializeComponent();

            button2.Visible = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Visible = true;
            test = (Form1)Owner;

            dataStress = test.GetStress();
            dataDisplacement = test.GetDisplacement();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            stressChart.Series.Clear();
            stressChart.Series.Add("1");
            stressChart.Series["1"].ChartType = SeriesChartType.Spline;
            stressChart.Series["1"].Color = Color.Orange;

            stressChart.Series["1"].MarkerStyle = MarkerStyle.Circle;
            //stressChart.Series["1"].IsValueShownAsLabel = true;

            //stressChart.ChartAreas["1"].Visible = true;

            // график Напряжения
            foreach (object[] data in dataStress)
            {
                stressChart.Series["1"].Points.AddXY((double)data[0], (double)data[1]);
            }

            displacementChart.Series.Clear();
            displacementChart.Series.Add("1");
            displacementChart.Series["1"].ChartType = SeriesChartType.Spline;
            displacementChart.Series["1"].Color = Color.Purple;

            displacementChart.Series["1"].MarkerStyle = MarkerStyle.Circle;
            //displacementChart.Series["1"].IsValueShownAsLabel = true;

            //displacementChart.ChartAreas["1"].Visible = true;

            // график Смещения
            foreach (object[] data in dataDisplacement)
            {
                displacementChart.Series["1"].Points.AddXY((double)data[0], (double)data[1]);
            }
        }
    }
}
