namespace Cube
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.stressChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.displacementChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.stressChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.displacementChart)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(196, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(182, 46);
            this.button1.TabIndex = 0;
            this.button1.Text = "Получить значения";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(405, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(182, 46);
            this.button2.TabIndex = 1;
            this.button2.Text = "Построить графики";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // stressChart
            // 
            chartArea1.AxisX.Title = "Ячейки";
            chartArea1.AxisY.Title = "Напряжение";
            chartArea1.Name = "ChartArea1";
            this.stressChart.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.stressChart.Legends.Add(legend1);
            this.stressChart.Location = new System.Drawing.Point(12, 93);
            this.stressChart.Name = "stressChart";
            series1.BorderWidth = 3;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.stressChart.Series.Add(series1);
            this.stressChart.Size = new System.Drawing.Size(366, 288);
            this.stressChart.TabIndex = 5;
            this.stressChart.Text = "chart1";
            // 
            // displacementChart
            // 
            chartArea2.AxisX.Title = "Ячейки";
            chartArea2.AxisY.Title = "Смещение";
            chartArea2.Name = "ChartArea1";
            this.displacementChart.ChartAreas.Add(chartArea2);
            legend2.Enabled = false;
            legend2.Name = "Legend1";
            this.displacementChart.Legends.Add(legend2);
            this.displacementChart.Location = new System.Drawing.Point(405, 93);
            this.displacementChart.Name = "displacementChart";
            series2.BorderWidth = 3;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Color = System.Drawing.Color.Purple;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.displacementChart.Series.Add(series2);
            this.displacementChart.Size = new System.Drawing.Size(368, 288);
            this.displacementChart.TabIndex = 6;
            this.displacementChart.Text = "chart2";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 393);
            this.Controls.Add(this.displacementChart);
            this.Controls.Add(this.stressChart);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form2";
            this.Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)(this.stressChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.displacementChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataVisualization.Charting.Chart stressChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart displacementChart;
    }
}