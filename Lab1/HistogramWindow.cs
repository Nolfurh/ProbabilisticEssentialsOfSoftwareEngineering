using Eto.Containers;
using Eto.Drawing;
using Eto.Forms;

namespace Lab1
{
    public class HistogramWindow : Form
    {
        public HistogramWindow()
        {
            Title = "Lab 1";
            MinimumSize = new Size(200, 50);
            Size = MinimumSize * 4;
            Padding = 10;
        }

        public void ShowFrequencyHistogram(FrequencyTable<int> frequencyTable)
        {
            List<Microcharts.ChartEntry> entries = new();
            foreach (var item in frequencyTable.Rows)
            {
                entries.Add(new Microcharts.ChartEntry(item.Value.Frequency) { Label = item.Key.ToString(), ValueLabel = item.Value.Frequency.ToString(), Color = SkiaSharp.SKColors.CornflowerBlue });
            }
            var charts = new Eto.Microcharts.ChartView() { Chart = new Microcharts.BarChart { Entries = entries, AnimationProgress = 100, LabelTextSize = 14 } };

            var scrollableContent = new Scrollable { Size = new Size(frequencyTable.Rows.Count * 10, 200), Content = charts};
            this.Content = new DragScrollable { Content = scrollableContent };
        }

        public static void RunHistogramWindow(FrequencyTable<int> frequencyTable)
        {
            Application application = new();
            HistogramWindow window = new();
            window.ShowFrequencyHistogram(frequencyTable);

            application.Run(window);
        }
    }
}
