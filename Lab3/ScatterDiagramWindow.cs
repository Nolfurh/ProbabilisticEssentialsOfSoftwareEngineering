using Eto.Drawing;
using Eto.Forms;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Eto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    internal class ScatterDiagramWindow : Form
    {
        public ScatterDiagramWindow(List<(double Sum, double Time)> data)
        {
            Title = "Lab3";
            MinimumSize = new Size(200, 100);
            Size = MinimumSize * 4;
            Padding = 10;

            //this.Content = new BoxChartPanel(max, q3, q1, min);
            this.Content = new ScatterPanel(data);
        }
    }


    //public partial class ScatterDiagram
    //{
    //    public ISeries[] Series { get; set; } =
    //    {
    //        new ScatterSeries<ObservablePoint>
    //        {
    //            Values = new ObservableCollection<ObservablePoint>
    //            {
    //                new(2.2, 5.4),
    //                new(4.5, 2.5),
    //                new(4.2, 7.4),
    //                new(6.4, 9.9),
    //                new(4.2, 9.2),
    //                new(5.8, 3.5),
    //                new(7.3, 5.8),
    //                new(8.9, 3.9),
    //                new(6.1, 4.6),
    //                new(9.4, 7.7),
    //                new(8.4, 8.5),
    //                new(3.6, 9.6),
    //                new(4.4, 6.3),
    //                new(5.8, 4.8),
    //                new(6.9, 3.4),
    //                new(7.6, 1.8),
    //                new(8.3, 8.3),
    //                new(9.9, 5.2),
    //                new(8.1, 4.7),
    //                new(7.4, 3.9),
    //                new(6.8, 2.3),
    //                new(5.3, 7.1),
    //            }
    //        }
    //    };
    //}

    public class ScatterPanel : Panel
    {
        private readonly CartesianChart cartesianChart;

        public ScatterPanel(List<(double Sum, double Time)> data)
        {

            ObservableCollection<ObservablePoint> points = new();
            foreach (var item in data)
            {
                points.Add(new(item.Time, item.Sum));
            }

            ISeries[] scatterSeries =
            {
                new ScatterSeries<ObservablePoint>
                {
                    Values = points
                }
            };

            cartesianChart = new CartesianChart
            {
                Series = scatterSeries
            };

            Content = cartesianChart;
        }
    }
}
