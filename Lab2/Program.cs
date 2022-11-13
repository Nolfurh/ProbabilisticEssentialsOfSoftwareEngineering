using System.IO;
using System.Collections.Generic;

namespace Lab2
{
    class Program
    {
        [STAThread]
        public static void Main()
        {
            Console.WriteLine("Enter name of file: ");
            string inputFilename = Console.ReadLine();
            if (File.Exists(inputFilename))
            {
                FrequencyTable frequencyTable = FrequencyTable.CreateFrequencyTableFromFile(inputFilename);

                Console.WriteLine($"Q1 = {frequencyTable.FindQuartile(1)}");
                Console.WriteLine($"Q3 = {frequencyTable.FindQuartile(3)}");
                Console.WriteLine($"P90 = {frequencyTable.FindPercentile(90)}");

                Console.WriteLine($"Standart deviation = {frequencyTable.StandartDeviation()}");
                Console.WriteLine($"Avarage deviation = {frequencyTable.AvarageDeviation()}");

                ForIncreasingAvarage(frequencyTable, 95, 100, out decimal a, out decimal b);
                Console.WriteLine($"y = ax + b for increasing avarage is y = {a}x + {b}");
                Console.WriteLine("New values: ");

                List<int> newScores = new();
                foreach (var item in frequencyTable.Rows)
                {
                    for (int i = 0; i < item.Value.Frequency; i++)
                    {
                        newScores.Add(IncreaseValue(item.Key, a, b));
                    }
                }
                newScores.ForEach((i) => Console.Write($"{i} "));
                Console.WriteLine();

                ShowRootAndLeafsDiagram(newScores);               

                string outputFilename = inputFilename;
                string toRemove = "input";
                if (outputFilename.ToLower().StartsWith(toRemove))
                {
                    outputFilename = outputFilename.ToLower().Replace(toRemove, "");
                    outputFilename = "output" + outputFilename;
                }
                WriteResultToFile(outputFilename, frequencyTable);

                BoxChartWindow.RunBoxChartWindow(frequencyTable.Rows.Max(a => a.Key), frequencyTable.FindQuartile(3),
                                                 frequencyTable.FindQuartile(1), frequencyTable.Rows.Min(a => a.Key));
            }
            else
                Console.WriteLine("Wrong filename");
        }

        public static void ForIncreasingAvarage(in FrequencyTable frequencyTable, int newAvarage, int permanentValue, out decimal a, out decimal b)
        {
            a = (permanentValue - newAvarage) / (decimal)(permanentValue - frequencyTable.Avarage());
            b = permanentValue - permanentValue * a;
        }
        public static int IncreaseValue(int value, decimal a, decimal b)
        {
            return (int)Math.Round(a * value + b, 0);
        }
        public static void ShowRootAndLeafsDiagram(List<int> list)
        {
            Dictionary<int, List<int>> rootAndLeafs = new();

            foreach (var item in list)
            {
                if (item < 10)
                {
                    if (!rootAndLeafs.ContainsKey(0))
                        rootAndLeafs.Add(0, new List<int>());

                    rootAndLeafs[0].Add(item);
                }
                else
                {    
                    int root = Int32.Parse(item.ToString().Remove(item.ToString().Length - 1));
                    int leaf = Int32.Parse(item.ToString()[item.ToString().Length - 1].ToString());

                    if (!rootAndLeafs.ContainsKey(root))
                        rootAndLeafs.Add(root, new List<int>());

                    rootAndLeafs[root].Add(leaf);
                }

            }

            foreach (var item in rootAndLeafs)
            {
                Console.Write("{0, -5}{1,-2}", item.Key, "|");
                item.Value.ForEach((i) => Console.Write($"{i} "));
                Console.WriteLine();
            }
            Console.WriteLine("Key 1|1 = 11");
        }

