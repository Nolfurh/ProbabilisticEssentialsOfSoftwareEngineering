using Eto.Forms;
using LiveChartsCore.Defaults;
using System.Collections.ObjectModel;

namespace Lab3
{
    class Program
    {
        public static List<(double Sum, double Time)> Data { get; set; }
        public static (double CenterWeightX, double CenterWeightY) CenterWeight { get; set; }

        [STAThread]
        public static void Main()
        {
            Console.WriteLine("Enter name of file: ");
            string inputFilename = Console.ReadLine();
            if (File.Exists(inputFilename))
            {
                Data = DataFromFile(inputFilename);

                CenterWeight = CalculateCenterWeight(Data);
                Console.WriteLine($"Center of weight = ({CenterWeight.CenterWeightX}; {CenterWeight.CenterWeightY})");

                Console.WriteLine($"Covariation = {Cov(Data, CenterWeight)}");

                Console.WriteLine(WriteEquelRegression(Data, CenterWeight));

                Console.WriteLine($"Corelation coeficient = {Corelation(Data, CenterWeight)}");

                Application application = new();
                application.Run(new ScatterDiagramWindow(Data));
            }
            else
                Console.WriteLine("Wrong filename");
        }

        public static List<(double Sum, double Time)> DataFromFile(string fileName)
        {
            List<(double Sum, double Time)> data = new List<(double Sum, double Time)>();

            using (StreamReader reader = new StreamReader(fileName))
            {
                string? line;
                int.TryParse(reader.ReadLine(), out int quantity);
                while ((line = reader.ReadLine()) is not null)
                {
                    string[] columns = line.Split('\t');

                    if (double.TryParse(columns[0], out double sum))
                    {
                        data.Add((sum, double.Parse(columns[1])));
                    }
                }
            }

            return data;
        }
        
        public static (double CenterWeightX, double CenterWeightY) CalculateCenterWeight(List<(double Sum, double Time)> data)
        {
            return (CenterWeightX: Avarage(data.Select(i => i.Time)), CenterWeightY: Avarage(data.Select(i => i.Sum)));
        }
        public static double Avarage(IEnumerable<double> values) => values.Sum() / values.Count();
        
        public static double Cov(List<(double Sum, double Time)> data, (double CenterWeightX, double CenterWeightY) centerWeight)
        {
            double res = 0;
            foreach (var item in data)
            {
                res += item.Sum * item.Time;
            }
            res /= data.Count;
            res -= centerWeight.CenterWeightX * centerWeight.CenterWeightY;

            return res;
        }

        #region RegressionLine
        public static string WriteEquelRegression(List<(double Sum, double Time)> data, (double CenterWeightX, double CenterWeightY) centerWeight)
        {
            double varX = Var(data, centerWeight);

            double m = Cov(data, centerWeight) / varX;

            return $"y = {m}x - {m * centerWeight.CenterWeightX + centerWeight.CenterWeightY}";
        }

        public static double Var(List<(double Sum, double Time)> data, (double CenterWeightX, double CenterWeightY) centerWeight)
        {
            double res = data.Sum(i => Math.Pow(i.Time, 2)) / data.Count();
            res -= Math.Pow(centerWeight.CenterWeightX, 2);

            return res;
        }
        #endregion

        public static double Corelation(List<(double Sum, double Time)> data, (double CenterWeightX, double CenterWeightY) centerWeight)
        {
            double numerator = 0, sigmXExpr = 0, sigmYExpr = 0;

            foreach (var item in data)
            {
                numerator += (item.Time - centerWeight.CenterWeightX) * (item.Sum - centerWeight.CenterWeightY);
                sigmXExpr += Math.Pow(item.Time - centerWeight.CenterWeightX, 2);
                sigmYExpr += Math.Pow(item.Sum - centerWeight.CenterWeightY, 2);
            }

            return numerator / Math.Sqrt(sigmXExpr * sigmYExpr);
        }
    }
}