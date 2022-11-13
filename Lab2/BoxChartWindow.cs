using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Eto;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using Eto.Drawing;

namespace Lab2
{
    public class BoxChartWindow : Form
    {
        public BoxChartWindow(int max, decimal q3, decimal q1, int min)
        {
            Title = "Lab2";
            MinimumSize = new Size(200, 100);
            Size = MinimumSize * 4;
            Padding = 10;

            this.Content = new BoxChartPanel(max, q3, q1, min);
        }

        public static void RunBoxChartWindow(int max, decimal q3, decimal q1, int min)
        {
            Application application = new();
            BoxChartWindow window = new BoxChartWindow(max, q3, q1, min);

            application.Run(window);
        }
    }


    public class BoxChart
    {   
        public Axis[] XAxes { get; set; } =
        {
            new Axis
            {
                LabelsRotation = 15,
                UnitWidth = TimeSpan.FromDays(1).Ticks
            }
        };

        public ISeries[] Series { get; set; } =
        {
            new CandlesticksSeries<FinancialPoint>()
        };

        public BoxChart(int max, decimal q3, decimal q1, int min)
        {
            Series[0] = new CandlesticksSeries<FinancialPoint>
            {
                DownFill = new SolidColorPaint(SKColors.Blue),
                DownStroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 3 },
                Values = new ObservableCollection<FinancialPoint>
                    {
                        new FinancialPoint(new DateTime(), max, decimal.ToDouble(q3), decimal.ToDouble(q1), min)
                    }
            };

        }
    }

    public class BoxChartPanel : Panel
    {
        private readonly CartesianChart cartesianChart;

        public BoxChartPanel(int max, decimal q3, decimal q1, int min)
        {
            var viewModel = new BoxChart(max, q3, q1, min);

            cartesianChart = new CartesianChart
            {
                Series = viewModel.Series,
                XAxes = viewModel.XAxes,
            };

            Content = cartesianChart;
        }
    }
}