        public static void WriteResultToFile(string path, FrequencyTable frequencyTable)
        {
            using (StreamWriter writer = new StreamWriter(path, new FileStreamOptions() { Mode = FileMode.Create, Access = FileAccess.Write }))
            {
                writer.WriteLine($"Q1 = {frequencyTable.FindQuartile(1)}");
                writer.WriteLine($"Q3 = {frequencyTable.FindQuartile(3)}");
                writer.WriteLine($"P90 = {frequencyTable.FindPercentile(90)}");

                writer.WriteLine($"Standart deviation = {frequencyTable.StandartDeviation()}");
                writer.WriteLine($"Avarage deviation = {frequencyTable.AvarageDeviation()}");

                ForIncreasingAvarage(frequencyTable, 95, 100, out decimal a, out decimal b);
                writer.WriteLine($"y = ax + b for increasing avarage is y = {a}x + {b}");
                writer.WriteLine("New values: ");

                List<int> newScores = new();
                foreach (var item in frequencyTable.Rows)
                {
                    for (int i = 0; i < item.Value.Frequency; i++)
                    {
                        newScores.Add(IncreaseValue(item.Key, a, b));
                    }
                }
                newScores.ForEach((i) => writer.Write($"{i} "));
                writer.WriteLine();

                Dictionary<int, List<int>> rootAndLeafs = new();

                foreach (var item in newScores)
                {
                    if (item < 10)
                    {
                        if (!rootAndLeafs.ContainsKey(0))
                            rootAndLeafs.Add(0, new List<int>());

                        rootAndLeafs[0].Add(item);
                    }
                    else
                    {
                        int root = Int32.Parse(item.ToString().Remove(item.ToString().Length - 1));
                        int leaf = Int32.Parse(item.ToString()[item.ToString().Length - 1].ToString());

                        if (!rootAndLeafs.ContainsKey(root))
                            rootAndLeafs.Add(root, new List<int>());

                        rootAndLeafs[root].Add(leaf);
                    }

                }

                foreach (var item in rootAndLeafs)
                {
                    writer.Write("{0, -5}{1,-2}", item.Key, "|");
                    item.Value.ForEach((i) => writer.Write($"{i} "));
                    writer.WriteLine();
                }
                writer.WriteLine("Key 1|1 = 11");
            }
        }
    }


    public class FrequencyTableRow
    {
        public int Item { get; set; }
        public int Frequency { get; set; }
        public int CumulativeFrequency { get; set; }

        public FrequencyTableRow(int item)
        {
            Item = item;
            Frequency = 1;
        }
    }

    public class FrequencyTable
    {
        public SortedList<int, FrequencyTableRow> Rows { get; set; }
        public int TotalFrequency
        {
            get
            {
                return Rows.Last().Value.CumulativeFrequency;
            }
        }
        public int MaxFrequency
        {
            get
            {
                return Rows.Select(i => i.Value.Frequency).Max();
            }
        }

        public FrequencyTable()
        {
            Rows = new();
        }
        public static FrequencyTable CreateFrequencyTableFromFile(string fileName)
        {
            FrequencyTable frequencyTable = new();

            using (StreamReader reader = new StreamReader(fileName))
            {
                string? line;
                int.TryParse(reader.ReadLine(), out int quantity);
                while ((line = reader.ReadLine()) is not null)
                {
                    int num;
                    if (Int32.TryParse(line, out num))
                    {
                        frequencyTable.AddRow(num);
                    }
                }
            }

            return frequencyTable;
        }


        public FrequencyTableRow this[int item]
        {
            get
            {
                return Rows[item];
            }
            set
            {
                Rows[item] = value;
            }
        }

        public void AddRow(int item)
        {
            if (Rows.ContainsKey(item))
            {
                this.Rows[item].Frequency++;
            }
            else
            {
                Rows.Add(item, new FrequencyTableRow(item));
            }
            UpdateCumulativeFrequency();
        }

        protected void UpdateCumulativeFrequency()
        {
            int cumulation = 0;
            foreach (var row in Rows)
            {
                cumulation += row.Value.Frequency;
                row.Value.CumulativeFrequency = cumulation;
            }
        }



        public decimal FindPercentile(int numP)
        {
            if (numP < 0 || numP > 100)
                throw new Exception("Unpossible percentile number");

            double posP = Math.Round((numP / 100d) * (this.TotalFrequency + 1), 0);

            List<int> fullList = new();
            foreach (var item in Rows)
            {
                for (int i = 0; i < item.Value.Frequency; i++)
                {
                    fullList.Add(item.Key);
                }
            }

            return fullList[(int)posP - 1];
        }
        public decimal FindQuartile(int numQ)
        {
            switch (numQ)
            {
                case 1:
                    return FindPercentile(25);

                case 2:
                    return FindPercentile(50);

                case 3:
                    return FindPercentile(75);

                default:
                    throw new Exception("Unpossible quartile number");
            }
        }

        public double Avarage()
        {
            double avarage = 0d;

            foreach (var item in this.Rows)
            {
                avarage += item.Value.Frequency * item.Key;
            }
            avarage /= this.TotalFrequency;

            return avarage;
        }

        public double Dispercion()
        {
            double varX = 0d, avarage = Avarage();

            foreach (var item in this.Rows)
            {
                varX += item.Value.Frequency * Math.Pow(item.Key - avarage, 2);
            }
            varX /= this.TotalFrequency;


            return varX;
        }

        public double StandartDeviation()
        {
            return Math.Sqrt(Dispercion());
        }

        public double AvarageDeviation()
        {
            double avarX = 0d, avarage = Avarage();

            foreach (var item in this.Rows)            
                avarX += item.Value.Frequency * Math.Abs(item.Key - avarage);            

            avarX /= TotalFrequency;
            return avarX;
        }
    }
}