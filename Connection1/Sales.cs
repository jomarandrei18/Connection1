using Connection1.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Connection1
{
    public partial class Sales : Form
    {
        private Chart _barChart;

        private readonly IMenuCategoryService _menuCategoryService;

        public Sales(IMenuCategoryService menuCategoryService)
        {
            InitializeComponent();
            _menuCategoryService = menuCategoryService;
            CreateChart();
        }

        private void CreateChart()
        {
            _barChart = new Chart
            {
                Dock = DockStyle.Fill
            };
            this.Controls.Add(_barChart);

            ChartArea chartArea = new ChartArea();
            _barChart.ChartAreas.Add(chartArea);

            Series series = new Series("Sales")
            {
                ChartType = SeriesChartType.Column,
                Color = System.Drawing.Color.Red,
                IsValueShownAsLabel = true
            };
            _barChart.Series.Add(series);

            Series lineSeries = new Series("Trend")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                Color = System.Drawing.Color.Beige,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 8,
                IsVisibleInLegend = false
            };

            List<double> values = new List<double>();
            foreach (var orderSummary in _menuCategoryService.GetOrder())
            {
                double value = (double)orderSummary.TotalPrice;
                series.Points.AddXY(orderSummary.OrderDate, value);
                values.Add(value);
            }

            for (int i = 0; i < values.Count; i++)
            {
                double xValue = series.Points[i].XValue;
                double yValue = values[i];
                lineSeries.Points.AddXY(xValue, yValue);
            }

            Title chartTitle = new Title("Sales Data");
            _barChart.Titles.Add(chartTitle);

            chartArea.AxisX.Title = "Month";
            chartArea.AxisY.Title = "Sales (in units)";
        }

    }
}
